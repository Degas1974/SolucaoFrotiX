/* ****************************************************************************************
 * ⚡ ARQUIVO: kendo-editor-helper.js (v4)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Adapter/Bridge para Kendo UI Editor (Telerik) com camada de
 *                   compatibilidade Syncfusion. 13 funções para gerenciar ciclo completo:
 *                   lazy initialization, destruction completa (wrapper removal, jQuery
 *                   data cleanup, ej2_instances delete), get/set HTML com pendingValue
 *                   buffer, refresh, clear, integração Bootstrap Modal (shown/hidden
 *                   events para init/destroy automático), createCompatibilityLayer para
 *                   emular ej2_instances[0] API (permite código Syncfusion-expecting
 *                   funcionar com Kendo Editor), overwrite window.refreshComponenteSafe,
 *                   orphan wrapper cleanup. IIFE wrapper com 'use strict'. 23-tools
 *                   toolbar (bold, italic, underline, strikethrough, justify*, lists,
 *                   indent/outdent, link, image, viewHtml, formatting, font, colors).
 * 📥 ENTRADAS     : getHtml() retorna HTML string, setHtml(html: string), init/destroy/
 *                   clear/refresh sem params, setupModalIntegration() sem params
 * 📤 SAÍDAS       : getHtml retorna string HTML (fallback pendingValue se editor não
 *                   existe), setHtml atualiza editor ou guarda em pendingValue, init
 *                   retorna Kendo Editor instance ou null, destroy retorna void (side
 *                   effects: DOM cleanup), compatibility layer cria textarea.ej2_instances
 *                   array, console.log/error/warn extensive logging
 * 🔗 CHAMADA POR  : exibe-viagem.js (inicialização modal, preenchimento dados), controls-init.js
 *                   (inicialização componentes), modal events (Bootstrap shown/hidden),
 *                   código que espera Syncfusion RichTextEditor mas usa Kendo (via compat
 *                   layer: textarea.ej2_instances[0].value, dataBind, etc.), document.ready
 *                   auto-init
 * 🔄 CHAMA        : $(textarea).kendoEditor({ tools, resizable }), editor.value() getter/
 *                   setter, editor.destroy(), editor.refresh(), $(textarea).data("kendoEditor"),
 *                   $(textarea).removeData("kendoEditor"), $(textarea).closest(".k-editor"),
 *                   jQuery.before/remove, $(modal).on/off ('shown.bs.modal.kendoEditor',
 *                   'hidden.bs.modal.kendoEditor' namespaced events), setTimeout (50ms,
 *                   100ms delays), document.getElementById, console logging, delete
 *                   textarea.ej2_instances, $(textarea).show().css(), $(document).ready
 * 📦 DEPENDÊNCIAS : Kendo UI Editor (Telerik: $.kendoEditor, data("kendoEditor"), .k-editor
 *                   CSS class), jQuery ($.ajax, $.data, $.on/off, $.closest, $.before,
 *                   $.remove, $.show, $.css, $.ready), Bootstrap 5 Modal (shown.bs.modal,
 *                   hidden.bs.modal events), DOM elements (#rteDescricao textarea,
 *                   #modalViagens modal), window.refreshComponenteSafe (external function,
 *                   optional), Syncfusion-expecting code (via compat layer: ej2_instances
 *                   API emulation)
 * 📝 OBSERVAÇÕES  : Exporta window.KendoEditorHelper = { init, destroy, getHtml, setHtml,
 *                   clear, refresh, setupModalIntegration, instance getter, isInitialized
 *                   getter } (9 methods/properties). IIFE pattern: (function(window) {
 *                   'use strict'; ... })(window) para isolation. pendingValue buffer:
 *                   armazena HTML quando editor não existe (lazy init), aplicado em init.
 *                   Orphan cleanup: verifica wrapper órfão (.k-editor sem instância Kendo)
 *                   antes de criar novo editor. Namespaced events: .kendoEditor suffix
 *                   permite remover apenas esses listeners ($.off específico). Compatibility
 *                   layer: cria fake ej2_instances[0] object com getters/setters value,
 *                   methods getHtml/getValue/dataBind/refresh, enabled property (permite
 *                   código Syncfusion funcionar sem modificação). refreshComponenteSafe
 *                   overwrite: intercepta calls para "rteDescricao" e delega para Kendo
 *                   refresh, fallback para original function. TOOLS array: 23 items
 *                   hardcoded (toolbar completa). Modal integration: auto-init on shown,
 *                   auto-destroy on hidden (ciclo completo gerenciado). Try-catch apenas
 *                   em funções críticas (getHtml, setHtml, destroy, init). Console logging
 *                   prefix: "[KendoEditorHelper]" para fácil identificação. $(document).ready:
 *                   auto-setup compatibility layer + modal integration no startup.
 *
 * 📋 ÍNDICE DE FUNÇÕES (13 funções dentro de IIFE, 1 export object):
 *
 * ┌─ ESTADO (module-level variables) ───────────────────────────────────┐
 * │ let pendingValue = null                                              │
 * │ → Buffer para HTML quando editor não existe                          │
 * │ → Aplicado em init quando editor criado                              │
 * │ → Limpo após aplicação (pendingValue = null)                         │
 * │                                                                      │
 * │ const TOOLS = [23 items]                                             │
 * │ → Array de strings: toolbar items Kendo Editor                       │
 * │ → bold, italic, underline, strikethrough, justify*, lists, indent,   │
 * │   link, image, viewHtml, formatting, font, colors, cleanFormatting   │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ getEditorInstance() ───────────────────────────────────────────────┐
 * │ → Obtém instância Kendo Editor se existir                            │
 * │ → returns Kendo Editor instance ou null                              │
 * │ → Fluxo:                                                              │
 * │   1. const textarea = getElementById("rteDescricao")                 │
 * │   2. if textarea: return $(textarea).data("kendoEditor")             │
 * │   3. return null                                                     │
 * │ → Usado internamente por todas as funções                            │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ getHtml() ──────────────────────────────────────────────────────────┐
 * │ → Obtém HTML do editor ou pendingValue                               │
 * │ → returns string HTML (ou "" se erro)                                │
 * │ → Fluxo:                                                              │
 * │   1. try-catch wrapper                                                │
 * │   2. const editor = getEditorInstance()                               │
 * │   3. if editor: return editor.value() || ""                           │
 * │   4. return pendingValue || ""                                        │
 * │   5. catch: console.error + return ""                                 │
 * │ → Fallback chain: editor.value() → pendingValue → ""                 │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ setHtml(html) ──────────────────────────────────────────────────────┐
 * │ → Define HTML do editor ou guarda em pendingValue                    │
 * │ → param html: string HTML content                                    │
 * │ → returns void                                                        │
 * │ → Fluxo:                                                              │
 * │   1. try-catch wrapper                                                │
 * │   2. const editor = getEditorInstance()                               │
 * │   3. if editor:                                                       │
 * │      a. editor.value(html || "")                                      │
 * │      b. console.log "HTML definido no editor"                         │
 * │   4. else:                                                            │
 * │      a. pendingValue = html                                           │
 * │      b. console.log "HTML guardado para aplicar depois"               │
 * │   5. catch: console.error                                             │
 * │ → Lazy pattern: armazena se editor não existe, aplica depois         │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ destroyKendoEditor() ───────────────────────────────────────────────┐
 * │ → Destrói COMPLETAMENTE Kendo Editor e limpa DOM                     │
 * │ → returns void                                                        │
 * │ → Fluxo:                                                              │
 * │   1. try-catch wrapper                                                │
 * │   2. const textarea = getElementById("rteDescricao")                  │
 * │   3. if !textarea: return                                             │
 * │   4. const editor = $(textarea).data("kendoEditor")                   │
 * │   5. if editor: editor.destroy() + console.log                        │
 * │   6. const wrapper = $(textarea).closest(".k-editor")                 │
 * │   7. if wrapper.length:                                               │
 * │      a. wrapper.before(textarea)  // move textarea out                │
 * │      b. wrapper.remove()          // remove wrapper HTML              │
 * │      c. console.log "Wrapper removido"                                │
 * │   8. $(textarea).removeData("kendoEditor")                            │
 * │   9. $(textarea).show().css({ height: "250px", width: "100%" })      │
 * │   10. delete textarea.ej2_instances                                   │
 * │   11. pendingValue = null                                             │
 * │   12. console.log "Editor destruído completamente"                    │
 * │   13. catch: console.error                                            │
 * │ → Critical: remove wrapper HTML (Kendo cria .k-editor wrapper div)   │
 * │ → Cleanup completo: destroy + removeData + delete ej2_instances +    │
 * │   pendingValue reset                                                  │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * [Continuação da documentação devido ao tamanho...]
 *
 * 📌 COMPATIBILITY LAYER (ej2_instances emulation):
 * Cria textarea.ej2_instances = [compatObj] onde compatObj tem:
 * - get value(): return getHtml()
 * - set value(val): setHtml(val)
 * - getHtml(): return getHtml()
 * - getValue(): return getHtml()
 * - dataBind(): refreshEditor()
 * - refresh(): refreshEditor()
 * - enabled: true (property)
 *
 * → Permite código como: textarea.ej2_instances[0].value = "..." funcionar
 * → Syncfusion API emulation para compatibilidade sem refactor
 *
 * 📌 MODAL INTEGRATION (Bootstrap events):
 * - shown.bs.modal.kendoEditor: setTimeout 100ms → initKendoEditor()
 * - hidden.bs.modal.kendoEditor: destroyKendoEditor() + setTimeout 50ms →
 *   createInitialCompatibilityLayer()
 * - Namespaced events (.kendoEditor) permitem $.off específico sem afetar
 *   outros listeners
 * - Cycle: modal abre → init editor → modal fecha → destroy editor →
 *   recreate compat layer → pronto para próxima abertura
 *
 * 📌 TOOLS ARRAY (23 items):
 * ["bold", "italic", "underline", "strikethrough", "justifyLeft",
 *  "justifyCenter", "justifyRight", "justifyFull", "insertUnorderedList",
 *  "insertOrderedList", "indent", "outdent", "createLink", "unlink",
 *  "insertImage", "viewHtml", "formatting", "fontName", "fontSize",
 *  "foreColor", "backColor", "cleanFormatting"]
 *
 * → Toolbar completa (text formatting, alignment, lists, links, images,
 *   HTML view, formatting dropdown, font controls, colors, clean)
 * → Hardcoded array (não customizável via config)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - IIFE + 'use strict': isolation + strict mode enforcement
 * - pendingValue pattern: permite setHtml antes de init (lazy loading)
 * - Orphan wrapper cleanup: previne múltiplos wrappers (bug common em SPAs)
 * - Namespaced events: .kendoEditor suffix crucial para cleanup seletivo
 * - .k-editor: CSS class do wrapper Kendo (div criado automaticamente)
 * - $(textarea).data("kendoEditor"): método jQuery para acessar instância widget
 * - editor.value(): Kendo Editor getter/setter para HTML content
 * - editor.destroy(): método oficial Kendo para cleanup (remove toolbar, iframe, etc.)
 * - $(textarea).removeData(): limpa jQuery internal data store
 * - delete textarea.ej2_instances: remove fake Syncfusion property
 * - setTimeout delays: 50ms (compat layer), 100ms (editor init) para aguardar DOM
 * - console logging: prefix [KendoEditorHelper] em todas as mensagens
 * - try-catch seletivo: apenas em funções com DOM manipulation ou Kendo API calls
 * - window.refreshComponenteSafe: função externa (definida em outro arquivo),
 *   overwrite necessário para interceptar calls "rteDescricao"
 * - originalRefresh: guarda referência função anterior para fallback chain
 * - $(document).ready: garante DOM loaded antes de setup
 * - resizable config: content=true (editor content resizable), toolbar=false
 *   (toolbar fixed)
 * - createCompatibilityLayer check: if (textarea.ej2_instances) return (previne
 *   múltiplas camadas)
 * - getter syntax: get value() { ... } (ES5+ property getter)
 * - setter syntax: set value(val) { ... } (ES5+ property setter)
 * - $(modal).off('shown.bs.modal.kendoEditor hidden.bs.modal.kendoEditor'):
 *   remove TODOS os listeners namespaced (previne duplicação)
 * - wrapper.before(textarea): move element DOM antes de remover wrapper (previne
 *   perda de textarea)
 * - $(textarea).show().css(): restore textarea visibility + dimensions (caso
 *   Kendo tenha ocultado)
 * - window.KendoEditorHelper.instance: getter que retorna getEditorInstance()
 * - window.KendoEditorHelper.isInitialized: getter boolean (instance !== null)
 *
 * 🔌 VERSÃO: 4.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

(function (window) {
    'use strict';

    // ============================================
    // ESTADO
    // ============================================
    let pendingValue = null;

    // ============================================
    // CONFIGURAÇÃO
    // ============================================
    const TOOLS = [
        "bold", "italic", "underline", "strikethrough",
        "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
        "insertUnorderedList", "insertOrderedList",
        "indent", "outdent",
        "createLink", "unlink",
        "insertImage",
        "viewHtml",
        "formatting",
        "fontName",
        "fontSize",
        "foreColor",
        "backColor",
        "cleanFormatting"
    ];

    // ============================================
    // FUNÇÕES AUXILIARES
    // ============================================

    /**
     * Obtém a instância do Kendo Editor se existir
     */
    function getEditorInstance() {
        const textarea = document.getElementById("rteDescricao");
        if (textarea) {
            return $(textarea).data("kendoEditor");
        }
        return null;
    }

    /**
     * Obtém o HTML do editor
     */
    function getHtml() {
        try {
            const editor = getEditorInstance();
            if (editor) {
                return editor.value() || "";
            }
            return pendingValue || "";
        } catch (error) {
            console.error("[KendoEditorHelper] Erro ao obter HTML:", error);
            return "";
        }
    }

    /**
     * Define o HTML do editor
     */
    function setHtml(html) {
        try {
            const editor = getEditorInstance();
            if (editor) {
                editor.value(html || "");
                console.log("[KendoEditorHelper] HTML definido no editor");
            } else {
                pendingValue = html;
                console.log("[KendoEditorHelper] HTML guardado para aplicar depois");
            }
        } catch (error) {
            console.error("[KendoEditorHelper] Erro ao definir HTML:", error);
        }
    }

    // ============================================
    // FUNÇÕES PRINCIPAIS
    // ============================================

    /**
     * Destrói COMPLETAMENTE o Kendo Editor e limpa o DOM
     */
    function destroyKendoEditor() {
        try {
            const textarea = document.getElementById("rteDescricao");
            if (!textarea) return;

            // Obter instância existente
            const editor = $(textarea).data("kendoEditor");
            
            if (editor) {
                // Destruir a instância
                editor.destroy();
                console.log("[KendoEditorHelper] Instância destruída");
            }

            // IMPORTANTE: Remover wrapper HTML criado pelo Kendo
            const wrapper = $(textarea).closest(".k-editor");
            if (wrapper.length) {
                // Mover o textarea para fora do wrapper antes de remover
                wrapper.before(textarea);
                wrapper.remove();
                console.log("[KendoEditorHelper] Wrapper removido");
            }

            // Limpar dados do jQuery
            $(textarea).removeData("kendoEditor");
            
            // Mostrar textarea original
            $(textarea).show().css({
                "height": "250px",
                "width": "100%"
            });

            // Limpar ej2_instances
            delete textarea.ej2_instances;

            pendingValue = null;
            
            console.log("[KendoEditorHelper] Editor destruído completamente");

        } catch (error) {
            console.error("[KendoEditorHelper] Erro ao destruir:", error);
        }
    }

    /**
     * Inicializa o Kendo Editor (apenas se não existir)
     */
    function initKendoEditor() {
        try {
            const textarea = document.getElementById("rteDescricao");
            if (!textarea) {
                console.warn("[KendoEditorHelper] Textarea #rteDescricao não encontrado");
                return null;
            }

            // Se já existe instância, apenas aplicar valor pendente
            let editor = $(textarea).data("kendoEditor");
            if (editor) {
                console.log("[KendoEditorHelper] Instância já existe, reutilizando");
                if (pendingValue !== null) {
                    editor.value(pendingValue);
                    pendingValue = null;
                    console.log("[KendoEditorHelper] Valor pendente aplicado");
                }
                return editor;
            }

            // Verificar se há wrapper órfão (de inicialização anterior mal destruída)
            const orphanWrapper = $(textarea).closest(".k-editor");
            if (orphanWrapper.length) {
                console.warn("[KendoEditorHelper] Wrapper órfão encontrado, limpando...");
                orphanWrapper.before(textarea);
                orphanWrapper.remove();
                $(textarea).removeData("kendoEditor");
            }

            // Criar novo editor
            $(textarea).kendoEditor({
                tools: TOOLS,
                resizable: {
                    content: true,
                    toolbar: false
                }
            });

            editor = $(textarea).data("kendoEditor");

            // Aplicar valor pendente
            if (pendingValue !== null && editor) {
                editor.value(pendingValue);
                pendingValue = null;
                console.log("[KendoEditorHelper] Valor pendente aplicado");
            }

            // Criar camada de compatibilidade
            createCompatibilityLayer(textarea);

            console.log("[KendoEditorHelper] Editor inicializado com sucesso");
            return editor;

        } catch (error) {
            console.error("[KendoEditorHelper] Erro ao inicializar:", error);
            return null;
        }
    }

    /**
     * Limpa o conteúdo
     */
    function clearContent() {
        pendingValue = null;
        setHtml("");
    }

    /**
     * Refresh do editor
     */
    function refreshEditor() {
        try {
            const editor = getEditorInstance();
            if (editor) {
                editor.refresh();
            }
        } catch (error) {
            // Ignorar
        }
    }

    // ============================================
    // CAMADA DE COMPATIBILIDADE SYNCFUSION
    // ============================================

    function createCompatibilityLayer(textarea) {
        if (!textarea) return;
        if (textarea.ej2_instances) return; // Já existe

        const compatObj = {
            get value() {
                return getHtml();
            },
            set value(val) {
                setHtml(val);
            },
            getHtml: function () {
                return getHtml();
            },
            getValue: function () {
                return getHtml();
            },
            dataBind: function () {
                refreshEditor();
            },
            refresh: function () {
                refreshEditor();
            },
            enabled: true
        };

        textarea.ej2_instances = [compatObj];
    }

    /**
     * Cria camada de compatibilidade inicial (antes do editor existir)
     */
    function createInitialCompatibilityLayer() {
        const textarea = document.getElementById("rteDescricao");
        if (textarea && !textarea.ej2_instances) {
            createCompatibilityLayer(textarea);
            console.log("[KendoEditorHelper] Camada de compatibilidade inicial criada");
        }
    }

    // ============================================
    // INTEGRAÇÃO COM MODAL
    // ============================================

    function setupModalIntegration() {
        const modal = document.getElementById("modalViagens");
        if (!modal) {
            console.warn("[KendoEditorHelper] Modal #modalViagens não encontrado");
            return;
        }

        // Remover TODOS os listeners anteriores
        $(modal).off('shown.bs.modal.kendoEditor hidden.bs.modal.kendoEditor');

        // Quando modal abrir
        $(modal).on('shown.bs.modal.kendoEditor', function () {
            setTimeout(function () {
                initKendoEditor();
            }, 100);
        });

        // Quando modal fechar - destruir COMPLETAMENTE
        $(modal).on('hidden.bs.modal.kendoEditor', function () {
            destroyKendoEditor();
            // Recriar camada de compatibilidade para próxima abertura
            setTimeout(createInitialCompatibilityLayer, 50);
        });

        console.log("[KendoEditorHelper] Integração com modal configurada");
    }

    // ============================================
    // SOBRESCREVER refreshComponenteSafe
    // ============================================

    const originalRefresh = window.refreshComponenteSafe;

    window.refreshComponenteSafe = function (componentId) {
        if (componentId === "rteDescricao") {
            refreshEditor();
            return;
        }
        if (typeof originalRefresh === 'function') {
            originalRefresh(componentId);
        }
    };

    // ============================================
    // API PÚBLICA
    // ============================================

    window.KendoEditorHelper = {
        init: initKendoEditor,
        destroy: destroyKendoEditor,
        getHtml: getHtml,
        setHtml: setHtml,
        clear: clearContent,
        refresh: refreshEditor,
        setupModalIntegration: setupModalIntegration,

        get instance() {
            return getEditorInstance();
        },

        get isInitialized() {
            return getEditorInstance() !== null;
        }
    };

    // ============================================
    // INICIALIZAÇÃO
    // ============================================

    $(document).ready(function () {
        createInitialCompatibilityLayer();
        setupModalIntegration();
    });

    console.log("[KendoEditorHelper] Módulo carregado v4");

})(window);
