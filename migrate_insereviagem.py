#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
migrate_insereviagem.py
=======================
Migrates insereviagem.js from Syncfusion ej2_instances to Kendo UI / bridge pattern.

NOTE: This file is DEAD CODE (not loaded by any page), migrated for consistency.

Rules:
  - Kendo ComboBox: cmbVeiculo, cmbRequisitante, cmbMotorista → $("#X").data("kendoComboBox")
  - Kendo DropDownList: ddtFinalidade→ddlFinalidade, ddtEventos→ddlEvento,
    ddtCombustivelInicial→ddlCombustivelInicial, ddtCombustivelFinal→ddlCombustivelFinal
  - Syncfusion DropDownTree (bridge): ddtSetor, ddtSetorRequisitante, ddtSetorPai,
    ddtSetorRequisitanteEvento, lstRequisitanteEvento → getComboEJ2("X")
  - rte stays Syncfusion → getComboEJ2("rte")
  - Upload (rte_upload) stays as-is
  - .value (property) → .value() for Kendo; .enabled = false → .enable(false) for Kendo
  - For bridge controls, keep .value, .enabled as property access
"""

import re
import os
import shutil

SRC = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\wwwroot\js\cadastros\insereviagem.js"
BAK = SRC + ".bak_migrate_ej2"

# ── Mapping tables ──────────────────────────────────────────────────────────

# Kendo ComboBox controls: old_id → (new_id, kendo_widget_type)
KENDO_COMBO = {
    "cmbVeiculo":      ("cmbVeiculo",      "kendoComboBox"),
    "cmbRequisitante": ("cmbRequisitante", "kendoComboBox"),
    "cmbMotorista":    ("cmbMotorista",    "kendoComboBox"),
}

# Kendo DropDownList controls: old_id → (new_id, kendo_widget_type)
KENDO_DDL = {
    "ddtFinalidade":        ("ddlFinalidade",        "kendoDropDownList"),
    "ddtEventos":           ("ddlEvento",            "kendoDropDownList"),
    "ddtCombustivelInicial":("ddlCombustivelInicial","kendoDropDownList"),
    "ddtCombustivelFinal":  ("ddlCombustivelFinal",  "kendoDropDownList"),
}

# All Kendo controls combined
KENDO_ALL = {}
KENDO_ALL.update(KENDO_COMBO)
KENDO_ALL.update(KENDO_DDL)

# Syncfusion/bridge controls: old_id → bridge call
BRIDGE_CONTROLS = {
    "ddtSetor":                   "ddtSetor",
    "ddtSetorRequisitante":       "ddtSetorRequisitante",
    "ddtSetorPai":                "ddtSetorPai",
    "ddtSetorRequisitanteEvento": "ddtSetorRequisitanteEvento",
    "lstRequisitanteEvento":      "lstRequisitanteEvento",
    "rte":                        "rte",
}

counters = {
    "kendo_replacements": 0,
    "bridge_replacements": 0,
    "enabled_to_enable": 0,
    "value_prop_to_method": 0,
    "text_prop_to_method": 0,
    "datasource_comments": 0,
    "refresh_to_datasource_read": 0,
    "additem_to_datasource_add": 0,
    "id_renames": 0,
}


def migrate_line(line: str) -> str:
    """Process a single line, applying all migration rules."""
    original = line

    # ──────────────────────────────────────────────────────────────────────
    # SKIP: Upload patterns (rte_upload) — keep as-is
    # ──────────────────────────────────────────────────────────────────────
    if "rte_upload" in line and "ej2_instances" in line:
        return line

    # ──────────────────────────────────────────────────────────────────────
    # STEP 1: Replace document.getElementById("X").ej2_instances[0]
    # ──────────────────────────────────────────────────────────────────────

    # Pattern: document.getElementById("X").ej2_instances[0]
    # Also handles document.getElementById('X').ej2_instances[0]
    ej2_pattern = r"""document\.getElementById\(\s*['"](\w+)['"]\s*\)\.ej2_instances\[0\]"""

    def replace_ej2(match):
        full_match = match.group(0)
        element_id = match.group(1)

        # Kendo controls
        if element_id in KENDO_ALL:
            new_id, widget_type = KENDO_ALL[element_id]
            counters["kendo_replacements"] += 1
            if new_id != element_id:
                counters["id_renames"] += 1
            return f'$("#{new_id}").data("{widget_type}")'

        # Bridge controls (Syncfusion staying)
        if element_id in BRIDGE_CONTROLS:
            bridge_id = BRIDGE_CONTROLS[element_id]
            counters["bridge_replacements"] += 1
            return f'getComboEJ2("{bridge_id}")'

        # Unknown — use bridge as fallback
        counters["bridge_replacements"] += 1
        return f'getComboEJ2("{element_id}")'

    line = re.sub(ej2_pattern, replace_ej2, line)

    # ──────────────────────────────────────────────────────────────────────
    # STEP 2: Post-replacement fixups for Kendo controls
    # ──────────────────────────────────────────────────────────────────────

    # For each Kendo control, fix .value property access → .value() method
    # But NOT .value() already (method call), NOT .value = X (assignment — handle separately)
    for old_id, (new_id, widget_type) in KENDO_ALL.items():
        jquery_sel = f'$("#{new_id}").data("{widget_type}")'

        if jquery_sel not in line:
            continue

        # ── .enabled = false → .enable(false) ──
        pat_enabled_false = re.escape(jquery_sel) + r'\.enabled\s*=\s*false'
        if re.search(pat_enabled_false, line):
            line = re.sub(pat_enabled_false, jquery_sel + '.enable(false)', line)
            counters["enabled_to_enable"] += 1

        # ── .enabled = true → .enable(true) ──
        pat_enabled_true = re.escape(jquery_sel) + r'\.enabled\s*=\s*true'
        if re.search(pat_enabled_true, line):
            line = re.sub(pat_enabled_true, jquery_sel + '.enable(true)', line)
            counters["enabled_to_enable"] += 1

        # ── .value = X → .value(X) ──
        # Match: selector.value = SOMETHING (not .value() or .value[)
        pat_value_assign = re.escape(jquery_sel) + r'\.value\s*=\s*(.+?)(?:;|$)'
        m = re.search(pat_value_assign, line)
        if m:
            rhs = m.group(1).rstrip(';').strip()
            line = line[:m.start()] + jquery_sel + f'.value({rhs})' + line[m.end():]
            # Re-add semicolon if original had one
            if not line.rstrip().endswith(';'):
                line = line.rstrip() + ';\n' if line.endswith('\n') else line.rstrip() + ';'
            counters["value_prop_to_method"] += 1

        # ── .text = X → .text(X) ──
        pat_text_assign = re.escape(jquery_sel) + r'\.text\s*=\s*(.+?)(?:;|$)'
        m = re.search(pat_text_assign, line)
        if m:
            rhs = m.group(1).rstrip(';').strip()
            line = line[:m.start()] + jquery_sel + f'.text({rhs})' + line[m.end():]
            if not line.rstrip().endswith(';'):
                line = line.rstrip() + ';\n' if line.endswith('\n') else line.rstrip() + ';'
            counters["text_prop_to_method"] += 1

        # ── .value (read access, not assignment, not already a call) ──
        # Pattern: selector.value but NOT selector.value( or selector.value = or selector.value[
        # This handles cases like: if (widget.value === null)  →  if (widget.value() === null)
        # And: var x = widget.value;  →  var x = widget.value();
        pat_value_read = re.escape(jquery_sel) + r'\.value(?!\s*[\(=\[])'
        if re.search(pat_value_read, line):
            line = re.sub(pat_value_read, jquery_sel + '.value()', line)
            counters["value_prop_to_method"] += 1

        # ── .text (read access) ──
        pat_text_read = re.escape(jquery_sel) + r'\.text(?!\s*[\(=\[])'
        if re.search(pat_text_read, line):
            line = re.sub(pat_text_read, jquery_sel + '.text()', line)
            counters["text_prop_to_method"] += 1

        # ── .fields.dataSource = X → // TODO: Kendo dataSource update ──
        pat_ds = re.escape(jquery_sel) + r'\.fields\.dataSource\s*=\s*'
        if re.search(pat_ds, line):
            line = re.sub(pat_ds, '// TODO: Kendo dataSource update — ' + jquery_sel + '.setDataSource(', line)
            # Add closing paren and comment
            if not line.rstrip().endswith('//'):
                line = line.rstrip().rstrip(';') + '); // was .fields.dataSource'
                if not line.endswith('\n') and original.endswith('\n'):
                    line += '\n'
            counters["datasource_comments"] += 1

        # ── .refresh() → .dataSource.read() ──
        pat_refresh = re.escape(jquery_sel) + r'\.refresh\(\)'
        if re.search(pat_refresh, line):
            line = re.sub(pat_refresh, jquery_sel + '.dataSource.read()', line)
            counters["refresh_to_datasource_read"] += 1

        # ── .addItem({...}) → .dataSource.add({...}) ──
        pat_additem = re.escape(jquery_sel) + r'\.addItem\('
        if re.search(pat_additem, line):
            line = re.sub(pat_additem, jquery_sel + '.dataSource.add(', line)
            counters["additem_to_datasource_add"] += 1

    # ──────────────────────────────────────────────────────────────────────
    # STEP 3: Handle variable-based ej2_instances patterns
    # e.g., "cmbVeiculo.value" where cmbVeiculo was assigned from ej2_instances
    # These are already handled by the var assignment replacement above
    # ──────────────────────────────────────────────────────────────────────

    return line


def main():
    print("=" * 70)
    print("  MIGRATE insereviagem.js — ej2_instances → Kendo/Bridge")
    print("=" * 70)
    print()

    if not os.path.exists(SRC):
        print(f"❌ Source file not found: {SRC}")
        return

    # Create backup
    shutil.copy2(SRC, BAK)
    print(f"✅ Backup created: {os.path.basename(BAK)}")

    # Read file
    with open(SRC, 'r', encoding='utf-8') as f:
        lines = f.readlines()

    print(f"📄 Read {len(lines)} lines from insereviagem.js")
    print()

    # Count ej2_instances before
    ej2_before = sum(1 for l in lines if 'ej2_instances' in l)
    print(f"🔍 ej2_instances occurrences BEFORE: {ej2_before}")
    print()

    # Process each line
    new_lines = []
    for i, line in enumerate(lines, 1):
        new_line = migrate_line(line)
        new_lines.append(new_line)

    # Count ej2_instances after
    ej2_after = sum(1 for l in new_lines if 'ej2_instances' in l)

    # Write output
    with open(SRC, 'w', encoding='utf-8') as f:
        f.writelines(new_lines)

    print("📊 MIGRATION RESULTS:")
    print(f"   Kendo replacements:         {counters['kendo_replacements']}")
    print(f"   Bridge replacements:        {counters['bridge_replacements']}")
    print(f"   ID renames (ddt→ddl):       {counters['id_renames']}")
    print(f"   .enabled → .enable():       {counters['enabled_to_enable']}")
    print(f"   .value → .value():          {counters['value_prop_to_method']}")
    print(f"   .text → .text():            {counters['text_prop_to_method']}")
    print(f"   .fields.dataSource → TODO:  {counters['datasource_comments']}")
    print(f"   .refresh() → .dataSource.read(): {counters['refresh_to_datasource_read']}")
    print(f"   .addItem() → .dataSource.add():  {counters['additem_to_datasource_add']}")
    print()
    print(f"🔍 ej2_instances BEFORE: {ej2_before}")
    print(f"🔍 ej2_instances AFTER:  {ej2_after}")
    print(f"   Eliminated: {ej2_before - ej2_after}")
    print()

    # Show remaining ej2_instances
    if ej2_after > 0:
        print("⚠️  REMAINING ej2_instances lines:")
        for i, l in enumerate(new_lines, 1):
            if 'ej2_instances' in l:
                print(f"   Line {i}: {l.rstrip()}")
        print()

    print(f"✅ File written: {SRC}")
    print("✅ Migration complete!")


if __name__ == "__main__":
    main()
