import re
from pathlib import Path

root = Path(r"d:\\FrotiX\\Solucao FrotiX 2026")
site_old = root / "FrotiX.Site.OLD"

entities = set()
for path in (site_old / "Data").rglob("*.cs"):
    text = path.read_text(encoding="utf-8", errors="ignore")
    entities.update(re.findall(r"DbSet<\s*([A-Za-z0-9_]+)\s*>", text))

source_sql = (root / "_tmp_model_schema_check.sql").read_text(encoding="utf-8", errors="ignore")
marker = "\nWITH DbObjects AS"
if marker in source_sql:
    head = source_sql.split(marker)[0]
else:
    head = source_sql

values = ",\n".join("(N'%s')" % name for name in sorted(entities))
filter_sql = f"""
\nDECLARE @DbSetObjects TABLE (ObjectName sysname);
INSERT INTO @DbSetObjects (ObjectName)
VALUES
{values};
\nDELETE mo
FROM @ModelObjects mo
LEFT JOIN @DbSetObjects ds ON ds.ObjectName = mo.ObjectName
WHERE ds.ObjectName IS NULL;
\nDELETE mc
FROM @ModelColumns mc
LEFT JOIN @DbSetObjects ds ON ds.ObjectName = mc.ObjectName
WHERE ds.ObjectName IS NULL;
\nWITH DbObjects AS (
    SELECT TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_TYPE IN ('BASE TABLE','VIEW')
)
SELECT m.ObjectName, m.ObjectType, m.SourcePath
FROM @ModelObjects m
LEFT JOIN DbObjects d
  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = m.ObjectName COLLATE Latin1_General_CI_AS
 AND d.TABLE_TYPE = CASE WHEN m.ObjectType = 'TABLE' THEN 'BASE TABLE' ELSE 'VIEW' END
WHERE d.TABLE_NAME IS NULL
ORDER BY m.ObjectType, m.ObjectName;
\nSELECT d.TABLE_SCHEMA, d.TABLE_NAME, CASE WHEN d.TABLE_TYPE='BASE TABLE' THEN 'TABLE' ELSE 'VIEW' END AS ObjectType
FROM DbObjects d
LEFT JOIN @ModelObjects m
  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = m.ObjectName COLLATE Latin1_General_CI_AS
 AND d.TABLE_TYPE = CASE WHEN m.ObjectType = 'TABLE' THEN 'BASE TABLE' ELSE 'VIEW' END
WHERE m.ObjectName IS NULL
ORDER BY ObjectType, d.TABLE_SCHEMA, d.TABLE_NAME;
\nWITH DbColumns AS (
    SELECT t.TABLE_SCHEMA, t.TABLE_NAME, t.TABLE_TYPE, c.COLUMN_NAME
    FROM INFORMATION_SCHEMA.TABLES t
    JOIN INFORMATION_SCHEMA.COLUMNS c
      ON c.TABLE_SCHEMA = t.TABLE_SCHEMA AND c.TABLE_NAME = t.TABLE_NAME
    WHERE t.TABLE_TYPE IN ('BASE TABLE','VIEW')
)
SELECT d.TABLE_SCHEMA, d.TABLE_NAME, d.COLUMN_NAME
FROM DbColumns d
JOIN @ModelObjects m
  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = m.ObjectName COLLATE Latin1_General_CI_AS
 AND d.TABLE_TYPE = CASE WHEN m.ObjectType = 'TABLE' THEN 'BASE TABLE' ELSE 'VIEW' END
LEFT JOIN @ModelColumns mc
  ON mc.ObjectName COLLATE Latin1_General_CI_AS = d.TABLE_NAME COLLATE Latin1_General_CI_AS
 AND mc.ColumnName COLLATE Latin1_General_CI_AS = d.COLUMN_NAME COLLATE Latin1_General_CI_AS
WHERE mc.ColumnName IS NULL
ORDER BY d.TABLE_SCHEMA, d.TABLE_NAME, d.COLUMN_NAME;
\nWITH DbColumns AS (
    SELECT t.TABLE_SCHEMA, t.TABLE_NAME, t.TABLE_TYPE, c.COLUMN_NAME
    FROM INFORMATION_SCHEMA.TABLES t
    JOIN INFORMATION_SCHEMA.COLUMNS c
      ON c.TABLE_SCHEMA = t.TABLE_SCHEMA AND c.TABLE_NAME = t.TABLE_NAME
    WHERE t.TABLE_TYPE IN ('BASE TABLE','VIEW')
)
SELECT m.ObjectName, mc.ColumnName, m.SourcePath
FROM @ModelColumns mc
JOIN @ModelObjects m
  ON m.ObjectName COLLATE Latin1_General_CI_AS = mc.ObjectName COLLATE Latin1_General_CI_AS
LEFT JOIN DbColumns d
  ON d.TABLE_NAME COLLATE Latin1_General_CI_AS = mc.ObjectName COLLATE Latin1_General_CI_AS
 AND d.COLUMN_NAME COLLATE Latin1_General_CI_AS = mc.ColumnName COLLATE Latin1_General_CI_AS
WHERE d.COLUMN_NAME IS NULL
ORDER BY m.ObjectName, mc.ColumnName;
"""

output = head + filter_sql
out_path = root / "_tmp_model_schema_check_filtered.sql"
out_path.write_text(output, encoding="utf-8")
print(str(out_path))
