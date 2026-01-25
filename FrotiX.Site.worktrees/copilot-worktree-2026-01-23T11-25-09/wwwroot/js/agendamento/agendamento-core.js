/**
 * ============================================================================
 * AGENDAMENTO CORE - Sistema Unificado de Agendamento FrotiX
 * ============================================================================
 *
 * Arquivo criado em: 20/01/2026
 * Autor: Claude Code
 *
 * OBJETIVO:
 * Este arquivo substitui TODOS os arquivos JavaScript de agendamento antigos.
 * Contém toda a lógica de inicialização, controle de recorrência, validação
 * e manipulação de dados em um único módulo bem estruturado.
 *
 * CONTROLES KENDO (criados por este arquivo):
 * - txtDataInicial (DatePicker)
 * - txtFinalRecorrencia (DatePicker)
 * - lstRecorrente (DropDownList)
 * - lstPeriodos (DropDownList)
 * - lstDias (MultiSelect)
 * - lstDiasMes (DropDownList)
 *
 * CONTROLES SYNCFUSION (mantidos - definidos no CSHTML):
 * - calDatasSelecionadas (Calendar)
 * - lstMotorista, lstVeiculo, lstFinalidade, etc.
 *
 * ============================================================================
 */

(function (window, $) {
    "use strict";

    // ========================================================================
    // SEÇÃO 1: CONFIGURAÇÃO GLOBAL
    // ========================================================================

    const CONFIG = {
        // IDs dos elementos HTML
        elementos: {
            // Campos de data/hora
            txtDataInicial: "#txtDataInicial",
            txtDataFinal: "#txtDataFinal",
            txtHoraInicial: "#txtHoraInicial",
            txtHoraFinal: "#txtHoraFinal",
            txtFinalRecorrencia: "#txtFinalRecorrencia",

            // Campos de recorrência (KENDO)
            lstRecorrente: "#lstRecorrente",
            lstPeriodos: "#lstPeriodos",
            lstDias: "#lstDias",
            lstDiasMes: "#lstDiasMes",

            // Calendário Syncfusion (mantido)
            calDatasSelecionadas: "#calDatasSelecionadas",
            lstDatasVariadas: "#lstDatasVariadas",
            lblSelecioneAsDatas: "#lblSelecioneAsDatas",
            badgeCalendario: "#badgeContadorDatas",
            badgeDatasVariadas: "#badgeContadorDatasVariadas",

            // Containers de visibilidade
            divPeriodo: "#divPeriodo",
            divDias: "#divDias",
            divDiasMes: "#divDiaMes",
            divFinalRecorrencia: "#divFinalRecorrencia",
            divCalendario: "#calendarContainer",
            divListaDatas: "#listboxDatasVariadasContainer"
        },

        // DataSources para os dropdowns
        dataSources: {
            recorrente: [
                { value: "N", text: "Não" },
                { value: "S", text: "Sim" }
            ],
            periodos: [
                { value: "D", text: "Diário" },
                { value: "S", text: "Semanal" },
                { value: "Q", text: "Quinzenal" },
                { value: "M", text: "Mensal" },
                { value: "V", text: "Dias Variados" }
            ],
            diasSemana: [
                { value: 0, text: "Domingo" },
                { value: 1, text: "Segunda" },
                { value: 2, text: "Terça" },
                { value: 3, text: "Quarta" },
                { value: 4, text: "Quinta" },
                { value: 5, text: "Sexta" },
                { value: 6, text: "Sábado" }
            ],
            diasMes: [] // Preenchido dinamicamente (1-31)
        },

        // Configurações de comportamento
        debug: true,
        tentativasMaximas: 3,
        intervaloRetentativa: 150
    };

    // Preencher dias do mês (1-31)
    for (let i = 1; i <= 31; i++) {
        CONFIG.dataSources.diasMes.push({ value: i, text: i.toString() });
    }

    // ========================================================================
    // SEÇÃO 2: UTILITÁRIOS
    // ========================================================================

    const Utils = {
        /**
         * Log condicional (apenas se debug está ativo)
         */
        log: function (...args) {
            if (CONFIG.debug) {
                console.log("[AgendamentoCore]", ...args);
            }
        },

        /**
         * Log de erro (sempre exibido)
         */
        erro: function (metodo, error) {
            console.error(`[AgendamentoCore] Erro em ${metodo}:`, error);
            if (typeof Alerta !== "undefined" && Alerta.TratamentoErroComLinha) {
                Alerta.TratamentoErroComLinha("agendamento-core.js", metodo, error);
            }
        },

        /**
         * Verifica se um elemento existe no DOM
         */
        elementoExiste: function (seletor) {
            return $(seletor).length > 0;
        },

        /**
         * Aguarda elemento estar disponível no DOM
         */
        aguardarElemento: function (seletor, timeout = 5000) {
            return new Promise((resolve, reject) => {
                const intervalo = 100;
                let tempoDecorrido = 0;

                const verificar = () => {
                    if ($(seletor).length > 0) {
                        resolve($(seletor));
                    } else if (tempoDecorrido >= timeout) {
                        reject(new Error(`Elemento ${seletor} não encontrado após ${timeout}ms`));
                    } else {
                        tempoDecorrido += intervalo;
                        setTimeout(verificar, intervalo);
                    }
                };

                verificar();
            });
        },

        /**
         * Formata data para exibição (dd/MM/yyyy)
         */
        formatarData: function (data) {
            if (!data) return "";
            const d = new Date(data);
            if (isNaN(d.getTime())) return "";
            const dia = String(d.getDate()).padStart(2, "0");
            const mes = String(d.getMonth() + 1).padStart(2, "0");
            const ano = d.getFullYear();
            return `${dia}/${mes}/${ano}`;
        },

        /**
         * Converte string dd/MM/yyyy para Date
         */
        parseData: function (str) {
            if (!str) return null;
            if (str instanceof Date) return str;
            const partes = str.split("/");
            if (partes.length === 3) {
                return new Date(partes[2], partes[1] - 1, partes[0]);
            }
            return new Date(str);
        },

        /**
         * ╭──────────────────────────────────────────────────────────────────────────────
         * │ FUNCIONALIDADE: Controla a visibilidade de containers de recorrÇ¦ncia,
         * │                 aplicando display com !important para superar CSS fixo.
         * │──────────────────────────────────────────────────────────────────────────────
         * │ PARÂMETROS:
         * │    -> seletor: Seletor CSS do elemento alvo.
         * │    -> mostrar: True para exibir, false para ocultar.
         * │    -> displayPadrao: Display a aplicar ao exibir (block, flex, etc.).
         * │──────────────────────────────────────────────────────────────────────────────
         */
        mostrar: function (seletor, mostrar = true, displayPadrao = "block") {
            try {
                const $el = $(seletor);
                if (!$el.length) {
                    return;
                }

                const displayFinal = mostrar ? displayPadrao : "none";

                $el.each(function () {
                    this.style.setProperty("display", displayFinal, "important");
                });
            } catch (error) {
                Utils.erro("Utils.mostrar", error);
            }
        }
    };

    // ========================================================================
    // SEÇÃO 3: MÓDULO DE CONTROLES KENDO
    // ========================================================================

    const Controles = {
        // Referências aos widgets Kendo
        _widgets: {
            txtDataInicial: null,
            txtFinalRecorrencia: null,
            lstRecorrente: null,
            lstPeriodos: null,
            lstDias: null,
            lstDiasMes: null
        },

        /**
         * Inicializa todos os controles Kendo
         */
        inicializar: function () {
            Utils.log("Inicializando controles Kendo...");

            try {
                this._criarDatePickerDataInicial();
                this._criarDatePickerFinalRecorrencia();
                this._criarDropdownRecorrente();
                this._criarDropdownPeriodos();
                this._criarMultiSelectDias();
                this._criarDropdownDiasMes();

                Utils.log("Todos os controles Kendo inicializados com sucesso");
                return true;
            } catch (error) {
                Utils.erro("Controles.inicializar", error);
                return false;
            }
        },

        /**
         * Cria DatePicker para Data Inicial
         */
        _criarDatePickerDataInicial: function () {
            const $el = $(CONFIG.elementos.txtDataInicial);
            if (!$el.length) {
                Utils.log("txtDataInicial não encontrado no DOM");
                return;
            }

            // Verificar se já existe instância Kendo
            if ($el.data("kendoDatePicker")) {
                this._widgets.txtDataInicial = $el.data("kendoDatePicker");
                Utils.log("txtDataInicial já inicializado (Kendo)");
                return;
            }

            // Criar novo DatePicker
            $el.kendoDatePicker({
                format: "dd/MM/yyyy",
                dateInput: true,
                change: function (e) {
                    Eventos.onDataInicialChange(e);
                }
            });

            this._widgets.txtDataInicial = $el.data("kendoDatePicker");
            Utils.log("txtDataInicial criado (DatePicker)");
        },

        /**
         * Cria DatePicker para Data Final de Recorrência
         */
        _criarDatePickerFinalRecorrencia: function () {
            const $el = $(CONFIG.elementos.txtFinalRecorrencia);
            if (!$el.length) {
                Utils.log("txtFinalRecorrencia não encontrado no DOM");
                return;
            }

            // Verificar se já existe instância Kendo
            if ($el.data("kendoDatePicker")) {
                this._widgets.txtFinalRecorrencia = $el.data("kendoDatePicker");
                Utils.log("txtFinalRecorrencia já inicializado (Kendo)");
                return;
            }

            // Criar novo DatePicker
            $el.kendoDatePicker({
                format: "dd/MM/yyyy",
                dateInput: true,
                min: new Date(),
                change: function (e) {
                    Eventos.onFinalRecorrenciaChange(e);
                }
            });

            this._widgets.txtFinalRecorrencia = $el.data("kendoDatePicker");
            Utils.log("txtFinalRecorrencia criado (DatePicker)");
        },

        /**
         * Cria DropDownList para Recorrente (Sim/Não)
         */
        _criarDropdownRecorrente: function () {
            const $el = $(CONFIG.elementos.lstRecorrente);
            if (!$el.length) {
                Utils.log("lstRecorrente não encontrado no DOM");
                return;
            }

            // Destruir instância anterior se existir
            const existente = $el.data("kendoDropDownList");
            if (existente) {
                existente.destroy();
                $el.empty();
            }

            // Criar novo DropDownList
            $el.kendoDropDownList({
                dataSource: CONFIG.dataSources.recorrente,
                dataTextField: "text",
                dataValueField: "value",
                value: "N",
                change: function (e) {
                    Eventos.onRecorrenteChange(e);
                }
            });

            this._widgets.lstRecorrente = $el.data("kendoDropDownList");
            Utils.log("lstRecorrente criado (DropDownList)");
        },

        /**
         * Cria DropDownList para Período
         */
        _criarDropdownPeriodos: function () {
            const $el = $(CONFIG.elementos.lstPeriodos);
            if (!$el.length) {
                Utils.log("lstPeriodos não encontrado no DOM");
                return;
            }

            // Destruir instância anterior se existir
            const existente = $el.data("kendoDropDownList");
            if (existente) {
                existente.destroy();
                $el.empty();
            }

            // Criar novo DropDownList
            $el.kendoDropDownList({
                dataSource: CONFIG.dataSources.periodos,
                dataTextField: "text",
                dataValueField: "value",
                optionLabel: "Selecione o período...",
                change: function (e) {
                    Eventos.onPeriodoChange(e);
                }
            });

            this._widgets.lstPeriodos = $el.data("kendoDropDownList");
            Utils.log("lstPeriodos criado (DropDownList)");
        },

        /**
         * Cria MultiSelect para Dias da Semana
         */
        _criarMultiSelectDias: function () {
            const $el = $(CONFIG.elementos.lstDias);
            if (!$el.length) {
                Utils.log("lstDias não encontrado no DOM");
                return;
            }

            // Destruir instância anterior se existir
            const existente = $el.data("kendoMultiSelect");
            if (existente) {
                existente.destroy();
                $el.empty();
            }

            // Criar novo MultiSelect
            $el.kendoMultiSelect({
                dataSource: CONFIG.dataSources.diasSemana,
                dataTextField: "text",
                dataValueField: "value",
                placeholder: "Selecione os dias...",
                autoClose: false,
                change: function (e) {
                    Eventos.onDiasChange(e);
                }
            });

            this._widgets.lstDias = $el.data("kendoMultiSelect");
            Utils.log("lstDias criado (MultiSelect)");
        },

        /**
         * Cria DropDownList para Dia do Mês
         */
        _criarDropdownDiasMes: function () {
            const $el = $(CONFIG.elementos.lstDiasMes);
            if (!$el.length) {
                Utils.log("lstDiasMes não encontrado no DOM");
                return;
            }

            // Destruir instância anterior se existir
            const existente = $el.data("kendoDropDownList");
            if (existente) {
                existente.destroy();
                $el.empty();
            }

            // Criar novo DropDownList
            $el.kendoDropDownList({
                dataSource: CONFIG.dataSources.diasMes,
                dataTextField: "text",
                dataValueField: "value",
                optionLabel: "Dia...",
                change: function (e) {
                    Eventos.onDiaMesChange(e);
                }
            });

            this._widgets.lstDiasMes = $el.data("kendoDropDownList");
            Utils.log("lstDiasMes criado (DropDownList)");
        },

        // ====================================================================
        // GETTERS - Obter valores dos controles
        // ====================================================================

        /**
         * Obtém valor de um controle Kendo por ID
         */
        getValor: function (id) {
            const widget = this._widgets[id];
            if (widget) {
                return widget.value();
            }

            // Fallback: tentar obter via jQuery
            const $el = $(CONFIG.elementos[id] || "#" + id);
            const kendo = $el.data("kendoDropDownList") ||
                          $el.data("kendoMultiSelect") ||
                          $el.data("kendoDatePicker");
            if (kendo) {
                return kendo.value();
            }

            // Último recurso: valor HTML
            return $el.val();
        },

        /**
         * Obtém data inicial
         */
        getDataInicial: function () {
            const widget = this._widgets.txtDataInicial ||
                           $(CONFIG.elementos.txtDataInicial).data("kendoDatePicker");
            return widget ? widget.value() : null;
        },

        /**
         * Obtém data final de recorrência
         */
        getDataFinalRecorrencia: function () {
            const widget = this._widgets.txtFinalRecorrencia ||
                           $(CONFIG.elementos.txtFinalRecorrencia).data("kendoDatePicker");
            return widget ? widget.value() : null;
        },

        /**
         * Obtém valor de recorrente (S/N)
         */
        getRecorrente: function () {
            const widget = this._widgets.lstRecorrente ||
                           $(CONFIG.elementos.lstRecorrente).data("kendoDropDownList");
            return widget ? widget.value() : "N";
        },

        /**
         * Obtém período selecionado
         */
        getPeriodo: function () {
            const widget = this._widgets.lstPeriodos ||
                           $(CONFIG.elementos.lstPeriodos).data("kendoDropDownList");
            return widget ? widget.value() : "";
        },

        /**
         * Obtém dias da semana selecionados
         */
        getDiasSemana: function () {
            const widget = this._widgets.lstDias ||
                           $(CONFIG.elementos.lstDias).data("kendoMultiSelect");
            return widget ? widget.value() : [];
        },

        /**
         * Obtém dia do mês selecionado
         */
        getDiaMes: function () {
            const widget = this._widgets.lstDiasMes ||
                           $(CONFIG.elementos.lstDiasMes).data("kendoDropDownList");
            return widget ? widget.value() : null;
        },

        // ====================================================================
        // SETTERS - Definir valores dos controles
        // ====================================================================

        /**
         * Define valor de um controle Kendo por ID
         */
        setValor: function (id, valor) {
            const widget = this._widgets[id];
            if (widget) {
                widget.value(valor);
                return true;
            }

            // Fallback: tentar via jQuery
            const $el = $(CONFIG.elementos[id] || "#" + id);
            const kendo = $el.data("kendoDropDownList") ||
                          $el.data("kendoMultiSelect") ||
                          $el.data("kendoDatePicker");
            if (kendo) {
                kendo.value(valor);
                return true;
            }

            return false;
        },

        /**
         * Define data inicial
         */
        setDataInicial: function (data) {
            const widget = this._widgets.txtDataInicial ||
                           $(CONFIG.elementos.txtDataInicial).data("kendoDatePicker");
            if (widget) {
                widget.value(data ? new Date(data) : null);
            }
        },

        /**
         * Define data final de recorrência
         */
        setDataFinalRecorrencia: function (data) {
            const widget = this._widgets.txtFinalRecorrencia ||
                           $(CONFIG.elementos.txtFinalRecorrencia).data("kendoDatePicker");
            if (widget) {
                widget.value(data ? new Date(data) : null);
            }
        },

        /**
         * Define recorrente (S/N)
         */
        setRecorrente: function (valor) {
            const widget = this._widgets.lstRecorrente ||
                           $(CONFIG.elementos.lstRecorrente).data("kendoDropDownList");
            if (widget) {
                widget.value(valor || "N");
            }
        },

        /**
         * Define período
         */
        setPeriodo: function (valor) {
            const widget = this._widgets.lstPeriodos ||
                           $(CONFIG.elementos.lstPeriodos).data("kendoDropDownList");
            if (widget) {
                widget.value(valor || "");
            }
        },

        /**
         * Define dias da semana
         */
        setDiasSemana: function (valores) {
            const widget = this._widgets.lstDias ||
                           $(CONFIG.elementos.lstDias).data("kendoMultiSelect");
            if (widget) {
                widget.value(Array.isArray(valores) ? valores : []);
            }
        },

        /**
         * Define dia do mês
         */
        setDiaMes: function (valor) {
            const widget = this._widgets.lstDiasMes ||
                           $(CONFIG.elementos.lstDiasMes).data("kendoDropDownList");
            if (widget) {
                widget.value(valor || null);
            }
        },

        // ====================================================================
        // HABILITAR/DESABILITAR
        // ====================================================================

        /**
         * Habilita ou desabilita um controle
         */
        habilitar: function (id, habilitar = true) {
            const widget = this._widgets[id];
            if (widget && typeof widget.enable === "function") {
                widget.enable(habilitar);
                return true;
            }

            // Fallback: tentar via jQuery
            const $el = $(CONFIG.elementos[id] || "#" + id);
            const kendo = $el.data("kendoDropDownList") ||
                          $el.data("kendoMultiSelect") ||
                          $el.data("kendoDatePicker");
            if (kendo && typeof kendo.enable === "function") {
                kendo.enable(habilitar);
                return true;
            }

            return false;
        },

        /**
         * Desabilita todos os controles de recorrência
         */
        desabilitarRecorrencia: function () {
            this.habilitar("lstRecorrente", false);
            this.habilitar("lstPeriodos", false);
            this.habilitar("lstDias", false);
            this.habilitar("lstDiasMes", false);
            this.habilitar("txtFinalRecorrencia", false);
        },

        /**
         * Habilita todos os controles de recorrência
         */
        habilitarRecorrencia: function () {
            this.habilitar("lstRecorrente", true);
            this.habilitar("lstPeriodos", true);
            this.habilitar("lstDias", true);
            this.habilitar("lstDiasMes", true);
            this.habilitar("txtFinalRecorrencia", true);
        },

        /**
         * Limpa todos os controles de recorrência
         */
        limparRecorrencia: function () {
            this.setRecorrente("N");
            this.setPeriodo("");
            this.setDiasSemana([]);
            this.setDiaMes(null);
            this.setDataFinalRecorrencia(null);
        }
    };

    // ========================================================================
    // SEÇÃO 4: MÓDULO DE RECORRÊNCIA
    // ========================================================================

    const Recorrencia = {
        /**
         * ╭──────────────────────────────────────────────────────────────────────────────
         * │ FUNCIONALIDADE: Atualiza a visibilidade dos campos de recorrência conforme
         * │                 o período selecionado, respeitando CSS com !important.
         * │──────────────────────────────────────────────────────────────────────────────
         * │ PARÂMETROS:
         * │    -> periodo: Código do período (D/S/Q/M/V).
         * │──────────────────────────────────────────────────────────────────────────────
         */
        atualizarVisibilidade: function (periodo) {
            try {
                const periodoNormalizado = (periodo || "").toString().trim().toUpperCase();
                Utils.log("Atualizando visibilidade para período:", periodoNormalizado);

                const recorrenteValor = (Controles.getRecorrente() || "").toString().trim().toUpperCase();
                const mostrarPeriodo = recorrenteValor === "S" || recorrenteValor === "SIM" || recorrenteValor === "TRUE" || recorrenteValor === "1";
                const mostrarDias = periodoNormalizado === "S" || periodoNormalizado === "Q";
                const mostrarDiasMes = periodoNormalizado === "M";
                const mostrarFinal = periodoNormalizado === "D" || periodoNormalizado === "S" || periodoNormalizado === "Q" || periodoNormalizado === "M";
                const mostrarCalendario = periodoNormalizado === "V";
                const estaEditandoRecorrencia = !!window.carregandoViagemExistente;
                const mostrarListaDatas = estaEditandoRecorrencia && !mostrarCalendario;

                // [UI] Containers principais
                Utils.mostrar(CONFIG.elementos.divPeriodo, mostrarPeriodo, "block");
                Utils.mostrar(CONFIG.elementos.divDias, mostrarDias, "block");
                Utils.mostrar(CONFIG.elementos.divDiasMes, mostrarDiasMes, "block");
                Utils.mostrar(CONFIG.elementos.divFinalRecorrencia, mostrarFinal, "block");
                Utils.mostrar(CONFIG.elementos.divCalendario, mostrarCalendario, "flex");
                Utils.mostrar(CONFIG.elementos.divListaDatas, mostrarListaDatas, "flex");
                Utils.mostrar(CONFIG.elementos.calDatasSelecionadas, mostrarCalendario, "block");
                Utils.mostrar(CONFIG.elementos.lstDatasVariadas, mostrarCalendario, "block");
                Utils.mostrar(CONFIG.elementos.lblSelecioneAsDatas, mostrarCalendario, "block");

                // [LOGICA] Calendário de dias variados + badges
                if (mostrarCalendario) {
                    this._garantirCalendario();
                    this._sincronizarDatasVariadas(this._obterDatasCalendario());
                    Utils.mostrar(CONFIG.elementos.badgeCalendario, true, "flex");
                } else {
                    // Ocultar badges quando calendário não está visível (sempreMostrar = false)
                    this._atualizarBadge(CONFIG.elementos.badgeCalendario, 0, "flex", false);
                    this._atualizarBadge(CONFIG.elementos.badgeDatasVariadas, 0, "inline-flex", false);
                }
            } catch (error) {
                Utils.erro("Recorrencia.atualizarVisibilidade", error);
            }
        },

        /**
         * Prepara modal para novo agendamento
         */
        prepararNovo: function () {
            Utils.log("Preparando para novo agendamento");

            // Limpar valores
            Controles.limparRecorrencia();

            // Habilitar controles
            Controles.habilitarRecorrencia();

            // Esconder campos de recorrência
            this.atualizarVisibilidade("");

            // Limpar calendário Syncfusion se existir
            this._limparCalendario();
        },

        /**
         * Carrega dados de recorrência existente
         */
        carregarDados: function (dados) {
            Utils.log("Carregando dados de recorrência:", dados);

            if (!dados) return;

            // Definir recorrente
            const ehRecorrente = dados.recorrente === "S" || dados.recorrente === true || dados.ehRecorrente;
            Controles.setRecorrente(ehRecorrente ? "S" : "N");

            if (ehRecorrente && dados.intervalo) {
                // Definir período
                Controles.setPeriodo(dados.intervalo);

                // Definir dias da semana (Semanal/Quinzenal)
                if ((dados.intervalo === "S" || dados.intervalo === "Q") && dados.diasSemana) {
                    const dias = Array.isArray(dados.diasSemana) ? dados.diasSemana : [dados.diasSemana];
                    Controles.setDiasSemana(dias);
                }

                // Definir dia do mês (Mensal)
                if (dados.intervalo === "M" && dados.diaMes) {
                    Controles.setDiaMes(dados.diaMes);
                }

                // Definir data final
                if (dados.dataFinalRecorrencia) {
                    Controles.setDataFinalRecorrencia(dados.dataFinalRecorrencia);
                }

                // Atualizar visibilidade
                this.atualizarVisibilidade(dados.intervalo);
            }
        },

        /**
         * Coleta dados de recorrência para salvar
         */
        coletarDados: function () {
            const recorrente = Controles.getRecorrente();

            if (recorrente !== "S") {
                return {
                    ehRecorrente: false,
                    intervalo: null,
                    diasSemana: null,
                    diaMes: null,
                    dataFinalRecorrencia: null,
                    datasVariadas: null
                };
            }

            const periodo = Controles.getPeriodo();
            const dados = {
                ehRecorrente: true,
                intervalo: periodo,
                diasSemana: null,
                diaMes: null,
                dataFinalRecorrencia: null,
                datasVariadas: null
            };

            switch (periodo) {
                case "D":
                    dados.dataFinalRecorrencia = Controles.getDataFinalRecorrencia();
                    break;

                case "S":
                case "Q":
                    dados.diasSemana = Controles.getDiasSemana();
                    dados.dataFinalRecorrencia = Controles.getDataFinalRecorrencia();
                    break;

                case "M":
                    dados.diaMes = Controles.getDiaMes();
                    dados.dataFinalRecorrencia = Controles.getDataFinalRecorrencia();
                    break;

                case "V":
                    dados.datasVariadas = this._obterDatasCalendario();
                    break;
            }

            return dados;
        },

        /**
         * Limpa calendário Syncfusion
         */
        _limparCalendario: function () {
            try {
                // [UI] Limpar seleÇõÇœo do calendÇ­rio
                const cal = document.getElementById("calDatasSelecionadas");
                if (cal && cal.ej2_instances && cal.ej2_instances[0]) {
                    cal.ej2_instances[0].values = [];
                    cal.ej2_instances[0].dataBind();
                }

            // Limpar variável global de datas selecionadas
            // [DADOS] Resetar seleÇõÇœo global
                window.selectedDates = [];
            // [UI] Sincronizar listbox e badges
                this._sincronizarDatasVariadas([]);
            } catch (error) {
                Utils.erro("Recorrencia._limparCalendario", error);
            }
        },

        /**
         * Obtém datas selecionadas do calendário Syncfusion
         */
        /**
         * ╭──────────────────────────────────────────────────────────────────────────────
         * │ FUNCIONALIDADE: Inicializa o calendÇ­rio Syncfusion para Dias Variados e
         * │                 registra o callback de sincronizaÇõÇœo de datas.
         * │──────────────────────────────────────────────────────────────────────────────
         * │ CHAMADO POR:
         * │    -> Recorrencia.atualizarVisibilidade
         * │──────────────────────────────────────────────────────────────────────────────
         */
        _garantirCalendario: async function () {
            try {
                // [UI] Validar elemento e dependencias
                const calElement = document.getElementById("calDatasSelecionadas");
                if (!calElement) {
                    Utils.log("calDatasSelecionadas nao encontrado para inicializacao");
                    return;
                }

                if (calElement.ej2_instances && calElement.ej2_instances[0]) {
                    this._sincronizarDatasVariadas(calElement.ej2_instances[0].values || []);
                    return;
                }

                if (typeof ej === "undefined" || !ej.calendars || !ej.calendars.Calendar) {
                    Utils.log("Syncfusion Calendar nao disponivel para inicializar calDatasSelecionadas");
                    return;
                }

                // [LOGICA] Garantir localizacao Syncfusion antes de renderizar o calendario
                if (typeof window.loadSyncfusionLocalization === "function" && !window.syncfusionLocalizationFailed) {
                    if (!window.syncfusionLocalizationPromise) {
                        try {
                            const retorno = window.loadSyncfusionLocalization();
                            if (retorno && typeof retorno.then === "function") {
                                window.syncfusionLocalizationPromise = retorno
                                    .then(() => {
                                        window.syncfusionLocalizationReady = true;
                                    })
                                    .catch((error) => {
                                        window.syncfusionLocalizationFailed = true;
                                        Utils.erro("Recorrencia._garantirCalendario.localizacao", error);
                                    });
                            } else {
                                window.syncfusionLocalizationReady = true;
                                window.syncfusionLocalizationPromise = Promise.resolve();
                            }
                        } catch (error) {
                            window.syncfusionLocalizationFailed = true;
                            Utils.erro("Recorrencia._garantirCalendario.localizacao", error);
                        }
                    }

                    if (window.syncfusionLocalizationPromise) {
                        await window.syncfusionLocalizationPromise;
                    }
                }

                if (calElement.ej2_instances && calElement.ej2_instances[0]) {
                    this._sincronizarDatasVariadas(calElement.ej2_instances[0].values || []);
                    return;
                }

                const hoje = new Date();
                hoje.setHours(0, 0, 0, 0);
                const localeCalendario = window.syncfusionLocalizationReady ? "pt-BR" : "en-US";

                const self = this;
                const calendario = new ej.calendars.Calendar({
                    isMultiSelection: true,
                    showTodayButton: false,
                    locale: localeCalendario,
                    min: hoje,
                    change: function (args) {
                        try {
                            const valores = Array.isArray(args?.values)
                                ? args.values
                                : (args?.value ? [args.value] : []);
                            self._sincronizarDatasVariadas(valores);
                        } catch (error) {
                            Utils.erro("Recorrencia._garantirCalendario.change", error);
                        }
                    }
                });

                calendario.appendTo(CONFIG.elementos.calDatasSelecionadas);
                this._sincronizarDatasVariadas(calendario.values || []);
            } catch (error) {
                Utils.erro("Recorrencia._garantirCalendario", error);
            }
        },

        /**
         * ╭──────────────────────────────────────────────────────────────────────────────
         * │ FUNCIONALIDADE: Sincroniza datas selecionadas com listbox e badges.
         * │──────────────────────────────────────────────────────────────────────────────
         * │ CHAMADO POR:
         * │    -> Recorrencia._garantirCalendario
         * │    -> Recorrencia._limparCalendario
         * │──────────────────────────────────────────────────────────────────────────────
         * │ PARÂMETROS:
         * │    -> datas: Array de datas selecionadas no calendÇ­rio.
         * │──────────────────────────────────────────────────────────────────────────────
         */
        _sincronizarDatasVariadas: function (datas) {
            try {
                // [DADOS] Normalizar datas selecionadas
                const datasNormalizadas = Array.isArray(datas)
                    ? datas.map(d => new Date(d)).filter(d => !isNaN(d.getTime()))
                    : [];

                datasNormalizadas.sort((a, b) => a.getTime() - b.getTime());
                window.selectedDates = datasNormalizadas;

                // [UI] Atualizar listbox de datas variadas
                const listbox = document.getElementById("lstDatasVariadas");
                if (listbox) {
                    listbox.innerHTML = "";
                    listbox.multiple = true;
                    listbox.disabled = false;
                    listbox.size = Math.max(Math.min(datasNormalizadas.length, 5), 1);
                    Utils.mostrar(CONFIG.elementos.lstDatasVariadas, true, "block");

                    datasNormalizadas.forEach(data => {
                        const option = document.createElement("option");
                        option.value = data.toISOString();
                        option.textContent = Utils.formatarData(data);
                        listbox.appendChild(option);
                    });
                }

                // [UI] Atualizar badges
                this._atualizarBadge(CONFIG.elementos.badgeCalendario, datasNormalizadas.length, "flex");
                this._atualizarBadge(CONFIG.elementos.badgeDatasVariadas, datasNormalizadas.length, "inline-flex");
            } catch (error) {
                Utils.erro("Recorrencia._sincronizarDatasVariadas", error);
            }
        },

        /**
         * ╭──────────────────────────────────────────────────────────────────────────────
         * │ FUNCIONALIDADE: Atualiza badge de contagem e aplica efeito de pulso.
         * │──────────────────────────────────────────────────────────────────────────────
         * │ PARÂMETROS:
         * │    -> seletor: Seletor CSS do badge.
         * │    -> total: Quantidade de itens.
         * │    -> displayPadrao: Display aplicado quando visível.
         * │──────────────────────────────────────────────────────────────────────────────
         */
        _atualizarBadge: function (seletor, total, displayPadrao = "flex", sempreMostrar = true) {
            try {
                const badge = document.querySelector(seletor);
                if (!badge) {
                    return;
                }

                badge.textContent = total.toString();

                if (total > 0) {
                    Utils.mostrar(seletor, true, displayPadrao);
                    badge.classList.remove("badge-pulse");
                    void badge.offsetWidth;
                    badge.classList.add("badge-pulse");
                } else if (sempreMostrar) {
                    // Sempre mostrar badge mesmo com zero (para calendário de dias variados)
                    Utils.mostrar(seletor, true, displayPadrao);
                } else {
                    Utils.mostrar(seletor, false, displayPadrao);
                }
            } catch (error) {
                Utils.erro("Recorrencia._atualizarBadge", error);
            }
        },

        _obterDatasCalendario: function () {
            const cal = document.getElementById("calDatasSelecionadas");
            if (cal && cal.ej2_instances && cal.ej2_instances[0]) {
                return cal.ej2_instances[0].values || [];
            }
            return Array.isArray(window.selectedDates) ? window.selectedDates : [];
        }
    };

    // ========================================================================
    // SEÇÃO 5: MÓDULO DE EVENTOS
    // ========================================================================

    const Eventos = {
        // Flag para ignorar eventos durante carregamento
        _ignorar: false,

        /**
         * Handler quando data inicial muda
         */
        onDataInicialChange: function (e) {
            if (this._ignorar) return;
            Utils.log("Data inicial alterada:", e.sender.value());

            // Calcular duração se hora estiver preenchida
            if (typeof window.calcularDuracaoViagem === "function") {
                window.calcularDuracaoViagem();
            }
        },

        /**
         * Handler quando recorrente muda
         */
        onRecorrenteChange: function (e) {
            if (this._ignorar) return;

            const valor = e.sender.value();
            Utils.log("Recorrente alterado:", valor);

            if (valor === "S") {
                // Mostrar campos de recorrência
                Utils.mostrar(CONFIG.elementos.divPeriodo, true);
            } else {
                // Esconder todos os campos de recorrência
                Recorrencia.atualizarVisibilidade("");
                Controles.setPeriodo("");
            }

            // Chamar função global se existir (compatibilidade)
            if (typeof window.RecorrenteValueChange === "function") {
                window.RecorrenteValueChange({ value: valor });
            }
        },

        /**
         * Handler quando período muda
         */
        onPeriodoChange: function (e) {
            if (this._ignorar) return;

            const valor = e.sender.value();
            Utils.log("Período alterado:", valor);

            // Atualizar visibilidade
            Recorrencia.atualizarVisibilidade(valor);

            // Chamar função global se existir (compatibilidade)
            if (typeof window.PeriodosValueChange === "function") {
                window.PeriodosValueChange({ value: valor });
            }
        },

        /**
         * Handler quando dias da semana mudam
         */
        onDiasChange: function (e) {
            if (this._ignorar) return;

            const valores = e.sender.value();
            Utils.log("Dias alterados:", valores);

            // Chamar função global se existir (compatibilidade)
            if (typeof window.onBlurLstDias === "function") {
                window.onBlurLstDias({ value: valores });
            }
        },

        /**
         * Handler quando dia do mês muda
         */
        onDiaMesChange: function (e) {
            if (this._ignorar) return;

            const valor = e.sender.value();
            Utils.log("Dia do mês alterado:", valor);
        },

        /**
         * Handler quando data final de recorrência muda
         */
        onFinalRecorrenciaChange: function (e) {
            if (this._ignorar) return;
            Utils.log("Data final recorrência alterada:", e.sender.value());
        },

        /**
         * Suspende eventos temporariamente
         */
        suspender: function () {
            this._ignorar = true;
        },

        /**
         * Retoma eventos
         */
        retomar: function () {
            this._ignorar = false;
        }
    };

    // ========================================================================
    // SEÇÃO 6: MÓDULO DE VALIDAÇÃO
    // ========================================================================

    const Validacao = {
        /**
         * Valida recorrência antes de salvar
         */
        validarRecorrencia: async function () {
            try {
                const recorrente = Controles.getRecorrente();
                const periodo = Controles.getPeriodo();

                // Se não é recorrente, não precisa validar
                if (recorrente !== "S") {
                    return true;
                }

                // Validar período obrigatório
                if (!periodo) {
                    await Alerta.Erro("Informação Ausente", "Se o Agendamento é Recorrente, você precisa escolher o Período de Recorrência");
                    return false;
                }

                // Validar dias da semana para Semanal/Quinzenal
                if (periodo === "S" || periodo === "Q") {
                    const dias = Controles.getDiasSemana();
                    if (!dias || dias.length === 0) {
                        await Alerta.Erro("Informação Ausente", "Para período Semanal ou Quinzenal, você precisa escolher ao menos um Dia da Semana");
                        return false;
                    }
                }

                // Validar dia do mês para Mensal
                if (periodo === "M") {
                    const diaMes = Controles.getDiaMes();
                    if (!diaMes) {
                        await Alerta.Erro("Informação Ausente", "Para período Mensal, você precisa escolher o Dia do Mês");
                        return false;
                    }
                }

                // Validar data final para períodos que exigem
                if (periodo === "D" || periodo === "S" || periodo === "Q" || periodo === "M") {
                    const dataFinal = Controles.getDataFinalRecorrencia();
                    if (!dataFinal) {
                        await Alerta.Erro("Informação Ausente", "Você precisa escolher a Data Final da Recorrência");
                        return false;
                    }
                }

                // Validar datas variadas
                if (periodo === "V") {
                    const datas = Recorrencia._obterDatasCalendario();
                    if (!datas || datas.length === 0) {
                        await Alerta.Erro("Informação Ausente", "Para Dias Variados, você precisa selecionar as datas no calendário");
                        return false;
                    }
                }

                return true;
            } catch (error) {
                Utils.erro("Validacao.validarRecorrencia", error);
                return false;
            }
        }
    };

    // ========================================================================
    // SEÇÃO 7: API PÚBLICA
    // ========================================================================

    const AgendamentoCore = {
        // Versão
        versao: "1.0.0",

        // Submódulos expostos
        Controles: Controles,
        Recorrencia: Recorrencia,
        Eventos: Eventos,
        Validacao: Validacao,
        Utils: Utils,
        Config: CONFIG,

        /**
         * Inicializa o sistema completo
         */
        inicializar: function () {
            Utils.log("Inicializando AgendamentoCore v" + this.versao);

            // Aguardar DOM pronto
            $(document).ready(() => {
                // Inicializar controles Kendo
                Controles.inicializar();

                // Esconder campos de recorrência inicialmente
                Recorrencia.atualizarVisibilidade("");

                Utils.log("AgendamentoCore inicializado com sucesso");
            });

            return this;
        },

        /**
         * Prepara modal para novo agendamento
         */
        novoAgendamento: function (dataInicial, horaInicial) {
            Utils.log("Preparando novo agendamento");

            Eventos.suspender();

            try {
                // Limpar recorrência
                Recorrencia.prepararNovo();

                // Definir data inicial se fornecida
                if (dataInicial) {
                    Controles.setDataInicial(dataInicial);
                }

                // Definir hora inicial se fornecida
                if (horaInicial) {
                    $(CONFIG.elementos.txtHoraInicial).val(horaInicial);
                }
            } finally {
                Eventos.retomar();
            }
        },

        /**
         * Carrega agendamento existente para edição
         */
        carregarAgendamento: function (dados) {
            Utils.log("Carregando agendamento para edição");

            Eventos.suspender();

            try {
                // Carregar dados de recorrência
                Recorrencia.carregarDados(dados);
            } finally {
                Eventos.retomar();
            }
        },

        /**
         * Coleta todos os dados de recorrência
         */
        coletarDadosRecorrencia: function () {
            return Recorrencia.coletarDados();
        },

        /**
         * Valida recorrência
         */
        validarRecorrencia: function () {
            return Validacao.validarRecorrencia();
        },

        /**
         * Desabilita todos os controles (modo visualização)
         */
        desabilitarControles: function () {
            Controles.desabilitarRecorrencia();
        },

        /**
         * Habilita todos os controles (modo edição)
         */
        habilitarControles: function () {
            Controles.habilitarRecorrencia();
        }
    };

    // ========================================================================
    // SEÇÃO 8: EXPOR GLOBALMENTE
    // ========================================================================

    // Expor módulo principal
    window.AgendamentoCore = AgendamentoCore;

    // Compatibilidade com código legado
    window.AgendamentoV2 = AgendamentoCore;

    if (!window.RecorrenciaController) {
        window.RecorrenciaController = {};
    }

    if (!window.RecorrenciaController.prepararNovo) {
        /**
         * ╭──────────────────────────────────────────────────────────────────────────────
         * │ FUNCIONALIDADE: Prepara o modal para novo agendamento recorrente usando
         * │                 o AgendamentoCore como controlador central.
         * │──────────────────────────────────────────────────────────────────────────────
         * │ CHAMADO POR:
         * │    -> exibe-viagem.js (exibirNovaViagem)
         * │──────────────────────────────────────────────────────────────────────────────
         * │ PARÂMETROS:
         * │    -> dataInicial: Data opcional para prÇ¦-preenchimento.
         * │    -> horaInicial: Hora opcional para prÇ¦-preenchimento.
         * │──────────────────────────────────────────────────────────────────────────────
         */
        window.RecorrenciaController.prepararNovo = function (dataInicial, horaInicial) {
            try {
                if (AgendamentoCore && typeof AgendamentoCore.novoAgendamento === "function") {
                    AgendamentoCore.novoAgendamento(dataInicial, horaInicial);
                    return;
                }

                if (Recorrencia && typeof Recorrencia.prepararNovo === "function") {
                    Recorrencia.prepararNovo();
                }
            } catch (error) {
                Utils.erro("RecorrenciaController.prepararNovo", error);
            }
        };
    }

    // Funções globais de compatibilidade
    window.inicializarControlesRecorrencia = function () {
        Controles.inicializar();
    };

    window.inicializarLstRecorrente = function () {
        Controles._criarDropdownRecorrente();
    };

    window.inicializarDropdownPeriodos = function () {
        Controles._criarDropdownPeriodos();
    };

    window.rebuildLstPeriodos = function () {
        Controles._criarDropdownPeriodos();
    };

    window.inicializarLstDias = function () {
        Controles._criarMultiSelectDias();
    };

    window.inicializarLstDiasMes = function () {
        Controles._criarDropdownDiasMes();
    };

    window.inicializarTxtFinalRecorrencia = function () {
        Controles._criarDatePickerFinalRecorrencia();
    };

    // Funções de acesso rápido
    window.getKendoDatePickerValue = function (id) {
        const $el = $("#" + id);
        const dp = $el.data("kendoDatePicker");
        return dp ? dp.value() : null;
    };

    window.setKendoDatePickerValue = function (id, valor) {
        const $el = $("#" + id);
        const dp = $el.data("kendoDatePicker");
        if (dp) {
            dp.value(valor ? new Date(valor) : null);
        }
    };

    // Auto-inicialização
    Utils.log("AgendamentoCore carregado, aguardando DOM...");

})(window, jQuery);


