#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Migrate ALL ej2_instances references in exibe-viagem.js
from Syncfusion EJ2 to Kendo UI jQuery API.

Widget mapping:
- lstRecorrente → kendoDropDownList
- lstMotorista → kendoComboBox
- lstVeiculo → kendoComboBox
- lstFinalidade → kendoDropDownList
- lstRequisitante → kendoComboBox (already migrated)
- rteDescricao → kendoEditor (or helpers)
- ddtCombustivelInicial → kendoDropDownList
- ddtCombustivelFinal → kendoDropDownList
- lstEventos → kendoComboBox
- txtDataInicioEvento → kendoDatePicker (helpers)
- txtDataFimEvento → kendoDatePicker (helpers)
- txtQtdParticipantesEvento → kendoNumericTextBox or .val()
- lstPeriodos → Syncfusion stays → window.getSyncfusionInstance()
- lstSetorRequisitanteAgendamento → Syncfusion stays
- lstDias → Syncfusion MultiSelect stays
- lstDiasMes → Syncfusion stays
- calDatasSelecionadas → Syncfusion Calendar stays
"""

import re
import sys
import os

filepath = os.path.join(os.path.dirname(os.path.abspath(__file__)),
                        'wwwroot', 'js', 'agendamento', 'components', 'exibe-viagem.js')

print(f"Reading: {filepath}")

with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

original = content
count_before = content.count('ej2_instances')
print(f"ej2_instances occurrences BEFORE: {count_before}")
print(f"File length: {len(content)} chars, {content.count(chr(10))} lines")

# =====================================================================
# HELPER: Generic pattern replacements
# =====================================================================

def replace_exact(old, new, label=""):
    global content
    if old in content:
        content = content.replace(old, new, 1)
        if label:
            print(f"  OK: {label}")
        return True
    else:
        if label:
            print(f"  SKIP (not found): {label}")
        return False


# =====================================================================
# STEP 1: HEADER COMMENTS
# =====================================================================
print("\n=== STEP 1: Header comments ===")

replace_exact(
    'etc.), Syncfusion components (ej2_instances, dataBind), jQuery',
    'etc.), Kendo UI jQuery widgets (.data("kendoXxx")), Syncfusion (DropDownTree, lstPeriodos), jQuery',
    "Header line ~22"
)

replace_exact(
    'Syncfusion EJ2 (DatePicker, TimePicker, DropDownList,\n *                   MultiSelect, Calendar, ListBox, RichTextEditor)',
    'Kendo UI (DropDownList, ComboBox, DatePicker, TimePicker,\n *                   NumericTextBox, Editor), Syncfusion EJ2 (DropDownTree, MultiSelect, Calendar)',
    "Header line ~26"
)

# =====================================================================
# STEP 2: inicializarLstRecorrente() - lines ~230-290
# =====================================================================
print("\n=== STEP 2: inicializarLstRecorrente() ===")

old_init_block = '''if (lstRecorrenteElement.ej2_instances && lstRecorrenteElement.ej2_instances[0])
        {
            const lstRecorrente = lstRecorrenteElement.ej2_instances[0];

            // Verificar se dataSource est\u00e1 vazio ou undefined
            if (!lstRecorrente.dataSource || lstRecorrente.dataSource.length === 0)
            {
                console.log("\ud83d\udd04 lstRecorrente existe mas dataSource est\u00e1 vazio - populando...");
                lstRecorrente.dataSource = dataRecorrente;
                lstRecorrente.fields = { text: 'Descricao', value: 'RecorrenteId' };
                lstRecorrente.dataBind();
                console.log("\u2705 lstRecorrente dataSource populado");
            }
            else
            {
                console.log("\u2139\ufe0f lstRecorrente j\u00e1 tem dataSource");
            }

            return true;
        }
        else
        {
            // Se n\u00e3o existe inst\u00e2ncia, criar nova
            console.log("\ud83c\udd95 Criando nova inst\u00e2ncia de lstRecorrente...");

            const dropdown = new ej.dropdowns.DropDownList({
                dataSource: dataRecorrente,
                fields: { text: 'Descricao', value: 'RecorrenteId' },
                placeholder: 'Selecione...',
                popupHeight: '200px',
                cssClass: 'e-outline text-center',
                floatLabelType: 'Never',
                value: 'N' // Padr\u00e3o
            });

            dropdown.appendTo('#lstRecorrente');
            console.log("\u2705 lstRecorrente criado com sucesso");
            return true;
        }'''

new_init_block = '''// KENDO: lstRecorrente \u00e9 kendoDropDownList
        const ddlRecorrente = $("#lstRecorrente").data("kendoDropDownList");
        if (ddlRecorrente)
        {
            // Verificar se dataSource est\u00e1 vazio ou undefined
            if (!ddlRecorrente.dataSource || ddlRecorrente.dataSource.data().length === 0)
            {
                console.log("\ud83d\udd04 lstRecorrente existe mas dataSource est\u00e1 vazio - populando...");
                ddlRecorrente.dataSource.data(dataRecorrente);
                console.log("\u2705 lstRecorrente dataSource populado");
            }
            else
            {
                console.log("\u2139\ufe0f lstRecorrente j\u00e1 tem dataSource");
            }

            return true;
        }
        else
        {
            // Se n\u00e3o existe inst\u00e2ncia Kendo, inicializar via jQuery
            console.log("\ud83c\udd95 Criando nova inst\u00e2ncia de lstRecorrente via Kendo...");

            $("#lstRecorrente").kendoDropDownList({
                dataSource: dataRecorrente,
                dataTextField: "Descricao",
                dataValueField: "RecorrenteId",
                optionLabel: "Selecione...",
                height: 200,
                value: "N"
            });

            console.log("\u2705 lstRecorrente criado com sucesso");
            return true;
        }'''

replace_exact(old_init_block, new_init_block, "inicializarLstRecorrente full block")

# =====================================================================
# STEP 3: exibirNovaViagem - Limpar lstRecorrente (lines ~553-557)
# =====================================================================
print("\n=== STEP 3: exibirNovaViagem - Limpar lstRecorrente ===")

replace_exact(
    '''const lstRecorrenteEl = document.getElementById("lstRecorrente");
            if (lstRecorrenteEl && lstRecorrenteEl.ej2_instances && lstRecorrenteEl.ej2_instances[0])
            {
                lstRecorrenteEl.ej2_instances[0].value = "N";
                lstRecorrenteEl.ej2_instances[0].dataBind();''',
    '''// KENDO: lstRecorrente \u00e9 kendoDropDownList
            const ddlRecorrenteNovo = $("#lstRecorrente").data("kendoDropDownList");
            if (ddlRecorrenteNovo)
            {
                ddlRecorrenteNovo.value("N");''',
    "exibirNovaViagem - limpar lstRecorrente"
)

# =====================================================================
# STEP 4: exibirNovaViagem - Limpar lstPeriodos (lines ~565-569)
# =====================================================================
print("\n=== STEP 4: exibirNovaViagem - Limpar lstPeriodos ===")

replace_exact(
    '''const lstPeriodosEl = document.getElementById("lstPeriodos");
            if (lstPeriodosEl && lstPeriodosEl.ej2_instances && lstPeriodosEl.ej2_instances[0])
            {
                lstPeriodosEl.ej2_instances[0].value = "1";
                lstPeriodosEl.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstPeriodos permanece Syncfusion
            const lstPeriodosInst = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
            if (lstPeriodosInst)
            {
                lstPeriodosInst.value = "1";
                lstPeriodosInst.dataBind();''',
    "exibirNovaViagem - limpar lstPeriodos"
)

# =====================================================================
# STEP 5: exibirNovaViagem - Limpar Motorista (lines ~363-383)
# =====================================================================
print("\n=== STEP 5: exibirNovaViagem - Limpar Motorista ===")

replace_exact(
    '''const lstMotorista = document.getElementById("lstMotorista");
            if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0])
            {
                const motoristaInst = lstMotorista.ej2_instances[0];
                motoristaInst.value = null;
                motoristaInst.text = '';
                motoristaInst.index = null;

                if (typeof motoristaInst.dataBind === 'function')
                {
                    motoristaInst.dataBind();
                }

                if (typeof motoristaInst.clear === 'function')
                {
                    motoristaInst.clear();
                }''',
    '''// KENDO: lstMotorista \u00e9 kendoComboBox
            const motoristaWidget = $("#lstMotorista").data("kendoComboBox");
            if (motoristaWidget)
            {
                motoristaWidget.value(null);
                motoristaWidget.text('');''',
    "exibirNovaViagem - limpar Motorista"
)

# =====================================================================
# STEP 6: exibirNovaViagem - Limpar Ve\u00edculo (lines ~385-405)
# =====================================================================
print("\n=== STEP 6: exibirNovaViagem - Limpar Ve\u00edculo ===")

replace_exact(
    '''const lstVeiculo = document.getElementById("lstVeiculo");
            if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0])
            {
                const veiculoInst = lstVeiculo.ej2_instances[0];
                veiculoInst.value = null;
                veiculoInst.text = '';
                veiculoInst.index = null;

                if (typeof veiculoInst.dataBind === 'function')
                {
                    veiculoInst.dataBind();
                }

                if (typeof veiculoInst.clear === 'function')
                {
                    veiculoInst.clear();
                }''',
    '''// KENDO: lstVeiculo \u00e9 kendoComboBox
            const veiculoWidget = $("#lstVeiculo").data("kendoComboBox");
            if (veiculoWidget)
            {
                veiculoWidget.value(null);
                veiculoWidget.text('');''',
    "exibirNovaViagem - limpar Ve\u00edculo"
)

# Save intermediate
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

count_mid = content.count('ej2_instances')
print(f"\nej2_instances after steps 1-6: {count_mid} (removed {count_before - count_mid})")

# =====================================================================
# STEP 7: exibirViagemExistente - Setar Motorista (lines ~912-926)
# =====================================================================
print("\n=== STEP 7: exibirViagemExistente - Setar Motorista ===")

replace_exact(
    '''const motorista = document.getElementById("lstMotorista");
            if (motorista && motorista.ej2_instances && motorista.ej2_instances[0])
            {
                motorista.ej2_instances[0].value = objViagem.motoristaId;
                motorista.ej2_instances[0].dataBind();''',
    '''// KENDO: lstMotorista \u00e9 kendoComboBox
            const motoristaExist = $("#lstMotorista").data("kendoComboBox");
            if (motoristaExist)
            {
                motoristaExist.value(objViagem.motoristaId);''',
    "exibirViagemExistente - setar Motorista"
)

# =====================================================================
# STEP 8: exibirViagemExistente - Setar Ve\u00edculo (lines ~923-937)
# =====================================================================
print("\n=== STEP 8: exibirViagemExistente - Setar Ve\u00edculo ===")

replace_exact(
    '''const veiculo = document.getElementById("lstVeiculo");
            if (veiculo && veiculo.ej2_instances && veiculo.ej2_instances[0])
            {
                veiculo.ej2_instances[0].value = objViagem.veiculoId;
                veiculo.ej2_instances[0].dataBind();''',
    '''// KENDO: lstVeiculo \u00e9 kendoComboBox
            const veiculoExist = $("#lstVeiculo").data("kendoComboBox");
            if (veiculoExist)
            {
                veiculoExist.value(objViagem.veiculoId);''',
    "exibirViagemExistente - setar Ve\u00edculo"
)

# =====================================================================
# STEP 9: exibirViagemExistente - Setar Finalidade (lines ~932-936)
# =====================================================================
print("\n=== STEP 9: exibirViagemExistente - Setar Finalidade ===")

replace_exact(
    '''const finalidade = document.getElementById("lstFinalidade");
            if (finalidade && finalidade.ej2_instances && finalidade.ej2_instances[0])
            {
                finalidade.ej2_instances[0].value = objViagem.finalidade;
                finalidade.ej2_instances[0].dataBind();''',
    '''// KENDO: lstFinalidade \u00e9 kendoDropDownList
            const ddlFinalidade = $("#lstFinalidade").data("kendoDropDownList");
            if (ddlFinalidade)
            {
                ddlFinalidade.value(objViagem.finalidade);''',
    "exibirViagemExistente - setar Finalidade"
)

# =====================================================================
# STEP 10: exibirViagemExistente - Setar Setor (lines ~997-1004)
# =====================================================================
print("\n=== STEP 10: exibirViagemExistente - Setar Setor ===")

replace_exact(
    '''const lstSetor = document.getElementById("lstSetorRequisitanteAgendamento");
            if (lstSetor && lstSetor.ej2_instances && lstSetor.ej2_instances[0])
            {
                lstSetor.ej2_instances[0].value = [objViagem.setorRequisitanteId];
                lstSetor.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstSetorRequisitanteAgendamento permanece Syncfusion DropDownTree
            const lstSetorInst = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteAgendamento") : null;
            if (lstSetorInst)
            {
                lstSetorInst.value = [objViagem.setorRequisitanteId];
                lstSetorInst.dataBind();''',
    "exibirViagemExistente - setar Setor"
)

# =====================================================================
# STEP 11: exibirViagemExistente - Setar RTE Descri\u00e7\u00e3o (lines ~1125-1128)
# =====================================================================
print("\n=== STEP 11: exibirViagemExistente - Setar RTE ===")

replace_exact(
    '''const rteDescricao = document.getElementById("rteDescricao");
            if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances[0])
            {
                rteDescricao.ej2_instances[0].value = objViagem.descricao || '';''',
    '''// KENDO: rteDescricao \u00e9 kendoEditor (via helper setEditorUpsertValue)
            const editorRte = $("#rteDescricao").data("kendoEditor");
            if (editorRte)
            {
                editorRte.value(objViagem.descricao || '');''',
    "exibirViagemExistente - setar RTE"
)

# Save intermediate
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

count_mid2 = content.count('ej2_instances')
print(f"\nej2_instances after steps 7-11: {count_mid2} (removed {count_mid - count_mid2})")

# =====================================================================
# STEP 12: Setar Combust\u00edvel Inicial (lines ~1156-1159)
# =====================================================================
print("\n=== STEP 12: Setar Combust\u00edvel Inicial ===")

replace_exact(
    '''const ddtCombInicial = document.getElementById("ddtCombustivelInicial");
            if (ddtCombInicial && ddtCombInicial.ej2_instances && ddtCombInicial.ej2_instances[0])
            {
                ddtCombInicial.ej2_instances[0].value = objViagem.combustivelInicial || '';
                ddtCombInicial.ej2_instances[0].dataBind();''',
    '''// KENDO: ddtCombustivelInicial \u00e9 kendoDropDownList
            const ddlCombIni = $("#ddtCombustivelInicial").data("kendoDropDownList");
            if (ddlCombIni)
            {
                ddlCombIni.value(objViagem.combustivelInicial || '');''',
    "Setar Combust\u00edvel Inicial"
)

# =====================================================================
# STEP 13: Setar Combust\u00edvel Final (lines ~1166-1169)
# =====================================================================
print("\n=== STEP 13: Setar Combust\u00edvel Final ===")

replace_exact(
    '''const ddtCombFinal = document.getElementById("ddtCombustivelFinal");
            if (ddtCombFinal && ddtCombFinal.ej2_instances && ddtCombFinal.ej2_instances[0])
            {
                ddtCombFinal.ej2_instances[0].value = objViagem.combustivelFinal || '';
                ddtCombFinal.ej2_instances[0].dataBind();''',
    '''// KENDO: ddtCombustivelFinal \u00e9 kendoDropDownList
            const ddlCombFim = $("#ddtCombustivelFinal").data("kendoDropDownList");
            if (ddlCombFim)
            {
                ddlCombFim.value(objViagem.combustivelFinal || '');''',
    "Setar Combust\u00edvel Final"
)

# =====================================================================
# STEP 14: Setar lstEventos (lines ~1185-1192)
# =====================================================================
print("\n=== STEP 14: Setar lstEventos ===")

replace_exact(
    '''const lstEventos = document.getElementById("lstEventos");
                if (lstEventos && lstEventos.ej2_instances && lstEventos.ej2_instances[0])
                {
                    lstEventos.ej2_instances[0].value = objViagem.eventoId;
                    lstEventos.ej2_instances[0].dataBind();

                    // [UI] Disparar evento change manualmente
                    lstEventos.ej2_instances[0].change();''',
    '''// KENDO: lstEventos \u00e9 kendoComboBox
                const cmbEventos = $("#lstEventos").data("kendoComboBox");
                if (cmbEventos)
                {
                    cmbEventos.value(objViagem.eventoId);

                    // [UI] Disparar evento change manualmente
                    cmbEventos.trigger("change");''',
    "Setar lstEventos"
)

# =====================================================================
# STEP 15: Setar txtDataInicioEvento (lines ~1250-1275)
# =====================================================================
print("\n=== STEP 15: Setar txtDataInicioEvento ===")

replace_exact(
    '''const txtDataInicioEvento = document.getElementById("txtDataInicioEvento");
                    if (txtDataInicioEvento && txtDataInicioEvento.ej2_instances && txtDataInicioEvento.ej2_instances[0])
                    {
                        txtDataInicioEvento.ej2_instances[0].value = dtInicioEvento;
                        txtDataInicioEvento.ej2_instances[0].dataBind();''',
    '''// KENDO: txtDataInicioEvento \u00e9 kendoDatePicker (via helper)
                    if (window.setKendoDateValue)
                    {
                        window.setKendoDateValue("txtDataInicioEvento", dtInicioEvento);''',
    "Setar txtDataInicioEvento"
)

# Check if there's an enable block for txtDataInicioEvento
replace_exact(
    '''const txtDataInicioEventoPicker = document.getElementById("txtDataInicioEvento");
                    if (txtDataInicioEventoPicker && txtDataInicioEventoPicker.ej2_instances && txtDataInicioEventoPicker.ej2_instances[0])
                    {
                        txtDataInicioEventoPicker.ej2_instances[0].enabled = true;''',
    '''// KENDO: habilitar txtDataInicioEvento
                    if (window.enableKendoDatePicker)
                    {
                        window.enableKendoDatePicker("txtDataInicioEvento", true);''',
    "Enable txtDataInicioEvento"
)

# =====================================================================
# STEP 16: Setar txtDataFimEvento (lines ~1291-1314)
# =====================================================================
print("\n=== STEP 16: Setar txtDataFimEvento ===")

replace_exact(
    '''const txtDataFimEvento = document.getElementById("txtDataFimEvento");
                    if (txtDataFimEvento && txtDataFimEvento.ej2_instances && txtDataFimEvento.ej2_instances[0])
                    {
                        txtDataFimEvento.ej2_instances[0].value = dtFimEvento;
                        txtDataFimEvento.ej2_instances[0].dataBind();''',
    '''// KENDO: txtDataFimEvento \u00e9 kendoDatePicker (via helper)
                    if (window.setKendoDateValue)
                    {
                        window.setKendoDateValue("txtDataFimEvento", dtFimEvento);''',
    "Setar txtDataFimEvento"
)

replace_exact(
    '''const txtDataFimEventoPicker = document.getElementById("txtDataFimEvento");
                    if (txtDataFimEventoPicker && txtDataFimEventoPicker.ej2_instances && txtDataFimEventoPicker.ej2_instances[0])
                    {
                        txtDataFimEventoPicker.ej2_instances[0].enabled = true;''',
    '''// KENDO: habilitar txtDataFimEvento
                    if (window.enableKendoDatePicker)
                    {
                        window.enableKendoDatePicker("txtDataFimEvento", true);''',
    "Enable txtDataFimEvento"
)

# =====================================================================
# STEP 17: Setar txtQtdParticipantesEvento (lines ~1330-1335)
# =====================================================================
print("\n=== STEP 17: Setar txtQtdParticipantesEvento ===")

replace_exact(
    '''const txtQtd = document.getElementById("txtQtdParticipantesEvento");
                    if (txtQtd && txtQtd.ej2_instances && txtQtd.ej2_instances[0])
                    {
                        txtQtd.ej2_instances[0].value = objViagem.qtdParticipantesEvento || 0;
                        txtQtd.ej2_instances[0].dataBind();''',
    '''// KENDO: txtQtdParticipantesEvento - usar kendoNumericTextBox ou val()
                    const numQtd = $("#txtQtdParticipantesEvento").data("kendoNumericTextBox");
                    if (numQtd)
                    {
                        numQtd.value(objViagem.qtdParticipantesEvento || 0);''',
    "Setar txtQtdParticipantesEvento"
)

# Save intermediate
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

count_mid3 = content.count('ej2_instances')
print(f"\nej2_instances after steps 12-17: {count_mid3} (removed {count_mid2 - count_mid3})")

# =====================================================================
# STEP 18: Limpar lstEventos (exibirNovaViagem) (lines ~1365-1370)
# =====================================================================
print("\n=== STEP 18: Limpar lstEventos (exibirNovaViagem) ===")

replace_exact(
    '''const lstEventosLimpar = document.getElementById("lstEventos");
                if (lstEventosLimpar && lstEventosLimpar.ej2_instances && lstEventosLimpar.ej2_instances[0])
                {
                    lstEventosLimpar.ej2_instances[0].value = null;
                    lstEventosLimpar.ej2_instances[0].dataBind();''',
    '''// KENDO: Limpar lstEventos (kendoComboBox)
                const cmbEventosLimpar = $("#lstEventos").data("kendoComboBox");
                if (cmbEventosLimpar)
                {
                    cmbEventosLimpar.value(null);
                    cmbEventosLimpar.text('');''',
    "Limpar lstEventos"
)

# =====================================================================
# STEP 19: lstRecorrente em exibirViagemExistente (lines ~1399-1421)
# =====================================================================
print("\n=== STEP 19: lstRecorrente em exibirViagemExistente ===")

replace_exact(
    '''const recorrenteEl = document.getElementById("lstRecorrente");
            if (recorrenteEl && recorrenteEl.ej2_instances && recorrenteEl.ej2_instances[0])
            {
                recorrenteEl.ej2_instances[0].value = objViagem.recorrente || 'N';
                recorrenteEl.ej2_instances[0].dataBind();''',
    '''// KENDO: lstRecorrente \u00e9 kendoDropDownList
            const ddlRecorrenteExist = $("#lstRecorrente").data("kendoDropDownList");
            if (ddlRecorrenteExist)
            {
                ddlRecorrenteExist.value(objViagem.recorrente || 'N');''',
    "exibirViagemExistente - setar lstRecorrente"
)

# =====================================================================
# STEP 20: lstPeriodos em exibirViagemExistente (lines ~1417-1421)
# =====================================================================
print("\n=== STEP 20: lstPeriodos em exibirViagemExistente ===")

replace_exact(
    '''const periodosEl = document.getElementById("lstPeriodos");
                if (periodosEl && periodosEl.ej2_instances && periodosEl.ej2_instances[0])
                {
                    periodosEl.ej2_instances[0].value = objViagem.periodoRecorrente || '1';
                    periodosEl.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstPeriodos permanece Syncfusion
                const lstPeriodosExist = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
                if (lstPeriodosExist)
                {
                    lstPeriodosExist.value = objViagem.periodoRecorrente || '1';
                    lstPeriodosExist.dataBind();''',
    "exibirViagemExistente - setar lstPeriodos"
)

# =====================================================================
# STEP 21: lstRecorrente em configurarBotoesPorStatus (lines ~1446-1449)
# =====================================================================
print("\n=== STEP 21: lstRecorrente em configurarBotoesPorStatus ===")

replace_exact(
    '''const lstRecorrenteConf = document.getElementById("lstRecorrente");
            if (lstRecorrenteConf && lstRecorrenteConf.ej2_instances && lstRecorrenteConf.ej2_instances[0])
            {
                lstRecorrenteConf.ej2_instances[0].enabled = false;''',
    '''// KENDO: desabilitar lstRecorrente (kendoDropDownList)
            const ddlRecorrenteConf = $("#lstRecorrente").data("kendoDropDownList");
            if (ddlRecorrenteConf)
            {
                ddlRecorrenteConf.enable(false);''',
    "configurarBotoesPorStatus - desabilitar lstRecorrente"
)

# =====================================================================
# STEP 22: lstDias na seção configurarRecorrenciaVariada (lines ~1646-1663)
# =====================================================================
print("\n=== STEP 22: lstDias em configurarRecorrenciaVariada ===")

# The lstDias pattern with ej2_instances
replace_exact(
    '''const lstDiasEl = document.getElementById("lstDias");
            if (lstDiasEl && lstDiasEl.ej2_instances && lstDiasEl.ej2_instances[0])
            {
                lstDiasEl.ej2_instances[0].value = diasSelecionados;
                lstDiasEl.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstDias permanece Syncfusion MultiSelect
            const lstDiasInst = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstDias") : null;
            if (lstDiasInst)
            {
                lstDiasInst.value = diasSelecionados;
                lstDiasInst.dataBind();''',
    "configurarRecorrenciaVariada - setar lstDias"
)

# Save intermediate
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

count_mid4 = content.count('ej2_instances')
print(f"\nej2_instances after steps 18-22: {count_mid4} (removed {count_mid3 - count_mid4})")

print("\n=== STEPS 23+: Remaining ej2_instances blocks ===")

# =====================================================================
# Now handle remaining blocks from mostrarCamposViagem and beyond
# (lines ~2700+)
# =====================================================================

# STEP 23: mostrarCamposViagem - ddtCombustivelInicial (line ~2777)
print("\n=== STEP 23: mostrarCamposViagem - ddtCombustivelInicial ===")

replace_exact(
    '''const ddtCombIni = document.getElementById("ddtCombustivelInicial");
            if (ddtCombIni && ddtCombIni.ej2_instances && ddtCombIni.ej2_instances[0])
            {
                ddtCombIni.ej2_instances[0].enabled = mostrar;''',
    '''// KENDO: ddtCombustivelInicial \u00e9 kendoDropDownList
            const ddlCombIniCampos = $("#ddtCombustivelInicial").data("kendoDropDownList");
            if (ddlCombIniCampos)
            {
                ddlCombIniCampos.enable(mostrar);''',
    "mostrarCamposViagem - enable/disable ddtCombustivelInicial"
)

# STEP 24: mostrarCamposViagem - ddtCombustivelFinal (line ~2812)
print("\n=== STEP 24: mostrarCamposViagem - ddtCombustivelFinal ===")

replace_exact(
    '''const ddtCombFim = document.getElementById("ddtCombustivelFinal");
            if (ddtCombFim && ddtCombFim.ej2_instances && ddtCombFim.ej2_instances[0])
            {
                ddtCombFim.ej2_instances[0].enabled = mostrar;''',
    '''// KENDO: ddtCombustivelFinal \u00e9 kendoDropDownList
            const ddlCombFimCampos = $("#ddtCombustivelFinal").data("kendoDropDownList");
            if (ddlCombFimCampos)
            {
                ddlCombFimCampos.enable(mostrar);''',
    "mostrarCamposViagem - enable/disable ddtCombustivelFinal"
)

# =====================================================================
# STEP 25: configurarRecorrencia - lstRecorrente value+enable (lines ~2858-2873)
# =====================================================================
print("\n=== STEP 25: configurarRecorrencia - lstRecorrente ===")

replace_exact(
    '''const lstRecorrenteConfig = document.getElementById("lstRecorrente");
            if (lstRecorrenteConfig && lstRecorrenteConfig.ej2_instances && lstRecorrenteConfig.ej2_instances[0])
            {
                lstRecorrenteConfig.ej2_instances[0].value = valorRecorrente;
                lstRecorrenteConfig.ej2_instances[0].dataBind();''',
    '''// KENDO: lstRecorrente \u00e9 kendoDropDownList
            const ddlRecorrenteConfig = $("#lstRecorrente").data("kendoDropDownList");
            if (ddlRecorrenteConfig)
            {
                ddlRecorrenteConfig.value(valorRecorrente);''',
    "configurarRecorrencia - setar lstRecorrente"
)

# Enable/disable lstRecorrente in configurarRecorrencia  
replace_exact(
    '''const lstRecorrenteEnable = document.getElementById("lstRecorrente");
            if (lstRecorrenteEnable && lstRecorrenteEnable.ej2_instances && lstRecorrenteEnable.ej2_instances[0])
            {
                lstRecorrenteEnable.ej2_instances[0].enabled = habilitar;''',
    '''// KENDO: habilitar/desabilitar lstRecorrente
            const ddlRecorrenteEnable = $("#lstRecorrente").data("kendoDropDownList");
            if (ddlRecorrenteEnable)
            {
                ddlRecorrenteEnable.enable(habilitar);''',
    "configurarRecorrencia - enable/disable lstRecorrente"
)

# =====================================================================
# STEP 26: configurarRecorrencia - lstPeriodos (lines ~2885)
# =====================================================================
print("\n=== STEP 26: configurarRecorrencia - lstPeriodos ===")

replace_exact(
    '''const lstPeriodosConfig = document.getElementById("lstPeriodos");
            if (lstPeriodosConfig && lstPeriodosConfig.ej2_instances && lstPeriodosConfig.ej2_instances[0])
            {
                lstPeriodosConfig.ej2_instances[0].enabled = habilitar;''',
    '''// SYNCFUSION: lstPeriodos permanece Syncfusion
            const lstPeriodosConfig = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
            if (lstPeriodosConfig)
            {
                lstPeriodosConfig.enabled = habilitar;
                lstPeriodosConfig.dataBind();''',
    "configurarRecorrencia - enable/disable lstPeriodos"
)

# =====================================================================
# STEP 27: mostrarCamposRecorrenciaSemanal - lstDias (lines ~2961)
# =====================================================================
print("\n=== STEP 27: mostrarCamposRecorrenciaSemanal - lstDias ===")

replace_exact(
    '''const lstDiasSemanal = document.getElementById("lstDias");
            if (lstDiasSemanal && lstDiasSemanal.ej2_instances && lstDiasSemanal.ej2_instances[0])
            {
                lstDiasSemanal.ej2_instances[0].enabled = mostrar;''',
    '''// SYNCFUSION: lstDias permanece Syncfusion MultiSelect
            const lstDiasSemanal = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstDias") : null;
            if (lstDiasSemanal)
            {
                lstDiasSemanal.enabled = mostrar;
                lstDiasSemanal.dataBind();''',
    "mostrarCamposRecorrenciaSemanal - enable/disable lstDias"
)

# =====================================================================
# STEP 28: mostrarCamposRecorrenciaMensal - lstDiasMes (lines ~3029)
# =====================================================================
print("\n=== STEP 28: mostrarCamposRecorrenciaMensal - lstDiasMes ===")

replace_exact(
    '''const lstDiasMesEl = document.getElementById("lstDiasMes");
            if (lstDiasMesEl && lstDiasMesEl.ej2_instances && lstDiasMesEl.ej2_instances[0])
            {
                lstDiasMesEl.ej2_instances[0].enabled = mostrar;''',
    '''// SYNCFUSION: lstDiasMes permanece Syncfusion
            const lstDiasMesInst = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstDiasMes") : null;
            if (lstDiasMesInst)
            {
                lstDiasMesInst.enabled = mostrar;
                lstDiasMesInst.dataBind();''',
    "mostrarCamposRecorrenciaMensal - enable/disable lstDiasMes"
)

# Save intermediate
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

count_mid5 = content.count('ej2_instances')
print(f"\nej2_instances after steps 23-28: {count_mid5} (removed {count_mid4 - count_mid5})")

# =====================================================================
# STEP 29: restaurarDadosRecorrencia - lstRecorrente (lines ~3099-3113)
# =====================================================================
print("\n=== STEP 29: restaurarDadosRecorrencia - lstRecorrente ===")

replace_exact(
    '''const lstRecorrenteRestaura = document.getElementById("lstRecorrente");
            if (lstRecorrenteRestaura && lstRecorrenteRestaura.ej2_instances && lstRecorrenteRestaura.ej2_instances[0])
            {
                lstRecorrenteRestaura.ej2_instances[0].value = recorrente;
                lstRecorrenteRestaura.ej2_instances[0].dataBind();''',
    '''// KENDO: lstRecorrente \u00e9 kendoDropDownList
            const ddlRecorrenteRestaura = $("#lstRecorrente").data("kendoDropDownList");
            if (ddlRecorrenteRestaura)
            {
                ddlRecorrenteRestaura.value(recorrente);''',
    "restaurarDadosRecorrencia - setar lstRecorrente"
)

# =====================================================================
# STEP 30: restaurarDadosRecorrencia - lstPeriodos (lines ~3113)
# =====================================================================
print("\n=== STEP 30: restaurarDadosRecorrencia - lstPeriodos ===")

replace_exact(
    '''const lstPeriodosRestaura = document.getElementById("lstPeriodos");
            if (lstPeriodosRestaura && lstPeriodosRestaura.ej2_instances && lstPeriodosRestaura.ej2_instances[0])
            {
                lstPeriodosRestaura.ej2_instances[0].value = periodoRecorrente;
                lstPeriodosRestaura.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstPeriodos permanece Syncfusion
            const lstPeriodosRestaura = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
            if (lstPeriodosRestaura)
            {
                lstPeriodosRestaura.value = periodoRecorrente;
                lstPeriodosRestaura.dataBind();''',
    "restaurarDadosRecorrencia - setar lstPeriodos"
)

# =====================================================================
# STEP 31: restaurarRecorrenciaSemanal - lstDias (lines ~3148-3186)
# =====================================================================
print("\n=== STEP 31: restaurarRecorrenciaSemanal - lstDias ===")

replace_exact(
    '''const lstDiasRestaura = document.getElementById("lstDias");
            if (lstDiasRestaura && lstDiasRestaura.ej2_instances && lstDiasRestaura.ej2_instances[0])
            {
                lstDiasRestaura.ej2_instances[0].value = diasSelecionados;
                lstDiasRestaura.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstDias permanece Syncfusion MultiSelect
            const lstDiasRestaura = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstDias") : null;
            if (lstDiasRestaura)
            {
                lstDiasRestaura.value = diasSelecionados;
                lstDiasRestaura.dataBind();''',
    "restaurarRecorrenciaSemanal - setar lstDias"
)

# Second lstDias block in restaurarRecorrenciaSemanal (enable)
replace_exact(
    '''const lstDiasEnable = document.getElementById("lstDias");
            if (lstDiasEnable && lstDiasEnable.ej2_instances && lstDiasEnable.ej2_instances[0])
            {
                lstDiasEnable.ej2_instances[0].enabled = true;''',
    '''// SYNCFUSION: habilitar lstDias MultiSelect
            const lstDiasEnable = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstDias") : null;
            if (lstDiasEnable)
            {
                lstDiasEnable.enabled = true;
                lstDiasEnable.dataBind();''',
    "restaurarRecorrenciaSemanal - enable lstDias"
)

# =====================================================================
# STEP 32: garantirVisibilidadeRecorrencia - lstPeriodos (lines ~3205-3218)
# =====================================================================
print("\n=== STEP 32: garantirVisibilidadeRecorrencia - lstPeriodos ===")

replace_exact(
    '''const lstPeriodosGarantir = document.getElementById("lstPeriodos");
                if (lstPeriodosGarantir && lstPeriodosGarantir.ej2_instances && lstPeriodosGarantir.ej2_instances[0])
                {
                    lstPeriodosGarantir.ej2_instances[0].value = periodoValue;
                    lstPeriodosGarantir.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstPeriodos permanece Syncfusion
                const lstPeriodosGarantir = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
                if (lstPeriodosGarantir)
                {
                    lstPeriodosGarantir.value = periodoValue;
                    lstPeriodosGarantir.dataBind();''',
    "garantirVisibilidadeRecorrencia - setar lstPeriodos"
)

# txtFinalRecorrencia in garantirVisibilidadeRecorrencia
replace_exact(
    '''const txtFinal = document.getElementById("txtFinalRecorrencia");
                if (txtFinal && txtFinal.ej2_instances && txtFinal.ej2_instances[0])
                {
                    txtFinal.ej2_instances[0].value = finalValue;
                    txtFinal.ej2_instances[0].dataBind();''',
    '''// KENDO: txtFinalRecorrencia \u00e9 kendoDatePicker (via helper)
                if (window.setKendoDateValue)
                {
                    window.setKendoDateValue("txtFinalRecorrencia", finalValue);
                }
                if (true)
                {
                    // Placeholder para manter estrutura do if''',
    "garantirVisibilidadeRecorrencia - setar txtFinalRecorrencia"
)

# =====================================================================
# STEP 33: restaurarRecorrenciaSemanal - lstDias (second block, lines ~3307)
# =====================================================================
print("\n=== STEP 33: segunda ocorr\u00eancia lstDias em restaurarRecorrenciaSemanal ===")

replace_exact(
    '''const lstDiasSemanal2 = document.getElementById("lstDias");
            if (lstDiasSemanal2 && lstDiasSemanal2.ej2_instances && lstDiasSemanal2.ej2_instances[0])
            {
                lstDiasSemanal2.ej2_instances[0].value = diasArray;
                lstDiasSemanal2.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstDias permanece Syncfusion MultiSelect
            const lstDiasSemanal2 = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstDias") : null;
            if (lstDiasSemanal2)
            {
                lstDiasSemanal2.value = diasArray;
                lstDiasSemanal2.dataBind();''',
    "restaurarRecorrenciaSemanal - segunda setar lstDias"
)

# =====================================================================
# STEP 34: restaurarRecorrenciaMensal - lstDiasMes (lines ~3380)
# =====================================================================
print("\n=== STEP 34: restaurarRecorrenciaMensal - lstDiasMes ===")

replace_exact(
    '''const lstDiasMesRestaura = document.getElementById("lstDiasMes");
            if (lstDiasMesRestaura && lstDiasMesRestaura.ej2_instances && lstDiasMesRestaura.ej2_instances[0])
            {
                lstDiasMesRestaura.ej2_instances[0].value = diaDoMes;
                lstDiasMesRestaura.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: lstDiasMes permanece Syncfusion
            const lstDiasMesRestaura = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstDiasMes") : null;
            if (lstDiasMesRestaura)
            {
                lstDiasMesRestaura.value = diaDoMes;
                lstDiasMesRestaura.dataBind();''',
    "restaurarRecorrenciaMensal - setar lstDiasMes"
)

# =====================================================================
# STEP 35: restaurarRecorrenciaVariada - calDatasSelecionadas (lines ~3450)
# =====================================================================
print("\n=== STEP 35: restaurarRecorrenciaVariada - calDatasSelecionadas ===")

replace_exact(
    '''const calDatas = document.getElementById("calDatasSelecionadas");
            if (calDatas && calDatas.ej2_instances && calDatas.ej2_instances[0])
            {
                calDatas.ej2_instances[0].values = datasArray;
                calDatas.ej2_instances[0].dataBind();''',
    '''// SYNCFUSION: calDatasSelecionadas permanece Syncfusion Calendar
            const calDatasInst = window.getSyncfusionInstance ? window.getSyncfusionInstance("calDatasSelecionadas") : null;
            if (calDatasInst)
            {
                calDatasInst.values = datasArray;
                calDatasInst.dataBind();''',
    "restaurarRecorrenciaVariada - setar calDatasSelecionadas"
)

# =====================================================================
# FINAL: Save and verify
# =====================================================================
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

count_after = content.count('ej2_instances')
print(f"\n{'='*60}")
print(f"FINAL RESULT:")
print(f"  ej2_instances BEFORE: {count_before}")
print(f"  ej2_instances AFTER:  {count_after}")
print(f"  Removed:              {count_before - count_after}")
print(f"{'='*60}")

if count_after > 0:
    # Find remaining occurrences
    lines = content.split('\n')
    print(f"\nRemaining ej2_instances on lines:")
    for i, line in enumerate(lines, 1):
        if 'ej2_instances' in line:
            print(f"  Line {i}: {line.strip()[:100]}")
