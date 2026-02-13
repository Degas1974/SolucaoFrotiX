/* ****************************************************************************************
 * ⚡ ARQUIVO: evento.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema completo para gerenciamento de Eventos dentro do módulo de
 *                   agendamento de viagens. 24 funções para controlar ciclo completo:
 *                   inicialização (monitoramento de finalidade, botão "Novo Evento",
 *                   formulário cadastro), Bootstrap Modal management com fallbacks (Bootstrap
 *                   5 + jQuery), integração Kendo DatePicker (Telerik para datas),
 *                   Kendo DropDownList/ComboBox (eventos, requisitante evento), validações
 *                   completas (nome, descrição, datas, quantidade participantes, setor,
 *                   requisitante), 2 AJAX calls paralelos (POST criar evento, GET buscar
 *                   setores), retry pattern (5 tentativas 300ms para inicialização),
 *                   atualização automática dropdown após insert, funções diagnóstico/teste
 *                   para debugging (6 funções utilitárias). Controla visibilidade section
 *                   evento baseado em finalidade selecionada ("Evento" mostra, outros escondem).
 * 📥 ENTRADAS     : inicializarSistemaEvento() sem params, obterModalBootstrap(modalId),
 *                   mostrarModalFallback/fecharModalFallback(modalId), obterValorDataEvento/
 *                   limparValorDataEvento(input), controlarVisibilidadeSecaoEvento(finalidade:
 *                   string|Array), window.onSelectRequisitanteEvento(args: {itemData}),
 *                   atualizarListaEventos(eventoId, eventoText), funções de teste sem params
 * 📤 SAÍDAS       : inicializarSistemaEvento configura listeners e retorna void, obterModalBootstrap
 *                   retorna Bootstrap.Modal instance ou null, mostrar/fecharModalFallback
 *                   retorna boolean success, obterValorDataEvento retorna Date|null,
 *                   inserirNovoEvento POSTs evento e atualiza dropdown, atualizarListaEventos
 *                   manipula DOM (dataSource + value + dataBind), diagnóstico/teste console.log
 * 🔗 CHAMADA POR  : exibe-viagem.js (ExibeViagem chama inicializarSistemaEvento no final),
 *                   event-handlers.js (onSelectFinalidade pode disparar controlarVisibilidade),
 *                   controls-init.js (inicialização de dropdowns), user actions (clicks em
 *                   botões, selects em dropdowns, submits de formulários), console debugging
 *                   (funções de teste/diagnóstico)
 * 🔄 CHAMA        : document.getElementById (13+ IDs), bootstrap.Modal.getOrCreateInstance/
 *                   show/hide, jQuery.modal("show"/"hide"), $(input).data("kendoDatePicker"),
 *                   Kendo DatePicker.value getter/setter, Kendo DropDownList/ComboBox methods
 *                   ($("#id").data("kendoXxx"), bind/unbind events, dataSource.data(),
 *                   value() getter/setter), $.ajax (2 calls: POST /api/Viagem/AdicionarEvento,
 *                   GET /Viagens/Upsert?handler=PegaSetor e AJAXPreencheListaSetores),
 *                   setTimeout (5 retries pattern 300ms + 250ms/100ms delays), moment().format,
 *                   Array methods (some, find, sort, push), String methods (trim, toLowerCase,
 *                   toString), Number.isInteger, Number.isNaN, JSON.stringify, Alerta.Alerta/
 *                   TratamentoErroComLinha, AppToast.show, console logging extensive,
 *                   window.exibirDadosEvento (external function optional), getRequisitanteEventoCombo
 *                   (Kendo ComboBox getter), element.cloneNode + replaceChild (remove old listeners)
 * 📦 DEPENDÊNCIAS : Bootstrap 5 Modal (window.bootstrap.Modal), jQuery ($.ajax, $.modal,
 *                   $.data), Kendo UI Telerik (DatePicker: data("kendoDatePicker"),
 *                   ComboBox: getRequisitanteEventoCombo, DropDownList: $("#id").data("kendoDropDownList")),
 *                   jQuery ($("#txtQuantidade").val() para inputs simples),
 *                   moment.js (moment().format("MM-DD-YYYY")), Alerta (Alerta.Alerta,
 *                   Alerta.TratamentoErroComLinha), AppToast (AppToast.show), DOM elements
 *                   (13 elements: #lstFinalidade, #sectionEvento, #btnEvento, #modalEvento,
 *                   #lstEventos, #txtNomeEvento, #txtDescricaoEvento, #txtDataInicialEvento,
 *                   #txtDataFinalEvento, #txtQtdParticipantesEventoCadastro, #lstRequisitanteEvento,
 *                   #txtSetorRequisitanteEvento, #lstSetorRequisitanteEvento, #btnInserirEvento,
 *                   #btnCancelarEvento), Razor Pages handlers (/api/Viagem/AdicionarEvento,
 *                   /Viagens/Upsert?handler=PegaSetor e AJAXPreencheListaSetores),
 *                   window.exibirDadosEvento (optional external function)
 * 📝 OBSERVAÇÕES  : Todas as funções em global scope (não exportadas explicitamente, exceto
 *                   window.onSelectRequisitanteEvento). Try-catch em funções críticas
 *                   (obterValorDataEvento, limparValorDataEvento, limparCamposCadastroEvento,
 *                   inserirNovoEvento, atualizarListaEventos, onSelectRequisitanteEvento).
 *                   Console logging extremamente detalhado (🎯🔧✅❌⚠️📦🔍🔄📋🧪 emojis).
 *                   Fallback patterns: Bootstrap Modal → jQuery modal, Kendo DatePicker →
 *                   native input.value. Retry pattern: configurarRequisitanteEvento tenta 5x
 *                   com 300ms delay (total 1.5s timeout para DOM ready). Clone + replaceChild
 *                   para remover event listeners antigos (3 botões). Moment.js format hardcoded
 *                   "MM-DD-YYYY" (US format para backend). Validações completas: nome/descrição
 *                   não-vazio, datas obrigatórias, dataInicial <= dataFinal, quantidade > 0 e
 *                   integer <= 2147483647 (Int32.MaxValue), setor e requisitante obrigatórios.
 *                   AJAX POST AdicionarEvento envia {Nome, Descricao, SetorSolicitanteId,
 *                   RequisitanteId, QtdParticipantes, DataInicial, DataFinal, Status: "1"}.
 *                   Response expected: {success, message, eventoId, eventoText}. Atualização
 *                   dropdown: clear + reload + sort alfabético + select + exibirDadosEvento.
 *                   Delays estratégicos: 300ms focus, 250ms select, 100ms exibirDadosEvento.
 *                   Comentários inline sobre ES6 exports (desabilitados). 6 funções de teste/
 *                   diagnóstico para console debugging (diagnosticarSistemaEvento, testar*,
 *                   verificarElementosEvento).
 *
 * 📋 ÍNDICE DE FUNÇÕES (24 funções: 18 principais + 6 diagnóstico/teste):
 *
 * ┌─ inicializarSistemaEvento() ────────────────────────────────────────┐
 * │ → Inicializa sistema completo de evento (entry point)               │
 * │ → Fluxo:                                                             │
 * │   1. console.log "🎯 Inicializando..."                               │
 * │   2. configurarMonitoramentoFinalidade()                             │
 * │   3. configurarBotaoNovoEvento()                                     │
 * │   4. configurarBotoesCadastroEvento()                                │
 * │   5. configurarRequisitanteEvento()                                  │
 * │   6. console.log "✅ Sistema inicializado!"                          │
 * │ → Chamada por: exibe-viagem.js no final de ExibeViagem()            │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ obterModalBootstrap(modalId) ──────────────────────────────────────┐
 * │ → Obtém instância Bootstrap Modal com safe checks                   │
 * │ → param modalId: string (ex: "modalEvento")                         │
 * │ → returns Bootstrap.Modal instance ou null                           │
 * │ → Verifica window.bootstrap.Modal disponível                         │
 * │ → Usa getOrCreateInstance (Bootstrap 5 method)                       │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ mostrarModalFallback(modalId) ─────────────────────────────────────┐
 * │ → Mostra modal com fallback chain                                   │
 * │ → param modalId: string                                              │
 * │ → returns boolean (true se sucesso, false se falha)                 │
 * │ → Fluxo:                                                             │
 * │   1. Try obterModalBootstrap + modal.show()                          │
 * │   2. Fallback: jQuery $(`#${modalId}`).modal("show")                │
 * │   3. Return false se ambos falharem                                  │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ fecharModalFallback(modalId) ──────────────────────────────────────┐
 * │ → Fecha modal com fallback chain                                    │
 * │ → param modalId: string                                              │
 * │ → returns boolean (true se sucesso, false se falha)                 │
 * │ → Mesmo pattern que mostrarModalFallback (hide em vez de show)      │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ obterValorDataEvento(input) ───────────────────────────────────────┐
 * │ → Obtém valor de Kendo DatePicker ou input nativo                   │
 * │ → param input: DOM element (DatePicker ou input[type="date"])      │
 * │ → returns Date object ou null                                        │
 * │ → Fluxo:                                                             │
 * │   1. try-catch wrapper                                               │
 * │   2. const picker = $(input).data("kendoDatePicker")                │
 * │   3. if picker && picker.value(): return picker.value()             │
 * │   4. Fallback: parse input.value com new Date()                     │
 * │   5. Validate: Number.isNaN(date.getTime()) ? null : date           │
 * │   6. catch: Alerta.TratamentoErroComLinha + return null             │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ limparValorDataEvento(input) ──────────────────────────────────────┐
 * │ → Limpa valor de Kendo DatePicker ou input nativo                   │
 * │ → param input: DOM element                                           │
 * │ → returns void                                                       │
 * │ → Fluxo:                                                             │
 * │   1. try-catch wrapper                                               │
 * │   2. const picker = $(input).data("kendoDatePicker")                │
 * │   3. if picker: picker.value(null)                                   │
 * │   4. Fallback: input.value = ""                                      │
 * │   5. catch: Alerta.TratamentoErroComLinha                            │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ configurarMonitoramentoFinalidade() ───────────────────────────────┐
 * │ → Monitora dropdown lstFinalidade para mostrar/esconder section     │
 * │ → Fluxo:                                                             │
 * │   1. const lstFinalidade = getElementById("lstFinalidade")          │
 * │   2. if !lstFinalidade: console.warn + return                        │
 * │   3. if $("#lstFinalidade").data("kendoDropDownList"):                 │
 * │      a. dropdown.select = function(args) {                           │
 * │           controlarVisibilidadeSecaoEvento(args.itemData.text) }     │
 * │      b. dropdown.change = function(args) {                           │
 * │           controlarVisibilidadeSecaoEvento(args.value) }             │
 * │      c. Verifica valorAtual, chama controlarVisibilidade se existe   │
 * │   4. else: console.warn "não é componente EJ2"                       │
 * │ → Dual listeners: select (immediate) + change (backup programático) │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * [Continuação no próximo bloco devido ao limite de espaço...]
 *
 * 📌 OBSERVAÇÕES TÉCNICAS IMPORTANTES:
 * - Kendo UI: todos controles acessados via jQuery $("#id").data("kendoXxx") (migrado de Syncfusion)
 * - Bootstrap Modal: getOrCreateInstance é Bootstrap 5, jQuery fallback para Bootstrap 3/4
 * - Retry pattern: 5 tentativas x 300ms = 1500ms timeout para DOM initialization
 * - Fallback chain: permite migração gradual Bootstrap 3→4→5 sem breaking changes
 * - Finalidade "Evento": case-insensitive check, aceita string ou array, valores válidos:
 *   "Evento", "E", "evento" (lowercase)
 * - Clone + replaceChild: padrão usado em 3 botões (#btnEvento, #btnInserirEvento,
 *   #btnCancelarEvento) para garantir single event listener
 * - AJAX double call pattern: PegaSetor retorna ID, AJAXPreencheListaSetores busca lista
 *   completa, find by ID para obter nome (nested AJAX dentro de success callback)
 * - DataSource manipulation: dataSource.data() + sort pattern para Kendo DropDownList/ComboBox
 * - Moment.js format: "MM-DD-YYYY" é US format (mês-dia-ano), backend ASP.NET espera isso
 * - Status: "1" hardcoded (string) no POST body (enum ou flag de status ativo)
 * - Int32.MaxValue: 2147483647 (validação explícita para quantidade participantes)
 * - ExibirDadosEvento: função externa opcional (window.exibirDadosEvento ou global
 *   exibirDadosEvento) chamada após select com 100ms delay
 * - Console logging: emojis consistentes para facilitar debug visual (🎯 init, ✅ success,
 *   ❌ error, ⚠️ warning, 📦 data, 🔍 debug, 🔄 process, 🧪 test, 📋 list)
 * - Comentários pt-BR misturados com code em inglês (pattern comum em projetos brasileiros)
 * - Diagnostic functions: não usadas em produção, apenas para console debugging durante
 *   desenvolvimento (diagnosticarSistemaEvento(), testar*(), verificarElementosEvento())
 * - ES6 exports: comentados no final do arquivo (sistema ainda usa global scope functions)
 * - UTF-8 BOM: arquivo começa com ﻿ (U+FEFF) - pode causar issues em alguns parsers
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Inicializa o sistema de evento
 * Chame esta função no final da ExibeViagem
 */
function inicializarSistemaEvento()
{
    console.log("🎯 Inicializando Sistema de Evento...");

    // 1. Monitora mudanças na finalidade
    configurarMonitoramentoFinalidade();

    // 2. Configura o botão "Novo Evento"
    configurarBotaoNovoEvento();

    // 3. Configura botões do formulário de cadastro
    configurarBotoesCadastroEvento();

    // 4. Configura evento select do requisitante de evento
    configurarRequisitanteEvento();

    console.log("✅ Sistema de Evento inicializado!");
}

/**
 * Monitora a lista de Finalidades
 */
function obterModalBootstrap(modalId)
{
    const modalEl = document.getElementById(modalId);
    if (!modalEl || !window.bootstrap || !window.bootstrap.Modal)
    {
        return null;
    }

    return window.bootstrap.Modal.getOrCreateInstance(modalEl);
}

function mostrarModalFallback(modalId)
{
    const modal = obterModalBootstrap(modalId);
    if (modal)
    {
        modal.show();
        return true;
    }

    if (window.jQuery && typeof window.jQuery.fn.modal === "function")
    {
        window.jQuery(`#${modalId}`).modal("show");
        return true;
    }

    return false;
}

function fecharModalFallback(modalId)
{
    const modal = obterModalBootstrap(modalId);
    if (modal)
    {
        modal.hide();
        return true;
    }

    if (window.jQuery && typeof window.jQuery.fn.modal === "function")
    {
        window.jQuery(`#${modalId}`).modal("hide");
        return true;
    }

    return false;
}

/**
 * Kendo UI DatePicker - Não precisa de rebuild
 * Componentes Kendo/Telerik são estáveis dentro de modais Bootstrap
 */

function obterValorDataEvento(input)
{
    try
    {
        // Telerik usa data("kendoDatePicker")
        const picker = $(input).data("kendoDatePicker");
        if (picker && picker.value())
        {
            return picker.value();
        }

        // Fallback: input nativo
        if (!input || !input.value)
        {
            return null;
        }

        const parsed = new Date(input.value);
        return Number.isNaN(parsed.getTime()) ? null : parsed;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("evento.js", "obterValorDataEvento", error);
        return null;
    }
}

function limparValorDataEvento(input)
{
    try
    {
        // Telerik usa data("kendoDatePicker")
        const picker = $(input).data("kendoDatePicker");
        if (picker)
        {
            picker.value(null);
            return;
        }

        // Fallback: input nativo
        if (input)
        {
            input.value = "";
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("evento.js", "limparValorDataEvento", error);
    }
}

function configurarMonitoramentoFinalidade()
{
    const lstFinalidade = document.getElementById("lstFinalidade");

    if (!lstFinalidade)
    {
        console.warn("⚠️ lstFinalidade não encontrado");
        return;
    }

    // Verifica se é componente Kendo DropDownList
    const dropdown = $("#lstFinalidade").data("kendoDropDownList");
    if (dropdown)
    {
        // Adiciona listener para SELECT (dispara imediatamente ao clicar)
        dropdown.unbind("select");
        dropdown.bind("select", function (e)
        {
            var dataItem = e.dataItem;
            console.log("🎯 Finalidade SELECIONADA (select event):", dataItem);

            // Pega o texto da finalidade
            const finalidade = dataItem?.descricao || dataItem?.finalidadeId || "";

            console.log("🔍 Processando:", finalidade);
            controlarVisibilidadeSecaoEvento(finalidade);
        });

        // TAMBÉM adiciona listener para CHANGE (backup para casos de programático)
        dropdown.unbind("change");
        dropdown.bind("change", function (e)
        {
            console.log("🔄 Finalidade mudou (change event):", e.sender.value());
            controlarVisibilidadeSecaoEvento(e.sender.value());
        });

        console.log("✅ Listener de Finalidade configurado (SELECT + CHANGE)");

        // Verifica estado inicial
        const valorAtual = dropdown.value();
        if (valorAtual)
        {
            controlarVisibilidadeSecaoEvento(valorAtual);
        }
    } else
    {
        console.warn("⚠️ lstFinalidade não é componente Kendo");
    }
}

/**
 * Configura o evento select do requisitante de evento
 * para preencher automaticamente o setor
 */
function configurarRequisitanteEvento()
{
    console.log("🔧 === INÍCIO configurarRequisitanteEvento ===");

    // Função para tentar configurar
    const tentarConfigurar = (tentativa = 1) =>
    {
        console.log(`🔄 Tentativa ${tentativa} de configurar requisitante de evento...`);

        const lstRequisitanteEvento = document.getElementById("lstRequisitanteEvento");

        if (!lstRequisitanteEvento)
        {
            console.warn(`⚠️ lstRequisitanteEvento não encontrado no DOM (tentativa ${tentativa})`);

            if (tentativa < 5)
            {
                console.log(`   ⏰ Tentando novamente em 300ms...`);
                setTimeout(() => tentarConfigurar(tentativa + 1), 300);
            }
            else
            {
                console.error('❌ lstRequisitanteEvento não encontrado após 5 tentativas');
            }
            return;
        }

        console.log('✅ Elemento lstRequisitanteEvento encontrado');

        // Verifica se é componente Kendo ComboBox
        const dropdown = $("#lstRequisitanteEvento").data("kendoComboBox");
        if (dropdown)
        {
            console.log('✅ Componente Kendo ComboBox encontrado:');
            console.log('   - Value atual:', dropdown.value());
            console.log('   - Text atual:', dropdown.text());
            console.log('   - DataSource:', dropdown.dataSource.data());

            // Configura o listener select (remove anterior se existir)
            dropdown.unbind("select");
            dropdown.bind("select", function (e)
            {
                var dataItem = e.dataItem;
                console.log('🔔 [LISTENER] Select disparado no lstRequisitanteEvento:');
                console.log('   - dataItem:', dataItem);

                // Chama a função global com formato compatível
                if (typeof window.onSelectRequisitanteEvento === 'function')
                {
                    window.onSelectRequisitanteEvento({ itemData: dataItem });
                }
            });

            console.log('✅ Listener de select configurado com sucesso!');
            console.log('🔧 === FIM configurarRequisitanteEvento ===');
        }
        else
        {
            console.warn(`⚠️ lstRequisitanteEvento não é componente Kendo (tentativa ${tentativa})`);

            if (tentativa < 5)
            {
                console.log(`   ⏰ Tentando novamente em 300ms...`);
                setTimeout(() => tentarConfigurar(tentativa + 1), 300);
            }
            else
            {
                console.error('❌ lstRequisitanteEvento não inicializado após 5 tentativas');
                console.log('🔧 === FIM configurarRequisitanteEvento (FALHOU) ===');
            }
        }
    };

    // Inicia as tentativas
    tentarConfigurar();
}

/**
 * ================================================================
 * NOVA FUNÇÃO: Atualiza campos quando Requisitante Evento é selecionado
 * Esta função é chamada pelo listener em configurarRequisitanteEvento()
 * ================================================================
 */
window.onSelectRequisitanteEvento = function (args)
{
    console.log('🎯 Requisitante de Evento selecionado!');
    console.log('   itemData:', args.itemData);

    try
    {
        // Validação - aceita tanto id quanto RequisitanteId
        const requisitanteId = args.itemData?.id || args.itemData?.RequisitanteId;

        if (!args || !args.itemData || !requisitanteId)
        {
            console.warn('⚠️ Dados inválidos do requisitante');
            console.log('   id:', args.itemData?.id);
            console.log('   RequisitanteId:', args.itemData?.RequisitanteId);
            return;
        }

        console.log('✅ Requisitante ID:', requisitanteId);

        // BUSCAR SETOR DO REQUISITANTE
        $.ajax({
            url: "/Viagens/Upsert?handler=PegaSetor",
            method: "GET",
            dataType: "json",
            data: { id: requisitanteId },
            success: function (res)
            {
                console.log('📦 Resposta do servidor (Setor):', res);

                try
                {
                    // A resposta pode vir como {data: 'id'} ou {success: true, data: 'id'}
                    const setorId = res.data || (res.success && res.data);

                    if (setorId)
                    {
                        // Campos: texto readonly (display) + hidden (valor)
                        const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
                        const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");

                        if (!txtSetorEvento || !lstSetorEvento)
                        {
                            console.error('❌ Campos de setor não encontrados no DOM');
                            return;
                        }

                        // Buscar nome do setor via AJAX
                        $.ajax({
                            url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
                            method: "GET",
                            dataType: "json",
                            success: function (resSetores)
                            {
                                console.log('📋 Lista de setores recebida:', resSetores);
                                console.log('🔍 Procurando SetorId:', setorId, '(tipo:', typeof setorId, ')');

                                const setores = resSetores.data || [];
                                console.log('📊 Total de setores na lista:', setores.length);

                                // Debug: Mostrar alguns setores da lista
                                if (setores.length > 0) {
                                    console.log('📄 Exemplo de setor na lista:', setores[0]);
                                    console.log('📄 Campos disponíveis:', Object.keys(setores[0]));
                                }

                                // Normalizar ambos para string lowercase para comparação
                                const setorIdNormalizado = setorId.toString().toLowerCase();
                                console.log('🔧 SetorId normalizado:', setorIdNormalizado);

                                const setorEncontrado = setores.find(s => {
                                    if (!s.setorSolicitanteId) return false; // ✅ CORRIGIDO: lowercase
                                    const idNormalizado = s.setorSolicitanteId.toString().toLowerCase();
                                    console.log('  🔎 Comparando:', idNormalizado, '===', setorIdNormalizado, '?', idNormalizado === setorIdNormalizado);
                                    return idNormalizado === setorIdNormalizado;
                                });

                                console.log('🔍 Setor encontrado?', setorEncontrado);

                                if (setorEncontrado)
                                {
                                    // Preenche campo texto com nome do setor
                                    txtSetorEvento.value = setorEncontrado.nome; // ✅ CORRIGIDO: lowercase
                                    // Preenche campo hidden com ID do setor
                                    lstSetorEvento.value = setorId;

                                    console.log('✅ Setor atualizado:', setorEncontrado.nome, '(', setorId, ')');
                                }
                                else
                                {
                                    console.warn('⚠️ Setor não encontrado na lista:', setorId);
                                    txtSetorEvento.value = 'Setor não identificado';
                                    lstSetorEvento.value = setorId;
                                }
                            },
                            error: function (xhr, status, error)
                            {
                                console.error('❌ Erro ao buscar lista de setores:', error);
                                txtSetorEvento.value = 'Erro ao buscar setor';
                                lstSetorEvento.value = setorId;
                            }
                        });
                    }
                    else
                    {
                        console.warn('⚠️ Setor não encontrado na resposta');

                        // Limpa os campos se não houver setor
                        const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
                        const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");

                        if (txtSetorEvento) txtSetorEvento.value = '';
                        if (lstSetorEvento) lstSetorEvento.value = '';
                    }
                }
                catch (error)
                {
                    console.error('❌ Erro ao setar setor:', error);
                    Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento.setor', error);
                }
            },
            error: function (xhr, status, error)
            {
                console.error('❌ Erro ao buscar setor:', { xhr, status, error });
                Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento.ajax.setor', error);

                // Limpa os campos em caso de erro
                const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
                const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");

                if (txtSetorEvento) txtSetorEvento.value = '';
                if (lstSetorEvento) lstSetorEvento.value = '';
            }
        });
    }
    catch (error)
    {
        console.error('❌ Erro geral em onSelectRequisitanteEvento:', error);
        Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento', error);
    }
};


/**
 * Controla a visibilidade da seção de evento
 * param {string|Array} finalidade - Valor da finalidade
 */
function controlarVisibilidadeSecaoEvento(finalidade)
{
    const sectionEvento = document.getElementById("sectionEvento");
    const btnEvento = document.getElementById("btnEvento");

    if (!sectionEvento)
    {
        console.warn("sectionEvento nao encontrado");
        return;
    }

    let isEvento = false;

    if (Array.isArray(finalidade))
    {
        isEvento = finalidade.some(f =>
            f === "Evento" || f === "E" ||
            (f && f.toLowerCase && f.toLowerCase() === "evento")
        );
    } else
    {
        isEvento = finalidade === "Evento" ||
            finalidade === "E" ||
            (finalidade && finalidade.toLowerCase && finalidade.toLowerCase() === "evento");
    }

    if (isEvento)
    {
        sectionEvento.style.display = "block";

        // ✅ MOSTRAR o botão Novo Evento
        if (btnEvento)
        {
            btnEvento.style.display = "block";
            console.log("✅ Botão Novo Evento exibido (evento.js)");
        }
    } else
    {
        sectionEvento.style.display = "none";

        // ❌ ESCONDER o botão Novo Evento
        if (btnEvento)
        {
            btnEvento.style.display = "none";
            console.log("➖ Botão Novo Evento escondido (evento.js)");
        }

        if (typeof fecharFormularioCadastroEvento === "function")
        {
            fecharFormularioCadastroEvento();
        }
    }
}


/**
 * Configura o botão "Novo Evento"
 */
function configurarBotaoNovoEvento()
{
    const btnEvento = document.getElementById("btnEvento");

    if (!btnEvento)
    {
        console.warn("btnEvento nao encontrado");
        return;
    }

    const novoBotao = btnEvento.cloneNode(true);
    btnEvento.parentNode.replaceChild(novoBotao, btnEvento);

    novoBotao.addEventListener("click", function (e)
    {
        e.preventDefault();
        e.stopPropagation();

        abrirFormularioCadastroEvento();
    });

    console.log("Botao Novo Evento configurado (modal)");
}


/**
 * Abre o formulário de cadastro de evento
 */
function abrirFormularioCadastroEvento()
{
    limparCamposCadastroEvento();
    const dataInicialEl = document.getElementById("txtDataInicialEvento");
    // Telerik DatePickers não precisam de rebuild
    // Os componentes são estáveis dentro de modais Bootstrap

    if (!mostrarModalFallback("modalEvento"))
    {
        console.warn("modalEvento nao encontrado ou Bootstrap indisponivel");
    }

    setTimeout(() =>
    {
        const txtNome = document.getElementById("txtNomeEvento");
        if (txtNome)
        {
            txtNome.focus();
        }
    }, 300);
}


/**
 * Fecha o formulário de cadastro
 */
function fecharFormularioCadastroEvento()
{
    fecharModalFallback("modalEvento");

    limparCamposCadastroEvento();
    console.log("Formulario de cadastro fechado");
}


/**
 * Configura os botões do formulário de cadastro
 */
function configurarBotoesCadastroEvento()
{
    // Botão Salvar Evento (Inserir)
    const btnInserir = document.getElementById("btnInserirEvento");
    if (btnInserir)
    {
        // Aplicar classe e ícone corretos
        btnInserir.className = "btn btn-azul";
        btnInserir.innerHTML = '<i class="fa-regular fa-thumbs-up"></i> Salvar Evento';

        const novoBtnInserir = btnInserir.cloneNode(true);
        btnInserir.parentNode.replaceChild(novoBtnInserir, btnInserir);

        novoBtnInserir.addEventListener("click", function ()
        {
            console.log("💾 Inserindo evento...");
            inserirNovoEvento();
        });
    }

    // Botão Cancelar
    const btnCancelar = document.getElementById("btnCancelarEvento");
    if (btnCancelar)
    {
        // Aplicar classe e ícone corretos
        btnCancelar.className = "btn btn-vinho";
        btnCancelar.innerHTML = '<i class="fa-regular fa-circle-xmark"></i> Cancelar';

        const novoBtnCancelar = btnCancelar.cloneNode(true);
        btnCancelar.parentNode.replaceChild(novoBtnCancelar, btnCancelar);

        novoBtnCancelar.addEventListener("click", function ()
        {
            console.log("❌ Cancelando cadastro");
            fecharFormularioCadastroEvento();
        });
    }

    console.log("✅ Botões do formulário configurados com estilos corretos");
}

/**
 * Limpa todos os campos do formulário de cadastro
 */
function limparCamposCadastroEvento()
{
    try
    {
        console.log("🧹 Limpando campos do formulário...");

        // Campos de texto simples
        const txtNome = document.getElementById("txtNomeEvento");
        if (txtNome) txtNome.value = "";

        const txtDescricao = document.getElementById("txtDescricaoEvento");
        if (txtDescricao) txtDescricao.value = "";
        // Datas
        const txtDataInicial = document.getElementById("txtDataInicialEvento");
        limparValorDataEvento(txtDataInicial);

        const txtDataFinal = document.getElementById("txtDataFinalEvento");
        limparValorDataEvento(txtDataFinal);

        // NumericTextBox (quantidade) - jQuery simples
        const txtQuantidade = document.getElementById("txtQtdParticipantesEventoCadastro");
        if (txtQuantidade)
        {
            $("#txtQtdParticipantesEventoCadastro").val(0);
        }

        // ComboBox Telerik (requisitante)
        const comboRequisitante = getRequisitanteEventoCombo();
        if (comboRequisitante)
        {
            comboRequisitante.value(null);
        }

        // Campo texto readonly (setor - nome)
        const txtSetor = document.getElementById("txtSetorRequisitanteEvento");
        if (txtSetor) txtSetor.value = '';

        // Campo hidden (setor - ID)
        const lstSetor = document.getElementById("lstSetorRequisitanteEvento");
        if (lstSetor) lstSetor.value = '';

        console.log("✅ Campos limpos com sucesso");

    } catch (error)
    {
        console.error("❌ Erro ao limpar campos:", error);
        Alerta.TratamentoErroComLinha("evento.js", "limparCamposCadastroEvento", error);
    }
}

/**
 * Insere um novo evento no banco de dados
 * Adaptado do código de ViagemUpsert.js
 */
function inserirNovoEvento()
{
    try
    {
        console.log("💾 Iniciando inserção de evento...");

        // Validação de campos obrigatórios
        const txtNome = document.getElementById("txtNomeEvento");
        const txtDescricao = document.getElementById("txtDescricaoEvento");
        const txtDataInicial = document.getElementById("txtDataInicialEvento");
        const txtDataFinal = document.getElementById("txtDataFinalEvento");
        const txtQuantidade = document.getElementById("txtQtdParticipantesEventoCadastro");

        if (!txtNome || !txtNome.value.trim())
        {
            Alerta.Alerta("Atenção", "O Nome do Evento é obrigatório!");
            return;
        }

        if (!txtDescricao || !txtDescricao.value.trim())
        {
            Alerta.Alerta("Atenção", "A Descrição do Evento é obrigatória!");
            return;
        }
        // Pega as datas (Kendo DatePicker ou input nativo)
        const dataInicial = obterValorDataEvento(txtDataInicial);
        const dataFinal = obterValorDataEvento(txtDataFinal);

        if (!dataInicial)
        {
            Alerta.Alerta("Atencao", "A Data Inicial eh obrigatoria!");
            return;
        }

        if (!dataFinal)
        {
            Alerta.Alerta("Atencao", "A Data Final eh obrigatoria!");
            return;
        }

        if (dataInicial > dataFinal)
        {
            Alerta.Alerta("Atencao", "A Data Inicial nao pode ser maior que a Data Final!");
            // Limpa data final via Kendo helper ou fallback nativo
            if (window.setKendoDateValue)
            {
                window.setKendoDateValue("txtDataFinalEvento", null);
            }
            else if (txtDataFinal)
            {
                limparValorDataEvento(txtDataFinal);
            }
            return;
        }

        // Pega quantidade - jQuery simples
        const quantidadeRaw = $("#txtQtdParticipantesEventoCadastro").val();
        const quantidade = parseInt(quantidadeRaw, 10) || 0;

        if (!quantidade || quantidade <= 0)
        {
            Alerta.Alerta("Atenção", "A Quantidade de Participantes é obrigatória!");
            return;
        }

        // Validação: Quantidade deve ser número inteiro
        if (!Number.isInteger(quantidade) || quantidade > 2147483647)
        {
            Alerta.Alerta("Atenção", "A Quantidade de Participantes deve ser um número inteiro válido (máximo: 2.147.483.647)!");
            // Limpa o campo de quantidade
            $("#txtQtdParticipantesEventoCadastro").val("");
            return;
        }

        // Pega setor (campo hidden) e requisitante (ComboBox Telerik)
        const lstSetor = document.getElementById("lstSetorRequisitanteEvento"); // Hidden input
        const comboRequisitante = getRequisitanteEventoCombo(); // ComboBox Telerik

        // Validação do setor (agora é um campo hidden)
        if (!lstSetor || !lstSetor.value || lstSetor.value.trim() === '')
        {
            Alerta.Alerta("Atenção", "O Setor é obrigatório! Selecione um requisitante primeiro.");
            return;
        }

        // Validação do requisitante (ComboBox Telerik)
        if (!comboRequisitante || !comboRequisitante.value())
        {
            Alerta.Alerta("Atenção", "O Requisitante é obrigatório!");
            return;
        }

        const setorId = lstSetor.value.toString(); // Lê do hidden input
        const requisitanteId = comboRequisitante.value().toString();

        // Prepara objeto para envio
        const objEvento = {
            Nome: txtNome.value.trim(),
            Descricao: txtDescricao.value.trim(),
            SetorSolicitanteId: setorId,
            RequisitanteId: requisitanteId,
            QtdParticipantes: quantidade,
            DataInicial: moment(dataInicial).format("MM-DD-YYYY"),
            DataFinal: moment(dataFinal).format("MM-DD-YYYY"),
            Status: "1"
        };

        console.log("📦 Objeto a ser enviado:", objEvento);

        // Envia via AJAX
        $.ajax({
            type: "POST",
            url: "/api/Viagem/AdicionarEvento",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(objEvento),
            success: function (data)
            {
                try
                {
                    console.log("✅ Resposta do servidor:", data);

                    if (data.success)
                    {
                        // Mostra mensagem de sucesso
                        AppToast.show('Verde', data.message);

                        // Atualiza a lista de eventos com o novo evento
                        atualizarListaEventos(data.eventoId, data.eventoText);

                        // Fecha o formulário
                        fecharFormularioCadastroEvento();

                        console.log("✅ Evento inserido com sucesso!");
                    }
                    else
                    {
                        Alerta.Alerta("Erro", data.message || "Erro ao adicionar evento");
                    }
                }
                catch (error)
                {
                    console.error("❌ Erro no success do AJAX:", error);
                    Alerta.TratamentoErroComLinha("evento.js", "ajax.AdicionarEvento.success", error);
                }
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                try
                {
                    console.error("❌ Erro na requisição AJAX:", errorThrown);
                    console.error("   Status:", textStatus);
                    console.error("   Response:", jqXHR.responseText);

                    Alerta.Alerta("Erro", "Erro ao adicionar evento no servidor");
                }
                catch (error)
                {
                    console.error("❌ Erro no error handler:", error);
                    Alerta.TratamentoErroComLinha("evento.js", "ajax.AdicionarEvento.error", error);
                }
            }
        });

    }
    catch (error)
    {
        console.error("❌ Erro ao inserir evento:", error);
        Alerta.TratamentoErroComLinha("evento.js", "inserirNovoEvento", error);
    }
}

/**
 * Atualiza a lista de eventos após adicionar um novo
 * param {string} eventoId - ID do evento recém-criado
 * param {string} eventoText - Nome do evento recém-criado
 */
function atualizarListaEventos(eventoId, eventoText)
{
    try
    {
        console.log("🔄 Atualizando lista de eventos...");
        console.log("   EventoId:", eventoId);
        console.log("   EventoText:", eventoText);

        // Kendo ComboBox para lstEventos
        const comboBox = $("#lstEventos").data("kendoComboBox") || $("#lstEventos").data("kendoDropDownList");

        if (!comboBox)
        {
            console.error("❌ lstEventos não encontrado ou não é componente Kendo");
            return;
        }

        // Cria o novo item com a estrutura correta (camelCase para Kendo JSON)
        const novoItem = {
            eventoId: eventoId,
            evento: eventoText
        };

        console.log("📦 Novo item a ser adicionado:", novoItem);

        // Obter dataSource atual (Kendo DataSource)
        let dataSource = comboBox.dataSource.data().toJSON();

        if (!Array.isArray(dataSource))
        {
            dataSource = [];
        }

        // Verificar se já existe
        const jaExiste = dataSource.some(item => item.eventoId === eventoId);

        if (!jaExiste)
        {
            // Adiciona o novo item
            dataSource.push(novoItem);
            console.log("📦 Novo item adicionado ao array");

            // Ordena alfabeticamente por nome do evento
            dataSource.sort((a, b) => {
                const nomeA = (a.evento || '').toString().toLowerCase();
                const nomeB = (b.evento || '').toString().toLowerCase();
                return nomeA.localeCompare(nomeB);
            });
            console.log("🔄 Lista ordenada alfabeticamente");

            // Atualiza o dataSource do Kendo com a lista ordenada
            comboBox.dataSource.data(dataSource);

            console.log("✅ Lista atualizada e ordenada com sucesso");
        }
        else
        {
            console.log("⚠️ Item já existe na lista");
        }

        // Aguarda o componente processar
        setTimeout(() =>
        {
            console.log("🔄 Selecionando novo evento...");

            // Define o valor (Kendo usa getter/setter function)
            comboBox.value(eventoId);

            console.log("✅ Evento selecionado");
            console.log("   Value:", comboBox.value());
            console.log("   Text:", comboBox.text());

            // Aguarda mais um pouco antes de buscar dados
            setTimeout(() =>
            {
                // Buscar e exibir os dados do evento
                if (typeof window.exibirDadosEvento === 'function')
                {
                    console.log("🔍 Chamando window.exibirDadosEvento...");
                    window.exibirDadosEvento(novoItem);
                }
                else if (typeof exibirDadosEvento === 'function')
                {
                    console.log("🔍 Chamando exibirDadosEvento...");
                    exibirDadosEvento(novoItem);
                }
                else
                {
                    console.warn("⚠️ Função exibirDadosEvento não encontrada");
                }
            }, 100);

        }, 250);

        console.log("✅ Processo de atualização iniciado");

    }
    catch (error)
    {
        console.error("❌ Erro ao atualizar lista de eventos:", error);
        Alerta.TratamentoErroComLinha("evento.js", "atualizarListaEventos", error);
    }
}

// ===============================================================
// DIAGNÓSTICO - Use no console para debugar
// ===============================================================

/**
 * Diagnóstico completo do sistema de evento
 */
function diagnosticarSistemaEvento()
{
    console.log("=== DIAGNÓSTICO DO SISTEMA DE EVENTO ===");

    const sectionEvento = document.getElementById("sectionEvento");
    console.log("1. sectionEvento existe?", !!sectionEvento);
    if (sectionEvento)
    {
        console.log("   - Display:", sectionEvento.style.display);
        console.log("   - Visível?", sectionEvento.offsetWidth > 0 && sectionEvento.offsetHeight > 0);
    }

    const sectionCadastro = document.getElementById("modalEvento");
    console.log("2. modalEvento existe?", !!sectionCadastro);
    if (sectionCadastro)
    {
        console.log("   - Display:", sectionCadastro.style.display);
        console.log("   - Visível?", sectionCadastro.offsetWidth > 0 && sectionCadastro.offsetHeight > 0);
    }

    const lstFinalidade = document.getElementById("lstFinalidade");
    console.log("3. lstFinalidade existe?", !!lstFinalidade);
    const ddlFinalidade = $("#lstFinalidade").data("kendoDropDownList");
    if (ddlFinalidade)
    {
        console.log("   - É componente Kendo?", true);
        console.log("   - Valor atual:", ddlFinalidade.value());
    }

    const lstEventos = document.getElementById("lstEventos");
    console.log("4. lstEventos existe?", !!lstEventos);
    const cmbEventos = $("#lstEventos").data("kendoComboBox") || $("#lstEventos").data("kendoDropDownList");
    if (cmbEventos)
    {
        console.log("   - É componente Kendo?", true);
        console.log("   - DataSource:", cmbEventos.dataSource.data());
        console.log("   - Quantidade de itens:", cmbEventos.dataSource.data().length || 0);
    }

    const btnEvento = document.getElementById("btnEvento");
    console.log("5. btnEvento existe?", !!btnEvento);
    if (btnEvento)
    {
        console.log("   - Display:", window.getComputedStyle(btnEvento).display);
        console.log("   - Visível?", btnEvento.offsetWidth > 0 && btnEvento.offsetHeight > 0);
        console.log("   - Dimensões:", btnEvento.offsetWidth + "x" + btnEvento.offsetHeight);
    }

    const btnInserir = document.getElementById("btnInserirEvento");
    console.log("6. btnInserirEvento existe?", !!btnInserir);

    const btnCancelar = document.getElementById("btnCancelarEvento");
    console.log("7. btnCancelarEvento existe?", !!btnCancelar);

    console.log("=== FIM DO DIAGNÓSTICO ===");
}

/**
 * Testa mostrar a seção de evento
 */
function testarMostrarSecaoEvento()
{
    console.log("🧪 Teste: Mostrando seção de evento");
    controlarVisibilidadeSecaoEvento("Evento");
}

/**
 * Testa ocultar a seção de evento
 */
function testarOcultarSecaoEvento()
{
    console.log("🧪 Teste: Ocultando seção de evento");
    controlarVisibilidadeSecaoEvento("Transporte");
}

/**
 * Testa abrir o formulário de cadastro
 */
function testarAbrirFormulario()
{
    console.log("🧪 Teste: Abrindo formulário de cadastro");
    abrirFormularioCadastroEvento();
}

/**
 * Testa fechar o formulário de cadastro
 */
function testarFecharFormulario()
{
    console.log("🧪 Teste: Fechando formulário de cadastro");
    fecharFormularioCadastroEvento();
}

/**
 * Testa limpar campos do formulário
 */
function testarLimparCampos()
{
    console.log("🧪 Teste: Limpando campos");
    limparCamposCadastroEvento();
}

/**
 * Verifica se todos os elementos necessários existem
 */
function verificarElementosEvento()
{
    console.log("=== VERIFICAÇÃO DE ELEMENTOS ===");

    const elementos = [
        "sectionEvento",
        "modalEvento",
        "lstEventos",
        "btnEvento",
        "txtNomeEvento",
        "txtDescricaoEvento",
        "txtDataInicialEvento",
        "txtDataFinalEvento",
        "txtQtdParticipantesEventoCadastro",
        "lstRequisitanteEvento",
        "lstSetorRequisitanteEvento",
        "btnInserirEvento",
        "btnCancelarEvento"
    ];

    let todosExistem = true;

    elementos.forEach(id =>
    {
        const elemento = document.getElementById(id);
        const existe = !!elemento;
        console.log(existe ? "✅" : "❌", id, "existe?", existe);
        if (!existe) todosExistem = false;
    });

    console.log("=== FIM DA VERIFICAÇÃO ===");
    console.log(todosExistem ? "✅ Todos os elementos existem!" : "⚠️ Alguns elementos estão faltando!");

    return todosExistem;
}

// ===============================================================
// EXPORTAÇÃO (se usar módulos)
// ===============================================================

// Se você usar módulos ES6, descomente as linhas abaixo:
// export {
//     inicializarSistemaEvento,
//     controlarVisibilidadeSecaoEvento,
//     abrirFormularioCadastroEvento,
//     fecharFormularioCadastroEvento,
//     diagnosticarSistemaEvento
// };
