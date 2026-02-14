#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
migrate_ViagemUpsert.py
=======================
Migrates ej2_instances references in ViagemUpsert.js from Syncfusion to Kendo UI jQuery.

Categories:
  1. cmbVeiculo     → Kendo ComboBox  ($("#cmbVeiculo").data("kendoComboBox"))
  2. cmbRequisitante→ Kendo ComboBox  ($("#cmbRequisitante").data("kendoComboBox"))
  3. DropDownTree   → getSyncfusionInstance("id") bridge
  4. Generic/RTE    → case-by-case
"""

import shutil
import re
import os

FILE_PATH = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\wwwroot\js\cadastros\ViagemUpsert.js"
BACKUP_PATH = FILE_PATH + ".bak_migrate_ej2"

def main():
    print("=" * 70)
    print("MIGRATION: ViagemUpsert.js — ej2_instances → Kendo/getSyncfusionInstance")
    print("=" * 70)

    # ── 0. Backup ──────────────────────────────────────────────────────────
    if not os.path.exists(FILE_PATH):
        print(f"ERROR: File not found: {FILE_PATH}")
        return
    shutil.copy2(FILE_PATH, BACKUP_PATH)
    print(f"✅ Backup created: {BACKUP_PATH}")

    with open(FILE_PATH, "r", encoding="utf-8") as f:
        content = f.read()

    original = content
    count = 0

    # ====================================================================
    # CATEGORY 3 FIRST: DropDownTree / Syncfusion controls → getSyncfusionInstance()
    # Must run BEFORE generic patterns to avoid double-replacements.
    # ====================================================================
    print("\n── CATEGORY 3: DropDownTree → getSyncfusionInstance() ──")

    ddt_controls = [
        "ddtSetorRequisitanteEvento",   # longest first to avoid partial matches
        "ddtSetorRequisitante",
        "ddtSetorPai",
        "ddtSetor",
        "lstRequisitanteEvento",
    ]

    for ctrl in ddt_controls:
        # Pattern A: document.getElementById("X").ej2_instances[0]  (with possible whitespace/newline before [0])
        pat = re.compile(
            r'document\.getElementById\(\s*"' + re.escape(ctrl) + r'"\s*\)\.ej2_instances\s*\[0\]',
            re.DOTALL,
        )
        repl = f'getSyncfusionInstance("{ctrl}")'
        content, n = pat.subn(repl, content)
        count += n
        if n:
            print(f"  {ctrl}: {n} (getElementById)")

        # Pattern B: document.getElementById("X")?.ej2_instances?.[0]
        pat2 = re.compile(
            r'document\.getElementById\(\s*"' + re.escape(ctrl) + r'"\s*\)\?\.ej2_instances\?\.\[0\]',
            re.DOTALL,
        )
        content, n = pat2.subn(repl, content)
        count += n
        if n:
            print(f"  {ctrl}: {n} (optional chaining)")

    # ====================================================================
    # CATEGORY 1 & 2: cmbVeiculo and cmbRequisitante → Kendo ComboBox
    # ====================================================================
    print("\n── CATEGORY 1 & 2: cmbVeiculo/cmbRequisitante → Kendo ComboBox ──")

    kendo_controls = ["cmbVeiculo", "cmbRequisitante"]

    for ctrl in kendo_controls:
        widget = "kendoComboBox"

        # Pattern A: document.getElementById("X")?.ej2_instances?.[0]?.value
        #   → ($("#X").data("kendoComboBox") ? $("#X").data("kendoComboBox").value() : null)
        pat_ov = re.compile(
            r'document\.getElementById\(\s*"' + re.escape(ctrl) +
            r'"\s*\)\?\.ej2_instances\?\.\[0\]\?\.value'
        )
        repl_ov = f'($("#{ctrl}").data("{widget}") ? $("#{ctrl}").data("{widget}").value() : null)'
        content, n = pat_ov.subn(repl_ov, content)
        count += n
        if n:
            print(f"  {ctrl}: {n} (optional chaining .value)")

        # Pattern B: document.getElementById("X").ej2_instances[0]  (plain)
        #   → $("#X").data("kendoComboBox")
        pat_plain = re.compile(
            r'document\.getElementById\(\s*"' + re.escape(ctrl) +
            r'"\s*\)\.ej2_instances\s*\[0\]'
        )
        repl_plain = f'$("#{ctrl}").data("{widget}")'
        content, n = pat_plain.subn(repl_plain, content)
        count += n
        if n:
            print(f"  {ctrl}: {n} (getElementById → .data())")

    # ── cmbVeiculo-specific patterns with variable name ──
    print("\n── cmbVeiculo variable-based patterns ──")

    # Pattern: cmbVeiculo.ej2_instances[0].value  (as property)
    pat_var_val = re.compile(r'cmbVeiculo\.ej2_instances\s*\[0\]\.value')
    repl_var_val = '($("#cmbVeiculo").data("kendoComboBox") ? $("#cmbVeiculo").data("kendoComboBox").value() : null)'
    content, n = pat_var_val.subn(repl_var_val, content)
    count += n
    if n:
        print(f"  cmbVeiculo.ej2_instances[0].value: {n}")

    # Pattern: !cmbVeiculo || !cmbVeiculo.ej2_instances || !cmbVeiculo.ej2_instances[0]
    pat_null = re.compile(
        r'!cmbVeiculo\s*\|\|\s*!cmbVeiculo\.ej2_instances\s*\|\|\s*!cmbVeiculo\.ej2_instances\s*\[0\]'
    )
    repl_null = '!$("#cmbVeiculo").data("kendoComboBox")'
    content, n = pat_null.subn(repl_null, content)
    count += n
    if n:
        print(f"  cmbVeiculo null-check: {n}")

    # Pattern: cmbVeiculo && cmbVeiculo.ej2_instances && cmbVeiculo.ej2_instances[0]
    pat_exist = re.compile(
        r'cmbVeiculo\s+&&\s+cmbVeiculo\.ej2_instances\s+&&\s+cmbVeiculo\.ej2_instances\s*\[0\]'
    )
    repl_exist = '$("#cmbVeiculo").data("kendoComboBox")'
    content, n = pat_exist.subn(repl_exist, content)
    count += n
    if n:
        print(f"  cmbVeiculo existence-check: {n}")

    # ====================================================================
    # CATEGORY 4: Specific context-dependent patterns
    # ====================================================================
    print("\n── CATEGORY 4: Specific patterns ──")

    # ── 4a. Lines 1685-1692: requisitantes = getElementById("cmbRequisitante")
    #        then requisitantes.ej2_instances && requisitantes.ej2_instances.length > 0
    #        then requisitantes.ej2_instances[0].dataSource = []
    #   cmbRequisitante is NOW Kendo ComboBox
    pat_req_check = re.compile(
        r'requisitantes\s*&&\s*\n?\s*requisitantes\.ej2_instances\s*&&\s*\n?\s*requisitantes\.ej2_instances\.length\s*>\s*0'
    )
    content, n = pat_req_check.subn(
        '$("#cmbRequisitante").data("kendoComboBox")',
        content
    )
    count += n
    if n:
        print(f"  requisitantes check → Kendo: {n}")

    pat_req_ds = re.compile(r'requisitantes\.ej2_instances\[0\]\.dataSource\s*=\s*\[\]')
    content, n = pat_req_ds.subn(
        '$("#cmbRequisitante").data("kendoComboBox").dataSource.data([])',
        content
    )
    count += n
    if n:
        print(f"  requisitantes.dataSource = [] → Kendo: {n}")

    # ── 4b. Lines 1705-1709: setor = getElementById("cmbSetor")
    #        then setor.ej2_instances && setor.ej2_instances.length > 0
    #        then setor.ej2_instances[0].dataSource = []
    #        then setor.ej2_instances[0].enabled = true
    #   cmbSetor stays Syncfusion — use getSyncfusionInstance
    pat_setor_check = re.compile(
        r'setor\s*&&\s*setor\.ej2_instances\s*&&\s*setor\.ej2_instances\.length\s*>\s*0'
    )
    content, n = pat_setor_check.subn(
        'getSyncfusionInstance("cmbSetor")',
        content
    )
    count += n
    if n:
        print(f"  setor check → getSyncfusionInstance: {n}")

    pat_setor_prop = re.compile(r'setor\.ej2_instances\[0\]\.(dataSource|enabled)')
    def setor_repl(m):
        return f'getSyncfusionInstance("cmbSetor").{m.group(1)}'
    content, n = pat_setor_prop.subn(setor_repl, content)
    count += n
    if n:
        print(f"  setor.ej2_instances[0].prop → getSyncfusionInstance: {n}")

    # ── 4c. Lines 3563-3564: RTE disable
    #   rteElement.ej2_instances[0].enabled = false  → already handled by disableEditorUpsert() below it
    #   Just remove the ej2 block and leave discardEditorUpsert
    pat_rte = re.compile(
        r'if\s*\(rteElement\s*&&\s*rteElement\.ej2_instances\s*&&\s*rteElement\.ej2_instances\[0\]\)\s*\{\s*\n\s*rteElement\.ej2_instances\[0\]\.enabled\s*=\s*false;\s*\n\s*\}'
    )
    content, n = pat_rte.subn(
        '// ej2_instances RTE block removed — disableEditorUpsert() below handles Kendo Editor',
        content
    )
    count += n
    if n:
        print(f"  RTE ej2 disable → removed (disableEditorUpsert handles it): {n}")

    # ── 4d. Line 3585: Generic control = document.getElementById(id).ej2_instances[0]
    #   This is in the ["cmbVeiculo", "cmbRequisitante"].forEach loop that disables controls.
    #   These are NOW Kendo ComboBox, so we need to replace the whole block.
    #   Pattern: const control = document.getElementById(id).ej2_instances[0];
    #            if (control) control.enabled = false;
    #   → const control = $("#" + id).data("kendoComboBox"); if (control) control.enable(false);
    pat_generic_disable = re.compile(
        r'const\s+control\s*=\s*document\.getElementById\(id\)\.ej2_instances\[0\];\s*\n\s*if\s*\(control\)\s*control\.enabled\s*=\s*false;'
    )
    content, n = pat_generic_disable.subn(
        'const control = $("#" + id).data("kendoComboBox");\n                        if (control) control.enable(false);',
        content
    )
    count += n
    if n:
        print(f"  Generic forEach disable → Kendo .enable(false): {n}")

    # ── 4e. ddtSetor optional chaining: document.getElementById("ddtSetor")?.ej2_instances?.[0]
    #   Should already be handled by Category 3 above, but let's verify
    pat_ddt_optional = re.compile(
        r'document\.getElementById\(\s*"ddtSetor"\s*\)\?\.ej2_instances\?\.\[0\]'
    )
    content, n = pat_ddt_optional.subn('getSyncfusionInstance("ddtSetor")', content)
    count += n
    if n:
        print(f"  ddtSetor optional chaining: {n}")

    # ── 4f. Line ~4099: InserirNovoRequisitante success callback
    #   cmb.dataSource.push(requisitante) → cmb.dataSource.add(requisitante)
    #   cmb.value = requisitante.id        → cmb.value(requisitante.id)
    #   cmb.dataBind()                     → removed
    #   Note: cmb was already replaced to $("#cmbRequisitante").data("kendoComboBox") in Cat 1&2
    content = content.replace(
        'cmb.dataSource.push(requisitante);',
        'cmb.dataSource.add(requisitante);'
    )
    if 'cmb.dataSource.add(requisitante)' in content:
        count += 1
        print("  cmb.dataSource.push → .add: 1")

    content = content.replace(
        'cmb.value = requisitante.id;',
        'cmb.value(requisitante.id);'
    )
    if 'cmb.value(requisitante.id)' in content:
        count += 1
        print("  cmb.value = → cmb.value(): 1")

    content = content.replace(
        'cmb.dataBind();',
        '// cmb.dataBind(); // Removed: Kendo ComboBox auto-updates'
    )
    if '// cmb.dataBind()' in content:
        count += 1
        print("  cmb.dataBind() → removed: 1")

    # ── 4g. Line ~4661: addItem for cmbRequisitante  
    #   Already replaced getElementById("cmbRequisitante").ej2_instances[0] in Cat 1&2
    #   But the .addItem(...) call needs to become .dataSource.add(...)
    #   Pattern after Cat 1&2: $("#cmbRequisitante").data("kendoComboBox").addItem(
    content = content.replace(
        '$("#cmbRequisitante").data("kendoComboBox").addItem(',
        '$("#cmbRequisitante").data("kendoComboBox").dataSource.add('
    )
    if '$("#cmbRequisitante").data("kendoComboBox").dataSource.add(' in content:
        count += 1
        print("  .addItem( → .dataSource.add(: 1")

    # ── 4h. Lines around VeiculoValueChange (line 4429):
    #   ddTreeObj was replaced: var ddTreeObj = $("#cmbVeiculo").data("kendoComboBox");
    #   But ddTreeObj.value (property) must become ddTreeObj.value() (method)
    #   And ddTreeObj.text (property) must become ddTreeObj.text() (method)
    #   These are context-sensitive, so handle specific known patterns:
    print("\n── Post-migration: .value/.text property → method conversion ──")

    # ddTreeObj.value === null  (cmbVeiculo context, VeiculoValueChange)
    # Need to be careful: ddTreeObj is also used for cmbRequisitante and lstRequisitanteEvento
    # For cmbVeiculo and cmbRequisitante (Kendo), .value → .value()
    # For lstRequisitanteEvento (Syncfusion), .value stays as .value

    # Strategy: Process line by line to track context
    lines = content.split('\n')
    modified_lines = []
    in_veiculo_context = False
    in_requisitante_context = False
    in_combo_context = False  # for var combo = ... cmbVeiculo
    changes_value_text = 0

    for i, line in enumerate(lines):
        original_line = line

        # Track context: which function are we in?
        if 'function VeiculoValueChange' in line:
            in_veiculo_context = True
            in_requisitante_context = False
        elif 'function RequisitanteValueChange' in line:
            in_requisitante_context = True
            in_veiculo_context = False
        elif 'function RequisitanteEventoValueChange' in line:
            # lstRequisitanteEvento stays Syncfusion — don't convert .value/.text
            in_requisitante_context = False
            in_veiculo_context = False
        elif line.strip().startswith('function ') and 'ValueChange' not in line:
            # Exiting a ValueChange function
            if in_veiculo_context or in_requisitante_context:
                in_veiculo_context = False
                in_requisitante_context = False

        # Track combo context (var combo = ... cmbVeiculo)
        if 'var combo = $("#cmbVeiculo").data("kendoComboBox")' in line:
            in_combo_context = True
        elif in_combo_context and (line.strip().startswith('function ') or line.strip() == '}'):
            if line.strip() == '}' and i > 0:
                pass  # don't exit on every }, only on function boundaries
            if line.strip().startswith('function '):
                in_combo_context = False

        # ── In VeiculoValueChange context: ddTreeObj.value → ddTreeObj.value() ──
        if in_veiculo_context:
            # ddTreeObj.value === null → (ddTreeObj ? (ddTreeObj.value() === null || ...) : true)
            if 'ddTreeObj.value === null' in line:
                line = line.replace(
                    'ddTreeObj.value === null',
                    '(ddTreeObj.value() === null || ddTreeObj.value() === "" || ddTreeObj.value() === undefined)'
                )
            # var veiculoid = String(ddTreeObj.value);
            if re.search(r'String\(ddTreeObj\.value\)', line):
                line = re.sub(r'String\(ddTreeObj\.value\)', 'String(ddTreeObj.value())', line)
            # ddTreeObj.text (property access, not a function call already)
            if re.search(r'ddTreeObj\.text(?!\()', line) and 'ddTreeObj.text()' not in line:
                line = re.sub(r'ddTreeObj\.text(?!\()', 'ddTreeObj.text()', line)

        # ── In RequisitanteValueChange context: ddTreeObj.value → ddTreeObj.value() ──
        if in_requisitante_context:
            if 'ddTreeObj.value === null' in line:
                line = line.replace(
                    'ddTreeObj.value === null',
                    '(ddTreeObj.value() === null || ddTreeObj.value() === "" || ddTreeObj.value() === undefined)'
                )
            if re.search(r'String\(ddTreeObj\.value\)', line):
                line = re.sub(r'String\(ddTreeObj\.value\)', 'String(ddTreeObj.value())', line)

        # ── combo.focusIn() context (VeiculoValueChange) ──
        if in_combo_context:
            # combo.focusIn() → combo.input.focus() (Kendo equivalent)
            if 'combo.focusIn()' in line:
                line = line.replace('combo.focusIn()', 'combo.input.focus()')

        # ── Line 5620: ddTreeObj = ... cmbVeiculo; then ddTreeObj.text ──
        # This is outside VeiculoValueChange, in abrirModalOcorrenciasVeiculoUpsert
        if '$("#cmbVeiculo").data("kendoComboBox")' in line and 'ddTreeObj' in line:
            # Mark that next lines using ddTreeObj.text need conversion
            pass  # handled below

        # ── Line ~4821-4822: veiculo.value === null ──
        if 'const veiculo = $("#cmbVeiculo").data("kendoComboBox")' in original_line:
            pass  # This line itself is fine
        # Next line after it: "if (veiculo.value === null)"
        if re.search(r'\bveiculo\.value\s*===\s*null\b', line) and 'veiculo.value()' not in line:
            line = re.sub(
                r'\bveiculo\.value\s*===\s*null\b',
                '(veiculo.value() === null || veiculo.value() === "" || veiculo.value() === undefined)',
                line
            )

        # ── Line ~4842-4843: requisitante.value (from const requisitante = ...) ──
        if re.search(r'\brequisitante\.value\b', line) and '$("#cmbRequisitante")' not in line:
            # Only convert if it's property access (not already a method call)
            if 'requisitante.value()' not in line and 'requisitante.value =' not in line:
                # Check if this is a Kendo context (requisitante from cmbRequisitante)
                # Pattern: !requisitante.value || requisitante.value[0] === null
                if '!requisitante.value' in line or 'requisitante.value[' in line:
                    line = line.replace('requisitante.value[0]', 'requisitante.value()')
                    line = line.replace('!requisitante.value', '(!requisitante.value() || requisitante.value() === "")')

        # ── ddTreeObj.text (line ~5621, outside VeiculoValueChange) ──
        # "const textoVeiculo = ddTreeObj.text || 'Veículo';"
        if "ddTreeObj.text" in line and "ddTreeObj.text()" not in line and not in_veiculo_context:
            # Check surrounding context — if ddTreeObj was set from cmbVeiculo Kendo
            # Look back a few lines for cmbVeiculo
            context_start = max(0, i - 5)
            context_lines = '\n'.join(lines[context_start:i+1])
            if 'cmbVeiculo' in context_lines or '$("#cmbVeiculo")' in context_lines:
                line = re.sub(r'ddTreeObj\.text(?!\()', 'ddTreeObj.text()', line)

        if line != original_line:
            changes_value_text += 1

        modified_lines.append(line)

    content = '\n'.join(modified_lines)
    count += changes_value_text
    if changes_value_text:
        print(f"  .value/.text property → method: {changes_value_text}")

    # ── 4i. getComboEJ2 function (line ~2121-2126): Leave AS-IS ──
    # The function body uses `host?.ej2_instances?.[0]` — this is the BRIDGE function itself.
    # Verify it wasn't touched:
    if 'getComboEJ2' in content and 'ej2_instances' in content:
        print("\n  ℹ️  getComboEJ2() bridge function preserved (uses ej2_instances internally)")

    # ── 4j. Tooltip ej2_instances loop (lines 5337-5341): Leave AS-IS ──
    print("  ℹ️  Tooltip ej2_instances loop at ~L5337 preserved")

    # ── 4k. Upload ej2_instances (lines 1752-1754): Leave AS-IS ──
    print("  ℹ️  Upload ej2_instances at ~L1752 preserved")

    # ====================================================================
    # WRITE RESULT
    # ====================================================================
    print("\n" + "=" * 70)
    print(f"Total replacements/changes: {count}")

    with open(FILE_PATH, "w", encoding="utf-8") as f:
        f.write(content)
    print(f"✅ File written successfully: {FILE_PATH}")

    # ====================================================================
    # VERIFICATION: Check remaining ej2_instances
    # ====================================================================
    remaining_lines = []
    for i, line in enumerate(content.split('\n'), 1):
        if 'ej2_instances' in line:
            remaining_lines.append((i, line.strip()[:140]))

    print(f"\n📊 Remaining ej2_instances occurrences: {len(remaining_lines)}")
    for ln, txt in remaining_lines:
        print(f"  L{ln}: {txt}")

    # Check that cmbVeiculo/cmbRequisitante NO LONGER have ej2_instances
    cmb_remaining = [
        (ln, txt) for ln, txt in remaining_lines
        if 'cmbVeiculo' in txt or 'cmbRequisitante' in txt
    ]
    if cmb_remaining:
        print(f"\n⚠️  WARNING: {len(cmb_remaining)} ej2_instances REMAIN for cmbVeiculo/cmbRequisitante!")
        for ln, txt in cmb_remaining:
            print(f"  L{ln}: {txt}")
    else:
        print("\n✅ No ej2_instances remain for cmbVeiculo or cmbRequisitante!")

    # Expected remaining: getComboEJ2 function body, upload block, tooltip loop, comments
    expected_remaining = set()
    for ln, txt in remaining_lines:
        if 'getComboEJ2' in txt or 'host?.ej2_instances' in txt:
            expected_remaining.add(ln)
        elif 'up.ej2_instances' in txt or 'rte_upload' in txt:
            expected_remaining.add(ln)
        elif 'el.ej2_instances' in txt or 'el && el.ej2_instances' in txt:
            expected_remaining.add(ln)
        elif 'ej2_instances do elemento' in txt:  # comment
            expected_remaining.add(ln)

    unexpected = [(ln, txt) for ln, txt in remaining_lines if ln not in expected_remaining]
    if unexpected:
        print(f"\n⚠️  {len(unexpected)} UNEXPECTED ej2_instances remaining:")
        for ln, txt in unexpected:
            print(f"  L{ln}: {txt}")
    else:
        print("✅ All remaining ej2_instances are expected (bridge function, upload, tooltip loop, comments).")

    print("\n" + "=" * 70)
    print("MIGRATION COMPLETE")
    print("=" * 70)


if __name__ == "__main__":
    main()
