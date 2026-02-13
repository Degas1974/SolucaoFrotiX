import os, re

root = r"d:\\FrotiX\\Solucao FrotiX 2026"
models_dir = os.path.join(root, "FrotiX.Site.OLD", "Models")

primitive_types = {
    "string", "int", "long", "short", "byte", "bool", "decimal", "double", "float",
    "DateTime", "Guid", "TimeSpan", "byte[]"
}

collection_markers = ("ICollection<", "IEnumerable<", "List<", "HashSet<")

class_pattern = re.compile(r"\bclass\s+(\w+)")
prop_pattern = re.compile(r"\bpublic\s+(?:virtual\s+)?([\w\.<>\[\]\?]+)\s+(\w+)\s*\{\s*get;\s*set;\s*\}")

def is_primitive(type_name: str) -> bool:
    t = type_name.replace("?", "").strip()
    if t in primitive_types:
        return True
    if t.startswith("Nullable<") and t.endswith(">"):
        inner = t[len("Nullable<"):-1]
        return inner in primitive_types
    return False

model_objects = {}

for dirpath, _, filenames in os.walk(models_dir):
    for filename in filenames:
        if not filename.endswith(".cs"):
            continue
        file_path = os.path.join(dirpath, filename)
        rel_path = os.path.relpath(file_path, root)
        with open(file_path, "r", encoding="utf-8", errors="ignore") as f:
            lines = f.readlines()

        pending_table = None
        current_class = None
        current_props = set()
        current_is_view = "\\\\Models\\\\Views\\\\" in file_path

        for i, line in enumerate(lines):
            stripped = line.strip()
            if stripped.startswith("[Table("):
                m = re.search(r"\[Table\(\s*\"([^\"]+)\"", stripped)
                if m:
                    pending_table = m.group(1)
                continue

            if "class " in stripped and not stripped.startswith("//"):
                m = class_pattern.search(stripped)
                if m:
                    if current_class:
                        obj_name = pending_table or current_class
                        model_objects[(obj_name, "VIEW" if current_is_view else "TABLE", rel_path)] = sorted(current_props)
                    current_class = m.group(1)
                    current_props = set()
                    if pending_table:
                        pending_table = None
                continue

            if "[NotMapped]" in stripped:
                lines[i] = lines[i] + "\n/*__NOTMAPPED__*/\n"
                continue

            if "public" in stripped and "get;" in stripped and "set;" in stripped:
                if "/*__NOTMAPPED__*/" in stripped:
                    continue
                j = i - 1
                not_mapped = False
                while j >= 0 and lines[j].strip() == "":
                    j -= 1
                if j >= 0 and "[NotMapped]" in lines[j]:
                    not_mapped = True
                if not_mapped:
                    continue

                m = prop_pattern.search(stripped)
                if not m:
                    continue
                type_name, prop_name = m.group(1), m.group(2)
                if any(marker in type_name for marker in collection_markers):
                    continue
                if not is_primitive(type_name):
                    continue
                current_props.add(prop_name)

        if current_class:
            obj_name = pending_table or current_class
            model_objects[(obj_name, "VIEW" if current_is_view else "TABLE", rel_path)] = sorted(current_props)

model_rows = []
for (obj_name, obj_type, rel_path), props in sorted(model_objects.items()):
    model_rows.append((obj_name, obj_type, rel_path, props))

sql_lines = []
sql_lines.append("SET NOCOUNT ON;")
sql_lines.append("DECLARE @ModelObjects TABLE (ObjectName sysname, ObjectType varchar(10), SourcePath nvarchar(400));")
sql_lines.append("DECLARE @ModelColumns TABLE (ObjectName sysname, ColumnName sysname);")

for obj_name, obj_type, rel_path, props in model_rows:
    sql_lines.append(f"INSERT INTO @ModelObjects (ObjectName, ObjectType, SourcePath) VALUES (N'{obj_name}', N'{obj_type}', N'{rel_path}');")
    for prop in props:
        sql_lines.append(f"INSERT INTO @ModelColumns (ObjectName, ColumnName) VALUES (N'{obj_name}', N'{prop}');")

sql_lines.append("\nWITH DbObjects AS (\n    SELECT TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE\n    FROM INFORMATION_SCHEMA.TABLES\n    WHERE TABLE_TYPE IN ('BASE TABLE','VIEW')\n)\n")

sql_lines.append("-- 1) Modelos sem tabela/view correspondente\nSELECT m.ObjectName, m.ObjectType, m.SourcePath\nFROM @ModelObjects m\nLEFT JOIN DbObjects d\n  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = m.ObjectName COLLATE Latin1_General_CI_AS\n AND d.TABLE_TYPE = CASE WHEN m.ObjectType = 'TABLE' THEN 'BASE TABLE' ELSE 'VIEW' END\nWHERE d.TABLE_NAME IS NULL\nORDER BY m.ObjectType, m.ObjectName;\n")

sql_lines.append("-- 2) Tabelas/views sem modelo correspondente\nSELECT d.TABLE_SCHEMA, d.TABLE_NAME, CASE WHEN d.TABLE_TYPE='BASE TABLE' THEN 'TABLE' ELSE 'VIEW' END AS ObjectType\nFROM DbObjects d\nLEFT JOIN @ModelObjects m\n  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = m.ObjectName COLLATE Latin1_General_CI_AS\n AND d.TABLE_TYPE = CASE WHEN m.ObjectType = 'TABLE' THEN 'BASE TABLE' ELSE 'VIEW' END\nWHERE m.ObjectName IS NULL\nORDER BY ObjectType, d.TABLE_SCHEMA, d.TABLE_NAME;\n")

sql_lines.append("-- 3) Colunas no DB que nao existem no modelo\nWITH DbColumns AS (\n    SELECT t.TABLE_SCHEMA, t.TABLE_NAME, t.TABLE_TYPE, c.COLUMN_NAME\n    FROM INFORMATION_SCHEMA.TABLES t\n    JOIN INFORMATION_SCHEMA.COLUMNS c\n      ON c.TABLE_SCHEMA = t.TABLE_SCHEMA AND c.TABLE_NAME = t.TABLE_NAME\n    WHERE t.TABLE_TYPE IN ('BASE TABLE','VIEW')\n)\nSELECT d.TABLE_SCHEMA, d.TABLE_NAME, d.COLUMN_NAME\nFROM DbColumns d\nJOIN @ModelObjects m\n  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = m.ObjectName COLLATE Latin1_General_CI_AS\n AND d.TABLE_TYPE = CASE WHEN m.ObjectType = 'TABLE' THEN 'BASE TABLE' ELSE 'VIEW' END\nLEFT JOIN @ModelColumns mc\n  ON mc.ObjectName COLLATE Latin1_General_CI_AS = d.TABLE_NAME COLLATE Latin1_General_CI_AS\n AND mc.ColumnName COLLATE Latin1_General_CI_AS = d.COLUMN_NAME COLLATE Latin1_General_CI_AS\nWHERE mc.ColumnName IS NULL\nORDER BY d.TABLE_SCHEMA, d.TABLE_NAME, d.COLUMN_NAME;\n")

sql_lines.append("-- 4) Colunas no modelo que nao existem no DB\nWITH DbColumns AS (\n    SELECT t.TABLE_SCHEMA, t.TABLE_NAME, t.TABLE_TYPE, c.COLUMN_NAME\n    FROM INFORMATION_SCHEMA.TABLES t\n    JOIN INFORMATION_SCHEMA.COLUMNS c\n      ON c.TABLE_SCHEMA = t.TABLE_SCHEMA AND c.TABLE_NAME = t.TABLE_NAME\n    WHERE t.TABLE_TYPE IN ('BASE TABLE','VIEW')\n)\nSELECT m.ObjectName, mc.ColumnName, m.SourcePath\nFROM @ModelColumns mc\nJOIN @ModelObjects m\n  ON m.ObjectName COLLATE Latin1_General_CI_AS = mc.ObjectName COLLATE Latin1_General_CI_AS\nLEFT JOIN DbColumns d\n  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = mc.ObjectName COLLATE Latin1_General_CI_AS\n AND d.COLUMN_NAME COLLATE Latin1_General_CI_AS = mc.ColumnName COLLATE Latin1_General_CI_AS\nWHERE d.COLUMN_NAME IS NULL\nORDER BY m.ObjectName, mc.ColumnName;\n")

script_path = os.path.join(root, "_tmp_model_schema_check.sql")
with open(script_path, "w", encoding="utf-8") as f:
    f.write("\n".join(sql_lines))

print("OK: SQL script generated at", script_path)
print("Model objects:", len(model_rows))
