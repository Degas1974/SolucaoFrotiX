/* ****************************************************************************************
 * ⚡ ARQUIVO: event-handlers.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Event handlers para componentes do formulário de agendamento de
 *                   viagens. Gerencia eventos de seleção/mudança de requisitante (com
 *                   preenchimento automático de ramal/setor via AJAX), motorista (com
 *                   templates de foto), veículo (busca km atual), finalidade (exibe/oculta
 *                   card de evento), eventos (preenche data início/fim/participantes),
 *                   e datas selecionadas (calendário multi-select). Armazena valores
 *                   originais em window.requisitanteOriginal para restauração.
 * 📥 ENTRADAS     : args objects (Syncfusion/Kendo event args com itemData, value, model),
 *                   IDs de elementos DOM (#lstRequisitante, #lstMotorista, #lstVeiculo,
 *                   etc.), referências globais (window.requisitanteOriginal)
 * 📤 SAÍDAS       : Campos preenchidos automaticamente (ramal, setor, km atual, dados
 *                   evento), templates aplicados (motorista com foto), divs exibidas/
 *                   ocultas (#sectionEvento, #divDadosEventoSelecionado), listbox
 *                   atualizada (selectedDates), console.log (produção!), valores retornados
 *                   (motorista ID), Alerta.Erro em caso de falha AJAX
 * 🔗 CHAMADA POR  : controls-init.js (atribuição de eventos), event listeners Syncfusion/
 *                   Kendo, inicializarEventoSelect (DOMContentLoaded)
 * 🔄 CHAMA        : $.ajax (GET requests para PegaRamal, PegaSetor, PegaKmAtualVeiculo,
 *                   ObterPorId), document.getElementById, jQuery $().val()/$().data(),
 *                   Kendo $(el).data("kendoComboBox"/"kendoDatePicker"/"kendoNumericTextBox"),
 *                   window.getSyncfusionInstance() bridge (DropDownTree, ComboBox),
 *                   Alerta.TratamentoErroComLinha, Alerta.Erro, window.criarErroAjax,
 *                   window.bootstrap.Modal, console.log/warn/error
 * 📦 DEPENDÊNCIAS : jQuery ($.ajax, $().val(), $().data()), Kendo UI (kendoComboBox,
 *                   kendoDatePicker, kendoNumericTextBox), window.getSyncfusionInstance
 *                   bridge (DropDownTree, ComboBox Syncfusion restantes),
 *                   Bootstrap 5 (Modal), Alerta.js, window.criarErroAjax,
 *                   imagens (/images/barbudo.jpg)
 * 📝 OBSERVAÇÕES  : Exporta 11 funções window.* + 1 variável global (requisitanteOriginal).
 *                   Todas têm try-catch completo. console.log em produção (80+ logs).
 *                   Typo "padrío" (line 473). AJAX responses: res.data || res (flexible).
 *                   onSelectRequisitante faz 2 AJAX paralelos (ramal + setor).
 *                   RequisitanteEventoValueChange duplica lógica de setor (deveria usar
 *                   onSelectRequisitanteEvento). exibirDadosEvento faz AJAX adicional
 *                   para buscar dados completos. inicializarEventoSelect não exportada
 *                   (setup interno).
 *
 * 📋 ÍNDICE DE FUNÇÕES (14 funções: 11 window.*, 3 internas):
 *
 * ┌─ VARIÁVEL GLOBAL ─────────────────────────────────────────────────────┐
 * │ 1. window.requisitanteOriginal = { id, ramal, setorId }               │
 * │    → Armazena valores originais para restauração                      │
 * │    → Populado por onSelectRequisitante                                │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ REQUISITANTE (auto-fill ramal + setor) ─────────────────────────────┐
 * │ 2. window.onSelectRequisitante(args)                                  │
 * │    → Evento SELECT de lstRequisitante (quando seleciona da lista)    │
 * │    → Valida args.itemData.RequisitanteId                              │
 * │    → Armazena window.requisitanteOriginal.id                          │
 * │    → AJAX 1: GET /Viagens/Upsert?handler=PegaRamal                    │
 * │      success: preenche txtRamalRequisitanteSF (Syncfusion TextBox)   │
 * │               armazena window.requisitanteOriginal.ramal              │
 * │      fallback: preenche ramalElement.value (HTML input)               │
 * │      error: limpa campo + Alerta.Erro                                 │
 * │    → AJAX 2: GET /Viagens/Upsert?handler=PegaSetor (paralelo)        │
 * │      success: preenche lstSetorRequisitanteAgendamento (DropDownTree)│
 * │               armazena window.requisitanteOriginal.setorId            │
 * │      error: limpa campo + Alerta.Erro                                 │
 * │    → try-catch: console.error + Alerta.Erro                           │
 * │                                                                        │
 * │ 3. window.onSelectRequisitanteEvento(args)                            │
 * │    → Evento SELECT de lstRequisitanteEvento                           │
 * │    → Valida args.itemData.RequisitanteId                              │
 * │    → AJAX: GET /Viagens/Upsert?handler=PegaSetor                      │
 * │      success: preenche lstSetorRequisitanteEvento (DropDownTree)     │
 * │      error: limpa campo + Alerta.Erro                                 │
 * │    → try-catch: console.error + Alerta.Erro                           │
 * │                                                                        │
 * │ 4. window.RequisitanteValueChange()                                   │
 * │    → Evento CHANGE de lstRequisitante (Kendo ComboBox)               │
 * │    → LEGACY: mantido para compatibilidade                             │
 * │    → Agora auto-fill é feito por onSelectRequisitante (SELECT event) │
 * │    → Apenas valida comboBox.value() !== null/''                       │
 * │    → console.log "RequisitanteValueChange chamado"                    │
 * │    → try-catch: Alerta.TratamentoErroComLinha                         │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FINALIDADE (show/hide evento card) ─────────────────────────────────┐
 * │ 5. window.lstFinalidade_Change(args)                                  │
 * │    → Evento CHANGE de lstFinalidade                                   │
 * │    → Se finalidade.toLowerCase().includes("evento"):                  │
 * │      • sectionEvento.style.display = "block"                          │
 * │    → Senão:                                                            │
 * │      • sectionEvento.style.display = "none"                           │
 * │      • Fecha modalEvento (Bootstrap Modal)                            │
 * │      • Limpa lstEventos.value = null + dataBind()                     │
 * │    → try-catch: Alerta.TratamentoErroComLinha                         │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MOTORISTA (template com fotos) ─────────────────────────────────────┐
 * │ 6. window.MotoristaValueChange()                                      │
 * │    → Evento CHANGE de lstMotorista                                    │
 * │    → Retorna String(ddTreeObj.value) (motorista ID)                   │
 * │    → console.log "Objeto Motorista"                                   │
 * │    → Se value===null || enabled===false: retorna undefined            │
 * │    → try-catch: Alerta.TratamentoErroComLinha                         │
 * │                                                                        │
 * │ 7. window.onLstMotoristaCreated()                                     │
 * │    → Evento CREATED de lstMotorista                                   │
 * │    → Define itemTemplate: div.d-flex com img (40x40px) + span        │
 * │      • Img: data.FotoBase64 (se startsWith 'data:image')             │
 * │             senão '/images/barbudo.jpg'                               │
 * │      • onerror: this.src='/images/barbudo.jpg'                        │
 * │      • Text: data.Nome                                                │
 * │    → Define valueTemplate: div.d-flex com img (30x30px) + span       │
 * │      • Idêntico a itemTemplate mas tamanho diferente                  │
 * │    → console.log "Templates de motorista configurados"                │
 * │    → try-catch: console.error + Alerta.TratamentoErroComLinha (se existir)│
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VEÍCULO (auto-fill km atual) ───────────────────────────────────────┐
 * │ 8. window.VeiculoValueChange()                                        │
 * │    → Evento CHANGE de lstVeiculo                                      │
 * │    → console.log "Objeto Veículo"                                     │
 * │    → Se value===null || enabled===false: retorna undefined            │
 * │    → AJAX: GET /Viagens/Upsert?handler=PegaKmAtualVeiculo             │
 * │      data: { id: veiculoid }                                          │
 * │      success: preenche txtKmAtual.value = res.data (km atual)         │
 * │      error: criarErroAjax + Alerta.TratamentoErroComLinha             │
 * │    → try-catch: Alerta.TratamentoErroComLinha                         │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ REQUISITANTE EVENTO (auto-fill setor) ──────────────────────────────┐
 * │ 9. window.RequisitanteEventoValueChange()                             │
 * │    → Evento CHANGE de lstRequisitanteEvento                           │
 * │    → Typo "padrío" (deveria ser "padrão") no comentário              │
 * │    → Se value===null || value==='': retorna                           │
 * │    → AJAX: GET /Viagens/Upsert?handler=PegaSetor                      │
 * │      success: preenche ddtSetorEvento.value = [res.data]              │
 * │      error: criarErroAjax + Alerta.TratamentoErroComLinha             │
 * │    → NOTA: duplica lógica de onSelectRequisitanteEvento               │
 * │    → try-catch: Alerta.TratamentoErroComLinha                         │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CALENDÁRIO (multi-select dates) ────────────────────────────────────┐
 * │ 10. window.onDateChange(args)                                         │
 * │     → Evento de calendário multi-select                               │
 * │     → Obtém selectedDates = args.model.values                         │
 * │     → Limpa listbox: listbox.innerHTML = ''                           │
 * │     → Para cada date: cria <li> com toLocaleDateString()              │
 * │     → Appends <li> ao listbox #selectedDates                          │
 * │     → try-catch: Alerta.TratamentoErroComLinha                        │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EVENTO (auto-fill data início/fim/participantes) ───────────────────┐
 * │ 11. inicializarEventoSelect() (interna, não exportada)               │
 * │     → Obtém lstEventos (ComboBox Syncfusion)                          │
 * │     → Define select event: exibirDadosEvento(args.itemData)           │
 * │     → Define clearing event: ocultarDadosEvento()                     │
 * │     → console.log "Handler de seleção de evento inicializado"         │
 * │     → try-catch: console.error                                        │
 * │                                                                        │
 * │ 12. exibirDadosEvento(eventoData) (exportada window.*)                │
 * │     → Mostra divDadosEventoSelecionado (display='flex')               │
 * │     → Extrai eventoId = eventoData.EventoId || eventoId               │
 * │     → AJAX: GET /api/ViagemEvento/ObterPorId?id={eventoId}            │
 * │       success: se response.success && response.data:                  │
 * │                  preencherCamposEvento(response.data)                 │
 * │                senão: preencherCamposEvento(eventoData)               │
 * │       error: console.error + preencherCamposEvento(eventoData)        │
 * │     → Se !eventoId: preencherCamposEvento(eventoData) direto          │
 * │     → try-catch: console.error                                        │
 * │                                                                        │
 * │ 13. preencherCamposEvento(dados) (interna, não exportada)             │
 * │     → Extrai dataInicial = dados.DataInicial || dataInicial || ...    │
 * │       Preenche txtDataInicioEvento (DatePicker): value = new Date()   │
 * │     → Extrai dataFinal = dados.DataFinal || dataFinal || ...          │
 * │       Preenche txtDataFimEvento (DatePicker): value = new Date()      │
 * │     → Extrai qtdParticipantes = dados.QtdParticipantes || ...         │
 * │       Preenche txtQtdParticipantesEvento (NumericTextBox): value      │
 * │     → console.log para cada campo preenchido ou warning se ausente    │
 * │     → try-catch: console.error                                        │
 * │                                                                        │
 * │ 14. ocultarDadosEvento() (exportada window.*)                         │
 * │     → Esconde divDadosEventoSelecionado (display='none')              │
 * │     → Limpa txtDataInicioEvento: value = null                         │
 * │     → Limpa txtDataFimEvento: value = null                            │
 * │     → Limpa txtQtdParticipantesEvento: value = null                   │
 * │     → console.log "Dados do evento limpos"                            │
 * │     → try-catch: console.error                                        │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE PREENCHIMENTO AUTOMÁTICO REQUISITANTE:
 * 1. Usuário seleciona requisitante no dropdown (lstRequisitante)
 * 2. Syncfusion dispara evento SELECT → onSelectRequisitante(args)
 * 3. Extrai RequisitanteId de args.itemData
 * 4. Armazena window.requisitanteOriginal.id
 * 5. AJAX paralelo 1: GET PegaRamal
 *    a. success: preenche txtRamalRequisitanteSF (Syncfusion) ou fallback HTML
 *    b. armazena window.requisitanteOriginal.ramal
 *    c. error: limpa campo + Alerta.Erro
 * 6. AJAX paralelo 2: GET PegaSetor
 *    a. success: preenche lstSetorRequisitanteAgendamento (DropDownTree)
 *    b. armazena window.requisitanteOriginal.setorId
 *    c. error: limpa campo + Alerta.Erro
 * 7. Usuário vê ramal e setor preenchidos automaticamente
 *
 * 🔄 FLUXO DE EVENTO SELECIONADO:
 * 1. Usuário seleciona evento no dropdown (lstEventos)
 * 2. inicializarEventoSelect configurou select event
 * 3. select dispara → exibirDadosEvento(args.itemData)
 * 4. Mostra divDadosEventoSelecionado
 * 5. Extrai eventoId
 * 6. Se eventoId:
 *    a. AJAX GET /api/ViagemEvento/ObterPorId
 *    b. success: preencherCamposEvento(response.data)
 *    c. error: preencherCamposEvento(eventoData) fallback
 * 7. preencherCamposEvento:
 *    a. txtDataInicioEvento = dataInicial
 *    b. txtDataFimEvento = dataFinal
 *    c. txtQtdParticipantesEvento = qtdParticipantes
 * 8. Usuário vê campos preenchidos
 * 9. Se clicar X (clearing): ocultarDadosEvento() → esconde div + limpa campos
 *
 * 🔄 FLUXO DE MUDANÇA DE FINALIDADE:
 * 1. Usuário seleciona Finalidade (lstFinalidade)
 * 2. lstFinalidade_Change(args) dispara
 * 3. Se finalidade.includes("evento"):
 *    a. sectionEvento.display = "block" (mostra card)
 * 4. Senão:
 *    a. sectionEvento.display = "none" (esconde card)
 *    b. Fecha modalEvento (Bootstrap Modal)
 *    c. Limpa lstEventos
 *
 * 📌 DIFERENÇAS ENTRE EVENTOS SELECT vs CHANGE:
 * - SELECT: dispara ao selecionar item da lista dropdown (não dispara ao digitar)
 * - CHANGE: dispara ao mudar valor (inclusive digitação, clear, programático)
 * - onSelectRequisitante usa SELECT (melhor para auto-fill, evita disparar em digitação)
 * - RequisitanteValueChange usa CHANGE (legacy, mantido para compatibilidade)
 *
 * 📌 ESTRUTURA DE AJAX RESPONSES:
 * - Flexible: res.data || res (suporta {data: valor} ou valor direto)
 * - PegaRamal: retorna número (ramal)
 * - PegaSetor: retorna ID (setorId)
 * - PegaKmAtualVeiculo: retorna número (km)
 * - ObterPorId: retorna {success, data: {DataInicial, DataFinal, QtdParticipantes, ...}}
 *
 * 📌 COMPONENTS UI ENVOLVIDOS:
 * - Kendo: lstRequisitante (ComboBox) - usa $(el).data("kendoComboBox")
 * - Syncfusion: lstFinalidade, lstMotorista, lstVeiculo, lstRequisitanteEvento,
 *               lstEventos, lstSetorRequisitanteAgendamento, lstSetorRequisitanteEvento,
 *               txtRamalRequisitanteSF, txtDataInicioEvento, txtDataFimEvento,
 *               txtQtdParticipantesEvento, ddtSetorEvento
 * - Bootstrap: modalEvento (Modal)
 * - HTML native: txtKmAtual, selectedDates (listbox)
 *
 * 📌 TEMPLATES MOTORISTA:
 * - itemTemplate: dropdown items (40x40px foto circular + nome)
 * - valueTemplate: selected value (30x30px foto circular + nome)
 * - Foto: data.FotoBase64 (base64 data URI) ou /images/barbudo.jpg
 * - onerror: duplo fallback para /images/barbudo.jpg
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - console.log em produção (80+ logs): console.log, console.warn, console.error
 * - Typo "padrío" (line 473) deveria ser "padrão"
 * - RequisitanteEventoValueChange duplica lógica de onSelectRequisitanteEvento
 * - AJAX paralelo em onSelectRequisitante (ramal + setor simultaneamente)
 * - exibirDadosEvento faz AJAX adicional para dados completos (ObterPorId)
 * - preencherCamposEvento flexível com aliases (DataInicial || dataInicial || ...)
 * - inicializarEventoSelect não exportada (setup interno, deve ser chamada em init)
 * - window.requisitanteOriginal usado para restaurar valores (possível undo)
 * - window.getSyncfusionInstance() bridge para DropDownTree/ComboBox Syncfusion restantes
 * - Kendo $("#id").data("kendoDatePicker"/"kendoNumericTextBox") para controles já migrados
 * - Evento clearing (lstEventos) chama ocultarDadosEvento quando clica X
 * - Código emoji: 🎯 🔧 ✅ ❌ ⚠️ 📦 🏢 📋 🔍 📝 🙈 📞 🆔
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

// ===================================================================
// VARIÁVEL GLOBAL: Armazena dados originais do requisitante
// ===================================================================
window.requisitanteOriginal = {
    id: null,
    ramal: null,
    setorId: null
};

/**
 * ===================================================================
 * NOVO: Event handler para SELEÇÃO de requisitante (evento SELECT)
 * Preenche automaticamente ramal e setor quando um requisitante é escolhido
 * ===================================================================
 */
window.onSelectRequisitante = function (args)
{
    console.log('🎯 Requisitante selecionado (SELECT event)!');
    console.log('📦 args:', args);

    try
    {
        // ===== OBTER ELEMENTOS DO DOM =====
        const txtRamal = document.getElementById("txtRamalRequisitante");
        const ddtSetorElement = document.getElementById("lstSetorRequisitanteAgendamento");

        // ===== VALIDAÇÃO DOS DADOS =====
        if (!args || !args.itemData || !args.itemData.RequisitanteId)
        {
            console.warn('⚠️ Dados inválidos no evento select');
            return;
        }

        const requisitanteId = args.itemData.RequisitanteId;
        console.log('✅ Requisitante ID:', requisitanteId);

        // ===== ARMAZENAR ID ORIGINAL =====
        window.requisitanteOriginal.id = requisitanteId;

        // ===== INDICADOR DE CARREGAMENTO =====
        if (txtRamal)
        {
            //    txtRamal.value = 'Carregando...';
            //    txtRamal.disabled = true;
        }

        // ===== BUSCAR RAMAL DO REQUISITANTE =====
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaRamal',
            method: "GET",
            dataType: "json",
            data: { id: requisitanteId },
            success: function (res)
            {
                console.log('📞 Resposta Ramal:', res);

                const ramalValue = res.data || res;

                if (ramalValue !== null && ramalValue !== undefined && ramalValue !== '')
                {
                    // Setar valor via jQuery (input simples, não é widget Kendo)
                    const $ramalInput = $('#txtRamalRequisitanteSF');

                    if ($ramalInput.length)
                    {
                        $ramalInput.val(String(ramalValue));
                        console.log('✓ Ramal atualizado (jQuery):', ramalValue);
                    } else
                    {
                        console.error('❌ Input txtRamalRequisitanteSF não encontrado');
                    }

                    window.requisitanteOriginal.ramal = parseInt(ramalValue);
                } else
                {
                    // Limpar o campo via jQuery
                    const $ramalEmpty = $('#txtRamalRequisitanteSF');
                    if ($ramalEmpty.length)
                    {
                        $ramalEmpty.val('');
                    }

                    window.requisitanteOriginal.ramal = null;
                    console.warn('⚠️ Ramal não encontrado ou vazio');
                }
            },
            error: function (xhr, status, error)
            {
                console.error('❌ Erro ao buscar ramal:', error);

                const $ramalError = $('#txtRamalRequisitanteSF');
                if ($ramalError.length)
                {
                    $ramalError.val('');
                    $ramalError.prop('disabled', false);
                }

                window.requisitanteOriginal.ramal = null;
                Alerta.Erro('Erro ao buscar ramal do requisitante');
            }
        });

        // ===== BUSCAR SETOR DO REQUISITANTE =====
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaSetor',
            method: "GET",
            dataType: "json",
            data: { id: requisitanteId },
            success: function (res)
            {
                console.log('🏢 Resposta Setor:', res);

                // A resposta pode vir como { data: valor } OU { success: true, data: valor }
                const setorValue = res.data || res;

                if (setorValue !== null && setorValue !== undefined && setorValue !== '')
                {
                    // Verifica se o DropDownTree existe via bridge
                    const ddtSetorObj = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteAgendamento") : null;
                    if (ddtSetorObj)
                    {
                        // Define o valor do setor
                        ddtSetorObj.value = [setorValue];
                        ddtSetorObj.dataBind();

                        // Armazena valor original
                        window.requisitanteOriginal.setorId = setorValue;

                        console.log('✓ Setor atualizado:', setorValue);
                    } else
                    {
                        console.error('❌ DropDownTree de setor não encontrado via getSyncfusionInstance');
                    }
                } else
                {
                    // Limpa o setor se não encontrou
                    const ddtSetorClear = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteAgendamento") : null;
                    if (ddtSetorClear)
                    {
                        ddtSetorClear.value = [];
                        ddtSetorClear.dataBind();
                    }

                    window.requisitanteOriginal.setorId = null;

                    console.warn('⚠️ Setor não encontrado ou vazio');
                }
            },
            error: function (xhr, status, error)
            {
                console.error('❌ Erro ao buscar setor:', error);
                console.error('Status:', status);
                console.error('Response:', xhr.responseText);

                // Limpa o setor em caso de erro
                const ddtSetorError = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteAgendamento") : null;
                if (ddtSetorError)
                {
                    ddtSetorError.value = [];
                    ddtSetorError.dataBind();
                }

                window.requisitanteOriginal.setorId = null;

                // Mostra mensagem ao usuário
                Alerta.Erro('Erro ao buscar setor do requisitante');
            }
        });

    } catch (error)
    {
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
window.onSelectRequisitanteEvento = function (args)
{
    console.log('🎯 Requisitante de EVENTO selecionado (SELECT event)!');
    console.log('📦 args:', args);

    try
    {
        // ===== OBTER ELEMENTO DO SETOR =====
        const ddtSetorElement = document.getElementById("lstSetorRequisitanteEvento");

        // ===== VALIDAÇÃO DOS DADOS =====
        if (!args || !args.itemData || !args.itemData.RequisitanteId)
        {
            console.warn('⚠️ Dados inválidos no evento select (Evento)');
            return;
        }

        const requisitanteId = args.itemData.RequisitanteId;
        console.log('✅ Requisitante ID (Evento):', requisitanteId);

        // ===== BUSCAR SETOR DO REQUISITANTE =====
        $.ajax({
            url: '/Viagens/Upsert?handler=PegaSetor',
            method: "GET",
            dataType: "json",
            data: { id: requisitanteId },
            success: function (res)
            {
                console.log('🏢 Resposta Setor (Evento):', res);

                // A resposta pode vir como { data: valor } OU { success: true, data: valor }
                const setorValue = res.data || res;

                if (setorValue !== null && setorValue !== undefined && setorValue !== '')
                {
                    // Verifica se o DropDownTree existe via bridge
                    const ddtSetorEventoObj = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteEvento") : null;
                    if (ddtSetorEventoObj)
                    {
                        // Define o valor do setor
                        ddtSetorEventoObj.value = [setorValue];
                        ddtSetorEventoObj.dataBind();

                        console.log('✓ Setor atualizado (Evento):', setorValue);
                    } else
                    {
                        console.error('❌ DropDownTree de setor (Evento) não encontrado via getSyncfusionInstance');
                    }
                } else
                {
                    // Limpa o setor se não encontrou
                    const ddtSetorEventoClear = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteEvento") : null;
                    if (ddtSetorEventoClear)
                    {
                        ddtSetorEventoClear.value = [];
                        ddtSetorEventoClear.dataBind();
                    }

                    console.warn('⚠️ Setor não encontrado ou vazio (Evento)');
                }
            },
            error: function (xhr, status, error)
            {
                console.error('❌ Erro ao buscar setor (Evento):', error);
                console.error('Status:', status);
                console.error('Response:', xhr.responseText);

                // Limpa o setor em caso de erro
                const ddtSetorEventoError = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteEvento") : null;
                if (ddtSetorEventoError)
                {
                    ddtSetorEventoError.value = [];
                    ddtSetorEventoError.dataBind();
                }

                // Mostra mensagem ao usuário
                Alerta.Erro('Erro ao buscar setor do requisitante');
            }
        });

    } catch (error)
    {
        console.error('❌ Erro na função onSelectRequisitanteEvento:', error);
        Alerta.Erro('Erro ao processar seleção do requisitante do evento');
    }
};

/**
 * Event handler para mudança da Finalidade
 * Exibe o card de Evento quando Finalidade = "Evento"
 */
window.lstFinalidade_Change = function (args)
{
    try
    {
        console.log("📋 Finalidade mudou:", args.value, args.itemData);

        const sectionEvento = document.getElementById("sectionEvento");
        const modalEvento = document.getElementById("modalEvento");

        if (!sectionEvento)
        {
            console.error("❌ sectionEvento não encontrado no DOM");
            return;
        }

        // Verificar se a finalidade é "Evento"
        const finalidadeSelecionada = args.itemData?.text || args.itemData?.Descricao || "";

        console.log("🔍 Finalidade selecionada:", finalidadeSelecionada);

        if (finalidadeSelecionada.toLowerCase().includes("evento"))
        {
            // ✅ MOSTRAR o card de evento (botão é controlado por evento.js)
            sectionEvento.style.display = "block";
            console.log("✅ Seção de Evento exibida");
        } else
        {
            // ❌ ESCONDER o card de evento e de cadastro (botão é controlado por evento.js)
            sectionEvento.style.display = "none";

            if (modalEvento && window.bootstrap && window.bootstrap.Modal)
            {
                window.bootstrap.Modal.getOrCreateInstance(modalEvento).hide();
            }

            // ❌ LIMPAR o lstEventos (mantém habilitado)
            const lstEventosWidget = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstEventos") : null;
            if (lstEventosWidget)
            {
                lstEventosWidget.value = null;
                lstEventosWidget.dataBind();
                console.log("✅ lstEventos limpo");
            }

            console.log("➖ Seção de Evento escondida");
        }

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("event-handlers.js", "lstFinalidade_Change", error);
    }
};
/**
 * Handler para mudança de requisitante (evento CHANGE - mantido para compatibilidade)
 * NOTA: Agora o preenchimento automático é feito pelo evento SELECT
 */
window.RequisitanteValueChange = function ()
{
    try
    {
        // lstRequisitante é um Kendo ComboBox, não Syncfusion EJ2
        const comboBox = $("#lstRequisitante").data("kendoComboBox");

        if (!comboBox)
        {
            console.warn('⚠️ lstRequisitante (Kendo ComboBox) não encontrado');
            return;
        }

        if (comboBox.value() === null || comboBox.value() === '')
        {
            return;
        }

        const requisitanteid = String(comboBox.value());

        // NOTA: O código de buscar ramal e setor foi movido para onSelectRequisitante
        // Mantendo esta função para compatibilidade com outros códigos que possam usar
        console.log('ℹ️ RequisitanteValueChange chamado (requisitante ID:', requisitanteid, ')');

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("event-handlers.js", "RequisitanteValueChange", error);
    }
};

/**
 * Handler para mudança de motorista
 */
window.MotoristaValueChange = function ()
{
    try
    {
        const ddTreeObj = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstMotorista") : null;

        if (!ddTreeObj)
        {
            console.warn('⚠️ lstMotorista não encontrado via getSyncfusionInstance');
            return;
        }

        console.log("Objeto Motorista:", ddTreeObj);

        if (ddTreeObj.value === null || ddTreeObj.enabled === false)
        {
            return;
        }

        const motoristaid = String(ddTreeObj.value);
        return motoristaid;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("event-handlers.js", "MotoristaValueChange", error);
    }
};

/**
 * Handler para mudança de veículo
 */
window.VeiculoValueChange = function ()
{
    try
    {
        const ddTreeObj = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstVeiculo") : null;

        if (!ddTreeObj)
        {
            console.warn('⚠️ lstVeiculo não encontrado via getSyncfusionInstance');
            return;
        }

        console.log("Objeto Veículo:", ddTreeObj);

        if (ddTreeObj.value === null || ddTreeObj.enabled === false)
        {
            return;
        }

        const veiculoid = String(ddTreeObj.value);

        // Pega Km Atual do Veículo
        $.ajax({
            url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
            method: "GET",
            datatype: "json",
            data: { id: veiculoid },
            success: function (res)
            {
                const km = res.data;
                const kmAtual = document.getElementById("txtKmAtual");
                kmAtual.value = km;
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                Alerta.TratamentoErroComLinha("event-handlers.js", "VeiculoValueChange", erro);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("event-handlers.js", "VeiculoValueChange", error);
    }
};

/**
 * Handler para mudança de requisitante no evento
 */
window.RequisitanteEventoValueChange = function ()
{
    try
    {
        const ddTreeObj = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstRequisitanteEvento") : null;

        if (!ddTreeObj)
        {
            console.warn('⚠️ lstRequisitanteEvento não encontrado via getSyncfusionInstance');
            return;
        }

        if (ddTreeObj.value === null || ddTreeObj.value === '')
        {
            return;
        }

        const requisitanteid = String(ddTreeObj.value);

        // Pega Setor Padrío do Requisitante
        $.ajax({
            url: "/Viagens/Upsert?handler=PegaSetor",
            method: "GET",
            datatype: "json",
            data: { id: requisitanteid },
            success: function (res)
            {
                const ddtSetorEvt = window.getSyncfusionInstance ? window.getSyncfusionInstance("ddtSetorEvento") : null;
                if (ddtSetorEvt)
                {
                    ddtSetorEvt.value = [res.data];
                    ddtSetorEvt.dataBind();
                }
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                Alerta.TratamentoErroComLinha("event-handlers.js", "RequisitanteEventoValueChange", erro);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("event-handlers.js", "RequisitanteEventoValueChange", error);
    }
};

/**
 * Handler para mudança de data no calendário
 */
window.onDateChange = function (args)
{
    try
    {
        const selectedDates = args.model.values;

        // Get the ListBox element
        const listbox = document.getElementById('selectedDates');
        listbox.innerHTML = '';

        // Add each selected date to the ListBox
        selectedDates.forEach(function (date)
        {
            const li = document.createElement('li');
            li.textContent = new Date(date).toLocaleDateString();
            listbox.appendChild(li);
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("event-handlers.js", "onDateChange", error);
    }
};

/**
 * ====================================
 * HANDLER DE SELEÇÃO DE EVENTO
 * ====================================
 * Controla a exibição e preenchimento dos campos quando um evento é selecionado
 */

// Inicializar o evento de seleção do ComboBox de Eventos
function inicializarEventoSelect()
{
    try
    {
        // Obter a instância do ComboBox de Eventos
        const lstEventosElement = document.getElementById('lstEventos');

        if (!lstEventosElement)
        {
            console.warn("⚠️ ComboBox lstEventos não encontrado");
            return;
        }

        const lstEventos = window.getSyncfusionInstance ? window.getSyncfusionInstance('lstEventos') : null;

        if (!lstEventos)
        {
            console.warn("⚠️ Instância do ComboBox lstEventos não encontrada via getSyncfusionInstance");
            return;
        }

        // Adicionar evento de seleção
        lstEventos.select = function (args)
        {
            if (args.itemData)
            {
                // Evento selecionado - mostrar os campos e preencher
                exibirDadosEvento(args.itemData);
            }
        };

        // Também adicionar evento de clear para limpar quando o botão X for clicado
        lstEventos.clearing = function (args)
        {
            // Nenhum evento selecionado - esconder os campos
            ocultarDadosEvento();
        };

        console.log("✅ Handler de seleção de evento inicializado");

    } catch (error)
    {
        console.error("❌ Erro ao inicializar handler de evento:", error);
    }
}

/**
 * Exibe e preenche os campos com os dados do evento selecionado
 * param {Object} eventoData - Dados do evento selecionado
 */
function exibirDadosEvento(eventoData)
{
    try
    {
        console.log("📋 Exibindo dados do evento:", eventoData);
        console.log("🔍 Estrutura completa do objeto:", JSON.stringify(eventoData, null, 2));

        // Mostrar a div dos dados do evento
        const divDados = document.getElementById('divDadosEventoSelecionado');
        if (divDados)
        {
            divDados.style.display = 'flex';
        }

        // Buscar os dados completos do evento pelo ID
        const eventoId = eventoData.EventoId || eventoData.eventoId;
        console.log("🆔 EventoId:", eventoId);

        if (eventoId)
        {
            // Fazer requisição AJAX para buscar os dados completos do evento
            $.ajax({
                url: '/api/ViagemEvento/ObterPorId',
                method: 'GET',
                data: { id: eventoId },
                success: function (response)
                {
                    console.log("✅ Dados do evento recebidos da API:", response);

                    if (response.success && response.data)
                    {
                        preencherCamposEvento(response.data);
                    } else
                    {
                        console.warn("⚠️ Resposta da API sem dados, usando itemData...");
                        preencherCamposEvento(eventoData);
                    }
                },
                error: function (xhr, status, error)
                {
                    console.error("❌ Erro ao buscar dados do evento:", error);
                    console.log("⚠️ Tentando usar dados do itemData...");
                    preencherCamposEvento(eventoData);
                }
            });
        } else
        {
            console.log("⚠️ EventoId não encontrado, usando dados do itemData...");
            preencherCamposEvento(eventoData);
        }

    } catch (error)
    {
        console.error("❌ Erro ao exibir dados do evento:", error);
    }
}

/**
 * Preenche os campos com os dados do evento
 * param {Object} dados - Dados do evento
 */
function preencherCamposEvento(dados)
{
    try
    {
        console.log("📝 Preenchendo campos com:", dados);

        // Preencher Data Início (Kendo DatePicker)
        const dataInicial = dados.DataInicial || dados.dataInicial || dados.DataInicialEvento;
        if (dataInicial)
        {
            const dtInicio = $("#txtDataInicioEvento").data("kendoDatePicker");
            if (dtInicio)
            {
                dtInicio.value(new Date(dataInicial));
                console.log("✅ Data Início preenchida:", dataInicial);
            }
        } else
        {
            console.warn("⚠️ Data Inicial não encontrada no objeto");
        }

        // Preencher Data Fim (Kendo DatePicker)
        const dataFinal = dados.DataFinal || dados.dataFinal || dados.DataFinalEvento;
        if (dataFinal)
        {
            const dtFim = $("#txtDataFimEvento").data("kendoDatePicker");
            if (dtFim)
            {
                dtFim.value(new Date(dataFinal));
                console.log("✅ Data Fim preenchida:", dataFinal);
            }
        } else
        {
            console.warn("⚠️ Data Final não encontrada no objeto");
        }

        // Preencher Quantidade de Participantes (Kendo NumericTextBox)
        const qtdParticipantes = dados.QtdParticipantes || dados.qtdParticipantes;
        console.log("🔍 Tentando preencher QtdParticipantes com valor:", qtdParticipantes);

        if (qtdParticipantes !== undefined && qtdParticipantes !== null)
        {
            const numParticipantes = $("#txtQtdParticipantesEvento").data("kendoNumericTextBox");
            if (numParticipantes)
            {
                numParticipantes.value(qtdParticipantes);
                console.log("✅ Qtd Participantes preenchida:", qtdParticipantes);
            } else
            {
                console.error("❌ Componente kendoNumericTextBox não encontrado!");
            }
        } else
        {
            console.warn("⚠️ QtdParticipantes não encontrado no objeto. Valor recebido:", qtdParticipantes);
            console.log("📋 Objeto completo recebido:", dados);
        }

        console.log("✅ Dados do evento preenchidos com sucesso");

    } catch (error)
    {
        console.error("❌ Erro ao preencher campos do evento:", error);
    }
}

/**
 * Oculta e limpa os campos do evento
 */
function ocultarDadosEvento()
{
    try
    {
        console.log("🙈 Ocultando dados do evento");

        // Esconder a div dos dados do evento
        const divDados = document.getElementById('divDadosEventoSelecionado');
        if (divDados)
        {
            divDados.style.display = 'none';
        }

        // Limpar Data Início (Kendo DatePicker)
        const dtInicio = $("#txtDataInicioEvento").data("kendoDatePicker");
        if (dtInicio)
        {
            dtInicio.value(null);
        }

        // Limpar Data Fim (Kendo DatePicker)
        const dtFim = $("#txtDataFimEvento").data("kendoDatePicker");
        if (dtFim)
        {
            dtFim.value(null);
        }

        // Limpar Quantidade de Participantes (Kendo NumericTextBox)
        const numParticipantes = $("#txtQtdParticipantesEvento").data("kendoNumericTextBox");
        if (numParticipantes)
        {
            numParticipantes.value(null);
        }

        console.log("✅ Dados do evento limpos");

    } catch (error)
    {
        console.error("❌ Erro ao ocultar dados do evento:", error);
    }
}

/**
 * Handler de criação do ComboBox de Motorista
 * Configura os templates para exibir fotos dos motoristas
 */
window.onLstMotoristaCreated = function ()
{
    try
    {
        console.log('🎯 onLstMotoristaCreated chamado');

        const comboInstance = window.getSyncfusionInstance ? window.getSyncfusionInstance('lstMotorista') : null;

        if (!comboInstance)
        {
            console.warn('❌ lstMotorista não encontrado via getSyncfusionInstance');
            return;
        }

        // Template para itens da lista
        comboInstance.itemTemplate = function (data)
        {
            let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
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
        comboInstance.valueTemplate = function (data)
        {
            if (!data) return '';

            let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
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

        console.log("✅ Templates de motorista configurados com sucesso");

    } catch (error)
    {
        console.error('❌ Erro:', error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("event-handlers.js", "onLstMotoristaCreated", error);
        }
    }
};

// ====================================================================
// EXPORTAÇÕES GLOBAIS
// ====================================================================
window.exibirDadosEvento = exibirDadosEvento;
window.ocultarDadosEvento = ocultarDadosEvento;
