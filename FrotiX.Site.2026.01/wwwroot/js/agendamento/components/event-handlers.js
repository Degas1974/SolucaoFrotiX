/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: event-handlers.js                                            ║
 * ║ 📍 LOCAL: wwwroot/js/agendamento/components/                            ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Handlers de Eventos do Formulário de Agendamento.                     ║
 * ║    • Eventos de change/blur/focus                                       ║
 * ║    • Armazenamento de dados originais                                   ║
 * ║    • Validações em tempo real                                            ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Alta (Agenda - Handlers)                                  ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

// ===================================================================
// VARIÁVEL GLOBAL: Armazena dados originais do requisitante
// ===================================================================
window.requisitanteOriginal = {
    id: null,
    ramal: null,
    setorId: null,
};

/**
 * ===================================================================
 * NOVO: Event handler para SELEÇÃO de requisitante (evento SELECT)
 * Preenche automaticamente ramal e setor quando um requisitante é escolhido
 * ===================================================================
 */
window.onSelectRequisitante = function (args) {
    console.log('🎯 Requisitante selecionado (SELECT event)!');
    console.log('📦 args:', args);

    try {
        // ===== OBTER ELEMENTOS DO DOM =====
        const txtRamal = document.getElementById('txtRamalRequisitante');
        const ddtSetorElement = document.getElementById(
            'lstSetorRequisitanteAgendamento',
        );

        // ===== VALIDAÇÃO DOS DADOS =====
        if (!args || !args.itemData || !args.itemData.RequisitanteId) {
            console.warn('⚠️ Dados inválidos no evento select');
            return;
        }

        const requisitanteId = args.itemData.RequisitanteId;
        console.log('✅ Requisitante ID:', requisitanteId);

        // ===== ARMAZENAR ID ORIGINAL =====
        window.requisitanteOriginal.id = requisitanteId;

        // ===== INDICADOR DE CARREGAMENTO =====
        if (txtRamal) {
            //    txtRamal.value = 'Carregando...';
            //    txtRamal.disabled = true;
        }

        // ===== BUSCAR RAMAL DO REQUISITANTE =====
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaRamal',
            method: 'GET',
            dataType: 'json',
            data: { id: requisitanteId },
            success: function (res) {
                console.log('📞 Resposta Ramal:', res);

                const ramalValue = res.data || res;

                if (
                    ramalValue !== null &&
                    ramalValue !== undefined &&
                    ramalValue !== ''
                ) {
                    // Buscar o componente Syncfusion
                    const ramalElement = document.getElementById(
                        'txtRamalRequisitanteSF',
                    );

                    if (
                        ramalElement &&
                        ramalElement.ej2_instances &&
                        ramalElement.ej2_instances[0]
                    ) {
                        const ramalTextBox = ramalElement.ej2_instances[0];

                        // Setar valor usando o método do Syncfusion
                        ramalTextBox.value = String(ramalValue);

                        // Forçar atualização visual
                        ramalTextBox.dataBind();

                        console.log(
                            '✓ Ramal atualizado (Syncfusion):',
                            ramalValue,
                        );
                    } else {
                        console.error(
                            '❌ TextBox Syncfusion não encontrado ou não inicializado',
                        );

                        // Fallback para input normal
                        if (ramalElement) {
                            ramalElement.value = ramalValue;
                        }
                    }

                    window.requisitanteOriginal.ramal = parseInt(ramalValue);
                } else {
                    // Limpar o campo
                    const ramalElement = document.getElementById(
                        'txtRamalRequisitanteSF',
                    );

                    if (
                        ramalElement &&
                        ramalElement.ej2_instances &&
                        ramalElement.ej2_instances[0]
                    ) {
                        ramalElement.ej2_instances[0].value = '';
                        ramalElement.ej2_instances[0].dataBind();
                    } else if (ramalElement) {
                        ramalElement.value = '';
                    }

                    window.requisitanteOriginal.ramal = null;
                    console.warn('⚠️ Ramal não encontrado ou vazio');
                }
            },
            error: function (xhr, status, error) {
                console.error('❌ Erro ao buscar ramal:', error);

                const ramalElement = document.getElementById(
                    'txtRamalRequisitanteSF',
                );
                if (
                    ramalElement &&
                    ramalElement.ej2_instances &&
                    ramalElement.ej2_instances[0]
                ) {
                    ramalElement.ej2_instances[0].value = '';
                    ramalElement.ej2_instances[0].enabled = true;
                } else if (ramalElement) {
                    ramalElement.value = '';
                }

                window.requisitanteOriginal.ramal = null;
                Alerta.Erro('Erro ao buscar ramal do requisitante');
            },
        });

        // ===== BUSCAR SETOR DO REQUISITANTE =====
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaSetor',
            method: 'GET',
            dataType: 'json',
            data: { id: requisitanteId },
            success: function (res) {
                console.log('🏢 Resposta Setor:', res);

                // A resposta pode vir como { data: valor } OU { success: true, data: valor }
                const setorValue = res.data || res;

                if (
                    setorValue !== null &&
                    setorValue !== undefined &&
                    setorValue !== ''
                ) {
                    // Verifica se o DropDownTree existe e tem instância
                    if (ddtSetorElement?.ej2_instances?.[0]) {
                        const ddtSetorObj = ddtSetorElement.ej2_instances[0];

                        // Define o valor do setor
                        ddtSetorObj.value = [setorValue];
                        ddtSetorObj.dataBind();

                        // Armazena valor original
                        window.requisitanteOriginal.setorId = setorValue;

                        console.log('✓ Setor atualizado:', setorValue);
                    } else {
                        console.error(
                            '❌ DropDownTree de setor não encontrado ou não inicializado',
                        );
                        console.log('Elemento encontrado:', ddtSetorElement);
                        console.log(
                            'Instâncias:',
                            ddtSetorElement?.ej2_instances,
                        );
                    }
                } else {
                    // Limpa o setor se não encontrou
                    if (ddtSetorElement?.ej2_instances?.[0]) {
                        const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                        ddtSetorObj.value = [];
                        ddtSetorObj.dataBind();
                    }

                    window.requisitanteOriginal.setorId = null;

                    console.warn('⚠️ Setor não encontrado ou vazio');
                }
            },
            error: function (xhr, status, error) {
                console.error('❌ Erro ao buscar setor:', error);
                console.error('Status:', status);
                console.error('Response:', xhr.responseText);

                // Limpa o setor em caso de erro
                if (ddtSetorElement?.ej2_instances?.[0]) {
                    const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                    ddtSetorObj.value = [];
                    ddtSetorObj.dataBind();
                }

                window.requisitanteOriginal.setorId = null;

                // Mostra mensagem ao usuário
                Alerta.Erro('Erro ao buscar setor do requisitante');
            },
        });
    } catch (error) {
        console.error('❌ Erro na função onSelectRequisitante:', error);
        Alerta.Erro('Erro ao processar seleção do requisitante');
    }
};

/**
 * ===================================================================
 * Event handler para SELEÇÃO de requisitante de EVENTO (evento SELECT)
 * Preenche automaticamente o setor quando um requisitante é escolhido
 * ===================================================================
 */
window.onSelectRequisitanteEvento = function (args) {
    console.log('🎯 Requisitante de EVENTO selecionado (SELECT event)!');
    console.log('📦 args:', args);

    try {
        // ===== OBTER ELEMENTO DO SETOR =====
        const ddtSetorElement = document.getElementById(
            'lstSetorRequisitanteEvento',
        );

        // ===== VALIDAÇÃO DOS DADOS =====
        if (!args || !args.itemData || !args.itemData.RequisitanteId) {
            console.warn('⚠️ Dados inválidos no evento select (Evento)');
            return;
        }

        const requisitanteId = args.itemData.RequisitanteId;
        console.log('✅ Requisitante ID (Evento):', requisitanteId);

        // ===== BUSCAR SETOR DO REQUISITANTE =====
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaSetor',
            method: 'GET',
            dataType: 'json',
            data: { id: requisitanteId },
            success: function (res) {
                console.log('🏢 Resposta Setor (Evento):', res);

                // A resposta pode vir como { data: valor } OU { success: true, data: valor }
                const setorValue = res.data || res;

                if (
                    setorValue !== null &&
                    setorValue !== undefined &&
                    setorValue !== ''
                ) {
                    // Verifica se o DropDownTree existe e tem instância
                    if (ddtSetorElement?.ej2_instances?.[0]) {
                        const ddtSetorObj = ddtSetorElement.ej2_instances[0];

                        // Define o valor do setor
                        ddtSetorObj.value = [setorValue];
                        ddtSetorObj.dataBind();

                        console.log('✓ Setor atualizado (Evento):', setorValue);
                    } else {
                        console.error(
                            '❌ DropDownTree de setor (Evento) não encontrado ou não inicializado',
                        );
                        console.log('Elemento encontrado:', ddtSetorElement);
                        console.log(
                            'Instâncias:',
                            ddtSetorElement?.ej2_instances,
                        );
                    }
                } else {
                    // Limpa o setor se não encontrou
                    if (ddtSetorElement?.ej2_instances?.[0]) {
                        const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                        ddtSetorObj.value = [];
                        ddtSetorObj.dataBind();
                    }

                    console.warn('⚠️ Setor não encontrado ou vazio (Evento)');
                }
            },
            error: function (xhr, status, error) {
                console.error('❌ Erro ao buscar setor (Evento):', error);
                console.error('Status:', status);
                console.error('Response:', xhr.responseText);

                // Limpa o setor em caso de erro
                if (ddtSetorElement?.ej2_instances?.[0]) {
                    const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                    ddtSetorObj.value = [];
                    ddtSetorObj.dataBind();
                }

                // Mostra mensagem ao usuário
                Alerta.Erro('Erro ao buscar setor do requisitante');
            },
        });
    } catch (error) {
        console.error('❌ Erro na função onSelectRequisitanteEvento:', error);
        Alerta.Erro('Erro ao processar seleção do requisitante do evento');
    }
};

/**
 * Event handler para mudança da Finalidade
 * Exibe o card de Evento quando Finalidade = "Evento"
 */
window.lstFinalidade_Change = function (args) {
    try {
        console.log('📋 Finalidade mudou:', args.value, args.itemData);

        const sectionEvento = document.getElementById('sectionEvento');
        const modalEvento = document.getElementById('modalEvento');

        if (!sectionEvento) {
            console.error('❌ sectionEvento não encontrado no DOM');
            return;
        }

        // Verificar se a finalidade é "Evento"
        const finalidadeSelecionada =
            args.itemData?.text || args.itemData?.Descricao || '';

        console.log('🔍 Finalidade selecionada:', finalidadeSelecionada);

        if (finalidadeSelecionada.toLowerCase().includes('evento')) {
            // ✅ MOSTRAR o card de evento (botão é controlado por evento.js)
            sectionEvento.style.display = 'block';
            console.log('✅ Seção de Evento exibida');
        } else {
            // ❌ ESCONDER o card de evento e de cadastro (botão é controlado por evento.js)
            sectionEvento.style.display = 'none';

            if (modalEvento && window.bootstrap && window.bootstrap.Modal) {
                window.bootstrap.Modal.getOrCreateInstance(modalEvento).hide();
            }

            // ❌ LIMPAR o lstEventos (mantém habilitado)
            const lstEventosElement = document.getElementById('lstEventos');
            if (
                lstEventosElement &&
                lstEventosElement.ej2_instances &&
                lstEventosElement.ej2_instances[0]
            ) {
                lstEventosElement.ej2_instances[0].value = null;
                lstEventosElement.ej2_instances[0].dataBind();
                console.log('✅ lstEventos limpo');
            }

            console.log('➖ Seção de Evento escondida');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'event-handlers.js',
            'lstFinalidade_Change',
            error,
        );
    }
};
/**
 * Handler para mudança de requisitante (evento CHANGE - mantido para compatibilidade)
 * NOTA: Agora o preenchimento automático é feito pelo evento SELECT
 */
window.RequisitanteValueChange = function () {
    try {
        // lstRequisitante é um Kendo ComboBox, não Syncfusion EJ2
        const comboBox = $('#lstRequisitante').data('kendoComboBox');

        if (!comboBox) {
            console.warn('⚠️ lstRequisitante (Kendo ComboBox) não encontrado');
            return;
        }

        if (comboBox.value() === null || comboBox.value() === '') {
            return;
        }

        const requisitanteid = String(comboBox.value());

        // NOTA: O código de buscar ramal e setor foi movido para onSelectRequisitante
        // Mantendo esta função para compatibilidade com outros códigos que possam usar
        console.log(
            'ℹ️ RequisitanteValueChange chamado (requisitante ID:',
            requisitanteid,
            ')',
        );
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'event-handlers.js',
            'RequisitanteValueChange',
            error,
        );
    }
};

/**
 * Handler para mudança de motorista
 */
window.MotoristaValueChange = function () {
    try {
        const ddTreeObj =
            document.getElementById('lstMotorista').ej2_instances[0];

        console.log('Objeto Motorista:', ddTreeObj);

        if (ddTreeObj.value === null || ddTreeObj.enabled === false) {
            return;
        }

        const motoristaid = String(ddTreeObj.value);
        return motoristaid;
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'event-handlers.js',
            'MotoristaValueChange',
            error,
        );
    }
};

/**
 * Handler para mudança de veículo
 */
window.VeiculoValueChange = function () {
    try {
        const ddTreeObj =
            document.getElementById('lstVeiculo').ej2_instances[0];

        console.log('Objeto Veículo:', ddTreeObj);

        if (ddTreeObj.value === null || ddTreeObj.enabled === false) {
            return;
        }

        const veiculoid = String(ddTreeObj.value);

        // Pega Km Atual do Veículo
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaKmAtualVeiculo',
            method: 'GET',
            datatype: 'json',
            data: { id: veiculoid },
            success: function (res) {
                const km = res.data;
                const kmAtual = document.getElementById('txtKmAtual');
                kmAtual.value = km;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                const erro = window.criarErroAjax(
                    jqXHR,
                    textStatus,
                    errorThrown,
                    this,
                );
                Alerta.TratamentoErroComLinha(
                    'event-handlers.js',
                    'VeiculoValueChange',
                    erro,
                );
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'event-handlers.js',
            'VeiculoValueChange',
            error,
        );
    }
};

/**
 * Handler para mudança de requisitante no evento
 */
window.RequisitanteEventoValueChange = function () {
    try {
        const ddTreeObj = document.getElementById('lstRequisitanteEvento')
            .ej2_instances[0];

        if (ddTreeObj.value === null || ddTreeObj.value === '') {
            return;
        }

        const requisitanteid = String(ddTreeObj.value);

        // Pega Setor Padrío do Requisitante
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaSetor',
            method: 'GET',
            datatype: 'json',
            data: { id: requisitanteid },
            success: function (res) {
                document.getElementById(
                    'ddtSetorEvento',
                ).ej2_instances[0].value = [res.data];
            },
            error: function (jqXHR, textStatus, errorThrown) {
                const erro = window.criarErroAjax(
                    jqXHR,
                    textStatus,
                    errorThrown,
                    this,
                );
                Alerta.TratamentoErroComLinha(
                    'event-handlers.js',
                    'RequisitanteEventoValueChange',
                    erro,
                );
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'event-handlers.js',
            'RequisitanteEventoValueChange',
            error,
        );
    }
};

/**
 * Handler para mudança de data no calendário
 */
window.onDateChange = function (args) {
    try {
        const selectedDates = args.model.values;

        // Get the ListBox element
        const listbox = document.getElementById('selectedDates');
        listbox.innerHTML = '';

        // Add each selected date to the ListBox
        selectedDates.forEach(function (date) {
            const li = document.createElement('li');
            li.textContent = new Date(date).toLocaleDateString();
            listbox.appendChild(li);
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'event-handlers.js',
            'onDateChange',
            error,
        );
    }
};

/**
 * ====================================
 * HANDLER DE SELEÇÃO DE EVENTO
 * ====================================
 * Controla a exibição e preenchimento dos campos quando um evento é selecionado
 */

// Inicializar o evento de seleção do ComboBox de Eventos
function inicializarEventoSelect() {
    try {
        // Obter a instância do ComboBox de Eventos
        const lstEventosElement = document.getElementById('lstEventos');

        if (!lstEventosElement) {
            console.warn('⚠️ ComboBox lstEventos não encontrado');
            return;
        }

        const lstEventos = ej.base.getComponent(lstEventosElement, 'combobox');

        if (!lstEventos) {
            console.warn('⚠️ Instância do ComboBox lstEventos não encontrada');
            return;
        }

        // Adicionar evento de seleção
        lstEventos.select = function (args) {
            if (args.itemData) {
                // Evento selecionado - mostrar os campos e preencher
                exibirDadosEvento(args.itemData);
            }
        };

        // Também adicionar evento de clear para limpar quando o botão X for clicado
        lstEventos.clearing = function (args) {
            // Nenhum evento selecionado - esconder os campos
            ocultarDadosEvento();
        };

        console.log('✅ Handler de seleção de evento inicializado');
    } catch (error) {
        console.error('❌ Erro ao inicializar handler de evento:', error);
    }
}

/**
 * Exibe e preenche os campos com os dados do evento selecionado
 * param {Object} eventoData - Dados do evento selecionado
 */
function exibirDadosEvento(eventoData) {
    try {
        console.log('📋 Exibindo dados do evento:', eventoData);
        console.log(
            '🔍 Estrutura completa do objeto:',
            JSON.stringify(eventoData, null, 2),
        );

        // Mostrar a div dos dados do evento
        const divDados = document.getElementById('divDadosEventoSelecionado');
        if (divDados) {
            divDados.style.display = 'flex';
        }

        // Buscar os dados completos do evento pelo ID
        const eventoId = eventoData.EventoId || eventoData.eventoId;
        console.log('🆔 EventoId:', eventoId);

        if (eventoId) {
            // Fazer requisição AJAX para buscar os dados completos do evento
            $.ajax({
                url: '/api/ViagemEvento/ObterPorId',
                method: 'GET',
                data: { id: eventoId },
                success: function (response) {
                    console.log(
                        '✅ Dados do evento recebidos da API:',
                        response,
                    );

                    if (response.success && response.data) {
                        preencherCamposEvento(response.data);
                    } else {
                        console.warn(
                            '⚠️ Resposta da API sem dados, usando itemData...',
                        );
                        preencherCamposEvento(eventoData);
                    }
                },
                error: function (xhr, status, error) {
                    console.error('❌ Erro ao buscar dados do evento:', error);
                    console.log('⚠️ Tentando usar dados do itemData...');
                    preencherCamposEvento(eventoData);
                },
            });
        } else {
            console.log(
                '⚠️ EventoId não encontrado, usando dados do itemData...',
            );
            preencherCamposEvento(eventoData);
        }
    } catch (error) {
        console.error('❌ Erro ao exibir dados do evento:', error);
    }
}

/**
 * Preenche os campos com os dados do evento
 * param {Object} dados - Dados do evento
 */
function preencherCamposEvento(dados) {
    try {
        console.log('📝 Preenchendo campos com:', dados);

        // Preencher Data Início
        const dataInicial =
            dados.DataInicial || dados.dataInicial || dados.DataInicialEvento;
        if (dataInicial) {
            const dtInicio = ej.base.getComponent(
                document.getElementById('txtDataInicioEvento'),
                'datepicker',
            );
            if (dtInicio) {
                dtInicio.value = new Date(dataInicial);
                console.log('✅ Data Início preenchida:', dataInicial);
            }
        } else {
            console.warn('⚠️ Data Inicial não encontrada no objeto');
        }

        // Preencher Data Fim
        const dataFinal =
            dados.DataFinal || dados.dataFinal || dados.DataFinalEvento;
        if (dataFinal) {
            const dtFim = ej.base.getComponent(
                document.getElementById('txtDataFimEvento'),
                'datepicker',
            );
            if (dtFim) {
                dtFim.value = new Date(dataFinal);
                console.log('✅ Data Fim preenchida:', dataFinal);
            }
        } else {
            console.warn('⚠️ Data Final não encontrada no objeto');
        }

        // Preencher Quantidade de Participantes (Kendo NumericTextBox)
        const qtdParticipantes =
            dados.QtdParticipantes || dados.qtdParticipantes;
        console.log(
            '🔍 Tentando preencher QtdParticipantes com valor:',
            qtdParticipantes,
        );

        if (qtdParticipantes !== undefined && qtdParticipantes !== null) {
            const numParticipantes = $('#txtQtdParticipantesEvento').data(
                'kendoNumericTextBox',
            );
            if (numParticipantes) {
                numParticipantes.value(qtdParticipantes);
                console.log(
                    '✅ Qtd Participantes preenchida:',
                    qtdParticipantes,
                );
            } else {
                console.error(
                    '❌ Componente NumericTextBox Kendo não encontrado!',
                );
            }
        } else {
            console.warn(
                '⚠️ QtdParticipantes não encontrado no objeto. Valor recebido:',
                qtdParticipantes,
            );
            console.log('📋 Objeto completo recebido:', dados);
        }

        console.log('✅ Dados do evento preenchidos com sucesso');
    } catch (error) {
        console.error('❌ Erro ao preencher campos do evento:', error);
    }
}

/**
 * Oculta e limpa os campos do evento
 */
function ocultarDadosEvento() {
    try {
        console.log('🙈 Ocultando dados do evento');

        // Esconder a div dos dados do evento
        const divDados = document.getElementById('divDadosEventoSelecionado');
        if (divDados) {
            divDados.style.display = 'none';
        }

        // Limpar Data Início
        const dtInicio = ej.base.getComponent(
            document.getElementById('txtDataInicioEvento'),
            'datepicker',
        );
        if (dtInicio) {
            dtInicio.value = null;
        }

        // Limpar Data Fim
        const dtFim = ej.base.getComponent(
            document.getElementById('txtDataFimEvento'),
            'datepicker',
        );
        if (dtFim) {
            dtFim.value = null;
        }

        // Limpar Quantidade de Participantes (Kendo NumericTextBox)
        const numParticipantes = $('#txtQtdParticipantesEvento').data(
            'kendoNumericTextBox',
        );
        if (numParticipantes) {
            numParticipantes.value(null);
        }

        console.log('✅ Dados do evento limpos');
    } catch (error) {
        console.error('❌ Erro ao ocultar dados do evento:', error);
    }
}

/**
 * Handler de criação do ComboBox de Motorista
 * Configura os templates para exibir fotos dos motoristas
 */
window.onLstMotoristaCreated = function () {
    try {
        console.log('🎯 onLstMotoristaCreated chamado');

        const combo = document.getElementById('lstMotorista');

        if (!combo || !combo.ej2_instances || !combo.ej2_instances[0]) {
            console.warn('❌ lstMotorista não encontrado');
            return;
        }

        const comboInstance = combo.ej2_instances[0];

        // Template para itens da lista
        comboInstance.itemTemplate = function (data) {
            let imgSrc =
                data.FotoBase64 && data.FotoBase64.startsWith('data:image')
                    ? data.FotoBase64
                    : '/images/barbudo.jpg';

            return `
                <div class="d-flex align-items-center">
                    <img src="${imgSrc}" 
                         alt="Foto" 
                         style="height:40px; width:40px; border-radius:50%; margin-right:10px; object-fit: cover;" 
                         onerror="this.src='/images/barbudo.jpg';" />
                    <span>${data.Nome}</span>
                </div>`;
        };

        // Template para valor selecionado
        comboInstance.valueTemplate = function (data) {
            if (!data) return '';

            let imgSrc =
                data.FotoBase64 && data.FotoBase64.startsWith('data:image')
                    ? data.FotoBase64
                    : '/images/barbudo.jpg';

            return `
                <div class="d-flex align-items-center">
                    <img src="${imgSrc}" 
                         alt="Foto" 
                         style="height:30px; width:30px; border-radius:50%; margin-right:10px; object-fit: cover;" 
                         onerror="this.src='/images/barbudo.jpg';" />
                    <span>${data.Nome}</span>
                </div>`;
        };

        console.log('✅ Templates de motorista configurados com sucesso');
    } catch (error) {
        console.error('❌ Erro:', error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha) {
            Alerta.TratamentoErroComLinha(
                'event-handlers.js',
                'onLstMotoristaCreated',
                error,
            );
        }
    }
};

// ====================================================================
// EXPORTAÇÕES GLOBAIS
// ====================================================================
window.exibirDadosEvento = exibirDadosEvento;
window.ocultarDadosEvento = ocultarDadosEvento;
