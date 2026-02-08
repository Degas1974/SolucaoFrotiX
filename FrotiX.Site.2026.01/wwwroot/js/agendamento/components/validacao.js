/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: validacao.js                                                 ║
 * ║ 📍 LOCAL: wwwroot/js/agendamento/components/                            ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Funções de Validação de Formulário de Agendamento.                     ║
 * ║    • Classe ValidadorAgendamento                                        ║
 * ║    • Validação de todos os campos                                        ║
 * ║    • Coleta de erros para exibição                                        ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Alta (Agenda - Validação)                                  ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

/**
 * Classe para validação de campos
 */
class ValidadorAgendamento {
    constructor() {
        this.erros = [];
    }

    /**
     * Valida todos os campos do formulário
     * param {string} viagemId - ID da viagem (opcional)
     * returns {Promise<boolean>} true se válido
     */
    async validar(viagemId = null) {
        try {
            this.erros = [];

            // Resetar flags de confirmação para nova validação
            this._kmConfirmado = false;
            this._finalizacaoConfirmada = false;

            // Validar data inicial
            if (!(await this.validarDataInicial())) return false;

            // Validar finalidade
            if (!(await this.validarFinalidade())) return false;

            // Validar origem
            if (!(await this.validarOrigem())) return false;

            // Validar destino
            if (!(await this.validarDestino())) return false;

            // Validar campos de finalização (se preenchidos)
            const algumFinalPreenchido = this.verificarCamposFinalizacao();
            if (algumFinalPreenchido) {
                if (!(await this.validarFinalizacao())) return false;
            }

            // Validações específicas de viagem ABERTA (não agendamento)
            // Motorista, Veículo, KM e Combustível NÃO são obrigatórios em agendamentos
            // SÓ validar esses campos se:
            // 1. Está criando/editando uma viagem JÁ ABERTA/REALIZADA (não agendamento)
            // 2. OU se algum campo de finalização foi preenchido (transformando agendamento em viagem)
            const btnTexto = $('#btnConfirma').text().trim();
            const ehAgendamento =
                btnTexto === 'Edita Agendamento' ||
                btnTexto === 'Confirma Agendamento' ||
                btnTexto === 'Confirmar';

            // Se NÃO for agendamento OU se tem campos de finalização preenchidos
            if (!ehAgendamento || algumFinalPreenchido) {
                if (!(await this.validarCamposViagem())) return false;
            }

            // Validar requisitante
            if (!(await this.validarRequisitante())) return false;

            // Validar ramal
            if (!(await this.validarRamal())) return false;

            // Validar setor
            if (!(await this.validarSetor())) return false;

            // Validar evento (se finalidade for "Evento")
            if (!(await this.validarEvento())) return false;

            // Validar recorrência
            if (window.transformandoEmViagem === false) {
                if (!(await this.validarRecorrencia())) return false;
            }

            // Validar período de recorrência
            if (!(await this.validarPeriodoRecorrencia())) return false;

            // Validar dias variados
            if (!(await this.validarDiasVariados())) return false;

            // Validar quilometragem final
            if (!(await this.validarKmFinal())) return false;

            // Validar campos de finalização completos
            if (algumFinalPreenchido) {
                if (!(await this.confirmarFinalizacao())) return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha('validacao.js', 'validar', error);
            return false;
        }
    }

    /**
     * Valida data inicial
     * ATUALIZADO 20/01/2026: Usar Telerik Kendo DatePicker ao invés de Syncfusion
     * ATUALIZADO 20/01/2026: Data Inicial NUNCA pode ser menor que hoje
     */
    async validarDataInicial() {
        try {
            // Telerik usa $(element).data("kendoDatePicker")
            const kendoDatePicker =
                $('#txtDataInicial').data('kendoDatePicker');

            if (kendoDatePicker) {
                const valDataInicial = kendoDatePicker.value();

                if (!valDataInicial || !moment(valDataInicial).isValid()) {
                    kendoDatePicker.value(new Date());
                    return true;
                }

                // VALIDACAO: Data Inicial NUNCA pode ser menor que hoje
                const dataInicial = new Date(valDataInicial);
                dataInicial.setHours(0, 0, 0, 0);
                const hoje = new Date();
                hoje.setHours(0, 0, 0, 0);

                if (dataInicial < hoje) {
                    await Alerta.Erro(
                        'Data Invalida',
                        'A <strong>Data Inicial</strong> nao pode ser anterior a data de hoje.',
                    );
                    kendoDatePicker.focus();
                    return false;
                }
            } else {
                // Fallback: input HTML padrao
                const txtDataInicial =
                    document.getElementById('txtDataInicial');
                if (
                    txtDataInicial &&
                    (!txtDataInicial.value || txtDataInicial.value === '')
                ) {
                    txtDataInicial.value = moment().format('YYYY-MM-DD');
                }
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarDataInicial',
                error,
            );
            return false;
        }
    }

    /**
     * Valida finalidade
     */
    async validarFinalidade() {
        try {
            const finalidade =
                document.getElementById('lstFinalidade').ej2_instances[0].value;

            if (finalidade === '' || finalidade === null) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'A <strong>Finalidade</strong> é obrigatória',
                );
                return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarFinalidade',
                error,
            );
            return false;
        }
    }

    /**
     * Valida origem
     */
    async validarOrigem() {
        try {
            const origem =
                document.getElementById('cmbOrigem').ej2_instances[0].value;

            if (origem === '' || origem === null) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'A Origem é obrigatória',
                );
                return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarOrigem',
                error,
            );
            return false;
        }
    }

    /**
     * Valida destino
     */
    async validarDestino() {
        try {
            const destino =
                document.getElementById('cmbDestino').ej2_instances[0].value;

            if (destino === '' || destino === null) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Destino é obrigatório',
                );
                return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarDestino',
                error,
            );
            return false;
        }
    }

    /**
     * Verifica se algum campo de finalização foi preenchido
     */
    verificarCamposFinalizacao() {
        try {
            const dataFinal = $('#txtDataFinal').val();
            const horaFinal = $('#txtHoraFinal').val();
            const combustivelFinal = document.getElementById(
                'ddtCombustivelFinal',
            ).ej2_instances[0].value;
            const kmFinal = $('#txtKmFinal').val();

            return dataFinal || horaFinal || combustivelFinal || kmFinal;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'verificarCamposFinalizacao',
                error,
            );
            return false;
        }
    }

    /**
     * Valida campos de finalização
     */
    async validarFinalizacao() {
        try {
            const dataFinal = $('#txtDataFinal').val();
            const horaFinal = $('#txtHoraFinal').val();
            const combustivelFinal = document.getElementById(
                'ddtCombustivelFinal',
            )?.ej2_instances?.[0]?.value;
            const kmFinal = $('#txtKmFinal').val();

            const todosFinalPreenchidos =
                dataFinal && horaFinal && combustivelFinal && kmFinal;

            if (!todosFinalPreenchidos) {
                await Alerta.Erro(
                    'Campos de Finalização Incompletos',
                    "Para gravar uma viagem como 'Realizada', é necessário preencher todos os campos de Finalização:\n\n• Data Final\n• Hora Final\n• Km Final\n• Combustível Final",
                );
                return false;
            }

            // Validação: Data Final não pode ser superior à data atual
            if (dataFinal) {
                const dtFinal = window.parseDate
                    ? window.parseDate(dataFinal)
                    : new Date(dataFinal);
                const dtAtual = new Date();

                // Zerar horas para comparar apenas datas
                dtFinal.setHours(0, 0, 0, 0);
                dtAtual.setHours(0, 0, 0, 0);

                if (dtFinal > dtAtual) {
                    await Alerta.Erro(
                        'Data Inválida',
                        'A Data Final não pode ser superior à data atual.',
                    );
                    $('#txtDataFinal').val('');
                    $('#txtDataFinal').focus();
                    return false;
                }
            }

            // Validar destino quando finalizado
            const destino =
                document.getElementById('cmbDestino')?.ej2_instances?.[0]
                    ?.value;
            if (destino === '' || destino === null) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Destino é obrigatório para finalizar a viagem',
                );
                return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarFinalizacao',
                error,
            );
            return false;
        }
    }

    /**
     * Valida campos específicos de viagem
     */
    async validarCamposViagem() {
        try {
            // REMOVIDO: Ficha de Vistoria não é mais obrigatória
            // Se não informada, será gravada como 0

            // Validar motorista
            const lstMotorista =
                document.getElementById('lstMotorista').ej2_instances[0];
            if (lstMotorista.value === null || lstMotorista.value === '') {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Motorista é obrigatório',
                );
                return false;
            }

            // Validar veículo
            const lstVeiculo =
                document.getElementById('lstVeiculo').ej2_instances[0];
            if (lstVeiculo.value === null || lstVeiculo.value === '') {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Veículo é obrigatório',
                );
                return false;
            }

            // Validar km
            const kmOk = await this.validarKmInicialFinal();
            if (!kmOk) return false;

            // Validar combustível inicial
            const ddtCombustivelInicial = document.getElementById(
                'ddtCombustivelInicial',
            ).ej2_instances[0];
            if (
                ddtCombustivelInicial.value === '' ||
                ddtCombustivelInicial.value === null
            ) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Combustível Inicial é obrigatório',
                );
                return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarCamposViagem',
                error,
            );
            return false;
        }
    }

    /**
     * Valida requisitante
     */
    async validarRequisitante() {
        try {
            // Telerik Kendo: usa $(element).data("kendoComboBox")
            const lstRequisitanteEl =
                document.getElementById('lstRequisitante');
            const kendoComboBox = lstRequisitanteEl
                ? $(lstRequisitanteEl).data('kendoComboBox')
                : null;

            const valorRequisitante = kendoComboBox
                ? kendoComboBox.value()
                : null;

            if (!valorRequisitante || valorRequisitante === '') {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Requisitante é obrigatório',
                );
                return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarRequisitante',
                error,
            );
            return false;
        }
    }

    /**
     * Valida ramal (VERSÃO CORRIGIDA)
     * Agora valida o campo correto: txtRamalRequisitanteSF
     */
    async validarRamal() {
        try {
            // Tentar validar o componente Syncfusion primeiro
            const ramalSFElement = document.getElementById(
                'txtRamalRequisitanteSF',
            );

            if (
                ramalSFElement &&
                ramalSFElement.ej2_instances &&
                ramalSFElement.ej2_instances[0]
            ) {
                // É um componente Syncfusion
                const ramalSF = ramalSFElement.ej2_instances[0];
                const valorRamalSF = document.getElementById(
                    'txtRamalRequisitanteSF',
                ).value;

                if (
                    !valorRamalSF ||
                    valorRamalSF === '' ||
                    valorRamalSF === null
                ) {
                    await Alerta.Erro(
                        'Informação Ausente',
                        'O Ramal do Requisitante é obrigatório',
                    );
                    return false;
                }

                console.log('✅ Ramal validado (Syncfusion):', valorRamalSF);
                return true;
            }

            // Fallback: tentar validar o input HTML padrío
            const valorRamal = $('#txtRamalRequisitante').val();
            if (!valorRamal || valorRamal === '' || valorRamal === null) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Ramal do Requisitante é obrigatório',
                );
                return false;
            }

            console.log('✅ Ramal validado (HTML):', valorRamal);
            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarRamal',
                error,
            );
            return false;
        }
    }

    /**
     * Valida setor (VERSÃO CORRIGIDA)
     * Agora valida o campo correto: lstSetorRequisitanteAgendamento
     */
    async validarSetor() {
        try {
            // Usar o nome correto do campo
            const lstSetorElement = document.getElementById(
                'lstSetorRequisitanteAgendamento',
            );

            // Verificar se o elemento existe
            if (!lstSetorElement) {
                console.error(
                    '❌ Elemento lstSetorRequisitanteAgendamento não encontrado',
                );
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Setor do Requisitante é obrigatório',
                );
                return false;
            }

            // Verificar se está visível (pode estar oculto em alguns casos)
            const isVisible =
                lstSetorElement.offsetWidth > 0 &&
                lstSetorElement.offsetHeight > 0;
            if (!isVisible) {
                console.log(
                    'ℹ️ lstSetorRequisitanteAgendamento está oculto - pulando validação',
                );
                return true; // Se está oculto, não valida
            }

            // Verificar se ej2_instances existe e tem elementos
            if (
                !lstSetorElement.ej2_instances ||
                lstSetorElement.ej2_instances.length === 0
            ) {
                console.error(
                    '❌ lstSetorRequisitanteAgendamento não está inicializado como componente EJ2',
                );
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Setor do Requisitante é obrigatório',
                );
                return false;
            }

            const lstSetor = lstSetorElement.ej2_instances[0];
            const valorSetor = lstSetor.value;

            // Validar o valor (pode ser array ou valor único)
            if (
                !valorSetor ||
                valorSetor === '' ||
                valorSetor === null ||
                (Array.isArray(valorSetor) && valorSetor.length === 0)
            ) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'O Setor do Requisitante é obrigatório',
                );
                return false;
            }

            console.log('✅ Setor validado:', valorSetor);
            return true;
        } catch (error) {
            console.error('❌ Erro em validarSetor:', error);
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarSetor',
                error,
            );
            return false;
        }
    }

    /**
     * Valida evento
     */
    async validarEvento() {
        try {
            const finalidade =
                document.getElementById('lstFinalidade').ej2_instances[0].value;

            if (finalidade && finalidade[0] === 'Evento') {
                const evento =
                    document.getElementById('lstEventos').ej2_instances[0]
                        .value;

                if (evento === '' || evento === null) {
                    await Alerta.Erro(
                        'Informação Ausente',
                        'O Nome do Evento é obrigatório',
                    );
                    return false;
                }
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarEvento',
                error,
            );
            return false;
        }
    }

    /**
     * Valida recorrência
     * ✅ ATUALIZADO 20/01/2026: Usar Kendo API para controles migrados
     */
    async validarRecorrencia() {
        try {
            // ✅ CORREÇÃO 20/01/2026: lstRecorrente e lstPeriodos agora são Kendo DropDownList
            const lstRecorrenteKendo =
                $('#lstRecorrente').data('kendoDropDownList');
            const lstPeriodosKendo =
                $('#lstPeriodos').data('kendoDropDownList');

            const recorrente = lstRecorrenteKendo
                ? lstRecorrenteKendo.value()
                : null;
            const periodo = lstPeriodosKendo ? lstPeriodosKendo.value() : null;

            // Validação 1: Se recorrente = Sim, Período é obrigatório
            if (recorrente === 'S' && (!periodo || periodo === '')) {
                await Alerta.Erro(
                    'Informação Ausente',
                    'Se o Agendamento é Recorrente, você precisa escolher o Período de Recorrência',
                );
                return false;
            }

            // Validação 2: Semanal/Quinzenal → Dias da Semana obrigatório
            if (periodo === 'S' || periodo === 'Q') {
                // ✅ CORREÇÃO 20/01/2026: lstDias agora é Kendo MultiSelect
                const lstDiasKendo = $('#lstDias').data('kendoMultiSelect');
                const diasSelecionados = lstDiasKendo
                    ? lstDiasKendo.value()
                    : [];

                if (!diasSelecionados || diasSelecionados.length === 0) {
                    await Alerta.Erro(
                        'Informação Ausente',
                        'Para período Semanal ou Quinzenal, você precisa escolher ao menos um Dia da Semana',
                    );
                    return false;
                }
            }

            // Validação 3: Mensal → Dia do Mês obrigatório
            if (periodo === 'M') {
                // ✅ CORREÇÃO 20/01/2026: lstDiasMes agora é Kendo DropDownList
                const lstDiasMesKendo =
                    $('#lstDiasMes').data('kendoDropDownList');
                const diaMes = lstDiasMesKendo ? lstDiasMesKendo.value() : null;

                if (!diaMes || diaMes === '' || diaMes === null) {
                    await Alerta.Erro(
                        'Informação Ausente',
                        'Para período Mensal, você precisa escolher o Dia do Mês',
                    );
                    return false;
                }
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarRecorrencia',
                error,
            );
            return false;
        }
    }

    /**
     * Valida período de recorrência
     * ✅ ATUALIZADO 20/01/2026: lstPeriodos agora é Kendo DropDownList
     */
    async validarPeriodoRecorrencia() {
        try {
            // ✅ CORREÇÃO 20/01/2026: lstPeriodos agora é Kendo DropDownList
            const lstPeriodosKendo =
                $('#lstPeriodos').data('kendoDropDownList');
            const periodo = lstPeriodosKendo ? lstPeriodosKendo.value() : null;

            if (
                periodo === 'D' ||
                periodo === 'S' ||
                periodo === 'Q' ||
                periodo === 'M'
            ) {
                // Obter valor do input date HTML padrão
                const txtFinalRecorrencia = document.getElementById(
                    'txtFinalRecorrencia',
                );
                const dataFinal = txtFinalRecorrencia
                    ? txtFinalRecorrencia.value?.trim()
                    : '';

                if (dataFinal === '' || dataFinal === null) {
                    await Alerta.Erro(
                        'Informação Ausente',
                        'Se o período foi escolhido como diário, semanal, quinzenal ou mensal, você precisa escolher a Data Final',
                    );
                    return false;
                }
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarPeriodoRecorrencia',
                error,
            );
            return false;
        }
    }

    /**
     * Valida dias variados
     * ✅ ATUALIZADO 20/01/2026: lstPeriodos agora é Kendo DropDownList
     */
    async validarDiasVariados() {
        try {
            // ✅ CORREÇÃO 20/01/2026: lstPeriodos agora é Kendo DropDownList
            const lstPeriodosKendo =
                $('#lstPeriodos').data('kendoDropDownList');
            const periodo = lstPeriodosKendo ? lstPeriodosKendo.value() : null;

            if (periodo === 'V') {
                // Verificar se o calendário existe e está disponível
                const calendarElement = document.getElementById(
                    'calDatasSelecionadas',
                );

                if (
                    !calendarElement ||
                    !calendarElement.ej2_instances ||
                    !calendarElement.ej2_instances[0]
                ) {
                    // Calendário não disponível (provavelmente está editando agendamento existente)
                    // Neste caso, a validação não se aplica pois os dias já estão definidos
                    console.log(
                        'ℹ️ Calendário não disponível - pulando validação de dias variados',
                    );
                    return true;
                }

                const calendarObj = calendarElement.ej2_instances[0];
                const selectedDates = calendarObj.values;

                if (!selectedDates || selectedDates.length === 0) {
                    await Alerta.Erro(
                        'Informação Ausente',
                        'Se o período foi escolhido como Dias Variados, você precisa escolher ao menos um dia no Calendário',
                    );
                    return false;
                }
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarDiasVariados',
                error,
            );
            return false;
        }
    }

    /**
     * Valida km inicial vs final
     */
    async validarKmInicialFinal() {
        try {
            const kmInicial = $('#txtKmInicial').val();
            const kmFinal = $('#txtKmFinal').val();

            if (!kmInicial || !kmFinal) return true;

            const ini = parseFloat(kmInicial.replace(',', '.'));
            const fim = parseFloat(kmFinal.replace(',', '.'));

            // Validação: Km Final deve ser maior que Km Inicial
            if (fim < ini) {
                await Alerta.Erro(
                    'Erro',
                    'A quilometragem final deve ser maior que a inicial.',
                );
                return false;
            }

            // Validação: Km Final não pode exceder Km Inicial em mais de 2.000km
            const diff = fim - ini;
            if (diff > 2000) {
                await Alerta.Erro(
                    'Quilometragem Inválida',
                    `A quilometragem final não pode exceder a inicial em mais de 2.000 km.\n\nDiferença informada: ${diff.toLocaleString('pt-BR')} km`,
                );
                $('#txtKmFinal').val('');
                $('#txtKmFinal').focus();
                return false;
            }

            // Alerta (não bloqueante) se diferença > 100km
            // Só perguntar se ainda não foi confirmado nesta sessão de validação
            if (diff > 100 && !this._kmConfirmado) {
                const confirmacao = await Alerta.Confirmar(
                    'Atenção',
                    'A quilometragem <strong>final</strong> excede em 100km a <strong>inicial</strong>. Tem certeza?',
                    'Tenho certeza! 💪🏼',
                    'Me enganei! 😟',
                );

                if (!confirmacao) {
                    $('#txtKmFinal').val('');
                    $('#txtKmFinal').focus();
                    return false;
                }

                // Marcar como confirmado para não perguntar novamente
                this._kmConfirmado = true;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarKmInicialFinal',
                error,
            );
            return false;
        }
    }

    /**
     * Valida km final
     */
    async validarKmFinal() {
        try {
            const kmFinal = $('#txtKmFinal').val();

            if (kmFinal && parseFloat(kmFinal) <= 0) {
                await Alerta.Erro(
                    'Informação Incorreta',
                    'A Quilometragem Final deve ser maior que zero',
                );
                return false;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'validarKmFinal',
                error,
            );
            return false;
        }
    }

    /**
     * Confirma finalização da viagem
     */
    async confirmarFinalizacao() {
        try {
            const dataFinal = $('#txtDataFinal').val();
            const horaFinal = $('#txtHoraFinal').val();
            const combustivelFinal = document.getElementById(
                'ddtCombustivelFinal',
            ).ej2_instances[0].value;
            const kmFinal = $('#txtKmFinal').val();

            const todosFinalPreenchidos =
                dataFinal && horaFinal && combustivelFinal && kmFinal;

            // Só perguntar se ainda não foi confirmado nesta sessão de validação
            if (todosFinalPreenchidos && !this._finalizacaoConfirmada) {
                const confirmacao = await Alerta.Confirmar(
                    'Confirmar Fechamento',
                    'Você está criando a viagem como "Realizada". Deseja continuar?',
                    'Sim, criar!',
                    'Cancelar',
                );

                if (!confirmacao) return false;

                // Marcar como confirmado para não perguntar novamente
                this._finalizacaoConfirmada = true;
            }

            return true;
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'validacao.js',
                'confirmarFinalizacao',
                error,
            );
            return false;
        }
    }
}

// Instância global
window.ValidadorAgendamento = new ValidadorAgendamento();

/**
 * Função legacy de validação (mantida para compatibilidade)
 */
window.ValidaCampos = async function (viagemId) {
    try {
        return await window.ValidadorAgendamento.validar(viagemId);
    } catch (error) {
        Alerta.TratamentoErroComLinha('validacao.js', 'ValidaCampos', error);
        return false;
    }
};

/**
 * Validações assíncronas de datas
 */
window.validarDatas = async function () {
    try {
        const txtDataInicial = $('#txtDataInicial').val();
        const txtDataFinal = $('#txtDataFinal').val();

        if (!txtDataFinal || !txtDataInicial) return true;

        const dtInicial = window.parseDate(txtDataInicial);
        const dtFinal = window.parseDate(txtDataFinal);

        dtInicial.setHours(0, 0, 0, 0);
        dtFinal.setHours(0, 0, 0, 0);

        const diferenca = (dtFinal - dtInicial) / (1000 * 60 * 60 * 24);

        if (diferenca >= 5) {
            const confirmacao = await Alerta.Confirmar(
                'Atenção',
                'A Data Final está 5 dias ou mais após a Inicial. Tem certeza?',
                'Tenho certeza! 💪🏼',
                'Me enganei! 😟',
            );

            if (!confirmacao) {
                $('#txtDataFinal').val('');
                $('#txtDataFinal').focus();
                return false;
            }
        }

        return true;
    } catch (error) {
        Alerta.TratamentoErroComLinha('validacao.js', 'validarDatas', error);
        return false;
    }
};

window.validarDatasInicialFinal = async function (DataInicial, DataFinal) {
    try {
        const dtIni = window.parseDate(DataInicial);
        const dtFim = window.parseDate(DataFinal);

        if (!dtIni || !dtFim || isNaN(dtIni) || isNaN(dtFim)) return true;

        const diff = (dtFim - dtIni) / (1000 * 60 * 60 * 24);

        if (diff >= 5) {
            const confirmacao = await Alerta.Confirmar(
                'Atenção',
                'A Data Final está 5 dias ou mais após a Inicial. Tem certeza?',
                'Tenho certeza! 💪🏼',
                'Me enganei! 😟',
            );

            if (!confirmacao) {
                const txtDataFinalElement =
                    document.getElementById('txtDataFinal');
                txtDataFinalElement.value = null;
                txtDataFinalElement.focus();
                return false;
            }
        }

        return true;
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'validacao.js',
            'validarDatasInicialFinal',
            error,
        );
        return false;
    }
};
