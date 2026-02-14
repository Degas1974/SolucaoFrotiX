"""
migrate_dashboard_viagens.py
Migra referências ej2_instances de 4 ComboBox Syncfusion → Kendo em dashboard-viagens.js

Controles migrados:
  - lstFinalidadeAlteradaDashboard
  - lstMotoristaAlteradoDashboard
  - lstVeiculoAlteradoDashboard
  - lstRequisitanteAlteradoDashboard

NÃO TOCA em:
  - lstEventoDashboard (DropDownTree Syncfusion)
  - lstSetorSolicitanteAlteradoDashboard (DropDownTree Syncfusion)
  - Charts (linhas 1695-3207)
  - Comentários mencionando ej2_instances
"""

import shutil
import re

FILE = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\wwwroot\js\dashboards\dashboard-viagens.js"
BACKUP = FILE + ".bak_migrate_ej2"

# ── Read original ──────────────────────────────────────────────────────────────
with open(FILE, "r", encoding="utf-8") as f:
    content = f.read()

original = content  # keep for comparison

# ── Backup ─────────────────────────────────────────────────────────────────────
shutil.copy2(FILE, BACKUP)
print(f"✅ Backup criado: {BACKUP}")

replacements = 0

# ══════════════════════════════════════════════════════════════════════════════
# Block 1: Populate Finalidade in modal (~line 3641-3645)
# ══════════════════════════════════════════════════════════════════════════════
OLD_1 = """                        // Finalidade
                        const lstFinalidade = document.getElementById('lstFinalidadeAlteradaDashboard');
                        if (lstFinalidade && lstFinalidade.ej2_instances)
                        {
                            lstFinalidade.ej2_instances[0].value = viagem.finalidade || null;
                        }"""

NEW_1 = """                        // Finalidade
                        const lstFinalidadeKendo = $("#lstFinalidadeAlteradaDashboard").data("kendoComboBox");
                        if (lstFinalidadeKendo)
                        {
                            lstFinalidadeKendo.value(viagem.finalidade || "");
                        }"""

if OLD_1 in content:
    content = content.replace(OLD_1, NEW_1, 1)
    replacements += 1
    print("✅ Block 1: Finalidade populate in modal — migrado")
else:
    print("❌ Block 1: NÃO ENCONTRADO")

# ══════════════════════════════════════════════════════════════════════════════
# Block 2: Populate Motorista/Veiculo/Requisitante in setTimeout (~lines 3681-3698)
# ══════════════════════════════════════════════════════════════════════════════
OLD_2 = """                                // Motorista
                                const lstMotorista = document.getElementById('lstMotoristaAlteradoDashboard');
                                if (lstMotorista && lstMotorista.ej2_instances && viagem.motoristaId)
                                {
                                    lstMotorista.ej2_instances[0].value = viagem.motoristaId;
                                }

                                // Veículo
                                const lstVeiculo = document.getElementById('lstVeiculoAlteradoDashboard');
                                if (lstVeiculo && lstVeiculo.ej2_instances && viagem.veiculoId)
                                {
                                    lstVeiculo.ej2_instances[0].value = viagem.veiculoId;
                                }

                                // Solicitante (Requisitante)
                                const lstRequisitante = document.getElementById('lstRequisitanteAlteradoDashboard');
                                if (lstRequisitante && lstRequisitante.ej2_instances && viagem.requisitanteId)
                                {
                                    lstRequisitante.ej2_instances[0].value = viagem.requisitanteId;
                                }"""

NEW_2 = """                                // Motorista
                                const lstMotoristaKendo = $("#lstMotoristaAlteradoDashboard").data("kendoComboBox");
                                if (lstMotoristaKendo && viagem.motoristaId)
                                {
                                    lstMotoristaKendo.value(viagem.motoristaId.toString());
                                }

                                // Veículo
                                const lstVeiculoKendo = $("#lstVeiculoAlteradoDashboard").data("kendoComboBox");
                                if (lstVeiculoKendo && viagem.veiculoId)
                                {
                                    lstVeiculoKendo.value(viagem.veiculoId.toString());
                                }

                                // Solicitante (Requisitante)
                                const lstRequisitanteKendo = $("#lstRequisitanteAlteradoDashboard").data("kendoComboBox");
                                if (lstRequisitanteKendo && viagem.requisitanteId)
                                {
                                    lstRequisitanteKendo.value(viagem.requisitanteId.toString());
                                }"""

if OLD_2 in content:
    content = content.replace(OLD_2, NEW_2, 1)
    replacements += 1
    print("✅ Block 2: Motorista/Veiculo/Requisitante populate in setTimeout — migrado")
else:
    print("❌ Block 2: NÃO ENCONTRADO")

# ══════════════════════════════════════════════════════════════════════════════
# Block 3: FinalidadeChangeDashboard function (~lines 3739-3745)
# Only finalidadeCb changes; eventoDdt stays Syncfusion.
# finalidadeCb.value (property) → finalidadeCb.value() (method)
# ══════════════════════════════════════════════════════════════════════════════
OLD_3 = """        var finalidadeCb = document.getElementById('lstFinalidadeAlteradaDashboard').ej2_instances[0];
        var eventoDdt = document.getElementById('lstEventoDashboard').ej2_instances[0];

        if (finalidadeCb && eventoDdt)
        {
            if (finalidadeCb.value === 'Evento')"""

NEW_3 = """        var finalidadeCb = $("#lstFinalidadeAlteradaDashboard").data("kendoComboBox");
        var eventoDdt = document.getElementById('lstEventoDashboard').ej2_instances[0];

        if (finalidadeCb && eventoDdt)
        {
            if (finalidadeCb.value() === 'Evento')"""

if OLD_3 in content:
    content = content.replace(OLD_3, NEW_3, 1)
    replacements += 1
    print("✅ Block 3: FinalidadeChangeDashboard — migrado (finalidadeCb only)")
else:
    print("❌ Block 3: NÃO ENCONTRADO")

# ══════════════════════════════════════════════════════════════════════════════
# Block 4: gravarViagemDashboard — read Finalidade (~line 3773)
# ══════════════════════════════════════════════════════════════════════════════
OLD_4 = """        // Finalidade
        const lstFinalidade = document.getElementById('lstFinalidadeAlteradaDashboard');
        const finalidade = lstFinalidade && lstFinalidade.ej2_instances ? lstFinalidade.ej2_instances[0].value : null;"""

NEW_4 = """        // Finalidade
        const lstFinalidadeK = $("#lstFinalidadeAlteradaDashboard").data("kendoComboBox");
        const finalidade = lstFinalidadeK ? lstFinalidadeK.value() : null;"""

if OLD_4 in content:
    content = content.replace(OLD_4, NEW_4, 1)
    replacements += 1
    print("✅ Block 4: gravarViagemDashboard read Finalidade — migrado")
else:
    print("❌ Block 4: NÃO ENCONTRADO")

# ══════════════════════════════════════════════════════════════════════════════
# Block 5: gravarViagemDashboard — read Motorista (~line 3799)
# ══════════════════════════════════════════════════════════════════════════════
OLD_5 = """        // Motorista
        const lstMotorista = document.getElementById('lstMotoristaAlteradoDashboard');
        const motoristaId = lstMotorista && lstMotorista.ej2_instances ? lstMotorista.ej2_instances[0].value : null;"""

NEW_5 = """        // Motorista
        const lstMotoristaK = $("#lstMotoristaAlteradoDashboard").data("kendoComboBox");
        const motoristaId = lstMotoristaK ? lstMotoristaK.value() : null;"""

if OLD_5 in content:
    content = content.replace(OLD_5, NEW_5, 1)
    replacements += 1
    print("✅ Block 5: gravarViagemDashboard read Motorista — migrado")
else:
    print("❌ Block 5: NÃO ENCONTRADO")

# ══════════════════════════════════════════════════════════════════════════════
# Block 6: gravarViagemDashboard — read Veiculo (~line 3803)
# ══════════════════════════════════════════════════════════════════════════════
OLD_6 = """        // Veículo
        const lstVeiculo = document.getElementById('lstVeiculoAlteradoDashboard');
        const veiculoId = lstVeiculo && lstVeiculo.ej2_instances ? lstVeiculo.ej2_instances[0].value : null;"""

NEW_6 = """        // Veículo
        const lstVeiculoK = $("#lstVeiculoAlteradoDashboard").data("kendoComboBox");
        const veiculoId = lstVeiculoK ? lstVeiculoK.value() : null;"""

if OLD_6 in content:
    content = content.replace(OLD_6, NEW_6, 1)
    replacements += 1
    print("✅ Block 6: gravarViagemDashboard read Veiculo — migrado")
else:
    print("❌ Block 6: NÃO ENCONTRADO")

# ══════════════════════════════════════════════════════════════════════════════
# Block 7: gravarViagemDashboard — read Requisitante (~line 3819)
# ══════════════════════════════════════════════════════════════════════════════
OLD_7 = """        // Solicitante (Requisitante)
        const lstRequisitante = document.getElementById('lstRequisitanteAlteradoDashboard');
        const requisitanteId = lstRequisitante && lstRequisitante.ej2_instances ? lstRequisitante.ej2_instances[0].value : null;"""

NEW_7 = """        // Solicitante (Requisitante)
        const lstRequisitanteK = $("#lstRequisitanteAlteradoDashboard").data("kendoComboBox");
        const requisitanteId = lstRequisitanteK ? lstRequisitanteK.value() : null;"""

if OLD_7 in content:
    content = content.replace(OLD_7, NEW_7, 1)
    replacements += 1
    print("✅ Block 7: gravarViagemDashboard read Requisitante — migrado")
else:
    print("❌ Block 7: NÃO ENCONTRADO")

# ── Write result ───────────────────────────────────────────────────────────────
with open(FILE, "w", encoding="utf-8") as f:
    f.write(content)

print(f"\n{'='*70}")
print(f"📊 RESULTADO: {replacements}/7 blocos migrados com sucesso")
print(f"{'='*70}")

# ── Count remaining ej2_instances ──────────────────────────────────────────────
remaining = []
for i, line in enumerate(content.split('\n'), 1):
    if 'ej2_instances' in line:
        remaining.append((i, line.strip()[:120]))

print(f"\n📋 Referências ej2_instances restantes no arquivo: {len(remaining)}")
for lineno, text in remaining:
    # Classify: comment, chart, lstEvento, lstSetor, or UNEXPECTED
    tag = ""
    if text.startswith("//") or text.startswith("*") or text.startswith("/*"):
        tag = " [COMENTÁRIO]"
    elif "lstEvento" in text or "EventoDashboard" in text:
        tag = " [lstEvento — Syncfusion DropDownTree ✅]"
    elif "lstSetor" in text or "SetorSolicitante" in text:
        tag = " [lstSetor — Syncfusion DropDownTree ✅]"
    elif "destroy()" in text or "chart" in text.lower() or "Chart" in text:
        tag = " [Chart — Syncfusion ✅]"
    elif "Finalidade" in text or "Motorista" in text or "Veiculo" in text or "Requisitante" in text:
        tag = " [⚠️ INESPERADO — deveria ter sido migrado!]"
    else:
        tag = " [Outro Syncfusion]"
    print(f"  L{lineno}: {text}{tag}")

# ── Verify no target controls left with ej2_instances ──────────────────────────
MIGRATED_IDS = [
    "lstFinalidadeAlteradaDashboard",
    "lstMotoristaAlteradoDashboard",
    "lstVeiculoAlteradoDashboard",
    "lstRequisitanteAlteradoDashboard",
]
problems = []
for i, line in enumerate(content.split('\n'), 1):
    for ctrl_id in MIGRATED_IDS:
        if ctrl_id in line and 'ej2_instances' in line:
            problems.append((i, ctrl_id, line.strip()[:120]))

if problems:
    print(f"\n❌ PROBLEMA: {len(problems)} referências ej2_instances ainda existem para controles migrados:")
    for lineno, ctrl, text in problems:
        print(f"  L{lineno} [{ctrl}]: {text}")
else:
    print(f"\n✅ VERIFICAÇÃO: Nenhuma referência ej2_instances restante para os 4 controles migrados.")

print(f"\n✅ Migração concluída. Arquivo salvo.")
