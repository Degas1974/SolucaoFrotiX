# wwwroot/js/agendamento/components/exibe-viagem.js

**Mudanca:** GRANDE | **+2559** linhas | **-3241** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/exibe-viagem.js
+++ ATUAL: wwwroot/js/agendamento/components/exibe-viagem.js
@@ -1,515 +1,391 @@
-function aguardarFuncaoDisponivel(nomeFuncao, timeout = 5000) {
-    try {
-        return new Promise((resolve, reject) => {
+function aguardarFuncaoDisponivel(nomeFuncao, timeout = 5000)
+{
+    try
+    {
+        return new Promise((resolve, reject) =>
+        {
             const inicio = Date.now();
 
-            const verificar = () => {
+            const verificar = () =>
+            {
 
                 const partes = nomeFuncao.split('.');
                 let objeto = window;
 
-                for (const parte of partes) {
+                for (const parte of partes)
+                {
                     if (parte === 'window') continue;
                     objeto = objeto[parte];
                     if (!objeto) break;
                 }
 
-                if (typeof objeto === 'function') {
+                if (typeof objeto === 'function')
+                {
                     resolve();
-                } else if (Date.now() - inicio > timeout) {
+                }
+                else if (Date.now() - inicio > timeout)
+                {
                     reject(new Error(`Timeout aguardando ${nomeFuncao}`));
-                } else {
+                }
+                else
+                {
                     setTimeout(verificar, 50);
                 }
             };
 
             verificar();
         });
-    } catch (error) {
-        console.error('❌ Erro em aguardarFuncaoDisponivel:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'aguardarFuncaoDisponivel',
-            error,
-        );
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em aguardarFuncaoDisponivel:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "aguardarFuncaoDisponivel", error);
     }
 }
 
-function formatarDataParaInput(date, inputType) {
-    const ano = date.getFullYear();
-    const mes = String(date.getMonth() + 1).padStart(2, '0');
-    const dia = String(date.getDate()).padStart(2, '0');
-
-    if (inputType === 'date') {
-        return `${ano}-${mes}-${dia}`;
-    }
-
-    return `${dia}/${mes}/${ano}`;
-}
-
-function preencherDataFinalRecorrencia(
-    dataFinalRecorrencia,
-    { disable = false, contexto = '' } = {},
-) {
-    try {
-        if (!dataFinalRecorrencia) {
-            console.warn(
-                `⚠️ Data Final Recorrência vazia${contexto ? ` (${contexto})` : ''}`,
-            );
-            return false;
-        }
-
-        const txtFinalRecorrencia = document.getElementById(
-            'txtFinalRecorrencia',
-        );
-        console.log(
-            `🔍 DEBUG Data Final Recorrência${contexto ? ` (${contexto})` : ''}:`,
-            dataFinalRecorrencia,
-        );
-        console.log(' - txtFinalRecorrencia existe?', !!txtFinalRecorrencia);
-
-        if (!txtFinalRecorrencia) {
-            console.error(
-                '❌ Campo txtFinalRecorrencia não encontrado no DOM!',
-            );
-            return false;
-        }
-
-        const dataFinal = new Date(dataFinalRecorrencia);
-        if (isNaN(dataFinal.getTime())) {
-            console.warn(
-                '⚠️ Data Final Recorrência inválida:',
-                dataFinalRecorrencia,
-            );
-            return false;
-        }
-
-        var kendoDatePicker = $('#txtFinalRecorrencia').data('kendoDatePicker');
-        if (kendoDatePicker) {
-            kendoDatePicker.value(dataFinal);
-            kendoDatePicker.enable(!disable);
-        } else {
-            const dataFormatada = formatarDataParaInput(
-                dataFinal,
-                txtFinalRecorrencia.type,
-            );
-            txtFinalRecorrencia.value = dataFormatada;
-        }
-
-        txtFinalRecorrencia.style.setProperty('display', 'block', 'important');
-        txtFinalRecorrencia.disabled = disable;
-
-        console.log('✅ Data Final Recorrência definida e atualizada');
-        return true;
-    } catch (error) {
-        console.error('❌ Erro ao preencher Data Final Recorrência:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'preencherDataFinalRecorrencia',
-            error,
-        );
-        return false;
-    }
-}
-
-window.ExibeViagem = function (
-    objViagem,
-    dataClicada = null,
-    horaClicada = null,
-) {
-    try {
-
-        if (window.modalDebounceTimer) {
+window.ExibeViagem = function (objViagem, dataClicada = null, horaClicada = null)
+{
+    try
+    {
+
+        if (window.modalDebounceTimer)
+        {
             clearTimeout(window.modalDebounceTimer);
         }
 
-        if (window.modalIsOpening) {
-            console.log('âš ï¸ Modal já está sendo aberto, aguardando...');
+        if (window.modalIsOpening)
+        {
+            console.log("âš ï¸ Modal já está sendo aberto, aguardando...");
             return;
         }
 
         window.modalIsOpening = true;
 
-        if (window.isReportViewerLoading) {
-            console.log('⚠️ Cancelando carregamento de relatório pendente');
+        if (window.isReportViewerLoading)
+        {
+            console.log("⚠️ Cancelando carregamento de relatório pendente");
             window.isReportViewerLoading = false;
         }
 
-        if (typeof window.esconderLoadingRelatorio === 'function') {
-            window.esconderLoadingRelatorio();
-        }
-
-        $('#modal-relatorio-loading-overlay').remove();
-
-        if (window.FtxSpin && typeof window.FtxSpin.hide === 'function') {
-            window.FtxSpin.hide();
-        }
-
-        $('.ftx-spin-overlay').each(function () {
-
-            if (this.id !== 'ftx-spin-overlay') {
-                $(this).remove();
-            }
-        });
-
-        document
-            .querySelectorAll(
-                '.e-spinner-pane, .e-spin-overlay, .e-spin-show, .e-overlay',
-            )
-            .forEach(function (el) {
-
-                if (el.parentElement === document.body) {
-                    try {
-                        el.remove();
-                    } catch (e) {
-                        el.style.display = 'none';
-                    }
-                }
-            });
-
-        $('.k-overlay, .k-loading-mask').remove();
-
-        $('body > .e-overlay').remove();
-
-        window.modalDebounceTimer = setTimeout(() => {
-
-            console.log('ðŸ“‹ ExibeViagem executando após debounce');
-
-            if (
-                !objViagem ||
-                objViagem === '' ||
-                typeof objViagem === 'string'
-            ) {
+        window.modalDebounceTimer = setTimeout(() =>
+        {
+
+            console.log("ðŸ“‹ ExibeViagem executando após debounce");
+
+            if (!objViagem || objViagem === "" || typeof objViagem === "string")
+            {
                 exibirNovaViagem(dataClicada, horaClicada);
-            } else {
+            } else
+            {
                 exibirViagemExistente(objViagem);
             }
 
-            setTimeout(() => {
+            setTimeout(() =>
+            {
                 window.modalIsOpening = false;
             }, 500);
         }, 300);
-    } catch (error) {
+    } catch (error)
+    {
         window.modalIsOpening = false;
-        Alerta.TratamentoErroComLinha('exibe-viagem.js', 'ExibeViagem', error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "ExibeViagem", error);
     }
 };
 
-function inicializarLstRecorrente() {
-    try {
-
-        if (
-            typeof window.AgendamentoV2 !== 'undefined' &&
-            window.AgendamentoV2._criarDropdownRecorrente
-        ) {
-            console.log(
-                '🔄 [inicializarLstRecorrente] Delegando para AgendamentoV2',
-            );
-            window.AgendamentoV2._criarDropdownRecorrente();
+function inicializarLstRecorrente()
+{
+    try
+    {
+        const lstRecorrenteElement = document.getElementById("lstRecorrente");
+
+        if (!lstRecorrenteElement)
+        {
+            console.warn("⚠️ lstRecorrente não encontrado no DOM");
+            return false;
+        }
+
+        const dataRecorrente = [
+            { RecorrenteId: "N", Descricao: "Não" },
+            { RecorrenteId: "S", Descricao: "Sim" }
+        ];
+
+        if (lstRecorrenteElement.ej2_instances && lstRecorrenteElement.ej2_instances[0])
+        {
+            const lstRecorrente = lstRecorrenteElement.ej2_instances[0];
+
+            if (!lstRecorrente.dataSource || lstRecorrente.dataSource.length === 0)
+            {
+                console.log("🔄 lstRecorrente existe mas dataSource está vazio - populando...");
+                lstRecorrente.dataSource = dataRecorrente;
+                lstRecorrente.fields = { text: 'Descricao', value: 'RecorrenteId' };
+                lstRecorrente.dataBind();
+                console.log("✅ lstRecorrente dataSource populado");
+            }
+            else
+            {
+                console.log("ℹ️ lstRecorrente já tem dataSource");
+            }
+
             return true;
         }
-
-        const $lstRecorrente = $('#lstRecorrente');
-        const kendoInstance = $lstRecorrente.data('kendoDropDownList');
-
-        if (kendoInstance) {
-            console.log('ℹ️ lstRecorrente (Kendo) já inicializado');
+        else
+        {
+
+            console.log("🆕 Criando nova instância de lstRecorrente...");
+
+            const dropdown = new ej.dropdowns.DropDownList({
+                dataSource: dataRecorrente,
+                fields: { text: 'Descricao', value: 'RecorrenteId' },
+                placeholder: 'Selecione...',
+                popupHeight: '200px',
+                cssClass: 'e-outline text-center',
+                floatLabelType: 'Never',
+                value: 'N'
+            });
+
+            dropdown.appendTo('#lstRecorrente');
+            console.log("✅ lstRecorrente criado com sucesso");
             return true;
         }
-
-        console.log('🆕 Criando Kendo DropDownList para lstRecorrente...');
-        $lstRecorrente.kendoDropDownList({
-            dataSource: [
-                { value: 'N', text: 'Não' },
-                { value: 'S', text: 'Sim' },
-            ],
-            dataTextField: 'text',
-            dataValueField: 'value',
-            value: 'N',
-            optionLabel: 'Selecione...',
-        });
-
-        console.log('✅ lstRecorrente (Kendo) criado com sucesso');
-        return true;
-    } catch (error) {
-        console.error('❌ Erro ao inicializar lstRecorrente:', error);
-        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'exibe-viagem.js',
-                'inicializarLstRecorrente',
-                error,
-            );
+    } catch (error)
+    {
+        console.error("❌ Erro ao inicializar lstRecorrente:", error);
+        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("exibe-viagem.js", "inicializarLstRecorrente", error);
         }
         return false;
     }
 }
 
-function exibirNovaViagem(dataClicada, horaClicada) {
-    try {
-        console.log('ðŸ†• Criando nova viagem');
-        console.log(
-            'ðŸ§¹ Limpando campos ANTES de configurar novo agendamento...',
-        );
-
-        if (typeof window.limparCamposModalViagens === 'function') {
+function exibirNovaViagem(dataClicada, horaClicada)
+{
+    try
+    {
+        console.log("ðŸ†• Criando nova viagem");
+        console.log("ðŸ§¹ Limpando campos ANTES de configurar novo agendamento...");
+
+        if (typeof window.limparCamposModalViagens === 'function')
+        {
             window.limparCamposModalViagens();
-        } else {
-            console.warn('⚠️ limparCamposModalViagens não disponível');
+        }
+        else
+        {
+            console.warn("⚠️ limparCamposModalViagens não disponível");
         }
 
         limparListboxDatasVariadas();
 
-        setTimeout(() => {
-            console.log(
-                'â° Iniciando configuração de novo agendamento após limpeza...',
-            );
+        setTimeout(() =>
+        {
+            console.log("â° Iniciando configuração de novo agendamento após limpeza...");
 
             window.setModalTitle('NOVO_AGENDAMENTO');
 
-            if (typeof window.inicializarCamposModal === 'function') {
-                if (typeof window.inicializarCamposModal === 'function') {
+            if (typeof window.inicializarCamposModal === 'function')
+            {
+                if (typeof window.inicializarCamposModal === 'function')
+                {
                     window.inicializarCamposModal();
-                } else {
-                    console.warn('⚠️ inicializarCamposModal não disponível');
-                }
-            } else {
-                console.warn('⚠️ inicializarCamposModal não disponível');
-            }
-
-            if (typeof window.esconderRelatorio === 'function') {
+                }
+                else
+                {
+                    console.warn("⚠️ inicializarCamposModal não disponível");
+                }
+            }
+            else
+            {
+                console.warn("⚠️ inicializarCamposModal não disponível");
+            }
+
+            if (typeof window.esconderRelatorio === 'function')
+            {
                 window.esconderRelatorio();
             }
 
-            console.log(
-                'ðŸ§¹ [Extra] Garantindo limpeza de Motorista e Veí­culo...',
-            );
-
-            const lstMotorista = document.getElementById('lstMotorista');
-            if (
-                lstMotorista &&
-                lstMotorista.ej2_instances &&
-                lstMotorista.ej2_instances[0]
-            ) {
+            console.log("ðŸ§¹ [Extra] Garantindo limpeza de Motorista e Veí­culo...");
+
+            const lstMotorista = document.getElementById("lstMotorista");
+            if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0])
+            {
                 const motoristaInst = lstMotorista.ej2_instances[0];
                 motoristaInst.value = null;
                 motoristaInst.text = '';
                 motoristaInst.index = null;
 
-                if (typeof motoristaInst.dataBind === 'function') {
+                if (typeof motoristaInst.dataBind === 'function')
+                {
                     motoristaInst.dataBind();
                 }
 
-                if (typeof motoristaInst.clear === 'function') {
+                if (typeof motoristaInst.clear === 'function')
+                {
                     motoristaInst.clear();
                 }
 
-                console.log(
-                    'âœ… Motorista limpo na criação de novo agendamento',
-                );
-            }
-
-            const lstVeiculo = document.getElementById('lstVeiculo');
-            if (
-                lstVeiculo &&
-                lstVeiculo.ej2_instances &&
-                lstVeiculo.ej2_instances[0]
-            ) {
+                console.log("âœ… Motorista limpo na criação de novo agendamento");
+            }
+
+            const lstVeiculo = document.getElementById("lstVeiculo");
+            if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0])
+            {
                 const veiculoInst = lstVeiculo.ej2_instances[0];
                 veiculoInst.value = null;
                 veiculoInst.text = '';
                 veiculoInst.index = null;
 
-                if (typeof veiculoInst.dataBind === 'function') {
+                if (typeof veiculoInst.dataBind === 'function')
+                {
                     veiculoInst.dataBind();
                 }
 
-                if (typeof veiculoInst.clear === 'function') {
+                if (typeof veiculoInst.clear === 'function')
+                {
                     veiculoInst.clear();
                 }
 
-                console.log(
-                    'âœ… Veí­culo limpo na criação de novo agendamento',
-                );
-            }
-
-            $('#btnConfirma').html(
-                "<i class='fa-duotone fa-floppy-disk icon-space'></i>Confirmar",
-            );
-            $('#btnConfirma').prop('disabled', false).show();
-            $('#btnApaga').hide();
-            $('#btnViagem').hide();
-            $('#btnImprime').hide();
-
-            const btnFichaVistoria = document.getElementById(
-                'btnVisualizarFichaVistoria',
-            );
-            if (btnFichaVistoria) {
-                btnFichaVistoria.style.display = 'none';
-                btnFichaVistoria.disabled = true;
-                btnFichaVistoria.dataset.viagemId = '';
-                btnFichaVistoria.dataset.noFicha = '';
-            }
+                console.log("âœ… Veí­culo limpo na criação de novo agendamento");
+            }
+
+            $("#btnConfirma").html("<i class='fa-duotone fa-floppy-disk icon-space'></i>Confirmar");
+            $("#btnConfirma").prop("disabled", false).show();
+            $("#btnApaga").hide();
+            $("#btnViagem").hide();
+            $("#btnImprime").hide();
 
             configurarRodapeLabelsNovo();
 
-            const cardRecorrencia = document.getElementById('cardRecorrencia');
-            if (cardRecorrencia) {
-                cardRecorrencia.style.display = 'block';
-                console.log(
-                    '✅ Card de recorrência VISÍVEL (novo agendamento)',
-                );
-            }
-
-            const lstRecorrenteKendo =
-                $('#lstRecorrente').data('kendoDropDownList');
-            if (lstRecorrenteKendo) {
-                lstRecorrenteKendo.enable(true);
-                lstRecorrenteKendo.value('N');
-                console.log(
-                    "✅ lstRecorrente habilitado e definido como 'Não'",
-                );
-            }
-
             const camposViagem = [
-                'divNoFichaVistoria',
-                'divDataFinal',
-                'divHoraFinal',
-                'divDuracao',
-                'divKmAtual',
-                'divKmInicial',
-                'divKmFinal',
-                'divQuilometragem',
-                'divCombustivelInicial',
-                'divCombustivelFinal',
+                "divNoFichaVistoria", "divDataFinal", "divHoraFinal", "divDuracao",
+                "divKmAtual", "divKmInicial", "divKmFinal", "divQuilometragem",
+                "divCombustivelInicial", "divCombustivelFinal"
             ];
 
-            camposViagem.forEach((id) => {
+            camposViagem.forEach(id =>
+            {
                 const el = document.getElementById(id);
-                if (el) el.style.display = 'none';
+                if (el) el.style.display = "none";
             });
 
-            console.log(
-                '🧹 [NovoAgendamento] Fechando Card de Evento e Accordion...',
-            );
-
-            const cardSelecaoEvento =
-                document.getElementById('cardSelecaoEvento');
-            if (cardSelecaoEvento) {
-                cardSelecaoEvento.style.display = 'none';
-                console.log('✅ Card de Seleção de Evento fechado');
-            }
-
-            const sectionEvento = document.getElementById('sectionEvento');
-            if (sectionEvento) {
-                sectionEvento.style.display = 'none';
-                console.log('✅ Section Evento fechada');
-            }
-
-            const txtDataInicial = document.getElementById('txtDataInicial');
-            if (txtDataInicial) {
+            console.log("🧹 [NovoAgendamento] Fechando Card de Evento e Accordion...");
+
+            const cardSelecaoEvento = document.getElementById("cardSelecaoEvento");
+            if (cardSelecaoEvento)
+            {
+                cardSelecaoEvento.style.display = "none";
+                console.log("✅ Card de Seleção de Evento fechado");
+            }
+
+            const sectionEvento = document.getElementById("sectionEvento");
+            if (sectionEvento)
+            {
+                sectionEvento.style.display = "none";
+                console.log("✅ Section Evento fechada");
+            }
+
+            const txtDataInicial = document.getElementById("txtDataInicial");
+            if (txtDataInicial)
+            {
 
                 let dataParaUsar;
-                if (dataClicada) {
+                if (dataClicada)
+                {
                     dataParaUsar = dataClicada;
-                    console.log(
-                        'ðŸ“… Usando data clicada:',
-                        dataParaUsar.toLocaleDateString('pt-BR'),
-                    );
-                } else if (window.calendar && window.calendar.getDate) {
+                    console.log("ðŸ“… Usando data clicada:", dataParaUsar.toLocaleDateString('pt-BR'));
+                } else if (window.calendar && window.calendar.getDate)
+                {
                     dataParaUsar = window.calendar.getDate();
-                    console.log(
-                        'ðŸ“… Usando data da agenda:',
-                        dataParaUsar.toLocaleDateString('pt-BR'),
-                    );
-                } else {
+                    console.log("ðŸ“… Usando data da agenda:", dataParaUsar.toLocaleDateString('pt-BR'));
+                } else
+                {
                     dataParaUsar = new Date();
-                    console.log(
-                        'ðŸ“… Usando data de hoje:',
-                        dataParaUsar.toLocaleDateString('pt-BR'),
-                    );
-                }
-
-                var kendoDataInicialPicker =
-                    $('#txtDataInicial').data('kendoDatePicker');
-                if (kendoDataInicialPicker) {
-                    kendoDataInicialPicker.value(dataParaUsar);
-                    kendoDataInicialPicker.enable(true);
-                    console.log(
-                        'Data definida no Kendo DatePicker txtDataInicial',
-                    );
-                } else {
-                    console.warn(
-                        'Kendo DatePicker txtDataInicial nao encontrado',
-                    );
-                }
+                    console.log("ðŸ“… Usando data de hoje:", dataParaUsar.toLocaleDateString('pt-BR'));
+                }
+
+                window.setKendoDateValue("txtDataInicial", dataParaUsar, true);
+                window.enableKendoDatePicker("txtDataInicial", true);
+                console.log("✅ Data definida no Kendo DatePicker");
 
                 txtDataInicial.style.display = '';
-                const parentDiv = txtDataInicial.closest(
-                    '.form-group, .mb-3, div[id*="divData"]',
-                );
-                if (parentDiv) {
+                const parentDiv = txtDataInicial.closest('.form-group, .mb-3, div[id*="divData"]');
+                if (parentDiv)
+                {
                     parentDiv.style.display = 'block';
                 }
             }
 
-            if (horaClicada) {
-                $('#txtHoraInicial').val(horaClicada);
-                console.log('ðŸ• Hora inicial definida:', horaClicada);
-            } else {
-                $('#txtHoraInicial').val('');
-            }
-
-            if (window.RecorrenciaController?.prepararNovo) {
-                window.RecorrenciaController.prepararNovo();
-                console.log(
-                    '�o. RecorrǦncia preparada para novo agendamento (controller)',
-                );
-            } else {
-                console.warn(
-                    '�s���? RecorrenciaController nǜo dispon��vel - recorrǦncia nǜo inicializada',
-                );
-            }
-
-            $('#txtViagemId').val('');
-            $('#txtRecorrenciaViagemId').val('');
-            $('#txtStatusAgendamento').val(true);
-
-            console.log('âœ… Campos hidden limpos');
+            if (horaClicada)
+            {
+                window.setKendoTimeValue("txtHoraInicial", horaClicada);
+                console.log("🕐 Hora inicial definida:", horaClicada);
+            } else
+            {
+                window.setKendoTimeValue("txtHoraInicial", "");
+            }
+
+            const cardRecorrencia = document.getElementById("cardRecorrencia");
+            if (cardRecorrencia)
+            {
+                cardRecorrencia.style.display = "block";
+                console.log("✅ Card de Configurações de Recorrência visível");
+            }
+
+            const lstRecorrente = document.getElementById("lstRecorrente");
+            if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])
+            {
+                lstRecorrente.ej2_instances[0].enabled = true;
+                lstRecorrente.ej2_instances[0].value = "N";
+                lstRecorrente.ej2_instances[0].dataBind();
+                console.log("âœ… Recorrente definido como 'Não'");
+            }
+
+            const lstPeriodos = document.getElementById("lstPeriodos");
+            if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0])
+            {
+                lstPeriodos.ej2_instances[0].enabled = true;
+                lstPeriodos.ej2_instances[0].value = null;
+                lstPeriodos.ej2_instances[0].dataBind();
+                console.log("âœ… Perí­odo habilitado e limpo");
+            }
+
+            $("#txtViagemId").val("");
+            $("#txtRecorrenciaViagemId").val("");
+            $("#txtStatusAgendamento").val(true);
+
+            console.log("âœ… Campos hidden limpos");
 
             window.CarregandoAgendamento = false;
             window.CarregandoViagemBloqueada = false;
             window.transformandoEmViagem = false;
             window.carregandoViagemExistente = false;
 
-            console.log('âœ… Flags globais resetadas');
-
-            console.log(
-                'âœ… ===== NOVO AGENDAMENTO CONFIGURADO COM SUCESSO =====',
-            );
+            console.log("âœ… Flags globais resetadas");
+
+            console.log("âœ… ===== NOVO AGENDAMENTO CONFIGURADO COM SUCESSO =====");
         }, 100);
-    } catch (error) {
-        console.error('âŒ Erro em exibirNovaViagem:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'exibirNovaViagem',
-            error,
-        );
+    } catch (error)
+    {
+        console.error("âŒ Erro em exibirNovaViagem:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "exibirNovaViagem", error);
     }
 }
 
-function exibirViagemExistente(objViagem) {
-    try {
-
-        console.log('✏️ Editando viagem:', objViagem.viagemId);
-        console.log('📋 Dados completos:', objViagem);
+function exibirViagemExistente(objViagem)
+{
+    try
+    {
+        console.log("âœï¸ Editando viagem:", objViagem.viagemId);
+        console.log
 
         limparListboxDatasVariadas();
+        ("ðŸ“‹ Dados completos:", objViagem);
 
         window.carregandoViagemExistente = true;
 
-        if (window.timeoutAbrirModal) {
+        if (window.timeoutAbrirModal)
+        {
             clearTimeout(window.timeoutAbrirModal);
-            console.log('⏰ Timeout anterior cancelado');
+            console.log("⏰ Timeout anterior cancelado");
         }
         window.dadosRecorrenciaCarregados = objViagem;
 
@@ -517,1465 +393,1316 @@
 
         definirTituloModal(objViagem);
 
-        console.log('Escondendo todos campos opcionais no inicio...');
+        console.log("Escondendo todos campos opcionais no inicio...");
         const camposOpcionais = [
-            'divNoFichaVistoria',
-            'divDataFinal',
-            'divHoraFinal',
-            'divDuracao',
-            'divKmAtual',
-            'divKmInicial',
-            'divKmFinal',
-            'divQuilometragem',
-            'divCombustivelInicial',
-            'divCombustivelFinal',
-            'divPeriodo',
-            'divDias',
-            'divDiaMes',
-            'divFinalRecorrencia',
-            'calendarContainer',
-            'cardSelecaoEvento',
-            'sectionEvento',
-            'sectionCadastroEvento',
-            'sectionCadastroRequisitante',
-            'listboxDatasVariadasContainer',
+            "divNoFichaVistoria", "divDataFinal", "divHoraFinal", "divDuracao",
+            "divKmAtual", "divKmInicial", "divKmFinal", "divQuilometragem",
+            "divCombustivelInicial", "divCombustivelFinal",
+            "divPeriodo", "divDias", "divDiaMes", "divFinalRecorrencia", "calendarContainer",
+            "cardSelecaoEvento", "sectionEvento", "sectionCadastroEvento", "sectionCadastroRequisitante",
+            "listboxDatasVariadasContainer"
         ];
 
-        camposOpcionais.forEach((id) => {
+        camposOpcionais.forEach(id =>
+        {
             const el = document.getElementById(id);
-            if (el) el.style.display = 'none';
+            if (el) el.style.display = "none";
         });
-        console.log('Campos opcionais escondidos');
-
-        const recorrenteValor = objViagem.recorrente ?? objViagem.Recorrente;
-        const cardRecorrencia = document.getElementById('cardRecorrencia');
-
-        console.log('🔄 [Recorrência] Valor:', recorrenteValor);
-
-        if (cardRecorrencia) {
-            if (recorrenteValor === 'S' || recorrenteValor === true) {
-
-                cardRecorrencia.style.display = 'block';
-                console.log(
-                    ' ✅ Card de recorrência VISÍVEL (edição de agendamento recorrente)',
-                );
-
-                const lstRecorrenteKendo =
-                    $('#lstRecorrente').data('kendoDropDownList');
-                if (lstRecorrenteKendo) {
-                    lstRecorrenteKendo.value('S');
-                    lstRecorrenteKendo.enable(false);
-                    console.log(
-                        ' 🔒 lstRecorrente desabilitado (edição recorrente)',
-                    );
-                }
-            } else {
-
-                cardRecorrencia.style.display = 'none';
-                console.log(
-                    ' 🚫 Card de recorrência ESCONDIDO (não é recorrente)',
-                );
-            }
-        }
+        console.log("Campos opcionais escondidos");
 
         const calendarElements = {
-            calendarContainer: document.getElementById('calendarContainer'),
-            calDatasSelecionadas: document.getElementById(
-                'calDatasSelecionadas',
-            ),
-            badgeContadorDatasSelecionadas: document.getElementById(
-                'badgeContadorDatasSelecionadas',
-            ),
+            'calendarContainer': document.getElementById('calendarContainer'),
+            'calDatasSelecionadas': document.getElementById('calDatasSelecionadas'),
+            'badgeContadorDatasSelecionadas': document.getElementById('badgeContadorDatasSelecionadas')
         };
 
-        Object.entries(calendarElements).forEach(([name, elem]) => {
-            if (elem) {
+        Object.entries(calendarElements).forEach(([name, elem]) =>
+        {
+            if (elem)
+            {
                 elem.style.display = 'none';
                 console.log(` ✅ ${name} ocultado explicitamente`);
             }
         });
 
-        const labelCal = document.querySelector(
-            'label[for="calDatasSelecionadas"]',
-        );
-        if (labelCal) {
+        const labelCal = document.querySelector('label[for="calDatasSelecionadas"]');
+        if (labelCal)
+        {
             labelCal.style.display = 'none';
-            console.log(' ✅ Label do calendário ocultada');
-        }
-
-        if (typeof window.inicializarCamposModal === 'function') {
+            console.log(" ✅ Label do calendário ocultada");
+        }
+
+        if (typeof window.inicializarCamposModal === 'function')
+        {
             window.inicializarCamposModal();
-        } else {
-            console.warn('⚠️ inicializarCamposModal não disponível');
-        }
-
-        $('#txtViagemId').val(objViagem.viagemId ?? objViagem.ViagemId);
-        $('#txtRecorrenciaViagemId').val(
-            objViagem.recorrenciaViagemId ??
-                objViagem.RecorrenciaViagemId ??
-                '',
-        );
-        $('#txtStatusAgendamento').val(
-            objViagem.statusAgendamento ?? objViagem.StatusAgendamento,
-        );
-        $('#txtUsuarioIdCriacao').val(
-            objViagem.usuarioIdCriacao ?? objViagem.UsuarioIdCriacao ?? '',
-        );
-        $('#txtDataCriacao').val(
-            objViagem.dataCriacao ?? objViagem.DataCriacao ?? '',
-        );
-
-        const dataInicialValor = objViagem.dataInicial ?? objViagem.DataInicial;
-        const dataFinalValor = objViagem.dataFinal ?? objViagem.DataFinal;
-
-        console.log('📅 [DEBUG] Preenchendo datas:');
-        console.log(' dataInicial (raw):', dataInicialValor);
-        console.log(' dataFinal (raw):', dataFinalValor);
-
-        if (dataInicialValor) {
-            var kendoDataInicialPicker =
-                $('#txtDataInicial').data('kendoDatePicker');
-            console.log(
-                ' Kendo DatePicker encontrado:',
-                !!kendoDataInicialPicker,
-            );
-
-            if (kendoDataInicialPicker) {
-                try {
-                    const dataObj = new Date(dataInicialValor);
-
-                    if (!isNaN(dataObj.getTime())) {
-
-                        kendoDataInicialPicker.min(new Date(1900, 0, 1));
-                        kendoDataInicialPicker.value(dataObj);
-                        console.log(
-                            ' Min removido para edicao, data preenchida:',
-                            dataObj.toLocaleDateString('pt-BR'),
-                        );
-                    } else {
-                        console.warn(
-                            ' Data inicial invalida, usando data atual',
-                        );
-                        kendoDataInicialPicker.value(new Date());
-                    }
-                } catch (error) {
-                    console.error(' Erro ao preencher data inicial:', error);
-                    kendoDataInicialPicker.value(new Date());
-                }
-            } else {
-                console.warn(
-                    ' ⚠️ Kendo DatePicker txtDataInicial NÃO encontrado!',
-                );
-
-                const inputNativo = document.querySelector(
-                    'input[name="txtDataInicial"]',
-                );
-                if (inputNativo) {
-                    const dataObj = new Date(dataInicialValor);
-                    if (!isNaN(dataObj.getTime())) {
-
-                        const ano = dataObj.getFullYear();
-                        const mes = String(dataObj.getMonth() + 1).padStart(
-                            2,
-                            '0',
-                        );
-                        const dia = String(dataObj.getDate()).padStart(2, '0');
-                        inputNativo.value = `${dia}/${mes}/${ano}`;
-                        console.log(
-                            ' ✅ Data inicial preenchida via input nativo:',
-                            inputNativo.value,
-                        );
-                    }
-                }
-            }
-        } else {
-            console.warn(
-                ' ⚠️ dataInicial é undefined/null no objeto de viagem',
-            );
-        }
-
-        if (dataFinalValor) {
-            var kendoDataFinalPicker =
-                $('#txtDataFinal').data('kendoDatePicker');
-            if (kendoDataFinalPicker) {
-                try {
-                    const dataObj = new Date(dataFinalValor);
-
-                    if (!isNaN(dataObj.getTime())) {
-                        kendoDataFinalPicker.value(dataObj);
-                        console.log(
-                            ' ✅ Data final preenchida:',
-                            dataObj.toLocaleDateString('pt-BR'),
-                        );
-                    } else {
-                        console.warn(
-                            ' ⚠️ Data final invalida, limpando campo',
-                        );
-                        kendoDataFinalPicker.value(null);
-                    }
-                } catch (error) {
-                    console.error(' ❌ Erro ao preencher data final:', error);
-                    kendoDataFinalPicker.value(null);
-                }
-            }
-        } else {
-            console.log(
-                ' ℹ️ dataFinal é undefined/null (normal para agendamento)',
-            );
-        }
-
-        console.log('ðŸ• [DEBUG] Processando hora inicial...');
-        console.log(' - objViagem.horaInicio:', objViagem.horaInicio);
-        console.log(' - objViagem.HoraInicio:', objViagem.HoraInicio);
-        console.log(
-            ' - objViagem.horaInicialTexto:',
-            objViagem.horaInicialTexto,
-        );
-        console.log(' - objViagem.horaInicial:', objViagem.horaInicial);
+        }
+        else
+        {
+            console.warn("⚠️ inicializarCamposModal não disponível");
+        }
+
+        $("#txtViagemId").val(objViagem.viagemId);
+        $("#txtRecorrenciaViagemId").val(objViagem.recorrenciaViagemId || "");
+        $("#txtStatusAgendamento").val(objViagem.statusAgendamento);
+        $("#txtUsuarioIdCriacao").val(objViagem.usuarioIdCriacao || "");
+        $("#txtDataCriacao").val(objViagem.dataCriacao || "");
+
+        console.log("📅 [DEBUG] Preenchendo datas:");
+        console.log(" dataInicial:", objViagem.dataInicial);
+        console.log(" dataFinal:", objViagem.dataFinal);
+
+        if (objViagem.dataInicial)
+        {
+            try
+            {
+                const dataObj = new Date(objViagem.dataInicial);
+                if (!isNaN(dataObj.getTime()))
+                {
+                    window.setKendoDateValue("txtDataInicial", dataObj, true);
+                    console.log(" ✅ Data inicial preenchida:", dataObj.toLocaleDateString('pt-BR'));
+                }
+                else
+                {
+                    console.warn(" ⚠️ Data inicial inválida, usando data atual");
+                    window.setKendoDateValue("txtDataInicial", new Date(), true);
+                }
+            } catch (error)
+            {
+                console.error(" ❌ Erro ao preencher data inicial:", error);
+                window.setKendoDateValue("txtDataInicial", new Date(), true);
+            }
+        }
+        else
+        {
+            console.warn(" ⚠️ objViagem.dataInicial é undefined/null");
+        }
+
+        if (objViagem.dataFinal)
+        {
+            try
+            {
+                const dataObj = new Date(objViagem.dataFinal);
+                if (!isNaN(dataObj.getTime()))
+                {
+                    window.setKendoDateValue("txtDataFinal", dataObj, true);
+                    console.log(" ✅ Data final preenchida:", dataObj.toLocaleDateString('pt-BR'));
+                }
+                else
+                {
+                    console.warn(" ⚠️ Data final inválida, limpando campo");
+                    window.setKendoDateValue("txtDataFinal", null, true);
+                }
+            } catch (error)
+            {
+                console.error(" ❌ Erro ao preencher data final:", error);
+                window.setKendoDateValue("txtDataFinal", null, true);
+            }
+        }
+        else
+        {
+            console.log(" ℹ️ objViagem.dataFinal é undefined/null (normal para agendamento)");
+        }
+
+        console.log("ðŸ• [DEBUG] Processando hora inicial...");
+        console.log(" - objViagem.horaInicio:", objViagem.horaInicio);
+        console.log(" - objViagem.HoraInicio:", objViagem.HoraInicio);
+        console.log(" - objViagem.horaInicialTexto:", objViagem.horaInicialTexto);
+        console.log(" - objViagem.horaInicial:", objViagem.horaInicial);
 
         let horaParaExibir = null;
 
-        if (objViagem.horaInicialTexto) {
+        if (objViagem.horaInicialTexto)
+        {
             horaParaExibir = objViagem.horaInicialTexto;
         }
 
-        else if (objViagem.horaInicio || objViagem.HoraInicio) {
+        else if (objViagem.horaInicio || objViagem.HoraInicio)
+        {
             const horaOriginal = objViagem.horaInicio || objViagem.HoraInicio;
 
-            if (horaOriginal.includes('T')) {
-
-                const parteHora = horaOriginal.split('T')[1];
-                if (parteHora) {
+            if (horaOriginal.includes("T"))
+            {
+
+                const parteHora = horaOriginal.split("T")[1];
+                if (parteHora)
+                {
                     horaParaExibir = parteHora.substring(0, 5);
                 }
             }
 
-            else if (horaOriginal.includes(':')) {
-
-                if (horaOriginal.length === 5) {
+            else if (horaOriginal.includes(":"))
+            {
+
+                if (horaOriginal.length === 5)
+                {
                     horaParaExibir = horaOriginal;
                 }
 
-                else if (horaOriginal.length >= 8) {
+                else if (horaOriginal.length >= 8)
+                {
                     horaParaExibir = horaOriginal.substring(0, 5);
-                } else {
+                }
+                else
+                {
                     horaParaExibir = horaOriginal;
                 }
             }
 
-            else {
-                try {
+            else
+            {
+                try
+                {
                     const dataHora = new Date(horaOriginal);
-                    if (!isNaN(dataHora)) {
-                        const horas = String(dataHora.getHours()).padStart(
-                            2,
-                            '0',
-                        );
-                        const minutos = String(dataHora.getMinutes()).padStart(
-                            2,
-                            '0',
-                        );
+                    if (!isNaN(dataHora))
+                    {
+                        const horas = String(dataHora.getHours()).padStart(2, '0');
+                        const minutos = String(dataHora.getMinutes()).padStart(2, '0');
                         horaParaExibir = `${horas}:${minutos}`;
                     }
-                } catch (e) {
-                    console.warn('âš ï¸ Não foi possível converter hora:', e);
-                }
-            }
-        }
-
-        else if (objViagem.horaInicial) {
+                } catch (e)
+                {
+                    console.warn("âš ï¸ Não foi possível converter hora:", e);
+                }
+            }
+        }
+
+        else if (objViagem.horaInicial)
+        {
 
             const horaOriginal = objViagem.horaInicial;
-            if (
-                typeof horaOriginal === 'string' &&
-                horaOriginal.includes(':')
-            ) {
+            if (typeof horaOriginal === 'string' && horaOriginal.includes(":"))
+            {
                 horaParaExibir = horaOriginal.substring(0, 5);
             }
         }
 
-        if (horaParaExibir) {
-            $('#txtHoraInicial').val(horaParaExibir);
-            console.log('âœ… Hora inicial definida:', horaParaExibir);
-        } else {
-            console.warn(
-                'âš ï¸ Hora inicial não encontrada ou em formato inválido',
-            );
-            $('#txtHoraInicial').val('');
-        }
-
-        console.log('ðŸ• [DEBUG] Processando hora final...');
-        console.log(' - objViagem.horaFim:', objViagem.horaFim);
-        console.log(' - objViagem.HoraFim:', objViagem.HoraFim);
-        console.log(' - objViagem.horaFinalTexto:', objViagem.horaFinalTexto);
+        if (horaParaExibir)
+        {
+            window.setKendoTimeValue("txtHoraInicial", horaParaExibir);
+            console.log("✅ Hora inicial definida:", horaParaExibir);
+        } else
+        {
+            console.warn("âš ï¸ Hora inicial não encontrada ou em formato inválido");
+            window.setKendoTimeValue("txtHoraInicial", "");
+        }
+
+        console.log("ðŸ• [DEBUG] Processando hora final...");
+        console.log(" - objViagem.horaFim:", objViagem.horaFim);
+        console.log(" - objViagem.HoraFim:", objViagem.HoraFim);
+        console.log(" - objViagem.horaFinalTexto:", objViagem.horaFinalTexto);
 
         let horaFinalParaExibir = null;
 
-        if (objViagem.horaFinalTexto) {
+        if (objViagem.horaFinalTexto)
+        {
             horaFinalParaExibir = objViagem.horaFinalTexto;
-        } else if (objViagem.horaFim || objViagem.HoraFim) {
+        }
+        else if (objViagem.horaFim || objViagem.HoraFim)
+        {
             const horaFinalOriginal = objViagem.horaFim || objViagem.HoraFim;
 
-            if (horaFinalOriginal.includes('T')) {
-                const parteHora = horaFinalOriginal.split('T')[1];
-                if (parteHora) {
+            if (horaFinalOriginal.includes("T"))
+            {
+                const parteHora = horaFinalOriginal.split("T")[1];
+                if (parteHora)
+                {
                     horaFinalParaExibir = parteHora.substring(0, 5);
                 }
-            } else if (horaFinalOriginal.includes(':')) {
+            }
+            else if (horaFinalOriginal.includes(":"))
+            {
                 horaFinalParaExibir = horaFinalOriginal.substring(0, 5);
-            } else {
-                try {
+            }
+            else
+            {
+                try
+                {
                     const dataHora = new Date(horaFinalOriginal);
-                    if (!isNaN(dataHora)) {
-                        const horas = String(dataHora.getHours()).padStart(
-                            2,
-                            '0',
-                        );
-                        const minutos = String(dataHora.getMinutes()).padStart(
-                            2,
-                            '0',
-                        );
+                    if (!isNaN(dataHora))
+                    {
+                        const horas = String(dataHora.getHours()).padStart(2, '0');
+                        const minutos = String(dataHora.getMinutes()).padStart(2, '0');
                         horaFinalParaExibir = `${horas}:${minutos}`;
                     }
-                } catch (e) {
-                    console.warn(
-                        'âš ï¸ Não foi possível converter hora final:',
-                        e,
-                    );
-                }
-            }
-        }
-
-        if (horaFinalParaExibir) {
-            $('#txtHoraFinal').val(horaFinalParaExibir);
-            console.log('âœ… Hora final definida:', horaFinalParaExibir);
-        } else {
-            $('#txtHoraFinal').val('');
-        }
-
-        if (objViagem.motoristaId) {
-            const lstMotorista = document.getElementById('lstMotorista');
-            if (
-                lstMotorista &&
-                lstMotorista.ej2_instances &&
-                lstMotorista.ej2_instances[0]
-            ) {
+                } catch (e)
+                {
+                    console.warn("âš ï¸ Não foi possível converter hora final:", e);
+                }
+            }
+        }
+
+        if (horaFinalParaExibir)
+        {
+            window.setKendoTimeValue("txtHoraFinal", horaFinalParaExibir);
+            console.log("✅ Hora final definida:", horaFinalParaExibir);
+        } else
+        {
+            window.setKendoTimeValue("txtHoraFinal", "");
+        }
+
+        if (objViagem.motoristaId)
+        {
+            const lstMotorista = document.getElementById("lstMotorista");
+            if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0])
+            {
                 lstMotorista.ej2_instances[0].value = objViagem.motoristaId;
                 lstMotorista.ej2_instances[0].dataBind();
             }
         }
 
-        if (objViagem.veiculoId) {
-            const lstVeiculo = document.getElementById('lstVeiculo');
-            if (
-                lstVeiculo &&
-                lstVeiculo.ej2_instances &&
-                lstVeiculo.ej2_instances[0]
-            ) {
+        if (objViagem.veiculoId)
+        {
+            const lstVeiculo = document.getElementById("lstVeiculo");
+            if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0])
+            {
                 lstVeiculo.ej2_instances[0].value = objViagem.veiculoId;
                 lstVeiculo.ej2_instances[0].dataBind();
             }
         }
 
-        const lstFinalidade = document.getElementById('lstFinalidade');
-        if (
-            lstFinalidade &&
-            lstFinalidade.ej2_instances &&
-            lstFinalidade.ej2_instances[0]
-        ) {
+        const lstFinalidade = document.getElementById("lstFinalidade");
+        if (lstFinalidade && lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0])
+        {
             const finalidadeInst = lstFinalidade.ej2_instances[0];
-            let finalidadeId =
-                objViagem.finalidadeViagemId || objViagem.finalidadeId;
+            let finalidadeId = objViagem.finalidadeViagemId || objViagem.finalidadeId;
             const finalidadeNome = objViagem.finalidade;
 
-            console.log('🔍 DEBUG Finalidade:');
-            console.log(' finalidadeViagemId:', objViagem.finalidadeViagemId);
-            console.log(' finalidadeId:', objViagem.finalidadeId);
-            console.log(' finalidade (nome):', finalidadeNome);
-
-            if (!finalidadeId && finalidadeNome) {
-
-                if (
-                    finalidadeInst.treeData &&
-                    finalidadeInst.treeData.length > 0
-                ) {
-                    const finalidadeEncontrada = finalidadeInst.treeData.find(
-                        (f) => f.Descricao === finalidadeNome,
-                    );
-                    if (finalidadeEncontrada) {
+            console.log("🔍 DEBUG Finalidade:");
+            console.log(" finalidadeViagemId:", objViagem.finalidadeViagemId);
+            console.log(" finalidadeId:", objViagem.finalidadeId);
+            console.log(" finalidade (nome):", finalidadeNome);
+
+            if (!finalidadeId && finalidadeNome)
+            {
+
+                if (finalidadeInst.treeData && finalidadeInst.treeData.length > 0)
+                {
+                    const finalidadeEncontrada = finalidadeInst.treeData.find(f => f.Descricao === finalidadeNome);
+                    if (finalidadeEncontrada)
+                    {
                         finalidadeId = finalidadeEncontrada.FinalidadeId;
-                        console.log(
-                            ' ✅ ID encontrado em treeData:',
-                            finalidadeId,
-                        );
+                        console.log(" ✅ ID encontrado em treeData:", finalidadeId);
                     }
                 }
 
-                if (
-                    !finalidadeId &&
-                    finalidadeInst.treeItems &&
-                    finalidadeInst.treeItems.length > 0
-                ) {
-                    const finalidadeEncontrada = finalidadeInst.treeItems.find(
-                        (f) => f.Descricao === finalidadeNome,
-                    );
-                    if (finalidadeEncontrada) {
+                if (!finalidadeId && finalidadeInst.treeItems && finalidadeInst.treeItems.length > 0)
+                {
+                    const finalidadeEncontrada = finalidadeInst.treeItems.find(f => f.Descricao === finalidadeNome);
+                    if (finalidadeEncontrada)
+                    {
                         finalidadeId = finalidadeEncontrada.FinalidadeId;
-                        console.log(
-                            ' ✅ ID encontrado em treeItems:',
-                            finalidadeId,
-                        );
+                        console.log(" ✅ ID encontrado em treeItems:", finalidadeId);
                     }
                 }
             }
 
-            if (finalidadeId) {
-                setTimeout(() => {
+            if (finalidadeId)
+            {
+                setTimeout(() =>
+                {
                     finalidadeInst.value = [finalidadeId];
-                    if (typeof finalidadeInst.dataBind === 'function') {
+                    if (typeof finalidadeInst.dataBind === 'function')
+                    {
                         finalidadeInst.dataBind();
                     }
-                    console.log('✅ Finalidade carregada:', finalidadeId);
+                    console.log("✅ Finalidade carregada:", finalidadeId);
                 }, 200);
-            } else {
-                console.log('ℹ️ Finalidade não informada');
-            }
-        }
-
-        const txtRamalRequisitanteSF = document.getElementById(
-            'txtRamalRequisitanteSF',
-        );
-        if (txtRamalRequisitanteSF) {
-            const ramal =
-                objViagem.ramalRequisitanteSF ||
-                objViagem.ramalRequisitante ||
-                '';
+            } else
+            {
+                console.log("ℹ️ Finalidade não informada");
+            }
+        }
+
+        const txtRamalRequisitanteSF = document.getElementById("txtRamalRequisitanteSF");
+        if (txtRamalRequisitanteSF)
+        {
+            const ramal = objViagem.ramalRequisitanteSF || objViagem.ramalRequisitante || "";
             txtRamalRequisitanteSF.value = ramal;
-            console.log('✅ Ramal requisitante carregado:', ramal);
-        }
-
-        const lstSetorRequisitanteAgendamento = document.getElementById(
-            'lstSetorRequisitanteAgendamento',
-        );
-        if (
-            lstSetorRequisitanteAgendamento &&
-            lstSetorRequisitanteAgendamento.ej2_instances &&
-            lstSetorRequisitanteAgendamento.ej2_instances[0]
-        ) {
+            console.log("✅ Ramal requisitante carregado:", ramal);
+        }
+
+        const lstSetorRequisitanteAgendamento = document.getElementById("lstSetorRequisitanteAgendamento");
+        if (lstSetorRequisitanteAgendamento && lstSetorRequisitanteAgendamento.ej2_instances && lstSetorRequisitanteAgendamento.ej2_instances[0])
+        {
             const setorInst = lstSetorRequisitanteAgendamento.ej2_instances[0];
 
-            const setorId =
-                objViagem.setorSolicitanteId ||
-                objViagem.setorRequisitanteId ||
-                objViagem.setorId;
-            let setorNome =
-                objViagem.setorSolicitante ||
-                objViagem.nomeSetorRequisitante ||
-                objViagem.setorRequisitanteNome ||
-                objViagem.setorNome ||
-                objViagem.nomeSetor ||
-                null;
-
-            console.log('🔍 DEBUG Setor Requisitante:');
-            console.log(' setorSolicitanteId:', objViagem.setorSolicitanteId);
-            console.log(' setorId:', setorId);
-            console.log(' setorNome (do objeto):', setorNome);
-
-            if (setorId) {
-
-                if (
-                    !setorNome &&
-                    setorInst.treeItems &&
-                    setorInst.treeItems.length > 0
-                ) {
-                    const setorEncontrado = setorInst.treeItems.find(
-                        (s) => s.SetorSolicitanteId === setorId,
-                    );
-                    if (setorEncontrado) {
+            const setorId = objViagem.setorSolicitanteId || objViagem.setorRequisitanteId || objViagem.setorId;
+            let setorNome = objViagem.setorSolicitante || objViagem.nomeSetorRequisitante || objViagem.setorRequisitanteNome || objViagem.setorNome || objViagem.nomeSetor || null;
+
+            console.log("🔍 DEBUG Setor Requisitante:");
+            console.log(" setorSolicitanteId:", objViagem.setorSolicitanteId);
+            console.log(" setorId:", setorId);
+            console.log(" setorNome (do objeto):", setorNome);
+
+            if (setorId)
+            {
+
+                if (!setorNome && setorInst.treeItems && setorInst.treeItems.length > 0)
+                {
+                    const setorEncontrado = setorInst.treeItems.find(s => s.SetorSolicitanteId === setorId);
+                    if (setorEncontrado)
+                    {
                         setorNome = setorEncontrado.Nome;
-                        console.log(
-                            ' ✅ Nome encontrado em treeItems:',
-                            setorNome,
-                        );
-                    } else {
-                        console.warn(' ⚠️ Setor não encontrado em treeItems');
+                        console.log(" ✅ Nome encontrado em treeItems:", setorNome);
+                    } else
+                    {
+                        console.warn(" ⚠️ Setor não encontrado em treeItems");
                     }
                 }
 
-                setTimeout(() => {
-                    try {
+                setTimeout(() =>
+                {
+                    try
+                    {
                         setorInst.value = [setorId];
 
-                        if (setorNome) {
+                        if (setorNome)
+                        {
                             setorInst.text = setorNome;
                         }
 
-                        if (typeof setorInst.dataBind === 'function') {
+                        if (typeof setorInst.dataBind === 'function')
+                        {
                             setorInst.dataBind();
                         }
 
-                        if (typeof setorInst.refresh === 'function') {
+                        if (typeof setorInst.refresh === 'function')
+                        {
                             setorInst.refresh();
                         }
 
-                        console.log(
-                            '✅ Setor requisitante carregado:',
-                            setorId,
-                        );
-                        console.log(' Texto exibido:', setorInst.text);
-                        console.log(' Value atual:', setorInst.value);
-                    } catch (error) {
-                        console.error('❌ Erro ao definir setor:', error);
+                        console.log("✅ Setor requisitante carregado:", setorId);
+                        console.log(" Texto exibido:", setorInst.text);
+                        console.log(" Value atual:", setorInst.value);
+                    } catch (error)
+                    {
+                        console.error("❌ Erro ao definir setor:", error);
                     }
                 }, 500);
-            } else {
-                console.log('ℹ️ Setor requisitante não informado');
-            }
-        } else {
-            console.error(
-                '❌ lstSetorRequisitanteAgendamento não encontrado ou não inicializado',
-            );
-        }
-
-        const requisitanteId =
-            objViagem.requisitanteId || objViagem.RequisitanteId;
-
-        console.log('🔍 DEBUG EXIBIÇÃO - Requisitante:');
-        console.log(' - objViagem.requisitanteId:', objViagem.requisitanteId);
-        console.log(' - objViagem.RequisitanteId:', objViagem.RequisitanteId);
-        console.log(' - requisitanteId final:', requisitanteId);
-
-        if (requisitanteId) {
-
-            const kendoComboBox = $('#lstRequisitante').data('kendoComboBox');
-            console.log(
-                ' - kendoComboBox encontrado:',
-                kendoComboBox ? 'SIM' : 'NÃO',
-            );
-
-            if (kendoComboBox) {
-                console.log(' - Tentando preencher com:', requisitanteId);
+            } else
+            {
+                console.log("ℹ️ Setor requisitante não informado");
+            }
+        } else
+        {
+            console.error("❌ lstSetorRequisitanteAgendamento não encontrado ou não inicializado");
+        }
+
+        const requisitanteId = objViagem.requisitanteId || objViagem.RequisitanteId;
+
+        console.log("🔍 DEBUG EXIBIÇÃO - Requisitante:");
+        console.log(" - objViagem.requisitanteId:", objViagem.requisitanteId);
+        console.log(" - objViagem.RequisitanteId:", objViagem.RequisitanteId);
+        console.log(" - requisitanteId final:", requisitanteId);
+
+        if (requisitanteId)
+        {
+
+            const kendoComboBox = $("#lstRequisitante").data("kendoComboBox");
+            console.log(" - kendoComboBox encontrado:", kendoComboBox ? "SIM" : "NÃO");
+
+            if (kendoComboBox)
+            {
+                console.log(" - Tentando preencher com:", requisitanteId);
 
                 setTimeout(() => {
                     kendoComboBox.value(requisitanteId);
-                    kendoComboBox.trigger('change');
+                    kendoComboBox.trigger("change");
 
                     const valorAtual = kendoComboBox.value();
                     const textoAtual = kendoComboBox.text();
-                    console.log(
-                        ' - Valor após preencher (com delay):',
-                        valorAtual,
-                    );
-                    console.log(' - Texto exibido:', textoAtual);
+                    console.log(" - Valor após preencher (com delay):", valorAtual);
+                    console.log(" - Texto exibido:", textoAtual);
                 }, 300);
             }
         }
 
-        if (objViagem.origem) {
-            const cmbOrigem = document.getElementById('cmbOrigem');
-            if (
-                cmbOrigem &&
-                cmbOrigem.ej2_instances &&
-                cmbOrigem.ej2_instances[0]
-            ) {
+        if (objViagem.origem)
+        {
+            const cmbOrigem = document.getElementById("cmbOrigem");
+            if (cmbOrigem && cmbOrigem.ej2_instances && cmbOrigem.ej2_instances[0])
+            {
                 cmbOrigem.ej2_instances[0].value = objViagem.origem;
                 cmbOrigem.ej2_instances[0].dataBind();
             }
         }
 
-        if (objViagem.destino) {
-            const cmbDestino = document.getElementById('cmbDestino');
-            if (
-                cmbDestino &&
-                cmbDestino.ej2_instances &&
-                cmbDestino.ej2_instances[0]
-            ) {
+        if (objViagem.destino)
+        {
+            const cmbDestino = document.getElementById("cmbDestino");
+            if (cmbDestino && cmbDestino.ej2_instances && cmbDestino.ej2_instances[0])
+            {
                 cmbDestino.ej2_instances[0].value = objViagem.destino;
                 cmbDestino.ej2_instances[0].dataBind();
             }
         }
 
-        if (objViagem.descricao) {
-            const rteDescricao = document.getElementById('rteDescricao');
-            if (
-                rteDescricao &&
-                rteDescricao.ej2_instances &&
-                rteDescricao.ej2_instances[0]
-            ) {
+        if (objViagem.descricao)
+        {
+            const rteDescricao = document.getElementById("rteDescricao");
+            if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances[0])
+            {
                 rteDescricao.ej2_instances[0].value = objViagem.descricao;
                 rteDescricao.ej2_instances[0].dataBind();
             }
         }
 
         const noFichaVal = objViagem.noFichaVistoria;
-        const txtNoFicha = $('#txtNoFichaVistoria');
-        if (
-            noFichaVal === 0 ||
-            noFichaVal === '0' ||
-            noFichaVal === null ||
-            noFichaVal === ''
-        ) {
-            txtNoFicha.val('');
-            txtNoFicha.attr('placeholder', '(mobile)');
-            txtNoFicha.addClass('placeholder-mobile');
-        } else {
+        const txtNoFicha = $("#txtNoFichaVistoria");
+        if (noFichaVal === 0 || noFichaVal === "0" || noFichaVal === null || noFichaVal === "")
+        {
+            txtNoFicha.val("");
+            txtNoFicha.attr("placeholder", "(mobile)");
+            txtNoFicha.addClass("placeholder-mobile");
+        }
+        else
+        {
             txtNoFicha.val(noFichaVal);
-            txtNoFicha.attr('placeholder', '');
-            txtNoFicha.removeClass('placeholder-mobile');
-        }
-
-        const btnFichaVistoria = document.getElementById(
-            'btnVisualizarFichaVistoria',
-        );
-        console.log('🔴 DEBUG BOTÃO FICHA:', {
-            botaoExiste: !!btnFichaVistoria,
-            temFichaVistoriaReal: objViagem.temFichaVistoriaReal,
-            noFichaVistoria: objViagem.noFichaVistoria,
-            viagemId: objViagem.viagemId,
-        });
-        if (btnFichaVistoria) {
-
-            const temFichaReal = objViagem.temFichaVistoriaReal === true;
-
-            if (temFichaReal) {
-                btnFichaVistoria.style.display = 'inline-flex';
-                btnFichaVistoria.disabled = false;
-                btnFichaVistoria.title = 'Visualizar Ficha de Vistoria';
-                btnFichaVistoria.setAttribute(
-                    'data-ejtip',
-                    'Clique para visualizar/imprimir a Ficha de Vistoria desta viagem',
-                );
-            } else {
-                btnFichaVistoria.style.display = 'none';
-                btnFichaVistoria.disabled = true;
-            }
-
-            btnFichaVistoria.dataset.viagemId = objViagem.viagemId;
-            btnFichaVistoria.dataset.noFicha = noFichaVal || '';
-        }
-
-        $('#txtKmAtual').val(objViagem.kmAtual || '');
-        $('#txtKmInicial').val(objViagem.kmInicial || '');
-        $('#txtKmFinal').val(objViagem.kmFinal || '');
-
-        if (objViagem.combustivelInicial) {
-            const ddtCombustivelInicial = document.getElementById(
-                'ddtCombustivelInicial',
-            );
-            if (
-                ddtCombustivelInicial &&
-                ddtCombustivelInicial.ej2_instances &&
-                ddtCombustivelInicial.ej2_instances[0]
-            ) {
-                ddtCombustivelInicial.ej2_instances[0].value =
-                    objViagem.combustivelInicial;
+            txtNoFicha.attr("placeholder", "");
+            txtNoFicha.removeClass("placeholder-mobile");
+        }
+        $("#txtKmAtual").val(objViagem.kmAtual || "");
+        $("#txtKmInicial").val(objViagem.kmInicial || "");
+        $("#txtKmFinal").val(objViagem.kmFinal || "");
+
+        if (objViagem.combustivelInicial)
+        {
+            const ddtCombustivelInicial = document.getElementById("ddtCombustivelInicial");
+            if (ddtCombustivelInicial && ddtCombustivelInicial.ej2_instances && ddtCombustivelInicial.ej2_instances[0])
+            {
+                ddtCombustivelInicial.ej2_instances[0].value = objViagem.combustivelInicial;
                 ddtCombustivelInicial.ej2_instances[0].dataBind();
             }
         }
 
-        if (objViagem.combustivelFinal) {
-            const ddtCombustivelFinal = document.getElementById(
-                'ddtCombustivelFinal',
-            );
-            if (
-                ddtCombustivelFinal &&
-                ddtCombustivelFinal.ej2_instances &&
-                ddtCombustivelFinal.ej2_instances[0]
-            ) {
-                ddtCombustivelFinal.ej2_instances[0].value =
-                    objViagem.combustivelFinal;
+        if (objViagem.combustivelFinal)
+        {
+            const ddtCombustivelFinal = document.getElementById("ddtCombustivelFinal");
+            if (ddtCombustivelFinal && ddtCombustivelFinal.ej2_instances && ddtCombustivelFinal.ej2_instances[0])
+            {
+                ddtCombustivelFinal.ej2_instances[0].value = objViagem.combustivelFinal;
                 ddtCombustivelFinal.ej2_instances[0].dataBind();
             }
         }
 
-        console.log('=== DEBUG EVENTO ===');
-        console.log('objViagem.eventoId:', objViagem.eventoId);
-        console.log('objViagem.evento:', objViagem.evento);
-        console.log('objViagem.eventoNome:', objViagem.eventoNome);
-
-        if (objViagem.eventoId) {
-            console.log('✅ EventoId existe:', objViagem.eventoId);
-
-            const lstEventos = document.getElementById('lstEventos');
-            console.log('lstEventos elemento:', lstEventos);
-            console.log(
-                'lstEventos tem ej2_instances?',
-                lstEventos?.ej2_instances,
-            );
-            console.log(
-                'lstEventos instância [0]:',
-                lstEventos?.ej2_instances?.[0],
-            );
-
-            if (
-                lstEventos &&
-                lstEventos.ej2_instances &&
-                lstEventos.ej2_instances[0]
-            ) {
-                console.log('✅ lstEventos ENCONTRADO e INICIALIZADO');
+        console.log("=== DEBUG EVENTO ===");
+        console.log("objViagem.eventoId:", objViagem.eventoId);
+        console.log("objViagem.evento:", objViagem.evento);
+        console.log("objViagem.eventoNome:", objViagem.eventoNome);
+
+        if (objViagem.eventoId)
+        {
+            console.log("✅ EventoId existe:", objViagem.eventoId);
+
+            const lstEventos = document.getElementById("lstEventos");
+            console.log("lstEventos elemento:", lstEventos);
+            console.log("lstEventos tem ej2_instances?", lstEventos?.ej2_instances);
+            console.log("lstEventos instância [0]:", lstEventos?.ej2_instances?.[0]);
+
+            if (lstEventos && lstEventos.ej2_instances && lstEventos.ej2_instances[0])
+            {
+                console.log("✅ lstEventos ENCONTRADO e INICIALIZADO");
 
                 const lstEventosInst = lstEventos.ej2_instances[0];
 
-                console.log(
-                    'DataSource do lstEventos:',
-                    lstEventosInst.dataSource,
-                );
-                console.log('Valor atual:', lstEventosInst.value);
-
-                console.log('🔄 Definindo valor:', objViagem.eventoId);
+                console.log("DataSource do lstEventos:", lstEventosInst.dataSource);
+                console.log("Valor atual:", lstEventosInst.value);
+
+                console.log("🔄 Definindo valor:", objViagem.eventoId);
                 lstEventosInst.value = objViagem.eventoId;
                 lstEventosInst.dataBind();
 
-                console.log(
-                    '✅ Valor definido. Novo valor:',
-                    lstEventosInst.value,
-                );
-                console.log('Texto selecionado:', lstEventosInst.text);
-
-                const divDadosEventoSelecionado = document.getElementById(
-                    'divDadosEventoSelecionado',
-                );
-                console.log(
-                    'divDadosEventoSelecionado elemento:',
-                    divDadosEventoSelecionado,
-                );
-
-                if (divDadosEventoSelecionado) {
-                    console.log(
-                        'Display ANTES:',
-                        divDadosEventoSelecionado.style.display,
-                    );
-                    divDadosEventoSelecionado.style.display = 'flex';
-                    console.log(
-                        'Display DEPOIS:',
-                        divDadosEventoSelecionado.style.display,
-                    );
-                    console.log(
-                        "✅ divDadosEventoSelecionado.display = 'flex'",
-                    );
-                } else {
-                    console.error(
-                        '❌ divDadosEventoSelecionado NÃO ENCONTRADA!',
-                    );
-                }
-
-                setTimeout(() => {
-                    console.log(
-                        '⏰ Timeout 500ms completado, iniciando AJAX...',
-                    );
-                    console.log(
-                        'URL que será chamada: /api/ViagemEvento/ObterPorId?id=' +
-                            objViagem.eventoId,
-                    );
+                console.log("✅ Valor definido. Novo valor:", lstEventosInst.value);
+                console.log("Texto selecionado:", lstEventosInst.text);
+
+                const divDadosEventoSelecionado = document.getElementById("divDadosEventoSelecionado");
+                console.log("divDadosEventoSelecionado elemento:", divDadosEventoSelecionado);
+
+                if (divDadosEventoSelecionado)
+                {
+                    console.log("Display ANTES:", divDadosEventoSelecionado.style.display);
+                    divDadosEventoSelecionado.style.display = "flex";
+                    console.log("Display DEPOIS:", divDadosEventoSelecionado.style.display);
+                    console.log("✅ divDadosEventoSelecionado.display = 'flex'");
+                }
+                else
+                {
+                    console.error("❌ divDadosEventoSelecionado NÃO ENCONTRADA!");
+                }
+
+                setTimeout(() =>
+                {
+                    console.log("⏰ Timeout 500ms completado, iniciando AJAX...");
+                    console.log("URL que será chamada: /api/ViagemEvento/ObterPorId?id=" + objViagem.eventoId);
 
                     $.ajax({
                         url: '/api/ViagemEvento/ObterPorId',
                         method: 'GET',
                         data: { id: objViagem.eventoId },
-                        success: function (response) {
-                            console.log('✅ AJAX SUCCESS!');
-                            console.log('Response completo:', response);
-                            console.log('response.success:', response.success);
-                            console.log('response.data:', response.data);
-
-                            if (response.success && response.data) {
+                        success: function (response)
+                        {
+                            console.log("✅ AJAX SUCCESS!");
+                            console.log("Response completo:", response);
+                            console.log("response.success:", response.success);
+                            console.log("response.data:", response.data);
+
+                            if (response.success && response.data)
+                            {
                                 const evento = response.data;
-                                console.log('Evento:', evento);
-
-                                console.log('--- Processando Data Início ---');
-                                const dataInicial =
-                                    evento.DataInicial || evento.dataInicial;
-                                console.log('dataInicial:', dataInicial);
-
-                                var kendoDataInicioEventoPicker = $(
-                                    '#txtDataInicioEvento',
-                                ).data('kendoDatePicker');
-                                console.log(
-                                    'txtDataInicioEvento elemento:',
-                                    document.getElementById(
-                                        'txtDataInicioEvento',
-                                    ),
-                                );
-                                console.log(
-                                    'kendoDatePicker:',
-                                    kendoDataInicioEventoPicker,
-                                );
-
-                                if (
-                                    dataInicial &&
-                                    kendoDataInicioEventoPicker
-                                ) {
-                                    try {
+                                console.log("Evento:", evento);
+
+                                console.log("--- Processando Data Início ---");
+                                const dataInicial = evento.DataInicial || evento.dataInicial;
+                                console.log("dataInicial:", dataInicial);
+
+                                const txtDataInicioEvento = document.getElementById("txtDataInicioEvento");
+                                console.log("txtDataInicioEvento elemento:", txtDataInicioEvento);
+                                console.log("txtDataInicioEvento.ej2_instances:", txtDataInicioEvento?.ej2_instances);
+
+                                if (dataInicial && txtDataInicioEvento && txtDataInicioEvento.ej2_instances && txtDataInicioEvento.ej2_instances[0])
+                                {
+                                    try
+                                    {
                                         const dataObj = new Date(dataInicial);
 
-                                        if (!isNaN(dataObj.getTime())) {
-                                            kendoDataInicioEventoPicker.value(
-                                                dataObj,
-                                            );
-                                            kendoDataInicioEventoPicker.enable(
-                                                false,
-                                            );
-                                            console.log(
-                                                'Data Inicio preenchida:',
-                                                dataObj.toLocaleDateString(
-                                                    'pt-BR',
-                                                ),
-                                            );
-                                        } else {
-                                            console.error(
-                                                'Data Inicio invalida:',
-                                                dataInicial,
-                                            );
-                                            kendoDataInicioEventoPicker.value(
-                                                null,
-                                            );
+                                        if (!isNaN(dataObj.getTime()))
+                                        {
+                                            txtDataInicioEvento.ej2_instances[0].value = dataObj;
+                                            txtDataInicioEvento.ej2_instances[0].enabled = false;
+                                            txtDataInicioEvento.ej2_instances[0].dataBind();
+                                            console.log("✅ Data Início preenchida:", dataObj.toLocaleDateString('pt-BR'));
                                         }
-                                    } catch (error) {
-                                        console.error(
-                                            'Erro ao preencher Data Inicio:',
-                                            error,
-                                        );
-                                        kendoDataInicioEventoPicker.value(null);
+                                        else
+                                        {
+                                            console.error("❌ Data Início inválida:", dataInicial);
+                                            txtDataInicioEvento.ej2_instances[0].value = null;
+                                            txtDataInicioEvento.ej2_instances[0].dataBind();
+                                        }
+                                    } catch (error)
+                                    {
+                                        console.error("❌ Erro ao preencher Data Início:", error);
+                                        txtDataInicioEvento.ej2_instances[0].value = null;
+                                        txtDataInicioEvento.ej2_instances[0].dataBind();
                                     }
-                                } else {
-                                    console.error(
-                                        'Nao foi possivel preencher Data Inicio - componente nao encontrado ou data vazia',
-                                    );
                                 }
-
-                                console.log('--- Processando Data Fim ---');
-                                const dataFinal =
-                                    evento.DataFinal || evento.dataFinal;
-                                console.log('dataFinal:', dataFinal);
-
-                                var kendoDataFimEventoPicker =
-                                    $('#txtDataFimEvento').data(
-                                        'kendoDatePicker',
-                                    );
-                                console.log(
-                                    'txtDataFimEvento elemento:',
-                                    document.getElementById('txtDataFimEvento'),
-                                );
-
-                                if (dataFinal && kendoDataFimEventoPicker) {
-                                    try {
+                                else
+                                {
+                                    console.error("❌ Não foi possível preencher Data Início - componente não encontrado ou data vazia");
+                                }
+
+                                console.log("--- Processando Data Fim ---");
+                                const dataFinal = evento.DataFinal || evento.dataFinal;
+                                console.log("dataFinal:", dataFinal);
+
+                                const txtDataFimEvento = document.getElementById("txtDataFimEvento");
+                                console.log("txtDataFimEvento elemento:", txtDataFimEvento);
+
+                                if (dataFinal && txtDataFimEvento && txtDataFimEvento.ej2_instances && txtDataFimEvento.ej2_instances[0])
+                                {
+                                    try
+                                    {
                                         const dataObj = new Date(dataFinal);
 
-                                        if (!isNaN(dataObj.getTime())) {
-                                            kendoDataFimEventoPicker.value(
-                                                dataObj,
-                                            );
-                                            kendoDataFimEventoPicker.enable(
-                                                false,
-                                            );
-                                            console.log(
-                                                'Data Fim preenchida:',
-                                                dataObj.toLocaleDateString(
-                                                    'pt-BR',
-                                                ),
-                                            );
-                                        } else {
-                                            console.error(
-                                                'Data Fim invalida:',
-                                                dataFinal,
-                                            );
-                                            kendoDataFimEventoPicker.value(
-                                                null,
-                                            );
+                                        if (!isNaN(dataObj.getTime()))
+                                        {
+                                            txtDataFimEvento.ej2_instances[0].value = dataObj;
+                                            txtDataFimEvento.ej2_instances[0].enabled = false;
+                                            txtDataFimEvento.ej2_instances[0].dataBind();
+                                            console.log("✅ Data Fim preenchida:", dataObj.toLocaleDateString('pt-BR'));
                                         }
-                                    } catch (error) {
-                                        console.error(
-                                            'Erro ao preencher Data Fim:',
-                                            error,
-                                        );
-                                        kendoDataFimEventoPicker.value(null);
+                                        else
+                                        {
+                                            console.error("❌ Data Fim inválida:", dataFinal);
+                                            txtDataFimEvento.ej2_instances[0].value = null;
+                                            txtDataFimEvento.ej2_instances[0].dataBind();
+                                        }
+                                    } catch (error)
+                                    {
+                                        console.error("❌ Erro ao preencher Data Fim:", error);
+                                        txtDataFimEvento.ej2_instances[0].value = null;
+                                        txtDataFimEvento.ej2_instances[0].dataBind();
                                     }
-                                } else {
-                                    console.error(
-                                        'Nao foi possivel preencher Data Fim - componente nao encontrado ou data vazia',
-                                    );
                                 }
-
-                                console.log(
-                                    '--- Processando Qtd Participantes ---',
-                                );
-                                const qtdParticipantes =
-                                    evento.QtdParticipantes ||
-                                    evento.qtdParticipantes;
-                                console.log(
-                                    'qtdParticipantes:',
-                                    qtdParticipantes,
-                                );
-
-                                const kendoNumericQtd = $(
-                                    '#txtQtdParticipantesEvento',
-                                ).data('kendoNumericTextBox');
-                                console.log(
-                                    'txtQtdParticipantesEvento Kendo:',
-                                    kendoNumericQtd,
-                                );
-
-                                if (
-                                    qtdParticipantes !== undefined &&
-                                    kendoNumericQtd
-                                ) {
-                                    const qtdNumero =
-                                        typeof qtdParticipantes === 'string'
-                                            ? parseInt(qtdParticipantes, 10)
-                                            : qtdParticipantes;
-                                    kendoNumericQtd.value(qtdNumero);
-                                    kendoNumericQtd.enable(false);
-                                    console.log(
-                                        '✅ Qtd Participantes preenchida',
-                                    );
-                                } else {
-                                    console.error(
-                                        '❌ Não foi possível preencher Qtd Participantes',
-                                    );
+                                else
+                                {
+                                    console.error("❌ Não foi possível preencher Data Fim - componente não encontrado ou data vazia");
                                 }
 
-                                console.log('=== FIM PREENCHIMENTO EVENTO ===');
-                            } else {
-                                console.error(
-                                    '❌ response.success é false OU response.data está vazio',
-                                );
+                                console.log("--- Processando Qtd Participantes ---");
+                                const qtdParticipantes = evento.QtdParticipantes || evento.qtdParticipantes;
+                                console.log("qtdParticipantes:", qtdParticipantes);
+
+                                const txtQtdParticipantesEvento = document.getElementById("txtQtdParticipantesEvento");
+                                console.log("txtQtdParticipantesEvento elemento:", txtQtdParticipantesEvento);
+
+                                if (qtdParticipantes !== undefined && txtQtdParticipantesEvento && txtQtdParticipantesEvento.ej2_instances && txtQtdParticipantesEvento.ej2_instances[0])
+                                {
+                                    const qtdNumero = typeof qtdParticipantes === 'string' ? parseInt(qtdParticipantes, 10) : qtdParticipantes;
+                                    txtQtdParticipantesEvento.ej2_instances[0].value = qtdNumero;
+                                    txtQtdParticipantesEvento.ej2_instances[0].enabled = false;
+                                    txtQtdParticipantesEvento.ej2_instances[0].dataBind();
+                                    console.log("✅ Qtd Participantes preenchida");
+                                }
+                                else
+                                {
+                                    console.error("❌ Não foi possível preencher Qtd Participantes");
+                                }
+
+                                console.log("=== FIM PREENCHIMENTO EVENTO ===");
+                            }
+                            else
+                            {
+                                console.error("❌ response.success é false OU response.data está vazio");
                             }
                         },
-                        error: function (xhr, status, error) {
-                            console.error('❌ AJAX ERROR!');
-                            console.error('Status:', status);
-                            console.error('Error:', error);
-                            console.error('xhr:', xhr);
-                            console.error(
-                                'xhr.responseText:',
-                                xhr.responseText,
-                            );
-                        },
+                        error: function (xhr, status, error)
+                        {
+                            console.error("❌ AJAX ERROR!");
+                            console.error("Status:", status);
+                            console.error("Error:", error);
+                            console.error("xhr:", xhr);
+                            console.error("xhr.responseText:", xhr.responseText);
+                        }
                     });
                 }, 500);
-            } else {
-                console.error(
-                    '❌ lstEventos NÃO encontrado ou NÃO inicializado!',
-                );
-                if (!lstEventos)
-                    console.error(' - Elemento não existe no DOM');
-                if (lstEventos && !lstEventos.ej2_instances)
-                    console.error(' - Não tem ej2_instances');
-                if (
-                    lstEventos &&
-                    lstEventos.ej2_instances &&
-                    !lstEventos.ej2_instances[0]
-                )
-                    console.error(' - ej2_instances[0] é undefined');
-            }
-        } else {
-            console.log('⚠️ objViagem.eventoId está vazio/null');
-        }
-        console.log('=== FIM DEBUG EVENTO ===');
-
-        if (window.RecorrenciaController?.aplicarEdicao) {
-            window.RecorrenciaController.aplicarEdicao(objViagem);
-        } else {
-            console.warn(
-                '?? RecorrenciaController n�o dispon�vel - recorr�ncia n�o sincronizada. Aplicando fallback local.',
-            );
-            restaurarDadosRecorrencia(objViagem);
-        }
-
-        if (objViagem.status === 'Aberta' || objViagem.status === 'Realizada') {
+            }
+            else
+            {
+                console.error("❌ lstEventos NÃO encontrado ou NÃO inicializado!");
+                if (!lstEventos) console.error(" - Elemento não existe no DOM");
+                if (lstEventos && !lstEventos.ej2_instances) console.error(" - Não tem ej2_instances");
+                if (lstEventos && lstEventos.ej2_instances && !lstEventos.ej2_instances[0]) console.error(" - ej2_instances[0] é undefined");
+            }
+        }
+        else
+        {
+            console.log("⚠️ objViagem.eventoId está vazio/null");
+        }
+        console.log("=== FIM DEBUG EVENTO ===");
+
+        const isRecorrente = objViagem.recorrente === "S" ||
+            (objViagem.intervalo && objViagem.intervalo !== "" && objViagem.intervalo !== "N") ||
+            (objViagem.recorrenciaViagemId && objViagem.recorrenciaViagemId !== "00000000-0000-0000-0000-000000000000");
+
+        if (isRecorrente)
+        {
+            console.log("RECORRENTE: Agendamento é RECORRENTE");
+            console.log(" - Recorrente:", objViagem.recorrente);
+            console.log(" - Intervalo:", objViagem.intervalo);
+            console.log(" - RecorrenciaViagemId:", objViagem.recorrenciaViagemId);
+
+            const cardRecorrencia = document.getElementById("cardRecorrencia");
+            if (cardRecorrencia)
+            {
+                cardRecorrencia.style.display = "block";
+                console.log("✅ Card de Configurações de Recorrência visível");
+            }
+
+            const lstRecorrente = document.getElementById("lstRecorrente");
+            if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])
+            {
+                lstRecorrente.ej2_instances[0].value = "S";
+                lstRecorrente.ej2_instances[0].enabled = false;
+                lstRecorrente.ej2_instances[0].dataBind();
+                console.log("âœ… lstRecorrente definido como 'Sim'");
+            }
+
+            const divPeriodo = document.getElementById("divPeriodo");
+            if (divPeriodo)
+            {
+                divPeriodo.style.setProperty('display', 'block', 'important');
+                console.log("âœ… divPeriodo visível");
+            }
+
+            const lstPeriodos = document.getElementById("lstPeriodos");
+            if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0])
+            {
+                lstPeriodos.ej2_instances[0].value = objViagem.intervalo;
+                lstPeriodos.ej2_instances[0].enabled = false;
+                lstPeriodos.ej2_instances[0].dataBind();
+                console.log("âœ… Perí­odo definido:", objViagem.intervalo);
+            }
+
+            setTimeout(() =>
+            {
+                configurarCamposRecorrencia(objViagem);
+
+                if (window.garantirVisibilidadeRecorrencia)
+                {
+                    window.garantirVisibilidadeRecorrencia(objViagem);
+                }
+
+                window.ignorarEventosRecorrencia = false;
+                console.log("âœ… Eventos de recorrência reabilitados");
+            }, 500);
+        } else
+        {
+            console.log("âŒ Agendamento NÃO é recorrente");
+
+            const lstRecorrente = document.getElementById("lstRecorrente");
+            if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])
+            {
+                lstRecorrente.ej2_instances[0].value = "N";
+                lstRecorrente.ej2_instances[0].enabled = false;
+                console.log("✅ lstRecorrente definido como 'Não' e desabilitado");
+            }
+
+            $("#divPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia, #calendarContainer").hide();
+
+            const cardRecorrencia = document.getElementById("cardRecorrencia");
+            if (cardRecorrencia)
+            {
+                cardRecorrencia.style.display = "none";
+                console.log("✅ Card de Configurações de Recorrência ocultado");
+            }
+
+            setTimeout(() =>
+            {
+                window.ignorarEventosRecorrencia = false;
+            }, 500);
+        }
+
+        if (objViagem.status === "Aberta" || objViagem.status === "Realizada")
+        {
             mostrarCamposViagem(objViagem);
         }
 
         configurarBotoesPorStatus(objViagem);
 
-        setTimeout(async () => {
+        setTimeout(async () =>
+        {
             await configurarRodapeLabelsExistente(objViagem);
         }, 400);
 
-        if (typeof window.calcularDuracaoViagem === 'function') {
+        if (typeof window.calcularDuracaoViagem === 'function')
+        {
             window.calcularDuracaoViagem();
         }
 
-        if (typeof window.calcularDistanciaViagem === 'function') {
+        if (typeof window.calcularDistanciaViagem === 'function')
+        {
             window.calcularDistanciaViagem();
         }
 
-        window.timeoutAbrirModal = setTimeout(() => {
-            console.log('⏰ [exibirViagemExistente] Tentando abrir modal...');
-            $('#modalViagens').modal('show');
-            console.log('✅ Modal aberto');
-
-            setTimeout(() => {
-                window.carregandoViagemExistente = false;
-                console.log(
-                    '✅ Flag carregandoViagemExistente resetada após modal aberto',
-                );
-            }, 1000);
+        window.timeoutAbrirModal = setTimeout(() =>
+        {
+            console.log("⏰ [exibirViagemExistente] Tentando abrir modal...");
+            $("#modalViagens").modal("show");
+            console.log("✅ Modal aberto");
         }, 550);
-    } catch (error) {
+    } catch (error)
+    {
 
         window.carregandoViagemExistente = false;
         window.ignorarEventosRecorrencia = false;
 
-        console.error('âŒ Erro ao exibir viagem existente:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'exibirViagemExistente',
-            error,
-        );
+        console.error("âŒ Erro ao exibir viagem existente:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "exibirViagemExistente", error);
     }
 }
 
-function configurarCamposRecorrencia(objViagem) {
-    try {
-        if (window.RecorrenciaController?.aplicarEdicao) {
-            window.RecorrenciaController.aplicarEdicao(objViagem);
-            return;
-        }
-
-        const intervalo = objViagem.Intervalo || objViagem.intervalo;
-        console.log(
-            '🔧 Configurando campos de recorrência para tipo:',
-            intervalo,
-        );
-
-        switch (intervalo) {
-            case 'D':
+function configurarCamposRecorrencia(objViagem)
+{
+    try
+    {
+        console.log("ðŸ”§ Configurando campos de recorrência para tipo:", objViagem.intervalo);
+
+        const intervalo = objViagem.intervalo;
+
+        switch (intervalo)
+        {
+            case "D":
                 configurarRecorrenciaDiaria(objViagem);
                 break;
 
-            case 'S':
-            case 'Q':
+            case "S":
+            case "Q":
                 configurarRecorrenciaSemanal(objViagem);
                 break;
 
-            case 'M':
+            case "M":
                 configurarRecorrenciaMensal(objViagem);
                 break;
 
-            case 'V':
-                console.log(
-                    '🔧 Tipo Variada detectado - chamando configurarRecorrenciaVariada',
-                );
-                console.log(' objViagem passado:', objViagem);
+            case "V":
+                console.log("🔧 Tipo Variada detectado - chamando configurarRecorrenciaVariada");
+                console.log(" objViagem passado:", objViagem);
 
                 configurarRecorrenciaVariada(objViagem)
-                    .then(() => {
-                        console.log(
-                            '✅ configurarRecorrenciaVariada concluída com sucesso',
-                        );
+                    .then(() =>
+                    {
+                        console.log("✅ configurarRecorrenciaVariada concluída com sucesso");
                     })
-                    .catch((err) => {
-                        console.error(
-                            '❌ Erro em configurarRecorrenciaVariada:',
-                            err,
-                        );
-                        console.error(' Stack:', err.stack);
+                    .catch(err =>
+                    {
+                        console.error("❌ Erro em configurarRecorrenciaVariada:", err);
+                        console.error(" Stack:", err.stack);
                     })
-                    .finally(() => {
-                        console.log(
-                            '📊 configurarRecorrenciaVariada finalizada (finally do then)',
-                        );
+                    .finally(() =>
+                    {
+                        console.log("📊 configurarRecorrenciaVariada finalizada (finally do then)");
                     });
                 break;
 
             default:
-                console.warn(
-                    'âš ï¸ Tipo de recorrência não reconhecido:',
-                    intervalo,
-                );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'configurarCamposRecorrencia',
-            error,
-        );
+                console.warn("âš ï¸ Tipo de recorrência não reconhecido:", intervalo);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarCamposRecorrencia", error);
     }
 }
 
-function configurarRecorrenciaDiaria(objViagem) {
-    try {
-        console.log('ðŸ“… Configurando campos para recorrência DIÃRIA');
-
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        console.log(
-            ' DEBUG - divFinalRecorrencia encontrado?',
-            !!divFinalRecorrencia,
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
+function configurarRecorrenciaDiaria(objViagem)
+{
+    try
+    {
+        console.log("ðŸ“… Configurando campos para recorrência DIÃRIA");
+
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        console.log(" DEBUG - divFinalRecorrencia encontrado?", !!divFinalRecorrencia);
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
             console.log(" DEBUG - divFinalRecorrencia display='block'");
         }
 
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Diária',
-        });
-    } catch (error) {
-        console.error('❌ Erro em configurarRecorrenciaDiaria:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'configurarRecorrenciaDiaria',
-            error,
-        );
+        console.log(" DEBUG - dataFinalRecorrencia:", objViagem.dataFinalRecorrencia);
+        if (objViagem.dataFinalRecorrencia)
+        {
+            const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
+            console.log(" DEBUG - txtFinalRecorrencia encontrado?", !!txtFinalRecorrencia);
+            console.log(" DEBUG - Kendo instance?", !!window.getKendoDatePicker("txtFinalRecorrencia"));
+
+            if (txtFinalRecorrencia)
+            {
+                console.log(" ✅ Kendo OK! Setando valor COM DELAY...");
+                const dataObj = new Date(objViagem.dataFinalRecorrencia);
+                console.log(" DEBUG - Data:", dataObj);
+
+                setTimeout(() => {
+                    window.setKendoDateValue("txtFinalRecorrencia", dataObj, true);
+                    window.enableKendoDatePicker("txtFinalRecorrencia", false);
+                    console.log(" ✅ SETADO (com delay 500ms) - value:", dataObj);
+                }, 500);
+            }
+            else
+            {
+                console.warn(" ⚠️ Componente Kendo NÃO encontrado!");
+            }
+        }
+        else
+        {
+            console.warn(" ⚠️ dataFinalRecorrencia VAZIO!");
+        }
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em configurarRecorrenciaDiaria:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRecorrenciaDiaria", error);
     }
 }
 
-function configurarRecorrenciaSemanal(objViagem) {
-    try {
-        console.log(
-            'ðŸ“… Configurando campos para recorrência SEMANAL/QUINZENAL',
-        );
-
-        const divDias = document.getElementById('divDias');
-        if (divDias) {
+function configurarRecorrenciaSemanal(objViagem)
+{
+    try
+    {
+        console.log("ðŸ“… Configurando campos para recorrência SEMANAL/QUINZENAL");
+
+        const divDias = document.getElementById("divDias");
+        if (divDias)
+        {
             divDias.style.setProperty('display', 'block', 'important');
         }
 
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        const lstDiasKendo = $('#lstDias').data('kendoMultiSelect');
-        if (lstDiasKendo) {
-
-            const dataSource = lstDiasKendo.dataSource.data();
-            if (!dataSource || dataSource.length === 0) {
-                console.log('🔄 lstDias vazio - populando antes de usar...');
-                if (typeof window.inicializarLstDias === 'function') {
-                    window.inicializarLstDias();
-                }
-            }
-
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        const lstDias = document.getElementById("lstDias");
+        if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0])
+        {
             const diasSelecionados = [];
 
-            const monday = objViagem.Monday || objViagem.monday;
-            const tuesday = objViagem.Tuesday || objViagem.tuesday;
-            const wednesday = objViagem.Wednesday || objViagem.wednesday;
-            const thursday = objViagem.Thursday || objViagem.thursday;
-            const friday = objViagem.Friday || objViagem.friday;
-            const saturday = objViagem.Saturday || objViagem.saturday;
-            const sunday = objViagem.Sunday || objViagem.sunday;
-
-            if (monday) diasSelecionados.push(1);
-            if (tuesday) diasSelecionados.push(2);
-            if (wednesday) diasSelecionados.push(3);
-            if (thursday) diasSelecionados.push(4);
-            if (friday) diasSelecionados.push(5);
-            if (saturday) diasSelecionados.push(6);
-            if (sunday) diasSelecionados.push(0);
-
-            console.log('📋 Dias selecionados (valores):', diasSelecionados);
-
-            setTimeout(() => {
-                lstDiasKendo.value(diasSelecionados);
-                lstDiasKendo.enable(false);
-                console.log(
-                    '✅ lstDias (Kendo) configurado com dias selecionados',
-                );
-            }, 100);
-        } else {
-            console.warn(
-                '⚠️ lstDias (Kendo) não encontrado ou não inicializado',
-            );
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Semanal/Quinzenal',
-        });
-    } catch (error) {
-        console.error('❌ Erro em configurarRecorrenciaSemanal:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'configurarRecorrenciaSemanal',
-            error,
-        );
+            if (objViagem.Monday) diasSelecionados.push("Segunda");
+            if (objViagem.Tuesday) diasSelecionados.push("Terça");
+            if (objViagem.Wednesday) diasSelecionados.push("Quarta");
+            if (objViagem.Thursday) diasSelecionados.push("Quinta");
+            if (objViagem.Friday) diasSelecionados.push("Sexta");
+            if (objViagem.Saturday) diasSelecionados.push("Sábado");
+            if (objViagem.Sunday) diasSelecionados.push("Domingo");
+
+            console.log("ðŸ“‹ Dias selecionados:", diasSelecionados);
+
+            lstDias.ej2_instances[0].value = diasSelecionados;
+            lstDias.ej2_instances[0].enabled = false;
+            lstDias.ej2_instances[0].dataBind();
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            try
+            {
+                const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
+                const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");
+
+                console.log("🔍 DEBUG Data Final Recorrência:");
+                console.log(" - txtFinalRecorrencia existe?", !!txtFinalRecorrencia);
+                console.log(" - txtFinalRecorrenciaTexto existe?", !!txtFinalRecorrenciaTexto);
+                console.log(" - dataFinalRecorrencia:", objViagem.dataFinalRecorrencia);
+
+                if (txtFinalRecorrenciaTexto)
+                {
+
+                    const dataFinal = new Date(objViagem.dataFinalRecorrencia);
+                    const dia = String(dataFinal.getDate()).padStart(2, '0');
+                    const mes = String(dataFinal.getMonth() + 1).padStart(2, '0');
+                    const ano = dataFinal.getFullYear();
+                    const dataFormatada = `${dia}/${mes}/${ano}`;
+
+                    txtFinalRecorrenciaTexto.value = dataFormatada;
+                    txtFinalRecorrenciaTexto.style.display = "block";
+                    console.log(" - Campo de texto definido como:", dataFormatada);
+
+                    if (txtFinalRecorrencia)
+                    {
+                        window.showKendoDatePicker("txtFinalRecorrencia", false);
+                        console.log(" - Wrapper do DatePicker também ocultado");
+                    }
+
+                    console.log(`✅ Data Final Recorrência exibida em campo de texto: ${dataFormatada}`);
+                }
+                else
+                {
+                    console.error("❌ Campo txtFinalRecorrenciaTexto não encontrado no DOM!");
+                }
+            }
+            catch (error)
+            {
+                console.error("❌ Erro ao definir Data Final Recorrência:", error);
+            }
+        }
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em configurarRecorrenciaSemanal:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRecorrenciaSemanal", error);
     }
 }
 
-function configurarRecorrenciaMensal(objViagem) {
-    try {
-        console.log('ðŸ“… Configurando campos para recorrência MENSAL');
-
-        const divDiaMes = document.getElementById('divDiaMes');
-        if (divDiaMes) {
+function configurarRecorrenciaMensal(objViagem)
+{
+    try
+    {
+        console.log("ðŸ“… Configurando campos para recorrência MENSAL");
+
+        const divDiaMes = document.getElementById("divDiaMes");
+        if (divDiaMes)
+        {
             divDiaMes.style.setProperty('display', 'block', 'important');
         }
 
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        if (objViagem.DiaMesRecorrencia) {
-            const lstDiasMesKendo = $('#lstDiasMes').data('kendoDropDownList');
-            if (lstDiasMesKendo) {
-                lstDiasMesKendo.value(objViagem.DiaMesRecorrencia);
-                lstDiasMesKendo.enable(false);
-            }
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Mensal',
-        });
-    } catch (error) {
-        console.error('❌ Erro em configurarRecorrenciaMensal:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'configurarRecorrenciaMensal',
-            error,
-        );
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        if (objViagem.diaMesRecorrencia)
+        {
+            const lstDiasMes = document.getElementById("lstDiasMes");
+            if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0])
+            {
+                lstDiasMes.ej2_instances[0].value = objViagem.diaMesRecorrencia;
+                lstDiasMes.ej2_instances[0].enabled = false;
+                lstDiasMes.ej2_instances[0].dataBind();
+            }
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            try
+            {
+                const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
+                const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");
+
+                console.log("🔍 DEBUG Data Final Recorrência:");
+                console.log(" - txtFinalRecorrencia existe?", !!txtFinalRecorrencia);
+                console.log(" - txtFinalRecorrenciaTexto existe?", !!txtFinalRecorrenciaTexto);
+                console.log(" - dataFinalRecorrencia:", objViagem.dataFinalRecorrencia);
+
+                if (txtFinalRecorrenciaTexto)
+                {
+
+                    const dataFinal = new Date(objViagem.dataFinalRecorrencia);
+                    const dia = String(dataFinal.getDate()).padStart(2, '0');
+                    const mes = String(dataFinal.getMonth() + 1).padStart(2, '0');
+                    const ano = dataFinal.getFullYear();
+                    const dataFormatada = `${dia}/${mes}/${ano}`;
+
+                    txtFinalRecorrenciaTexto.value = dataFormatada;
+                    txtFinalRecorrenciaTexto.style.display = "block";
+                    console.log(" - Campo de texto definido como:", dataFormatada);
+
+                    if (txtFinalRecorrencia)
+                    {
+                        window.showKendoDatePicker("txtFinalRecorrencia", false);
+                        console.log(" - Wrapper do DatePicker também ocultado");
+                    }
+
+                    console.log(`✅ Data Final Recorrência exibida em campo de texto: ${dataFormatada}`);
+                }
+                else
+                {
+                    console.error("❌ Campo txtFinalRecorrenciaTexto não encontrado no DOM!");
+                }
+            }
+            catch (error)
+            {
+                console.error("❌ Erro ao definir Data Final Recorrência:", error);
+            }
+        }
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em configurarRecorrenciaMensal:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRecorrenciaMensal", error);
     }
 }
 
-async function buscarDatasRecorrenciaVariada(recorrenciaViagemId, viagemId) {
-    try {
-        console.log(
-            '🔍 [buscarDatasRecorrenciaVariada] === INICIANDO BUSCA ===',
-        );
-        console.log(' - RecorrenciaViagemId:', recorrenciaViagemId);
-        console.log(' - ViagemId:', viagemId);
+async function buscarDatasRecorrenciaVariada(recorrenciaViagemId, viagemId)
+{
+    try
+    {
+        console.log("🔍 [buscarDatasRecorrenciaVariada] === INICIANDO BUSCA ===");
+        console.log(" - RecorrenciaViagemId:", recorrenciaViagemId);
+        console.log(" - ViagemId:", viagemId);
 
         let idParaBusca;
 
-        if (
-            recorrenciaViagemId &&
-            recorrenciaViagemId !== '00000000-0000-0000-0000-000000000000' &&
-            recorrenciaViagemId !== 0
-        ) {
+        if (recorrenciaViagemId &&
+            recorrenciaViagemId !== "00000000-0000-0000-0000-000000000000" &&
+            recorrenciaViagemId !== 0)
+        {
             idParaBusca = recorrenciaViagemId;
-            console.log('✅ Usando RecorrenciaViagemId:', idParaBusca);
-        } else if (
-            viagemId &&
-            viagemId !== '00000000-0000-0000-0000-000000000000' &&
-            viagemId !== 0
-        ) {
+            console.log("✅ Usando RecorrenciaViagemId:", idParaBusca);
+        }
+        else if (viagemId &&
+            viagemId !== "00000000-0000-0000-0000-000000000000" &&
+            viagemId !== 0)
+        {
             idParaBusca = viagemId;
-            console.log('✅ Usando ViagemId:', idParaBusca);
-        } else {
-            console.error('❌ Nenhum ID válido fornecido!');
+            console.log("✅ Usando ViagemId:", idParaBusca);
+        }
+        else
+        {
+            console.error("❌ Nenhum ID válido fornecido!");
             return [];
         }
 
         const url = `/api/Agenda/BuscarViagensRecorrencia?id=${idParaBusca}`;
-        console.log('📡 Chamando nova API:', url);
+        console.log("📡 Chamando nova API:", url);
 
         const response = await fetch(url);
 
-        if (!response.ok) {
-            console.error(
-                '❌ Erro HTTP:',
-                response.status,
-                response.statusText,
-            );
-
-            try {
+        if (!response.ok)
+        {
+            console.error("❌ Erro HTTP:", response.status, response.statusText);
+
+            try
+            {
                 const errorText = await response.text();
-                console.error('❌ Detalhes do erro:', errorText);
-            } catch (e) {
-                console.error('❌ Não foi possível ler detalhes do erro');
+                console.error("❌ Detalhes do erro:", errorText);
+            } catch (e)
+            {
+                console.error("❌ Não foi possível ler detalhes do erro");
             }
 
             return [];
         }
 
         const result = await response.json();
-        console.log('📦 Resposta da API:', result);
+        console.log("📦 Resposta da API:", result);
 
         let viagensArray = Array.isArray(result) ? result : [];
 
-        console.log(
-            `📦 Total de ${viagensArray.length} registro(s) encontrado(s)`,
-        );
-
-        if (viagensArray.length === 0) {
-            console.warn('⚠️ Nenhuma viagem encontrada para a recorrência');
-            console.warn(' ID buscado:', idParaBusca);
+        console.log(`📦 Total de ${viagensArray.length} registro(s) encontrado(s)`);
+
+        if (viagensArray.length === 0)
+        {
+            console.warn("⚠️ Nenhuma viagem encontrada para a recorrência");
+            console.warn(" ID buscado:", idParaBusca);
             return [];
         }
 
-        console.log('📋 Detalhes das viagens encontradas:');
-        viagensArray.forEach((viagem, index) => {
+        console.log("📋 Detalhes das viagens encontradas:");
+        viagensArray.forEach((viagem, index) =>
+        {
             console.log(` Viagem ${index + 1}:`);
             console.log(` - ViagemId: ${viagem.viagemId}`);
-            console.log(
-                ` - RecorrenciaViagemId: ${viagem.recorrenciaViagemId}`,
-            );
+            console.log(` - RecorrenciaViagemId: ${viagem.recorrenciaViagemId}`);
             console.log(` - DataInicial: ${viagem.dataInicial}`);
         });
 
         const datas = viagensArray
-            .map((viagem) => {
+            .map(viagem =>
+            {
                 const dataStr = viagem.dataInicial;
 
-                if (!dataStr) {
+                if (!dataStr)
+                {
                     console.warn(`⚠️ Viagem sem data inicial:`, viagem);
                     return null;
                 }
 
                 return new Date(dataStr);
             })
-            .filter((data) => data !== null && !isNaN(data.getTime()))
+            .filter(data => data !== null && !isNaN(data.getTime()))
             .sort((a, b) => a - b);
 
-        console.log('✅ Total de datas válidas processadas:', datas.length);
-
-        if (datas.length > 0) {
-            console.log('📅 Datas da recorrência:');
-            datas.forEach((data, index) => {
+        console.log("✅ Total de datas válidas processadas:", datas.length);
+
+        if (datas.length > 0)
+        {
+            console.log("📅 Datas da recorrência:");
+            datas.forEach((data, index) =>
+            {
                 const dataFormatada = data.toLocaleDateString('pt-BR', {
                     weekday: 'long',
                     year: 'numeric',
                     month: 'long',
-                    day: 'numeric',
+                    day: 'numeric'
                 });
                 console.log(` ${index + 1}. ${dataFormatada}`);
             });
-        } else {
-            console.error('❌ NENHUMA DATA VÁLIDA FOI EXTRAÍDA!');
+        } else
+        {
+            console.error("❌ NENHUMA DATA VÁLIDA FOI EXTRAÍDA!");
         }
 
         return datas;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'buscarDatasRecorrenciaVariada',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "buscarDatasRecorrenciaVariada", error);
         return [];
     }
 }
 
-function limparListboxDatasVariadasCompleto() {
-    try {
-        console.log('🧹 Limpando listbox de datas variadas completamente...');
-
-        const listbox = document.getElementById('lstDatasVariadas');
-        if (listbox) {
+function limparListboxDatasVariadasCompleto()
+{
+    try
+    {
+        console.log("🧹 Limpando listbox de datas variadas completamente...");
+
+        const listbox = document.getElementById("lstDatasVariadas");
+        if (listbox)
+        {
             listbox.innerHTML = '';
             listbox.options.length = 0;
             listbox.style.display = 'none';
-            console.log(' ✅ Listbox limpa');
-        }
-
-        const container = document.getElementById(
-            'listboxDatasVariadasContainer',
-        );
-        if (container) {
+            console.log(" ✅ Listbox limpa");
+        }
+
+        const container = document.getElementById('listboxDatasVariadasContainer');
+        if (container)
+        {
             container.style.display = 'none';
-            console.log(' ✅ Container ocultado');
+            console.log(" ✅ Container ocultado");
         }
 
         const badge = document.getElementById('badgeContadorDatasVariadas');
-        if (badge) {
+        if (badge)
+        {
             badge.textContent = '0';
             badge.style.display = 'none';
-            console.log(' ✅ Badge limpo');
-        }
-
-        document.querySelectorAll('label').forEach((label) => {
-            if (
-                label.textContent &&
-                label.textContent.includes('Selecione as Datas')
-            ) {
+            console.log(" ✅ Badge limpo");
+        }
+
+        document.querySelectorAll('label').forEach(label =>
+        {
+            if (label.textContent && label.textContent.includes('Selecione as Datas'))
+            {
                 label.style.display = 'none';
                 label.style.visibility = 'hidden';
             }
         });
 
-        console.log('✅ Limpeza completa concluída');
-    } catch (error) {
-        console.error('❌ Erro na limpeza:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'limparListboxDatasVariadasCompleto',
-            error,
-        );
+        console.log("✅ Limpeza completa concluída");
+    }
+    catch (error)
+    {
+        console.error("❌ Erro na limpeza:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "limparListboxDatasVariadasCompleto", error);
     }
 }
 
 window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
 
-async function verificarAPICorrigida() {
-    try {
-        console.log('🔍 Verificando se API ObterAgendamento foi corrigida...');
-
-        const testGuid = '00000000-0000-0000-0000-000000000000';
+async function verificarAPICorrigida()
+{
+    try
+    {
+        console.log("🔍 Verificando se API ObterAgendamento foi corrigida...");
+
+        const testGuid = "00000000-0000-0000-0000-000000000000";
         const url = `/api/Agenda/ObterAgendamento?viagemId=${testGuid}`;
 
         const response = await fetch(url, {
             method: 'GET',
-            headers: { 'Content-Type': 'application/json' },
+            headers: { 'Content-Type': 'application/json' }
         });
 
-        if (response.status === 404 || response.status === 200) {
-            console.log('✅ API ObterAgendamento parece estar corrigida!');
+        if (response.status === 404 || response.status === 200)
+        {
+            console.log("✅ API ObterAgendamento parece estar corrigida!");
             window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
             return true;
-        } else {
-            console.warn(
-                '⚠️ API ObterAgendamento pode não estar corrigida (status: ' +
-                    response.status +
-                    ')',
-            );
+        }
+        else
+        {
+            console.warn("⚠️ API ObterAgendamento pode não estar corrigida (status: " + response.status + ")");
             window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
             return false;
         }
-    } catch (error) {
-        console.error('❌ Erro ao verificar API:', error);
+    }
+    catch (error)
+    {
+        console.error("❌ Erro ao verificar API:", error);
         window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
         return false;
     }
 }
 
-async function configurarRecorrenciaVariada(objViagem) {
-    try {
-        console.log('=== configurarRecorrenciaVariada - INÍCIO ===');
+async function configurarRecorrenciaVariada(objViagem)
+{
+    try
+    {
+        console.log("=== configurarRecorrenciaVariada - INÍCIO ===");
         const startTime = Date.now();
-        console.log(
-            '📊 Status BUSCAR_PRIMEIRO_AGENDAMENTO:',
-            window.BUSCAR_PRIMEIRO_AGENDAMENTO,
-        );
-        if (window.BUSCAR_PRIMEIRO_AGENDAMENTO) {
-            console.warn(
-                '⚠️ ATENÇÃO: Busca do primeiro habilitada - pode causar DELAYS!',
-            );
-        } else {
-            console.log('✅ Busca do primeiro DESABILITADA - sem delays!');
-        }
-
-        if (!objViagem?.viagemId) {
-            console.error('❌ objViagem inválido');
+        console.log("📊 Status BUSCAR_PRIMEIRO_AGENDAMENTO:", window.BUSCAR_PRIMEIRO_AGENDAMENTO);
+        if (window.BUSCAR_PRIMEIRO_AGENDAMENTO)
+        {
+            console.warn("⚠️ ATENÇÃO: Busca do primeiro habilitada - pode causar DELAYS!");
+        } else
+        {
+            console.log("✅ Busca do primeiro DESABILITADA - sem delays!");
+        }
+
+        if (!objViagem?.viagemId)
+        {
+            console.error("❌ objViagem inválido");
             return;
         }
 
-        document.querySelectorAll('label').forEach((label) => {
-            if (
-                label.textContent &&
-                label.textContent.includes('Selecione as Datas')
-            ) {
+        document.querySelectorAll('label').forEach(label =>
+        {
+            if (label.textContent && label.textContent.includes('Selecione as Datas'))
+            {
                 label.style.display = 'none';
                 label.style.visibility = 'hidden';
                 console.log(" ✅ Label 'Selecione as Datas' ocultada");
@@ -1983,336 +1710,295 @@
         });
 
         const calendarContainer = document.getElementById('calendarContainer');
-        if (calendarContainer) {
+        if (calendarContainer)
+        {
             calendarContainer.style.display = 'none';
-            console.log(' ✅ Calendário ocultado');
-        }
-
-        const labelCalendar = document.querySelector(
-            'label[for="calDatasSelecionadas"]',
-        );
-        if (labelCalendar) {
+            console.log(" ✅ Calendário ocultado");
+        }
+
+        const labelCalendar = document.querySelector('label[for="calDatasSelecionadas"]');
+        if (labelCalendar)
+        {
             labelCalendar.style.display = 'none';
-            console.log(' ✅ Label do calendário ocultada');
-        }
-
-        const badgeCalendar = document.getElementById(
-            'badgeContadorDatasSelecionadas',
-        );
-        if (badgeCalendar) {
+            console.log(" ✅ Label do calendário ocultada");
+        }
+
+        const badgeCalendar = document.getElementById('badgeContadorDatasSelecionadas');
+        if (badgeCalendar)
+        {
             badgeCalendar.style.display = 'none';
-            console.log(' ✅ Badge do calendário ocultado');
+            console.log(" ✅ Badge do calendário ocultado");
         }
 
         const todasDatas = [];
 
-        if (objViagem.dataInicial) {
+        if (objViagem.dataInicial)
+        {
             todasDatas.push(objViagem.dataInicial);
         }
 
-        const container = document.getElementById(
-            'listboxDatasVariadasContainer',
-        );
+        const container = document.getElementById('listboxDatasVariadasContainer');
         const listbox = document.getElementById('lstDatasVariadas');
 
-        if (container) {
+        if (container)
+        {
             container.style.display = 'block';
-            console.log(' ✅ Container tornado visível');
-        }
-
-        if (listbox) {
+            console.log(" ✅ Container tornado visível");
+        }
+
+        if (listbox)
+        {
             listbox.style.display = 'block';
-            console.log(' ✅ Listbox tornada visível');
+            console.log(" ✅ Listbox tornada visível");
         }
 
         let idParaBuscar = null;
-        const recorrenciaVazia =
-            !objViagem.RecorrenciaViagemId ||
-            objViagem.RecorrenciaViagemId ===
-                '00000000-0000-0000-0000-000000000000';
-
-        if (recorrenciaVazia) {
+        const recorrenciaVazia = !objViagem.recorrenciaViagemId ||
+            objViagem.recorrenciaViagemId === "00000000-0000-0000-0000-000000000000";
+
+        if (recorrenciaVazia)
+        {
 
             idParaBuscar = objViagem.viagemId;
-            console.log(
-                ` 📌 Primeiro agendamento - buscar por ViagemId: ${idParaBuscar}`,
-            );
-        } else {
-
-            idParaBuscar = objViagem.RecorrenciaViagemId;
-            console.log(
-                ` 📌 Subsequente - buscar por RecorrenciaViagemId: ${idParaBuscar}`,
-            );
-        }
-
-        if (idParaBuscar) {
-            try {
+            console.log(` 📌 Primeiro agendamento - buscar por ViagemId: ${idParaBuscar}`);
+        }
+        else
+        {
+
+            idParaBuscar = objViagem.recorrenciaViagemId;
+            console.log(` 📌 Subsequente - buscar por RecorrenciaViagemId: ${idParaBuscar}`);
+        }
+
+        if (idParaBuscar)
+        {
+            try
+            {
                 const url = `/api/Agenda/BuscarViagensRecorrencia?id=${idParaBuscar}`;
                 console.log(`⏱️ Chamando API BuscarViagensRecorrencia...`);
                 const apiStartTime = Date.now();
 
                 const controller = new AbortController();
-                const timeoutId = setTimeout(() => {
-                    console.error(
-                        '❌ Timeout na API BuscarViagensRecorrencia (2s)',
-                    );
+                const timeoutId = setTimeout(() =>
+                {
+                    console.error("❌ Timeout na API BuscarViagensRecorrencia (2s)");
                     controller.abort();
                 }, 2000);
 
                 const response = await fetch(url);
 
-                if (response.ok) {
+                if (response.ok)
+                {
                     const viagens = await response.json();
-                    console.log(
-                        ` 📦 Retornaram ${viagens.length} viagens da API`,
-                    );
-                    console.log(
-                        ` 📊 Detalhes das viagens retornadas:`,
-                        viagens,
-                    );
+                    console.log(` 📦 Retornaram ${viagens.length} viagens da API`);
+                    console.log(` 📊 Detalhes das viagens retornadas:`, viagens);
 
                     let datasAdicionadas = 0;
-                    viagens.forEach((v) => {
-                        console.log(
-                            ` Verificando viagem: ${v.viagemId}, Data: ${v.dataInicial}`,
-                        );
-                        if (
-                            v.dataInicial &&
-                            v.viagemId !== objViagem.viagemId
-                        ) {
+                    viagens.forEach(v =>
+                    {
+                        console.log(` Verificando viagem: ${v.viagemId}, Data: ${v.dataInicial}`);
+                        if (v.dataInicial && v.viagemId !== objViagem.viagemId)
+                        {
                             todasDatas.push(v.dataInicial);
                             datasAdicionadas++;
-                            console.log(
-                                ` ✅ Data adicionada: ${v.dataInicial}`,
-                            );
-                        } else if (v.viagemId === objViagem.viagemId) {
-                            console.log(
-                                ` ⏩ Pulando data atual: ${v.dataInicial}`,
-                            );
+                            console.log(` ✅ Data adicionada: ${v.dataInicial}`);
+                        }
+                        else if (v.viagemId === objViagem.viagemId)
+                        {
+                            console.log(` ⏩ Pulando data atual: ${v.dataInicial}`);
                         }
                     });
-                    console.log(
-                        ` 📊 Total de datas adicionadas da API: ${datasAdicionadas}`,
-                    );
-
-                    if (!recorrenciaVazia) {
-                        console.log(
-                            ` 🔍 Verificando se o primeiro agendamento está na lista...`,
-                        );
-                        console.log(
-                            ` ID do primeiro (RecorrenciaViagemId): ${idParaBuscar}`,
-                        );
-
-                        const primeiroNaLista = viagens.find((v) => {
-                            const ehPrimeiro =
-                                v.viagemId === idParaBuscar ||
+                    console.log(` 📊 Total de datas adicionadas da API: ${datasAdicionadas}`);
+
+                    if (!recorrenciaVazia)
+                    {
+                        console.log(` 🔍 Verificando se o primeiro agendamento está na lista...`);
+                        console.log(` ID do primeiro (RecorrenciaViagemId): ${idParaBuscar}`);
+
+                        const primeiroNaLista = viagens.find(v =>
+                        {
+                            const ehPrimeiro = v.viagemId === idParaBuscar ||
                                 !v.recorrenciaViagemId ||
-                                v.recorrenciaViagemId ===
-                                    '00000000-0000-0000-0000-000000000000';
+                                v.recorrenciaViagemId === "00000000-0000-0000-0000-000000000000";
                             return ehPrimeiro;
                         });
 
-                        if (primeiroNaLista) {
-                            console.log(
-                                ` ✅ Primeiro encontrado na lista: ${primeiroNaLista.viagemId}`,
-                            );
-
-                            if (
-                                primeiroNaLista.dataInicial &&
-                                !todasDatas.includes(
-                                    primeiroNaLista.dataInicial,
-                                )
-                            ) {
+                        if (primeiroNaLista)
+                        {
+                            console.log(` ✅ Primeiro encontrado na lista: ${primeiroNaLista.viagemId}`);
+
+                            if (primeiroNaLista.dataInicial && !todasDatas.includes(primeiroNaLista.dataInicial))
+                            {
                                 todasDatas.push(primeiroNaLista.dataInicial);
-                                console.log(
-                                    ` ✅ Data do primeiro adicionada: ${primeiroNaLista.dataInicial}`,
-                                );
+                                console.log(` ✅ Data do primeiro adicionada: ${primeiroNaLista.dataInicial}`);
                             }
-                        } else if (window.BUSCAR_PRIMEIRO_AGENDAMENTO) {
-
-                            console.log(
-                                ` ⚠️ Primeiro não encontrado na lista! Buscando separadamente...`,
-                            );
-
-                            try {
+                        }
+                        else if (window.BUSCAR_PRIMEIRO_AGENDAMENTO)
+                        {
+
+                            console.log(` ⚠️ Primeiro não encontrado na lista! Buscando separadamente...`);
+
+                            try
+                            {
                                 const urlPrimeiro = `/api/Agenda/ObterAgendamento?viagemId=${idParaBuscar}`;
-                                console.log(
-                                    ` Chamando API corrigida: ${urlPrimeiro}`,
-                                );
+                                console.log(` Chamando API corrigida: ${urlPrimeiro}`);
 
                                 const controller = new AbortController();
-                                const timeoutId = setTimeout(
-                                    () => controller.abort(),
-                                    1000,
-                                );
-
-                                const responsePrimeiro = await fetch(
-                                    urlPrimeiro,
+                                const timeoutId = setTimeout(() => controller.abort(), 1000)
+
+                                const responsePrimeiro = await fetch(urlPrimeiro, {
+                                    signal: controller.signal
+                                });
+
+                                clearTimeout(timeoutId);
+
+                                if (responsePrimeiro.ok)
+                                {
+                                    const primeiro = await responsePrimeiro.json();
+                                    console.log(` ✅ Resposta recebida:`, primeiro);
+
+                                    if (primeiro && primeiro.dataInicial)
                                     {
-                                        signal: controller.signal,
-                                    },
-                                );
-
-                                clearTimeout(timeoutId);
-
-                                if (responsePrimeiro.ok) {
-                                    const primeiro =
-                                        await responsePrimeiro.json();
-                                    console.log(
-                                        ` ✅ Resposta recebida:`,
-                                        primeiro,
-                                    );
-
-                                    if (primeiro && primeiro.dataInicial) {
                                         todasDatas.push(primeiro.dataInicial);
-                                        console.log(
-                                            ` ✅ Data do PRIMEIRO agendamento adicionada: ${primeiro.dataInicial}`,
-                                        );
-                                    } else {
-                                        console.log(
-                                            ` ⚠️ Primeiro agendamento sem dataInicial`,
-                                        );
+                                        console.log(` ✅ Data do PRIMEIRO agendamento adicionada: ${primeiro.dataInicial}`);
                                     }
-                                } else {
-                                    console.error(
-                                        ` ❌ Erro na resposta: ${responsePrimeiro.status}`,
-                                    );
-                                    console.error(
-                                        ` Verifique se a API ObterAgendamento foi corrigida`,
-                                    );
-
-                                    if (responsePrimeiro.status === 500) {
-                                        console.warn(
-                                            ' ⚠️ Desabilitando busca do primeiro agendamento para evitar delays',
-                                        );
+                                    else
+                                    {
+                                        console.log(` ⚠️ Primeiro agendamento sem dataInicial`);
+                                    }
+                                }
+                                else
+                                {
+                                    console.error(` ❌ Erro na resposta: ${responsePrimeiro.status}`);
+                                    console.error(` Verifique se a API ObterAgendamento foi corrigida`);
+
+                                    if (responsePrimeiro.status === 500)
+                                    {
+                                        console.warn(" ⚠️ Desabilitando busca do primeiro agendamento para evitar delays");
                                         window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
                                     }
                                 }
-                            } catch (errPrimeiro) {
-                                if (errPrimeiro.name === 'AbortError') {
-                                    console.error(
-                                        ' ❌ Timeout ao buscar primeiro agendamento (5s)',
-                                    );
-                                    console.error(
-                                        ' API provavelmente não foi corrigida - desabilitando busca',
-                                    );
+                            }
+                            catch (errPrimeiro)
+                            {
+                                if (errPrimeiro.name === 'AbortError')
+                                {
+                                    console.error(" ❌ Timeout ao buscar primeiro agendamento (5s)");
+                                    console.error(" API provavelmente não foi corrigida - desabilitando busca");
                                     window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-                                } else {
-                                    console.error(
-                                        ' ❌ Erro ao buscar primeiro:',
-                                        errPrimeiro,
-                                    );
+                                }
+                                else
+                                {
+                                    console.error(" ❌ Erro ao buscar primeiro:", errPrimeiro);
                                 }
                             }
-                        } else {
-                            console.log(
-                                ` ℹ️ Busca do primeiro agendamento desabilitada`,
-                            );
-                            console.log(
-                                ` Para habilitar: window.BUSCAR_PRIMEIRO_AGENDAMENTO = true`,
-                            );
+                        }
+                        else
+                        {
+                            console.log(` ℹ️ Busca do primeiro agendamento desabilitada`);
+                            console.log(` Para habilitar: window.BUSCAR_PRIMEIRO_AGENDAMENTO = true`);
                         }
                     }
                 }
-            } catch (err) {
-                console.error('Erro ao buscar viagens:', err);
-                Alerta.TratamentoErroComLinha(
-                    'exibe-viagem.js',
-                    'configurarRecorrenciaVariada.busca',
-                    err,
-                );
-            }
-        } else {
-            console.log(' ⚠️ Sem ID para buscar viagens relacionadas');
+            }
+            catch (err)
+            {
+                console.error("Erro ao buscar viagens:", err);
+                Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRecorrenciaVariada.busca", err);
+            }
+        }
+        else
+        {
+            console.log(" ⚠️ Sem ID para buscar viagens relacionadas");
         }
 
         console.log(`📊 TOTAL DE DATAS COLETADAS: ${todasDatas.length}`);
         console.log(` Datas coletadas:`, todasDatas);
 
         let datasUnicas = [];
-        try {
+        try
+        {
             datasUnicas = [...new Set(todasDatas)]
-                .map((d) => new Date(d))
+                .map(d => new Date(d))
                 .sort((a, b) => a - b);
 
-            console.log(
-                `✅ Datas processadas e ordenadas: ${datasUnicas.length} datas únicas`,
-            );
-        } catch (errOrdenacao) {
-            console.error('❌ Erro ao ordenar datas:', errOrdenacao);
-            console.error(' Datas originais:', todasDatas);
+            console.log(`✅ Datas processadas e ordenadas: ${datasUnicas.length} datas únicas`);
+        } catch (errOrdenacao)
+        {
+            console.error("❌ Erro ao ordenar datas:", errOrdenacao);
+            console.error(" Datas originais:", todasDatas);
             return;
         }
 
         console.log(`Total: ${datasUnicas.length} datas`);
 
-        if (datasUnicas.length === 0) {
-            console.error('⚠️ Nenhuma data encontrada! Verificar API.');
-            console.error(' todasDatas original:', todasDatas);
-
-            const container = document.getElementById(
-                'listboxDatasVariadasContainer',
-            );
-            if (container) {
+        if (datasUnicas.length === 0)
+        {
+            console.error("⚠️ Nenhuma data encontrada! Verificar API.");
+            console.error(" todasDatas original:", todasDatas);
+
+            const container = document.getElementById('listboxDatasVariadasContainer');
+            if (container)
+            {
                 container.style.display = 'block';
                 const label = container.querySelector('label');
-                if (label) {
-                    label.innerHTML =
-                        '<i class="fas fa-exclamation-triangle"></i> Nenhuma data encontrada';
-                }
-            }
-
-            console.log('=== FIM configurarRecorrenciaVariada (sem datas) ===');
+                if (label)
+                {
+                    label.innerHTML = '<i class="fas fa-exclamation-triangle"></i> Nenhuma data encontrada';
+                }
+            }
+
+            console.log("=== FIM configurarRecorrenciaVariada (sem datas) ===");
             return;
         }
 
-        console.log('✅ Continuando com população da listbox...');
-
-        console.log('🎯 Iniciando preenchimento da listbox...');
-        const select = document.getElementById('lstDatasVariadas');
-        console.log(' Elemento select encontrado?', !!select);
-
-        if (!select) {
-            console.error('❌ lstDatasVariadas não encontrado no DOM!');
-
-            const container = document.getElementById(
-                'listboxDatasVariadasContainer',
-            );
-            if (container) {
-                console.log(' 🔧 Container existe, tentando criar select...');
+        console.log("✅ Continuando com população da listbox...");
+
+        console.log("🎯 Iniciando preenchimento da listbox...");
+        const select = document.getElementById("lstDatasVariadas");
+        console.log(" Elemento select encontrado?", !!select);
+
+        if (!select)
+        {
+            console.error("❌ lstDatasVariadas não encontrado no DOM!");
+
+            const container = document.getElementById('listboxDatasVariadasContainer');
+            if (container)
+            {
+                console.log(" 🔧 Container existe, tentando criar select...");
 
                 let selectExistente = container.querySelector('select');
-                if (!selectExistente) {
+                if (!selectExistente)
+                {
                     selectExistente = document.createElement('select');
                     selectExistente.id = 'lstDatasVariadas';
                     selectExistente.className = 'form-control';
                     selectExistente.multiple = true;
                     selectExistente.size = 5;
                     container.appendChild(selectExistente);
-                    console.log(' ✅ Select criado dinamicamente');
-                }
-
-                const selectNovo = document.getElementById('lstDatasVariadas');
-                if (selectNovo) {
-                    console.log(' ✅ Select encontrado após criação');
-
-                } else {
-                    console.error(
-                        ' ❌ Ainda não foi possível encontrar/criar select',
-                    );
+                    console.log(" ✅ Select criado dinamicamente");
+                }
+
+                const selectNovo = document.getElementById("lstDatasVariadas");
+                if (selectNovo)
+                {
+                    console.log(" ✅ Select encontrado após criação");
+
+                } else
+                {
+                    console.error(" ❌ Ainda não foi possível encontrar/criar select");
                     return;
                 }
-            } else {
-                console.error(' ❌ Container também não encontrado');
+            } else
+            {
+                console.error(" ❌ Container também não encontrado");
                 return;
             }
         }
 
-        const selectFinal = document.getElementById('lstDatasVariadas');
-        if (!selectFinal) {
-            console.error(
-                '❌ Problema crítico: não foi possível obter referência ao select',
-            );
+        const selectFinal = document.getElementById("lstDatasVariadas");
+        if (!selectFinal)
+        {
+            console.error("❌ Problema crítico: não foi possível obter referência ao select");
             return;
         }
 
@@ -2325,7 +2011,8 @@
         const dataAtualStr = objViagem.dataInicial.split('T')[0];
         console.log(`Data a destacar: ${dataAtualStr}`);
 
-        datasUnicas.forEach((data) => {
+        datasUnicas.forEach(data =>
+        {
             const dataIso = data.toISOString();
             const dataStr = dataIso.split('T')[0];
 
@@ -2333,18 +2020,21 @@
                 weekday: 'long',
                 day: '2-digit',
                 month: 'long',
-                year: 'numeric',
+                year: 'numeric'
             });
 
             const option = document.createElement('option');
             option.value = dataIso;
 
-            if (dataStr === dataAtualStr) {
+            if (dataStr === dataAtualStr)
+            {
                 option.text = `📅 ${texto}`;
                 option.style.color = '#dc3545';
                 option.style.fontWeight = 'bold';
                 option.className = 'data-agendamento-atual';
-            } else {
+            }
+            else
+            {
                 option.text = texto;
             }
 
@@ -2355,94 +2045,87 @@
         select.multiple = true;
         select.disabled = false;
 
-        for (let i = 0; i < select.options.length; i++) {
+        for (let i = 0; i < select.options.length; i++)
+        {
             select.options[i].selected = false;
         }
-        console.log('✅ Todos os itens desselecionados explicitamente');
+        console.log("✅ Todos os itens desselecionados explicitamente");
 
         const badge = document.getElementById('badgeContadorDatasVariadas');
-        if (badge) {
+        if (badge)
+        {
             badge.textContent = datasUnicas.length;
             badge.style.display = 'inline-block';
         }
 
-        if (container) {
+        if (container)
+        {
             container.style.setProperty('display', 'block', 'important');
             container.style.setProperty('visibility', 'visible', 'important');
-            console.log('✅ Container visível');
-        } else {
-            console.error(
-                '❌ Container listboxDatasVariadasContainer NÃO encontrado!',
-            );
-        }
-
-        if (select) {
+            console.log("✅ Container visível");
+        }
+        else
+        {
+            console.error("❌ Container listboxDatasVariadasContainer NÃO encontrado!");
+        }
+
+        if (select)
+        {
             select.style.setProperty('display', 'block', 'important');
             select.style.setProperty('visibility', 'visible', 'important');
             select.style.setProperty('opacity', '1', 'important');
-            console.log('✅ Listbox forçada visível novamente');
-        }
-
-        console.log('📊 DIAGNÓSTICO FINAL:');
-        console.log(' lstDatasVariadas existe?', !!select);
-        console.log(
-            ' lstDatasVariadas visível?',
-            select ? window.getComputedStyle(select).display : 'N/A',
-        );
-        console.log(' Total de options:', select ? select.options.length : 0);
-        console.log(' Container existe?', !!container);
-        console.log(
-            ' Container visível?',
-            container ? window.getComputedStyle(container).display : 'N/A',
-        );
-
-        console.log('✅ Listbox configurada!');
+            console.log("✅ Listbox forçada visível novamente");
+        }
+
+        console.log("📊 DIAGNÓSTICO FINAL:");
+        console.log(" lstDatasVariadas existe?", !!select);
+        console.log(" lstDatasVariadas visível?", select ? window.getComputedStyle(select).display : "N/A");
+        console.log(" Total de options:", select ? select.options.length : 0);
+        console.log(" Container existe?", !!container);
+        console.log(" Container visível?", container ? window.getComputedStyle(container).display : "N/A");
+
+        console.log("✅ Listbox configurada!");
         const totalTime = Date.now() - startTime;
-        console.log(
-            `⏱️ Tempo total de configurarRecorrenciaVariada: ${totalTime}ms`,
-        );
-        if (totalTime > 1000) {
-            console.warn(
-                `⚠️ DELAY DETECTADO: ${totalTime}ms (maior que 1 segundo)`,
-            );
-        }
-        console.log('=== FIM configurarRecorrenciaVariada ===');
-    } catch (error) {
-        console.error(
-            '❌ ERRO CRÍTICO em configurarRecorrenciaVariada:',
-            error,
-        );
-        console.error(' Stack trace:', error.stack);
-
-        const container = document.getElementById(
-            'listboxDatasVariadasContainer',
-        );
-        if (container) {
+        console.log(`⏱️ Tempo total de configurarRecorrenciaVariada: ${totalTime}ms`);
+        if (totalTime > 1000)
+        {
+            console.warn(`⚠️ DELAY DETECTADO: ${totalTime}ms (maior que 1 segundo)`);
+        }
+        console.log("=== FIM configurarRecorrenciaVariada ===");
+    }
+    catch (error)
+    {
+        console.error("❌ ERRO CRÍTICO em configurarRecorrenciaVariada:", error);
+        console.error(" Stack trace:", error.stack);
+
+        const container = document.getElementById('listboxDatasVariadasContainer');
+        if (container)
+        {
             container.style.display = 'block';
             const label = container.querySelector('label');
-            if (label) {
+            if (label)
+            {
                 label.textContent = `Erro ao carregar datas: ${error.message}`;
             }
         }
 
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'configurarRecorrenciaVariada',
-            error,
-        );
-    } finally {
-        console.log(
-            '=== configurarRecorrenciaVariada FINALIZADA (finally) ===',
-        );
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRecorrenciaVariada", error);
+    }
+    finally
+    {
+        console.log("=== configurarRecorrenciaVariada FINALIZADA (finally) ===");
     }
 }
 
-function limparListboxDatasVariadas() {
-    try {
-        console.log('🧹 Limpando ListBox de Datas Variadas e Calendário...');
-
-        const listbox = document.getElementById('lstDatasVariadas');
-        if (listbox) {
+function limparListboxDatasVariadas()
+{
+    try
+    {
+        console.log("🧹 Limpando ListBox de Datas Variadas e Calendário...");
+
+        const listbox = document.getElementById("lstDatasVariadas");
+        if (listbox)
+        {
             listbox.innerHTML = '';
 
             listbox.removeAttribute('style');
@@ -2450,547 +2133,506 @@
             listbox.style.display = 'none';
             listbox.size = 1;
             listbox.multiple = false;
-            console.log(
-                ' ✅ lstDatasVariadas limpa e oculta (estilos removidos)',
-            );
+            console.log(" ✅ lstDatasVariadas limpa e oculta (estilos removidos)");
         }
 
         const badge = document.getElementById('badgeContadorDatasVariadas');
-        if (badge) {
+        if (badge)
+        {
             badge.textContent = '0';
             badge.style.display = 'none';
-            console.log(' ✅ Badge da listbox limpo e oculto');
-        }
-
-        const container = document.getElementById(
-            'listboxDatasVariadasContainer',
-        );
-        if (container) {
+            console.log(" ✅ Badge da listbox limpo e oculto");
+        }
+
+        const container = document.getElementById('listboxDatasVariadasContainer');
+        if (container)
+        {
 
             container.removeAttribute('style');
 
             container.style.display = 'none';
-            console.log(
-                ' ✅ Container da listbox oculto (estilos removidos)',
-            );
+            console.log(" ✅ Container da listbox oculto (estilos removidos)");
         }
 
         const calendarContainer = document.getElementById('calendarContainer');
-        if (calendarContainer) {
+        if (calendarContainer)
+        {
             calendarContainer.style.display = 'none';
-            console.log(' ✅ Container do calendário oculto');
-        }
-
-        const calDatasSelecionadas = document.getElementById(
-            'calDatasSelecionadas',
-        );
-        if (calDatasSelecionadas) {
+            console.log(" ✅ Container do calendário oculto");
+        }
+
+        const calDatasSelecionadas = document.getElementById('calDatasSelecionadas');
+        if (calDatasSelecionadas)
+        {
             calDatasSelecionadas.style.display = 'none';
-            console.log(' ✅ Calendário oculto');
-        }
-
-        const badgeCalendar = document.getElementById(
-            'badgeContadorDatasSelecionadas',
-        );
-        if (badgeCalendar) {
+            console.log(" ✅ Calendário oculto");
+        }
+
+        const badgeCalendar = document.getElementById('badgeContadorDatasSelecionadas');
+        if (badgeCalendar)
+        {
             badgeCalendar.textContent = '0';
             badgeCalendar.style.display = 'none';
-            console.log(' ✅ Badge do calendário limpo e oculto');
-        }
-
-        const labelCalendar = document.querySelector(
-            'label[for="calDatasSelecionadas"]',
-        );
-        if (labelCalendar) {
+            console.log(" ✅ Badge do calendário limpo e oculto");
+        }
+
+        const labelCalendar = document.querySelector('label[for="calDatasSelecionadas"]');
+        if (labelCalendar)
+        {
             labelCalendar.style.display = 'none';
-            console.log(' ✅ Label do calendário oculta');
-        }
-
-        console.log('✅ Limpeza completa concluída');
-    } catch (error) {
-        console.error('❌ Erro ao limpar ListBox:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'limparListboxDatasVariadas',
-            error,
-        );
+            console.log(" ✅ Label do calendário oculta");
+        }
+
+        console.log("✅ Limpeza completa concluída");
+    }
+    catch (error)
+    {
+        console.error("❌ Erro ao limpar ListBox:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "limparListboxDatasVariadas", error);
     }
 }
 
 window.limparListboxDatasVariadas = limparListboxDatasVariadas;
 
-function mostrarCamposViagem(objViagem) {
-    try {
-
-        console.log(
-            '🔴 TESTE CLAUDE: divNoFichaVistoria NÃO deve aparecer! Versão 21/01/2026 15:30',
-        );
-        $('#divKmAtual, #divKmInicial, #divCombustivelInicial').show();
+function mostrarCamposViagem(objViagem)
+{
+    try
+    {
+
+        $("#divNoFichaVistoria, #divKmAtual, #divKmInicial, #divCombustivelInicial").show();
 
         const noFichaVal = objViagem.noFichaVistoria;
-        const txtNoFicha = $('#txtNoFichaVistoria');
-        if (noFichaVal === 0 || noFichaVal === '0' || !noFichaVal) {
-            txtNoFicha.val('');
-            txtNoFicha.attr('placeholder', '');
-            txtNoFicha.removeClass('placeholder-mobile');
-        } else {
+        const txtNoFicha = $("#txtNoFichaVistoria");
+        if (noFichaVal === 0 || noFichaVal === "0" || !noFichaVal)
+        {
+            txtNoFicha.val("");
+            txtNoFicha.attr("placeholder", "");
+            txtNoFicha.removeClass("placeholder-mobile");
+        }
+        else
+        {
             txtNoFicha.val(noFichaVal);
-            txtNoFicha.attr('placeholder', '');
-            txtNoFicha.removeClass('placeholder-mobile');
-        }
-
-        if (objViagem.kmAtual) {
-            $('#txtKmAtual').val(objViagem.kmAtual);
-        }
-
-        if (objViagem.kmInicial) {
-            $('#txtKmInicial').val(objViagem.kmInicial);
-        }
-
-        if (objViagem.combustivelInicial) {
-            const ddtCombIni = document.getElementById('ddtCombustivelInicial');
-            if (
-                ddtCombIni &&
-                ddtCombIni.ej2_instances &&
-                ddtCombIni.ej2_instances[0]
-            ) {
-                ddtCombIni.ej2_instances[0].value = [
-                    objViagem.combustivelInicial,
-                ];
+            txtNoFicha.attr("placeholder", "");
+            txtNoFicha.removeClass("placeholder-mobile");
+        }
+
+        if (objViagem.kmAtual)
+        {
+            $("#txtKmAtual").val(objViagem.kmAtual);
+        }
+
+        if (objViagem.kmInicial)
+        {
+            $("#txtKmInicial").val(objViagem.kmInicial);
+        }
+
+        if (objViagem.combustivelInicial)
+        {
+            const ddtCombIni = document.getElementById("ddtCombustivelInicial");
+            if (ddtCombIni && ddtCombIni.ej2_instances && ddtCombIni.ej2_instances[0])
+            {
+                ddtCombIni.ej2_instances[0].value = [objViagem.combustivelInicial];
                 ddtCombIni.ej2_instances[0].dataBind();
             }
         }
 
-        if (
-            objViagem.status === 'Aberta' ||
-            objViagem.status === 'Agendada' ||
-            objViagem.status === 'Realizada'
-        ) {
-            $(
-                '#divDataFinal, #divHoraFinal, #divKmFinal, #divCombustivelFinal, #divQuilometragem, #divDuracao',
-            ).show();
-
-            if (objViagem.kmFinal) {
-                $('#txtKmFinal').val(objViagem.kmFinal);
-            }
-
-            if (objViagem.combustivelFinal) {
-                const ddtCombFim = document.getElementById(
-                    'ddtCombustivelFinal',
-                );
-                if (
-                    ddtCombFim &&
-                    ddtCombFim.ej2_instances &&
-                    ddtCombFim.ej2_instances[0]
-                ) {
-                    ddtCombFim.ej2_instances[0].value = [
-                        objViagem.combustivelFinal,
-                    ];
+        if (objViagem.status === "Aberta" || objViagem.status === "Agendada" || objViagem.status === "Realizada")
+        {
+            $("#divDataFinal, #divHoraFinal, #divKmFinal, #divCombustivelFinal, #divQuilometragem, #divDuracao").show();
+
+            if (objViagem.kmFinal)
+            {
+                $("#txtKmFinal").val(objViagem.kmFinal);
+            }
+
+            if (objViagem.combustivelFinal)
+            {
+                const ddtCombFim = document.getElementById("ddtCombustivelFinal");
+                if (ddtCombFim && ddtCombFim.ej2_instances && ddtCombFim.ej2_instances[0])
+                {
+                    ddtCombFim.ej2_instances[0].value = [objViagem.combustivelFinal];
                     ddtCombFim.ej2_instances[0].dataBind();
                 }
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'mostrarCamposViagem',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "mostrarCamposViagem", error);
     }
 }
 
-function configurarBotoesPorStatus(objViagem) {
-    try {
+function configurarBotoesPorStatus(objViagem)
+{
+    try
+    {
         const status = objViagem.status;
         const statusAgendamento = objViagem.statusAgendamento;
 
-        if (status === 'Cancelada') {
-
-            $('#btnConfirma').hide();
-            $('#btnApaga').hide();
-            $('#btnViagem').hide();
-            $('#btnImprime').show();
+        if (status === "Cancelada")
+        {
+
+            $("#btnConfirma").hide();
+            $("#btnApaga").hide();
+            $("#btnViagem").hide();
+            $("#btnImprime").show();
             window.desabilitarTodosControles();
             window.CarregandoViagemBloqueada = true;
-        } else if (status === 'Realizada') {
-
-            $('#btnConfirma').hide();
-            $('#btnApaga').hide();
-            $('#btnViagem').hide();
-            $('#btnImprime').show();
+        } else if (status === "Realizada")
+        {
+
+            $("#btnConfirma").hide();
+            $("#btnApaga").hide();
+            $("#btnViagem").hide();
+            $("#btnImprime").show();
             window.desabilitarTodosControles();
             window.CarregandoViagemBloqueada = true;
-        } else if (status === 'Aberta') {
-
-            $('#btnConfirma')
-                .html(
-                    "<i class='fa-duotone fa-floppy-disk icon-space'></i>Editar",
-                )
-                .show();
-            $('#btnApaga').hide();
-            $('#btnViagem').hide();
-            $('#btnImprime').show();
-        } else if (statusAgendamento === true) {
-
-            $('#btnConfirma')
-                .html(
-                    "<i class='fa-duotone fa-floppy-disk icon-space'></i>Edita Agendamento",
-                )
-                .show();
-            $('#btnApaga').show();
-            $('#btnViagem').show();
-            $('#btnImprime').show();
-        } else {
-
-            $('#btnConfirma')
-                .html(
-                    "<i class='fa-duotone fa-floppy-disk icon-space'></i>Salvar",
-                )
-                .show();
-            $('#btnApaga').show();
-            $('#btnViagem').hide();
-            $('#btnImprime').show();
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'configurarBotoesPorStatus',
-            error,
-        );
+        } else if (status === "Aberta")
+        {
+
+            $("#btnConfirma").html("<i class='fa-duotone fa-floppy-disk icon-space'></i>Editar").show();
+            $("#btnApaga").hide();
+            $("#btnViagem").hide();
+            $("#btnImprime").show();
+        } else if (statusAgendamento === true)
+        {
+
+            $("#btnConfirma").html("<i class='fa-duotone fa-floppy-disk icon-space'></i>Edita Agendamento").show();
+            $("#btnApaga").show();
+            $("#btnViagem").show();
+            $("#btnImprime").show();
+        } else
+        {
+
+            $("#btnConfirma").html("<i class='fa-duotone fa-floppy-disk icon-space'></i>Salvar").show();
+            $("#btnApaga").show();
+            $("#btnViagem").hide();
+            $("#btnImprime").show();
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarBotoesPorStatus", error);
     }
 }
 
-function configurarRecorrencia(objViagem) {
-    try {
-        if (window.RecorrenciaController?.aplicarEdicao) {
-            window.RecorrenciaController.aplicarEdicao(objViagem);
-            return;
-        }
-
-        console.log('ðŸ”„ Configurando recorrência:', objViagem);
-
-        const lstRecorrenteKendo =
-            $('#lstRecorrente').data('kendoDropDownList');
-        if (lstRecorrenteKendo) {
-            lstRecorrenteKendo.value('S');
-            lstRecorrenteKendo.enable(false);
-        }
-
-        const divPeriodo = document.getElementById('divPeriodo');
-        if (divPeriodo) {
+function configurarRecorrencia(objViagem)
+{
+    try
+    {
+        console.log("ðŸ”„ Configurando recorrência:", objViagem);
+
+        const lstRecorrente = document.getElementById("lstRecorrente");
+        if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])
+        {
+            lstRecorrente.ej2_instances[0].value = "S";
+            lstRecorrente.ej2_instances[0].enabled = false;
+            lstRecorrente.ej2_instances[0].dataBind();
+        }
+
+        const divPeriodo = document.getElementById("divPeriodo");
+        if (divPeriodo)
+        {
             divPeriodo.style.setProperty('display', 'block', 'important');
         }
 
-        const lstPeriodosKendo = $('#lstPeriodos').data('kendoDropDownList');
-        if (lstPeriodosKendo) {
-            lstPeriodosKendo.enable(false);
-
-            if (objViagem.Intervalo) {
-                lstPeriodosKendo.value(objViagem.Intervalo);
-
-                console.log('ðŸ“‹ Tipo de intervalo:', objViagem.Intervalo);
-
-                switch (objViagem.Intervalo) {
-                    case 'D':
+        const lstPeriodos = document.getElementById("lstPeriodos");
+        if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0])
+        {
+            lstPeriodos.ej2_instances[0].enabled = false;
+
+            if (objViagem.intervalo)
+            {
+                lstPeriodos.ej2_instances[0].value = objViagem.intervalo;
+                lstPeriodos.ej2_instances[0].dataBind();
+
+                console.log("ðŸ“‹ Tipo de intervalo:", objViagem.intervalo);
+
+                switch (objViagem.intervalo)
+                {
+                    case "D":
                         mostrarCamposRecorrenciaDiaria(objViagem);
                         break;
 
-                    case 'S':
-                    case 'Q':
+                    case "S":
+                    case "Q":
                         mostrarCamposRecorrenciaSemanal(objViagem);
                         break;
 
-                    case 'M':
+                    case "M":
                         mostrarCamposRecorrenciaMensal(objViagem);
                         break;
 
-                    case 'V':
+                    case "V":
                         mostrarCamposRecorrenciaVariada(objViagem);
                         break;
 
                     default:
-                        console.warn(
-                            'âš ï¸ Tipo de recorrência não reconhecido:',
-                            objViagem.Intervalo,
-                        );
-                }
-            }
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'configurarRecorrencia',
-            error,
-        );
+                        console.warn("âš ï¸ Tipo de recorrência não reconhecido:", objViagem.intervalo);
+                }
+            }
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRecorrencia", error);
     }
 }
 
-function mostrarCamposRecorrenciaDiaria(objViagem) {
-    try {
-        console.log('ðŸ“… Configurando recorrência diária');
-
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Mostrar diária',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'mostrarCamposRecorrenciaDiaria',
-            error,
-        );
+function mostrarCamposRecorrenciaDiaria(objViagem)
+{
+    try
+    {
+        console.log("ðŸ“… Configurando recorrência diária");
+
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            setTimeout(() => {
+                const dataObj = new Date(objViagem.dataFinalRecorrencia);
+                window.setKendoDateValue("txtFinalRecorrencia", dataObj, true);
+                window.enableKendoDatePicker("txtFinalRecorrencia", false);
+            }, 500);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "mostrarCamposRecorrenciaDiaria", error);
     }
 }
 
-function mostrarCamposRecorrenciaSemanal(objViagem) {
-    try {
-        console.log('ðŸ“… Configurando recorrência semanal/quinzenal');
-
-        const divDias = document.getElementById('divDias');
-        if (divDias) {
+function mostrarCamposRecorrenciaSemanal(objViagem)
+{
+    try
+    {
+        console.log("ðŸ“… Configurando recorrência semanal/quinzenal");
+
+        const divDias = document.getElementById("divDias");
+        if (divDias)
+        {
             divDias.style.setProperty('display', 'block', 'important');
         }
 
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        const lstDiasKendo = $('#lstDias').data('kendoMultiSelect');
-        if (lstDiasKendo) {
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        const lstDias = document.getElementById("lstDias");
+        if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0])
+        {
             const diasSelecionados = [];
 
-            if (objViagem.Sunday) diasSelecionados.push(0);
-            if (objViagem.Monday) diasSelecionados.push(1);
-            if (objViagem.Tuesday) diasSelecionados.push(2);
-            if (objViagem.Wednesday) diasSelecionados.push(3);
-            if (objViagem.Thursday) diasSelecionados.push(4);
-            if (objViagem.Friday) diasSelecionados.push(5);
-            if (objViagem.Saturday) diasSelecionados.push(6);
-
-            lstDiasKendo.value(diasSelecionados);
-            lstDiasKendo.enable(false);
-
-            console.log('✅ Dias selecionados:', diasSelecionados);
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Mostrar semanal/quinzenal',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'mostrarCamposRecorrenciaSemanal',
-            error,
-        );
+            if (objViagem.monday) diasSelecionados.push("Segunda");
+            if (objViagem.tuesday) diasSelecionados.push("Terça");
+            if (objViagem.wednesday) diasSelecionados.push("Quarta");
+            if (objViagem.thursday) diasSelecionados.push("Quinta");
+            if (objViagem.friday) diasSelecionados.push("Sexta");
+            if (objViagem.saturday) diasSelecionados.push("Sábado");
+            if (objViagem.sunday) diasSelecionados.push("Domingo");
+
+            lstDias.ej2_instances[0].value = diasSelecionados;
+            lstDias.ej2_instances[0].enabled = false;
+            lstDias.ej2_instances[0].dataBind();
+
+            console.log("âœ… Dias selecionados:", diasSelecionados);
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            setTimeout(() => {
+                const dataObj = new Date(objViagem.dataFinalRecorrencia);
+                window.setKendoDateValue("txtFinalRecorrencia", dataObj, true);
+                window.enableKendoDatePicker("txtFinalRecorrencia", false);
+            }, 500);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "mostrarCamposRecorrenciaSemanal", error);
     }
 }
 
-function mostrarCamposRecorrenciaMensal(objViagem) {
-    try {
-        console.log('ðŸ“… Configurando recorrência mensal');
-
-        const divDiaMes = document.getElementById('divDiaMes');
-        if (divDiaMes) {
+function mostrarCamposRecorrenciaMensal(objViagem)
+{
+    try
+    {
+        console.log("ðŸ“… Configurando recorrência mensal");
+
+        const divDiaMes = document.getElementById("divDiaMes");
+        if (divDiaMes)
+        {
             divDiaMes.style.setProperty('display', 'block', 'important');
         }
 
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        if (objViagem.DiaMesRecorrencia) {
-            const lstDiasMesKendo = $('#lstDiasMes').data('kendoDropDownList');
-            if (lstDiasMesKendo) {
-                lstDiasMesKendo.value(objViagem.DiaMesRecorrencia);
-                lstDiasMesKendo.enable(false);
-
-                console.log('✅ Dia do mês:', objViagem.DiaMesRecorrencia);
-            }
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Mostrar mensal',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'mostrarCamposRecorrenciaMensal',
-            error,
-        );
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        if (objViagem.diaMesRecorrencia)
+        {
+            const lstDiasMes = document.getElementById("lstDiasMes");
+            if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0])
+            {
+                lstDiasMes.ej2_instances[0].value = objViagem.diaMesRecorrencia;
+                lstDiasMes.ej2_instances[0].enabled = false;
+                lstDiasMes.ej2_instances[0].dataBind();
+
+                console.log("âœ… Dia do mês:", objViagem.diaMesRecorrencia);
+            }
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            setTimeout(() => {
+                const dataObj = new Date(objViagem.dataFinalRecorrencia);
+                window.setKendoDateValue("txtFinalRecorrencia", dataObj, true);
+                window.enableKendoDatePicker("txtFinalRecorrencia", false);
+            }, 500);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "mostrarCamposRecorrenciaMensal", error);
     }
 }
 
-async function mostrarCamposRecorrenciaVariada(objViagem) {
-    try {
-        console.log('📅 [mostrarCamposRecorrenciaVariada] Iniciando...');
-        console.log(' objViagem:', objViagem);
-
-        const calendarContainer = document.getElementById('calendarContainer');
-        if (calendarContainer) {
+async function mostrarCamposRecorrenciaVariada(objViagem)
+{
+    try
+    {
+        console.log("📅 [mostrarCamposRecorrenciaVariada] Iniciando...");
+        console.log(" objViagem:", objViagem);
+
+        const calendarContainer = document.getElementById("calendarContainer");
+        if (calendarContainer)
+        {
             calendarContainer.style.display = 'none';
-            console.log('✅ calendarContainer escondido');
-        } else {
-            console.log('⚠️ calendarContainer não encontrado');
-        }
-
-        const listboxContainer = document.getElementById(
-            'listboxDatasVariadasContainer',
-        );
-        console.log('🔍 Procurando listboxDatasVariadasContainer...');
-
-        if (listboxContainer) {
-            console.log('✅ listboxDatasVariadasContainer encontrado!');
-            console.log(' Display antes:', listboxContainer.style.display);
+            console.log("✅ calendarContainer escondido");
+        }
+        else
+        {
+            console.log("⚠️ calendarContainer não encontrado");
+        }
+
+        const listboxContainer = document.getElementById("listboxDatasVariadasContainer");
+        console.log("🔍 Procurando listboxDatasVariadasContainer...");
+
+        if (listboxContainer)
+        {
+            console.log("✅ listboxDatasVariadasContainer encontrado!");
+            console.log(" Display antes:", listboxContainer.style.display);
 
             listboxContainer.style.display = 'block';
             listboxContainer.style.setProperty('display', 'block', 'important');
 
-            console.log(' Display depois:', listboxContainer.style.display);
-            console.log(
-                ' InnerHTML:',
-                listboxContainer.innerHTML.substring(0, 200),
-            );
-        } else {
-            console.error('❌ listboxDatasVariadasContainer não encontrado!');
-        }
-
-        console.log('🔧 Chamando configurarRecorrenciaVariada...');
+            console.log(" Display depois:", listboxContainer.style.display);
+            console.log(" InnerHTML:", listboxContainer.innerHTML.substring(0, 200));
+        }
+        else
+        {
+            console.error("❌ listboxDatasVariadasContainer não encontrado!");
+        }
+
+        console.log("🔧 Chamando configurarRecorrenciaVariada...");
         await configurarRecorrenciaVariada(objViagem);
-        console.log('✅ configurarRecorrenciaVariada concluída');
-    } catch (error) {
-        console.error('❌ Erro em mostrarCamposRecorrenciaVariada:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'mostrarCamposRecorrenciaVariada',
-            error,
-        );
+        console.log("✅ configurarRecorrenciaVariada concluída");
+    } catch (error)
+    {
+        console.error("❌ Erro em mostrarCamposRecorrenciaVariada:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "mostrarCamposRecorrenciaVariada", error);
     }
 }
 
-function definirTituloModal(objViagem) {
-    try {
+function definirTituloModal(objViagem)
+{
+    try
+    {
         let tipoModal = 'NOVO_AGENDAMENTO';
 
-        if (objViagem && (objViagem.viagemId || objViagem.ViagemId)) {
+        if (objViagem && (objViagem.viagemId || objViagem.ViagemId))
+        {
 
             const status = objViagem.status ?? objViagem.Status;
 
-            const statusAgendamentoRaw =
-                objViagem.statusAgendamento ?? objViagem.StatusAgendamento;
-            const statusAgendamento =
-                statusAgendamentoRaw === true ||
-                statusAgendamentoRaw === 1 ||
-                statusAgendamentoRaw === '1' ||
-                statusAgendamentoRaw === 'true' ||
-                statusAgendamentoRaw === 'True';
-
-            const foiAgendamentoRaw =
-                objViagem.foiAgendamento ?? objViagem.FoiAgendamento;
-            const foiAgendamento =
-                foiAgendamentoRaw === true ||
-                foiAgendamentoRaw === 1 ||
-                foiAgendamentoRaw === '1' ||
-                foiAgendamentoRaw === 'true' ||
-                foiAgendamentoRaw === 'True';
-
-            const finalidadeNome = (
-                objViagem.finalidade ??
-                objViagem.Finalidade ??
-                ''
-            )
-                .toString()
-                .toLowerCase()
-                .trim();
-            const isEvento =
-                finalidadeNome === 'evento' || finalidadeNome === 'eventos';
-
-            console.log('🔍 DEBUG definirTituloModal:');
-            console.log(
-                ' objViagem COMPLETO:',
-                JSON.stringify(objViagem, null, 2),
-            );
-            console.log(' status:', status);
-            console.log(
-                ' statusAgendamentoRaw:',
-                statusAgendamentoRaw,
-                'tipo:',
-                typeof statusAgendamentoRaw,
-            );
-            console.log(' statusAgendamento (calculado):', statusAgendamento);
-            console.log(
-                ' foiAgendamentoRaw:',
-                foiAgendamentoRaw,
-                'tipo:',
-                typeof foiAgendamentoRaw,
-            );
-            console.log(' foiAgendamento (calculado):', foiAgendamento);
-            console.log(' finalidadeNome:', finalidadeNome);
-            console.log(' isEvento:', isEvento);
-
-            if (isEvento) {
-                if (status === 'Cancelada') {
+            const statusAgendamentoRaw = objViagem.statusAgendamento ?? objViagem.StatusAgendamento;
+            const statusAgendamento = statusAgendamentoRaw === true ||
+                                      statusAgendamentoRaw === 1 ||
+                                      statusAgendamentoRaw === "1" ||
+                                      statusAgendamentoRaw === "true" ||
+                                      statusAgendamentoRaw === "True";
+
+            const foiAgendamentoRaw = objViagem.foiAgendamento ?? objViagem.FoiAgendamento;
+            const foiAgendamento = foiAgendamentoRaw === true ||
+                                   foiAgendamentoRaw === 1 ||
+                                   foiAgendamentoRaw === "1" ||
+                                   foiAgendamentoRaw === "true" ||
+                                   foiAgendamentoRaw === "True";
+
+            const finalidadeNome = (objViagem.finalidade ?? objViagem.Finalidade ?? '').toString().toLowerCase().trim();
+            const isEvento = finalidadeNome === 'evento' || finalidadeNome === 'eventos';
+
+            console.log("🔍 DEBUG definirTituloModal:");
+            console.log(" objViagem COMPLETO:", JSON.stringify(objViagem, null, 2));
+            console.log(" status:", status);
+            console.log(" statusAgendamentoRaw:", statusAgendamentoRaw, "tipo:", typeof statusAgendamentoRaw);
+            console.log(" statusAgendamento (calculado):", statusAgendamento);
+            console.log(" foiAgendamentoRaw:", foiAgendamentoRaw, "tipo:", typeof foiAgendamentoRaw);
+            console.log(" foiAgendamento (calculado):", foiAgendamento);
+            console.log(" finalidadeNome:", finalidadeNome);
+            console.log(" isEvento:", isEvento);
+
+            if (isEvento)
+            {
+                if (status === "Cancelada")
+                {
                     tipoModal = 'EVENTO_CANCELADO';
-                } else if (status === 'Realizada') {
+                }
+                else if (status === "Realizada")
+                {
                     tipoModal = 'EVENTO_REALIZADO';
-                } else if (status === 'Aberta' && !statusAgendamento) {
+                }
+                else if (status === "Aberta" && !statusAgendamento)
+                {
                     tipoModal = 'EVENTO_ABERTO';
-                } else
+                }
+                else
                 {
                     tipoModal = 'EVENTO_AGENDADO';
                 }
             }
 
-            else if (status === 'Cancelada') {
+            else if (status === "Cancelada")
+            {
                 tipoModal = 'VIAGEM_CANCELADA';
             }
 
-            else if (status === 'Realizada') {
+            else if (status === "Realizada")
+            {
                 tipoModal = 'VIAGEM_REALIZADA';
 
                 window._foiAgendamentoAtual = foiAgendamento;
             }
 
-            else if (status === 'Aberta' && !statusAgendamento) {
+            else if (status === "Aberta" && !statusAgendamento)
+            {
                 tipoModal = 'VIAGEM_ABERTA';
                 window._foiAgendamentoAtual = false;
             }
 
-            else if (
-                status === 'Agendada' ||
-                (status === 'Aberta' && statusAgendamento)
-            ) {
+            else if (status === "Agendada" || (status === "Aberta" && statusAgendamento))
+            {
                 tipoModal = 'VIAGEM_AGENDADA';
                 window._foiAgendamentoAtual = false;
             }
 
-            else {
+            else
+            {
                 tipoModal = 'EDITAR_AGENDAMENTO';
                 window._foiAgendamentoAtual = false;
             }
@@ -2998,207 +2640,100 @@
 
         console.log(`✅ Tipo de modal determinado: ${tipoModal}`);
         window.setModalTitle(tipoModal);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'definirTituloModal',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "definirTituloModal", error);
     }
 }
 
-function restaurarDadosRecorrencia(objViagem) {
-    try {
-        if (window.RecorrenciaController?.aplicarEdicao) {
-            window.RecorrenciaController.aplicarEdicao(objViagem);
+function restaurarDadosRecorrencia(objViagem)
+{
+    try
+    {
+        console.log("ðŸ”„ Restaurando dados de recorrência:", objViagem);
+
+        if (!objViagem)
+        {
+            console.warn("âš ï¸ Nenhum objeto de viagem fornecido");
             return;
         }
 
-        console.log('ðŸ”„ Restaurando dados de recorrência:', objViagem);
-
-        if (!objViagem) {
-            console.warn('âš ï¸ Nenhum objeto de viagem fornecido');
-            return;
-        }
-
-        const intervaloValor =
-            objViagem.Intervalo ??
-            objViagem.intervalo ??
-            objViagem.periodo ??
-            objViagem.Periodo;
-        const dadosRecorrencia = {
-            ...objViagem,
-            Recorrente:
-                objViagem.Recorrente ??
-                objViagem.recorrente ??
-                objViagem.ehRecorrente ??
-                objViagem.EhRecorrente,
-            Intervalo: intervaloValor
-                ? intervaloValor.toString().trim().toUpperCase()
-                : intervaloValor,
-            DataFinalRecorrencia:
-                objViagem.DataFinalRecorrencia ??
-                objViagem.dataFinalRecorrencia,
-            DiaMesRecorrencia:
-                objViagem.DiaMesRecorrencia ??
-                objViagem.diaMesRecorrencia ??
-                objViagem.diaDoMes,
-            diaDoMes:
-                objViagem.diaDoMes ??
-                objViagem.DiaMesRecorrencia ??
-                objViagem.diaMesRecorrencia,
-            Sunday: objViagem.Sunday ?? objViagem.sunday,
-            Monday: objViagem.Monday ?? objViagem.monday,
-            Tuesday: objViagem.Tuesday ?? objViagem.tuesday,
-            Wednesday: objViagem.Wednesday ?? objViagem.wednesday,
-            Thursday: objViagem.Thursday ?? objViagem.thursday,
-            Friday: objViagem.Friday ?? objViagem.friday,
-            Saturday: objViagem.Saturday ?? objViagem.saturday,
-        };
-
-        objViagem = dadosRecorrencia;
-        const recorrenteValor = objViagem.Recorrente;
-        const recorrenteTexto = (recorrenteValor ?? '')
-            .toString()
-            .trim()
-            .toUpperCase();
-        const ehRecorrente =
-            recorrenteValor === true ||
-            recorrenteValor === 1 ||
-            recorrenteValor === '1' ||
-            recorrenteTexto === 'S' ||
-            recorrenteTexto === 'SIM' ||
-            recorrenteTexto === 'TRUE';
-
-        if (ehRecorrente) {
-            console.log('âœ… Ã‰ recorrente, restaurando configurações...');
-
-            const lstRecorrenteKendo =
-                $('#lstRecorrente').data('kendoDropDownList');
-            if (lstRecorrenteKendo) {
-                lstRecorrenteKendo.value('S');
-                lstRecorrenteKendo.enable(false);
-                console.log('✅ lstRecorrente = Sim (Kendo)');
-            }
-
-            const divPeriodo = document.getElementById('divPeriodo');
-            if (divPeriodo) {
+        if (objViagem.recorrente === 'S' || objViagem.recorrente === true)
+        {
+            console.log("âœ… Ã‰ recorrente, restaurando configurações...");
+
+            const lstRecorrente = document.getElementById("lstRecorrente");
+            if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])
+            {
+                lstRecorrente.ej2_instances[0].value = 'S';
+                lstRecorrente.ej2_instances[0].enabled = false;
+                lstRecorrente.ej2_instances[0].dataBind();
+                console.log("âœ… lstRecorrente = Sim");
+            }
+
+            const divPeriodo = document.getElementById("divPeriodo");
+            if (divPeriodo)
+            {
                 divPeriodo.style.setProperty('display', 'block', 'important');
             }
 
-            if (objViagem.Intervalo) {
-                const lstPeriodosKendo =
-                    $('#lstPeriodos').data('kendoDropDownList');
-                if (lstPeriodosKendo) {
-                    lstPeriodosKendo.value(objViagem.Intervalo);
-                    lstPeriodosKendo.enable(false);
-                    console.log(
-                        '✅ Período = ' + objViagem.Intervalo + ' (Kendo)',
-                    );
-                }
-
-                if (objViagem.Intervalo === 'D') {
-                    const divFinalRecorrencia = document.getElementById(
-                        'divFinalRecorrencia',
-                    );
-                    if (divFinalRecorrencia) {
-                        divFinalRecorrencia.style.setProperty(
-                            'display',
-                            'block',
-                            'important',
-                        );
-                    }
-
-                    preencherDataFinalRecorrencia(
-                        objViagem.DataFinalRecorrencia,
+            if (objViagem.intervalo)
+            {
+                const lstPeriodos = document.getElementById("lstPeriodos");
+                if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0])
+                {
+                    lstPeriodos.ej2_instances[0].value = objViagem.intervalo;
+                    lstPeriodos.ej2_instances[0].enabled = false;
+                    lstPeriodos.ej2_instances[0].dataBind();
+                    console.log("âœ… Perí­odo = " + objViagem.intervalo);
+                }
+
+                if (objViagem.intervalo === 'S' || objViagem.intervalo === 'Q')
+                {
+                    setTimeout(() =>
+                    {
+                        console.log("ðŸ“… [FINAL] Preenchendo lstDias...");
+
+                        const lstDias = document.getElementById("lstDias");
+                        if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0])
                         {
-                            disable: true,
-                            contexto: 'Diaria (restaurar)',
-                        },
-                    );
-                }
-
-                if (objViagem.Intervalo === 'V') {
-                    restaurarRecorrenciaVariada(objViagem);
-                }
-
-                if (
-                    objViagem.Intervalo === 'S' ||
-                    objViagem.Intervalo === 'Q'
-                ) {
-                    const divDias = document.getElementById('divDias');
-                    if (divDias) {
-                        divDias.style.setProperty(
-                            'display',
-                            'block',
-                            'important',
-                        );
-                    }
-
-                    const divFinalRecorrencia = document.getElementById(
-                        'divFinalRecorrencia',
-                    );
-                    if (divFinalRecorrencia) {
-                        divFinalRecorrencia.style.setProperty(
-                            'display',
-                            'block',
-                            'important',
-                        );
-                    }
-
-                    setTimeout(() => {
-                        console.log('📅 [FINAL] Preenchendo lstDias...');
-
-                        const lstDiasKendo =
-                            $('#lstDias').data('kendoMultiSelect');
-                        if (lstDiasKendo) {
                             const diasSelecionados = [];
 
-                            if (objViagem.Sunday || objViagem.sunday)
-                                diasSelecionados.push(0);
-                            if (objViagem.Monday || objViagem.monday)
-                                diasSelecionados.push(1);
-                            if (objViagem.Tuesday || objViagem.tuesday)
-                                diasSelecionados.push(2);
-                            if (objViagem.Wednesday || objViagem.wednesday)
-                                diasSelecionados.push(3);
-                            if (objViagem.Thursday || objViagem.thursday)
-                                diasSelecionados.push(4);
-                            if (objViagem.Friday || objViagem.friday)
-                                diasSelecionados.push(5);
-                            if (objViagem.Saturday || objViagem.saturday)
-                                diasSelecionados.push(6);
-
-                            if (diasSelecionados.length > 0) {
-                                lstDiasKendo.value(diasSelecionados);
-                                lstDiasKendo.enable(false);
-                                console.log(
-                                    '✅ [FINAL] lstDias (Kendo) preenchido:',
-                                    diasSelecionados,
-                                );
+                            if (objViagem.Monday || objViagem.monday) diasSelecionados.push("Segunda");
+                            if (objViagem.Tuesday || objViagem.tuesday) diasSelecionados.push("Terça");
+                            if (objViagem.Wednesday || objViagem.wednesday) diasSelecionados.push("Quarta");
+                            if (objViagem.Thursday || objViagem.thursday) diasSelecionados.push("Quinta");
+                            if (objViagem.Friday || objViagem.friday) diasSelecionados.push("Sexta");
+                            if (objViagem.Saturday || objViagem.saturday) diasSelecionados.push("Sábado");
+                            if (objViagem.Sunday || objViagem.sunday) diasSelecionados.push("Domingo");
+
+                            if (diasSelecionados.length > 0)
+                            {
+                                lstDias.ej2_instances[0].value = diasSelecionados;
+                                lstDias.ej2_instances[0].dataBind();
+                                console.log("âœ… [FINAL] lstDias preenchido:", diasSelecionados);
                             }
                         }
 
-                        setTimeout(() => {
-                            console.log(
-                                '📅 [RENDER] Verificando renderização Kendo...',
-                            );
-
-                            const lstDiasKendo =
-                                $('#lstDias').data('kendoMultiSelect');
-                            if (lstDiasKendo) {
-                                const dias = lstDiasKendo.value();
-
-                                console.log(
-                                    'ðŸ“‹ [RENDER] Dias encontrados:',
-                                    dias,
-                                );
-
-                                if (dias && dias.length > 0) {
-                                    const wrapper =
-                                        lstDias.closest('.e-input-group');
-
-                                    if (wrapper) {
+                        setTimeout(() =>
+                        {
+                            console.log("ðŸ“… [RENDER] Renderizando chips visualmente...");
+
+                            const lstDias = document.getElementById("lstDias");
+
+                            if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0])
+                            {
+                                const instance = lstDias.ej2_instances[0];
+                                const dias = instance.value;
+
+                                console.log("ðŸ“‹ [RENDER] Dias encontrados:", dias);
+
+                                if (dias && dias.length > 0)
+                                {
+                                    const wrapper = lstDias.closest('.e-input-group');
+
+                                    if (wrapper)
+                                    {
 
                                         wrapper.style.cssText = `
                                             width: 100% !important;
@@ -3213,20 +2748,13 @@
                                             box-sizing: border-box !important;
                                         `;
 
-                                        let chipsContainer =
-                                            wrapper.querySelector(
-                                                '.e-chips-collection',
-                                            );
-
-                                        if (!chipsContainer) {
-                                            chipsContainer =
-                                                document.createElement('span');
-                                            chipsContainer.className =
-                                                'e-chips-collection';
-                                            wrapper.insertBefore(
-                                                chipsContainer,
-                                                wrapper.firstChild,
-                                            );
+                                        let chipsContainer = wrapper.querySelector('.e-chips-collection');
+
+                                        if (!chipsContainer)
+                                        {
+                                            chipsContainer = document.createElement('span');
+                                            chipsContainer.className = 'e-chips-collection';
+                                            wrapper.insertBefore(chipsContainer, wrapper.firstChild);
                                         }
 
                                         chipsContainer.style.cssText = `
@@ -3242,9 +2770,9 @@
 
                                         chipsContainer.innerHTML = '';
 
-                                        dias.forEach((dia) => {
-                                            const chip =
-                                                document.createElement('span');
+                                        dias.forEach(dia =>
+                                        {
+                                            const chip = document.createElement('span');
                                             chip.className = 'e-chips';
 
                                             chip.style.cssText = `
@@ -3262,494 +2790,476 @@
                                                 box-sizing: border-box !important;
                                             `;
 
-                                            const chipContent =
-                                                document.createElement('span');
-                                            chipContent.className =
-                                                'e-chipcontent';
+                                            const chipContent = document.createElement('span');
+                                            chipContent.className = 'e-chipcontent';
                                             chipContent.textContent = dia;
 
                                             chip.appendChild(chipContent);
                                             chipsContainer.appendChild(chip);
                                         });
 
-                                        Array.from(wrapper.children).forEach(
-                                            (child) => {
-                                                if (
-                                                    child !== chipsContainer &&
-                                                    child.offsetHeight > 30
-                                                ) {
-                                                    child.style.display =
-                                                        'none';
-                                                }
-                                            },
-                                        );
-
-                                        console.log(
-                                            'âœ… [RENDER] Total de chips renderizados:',
-                                            dias.length,
-                                        );
-                                    } else {
-                                        console.error(
-                                            'âŒ [RENDER] Wrapper não encontrado',
-                                        );
+                                        Array.from(wrapper.children).forEach(child =>
+                                        {
+                                            if (child !== chipsContainer && child.offsetHeight > 30)
+                                            {
+                                                child.style.display = 'none';
+                                            }
+                                        });
+
+                                        console.log("âœ… [RENDER] Total de chips renderizados:", dias.length);
                                     }
-                                } else {
-                                    console.warn(
-                                        'âš ï¸ [RENDER] Nenhum dia encontrado no componente',
-                                    );
+                                    else
+                                    {
+                                        console.error("âŒ [RENDER] Wrapper não encontrado");
+                                    }
+                                }
+                                else
+                                {
+                                    console.warn("âš ï¸ [RENDER] Nenhum dia encontrado no componente");
                                 }
                             }
 
-                            const lstPeriodosKendo =
-                                $('#lstPeriodos').data('kendoDropDownList');
-                            if (lstPeriodosKendo) {
-                                lstPeriodosKendo.enable(false);
-                                console.log(
-                                    '✅ [RENDER] lstPeriodos desabilitado (Kendo)',
-                                );
+                            const lstPeriodos = document.getElementById("lstPeriodos");
+                            if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0])
+                            {
+                                lstPeriodos.ej2_instances[0].enabled = false;
+                                lstPeriodos.ej2_instances[0].dataBind();
+                                console.log("âœ… [RENDER] lstPeriodos desabilitado");
                             }
 
-                            preencherDataFinalRecorrencia(
-                                objViagem.DataFinalRecorrencia,
-                                {
-                                    disable: true,
-                                    contexto: '[RENDER]',
-                                },
-                            );
+                            if (objViagem.dataFinalRecorrencia)
+                            {
+                                const dataObj = new Date(objViagem.dataFinalRecorrencia);
+                                window.setKendoDateValue("txtFinalRecorrencia", dataObj, true);
+                                window.enableKendoDatePicker("txtFinalRecorrencia", false);
+                                console.log("✅ [RENDER] Data final preenchida e desabilitada");
+                            }
                         }, 1000);
                     }, 1000);
                 }
 
-                if (objViagem.Intervalo === 'M') {
-                    const divDiaMes = document.getElementById('divDiaMes');
-                    if (divDiaMes) {
-                        divDiaMes.style.setProperty(
-                            'display',
-                            'block',
-                            'important',
-                        );
-                    }
-
-                    const divFinalRecorrencia = document.getElementById(
-                        'divFinalRecorrencia',
-                    );
-                    if (divFinalRecorrencia) {
-                        divFinalRecorrencia.style.setProperty(
-                            'display',
-                            'block',
-                            'important',
-                        );
-                    }
-
-                    console.log(
-                        '📅 Preenchendo lstDiasMes com dados de recorrência...',
-                    );
-
-                    if (objViagem.diaDoMes) {
-                        const lstDiasMesKendo =
-                            $('#lstDiasMes').data('kendoDropDownList');
-                        if (lstDiasMesKendo) {
-                            lstDiasMesKendo.value(objViagem.diaDoMes);
-                            lstDiasMesKendo.enable(false);
-                            console.log(
-                                '✅ lstDiasMes = ' +
-                                    objViagem.diaDoMes +
-                                    ' (Kendo)',
-                            );
+                if (objViagem.intervalo === 'M')
+                {
+                    console.log("ðŸ“… Preenchendo lstDiasMes com dados de recorrência...");
+
+                    if (objViagem.diaDoMes)
+                    {
+                        const lstDiasMes = document.getElementById("lstDiasMes");
+                        if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0])
+                        {
+                            lstDiasMes.ej2_instances[0].value = objViagem.diaDoMes;
+                            lstDiasMes.ej2_instances[0].enabled = false;
+                            lstDiasMes.ej2_instances[0].dataBind();
+                            console.log("âœ… lstDiasMes = " + objViagem.diaDoMes);
                         }
                     }
 
-                    preencherDataFinalRecorrencia(
-                        objViagem.DataFinalRecorrencia,
-                        {
-                            disable: true,
-                            contexto: 'Mensal (render)',
-                        },
+                    if (objViagem.dataFinalRecorrencia)
+                    {
+                        const dataObj = new Date(objViagem.dataFinalRecorrencia);
+                        window.setKendoDateValue("txtFinalRecorrencia", dataObj, true);
+                        window.enableKendoDatePicker("txtFinalRecorrencia", false);
+                        console.log("✅ Data final de recorrência preenchida");
+                    }
+                }
+            }
+        }
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em restaurarDadosRecorrencia:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "restaurarDadosRecorrencia", error);
+    }
+}
+function restaurarRecorrenciaDiaria(objViagem)
+{
+    try
+    {
+
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            setTimeout(() => {
+                const dataObj = new Date(objViagem.dataFinalRecorrencia);
+                window.setKendoDateValue("txtFinalRecorrencia", dataObj, true);
+                window.enableKendoDatePicker("txtFinalRecorrencia", false);
+            }, 500);
+        }
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em restaurarRecorrenciaDiaria:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "restaurarRecorrenciaDiaria", error);
+    }
+}
+
+function restaurarRecorrenciaSemanal(objViagem)
+{
+    try
+    {
+
+        const divDias = document.getElementById("divDias");
+        if (divDias)
+        {
+            divDias.style.setProperty('display', 'block', 'important');
+        }
+
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        const lstDias = document.getElementById("lstDias");
+        if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0])
+        {
+            const diasSelecionados = [];
+
+            if (objViagem.Monday) diasSelecionados.push("Segunda");
+            if (objViagem.Tuesday) diasSelecionados.push("Terça");
+            if (objViagem.Wednesday) diasSelecionados.push("Quarta");
+            if (objViagem.Thursday) diasSelecionados.push("Quinta");
+            if (objViagem.Friday) diasSelecionados.push("Sexta");
+            if (objViagem.Saturday) diasSelecionados.push("Sábado");
+            if (objViagem.Sunday) diasSelecionados.push("Domingo");
+
+            lstDias.ej2_instances[0].value = diasSelecionados;
+            lstDias.ej2_instances[0].enabled = false;
+            lstDias.ej2_instances[0].dataBind();
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            try
+            {
+                const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
+                const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");
+
+                console.log("🔍 DEBUG Data Final Recorrência:");
+                console.log(" - txtFinalRecorrencia existe?", !!txtFinalRecorrencia);
+                console.log(" - txtFinalRecorrenciaTexto existe?", !!txtFinalRecorrenciaTexto);
+                console.log(" - dataFinalRecorrencia:", objViagem.dataFinalRecorrencia);
+
+                if (txtFinalRecorrenciaTexto)
+                {
+
+                    const dataFinal = new Date(objViagem.dataFinalRecorrencia);
+                    const dia = String(dataFinal.getDate()).padStart(2, '0');
+                    const mes = String(dataFinal.getMonth() + 1).padStart(2, '0');
+                    const ano = dataFinal.getFullYear();
+                    const dataFormatada = `${dia}/${mes}/${ano}`;
+
+                    txtFinalRecorrenciaTexto.value = dataFormatada;
+                    txtFinalRecorrenciaTexto.style.display = "block";
+                    console.log(" - Campo de texto definido como:", dataFormatada);
+
+                    if (txtFinalRecorrencia)
+                    {
+                        window.showKendoDatePicker("txtFinalRecorrencia", false);
+                        console.log(" - Wrapper do DatePicker também ocultado");
+                    }
+
+                    console.log(`✅ Data Final Recorrência exibida em campo de texto: ${dataFormatada}`);
+                }
+                else
+                {
+                    console.error("❌ Campo txtFinalRecorrenciaTexto não encontrado no DOM!");
+                }
+            }
+            catch (error)
+            {
+                console.error("❌ Erro ao definir Data Final Recorrência:", error);
+            }
+        }
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em restaurarRecorrenciaSemanal:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "restaurarRecorrenciaSemanal", error);
+    }
+}
+
+function restaurarRecorrenciaMensal(objViagem)
+{
+    try
+    {
+
+        const divDiaMes = document.getElementById("divDiaMes");
+        if (divDiaMes)
+        {
+            divDiaMes.style.setProperty('display', 'block', 'important');
+        }
+
+        const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
+        if (divFinalRecorrencia)
+        {
+            divFinalRecorrencia.style.setProperty('display', 'block', 'important');
+        }
+
+        if (objViagem.diaMesRecorrencia)
+        {
+            const lstDiasMes = document.getElementById("lstDiasMes");
+            if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0])
+            {
+                lstDiasMes.ej2_instances[0].value = objViagem.diaMesRecorrencia;
+                lstDiasMes.ej2_instances[0].enabled = false;
+                lstDiasMes.ej2_instances[0].dataBind();
+            }
+        }
+
+        if (objViagem.dataFinalRecorrencia)
+        {
+
+            try
+            {
+                const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
+                const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");
+
+                console.log("🔍 DEBUG Data Final Recorrência:");
+                console.log(" - txtFinalRecorrencia existe?", !!txtFinalRecorrencia);
+                console.log(" - txtFinalRecorrenciaTexto existe?", !!txtFinalRecorrenciaTexto);
+                console.log(" - dataFinalRecorrencia:", objViagem.dataFinalRecorrencia);
+
+                if (txtFinalRecorrenciaTexto)
+                {
+
+                    const dataFinal = new Date(objViagem.dataFinalRecorrencia);
+                    const dia = String(dataFinal.getDate()).padStart(2, '0');
+                    const mes = String(dataFinal.getMonth() + 1).padStart(2, '0');
+                    const ano = dataFinal.getFullYear();
+                    const dataFormatada = `${dia}/${mes}/${ano}`;
+
+                    txtFinalRecorrenciaTexto.value = dataFormatada;
+                    txtFinalRecorrenciaTexto.style.display = "block";
+                    console.log(" - Campo de texto definido como:", dataFormatada);
+
+                    if (txtFinalRecorrencia)
+                    {
+                        window.showKendoDatePicker("txtFinalRecorrencia", false);
+                        console.log(" - Wrapper do DatePicker também ocultado");
+                    }
+
+                    console.log(`✅ Data Final Recorrência exibida em campo de texto: ${dataFormatada}`);
+                }
+                else
+                {
+                    console.error("❌ Campo txtFinalRecorrenciaTexto não encontrado no DOM!");
+                }
+            }
+            catch (error)
+            {
+                console.error("❌ Erro ao definir Data Final Recorrência:", error);
+            }
+        }
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em restaurarRecorrenciaMensal:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "restaurarRecorrenciaMensal", error);
+    }
+}
+
+async function restaurarRecorrenciaVariada(objViagem)
+{
+    try
+    {
+
+        const calendarContainer = document.getElementById("calendarContainer");
+        if (calendarContainer)
+        {
+            calendarContainer.style.setProperty('display', 'block', 'important');
+        }
+
+        if (typeof window.inicializarCalendarioSyncfusion === 'function')
+        {
+            window.inicializarCalendarioSyncfusion();
+        }
+
+        if (recorrenciaId)
+        {
+            setTimeout(async () =>
+            {
+                const calDatasSelecionadas = document.getElementById("calDatasSelecionadas");
+
+                if (calDatasSelecionadas && calDatasSelecionadas.ej2_instances && calDatasSelecionadas.ej2_instances[0])
+                {
+                    const calendario = calDatasSelecionadas.ej2_instances[0];
+
+                    const datasArray = await buscarDatasRecorrenciaVariada(
+                        objViagem.recorrenciaViagemId,
+                        objViagem.viagemId
                     );
-                }
-            }
-        }
-    } catch (error) {
-        console.error('❌ Erro em restaurarDadosRecorrencia:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'restaurarDadosRecorrencia',
-            error,
-        );
-    }
-}
-function restaurarRecorrenciaDiaria(objViagem) {
-    try {
-
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Restaurar diária',
-        });
-    } catch (error) {
-        console.error('❌ Erro em restaurarRecorrenciaDiaria:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'restaurarRecorrenciaDiaria',
-            error,
-        );
-    }
-}
-
-function restaurarRecorrenciaSemanal(objViagem) {
-    try {
-
-        const divDias = document.getElementById('divDias');
-        if (divDias) {
-            divDias.style.setProperty('display', 'block', 'important');
-        }
-
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        const lstDiasKendo = $('#lstDias').data('kendoMultiSelect');
-        if (lstDiasKendo) {
-            const diasSelecionados = [];
-
-            if (objViagem.Sunday) diasSelecionados.push(0);
-            if (objViagem.Monday) diasSelecionados.push(1);
-            if (objViagem.Tuesday) diasSelecionados.push(2);
-            if (objViagem.Wednesday) diasSelecionados.push(3);
-            if (objViagem.Thursday) diasSelecionados.push(4);
-            if (objViagem.Friday) diasSelecionados.push(5);
-            if (objViagem.Saturday) diasSelecionados.push(6);
-
-            lstDiasKendo.value(diasSelecionados);
-            lstDiasKendo.enable(false);
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Restaurar semanal',
-        });
-    } catch (error) {
-        console.error('❌ Erro em restaurarRecorrenciaSemanal:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'restaurarRecorrenciaSemanal',
-            error,
-        );
-    }
-}
-
-function restaurarRecorrenciaMensal(objViagem) {
-    try {
-
-        const divDiaMes = document.getElementById('divDiaMes');
-        if (divDiaMes) {
-            divDiaMes.style.setProperty('display', 'block', 'important');
-        }
-
-        const divFinalRecorrencia = document.getElementById(
-            'divFinalRecorrencia',
-        );
-        if (divFinalRecorrencia) {
-            divFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        if (objViagem.DiaMesRecorrencia) {
-            const lstDiasMesKendo = $('#lstDiasMes').data('kendoDropDownList');
-            if (lstDiasMesKendo) {
-                lstDiasMesKendo.value(objViagem.DiaMesRecorrencia);
-                lstDiasMesKendo.enable(false);
-            }
-        }
-
-        preencherDataFinalRecorrencia(objViagem.DataFinalRecorrencia, {
-            disable: true,
-            contexto: 'Restaurar mensal',
-        });
-    } catch (error) {
-        console.error('❌ Erro em restaurarRecorrenciaMensal:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'restaurarRecorrenciaMensal',
-            error,
-        );
-    }
-}
-
-async function restaurarRecorrenciaVariada(objViagem) {
-    try {
-        const recorrenciaId =
-            objViagem?.RecorrenciaViagemId ?? objViagem?.recorrenciaViagemId;
-        const viagemId = objViagem?.ViagemId ?? objViagem?.viagemId;
-
-        const calendarContainer = document.getElementById('calendarContainer');
-        if (calendarContainer) {
-            calendarContainer.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        const calDatasSelecionadasEl = document.getElementById(
-            'calDatasSelecionadas',
-        );
-        if (calDatasSelecionadasEl) {
-            calDatasSelecionadasEl.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-        }
-
-        const labelCalendario =
-            document.getElementById('lblSelecioneAsDatas') ||
-            document.querySelector('label[for="calDatasSelecionadas"]');
-        if (labelCalendario) {
-            labelCalendario.style.setProperty('display', 'block', 'important');
-        }
-
-        if (typeof window.inicializarCalendarioSyncfusion === 'function') {
-            window.inicializarCalendarioSyncfusion();
-        }
-
-        if (recorrenciaId || viagemId) {
-            setTimeout(async () => {
-                const calDatasSelecionadas = document.getElementById(
-                    'calDatasSelecionadas',
-                );
-
-                if (
-                    calDatasSelecionadas &&
-                    calDatasSelecionadas.ej2_instances &&
-                    calDatasSelecionadas.ej2_instances[0]
-                ) {
-                    const calendario = calDatasSelecionadas.ej2_instances[0];
-
-                    const datasArray = await buscarDatasRecorrenciaVariada(
-                        recorrenciaId,
-                        viagemId,
-                    );
-
-                    if (datasArray.length > 0) {
+
+                    if (datasArray.length > 0)
+                    {
                         calendario.values = datasArray;
                         calendario.enabled = false;
                         calendario.dataBind();
 
-                        if (window.atualizarBadgeCalendario) {
+                        if (window.atualizarBadgeCalendario)
+                        {
                             window.atualizarBadgeCalendario(datasArray.length);
                         }
                     }
                 }
             }, 1000);
         }
-    } catch (error) {
-        console.error('❌ Erro em restaurarRecorrenciaVariada:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'restaurarRecorrenciaVariada',
-            error,
-        );
+    }
+    catch (error)
+    {
+        console.error("❌ Erro em restaurarRecorrenciaVariada:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "restaurarRecorrenciaVariada", error);
     }
 }
 
-window.garantirVisibilidadeRecorrencia = function (objViagem) {
-    try {
-        if (!objViagem || !objViagem.Intervalo) return;
-
-        console.log('ðŸ” Garantindo visibilidade dos controles de recorrência');
-
-        const divPeriodo = document.getElementById('divPeriodo');
-        if (divPeriodo) {
+window.garantirVisibilidadeRecorrencia = function (objViagem)
+{
+    try
+    {
+        if (!objViagem || !objViagem.intervalo) return;
+
+        console.log("ðŸ” Garantindo visibilidade dos controles de recorrência");
+
+        const divPeriodo = document.getElementById("divPeriodo");
+        if (divPeriodo)
+        {
             divPeriodo.style.setProperty('display', 'block', 'important');
         }
 
-        switch (objViagem.Intervalo) {
-            case 'D':
-                const divFinalRecorrenciaD = document.getElementById(
-                    'divFinalRecorrencia',
-                );
-                if (divFinalRecorrenciaD) {
-                    divFinalRecorrenciaD.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+        switch (objViagem.intervalo)
+        {
+            case "D":
+                const divFinalRecorrenciaD = document.getElementById("divFinalRecorrencia");
+                if (divFinalRecorrenciaD)
+                {
+                    divFinalRecorrenciaD.style.setProperty('display', 'block', 'important');
                 }
                 break;
 
-            case 'S':
-            case 'Q':
-                const divDias = document.getElementById('divDias');
-                if (divDias) {
+            case "S":
+            case "Q":
+                const divDias = document.getElementById("divDias");
+                if (divDias)
+                {
                     divDias.style.setProperty('display', 'block', 'important');
                 }
-                const divFinalRecorrenciaS = document.getElementById(
-                    'divFinalRecorrencia',
-                );
-                if (divFinalRecorrenciaS) {
-                    divFinalRecorrenciaS.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+                const divFinalRecorrenciaS = document.getElementById("divFinalRecorrencia");
+                if (divFinalRecorrenciaS)
+                {
+                    divFinalRecorrenciaS.style.setProperty('display', 'block', 'important');
                 }
                 break;
 
-            case 'M':
-                const divDiaMes = document.getElementById('divDiaMes');
-                if (divDiaMes) {
-                    divDiaMes.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
-                }
-                const divFinalRecorrenciaM = document.getElementById(
-                    'divFinalRecorrencia',
-                );
-                if (divFinalRecorrenciaM) {
-                    divFinalRecorrenciaM.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+            case "M":
+                const divDiaMes = document.getElementById("divDiaMes");
+                if (divDiaMes)
+                {
+                    divDiaMes.style.setProperty('display', 'block', 'important');
+                }
+                const divFinalRecorrenciaM = document.getElementById("divFinalRecorrencia");
+                if (divFinalRecorrenciaM)
+                {
+                    divFinalRecorrenciaM.style.setProperty('display', 'block', 'important');
                 }
                 break;
 
-            case 'V':
-                const calendarContainer =
-                    document.getElementById('calendarContainer');
-                if (calendarContainer) {
-                    calendarContainer.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+            case "V":
+                const calendarContainer = document.getElementById("calendarContainer");
+                if (calendarContainer)
+                {
+                    calendarContainer.style.setProperty('display', 'block', 'important');
                 }
                 break;
         }
-    } catch (error) {
-        console.error('Erro ao garantir visibilidade:', error);
+    } catch (error)
+    {
+        console.error("Erro ao garantir visibilidade:", error);
     }
 };
 
-window.garantirVisibilidadeRecorrencia = function (objViagem) {
-    try {
-        if (!objViagem || !objViagem.Intervalo) return;
-
-        console.log('ðŸ” Garantindo visibilidade dos controles de recorrência');
-
-        const divPeriodo = document.getElementById('divPeriodo');
-        if (divPeriodo) {
+window.garantirVisibilidadeRecorrencia = function (objViagem)
+{
+    try
+    {
+        if (!objViagem || !objViagem.intervalo) return;
+
+        console.log("ðŸ” Garantindo visibilidade dos controles de recorrência");
+
+        const divPeriodo = document.getElementById("divPeriodo");
+        if (divPeriodo)
+        {
             divPeriodo.style.setProperty('display', 'block', 'important');
         }
 
-        switch (objViagem.Intervalo) {
-            case 'D':
-                const divFinalRecorrenciaD = document.getElementById(
-                    'divFinalRecorrencia',
-                );
-                if (divFinalRecorrenciaD) {
-                    divFinalRecorrenciaD.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+        switch (objViagem.intervalo)
+        {
+            case "D":
+                const divFinalRecorrenciaD = document.getElementById("divFinalRecorrencia");
+                if (divFinalRecorrenciaD)
+                {
+                    divFinalRecorrenciaD.style.setProperty('display', 'block', 'important');
                 }
                 break;
 
-            case 'S':
-            case 'Q':
-                const divDias = document.getElementById('divDias');
-                if (divDias) {
+            case "S":
+            case "Q":
+                const divDias = document.getElementById("divDias");
+                if (divDias)
+                {
                     divDias.style.setProperty('display', 'block', 'important');
                 }
-                const divFinalRecorrenciaS = document.getElementById(
-                    'divFinalRecorrencia',
-                );
-                if (divFinalRecorrenciaS) {
-                    divFinalRecorrenciaS.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+                const divFinalRecorrenciaS = document.getElementById("divFinalRecorrencia");
+                if (divFinalRecorrenciaS)
+                {
+                    divFinalRecorrenciaS.style.setProperty('display', 'block', 'important');
                 }
                 break;
 
-            case 'M':
-                const divDiaMes = document.getElementById('divDiaMes');
-                if (divDiaMes) {
-                    divDiaMes.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
-                }
-                const divFinalRecorrenciaM = document.getElementById(
-                    'divFinalRecorrencia',
-                );
-                if (divFinalRecorrenciaM) {
-                    divFinalRecorrenciaM.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+            case "M":
+                const divDiaMes = document.getElementById("divDiaMes");
+                if (divDiaMes)
+                {
+                    divDiaMes.style.setProperty('display', 'block', 'important');
+                }
+                const divFinalRecorrenciaM = document.getElementById("divFinalRecorrencia");
+                if (divFinalRecorrenciaM)
+                {
+                    divFinalRecorrenciaM.style.setProperty('display', 'block', 'important');
                 }
                 break;
 
-            case 'V':
-                const calendarContainer =
-                    document.getElementById('calendarContainer');
-                if (calendarContainer) {
-                    calendarContainer.style.setProperty(
-                        'display',
-                        'block',
-                        'important',
-                    );
+            case "V":
+                const calendarContainer = document.getElementById("calendarContainer");
+                if (calendarContainer)
+                {
+                    calendarContainer.style.setProperty('display', 'block', 'important');
                 }
                 break;
         }
 
-        console.log(
-            'âœ… Visibilidade dos controles garantida para tipo:',
-            objViagem.Intervalo,
-        );
-    } catch (error) {
-        console.error('âŒ Erro ao garantir visibilidade:', error);
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'garantirVisibilidadeRecorrencia',
-            error,
-        );
+        console.log("âœ… Visibilidade dos controles garantida para tipo:", objViagem.intervalo);
+    } catch (error)
+    {
+        console.error("âŒ Erro ao garantir visibilidade:", error);
+        Alerta.TratamentoErroComLinha("exibe-viagem.js", "garantirVisibilidadeRecorrencia", error);
     }
 };
 
-function configurarRodapeLabelsNovo() {
-    try {
-        console.log('🆕 [configurarRodapeLabelsNovo] === INICIANDO ===');
-
-        const ocultarForcado = (elemento) => {
+function configurarRodapeLabelsNovo()
+{
+    try
+    {
+        console.log("🆕 [configurarRodapeLabelsNovo] === INICIANDO ===");
+
+        const ocultarForcado = (elemento) =>
+        {
             if (!elemento) return false;
 
             elemento.style.setProperty('display', 'none', 'important');
@@ -3760,17 +3270,12 @@
             elemento.style.setProperty('margin', '0', 'important');
             elemento.style.setProperty('padding', '0', 'important');
 
-            elemento.classList.remove(
-                'show',
-                'visible',
-                'd-flex',
-                'd-block',
-                'd-inline',
-            );
+            elemento.classList.remove('show', 'visible', 'd-flex', 'd-block', 'd-inline');
             elemento.classList.add('d-none');
 
             const spans = elemento.querySelectorAll('span, i, svg');
-            spans.forEach((s) => {
+            spans.forEach(s =>
+            {
                 s.textContent = '';
                 s.style.setProperty('display', 'none', 'important');
             });
@@ -3782,103 +3287,103 @@
             'lblUsuarioAgendamento',
             'lblUsuarioCriacao',
             'lblUsuarioFinalizacao',
-            'lblUsuarioCancelamento',
+            'lblUsuarioCancelamento'
         ];
 
-        labelsIds.forEach((id) => {
+        labelsIds.forEach(id =>
+        {
             const el = document.getElementById(id);
-            if (el) {
+            if (el)
+            {
                 const ocultou = ocultarForcado(el);
                 console.log(` ${ocultou ? '✅' : '❌'} ${id} ocultado`);
             }
         });
 
-        const modalFooter = document.querySelector(
-            '#modalViagens .modal-footer',
-        );
-        if (modalFooter) {
-            const labelsUsuario =
-                modalFooter.querySelectorAll('[id^="lblUsuario"]');
-            labelsUsuario.forEach((el) => {
+        const modalFooter = document.querySelector('#modalViagens .modal-footer');
+        if (modalFooter)
+        {
+            const labelsUsuario = modalFooter.querySelectorAll('[id^="lblUsuario"]');
+            labelsUsuario.forEach(el =>
+            {
                 ocultarForcado(el);
             });
-            console.log(
-                ` ✅ ${labelsUsuario.length} labels de usuário ocultados no rodapé`,
-            );
+            console.log(` ✅ ${labelsUsuario.length} labels de usuário ocultados no rodapé`);
         }
 
         const iconesEspecificos = [
             '#lblUsuarioAgendamento .fa-user-clock',
             '#lblUsuarioCriacao .fa-user-plus',
             '#lblUsuarioFinalizacao .fa-user-check',
-            '#lblUsuarioCancelamento .fa-trash-can-xmark',
+            '#lblUsuarioCancelamento .fa-trash-can-xmark'
         ];
 
-        iconesEspecificos.forEach((sel) => {
+        iconesEspecificos.forEach(sel =>
+        {
             const icone = document.querySelector(sel);
-            if (icone) {
+            if (icone)
+            {
                 const labelPai = icone.closest('[id^="lblUsuario"]');
                 if (labelPai) ocultarForcado(labelPai);
             }
         });
 
-        if (typeof $ !== 'undefined') {
+        if (typeof $ !== 'undefined')
+        {
             $('[id^="lblUsuario"]').hide().css({
-                display: 'none !important',
-                visibility: 'hidden !important',
-                opacity: '0 !important',
+                'display': 'none !important',
+                'visibility': 'hidden !important',
+                'opacity': '0 !important'
             });
         }
 
-        setTimeout(() => {
-            console.log(
-                '🔄 [configurarRodapeLabelsNovo] Reforçando ocultação...',
-            );
-
-            labelsIds.forEach((id) => {
+        setTimeout(() =>
+        {
+            console.log("🔄 [configurarRodapeLabelsNovo] Reforçando ocultação...");
+
+            labelsIds.forEach(id =>
+            {
                 const el = document.getElementById(id);
                 if (el) ocultarForcado(el);
             });
 
-            const modalFooter = document.querySelector(
-                '#modalViagens .modal-footer',
-            );
-            if (modalFooter) {
-
-                const labelsUsuario =
-                    modalFooter.querySelectorAll('[id^="lblUsuario"]');
-                labelsUsuario.forEach((label) => {
+            const modalFooter = document.querySelector('#modalViagens .modal-footer');
+            if (modalFooter)
+            {
+
+                const labelsUsuario = modalFooter.querySelectorAll('[id^="lblUsuario"]');
+                labelsUsuario.forEach(label =>
+                {
                     ocultarForcado(label);
 
-                    const iconesInterno =
-                        label.querySelectorAll('[class*="fa-"]');
-                    iconesInterno.forEach((icone) => {
+                    const iconesInterno = label.querySelectorAll('[class*="fa-"]');
+                    iconesInterno.forEach(icone =>
+                    {
                         icone.style.setProperty('display', 'none', 'important');
                     });
                 });
             }
 
-            console.log('✅ [configurarRodapeLabelsNovo] Reforço concluído');
+            console.log("✅ [configurarRodapeLabelsNovo] Reforço concluído");
         }, 100);
 
-        console.log(
-            '✅ [configurarRodapeLabelsNovo] Labels ocultados (múltiplas estratégias)',
-        );
-    } catch (error) {
-        console.error('❌ [configurarRodapeLabelsNovo] Erro:', error);
-        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'exibe-viagem.js',
-                'configurarRodapeLabelsNovo',
-                error,
-            );
+        console.log("✅ [configurarRodapeLabelsNovo] Labels ocultados (múltiplas estratégias)");
+    } catch (error)
+    {
+        console.error("❌ [configurarRodapeLabelsNovo] Erro:", error);
+        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRodapeLabelsNovo", error);
         }
     }
 }
 
-async function recuperarNomeUsuario(usuarioId) {
-    try {
-        if (!usuarioId || usuarioId.trim() === '') {
+async function recuperarNomeUsuario(usuarioId)
+{
+    try
+    {
+        if (!usuarioId || usuarioId.trim() === '')
+        {
             return '';
         }
 
@@ -3886,58 +3391,55 @@
             url: '/api/Agenda/RecuperaUsuario',
             type: 'GET',
             data: { Id: usuarioId },
-            dataType: 'json',
+            dataType: 'json'
         });
 
         return response.data || '';
-    } catch (error) {
-        console.error(
-            '❌ [recuperarNomeUsuario] Erro ao recuperar usuário:',
-            usuarioId,
-            error,
-        );
+    } catch (error)
+    {
+        console.error("❌ [recuperarNomeUsuario] Erro ao recuperar usuário:", usuarioId, error);
         return '';
     }
 }
 
-async function configurarRodapeLabelsExistente(objViagem) {
-    try {
-        console.log('🏷️ [configurarRodapeLabelsExistente] === INICIANDO ===');
-        console.log(
-            '🔍 [configurarRodapeLabelsExistente] objViagem:',
-            objViagem,
-        );
-
-        function formatarDataHora(data, usuario, acao) {
-            try {
-
-                if (!usuario || usuario.trim() === '') {
+async function configurarRodapeLabelsExistente(objViagem)
+{
+    try
+    {
+        console.log("🏷️ [configurarRodapeLabelsExistente] === INICIANDO ===");
+        console.log("🔍 [configurarRodapeLabelsExistente] objViagem:", objViagem);
+
+        function formatarDataHora(data, usuario, acao)
+        {
+            try
+            {
+
+                if (!usuario || usuario.trim() === '')
+                {
                     return `${acao} por Usuário não encontrado`;
                 }
 
-                if (!data) {
+                if (!data)
+                {
                     return `${acao} por ${usuario}`;
                 }
 
-                try {
+                try
+                {
                     const d = new Date(data);
                     const dataStr = d.toLocaleDateString('pt-BR');
-                    const horaStr = d.toLocaleTimeString('pt-BR', {
-                        hour: '2-digit',
-                        minute: '2-digit',
-                    });
+                    const horaStr = d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
                     return `${acao} por ${usuario} em ${dataStr} às ${horaStr}`;
-                } catch {
+                } catch
+                {
 
                     return `${acao} por ${usuario}`;
                 }
-            } catch (error) {
-                console.error('❌ Erro em formatarDataHora:', error);
-                Alerta.TratamentoErroComLinha(
-                    'exibe-viagem.js',
-                    'formatarDataHora',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                console.error("❌ Erro em formatarDataHora:", error);
+                Alerta.TratamentoErroComLinha("exibe-viagem.js", "formatarDataHora", error);
             }
         }
 
@@ -3945,300 +3447,211 @@
             '#lblUsuarioAgendamento',
             '#lblUsuarioCriacao',
             '#lblUsuarioFinalizacao',
-            '#lblUsuarioCancelamento',
+            '#lblUsuarioCancelamento'
         ];
 
-        todosLabels.forEach((sel) => {
+        todosLabels.forEach(sel =>
+        {
             const el = document.querySelector(sel);
-            if (el) {
+            if (el)
+            {
                 el.style.display = 'none';
             }
         });
 
-        console.log(
-            '🔍 [configurarRodapeLabelsExistente] ==========================================',
-        );
-        console.log(
-            '🔍 [configurarRodapeLabelsExistente] DADOS RECEBIDOS DO OBJETO:',
-        );
-        console.log(
-            ' - status (camel):',
-            objViagem.status,
-            '| Status (Pascal):',
-            objViagem.Status,
-        );
-        console.log(
-            ' - statusAgendamento (camel):',
-            objViagem.statusAgendamento,
-            '| StatusAgendamento (Pascal):',
-            objViagem.StatusAgendamento,
-        );
-        console.log(
-            ' - foiAgendamento (camel):',
-            objViagem.foiAgendamento,
-            '| FoiAgendamento (Pascal):',
-            objViagem.FoiAgendamento,
-        );
-        console.log(
-            ' - usuarioIdAgendamento (camel):',
-            objViagem.usuarioIdAgendamento,
-            '| UsuarioIdAgendamento (Pascal):',
-            objViagem.UsuarioIdAgendamento,
-        );
-        console.log(
-            ' - dataAgendamento (camel):',
-            objViagem.dataAgendamento,
-            '| DataAgendamento (Pascal):',
-            objViagem.DataAgendamento,
-        );
-        console.log(
-            ' - usuarioIdCriacao (camel):',
-            objViagem.usuarioIdCriacao,
-            '| UsuarioIdCriacao (Pascal):',
-            objViagem.UsuarioIdCriacao,
-        );
-        console.log(
-            ' - dataCriacao (camel):',
-            objViagem.dataCriacao,
-            '| DataCriacao (Pascal):',
-            objViagem.DataCriacao,
-        );
-        console.log(
-            '🔍 [configurarRodapeLabelsExistente] ==========================================',
-        );
-
-        const foiAgendamentoRaw =
-            objViagem.foiAgendamento ?? objViagem.FoiAgendamento;
-        const foiAgendamento =
-            foiAgendamentoRaw === true ||
-            foiAgendamentoRaw === 1 ||
-            foiAgendamentoRaw === '1' ||
-            foiAgendamentoRaw === 'true' ||
-            foiAgendamentoRaw === 'True';
-
-        const statusAgendamentoRaw =
-            objViagem.statusAgendamento ?? objViagem.StatusAgendamento;
-        const ehAgendamento =
-            statusAgendamentoRaw === true ||
-            statusAgendamentoRaw === 1 ||
-            statusAgendamentoRaw === '1' ||
-            statusAgendamentoRaw === 'true' ||
-            statusAgendamentoRaw === 'True';
-
-        const mostrarAgendadoPor = ehAgendamento || foiAgendamento;
-
-        console.log(
-            '🔍 [configurarRodapeLabelsExistente] foiAgendamentoRaw:',
-            foiAgendamentoRaw,
-            'tipo:',
-            typeof foiAgendamentoRaw,
-        );
-        console.log(
-            '🔍 [configurarRodapeLabelsExistente] statusAgendamentoRaw:',
-            statusAgendamentoRaw,
-            'tipo:',
-            typeof statusAgendamentoRaw,
-        );
-        console.log(
-            '🔍 [configurarRodapeLabelsExistente] mostrarAgendadoPor (calculado):',
-            mostrarAgendadoPor,
-        );
-
-        if (mostrarAgendadoPor) {
-            const labelAgendamento = document.querySelector(
-                '#lblUsuarioAgendamento',
-            );
-            if (labelAgendamento) {
-
-                const usuarioIdAgendamento =
-                    objViagem.usuarioIdAgendamento ??
-                    objViagem.UsuarioIdAgendamento;
-                const dataAgendamento =
-                    objViagem.dataAgendamento ?? objViagem.DataAgendamento;
-                const usuario =
-                    await recuperarNomeUsuario(usuarioIdAgendamento);
+        console.log("🔍 [configurarRodapeLabelsExistente] Status:", objViagem.status ?? objViagem.Status);
+        console.log("🔍 [configurarRodapeLabelsExistente] FoiAgendamento:", objViagem.foiAgendamento ?? objViagem.FoiAgendamento);
+        console.log("🔍 [configurarRodapeLabelsExistente] StatusAgendamento:", objViagem.statusAgendamento ?? objViagem.StatusAgendamento);
+
+        const foiAgendamentoRaw = objViagem.foiAgendamento ?? objViagem.FoiAgendamento;
+        const foiAgendamento = foiAgendamentoRaw === true ||
+                               foiAgendamentoRaw === 1 ||
+                               foiAgendamentoRaw === "1" ||
+                               foiAgendamentoRaw === "true" ||
+                               foiAgendamentoRaw === "True";
+
+        console.log("🔍 [configurarRodapeLabelsExistente] foiAgendamentoRaw:", foiAgendamentoRaw, "tipo:", typeof foiAgendamentoRaw);
+        console.log("🔍 [configurarRodapeLabelsExistente] foiAgendamento (calculado):", foiAgendamento);
+
+        if (foiAgendamento)
+        {
+            const labelAgendamento = document.querySelector('#lblUsuarioAgendamento');
+            if (labelAgendamento)
+            {
+
+                const usuario = await recuperarNomeUsuario(objViagem.usuarioIdAgendamento);
 
                 let texto;
-                if (usuario && usuario.trim() !== '') {
-                    texto = formatarDataHora(
-                        dataAgendamento,
-                        usuario,
-                        'Agendado',
-                    );
-                } else {
+                if (usuario && usuario.trim() !== '')
+                {
+                    texto = formatarDataHora(objViagem.dataAgendamento, usuario, 'Agendado');
+                }
+                else
+                {
                     texto = 'Agendado por Usuário não encontrado';
                 }
 
                 const span = labelAgendamento.querySelector('span');
-                if (span) {
+                if (span)
+                {
                     span.textContent = texto;
-                } else {
+                }
+                else
+                {
                     labelAgendamento.innerHTML = `<i class="fa-duotone fa-solid fa-user-clock" style="--fa-primary-color: #C2410C; --fa-secondary-color: #fed7aa; --fa-secondary-opacity: 0.6;"></i> <span>${texto}</span>`;
                 }
 
                 labelAgendamento.style.display = 'flex';
-                console.log(
-                    '✅ [configurarRodapeLabelsExistente] lblUsuarioAgendamento exibido',
-                );
-            }
-        } else {
-
-            const labelAgendamento = document.querySelector(
-                '#lblUsuarioAgendamento',
-            );
-            if (labelAgendamento) {
+                console.log("✅ [configurarRodapeLabelsExistente] lblUsuarioAgendamento exibido");
+            }
+        }
+        else
+        {
+
+            const labelAgendamento = document.querySelector('#lblUsuarioAgendamento');
+            if (labelAgendamento)
+            {
                 labelAgendamento.style.display = 'none';
                 labelAgendamento.innerHTML = '';
             }
         }
 
-        if (!mostrarAgendadoPor) {
+        if (!foiAgendamento)
+        {
             const labelCriacao = document.querySelector('#lblUsuarioCriacao');
-            if (labelCriacao) {
-
-                const usuarioIdCriacao =
-                    objViagem.usuarioIdCriacao ?? objViagem.UsuarioIdCriacao;
-                const dataCriacao =
-                    objViagem.dataCriacao ?? objViagem.DataCriacao;
-                const usuario = await recuperarNomeUsuario(usuarioIdCriacao);
+            if (labelCriacao)
+            {
+
+                const usuario = await recuperarNomeUsuario(objViagem.usuarioIdCriacao);
 
                 let texto;
-                if (usuario && usuario.trim() !== '') {
-                    texto = formatarDataHora(dataCriacao, usuario, 'Criado');
-                } else {
+                if (usuario && usuario.trim() !== '')
+                {
+                    texto = formatarDataHora(objViagem.dataCriacao, usuario, 'Criado');
+                }
+                else
+                {
                     texto = 'Criado por Usuário não encontrado';
                 }
 
                 const span = labelCriacao.querySelector('span');
-                if (span) {
+                if (span)
+                {
                     span.textContent = texto;
-                } else {
+                }
+                else
+                {
                     labelCriacao.innerHTML = `<i class="fa-sharp-duotone fa-solid fa-user-plus" style="--fa-primary-color: #1e3a8a; --fa-secondary-color: #bfdbfe; --fa-secondary-opacity: 0.6;"></i> <span>${texto}</span>`;
                 }
 
                 labelCriacao.style.display = 'flex';
-                console.log(
-                    '✅ [configurarRodapeLabelsExistente] lblUsuarioCriacao exibido',
-                );
-            }
-        } else {
+                console.log("✅ [configurarRodapeLabelsExistente] lblUsuarioCriacao exibido");
+            }
+        }
+        else
+        {
 
             const labelCriacao = document.querySelector('#lblUsuarioCriacao');
-            if (labelCriacao) {
+            if (labelCriacao)
+            {
                 labelCriacao.style.display = 'none';
                 labelCriacao.innerHTML = '';
             }
         }
 
-        const statusViagem = objViagem.status ?? objViagem.Status;
-        if (statusViagem === 'Realizada') {
-            const labelFinalizacao = document.querySelector(
-                '#lblUsuarioFinalizacao',
-            );
-            if (labelFinalizacao) {
-
-                const usuarioIdFinalizacao =
-                    objViagem.usuarioIdFinalizacao ??
-                    objViagem.UsuarioIdFinalizacao;
-                const dataFinalizacao =
-                    objViagem.dataFinalizacao ?? objViagem.DataFinalizacao;
-                const usuario =
-                    await recuperarNomeUsuario(usuarioIdFinalizacao);
+        if (objViagem.status === "Realizada")
+        {
+            const labelFinalizacao = document.querySelector('#lblUsuarioFinalizacao');
+            if (labelFinalizacao)
+            {
+
+                const usuario = await recuperarNomeUsuario(objViagem.usuarioIdFinalizacao);
 
                 let texto;
-                if (usuario && usuario.trim() !== '') {
-                    texto = formatarDataHora(
-                        dataFinalizacao,
-                        usuario,
-                        'Finalizado',
-                    );
-                } else {
+                if (usuario && usuario.trim() !== '')
+                {
+                    texto = formatarDataHora(objViagem.dataFinalizacao, usuario, 'Finalizado');
+                }
+                else
+                {
                     texto = 'Finalizado por Usuário não encontrado';
                 }
 
                 const span = labelFinalizacao.querySelector('span');
-                if (span) {
+                if (span)
+                {
                     span.textContent = texto;
-                } else {
+                }
+                else
+                {
                     labelFinalizacao.innerHTML = `<i class="fa-duotone fa-solid fa-user-check" style="--fa-primary-color: #155724; --fa-secondary-color: #c3e6cb; --fa-secondary-opacity: 0.6;"></i> <span>${texto}</span>`;
                 }
 
                 labelFinalizacao.style.display = 'flex';
-                console.log(
-                    '✅ [configurarRodapeLabelsExistente] lblUsuarioFinalizacao exibido',
-                );
-            }
-        } else {
-
-            const labelFinalizacao = document.querySelector(
-                '#lblUsuarioFinalizacao',
-            );
-            if (labelFinalizacao) {
+                console.log("✅ [configurarRodapeLabelsExistente] lblUsuarioFinalizacao exibido");
+            }
+        }
+        else
+        {
+
+            const labelFinalizacao = document.querySelector('#lblUsuarioFinalizacao');
+            if (labelFinalizacao)
+            {
                 labelFinalizacao.style.display = 'none';
                 labelFinalizacao.innerHTML = '';
             }
         }
 
-        if (statusViagem === 'Cancelada') {
-            const labelCancelamento = document.querySelector(
-                '#lblUsuarioCancelamento',
-            );
-            if (labelCancelamento) {
-
-                const usuarioIdCancelamento =
-                    objViagem.usuarioIdCancelamento ??
-                    objViagem.UsuarioIdCancelamento;
-                const dataCancelamento =
-                    objViagem.dataCancelamento ?? objViagem.DataCancelamento;
-                const usuario = await recuperarNomeUsuario(
-                    usuarioIdCancelamento,
-                );
+        if (objViagem.status === "Cancelada")
+        {
+            const labelCancelamento = document.querySelector('#lblUsuarioCancelamento');
+            if (labelCancelamento)
+            {
+
+                const usuario = await recuperarNomeUsuario(objViagem.usuarioIdCancelamento);
 
                 let texto;
-                if (usuario && usuario.trim() !== '') {
-                    texto = formatarDataHora(
-                        dataCancelamento,
-                        usuario,
-                        'Cancelado',
-                    );
-                } else {
+                if (usuario && usuario.trim() !== '')
+                {
+                    texto = formatarDataHora(objViagem.dataCancelamento, usuario, 'Cancelado');
+                }
+                else
+                {
                     texto = 'Cancelado por Usuário não encontrado';
                 }
 
                 const span = labelCancelamento.querySelector('span');
-                if (span) {
+                if (span)
+                {
                     span.textContent = texto;
-                } else {
+                }
+                else
+                {
                     labelCancelamento.innerHTML = `<i class="fa-duotone fa-regular fa-trash-can-xmark" style="--fa-primary-color: #8B0000; --fa-secondary-color: #f5c6cb; --fa-secondary-opacity: 0.6;"></i> <span>${texto}</span>`;
                 }
 
                 labelCancelamento.style.display = 'flex';
-                console.log(
-                    '✅ [configurarRodapeLabelsExistente] lblUsuarioCancelamento exibido',
-                );
-            }
-        } else {
-
-            const labelCancelamento = document.querySelector(
-                '#lblUsuarioCancelamento',
-            );
-            if (labelCancelamento) {
+                console.log("✅ [configurarRodapeLabelsExistente] lblUsuarioCancelamento exibido");
+            }
+        }
+        else
+        {
+
+            const labelCancelamento = document.querySelector('#lblUsuarioCancelamento');
+            if (labelCancelamento)
+            {
                 labelCancelamento.style.display = 'none';
                 labelCancelamento.innerHTML = '';
             }
         }
 
-        console.log(
-            '✅ [configurarRodapeLabelsExistente] Labels de rodapé configurados',
-        );
-    } catch (error) {
-        console.error('❌ [configurarRodapeLabelsExistente] Erro:', error);
-        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'exibe-viagem.js',
-                'configurarRodapeLabelsExistente',
-                error,
-            );
+        console.log("✅ [configurarRodapeLabelsExistente] Labels de rodapé configurados");
+    } catch (error)
+    {
+        console.error("❌ [configurarRodapeLabelsExistente] Erro:", error);
+        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("exibe-viagem.js", "configurarRodapeLabelsExistente", error);
         }
     }
 }
@@ -4246,90 +3659,91 @@
 window.ModalConfig = {
     NOVO_AGENDAMENTO: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-plus fa-lg me-2" style="--fa-primary-color: #cc5500; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
-        titulo: 'Criar Agendamento',
-        headerClass: 'modal-header-novo-agendamento',
+        titulo: "Criar Agendamento",
+        headerClass: "modal-header-novo-agendamento"
     },
     EDITAR_AGENDAMENTO: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-lines-pen fa-lg me-2" style="--fa-primary-color: #003d82; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
-        titulo: 'Editar Agendamento',
-        headerClass: 'modal-header-editar-agendamento',
+        titulo: "Editar Agendamento",
+        headerClass: "modal-header-editar-agendamento"
     },
     VIAGEM_ABERTA: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-range fa-lg me-2" style="--fa-primary-color: #314f31; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Viagem Aberta <span class="titulo-subtexto">(permite edição)</span>',
-        headerClass: 'modal-header-viagem-aberta',
+        headerClass: "modal-header-viagem-aberta"
     },
     VIAGEM_AGENDADA: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-circle-user fa-lg me-2" style="--fa-primary-color: #E07435; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Viagem Agendada <span class="titulo-subtexto">(permite edição)</span>',
-        headerClass: 'modal-header-viagem-agendada',
+        headerClass: "modal-header-viagem-agendada"
     },
     VIAGEM_REALIZADA: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-check fa-lg me-2" style="--fa-primary-color: #113D4E; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Viagem Realizada <span class="titulo-subtexto">(não permite edição)</span>',
-        headerClass: 'modal-header-viagem-realizada',
+        headerClass: "modal-header-viagem-realizada"
     },
     VIAGEM_CANCELADA: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-xmark fa-lg me-2" style="--fa-primary-color: #a24e58; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Viagem Cancelada <span class="titulo-subtexto">(não permite edição)</span>',
-        headerClass: 'modal-header-viagem-cancelada',
+        headerClass: "modal-header-viagem-cancelada"
     },
 
     EVENTO_ABERTO: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-users fa-lg me-2" style="--fa-primary-color: #84593D; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Evento Aberto <span class="titulo-subtexto">(permite edição)</span>',
-        headerClass: 'modal-header-viagem-evento',
+        headerClass: "modal-header-viagem-evento"
     },
     EVENTO_AGENDADO: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-users fa-lg me-2" style="--fa-primary-color: #84593D; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Evento Agendado <span class="titulo-subtexto">(permite edição)</span>',
-        headerClass: 'modal-header-viagem-evento',
+        headerClass: "modal-header-viagem-evento"
     },
     EVENTO_REALIZADO: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-users fa-lg me-2" style="--fa-primary-color: #84593D; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Evento Realizado <span class="titulo-subtexto">(não permite edição)</span>',
-        headerClass: 'modal-header-viagem-evento',
+        headerClass: "modal-header-viagem-evento"
     },
     EVENTO_CANCELADO: {
         icone: '<i class="fa-duotone fa-solid fa-calendar-users fa-lg me-2" style="--fa-primary-color: #84593D; --fa-secondary-color: #ffffff; --fa-secondary-opacity: 0.8;"></i>',
         titulo: 'Evento Cancelado <span class="titulo-subtexto">(não permite edição)</span>',
-        headerClass: 'modal-header-viagem-evento',
-    },
+        headerClass: "modal-header-viagem-evento"
+    }
 };
 
-window.setModalTitle = function (tipo, tituloCustomizado = null) {
-    try {
+window.setModalTitle = function (tipo, tituloCustomizado = null)
+{
+    try
+    {
         const config = window.ModalConfig[tipo];
 
-        if (!config) {
-            console.warn('⚠️ Tipo de modal não encontrado:', tipo);
+        if (!config)
+        {
+            console.warn("⚠️ Tipo de modal não encontrado:", tipo);
             return;
         }
 
         let titulo = tituloCustomizado || config.titulo;
 
-        if (
-            tipo === 'VIAGEM_REALIZADA' &&
-            window._foiAgendamentoAtual === true
-        ) {
-            titulo =
-                'Viagem Realizada <span class="titulo-subtexto">(não permite edição)</span> <span class="titulo-via-agendamento">(através de Agendamento)</span>';
+        if (tipo === 'VIAGEM_REALIZADA' && window._foiAgendamentoAtual === true)
+        {
+            titulo = 'Viagem Realizada <span class="titulo-subtexto">(não permite edição)</span> <span class="titulo-via-agendamento">(através de Agendamento)</span>';
         }
 
         const tituloCompleto = config.icone + titulo;
 
-        const modalTitle = document.querySelector('#modalViagens .modal-title');
-        if (modalTitle) {
+        const modalTitle = document.querySelector("#modalViagens .modal-title");
+        if (modalTitle)
+        {
             modalTitle.innerHTML = tituloCompleto;
-            console.log(
-                `✅ Título do modal definido: ${tipo}${window._foiAgendamentoAtual ? ' (via Agendamento)' : ''}`,
-            );
-        } else {
-            console.warn('⚠️ Elemento .modal-title não encontrado');
-        }
-
-        const modalHeader = document.querySelector('#modalViagens #Titulo');
-        if (modalHeader && config.headerClass) {
+            console.log(`✅ Título do modal definido: ${tipo}${window._foiAgendamentoAtual ? ' (via Agendamento)' : ''}`);
+        } else
+        {
+            console.warn("⚠️ Elemento .modal-title não encontrado");
+        }
+
+        const modalHeader = document.querySelector("#modalViagens #Titulo");
+        if (modalHeader && config.headerClass)
+        {
 
             const classesToRemove = [
                 'modal-header-dinheiro',
@@ -4343,106 +3757,96 @@
                 'modal-header-viagem-agendada',
                 'modal-header-viagem-realizada',
                 'modal-header-viagem-cancelada',
-                'modal-header-viagem-evento',
+                'modal-header-viagem-evento'
             ];
 
-            classesToRemove.forEach((cls) => {
+            classesToRemove.forEach(cls => {
                 modalHeader.classList.remove(cls);
             });
 
             modalHeader.classList.add(config.headerClass);
-            console.log(
-                `✅ Classe do header alterada para: ${config.headerClass}`,
-            );
+            console.log(`✅ Classe do header alterada para: ${config.headerClass}`);
         }
 
         window._foiAgendamentoAtual = false;
-    } catch (error) {
-        console.error('❌ Erro ao definir título do modal:', error);
-        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'exibe-viagem.js',
-                'setModalTitle',
-                error,
-            );
+    } catch (error)
+    {
+        console.error("❌ Erro ao definir título do modal:", error);
+        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("exibe-viagem.js", "setModalTitle", error);
         }
     }
 };
 
-$(document).ready(function () {
-
-    if (!window._lstRecorrenteListenerRegistrado) {
-        $('#modalViagens').on('shown.bs.modal', function () {
-
-            if (window.carregandoViagemExistente) {
-                return;
-            }
-
-            setTimeout(() => {
-                inicializarLstRecorrente();
-            }, 100);
-        });
-        window._lstRecorrenteListenerRegistrado = true;
-    }
+$(document).ready(function ()
+{
+
+    $('#modalViagens').on('shown.bs.modal', function ()
+    {
+        console.log("🎯 Modal mostrado - garantindo lstRecorrente inicializado...");
+
+        setTimeout(() =>
+        {
+            inicializarLstRecorrente();
+
+        }, 100);
+    });
+
+    console.log("✅ Listener de modal configurado");
 });
 
-window.habilitarBuscaPrimeiro = function () {
+window.habilitarBuscaPrimeiro = function ()
+{
     window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-    console.log('✅ Busca do primeiro agendamento HABILITADA');
-    console.log(
-        ' A listbox agora tentará buscar o primeiro agendamento da série',
-    );
-    return 'Habilitado';
+    console.log("✅ Busca do primeiro agendamento HABILITADA");
+    console.log(" A listbox agora tentará buscar o primeiro agendamento da série");
+    return "Habilitado";
 };
 
-window.desabilitarBuscaPrimeiro = function () {
+window.desabilitarBuscaPrimeiro = function ()
+{
     window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-    console.log('✅ Busca do primeiro agendamento DESABILITADA');
-    console.log(' A listbox NÃO tentará buscar o primeiro agendamento');
-    console.log(' Isso evita delays se a API não foi corrigida');
-    return 'Desabilitado';
+    console.log("✅ Busca do primeiro agendamento DESABILITADA");
+    console.log(" A listbox NÃO tentará buscar o primeiro agendamento");
+    console.log(" Isso evita delays se a API não foi corrigida");
+    return "Desabilitado";
 };
 
-window.statusBuscaPrimeiro = function () {
-    console.log('📊 STATUS DA BUSCA DO PRIMEIRO AGENDAMENTO:');
-    console.log(
-        ' Habilitada?',
-        window.BUSCAR_PRIMEIRO_AGENDAMENTO ? 'SIM ✅' : 'NÃO ❌',
-    );
-
-    if (window.BUSCAR_PRIMEIRO_AGENDAMENTO) {
-        console.log(
-            ' ℹ️ A listbox tentará buscar o primeiro agendamento da série',
-        );
-        console.log(
-            ' ⚠️ Se a API não foi corrigida, pode causar delay de 5 segundos',
-        );
-        console.log(' Para desabilitar: window.desabilitarBuscaPrimeiro()');
-    } else {
-        console.log(
-            ' ℹ️ A listbox NÃO tentará buscar o primeiro agendamento',
-        );
-        console.log(
-            ' ✅ Não haverá delays, mas o primeiro pode não aparecer na lista',
-        );
-        console.log(' Para habilitar: window.habilitarBuscaPrimeiro()');
+window.statusBuscaPrimeiro = function ()
+{
+    console.log("📊 STATUS DA BUSCA DO PRIMEIRO AGENDAMENTO:");
+    console.log(" Habilitada?", window.BUSCAR_PRIMEIRO_AGENDAMENTO ? "SIM ✅" : "NÃO ❌");
+
+    if (window.BUSCAR_PRIMEIRO_AGENDAMENTO)
+    {
+        console.log(" ℹ️ A listbox tentará buscar o primeiro agendamento da série");
+        console.log(" ⚠️ Se a API não foi corrigida, pode causar delay de 5 segundos");
+        console.log(" Para desabilitar: window.desabilitarBuscaPrimeiro()");
+    }
+    else
+    {
+        console.log(" ℹ️ A listbox NÃO tentará buscar o primeiro agendamento");
+        console.log(" ✅ Não haverá delays, mas o primeiro pode não aparecer na lista");
+        console.log(" Para habilitar: window.habilitarBuscaPrimeiro()");
     }
 
     return window.BUSCAR_PRIMEIRO_AGENDAMENTO;
 };
 
-window.testarAPIObterAgendamento = async function (viagemId) {
-    console.log('🧪 TESTANDO API ObterAgendamento...');
-
-    if (!viagemId) {
-        console.error('❌ Forneça um viagemId válido');
-        console.log(
-            " Exemplo: window.testarAPIObterAgendamento('7b89ce20-7319-4ba0-848a-08de15965414')",
-        );
+window.testarAPIObterAgendamento = async function (viagemId)
+{
+    console.log("🧪 TESTANDO API ObterAgendamento...");
+
+    if (!viagemId)
+    {
+        console.error("❌ Forneça um viagemId válido");
+        console.log(" Exemplo: window.testarAPIObterAgendamento('7b89ce20-7319-4ba0-848a-08de15965414')");
         return;
     }
 
-    try {
+    try
+    {
         const url = `/api/Agenda/ObterAgendamento?viagemId=${viagemId}`;
         console.log(` URL: ${url}`);
 
@@ -4453,324 +3857,205 @@
         console.log(` ⏱️ Tempo de resposta: ${tempo}ms`);
         console.log(` 📊 Status: ${response.status}`);
 
-        if (response.ok) {
+        if (response.ok)
+        {
             const dados = await response.json();
-            console.log(' ✅ API funcionando corretamente!');
-            console.log(' Dados retornados:', dados);
+            console.log(" ✅ API funcionando corretamente!");
+            console.log(" Dados retornados:", dados);
 
             window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-            console.log(
-                ' ✅ Busca do primeiro agendamento HABILITADA automaticamente',
-            );
-        } else if (response.status === 404) {
-            console.log(' ⚠️ Agendamento não encontrado (404)');
-            console.log(' Mas a API está funcionando corretamente!');
+            console.log(" ✅ Busca do primeiro agendamento HABILITADA automaticamente");
+        }
+        else if (response.status === 404)
+        {
+            console.log(" ⚠️ Agendamento não encontrado (404)");
+            console.log(" Mas a API está funcionando corretamente!");
 
             window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-            console.log(' ✅ Busca do primeiro agendamento HABILITADA');
-        } else {
+            console.log(" ✅ Busca do primeiro agendamento HABILITADA");
+        }
+        else
+        {
             console.error(` ❌ Erro: ${response.status}`);
             const erro = await response.text();
-            console.error(' Resposta:', erro);
-
-            if (response.status === 500) {
+            console.error(" Resposta:", erro);
+
+            if (response.status === 500)
+            {
                 window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-                console.warn(
-                    ' ⚠️ Busca do primeiro agendamento DESABILITADA',
-                );
-                console.warn(' API provavelmente não foi corrigida');
-            }
-        }
-    } catch (error) {
-        console.error(' ❌ Erro ao testar API:', error);
+                console.warn(" ⚠️ Busca do primeiro agendamento DESABILITADA");
+                console.warn(" API provavelmente não foi corrigida");
+            }
+        }
+    }
+    catch (error)
+    {
+        console.error(" ❌ Erro ao testar API:", error);
         window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-        console.warn(' ⚠️ Busca do primeiro agendamento DESABILITADA');
+        console.warn(" ⚠️ Busca do primeiro agendamento DESABILITADA");
     }
 };
 
-$(document).ready(function () {
-    if (!window._listboxListenerRegistrado) {
-        $('#modalViagens').on('hidden.bs.modal', function () {
-            limparListboxDatasVariadasCompleto();
-        });
-        window._listboxListenerRegistrado = true;
-    }
-
-    if (!window._labelsListenerRegistrado) {
-        $('#modalViagens').on('shown.bs.modal', function () {
-            setTimeout(() => {
-                document.querySelectorAll('label').forEach((label) => {
-                    if (
-                        label.textContent &&
-                        label.textContent.includes('Selecione as Datas')
-                    ) {
-                        label.style.display = 'none';
-                        label.style.visibility = 'hidden';
-                    }
-                });
-            }, 100);
-        });
-        window._labelsListenerRegistrado = true;
-    }
+$(document).ready(function ()
+{
+    $('#modalViagens').on('hidden.bs.modal', function ()
+    {
+        limparListboxDatasVariadasCompleto();
+        console.log("✅ Modal fechado - listbox limpa");
+    });
+
+    $('#modalViagens').on('shown.bs.modal', function ()
+    {
+        setTimeout(() =>
+        {
+            document.querySelectorAll('label').forEach(label =>
+            {
+                if (label.textContent && label.textContent.includes('Selecione as Datas'))
+                {
+                    label.style.display = 'none';
+                    label.style.visibility = 'hidden';
+                }
+            });
+        }, 100);
+    });
 });
 
-window.habilitarBuscaPrimeiro = function () {
+window.habilitarBuscaPrimeiro = function ()
+{
     window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-    console.log('✅ Busca do primeiro agendamento HABILITADA');
-    console.log('⚠️ ATENÇÃO: Só use se a API ObterAgendamento foi corrigida!');
-    return 'Habilitado';
+    console.log("✅ Busca do primeiro agendamento HABILITADA");
+    console.log("⚠️ ATENÇÃO: Só use se a API ObterAgendamento foi corrigida!");
+    return "Habilitado";
 };
 
-window.desabilitarBuscaPrimeiro = function () {
+window.desabilitarBuscaPrimeiro = function ()
+{
     window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
-    console.log('✅ Busca do primeiro agendamento DESABILITADA');
-    console.log('✅ Sem delays de 30 segundos!');
-    return 'Desabilitado';
+    console.log("✅ Busca do primeiro agendamento DESABILITADA");
+    console.log("✅ Sem delays de 30 segundos!");
+    return "Desabilitado";
 };
 
-window.statusBuscaPrimeiro = function () {
+window.statusBuscaPrimeiro = function ()
+{
     const status = window.BUSCAR_PRIMEIRO_AGENDAMENTO;
-    console.log(
-        `📊 Status da busca do primeiro: ${status ? 'HABILITADA' : 'DESABILITADA'}`,
-    );
-    if (status) {
-        console.log('⚠️ Pode causar delays se a API não foi corrigida!');
-    } else {
-        console.log('✅ Sem delays - performace otimizada!');
+    console.log(`📊 Status da busca do primeiro: ${status ? 'HABILITADA' : 'DESABILITADA'}`);
+    if (status)
+    {
+        console.log("⚠️ Pode causar delays se a API não foi corrigida!");
+    } else
+    {
+        console.log("✅ Sem delays - performace otimizada!");
     }
     return status;
 };
 
-window.testarAPIObterAgendamento = async function (viagemId) {
-    if (!viagemId) {
-        console.error('❌ Forneça um viagemId válido!');
+window.testarAPIObterAgendamento = async function (viagemId)
+{
+    if (!viagemId)
+    {
+        console.error("❌ Forneça um viagemId válido!");
         return;
     }
 
     console.log(`🔍 Testando API com viagemId: ${viagemId}`);
     const url = `/api/Agenda/ObterAgendamento?viagemId=${viagemId}`;
 
-    try {
+    try
+    {
         const inicio = Date.now();
         const response = await fetch(url);
         const tempo = Date.now() - inicio;
 
         console.log(`⏱️ Tempo de resposta: ${tempo}ms`);
 
-        if (response.ok) {
+        if (response.ok)
+        {
             const data = await response.json();
-            console.log('✅ API funcionando!', data);
+            console.log("✅ API funcionando!", data);
             return true;
-        } else {
+        } else
+        {
             console.error(`❌ Erro ${response.status}`);
             return false;
         }
-    } catch (error) {
-        console.error('❌ Erro na API:', error);
+    } catch (error)
+    {
+        console.error("❌ Erro na API:", error);
         return false;
     }
 };
 
-(function () {
-    console.log('🚀 Aplicando override final para ZERO delays...');
+(function ()
+{
+    console.log("🚀 Aplicando override final para ZERO delays...");
 
     window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
     window.apiVerificada = true;
 
-    window.verificarAPICorrigida = async function () {
-        console.log('⚡ verificarAPICorrigida DESABILITADA (override)');
+    window.verificarAPICorrigida = async function ()
+    {
+        console.log("⚡ verificarAPICorrigida DESABILITADA (override)");
         return false;
     };
 
-    console.log('✅ OVERRIDE APLICADO:');
-    console.log(' - Busca do primeiro: DESABILITADA');
-    console.log(' - Verificação de API: DESABILITADA');
-    console.log(' - Modal deve abrir INSTANTANEAMENTE!');
+    console.log("✅ OVERRIDE APLICADO:");
+    console.log(" - Busca do primeiro: DESABILITADA");
+    console.log(" - Verificação de API: DESABILITADA");
+    console.log(" - Modal deve abrir INSTANTANEAMENTE!");
 })();
 
-console.log('===========================================');
-console.log('📌 FUNÇÕES DE CONTROLE DISPONÍVEIS:');
-console.log(
-    ' window.habilitarBuscaPrimeiro() - Habilita busca (se API corrigida)',
-);
-console.log(
-    ' window.desabilitarBuscaPrimeiro() - Desabilita busca (sem delays)',
-);
-console.log('  window.statusBuscaPrimeiro()     - Verifica status atual');
+console.log("===========================================");
+console.log("📌 FUNÇÕES DE CONTROLE DISPONÍVEIS:");
+console.log("  window.habilitarBuscaPrimeiro()  - Habilita busca (se API corrigida)");
+console.log("  window.desabilitarBuscaPrimeiro() - Desabilita busca (sem delays)");
+console.log("  window.statusBuscaPrimeiro()     - Verifica status atual");
 console.log("  window.testarAPIObterAgendamento('id') - Testa a API");
-console.log('  window.forcarLimpeza()           - Força limpeza completa');
-console.log('===========================================');
-
-window.debugDelay = function () {
-    console.log('🔍 DEBUG DE DELAYS:');
-    console.log(
-        '1. BUSCAR_PRIMEIRO_AGENDAMENTO:',
-        window.BUSCAR_PRIMEIRO_AGENDAMENTO,
-    );
-    console.log('2. apiVerificada:', window.apiVerificada);
-    console.log('3. Para ELIMINAR delays: window.desabilitarTodosDelays()');
+console.log("  window.forcarLimpeza()           - Força limpeza completa");
+console.log("===========================================");
+
+window.debugDelay = function ()
+{
+    console.log("🔍 DEBUG DE DELAYS:");
+    console.log("1. BUSCAR_PRIMEIRO_AGENDAMENTO:", window.BUSCAR_PRIMEIRO_AGENDAMENTO);
+    console.log("2. apiVerificada:", window.apiVerificada);
+    console.log("3. Para ELIMINAR delays: window.desabilitarTodosDelays()");
     return {
         buscarPrimeiro: window.BUSCAR_PRIMEIRO_AGENDAMENTO,
-        apiVerificada: window.apiVerificada,
+        apiVerificada: window.apiVerificada
     };
 };
 
-window.desabilitarTodosDelays = function () {
-    console.log('🚀 DESABILITANDO TODOS OS DELAYS...');
+window.desabilitarTodosDelays = function ()
+{
+    console.log("🚀 DESABILITANDO TODOS OS DELAYS...");
     window.BUSCAR_PRIMEIRO_AGENDAMENTO = false;
     window.apiVerificada = true;
-    console.log('✅ Todos os delays desabilitados!');
-    console.log('✅ Modal deve abrir INSTANTANEAMENTE agora!');
-    return 'Delays eliminados';
+    console.log("✅ Todos os delays desabilitados!");
+    console.log("✅ Modal deve abrir INSTANTANEAMENTE agora!");
+    return "Delays eliminados";
 };
 
-try {
+try
+{
     const diagnostico = {
         exibirViagemExistente: typeof exibirViagemExistente === 'function',
-        configurarRecorrenciaVariada:
-            typeof configurarRecorrenciaVariada === 'function',
-        limparListbox:
-            typeof limparListboxDatasVariadas === 'function' ||
-            typeof limparListboxDatasVariadasCompleto === 'function',
-        delays: !window.BUSCAR_PRIMEIRO_AGENDAMENTO,
+        configurarRecorrenciaVariada: typeof configurarRecorrenciaVariada === 'function',
+        limparListbox: typeof limparListboxDatasVariadas === 'function' || typeof limparListboxDatasVariadasCompleto === 'function',
+        delays: !window.BUSCAR_PRIMEIRO_AGENDAMENTO
     };
 
-    console.log('📊 DIAGNÓSTICO exibe-viagem.js:');
-    console.log(
-        ' exibirViagemExistente:',
-        diagnostico.exibirViagemExistente ? '✅' : '❌',
-    );
-    console.log(
-        ' configurarRecorrenciaVariada:',
-        diagnostico.configurarRecorrenciaVariada ? '✅' : '❌',
-    );
-    console.log(' Limpeza listbox:', diagnostico.limparListbox ? '✅' : '❌');
-    console.log(' Delays desabilitados:', diagnostico.delays ? '✅' : '⚠️');
-
-    if (Object.values(diagnostico).every((v) => v)) {
-        console.log('✅ exibe-viagem.js 100% funcional!');
-    }
-} catch (e) {
-    console.error('❌ Erro no diagnóstico:', e);
+    console.log("📊 DIAGNÓSTICO exibe-viagem.js:");
+    console.log(" exibirViagemExistente:", diagnostico.exibirViagemExistente ? "✅" : "❌");
+    console.log(" configurarRecorrenciaVariada:", diagnostico.configurarRecorrenciaVariada ? "✅" : "❌");
+    console.log(" Limpeza listbox:", diagnostico.limparListbox ? "✅" : "❌");
+    console.log(" Delays desabilitados:", diagnostico.delays ? "✅" : "⚠️");
+
+    if (Object.values(diagnostico).every(v => v))
+    {
+        console.log("✅ exibe-viagem.js 100% funcional!");
+    }
+} catch (e)
+{
+    console.error("❌ Erro no diagnóstico:", e);
 }
-
-window.abrirModalFichaVistoria = async function (viagemId, noFicha) {
-    try {
-        console.log('[FichaVistoria] Abrindo modal para viagem:', viagemId);
-
-        const modal = document.getElementById('modalFichaVistoria');
-        const spanNoFicha = document.getElementById('spanNoFichaModal');
-        const loading = document.getElementById('fichaVistoriaLoading');
-        const imgFicha = document.getElementById('imgFichaVistoria');
-        const semImagem = document.getElementById('fichaVistoriaSemImagem');
-
-        if (!modal) {
-            console.error('[FichaVistoria] Modal não encontrado');
-            return;
-        }
-
-        if (spanNoFicha) {
-            spanNoFicha.textContent = noFicha ? `Nº ${noFicha}` : '';
-        }
-
-        if (loading) loading.style.display = 'block';
-        if (imgFicha) {
-            imgFicha.style.display = 'none';
-            imgFicha.src = '';
-        }
-        if (semImagem) semImagem.style.display = 'none';
-
-        const bsModal = new bootstrap.Modal(modal);
-        bsModal.show();
-
-        console.log('[FichaVistoria] Carregando imagem...');
-        const response = await fetch(
-            `/api/Viagem/ObterFichaVistoria?viagemId=${viagemId}`,
-        );
-
-        if (!response.ok) {
-            throw new Error(`HTTP ${response.status}`);
-        }
-
-        const data = await response.json();
-
-        if (loading) loading.style.display = 'none';
-
-        if (data.temImagem && data.imagemBase64) {
-
-            console.log('[FichaVistoria] Imagem carregada com sucesso');
-            imgFicha.src = data.imagemBase64;
-            imgFicha.style.display = 'block';
-        } else {
-
-            console.log('[FichaVistoria] Nenhuma imagem disponível');
-            if (semImagem) semImagem.style.display = 'block';
-        }
-    } catch (error) {
-        console.error('[FichaVistoria] Erro ao abrir modal:', error);
-
-        const loading = document.getElementById('fichaVistoriaLoading');
-        const semImagem = document.getElementById('fichaVistoriaSemImagem');
-
-        if (loading) loading.style.display = 'none';
-        if (semImagem) {
-            semImagem.innerHTML = `
-                <i class="fa-duotone fa-triangle-exclamation text-danger" style="font-size: 4rem;"></i>
-                <p class="mt-3 text-danger">Erro ao carregar a Ficha de Vistoria.</p>
-                <p class="text-muted small">${error.message}</p>
-            `;
-            semImagem.style.display = 'block';
-        }
-
-        Alerta.TratamentoErroComLinha(
-            'exibe-viagem.js',
-            'abrirModalFichaVistoria',
-            error,
-        );
-    }
-};
-
-$(document).ready(function () {
-
-    if (window._fichaVistoriaListenerRegistrado) return;
-    window._fichaVistoriaListenerRegistrado = true;
-
-    $(document).on('click', '#btnVisualizarFichaVistoria', function (e) {
-        e.preventDefault();
-        e.stopPropagation();
-
-        const btn = this;
-        if (btn.disabled) return;
-
-        const viagemId = btn.dataset.viagemId;
-        const noFicha = btn.dataset.noFicha;
-
-        if (viagemId) {
-            window.abrirModalFichaVistoria(viagemId, noFicha);
-        } else {
-            console.warn('[FichaVistoria] ViagemId não definido no botão');
-        }
-    });
-
-    $('#modalFichaVistoria').on('hidden.bs.modal', function () {
-        const imgFicha = document.getElementById('imgFichaVistoria');
-        const loading = document.getElementById('fichaVistoriaLoading');
-        const semImagem = document.getElementById('fichaVistoriaSemImagem');
-
-        if (imgFicha) {
-            imgFicha.src = '';
-            imgFicha.style.display = 'none';
-        }
-        if (loading) loading.style.display = 'block';
-        if (semImagem) {
-            semImagem.style.display = 'none';
-
-            semImagem.innerHTML = `
-                <i class="fa-duotone fa-file-circle-question text-muted" style="font-size: 4rem;"></i>
-                <p class="mt-3 text-muted">Nenhuma imagem de Ficha de Vistoria disponível.</p>
-            `;
-        }
-    });
-
-    console.log('[FichaVistoria] Event listeners registrados com sucesso');
-});
```
