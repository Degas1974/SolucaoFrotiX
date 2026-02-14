#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
migrate_viagens_001_014.py
==========================
Migrates 4 Syncfusion ej2_instances controls to Kendo ComboBox in:
  - viagens_001.js (double quotes)
  - viagens_014.js (single quotes)

Controls migrated:
  lstVeiculos  -> $("#lstVeiculos").data("kendoComboBox")
  lstMotorista -> $("#lstMotorista").data("kendoComboBox")
  lstEventos   -> $("#lstEventos").data("kendoComboBox")
  lstStatus    -> $("#lstStatus").data("kendoComboBox")

Controls NOT touched (stay Syncfusion):
  ddtCombustivelInicial, ddtCombustivelFinal, rteDescricao, rteOcorrencias
"""

import os
import re
import shutil

BASE = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\wwwroot\js\cadastros"

FILES = [
    os.path.join(BASE, "viagens_001.js"),
    os.path.join(BASE, "viagens_014.js"),
]

# Controls to migrate
CONTROLS = ["lstVeiculos", "lstMotorista", "lstEventos", "lstStatus"]

# Variable names used after initialization
# (the var name assigned from ej2_instances)
VAR_NAMES = ["veiculos", "motoristas", "eventos", "status"]

BACKUP_SUFFIX = ".bak_migrate_ej2"


def count_ej2(content: str) -> int:
    return content.count("ej2_instances")


def migrate_file(filepath: str) -> dict:
    """Migrate a single file. Returns stats dict."""
    print(f"\n{'='*70}")
    print(f"Processing: {os.path.basename(filepath)}")
    print(f"{'='*70}")

    # Read
    with open(filepath, "r", encoding="utf-8") as f:
        original = f.read()

    # Backup
    backup_path = filepath + BACKUP_SUFFIX
    shutil.copy2(filepath, backup_path)
    print(f"  Backup created: {os.path.basename(backup_path)}")

    ej2_before = count_ej2(original)
    print(f"  ej2_instances BEFORE: {ej2_before}")

    content = original
    total_replacements = 0

    # ── STEP 1: Replace ej2_instances initialization lines ──────────────
    # Handles both double and single quotes
    for ctrl in CONTROLS:
        # Pattern: document.getElementById("lstXxx").ej2_instances[0]
        #      or: document.getElementById('lstXxx').ej2_instances[0]
        pattern = rf"""document\.getElementById\(['"]{ctrl}['"]\)\.ej2_instances\[0\]"""
        replacement = f'$("#{ctrl}").data("kendoComboBox")'

        matches = len(re.findall(pattern, content))
        if matches > 0:
            content = re.sub(pattern, replacement, content)
            total_replacements += matches
            print(f"  [INIT] {ctrl}: {matches} ej2_instances → kendoComboBox")
        else:
            print(f"  [INIT] {ctrl}: no ej2_instances found (already migrated?)")

    # ── STEP 2: Replace .value property access → .value() method ───────
    # For each variable name, replace VARNAME.value with VARNAME.value()
    # ONLY when NOT already followed by ( (to avoid .value() → .value()())
    # and NOT followed by single = (assignment), but ALLOW == and ===
    for var in VAR_NAMES:
        # Pattern: varname.value NOT followed by ( and NOT followed by assignment (= but not == or ===)
        # (?!\s*\() avoids double-wrapping .value()
        # (?!\s*=[^=]) avoids .value = x (assignment) but allows .value == and .value ===
        pattern = rf'\b{var}\.value\b(?!\s*\()(?!\s*=[^=])'
        replacement = f'{var}.value()'

        matches = len(re.findall(pattern, content))
        if matches > 0:
            content = re.sub(pattern, replacement, content)
            total_replacements += matches
            print(f"  [VALUE] {var}.value → {var}.value(): {matches} replacements")
        else:
            print(f"  [VALUE] {var}.value: no matches (already migrated?)")

    # ── STEP 3: Verify remaining ej2_instances ──────────────────────────
    ej2_after = count_ej2(content)

    # Identify which controls still have ej2_instances
    remaining = re.findall(
        r"""document\.getElementById\(['"]([\w]+)['"]\)\.ej2_instances""",
        content
    )
    remaining_unique = sorted(set(remaining))

    # ── STEP 4: Write result ────────────────────────────────────────────
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(content)

    print(f"\n  Total replacements: {total_replacements}")
    print(f"  ej2_instances AFTER: {ej2_after} (was {ej2_before}, removed {ej2_before - ej2_after})")

    if remaining_unique:
        print(f"  Remaining ej2_instances controls (Syncfusion, untouched):")
        for ctrl in remaining_unique:
            occurrences = remaining.count(ctrl)
            print(f"    - {ctrl}: {occurrences} occurrence(s)")
    else:
        print(f"  No remaining ej2_instances (all migrated)")

    # Verify none of our 4 controls remain
    leaked = [c for c in CONTROLS if c in remaining_unique]
    if leaked:
        print(f"\n  ⚠️ WARNING: These controls still have ej2_instances: {leaked}")
    else:
        print(f"  ✅ All 4 target controls migrated successfully")

    return {
        "file": os.path.basename(filepath),
        "replacements": total_replacements,
        "ej2_before": ej2_before,
        "ej2_after": ej2_after,
        "remaining_controls": remaining_unique,
        "leaked": leaked,
    }


def main():
    print("=" * 70)
    print("FrotiX Migration: Syncfusion ej2_instances → Kendo ComboBox")
    print("Controls: lstVeiculos, lstMotorista, lstEventos, lstStatus")
    print("=" * 70)

    results = []
    for fpath in FILES:
        if not os.path.exists(fpath):
            print(f"\n  ❌ File not found: {fpath}")
            continue
        stats = migrate_file(fpath)
        results.append(stats)

    # ── Summary ─────────────────────────────────────────────────────────
    print("\n" + "=" * 70)
    print("SUMMARY")
    print("=" * 70)
    for r in results:
        print(f"\n  📄 {r['file']}:")
        print(f"     Replacements:  {r['replacements']}")
        print(f"     ej2_instances: {r['ej2_before']} → {r['ej2_after']} (removed {r['ej2_before'] - r['ej2_after']})")
        print(f"     Remaining Syncfusion controls: {', '.join(r['remaining_controls']) if r['remaining_controls'] else 'none'}")
        if r['leaked']:
            print(f"     ⚠️ LEAKED (still ej2): {', '.join(r['leaked'])}")
        else:
            print(f"     ✅ All 4 targets migrated OK")

    all_ok = all(len(r['leaked']) == 0 for r in results)
    print(f"\n  {'✅ MIGRATION COMPLETE — ALL OK' if all_ok else '⚠️ MIGRATION COMPLETED WITH WARNINGS'}")
    print("=" * 70)


if __name__ == "__main__":
    main()
