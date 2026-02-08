# wwwroot/js/cadastros/modal_agenda.js

**Mudanca:** GRANDE | **+542** linhas | **-513** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/modal_agenda.js
+++ ATUAL: wwwroot/js/cadastros/modal_agenda.js
@@ -1,270 +1,284 @@
 let calendar;
 let modalLock = false;
 let isSubmitting = false;
-let viagemId = '';
-let recorrenciaViagemId = '';
-let dataInicial = '';
+let viagemId = "";
+let recorrenciaViagemId = "";
+let dataInicial = "";
 let editarTodosRecorrentes = false;
 let transformandoEmViagem = false;
 let selectedDates = [];
 
-$(document).ready(function () {
+$(document).ready(function ()
+{
     inicializarComponentes();
     inicializarEventos();
     inicializarCalendario();
 });
 
-function inicializarComponentes() {
+function inicializarComponentes()
+{
 
     new ej.dropdowns.DropDownList({
         dataSource: [
-            { text: 'Sim', value: 'S' },
-            { text: 'Não', value: 'N' },
+            { text: "Sim", value: "S" },
+            { text: "Não", value: "N" }
         ],
-        fields: { text: 'text', value: 'value' },
-        value: 'N',
-        placeholder: 'Selecione uma opção',
-        change: onRecorrenciaChange,
-    }).appendTo('#lstRecorrente');
+        fields: { text: "text", value: "value" },
+        value: "N",
+        placeholder: "Selecione uma opção",
+        change: onRecorrenciaChange
+    }).appendTo("#lstRecorrente");
 
     new ej.dropdowns.DropDownList({
         dataSource: [
-            { text: 'Diário', value: 'D' },
-            { text: 'Semanal', value: 'S' },
-            { text: 'Quinzenal', value: 'Q' },
-            { text: 'Mensal', value: 'M' },
-            { text: 'Dias Variados', value: 'V' },
+            { text: "Diário", value: "D" },
+            { text: "Semanal", value: "S" },
+            { text: "Quinzenal", value: "Q" },
+            { text: "Mensal", value: "M" },
+            { text: "Dias Variados", value: "V" }
         ],
-        fields: { text: 'text', value: 'value' },
-        placeholder: 'Selecione um período',
-        change: onPeriodoChange,
-    }).appendTo('#lstPeriodos');
+        fields: { text: "text", value: "value" },
+        placeholder: "Selecione um período",
+        change: onPeriodoChange
+    }).appendTo("#lstPeriodos");
 
     new ej.dropdowns.MultiSelect({
-        placeholder: 'Selecione os dias...',
+        placeholder: "Selecione os dias...",
         dataSource: [
-            { id: 'Monday', name: 'Segunda' },
-            { id: 'Tuesday', name: 'Terça' },
-            { id: 'Wednesday', name: 'Quarta' },
-            { id: 'Thursday', name: 'Quinta' },
-            { id: 'Friday', name: 'Sexta' },
-            { id: 'Saturday', name: 'Sábado' },
-            { id: 'Sunday', name: 'Domingo' },
+            { id: "Monday", name: "Segunda" },
+            { id: "Tuesday", name: "Terça" },
+            { id: "Wednesday", name: "Quarta" },
+            { id: "Thursday", name: "Quinta" },
+            { id: "Friday", name: "Sexta" },
+            { id: "Saturday", name: "Sábado" },
+            { id: "Sunday", name: "Domingo" }
         ],
-        fields: { text: 'name', value: 'id' },
-        maximumSelectionLength: 7,
-    }).appendTo('#lstDias');
+        fields: { text: "name", value: "id" },
+        maximumSelectionLength: 7
+    }).appendTo("#lstDias");
 
     let hoje = new Date();
     hoje.setHours(0, 0, 0, 0);
 
     new ej.calendars.DatePicker({
         min: hoje,
-        format: 'dd/MM/yyyy',
-        change: onDataInicialChange,
-    }).appendTo('#txtDataInicial');
+        format: "dd/MM/yyyy",
+        change: onDataInicialChange
+    }).appendTo("#txtDataInicial");
 
     new ej.calendars.DatePicker({
         min: hoje,
-        format: 'dd/MM/yyyy',
-    }).appendTo('#txtDataFinal');
+        format: "dd/MM/yyyy"
+    }).appendTo("#txtDataFinal");
 
     new ej.calendars.DatePicker({
         min: hoje,
-        format: 'dd/MM/yyyy',
-    }).appendTo('#txtFinalRecorrencia');
+        format: "dd/MM/yyyy"
+    }).appendTo("#txtFinalRecorrencia");
 
     new ej.calendars.Calendar({
         isMultiSelection: true,
         showTodayButton: false,
-        locale: 'pt-BR',
+        locale: "pt-BR",
         min: hoje,
-        change: onCalendarioChange,
-    }).appendTo('#calDatasSelecionadas');
-}
-
-function inicializarEventos() {
-
-    $('#btnConfirma').off('click').on('click', onConfirmarClick);
-
-    $('#btnFecha').on('click', fecharModal);
-
-    $('#btnApaga').on('click', onApagarClick);
-
-    $('#btnCancela').on('click', onCancelarClick);
-
-    $('#btnViagem').on('click', onViagemClick);
-
-    $('#txtDataFinal').on('focusout', validarDataFinal);
-    $('#txtHoraFinal').on('focusout', validarHoraFinal);
-    $('#txtKmInicial').on('focusout', validarKmInicial);
-    $('#txtKmFinal').on('focusout', validarKmFinal);
-}
-
-function inicializarCalendario() {
-    const calendarEl = document.getElementById('agenda');
+        change: onCalendarioChange
+    }).appendTo("#calDatasSelecionadas");
+}
+
+function inicializarEventos()
+{
+
+    $("#btnConfirma").off("click").on("click", onConfirmarClick);
+
+    $("#btnFecha").on("click", fecharModal);
+
+    $("#btnApaga").on("click", onApagarClick);
+
+    $("#btnCancela").on("click", onCancelarClick);
+
+    $("#btnViagem").on("click", onViagemClick);
+
+    $("#txtDataFinal").on("focusout", validarDataFinal);
+    $("#txtHoraFinal").on("focusout", validarHoraFinal);
+    $("#txtKmInicial").on("focusout", validarKmInicial);
+    $("#txtKmFinal").on("focusout", validarKmFinal);
+}
+
+function inicializarCalendario()
+{
+    const calendarEl = document.getElementById("agenda");
 
     calendar = new FullCalendar.Calendar(calendarEl, {
-        timeZone: 'local',
+        timeZone: "local",
         headerToolbar: {
-            left: 'prev,next today',
-            center: 'title',
-            right: 'dayGridMonth,timeGridWeek,timeGridDay',
+            left: "prev,next today",
+            center: "title",
+            right: "dayGridMonth,timeGridWeek,timeGridDay"
         },
-        locale: 'pt-br',
+        locale: "pt-br",
         selectable: true,
         editable: true,
         navLinks: true,
         events: carregarEventos,
         eventClick: onEventoClick,
-        select: onSelecionarData,
+        select: onSelecionarData
     });
 
     calendar.render();
 }
 
-function carregarEventos(fetchInfo, successCallback, failureCallback) {
+function carregarEventos(fetchInfo, successCallback, failureCallback)
+{
     $.ajax({
-        url: '/api/Agenda/CarregaViagens',
-        type: 'GET',
-        dataType: 'json',
+        url: "/api/Agenda/CarregaViagens",
+        type: "GET",
+        dataType: "json",
         data: {
             start: fetchInfo.startStr,
-            end: fetchInfo.endStr,
+            end: fetchInfo.endStr
         },
-        success: function (data) {
-            const events = data.map((item) => ({
+        success: function (data)
+        {
+            const events = data.map(item => ({
                 id: item.id,
                 title: item.title,
                 start: item.start,
                 end: item.end,
                 backgroundColor: item.backgroundColor,
-                textColor: item.textColor,
+                textColor: item.textColor
             }));
             successCallback(events);
         },
-        error: function (error) {
+        error: function (error)
+        {
             failureCallback(error);
-        },
+        }
     });
 }
 
-function onRecorrenciaChange(e) {
+function onRecorrenciaChange(e)
+{
     const valor = e.value;
 
-    if (valor === 'S') {
-        $('#divPeriodo').show();
+    if (valor === "S")
+    {
+        $("#divPeriodo").show();
         rebuildLstPeriodos();
-    } else {
-        $('#divPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia').hide();
-        $('#calendarContainer, #listboxContainer').hide();
-    }
-}
-
-function onPeriodoChange(e) {
-
-    if (window.carregandoViagemExistente || window.ignorarEventosRecorrencia) {
-        console.log(
-            '🔒 onPeriodoChange BLOQUEADA - carregando viagem existente',
-        );
-        return;
-    }
-
+    } else
+    {
+        $("#divPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia").hide();
+        $("#calendarContainer, #listboxContainer").hide();
+    }
+}
+
+function onPeriodoChange(e)
+{
     const valor = e.value;
 
     limparCamposRecorrencia();
 
-    switch (valor) {
-        case 'D':
-            $('#divFinalRecorrencia').show();
+    switch (valor)
+    {
+        case "D":
+            $("#divFinalRecorrencia").show();
             break;
-        case 'S':
-        case 'Q':
-            $('#divDias, #divFinalRecorrencia').show();
+        case "S":
+        case "Q":
+            $("#divDias, #divFinalRecorrencia").show();
             break;
-        case 'M':
-            $('#divDiaMes, #divFinalRecorrencia').show();
+        case "M":
+            $("#divDiaMes, #divFinalRecorrencia").show();
             break;
-        case 'V':
-            $('#calendarContainer, #listboxContainer').show();
+        case "V":
+            $("#calendarContainer, #listboxContainer").show();
             break;
         default:
             ocultarCamposRecorrencia();
     }
 }
 
-function onCalendarioChange(args) {
+function onCalendarioChange(args)
+{
     const datas = Array.isArray(args.values) ? args.values : [];
     atualizarListaDatas(datas);
 }
 
-function onDataInicialChange(e) {
+function onDataInicialChange(e)
+{
     const dataInicial = e.value;
-    const datePickerFinal =
-        document.getElementById('txtDataFinal').ej2_instances[0];
-    if (datePickerFinal) {
+    const datePickerFinal = document.getElementById("txtDataFinal").ej2_instances[0];
+    if (datePickerFinal)
+    {
         datePickerFinal.min = dataInicial;
     }
 }
 
-async function onConfirmarClick(event) {
+async function onConfirmarClick(event)
+{
     event.preventDefault();
 
     if (isSubmitting) return;
 
     const $btn = $(this);
-    $btn.prop('disabled', true);
+    $btn.prop("disabled", true);
     isSubmitting = true;
 
-    try {
-        const viagemId = $('#txtViagemId').val();
+    try
+    {
+        const viagemId = $("#txtViagemId").val();
 
         const valido = await validarCampos(viagemId);
-        if (!valido) {
-            $btn.prop('disabled', false);
+        if (!valido)
+        {
+            $btn.prop("disabled", false);
             isSubmitting = false;
             return;
         }
 
-        if (!viagemId) {
+        if (!viagemId)
+        {
             await criarNovoAgendamento();
-        } else {
+        } else
+        {
             await editarAgendamento(viagemId);
         }
 
         fecharModal();
         calendar.refetchEvents();
-    } catch (error) {
-        console.error('Erro ao processar agendamento:', error);
-        Alerta.Erro('Erro', 'Não foi possível processar o agendamento');
-    } finally {
-        $btn.prop('disabled', false);
+    } catch (error)
+    {
+        console.error("Erro ao processar agendamento:", error);
+        Alerta.Erro("Erro", "Não foi possível processar o agendamento");
+    } finally
+    {
+        $btn.prop("disabled", false);
         isSubmitting = false;
     }
 }
 
-async function onApagarClick(event) {
+async function onApagarClick(event)
+{
     event.preventDefault();
 
-    const viagemId = $('#txtViagemId').val();
-    const recorrenciaId = $('#txtRecorrenciaViagemId').val();
-
-    const titulo =
-        recorrenciaId &&
-        recorrenciaId !== '00000000-0000-0000-0000-000000000000'
-            ? 'Deseja apagar todos os agendamentos recorrentes ou apenas o atual?'
-            : 'Deseja apagar este agendamento?';
+    const viagemId = $("#txtViagemId").val();
+    const recorrenciaId = $("#txtRecorrenciaViagemId").val();
+
+    const titulo = recorrenciaId && recorrenciaId !== "00000000-0000-0000-0000-000000000000"
+        ? "Deseja apagar todos os agendamentos recorrentes ou apenas o atual?"
+        : "Deseja apagar este agendamento?";
 
     const confirmacao = await Alerta.Confirmar(
         titulo,
-        'Não será possível recuperar os dados eliminados',
-        'Apagar Todos',
-        'Apenas Atual',
+        "Não será possível recuperar os dados eliminados",
+        "Apagar Todos",
+        "Apenas Atual"
     );
 
-    if (confirmacao && recorrenciaId) {
+    if (confirmacao && recorrenciaId)
+    {
         await apagarRecorrentes(recorrenciaId);
-    } else {
+    } else
+    {
         await apagarAgendamento(viagemId);
     }
 
@@ -272,31 +286,33 @@
     calendar.refetchEvents();
 }
 
-async function onCancelarClick(event) {
+async function onCancelarClick(event)
+{
     event.preventDefault();
 
-    const viagemId = $('#txtViagemId').val();
-    const recorrenciaId = $('#txtRecorrenciaViagemId').val();
-
-    const isRecorrente =
-        recorrenciaId &&
-        recorrenciaId !== '' &&
-        recorrenciaId !== '00000000-0000-0000-0000-000000000000';
+    const viagemId = $("#txtViagemId").val();
+    const recorrenciaId = $("#txtRecorrenciaViagemId").val();
+
+    const isRecorrente = recorrenciaId &&
+        recorrenciaId !== "" &&
+        recorrenciaId !== "00000000-0000-0000-0000-000000000000";
 
     const titulo = isRecorrente
-        ? 'Deseja cancelar todos os agendamentos recorrentes ou apenas o atual?'
-        : 'Deseja cancelar este agendamento?';
+        ? "Deseja cancelar todos os agendamentos recorrentes ou apenas o atual?"
+        : "Deseja cancelar este agendamento?";
 
     const confirmacao = await Alerta.Confirmar(
         titulo,
-        'Não será possível desfazer essa ação',
-        'Cancelar Todos',
-        'Apenas Atual',
+        "Não será possível desfazer essa ação",
+        "Cancelar Todos",
+        "Apenas Atual"
     );
 
-    if (confirmacao && isRecorrente) {
+    if (confirmacao && isRecorrente)
+    {
         await cancelarRecorrentes(recorrenciaId);
-    } else {
+    } else
+    {
         await cancelarAgendamento(viagemId);
     }
 
@@ -304,241 +320,245 @@
     calendar.refetchEvents();
 }
 
-function onViagemClick(event) {
+function onViagemClick(event)
+{
     event.preventDefault();
 
-    $('#btnViagem').hide();
-    $('#btnConfirma').html('<i class="fa fa-save"></i> Registra Viagem');
-
-    $('#divDataFinal, #divHoraFinal').show();
-    $('#divDuracao, #divKmAtual, #divKmInicial, #divKmFinal').show();
-    $('#divQuilometragem, #divCombustivelInicial, #divCombustivelFinal').show();
-
-    $('#txtStatusAgendamento').val(false);
-}
-
-function onEventoClick(info) {
+    $("#btnViagem").hide();
+    $("#btnConfirma").html('<i class="fa fa-save"></i> Registra Viagem');
+
+    $("#divNoFichaVistoria, #divDataFinal, #divHoraFinal").show();
+    $("#divDuracao, #divKmAtual, #divKmInicial, #divKmFinal").show();
+    $("#divQuilometragem, #divCombustivelInicial, #divCombustivelFinal").show();
+
+    $("#txtStatusAgendamento").val(false);
+}
+
+function onEventoClick(info)
+{
     info.jsEvent.preventDefault();
 
     const idViagem = info.event.id;
 
     $.ajax({
-        type: 'GET',
-        url: '/api/Agenda/RecuperaViagem',
+        type: "GET",
+        url: "/api/Agenda/RecuperaViagem",
         data: { id: idViagem },
-        contentType: 'application/json',
-        dataType: 'json',
-        success: function (response) {
+        contentType: "application/json",
+        dataType: "json",
+        success: function (response)
+        {
             exibirViagem(response.data);
-            $('#modalViagens').modal('show');
-        },
+            $("#modalViagens").modal("show");
+        }
     });
 }
 
-function onSelecionarData(info) {
-    const dataInicial = moment(info.start).format('YYYY-MM-DD');
-    const horaInicial = moment(info.start).format('HH:mm');
+function onSelecionarData(info)
+{
+    const dataInicial = moment(info.start).format("YYYY-MM-DD");
+    const horaInicial = moment(info.start).format("HH:mm");
 
     limparCampos();
-    exibirViagem('');
-
-    $('#txtDataInicial').val(dataInicial);
-    $('#txtHoraInicial').val(horaInicial);
-
-    $('#modalViagens').modal('show');
-}
-
-function criarObjetoAgendamento() {
-    const rteDescricao =
-        document.getElementById('rteDescricao')?.ej2_instances?.[0];
+    exibirViagem("");
+
+    $("#txtDataInicial").val(dataInicial);
+    $("#txtHoraInicial").val(horaInicial);
+
+    $("#modalViagens").modal("show");
+}
+
+function criarObjetoAgendamento()
+{
+    const rteDescricao = document.getElementById("rteDescricao")?.ej2_instances?.[0];
 
     return {
-        DataInicial: $('#txtDataInicial').val(),
-        DataFinal: $('#txtDataFinal').val(),
-        HoraInicio: $('#txtHoraInicial').val(),
-        HoraFim: $('#txtHoraFinal').val(),
-        Finalidade: $('#lstFinalidade').val(),
-        Origem: $('#cmbOrigem').val(),
-        Destino: $('#cmbDestino').val(),
-        MotoristaId: $('#lstMotorista').val(),
-        VeiculoId: $('#lstVeiculo').val(),
-        CombustivelInicial: $('#ddtCombustivelInicial').val(),
-        CombustivelFinal: $('#ddtCombustivelFinal').val(),
-        KmAtual: parseInt($('#txtKmAtual').val()) || null,
-        KmInicial: parseInt($('#txtKmInicial').val()) || null,
-        KmFinal: parseInt($('#txtKmFinal').val()) || null,
-        RequisitanteId: $('#lstRequisitante').val(),
-        RamalRequisitante: $('#txtRamalRequisitante').val(),
-        SetorSolicitanteId: $('#ddtSetor').val(),
-        Descricao: rteDescricao?.getHtml() || '',
-        EventoId: $('#lstEventos').val(),
-        Recorrente: $('#lstRecorrente').val(),
-        Intervalo: $('#lstPeriodos').val(),
-        dataFinalRecorrencia: $('#txtFinalRecorrencia').val(),
-        NoFichaVistoria: $('#txtNoFichaVistoria').val(),
+        DataInicial: $("#txtDataInicial").val(),
+        DataFinal: $("#txtDataFinal").val(),
+        HoraInicio: $("#txtHoraInicial").val(),
+        HoraFim: $("#txtHoraFinal").val(),
+        Finalidade: $("#lstFinalidade").val(),
+        Origem: $("#cmbOrigem").val(),
+        Destino: $("#cmbDestino").val(),
+        MotoristaId: $("#lstMotorista").val(),
+        VeiculoId: $("#lstVeiculo").val(),
+        CombustivelInicial: $("#ddtCombustivelInicial").val(),
+        CombustivelFinal: $("#ddtCombustivelFinal").val(),
+        KmAtual: parseInt($("#txtKmAtual").val()) || null,
+        KmInicial: parseInt($("#txtKmInicial").val()) || null,
+        KmFinal: parseInt($("#txtKmFinal").val()) || null,
+        RequisitanteId: $("#lstRequisitante").val(),
+        RamalRequisitante: $("#txtRamalRequisitante").val(),
+        SetorSolicitanteId: $("#ddtSetor").val(),
+        Descricao: rteDescricao?.getHtml() || "",
+        EventoId: $("#lstEventos").val(),
+        Recorrente: $("#lstRecorrente").val(),
+        Intervalo: $("#lstPeriodos").val(),
+        DataFinalRecorrencia: $("#txtFinalRecorrencia").val(),
+        NoFichaVistoria: $("#txtNoFichaVistoria").val(),
         StatusAgendamento: true,
         FoiAgendamento: true,
-        Status: 'Agendada',
+        Status: "Agendada"
     };
 }
 
-async function criarNovoAgendamento() {
+async function criarNovoAgendamento()
+{
     const agendamento = criarObjetoAgendamento();
-    const periodoRecorrente = $('#lstPeriodos').val();
-
-    if (!periodoRecorrente) {
+    const periodoRecorrente = $("#lstPeriodos").val();
+
+    if (!periodoRecorrente)
+    {
 
         await enviarAgendamento(agendamento);
-    } else {
+    } else
+    {
 
         const datasRecorrentes = calcularDatasRecorrentes(periodoRecorrente);
         await criarAgendamentosRecorrentes(agendamento, datasRecorrentes);
     }
 }
 
-async function editarAgendamento(viagemId) {
+async function editarAgendamento(viagemId)
+{
     const agendamento = criarObjetoAgendamento();
     agendamento.ViagemId = viagemId;
 
     await enviarAgendamento(agendamento);
 }
 
-async function enviarAgendamento(agendamento) {
+async function enviarAgendamento(agendamento)
+{
     return $.ajax({
-        type: 'POST',
-        url: '/api/Agenda/Agendamento',
-        contentType: 'application/json',
-        dataType: 'json',
+        type: "POST",
+        url: "/api/Agenda/Agendamento",
+        contentType: "application/json",
+        dataType: "json",
         data: JSON.stringify(agendamento),
-        success: function (data) {
-            if (data.success) {
-                AppToast.show(
-                    data.message || 'Verde',
-                    'Agendamento processado com sucesso',
-                    2000,
-                );
-            } else {
-                throw new Error(
-                    data.message || 'Erro ao processar agendamento',
-                );
+        success: function (data)
+        {
+            if (data.success)
+            {
+                AppToast.show(data.message || "Verde", "Agendamento processado com sucesso", 2000);
+            } else
+            {
+                throw new Error(data.message || "Erro ao processar agendamento");
             }
-        },
+        }
     });
 }
 
-async function apagarAgendamento(viagemId) {
+async function apagarAgendamento(viagemId)
+{
     return $.ajax({
-        type: 'POST',
-        url: '/api/Agenda/ApagaAgendamento',
-        contentType: 'application/json',
-        dataType: 'json',
+        type: "POST",
+        url: "/api/Agenda/ApagaAgendamento",
+        contentType: "application/json",
+        dataType: "json",
         data: JSON.stringify({ ViagemId: viagemId }),
-        success: function (data) {
-            if (data.success) {
-                AppToast.show(
-                    'Verde',
-                    'Agendamento excluído com sucesso',
-                    2000,
-                );
+        success: function (data)
+        {
+            if (data.success)
+            {
+                AppToast.show("Verde", "Agendamento excluído com sucesso", 2000);
             }
-        },
+        }
     });
 }
 
-async function cancelarAgendamento(viagemId) {
-    const descricao = $('#rteDescricao').val();
+async function cancelarAgendamento(viagemId)
+{
+    const descricao = $("#rteDescricao").val();
 
     return $.ajax({
-        type: 'POST',
-        url: '/api/Agenda/CancelaAgendamento',
-        contentType: 'application/json',
-        dataType: 'json',
+        type: "POST",
+        url: "/api/Agenda/CancelaAgendamento",
+        contentType: "application/json",
+        dataType: "json",
         data: JSON.stringify({ ViagemId: viagemId, Descricao: descricao }),
-        success: function (data) {
-            if (data.success) {
-                AppToast.show(
-                    'Verde',
-                    'Agendamento cancelado com sucesso',
-                    2000,
-                );
+        success: function (data)
+        {
+            if (data.success)
+            {
+                AppToast.show("Verde", "Agendamento cancelado com sucesso", 2000);
             }
-        },
+        }
     });
 }
 
-async function validarCampos(viagemId) {
-
-    const dataInicial = $('#txtDataInicial').val();
-    if (!dataInicial) {
-        await Alerta.Erro('Informação Ausente', 'A Data Inicial é obrigatória');
+async function validarCampos(viagemId)
+{
+
+    const dataInicial = $("#txtDataInicial").val();
+    if (!dataInicial)
+    {
+        await Alerta.Erro("Informação Ausente", "A Data Inicial é obrigatória");
         return false;
     }
 
-    const finalidade = $('#lstFinalidade').val();
-    if (!finalidade) {
-        await Alerta.Erro('Informação Ausente', 'A Finalidade é obrigatória');
+    const finalidade = $("#lstFinalidade").val();
+    if (!finalidade)
+    {
+        await Alerta.Erro("Informação Ausente", "A Finalidade é obrigatória");
         return false;
     }
 
-    const origem = $('#cmbOrigem').val();
-    if (!origem) {
-        await Alerta.Erro('Informação Ausente', 'A Origem é obrigatória');
+    const origem = $("#cmbOrigem").val();
+    if (!origem)
+    {
+        await Alerta.Erro("Informação Ausente", "A Origem é obrigatória");
         return false;
     }
 
-    const requisitante = $('#lstRequisitante').val();
-    if (!requisitante) {
-        await Alerta.Erro('Informação Ausente', 'O Requisitante é obrigatório');
+    const requisitante = $("#lstRequisitante").val();
+    if (!requisitante)
+    {
+        await Alerta.Erro("Informação Ausente", "O Requisitante é obrigatório");
         return false;
     }
 
-    const ramal = $('#txtRamalRequisitante').val();
-    if (!ramal) {
-        await Alerta.Erro(
-            'Informação Ausente',
-            'O Ramal do Requisitante é obrigatório',
-        );
+    const ramal = $("#txtRamalRequisitante").val();
+    if (!ramal)
+    {
+        await Alerta.Erro("Informação Ausente", "O Ramal do Requisitante é obrigatório");
         return false;
     }
 
-    const setor = $('#ddtSetor').val();
-    if (!setor) {
-        await Alerta.Erro('Informação Ausente', 'O Setor é obrigatório');
+    const setor = $("#ddtSetor").val();
+    if (!setor)
+    {
+        await Alerta.Erro("Informação Ausente", "O Setor é obrigatório");
         return false;
     }
 
-    if (viagemId && $('#btnConfirma').text().includes('Registra Viagem')) {
-
-        const fichaVistoria = $('#txtNoFichaVistoria').val();
-        if (!fichaVistoria) {
-            await Alerta.Erro(
-                'Informação Ausente',
-                'O Nº da Ficha de Vistoria é obrigatório',
-            );
+    if (viagemId && $("#btnConfirma").text().includes("Registra Viagem"))
+    {
+
+        const fichaVistoria = $("#txtNoFichaVistoria").val();
+        if (!fichaVistoria)
+        {
+            await Alerta.Erro("Informação Ausente", "O Nº da Ficha de Vistoria é obrigatório");
             return false;
         }
 
-        const motorista = $('#lstMotorista').val();
-        if (!motorista) {
-            await Alerta.Erro(
-                'Informação Ausente',
-                'O Motorista é obrigatório',
-            );
+        const motorista = $("#lstMotorista").val();
+        if (!motorista)
+        {
+            await Alerta.Erro("Informação Ausente", "O Motorista é obrigatório");
             return false;
         }
 
-        const veiculo = $('#lstVeiculo').val();
-        if (!veiculo) {
-            await Alerta.Erro('Informação Ausente', 'O Veículo é obrigatório');
+        const veiculo = $("#lstVeiculo").val();
+        if (!veiculo)
+        {
+            await Alerta.Erro("Informação Ausente", "O Veículo é obrigatório");
             return false;
         }
 
-        const combustivelInicial = $('#ddtCombustivelInicial').val();
-        if (!combustivelInicial) {
-            await Alerta.Erro(
-                'Informação Ausente',
-                'O Combustível Inicial é obrigatório',
-            );
+        const combustivelInicial = $("#ddtCombustivelInicial").val();
+        if (!combustivelInicial)
+        {
+            await Alerta.Erro("Informação Ausente", "O Combustível Inicial é obrigatório");
             return false;
         }
 
@@ -549,25 +569,26 @@
     return true;
 }
 
-async function validarQuilometragem() {
-    const kmInicial = parseFloat($('#txtKmInicial').val());
-    const kmFinal = parseFloat($('#txtKmFinal').val());
-
-    if (kmInicial && kmFinal) {
-        if (kmFinal < kmInicial) {
-            await Alerta.Erro(
-                'Erro',
-                'A quilometragem final deve ser maior que a inicial',
-            );
+async function validarQuilometragem()
+{
+    const kmInicial = parseFloat($("#txtKmInicial").val());
+    const kmFinal = parseFloat($("#txtKmFinal").val());
+
+    if (kmInicial && kmFinal)
+    {
+        if (kmFinal < kmInicial)
+        {
+            await Alerta.Erro("Erro", "A quilometragem final deve ser maior que a inicial");
             return false;
         }
 
-        if (kmFinal - kmInicial > 100) {
+        if (kmFinal - kmInicial > 100)
+        {
             const confirma = await Alerta.Confirmar(
-                'Atenção',
-                'A quilometragem final excede em 100km a inicial. Tem certeza?',
-                'Sim',
-                'Não',
+                "Atenção",
+                "A quilometragem final excede em 100km a inicial. Tem certeza?",
+                "Sim",
+                "Não"
             );
             if (!confirma) return false;
         }
@@ -576,123 +597,136 @@
     return true;
 }
 
-function validarDataFinal() {
-    const dataInicial = $('#txtDataInicial').val();
-    const dataFinal = $('#txtDataFinal').val();
-
-    if (dataFinal && dataInicial) {
-        if (new Date(dataFinal) < new Date(dataInicial)) {
-            Alerta.Erro('Atenção', 'A data final deve ser maior que a inicial');
-            $('#txtDataFinal').val('');
+function validarDataFinal()
+{
+    const dataInicial = $("#txtDataInicial").val();
+    const dataFinal = $("#txtDataFinal").val();
+
+    if (dataFinal && dataInicial)
+    {
+        if (new Date(dataFinal) < new Date(dataInicial))
+        {
+            Alerta.Erro("Atenção", "A data final deve ser maior que a inicial");
+            $("#txtDataFinal").val("");
         }
     }
 
     calcularDuracao();
 }
 
-function validarHoraFinal() {
-    const horaInicial = $('#txtHoraInicial').val();
-    const horaFinal = $('#txtHoraFinal').val();
-    const dataInicial = $('#txtDataInicial').val();
-    const dataFinal = $('#txtDataFinal').val();
-
-    if (!dataFinal) {
-        $('#txtHoraFinal').val('');
-        Alerta.Erro('Atenção', 'Preencha a Data Final primeiro');
+function validarHoraFinal()
+{
+    const horaInicial = $("#txtHoraInicial").val();
+    const horaFinal = $("#txtHoraFinal").val();
+    const dataInicial = $("#txtDataInicial").val();
+    const dataFinal = $("#txtDataFinal").val();
+
+    if (!dataFinal)
+    {
+        $("#txtHoraFinal").val("");
+        Alerta.Erro("Atenção", "Preencha a Data Final primeiro");
         return;
     }
 
-    if (dataInicial === dataFinal && horaFinal < horaInicial) {
-        $('#txtHoraFinal').val('');
-        Alerta.Erro('Atenção', 'A hora final deve ser maior que a inicial');
+    if (dataInicial === dataFinal && horaFinal < horaInicial)
+    {
+        $("#txtHoraFinal").val("");
+        Alerta.Erro("Atenção", "A hora final deve ser maior que a inicial");
     }
 
     calcularDuracao();
 }
 
-function validarKmInicial() {
-    const kmInicial = parseFloat($('#txtKmInicial').val());
-    const kmAtual = parseFloat($('#txtKmAtual').val());
-
-    if (kmInicial && kmAtual) {
-        if (kmInicial < kmAtual) {
-            $('#txtKmInicial').val('');
-            Alerta.Erro(
-                'Erro',
-                'A quilometragem inicial deve ser maior que a atual',
-            );
+function validarKmInicial()
+{
+    const kmInicial = parseFloat($("#txtKmInicial").val());
+    const kmAtual = parseFloat($("#txtKmAtual").val());
+
+    if (kmInicial && kmAtual)
+    {
+        if (kmInicial < kmAtual)
+        {
+            $("#txtKmInicial").val("");
+            Alerta.Erro("Erro", "A quilometragem inicial deve ser maior que a atual");
         }
     }
 
     calcularDistancia();
 }
 
-function validarKmFinal() {
-    const kmInicial = parseFloat($('#txtKmInicial').val());
-    const kmFinal = parseFloat($('#txtKmFinal').val());
-
-    if (kmInicial && kmFinal) {
-        if (kmFinal < kmInicial) {
-            $('#txtKmFinal').val('');
-            Alerta.Erro(
-                'Erro',
-                'A quilometragem final deve ser maior que a inicial',
-            );
+function validarKmFinal()
+{
+    const kmInicial = parseFloat($("#txtKmInicial").val());
+    const kmFinal = parseFloat($("#txtKmFinal").val());
+
+    if (kmInicial && kmFinal)
+    {
+        if (kmFinal < kmInicial)
+        {
+            $("#txtKmFinal").val("");
+            Alerta.Erro("Erro", "A quilometragem final deve ser maior que a inicial");
         }
     }
 
     calcularDistancia();
 }
 
-function calcularDuracao() {
-    const dataInicial = $('#txtDataInicial').val();
-    const horaInicial = $('#txtHoraInicial').val();
-    const dataFinal = $('#txtDataFinal').val();
-    const horaFinal = $('#txtHoraFinal').val();
-
-    if (dataInicial && horaInicial && dataFinal && horaFinal) {
+function calcularDuracao()
+{
+    const dataInicial = $("#txtDataInicial").val();
+    const horaInicial = $("#txtHoraInicial").val();
+    const dataFinal = $("#txtDataFinal").val();
+    const horaFinal = $("#txtHoraFinal").val();
+
+    if (dataInicial && horaInicial && dataFinal && horaFinal)
+    {
         const dtInicial = new Date(`${dataInicial}T${horaInicial}`);
         const dtFinal = new Date(`${dataFinal}T${horaFinal}`);
 
-        if (dtFinal > dtInicial) {
+        if (dtFinal > dtInicial)
+        {
             const horas = Math.round((dtFinal - dtInicial) / (1000 * 60 * 60));
-            $('#txtDuracao').val(horas);
-        }
-    }
-}
-
-function calcularDistancia() {
-    const kmInicial = parseFloat($('#txtKmInicial').val());
-    const kmFinal = parseFloat($('#txtKmFinal').val());
-
-    if (kmInicial && kmFinal && kmFinal >= kmInicial) {
+            $("#txtDuracao").val(horas);
+        }
+    }
+}
+
+function calcularDistancia()
+{
+    const kmInicial = parseFloat($("#txtKmInicial").val());
+    const kmFinal = parseFloat($("#txtKmFinal").val());
+
+    if (kmInicial && kmFinal && kmFinal >= kmInicial)
+    {
         const distancia = Math.round(kmFinal - kmInicial);
-        $('#txtQuilometragem').val(distancia);
-    }
-}
-
-function calcularDatasRecorrentes(periodo) {
-    const dataInicial = $('#txtDataInicial').val();
-    const dataFinal = $('#txtFinalRecorrencia').val();
-    const diasSemana = $('#lstDias').val();
-    const diaMes = $('#lstDiasMes').val();
+        $("#txtQuilometragem").val(distancia);
+    }
+}
+
+function calcularDatasRecorrentes(periodo)
+{
+    const dataInicial = $("#txtDataInicial").val();
+    const dataFinal = $("#txtFinalRecorrencia").val();
+    const diasSemana = $("#lstDias").val();
+    const diaMes = $("#lstDiasMes").val();
 
     const datas = [];
 
-    switch (periodo) {
-        case 'D':
+    switch (periodo)
+    {
+        case "D":
 
             break;
-        case 'S':
+        case "S":
 
             break;
-        case 'Q':
+        case "Q":
 
             break;
-        case 'M':
+        case "M":
 
             break;
-        case 'V':
+        case "V":
             datas.push(...selectedDates);
             break;
     }
@@ -700,75 +734,75 @@
     return datas;
 }
 
-function exibirViagem(viagem) {
+function exibirViagem(viagem)
+{
     limparCampos();
 
-    if (!viagem) {
-
-        $('#Titulo').html(
-            '<h3 class="modal-title"><i class="fad fa-calendar-alt"></i> Criar Agendamento</h3>',
-        );
-        $('#btnConfirma').html('<i class="fa fa-save"></i> Criar Agendamento');
-        $('#btnViagem, #btnApaga, #btnCancela').hide();
-    } else {
+    if (!viagem)
+    {
+
+        $("#Titulo").html('<h3 class="modal-title"><i class="fad fa-calendar-alt"></i> Criar Agendamento</h3>');
+        $("#btnConfirma").html('<i class="fa fa-save"></i> Criar Agendamento');
+        $("#btnViagem, #btnApaga, #btnCancela").hide();
+    } else
+    {
 
         preencherCamposViagem(viagem);
 
-        if (viagem.statusAgendamento) {
-            $('#Titulo').html(
-                '<h3 class="modal-title"><i class="fad fa-calendar-alt"></i> Editar Agendamento</h3>',
-            );
-            $('#btnConfirma').html(
-                '<i class="fa fa-save"></i> Editar Agendamento',
-            );
-        } else {
-            $('#Titulo').html(
-                '<h3 class="modal-title"><i class="fa fa-suitcase-rolling"></i> Exibir Viagem</h3>',
-            );
-        }
-    }
-}
-
-function preencherCamposViagem(viagem) {
-    $('#txtViagemId').val(viagem.viagemId);
-    $('#txtRecorrenciaViagemId').val(viagem.recorrenciaViagemId);
-    $('#txtStatusAgendamento').val(viagem.statusAgendamento);
-    $('#txtNoFichaVistoria').val(viagem.noFichaVistoria);
-
-    $('#txtDataInicial').val(viagem.dataInicial);
-    $('#txtDataFinal').val(viagem.dataFinal);
-    $('#txtHoraInicial').val(viagem.horaInicio);
-    $('#txtHoraFinal').val(viagem.horaFim);
-
-    $('#lstFinalidade').val(viagem.finalidade);
-    $('#cmbOrigem').val(viagem.origem);
-    $('#cmbDestino').val(viagem.destino);
-
-    $('#lstMotorista').val(viagem.motoristaId);
-    $('#lstVeiculo').val(viagem.veiculoId);
-    $('#lstRequisitante').val(viagem.requisitanteId);
-    $('#ddtSetor').val(viagem.setorSolicitanteId);
-
-    $('#txtKmAtual').val(viagem.kmAtual);
-    $('#txtKmInicial').val(viagem.kmInicial);
-    $('#txtKmFinal').val(viagem.kmFinal);
-
-    $('#ddtCombustivelInicial').val(viagem.combustivelInicial);
-    $('#ddtCombustivelFinal').val(viagem.combustivelFinal);
-
-    $('#txtRamalRequisitante').val(viagem.ramalRequisitante);
-    $('#rteDescricao').val(viagem.descricao);
-
-    if (viagem.eventoId) {
-        $('#lstEventos').val(viagem.eventoId);
-    }
-
-    if (viagem.recorrente === 'S') {
-        $('#lstRecorrente').val('S');
-        $('#lstPeriodos').val(viagem.intervalo);
-        $('#txtFinalRecorrencia').val(viagem.dataFinalRecorrencia);
-
-        onRecorrenciaChange({ value: 'S' });
+        if (viagem.statusAgendamento)
+        {
+            $("#Titulo").html('<h3 class="modal-title"><i class="fad fa-calendar-alt"></i> Editar Agendamento</h3>');
+            $("#btnConfirma").html('<i class="fa fa-save"></i> Editar Agendamento');
+        } else
+        {
+            $("#Titulo").html('<h3 class="modal-title"><i class="fa fa-suitcase-rolling"></i> Exibir Viagem</h3>');
+        }
+    }
+}
+
+function preencherCamposViagem(viagem)
+{
+    $("#txtViagemId").val(viagem.viagemId);
+    $("#txtRecorrenciaViagemId").val(viagem.recorrenciaViagemId);
+    $("#txtStatusAgendamento").val(viagem.statusAgendamento);
+    $("#txtNoFichaVistoria").val(viagem.noFichaVistoria);
+
+    $("#txtDataInicial").val(viagem.dataInicial);
+    $("#txtDataFinal").val(viagem.dataFinal);
+    $("#txtHoraInicial").val(viagem.horaInicio);
+    $("#txtHoraFinal").val(viagem.horaFim);
+
+    $("#lstFinalidade").val(viagem.finalidade);
+    $("#cmbOrigem").val(viagem.origem);
+    $("#cmbDestino").val(viagem.destino);
+
+    $("#lstMotorista").val(viagem.motoristaId);
+    $("#lstVeiculo").val(viagem.veiculoId);
+    $("#lstRequisitante").val(viagem.requisitanteId);
+    $("#ddtSetor").val(viagem.setorSolicitanteId);
+
+    $("#txtKmAtual").val(viagem.kmAtual);
+    $("#txtKmInicial").val(viagem.kmInicial);
+    $("#txtKmFinal").val(viagem.kmFinal);
+
+    $("#ddtCombustivelInicial").val(viagem.combustivelInicial);
+    $("#ddtCombustivelFinal").val(viagem.combustivelFinal);
+
+    $("#txtRamalRequisitante").val(viagem.ramalRequisitante);
+    $("#rteDescricao").val(viagem.descricao);
+
+    if (viagem.eventoId)
+    {
+        $("#lstEventos").val(viagem.eventoId);
+    }
+
+    if (viagem.recorrente === "S")
+    {
+        $("#lstRecorrente").val("S");
+        $("#lstPeriodos").val(viagem.intervalo);
+        $("#txtFinalRecorrencia").val(viagem.dataFinalRecorrencia);
+
+        onRecorrenciaChange({ value: "S" });
         onPeriodoChange({ value: viagem.intervalo });
     }
 
@@ -776,38 +810,26 @@
     calcularDistancia();
 }
 
-function limparCampos() {
-
-    $(
-        "#modalViagens input[type='text'], #modalViagens input[type='time'], #modalViagens textarea",
-    ).val('');
-    $('#modalViagens select').val('').trigger('change');
+function limparCampos()
+{
+
+    $("#modalViagens input[type='text'], #modalViagens input[type='time'], #modalViagens textarea").val("");
+    $("#modalViagens select").val("").trigger("change");
 
     const components = [
-        'lstRecorrente',
-        'lstPeriodos',
-        'lstDias',
-        'lstDiasMes',
-        'txtDataInicial',
-        'txtDataFinal',
-        'txtFinalRecorrencia',
-        'lstFinalidade',
-        'cmbOrigem',
-        'cmbDestino',
-        'lstMotorista',
-        'lstVeiculo',
-        'lstRequisitante',
-        'ddtSetor',
-        'lstEventos',
-        'ddtCombustivelInicial',
-        'ddtCombustivelFinal',
-        'rteDescricao',
-        'calDatasSelecionadas',
+        "lstRecorrente", "lstPeriodos", "lstDias", "lstDiasMes",
+        "txtDataInicial", "txtDataFinal", "txtFinalRecorrencia",
+        "lstFinalidade", "cmbOrigem", "cmbDestino",
+        "lstMotorista", "lstVeiculo", "lstRequisitante", "ddtSetor",
+        "lstEventos", "ddtCombustivelInicial", "ddtCombustivelFinal",
+        "rteDescricao", "calDatasSelecionadas"
     ];
 
-    components.forEach((id) => {
+    components.forEach(id =>
+    {
         const el = document.getElementById(id);
-        if (el && el.ej2_instances && el.ej2_instances[0]) {
+        if (el && el.ej2_instances && el.ej2_instances[0])
+        {
             const instance = el.ej2_instances[0];
             instance.value = null;
             if (instance.dataBind) instance.dataBind();
@@ -820,128 +842,133 @@
     atualizarListaDatas([]);
 }
 
-function limparCamposRecorrencia() {
-    $('#lstDias').val([]).trigger('change');
-    $('#lstDiasMes').val('').trigger('change');
-    $('#txtFinalRecorrencia').val('');
-
-    const cal = document.getElementById('calDatasSelecionadas');
-    if (cal && cal.ej2_instances && cal.ej2_instances[0]) {
+function limparCamposRecorrencia()
+{
+    $("#lstDias").val([]).trigger("change");
+    $("#lstDiasMes").val("").trigger("change");
+    $("#txtFinalRecorrencia").val("");
+
+    const cal = document.getElementById("calDatasSelecionadas");
+    if (cal && cal.ej2_instances && cal.ej2_instances[0])
+    {
         cal.ej2_instances[0].values = [];
     }
 
     selectedDates = [];
 }
 
-function ocultarCamposRecorrencia() {
-    $('#divPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia').hide();
-    $('#calendarContainer, #listboxContainer').hide();
-}
-
-function atualizarListaDatas(datas) {
-    selectedDates = datas.map((d) => ({
+function ocultarCamposRecorrencia()
+{
+    $("#divPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia").hide();
+    $("#calendarContainer, #listboxContainer").hide();
+}
+
+function atualizarListaDatas(datas)
+{
+    selectedDates = datas.map(d => ({
         Timestamp: new Date(d).getTime(),
-        DateText: moment(d).format('DD/MM/YYYY'),
+        DateText: moment(d).format("DD/MM/YYYY")
     }));
 
-    const listBox = document.getElementById('lstDiasCalendario');
-    if (listBox && listBox.ej2_instances && listBox.ej2_instances[0]) {
+    const listBox = document.getElementById("lstDiasCalendario");
+    if (listBox && listBox.ej2_instances && listBox.ej2_instances[0])
+    {
         listBox.ej2_instances[0].dataSource = selectedDates;
         listBox.ej2_instances[0].dataBind();
     }
 
-    $('#itensBadge').text(selectedDates.length);
-}
-
-function fecharModal() {
-    $('#modalViagens').modal('hide');
-    $('.modal-backdrop').remove();
-    $('body').removeClass('modal-open').css('overflow', '');
-}
-
-function rebuildLstPeriodos() {
-    const lstPeriodos = document.getElementById('lstPeriodos');
-    if (
-        lstPeriodos &&
-        lstPeriodos.ej2_instances &&
-        lstPeriodos.ej2_instances[0]
-    ) {
+    $("#itensBadge").text(selectedDates.length);
+}
+
+function fecharModal()
+{
+    $("#modalViagens").modal("hide");
+    $(".modal-backdrop").remove();
+    $("body").removeClass("modal-open").css("overflow", "");
+}
+
+function rebuildLstPeriodos()
+{
+    const lstPeriodos = document.getElementById("lstPeriodos");
+    if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0])
+    {
         lstPeriodos.ej2_instances[0].refresh();
     }
 }
 
-async function criarAgendamentosRecorrentes(agendamentoBase, datas) {
-    if (!datas || datas.length === 0) {
-        console.error('Nenhuma data para criar agendamentos recorrentes');
+async function criarAgendamentosRecorrentes(agendamentoBase, datas)
+{
+    if (!datas || datas.length === 0)
+    {
+        console.error("Nenhuma data para criar agendamentos recorrentes");
         return;
     }
 
     let recorrenciaId = null;
 
-    for (let i = 0; i < datas.length; i++) {
+    for (let i = 0; i < datas.length; i++)
+    {
         const agendamento = { ...agendamentoBase };
         agendamento.DataInicial = datas[i];
 
-        if (i === 0) {
+        if (i === 0)
+        {
 
             const response = await enviarAgendamento(agendamento);
             recorrenciaId = response.viagemId;
-        } else {
+        } else
+        {
 
             agendamento.RecorrenciaViagemId = recorrenciaId;
             await enviarAgendamento(agendamento);
         }
     }
 
-    AppToast.show(
-        'Verde',
-        `${datas.length} agendamentos criados com sucesso`,
-        3000,
-    );
-}
-
-async function apagarRecorrentes(recorrenciaId) {
+    AppToast.show("Verde", `${datas.length} agendamentos criados com sucesso`, 3000);
+}
+
+async function apagarRecorrentes(recorrenciaId)
+{
     const agendamentos = await obterAgendamentosRecorrentes(recorrenciaId);
 
-    for (const agendamento of agendamentos) {
+    for (const agendamento of agendamentos)
+    {
         await apagarAgendamento(agendamento.viagemId);
     }
 
-    AppToast.show(
-        'Verde',
-        'Agendamentos recorrentes excluídos com sucesso',
-        3000,
-    );
-}
-
-async function cancelarRecorrentes(recorrenciaId) {
+    AppToast.show("Verde", "Agendamentos recorrentes excluídos com sucesso", 3000);
+}
+
+async function cancelarRecorrentes(recorrenciaId)
+{
     const agendamentos = await obterAgendamentosRecorrentes(recorrenciaId);
 
-    for (const agendamento of agendamentos) {
+    for (const agendamento of agendamentos)
+    {
         await cancelarAgendamento(agendamento.viagemId);
     }
 
-    AppToast.show(
-        'Verde',
-        'Agendamentos recorrentes cancelados com sucesso',
-        3000,
-    );
-}
-
-async function obterAgendamentosRecorrentes(recorrenciaId) {
-    return new Promise((resolve, reject) => {
+    AppToast.show("Verde", "Agendamentos recorrentes cancelados com sucesso", 3000);
+}
+
+async function obterAgendamentosRecorrentes(recorrenciaId)
+{
+    return new Promise((resolve, reject) =>
+    {
         $.ajax({
-            url: '/api/Agenda/ObterAgendamentoExclusao',
-            type: 'GET',
-            contentType: 'application/json',
+            url: "/api/Agenda/ObterAgendamentoExclusao",
+            type: "GET",
+            contentType: "application/json",
             data: { recorrenciaViagemId: recorrenciaId },
-            success: function (data) {
+            success: function (data)
+            {
                 resolve(data || []);
             },
-            error: function (err) {
-                console.error('Erro ao obter agendamentos recorrentes:', err);
+            error: function (err)
+            {
+                console.error("Erro ao obter agendamentos recorrentes:", err);
                 resolve([]);
-            },
+            }
         });
     });
 }
@@ -951,5 +978,5 @@
     limparCampos,
     calcularDuracao,
     calcularDistancia,
-    validarCampos,
+    validarCampos
 };
```
