/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está completamente documentado em:                         ║
 * ║  📄 Documentacao/Pages/Agenda - Index.md                                 ║
 * ║                                                                          ║
 * ║  A documentação inclui:                                                   ║
 * ║  • Explicação detalhada de todas as funções principais                   ║
 * ║  • Fluxo completo de inicialização                                      ║
 * ║  • Handlers de eventos e validações                                      ║
 * ║  • Sistema de recorrência explicado                                     ║
 * ║  • Interconexões com outros módulos                                      ║
 * ║                                                                          ║
 * ║  Última atualização: 08/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

// ====================================================================
// MAIN.JS - Entry Point da Aplicação de Agendamento
// ====================================================================
// Este arquivo deve ser carregado POR ÚLTIMO após todos os módulos
// ====================================================================

(function ()
{
    'use strict';

    console.log("🚀 Inicializando Sistema de Agendamento...");

    // ====================================================================
    // VARIÁVEIS GLOBAIS LEGADAS (mantidas para compatibilidade)
    // ====================================================================

    window.defaultRTE = null;
    window.modalDebounceTimer = null;
    window.modalIsOpening = false;

    // ====================================================================
    // FUNÇÕES AUXILIARES PARA DATEPICKERS TELERIK/KENDO
    // ====================================================================

    /**
     * Obtém o valor de um DatePicker Kendo/Telerik
     * @param {string} elementId - ID do elemento datepicker
     * @returns {Date|null} - Valor da data ou null se não disponível
     */
    function getKendoDatePickerValue(elementId) {
        try {
            var element = $("#" + elementId);
            if (element.length === 0) return null;
            var datepicker = element.data("kendoDatePicker");
            return datepicker ? datepicker.value() : null;
        } catch (error) {
            console.warn("Erro ao obter valor do DatePicker " + elementId + ":", error);
            return null;
        }
    }

    /**
     * Define o valor de um DatePicker Kendo/Telerik
     * @param {string} elementId - ID do elemento datepicker
     * @param {Date|string} value - Valor da data a ser definido
     */
    function setKendoDatePickerValue(elementId, value) {
        try {
            var element = $("#" + elementId);
            if (element.length === 0) return;
            var datepicker = element.data("kendoDatePicker");
            if (datepicker) {
                datepicker.value(value);
            }
        } catch (error) {
            console.warn("Erro ao definir valor do DatePicker " + elementId + ":", error);
        }
    }

    // Exportar funções globalmente para uso em outros módulos
    window.getKendoDatePickerValue = getKendoDatePickerValue;
    window.setKendoDatePickerValue = setKendoDatePickerValue;

    // ====================================================================
    // INICIALIZAÇÃO DOS COMPONENTES SYNCFUSION
    // ====================================================================

    /**
     * Configura localização PT-BR do Syncfusion
     */
    function configurarLocalizacao()
    {
        try
        {
            console.log("⚙️ Configurando localização Syncfusion...");
            window.configurarLocalizacaoSyncfusion();
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarLocalizacao", error);
        }
    }

    /**
     * Inicializa tooltips em modais
     */
    function inicializarTooltips()
    {
        try
        {
            // NOTA: Listener registrado uma vez - executa para todos os modais
            if (!window._tooltipsListenerRegistrado) {
                $(document).on('shown.bs.modal', '.modal', function ()
                {
                    try
                    {
                        window.initializeModalTooltips();
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("main.js", "inicializarTooltips_shown", error);
                    }
                });
                window._tooltipsListenerRegistrado = true;
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "inicializarTooltips", error);
        }
    }

    // ====================================================================
    // CONFIGURAÇÃO DE EVENT HANDLERS GLOBAIS
    // ====================================================================

    /**
     * Configura botão de confirmação
     */
    function configurarBotaoConfirmar()
    {
        try
        {
            $("#btnConfirma").off("click").on("click", async function (event)
            {
                try
                {
                    event.preventDefault();
                    const $btn = $(this);

                    if ($btn.prop("disabled"))
                    {
                        console.log("Botão desabilitado, impedindo clique duplo.");
                        return;
                    }

                    $btn.prop("disabled", true);

                    try
                    {
                        const viagemId = document.getElementById("txtViagemId").value;
                        const validado = await window.ValidaCampos(viagemId);

                        if (!validado)
                        {
                            console.warn("Validação de campos reprovada.");
                            $btn.prop("disabled", false);
                            return;
                        }

                        // VALIDAÇÃO IA CONSOLIDADA - Verifica se há alertas pendentes ao registrar viagem
                        const isRegistraViagem = $("#btnConfirma").text().includes("Registra Viagem");
                        if (isRegistraViagem && typeof window.validarFinalizacaoConsolidadaIA === 'function')
                        {
                            const DataInicial = getKendoDatePickerValue("txtDataInicial");
                            const HoraInicial = $("#txtHoraInicial").val();
                            const DataFinal = $("#txtDataFinal").val();
                            const HoraFinal = $("#txtHoraFinal").val();
                            const KmInicial = parseInt($("#txtKmInicial").val()) || 0;
                            const KmFinal = parseInt($("#txtKmFinal").val()) || 0;
                            // lstVeiculo ainda é Syncfusion
                            const lstVeiculoEl = document.getElementById("lstVeiculo");
                            const veiculoId = lstVeiculoEl?.ej2_instances?.[0]?.value || '';

                            // Só valida se temos dados de finalização
                            if (DataFinal && HoraFinal && KmFinal > 0)
                            {
                                const iaValida = await window.validarFinalizacaoConsolidadaIA({
                                    dataInicial: DataInicial,
                                    horaInicial: HoraInicial,
                                    dataFinal: DataFinal,
                                    horaFinal: HoraFinal,
                                    kmInicial: KmInicial,
                                    kmFinal: KmFinal,
                                    veiculoId: veiculoId
                                });

                                if (!iaValida)
                                {
                                    $btn.prop("disabled", false);
                                    return;
                                }
                            }
                        }

                        // ✅ TODAS AS VALIDAÇÕES PASSARAM - Agora sim exibir modal de espera
                        FtxSpin.show("Gravando Agendamento(s)");

                        window.dataInicial = moment(getKendoDatePickerValue("txtDataInicial")).toISOString().split("T")[0];
                        // ✅ CORREÇÃO 20/01/2026: lstPeriodos agora é Kendo DropDownList
                        const lstPeriodosKendo = $("#lstPeriodos").data("kendoDropDownList");
                        const periodoRecorrente = lstPeriodosKendo ? lstPeriodosKendo.value() : "";

                        if (!viagemId)
                        {
                            // CRIAR NOVO AGENDAMENTO
                            await handleCriarNovoAgendamento(periodoRecorrente);
                        } else if (viagemId != null && viagemId !== "" && $("#btnConfirma").text() === "Registra Viagem")
                        {
                            // REGISTRAR VIAGEM
                            await handleRegistrarViagem(viagemId);
                        } else
                        {
                            // EDITAR AGENDAMENTO EXISTENTE
                            await handleEditarAgendamento(viagemId);
                        }
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("main.js", "btnConfirma_inner", error);
                    } finally
                    {
                        // Ocultar modal de espera FrotiX
                        FtxSpin.hide();
                        $btn.prop("disabled", false);
                    }
                } catch (error)
                {
                    FtxSpin.hide();
                    Alerta.TratamentoErroComLinha("main.js", "btnConfirma_click", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarBotaoConfirmar", error);
        }
    }

    // Função auxiliar para gerar datas semanais/quinzenais
    function gerarDatasSemanais(dataInicial, dataFinal, diasSemana, intervaloSemanas = 1)
    {
        console.log("📅 [gerarDatasSemanais] Iniciando...");
        console.log("   - diasSemana recebidos:", diasSemana);
        console.log("   - Tipo dos dias:", typeof diasSemana[0]);

        const datas = [];
        const inicio = new Date(dataInicial);
        const fim = new Date(dataFinal);

        // ✅ VERIFICAR se já são números ou strings
        let diasNumeros;

        if (diasSemana.length > 0 && typeof diasSemana[0] === 'number')
        {
            // Já são números! Usar diretamente
            diasNumeros = diasSemana;
            console.log("   ✅ Dias já são números:", diasNumeros);
        } else
        {
            // São strings, precisam ser mapeados
            diasNumeros = diasSemana.map(dia =>
            {
                const mapa = {
                    // Português
                    "Domingo": 0,
                    "Segunda": 1,
                    "Terça": 2,
                    "Quarta": 3,
                    "Quinta": 4,
                    "Sexta": 5,
                    "Sábado": 6,
                    // Inglês
                    "Sunday": 0,
                    "Monday": 1,
                    "Tuesday": 2,
                    "Wednesday": 3,
                    "Thursday": 4,
                    "Friday": 5,
                    "Saturday": 6
                };

                const numeroMapeado = mapa[dia];
                if (numeroMapeado === undefined)
                {
                    console.warn(`⚠️ Dia não reconhecido: "${dia}"`);
                }
                return numeroMapeado;
            }).filter(d => d !== undefined);
        }

        // Converter números dos dias em nomes para debug
        const nomeDias = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'];
        const diasSelecionadosNomes = diasNumeros.map(n => nomeDias[n]);
        console.log("   📋 Dias selecionados:", diasSelecionadosNomes);

        if (diasNumeros.length === 0)
        {
            console.error("❌ Nenhum dia válido foi identificado!");
            return datas;
        }

        let dataAtual = new Date(inicio);
        let contadorSemanas = 0;

        while (dataAtual <= fim)
        {
            const diaSemanaAtual = dataAtual.getDay();

            if (diasNumeros.includes(diaSemanaAtual))
            {
                // Para quinzenal, só adiciona em semanas alternadas
                if (intervaloSemanas === 1 || contadorSemanas % intervaloSemanas === 0)
                {
                    datas.push(window.toDateOnlyString(dataAtual));
                }
            }

            dataAtual.setDate(dataAtual.getDate() + 1);

            // Contador de semanas (incrementa no domingo)
            if (dataAtual.getDay() === 0)
            {
                contadorSemanas++;
            }
        }

        console.log("✅ [gerarDatasSemanais] Resultado:");
        console.log("   - Total de datas geradas:", datas.length);
        console.log("   - Primeiras 3 datas:", datas.slice(0, 3));
        console.log("   - Últimas 3 datas:", datas.slice(-3));

        return datas;
    }

    /**
     * Handle criar novo agendamento
     */
    async function handleCriarNovoAgendamento(periodoRecorrente)
    {
        try
        {
            if (periodoRecorrente === "N" || periodoRecorrente === null)
            {
                // Agendamento único (não recorrente)
                const agendamento = window.criarAgendamentoNovo();
                if (!agendamento)
                {
                    throw new Error("Erro ao criar agendamento");
                }
                await window.enviarNovoAgendamento(agendamento);
                window.exibirMensagemSucesso(1); // Agendamento único = 1

            } else if (periodoRecorrente === "D")
            {
                // DIÁRIA: Cria agendamento para cada dia entre data inicial e final (INCLUSIVE)
                const dataInicial = getKendoDatePickerValue("txtDataInicial");
                const dataFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");

                if (!dataInicial || !dataFinalRecorrencia)
                {
                    throw new Error("Datas inicial e final são obrigatórias para recorrência diária");
                }

                const datasRecorrentes = [];
                let dataAtual = new Date(dataInicial);
                const dataFim = new Date(dataFinalRecorrencia);

                // ✅ CORREÇÃO: Zerar horas para comparar apenas datas (evita problema de fuso horário)
                dataAtual.setHours(0, 0, 0, 0);
                dataFim.setHours(23, 59, 59, 999); // Final do dia para garantir inclusão

                // Usar <= para incluir o último dia
                while (dataAtual <= dataFim)
                {
                    datasRecorrentes.push(window.toDateOnlyString(dataAtual));
                    dataAtual.setDate(dataAtual.getDate() + 1);
                }

                // Debug para verificar
                console.log("📅 [Recorrência Diária] Datas geradas:");
                console.log("   - Data inicial:", window.toDateOnlyString(new Date(dataInicial)));
                console.log("   - Data final:", window.toDateOnlyString(new Date(dataFinalRecorrencia)));
                console.log("   - Total de dias:", datasRecorrentes.length);
                console.log("   - Primeiro dia:", datasRecorrentes[0]);
                console.log("   - Último dia:", datasRecorrentes[datasRecorrentes.length - 1]);

                const resultadoDiario = await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
                window.exibirMensagemSucesso(resultadoDiario?.totalCriados || datasRecorrentes.length);
            } else if (periodoRecorrente === "S" || periodoRecorrente === "Q")
            {
                // SEMANAL ou QUINZENAL: Repete nos dias da semana selecionados
                // ✅ CORREÇÃO 20/01/2026: lstDias agora é Kendo MultiSelect
                const lstDiasKendo = $("#lstDias").data("kendoMultiSelect");
                const diasSelecionados = lstDiasKendo ? lstDiasKendo.value() : [];

                if (diasSelecionados.length === 0)
                {
                    throw new Error("Selecione pelo menos um dia da semana");
                }

                const dataInicial = getKendoDatePickerValue("txtDataInicial");
                const dataFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");

                const datasRecorrentes = gerarDatasSemanais(
                    dataInicial,
                    dataFinalRecorrencia,
                    diasSelecionados,
                    periodoRecorrente === "Q" ? 2 : 1 // Quinzenal = 2 semanas
                );

                const resultadoSemanal = await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
                window.exibirMensagemSucesso(resultadoSemanal?.totalCriados || datasRecorrentes.length);

            } else if (periodoRecorrente === "M")
            {
                // MENSAL: Repete no mesmo dia do mês
                // ✅ CORREÇÃO 20/01/2026: lstDiasMes agora é Kendo DropDownList
                const lstDiasMesKendo = $("#lstDiasMes").data("kendoDropDownList");
                const diaMes = lstDiasMesKendo ? lstDiasMesKendo.value() : null;
                const dataInicial = getKendoDatePickerValue("txtDataInicial");
                const dataFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");

                if (!diaMes)
                {
                    throw new Error("Selecione o dia do mês para recorrência mensal");
                }

                const datasRecorrentes = [];
                let dataAtual = new Date(dataInicial);
                const dataFim = new Date(dataFinalRecorrencia);

                while (dataAtual <= dataFim)
                {
                    datasRecorrentes.push(window.toDateOnlyString(dataAtual));
                    // Avança para o próximo mês
                    dataAtual.setMonth(dataAtual.getMonth() + 1);
                }

                const resultadoMensal = await window.handleRecurrence(periodoRecorrente, datasRecorrentes);
                window.exibirMensagemSucesso(resultadoMensal?.totalCriados || datasRecorrentes.length);

            } else if (periodoRecorrente === "V")
            {
                // VARIADA: Usa as datas específicas selecionadas no calendário
                const calDatasSelecionadas = document.getElementById("calDatasSelecionadas")?.ej2_instances?.[0];
                const datasSelecionadas = calDatasSelecionadas?.values || [];

                if (datasSelecionadas.length === 0)
                {
                    throw new Error("Selecione as datas para agendamento variado");
                }

                const datasFormatadas = datasSelecionadas.map(d =>
                    window.toDateOnlyString(new Date(d))
                );

                const resultadoVariado = await window.handleRecurrence(periodoRecorrente, datasFormatadas);
                window.exibirMensagemSucesso(resultadoVariado?.totalCriados || datasFormatadas.length);

            } else
            {
                throw new Error("Tipo de recorrência não reconhecido: " + periodoRecorrente);
            }

        } catch (error)
        {
            console.error("❌ Erro em handleCriarNovoAgendamento:", error);
            Alerta.Erro("Erro ao criar agendamento", error.message);
        }
    }

    /**
     * Handle registrar viagem
     */
    async function handleRegistrarViagem(viagemId)
    {
        try
        {
            window.transformandoEmViagem = true;

            const agendamentoUnicoAlterado = await window.recuperarViagemEdicao(viagemId);
            let objViagem = window.criarAgendamentoViagem(agendamentoUnicoAlterado);

            const response = await fetch("/api/Agenda/Agendamento", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(objViagem)
            });

            const data = await response.json();

            if (data.success)
            {
                AppToast.show("Verde", "Viagem Criada com Sucesso", 2000);
                fecharModalESucesso();
            } else
            {
                AppToast.show("Vermelho", "Erro ao Criar Viagem", 2000);
                Alerta.Erro("Erro ao criar Agendamento", "Não foi possível criar a Viagem com os dados informados");
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "handleRegistrarViagem", error);
        }
    }

    /**
     * Handle editar agendamento
     */
    async function handleEditarAgendamento(viagemId)
    {
        try
        {
            window._recorrenciaOverlayAtivo = false;
            const isAberta = await new Promise((resolve, reject) =>
            {
                try
                {
                    $.ajax({
                        url: `/api/Viagem/PegarStatusViagem`,
                        type: "GET",
                        data: { viagemId: viagemId },
                        success: function (resultado)
                        {
                            try
                            {
                                resolve(resultado);
                            } catch (error)
                            {
                                Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_successResolve", error);
                                reject(error);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown)
                        {
                            const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                            Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_error", erro);
                            reject(erro);
                        }
                    });
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento_ajax", error);
                    reject(error);
                }
            });

            if (isAberta)
            {
                await window.editarAgendamento(viagemId);
                return;
            }

            const objViagem = await window.recuperarViagemEdicao(viagemId);

            if (objViagem?.recorrente === "S")
            {
                const confirmacao = await Alerta.Confirmar(
                    "Editar Agendamento Recorrente",
                    "Deseja aplicar as alterações a todos os agendamentos recorrentes ou apenas ao atual?",
                    "Todos",
                    "Apenas ao Atual"
                );

                window.editarTodosRecorrentes = confirmacao;

                if (confirmacao)
                {
                    window._recorrenciaOverlayAtivo = true;
                    FtxSpin.show("Atualizando agendamentos recorrentes");
                }

                await window.editarAgendamentoRecorrente(
                    viagemId,
                    confirmacao,
                    objViagem.dataInicial,
                    objViagem.recorrenciaViagemId,
                    window.editarTodosRecorrentes
                );
            } else
            {
                await window.editarAgendamento(viagemId);
            }
        } catch (error)
        {
            if (window._recorrenciaOverlayAtivo)
            {
                FtxSpin.hide();
                window._recorrenciaOverlayAtivo = false;
            }
            Alerta.TratamentoErroComLinha("main.js", "handleEditarAgendamento", error);
        }
    }

    /**
     * Fecha modal e atualiza calendário
     */
    function fecharModalESucesso()
    {
        try
        {
            $("#modalViagens").modal("hide");
            $(document.body).removeClass("modal-open");
            $(".modal-backdrop").remove();
            $(document.body).css("overflow", "");
            window.calendar.refetchEvents();
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "fecharModalESucesso", error);
        }
    }

    /**
     * Configura botão de viagem
     */
    function configurarBotaoViagem()
    {
        try
        {
            $("#btnViagem").click(function (event)
            {
                try
                {
                    event.preventDefault();
                    $("#btnViagem").hide();
                    $("#btnConfirma").html("<i class='fa fa-save' aria-hidden='true'></i>Registra Viagem");

                    // divNoFichaVistoria removido - agora usa botão ao lado do Destino (21/01/2026)
                    const camposViagem = [
                        "divDataFinal", "divHoraFinal", "divDuracao",
                        "divKmAtual", "divKmInicial", "divKmFinal", "divQuilometragem",
                        "divCombustivelInicial", "divCombustivelFinal"
                    ];

                    camposViagem.forEach(id =>
                    {
                        const el = document.getElementById(id);
                        if (el) el.style.display = "block";
                    });

                    // Buscar Km do veículo selecionado
                    const lstVeiculo = document.getElementById("lstVeiculo");
                    if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0])
                    {
                        const veiculoId = lstVeiculo.ej2_instances[0].value;
                        if (veiculoId)
                        {
                            $.ajax({
                                url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
                                method: "GET",
                                datatype: "json",
                                data: { id: veiculoId },
                                success: function (res)
                                {
                                    try
                                    {
                                        const km = res.data || 0;
                                        $("#txtKmAtual").val(km);
                                        $("#txtKmInicial").val(km);
                                        console.log("✅ Km do Veículo carregado ao transformar:", km);
                                    } catch (innerError)
                                    {
                                        Alerta.TratamentoErroComLinha("main.js", "btnViagem_ajax_success", innerError);
                                    }
                                },
                                error: function (jqXHR, textStatus, errorThrown)
                                {
                                    const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                                    Alerta.TratamentoErroComLinha("main.js", "btnViagem_ajax_error", erro);
                                }
                            });
                        }
                    }

                    $("#txtKmFinal").val("");
                    $("#txtStatusAgendamento").val(false);
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "btnViagem_click", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarBotaoViagem", error);
        }
    }

    /**
     * Configura botão de apagar
     */
    function configurarBotaoApagar()
    {
        try
        {
            $("#btnApaga").click(async function (event)
            {
                try
                {
                    const viagemId = document.getElementById("txtViagemId").value;
                    const recorrenciaViagemId = document.getElementById("txtRecorrenciaViagemId").value;

                    let titulo = "";
                    if (recorrenciaViagemId != null && recorrenciaViagemId != "" && recorrenciaViagemId != "00000000-0000-0000-0000-000000000000")
                    {
                        titulo = "Você gostaria de apagar todos os agendamentos recorrentes? Ou somente o atual?";
                    } else
                    {
                        titulo = "Você gostaria de apagar este agendamento?";
                    }

                    const confirmacao = await Alerta.Confirmar(titulo, "Não será possível recuperar os dados eliminados", "Apagar Todos", "Apenas Atual");

                    if (confirmacao)
                    {
                        // CORREÇÃO: Usar novo endpoint otimizado para deletar todos de uma vez
                        let recorrenciaId = recorrenciaViagemId;
                        if (recorrenciaViagemId === "00000000-0000-0000-0000-000000000000")
                        {
                            recorrenciaId = viagemId;
                        }

                        // Chamar novo endpoint que deleta todos de uma vez (mais eficiente e sem erro de FK)
                        const result = await window.AgendamentoService.excluirRecorrentes(recorrenciaId);

                        if (result.success)
                        {
                            AppToast.show("Verde", result.message || "Todos os agendamentos foram excluídos com sucesso!", 3000);
                        }
                        else
                        {
                            Alerta.Erro("Erro ao Excluir", result.message || result.error || "Erro desconhecido", "OK");
                        }
                    } else
                    {
                        await window.excluirAgendamento(viagemId);
                        AppToast.show("Verde", "O agendamento foi excluído com sucesso!", 3000);
                    }

                    fecharModalESucesso();
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "btnApaga_click", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarBotaoApagar", error);
        }
    }
    /**
     * Configura botão de imprimir
     */
    function configurarBotaoImprimir()
    {
        try
        {
            $("#btnImprime").click(function (event)
            {
                try
                {
                    const viagemId = document.getElementById("txtViagemId").value;

                    $("#fichaReport").telerik_ReportViewer({
                        serviceUrl: "/api/reports/",
                        reportSource: {
                            report: "Agendamento.trdp",
                            parameters: {
                                ViagemId: viagemId.toString().toUpperCase()
                            }
                        },
                        viewMode: telerikReportViewer.ViewModes.PRINT_PREVIEW,
                        scaleMode: telerikReportViewer.ScaleModes.SPECIFIC,
                        scale: 1.0,
                        enableAccessibility: false,
                        sendEmail: {
                            enabled: true
                        }
                    });
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "btnImprime_click", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarBotaoImprimir", error);
        }
    }

    /**
     * Configura botões de fechar modais
     */
    function configurarBotõesFechar()
    {
        try
        {
            $("#btnFecha").on("click", function ()
            {
                try
                {
                    $("#modalViagens").modal("hide");
                    $(document.body).removeClass("modal-open");
                    $(".modal-backdrop").remove();
                    $(document.body).css("overflow", "");
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "btnFecha_click", error);
                }
            });

            $("#btnFecharFicha").on("click", function ()
            {
                try
                {
                    $("#modalPrint").modal("hide");
                    $(document.body).removeClass("modal-open");
                    $(".modal-backdrop").remove();
                    $(document.body).css("overflow", "");
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "btnFecharFicha_click", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarBotõesFechar", error);
        }
    }

    /**
     * Inicialização do módulo de relatório
     */
    function inicializarRelatorio()
    {
        // Verifica se as funções principais do módulo estão disponíveis
        const funcoesRelatorio = {
            carregarRelatorioViagem: typeof window.carregarRelatorioViagem,
            mostrarRelatorio: typeof window.mostrarRelatorio,
            esconderRelatorio: typeof window.esconderRelatorio,
            limparRelatorio: typeof window.limparRelatorio,
            obterEstadoRelatorio: typeof window.obterEstadoRelatorio
        };

        // Verifica se todas as funções estão definidas
        const todasDisponiveis = Object.values(funcoesRelatorio).every(tipo => tipo === 'function');

        if (todasDisponiveis)
        {
            console.log('[Main] ✅ Módulo de relatório carregado com sucesso');
            console.log('[Main] Funções disponíveis:', funcoesRelatorio);
            return true;
        } else
        {
            console.error('[Main] ❌ Módulo de relatório NÃO encontrado ou incompleto!');
            console.error('[Main] Status das funções:', funcoesRelatorio);
            return false;
        }
    }

    // ====================================================================
    // CONFIGURAÇÃO DE VALIDAÇÕES EM CAMPOS
    // ====================================================================

    /**
     * Configura validações de campos
     */
    function configurarValidacoesCampos()
    {
        try
        {
            // Data Final - VALIDAÇÃO IA
            $("#txtDataFinal").focusout(async function ()
            {
                try
                {
                    const DataFinal = $("#txtDataFinal").val();
                    if (DataFinal === "") return;

                    // Calcular duração primeiro
                    window.calcularDuracaoViagem();

                    // Validação IA (se disponível)
                    if (typeof ValidadorFinalizacaoIA !== 'undefined')
                    {
                        const validador = ValidadorFinalizacaoIA.obterInstancia();

                        // Validar data não futura (bloqueante)
                        const resultadoDataFutura = await validador.validarDataNaoFutura(DataFinal);
                        if (!resultadoDataFutura.valido)
                        {
                            await Alerta.Erro(resultadoDataFutura.titulo, resultadoDataFutura.mensagem);
                            $("#txtDataFinal").val("");
                            return;
                        }

                        // Validar datas e horas com IA (não bloqueante - apenas aviso)
                        const DataInicial = getKendoDatePickerValue("txtDataInicial");
                        const HoraInicial = $("#txtHoraInicial").val();
                        const HoraFinal = $("#txtHoraFinal").val();

                        if (DataInicial && HoraInicial && HoraFinal)
                        {
                            const dadosDatas = {
                                dataInicial: DataInicial,
                                horaInicial: HoraInicial,
                                dataFinal: DataFinal,
                                horaFinal: HoraFinal
                            };

                            const resultadoDatas = await validador.analisarDatasHoras(dadosDatas);
                            if (!resultadoDatas.valido && resultadoDatas.nivel === 'erro')
                            {
                                await Alerta.Erro(resultadoDatas.titulo, resultadoDatas.mensagem);
                                $("#txtDataFinal").val("");
                                return;
                            }
                            else if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
                            {
                                const confirma = await Alerta.ValidacaoIAConfirmar(
                                    resultadoDatas.titulo,
                                    resultadoDatas.mensagem,
                                    "Manter Data",
                                    "Corrigir"
                                );
                                if (!confirma)
                                {
                                    $("#txtDataFinal").val("");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Fallback: validação simples
                        const DataInicial = getKendoDatePickerValue("txtDataInicial");
                        if (DataFinal < DataInicial)
                        {
                            Alerta.Erro("Atenção", "A data final deve ser maior que a inicial");
                            $("#txtDataFinal").val("");
                        }
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtDataFinal_focusout", error);
                }
            });

            // Data Inicial - FocusOut
            $(document).on("focusout", "#txtDataInicial", function ()
            {
                try
                {
                    const selectedDate = getKendoDatePickerValue("txtDataInicial");

                    if (!selectedDate)
                    {
                        console.warn("⚠️ txtDataInicial não tem valor selecionado");
                        return;
                    }

                    // Atualizar data final mínima usando Kendo API
                    var txtDataFinalPicker = $("#txtDataFinal").data("kendoDatePicker");
                    if (txtDataFinalPicker)
                    {
                        txtDataFinalPicker.min(selectedDate);
                    }

                    window.calcularDuracaoViagem();
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtDataInicial_focusout", error);
                }
            });

            // Hora Final - VALIDAÇÃO IA
            $("#txtHoraFinal").focusout(async function ()
            {
                try
                {
                    const HoraFinal = $("#txtHoraFinal").val();
                    const DataFinal = $("#txtDataFinal").val();

                    if (DataFinal === "")
                    {
                        $("#txtHoraFinal").val("");
                        Alerta.Erro("Atenção", "Preencha a Data Final para poder preencher a Hora Final");
                        return;
                    }

                    if (HoraFinal === "") return;

                    // Calcular duração primeiro
                    window.calcularDuracaoViagem();

                    // Validação IA (se disponível)
                    if (typeof ValidadorFinalizacaoIA !== 'undefined')
                    {
                        const validador = ValidadorFinalizacaoIA.obterInstancia();

                        const DataInicial = getKendoDatePickerValue("txtDataInicial");
                        const HoraInicial = $("#txtHoraInicial").val();

                        if (DataInicial && HoraInicial)
                        {
                            const dadosDatas = {
                                dataInicial: DataInicial,
                                horaInicial: HoraInicial,
                                dataFinal: DataFinal,
                                horaFinal: HoraFinal
                            };

                            const resultadoDatas = await validador.analisarDatasHoras(dadosDatas);
                            if (!resultadoDatas.valido && resultadoDatas.nivel === 'erro')
                            {
                                await Alerta.Erro(resultadoDatas.titulo, resultadoDatas.mensagem);
                                $("#txtHoraFinal").val("");
                                return;
                            }
                            else if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
                            {
                                const confirma = await Alerta.ValidacaoIAConfirmar(
                                    resultadoDatas.titulo,
                                    resultadoDatas.mensagem,
                                    "Manter Hora",
                                    "Corrigir"
                                );
                                if (!confirma)
                                {
                                    $("#txtHoraFinal").val("");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Fallback: validação simples
                        const HoraInicial = $("#txtHoraInicial").val();
                        const DataInicial = getKendoDatePickerValue("txtDataInicial");

                        if (HoraFinal < HoraInicial && DataInicial === DataFinal)
                        {
                            $("#txtHoraFinal").val("");
                            Alerta.Erro("Atenção", "A hora final deve ser maior que a inicial");
                        }
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtHoraFinal_focusout", error);
                }
            });

            // Hora Inicial
            $("#txtHoraInicial").focusout(function ()
            {
                try
                {
                    const HoraInicial = $("#txtHoraInicial").val();
                    const HoraFinal = $("#txtHoraFinal").val();

                    if (HoraFinal === "") return;

                    if (HoraInicial > HoraFinal)
                    {
                        $("#txtHoraInicial").val("");
                        Alerta.Erro("Atenção", "A hora inicial deve ser menor que a final");
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtHoraInicial_focusout", error);
                }
            });

            // KM Inicial
            $("#txtKmInicial").focusout(function ()
            {
                try
                {
                    const kmInicialStr = $("#txtKmInicial").val();
                    const kmAtualStr = $("#txtKmAtual").val();

                    if (!kmInicialStr || !kmAtualStr)
                    {
                        $("#txtKmPercorrido").val("");
                        return;
                    }

                    const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
                    const kmAtual = parseFloat(kmAtualStr.replace(",", "."));

                    if (isNaN(kmInicial) || isNaN(kmAtual))
                    {
                        $("#txtKmPercorrido").val("");
                        return;
                    }

                    if (kmInicial < 0)
                    {
                        $("#txtKmInicial").val("");
                        $("#txtKmPercorrido").val("");
                        Alerta.Erro("Erro na Quilometragem", "A quilometragem <strong>inicial</strong> deve ser maior que <strong>zero</strong>!");
                        return;
                    }

                    if (kmInicial < kmAtual)
                    {
                        $("#txtKmInicial").val("");
                        $("#txtKmPercorrido").val("");
                        Alerta.Erro("Erro na Quilometragem", "A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>!");
                        return;
                    }

                    if (kmInicial != kmAtual)
                    {
                        Alerta.Erro("Erro na Quilometragem", "A quilometragem inicial não confere com a atual");
                        return;
                    }

                    window.calcularDistanciaViagem();
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtKmInicial_focusout", error);
                }
            });

            // KM Final - VALIDAÇÃO IA
            $("#txtKmFinal").focusout(async function ()
            {
                try
                {
                    const kmInicialStr = $("#txtKmInicial").val();
                    const kmFinalStr = $("#txtKmFinal").val();

                    if (!kmFinalStr) return;

                    const kmInicial = parseInt(kmInicialStr) || 0;
                    const kmFinal = parseInt(kmFinalStr) || 0;

                    // Validação básica: KM final < inicial (bloqueante)
                    if (kmFinal < kmInicial)
                    {
                        $("#txtKmFinal").val("");
                        Alerta.Erro("Erro na Quilometragem", "A quilometragem final deve ser maior que a inicial");
                        return;
                    }

                    // Calcular distância
                    window.calcularDistanciaViagem();

                    // Validação IA (se disponível)
                    if (typeof ValidadorFinalizacaoIA !== 'undefined')
                    {
                        const validador = ValidadorFinalizacaoIA.obterInstancia();

                        // Obter veiculoId do modal
                        const veiculoId = document.getElementById("lstVeiculo")?.ej2_instances?.[0]?.value || '';

                        if (veiculoId && kmInicial > 0 && kmFinal > 0)
                        {
                            const dadosKm = {
                                kmInicial: kmInicial,
                                kmFinal: kmFinal,
                                veiculoId: veiculoId
                            };

                            const resultadoKm = await validador.analisarKm(dadosKm);
                            if (!resultadoKm.valido)
                            {
                                if (resultadoKm.nivel === 'erro')
                                {
                                    await Alerta.Erro(resultadoKm.titulo, resultadoKm.mensagem);
                                    $("#txtKmFinal").val("");
                                    window.calcularDistanciaViagem();
                                    return;
                                }
                                else if (resultadoKm.nivel === 'aviso')
                                {
                                    const confirma = await Alerta.ValidacaoIAConfirmar(
                                        resultadoKm.titulo,
                                        resultadoKm.mensagem,
                                        "Manter KM",
                                        "Corrigir"
                                    );
                                    if (!confirma)
                                    {
                                        $("#txtKmFinal").val("");
                                        window.calcularDistanciaViagem();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtKmFinal_focusout", error);
                }
            });

            // Ficha de Vistoria
            $("#txtNoFichaVistoria").focusout(function ()
            {
                try
                {
                    const noFicha = $("#txtNoFichaVistoria").val();
                    if (noFicha === "") return;

                    // Verificar diferença
                    window.ViagemService.verificarFicha(noFicha).then(result =>
                    {
                        if (result.success && result.diferencaGrande)
                        {
                            Alerta.Warning("Alerta na Ficha de Vistoria", "O número inserido difere em ±100 da última Ficha inserida");
                        }
                    });

                    // Verificar se existe
                    window.ViagemService.fichaExiste(noFicha).then(result =>
                    {
                        if (result.success && result.existe)
                        {
                            Alerta.Warning("Alerta na Ficha de Vistoria", "Já existe uma Ficha inserida com esta numeração");
                        }
                    });
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtNoFichaVistoria_focusout", error);
                }
            });

            // Data Final Recorrência
            $("#txtFinalRecorrencia").focusout(function ()
            {
                try
                {
                    const txtDataInicial = getKendoDatePickerValue("txtDataInicial");
                    const txtFinalRecorrencia = getKendoDatePickerValue("txtFinalRecorrencia");

                    if (txtDataInicial && txtFinalRecorrencia)
                    {
                        const dataInicial = moment(txtDataInicial, "DD-MM-YYYY");
                        const dataFinal = moment(txtFinalRecorrencia, "DD-MM-YYYY");
                        const diferencaDias = dataFinal.diff(dataInicial, "days");

                        if (diferencaDias > 365)
                        {
                            Alerta.Warning("Atenção", "A data final não pode ser maior que 365 dias após a data inicial");
                            $("#txtFinalRecorrencia").val("");
                        }
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "txtFinalRecorrencia_focusout", error);
                }
            });

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarValidacoesCampos", error);
        }
    }

    // ====================================================================
    // CONFIGURAÇÃO DE MODAIS
    // ====================================================================

    /**
     * Configura eventos de modais
     */
    function configurarModais()
    {
        try
        {
            /**
             * Helper: largura fixa + compacto nos campos de Recorrência
             */
            function RecorrenciasCompactar()
            {
                try
                {
                    // Mapa de campos (larguras podem ser ajustadas aqui)
                    var campos = [
                        { id: "lstRecorrente", cls: "sw-recorrente", w: "128px" },
                        { id: "lstPeriodos", cls: "sw-periodo", w: "220px" },
                        { id: "lstDiasMes", cls: "sw-diasmes", w: "140px" },
                        { id: "lstDias", cls: "sw-dias", w: "100%" },
                        { id: "txtFinalRecorrencia", cls: "sw-final", w: "100%" }
                    ];

                    campos.forEach(function (c)
                    {
                        try
                        {
                            var el = document.getElementById(c.id);
                            if (!el)
                            {
                                console.warn(`⚠️ Elemento ${c.id} não encontrado`);
                                return;
                            }

                            // ============================================
                            // TRATAMENTO ESPECIAL PARA CONTROLES KENDO
                            // lstRecorrente, lstPeriodos, lstDiasMes, lstDias e txtFinalRecorrencia são Kendo
                            // ============================================

                            // 1. Kendo DatePicker
                            if (c.id === "txtFinalRecorrencia")
                            {
                                try
                                {
                                    var kendoDP = $("#txtFinalRecorrencia").data("kendoDatePicker");
                                    if (kendoDP)
                                    {
                                        var wrapper = kendoDP.wrapper;
                                        if (wrapper && wrapper.length)
                                        {
                                            wrapper.css({
                                                "width": c.w,
                                                "min-height": "34px",
                                                "height": "34px"
                                            });

                                            wrapper.addClass("e-small");

                                            var input = wrapper.find("input.k-input-inner");
                                            if (input.length)
                                            {
                                                input.css({
                                                    "height": "34px",
                                                    "line-height": "34px",
                                                    "padding-top": "0",
                                                    "padding-bottom": "0"
                                                });
                                            }

                                            console.log("✅ txtFinalRecorrencia (Kendo DatePicker) compactado");
                                        }
                                    }
                                }
                                catch (errKendo)
                                {
                                    console.error("❌ Erro ao compactar txtFinalRecorrencia:", errKendo.message);
                                }
                                return;
                            }

                            // 2. Kendo DropDownList (lstRecorrente, lstPeriodos, lstDiasMes)
                            if (c.id === "lstRecorrente" || c.id === "lstPeriodos" || c.id === "lstDiasMes")
                            {
                                try
                                {
                                    var kendoDDL = $("#" + c.id).data("kendoDropDownList");
                                    if (kendoDDL)
                                    {
                                        var wrapper = kendoDDL.wrapper;
                                        if (wrapper && wrapper.length)
                                        {
                                            wrapper.css({
                                                "width": c.w,
                                                "min-height": "34px",
                                                "height": "34px"
                                            });
                                            wrapper.addClass("e-small");
                                            console.log("✅ " + c.id + " (Kendo DropDownList) compactado");
                                        }
                                    }
                                }
                                catch (errKendo)
                                {
                                    // Silencioso - componente pode não estar inicializado ainda
                                }
                                return;
                            }

                            // 3. Kendo MultiSelect (lstDias)
                            if (c.id === "lstDias")
                            {
                                try
                                {
                                    var kendoMS = $("#lstDias").data("kendoMultiSelect");
                                    if (kendoMS)
                                    {
                                        var wrapper = kendoMS.wrapper;
                                        if (wrapper && wrapper.length)
                                        {
                                            wrapper.css({
                                                "width": c.w,
                                                "min-height": "34px"
                                            });
                                            wrapper.addClass("e-small");
                                            console.log("✅ lstDias (Kendo MultiSelect) compactado");
                                        }
                                    }
                                }
                                catch (errKendo)
                                {
                                    // Silencioso - componente pode não estar inicializado ainda
                                }
                                return;
                            }

                            // ============================================
                            // TRATAMENTO PADRÃO PARA SYNCFUSION
                            // (nenhum dos campos acima chegará aqui)
                            // ============================================
                            if (!el.ej2_instances || !Array.isArray(el.ej2_instances) || el.ej2_instances.length === 0)
                            {
                                console.warn(`⚠️ ${c.id} não tem instância Syncfusion inicializada`);
                                return;
                            }

                            var inst = el.ej2_instances[0];

                            if (!inst)
                            {
                                console.warn(`⚠️ ${c.id}: instância é null ou undefined`);
                                return;
                            }

                            if (typeof inst.setProperties !== 'function')
                            {
                                console.warn(`⚠️ ${c.id}: setProperties não é uma função`);
                                return;
                            }

                            if (!inst.element || inst.isDestroyed)
                            {
                                console.warn(`⚠️ ${c.id}: componente destruído ou sem elemento`);
                                return;
                            }

                            // 1) Marca o CONTAINER do campo
                            var cont = el.closest(".select-wrapper") ||
                                el.closest("[class*='col-']") ||
                                el.parentElement;

                            if (cont && !cont.classList.contains(c.cls))
                            {
                                cont.classList.add(c.cls);
                            }

                            // 2) Ajustes EJ2 (largura fixa + compacto)
                            try
                            {
                                inst.setProperties({ width: c.w });
                                inst.cssClass = (inst.cssClass || "") + " e-small";

                                // 3) Wrapper real do EJ2
                                var wrapper = (inst.inputWrapper && inst.inputWrapper.container) ||
                                    inst.overAllWrapper ||
                                    el.nextElementSibling;

                                if (wrapper)
                                {
                                    wrapper.style.setProperty("width", "100%", "important");
                                    wrapper.style.setProperty("display", "inline-flex", "important");
                                    wrapper.style.setProperty("align-items", "center", "important");
                                    wrapper.style.setProperty("min-height", "34px", "important");
                                    wrapper.style.setProperty("height", "34px", "important");
                                }

                                // 4) Entrada interna
                                var input = wrapper?.querySelector("input.e-input");
                                if (input)
                                {
                                    input.style.setProperty("height", "34px", "important");
                                    input.style.setProperty("line-height", "34px", "important");
                                    input.style.setProperty("padding-top", "0", "important");
                                    input.style.setProperty("padding-bottom", "0", "important");
                                }

                                // 5) Ícone do dropdown
                                var icon = wrapper?.querySelector(".e-input-group-icon");
                                if (icon)
                                {
                                    icon.style.setProperty("height", "34px", "important");
                                    icon.style.setProperty("min-height", "34px", "important");
                                    icon.style.setProperty("display", "flex", "important");
                                    icon.style.setProperty("align-items", "center", "important");
                                }

                                if (typeof inst.dataBind === 'function')
                                {
                                    inst.dataBind();
                                }

                                console.log(`✅ ${c.id} compactado com sucesso`);

                            } catch (errSetProperties)
                            {
                                console.error(`❌ Erro ao aplicar setProperties em ${c.id}:`, errSetProperties.message);
                            }
                        }
                        catch (errEach)
                        {
                            console.error(`❌ Erro ao processar campo ${c.id}:`, errEach.message);
                        }
                    });

                    console.log("✅ RecorrenciasCompactar concluído");

                }
                catch (error)
                {
                    console.error("❌ Erro geral em RecorrenciasCompactar:", error);
                    if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
                    {
                        Alerta.TratamentoErroComLinha("main.js", "RecorrenciasCompactar", error);
                    }
                }
            }

            // ====================================================================
            // MODAL VIAGENS - EVENTO DE ABERTURA (AJUSTADO)
            // NOTA: Usando flag para evitar execucao multipla em vez de .off()
            // ====================================================================
            $("#modalViagens").on("shown.bs.modal", function (event)
            {
                try
                {
                    // Evitar execucao multipla usando flag
                    if (window._mainHandlerExecutando) return;
                    window._mainHandlerExecutando = true;
                    setTimeout(() => { window._mainHandlerExecutando = false; }, 500);

                    // RESETAR FLAGS IMPORTANTES
                    window.modalJaFoiLimpo = false;
                    // window.ignorarEventosRecorrencia = false; // Comentado - flag controlada pelo fluxo

                    // ============================================
                    // LIMPEZA PREVENTIVA DE OVERLAYS (20/01/2026)
                    // Garante que nenhum overlay está bloqueando o modal
                    // ============================================

                    // Remover overlay de loading do relatório
                    $('#modal-relatorio-loading-overlay').remove();

                    // Esconder FtxSpin
                    if (window.FtxSpin) window.FtxSpin.hide();

                    // Remover spinners e overlays Syncfusion órfãos
                    document.querySelectorAll('.e-spinner-pane, .e-spin-overlay, .e-spin-show, .e-overlay').forEach(function(el) {
                        // Não remover se for filho de um componente ativo
                        if (!el.closest('.e-dialog') && !el.closest('.e-popup-open')) {
                            try { el.remove(); } catch(e) { el.style.display = 'none'; }
                        }
                    });

                    // Remover overlays Kendo
                    $('.k-overlay, .k-loading-mask').remove();

                    // Remover overlays ftx órfãos (exceto o principal)
                    $('.ftx-spin-overlay').each(function() {
                        if (this !== window.FtxSpin?._el) $(this).remove();
                    });

                    // Esconder divs de eventos e desabilitar focusin do modal
                    $(".esconde-diveventos").hide();
                    $(document).off("focusin.modal");

                    // ============================================
                    // 1. INICIALIZAR EVENT HANDLERS DOS CONTROLES
                    // ============================================
                    if (window.aguardarControlesEInicializar)
                    {
                        window.aguardarControlesEInicializar();
                    }

                    // ============================================
                    // 2. VERIFICAR SE É NOVO OU EDIÇÃO
                    // ============================================
                    const viagemId = document.getElementById("txtViagemId").value;
                    const isEdicao = viagemId && viagemId !== "" && viagemId !== "00000000-0000-0000-0000-000000000000";

                    console.log("📋 Modo:", isEdicao ? "Edição" : "Novo Agendamento");
                    console.log("📋 ViagemId:", viagemId);
                    console.log("📋 Flag carregandoViagemExistente:", window.carregandoViagemExistente);

                    // ============================================
                    // 3. INICIALIZAR CONTROLES DE RECORRÊNCIA
                    // ============================================
                    setTimeout(() =>
                    {
                        // Inicializar controles de recorrência (dias, meses, data final)
                        if (window.inicializarControlesRecorrencia)
                        {
                            console.log("🔧 Inicializando controles de recorrência...");
                            if (!window.carregandoViagemExistente)
                            {
                                console.log("🔧 Inicializando controles de recorrência...");
                                if (typeof window.inicializarRecorrencia === 'function')
                                {
                                    window.inicializarRecorrencia();
                                }
                                console.log("✅ Controles inicializados");
                            } else
                            {
                                console.log("⚠️ Pulando inicialização - carregando dados existentes");
                            }
                        }

                        // Inicializar dropdown de períodos
                        if (window.inicializarDropdownPeriodos)
                        {
                            console.log("🔧 Inicializando dropdown de períodos...");
                            window.inicializarDropdownPeriodos();
                        }
                        else
                        {
                            console.warn("⚠️ Função inicializarDropdownPeriodos não encontrada");
                        }

                        console.log("✅ Controles de recorrência inicializados");

                        // ============================================
                        // 4. AGUARDAR RENDERIZAÇÃO COMPLETA E COMPACTAR
                        // ============================================
                        setTimeout(() =>
                        {
                            console.log("🔧 Aplicando compactação dos campos...");

                            // Após render inicial dos wrappers EJ2, forçar largura/compacto
                            requestAnimationFrame(() =>
                            {
                                requestAnimationFrame(() =>
                                {
                                    RecorrenciasCompactar();
                                });
                            });
                        }, 800); // Aguardar 800ms para renderização completa

                        // ============================================
                        // 5. INICIALIZAR LÓGICA DE RECORRÊNCIA
                        // ============================================
                        setTimeout(() =>
                        {
                            // VERIFICAÇÃO CRÍTICA: Só inicializar se NÃO estiver carregando dados existentes
                            if (!window.carregandoViagemExistente)
                            {
                                if (window.inicializarLogicaRecorrencia)
                                {
                                    console.log("🔧 Inicializando lógica de recorrência (novo agendamento)...");
                                    window.inicializarLogicaRecorrencia();
                                }

                                // Se for novo agendamento, definir valor padrão
                                // CRÍTICO: Verificar TAMBÉM a flag carregandoViagemExistente
                                if (!isEdicao && !window.carregandoViagemExistente)
                                {
                                    console.log("🆕 Modo: Novo Agendamento");
                                    window.setModalTitle('NOVO_AGENDAMENTO');
                                    // [REGRA] Valor padrão e estado de recorrência são definidos pelo RecorrenciaController
                                }
                            } else
                            {
                                console.log("⚠️ Pulando inicialização de recorrência - carregando dados existentes");

                                // [REGRA] Sincronização de recorrência gerenciada pelo RecorrenciaController
                            }
                        }, 1200); // Aguardar controles renderizarem

                    }, 500);

                    // ============================================
                    // 6. CÁLCULOS INICIAIS
                    // ============================================
                    if (window.calcularDistanciaViagem)
                    {
                        window.calcularDistanciaViagem();
                    }

                    if (window.calcularDuracaoViagem)
                    {
                        window.calcularDuracaoViagem();
                    }

                    // ============================================
                    // 7. AJUSTAR DATA MÍNIMA (data de hoje)
                    // ============================================
                    const novaDataMinima = new Date();
                    // Zerar horas para comparar apenas a data
                    novaDataMinima.setHours(0, 0, 0, 0);

                    // ✅ CORREÇÃO 20/01/2026: txtDataInicial agora é Kendo DatePicker
                    const txtDataInicialKendo = $("#txtDataInicial").data("kendoDatePicker");
                    if (txtDataInicialKendo)
                    {
                        txtDataInicialKendo.min(novaDataMinima);
                        console.log("✅ Data mínima ajustada para hoje (Kendo):", novaDataMinima);
                    }

                    // ============================================
                    // 8. CONFIGURAR EVENTO SELECT DO REQUISITANTE (KENDO)
                    // ============================================
                    setTimeout(() =>
                    {
                        console.log('🎯 [MODAL] Configurando SELECT do requisitante (Kendo)...');

                        const lstRequisitante = document.getElementById('lstRequisitante');

                        if (lstRequisitante)
                        {
                            console.log('  ✅ Elemento lstRequisitante encontrado');

                            // Telerik Kendo: usa $(element).data("kendoComboBox")
                            const kendoComboBox = $(lstRequisitante).data("kendoComboBox");

                            if (kendoComboBox)
                            {
                                console.log('  ✅ Kendo ComboBox encontrado');

                                // CONFIGURAR evento SELECT (Kendo)
                                kendoComboBox.bind("select", function (e)
                                {
                                    console.log('🎉 [MODAL-SELECT] Evento SELECT Kendo disparou!');
                                    console.log('DataItem:', e.dataItem);

                                    if (window.onSelectRequisitante)
                                    {
                                        console.log('  ✅ Chamando window.onSelectRequisitante...');
                                        const args = {
                                            itemData: e.dataItem,
                                            value: e.dataItem ? e.dataItem.RequisitanteId : null
                                        };
                                        window.onSelectRequisitante(args);
                                    }
                                    else
                                    {
                                        console.error('  ❌ window.onSelectRequisitante NÃO EXISTE!');
                                    }
                                });

                                // CONFIGURAR evento CHANGE (para compatibilidade)
                                kendoComboBox.bind("change", function (e)
                                {
                                    console.log('🔍 [MODAL-CHANGE] Evento CHANGE Kendo disparou!');
                                    if (window.RequisitanteValueChange)
                                    {
                                        const args = {
                                            value: kendoComboBox.value(),
                                            text: kendoComboBox.text()
                                        };
                                        window.RequisitanteValueChange(args);
                                    }
                                });

                                console.log('  ✅✅✅ [MODAL] Eventos Kendo configurados com SUCESSO!');
                            }
                            else
                            {
                                console.warn('  ⚠️ Kendo ComboBox não inicializado ainda');
                            }
                        }
                        else
                        {
                            console.error('  ❌ Elemento lstRequisitante não encontrado');
                        }

                    }, 2000); // 2 segundos após o modal abrir

                    // ============================================
                    // INTERVALO DE LIMPEZA DE OVERLAYS (20/01/2026)
                    // Remove overlays órfãos a cada 500ms enquanto modal está aberto
                    // ============================================
                    if (window._overlayCleanupInterval) {
                        clearInterval(window._overlayCleanupInterval);
                    }
                    window._overlayCleanupInterval = setInterval(function() {
                        // Só executar se modal ainda estiver aberto
                        if (!$('#modalViagens').hasClass('show')) {
                            clearInterval(window._overlayCleanupInterval);
                            window._overlayCleanupInterval = null;
                            return;
                        }
                        // Remover overlays Syncfusion órfãos no body (fora do modal)
                        // CRÍTICO: e-overlay do Syncfusion bloqueia toda interação
                        $('body > .e-overlay, body > .e-spinner-pane').each(function() {
                            console.log('[Limpeza] Removendo overlay órfão:', this.className);
                            $(this).remove();
                        });
                    }, 500); // Reduzido para 500ms para capturar mais rápido

                    // ============================================
                    // LISTENERS NOS COMBOBOXES SYNCFUSION (20/01/2026)
                    // Limpa overlays quando popup fecha
                    // ============================================
                    const syncfusionComboIds = ['lstVeiculo', 'lstMotorista', 'lstFinalidade', 'cmbOrigem', 'cmbDestino', 'lstEventos'];
                    syncfusionComboIds.forEach(function(id) {
                        const el = document.getElementById(id);
                        if (el && el.ej2_instances && el.ej2_instances[0]) {
                            const inst = el.ej2_instances[0];
                            // Remover listener anterior se existir
                            if (inst._overlayCleanupBound) return;
                            inst._overlayCleanupBound = true;

                            // Evento close: limpar overlays órfãos
                            inst.close = (function(originalClose) {
                                return function() {
                                    if (originalClose) originalClose.apply(this, arguments);
                                    // Aguardar animação e limpar overlays
                                    setTimeout(function() {
                                        $('body > .e-overlay').each(function() {
                                            console.log('[ComboBox Close] Removendo overlay órfão');
                                            $(this).remove();
                                        });
                                    }, 100);
                                };
                            })(inst.close);

                            console.log('[Overlay] Listener de close configurado para:', id);
                        }
                    });

                    console.log("✅ Modal Viagens inicializado com sucesso");

                }
                catch (error)
                {
                    console.error("❌ Erro ao inicializar Modal Viagens:", error);
                    Alerta.TratamentoErroComLinha("main.js", "modalViagens_shown", error);
                }
            }).on("hide.bs.modal", function (event)
            {
                try
                {
                    console.log("🚪 Modal Viagens fechando...");

                    // Parar intervalo de limpeza de overlays
                    if (window._overlayCleanupInterval) {
                        clearInterval(window._overlayCleanupInterval);
                        window._overlayCleanupInterval = null;
                    }

                    // Remover todos os overlays órfãos ao fechar
                    $('body > .e-overlay, body > .e-spinner-pane, body > .e-spin-overlay').remove();

                    // ✅ LIMPAR RELATÓRIO
                    if (window.limparRelatorio)
                    {
                        window.limparRelatorio();
                    }

                    // Limpar viewer de ficha
                    $("#fichaReport").remove();
                    $("#ReportContainer").append("<div id='fichaReport' style='width:100%' class='pb-3'> Carregando... </div>");

                    // Remover backdrop e classes do modal
                    $(document.body).removeClass("modal-open");
                    $(".modal-backdrop").remove();
                    $(document.body).css("overflow", "");

                    console.log("✅ Modal Viagens fechado");

                }
                catch (error)
                {
                    console.error("❌ Erro ao fechar Modal Viagens:", error);
                    Alerta.TratamentoErroComLinha("main.js", "modalViagens_hide", error);
                }
            });


            // Modal Requisitante
            $("#modalRequisitante").modal({
                keyboard: true,
                backdrop: "static",
                show: false
            }).on("hide.bs.modal", function ()
            {
                try
                {
                    const setores = document.getElementById("ddtSetorRequisitante").ej2_instances[0];
                    setores.value = "";
                    $("#txtPonto, #txtNome, #txtRamal, #txtEmail").val("");
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "modalRequisitante_hide", error);
                }
            });

            // Modal Setor
            $("#modalSetor").modal({
                keyboard: true,
                backdrop: "static",
                show: false
            }).on("hide.bs.modal", function ()
            {
                try
                {
                    $("#txtSigla, #txtNomeSetor, #txtRamalSetor").val("");
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "modalSetor_hide", error);
                }
            });

            // Prevenir focus do Bootstrap em modais
            if ($.fn.modal && $.fn.modal.Constructor)
            {
                $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarModais", error);
        }
    }

    // ====================================================================
    // CONFIGURAÇÃO DE ACCORDIONS
    // ====================================================================

    /**
     * Configura accordions
     */
    // ⚠️ ACCORDION REMOVIDO: Função desabilitada após migração para modal Bootstrap
    /*
    function configurarAccordions()
    {
        try
        {
            const modalRequisitante = document.getElementById("modalRequisitante");
            if (modalRequisitante)
            {
                return;
            }
            // Accordion Requisitante
            const accordionContainer = $("#accordionRequisitante");
            accordionContainer.hide();

            let isAnimating = false;

            $("#btnRequisitante").off("click").on("click", function (e)
            {
                e.preventDefault();
                e.stopPropagation();

                if (isAnimating) return false;
                isAnimating = true;

                if (accordionContainer.is(":visible"))
                {
                    accordionContainer.slideUp(300, function ()
                    {
                        isAnimating = false;
                    });
                } else
                {
                    accordionContainer.slideDown(300, function ()
                    {
                        isAnimating = false;
                    });
                }

                return false;
            });

            $("#btnFecharAccordionRequisitante").off("click").on("click", function (e)
            {
                e.preventDefault();
                e.stopPropagation();

                if (isAnimating) return false;
                isAnimating = true;

                accordionContainer.slideUp(300, function ()
                {
                    isAnimating = false;
                    try
                    {
                        const setores = document.getElementById("ddtSetorRequisitante").ej2_instances[0];
                        if (setores) setores.value = "";
                        $("#txtPonto, #txtNome, #txtRamal, #txtEmail").val("");
                    } catch (error)
                    {
                        console.error("Erro ao limpar campos:", error);
                    }
                });

                return false;
            });

            // Bloquear eventos do accordion Syncfusion
            accordionContainer.find(".e-acrdn-header").off("click").on("click", function (e)
            {
                e.preventDefault();
                e.stopPropagation();
                return false;
            });

            // Fechar ao clicar fora
            $(document).on("click", function (event)
            {
                try
                {
                    const accordionElement = document.getElementById("accordionRequisitante");
                    const btnRequisitante = document.getElementById("btnRequisitante");

                    if (!accordionElement.contains(event.target) && event.target !== btnRequisitante)
                    {
                        accordionContainer.slideUp(300);
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "accordion_clickOutside", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarAccordions", error);
        }
    }
    */

    // ====================================================================
    // CONFIGURAÇÃO DE REQUISITANTE E EVENTO
    // ====================================================================

    /**
     * Configura botão inserir requisitante
     * ⚠️ DESABILITADO: Lógica movida para requisitante.service.js (modal Bootstrap)
     */
    /*
    function configurarInserirRequisitante()
    {
        try
        {
            $("#btnInserirRequisitante").click(function (e)
            {
                try
                {
                    e.preventDefault();

                    if ($("#txtPonto").val() === "")
                    {
                        Alerta.Erro("Atenção", "O Ponto do Requisitante é obrigatório");
                        return;
                    }

                    if ($("#txtNome").val() === "")
                    {
                        Alerta.Erro("Atenção", "O Nome do Requisitante é obrigatório");
                        return;
                    }

                    if ($("#txtRamal").val() === "")
                    {
                        Alerta.Erro("Atenção", "O Ramal do Requisitante é obrigatório");
                        return;
                    }

                    const setores = document.getElementById("ddtSetorRequisitante").ej2_instances[0];
                    if (setores.value.toString() === "")
                    {
                        Alerta.Erro("Atenção", "O Setor do Requisitante é obrigatório");
                        return;
                    }

                    const objRequisitante = {
                        Nome: $("#txtNome").val(),
                        Ponto: $("#txtPonto").val(),
                        Ramal: $("#txtRamal").val(),
                        Email: $("#txtEmail").val(),
                        SetorSolicitanteId: setores.value.toString()
                    };

                    window.RequisitanteService.adicionar(objRequisitante).then(result =>
                    {
                        if (result.success)
                        {
                            AppToast.show("Verde", result.message, 2000);

                            document.getElementById("lstRequisitante").ej2_instances[0].addItem({
                                RequisitanteId: result.requisitanteId,
                                Requisitante: $("#txtNome").val() + " - " + $("#txtPonto").val()
                            }, 0);

                            const modalReq = document.getElementById("modalRequisitante");
                            if (modalReq && window.bootstrap && window.bootstrap.Modal)
                            {
                                window.bootstrap.Modal.getOrCreateInstance(modalReq).hide();
                            }

                            const comboBoxInstance = document.getElementById("lstRequisitante").ej2_instances[0];
                            comboBoxInstance.value = result.requisitanteId;
                            comboBoxInstance.dataBind();
                        } else
                        {
                            AppToast.show("Vermelho", result.message, 2000);
                        }
                    });
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "btnInserirRequisitante_click", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarInserirRequisitante", error);
        }
    }
    */

    /**
     * Configura botão inserir evento
     */
    function configurarInserirEvento()
    {
        try
        {
            $("#btnInserirEvento").click(async function (e)
            {
                try
                {
                    e.preventDefault();

                    if ($("#txtNomeEvento").val() === "")
                    {
                        Alerta.Erro("Atenção", "O Nome do Evento é obrigatório");
                        return;
                    }

                    if ($("#txtDescricaoEvento").val() === "")
                    {
                        Alerta.Erro("Atenção", "A Descrição do Evento é obrigatória");
                        return;
                    }

                    if ($("#txtDataInicialEvento").val() === "")
                    {
                        Alerta.Erro("Atenção", "A Data Inicial é obrigatória");
                        return;
                    }

                    if ($("#txtDataFinalEvento").val() === "")
                    {
                        Alerta.Erro("Atenção", "A Data Final é obrigatória");
                        return;
                    }

                    if ($("#txtQtdPessoas").val() === "")
                    {
                        Alerta.Erro("Atenção", "A Quantidade de Pessoas é obrigatória");
                        return;
                    }

                    const setores = document.getElementById("ddtSetorEvento");
                    if (!setores || !setores.ej2_instances || !setores.ej2_instances[0] || setores.ej2_instances[0].value === null)
                    {
                        Alerta.Erro("Atenção", "O Setor do Requisitante é obrigatório");
                        return;
                    }

                    const requisitantes = document.getElementById("lstRequisitanteEvento");
                    if (!requisitantes || !requisitantes.ej2_instances || !requisitantes.ej2_instances[0] || requisitantes.ej2_instances[0].value === null)
                    {
                        Alerta.Erro("Atenção", "O Requisitante é obrigatório");
                        return;
                    }

                    const objEvento = {
                        Nome: $("#txtNomeEvento").val(),
                        Descricao: $("#txtDescricaoEvento").val(),
                        SetorSolicitanteId: setores.ej2_instances[0].value.toString(),
                        RequisitanteId: requisitantes.ej2_instances[0].value.toString(),
                        QtdParticipantes: $("#txtQtdPessoas").val(),
                        DataInicial: moment($("#txtDataInicialEvento").val()).format("MM-DD-YYYY"),
                        DataFinal: moment($("#txtDataFinalEvento").val()).format("MM-DD-YYYY"),
                        Status: "1"
                    };

                    document.body.style.cursor = "wait";

                    try
                    {
                        const result = await window.EventoService.adicionar(objEvento);

                        if (result.success)
                        {
                            AppToast.show("Verde", result.message, 2000);

                            await window.EventoService.atualizarListaDropdown(result.eventoId);

                            if (typeof window.fecharFormularioCadastroEvento === 'function')
                            {
                                window.fecharFormularioCadastroEvento();
                            }
                            else
                            {
                                const modalEventoEl = document.getElementById("modalEvento");
                                if (modalEventoEl && window.bootstrap && window.bootstrap.Modal)
                                {
                                    window.bootstrap.Modal.getOrCreateInstance(modalEventoEl).hide();
                                }
                            }
                        } else
                        {
                            AppToast.show("Vermelho", result.message, 2000);
                        }
                    } finally
                    {
                        document.body.style.cursor = "default";
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("main.js", "btnInserirEvento_click", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "configurarInserirEvento", error);
        }
    }

    // ====================================================================
    // CARREGAMENTO DE DADOS INICIAIS
    // ====================================================================

    /**
     * Carrega dados iniciais
     */
    function carregarDadosIniciais()
    {
        try
        {
            console.log("📊 Carregando dados iniciais...");

            // Preencher lista de setores
            window.ViagemService.listarSetores().then(result =>
            {
                if (result.success)
                {
                    const ddtSetor = document.getElementById("ddtSetor");
                    const ddtSetorReq = document.getElementById("ddtSetorRequisitante");

                    if (ddtSetor && ddtSetor.ej2_instances && ddtSetor.ej2_instances[0])
                    {
                        ddtSetor.ej2_instances[0].fields.dataSource = result.data;
                        ddtSetor.ej2_instances[0].refresh();
                    }

                    if (ddtSetorReq && ddtSetorReq.ej2_instances && ddtSetorReq.ej2_instances[0])
                    {
                        ddtSetorReq.ej2_instances[0].fields.dataSource = result.data;
                        ddtSetorReq.ej2_instances[0].refresh();
                    }
                }
            });

            // Preencher lista de eventos
            window.EventoService.listar().then(result =>
            {
                if (result.success)
                {
                    const lstEventos = document.getElementById("lstEventos");
                    if (lstEventos && lstEventos.ej2_instances && lstEventos.ej2_instances[0])
                    {
                        lstEventos.ej2_instances[0].fields.dataSource = result.data;
                        lstEventos.ej2_instances[0].refresh();
                    }
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "carregarDadosIniciais", error);
        }
    }

    // ====================================================================
    // INICIALIZAÇÃO PRINCIPAL
    // ====================================================================

    /**
     * Função principal de inicialização
     */
    function inicializar()
    {
        try
        {
            console.log("⚡ Iniciando configuração...");

            // 1. Configurar localização
            configurarLocalizacao();

            // 2. Inicializar tooltips
            inicializarTooltips();

            // 3. Configurar event handlers
            configurarBotaoConfirmar();
            configurarBotaoViagem();
            configurarBotaoApagar();
            configurarBotaoImprimir();
            configurarBotõesFechar();

            // 4. Configurar validações
            configurarValidacoesCampos();

            // 5. Configurar modais
            configurarModais();

            // 6. Configurar accordions
            // ⚠️ DESABILITADO: Accordions removidos, migrado 100% para modais Bootstrap
            // configurarAccordions();

            // 7. Configurar requisitante e evento
            // ⚠️ DESABILITADO: Lógica movida para requisitante.service.js (modal Bootstrap)
            // configurarInserirRequisitante();
            configurarInserirEvento();

            // 8. Inicializar calendário
            console.log("📅 Inicializando calendário...");
            window.InitializeCalendar("api/Agenda/CarregaViagens");

            // 9. Carregar dados iniciais
            carregarDadosIniciais();


            // ✅ ADICIONE ESTE BLOCO:
            setTimeout(function ()
            {
                try
                {
                    console.log("🎯 Inicializando Sistema de Evento...");
                    inicializarSistemaEvento();
                } catch (error)
                {
                    console.error("❌ Erro ao inicializar Sistema de Evento:", error);
                }
            }, 500);

            // Inicializa relatório
            const relatorioOk = inicializarRelatorio();

            if (!relatorioOk)
            {
                AppToast.show("Amarelo", '[Main] ⚠️ Sistema funcionará sem o módulo de relatório', 3000);
                console.warn('[Main] ⚠️ Sistema funcionará sem o módulo de relatório');
            }

            inicializarRamalTextBox();

            // ✅ NOVO 20/01/2026: Inicializar AgendamentoCore (controles Kendo de recorrência)
            setTimeout(function () {
                if (window.AgendamentoCore && typeof window.AgendamentoCore.inicializar === "function") {
                    console.log("🎯 Inicializando AgendamentoCore...");
                    window.AgendamentoCore.inicializar();
                } else {
                    console.warn("⚠️ AgendamentoCore não encontrado");
                }
            }, 300);

            // Aguardar um pouco para garantir que os componentes Syncfusion estejam carregados
            setTimeout(function ()
            {
                inicializarEventoSelect();
            }, 500);

            // Aguardar o modal estar totalmente renderizado
            setTimeout(function ()
            {
                if (window.onLstMotoristaCreated)
                {
                    window.onLstMotoristaCreated();
                }
            }, 200);

            console.log("✅ Sistema de Agendamento inicializado com sucesso!");

        } catch (error)
        {
            console.error("❌ Erro fatal ao inicializar sistema:", error);
            Alerta.TratamentoErroComLinha("main.js", "inicializar", error);
        }
    }

    /**
 * Inicializa o dropdown de dias do mês (1-31)
 * ✅ ATUALIZADO 20/01/2026: Usar Kendo DropDownList
 */
    function inicializarLstDiasMes()
    {
        try
        {
            // ✅ CORREÇÃO 20/01/2026: lstDiasMes agora é Kendo DropDownList
            const lstDiasMesKendo = $("#lstDiasMes").data("kendoDropDownList");

            if (!lstDiasMesKendo)
            {
                console.warn("⚠️ lstDiasMes (Kendo) não inicializado - pode ser renderizado posteriormente");
                return false;
            }

            // Criar array com dias de 1 a 31
            const diasDoMes = [];
            for (let i = 1; i <= 31; i++)
            {
                diasDoMes.push({
                    value: i,
                    text: i.toString()
                });
            }

            // Definir dataSource via Kendo API
            lstDiasMesKendo.setDataSource(new kendo.data.DataSource({ data: diasDoMes }));

            console.log("✅ lstDiasMes populado com 31 dias (Kendo)");
            return true;

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "inicializarLstDiasMes", error);
            return false;
        }
    }

    /**
 * Inicializa o multiselect de dias da semana
 * ✅ ATUALIZADO 20/01/2026: Usar Kendo MultiSelect
 */
    function inicializarLstDias()
    {
        try
        {
            // ✅ CORREÇÃO 20/01/2026: lstDias agora é Kendo MultiSelect
            const lstDiasKendo = $("#lstDias").data("kendoMultiSelect");

            if (!lstDiasKendo)
            {
                console.warn("⚠️ lstDias (Kendo) não inicializado - pode ser renderizado posteriormente");
                return false;
            }

            // Dias da semana
            const diasDaSemana = [
                { value: 0, text: "Domingo" },
                { value: 1, text: "Segunda" },
                { value: 2, text: "Terça" },
                { value: 3, text: "Quarta" },
                { value: 4, text: "Quinta" },
                { value: 5, text: "Sexta" },
                { value: 6, text: "Sábado" }
            ];

            // Definir dataSource via Kendo API
            lstDiasKendo.setDataSource(new kendo.data.DataSource({ data: diasDaSemana }));

            console.log("✅ lstDias populado com dias da semana (Kendo)");
            return true;

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("main.js", "inicializarLstDias", error);
        }
    }

    // ====================================================================
    // AUTO-INICIALIZAÇÃO
    // ====================================================================

    // Aguardar DOM ready
    $(document).ready(function ()
    {
        inicializar();

        console.log('[Main] Inicialização completa');

        console.log('[Main] === DEBUG: FUNÇÕES GLOBAIS DE RELATÓRIO ===');
        console.log('carregarRelatorioViagem:', typeof window.carregarRelatorioViagem);
        console.log('mostrarRelatorio:', typeof window.mostrarRelatorio);
        console.log('esconderRelatorio:', typeof window.esconderRelatorio);
        console.log('limparRelatorio:', typeof window.limparRelatorio);
        console.log('obterEstadoRelatorio:', typeof window.obterEstadoRelatorio);
        console.log('===========================================');

    });

})();

// Correção forçada via JavaScript
document.addEventListener('DOMContentLoaded', function ()
{
    // Aguardar Syncfusion renderizar
    setTimeout(function ()
    {
        // Forçar display block em todos os DropDownTrees
        const dropdowns = document.querySelectorAll('.e-dropdowntree.e-control, .e-combobox.e-control');
        dropdowns.forEach(el =>
        {
            el.style.display = 'block';
            el.style.width = '100%';
        });

        // Corrigir containers flex
        const flexCols = document.querySelectorAll('.d-flex.flex-column.ml-2');
        flexCols.forEach(el =>
        {
            el.style.display = 'block';
        });

        // Corrigir labels
        const labelContainers = document.querySelectorAll('.d-flex.align-items-center.mb-1');
        labelContainers.forEach(el =>
        {
            el.style.display = 'block';
            el.style.marginBottom = '8px';
        });
    }, 500);
});

// ===== INICIALIZAR TEXTBOX SYNCFUSION PARA RAMAL =====
function inicializarRamalTextBox()
{
    const ramalElement = document.getElementById('txtRamalRequisitanteSF');

    if (!ramalElement)
    {
        console.warn('⚠️ Campo txtRamalRequisitanteSF não encontrado');
        return;
    }

    // Destruir instância anterior se existir
    if (ramalElement.ej2_instances && ramalElement.ej2_instances[0])
    {
        ramalElement.ej2_instances[0].destroy();
    }

    // Criar novo TextBox Syncfusion
    const ramalTextBox = new ej.inputs.TextBox({
        placeholder: 'Ex: 1234',
        floatLabelType: 'Never',
        cssClass: 'e-outline',
        maxLength: 10,
        enabled: true,
        readonly: false
    });

    ramalTextBox.appendTo('#txtRamalRequisitanteSF');

    console.log('✅ TextBox Ramal Syncfusion inicializado');

    return ramalTextBox;
}

// ====================================================================
// CORREÇÃO CIRÊRGICA - BORDA INFERIOR DOS CONTROLES
// ====================================================================
window.corrigirBordaInferior = function ()
{
    try
    {
        const controlIds = ['lstFinalidade', 'ddtSetorRequisitante'];

        controlIds.forEach(id =>
        {
            const elemento = document.getElementById(id);
            if (elemento)
            {
                const wrapper = elemento.querySelector('.e-ddt-wrapper');
                if (wrapper)
                {
                    wrapper.style.borderBottom = '1px solid #ced4da';
                }
            }
        });

    } catch (error)
    {
        console.error('Erro ao corrigir borda inferior:', error);
    }
};

// Aplicar correcao quando modal abrir (registrado uma vez)
if (!window._bordaInferiorListenerRegistrado) {
    $(document).on('shown.bs.modal', '#modalViagens', function ()
    {
        setTimeout(() => window.corrigirBordaInferior(), 200);
    });
    window._bordaInferiorListenerRegistrado = true;
}

// ====================================================================
// EVENTO CRITICO: LIMPAR RELATORIO AO FECHAR MODAL
// ====================================================================
// Previne erro "collapsible" do Telerik/Kendo ao abrir/fechar modal rapidamente
if (!window._limparRelatorioListenerRegistrado) {
    $(document).on('hidden.bs.modal', '#modalViagens', async function ()
    {
    try
    {
        console.log('[Main] 🧹 Modal fechado - limpando...');

        // GARANTIR que overlay seja removido
        if (typeof window.esconderLoadingRelatorio === 'function')
        {
            window.esconderLoadingRelatorio();
        }

        await new Promise(resolve => setTimeout(resolve, 200));

        // Limpar o relatório
        if (typeof window.limparRelatorio === 'function')
        {
            await window.limparRelatorio();
            console.log('[Main] ✅ Relatório limpo com sucesso');
        }

        // Resetar flags de estado
        window.modalIsOpening = false;
        window.isReportViewerLoading = false;

        if (window.modalDebounceTimer)
        {
            clearTimeout(window.modalDebounceTimer);
            window.modalDebounceTimer = null;
        }

    } catch (error)
    {
        console.error('[Main] Erro no evento hidden.bs.modal:', error);
        // GARANTIR remocao do overlay mesmo com erro
        if (typeof window.esconderLoadingRelatorio === 'function')
        {
            window.esconderLoadingRelatorio();
        }
    }
    });
    window._limparRelatorioListenerRegistrado = true;
}

// Aplicar na inicializacao
$(document).ready(function ()
{
    setTimeout(() => window.corrigirBordaInferior(), 500);
});
