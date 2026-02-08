# wwwroot/js/agendamento/components/modal-viagem-novo.js

**Mudanca:** GRANDE | **+1317** linhas | **-1891** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/modal-viagem-novo.js
+++ ATUAL: wwwroot/js/agendamento/components/modal-viagem-novo.js
@@ -5,94 +5,78 @@
 
 window.ultimoViagemIdCarregado = null;
 
-window.refreshComponenteSafe = function (elementId) {
-    try {
+window.refreshComponenteSafe = function (elementId)
+{
+    try
+    {
         const elemento = document.getElementById(elementId);
-        if (elemento && elemento.ej2_instances && elemento.ej2_instances[0]) {
+        if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
+        {
             const instancia = elemento.ej2_instances[0];
 
-            if (typeof instancia.refresh === 'function') {
+            if (typeof instancia.refresh === 'function')
+            {
                 instancia.refresh();
-            } else if (typeof instancia.dataBind === 'function') {
+            } else if (typeof instancia.dataBind === 'function')
+            {
                 instancia.dataBind();
             }
 
             return true;
         }
         return false;
-    } catch (error) {
+    } catch (error)
+    {
         console.warn(`âš ï¸ Não foi possível atualizar ${elementId}:`, error);
         return false;
     }
 };
 
-window.criarAgendamentoNovo = function () {
-    try {
-        console.log('ðŸ“ [criarAgendamentoNovo] === INICIANDO ===');
-
-        const txtDataInicial = $('#txtDataInicial').data('kendoDatePicker');
-        const txtDataFinal = $('#txtDataFinal').data('kendoDatePicker');
-        const rteDescricao =
-            document.getElementById('rteDescricao')?.ej2_instances?.[0];
-        const lstMotorista =
-            document.getElementById('lstMotorista')?.ej2_instances?.[0];
-        const lstVeiculo =
-            document.getElementById('lstVeiculo')?.ej2_instances?.[0];
-
-        const lstRequisitanteEl = document.getElementById('lstRequisitante');
-        const lstRequisitante = lstRequisitanteEl
-            ? $(lstRequisitanteEl).data('kendoComboBox')
-            : null;
-        const lstSetorRequisitanteAgendamento = document.getElementById(
-            'lstSetorRequisitanteAgendamento',
-        )?.ej2_instances?.[0];
-        const cmbOrigem =
-            document.getElementById('cmbOrigem')?.ej2_instances?.[0];
-        const cmbDestino =
-            document.getElementById('cmbDestino')?.ej2_instances?.[0];
-        const lstFinalidade =
-            document.getElementById('lstFinalidade')?.ej2_instances?.[0];
-        const ddtCombustivelInicial = document.getElementById(
-            'ddtCombustivelInicial',
-        )?.ej2_instances?.[0];
-        const ddtCombustivelFinal = document.getElementById(
-            'ddtCombustivelFinal',
-        )?.ej2_instances?.[0];
-        const lstEventos =
-            document.getElementById('lstEventos')?.ej2_instances?.[0];
-
-        const lstRecorrente = $('#lstRecorrente').data('kendoDropDownList');
-        const lstPeriodos = $('#lstPeriodos').data('kendoDropDownList');
-        const txtFinalRecorrencia = $('#txtFinalRecorrencia').data(
-            'kendoDatePicker',
-        );
-        const lstDias = $('#lstDias').data('kendoMultiSelect');
-        const calDatasSelecionadas = document.getElementById(
-            'calDatasSelecionadas',
-        )?.ej2_instances?.[0];
-        const lstDiasMes = $('#lstDiasMes').data('kendoDropDownList');
-
-        const dataInicialValue = txtDataInicial ? txtDataInicial.value() : null;
-        const dataFinalValue = txtDataFinal ? txtDataFinal.value() : null;
-        const horaInicioTexto = $('#txtHoraInicial').val();
-        const horaFimTexto = $('#txtHoraFinal').val();
-
-        console.log('ðŸ” [DEBUG] Valores capturados:');
-        console.log(' - lstMotorista?.value:', lstMotorista?.value);
-        console.log(' - lstVeiculo?.value:', lstVeiculo?.value);
+window.criarAgendamentoNovo = function ()
+{
+    try
+    {
+        console.log("ðŸ“ [criarAgendamentoNovo] === INICIANDO ===");
+
+        const rteDescricao = document.getElementById("rteDescricao")?.ej2_instances?.[0];
+        const lstMotorista = document.getElementById("lstMotorista")?.ej2_instances?.[0];
+        const lstVeiculo = document.getElementById("lstVeiculo")?.ej2_instances?.[0];
+
+        const lstRequisitanteEl = document.getElementById("lstRequisitante");
+        const lstRequisitante = lstRequisitanteEl ? $(lstRequisitanteEl).data("kendoComboBox") : null;
+        const lstSetorRequisitanteAgendamento = document.getElementById("lstSetorRequisitanteAgendamento")?.ej2_instances?.[0];
+        const cmbOrigem = document.getElementById("cmbOrigem")?.ej2_instances?.[0];
+        const cmbDestino = document.getElementById("cmbDestino")?.ej2_instances?.[0];
+        const lstFinalidade = document.getElementById("lstFinalidade")?.ej2_instances?.[0];
+        const ddtCombustivelInicial = document.getElementById("ddtCombustivelInicial")?.ej2_instances?.[0];
+        const ddtCombustivelFinal = document.getElementById("ddtCombustivelFinal")?.ej2_instances?.[0];
+        const lstEventos = document.getElementById("lstEventos")?.ej2_instances?.[0];
+        const lstRecorrente = document.getElementById("lstRecorrente")?.ej2_instances?.[0];
+        const lstPeriodos = document.getElementById("lstPeriodos")?.ej2_instances?.[0];
+        const txtFinalRecorrencia = window.getKendoDateValue("txtFinalRecorrencia");
+        const lstDias = document.getElementById("lstDias")?.ej2_instances?.[0];
+        const calDatasSelecionadas = document.getElementById("calDatasSelecionadas")?.ej2_instances?.[0];
+        const lstDiasMes = document.getElementById("lstDiasMes")?.ej2_instances?.[0];
+
+        const dataInicialValue = window.getKendoDateValue("txtDataInicial");
+        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
+        const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");
+        const horaFimTexto = window.getKendoTimeValue("txtHoraFinal");
+
+        console.log("ðŸ” [DEBUG] Valores capturados:");
+        console.log(" - lstMotorista?.value:", lstMotorista?.value);
+        console.log(" - lstVeiculo?.value:", lstVeiculo?.value);
 
         const motoristaId = lstMotorista?.value;
         const veiculoId = lstVeiculo?.value;
 
-        const motoristaIdFinal =
-            motoristaId && motoristaId !== 'null' && motoristaId !== 'undefined'
-                ? String(motoristaId)
-                : null;
-
-        const veiculoIdFinal =
-            veiculoId && veiculoId !== 'null' && veiculoId !== 'undefined'
-                ? String(veiculoId)
-                : null;
+        const motoristaIdFinal = (motoristaId && motoristaId !== "null" && motoristaId !== "undefined")
+            ? String(motoristaId)
+            : null;
+
+        const veiculoIdFinal = (veiculoId && veiculoId !== "null" && veiculoId !== "undefined")
+            ? String(veiculoId)
+            : null;
 
         const requisitanteId = lstRequisitante?.value();
 
@@ -102,80 +86,67 @@
         const finalidade = window.getSfValue0(lstFinalidade);
         const combustivelInicial = window.getSfValue0(ddtCombustivelInicial);
         const combustivelFinal = window.getSfValue0(ddtCombustivelFinal);
-        const descricaoHtml = rteDescricao?.getHtml() ?? '';
-        const ramal = $('#txtRamalRequisitanteSF').val();
-        const kmAtual = window.parseIntSafe($('#txtKmAtual').val());
-        const kmInicial = window.parseIntSafe($('#txtKmInicial').val());
-        const kmFinal = window.parseIntSafe($('#txtKmFinal').val());
-        const noFichaVistoria = $('#txtNoFichaVistoria').val() || 0;
-
-        const caboEntregue = $('#hidCaboEntregue').val() === 'true';
-        const caboDevolvido = $('#hidCaboDevolvido').val() === 'true';
-        const arlaEntregue = $('#hidArlaEntregue').val() === 'true';
-        const arlaDevolvido = $('#hidArlaDevolvido').val() === 'true';
+        const descricaoHtml = rteDescricao?.getHtml() ?? "";
+        const ramal = $("#txtRamalRequisitanteSF").val();
+        const kmAtual = window.parseIntSafe($("#txtKmAtual").val());
+        const kmInicial = window.parseIntSafe($("#txtKmInicial").val());
+        const kmFinal = window.parseIntSafe($("#txtKmFinal").val());
+        const noFichaVistoria = $("#txtNoFichaVistoria").val() || 0;
 
         let eventoId = null;
 
-        if (lstEventos?.value) {
+        if (lstEventos?.value)
+        {
             const eventosVal = lstEventos.value;
 
-            if (Array.isArray(eventosVal) && eventosVal.length > 0) {
+            if (Array.isArray(eventosVal) && eventosVal.length > 0)
+            {
                 eventoId = eventosVal[0];
-            } else if (eventosVal) {
+            } else if (eventosVal)
+            {
                 eventoId = eventosVal;
             }
         }
 
-        console.log('🎪 EventoId capturado:', eventoId);
+        console.log("🎪 EventoId capturado:", eventoId);
 
         let dataInicial = null;
         let horaInicio = null;
 
-        if (dataInicialValue) {
+        if (dataInicialValue)
+        {
             const dataInicialDate = new Date(dataInicialValue);
             dataInicial = window.toDateOnlyString(dataInicialDate);
 
-            if (horaInicioTexto) {
-                horaInicio = window.toLocalDateTimeString(
-                    dataInicialDate,
-                    horaInicioTexto,
-                );
+            if (horaInicioTexto)
+            {
+                horaInicio = window.toLocalDateTimeString(dataInicialDate, horaInicioTexto);
             }
         }
 
         let dataFinal = null;
-        if (dataFinalValue) {
+        if (dataFinalValue)
+        {
             const dataFinalDate = new Date(dataFinalValue);
             dataFinal = window.toDateOnlyString(dataFinalDate);
         }
 
-        const recorrente = lstRecorrente ? lstRecorrente.value() : 'N';
-        const intervalo = lstPeriodos ? lstPeriodos.value() : '';
+        const recorrente = lstRecorrente?.value ?? "N";
+        const intervalo = window.getSfValue0(lstPeriodos) ?? "";
 
         let dataFinalRecorrencia = null;
-        const dataFinalRecValue = txtFinalRecorrencia
-            ? txtFinalRecorrencia.value()
-            : null;
-        if (dataFinalRecValue) {
-            const dataFinalRecDate = new Date(dataFinalRecValue);
+        if (txtFinalRecorrencia)
+        {
+            const dataFinalRecDate = new Date(txtFinalRecorrencia);
             dataFinalRecorrencia = window.toDateOnlyString(dataFinalRecDate);
         }
 
-        let monday = false,
-            tuesday = false,
-            wednesday = false;
-        let thursday = false,
-            friday = false,
-            saturday = false,
-            sunday = false;
-
-        const lstDiasValue = lstDias ? lstDias.value() : [];
-        if (
-            lstDiasValue &&
-            Array.isArray(lstDiasValue) &&
-            lstDiasValue.length > 0
-        ) {
-            const diasSelecionados = lstDiasValue;
+        let monday = false, tuesday = false, wednesday = false;
+        let thursday = false, friday = false, saturday = false, sunday = false;
+
+        if (lstDias?.value && Array.isArray(lstDias.value))
+        {
+            const diasSelecionados = lstDias.value;
 
             sunday = diasSelecionados.includes(0);
             monday = diasSelecionados.includes(1);
@@ -185,33 +156,31 @@
             friday = diasSelecionados.includes(5);
             saturday = diasSelecionados.includes(6);
 
-            console.log('ðŸ“… Dias selecionados (números):', diasSelecionados);
-            console.log('ðŸ“‹ Mapeamento booleano:', {
+            console.log("ðŸ“… Dias selecionados (números):", diasSelecionados);
+            console.log("ðŸ“‹ Mapeamento booleano:", {
                 domingo: sunday,
                 segunda: monday,
                 terca: tuesday,
                 quarta: wednesday,
                 quinta: thursday,
                 sexta: friday,
-                sabado: saturday,
+                sabado: saturday
             });
         }
 
         let datasSelecionadas = null;
-        if (
-            calDatasSelecionadas?.values &&
-            Array.isArray(calDatasSelecionadas.values)
-        ) {
+        if (calDatasSelecionadas?.values && Array.isArray(calDatasSelecionadas.values))
+        {
             datasSelecionadas = calDatasSelecionadas.values
-                .map((d) => window.toDateOnlyString(new Date(d)))
-                .join(',');
-        }
-
-        const diaMesRecorrencia = lstDiasMes ? lstDiasMes.value() : null;
+                .map(d => window.toDateOnlyString(new Date(d)))
+                .join(",");
+        }
+
+        const diaMesRecorrencia = window.getSfValue0(lstDiasMes);
 
         const agendamento = {
-            ViagemId: '00000000-0000-0000-0000-000000000000',
-            RecorrenciaViagemId: '00000000-0000-0000-0000-000000000000',
+            ViagemId: "00000000-0000-0000-0000-000000000000",
+            RecorrenciaViagemId: "00000000-0000-0000-0000-000000000000",
             DataInicial: dataInicial,
             HoraInicio: horaInicio,
             DataFinal: dataFinal,
@@ -234,11 +203,11 @@
             Descricao: descricaoHtml,
             StatusAgendamento: true,
             FoiAgendamento: false,
-            Status: 'Agendada',
+            Status: "Agendada",
             EventoId: eventoId,
             Recorrente: recorrente,
             Intervalo: intervalo,
-            dataFinalRecorrencia: dataFinalRecorrencia,
+            DataFinalRecorrencia: dataFinalRecorrencia,
             Monday: monday,
             Tuesday: tuesday,
             Wednesday: wednesday,
@@ -248,311 +217,251 @@
             Sunday: sunday,
 
             DiaMesRecorrencia: diaMesRecorrencia,
-            NoFichaVistoria: noFichaVistoria,
-            CaboEntregue: caboEntregue,
-            CaboDevolvido: caboDevolvido,
-            ArlaEntregue: arlaEntregue,
-            ArlaDevolvido: arlaDevolvido,
+            NoFichaVistoria: noFichaVistoria
         };
 
-        console.log(
-            'âœ… [criarAgendamentoNovo] Agendamento criado:',
-            agendamento,
-        );
+        console.log("âœ… [criarAgendamentoNovo] Agendamento criado:", agendamento);
         return agendamento;
-    } catch (error) {
-        console.error('âŒ [criarAgendamentoNovo] ERRO:', error);
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'criarAgendamentoNovo',
-            error,
-        );
+    } catch (error)
+    {
+        console.error("âŒ [criarAgendamentoNovo] ERRO:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamentoNovo", error);
         return null;
     }
 };
 
-window.criarAgendamento = function (viagemId, viagemIdRecorrente, dataInicial) {
-    try {
-        console.log('ðŸ“ [criarAgendamento] === INICIANDO ===');
-        console.log(' ðŸ“‹ Parâmetros recebidos:');
-        console.log(' - viagemId:', viagemId);
-        console.log(' - viagemIdRecorrente:', viagemIdRecorrente);
-        console.log(' - dataInicial:', dataInicial);
-
-        console.log(' ðŸ”§ Chamando criarAgendamentoNovo()...');
+window.criarAgendamento = function (viagemId, viagemIdRecorrente, dataInicial)
+{
+    try
+    {
+        console.log("ðŸ“ [criarAgendamento] === INICIANDO ===");
+        console.log(" ðŸ“‹ Parâmetros recebidos:");
+        console.log(" - viagemId:", viagemId);
+        console.log(" - viagemIdRecorrente:", viagemIdRecorrente);
+        console.log(" - dataInicial:", dataInicial);
+
+        console.log(" ðŸ”§ Chamando criarAgendamentoNovo()...");
         const agendamentoBase = window.criarAgendamentoNovo();
 
-        if (!agendamentoBase) {
-            console.error(' âŒ criarAgendamentoNovo retornou NULL!');
-            throw new Error(
-                'Não foi possível criar o objeto base do agendamento',
-            );
-        }
-
-        console.log(' âœ… Agendamento base criado com sucesso');
-        console.log(
-            ' ðŸ“‹ DataInicial do base:',
-            agendamentoBase.DataInicial,
-        );
+        if (!agendamentoBase)
+        {
+            console.error(" âŒ criarAgendamentoNovo retornou NULL!");
+            throw new Error("Não foi possível criar o objeto base do agendamento");
+        }
+
+        console.log(" âœ… Agendamento base criado com sucesso");
+        console.log(" ðŸ“‹ DataInicial do base:", agendamentoBase.DataInicial);
 
         const agendamento = { ...agendamentoBase };
 
-        agendamento.ViagemId =
-            viagemId || '00000000-0000-0000-0000-000000000000';
-        agendamento.RecorrenciaViagemId =
-            viagemIdRecorrente || '00000000-0000-0000-0000-000000000000';
-
-        if (dataInicial) {
-            const horaInicioTexto = $('#txtHoraInicial').val();
-
-            if (horaInicioTexto) {
+        agendamento.ViagemId = viagemId || "00000000-0000-0000-0000-000000000000";
+        agendamento.RecorrenciaViagemId = viagemIdRecorrente || "00000000-0000-0000-0000-000000000000";
+
+        if (dataInicial)
+        {
+            const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");
+
+            if (horaInicioTexto)
+            {
                 const dataInicialDate = new Date(dataInicial + 'T00:00:00');
                 agendamento.DataInicial = dataInicial;
-                agendamento.HoraInicio = window.toLocalDateTimeString(
-                    dataInicialDate,
-                    horaInicioTexto,
-                );
-
-                console.log(
-                    ' ðŸ”„ DataInicial SOBRESCRITA para:',
-                    dataInicial,
-                );
-                console.log(
-                    ' ðŸ”„ HoraInicio RECALCULADA para:',
-                    agendamento.HoraInicio,
-                );
-            } else {
-                console.error(' âŒ Hora inicial não encontrada!');
-                throw new Error('Hora de Início é obrigatória');
+                agendamento.HoraInicio = window.toLocalDateTimeString(dataInicialDate, horaInicioTexto);
+
+                console.log(" ðŸ”„ DataInicial SOBRESCRITA para:", dataInicial);
+                console.log(" ðŸ”„ HoraInicio RECALCULADA para:", agendamento.HoraInicio);
+            } else
+            {
+                console.error(" âŒ Hora inicial não encontrada!");
+                throw new Error("Hora de Início é obrigatória");
             }
         }
 
         const erros = [];
 
-        if (!agendamento.DataInicial) {
-            erros.push('Data Inicial é obrigatória');
-        }
-
-        if (!agendamento.HoraInicio) {
-            erros.push('Hora de Início é obrigatória');
-        }
-
-        if (!agendamento.RequisitanteId) {
-            erros.push('Requisitante é obrigatório');
-        }
-
-        if (!agendamento.Finalidade) {
-            erros.push('Finalidade é obrigatória');
-        }
-
-        if (erros.length > 0) {
+        if (!agendamento.DataInicial)
+        {
+            erros.push("Data Inicial é obrigatória");
+        }
+
+        if (!agendamento.HoraInicio)
+        {
+            erros.push("Hora de Início é obrigatória");
+        }
+
+        if (!agendamento.RequisitanteId)
+        {
+            erros.push("Requisitante é obrigatório");
+        }
+
+        if (!agendamento.Finalidade)
+        {
+            erros.push("Finalidade é obrigatória");
+        }
+
+        if (erros.length > 0)
+        {
             console.error('âŒ ERRO DE VALIDAÇÃO:');
             console.error(' - ' + erros[0]);
             Alerta.Erro(erros[0]);
             return null;
         }
 
-        console.log(' âœ… === AGENDAMENTO CRIADO COM SUCESSO ===');
-        console.log(' ðŸ“‹ Resumo do agendamento:');
-        console.log(' - ViagemId:', agendamento.ViagemId);
-        console.log(
-            ' - RecorrenciaViagemId:',
-            agendamento.RecorrenciaViagemId,
-        );
-        console.log(' - DataInicial:', agendamento.DataInicial);
-        console.log(' - HoraInicio:', agendamento.HoraInicio);
-        console.log(' - Recorrente:', agendamento.Recorrente);
-        console.log(' - Intervalo:', agendamento.Intervalo);
-        console.log(' - MotoristaId:', agendamento.MotoristaId);
-        console.log(' - VeiculoId:', agendamento.VeiculoId);
-        console.log(' - RequisitanteId:', agendamento.RequisitanteId);
-        console.log(' - Finalidade:', agendamento.Finalidade);
+        console.log(" âœ… === AGENDAMENTO CRIADO COM SUCESSO ===");
+        console.log(" ðŸ“‹ Resumo do agendamento:");
+        console.log(" - ViagemId:", agendamento.ViagemId);
+        console.log(" - RecorrenciaViagemId:", agendamento.RecorrenciaViagemId);
+        console.log(" - DataInicial:", agendamento.DataInicial);
+        console.log(" - HoraInicio:", agendamento.HoraInicio);
+        console.log(" - Recorrente:", agendamento.Recorrente);
+        console.log(" - Intervalo:", agendamento.Intervalo);
+        console.log(" - MotoristaId:", agendamento.MotoristaId);
+        console.log(" - VeiculoId:", agendamento.VeiculoId);
+        console.log(" - RequisitanteId:", agendamento.RequisitanteId);
+        console.log(" - Finalidade:", agendamento.Finalidade);
 
         return agendamento;
-    } catch (error) {
-        console.error('âŒ [criarAgendamento] ERRO FATAL:', error);
-        console.error(' Stack trace:', error.stack);
-
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'criarAgendamento',
-            error,
-        );
-        AppToast.show(
-            'Vermelho',
-            'Erro ao criar agendamento: ' + error.message,
-            5000,
-        );
+    } catch (error)
+    {
+        console.error("âŒ [criarAgendamento] ERRO FATAL:", error);
+        console.error(" Stack trace:", error.stack);
+
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamento", error);
+        AppToast.show("Vermelho", "Erro ao criar agendamento: " + error.message, 5000);
 
         return null;
     }
 };
 
-window.criarAgendamentoEdicao = function (agendamentoOriginal) {
-    try {
-
-        const rteDescricao =
-            document.getElementById('rteDescricao')?.ej2_instances?.[0];
-        const lstMotorista =
-            document.getElementById('lstMotorista')?.ej2_instances?.[0];
-        const lstVeiculo =
-            document.getElementById('lstVeiculo')?.ej2_instances?.[0];
-        const ddtSetor = document.getElementById(
-            'lstSetorRequisitanteAgendamento',
-        )?.ej2_instances?.[0];
-        const ddtFinalidade =
-            document.getElementById('lstFinalidade')?.ej2_instances?.[0];
-        const ddtCombIniInst = document.getElementById('ddtCombustivelInicial')
-            ?.ej2_instances?.[0];
-        const ddtCombFimInst = document.getElementById('ddtCombustivelFinal')
-            ?.ej2_instances?.[0];
-        const lstEventosInst =
-            document.getElementById('lstEventos')?.ej2_instances?.[0];
-        const txtDataInicialKendo =
-            $('#txtDataInicial').data('kendoDatePicker');
-        const txtDataFinalKendo = $('#txtDataFinal').data('kendoDatePicker');
-        const rteDescricaoHtmlContent = rteDescricao?.getHtml() ?? '';
+window.criarAgendamentoEdicao = function (agendamentoOriginal)
+{
+    try
+    {
+
+        const rteDescricao = document.getElementById("rteDescricao")?.ej2_instances?.[0];
+        const lstMotorista = document.getElementById("lstMotorista")?.ej2_instances?.[0];
+        const lstVeiculo = document.getElementById("lstVeiculo")?.ej2_instances?.[0];
+        const ddtSetor = document.getElementById("lstSetorRequisitanteAgendamento")?.ej2_instances?.[0];
+        const ddtFinalidade = document.getElementById("lstFinalidade")?.ej2_instances?.[0];
+        const ddtCombIniInst = document.getElementById("ddtCombustivelInicial")?.ej2_instances?.[0];
+        const ddtCombFimInst = document.getElementById("ddtCombustivelFinal")?.ej2_instances?.[0];
+        const lstEventosInst = document.getElementById("lstEventos")?.ej2_instances?.[0];
+        const rteDescricaoHtmlContent = rteDescricao?.getHtml() ?? "";
 
         const motoristaId = lstMotorista?.value ?? null;
         const veiculoId = lstVeiculo?.value ?? null;
         const setorId = window.getSfValue0(ddtSetor);
 
-        const lstReqEl = document.getElementById('lstRequisitante');
-        const lstReqKendo = lstReqEl ? $(lstReqEl).data('kendoComboBox') : null;
+        const lstReqEl = document.getElementById("lstRequisitante");
+        const lstReqKendo = lstReqEl ? $(lstReqEl).data("kendoComboBox") : null;
         const requisitanteId = lstReqKendo?.value() ?? null;
 
-        console.log('🔍 DEBUG GRAVAÇÃO Requisitante:');
-        console.log(' - lstReqEl encontrado:', lstReqEl ? 'SIM' : 'NÃO');
-        console.log(' - lstReqKendo encontrado:', lstReqKendo ? 'SIM' : 'NÃO');
-        console.log(' - requisitanteId extraído:', requisitanteId);
-        const destino =
-            document.getElementById('cmbDestino')?.ej2_instances?.[0]?.value ??
-            null;
-        const origem =
-            document.getElementById('cmbOrigem')?.ej2_instances?.[0]?.value ??
-            null;
+        console.log("🔍 DEBUG GRAVAÇÃO Requisitante:");
+        console.log(" - lstReqEl encontrado:", lstReqEl ? "SIM" : "NÃO");
+        console.log(" - lstReqKendo encontrado:", lstReqKendo ? "SIM" : "NÃO");
+        console.log(" - requisitanteId extraído:", requisitanteId);
+        const destino = document.getElementById("cmbDestino")?.ej2_instances?.[0]?.value ?? null;
+        const origem = document.getElementById("cmbOrigem")?.ej2_instances?.[0]?.value ?? null;
         const finalidade = window.getSfValue0(ddtFinalidade);
         const combustivelInicial = window.getSfValue0(ddtCombIniInst);
         const combustivelFinal = window.getSfValue0(ddtCombFimInst);
-        const noFichaVistoria = $('#txtNoFichaVistoria').val() || 0;
-        const kmAtual = window.parseIntSafe($('#txtKmAtual').val());
-        const kmInicial = window.parseIntSafe($('#txtKmInicial').val());
-        const kmFinal = window.parseIntSafe($('#txtKmFinal').val());
-
-        const caboEntregue = $('#hidCaboEntregue').val() === 'true';
-        const caboDevolvido = $('#hidCaboDevolvido').val() === 'true';
-        const arlaEntregue = $('#hidArlaEntregue').val() === 'true';
-        const arlaDevolvido = $('#hidArlaDevolvido').val() === 'true';
-
-        const txtFinalRecorrenciaInst = $('#txtFinalRecorrencia').data(
-            'kendoDatePicker',
-        );
-        const dataFinalRecorrenciaValue = txtFinalRecorrenciaInst
-            ? txtFinalRecorrenciaInst.value()
-            : null;
+        const noFichaVistoria = $("#txtNoFichaVistoria").val() || 0;
+        const kmAtual = window.parseIntSafe($("#txtKmAtual").val());
+        const kmInicial = window.parseIntSafe($("#txtKmInicial").val());
+        const kmFinal = window.parseIntSafe($("#txtKmFinal").val());
+
+        const dataFinalRecorrenciaValue = window.getKendoDateValue("txtFinalRecorrencia");
         let dataFinalRecorrenciaStr = null;
-        if (dataFinalRecorrenciaValue) {
-            const dataFinalRecorrenciaDate = new Date(
-                dataFinalRecorrenciaValue,
-            );
-            dataFinalRecorrenciaStr = window.toDateOnlyString(
-                dataFinalRecorrenciaDate,
-            );
+        if (dataFinalRecorrenciaValue)
+        {
+            const dataFinalRecorrenciaDate = new Date(dataFinalRecorrenciaValue);
+            dataFinalRecorrenciaStr = window.toDateOnlyString(dataFinalRecorrenciaDate);
         }
 
         let eventoId = null;
 
-        if (lstEventosInst?.value) {
+        if (lstEventosInst?.value)
+        {
             const eventosVal = lstEventosInst.value;
 
-            if (Array.isArray(eventosVal) && eventosVal.length > 0) {
+            if (Array.isArray(eventosVal) && eventosVal.length > 0)
+            {
                 eventoId = eventosVal[0];
-            } else if (eventosVal) {
+            } else if (eventosVal)
+            {
                 eventoId = eventosVal;
             }
         }
 
-        console.log('🎪 EventoId capturado:', eventoId);
+        console.log("🎪 EventoId capturado:", eventoId);
 
         let dataInicialStr = null;
         let horaInicioLocal = null;
 
-        if (agendamentoOriginal?.dataInicial) {
+        if (agendamentoOriginal?.dataInicial)
+        {
             const dataOriginalDate = new Date(agendamentoOriginal.dataInicial);
             dataInicialStr = window.toDateOnlyString(dataOriginalDate);
 
-            const horaInicioTexto = $('#txtHoraInicial').val();
-            if (horaInicioTexto) {
-                horaInicioLocal = window.toLocalDateTimeString(
-                    dataOriginalDate,
-                    horaInicioTexto,
-                );
-            }
-
-            console.log(
-                `📅 Usando data ORIGINAL do agendamento: ${dataInicialStr}`,
-            );
-        }
-
-        else {
-            const txtDataInicialValue = txtDataInicial?.value;
-            const horaInicioTexto = $('#txtHoraInicial').val();
-
-            if (txtDataInicialValue) {
+            const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");
+            if (horaInicioTexto)
+            {
+                horaInicioLocal = window.toLocalDateTimeString(dataOriginalDate, horaInicioTexto);
+            }
+
+            console.log(`📅 Usando data ORIGINAL do agendamento: ${dataInicialStr}`);
+        }
+
+        else
+        {
+            const txtDataInicialValue = window.getKendoDateValue("txtDataInicial");
+            const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");
+
+            if (txtDataInicialValue)
+            {
                 const dataInicialDate = new Date(txtDataInicialValue);
                 dataInicialStr = window.toDateOnlyString(dataInicialDate);
 
-                if (horaInicioTexto) {
-                    horaInicioLocal = window.toLocalDateTimeString(
-                        dataInicialDate,
-                        horaInicioTexto,
-                    );
+                if (horaInicioTexto)
+                {
+                    horaInicioLocal = window.toLocalDateTimeString(dataInicialDate, horaInicioTexto);
                 }
 
                 console.log(`📅 Usando data do FORMULÁRIO: ${dataInicialStr}`);
             }
         }
 
-        const dataFinalDate = txtDataFinal?.value
-            ? new Date(txtDataFinal.value)
-            : null;
-        const dataFinalStr = dataFinalDate
-            ? window.toDateOnlyString(dataFinalDate)
-            : null;
-        const horaFimTexto = $('#txtHoraFinal').val() || null;
-
-        const todosFinalPreenchidos =
-            dataFinalStr && horaFimTexto && combustivelFinal && kmFinal;
+        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
+        const dataFinalDate = dataFinalValue ? new Date(dataFinalValue) : null;
+        const dataFinalStr = dataFinalDate ? window.toDateOnlyString(dataFinalDate) : null;
+        const horaFimTexto = window.getKendoTimeValue("txtHoraFinal") || null;
+
+        const todosFinalPreenchidos = dataFinalStr && horaFimTexto && combustivelFinal && kmFinal;
 
         const statusOriginal = agendamentoOriginal?.status;
-        const statusAgendamentoOriginal =
-            agendamentoOriginal?.statusAgendamento;
-
-        const eraAgendamento =
-            statusOriginal === 'Agendada' ||
-            statusAgendamentoOriginal === true ||
-            statusAgendamentoOriginal === 1 ||
-            statusAgendamentoOriginal === '1' ||
-            statusAgendamentoOriginal === 'true';
+        const statusAgendamentoOriginal = agendamentoOriginal?.statusAgendamento;
+
+        const eraAgendamento = statusOriginal === "Agendada" ||
+                              statusAgendamentoOriginal === true ||
+                              statusAgendamentoOriginal === 1 ||
+                              statusAgendamentoOriginal === "1" ||
+                              statusAgendamentoOriginal === "true";
 
         let novoStatus = statusOriginal;
         let novoStatusAgendamento = statusAgendamentoOriginal;
         let novoFoiAgendamento = agendamentoOriginal?.foiAgendamento ?? false;
 
-        if (todosFinalPreenchidos) {
-            novoStatus = 'Realizada';
+        if (todosFinalPreenchidos)
+        {
+            novoStatus = "Realizada";
             novoStatusAgendamento = false;
 
-            if (eraAgendamento) {
+            if (eraAgendamento)
+            {
                 novoFoiAgendamento = true;
-                console.log(
-                    '✅ Viagem finalizada a partir de Agendamento - FoiAgendamento = true',
-                );
-            }
-
-            console.log(
-                "✅ Todos campos de finalização preenchidos - Status = 'Realizada'",
-            );
+                console.log("✅ Viagem finalizada a partir de Agendamento - FoiAgendamento = true");
+            }
+
+            console.log("✅ Todos campos de finalização preenchidos - Status = 'Realizada'");
         }
 
         const payload = {
@@ -572,7 +481,7 @@
             KmInicial: kmInicial,
             KmFinal: kmFinal,
             RequisitanteId: requisitanteId,
-            RamalRequisitante: $('#txtRamalRequisitanteSF').val(),
+            RamalRequisitante: $("#txtRamalRequisitanteSF").val(),
             SetorSolicitanteId: setorId,
             Descricao: rteDescricaoHtmlContent,
             StatusAgendamento: novoStatusAgendamento,
@@ -583,9 +492,7 @@
             RecorrenciaViagemId: agendamentoOriginal?.recorrenciaViagemId,
 
             Intervalo: agendamentoOriginal?.intervalo,
-            dataFinalRecorrencia:
-                dataFinalRecorrenciaStr ||
-                agendamentoOriginal?.dataFinalRecorrencia,
+            DataFinalRecorrencia: dataFinalRecorrenciaStr || agendamentoOriginal?.dataFinalRecorrencia,
             Monday: agendamentoOriginal?.monday,
             Tuesday: agendamentoOriginal?.tuesday,
             Wednesday: agendamentoOriginal?.wednesday,
@@ -594,152 +501,109 @@
             Saturday: agendamentoOriginal?.saturday,
             Sunday: agendamentoOriginal?.sunday,
             DiaMesRecorrencia: agendamentoOriginal?.diaMesRecorrencia,
-            NoFichaVistoria: noFichaVistoria,
-            CaboEntregue: caboEntregue,
-            CaboDevolvido: caboDevolvido,
-            ArlaEntregue: arlaEntregue,
-            ArlaDevolvido: arlaDevolvido,
+            NoFichaVistoria: noFichaVistoria
         };
 
         return payload;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'criarAgendamentoEdicao',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamentoEdicao", error);
         return null;
     }
 };
 
-window.criarAgendamentoViagem = function (agendamentoUnicoAlterado) {
-    try {
-        const rteDescricao =
-            document.getElementById('rteDescricao').ej2_instances[0];
+window.criarAgendamentoViagem = function (agendamentoUnicoAlterado)
+{
+    try
+    {
+        const rteDescricao = document.getElementById("rteDescricao").ej2_instances[0];
         const rteDescricaoHtmlContent = rteDescricao.getHtml();
 
-        let motoristaId =
-            document.getElementById('lstMotorista').ej2_instances[0].value;
-        let veiculoId =
-            document.getElementById('lstVeiculo').ej2_instances[0].value;
+        let motoristaId = document.getElementById("lstMotorista").ej2_instances[0].value;
+        let veiculoId = document.getElementById("lstVeiculo").ej2_instances[0].value;
 
         let eventoId = null;
-        const lstEventosInst =
-            document.getElementById('lstEventos')?.ej2_instances?.[0];
-
-        if (lstEventosInst?.value) {
+        const lstEventosInst = document.getElementById("lstEventos")?.ej2_instances?.[0];
+
+        if (lstEventosInst?.value)
+        {
             const eventosVal = lstEventosInst.value;
 
-            if (Array.isArray(eventosVal) && eventosVal.length > 0) {
+            if (Array.isArray(eventosVal) && eventosVal.length > 0)
+            {
                 eventoId = eventosVal[0];
-            } else if (eventosVal) {
+            } else if (eventosVal)
+            {
                 eventoId = eventosVal;
             }
         }
 
-        console.log('🎪 EventoId capturado:', eventoId);
-
-        let setorId = document.getElementById('lstSetorRequisitanteAgendamento')
-            .ej2_instances[0].value[0];
-        let ramal = $('#txtRamalRequisitanteSF').val();
-
-        const lstReqElement = document.getElementById('lstRequisitante');
-        const lstReqKendoCB = lstReqElement
-            ? $(lstReqElement).data('kendoComboBox')
-            : null;
+        console.log("🎪 EventoId capturado:", eventoId);
+
+        let setorId = document.getElementById("lstSetorRequisitanteAgendamento").ej2_instances[0].value[0];
+        let ramal = $("#txtRamalRequisitanteSF").val();
+
+        const lstReqElement = document.getElementById("lstRequisitante");
+        const lstReqKendoCB = lstReqElement ? $(lstReqElement).data("kendoComboBox") : null;
         let requisitanteId = lstReqKendoCB?.value() ?? null;
 
-        console.log('🔍 DEBUG GRAVAÇÃO Requisitante (Registra Viagem):');
-        console.log(
-            ' - lstReqElement encontrado:',
-            lstReqElement ? 'SIM' : 'NÃO',
-        );
-        console.log(
-            ' - lstReqKendoCB encontrado:',
-            lstReqKendoCB ? 'SIM' : 'NÃO',
-        );
-        console.log(' - requisitanteId extraído:', requisitanteId);
-        let kmAtual = parseInt($('#txtKmAtual').val(), 10);
-        let kmInicial = parseInt($('#txtKmInicial').val(), 10);
-        let kmFinal = parseInt($('#txtKmFinal').val(), 10);
-        let destino =
-            document.getElementById('cmbDestino').ej2_instances[0].value;
-        let origem =
-            document.getElementById('cmbOrigem').ej2_instances[0].value;
-        let finalidade =
-            document.getElementById('lstFinalidade').ej2_instances[0].value[0];
-        let combustivelInicial = document.getElementById(
-            'ddtCombustivelInicial',
-        ).ej2_instances[0].value[0];
-
-        let combustivelFinal = '';
-        if (
-            document.getElementById('ddtCombustivelFinal').ej2_instances[0]
-                .value[0] === null ||
-            document.getElementById('ddtCombustivelFinal').ej2_instances[0]
-                .value[0] === undefined
-        ) {
+        console.log("🔍 DEBUG GRAVAÇÃO Requisitante (Registra Viagem):");
+        console.log(" - lstReqElement encontrado:", lstReqElement ? "SIM" : "NÃO");
+        console.log(" - lstReqKendoCB encontrado:", lstReqKendoCB ? "SIM" : "NÃO");
+        console.log(" - requisitanteId extraído:", requisitanteId);
+        let kmAtual = parseInt($("#txtKmAtual").val(), 10);
+        let kmInicial = parseInt($("#txtKmInicial").val(), 10);
+        let kmFinal = parseInt($("#txtKmFinal").val(), 10);
+        let destino = document.getElementById("cmbDestino").ej2_instances[0].value;
+        let origem = document.getElementById("cmbOrigem").ej2_instances[0].value;
+        let finalidade = document.getElementById("lstFinalidade").ej2_instances[0].value[0];
+        let combustivelInicial = document.getElementById("ddtCombustivelInicial").ej2_instances[0].value[0];
+
+        let combustivelFinal = "";
+        if (document.getElementById("ddtCombustivelFinal").ej2_instances[0].value[0] === null ||
+            document.getElementById("ddtCombustivelFinal").ej2_instances[0].value[0] === undefined)
+        {
             combustivelFinal = null;
-        } else {
-            combustivelFinal = document.getElementById('ddtCombustivelFinal')
-                .ej2_instances[0].value[0];
-        }
-
-        let dataFinal = '';
-        var kendoDataFinalPicker = $('#txtDataFinal').data('kendoDatePicker');
-        var dataFinalKendoValue = kendoDataFinalPicker
-            ? kendoDataFinalPicker.value()
-            : null;
-        if (dataFinalKendoValue === null || dataFinalKendoValue === undefined) {
-            dataFinal = null;
-        } else {
-            dataFinal = moment(dataFinalKendoValue).format('YYYY-MM-DD');
-        }
-
-        let horaInicio = $('#txtHoraInicial').val();
-
-        let horaFim = '';
-        if (
-            document.getElementById('txtHoraFinal').value === null ||
-            document.getElementById('txtHoraFinal').value === undefined ||
-            document.getElementById('txtHoraFinal').value === ''
-        ) {
+        } else
+        {
+            combustivelFinal = document.getElementById("ddtCombustivelFinal").ej2_instances[0].value[0];
+        }
+
+        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
+        let dataFinal = dataFinalValue ? moment(dataFinalValue).format("YYYY-MM-DD") : null;
+
+        let horaInicio = window.getKendoTimeValue("txtHoraInicial");
+
+        let horaFim = "";
+        const horaFimValue = window.getKendoTimeValue("txtHoraFinal");
+        if (!horaFimValue)
+        {
             horaFim = null;
-        } else {
-            horaFim = document.getElementById('txtHoraFinal').value;
-        }
-
-        let statusAgendamento = document.getElementById(
-            'txtStatusAgendamento',
-        ).value;
+        } else
+        {
+            horaFim = horaFimValue;
+        }
+
+        let statusAgendamento = document.getElementById("txtStatusAgendamento").value;
         let criarViagemFechada = true;
-        let noFichaVistoria =
-            document.getElementById('txtNoFichaVistoria').value || 0;
-        let status = 'Aberta';
-
-        const caboEntregue = $('#hidCaboEntregue').val() === 'true';
-        const caboDevolvido = $('#hidCaboDevolvido').val() === 'true';
-        const arlaEntregue = $('#hidArlaEntregue').val() === 'true';
-        const arlaDevolvido = $('#hidArlaDevolvido').val() === 'true';
-
-        const txtFinalRecorrenciaInst2 = $('#txtFinalRecorrencia').data(
-            'kendoDatePicker',
-        );
-        const dataFinalRecorrenciaValue2 = txtFinalRecorrenciaInst2
-            ? txtFinalRecorrenciaInst2.value()
-            : null;
+        let noFichaVistoria = document.getElementById("txtNoFichaVistoria").value || 0;
+        let status = "Aberta";
+
+        const dataFinalRecorrenciaValue2 = window.getKendoDateValue("txtFinalRecorrencia");
         let dataFinalRecorrenciaStr2 = null;
-        if (dataFinalRecorrenciaValue2) {
-            dataFinalRecorrenciaStr2 = moment(
-                dataFinalRecorrenciaValue2,
-            ).format('YYYY-MM-DD');
-        }
-
-        if (dataFinal && horaFim && combustivelFinal && kmFinal) {
-            status = 'Realizada';
-            if (statusAgendamento) {
+        if (dataFinalRecorrenciaValue2)
+        {
+            dataFinalRecorrenciaStr2 = moment(dataFinalRecorrenciaValue2).format("YYYY-MM-DD");
+        }
+
+        if (dataFinal && horaFim && combustivelFinal && kmFinal)
+        {
+            status = "Realizada";
+            if (statusAgendamento)
+            {
                 criarViagemFechada = true;
-            } else {
+            } else
+            {
                 criarViagemFechada = false;
             }
         }
@@ -773,9 +637,7 @@
             RecorrenciaViagemId: agendamentoUnicoAlterado.recorrenciaViagemId,
 
             Intervalo: agendamentoUnicoAlterado.intervalo,
-            dataFinalRecorrencia:
-                dataFinalRecorrenciaStr2 ||
-                agendamentoUnicoAlterado.dataFinalRecorrencia,
+            DataFinalRecorrencia: dataFinalRecorrenciaStr2 || agendamentoUnicoAlterado.dataFinalRecorrencia,
             Monday: agendamentoUnicoAlterado.monday,
             Tuesday: agendamentoUnicoAlterado.tuesday,
             Wednesday: agendamentoUnicoAlterado.wednesday,
@@ -784,390 +646,324 @@
             Saturday: agendamentoUnicoAlterado.saturday,
             Sunday: agendamentoUnicoAlterado.sunday,
             DiaMesRecorrencia: agendamentoUnicoAlterado.diaMesRecorrencia,
-            CriarViagemFechada: criarViagemFechada,
-            CaboEntregue: caboEntregue,
-            CaboDevolvido: caboDevolvido,
-            ArlaEntregue: arlaEntregue,
-            ArlaDevolvido: arlaDevolvido,
+            CriarViagemFechada: criarViagemFechada
         };
 
         return agendamento;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'criarAgendamentoViagem',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamentoViagem", error);
         return null;
     }
 };
 
-window.enviarAgendamento = async function (agendamento) {
-    try {
-
-        if (window.isSubmitting) {
-            console.warn(
-                'âš ï¸ Tentativa de enviar enquanto outra requisição está em andamento.',
-            );
+window.enviarAgendamento = async function (agendamento)
+{
+    try
+    {
+
+        if (window.isSubmitting)
+        {
+            console.warn("âš ï¸ Tentativa de enviar enquanto outra requisição está em andamento.");
             return;
         }
 
-        if (agendamento.DataFinal) {
-            const dataFinalDate = new Date(agendamento.DataFinal + 'T00:00:00');
+        if (agendamento.DataFinal)
+        {
+            const dataFinalDate = new Date(agendamento.DataFinal + "T00:00:00");
             const hoje = new Date();
             hoje.setHours(0, 0, 0, 0);
-            if (dataFinalDate > hoje) {
-
-                var kendoDataFinalClear =
-                    $('#txtDataFinal').data('kendoDatePicker');
-                if (kendoDataFinalClear) {
-                    kendoDataFinalClear.value(null);
-                }
-                AppToast.show(
-                    'Amarelo',
-                    'A Data Final não pode ser superior à data atual.',
-                    4000,
-                );
-                return { success: false, message: 'Data Final inválida' };
+            if (dataFinalDate > hoje)
+            {
+
+                window.setKendoDateValue("txtDataFinal", null);
+                AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
+                return { success: false, message: "Data Final inválida" };
             }
         }
 
         window.isSubmitting = true;
-        $('#btnConfirma').prop('disabled', true);
-
-        try {
+        $("#btnConfirma").prop("disabled", true);
+
+        try
+        {
             const response = await $.ajax({
-                type: 'POST',
-                url: '/api/Agenda/Agendamento',
-                contentType: 'application/json; charset=utf-8',
-                dataType: 'json',
-                data: JSON.stringify(agendamento),
+                type: "POST",
+                url: "/api/Agenda/Agendamento",
+                contentType: "application/json; charset=utf-8",
+                dataType: "json",
+                data: JSON.stringify(agendamento)
             });
 
-            if (response?.success === true) {
-                console.log('Agendamento enviado com sucesso.');
-            } else {
-                console.error(
-                    'Erro ao enviar agendamento: operação mal sucedida.',
-                    response,
-                );
-                throw new Error(
-                    'Erro ao criar agendamento. Operação mal sucedida.',
-                );
+            if (response?.success === true)
+            {
+                console.log("Agendamento enviado com sucesso.");
+            } else
+            {
+                console.error("Erro ao enviar agendamento: operação mal sucedida.", response);
+                throw new Error("Erro ao criar agendamento. Operação mal sucedida.");
             }
 
             response.operacaoBemSucedida = true;
             return response;
-        } catch (error) {
-            if (error.statusText) {
-
-                const erroAjax = window.criarErroAjax(
-                    error,
-                    error.statusText,
-                    error.responseText,
-                    { url: '/api/Agenda/Agendamento', type: 'POST' },
-                );
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'enviarAgendamento',
-                    erroAjax,
-                );
-            } else {
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'enviarAgendamento',
-                    error,
-                );
+        } catch (error)
+        {
+            if (error.statusText)
+            {
+
+                const erroAjax = window.criarErroAjax(error, error.statusText, error.responseText, { url: "/api/Agenda/Agendamento", type: "POST" });
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamento", erroAjax);
+            } else
+            {
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamento", error);
             }
             throw error;
-        } finally {
+        } finally
+        {
             window.isSubmitting = false;
-            $('#btnConfirma').prop('disabled', false);
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'enviarAgendamento',
-            error,
-        );
+            $("#btnConfirma").prop("disabled", false);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamento", error);
         throw error;
     }
 };
 
-window.enviarNovoAgendamento = async function (
-    agendamento,
-    isUltimoAgendamento = true,
-) {
-    try {
-        try {
+window.enviarNovoAgendamento = async function (agendamento, isUltimoAgendamento = true)
+{
+    try
+    {
+        try
+        {
             const objViagem = await window.enviarAgendamento(agendamento);
 
-            if (!objViagem.operacaoBemSucedida) {
-                console.error(
-                    'âŒ Erro ao criar novo agendamento: operação não bem-sucedida',
-                    objViagem,
-                );
-                throw new Error('Erro ao criar novo agendamento');
-            }
-
-            if (isUltimoAgendamento) {
+            if (!objViagem.operacaoBemSucedida)
+            {
+                console.error("âŒ Erro ao criar novo agendamento: operação não bem-sucedida", objViagem);
+                throw new Error("Erro ao criar novo agendamento");
+            }
+
+            if (isUltimoAgendamento)
+            {
                 window.exibirMensagemSucesso();
             }
 
             return objViagem;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'modal-viagem.js',
-                'enviarNovoAgendamento_inner',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarNovoAgendamento_inner", error);
             throw error;
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'enviarNovoAgendamento',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarNovoAgendamento", error);
         throw error;
     }
 };
 
-window.enviarAgendamentoComOpcao = async function (
-    viagemId,
-    editarTodos,
-    editarProximos,
-    dataInicial = null,
-    viagemIdRecorrente = null,
-) {
-    try {
-        try {
-            if (!dataInicial) {
-                dataInicial = moment().format('YYYY-MM-DD');
-            }
-
-            const agendamento = window.criarAgendamento(
-                viagemId,
-                viagemIdRecorrente,
-                dataInicial,
-            );
+window.enviarAgendamentoComOpcao = async function (viagemId, editarTodos, editarProximos, dataInicial = null, viagemIdRecorrente = null)
+{
+    try
+    {
+        try
+        {
+            if (!dataInicial)
+            {
+                dataInicial = moment().format("YYYY-MM-DD");
+            }
+
+            const agendamento = window.criarAgendamento(viagemId, viagemIdRecorrente, dataInicial);
 
             agendamento.EditarTodos = editarTodos;
             agendamento.EditarProximos = editarProximos;
 
             const objViagem = await window.enviarAgendamento(agendamento);
 
-            if (objViagem) {
-                AppToast.show(
-                    'Verde',
-                    'Agendamento atualizado com sucesso',
-                    3000,
-                );
-                $('#modalViagens').modal('hide');
-
+            if (objViagem)
+            {
+                AppToast.show("Verde", "Agendamento atualizado com sucesso", 3000);
+                $("#modalViagens").modal("hide");
+                $(document.body).removeClass("modal-open");
+                $(".modal-backdrop").remove();
+                $(document.body).css("overflow", "");
                 window.calendar.refetchEvents();
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'modal-viagem.js',
-                'enviarAgendamentoComOpcao_inner',
-                error,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'enviarAgendamentoComOpcao',
-            error,
-        );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamentoComOpcao_inner", error);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamentoComOpcao", error);
     }
 };
 
-window.aplicarAtualizacao = async function (objViagem) {
-    try {
-        const response = await fetch('/api/Agenda/Agendamento', {
-            method: 'POST',
+window.aplicarAtualizacao = async function (objViagem)
+{
+    try
+    {
+        const response = await fetch("/api/Agenda/Agendamento", {
+            method: "POST",
             headers: {
-                'Content-Type': 'application/json',
+                "Content-Type": "application/json"
             },
-            body: JSON.stringify(objViagem),
+            body: JSON.stringify(objViagem)
         });
 
         const data = await response.json();
 
-        if (data?.success || data?.data) {
-            AppToast.show(
-                'Verde',
-                data.message || 'Agendamento Atualizado',
-                2000,
-            );
+        if (data?.success || data?.data)
+        {
+            AppToast.show("Verde", data.message || "Agendamento Atualizado", 2000);
             return true;
-        } else {
-            AppToast.show(
-                'Vermelho',
-                data?.message || 'Falha ao atualizar agendamento',
-                2000,
-            );
+        } else
+        {
+            AppToast.show("Vermelho", data?.message || "Falha ao atualizar agendamento", 2000);
             return false;
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'aplicarAtualizacao',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "aplicarAtualizacao", error);
         return false;
     }
 };
 
-window.recuperarViagemEdicao = async function (viagemId) {
-    try {
-        const result =
-            await window.AgendamentoService.obterParaEdicao(viagemId);
-
-        if (result.success) {
-            console.log('DEBUG - Dados carregados do banco:', result.data);
-            console.log(
-                'DEBUG - dataFinalRecorrencia:',
-                result.data.dataFinalRecorrencia,
-            );
+window.recuperarViagemEdicao = async function (viagemId)
+{
+    try
+    {
+        const result = await window.AgendamentoService.obterParaEdicao(viagemId);
+
+        if (result.success)
+        {
+            console.log("DEBUG - Dados carregados do banco:", result.data);
+            console.log("DEBUG - dataFinalRecorrencia:", result.data.dataFinalRecorrencia);
             return result.data;
-        } else {
+        } else
+        {
             throw new Error(result.error);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'recuperarViagemEdicao',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "recuperarViagemEdicao", error);
         return null;
     }
 };
 
-window.obterAgendamentosRecorrentes = async function (recorrenciaViagemId) {
-    try {
-        const result =
-            await window.AgendamentoService.obterRecorrentes(
-                recorrenciaViagemId,
-            );
-
-        if (result.success) {
+window.obterAgendamentosRecorrentes = async function (recorrenciaViagemId)
+{
+    try
+    {
+        const result = await window.AgendamentoService.obterRecorrentes(recorrenciaViagemId);
+
+        if (result.success)
+        {
             return result.data;
-        } else {
+        } else
+        {
             throw new Error(result.error);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'obterAgendamentosRecorrentes',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "obterAgendamentosRecorrentes", error);
         return [];
     }
 };
 
-window.obterAgendamentosRecorrenteInicial = async function (viagemId) {
-    try {
-        const result =
-            await window.AgendamentoService.obterRecorrenteInicial(viagemId);
-
-        if (result.success) {
+window.obterAgendamentosRecorrenteInicial = async function (viagemId)
+{
+    try
+    {
+        const result = await window.AgendamentoService.obterRecorrenteInicial(viagemId);
+
+        if (result.success)
+        {
             return result.data;
-        } else {
+        } else
+        {
             throw new Error(result.error);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'obterAgendamentosRecorrenteInicial',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "obterAgendamentosRecorrenteInicial", error);
         return [];
     }
 };
 
-window.excluirAgendamento = async function (viagemId) {
-    try {
+window.excluirAgendamento = async function (viagemId)
+{
+    try
+    {
         const result = await window.AgendamentoService.excluir(viagemId);
 
-        if (result.success) {
-
-        } else {
-            AppToast.show('Vermelho', result.message, 2000);
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'excluirAgendamento',
-            error,
-        );
+        if (result.success)
+        {
+
+        } else
+        {
+            AppToast.show("Vermelho", result.message, 2000);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "excluirAgendamento", error);
     }
 };
 
-window.cancelarAgendamento = async function (
-    viagemId,
-    descricao,
-    mostrarToast = true,
-) {
-    try {
-        const result = await window.AgendamentoService.cancelar(
-            viagemId,
-            descricao,
-        );
-
-        if (result.success) {
-            if (mostrarToast) {
-                AppToast.show(
-                    'Verde',
-                    'O agendamento foi cancelado com sucesso!',
-                    2000,
-                );
+window.cancelarAgendamento = async function (viagemId, descricao, mostrarToast = true)
+{
+    try
+    {
+        const result = await window.AgendamentoService.cancelar(viagemId, descricao);
+
+        if (result.success)
+        {
+            if (mostrarToast)
+            {
+                AppToast.show("Verde", "O agendamento foi cancelado com sucesso!", 2000);
             }
             return result;
-        } else {
-            AppToast.show('Vermelho', result.message, 2000);
+        } else
+        {
+            AppToast.show("Vermelho", result.message, 2000);
             return result;
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'cancelarAgendamento',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "cancelarAgendamento", error);
         return { success: false, error: error.message };
     }
 };
 
-function detectarAlteracaoDataInicial(agendamentoOriginal) {
-    try {
+function detectarAlteracaoDataInicial(agendamentoOriginal)
+{
+    try
+    {
 
         const dataOriginalStr = agendamentoOriginal?.dataInicial;
-        if (!dataOriginalStr) {
+        if (!dataOriginalStr)
+        {
             return { alterou: false, dataOriginal: null, dataNova: null };
         }
 
         const dataOriginal = new Date(dataOriginalStr);
         dataOriginal.setHours(0, 0, 0, 0);
 
-        const txtDataInicialKendoCheck =
-            $('#txtDataInicial').data('kendoDatePicker');
-        const txtDataInicialKendoValue = txtDataInicialKendoCheck
-            ? txtDataInicialKendoCheck.value()
-            : null;
-        if (!txtDataInicialKendoCheck || !txtDataInicialKendoValue) {
+        const dataNovaValue = window.getKendoDateValue("txtDataInicial");
+        if (!dataNovaValue)
+        {
             return { alterou: false, dataOriginal: null, dataNova: null };
         }
 
-        const dataNova = new Date(txtDataInicialKendoValue);
+        const dataNova = new Date(dataNovaValue);
         dataNova.setHours(0, 0, 0, 0);
 
         const alterou = dataOriginal.getTime() !== dataNova.getTime();
 
-        console.log('ðŸ“… [DataInicial] Detecção de alteração:', {
+        console.log("ðŸ“… [DataInicial] Detecção de alteração:", {
             dataOriginal: dataOriginal.toLocaleDateString('pt-BR'),
             dataNova: dataNova.toLocaleDateString('pt-BR'),
-            alterou: alterou,
+            alterou: alterou
         });
 
         return {
@@ -1175,131 +971,113 @@
             dataOriginal: dataOriginal,
             dataNova: dataNova,
             dataOriginalStr: dataOriginalStr,
-            dataNovaStr: window.toDateOnlyString(dataNova),
+            dataNovaStr: window.toDateOnlyString(dataNova)
         };
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'detectarAlteracaoDataInicial',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "detectarAlteracaoDataInicial", error);
         return { alterou: false, dataOriginal: null, dataNova: null };
     }
 }
 
-function calcularPushDatas(dataOriginal, dataNova, intervalo) {
-    try {
-        const diffDias = Math.floor(
-            (dataNova - dataOriginal) / (1000 * 60 * 60 * 24),
-        );
-
-        console.log('ðŸ“Š [Push] Diferença em dias:', diffDias);
-
-        switch (intervalo) {
-            case 'D':
+function calcularPushDatas(dataOriginal, dataNova, intervalo)
+{
+    try
+    {
+        const diffDias = Math.floor((dataNova - dataOriginal) / (1000 * 60 * 60 * 24));
+
+        console.log("ðŸ“Š [Push] Diferença em dias:", diffDias);
+
+        switch (intervalo)
+        {
+            case "D":
                 return diffDias;
 
-            case 'S':
+            case "S":
                 return Math.floor(diffDias / 7);
 
-            case 'Q':
+            case "Q":
                 return Math.floor(diffDias / 14);
 
-            case 'M':
+            case "M":
                 const mOriginal = moment(dataOriginal);
                 const mNova = moment(dataNova);
                 return mNova.diff(mOriginal, 'months');
 
             default:
-                console.warn('âš ï¸ Intervalo não reconhecido:', intervalo);
+                console.warn("âš ï¸ Intervalo não reconhecido:", intervalo);
                 return 0;
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'calcularPushDatas',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "calcularPushDatas", error);
         return 0;
     }
 }
 
-async function aplicarPushDatasSubsequentes(
-    recorrenciaViagemId,
-    dataOriginal,
-    dataNova,
-    intervalo,
-    dataReferencia,
-) {
-    try {
-        console.log(
-            'ðŸ”„ [Push] Iniciando aplicação de push nas datas subsequentes...',
-        );
-
-        const agendamentos =
-            await window.obterAgendamentosRecorrentes(recorrenciaViagemId);
-
-        if (!agendamentos || agendamentos.length === 0) {
-            console.warn('âš ï¸ Nenhum agendamento recorrente encontrado');
+async function aplicarPushDatasSubsequentes(recorrenciaViagemId, dataOriginal, dataNova, intervalo, dataReferencia)
+{
+    try
+    {
+        console.log("ðŸ”„ [Push] Iniciando aplicação de push nas datas subsequentes...");
+
+        const agendamentos = await window.obterAgendamentosRecorrentes(recorrenciaViagemId);
+
+        if (!agendamentos || agendamentos.length === 0)
+        {
+            console.warn("âš ï¸ Nenhum agendamento recorrente encontrado");
             return false;
         }
 
-        const pushUnidades = calcularPushDatas(
-            dataOriginal,
-            dataNova,
-            intervalo,
-        );
-
-        console.log(
-            'ðŸ“Š [Push] Unidades a avançar:',
-            pushUnidades,
-            'no intervalo:',
-            intervalo,
-        );
+        const pushUnidades = calcularPushDatas(dataOriginal, dataNova, intervalo);
+
+        console.log("ðŸ“Š [Push] Unidades a avançar:", pushUnidades, "no intervalo:", intervalo);
 
         let contadorSucesso = 0;
         let contadorErro = 0;
 
-        const agendamentosFiltrados = agendamentos.filter((ag) => {
+        const agendamentosFiltrados = agendamentos.filter(ag =>
+        {
             const dataAg = new Date(ag.dataInicial);
             dataAg.setHours(0, 0, 0, 0);
             return dataAg.getTime() >= dataReferencia.getTime();
         });
 
-        console.log(
-            `ðŸ“‹ [Push] Total de agendamentos a atualizar: ${agendamentosFiltrados.length}`,
-        );
-
-        for (const agendamento of agendamentosFiltrados) {
-            try {
+        console.log(`ðŸ“‹ [Push] Total de agendamentos a atualizar: ${agendamentosFiltrados.length}`);
+
+        for (const agendamento of agendamentosFiltrados)
+        {
+            try
+            {
                 const dataAtual = moment(agendamento.dataInicial);
                 let novaData;
 
-                switch (intervalo) {
-                    case 'D':
+                switch (intervalo)
+                {
+                    case "D":
                         novaData = dataAtual.add(pushUnidades, 'days');
                         break;
 
-                    case 'S':
+                    case "S":
                         novaData = dataAtual.add(pushUnidades, 'weeks');
                         break;
 
-                    case 'Q':
+                    case "Q":
                         novaData = dataAtual.add(pushUnidades * 2, 'weeks');
                         break;
 
-                    case 'M':
+                    case "M":
                         novaData = dataAtual.add(pushUnidades, 'months');
                         break;
 
                     default:
-                        console.warn('âš ï¸ Intervalo inválido:', intervalo);
+                        console.warn("âš ï¸ Intervalo inválido:", intervalo);
                         continue;
                 }
 
                 const payload = {
                     ViagemId: agendamento.viagemId,
-                    DataInicial: novaData.format('YYYY-MM-DD'),
+                    DataInicial: novaData.format("YYYY-MM-DD"),
                     HoraInicio: agendamento.horaInicio,
                     DataFinal: agendamento.dataFinal,
                     HoraFim: agendamento.horaFim,
@@ -1325,7 +1103,7 @@
                     RecorrenciaViagemId: agendamento.recorrenciaViagemId,
 
                     Intervalo: agendamento.intervalo,
-                    dataFinalRecorrencia: agendamento.dataFinalRecorrencia,
+                    DataFinalRecorrencia: agendamento.dataFinalRecorrencia,
                     Monday: agendamento.monday,
                     Tuesday: agendamento.tuesday,
                     Wednesday: agendamento.wednesday,
@@ -1334,54 +1112,43 @@
                     Saturday: agendamento.saturday,
                     Sunday: agendamento.sunday,
                     DiaMesRecorrencia: agendamento.diaMesRecorrencia,
-                    NoFichaVistoria: agendamento.noFichaVistoria,
+                    NoFichaVistoria: agendamento.noFichaVistoria
                 };
 
                 const sucesso = await window.aplicarAtualizacao(payload);
 
-                if (sucesso) {
+                if (sucesso)
+                {
                     contadorSucesso++;
-                    console.log(
-                        `âœ… [Push] Agendamento ${agendamento.viagemId} atualizado para ${novaData.format('DD/MM/YYYY')}`,
-                    );
-                } else {
+                    console.log(`âœ… [Push] Agendamento ${agendamento.viagemId} atualizado para ${novaData.format("DD/MM/YYYY")}`);
+                } else
+                {
                     contadorErro++;
-                    console.error(
-                        `âŒ [Push] Falha ao atualizar ${agendamento.viagemId}`,
-                    );
+                    console.error(`âŒ [Push] Falha ao atualizar ${agendamento.viagemId}`);
                 }
-            } catch (error) {
+            } catch (error)
+            {
                 contadorErro++;
-                console.error(
-                    `âŒ [Push] Erro ao processar agendamento:`,
-                    error,
-                );
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'aplicarPushDatasSubsequentes_loop',
-                    error,
-                );
-            }
-        }
-
-        console.log(
-            `ðŸ“Š [Push] Resultado: ${contadorSucesso} sucessos, ${contadorErro} erros`,
-        );
+                console.error(`âŒ [Push] Erro ao processar agendamento:`, error);
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "aplicarPushDatasSubsequentes_loop", error);
+            }
+        }
+
+        console.log(`ðŸ“Š [Push] Resultado: ${contadorSucesso} sucessos, ${contadorErro} erros`);
 
         return contadorErro === 0;
-    } catch (error) {
-        console.error('âŒ [Push] Erro geral:', error);
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'aplicarPushDatasSubsequentes',
-            error,
-        );
+    } catch (error)
+    {
+        console.error("âŒ [Push] Erro geral:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "aplicarPushDatasSubsequentes", error);
         return false;
     }
 }
 
-async function perguntarAlteracaoRecorrente(dataOriginalStr, dataNovaStr) {
-    try {
+async function perguntarAlteracaoRecorrente(dataOriginalStr, dataNovaStr)
+{
+    try
+    {
         const mensagem = `
             <div class="text-start">
                 <p><strong>Você está alterando a Data Inicial de um agendamento recorrente:</strong></p>
@@ -1394,700 +1161,484 @@
         `;
 
         const resultado = await Alerta.Confirmar3(
-            'Alteração de Data Inicial',
+            "Alteração de Data Inicial",
             mensagem,
-            'Alterar apenas este',
-            'Alterar este e subsequentes',
-            'Cancelar operação',
+            "Alterar apenas este",
+            "Alterar este e subsequentes",
+            "Cancelar operação"
         );
 
-        console.log('ðŸ¤” [Pergunta] Resposta do usuário:', resultado);
-
-        switch (resultado) {
+        console.log("ðŸ¤” [Pergunta] Resposta do usuário:", resultado);
+
+        switch (resultado)
+        {
             case 1:
-                return 'apenas_este';
+                return "apenas_este";
             case 2:
-                return 'todos_subsequentes';
+                return "todos_subsequentes";
             case 3:
             default:
-                return 'cancelar';
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'perguntarAlteracaoRecorrente',
-            error,
-        );
-        return 'cancelar';
+                return "cancelar";
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "perguntarAlteracaoRecorrente", error);
+        return "cancelar";
     }
 }
 
-async function processarAlteracaoDataInicial(
-    agendamentoOriginal,
-    agendamentoEditado,
-) {
-    try {
-        console.log('ðŸ”§ [ProcessarData] Iniciando processamento...');
+async function processarAlteracaoDataInicial(agendamentoOriginal, agendamentoEditado)
+{
+    try
+    {
+        console.log("ðŸ”§ [ProcessarData] Iniciando processamento...");
 
         const deteccao = detectarAlteracaoDataInicial(agendamentoOriginal);
 
-        if (!deteccao.alterou) {
-            console.log(
-                'â„¹ï¸ [ProcessarData] Data não foi alterada, seguindo fluxo normal',
-            );
+        if (!deteccao.alterou)
+        {
+            console.log("â„¹ï¸ [ProcessarData] Data não foi alterada, seguindo fluxo normal");
             return {
                 sucesso: true,
                 agendamentoFinal: agendamentoEditado,
-                precisaRecarregar: false,
+                precisaRecarregar: false
             };
         }
 
-        const status = agendamentoOriginal?.status || '';
-        if (status !== 'Aberta' && status !== 'Agendada') {
-            console.warn(
-                'âš ï¸ [ProcessarData] Status não permite alteração de data:',
-                status,
-            );
-            AppToast.show(
-                'Amarelo',
-                "Não é possível alterar a data de viagens com status '" +
-                    status +
-                    "'",
-                3000,
-            );
+        const status = agendamentoOriginal?.status || "";
+        if (status !== "Aberta" && status !== "Agendada")
+        {
+            console.warn("âš ï¸ [ProcessarData] Status não permite alteração de data:", status);
+            AppToast.show("Amarelo", "Não é possível alterar a data de viagens com status '" + status + "'", 3000);
             return {
                 sucesso: false,
                 agendamentoFinal: null,
-                precisaRecarregar: false,
+                precisaRecarregar: false
             };
         }
 
-        const isRecorrente =
-            agendamentoOriginal?.recorrente === 'S' ||
-            agendamentoOriginal?.recorrente === 'M' ||
-            agendamentoOriginal?.recorrente === 'Q' ||
-            agendamentoOriginal?.recorrente === 'D';
-        const intervalo = agendamentoOriginal?.intervalo || '';
-        const recorrenciaViagemId =
-            agendamentoOriginal?.recorrenciaViagemId || '';
-
-        if (!isRecorrente || intervalo === 'V') {
-            console.log(
-                'â„¹ï¸ [ProcessarData] Não é recorrente ou é variada, permitindo alteração direta',
-            );
+        const isRecorrente = agendamentoOriginal?.recorrente === "S" || agendamentoOriginal?.recorrente === "M" ||
+            agendamentoOriginal?.recorrente === "Q" || agendamentoOriginal?.recorrente === "D";
+        const intervalo = agendamentoOriginal?.intervalo || "";
+        const recorrenciaViagemId = agendamentoOriginal?.recorrenciaViagemId || "";
+
+        if (!isRecorrente || intervalo === "V")
+        {
+            console.log("â„¹ï¸ [ProcessarData] Não é recorrente ou é variada, permitindo alteração direta");
             return {
                 sucesso: true,
                 agendamentoFinal: agendamentoEditado,
-                precisaRecarregar: false,
+                precisaRecarregar: false
             };
         }
 
-        console.log(
-            'â“ [ProcessarData] Ã‰ recorrente, perguntando ao usuário...',
-        );
-
-        const dataOriginalFormatada =
-            deteccao.dataOriginal.toLocaleDateString('pt-BR');
+        console.log("â“ [ProcessarData] Ã‰ recorrente, perguntando ao usuário...");
+
+        const dataOriginalFormatada = deteccao.dataOriginal.toLocaleDateString('pt-BR');
         const dataNovaFormatada = deteccao.dataNova.toLocaleDateString('pt-BR');
 
-        const escolha = await perguntarAlteracaoRecorrente(
-            dataOriginalFormatada,
-            dataNovaFormatada,
-        );
-
-        console.log('âœ… [ProcessarData] Escolha do usuário:', escolha);
-
-        if (escolha === 'cancelar') {
-
-            console.log('ðŸš« [ProcessarData] Operação cancelada pelo usuário');
+        const escolha = await perguntarAlteracaoRecorrente(dataOriginalFormatada, dataNovaFormatada);
+
+        console.log("âœ… [ProcessarData] Escolha do usuário:", escolha);
+
+        if (escolha === "cancelar")
+        {
+
+            console.log("ðŸš« [ProcessarData] Operação cancelada pelo usuário");
             return {
                 sucesso: false,
                 agendamentoFinal: null,
-                precisaRecarregar: false,
+                precisaRecarregar: false
             };
         }
 
-        if (escolha === 'apenas_este') {
-
-            console.log(
-                'âœï¸ [ProcessarData] Alterando apenas este agendamento',
-            );
+        if (escolha === "apenas_este")
+        {
+
+            console.log("âœï¸ [ProcessarData] Alterando apenas este agendamento");
             return {
                 sucesso: true,
                 agendamentoFinal: agendamentoEditado,
-                precisaRecarregar: false,
+                precisaRecarregar: false
             };
         }
 
-        if (escolha === 'todos_subsequentes') {
-
-            console.log(
-                'ðŸ”„ [ProcessarData] Alterando este e aplicando push nos subsequentes',
-            );
+        if (escolha === "todos_subsequentes")
+        {
+
+            console.log("ðŸ”„ [ProcessarData] Alterando este e aplicando push nos subsequentes");
 
             const pushSucesso = await aplicarPushDatasSubsequentes(
                 recorrenciaViagemId,
                 deteccao.dataOriginal,
                 deteccao.dataNova,
                 intervalo,
-                deteccao.dataOriginal,
+                deteccao.dataOriginal
             );
 
-            if (pushSucesso) {
-                console.log('âœ… [ProcessarData] Push aplicado com sucesso');
-                AppToast.show(
-                    'Verde',
-                    'Data inicial atualizada em todos os agendamentos subsequentes',
-                    3000,
-                );
-            } else {
-                console.warn(
-                    'âš ï¸ [ProcessarData] Push teve erros, mas prosseguindo',
-                );
-                AppToast.show(
-                    'Amarelo',
-                    'Alguns agendamentos não puderam ser atualizados',
-                    3000,
-                );
+            if (pushSucesso)
+            {
+                console.log("âœ… [ProcessarData] Push aplicado com sucesso");
+                AppToast.show("Verde", "Data inicial atualizada em todos os agendamentos subsequentes", 3000);
+            } else
+            {
+                console.warn("âš ï¸ [ProcessarData] Push teve erros, mas prosseguindo");
+                AppToast.show("Amarelo", "Alguns agendamentos não puderam ser atualizados", 3000);
             }
 
             return {
                 sucesso: true,
                 agendamentoFinal: agendamentoEditado,
-                precisaRecarregar: true,
+                precisaRecarregar: true
             };
         }
 
-        console.warn('âš ï¸ [ProcessarData] Escolha não reconhecida:', escolha);
+        console.warn("âš ï¸ [ProcessarData] Escolha não reconhecida:", escolha);
         return {
             sucesso: false,
             agendamentoFinal: null,
-            precisaRecarregar: false,
+            precisaRecarregar: false
         };
-    } catch (error) {
-        console.error('âŒ [ProcessarData] Erro:', error);
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'processarAlteracaoDataInicial',
-            error,
-        );
+    } catch (error)
+    {
+        console.error("âŒ [ProcessarData] Erro:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "processarAlteracaoDataInicial", error);
         return {
             sucesso: false,
             agendamentoFinal: null,
-            precisaRecarregar: false,
+            precisaRecarregar: false
         };
     }
 }
 
-window.editarAgendamento = async function (viagemId) {
-    try {
-        if (!viagemId) {
-            throw new Error('ViagemId é obrigatório.');
-        }
-
-        try {
-
-            const agendamentoBase =
-                await window.recuperarViagemEdicao(viagemId);
-
-            if (!agendamentoBase) {
-                throw new Error('Agendamento inexistente.');
-            }
-
-            const agendamentoEditado =
-                window.criarAgendamentoEdicao(agendamentoBase);
-
-            const resultadoProcessamento = await processarAlteracaoDataInicial(
-                agendamentoBase,
-                agendamentoEditado,
-            );
-
-            if (!resultadoProcessamento.sucesso) {
-                console.log('ðŸš« [EditarAgendamento] Operação não prosseguiu');
+window.editarAgendamento = async function (viagemId)
+{
+    try
+    {
+        if (!viagemId)
+        {
+            throw new Error("ViagemId é obrigatório.");
+        }
+
+        try
+        {
+
+            const agendamentoBase = await window.recuperarViagemEdicao(viagemId);
+
+            if (!agendamentoBase)
+            {
+                throw new Error("Agendamento inexistente.");
+            }
+
+            const agendamentoEditado = window.criarAgendamentoEdicao(agendamentoBase);
+
+            const resultadoProcessamento = await processarAlteracaoDataInicial(agendamentoBase, agendamentoEditado);
+
+            if (!resultadoProcessamento.sucesso)
+            {
+                console.log("ðŸš« [EditarAgendamento] Operação não prosseguiu");
                 return;
             }
 
             const agendamentoFinal = resultadoProcessamento.agendamentoFinal;
 
-            if (await window.ValidaCampos(agendamentoFinal.ViagemId)) {
-                const response = await fetch('/api/Agenda/Agendamento', {
-                    method: 'POST',
+            if (await window.ValidaCampos(agendamentoFinal.ViagemId))
+            {
+                const response = await fetch("/api/Agenda/Agendamento", {
+                    method: "POST",
                     headers: {
-                        'Content-Type': 'application/json',
+                        "Content-Type": "application/json"
                     },
-                    body: JSON.stringify(agendamentoFinal),
+                    body: JSON.stringify(agendamentoFinal)
                 });
 
-                let tipoAgendamento = 'Viagem';
-                if (agendamentoFinal.Status === 'Aberta') {
-                    tipoAgendamento = 'Viagem';
-                } else {
-                    tipoAgendamento = 'Agendamento';
+                let tipoAgendamento = "Viagem";
+                if (agendamentoFinal.Status === "Aberta")
+                {
+                    tipoAgendamento = "Viagem";
+                } else
+                {
+                    tipoAgendamento = "Agendamento";
                 }
 
                 const resultado = await response.json();
 
-                if (resultado.success) {
-                    AppToast.show(
-                        'Verde',
-                        tipoAgendamento + ' atualizado com sucesso!',
-                        2000,
-                    );
-
-                    $('#modalViagens').modal('hide');
-                } else {
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao atualizar ' + tipoAgendamento,
-                        2000,
-                    );
+                if (resultado.success)
+                {
+                    AppToast.show("Verde", tipoAgendamento + " atualizado com sucesso!", 2000);
+
+                    $("#modalViagens").modal("hide");
+                    $(document.body).removeClass("modal-open");
+                    $(".modal-backdrop").remove();
+                    $(document.body).css("overflow", "");
+                } else
+                {
+                    AppToast.show("Vermelho", "Erro ao atualizar " + tipoAgendamento, 2000);
                 }
 
-                if (window.calendar?.refetchEvents) {
+                if (window.calendar?.refetchEvents)
+                {
                     window.calendar.refetchEvents();
                 }
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'modal-viagem.js',
-                'editarAgendamento_inner',
-                error,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'editarAgendamento',
-            error,
-        );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamento_inner", error);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamento", error);
     }
 };
 
-window.editarAgendamentoRecorrente = async function (
-    viagemId,
-    editaTodos,
-    dataInicialRecorrencia,
-    recorrenciaViagemId,
-    editarAgendamentoRecorrente,
-) {
-    try {
-
-        const isSameOrAfterDay = (left, right) => {
-            try {
+window.editarAgendamentoRecorrente = async function (viagemId, editaTodos, dataInicialRecorrencia, recorrenciaViagemId, editarAgendamentoRecorrente)
+{
+    try
+    {
+
+        const isSameOrAfterDay = (left, right) =>
+        {
+            try
+            {
                 const L = window.toLocalDateOnly(left);
                 const R = window.toLocalDateOnly(right);
                 if (!L || !R) return false;
                 return L.getTime() >= R.getTime();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'isSameOrAfterDay',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "isSameOrAfterDay", error);
                 return false;
             }
         };
 
-        let mensagemToastRecorrencia = null;
-        let corToastRecorrencia = 'Verde';
-
-        const fecharModalComSucesso = () => {
-            try {
-
-                $('#modalViagens').modal('hide');
-                if (window.calendar?.refetchEvents)
-                    window.calendar.refetchEvents();
-
-                if (window._recorrenciaOverlayAtivo) {
-                    setTimeout(() => {
-                        try {
-                            FtxSpin.hide();
-                            window._recorrenciaOverlayAtivo = false;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'modal-viagem.js',
-                                'fecharModalComSucesso_hideOverlay',
-                                error,
-                            );
-                        }
-                    }, 200);
+        const fecharModalComSucesso = () =>
+        {
+            try
+            {
+                try
+                {
+                    $("#modalViagens").modal("hide");
+                } catch { }
+                $(".modal-backdrop").remove();
+                $("body").removeClass("modal-open").css("overflow", "");
+                if (window.calendar?.refetchEvents) window.calendar.refetchEvents();
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "fecharModalComSucesso", error);
+            }
+        };
+
+        try
+        {
+            if (!viagemId) throw new Error("ViagemId não fornecido.");
+
+            let houveSucesso = false;
+
+            if (editaTodos)
+            {
+
+                if (recorrenciaViagemId === "00000000-0000-0000-0000-000000000000" || !recorrenciaViagemId)
+                {
+                    recorrenciaViagemId = viagemId;
+                    const [primeiroDaSerie = {}] = await window.obterAgendamentosRecorrenteInicial(viagemId);
+                    let objViagem = window.criarAgendamentoEdicao(primeiroDaSerie);
+
+                    objViagem.editarTodosRecorrentes = true;
+                    objViagem.editarAPartirData = dataInicialRecorrencia;
+                    const ok = await window.aplicarAtualizacao(objViagem);
+                    houveSucesso = houveSucesso || ok;
                 }
 
-                if (mensagemToastRecorrencia) {
-                    setTimeout(() => {
-                        try {
-                            AppToast.show(
-                                corToastRecorrencia,
-                                mensagemToastRecorrencia,
-                                4000,
-                            );
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'modal-viagem.js',
-                                'fecharModalComSucesso_toast',
-                                error,
-                            );
-                        }
-                    }, 300);
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'fecharModalComSucesso',
-                    error,
-                );
-            }
-        };
-
-        try {
-            if (!viagemId) throw new Error('ViagemId não fornecido.');
-
-            let houveSucesso = false;
-            let totalTentativas = 0;
-            let totalAtualizados = 0;
-            const registrosProcessados = new Set();
-
-            if (editaTodos) {
-
-                if (
-                    recorrenciaViagemId ===
-                        '00000000-0000-0000-0000-000000000000' ||
-                    !recorrenciaViagemId
-                ) {
-                    recorrenciaViagemId = viagemId;
-                    const [primeiroDaSerie = {}] =
-                        await window.obterAgendamentosRecorrenteInicial(
-                            viagemId,
-                        );
-                    const chavePrimeiro =
-                        primeiroDaSerie?.viagemId ||
-                        primeiroDaSerie?.ViagemId ||
-                        primeiroDaSerie?.id ||
-                        primeiroDaSerie?.Id ||
-                        `sem-id-${registrosProcessados.size + 1}`;
-
-                    if (!registrosProcessados.has(chavePrimeiro)) {
-                        registrosProcessados.add(chavePrimeiro);
-                        totalTentativas++;
-
-                        let objViagem =
-                            window.criarAgendamentoEdicao(primeiroDaSerie);
-
-                        objViagem.editarTodosRecorrentes = true;
-                        objViagem.editarAPartirData = dataInicialRecorrencia;
+                const agendamentos = await window.obterAgendamentosRecorrentes(recorrenciaViagemId);
+                for (const agendamentoRecorrente of agendamentos)
+                {
+                    if (isSameOrAfterDay(agendamentoRecorrente.dataInicial, dataInicialRecorrencia))
+                    {
+                        let objViagem = window.criarAgendamentoEdicao(agendamentoRecorrente);
                         const ok = await window.aplicarAtualizacao(objViagem);
                         houveSucesso = houveSucesso || ok;
-
-                        if (ok) totalAtualizados++;
                     }
                 }
-
-                const agendamentos =
-                    await window.obterAgendamentosRecorrentes(
-                        recorrenciaViagemId,
-                    );
-                for (const agendamentoRecorrente of agendamentos) {
-                    if (
-                        isSameOrAfterDay(
-                            agendamentoRecorrente.dataInicial,
-                            dataInicialRecorrencia,
-                        )
-                    ) {
-                        const chaveAtual =
-                            agendamentoRecorrente?.viagemId ||
-                            agendamentoRecorrente?.ViagemId ||
-                            agendamentoRecorrente?.id ||
-                            agendamentoRecorrente?.Id ||
-                            `sem-id-${registrosProcessados.size + 1}`;
-
-                        if (!registrosProcessados.has(chaveAtual)) {
-                            registrosProcessados.add(chaveAtual);
-                            totalTentativas++;
-
-                            let objViagem = window.criarAgendamentoEdicao(
-                                agendamentoRecorrente,
-                            );
-                            const ok =
-                                await window.aplicarAtualizacao(objViagem);
-                            houveSucesso = houveSucesso || ok;
-
-                            if (ok) totalAtualizados++;
-                        }
-                    }
-                }
-            } else {
-
-                const agendamentoUnicoAlterado =
-                    await window.recuperarViagemEdicao(viagemId);
-                let objViagem = window.criarAgendamentoEdicao(
-                    agendamentoUnicoAlterado,
-                );
+            } else
+            {
+
+                const agendamentoUnicoAlterado = await window.recuperarViagemEdicao(viagemId);
+                let objViagem = window.criarAgendamentoEdicao(agendamentoUnicoAlterado);
                 const ok = await window.aplicarAtualizacao(objViagem);
                 houveSucesso = houveSucesso || ok;
             }
 
-            if (editaTodos) {
-                if (totalTentativas > 0) {
-                    if (totalAtualizados === totalTentativas) {
-                        mensagemToastRecorrencia = `${totalAtualizados} agendamento(s) recorrente(s) atualizado(s) com sucesso.`;
-                    } else if (totalAtualizados > 0) {
-                        mensagemToastRecorrencia = `${totalAtualizados} de ${totalTentativas} agendamentos recorrentes atualizados.`;
-                        corToastRecorrencia = 'Amarelo';
-                    } else {
-                        mensagemToastRecorrencia =
-                            'Nenhum agendamento recorrente foi atualizado.';
-                        corToastRecorrencia = 'Vermelho';
-                    }
-                }
-            }
-
-            if (houveSucesso) {
-                fecharModalComSucesso();
-            } else if (window._recorrenciaOverlayAtivo) {
-                try {
-                    FtxSpin.hide();
-                    window._recorrenciaOverlayAtivo = false;
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'modal-viagem.js',
-                        'editarAgendamentoRecorrente_hideOverlay',
-                        error,
-                    );
-                }
-            }
-        } catch (error) {
-            if (window._recorrenciaOverlayAtivo) {
-                try {
-                    FtxSpin.hide();
-                    window._recorrenciaOverlayAtivo = false;
-                } catch (innerError) {
-                    Alerta.TratamentoErroComLinha(
-                        'modal-viagem.js',
-                        'editarAgendamentoRecorrente_hideOverlay',
-                        innerError,
-                    );
-                }
-            }
-            Alerta.TratamentoErroComLinha(
-                'modal-viagem.js',
-                'editarAgendamentoRecorrente_inner',
-                error,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'editarAgendamentoRecorrente',
-            error,
-        );
+            if (houveSucesso) fecharModalComSucesso();
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamentoRecorrente_inner", error);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamentoRecorrente", error);
     }
 };
 
-window.exibirMensagemSucesso = function (quantidade = 1) {
-    try {
-
-        let mensagemToast, tituloAlerta, mensagemAlerta;
-
-        if (quantidade === 1) {
-            mensagemToast = 'Agendamento criado com sucesso!';
-            tituloAlerta = 'Agendamento Criado';
-            mensagemAlerta = 'O agendamento foi criado com sucesso.';
-        } else {
-            mensagemToast = `${quantidade} agendamentos criados com sucesso!`;
-            tituloAlerta = 'Agendamentos Criados';
-            mensagemAlerta = `Foram criados <strong>${quantidade}</strong> agendamentos com sucesso.`;
-        }
-
-        AppToast.show('Verde', mensagemToast, 3000);
-        Alerta.Sucesso(tituloAlerta, mensagemAlerta);
-
-        $('#modalViagens').modal('hide');
+window.exibirMensagemSucesso = function ()
+{
+    try
+    {
+        AppToast.show("Verde", "Todos os agendamentos foram criados com sucesso", 3000);
+        Alerta.Sucesso("Agendamento criado com sucesso", "Todos os agendamentos foram criados com sucesso");
+        $("#modalViagens").modal("hide");
+        $(document.body).removeClass("modal-open");
+        $(".modal-backdrop").remove();
+        $(document.body).css("overflow", "");
         window.calendar.refetchEvents();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'exibirMensagemSucesso',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "exibirMensagemSucesso", error);
     }
 };
 
-window.exibirErroAgendamento = function () {
-    try {
-        AppToast.show(
-            'Vermelho',
-            'Não foi possível criar o agendamento com os dados informados',
-            3000,
-        );
-        Alerta.Erro(
-            'Erro ao criar agendamento',
-            'Não foi possível criar o agendamento com os dados informados',
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'exibirErroAgendamento',
-            error,
-        );
+window.exibirErroAgendamento = function ()
+{
+    try
+    {
+        AppToast.show("Vermelho", "Não foi possível criar o agendamento com os dados informados", 3000);
+        Alerta.Erro("Erro ao criar agendamento", "Não foi possível criar o agendamento com os dados informados");
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "exibirErroAgendamento", error);
     }
 };
 
-window.handleAgendamentoError = function (error) {
-    try {
+window.handleAgendamentoError = function (error)
+{
+    try
+    {
         window.exibirErroAgendamento();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'handleAgendamentoError',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "handleAgendamentoError", error);
     }
 };
 
-window.carregarRelatorioNoModal = function () {
-    try {
-        console.log(
-            'ðŸ“Š [ModalViagem] ===== INICIANDO CARREGAMENTO DE RELAtÓRIO =====',
-        );
-
-        const viagemId =
-            window.State?.get('viagemAtual')?.viagemId ||
+window.carregarRelatorioNoModal = function ()
+{
+    try
+    {
+        console.log("ðŸ“Š [ModalViagem] ===== INICIANDO CARREGAMENTO DE RELAtÓRIO =====");
+
+        const viagemId = window.State?.get('viagemAtual')?.viagemId ||
             $('#txtViagemIdRelatorio').val() ||
             $('#txtViagemId').val() ||
             window.currentViagemId ||
             window.viagemId;
 
-        console.log('ðŸ” [ModalViagem] Fontes de ViagemId:', {
+        console.log("ðŸ” [ModalViagem] Fontes de ViagemId:", {
             state: window.State?.get('viagemAtual')?.viagemId,
             txtViagemIdRelatorio: $('#txtViagemIdRelatorio').val(),
             txtViagemId: $('#txtViagemId').val(),
             currentViagemId: window.currentViagemId,
             viagemId: window.viagemId,
-            final: viagemId,
+            final: viagemId
         });
 
-        if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000') {
-            console.error(
-                'âŒ [ModalViagem] ViagemId não encontrado ou inválido:',
-                viagemId,
-            );
-
-            if (typeof AppToast !== 'undefined') {
+        if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000')
+        {
+            console.error("âŒ [ModalViagem] ViagemId não encontrado ou inválido:", viagemId);
+
+            if (typeof AppToast !== 'undefined')
+            {
                 AppToast.show('Amarelo', 'ID da viagem não identificado', 3000);
             }
 
             return;
         }
 
-        console.log('âœ… [ModalViagem] ViagemId válido encontrado:', viagemId);
-
-        if (typeof window.carregarRelatorioViagem !== 'function') {
-            console.error(
-                'âŒ [ModalViagem] Função carregarRelatorioViagem não encontrada!',
-            );
-            console.error(' Verifique se relatorio.js está carregado');
-
-            if (typeof AppToast !== 'undefined') {
-                AppToast.show(
-                    'Vermelho',
-                    'Módulo de relatório não carregado',
-                    3000,
-                );
+        console.log("âœ… [ModalViagem] ViagemId válido encontrado:", viagemId);
+
+        if (typeof window.carregarRelatorioViagem !== 'function')
+        {
+            console.error("âŒ [ModalViagem] Função carregarRelatorioViagem não encontrada!");
+            console.error(" Verifique se relatorio.js está carregado");
+
+            if (typeof AppToast !== 'undefined')
+            {
+                AppToast.show('Vermelho', 'Módulo de relatório não carregado', 3000);
             }
 
             return;
         }
 
-        console.log('âœ… [ModalViagem] Módulo de relatório encontrado');
+        console.log("âœ… [ModalViagem] Módulo de relatório encontrado");
 
         const reportContainer = document.getElementById('reportViewerAgenda');
-        if (!reportContainer) {
-            console.error(
-                'âŒ [ModalViagem] Container #reportViewerAgenda não encontrado no DOM',
-            );
-
-            if (typeof AppToast !== 'undefined') {
-                AppToast.show(
-                    'Vermelho',
-                    'Container do relatório não encontrado',
-                    3000,
-                );
+        if (!reportContainer)
+        {
+            console.error("âŒ [ModalViagem] Container #reportViewerAgenda não encontrado no DOM");
+
+            if (typeof AppToast !== 'undefined')
+            {
+                AppToast.show('Vermelho', 'Container do relatório não encontrado', 3000);
             }
 
             return;
         }
 
-        console.log('âœ… [ModalViagem] Container do relatório encontrado');
+        console.log("âœ… [ModalViagem] Container do relatório encontrado");
 
         const cardRelatorio = $('#cardRelatorio');
         const reportContainerDiv = $('#ReportContainerAgenda');
 
-        if (cardRelatorio.length > 0) {
-            console.log('ðŸ“º [ModalViagem] Exibindo card do relatório');
+        if (cardRelatorio.length > 0)
+        {
+            console.log("ðŸ“º [ModalViagem] Exibindo card do relatório");
             cardRelatorio.slideDown(300);
         }
 
-        if (reportContainerDiv.length > 0) {
-            console.log('ðŸ“º [ModalViagem] Exibindo container do relatório');
+        if (reportContainerDiv.length > 0)
+        {
+            console.log("ðŸ“º [ModalViagem] Exibindo container do relatório");
             reportContainerDiv.slideDown(300);
         }
 
-        setTimeout(() => {
-            console.log(
-                'ðŸš€ [ModalViagem] Chamando carregarRelatorioViagem com ViagemId:',
-                viagemId,
-            );
+        setTimeout(() =>
+        {
+            console.log("ðŸš€ [ModalViagem] Chamando carregarRelatorioViagem com ViagemId:", viagemId);
 
             const card = document.getElementById('cardRelatorio');
-            if (card) {
+            if (card)
+            {
                 card.scrollIntoView({
                     behavior: 'smooth',
-                    block: 'start',
+                    block: 'start'
                 });
             }
 
-            window
-                .carregarRelatorioViagem(viagemId)
-                .then(() => {
-                    console.log(
-                        'âœ… [ModalViagem] Relatório carregado com sucesso',
-                    );
-
-                    if (typeof AppToast !== 'undefined') {
-                        AppToast.show(
-                            'Verde',
-                            'Relatório carregado com sucesso',
-                            2000,
-                        );
+            window.carregarRelatorioViagem(viagemId)
+                .then(() =>
+                {
+                    console.log("âœ… [ModalViagem] Relatório carregado com sucesso");
+
+                    if (typeof AppToast !== 'undefined')
+                    {
+                        AppToast.show('Verde', 'Relatório carregado com sucesso', 2000);
                     }
                 })
-                .catch((error) => {
-                    console.error(
-                        'âŒ [ModalViagem] Erro ao carregar relatório:',
-                        error,
-                    );
-
-                    if (typeof AppToast !== 'undefined') {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar relatório: ' + error.message,
-                            3000,
-                        );
+                .catch((error) =>
+                {
+                    console.error("âŒ [ModalViagem] Erro ao carregar relatório:", error);
+
+                    if (typeof AppToast !== 'undefined')
+                    {
+                        AppToast.show('Vermelho', 'Erro ao carregar relatório: ' + error.message, 3000);
                     }
                 });
         }, 500);
-    } catch (error) {
-        console.error(
-            'âŒ [ModalViagem] Erro crí­tico em carregarRelatorioNoModal:',
-            error,
-        );
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'carregarRelatorioNoModal',
-            error,
-        );
-
-        if (typeof AppToast !== 'undefined') {
+    } catch (error)
+    {
+        console.error("âŒ [ModalViagem] Erro crí­tico em carregarRelatorioNoModal:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "carregarRelatorioNoModal", error);
+
+        if (typeof AppToast !== 'undefined')
+        {
             AppToast.show('Vermelho', 'Erro ao inicializar relatório', 3000);
         }
     }
@@ -2095,108 +1646,106 @@
 
 window.ultimoViagemIdCarregado = null;
 
-function aoAbrirModalViagem(event) {
-    try {
-        console.log('ðŸ“‚ [ModalViagem] ===== MODAL ABERTO =====');
+function aoAbrirModalViagem(event)
+{
+    try
+    {
+        console.log("ðŸ“‚ [ModalViagem] ===== MODAL ABERTO =====");
 
         window.modalJaFoiLimpo = false;
         window.ignorarEventosRecorrencia = false;
 
-        const viagemId =
-            $('#txtViagemId').val() ||
+        const viagemId = $('#txtViagemId').val() ||
             $('#txtViagemIdRelatorio').val() ||
             window.currentViagemId;
 
-        console.log('ðŸ“‹ [ModalViagem] ViagemId encontrado:', viagemId);
-        console.log(
-            'ðŸ“‹ [ModalViagem] Último ViagemId carregado:',
-            window.ultimoViagemIdCarregado,
-        );
-
-        if (
-            viagemId &&
-            viagemId !== '' &&
-            viagemId !== '00000000-0000-0000-0000-000000000000'
-        ) {
-
-            if (viagemId !== window.ultimoViagemIdCarregado) {
-                console.log(
-                    'ðŸ“Š [ModalViagem] ViagemId diferente, recarregando relatório...',
-                );
-
-                if (typeof destruirViewerAnterior === 'function') {
-                    destruirViewerAnterior().then(() => {
-
-                        setTimeout(() => {
-                            if (
-                                typeof window.carregarRelatorioViagem ===
-                                'function'
-                            ) {
+        console.log("ðŸ“‹ [ModalViagem] ViagemId encontrado:", viagemId);
+        console.log("ðŸ“‹ [ModalViagem] Último ViagemId carregado:", window.ultimoViagemIdCarregado);
+
+        if (viagemId && viagemId !== "" && viagemId !== "00000000-0000-0000-0000-000000000000")
+        {
+
+            if (viagemId !== window.ultimoViagemIdCarregado)
+            {
+                console.log("ðŸ“Š [ModalViagem] ViagemId diferente, recarregando relatório...");
+
+                if (typeof destruirViewerAnterior === 'function')
+                {
+                    destruirViewerAnterior().then(() =>
+                    {
+
+                        setTimeout(() =>
+                        {
+                            if (typeof window.carregarRelatorioViagem === 'function')
+                            {
                                 window.carregarRelatorioViagem(viagemId);
-                                $('#cardRelatorio').show();
+                                $("#cardRelatorio").show();
                                 window.ultimoViagemIdCarregado = viagemId;
                             }
                         }, 300);
                     });
-                } else {
-
-                    setTimeout(() => {
-                        if (
-                            typeof window.carregarRelatorioViagem === 'function'
-                        ) {
+                } else
+                {
+
+                    setTimeout(() =>
+                    {
+                        if (typeof window.carregarRelatorioViagem === 'function')
+                        {
                             window.carregarRelatorioViagem(viagemId);
-                            $('#cardRelatorio').show();
+                            $("#cardRelatorio").show();
                             window.ultimoViagemIdCarregado = viagemId;
                         }
                     }, 500);
                 }
-            } else {
-                console.log(
-                    'ðŸ“Š [ModalViagem] Mesmo ViagemId, mantendo relatório atual',
-                );
-            }
-        } else {
-            console.log(
-                'â„¹ï¸ [ModalViagem] Novo agendamento - não carregar relatório',
-            );
+            } else
+            {
+                console.log("ðŸ“Š [ModalViagem] Mesmo ViagemId, mantendo relatório atual");
+            }
+        } else
+        {
+            console.log("â„¹ï¸ [ModalViagem] Novo agendamento - não carregar relatório");
             $('#cardRelatorio').hide();
             window.ultimoViagemIdCarregado = null;
         }
 
-        setTimeout(() => {
-            if (typeof inicializarSistemaRequisitante === 'function') {
+        setTimeout(() =>
+        {
+            if (typeof inicializarSistemaRequisitante === 'function')
+            {
                 inicializarSistemaRequisitante();
             }
         }, 500);
-    } catch (error) {
-        console.error('âŒ [ModalViagem] Erro ao abrir modal:', error);
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'aoAbrirModalViagem',
-            error,
-        );
+    } catch (error)
+    {
+        console.error("âŒ [ModalViagem] Erro ao abrir modal:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "aoAbrirModalViagem", error);
     }
 }
 
-function aoFecharModalViagem() {
-    try {
-        console.log('ðŸšª [ModalViagem] ===== MODAL FECHANDO =====');
-
-        if (typeof window.limparRelatorio === 'function') {
+function aoFecharModalViagem()
+{
+    try
+    {
+        console.log("ðŸšª [ModalViagem] ===== MODAL FECHANDO =====");
+
+        if (typeof window.limparRelatorio === 'function')
+        {
             window.limparRelatorio();
         }
 
         window.ignorarEventosRecorrencia = false;
         window.carregandoViagemExistente = false;
 
-        if (window.timeoutAbrirModal) {
+        if (window.timeoutAbrirModal)
+        {
             clearTimeout(window.timeoutAbrirModal);
             window.timeoutAbrirModal = null;
         }
 
-        if (typeof window.limparCamposModalViagens === 'function') {
+        if (typeof window.limparCamposModalViagens === 'function')
+        {
             window.limparCamposModalViagens();
-            console.log('Campos limpos ao fechar modal');
+            console.log("Campos limpos ao fechar modal");
         }
 
         window.modalJaFoiLimpo = false;
@@ -2204,29 +1753,29 @@
         window.currentViagemId = null;
         window.ultimoViagemIdCarregado = null;
 
-        console.log('Modal fechado e limpo');
-        console.log('âœ… [ModalViagem] Modal fechado e limpo');
-    } catch (error) {
-        console.error('âŒ [ModalViagem] Erro ao fechar modal:', error);
+        console.log("Modal fechado e limpo");
+        console.log("âœ… [ModalViagem] Modal fechado e limpo");
+    } catch (error)
+    {
+        console.error("âŒ [ModalViagem] Erro ao fechar modal:", error);
     }
 }
 
-function inicializarEventosRelatorioModal() {
-    try {
-        console.log(
-            'ðŸŽ¬ [ModalViagem] ===== INICIALIZANDO EVENTOS DE RELAtÓRIO =====',
-        );
+function inicializarEventosRelatorioModal()
+{
+    try
+    {
+        console.log("ðŸŽ¬ [ModalViagem] ===== INICIALIZANDO EVENTOS DE RELAtÓRIO =====");
 
         const $modal = $('#modalViagens');
 
-        if ($modal.length === 0) {
-            console.warn(
-                'âš ï¸ [ModalViagem] Modal #modalViagens não encontrado no DOM',
-            );
+        if ($modal.length === 0)
+        {
+            console.warn("âš ï¸ [ModalViagem] Modal #modalViagens não encontrado no DOM");
             return;
         }
 
-        console.log('âœ… [ModalViagem] Modal #modalViagens encontrado');
+        console.log("âœ… [ModalViagem] Modal #modalViagens encontrado");
 
         $modal.off('shown.bs.modal', aoAbrirModalViagem);
         $modal.off('hidden.bs.modal', aoFecharModalViagem);
@@ -2234,797 +1783,643 @@
         $modal.on('shown.bs.modal', aoAbrirModalViagem);
         $modal.on('hidden.bs.modal', aoFecharModalViagem);
 
-        console.log(
-            'âœ… [ModalViagem] Eventos de relatório inicializados com sucesso',
-        );
-        console.log(' - shown.bs.modal â†’ aoAbrirModalViagem');
-        console.log(' - hidden.bs.modal â†’ aoFecharModalViagem');
-    } catch (error) {
-        console.error('âŒ [ModalViagem] Erro ao inicializar eventos:', error);
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'inicializarEventosRelatorioModal',
-            error,
-        );
+        console.log("âœ… [ModalViagem] Eventos de relatório inicializados com sucesso");
+        console.log(" - shown.bs.modal â†’ aoAbrirModalViagem");
+        console.log(" - hidden.bs.modal â†’ aoFecharModalViagem");
+    } catch (error)
+    {
+        console.error("âŒ [ModalViagem] Erro ao inicializar eventos:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "inicializarEventosRelatorioModal", error);
     }
 }
 
 window.carregarRelatorioNoModal = carregarRelatorioNoModal;
 
-$(function () {
-    console.log('ðŸŽ¬ [ModalViagem] ===== DOCUMENTO PRONTO =====');
-    console.log('ðŸŽ¬ [ModalViagem] Inicializando eventos de relatório...');
+$(function ()
+{
+    console.log("ðŸŽ¬ [ModalViagem] ===== DOCUMENTO PRONTO =====");
+    console.log("ðŸŽ¬ [ModalViagem] Inicializando eventos de relatório...");
     inicializarEventosRelatorioModal();
 
-    const configurarValidacaoDataFinal = function () {
-        try {
-            var kendoDataFinalPicker =
-                $('#txtDataFinal').data('kendoDatePicker');
-            if (
-                kendoDataFinalPicker &&
-                !kendoDataFinalPicker._dataFinalValidacaoConfigurada
-            ) {
-
-                kendoDataFinalPicker.bind('change', function (e) {
-                    try {
-                        var dataFinalValue = this.value();
-                        if (dataFinalValue) {
+    const configurarValidacaoDataFinal = function ()
+    {
+        try
+        {
+            const datePicker = window.getKendoDatePicker("txtDataFinal");
+            if (datePicker)
+            {
+                if (!datePicker._dataFinalValidacaoConfigurada)
+                {
+                    datePicker.bind("change", function ()
+                    {
+                        try
+                        {
+                            const dataFinalValue = datePicker.value();
+                            if (dataFinalValue)
+                            {
+                                const dataFinal = new Date(dataFinalValue);
+                                dataFinal.setHours(0, 0, 0, 0);
+                                const hoje = new Date();
+                                hoje.setHours(0, 0, 0, 0);
+
+                                if (dataFinal > hoje)
+                                {
+                                    datePicker.value(null);
+                                    AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
+                                }
+                            }
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("modal-viagem.js", "txtDataFinal.change", error);
+                        }
+                    });
+                    datePicker._dataFinalValidacaoConfigurada = true;
+                    console.log("✅ [ModalViagem] Validação de Data Final configurada (Kendo)");
+                }
+                return;
+            }
+
+            const txtDataFinal = document.getElementById("txtDataFinal");
+            if (txtDataFinal && !txtDataFinal._dataFinalValidacaoConfigurada)
+            {
+                txtDataFinal.addEventListener("blur", function ()
+                {
+                    try
+                    {
+                        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
+                        if (dataFinalValue)
+                        {
                             const dataFinal = new Date(dataFinalValue);
                             dataFinal.setHours(0, 0, 0, 0);
                             const hoje = new Date();
                             hoje.setHours(0, 0, 0, 0);
 
-                            if (dataFinal > hoje) {
-                                this.value(null);
-                                AppToast.show(
-                                    'Amarelo',
-                                    'A Data Final nao pode ser superior a data atual.',
-                                    4000,
-                                );
+                            if (dataFinal > hoje)
+                            {
+                                window.setKendoDateValue("txtDataFinal", null);
+                                AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
                             }
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'modal-viagem.js',
-                            'txtDataFinal.change',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("modal-viagem.js", "txtDataFinal.blur", error);
                     }
                 });
-                kendoDataFinalPicker._dataFinalValidacaoConfigurada = true;
-                console.log(
-                    '[ModalViagem] Validacao de Data Final configurada (change)',
-                );
-            }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'modal-viagem.js',
-                'configurarValidacaoDataFinal',
-                error,
-            );
+                txtDataFinal._dataFinalValidacaoConfigurada = true;
+            }
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("modal-viagem.js", "configurarValidacaoDataFinal", error);
         }
     };
+
+    $(document).on("shown.bs.modal", "#ModalViagem", function ()
+    {
+        setTimeout(configurarValidacaoDataFinal, 100);
+    });
 
     setTimeout(configurarValidacaoDataFinal, 500);
 });
 
-window.inicializarCamposModal = function () {
-    try {
-
-        const controlesEspeciais = [
-            'txtDataInicial',
-            'txtDataFinal',
-            'txtFinalRecorrencia',
-            'txtDataInicialEvento',
-            'txtDataFinalEvento',
-            'lstRecorrente',
-            'lstPeriodos',
-            'lstDias',
-            'lstDiasMes',
-            'lstMotorista',
-            'lstVeiculo',
-            'lstRequisitante',
-            'lstSetorRequisitanteAgendamento',
-            'lstFinalidade',
-            'lstEventos',
-            'rteDescricao',
-            'calDatasSelecionadas',
-            'lstDiasCalendario',
+window.inicializarCamposModal = function ()
+{
+    try
+    {
+
+        const divModal = document.getElementById("divModal");
+        if (divModal)
+        {
+            const childNodes = divModal.getElementsByTagName("*");
+            for (const node of childNodes)
+            {
+                if (node.id !== "divBotoes")
+                {
+                    node.disabled = false;
+                    node.value = "";
+                }
+            }
+        }
+
+        window.setKendoTimeValue("txtHoraInicial", "");
+        window.setKendoTimeValue("txtHoraFinal", "");
+
+        const camposViagem = [
+            "divNoFichaVistoria", "divDataFinal", "divHoraFinal", "divDuracao",
+            "divKmAtual", "divKmInicial", "divKmFinal", "divQuilometragem",
+            "divCombustivelInicial", "divCombustivelFinal"
         ];
 
-        const divModal = document.getElementById('divModal');
-        if (divModal) {
-            const childNodes = divModal.getElementsByTagName('*');
-            for (const node of childNodes) {
-
-                if (node.id === 'divBotoes') continue;
-
-                if (controlesEspeciais.includes(node.id)) continue;
-
-                if (
-                    node.className &&
-                    typeof node.className === 'string' &&
-                    (node.className.includes('k-') ||
-                        node.className.includes('e-'))
-                )
-                    continue;
-
-                node.disabled = false;
-                if (
-                    node.tagName === 'INPUT' &&
-                    node.type !== 'hidden' &&
-                    !controlesEspeciais.includes(node.name)
-                ) {
-                    node.value = '';
-                }
-            }
-        }
-
-        $('#txtHoraInicial, #txtHoraFinal').attr('type', 'time');
-
-        const camposViagem = [
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
-            'cardEquipamentos',
+        camposViagem.forEach(id =>
+        {
+            const elemento = document.getElementById(id);
+            if (elemento) elemento.style.display = "none";
+        });
+
+        window.inicializarComponentesEJ2();
+
+        $("#btnImprime, #btnConfirma, #btnApaga, #btnCancela").show();
+
+        const btnRequisitante = document.getElementById("btnRequisitante");
+        if (btnRequisitante)
+        {
+            btnRequisitante.classList.remove("disabled");
+        }
+
+        console.log("âœ… [ModalViagem] Campos inicializados");
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "inicializarCamposModal", error);
+    }
+};
+
+window.inicializarComponentesEJ2 = function ()
+{
+    try
+    {
+        const componentes = [
+            { id: "rteDescricao", propriedades: { enabled: true, value: "" } },
+            { id: "lstMotorista", propriedades: { enabled: true, value: "" } },
+            { id: "lstVeiculo", propriedades: { enabled: true, value: "" } },
+            { id: "lstRequisitante", propriedades: { enabled: true, value: "" } },
+
+            { id: "ddtCombustivelInicial", propriedades: { value: "" } },
+            { id: "ddtCombustivelFinal", propriedades: { value: "" } }
         ];
 
-        camposViagem.forEach((id) => {
-            const elemento = document.getElementById(id);
-            if (elemento) elemento.style.display = 'none';
-        });
-
-        window.inicializarComponentesEJ2();
-
-        $('#btnImprime, #btnConfirma, #btnApaga, #btnCancela').show();
-
-        const btnRequisitante = document.getElementById('btnRequisitante');
-        if (btnRequisitante) {
-            btnRequisitante.classList.remove('disabled');
-        }
-
-        console.log('âœ… [ModalViagem] Campos inicializados');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'inicializarCamposModal',
-            error,
-        );
-    }
-};
-
-window.inicializarComponentesEJ2 = function () {
-    try {
-        const componentes = [
-            { id: 'rteDescricao', propriedades: { enabled: true, value: '' } },
-            { id: 'lstMotorista', propriedades: { enabled: true, value: '' } },
-            { id: 'lstVeiculo', propriedades: { enabled: true, value: '' } },
-            {
-                id: 'lstRequisitante',
-                propriedades: { enabled: true, value: '' },
-            },
-
-            { id: 'ddtCombustivelInicial', propriedades: { value: '' } },
-            { id: 'ddtCombustivelFinal', propriedades: { value: '' } },
-        ];
-
-        componentes.forEach(({ id, propriedades }) => {
-            try {
+        componentes.forEach(({ id, propriedades }) =>
+        {
+            try
+            {
                 const elemento = document.getElementById(id);
-                if (
-                    elemento &&
-                    elemento.ej2_instances &&
-                    elemento.ej2_instances[0]
-                ) {
+                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
+                {
                     const componente = elemento.ej2_instances[0];
                     Object.assign(componente, propriedades);
                 }
-            } catch (error) {
-                console.warn(
-                    `âš ï¸ Não foi possível inicializar o componente: ${id}`,
-                );
+            } catch (error)
+            {
+                console.warn(`âš ï¸ Não foi possível inicializar o componente: ${id}`);
             }
         });
 
-        console.log('âœ… [ModalViagem] Componentes EJ2 inicializados');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'inicializarComponentesEJ2',
-            error,
-        );
+        console.log("âœ… [ModalViagem] Componentes EJ2 inicializados");
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "inicializarComponentesEJ2", error);
     }
 };
 
-window.limparCamposRecorrencia = function () {
-    try {
-
-        const lstRecorrente = $('#lstRecorrente').data('kendoDropDownList');
-        if (lstRecorrente) {
-            lstRecorrente.value('N');
-        }
-
-        const lstPeriodos = $('#lstPeriodos').data('kendoDropDownList');
-        if (lstPeriodos) {
-            lstPeriodos.value('');
-        }
-
-        const lstDias = $('#lstDias').data('kendoMultiSelect');
-        if (lstDias) {
-            lstDias.value([]);
-        }
-
-        const lstDiasMes = $('#lstDiasMes').data('kendoDropDownList');
-        if (lstDiasMes) {
-            lstDiasMes.value('');
-        }
-
-        const txtFinalRecorrencia = $('#txtFinalRecorrencia').data(
-            'kendoDatePicker',
-        );
-        if (txtFinalRecorrencia) {
-            txtFinalRecorrencia.value(null);
-            txtFinalRecorrencia.enable(true);
-        }
-
-        const calDatasSelecionadas = document.getElementById(
-            'calDatasSelecionadas',
-        );
-        if (
-            calDatasSelecionadas &&
-            calDatasSelecionadas.ej2_instances &&
-            calDatasSelecionadas.ej2_instances[0]
-        ) {
-            calDatasSelecionadas.ej2_instances[0].values = [];
-        }
-
-        const listBox = document.getElementById('lstDiasCalendario');
-        if (listBox && listBox.ej2_instances && listBox.ej2_instances[0]) {
+window.limparCamposRecorrencia = function ()
+{
+    try
+    {
+        const componentesRecorrencia = [
+            { id: "lstRecorrente", valor: "N" },
+            { id: "lstPeriodos", valor: "" },
+            { id: "lstDias", valor: [] },
+            { id: "txtFinalRecorrencia", valor: null },
+            { id: "calDatasSelecionadas", valor: null }
+        ];
+
+        componentesRecorrencia.forEach(({ id, valor }) =>
+        {
+            if (id === "txtFinalRecorrencia")
+            {
+                window.setKendoDateValue(id, null);
+                return;
+            }
+            const elemento = document.getElementById(id);
+            if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
+            {
+                elemento.ej2_instances[0].value = valor;
+            } else if (elemento)
+            {
+                elemento.value = valor;
+            }
+        });
+
+        const listBox = document.getElementById("lstDiasCalendario");
+        if (listBox && listBox.ej2_instances && listBox.ej2_instances[0])
+        {
             listBox.ej2_instances[0].dataSource = [];
         }
 
-        const badge = document.getElementById('itensBadge');
+        const badge = document.getElementById("itensBadge");
         if (badge) badge.textContent = 0;
 
-        const lstDatasVariadas = document.getElementById('lstDatasVariadas');
-        if (lstDatasVariadas) {
+        const lstDatasVariadas = document.getElementById("lstDatasVariadas");
+        if (lstDatasVariadas)
+        {
             lstDatasVariadas.innerHTML = '';
             lstDatasVariadas.size = 3;
         }
 
-        const badgeDatasVariadas = document.getElementById(
-            'badgeContadorDatasVariadas',
-        );
-        if (badgeDatasVariadas) {
+        const badgeDatasVariadas = document.getElementById("badgeContadorDatasVariadas");
+        if (badgeDatasVariadas)
+        {
             badgeDatasVariadas.textContent = 0;
             badgeDatasVariadas.style.display = 'none';
         }
 
-        const listboxContainer = document.getElementById(
-            'listboxDatasVariadasContainer',
-        );
-        if (listboxContainer) {
+        const listboxContainer = document.getElementById("listboxDatasVariadasContainer");
+        if (listboxContainer)
+        {
             listboxContainer.style.display = 'none';
         }
 
-        console.log('âœ… [ModalViagem] Campos de recorrência limpos');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'limparCamposRecorrencia',
-            error,
-        );
+        console.log("âœ… [ModalViagem] Campos de recorrência limpos");
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposRecorrencia", error);
     }
 };
 
-window.limparCamposModalViagens = function () {
-    try {
-
-        if (window.modalJaFoiLimpo) {
-            console.log(
-                'â­•ï¸ [ModalViagem] Modal já foi limpo, pulando limpeza...',
-            );
+window.limparCamposModalViagens = function ()
+{
+    try
+    {
+
+        if (window.modalJaFoiLimpo)
+        {
+            console.log("â­•ï¸ [ModalViagem] Modal já foi limpo, pulando limpeza...");
             return;
         }
 
-        if (window.carregandoViagemExistente) {
-            console.log(
-                'ðŸ“Œ [ModalViagem] Carregando viagem existente, pulando limpeza',
-            );
+        if (window.carregandoViagemExistente)
+        {
+            console.log("ðŸ“Œ [ModalViagem] Carregando viagem existente, pulando limpeza");
             return;
         }
 
-        console.log('ðŸ§¹ [ModalViagem] Limpando todos os campos...');
+        console.log("ðŸ§¹ [ModalViagem] Limpando todos os campos...");
 
         document.body.classList.remove('modo-edicao-variada');
         document.body.classList.remove('modo-criacao-variada');
 
         window.modalJaFoiLimpo = true;
 
-        $('#cardRecorrencia').show();
-
-        $(
-            '#txtReport, #txtViagemId, #txtRecorrenciaViagemId, #txtStatusAgendamento, #txtUsuarioIdCriacao, #txtDataCriacao, #txtNoFichaVistoria, #txtDataFinal, #txtHoraFinal, #txtKmAtual, #txtKmInicial, #txtKmFinal, #txtRamalRequisitante, #txtNomeDoEvento, #txtDescricaoEvento, #txtDataInicialEvento, #txtDataFinalEvento, #txtQtdPessoas, #txtPonto, #txtNome, #txtRamal, #txtEmail',
-        ).val('');
-
-        $(
-            '#hidCaboEntregue, #hidCaboDevolvido, #hidArlaEntregue, #hidArlaDevolvido',
-        ).val('false');
-        $(
-            '#chkCaboEntregue, #chkCaboDevolvido, #chkArlaEntregue, #chkArlaDevolvido',
-        ).prop('checked', false);
-
-        const lstSetor = document.getElementById(
-            'lstSetorRequisitanteAgendamento',
-        );
-        if (lstSetor && lstSetor.ej2_instances && lstSetor.ej2_instances[0]) {
+        $("#cardRecorrencia").show();
+
+        $("#txtReport, #txtViagemId, #txtRecorrenciaViagemId, #txtStatusAgendamento, #txtUsuarioIdCriacao, #txtDataCriacao, #txtNoFichaVistoria, #txtDataFinal, #txtHoraFinal, #txtKmAtual, #txtKmInicial, #txtKmFinal, #txtRamalRequisitante, #txtNomeDoEvento, #txtDescricaoEvento, #txtDataInicialEvento, #txtDataFinalEvento, #txtQtdPessoas, #txtPonto, #txtNome, #txtRamal, #txtEmail").val("");
+
+        const lstSetor = document.getElementById("lstSetorRequisitanteAgendamento");
+        if (lstSetor && lstSetor.ej2_instances && lstSetor.ej2_instances[0])
+        {
             lstSetor.ej2_instances[0].value = null;
-            window.refreshComponenteSafe('lstSetorRequisitanteAgendamento');
-        }
-
-        ['txtDuracao', 'txtQuilometragem'].forEach((id) => {
-            try {
-                const elemento = $(`#${id}`).data('kendoNumericTextBox');
-                if (elemento) {
-                    elemento.value(null);
-                } else {
-
-                    $(`#${id}`).val('');
+            window.refreshComponenteSafe("lstSetorRequisitanteAgendamento");
+        }
+
+        ["txtDuracao", "txtQuilometragem"].forEach(id =>
+        {
+            try
+            {
+                const elemento = document.getElementById(id);
+                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
+                {
+                    const instance = elemento.ej2_instances[0];
+                    instance.value = null;
+                    window.refreshComponenteSafe(id);
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'limparCamposModalViagens_forEach1',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens_forEach1", error);
             }
         });
 
-        const syncIds = [
-            'lstFinalidade',
-            'ddtSetor',
-            'cmbOrigem',
-            'cmbDestino',
-            'lstMotorista',
-            'lstVeiculo',
-            'lstRequisitante',
-            'lstSetorRequisitanteAgendamento',
-            'lstEventos',
-            'ddtCombustivelInicial',
-            'ddtCombustivelFinal',
-        ];
-        syncIds.forEach((id) => {
-            try {
+        const syncIds = ["lstFinalidade", "ddtSetor", "cmbOrigem", "cmbDestino", "lstMotorista", "lstVeiculo", "lstRequisitante", "lstSetorRequisitanteAgendamento", "lstEventos", "ddtCombustivelInicial", "ddtCombustivelFinal", "lstDiasMes", "lstDias"];
+        syncIds.forEach(id =>
+        {
+            try
+            {
                 const elemento = document.getElementById(id);
-                if (
-                    elemento &&
-                    elemento.ej2_instances &&
-                    elemento.ej2_instances[0]
-                ) {
+                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
+                {
                     const instance = elemento.ej2_instances[0];
 
                     instance.value = null;
                     instance.text = '';
 
-                    if (typeof instance.enabled !== 'undefined') {
+                    if (typeof instance.enabled !== "undefined")
+                    {
                         instance.enabled = true;
                     }
 
-                    if (typeof instance.dataBind === 'function') {
+                    if (typeof instance.dataBind === 'function')
+                    {
                         instance.dataBind();
                     }
 
-                    if (typeof instance.refresh === 'function') {
+                    if (typeof instance.refresh === 'function')
+                    {
                         instance.refresh();
                     }
 
                     console.log(`âœ… ${id} limpo com sucesso`);
-                } else {
-                    console.warn(
-                        `âš ï¸ ${id} não encontrado ou não inicializado`,
-                    );
+                } else
+                {
+                    console.warn(`âš ï¸ ${id} não encontrado ou não inicializado`);
                 }
-            } catch (error) {
+            } catch (error)
+            {
                 console.error(`âŒ Erro ao limpar ${id}:`, error);
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'limparCamposModalViagens_forEach2',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens_forEach2", error);
             }
         });
 
-        try {
-            const lstDiasMesKendo = $('#lstDiasMes').data('kendoDropDownList');
-            if (lstDiasMesKendo) {
-                lstDiasMesKendo.value(null);
-                lstDiasMesKendo.enable(true);
-                console.log('lstDiasMes (Kendo) limpo');
-            }
-        } catch (error) {
-            console.warn('Erro ao limpar lstDiasMes Kendo:', error);
-        }
-
-        try {
-            const lstDiasKendo = $('#lstDias').data('kendoMultiSelect');
-            if (lstDiasKendo) {
-                lstDiasKendo.value([]);
-                lstDiasKendo.enable(true);
-                console.log('lstDias (Kendo) limpo');
-            }
-        } catch (error) {
-            console.warn('Erro ao limpar lstDias Kendo:', error);
-        }
-
-        console.log(
-            'ðŸ§¹ [Limpeza Extra] Garantindo limpeza de Motorista e Veí­culo...',
-        );
-
-        const lstMotorista = document.getElementById('lstMotorista');
-        if (
-            lstMotorista &&
-            lstMotorista.ej2_instances &&
-            lstMotorista.ej2_instances[0]
-        ) {
+        console.log("ðŸ§¹ [Limpeza Extra] Garantindo limpeza de Motorista e Veí­culo...");
+
+        const lstMotorista = document.getElementById("lstMotorista");
+        if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0])
+        {
             const motoristaInst = lstMotorista.ej2_instances[0];
             motoristaInst.value = null;
             motoristaInst.text = '';
             motoristaInst.index = null;
 
-            if (typeof motoristaInst.dataBind === 'function') {
+            if (typeof motoristaInst.dataBind === 'function')
+            {
                 motoristaInst.dataBind();
             }
 
-            if (typeof motoristaInst.clear === 'function') {
+            if (typeof motoristaInst.clear === 'function')
+            {
                 motoristaInst.clear();
             }
 
-            console.log('âœ… Motorista limpo completamente');
-        }
-
-        const lstVeiculo = document.getElementById('lstVeiculo');
-        if (
-            lstVeiculo &&
-            lstVeiculo.ej2_instances &&
-            lstVeiculo.ej2_instances[0]
-        ) {
+            console.log("âœ… Motorista limpo completamente");
+        }
+
+        const lstVeiculo = document.getElementById("lstVeiculo");
+        if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0])
+        {
             const veiculoInst = lstVeiculo.ej2_instances[0];
             veiculoInst.value = null;
             veiculoInst.text = '';
             veiculoInst.index = null;
 
-            if (typeof veiculoInst.dataBind === 'function') {
+            if (typeof veiculoInst.dataBind === 'function')
+            {
                 veiculoInst.dataBind();
             }
 
-            if (typeof veiculoInst.clear === 'function') {
+            if (typeof veiculoInst.clear === 'function')
+            {
                 veiculoInst.clear();
             }
 
-            console.log('âœ… Veí­culo limpo completamente');
-        }
-
-        if (!window.carregandoViagemExistente) {
-            ['txtDataInicial', 'txtDataFinal', 'txtFinalRecorrencia'].forEach(
-                (id) => {
-                    try {
-                        const elemento = document.getElementById(id);
-                        if (
-                            elemento &&
-                            elemento.ej2_instances &&
-                            elemento.ej2_instances[0]
-                        ) {
-                            const instance = elemento.ej2_instances[0];
-                            instance.value = null;
-                            instance.enabled = true;
-                            window.refreshComponenteSafe(id);
-                        }
-                    } catch (error) {
-                        console.error(`âŒ Erro ao limpar ${id}:`, error);
-                    }
-                },
-            );
-        } else {
-            console.log(
-                '[limparCampos] Pulando limpeza de datas (carregando viagem existente)',
-            );
-        }
-
-        const lstFinalidade = document.getElementById('lstFinalidade');
-        if (
-            lstFinalidade &&
-            lstFinalidade.ej2_instances &&
-            lstFinalidade.ej2_instances[0]
-        ) {
+            console.log("âœ… Veí­culo limpo completamente");
+        }
+
+        ["txtDataInicial", "txtDataFinal", "txtFinalRecorrencia"].forEach(id =>
+        {
+            try
+            {
+                window.setKendoDateValue(id, null);
+                window.enableKendoDatePicker(id, true);
+            } catch (error)
+            {
+                console.error(`âŒ Erro ao limpar ${id}:`, error);
+            }
+        });
+
+        const lstFinalidade = document.getElementById("lstFinalidade");
+        if (lstFinalidade && lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0])
+        {
             lstFinalidade.ej2_instances[0].value = null;
             lstFinalidade.ej2_instances[0].enabled = true;
-            window.refreshComponenteSafe('lstFinalidade');
-        }
-
-        console.log('🔄 [limparCampos] Inicializando lstRecorrente...');
-
-        if (typeof window.inicializarLstRecorrente === 'function') {
+            window.refreshComponenteSafe("lstFinalidade");
+        }
+
+        console.log("🔄 [limparCampos] Inicializando lstRecorrente...");
+
+        if (typeof window.inicializarLstRecorrente === 'function')
+        {
             window.inicializarLstRecorrente();
         }
 
-        setTimeout(() => {
-
-            if (window.carregandoViagemExistente) {
-                console.log(
-                    '📌 [limparCampos] Pulando reset lstRecorrente - edição em andamento',
-                );
-                return;
-            }
-
-            try {
-                const lstRecorrenteKendo =
-                    $('#lstRecorrente').data('kendoDropDownList');
-                if (lstRecorrenteKendo) {
-                    window.ignorarEventosRecorrencia = true;
-                    lstRecorrenteKendo.value('N');
-                    lstRecorrenteKendo.enable(true);
-                    console.log(
-                        "✅ [limparCampos] lstRecorrente (Kendo) definido como 'Não'",
-                    );
-                    window.ignorarEventosRecorrencia = false;
-                } else {
-                    console.warn('⚠️ lstRecorrente Kendo não encontrado');
+        setTimeout(() =>
+        {
+            const elRecorrente = document.getElementById("lstRecorrente");
+            if (elRecorrente && elRecorrente.ej2_instances && elRecorrente.ej2_instances[0])
+            {
+                window.ignorarEventosRecorrencia = true;
+
+                const instance = elRecorrente.ej2_instances[0];
+                if (!instance.dataSource || instance.dataSource.length === 0)
+                {
+                    instance.dataSource = [
+                        { RecorrenteId: "N", Descricao: "Não" },
+                        { RecorrenteId: "S", Descricao: "Sim" }
+                    ];
+                    instance.fields = { text: 'Descricao', value: 'RecorrenteId' };
                 }
-            } catch (error) {
-                console.warn('⚠️ Erro ao limpar lstRecorrente Kendo:', error);
+
+                instance.value = "N";
+                instance.enabled = true;
+
+                if (typeof instance.dataBind === 'function')
+                {
+                    instance.dataBind();
+                }
+
+                console.log("✅ [limparCampos] lstRecorrente definido como 'Não' (com timeout)");
+                window.ignorarEventosRecorrencia = false;
             }
         }, 100);
 
-        try {
-            const lstPeriodosKendo =
-                $('#lstPeriodos').data('kendoDropDownList');
-            if (lstPeriodosKendo) {
-                lstPeriodosKendo.value(null);
-                lstPeriodosKendo.enable(true);
-                console.log('✅ lstPeriodos (Kendo) limpo');
-            } else if (typeof window.rebuildLstPeriodos === 'function') {
-                window.rebuildLstPeriodos();
-            }
-        } catch (error) {
-            console.warn('⚠️ Erro ao limpar lstPeriodos Kendo:', error);
-        }
-
-        const rteDescricao = document.getElementById('rteDescricao');
-        if (
-            rteDescricao &&
-            rteDescricao.ej2_instances &&
-            rteDescricao.ej2_instances[0]
-        ) {
-            rteDescricao.ej2_instances[0].value = '';
-            window.refreshComponenteSafe('rteDescricao');
-        }
-
-        const idsToReset = [
-            'lstRequisitanteEvento',
-            'lstSetorRequisitanteEvento',
-            'ddtSetorRequisitante',
-        ];
-        idsToReset.forEach((id) => {
-            try {
+        const elPeriodos = document.getElementById("lstPeriodos");
+        if (elPeriodos && elPeriodos.ej2_instances && elPeriodos.ej2_instances[0])
+        {
+            elPeriodos.ej2_instances[0].value = null;
+            elPeriodos.ej2_instances[0].enabled = true;
+            window.refreshComponenteSafe("lstPeriodos");
+        } else if (typeof window.rebuildLstPeriodos === "function")
+        {
+            window.rebuildLstPeriodos();
+        }
+
+        const rteDescricao = document.getElementById("rteDescricao");
+        if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances[0])
+        {
+            rteDescricao.ej2_instances[0].value = "";
+            window.refreshComponenteSafe("rteDescricao");
+        }
+
+        const idsToReset = ["lstRequisitanteEvento", "lstSetorRequisitanteEvento", "ddtSetorRequisitante"];
+        idsToReset.forEach(id =>
+        {
+            try
+            {
                 const elemento = document.getElementById(id);
-                if (
-                    elemento &&
-                    elemento.ej2_instances &&
-                    elemento.ej2_instances[0]
-                ) {
+                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
+                {
                     const instance = elemento.ej2_instances[0];
                     instance.value = null;
                     window.refreshComponenteSafe(id);
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'modal-viagem.js',
-                    'limparCamposModalViagens_forEach3',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens_forEach3", error);
             }
         });
 
-        $(
-            '#divPeriodo, #divTxtPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia, #divFinalFalsoRecorrencia, #calendarContainer, #listboxContainer, #listboxContainerHTML',
-        ).hide();
-
-        $(
-            '#lblUsuarioAgendamento, #lblUsuarioCriacao, #lblUsuarioFinalizacao, #lblUsuarioCancelamento',
-        ).text('');
-
-        $('#btnConfirma')
-            .html("<i class='fa-regular fa-thumbs-up'></i> Confirmar")
-            .prop('disabled', false);
-
-        const calInstance = document.getElementById('calDatasSelecionadas');
-        if (
-            calInstance &&
-            calInstance.ej2_instances &&
-            calInstance.ej2_instances[0]
-        ) {
+        $("#divPeriodo, #divTxtPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia, #divFinalFalsoRecorrencia, #calendarContainer, #listboxContainer, #listboxContainerHTML").hide();
+
+        $("#lblUsuarioAgendamento, #lblUsuarioCriacao, #lblUsuarioFinalizacao, #lblUsuarioCancelamento").text("");
+
+        $("#btnConfirma").html("<i class='fa-regular fa-thumbs-up'></i> Confirmar").prop("disabled", false);
+
+        const calInstance = document.getElementById("calDatasSelecionadas");
+        if (calInstance && calInstance.ej2_instances && calInstance.ej2_instances[0])
+        {
             const calendario = calInstance.ej2_instances[0];
-            if ('values' in calendario) calendario.values = [];
-            if ('value' in calendario) calendario.value = null;
-            window.refreshComponenteSafe('calDatasSelecionadas');
-        }
-
-        const lstDiasHTML = document.getElementById('lstDiasCalendarioHTML');
-        if (lstDiasHTML) lstDiasHTML.innerHTML = '';
-
-        const listBox = document.getElementById('lstDiasCalendario');
-        if (listBox && listBox.ej2_instances && listBox.ej2_instances[0]) {
+            if ("values" in calendario) calendario.values = [];
+            if ("value" in calendario) calendario.value = null;
+            window.refreshComponenteSafe("calDatasSelecionadas");
+        }
+
+        const lstDiasHTML = document.getElementById("lstDiasCalendarioHTML");
+        if (lstDiasHTML) lstDiasHTML.innerHTML = "";
+
+        const listBox = document.getElementById("lstDiasCalendario");
+        if (listBox && listBox.ej2_instances && listBox.ej2_instances[0])
+        {
             listBox.ej2_instances[0].dataSource = [];
         }
 
-        const badge = document.getElementById('itensBadge');
+        const badge = document.getElementById("itensBadge");
         if (badge) badge.textContent = 0;
 
-        console.log('ðŸ§¹ [ModalViagem] Limpando relatório...');
-
-        if (typeof window.limparRelatorio === 'function') {
+        console.log("ðŸ§¹ [ModalViagem] Limpando relatório...");
+
+        if (typeof window.limparRelatorio === 'function')
+        {
             window.limparRelatorio();
-        } else {
-
-            $('#ReportContainerAgenda').hide();
-            $('#reportViewerAgenda').html('');
-            $('#cardRelatorio').hide();
+        } else
+        {
+
+            $("#ReportContainerAgenda").hide();
+            $("#reportViewerAgenda").html("");
+            $("#cardRelatorio").hide();
         }
 
         $('#txtViagemIdRelatorio').val('');
         window.currentViagemId = null;
 
-        if (window.xhrRelatorio && window.xhrRelatorio.abort) {
+        if (window.xhrRelatorio && window.xhrRelatorio.abort)
+        {
             window.xhrRelatorio.abort();
         }
 
-        console.log(
-            '🔄 [ModalViagem] Restaurando input date de Data Final Recorrência...',
-        );
-        const txtFinalRecorrencia = document.getElementById(
-            'txtFinalRecorrencia',
-        );
-
-        if (txtFinalRecorrencia) {
-
-            txtFinalRecorrencia.value = '';
-            txtFinalRecorrencia.style.setProperty(
-                'display',
-                'block',
-                'important',
-            );
-            txtFinalRecorrencia.disabled = false;
-            console.log('✅ Input date txtFinalRecorrencia restaurado');
-        }
-
-        console.log('âœ… [ModalViagem] Todos os campos limpos');
-    } catch (error) {
-        console.error('âŒ [ModalViagem] Erro ao limpar campos:', error);
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'limparCamposModalViagens',
-            error,
-        );
+        console.log("🔄 [ModalViagem] Restaurando DatePicker de Data Final Recorrência...");
+        const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
+        const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");
+
+        if (txtFinalRecorrenciaTexto)
+        {
+            txtFinalRecorrenciaTexto.value = "";
+            txtFinalRecorrenciaTexto.style.display = "none";
+        }
+
+        if (txtFinalRecorrencia)
+        {
+            window.showKendoDatePicker("txtFinalRecorrencia", true);
+            window.setKendoDateValue("txtFinalRecorrencia", null);
+            window.enableKendoDatePicker("txtFinalRecorrencia", true);
+        }
+
+        console.log("âœ… [ModalViagem] Todos os campos limpos");
+    } catch (error)
+    {
+        console.error("âŒ [ModalViagem] Erro ao limpar campos:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens", error);
     }
 };
 
-window.desabilitarTodosControles = function () {
-    try {
-        console.log('ðŸ”’ [ModalViagem] Desabilitando controles...');
+window.desabilitarTodosControles = function ()
+{
+    try
+    {
+        console.log("ðŸ”’ [ModalViagem] Desabilitando controles...");
 
         const botoesProtegidos = [
             'btnFecha',
             'btnFechar',
             'btnCancelar',
             'btnClose',
-            'btnCancel',
+            'btnCancel'
         ];
 
-        const divModal = document.getElementById('divModal');
-        if (divModal) {
-            const childNodes = divModal.getElementsByTagName('*');
-            for (const node of childNodes) {
-
-                const isProtegido =
-                    botoesProtegidos.includes(node.id) ||
+        const divModal = document.getElementById("divModal");
+        if (divModal)
+        {
+            const childNodes = divModal.getElementsByTagName("*");
+            for (const node of childNodes)
+            {
+
+                const isProtegido = botoesProtegidos.includes(node.id) ||
                     node.hasAttribute('data-bs-dismiss') ||
                     node.classList.contains('btn-close') ||
                     node.closest('.modal-header') !== null ||
                     node.closest('[data-bs-dismiss]') !== null;
 
-                if (!isProtegido) {
+                if (!isProtegido)
+                {
                     node.disabled = true;
                 }
             }
         }
 
         const componentesEJ2 = [
-            'txtDataFinal',
-            'lstFinalidade',
-            'lstMotorista',
-            'lstVeiculo',
-            'lstRequisitante',
-            'lstSetorRequisitanteAgendamento',
-            'cmbOrigem',
-            'cmbDestino',
-            'ddtCombustivelInicial',
-            'ddtCombustivelFinal',
-            'rteDescricao',
-            'lstEventos',
+            "txtDataInicial", "txtDataFinal", "lstFinalidade",
+            "lstMotorista", "lstVeiculo", "lstRequisitante",
+            "lstSetorRequisitanteAgendamento", "cmbOrigem", "cmbDestino",
+            "ddtCombustivelInicial", "ddtCombustivelFinal", "rteDescricao",
+            "lstRecorrente", "lstPeriodos", "lstDias", "lstEventos"
         ];
 
-        componentesEJ2.forEach((id) => {
-            try {
+        componentesEJ2.forEach(id =>
+        {
+            try
+            {
                 const elemento = document.getElementById(id);
-                if (
-                    elemento &&
-                    elemento.ej2_instances &&
-                    elemento.ej2_instances[0]
-                ) {
+                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
+                {
                     elemento.ej2_instances[0].enabled = false;
                 }
-            } catch (error) {
-                console.warn(
-                    `âš ï¸ Erro ao desabilitar componente ${id}:`,
-                    error,
-                );
+            } catch (error)
+            {
+                console.warn(`âš ï¸ Erro ao desabilitar componente ${id}:`, error);
             }
         });
 
-        try {
-            const kendoControles = [
-                { id: 'lstRecorrente', tipo: 'kendoDropDownList' },
-                { id: 'lstPeriodos', tipo: 'kendoDropDownList' },
-                { id: 'lstDias', tipo: 'kendoMultiSelect' },
-                { id: 'lstDiasMes', tipo: 'kendoDropDownList' },
-                { id: 'txtDataInicial', tipo: 'kendoDatePicker' },
-                { id: 'txtFinalRecorrencia', tipo: 'kendoDatePicker' },
-            ];
-            kendoControles.forEach((ctrl) => {
-                const kendo = $('#' + ctrl.id).data(ctrl.tipo);
-                if (kendo) {
-                    kendo.enable(false);
-                }
-            });
-        } catch (error) {
-            console.warn('⚠️ Erro ao desabilitar controles Kendo:', error);
-        }
-
-        botoesProtegidos.forEach((id) => {
+        botoesProtegidos.forEach(id =>
+        {
             const btn = document.getElementById(id);
-            if (btn) {
+            if (btn)
+            {
                 btn.disabled = false;
                 btn.classList.remove('disabled');
                 btn.style.pointerEvents = 'auto';
             }
         });
 
-        const btnClose = document.querySelector(
-            '#modalViagens .btn-close, #modalViagens [data-bs-dismiss="modal"]',
-        );
-        if (btnClose) {
+        const btnClose = document.querySelector('#modalViagens .btn-close, #modalViagens [data-bs-dismiss="modal"]');
+        if (btnClose)
+        {
             btnClose.disabled = false;
             btnClose.style.pointerEvents = 'auto';
         }
 
-        console.log(
-            'ðŸ”’ [ModalViagem] Controles desabilitados (exceto botões de fechar)',
-        );
-    } catch (error) {
-        console.error('âŒ [ModalViagem] Erro ao desabilitar controles:', error);
-        Alerta.TratamentoErroComLinha(
-            'modal-viagem.js',
-            'desabilitarTodosControles',
-            error,
-        );
+        console.log("ðŸ”’ [ModalViagem] Controles desabilitados (exceto botões de fechar)");
+    } catch (error)
+    {
+        console.error("âŒ [ModalViagem] Erro ao desabilitar controles:", error);
+        Alerta.TratamentoErroComLinha("modal-viagem.js", "desabilitarTodosControles", error);
     }
 };
 
-console.log('âœ… [ModalViagem] Arquivo carregado completamente');
+console.log("âœ… [ModalViagem] Arquivo carregado completamente");
```
