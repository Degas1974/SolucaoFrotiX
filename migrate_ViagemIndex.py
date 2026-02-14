"""
migrate_ViagemIndex.py
─────────────────────
Migra referências ej2_instances de controles Kendo ComboBox em ViagemIndex.js.

Controles migrados (já são Kendo ComboBox):
  - lstVeiculos, lstMotorista, lstEventos, lstStatus

Controles que PERMANECEM Syncfusion (NÃO tocados):
  - ddtCombustivelInicial, ddtCombustivelFinal (DropDownTree)
  - rteDescricao (RichTextEditor)
  - combInicial, combFinal, combKm (variáveis Syncfusion)
  - Generic cleanup loops (linhas 2848-2852)
  - Generic helpers (linha 2058)
  - niveis/descricao (linhas 2215, 2257)
"""

import shutil
import os

FILE_PATH = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\wwwroot\js\cadastros\ViagemIndex.js"
BACKUP_EXT = ".bak_migrate_ej2"

def main():
    backup_path = FILE_PATH + BACKUP_EXT
    print("=" * 70)
    print("  migrate_ViagemIndex.py — ej2_instances → Kendo ComboBox")
    print("=" * 70)

    # ── 1. Backup ──────────────────────────────────────────────────────
    shutil.copy2(FILE_PATH, backup_path)
    print(f"\n✅ Backup criado: {os.path.basename(backup_path)}")

    # ── 2. Ler arquivo ─────────────────────────────────────────────────
    with open(FILE_PATH, "r", encoding="utf-8") as f:
        content = f.read()

    original_count = content.count("ej2_instances")
    print(f"📊 Total ej2_instances ANTES: {original_count}")

    count = 0

    # ── 3. Substituir bloco veiculosCombo ──────────────────────────────
    old_veiculos = (
        '        let veiculoId = "";\n'
        '        const veiculosCombo = document.getElementById("lstVeiculos");\n'
        '        if (veiculosCombo && veiculosCombo.ej2_instances && veiculosCombo.ej2_instances.length > 0)\n'
        '        {\n'
        '            const combo = veiculosCombo.ej2_instances[0];\n'
        '            if (combo.value != null && combo.value !== "")\n'
        '            {\n'
        '                veiculoId = combo.value;\n'
        '            }\n'
        '        }'
    )
    new_veiculos = (
        '        let veiculoId = "";\n'
        '        const veiculosComboKendo = $("#lstVeiculos").data("kendoComboBox");\n'
        '        if (veiculosComboKendo)\n'
        '        {\n'
        '            const val = veiculosComboKendo.value();\n'
        '            if (val != null && val !== "")\n'
        '            {\n'
        '                veiculoId = val;\n'
        '            }\n'
        '        }'
    )

    if old_veiculos in content:
        content = content.replace(old_veiculos, new_veiculos, 1)
        count += 1
        print("  ✅ Replaced: veiculosCombo (lstVeiculos)")
    else:
        print("  ❌ NOT FOUND: veiculosCombo block")

    # ── 4. Substituir bloco motoristasCombo ────────────────────────────
    old_motorista = (
        '        let motoristaId = "";\n'
        '        const motoristasCombo = document.getElementById("lstMotorista");\n'
        '        if (motoristasCombo && motoristasCombo.ej2_instances && motoristasCombo.ej2_instances.length > 0)\n'
        '        {\n'
        '            const combo = motoristasCombo.ej2_instances[0];\n'
        '            if (combo.value != null && combo.value !== "")\n'
        '            {\n'
        '                motoristaId = combo.value;\n'
        '            }\n'
        '        }'
    )
    new_motorista = (
        '        let motoristaId = "";\n'
        '        const motoristasComboKendo = $("#lstMotorista").data("kendoComboBox");\n'
        '        if (motoristasComboKendo)\n'
        '        {\n'
        '            const val = motoristasComboKendo.value();\n'
        '            if (val != null && val !== "")\n'
        '            {\n'
        '                motoristaId = val;\n'
        '            }\n'
        '        }'
    )

    if old_motorista in content:
        content = content.replace(old_motorista, new_motorista, 1)
        count += 1
        print("  ✅ Replaced: motoristasCombo (lstMotorista)")
    else:
        print("  ❌ NOT FOUND: motoristasCombo block")

    # ── 5. Substituir bloco eventosCombo ───────────────────────────────
    old_eventos = (
        '        let eventoId = "";\n'
        '        const eventosCombo = document.getElementById("lstEventos");\n'
        '        if (eventosCombo && eventosCombo.ej2_instances && eventosCombo.ej2_instances.length > 0)\n'
        '        {\n'
        '            const combo = eventosCombo.ej2_instances[0];\n'
        '            if (combo.value != null && combo.value !== "")\n'
        '            {\n'
        '                eventoId = combo.value;\n'
        '            }\n'
        '        }'
    )
    new_eventos = (
        '        let eventoId = "";\n'
        '        const eventosComboKendo = $("#lstEventos").data("kendoComboBox");\n'
        '        if (eventosComboKendo)\n'
        '        {\n'
        '            const val = eventosComboKendo.value();\n'
        '            if (val != null && val !== "")\n'
        '            {\n'
        '                eventoId = val;\n'
        '            }\n'
        '        }'
    )

    if old_eventos in content:
        content = content.replace(old_eventos, new_eventos, 1)
        count += 1
        print("  ✅ Replaced: eventosCombo (lstEventos)")
    else:
        print("  ❌ NOT FOUND: eventosCombo block")

    # ── 6. Substituir bloco statusCombo ────────────────────────────────
    old_status = (
        '        let statusId = "Aberta";\n'
        '        const statusCombo = document.getElementById("lstStatus");\n'
        '        if (statusCombo && statusCombo.ej2_instances && statusCombo.ej2_instances.length > 0)\n'
        '        {\n'
        '            const status = statusCombo.ej2_instances[0];\n'
        '            if (status.value === "" || status.value === null)\n'
        '            {\n'
        '                if (motoristaId || veiculoId || eventoId || ($("#txtData").val() != null && $("#txtData").val() !== ""))\n'
        '                {\n'
        '                    statusId = "Todas";\n'
        '                }\n'
        '            } else\n'
        '            {\n'
        '                statusId = status.value;\n'
        '            }\n'
        '        }'
    )
    new_status = (
        '        let statusId = "Aberta";\n'
        '        const statusComboKendo = $("#lstStatus").data("kendoComboBox");\n'
        '        if (statusComboKendo)\n'
        '        {\n'
        '            const statusVal = statusComboKendo.value();\n'
        '            if (statusVal === "" || statusVal === null)\n'
        '            {\n'
        '                if (motoristaId || veiculoId || eventoId || ($("#txtData").val() != null && $("#txtData").val() !== ""))\n'
        '                {\n'
        '                    statusId = "Todas";\n'
        '                }\n'
        '            } else\n'
        '            {\n'
        '                statusId = statusVal;\n'
        '            }\n'
        '        }'
    )

    if old_status in content:
        content = content.replace(old_status, new_status, 1)
        count += 1
        print("  ✅ Replaced: statusCombo (lstStatus)")
    else:
        print("  ❌ NOT FOUND: statusCombo block")

    # ── 7. Escrever arquivo ────────────────────────────────────────────
    with open(FILE_PATH, "w", encoding="utf-8") as f:
        f.write(content)

    # ── 8. Relatório ───────────────────────────────────────────────────
    final_count = content.count("ej2_instances")
    removed = original_count - final_count

    print(f"\n{'=' * 70}")
    print(f"  RESULTADO")
    print(f"{'=' * 70}")
    print(f"  Blocos substituídos: {count}/4")
    print(f"  ej2_instances removidos: {removed}")
    print(f"  ej2_instances restantes: {final_count}")
    print()

    # ── 9. Listar restantes ────────────────────────────────────────────
    print("  📋 ej2_instances restantes (todos devem ser Syncfusion):")
    for i, line in enumerate(content.split("\n"), 1):
        if "ej2_instances" in line:
            trimmed = line.strip()[:120]
            print(f"     L{i}: {trimmed}")

    # ── 10. Verificar se algum Kendo control ficou para trás ───────────
    kendo_controls = ["lstVeiculos", "lstMotorista", "lstStatus", "lstEventos"]
    orphans = []
    for i, line in enumerate(content.split("\n"), 1):
        if "ej2_instances" in line:
            for ctrl in kendo_controls:
                if ctrl in line:
                    orphans.append((i, ctrl, line.strip()[:120]))

    if orphans:
        print(f"\n  ⚠️  ATENÇÃO: {len(orphans)} referência(s) ej2_instances para controles Kendo ainda presentes!")
        for ln, ctrl, txt in orphans:
            print(f"     L{ln} [{ctrl}]: {txt}")
    else:
        print(f"\n  ✅ Nenhuma referência ej2_instances para controles Kendo restante.")

    print(f"\n{'=' * 70}")
    print(f"  Migração concluída com sucesso!")
    print(f"{'=' * 70}")


if __name__ == "__main__":
    main()
