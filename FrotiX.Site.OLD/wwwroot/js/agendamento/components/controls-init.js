/* ****************************************************************************************
 * ⚡ ARQUIVO: controls-init.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Inicialização programática de event handlers para controles Syncfusion
 *                   do formulário de agendamento. Configura eventos (change, select, blur,
 *                   created, toolbarClick) e templates customizados (motorista com foto)
 *                   para 10 componentes diferentes. Remove eventos anteriores (=null)
 *                   antes de atribuir novos para evitar duplicação. Deve ser chamado
 *                   APÓS DOM pronto E controles renderizados.
 * 📥 ENTRADAS     : Nenhum parâmetro (função void), acessa DOM diretamente
 *                   (getElementById), referências globais (window.* callbacks), ej2_instances
 * 📤 SAÍDAS       : Event handlers configurados (change, select, blur, created, toolbarClick),
 *                   templates aplicados (itemTemplate, valueTemplate para motorista),
 *                   console.log com status de configuração (produção!), callbacks invocados
 *                   (onLstMotoristaCreated)
 * 🔗 CHAMADA POR  : DOMContentLoaded handlers, main.js inicialização, após render de
 *                   controles Syncfusion
 * 🔄 CHAMA        : document.getElementById, console.log, window.* callbacks
 *                   (lstFinalidade_Change, MotoristaValueChange, VeiculoValueChange,
 *                   onSelectRequisitante, RequisitanteValueChange, etc.),
 *                   Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 (ej2_instances), window.* callback functions
 *                   (event-handlers.js), Alerta.js, imagens (/images/barbudo.jpg)
 * 📝 OBSERVAÇÕES  : Exporta window.inicializarEventHandlersControles (função global).
 *                   Todos os eventos resetados (=null) antes de atribuir para evitar
 *                   múltiplos handlers. console.log em produção (lines 11-258). Templates
 *                   de motorista com foto circular 40x40px (item) e 30x30px (value).
 *                   lstRequisitante tem 2 eventos (select + change). lstPeriodos condicional
 *                   (só configura se window.PeriodosValueChange existir).
 *
 * 📋 ÍNDICE DE FUNÇÕES (1 função global window.*):
 *
 * ┌─ FUNÇÃO PRINCIPAL ────────────────────────────────────────────────────┐
 * │ 1. window.inicializarEventHandlersControles()                        │
 * │    → Inicializa event handlers para 10 componentes Syncfusion        │
 * │    → Para cada componente:                                           │
 * │      1. getElementById + verifica ej2_instances[0] existe            │
 * │      2. Remove evento anterior: obj.event = null                     │
 * │      3. Atribui novo evento: obj.event = function(args) { ... }      │
 * │      4. console.log status                                           │
 * │    → try-catch: Alerta.TratamentoErroComLinha                        │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 📦 COMPONENTES CONFIGURADOS (10 componentes):
 *
 * ┌─ 1. FINALIDADE ───────────────────────────────────────────────────────┐
 * │ • ID: lstFinalidade (Syncfusion DropDownList)                        │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.lstFinalidade_Change(args)                        │
 * │ • Reset: finalidadeObj.change = null antes                           │
 * │ • Log: "✅ lstFinalidade: change event configurado"                  │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 2. MOTORISTA (com templates customizados) ──────────────────────────┐
 * │ • ID: lstMotorista (Syncfusion DropDownList)                         │
 * │ • Eventos: created, change                                           │
 * │ • Callbacks:                                                          │
 * │   - created: window.onLstMotoristaCreated()                          │
 * │   - change: window.MotoristaValueChange(args)                        │
 * │ • Templates:                                                          │
 * │   - itemTemplate: div.d-flex com img (40x40px circular) + span       │
 * │   - valueTemplate: div.d-flex com img (30x30px circular) + span      │
 * │   - Imagem: data.FotoBase64 (se startsWith 'data:image')             │
 * │              senão '/images/barbudo.jpg'                             │
 * │   - onerror: fallback para '/images/barbudo.jpg'                     │
 * │   - Text: data.Nome || data.MotoristaCondutor || ''                  │
 * │ • Executa onLstMotoristaCreated() imediatamente após templates       │
 * │ • Log: "🔧 Inicializando lstMotorista...", "✅ configurado"          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 3. VEÍCULO ──────────────────────────────────────────────────────────┐
 * │ • ID: lstVeiculo (Syncfusion DropDownList)                           │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.VeiculoValueChange(args)                          │
 * │ • Reset: veiculoObj.change = null antes                              │
 * │ • Log: "✅ lstVeiculo: change event configurado"                     │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 4. REQUISITANTE (2 eventos) ────────────────────────────────────────┐
 * │ • ID: lstRequisitante (Syncfusion DropDownList)                      │
 * │ • Eventos: select, change                                            │
 * │ • Callbacks:                                                          │
 * │   - select: window.onSelectRequisitante(args) (preenche ramal/setor) │
 * │   - change: window.RequisitanteValueChange(args)                     │
 * │ • console.log: Antes/Depois de cada evento (debug)                   │
 * │ • Reset: requisitanteObj.select = null, .change = null               │
 * │ • Log: "🔧 Configurando eventos...", "✅ select e change configurados"│
 * │ • Diferença:                                                          │
 * │   - select: dispara ao selecionar item da lista                      │
 * │   - change: dispara ao mudar valor (inclusive digitação)             │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 5. REQUISITANTE EVENTO ─────────────────────────────────────────────┐
 * │ • ID: lstRequisitanteEvento (Syncfusion DropDownList)                │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.RequisitanteEventoValueChange(args)               │
 * │ • Reset: requisitanteEventoObj.change = null antes                   │
 * │ • Log: "✅ lstRequisitanteEvento: change event configurado"          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 6. DIAS DA SEMANA ───────────────────────────────────────────────────┐
 * │ • ID: lstDias (Syncfusion MultiSelect)                               │
 * │ • Eventos: blur                                                       │
 * │ • Callback: window.onBlurLstDias(args)                               │
 * │ • Reset: diasObj.blur = null antes                                   │
 * │ • Log: "✅ lstDias: blur event configurado"                          │
 * │ • Nota: usa blur (não change) para validar após seleção completa     │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 7. RICH TEXT EDITOR (Descrição) ────────────────────────────────────┐
 * │ • ID: rteDescricao (Syncfusion RichTextEditor)                       │
 * │ • Eventos: created, toolbarClick                                     │
 * │ • Callbacks:                                                          │
 * │   - created: window.onCreate() (se função existir)                   │
 * │   - toolbarClick: window.toolbarClick(args) (se função existir)      │
 * │ • Verificação condicional: if (window.onCreate) antes de atribuir    │
 * │ • Log: "✅ rteDescricao: created e toolbarClick events configurados" │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 8. RECORRENTE ───────────────────────────────────────────────────────┐
 * │ • ID: lstRecorrente (Syncfusion DropDownList)                        │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.RecorrenteValueChange(args)                       │
 * │ • Reset: recorrenteObj.change = null antes                           │
 * │ • Log: "✅ lstRecorrente: change event configurado"                  │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 9. PERÍODOS (condicional) ──────────────────────────────────────────┐
 * │ • ID: lstPeriodos (Syncfusion DropDownList)                          │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.PeriodosValueChange(args)                         │
 * │ • Condicional: if (window.PeriodosValueChange) antes de atribuir     │
 * │ • Nota: só configura se callback existir globalmente                 │
 * │ • Log: "✅ lstPeriodos: change event configurado" (se configurado)   │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 10. SETOR REQUISITANTE (comentado) ─────────────────────────────────┐
 * │ • ID: ddtSetorRequisitante (comentado, linhas 171-181)               │
 * │ • Nota: estava com change="MotoristaValueChange" (provavelmente erro)│
 * │ • Deixado sem evento específico (comentado)                          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE INICIALIZAÇÃO:
 * 1. inicializarEventHandlersControles() chamado
 * 2. console.log "🎯 Inicializando event handlers..."
 * 3. Para cada componente (10 total):
 *    a. getElementById('id')
 *    b. Verifica ej2_instances && ej2_instances[0] existe
 *    c. Se existe:
 *       - Obtém referência: obj = element.ej2_instances[0]
 *       - Remove evento anterior: obj.event = null
 *       - Atribui novo evento: obj.event = function(args) { callback(args) }
 *       - console.log status
 *    d. Se não existe: pula (nenhum erro lançado)
 * 4. console.log "✅ Todos os event handlers foram configurados!"
 * 5. try-catch global: Alerta.TratamentoErroComLinha se qualquer erro
 *
 * 🔄 FLUXO DE TEMPLATES MOTORISTA:
 * 1. Obtém referência motoristaObj = lstMotorista.ej2_instances[0]
 * 2. Define itemTemplate = function(data):
 *    a. Verifica data.FotoBase64 && startsWith('data:image')
 *    b. Se sim: imgSrc = data.FotoBase64
 *    c. Senão: imgSrc = '/images/barbudo.jpg'
 *    d. Retorna HTML: div.d-flex com img (40x40px) + span
 *    e. img onerror: this.src='/images/barbudo.jpg'
 * 3. Define valueTemplate = function(data):
 *    a. Idêntico a itemTemplate mas img 30x30px
 * 4. Executa onLstMotoristaCreated() imediatamente
 * 5. console.log "✅ lstMotorista configurado"
 *
 * 🔄 FLUXO DE EVENTO REQUISITANTE (dual events):
 * 1. Obtém referência requisitanteObj
 * 2. console.log "Antes - select/change" (valores anteriores)
 * 3. Reset: requisitanteObj.select = null, .change = null
 * 4. Atribui select: function(args) { onSelectRequisitante(args) }
 * 5. Atribui change: function(args) { RequisitanteValueChange(args) }
 * 6. console.log "Depois - select/change" (novos valores)
 * 7. console.log "✅ select e change events configurados"
 *
 * 📌 PATTERN DE RESET E ATRIBUIÇÃO:
 * - Sempre: obj.event = null ANTES de obj.event = function() { ... }
 * - Motivo: evita múltiplos handlers se função chamada repetidamente
 * - Exemplo: finalidadeObj.change = null; finalidadeObj.change = function() {}
 *
 * 📌 TEMPLATES MOTORISTA (itemTemplate vs valueTemplate):
 * - itemTemplate: renderizado em cada item da lista dropdown (40x40px)
 * - valueTemplate: renderizado no campo selecionado (30x30px)
 * - Ambos: div.d-flex.align-items-center com img circular + span
 * - Foto: data.FotoBase64 (base64 data URI) ou fallback /images/barbudo.jpg
 * - Text: data.Nome || data.MotoristaCondutor || ''
 * - onerror: this.src='/images/barbudo.jpg' (duplo fallback)
 *
 * 📌 EVENTOS CONDICIONAIS:
 * - rteDescricao: if (window.onCreate) antes de atribuir created
 * - lstPeriodos: if (window.PeriodosValueChange) antes de atribuir change
 * - Motivo: funções callback podem não existir em todos os contextos
 *
 * 📌 CALLBACKS WINDOW.* ESPERADOS:
 * 1. lstFinalidade_Change(args)
 * 2. onLstMotoristaCreated() - sem args
 * 3. MotoristaValueChange(args)
 * 4. VeiculoValueChange(args)
 * 5. onSelectRequisitante(args) - preenche ramal/setor
 * 6. RequisitanteValueChange(args)
 * 7. RequisitanteEventoValueChange(args)
 * 8. onBlurLstDias(args)
 * 9. onCreate() - RTE created
 * 10. toolbarClick(args) - RTE toolbar
 * 11. RecorrenteValueChange(args)
 * 12. PeriodosValueChange(args) - condicional
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - console.log em produção (11 + console.log por componente)
 * - Função deve ser chamada APÓS controles renderizados (não em DOMContentLoaded cedo demais)
 * - lstRequisitante único com 2 eventos (select + change)
 * - lstDias usa blur (não change) para validar após seleção completa (MultiSelect)
 * - ddtSetorRequisitante comentado (linhas 169-181) - estava com evento errado
 * - onLstMotoristaCreated() chamado imediatamente após definir templates (linha 92)
 * - Nenhum erro lançado se componente não existir (if verifica antes de acessar)
 * - try-catch global captura qualquer erro de qualquer componente
 * - Imagens motorista: object-fit: cover; border-radius: 50%; (circular)
 * - lstPeriodos condicional sugere que nem todos contextos precisam desse evento
 * - console.log emoji: 🎯 (início), 🔧 (configurando), ✅ (sucesso)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Inicializa todos os event handlers dos controles Kendo UI.
 * Deve ser chamado APÓS o DOM estar pronto E após widgets Kendo inicializados (ScriptsBlock).
 * MIGRADO de Syncfusion ej2_instances para Kendo API em 02/2026.
 */

// Helper para bind seguro: unbind + bind (evita handlers duplicados)
function bindKendoEvent(widget, eventName, handler) {
    if (!widget || !eventName || !handler) return;
    widget.unbind(eventName);
    widget.bind(eventName, handler);
}

// Template de item do dropdown Motorista (40x40px circular com foto)
function motoristaItemTemplate(data) {
    if (!data) return '';
    var imgSrc = (data.fotoBase64 && data.fotoBase64.indexOf('data:image') === 0)
        ? data.fotoBase64 : '/images/barbudo.jpg';
    var nome = data.nome || data.motoristaCondutor || '';
    return '<div class="d-flex align-items-center">' +
        '<img src="' + imgSrc + '" alt="Foto" ' +
        'style="height:40px;width:40px;border-radius:50%;margin-right:10px;object-fit:cover;" ' +
        'onerror="this.src=\'/images/barbudo.jpg\';" />' +
        '<span>' + kendo.htmlEncode(nome) + '</span></div>';
}

// Template de valor selecionado do Motorista (30x30px)
function motoristaValueTemplate(data) {
    if (!data) return '';
    var imgSrc = (data.fotoBase64 && data.fotoBase64.indexOf('data:image') === 0)
        ? data.fotoBase64 : '/images/barbudo.jpg';
    var nome = data.nome || data.motoristaCondutor || '';
    return '<div class="d-flex align-items-center">' +
        '<img src="' + imgSrc + '" alt="Foto" ' +
        'style="height:30px;width:30px;border-radius:50%;margin-right:10px;object-fit:cover;" ' +
        'onerror="this.src=\'/images/barbudo.jpg\';" />' +
        '<span>' + kendo.htmlEncode(nome) + '</span></div>';
}

window.inicializarEventHandlersControles = function () {
    try {
        console.log('🎯 Inicializando event handlers dos controles Kendo...');

        // ============================================
        // 1. FINALIDADE (DropDownList)
        // ============================================
        var ddlFinalidade = $("#lstFinalidade").data("kendoDropDownList");
        if (ddlFinalidade) {
            bindKendoEvent(ddlFinalidade, "change", function (e) {
                if (window.lstFinalidade_Change) window.lstFinalidade_Change(e);
            });
            console.log('✅ lstFinalidade: change event configurado');
        }

        // ============================================
        // 2. MOTORISTA (ComboBox com templates customizados)
        // ============================================
        var cmbMotorista = $("#lstMotorista").data("kendoComboBox");
        if (cmbMotorista) {
            console.log('🔧 Inicializando lstMotorista...');

            // [UI] Aplicar templates com foto circular
            cmbMotorista.setOptions({
                template: motoristaItemTemplate,
                valueTemplate: motoristaValueTemplate
            });

            // [LOGICA] Bind evento change
            bindKendoEvent(cmbMotorista, "change", function (e) {
                if (window.MotoristaValueChange) window.MotoristaValueChange(e);
            });

            // [LOGICA] Executar callback de inicialização
            if (window.onLstMotoristaCreated) window.onLstMotoristaCreated();

            console.log('✅ lstMotorista configurado com templates e eventos');
        }

        // ============================================
        // 3. VEÍCULO (ComboBox)
        // ============================================
        var cmbVeiculo = $("#lstVeiculo").data("kendoComboBox");
        if (cmbVeiculo) {
            bindKendoEvent(cmbVeiculo, "change", function (e) {
                if (window.VeiculoValueChange) window.VeiculoValueChange(e);
            });
            console.log('✅ lstVeiculo: change event configurado');
        }

        // ============================================
        // 4. REQUISITANTE (ComboBox — select + change)
        // ============================================
        var cmbRequisitante = $("#lstRequisitante").data("kendoComboBox");
        if (cmbRequisitante) {
            console.log('🔧 Configurando eventos do lstRequisitante...');

            // select: dispara ao selecionar item da lista (preenche ramal/setor)
            bindKendoEvent(cmbRequisitante, "select", function (e) {
                if (window.onSelectRequisitante) window.onSelectRequisitante(e);
            });

            // change: dispara ao mudar valor (inclusive digitação)
            bindKendoEvent(cmbRequisitante, "change", function (e) {
                if (window.RequisitanteValueChange) window.RequisitanteValueChange(e);
            });

            console.log('✅ lstRequisitante: select e change events configurados');
        }

        // ============================================
        // 5. REQUISITANTE EVENTO (ComboBox)
        // ============================================
        var cmbRequisitanteEvento = $("#lstRequisitanteEvento").data("kendoComboBox");
        if (cmbRequisitanteEvento) {
            bindKendoEvent(cmbRequisitanteEvento, "change", function (e) {
                if (window.RequisitanteEventoValueChange) window.RequisitanteEventoValueChange(e);
            });
            console.log('✅ lstRequisitanteEvento: change event configurado');
        }

        // ============================================
        // 6. SETOR REQUISITANTE — TreeView select já configurado no CSHTML init
        // ============================================

        // ============================================
        // 7. DIAS DA SEMANA (MultiSelect — close = equiv. blur Syncfusion)
        // ============================================
        var msLstDias = $("#lstDias").data("kendoMultiSelect");
        if (msLstDias) {
            bindKendoEvent(msLstDias, "close", function (e) {
                if (window.onBlurLstDias) window.onBlurLstDias(e);
            });
            console.log('✅ lstDias: close event configurado (equiv. blur)');
        }

        // ============================================
        // 8. RICH TEXT EDITOR — Descrição (Kendo Editor)
        // ============================================
        var kendoEditor = $("#rteDescricao").data("kendoEditor");
        if (kendoEditor) {
            if (window.onCreate && !window.defaultRTE) {
                window.onCreate();
            }
            console.log('✅ rteDescricao: Kendo Editor configurado');
        }

        // ============================================
        // 9. RECORRENTE (DropDownList)
        // ============================================
        var ddlRecorrente = $("#lstRecorrente").data("kendoDropDownList");
        if (ddlRecorrente) {
            bindKendoEvent(ddlRecorrente, "change", function (e) {
                if (window.RecorrenteValueChange) window.RecorrenteValueChange(e);
            });
            console.log('✅ lstRecorrente: change event configurado');
        }

        // ============================================
        // 10. PERÍODOS (DropDownList — condicional)
        // ============================================
        var ddlPeriodos = $("#lstPeriodos").data("kendoDropDownList");
        if (ddlPeriodos && window.PeriodosValueChange) {
            bindKendoEvent(ddlPeriodos, "change", function (e) {
                window.PeriodosValueChange(e);
            });
            console.log('✅ lstPeriodos: change event configurado');
        }

        console.log('✅ Todos os event handlers Kendo foram configurados!');

    } catch (error) {
        Alerta.TratamentoErroComLinha("controls-init.js", "inicializarEventHandlersControles", error);
    }
};
