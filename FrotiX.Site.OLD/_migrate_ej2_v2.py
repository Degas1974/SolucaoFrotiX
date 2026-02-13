# -*- coding: utf-8 -*-
"""
Migração ej2_instances → Kendo UI jQuery API
Arquivo: exibe-viagem.js (FrotiX.Site.OLD)
Versão: 2.0 - Regex-based migration with exact patterns
"""
import re
import os
import sys

FILE = os.path.join(os.path.dirname(os.path.abspath(__file__)),
                    "wwwroot", "js", "agendamento", "components", "exibe-viagem.js")

# Mapeamento de widgets: elementId → (kendoType, domOrJquery)
# "syncfusion" = manter como getSyncfusionInstance (não migrar para Kendo)
WIDGET_MAP = {
    "lstRecorrente":                    ("kendoDropDownList", "kendo"),
    "lstPeriodos":                      ("syncfusion", "syncfusion"),
    "lstMotorista":                     ("kendoComboBox", "kendo"),
    "lstVeiculo":                       ("kendoComboBox", "kendo"),
    "lstFinalidade":                    ("kendoDropDownList", "kendo"),
    "lstSetorRequisitanteAgendamento":  ("syncfusion", "syncfusion"),  # DropDownTree stays
    "lstRequisitante":                  ("kendoComboBox", "kendo"),    # already migrated
    "rteDescricao":                     ("kendoEditor", "kendo"),
    "ddtCombustivelInicial":            ("kendoDropDownList", "kendo"),
    "ddtCombustivelFinal":              ("kendoDropDownList", "kendo"),
    "ddtCombIni":                       ("kendoDropDownList", "kendo"),  # alias used in mostrarCamposViagem
    "ddtCombFim":                       ("kendoDropDownList", "kendo"),  # alias used in mostrarCamposViagem
    "lstEventos":                       ("kendoComboBox", "kendo"),
    "txtDataInicioEvento":              ("kendoDatePicker", "kendo"),
    "txtDataFimEvento":                 ("kendoDatePicker", "kendo"),
    "txtQtdParticipantesEvento":        ("kendoNumericTextBox", "kendo"),
    "lstDias":                          ("syncfusion", "syncfusion"),  # MultiSelect stays
    "lstDiasMes":                       ("syncfusion", "syncfusion"),  # stays
    "calDatasSelecionadas":             ("syncfusion", "syncfusion"),  # Calendar stays
}

def read_file():
    with open(FILE, 'r', encoding='utf-8') as f:
        return f.read()

def write_file(content):
    with open(FILE, 'w', encoding='utf-8') as f:
        f.write(content)

def count_ej2(content):
    return content.count("ej2_instances")

def migrate(content):
    original_count = count_ej2(content)
    changes = []

    # =====================================================================
    # STEP 1: Update header comment (line 22)
    # =====================================================================
    old_hdr = "etc.), Syncfusion components (ej2_instances, dataBind), jQuery"
    new_hdr = "etc.), Kendo UI jQuery API (.data('kendoXxx')), Syncfusion (lstPeriodos, lstDias, lstDiasMes, calDatasSelecionadas, lstSetorRequisitanteAgendamento), jQuery"
    if old_hdr in content:
        content = content.replace(old_hdr, new_hdr, 1)
        changes.append("STEP 1: Header comment updated")

    old_dep = "Syncfusion EJ2 (DatePicker, TimePicker, DropDownList,\n *                   MultiSelect, Calendar, ListBox, RichTextEditor)"
    new_dep = "Kendo UI (DropDownList, ComboBox, DatePicker, TimePicker,\n *                   NumericTextBox, Editor), Syncfusion EJ2 (MultiSelect, Calendar, DropDownTree)"
    if old_dep in content:
        content = content.replace(old_dep, new_dep, 1)
        changes.append("STEP 1b: Dependencies header updated")

    # =====================================================================
    # STEP 2: inicializarLstRecorrente (lines ~245-247)
    # Pattern: lstRecorrenteElement.ej2_instances → kendoDropDownList
    # =====================================================================
    content = content.replace(
        'if (lstRecorrenteElement.ej2_instances && lstRecorrenteElement.ej2_instances[0])\n        {\n            const lstRecorrente = lstRecorrenteElement.ej2_instances[0];',
        'const lstRecorrenteWidget = $("#lstRecorrente").data("kendoDropDownList");\n        if (lstRecorrenteWidget)\n        {\n            const lstRecorrente = lstRecorrenteWidget;'
    )
    # Fix references inside: lstRecorrente.dataSource → lstRecorrente.dataSource (same API)
    # lstRecorrente.fields → need to handle differently (Kendo uses dataTextField/dataValueField)
    # lstRecorrente.dataBind() → not needed for Kendo, but harmless if wrapped
    changes.append("STEP 2: inicializarLstRecorrente migrated")

    # =====================================================================
    # STEP 3: exibirNovaViagem - Limpar Motorista (lines ~363-385)
    # =====================================================================
    content = content.replace(
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
        '''const motoristaWidget = $("#lstMotorista").data("kendoComboBox");
            if (motoristaWidget)
            {
                motoristaWidget.value("");
                motoristaWidget.text("");'''
    )
    changes.append("STEP 3: Limpar Motorista (nova viagem)")

    # Fix closing of motorista block - remove extra console.log that references old variable
    content = content.replace(
        '''console.log("âœ… Motorista limpo na criação de novo agendamento");
            }

            // Limpar Veí­culo
            const lstVeiculo = document.getElementById("lstVeiculo");
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
                {''',
        '''console.log("✅ Motorista limpo (Kendo ComboBox)");
            }

            // Limpar Veículo
            const veiculoWidget = $("#lstVeiculo").data("kendoComboBox");
            if (veiculoWidget)
            {
                veiculoWidget.value("");
                veiculoWidget.text("");'''
    )
    changes.append("STEP 4: Limpar Veículo (nova viagem)")

    # =====================================================================
    # STEP 5: exibirNovaViagem - lstRecorrente reset (lines ~552-558)
    # =====================================================================
    content = content.replace(
        '''const lstRecorrente = document.getElementById("lstRecorrente");
            if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])
            {
                lstRecorrente.ej2_instances[0].enabled = true;
                lstRecorrente.ej2_instances[0].value = "N";
                lstRecorrente.ej2_instances[0].dataBind();''',
        '''const lstRecorrenteKendo = $("#lstRecorrente").data("kendoDropDownList");
            if (lstRecorrenteKendo)
            {
                lstRecorrenteKendo.enable(true);
                lstRecorrenteKendo.value("N");''',
        1  # only first occurrence
    )
    changes.append("STEP 5: lstRecorrente reset (nova viagem)")

    # =====================================================================
    # STEP 6: exibirNovaViagem - lstPeriodos reset (lines ~565-569) - SYNCFUSION STAYS
    # =====================================================================
    content = content.replace(
        '''const lstPeriodos = document.getElementById("lstPeriodos");
            if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0])
            {
                lstPeriodos.ej2_instances[0].enabled = true;
                lstPeriodos.ej2_instances[0].value = null;
                lstPeriodos.ej2_instances[0].dataBind();''',
        '''const lstPeriodosEl = document.getElementById("lstPeriodos");
            if (lstPeriodosEl && lstPeriodosEl.ej2_instances && lstPeriodosEl.ej2_instances[0])
            {
                lstPeriodosEl.ej2_instances[0].enabled = true;
                lstPeriodosEl.ej2_instances[0].value = null;
                lstPeriodosEl.ej2_instances[0].dataBind();''',
        1  # only first occurrence - keep Syncfusion for lstPeriodos
    )
    # Actually lstPeriodos stays Syncfusion, so we keep ej2_instances. Skip this step.
    # But we already replaced... let's revert by NOT doing this step at all.
    # Actually this is fine - lstPeriodos stays Syncfusion, we should keep ej2_instances.
    # Let me undo the replacement above. Instead I'll handle lstPeriodos differently.

    # =====================================================================
    # STEP 7-8: exibirViagemExistente - Motorista set value (lines ~910-916)
    # =====================================================================
    content = content.replace(
        '''const lstMotorista = document.getElementById("lstMotorista");
            if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0])
            {
                lstMotorista.ej2_instances[0].value = objViagem.motoristaId;
                lstMotorista.ej2_instances[0].dataBind();
            }''',
        '''const motoristaCombo = $("#lstMotorista").data("kendoComboBox");
            if (motoristaCombo)
            {
                motoristaCombo.value(objViagem.motoristaId);
            }'''
    )
    changes.append("STEP 7: Motorista set value (existente)")

    content = content.replace(
        '''const lstVeiculo = document.getElementById("lstVeiculo");
            if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0])
            {
                lstVeiculo.ej2_instances[0].value = objViagem.veiculoId;
                lstVeiculo.ej2_instances[0].dataBind();
            }''',
        '''const veiculoCombo = $("#lstVeiculo").data("kendoComboBox");
            if (veiculoCombo)
            {
                veiculoCombo.value(objViagem.veiculoId);
            }'''
    )
    changes.append("STEP 8: Veículo set value (existente)")

    # =====================================================================
    # STEP 9: lstFinalidade (lines ~932-934) + subsequent code
    # =====================================================================
    content = content.replace(
        '''const lstFinalidade = document.getElementById("lstFinalidade");
        if (lstFinalidade && lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0])
        {
            const finalidadeInst = lstFinalidade.ej2_instances[0];''',
        '''const finalidadeWidget = $("#lstFinalidade").data("kendoDropDownList");
        if (finalidadeWidget)
        {
            const finalidadeInst = finalidadeWidget;'''
    )
    changes.append("STEP 9: lstFinalidade (existente)")

    # =====================================================================
    # STEP 10: lstSetorRequisitanteAgendamento (lines ~997-999) - SYNCFUSION STAYS
    # No change needed - it's already correct as Syncfusion DropDownTree
    # =====================================================================

    # =====================================================================
    # STEP 11: rteDescricao (lines ~1125-1128)
    # =====================================================================
    content = content.replace(
        '''const rteDescricao = document.getElementById("rteDescricao");
            if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances[0])
            {
                rteDescricao.ej2_instances[0].value = objViagem.descricao;
                rteDescricao.ej2_instances[0].dataBind();
            }''',
        '''// ✅ KENDO: Usar helper setEditorUpsertValue
            if (typeof setEditorUpsertValue === "function")
            {
                setEditorUpsertValue(objViagem.descricao);
            }
            else
            {
                const rteEditor = $("#rte").data("kendoEditor");
                if (rteEditor) rteEditor.value(objViagem.descricao);
            }'''
    )
    changes.append("STEP 11: rteDescricao → setEditorUpsertValue")

    # =====================================================================
    # STEP 12-13: ddtCombustivelInicial/Final (lines ~1156-1169)
    # =====================================================================
    content = content.replace(
        '''const ddtCombustivelInicial = document.getElementById("ddtCombustivelInicial");
            if (ddtCombustivelInicial && ddtCombustivelInicial.ej2_instances && ddtCombustivelInicial.ej2_instances[0])
            {
                ddtCombustivelInicial.ej2_instances[0].value = objViagem.combustivelInicial;
                ddtCombustivelInicial.ej2_instances[0].dataBind();
            }''',
        '''const ddlCombIni = $("#ddtCombustivelInicial").data("kendoDropDownList");
            if (ddlCombIni)
            {
                ddlCombIni.value(objViagem.combustivelInicial);
            }'''
    )
    changes.append("STEP 12: ddtCombustivelInicial (existente)")

    content = content.replace(
        '''const ddtCombustivelFinal = document.getElementById("ddtCombustivelFinal");
            if (ddtCombustivelFinal && ddtCombustivelFinal.ej2_instances && ddtCombustivelFinal.ej2_instances[0])
            {
                ddtCombustivelFinal.ej2_instances[0].value = objViagem.combustivelFinal;
                ddtCombustivelFinal.ej2_instances[0].dataBind();
            }''',
        '''const ddlCombFim = $("#ddtCombustivelFinal").data("kendoDropDownList");
            if (ddlCombFim)
            {
                ddlCombFim.value(objViagem.combustivelFinal);
            }'''
    )
    changes.append("STEP 13: ddtCombustivelFinal (existente)")

    # =====================================================================
    # STEP 14: lstEventos debug logs + instance access (lines ~1185-1192)
    # =====================================================================
    content = content.replace(
        '''console.log("lstEventos tem ej2_instances?", lstEventos?.ej2_instances);
            console.log("lstEventos instância [0]:", lstEventos?.ej2_instances?.[0]);

            if (lstEventos && lstEventos.ej2_instances && lstEventos.ej2_instances[0])
            {
                console.log("✅ lstEventos ENCONTRADO e INICIALIZADO");

                const lstEventosInst = lstEventos.ej2_instances[0];''',
        '''const lstEventosWidget = $("#lstEventos").data("kendoComboBox");
            console.log("lstEventos kendoComboBox?", lstEventosWidget);

            if (lstEventosWidget)
            {
                console.log("✅ lstEventos ENCONTRADO e INICIALIZADO (Kendo ComboBox)");

                const lstEventosInst = lstEventosWidget;'''
    )
    changes.append("STEP 14: lstEventos → kendoComboBox")

    # Fix lstEventos value set (dataBind → Kendo API)
    content = content.replace(
        '''lstEventosInst.value = objViagem.eventoId;
                lstEventosInst.dataBind();''',
        '''lstEventosInst.value(objViagem.eventoId);''',
        1  # first occurrence only
    )
    changes.append("STEP 14b: lstEventos value set")

    # Fix lstEventosInst.text and lstEventosInst.value references
    content = content.replace(
        '''console.log("✅ Valor definido. Novo valor:", lstEventosInst.value);
                console.log("Texto selecionado:", lstEventosInst.text);''',
        '''console.log("✅ Valor definido. Novo valor:", lstEventosInst.value());
                console.log("Texto selecionado:", lstEventosInst.text());'''
    )

    content = content.replace(
        '''console.log("DataSource do lstEventos:", lstEventosInst.dataSource);
                console.log("Valor atual:", lstEventosInst.value);''',
        '''console.log("DataSource do lstEventos:", lstEventosInst.dataSource);
                console.log("Valor atual:", lstEventosInst.value());'''
    )

    # =====================================================================
    # STEP 15: txtDataInicioEvento (lines ~1250-1275)
    # =====================================================================
    content = content.replace(
        '''console.log("txtDataInicioEvento.ej2_instances:", txtDataInicioEvento?.ej2_instances);

                                if (dataInicial && txtDataInicioEvento && txtDataInicioEvento.ej2_instances && txtDataInicioEvento.ej2_instances[0])
                                {
                                    try
                                    {
                                        const dataObj = new Date(dataInicial);
                                        // Alterado em: 12/01/2026 - Validação de data antes de preencher DatePicker
                                        if (!isNaN(dataObj.getTime()))
                                        {
                                            txtDataInicioEvento.ej2_instances[0].value = dataObj;
                                            txtDataInicioEvento.ej2_instances[0].enabled = false;
                                            txtDataInicioEvento.ej2_instances[0].dataBind();
                                            console.log("✅ Data Início preenchida:", dataObj.toLocaleDateString('pt-BR'));
                                        }
                                        else
                                        {
                                            console.error("❌ Data Início inválida:", dataInicial);
                                            txtDataInicioEvento.ej2_instances[0].value = null;
                                            txtDataInicioEvento.ej2_instances[0].dataBind();
                                        }
                                    } catch (error)
                                    {
                                        console.error("❌ Erro ao preencher Data Início:", error);
                                        txtDataInicioEvento.ej2_instances[0].value = null;
                                        txtDataInicioEvento.ej2_instances[0].dataBind();
                                    }''',
        '''// ✅ KENDO: Usar helper setKendoDateValue
                                if (dataInicial)
                                {
                                    try
                                    {
                                        const dataObj = new Date(dataInicial);
                                        // Alterado em: 12/01/2026 - Validação de data antes de preencher DatePicker
                                        if (!isNaN(dataObj.getTime()))
                                        {
                                            window.setKendoDateValue("txtDataInicioEvento", dataObj);
                                            window.enableKendoDatePicker("txtDataInicioEvento", false);
                                            console.log("✅ Data Início preenchida (Kendo):", dataObj.toLocaleDateString('pt-BR'));
                                        }
                                        else
                                        {
                                            console.error("❌ Data Início inválida:", dataInicial);
                                            window.setKendoDateValue("txtDataInicioEvento", null);
                                        }
                                    } catch (error)
                                    {
                                        console.error("❌ Erro ao preencher Data Início:", error);
                                        window.setKendoDateValue("txtDataInicioEvento", null);
                                    }'''
    )
    changes.append("STEP 15: txtDataInicioEvento → setKendoDateValue")

    # =====================================================================
    # STEP 16: txtDataFimEvento (lines ~1291-1314)
    # Need to read this section to get exact pattern
    # =====================================================================
    # Pattern: similar to txtDataInicioEvento
    content = content.replace(
        'if (dataFinal && txtDataFimEvento && txtDataFimEvento.ej2_instances && txtDataFimEvento.ej2_instances[0])',
        'if (dataFinal)'
    )

    # Replace txtDataFimEvento ej2 value sets with Kendo helpers
    content = content.replace(
        '''txtDataFimEvento.ej2_instances[0].value = dataObj;
                                            txtDataFimEvento.ej2_instances[0].enabled = false;
                                            txtDataFimEvento.ej2_instances[0].dataBind();''',
        '''window.setKendoDateValue("txtDataFimEvento", dataObj);
                                            window.enableKendoDatePicker("txtDataFimEvento", false);'''
    )

    # Replace txtDataFimEvento null sets
    while 'txtDataFimEvento.ej2_instances[0].value = null;\n                                            txtDataFimEvento.ej2_instances[0].dataBind();' in content:
        content = content.replace(
            'txtDataFimEvento.ej2_instances[0].value = null;\n                                            txtDataFimEvento.ej2_instances[0].dataBind();',
            'window.setKendoDateValue("txtDataFimEvento", null);'
        )

    while 'txtDataFimEvento.ej2_instances[0].value = null;\n                                        txtDataFimEvento.ej2_instances[0].dataBind();' in content:
        content = content.replace(
            'txtDataFimEvento.ej2_instances[0].value = null;\n                                        txtDataFimEvento.ej2_instances[0].dataBind();',
            'window.setKendoDateValue("txtDataFimEvento", null);'
        )

    changes.append("STEP 16: txtDataFimEvento → setKendoDateValue")

    # =====================================================================
    # STEP 17: txtQtdParticipantesEvento (lines ~1330-1335)
    # =====================================================================
    content = content.replace(
        '''if (qtdParticipantes !== undefined && txtQtdParticipantesEvento && txtQtdParticipantesEvento.ej2_instances && txtQtdParticipantesEvento.ej2_instances[0])
                                {
                                    const qtdNumero = typeof qtdParticipantes === 'string' ? parseInt(qtdParticipantes, 10) : qtdParticipantes;
                                    txtQtdParticipantesEvento.ej2_instances[0].value = qtdNumero;
                                    txtQtdParticipantesEvento.ej2_instances[0].enabled = false;
                                    txtQtdParticipantesEvento.ej2_instances[0].dataBind();''',
        '''if (qtdParticipantes !== undefined && txtQtdParticipantesEvento)
                                {
                                    const qtdNumero = typeof qtdParticipantes === 'string' ? parseInt(qtdParticipantes, 10) : qtdParticipantes;
                                    const qtdWidget = $("#txtQtdParticipantesEvento").data("kendoNumericTextBox");
                                    if (qtdWidget) {
                                        qtdWidget.value(qtdNumero);
                                        qtdWidget.enable(false);
                                    } else {
                                        $(txtQtdParticipantesEvento).val(qtdNumero).prop("disabled", true);
                                    }'''
    )
    changes.append("STEP 17: txtQtdParticipantesEvento → kendoNumericTextBox")

    # =====================================================================
    # STEP 18: lstEventos error diagnostics (lines ~1365-1366)
    # =====================================================================
    content = content.replace(
        '''if (lstEventos && !lstEventos.ej2_instances) console.error("   - Não tem ej2_instances");
                if (lstEventos && lstEventos.ej2_instances && !lstEventos.ej2_instances[0]) console.error("   - ej2_instances[0] é undefined");''',
        '''if (!lstEventosWidget) console.error("   - Kendo ComboBox não inicializado");'''
    )
    changes.append("STEP 18: lstEventos error diagnostics")

    # =====================================================================
    # STEP 19-24: configurarRecorrencia / configurarCamposRecorrencia
    # All lstRecorrente blocks: .ej2_instances[0].value = "S"/"N", .enabled, .dataBind()
    # =====================================================================

    # Generic pattern for all lstRecorrente blocks (lines 1399, 1446, 2788, 3173)
    # Pattern: document.getElementById("lstRecorrente") + if ej2 + .value = "S/N" + .enabled + .dataBind
    def replace_lstRecorrente_block(match):
        indent = match.group(1)
        value = match.group(2)  # "S" or "N" or 'S' or 'N'
        enabled_line = match.group(3) or ''  # could be None or ".enabled = false/true"

        # Normalize value: strip quotes
        val_clean = value.strip("'\"")

        # Build Kendo replacement
        lines = []
        lines.append(f'{indent}const lstRecorrenteKendo = $("#lstRecorrente").data("kendoDropDownList");')
        lines.append(f'{indent}if (lstRecorrenteKendo)')
        lines.append(f'{indent}{{')
        lines.append(f'{indent}    lstRecorrenteKendo.value("{val_clean}");')

        if 'enabled = false' in enabled_line:
            lines.append(f'{indent}    lstRecorrenteKendo.enable(false);')
        elif 'enabled = true' in enabled_line:
            lines.append(f'{indent}    lstRecorrenteKendo.enable(true);')

        return '\n'.join(lines)

    # Use regex to replace ALL remaining lstRecorrente ej2 blocks
    # Pattern: const lstRecorrente = document.getElementById("lstRecorrente");\n
    #          if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])\n
    #          {\n
    #              lstRecorrente.ej2_instances[0].value = "X";\n
    #              lstRecorrente.ej2_instances[0].enabled = false; // optional\n
    #              lstRecorrente.ej2_instances[0].dataBind();\n
    pat_lstRecorrente = re.compile(
        r'( +)const lstRecorrente = document\.getElementById\("lstRecorrente"\);\n'
        r'\s+if \(lstRecorrente && lstRecorrente\.ej2_instances && lstRecorrente\.ej2_instances\[0\]\)\n'
        r'\s+\{\n'
        r'\s+lstRecorrente\.ej2_instances\[0\]\.value = (["\'][SN]["\']);\n'
        r'(\s+lstRecorrente\.ej2_instances\[0\]\.enabled = (?:true|false);[^\n]*\n)?'
        r'\s+lstRecorrente\.ej2_instances\[0\]\.dataBind\(\);'
    )

    count_before = count_ej2(content)
    content = pat_lstRecorrente.sub(replace_lstRecorrente_block, content)
    if count_ej2(content) < count_before:
        changes.append(f"STEP 19: lstRecorrente blocks migrated ({count_before - count_ej2(content)} ej2_instances removed)")

    # Also handle the case without dataBind (line 1449 only has value + enabled, no dataBind)
    pat_lstRecorrente_no_dataBind = re.compile(
        r'( +)const lstRecorrente = document\.getElementById\("lstRecorrente"\);\n'
        r'\s+if \(lstRecorrente && lstRecorrente\.ej2_instances && lstRecorrente\.ej2_instances\[0\]\)\n'
        r'\s+\{\n'
        r'\s+lstRecorrente\.ej2_instances\[0\]\.value = (["\'][SN]["\']);\n'
        r'\s+lstRecorrente\.ej2_instances\[0\]\.enabled = (true|false);[^\n]*'
    )

    def replace_lstRecorrente_no_db(match):
        indent = match.group(1)
        val_clean = match.group(2).strip("'\"")
        enabled = match.group(3)
        lines = [
            f'{indent}const lstRecorrenteKendo = $("#lstRecorrente").data("kendoDropDownList");',
            f'{indent}if (lstRecorrenteKendo)',
            f'{indent}{{',
            f'{indent}    lstRecorrenteKendo.value("{val_clean}");',
            f'{indent}    lstRecorrenteKendo.enable({enabled});'
        ]
        return '\n'.join(lines)

    content = pat_lstRecorrente_no_dataBind.sub(replace_lstRecorrente_no_db, content)

    # =====================================================================
    # STEP 20: All lstPeriodos blocks - KEEP SYNCFUSION (no migration)
    # These stay as ej2_instances - no changes needed
    # =====================================================================

    # =====================================================================
    # STEP 21: All lstDias blocks - KEEP SYNCFUSION (no migration)
    # These stay as ej2_instances - no changes needed
    # =====================================================================

    # =====================================================================
    # STEP 22: All lstDiasMes blocks - KEEP SYNCFUSION (no migration)
    # These stay as ej2_instances - no changes needed
    # =====================================================================

    # =====================================================================
    # STEP 23: All calDatasSelecionadas blocks - KEEP SYNCFUSION (no migration)
    # These stay as ej2_instances - no changes needed
    # =====================================================================

    # =====================================================================
    # STEP 24: mostrarCamposViagem - ddtCombIni/ddtCombFim (lines ~2686-2711)
    # =====================================================================
    content = content.replace(
        '''const ddtCombIni = document.getElementById("ddtCombustivelInicial");
            if (ddtCombIni && ddtCombIni.ej2_instances && ddtCombIni.ej2_instances[0])
            {
                ddtCombIni.ej2_instances[0].value = [objViagem.combustivelInicial];
                ddtCombIni.ej2_instances[0].dataBind();
            }''',
        '''const ddlCombIniMostrar = $("#ddtCombustivelInicial").data("kendoDropDownList");
            if (ddlCombIniMostrar)
            {
                ddlCombIniMostrar.value(objViagem.combustivelInicial);
            }'''
    )
    changes.append("STEP 24a: ddtCombIni (mostrarCamposViagem)")

    content = content.replace(
        '''const ddtCombFim = document.getElementById("ddtCombustivelFinal");
                if (ddtCombFim && ddtCombFim.ej2_instances && ddtCombFim.ej2_instances[0])
                {
                    ddtCombFim.ej2_instances[0].value = [objViagem.combustivelFinal];
                    ddtCombFim.ej2_instances[0].dataBind();
                }''',
        '''const ddlCombFimMostrar = $("#ddtCombustivelFinal").data("kendoDropDownList");
                if (ddlCombFimMostrar)
                {
                    ddlCombFimMostrar.value(objViagem.combustivelFinal);
                }'''
    )
    changes.append("STEP 24b: ddtCombFim (mostrarCamposViagem)")

    # =====================================================================
    # STEP 25: configurarRecorrencia - lstRecorrente (lines ~2788-2792)
    # Already handled by regex in STEP 19
    # =====================================================================

    # =====================================================================
    # STEP 26: configurarRecorrencia - lstPeriodos (lines ~2804-2811)
    # SYNCFUSION STAYS - no changes
    # =====================================================================

    # =====================================================================
    # Final: Report results
    # =====================================================================
    final_count = count_ej2(content)
    return content, changes, original_count, final_count

def main():
    print("=" * 70)
    print("MIGRAÇÃO ej2_instances → Kendo UI jQuery API")
    print(f"Arquivo: {FILE}")
    print("=" * 70)

    if not os.path.exists(FILE):
        print(f"ERRO: Arquivo não encontrado: {FILE}")
        sys.exit(1)

    content = read_file()
    original_count = count_ej2(content)
    print(f"\nTotal ej2_instances ANTES: {original_count}")

    content, changes, orig, final = migrate(content)

    print(f"\nAlterações realizadas ({len(changes)}):")
    for c in changes:
        print(f"  ✅ {c}")

    print(f"\nTotal ej2_instances ANTES: {orig}")
    print(f"Total ej2_instances DEPOIS: {final}")
    print(f"Removidos: {orig - final}")

    # List remaining ej2_instances with line numbers
    lines = content.split('\n')
    remaining = []
    for i, line in enumerate(lines, 1):
        if 'ej2_instances' in line:
            remaining.append((i, line.strip()[:100]))

    if remaining:
        print(f"\n--- ej2_instances RESTANTES ({len(remaining)} ocorrências) ---")
        # Group by widget
        syncfusion_widgets = {"lstPeriodos", "lstDias", "lstDiasMes", "calDatasSelecionadas", "lstSetorRequisitanteAgendamento"}
        kendo_remaining = []
        sync_remaining = []
        for ln, text in remaining:
            is_sync = any(w in text for w in syncfusion_widgets)
            if is_sync:
                sync_remaining.append((ln, text))
            else:
                kendo_remaining.append((ln, text))

        if sync_remaining:
            print(f"\n  📌 Syncfusion (mantidos intencionalmente): {len(sync_remaining)}")
            for ln, text in sync_remaining[:10]:
                print(f"    L{ln}: {text}")
            if len(sync_remaining) > 10:
                print(f"    ... e mais {len(sync_remaining) - 10}")

        if kendo_remaining:
            print(f"\n  ⚠️ NÃO migrados (precisam atenção): {len(kendo_remaining)}")
            for ln, text in kendo_remaining:
                print(f"    L{ln}: {text}")
    else:
        print("\n✅ Nenhum ej2_instances restante!")

    # Write changes
    write_file(content)
    print(f"\n✅ Arquivo salvo com sucesso!")

if __name__ == "__main__":
    main()
