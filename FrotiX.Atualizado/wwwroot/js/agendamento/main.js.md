# wwwroot/js/agendamento/main.js

**Mudanca:** GRANDE | **+195** linhas | **-369** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/main.js
+++ ATUAL: wwwroot/js/agendamento/main.js
@@ -8,34 +8,6 @@
     window.modalDebounceTimer = null;
     window.modalIsOpening = false;
 
-    function getKendoDatePickerValue(elementId) {
-        try {
-            var element = $("#" + elementId);
-            if (element.length === 0) return null;
-            var datepicker = element.data("kendoDatePicker");
-            return datepicker ? datepicker.value() : null;
-        } catch (error) {
-            console.warn("Erro ao obter valor do DatePicker " + elementId + ":", error);
-            return null;
-        }
-    }
-
-    function setKendoDatePickerValue(elementId, value) {
-        try {
-            var element = $("#" + elementId);
-            if (element.length === 0) return;
-            var datepicker = element.data("kendoDatePicker");
-            if (datepicker) {
-                datepicker.value(value);
-            }
-        } catch (error) {
-            console.warn("Erro ao definir valor do DatePicker " + elementId + ":", error);
-        }
-    }
-
-    window.getKendoDatePickerValue = getKendoDatePickerValue;
-    window.setKendoDatePickerValue = setKendoDatePickerValue;
-
     function configurarLocalizacao()
     {
         try
@@ -52,20 +24,16 @@
     {
         try
         {
-
-            if (!window._tooltipsListenerRegistrado) {
-                $(document).on('shown.bs.modal', '.modal', function ()
-                {
-                    try
-                    {
-                        window.initializeModalTooltips();
-                    } catch (error)
-                    {
-                        Alerta.TratamentoErroComLinha("main.js", "inicializarTooltips_shown", error);
-                    }
-                });
-                window._tooltipsListenerRegistrado = true;
-            }
+            $(document).on('shown.bs.modal', '.modal', function ()
+            {
+                try
+                {
+                    window.initializeModalTooltips();
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("main.js", "inicializarTooltips_shown", error);
+                }
+            });
         } catch (error)
         {
             Alerta.TratamentoErroComLinha("main.js", "inicializarTooltips", error);
@@ -106,15 +74,13 @@
                         const isRegistraViagem = $("#btnConfirma").text().includes("Registra Viagem");
                         if (isRegistraViagem && typeof window.validarFinalizacaoConsolidadaIA === 'function')
                         {
-                            const DataInicial = getKendoDatePickerValue("txtDataInicial");
-                            const HoraInicial = $("#txtHoraInicial").val();
+                            const DataInicial = window.getKendoDateValue("txtDataInicial");
+                            const HoraInicial = window.getKendoTimeValue("txtHoraInicial");
                             const DataFinal = $("#txtDataFinal").val();
-                            const HoraFinal = $("#txtHoraFinal").val();
+                            const HoraFinal = window.getKendoTimeValue("txtHoraFinal");
                             const KmInicial = parseInt($("#txtKmInicial").val()) || 0;
                             const KmFinal = parseInt($("#txtKmFinal").val()) || 0;
-
-                            const lstVeiculoEl = document.getElementById("lstVeiculo");
-                            const veiculoId = lstVeiculoEl?.ej2_instances?.[0]?.value || '';
+                            const veiculoId = document.getElementById("lstVeiculo")?.ej2_instances?.[0]?.value || '';
 
                             if (DataFinal && HoraFinal && KmFinal > 0)
                             {
@@ -138,10 +104,8 @@
 
                         FtxSpin.show("Gravando Agendamento(s)");
 
-                        window.dataInicial = moment(getKendoDatePickerValue("txtDataInicial")).toISOString().split("T")[0];
-
-                        const lstPeriodosKendo = $("#lstPeriodos").data("kendoDropDownList");
-                        const periodoRecorrente = lstPeriodosKendo ? lstPeriodosKendo.value() : "";
+                        window.dataInicial = moment(window.getKendoDateValue("txtDataInicial")).toISOString().split("T")[0];
+                        const periodoRecorrente = document.getElementById("lstPeriodos").ej2_instances[0].value;
 
                         if (!viagemId)
                         {
@@ -282,13 +246,13 @@
                     throw new Error("Erro ao criar agendamento");
                 }
                 await window.enviarNovoAgendamento(agendamento);
-                window.exibirMensagemSucesso(1);
+                window.exibirMensagemSucesso();
 
             } else if (periodoRecorrente === "D")
             {
 
-                const dataInicial = getKendoDatePickerValue("txtDataInicial");
-                const dataFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");
+                const dataInicial = window.getKendoDateValue("txtDataInicial");
+                const dataFinalRecorrencia = window.getKendoDateValue("txtFinalRecorrencia");
 
                 if (!dataInicial || !dataFinalRecorrencia)
                 {
@@ -315,21 +279,21 @@
                 console.log(" - Primeiro dia:", datasRecorrentes[0]);
                 console.log(" - Último dia:", datasRecorrentes[datasRecorrentes.length - 1]);
 
-                const resultadoDiario = await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
-                window.exibirMensagemSucesso(resultadoDiario?.totalCriados || datasRecorrentes.length);
+                await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
+                window.exibirMensagemSucesso();
             } else if (periodoRecorrente === "S" || periodoRecorrente === "Q")
             {
 
-                const lstDiasKendo = $("#lstDias").data("kendoMultiSelect");
-                const diasSelecionados = lstDiasKendo ? lstDiasKendo.value() : [];
+                const lstDias = document.getElementById("lstDias")?.ej2_instances?.[0];
+                const diasSelecionados = lstDias?.value || [];
 
                 if (diasSelecionados.length === 0)
                 {
                     throw new Error("Selecione pelo menos um dia da semana");
                 }
 
-                const dataInicial = getKendoDatePickerValue("txtDataInicial");
-                const dataFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");
+                const dataInicial = window.getKendoDateValue("txtDataInicial");
+                const dataFinalRecorrencia = window.getKendoDateValue("txtFinalRecorrencia");
 
                 const datasRecorrentes = gerarDatasSemanais(
                     dataInicial,
@@ -338,16 +302,15 @@
                     periodoRecorrente === "Q" ? 2 : 1
                 );
 
-                const resultadoSemanal = await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
-                window.exibirMensagemSucesso(resultadoSemanal?.totalCriados || datasRecorrentes.length);
+                await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
+                window.exibirMensagemSucesso();
 
             } else if (periodoRecorrente === "M")
             {
 
-                const lstDiasMesKendo = $("#lstDiasMes").data("kendoDropDownList");
-                const diaMes = lstDiasMesKendo ? lstDiasMesKendo.value() : null;
-                const dataInicial = getKendoDatePickerValue("txtDataInicial");
-                const dataFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");
+                const diaMes = document.getElementById("lstDiasMes")?.ej2_instances?.[0]?.value;
+                const dataInicial = window.getKendoDateValue("txtDataInicial");
+                const dataFinalRecorrencia = window.getKendoDateValue("txtFinalRecorrencia");
 
                 if (!diaMes)
                 {
@@ -365,8 +328,8 @@
                     dataAtual.setMonth(dataAtual.getMonth() + 1);
                 }
 
-                const resultadoMensal = await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
-                window.exibirMensagemSucesso(resultadoMensal?.totalCriados || datasRecorrentes.length);
+                await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
+                window.exibirMensagemSucesso();
 
             } else if (periodoRecorrente === "V")
             {
@@ -383,8 +346,8 @@
                     window.toDateOnlyString(new Date(d))
                 );
 
-                const resultadoVariado = await window.handleRecurrence(periodoRecorrente, datasFormatadas);
-                window.exibirMensagemSucesso(resultadoVariado?.totalCriados || datasFormatadas.length);
+                await window.handleRecurrence(periodoRecorrente, datasFormatadas);
+                window.exibirMensagemSucesso();
 
             } else
             {
@@ -434,83 +397,57 @@
     {
         try
         {
-            window._recorrenciaOverlayAtivo = false;
-            const isAberta = await new Promise((resolve, reject) =>
-            {
-                try
-                {
-                    $.ajax({
-                        url: `/api/Viagem/PegarStatusViagem`,
-                        type: "GET",
-                        data: { viagemId: viagemId },
-                        success: function (resultado)
-                        {
-                            try
-                            {
-                                resolve(resultado);
-                            } catch (error)
-                            {
-                                Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_successResolve", error);
-                                reject(error);
+            $.ajax({
+                url: `/api/Viagem/PegarStatusViagem`,
+                type: "GET",
+                data: { viagemId: viagemId },
+                success: async function (isAberta)
+                {
+                    try
+                    {
+                        if (isAberta)
+                        {
+                            await window.editarAgendamento(viagemId);
+                        } else
+                        {
+                            const objViagem = await window.recuperarViagemEdicao(viagemId);
+
+                            if (objViagem.recorrente === "S")
+                            {
+                                const confirmacao = await Alerta.Confirmar(
+                                    "Editar Agendamento Recorrente",
+                                    "Deseja aplicar as alterações a todos os agendamentos recorrentes ou apenas ao atual?",
+                                    "Todos",
+                                    "Apenas ao Atual"
+                                );
+
+                                window.editarTodosRecorrentes = confirmacao;
+
+                                await window.editarAgendamentoRecorrente(
+                                    viagemId,
+                                    confirmacao,
+                                    objViagem.dataInicial,
+                                    objViagem.recorrenciaViagemId,
+                                    window.editarTodosRecorrentes
+                                );
+                            } else
+                            {
+                                await window.editarAgendamento(viagemId);
                             }
-                        },
-                        error: function (jqXHR, textStatus, errorThrown)
-                        {
-                            const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
-                            Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_error", erro);
-                            reject(erro);
-                        }
-                    });
-                } catch (error)
-                {
-                    Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_ajax", error);
-                    reject(error);
-                }
-            });
-
-            if (isAberta)
-            {
-                await window.editarAgendamento(viagemId);
-                return;
-            }
-
-            const objViagem = await window.recuperarViagemEdicao(viagemId);
-
-            if (objViagem?.recorrente === "S")
-            {
-                const confirmacao = await Alerta.Confirmar(
-                    "Editar Agendamento Recorrente",
-                    "Deseja aplicar as alterações a todos os agendamentos recorrentes ou apenas ao atual?",
-                    "Todos",
-                    "Apenas ao Atual"
-                );
-
-                window.editarTodosRecorrentes = confirmacao;
-
-                if (confirmacao)
-                {
-                    window._recorrenciaOverlayAtivo = true;
-                    FtxSpin.show("Atualizando agendamentos recorrentes");
-                }
-
-                await window.editarAgendamentoRecorrente(
-                    viagemId,
-                    confirmacao,
-                    objViagem.dataInicial,
-                    objViagem.recorrenciaViagemId,
-                    window.editarTodosRecorrentes
-                );
-            } else
-            {
-                await window.editarAgendamento(viagemId);
-            }
+                        }
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_success", error);
+                    }
+                },
+                error: function (jqXHR, textStatus, errorThrown)
+                {
+                    const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
+                    Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_error", erro);
+                }
+            });
         } catch (error)
         {
-            if (window._recorrenciaOverlayAtivo)
-            {
-                FtxSpin.hide();
-                window._recorrenciaOverlayAtivo = false;
-            }
             Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento", error);
         }
     }
@@ -543,15 +480,14 @@
                     $("#btnConfirma").html("<i class='fa fa-save' aria-hidden='true'></i>Registra Viagem");
 
                     const camposViagem = [
-                        "divDataFinal", "divHoraFinal", "divDuracao",
+                        "divNoFichaVistoria", "divDataFinal", "divHoraFinal", "divDuracao",
                         "divKmAtual", "divKmInicial", "divKmFinal", "divQuilometragem",
                         "divCombustivelInicial", "divCombustivelFinal"
                     ];
 
                     camposViagem.forEach(id =>
                     {
-                        const el = document.getElementById(id);
-                        if (el) el.style.display = "block";
+                        document.getElementById(id).style.display = "block";
                     });
 
                     const lstVeiculo = document.getElementById("lstVeiculo");
@@ -781,13 +717,13 @@
                         if (!resultadoDataFutura.valido)
                         {
                             await Alerta.Erro(resultadoDataFutura.titulo, resultadoDataFutura.mensagem);
-                            $("#txtDataFinal").val("");
+                            window.setKendoDateValue("txtDataFinal", null);
                             return;
                         }
 
-                        const DataInicial = getKendoDatePickerValue("txtDataInicial");
-                        const HoraInicial = $("#txtHoraInicial").val();
-                        const HoraFinal = $("#txtHoraFinal").val();
+                        const DataInicial = window.getKendoDateValue("txtDataInicial");
+                        const HoraInicial = window.getKendoTimeValue("txtHoraInicial");
+                        const HoraFinal = window.getKendoTimeValue("txtHoraFinal");
 
                         if (DataInicial && HoraInicial && HoraFinal)
                         {
@@ -801,10 +737,10 @@
                             const resultadoDatas = await validador.analisarDatasHoras(dadosDatas);
                             if (!resultadoDatas.valido && resultadoDatas.nivel === 'erro')
                             {
-                                await Alerta.Erro(resultadoDatas.titulo, resultadoDatas.mensagem);
-                                $("#txtDataFinal").val("");
-                                return;
-                            }
+                            await Alerta.Erro(resultadoDatas.titulo, resultadoDatas.mensagem);
+                            window.setKendoDateValue("txtDataFinal", null);
+                            return;
+                        }
                             else if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
                             {
                                 const confirma = await Alerta.ValidacaoIAConfirmar(
@@ -815,7 +751,7 @@
                                 );
                                 if (!confirma)
                                 {
-                                    $("#txtDataFinal").val("");
+                                    window.setKendoDateValue("txtDataFinal", null);
                                     return;
                                 }
                             }
@@ -824,11 +760,12 @@
                     else
                     {
 
-                        const DataInicial = getKendoDatePickerValue("txtDataInicial");
-                        if (DataFinal < DataInicial)
+                        const dataInicial = window.getKendoDateValue("txtDataInicial");
+                        const dataFinalDate = window.getKendoDateValue("txtDataFinal");
+                        if (dataFinalDate && dataInicial && dataFinalDate < dataInicial)
                         {
                             Alerta.Erro("Atenção", "A data final deve ser maior que a inicial");
-                            $("#txtDataFinal").val("");
+                            window.setKendoDateValue("txtDataFinal", null);
                         }
                     }
                 } catch (error)
@@ -841,18 +778,18 @@
             {
                 try
                 {
-                    const selectedDate = getKendoDatePickerValue("txtDataInicial");
+                    const selectedDate = window.getKendoDateValue("txtDataInicial");
 
                     if (!selectedDate)
                     {
-                        console.warn("⚠️ txtDataInicial não tem valor selecionado");
+                        console.warn("⚠️ txtDataInicial não está inicializado como Kendo DatePicker");
                         return;
                     }
 
-                    var txtDataFinalPicker = $("#txtDataFinal").data("kendoDatePicker");
-                    if (txtDataFinalPicker)
-                    {
-                        txtDataFinalPicker.min(selectedDate);
+                    const dataFinalPicker = window.getKendoDatePicker("txtDataFinal");
+                    if (dataFinalPicker && typeof dataFinalPicker.min === "function")
+                    {
+                        dataFinalPicker.min(selectedDate);
                     }
 
                     window.calcularDuracaoViagem();
@@ -866,12 +803,12 @@
             {
                 try
                 {
-                    const HoraFinal = $("#txtHoraFinal").val();
+                    const HoraFinal = window.getKendoTimeValue("txtHoraFinal");
                     const DataFinal = $("#txtDataFinal").val();
 
                     if (DataFinal === "")
                     {
-                        $("#txtHoraFinal").val("");
+                        window.setKendoTimeValue("txtHoraFinal", "");
                         Alerta.Erro("Atenção", "Preencha a Data Final para poder preencher a Hora Final");
                         return;
                     }
@@ -884,8 +821,8 @@
                     {
                         const validador = ValidadorFinalizacaoIA.obterInstancia();
 
-                        const DataInicial = getKendoDatePickerValue("txtDataInicial");
-                        const HoraInicial = $("#txtHoraInicial").val();
+                        const DataInicial = window.getKendoDateValue("txtDataInicial");
+                        const HoraInicial = window.getKendoTimeValue("txtHoraInicial");
 
                         if (DataInicial && HoraInicial)
                         {
@@ -900,7 +837,7 @@
                             if (!resultadoDatas.valido && resultadoDatas.nivel === 'erro')
                             {
                                 await Alerta.Erro(resultadoDatas.titulo, resultadoDatas.mensagem);
-                                $("#txtHoraFinal").val("");
+                                window.setKendoTimeValue("txtHoraFinal", "");
                                 return;
                             }
                             else if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
@@ -913,7 +850,7 @@
                                 );
                                 if (!confirma)
                                 {
-                                    $("#txtHoraFinal").val("");
+                                    window.setKendoTimeValue("txtHoraFinal", "");
                                     return;
                                 }
                             }
@@ -922,12 +859,13 @@
                     else
                     {
 
-                        const HoraInicial = $("#txtHoraInicial").val();
-                        const DataInicial = getKendoDatePickerValue("txtDataInicial");
-
-                        if (HoraFinal < HoraInicial && DataInicial === DataFinal)
-                        {
-                            $("#txtHoraFinal").val("");
+                        const HoraInicial = window.getKendoTimeValue("txtHoraInicial");
+                        const DataInicial = window.getKendoDateValue("txtDataInicial");
+                        const DataFinalDate = window.getKendoDateValue("txtDataFinal");
+
+                        if (HoraFinal < HoraInicial && DataInicial && DataFinalDate && DataInicial.toDateString() === DataFinalDate.toDateString())
+                        {
+                            window.setKendoTimeValue("txtHoraFinal", "");
                             Alerta.Erro("Atenção", "A hora final deve ser maior que a inicial");
                         }
                     }
@@ -941,14 +879,14 @@
             {
                 try
                 {
-                    const HoraInicial = $("#txtHoraInicial").val();
-                    const HoraFinal = $("#txtHoraFinal").val();
+                            const HoraInicial = window.getKendoTimeValue("txtHoraInicial");
+                            const HoraFinal = window.getKendoTimeValue("txtHoraFinal");
 
                     if (HoraFinal === "") return;
 
                     if (HoraInicial > HoraFinal)
                     {
-                        $("#txtHoraInicial").val("");
+                        window.setKendoTimeValue("txtHoraInicial", "");
                         Alerta.Erro("Atenção", "A hora inicial deve ser menor que a final");
                     }
                 } catch (error)
@@ -1109,19 +1047,17 @@
             {
                 try
                 {
-                    const txtDataInicial = getKendoDatePickerValue("txtDataInicial");
-                    const txtFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");
-
-                    if (txtDataInicial && txtFinalRecorrencia)
-                    {
-                        const dataInicial = moment(txtDataInicial, "DD-MM-YYYY");
-                        const dataFinal = moment(txtFinalRecorrencia, "DD-MM-YYYY");
-                        const diferencaDias = dataFinal.diff(dataInicial, "days");
+                    const dataInicial = window.getKendoDateValue("txtDataInicial");
+                    const dataFinalRecorrencia = window.getKendoDateValue("txtFinalRecorrencia");
+
+                    if (dataInicial && dataFinalRecorrencia)
+                    {
+                        const diferencaDias = moment(dataFinalRecorrencia).diff(moment(dataInicial), "days");
 
                         if (diferencaDias > 365)
                         {
                             Alerta.Warning("Atenção", "A data final não pode ser maior que 365 dias após a data inicial");
-                            $("#txtFinalRecorrencia").val("");
+                            window.setKendoDateValue("txtFinalRecorrencia", null);
                         }
                     }
                 } catch (error)
@@ -1162,99 +1098,6 @@
                             if (!el)
                             {
                                 console.warn(`⚠️ Elemento ${c.id} não encontrado`);
-                                return;
-                            }
-
-                            if (c.id === "txtFinalRecorrencia")
-                            {
-                                try
-                                {
-                                    var kendoDP = $("#txtFinalRecorrencia").data("kendoDatePicker");
-                                    if (kendoDP)
-                                    {
-                                        var wrapper = kendoDP.wrapper;
-                                        if (wrapper && wrapper.length)
-                                        {
-                                            wrapper.css({
-                                                "width": c.w,
-                                                "min-height": "34px",
-                                                "height": "34px"
-                                            });
-
-                                            wrapper.addClass("e-small");
-
-                                            var input = wrapper.find("input.k-input-inner");
-                                            if (input.length)
-                                            {
-                                                input.css({
-                                                    "height": "34px",
-                                                    "line-height": "34px",
-                                                    "padding-top": "0",
-                                                    "padding-bottom": "0"
-                                                });
-                                            }
-
-                                            console.log("✅ txtFinalRecorrencia (Kendo DatePicker) compactado");
-                                        }
-                                    }
-                                }
-                                catch (errKendo)
-                                {
-                                    console.error("❌ Erro ao compactar txtFinalRecorrencia:", errKendo.message);
-                                }
-                                return;
-                            }
-
-                            if (c.id === "lstRecorrente" || c.id === "lstPeriodos" || c.id === "lstDiasMes")
-                            {
-                                try
-                                {
-                                    var kendoDDL = $("#" + c.id).data("kendoDropDownList");
-                                    if (kendoDDL)
-                                    {
-                                        var wrapper = kendoDDL.wrapper;
-                                        if (wrapper && wrapper.length)
-                                        {
-                                            wrapper.css({
-                                                "width": c.w,
-                                                "min-height": "34px",
-                                                "height": "34px"
-                                            });
-                                            wrapper.addClass("e-small");
-                                            console.log("✅ " + c.id + " (Kendo DropDownList) compactado");
-                                        }
-                                    }
-                                }
-                                catch (errKendo)
-                                {
-
-                                }
-                                return;
-                            }
-
-                            if (c.id === "lstDias")
-                            {
-                                try
-                                {
-                                    var kendoMS = $("#lstDias").data("kendoMultiSelect");
-                                    if (kendoMS)
-                                    {
-                                        var wrapper = kendoMS.wrapper;
-                                        if (wrapper && wrapper.length)
-                                        {
-                                            wrapper.css({
-                                                "width": c.w,
-                                                "min-height": "34px"
-                                            });
-                                            wrapper.addClass("e-small");
-                                            console.log("✅ lstDias (Kendo MultiSelect) compactado");
-                                        }
-                                    }
-                                }
-                                catch (errKendo)
-                                {
-
-                                }
                                 return;
                             }
 
@@ -1364,29 +1207,9 @@
             {
                 try
                 {
-
-                    if (window._mainHandlerExecutando) return;
-                    window._mainHandlerExecutando = true;
-                    setTimeout(() => { window._mainHandlerExecutando = false; }, 500);
+                    console.log("🚀 Modal Viagens aberto");
 
                     window.modalJaFoiLimpo = false;
-
-                    $('#modal-relatorio-loading-overlay').remove();
-
-                    if (window.FtxSpin) window.FtxSpin.hide();
-
-                    document.querySelectorAll('.e-spinner-pane, .e-spin-overlay, .e-spin-show, .e-overlay').forEach(function(el) {
-
-                        if (!el.closest('.e-dialog') && !el.closest('.e-popup-open')) {
-                            try { el.remove(); } catch(e) { el.style.display = 'none'; }
-                        }
-                    });
-
-                    $('.k-overlay, .k-loading-mask').remove();
-
-                    $('.ftx-spin-overlay').each(function() {
-                        if (this !== window.FtxSpin?._el) $(this).remove();
-                    });
 
                     $(".esconde-diveventos").hide();
                     $(document).off("focusin.modal");
@@ -1459,16 +1282,43 @@
                                     window.inicializarLogicaRecorrencia();
                                 }
 
-                                if (!isEdicao && !window.carregandoViagemExistente)
+                                if (!isEdicao)
                                 {
                                     console.log("🆕 Modo: Novo Agendamento");
                                     window.setModalTitle('NOVO_AGENDAMENTO');
 
+                                    setTimeout(() =>
+                                    {
+                                        const lstRecorrenteElement = document.getElementById("lstRecorrente");
+                                        if (lstRecorrenteElement && lstRecorrenteElement.ej2_instances)
+                                        {
+                                            const lstRecorrente = lstRecorrenteElement.ej2_instances[0];
+                                            if (lstRecorrente)
+                                            {
+                                                lstRecorrente.value = "N";
+                                                lstRecorrente.dataBind?.();
+                                                console.log("✅ Recorrente definido como 'Não' (padrío)");
+
+                                                requestAnimationFrame(() => RecorrenciasCompactar());
+                                            }
+                                        }
+                                    }, 100);
                                 }
                             } else
                             {
                                 console.log("⚠️ Pulando inicialização de recorrência - carregando dados existentes");
 
+                                if (window.dadosRecorrenciaCarregados)
+                                {
+                                    setTimeout(() =>
+                                    {
+                                        console.log("🔄 Restaurando dados de recorrência...");
+                                        if (typeof restaurarDadosRecorrencia === 'function')
+                                        {
+                                            restaurarDadosRecorrencia(window.dadosRecorrenciaCarregados);
+                                        }
+                                    }, 500);
+                                }
                             }
                         }, 1200);
 
@@ -1487,12 +1337,11 @@
                     const novaDataMinima = new Date();
 
                     novaDataMinima.setHours(0, 0, 0, 0);
-
-                    const txtDataInicialKendo = $("#txtDataInicial").data("kendoDatePicker");
-                    if (txtDataInicialKendo)
-                    {
-                        txtDataInicialKendo.min(novaDataMinima);
-                        console.log("✅ Data mínima ajustada para hoje (Kendo):", novaDataMinima);
+                    const datePickerInstance = window.getKendoDatePicker("txtDataInicial");
+                    if (datePickerInstance && typeof datePickerInstance.min === "function")
+                    {
+                        datePickerInstance.min(novaDataMinima);
+                        console.log("✅ Data mínima ajustada para hoje:", novaDataMinima);
                     }
 
                     setTimeout(() =>
@@ -1558,49 +1407,6 @@
 
                     }, 2000);
 
-                    if (window._overlayCleanupInterval) {
-                        clearInterval(window._overlayCleanupInterval);
-                    }
-                    window._overlayCleanupInterval = setInterval(function() {
-
-                        if (!$('#modalViagens').hasClass('show')) {
-                            clearInterval(window._overlayCleanupInterval);
-                            window._overlayCleanupInterval = null;
-                            return;
-                        }
-
-                        $('body > .e-overlay, body > .e-spinner-pane').each(function() {
-                            console.log('[Limpeza] Removendo overlay órfão:', this.className);
-                            $(this).remove();
-                        });
-                    }, 500);
-
-                    const syncfusionComboIds = ['lstVeiculo', 'lstMotorista', 'lstFinalidade', 'cmbOrigem', 'cmbDestino', 'lstEventos'];
-                    syncfusionComboIds.forEach(function(id) {
-                        const el = document.getElementById(id);
-                        if (el && el.ej2_instances && el.ej2_instances[0]) {
-                            const inst = el.ej2_instances[0];
-
-                            if (inst._overlayCleanupBound) return;
-                            inst._overlayCleanupBound = true;
-
-                            inst.close = (function(originalClose) {
-                                return function() {
-                                    if (originalClose) originalClose.apply(this, arguments);
-
-                                    setTimeout(function() {
-                                        $('body > .e-overlay').each(function() {
-                                            console.log('[ComboBox Close] Removendo overlay órfão');
-                                            $(this).remove();
-                                        });
-                                    }, 100);
-                                };
-                            })(inst.close);
-
-                            console.log('[Overlay] Listener de close configurado para:', id);
-                        }
-                    });
-
                     console.log("✅ Modal Viagens inicializado com sucesso");
 
                 }
@@ -1614,13 +1420,6 @@
                 try
                 {
                     console.log("🚪 Modal Viagens fechando...");
-
-                    if (window._overlayCleanupInterval) {
-                        clearInterval(window._overlayCleanupInterval);
-                        window._overlayCleanupInterval = null;
-                    }
-
-                    $('body > .e-overlay, body > .e-spinner-pane, body > .e-spin-overlay').remove();
 
                     if (window.limparRelatorio)
                     {
@@ -1888,15 +1687,6 @@
 
             inicializarRamalTextBox();
 
-            setTimeout(function () {
-                if (window.AgendamentoCore && typeof window.AgendamentoCore.inicializar === "function") {
-                    console.log("🎯 Inicializando AgendamentoCore...");
-                    window.AgendamentoCore.inicializar();
-                } else {
-                    console.warn("⚠️ AgendamentoCore não encontrado");
-                }
-            }, 300);
-
             setTimeout(function ()
             {
                 inicializarEventoSelect();
@@ -1923,33 +1713,33 @@
     {
         try
         {
-
-            const lstDiasMesKendo = $("#lstDiasMes").data("kendoDropDownList");
-
-            if (!lstDiasMesKendo)
-            {
-                console.warn("⚠️ lstDiasMes (Kendo) não inicializado - pode ser renderizado posteriormente");
-                return false;
+            const lstDiasMesElement = document.getElementById("lstDiasMes");
+
+            if (!lstDiasMesElement || !lstDiasMesElement.ej2_instances || !lstDiasMesElement.ej2_instances[0])
+            {
+                console.warn("⚠️ lstDiasMes não inicializado");
+                return;
             }
+
+            const lstDiasMesObj = lstDiasMesElement.ej2_instances[0];
 
             const diasDoMes = [];
             for (let i = 1; i <= 31; i++)
             {
                 diasDoMes.push({
-                    value: i,
-                    text: i.toString()
+                    Value: i,
+                    Text: i.toString()
                 });
             }
 
-            lstDiasMesKendo.setDataSource(new kendo.data.DataSource({ data: diasDoMes }));
-
-            console.log("✅ lstDiasMes populado com 31 dias (Kendo)");
-            return true;
+            lstDiasMesObj.dataSource = diasDoMes;
+            lstDiasMesObj.dataBind();
+
+            console.log("✅ lstDiasMes populado com 31 dias");
 
         } catch (error)
         {
             Alerta.TratamentoErroComLinha("main.js", "inicializarLstDiasMes", error);
-            return false;
         }
     }
 
@@ -1957,29 +1747,30 @@
     {
         try
         {
-
-            const lstDiasKendo = $("#lstDias").data("kendoMultiSelect");
-
-            if (!lstDiasKendo)
-            {
-                console.warn("⚠️ lstDias (Kendo) não inicializado - pode ser renderizado posteriormente");
-                return false;
+            const lstDiasElement = document.getElementById("lstDias");
+
+            if (!lstDiasElement || !lstDiasElement.ej2_instances || !lstDiasElement.ej2_instances[0])
+            {
+                console.warn("⚠️ lstDias não inicializado");
+                return;
             }
 
+            const lstDiasObj = lstDiasElement.ej2_instances[0];
+
             const diasDaSemana = [
-                { value: 0, text: "Domingo" },
-                { value: 1, text: "Segunda" },
-                { value: 2, text: "Terça" },
-                { value: 3, text: "Quarta" },
-                { value: 4, text: "Quinta" },
-                { value: 5, text: "Sexta" },
-                { value: 6, text: "Sábado" }
+                { Value: 0, Text: "Domingo" },
+                { Value: 1, Text: "Segunda" },
+                { Value: 2, Text: "Terça" },
+                { Value: 3, Text: "Quarta" },
+                { Value: 4, Text: "Quinta" },
+                { Value: 5, Text: "Sexta" },
+                { Value: 6, Text: "Sábado" }
             ];
 
-            lstDiasKendo.setDataSource(new kendo.data.DataSource({ data: diasDaSemana }));
-
-            console.log("✅ lstDias populado com dias da semana (Kendo)");
-            return true;
+            lstDiasObj.dataSource = diasDaSemana;
+            lstDiasObj.dataBind();
+
+            console.log("✅ lstDias populado com dias da semana");
 
         } catch (error)
         {
@@ -2035,6 +1826,14 @@
 
 function inicializarRamalTextBox()
 {
+
+    if (typeof ej === 'undefined' || !ej.inputs || !ej.inputs.TextBox)
+    {
+        console.warn('⚠️ Syncfusion (ej.inputs.TextBox) ainda não carregado. Aguardando...');
+        setTimeout(inicializarRamalTextBox, 200);
+        return;
+    }
+
     const ramalElement = document.getElementById('txtRamalRequisitanteSF');
 
     if (!ramalElement)
@@ -2089,17 +1888,13 @@
     }
 };
 
-if (!window._bordaInferiorListenerRegistrado) {
-    $(document).on('shown.bs.modal', '#modalViagens', function ()
-    {
-        setTimeout(() => window.corrigirBordaInferior(), 200);
-    });
-    window._bordaInferiorListenerRegistrado = true;
-}
-
-if (!window._limparRelatorioListenerRegistrado) {
-    $(document).on('hidden.bs.modal', '#modalViagens', async function ()
-    {
+$(document).on('shown.bs.modal', '#modalViagens', function ()
+{
+    setTimeout(() => window.corrigirBordaInferior(), 200);
+});
+
+$(document).on('hidden.bs.modal', '#modalViagens', async function ()
+{
     try
     {
         console.log('[Main] 🧹 Modal fechado - limpando...');
@@ -2128,16 +1923,14 @@
 
     } catch (error)
     {
-        console.error('[Main] Erro no evento hidden.bs.modal:', error);
+        console.error('[Main] ❌ Erro no evento hidden.bs.modal:', error);
 
         if (typeof window.esconderLoadingRelatorio === 'function')
         {
             window.esconderLoadingRelatorio();
         }
     }
-    });
-    window._limparRelatorioListenerRegistrado = true;
-}
+});
 
 $(document).ready(function ()
 {
```
