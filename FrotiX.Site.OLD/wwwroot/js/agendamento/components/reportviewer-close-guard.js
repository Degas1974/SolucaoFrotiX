/* ****************************************************************************************
 * ⚡ ARQUIVO: reportviewer-close-guard.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Proteção contra fechamento acidental do modal Bootstrap enquanto Telerik
 *                   ReportViewer está renderizando. IIFE com 4 funções para interceptar
 *                   hide.bs.modal event, bloquear fechamento durante renderização (flag
 *                   isReportViewerLoading), agendar fechamento pendente (flag _pendingCloseAfterRender),
 *                   e patch no plugin jQuery telerik_ReportViewer para wrapar callbacks
 *                   (renderingBegin, renderingEnd, ready, error) que controlam flags.
 *                   Principais fluxos: renderingBegin → isReportViewerLoading=true →
 *                   hide.bs.modal interceptado → preventDefault + _pendingCloseAfterRender=true
 *                   → renderingEnd → isReportViewerLoading=false → se pending: setTimeout
 *                   closeModalViagens(). Usa Bootstrap 5/4 API, jQuery delegation, native
 *                   addEventListener capture phase.
 * 📥 ENTRADAS     : hide.bs.modal event (Bootstrap Modal), Telerik ReportViewer options
 *                   object (callbacks: renderingBegin/renderingEnd/ready/error), window
 *                   flags (isReportViewerLoading, _pendingCloseAfterRender)
 * 📤 SAÍDAS       : Void (side effects: event.preventDefault(), flags updates, modal.hide(),
 *                   console.log debug, Alerta.Alerta toast), patched $.fn.telerik_ReportViewer
 *                   (jQuery plugin wrapper), boolean return false em onAttemptClose
 * 🔗 CHAMADA POR  : Auto-init IIFE (DOMContentLoaded ou readyState ready), Bootstrap Modal
 *                   hide.bs.modal event (trigger ao tentar fechar), modal-viagem-novo.carregarRelatorioNoModal
 *                   (cria ReportViewer com options → patch intercepta)
 * 🔄 CHAMA        : Bootstrap 5 API (bootstrap.Modal.getOrCreateInstance().hide()),
 *                   jQuery API ($.fn.telerik_ReportViewer original, $('#modalViagens').modal('hide'),
 *                   $(document).on/off delegation), DOM API (getElementById, classList.remove,
 *                   setAttribute, addEventListener capture), setTimeout (100ms delay close),
 *                   Alerta.Alerta (toast warning "Relatório em processamento"), console.log/warn
 *                   (debug extensivo com emoji prefixes)
 * 📦 DEPENDÊNCIAS : Bootstrap 5 ou 4 (Modal API, hide.bs.modal event), jQuery ($.fn plugin
 *                   system, event delegation, .modal() method), Telerik Reporting jQuery
 *                   plugin ($.fn.telerik_ReportViewer, callbacks renderingBegin/renderingEnd/
 *                   ready/error), Alerta (window.Alerta.Alerta toast), DOM element
 *                   #modalViagens (Bootstrap Modal container), window flags (isReportViewerLoading,
 *                   _pendingCloseAfterRender como global state)
 * 📝 OBSERVAÇÕES  : IIFE pattern (Immediately Invoked Function Expression) com 'use strict'.
 *                   4 funções internas: closeModalViagens (programmatic close com 3 fallbacks),
 *                   patchReportViewerPlugin (wrapper do jQuery plugin), bindModalGuard
 *                   (event listeners), init (entry point). Auto-initialization via
 *                   DOMContentLoaded. 2 global flags: isReportViewerLoading (boolean,
 *                   controla bloqueio), _pendingCloseAfterRender (boolean, agenda close).
 *                   Patch $.fn.telerik_ReportViewer: preserva callback original com
 *                   originalRenderingBegin.apply(this, arguments), adiciona logic antes/depois.
 *                   Event listening: jQuery delegation (hide.bs.modal via $(document).on)
 *                   + native capture phase (addEventListener capture:true) para garantir
 *                   interceptação antes de outros listeners. Console.log com emoji prefixes:
 *                   🚀 (init), 🔧 (patch), 🎬 (render events), 🛑 (block), 🚪 (close),
 *                   ✅ (success), ❌ (error). Fallbacks: Bootstrap 5 → 4/jQuery → manual
 *                   (classList, setAttribute). setTimeout 100ms: delay para garantir que
 *                   renderingEnd completa antes de close. Alerta.Alerta: toast amarelo
 *                   "Aguarde renderização completa". $.fn._trv_patched flag: previne
 *                   double-patching.
 *
 * 📋 ÍNDICE DE FUNÇÕES (4 funções internas + 2 global flags + auto-init):
 *
 * ┌─ GLOBAL FLAGS ───────────────────────────────────────────────────────┐
 * │ window.isReportViewerLoading                                          │
 * │   → Boolean flag (controla se ReportViewer está renderizando)        │
 * │   → Setado true em renderingBegin, false em renderingEnd/ready/error │
 * │   → Usado em onAttemptClose para bloquear hide.bs.modal              │
 * │   → Inicializado: window.isReportViewerLoading === true ? true : false│
 * │                                                                        │
 * │ window._pendingCloseAfterRender                                       │
 * │   → Boolean flag (marca que houve tentativa de fechar bloqueada)     │
 * │   → Setado true quando isReportViewerLoading && hide.bs.modal        │
 * │   → Verificado em renderingEnd/ready: se true → closeModalViagens()  │
 * │   → Inicializado: window._pendingCloseAfterRender === true ? true : false│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES INTERNAS (4 funções no IIFE) ──────────────────────────────┐
 * │ 1. closeModalViagens()                                                │
 * │    → Fecha modal Bootstrap programaticamente com fallbacks           │
 * │    → returns void (side effect: modal hidden)                        │
 * │    → Fluxo:                                                           │
 * │      1. getElementById('modalViagens')                               │
 * │      2. console.log "Fechando modal programaticamente"               │
 * │      3. Try Bootstrap 5:                                             │
 * │         - if window.bootstrap && bootstrap.Modal.getOrCreateInstance:│
 * │           bootstrap.Modal.getOrCreateInstance(el).hide()             │
 * │      4. Fallback Bootstrap 4/jQuery:                                 │
 * │         - else if window.$ && $('#modalViagens').modal:              │
 * │           $('#modalViagens').modal('hide')                           │
 * │      5. Fallback manual:                                             │
 * │         - else: el.classList.remove('show'),                         │
 * │           el.setAttribute('aria-hidden', 'true')                     │
 * │      6. console.log "Modal fechado"                                  │
 * │      7. try-catch: console.warn erro                                 │
 * │    → Chamado por: renderingEnd/ready após _pendingCloseAfterRender  │
 * │                                                                       │
 * │ 2. patchReportViewerPlugin($)                                        │
 * │    → Wrapar jQuery plugin telerik_ReportViewer para interceptar      │
 * │      callbacks                                                       │
 * │    → param $: jQuery object                                          │
 * │    → returns void (side effect: $.fn.telerik_ReportViewer patched)   │
 * │    → Fluxo: (97 linhas)                                              │
 * │      1. Verificar: $ && $.fn && $.fn.telerik_ReportViewer &&        │
 * │         !$.fn._trv_patched                                           │
 * │      2. console.log "Aplicando patch no Telerik ReportViewer"        │
 * │      3. Salvar original = $.fn.telerik_ReportViewer                  │
 * │      4. $.fn.telerik_ReportViewer = function(options) {              │
 * │         a. Se options é object:                                      │
 * │            - Salvar callbacks originais (renderingBegin, renderingEnd,│
 * │              ready, error)                                           │
 * │            - options.renderingBegin = wrapped function:              │
 * │              * console.log "renderingBegin - BLOQUEANDO"             │
 * │              * isReportViewerLoading = true                          │
 * │              * _pendingCloseAfterRender = false                      │
 * │              * Call originalRenderingBegin.apply(this, arguments)    │
 * │            - options.renderingEnd = wrapped function:                │
 * │              * console.log "renderingEnd - DESBLOQUEANDO"            │
 * │              * isReportViewerLoading = false                         │
 * │              * Se _pendingCloseAfterRender: setTimeout(closeModalViagens, 100)│
 * │              * Call originalRenderingEnd.apply(this, arguments)      │
 * │            - options.ready = wrapped function (similar renderingEnd) │
 * │            - options.error = wrapped function:                       │
 * │              * console.log "error - DESBLOQUEANDO"                   │
 * │              * isReportViewerLoading = false                         │
 * │              * _pendingCloseAfterRender = false                      │
 * │              * Call originalError.apply(this, arguments)             │
 * │         b. try-catch: console.warn erro patch                        │
 * │         c. return original.apply(this, arguments) (call original)    │
 * │      }                                                                │
 * │      5. $.fn._trv_patched = true (flag para evitar double-patch)     │
 * │      6. console.log "Patch aplicado com sucesso"                     │
 * │    → Chamado por: init() se window.$ exists                          │
 * │                                                                       │
 * │ 3. bindModalGuard()                                                   │
 * │    → Registra event listeners para interceptar hide.bs.modal         │
 * │    → returns void (side effect: event listeners registered)          │
 * │    → Fluxo: (52 linhas)                                              │
 * │      1. Definir onAttemptClose(e):                                   │
 * │         a. try-catch:                                                │
 * │            - Se isReportViewerLoading:                               │
 * │              * console.log "Tentativa de fechar BLOQUEADA"           │
 * │              * e.preventDefault()                                    │
 * │              * e.stopImmediatePropagation()                          │
 * │              * _pendingCloseAfterRender = true                       │
 * │              * Alerta.Alerta("Relatório em processamento",           │
 * │                "Aguarde renderização...")                            │
 * │              * return false                                          │
 * │            - console.warn erro                                       │
 * │      2. Bind via jQuery delegation:                                  │
 * │         - $(document).off('hide.bs.modal', '#modalViagens', onAttemptClose)│
 * │         - $(document).on('hide.bs.modal', '#modalViagens', onAttemptClose)│
 * │         - console.log "Event listener jQuery registrado"             │
 * │      3. Bind nativo (capture phase):                                 │
 * │         - getElementById('modalViagens')                             │
 * │         - el.addEventListener('hide.bs.modal', onAttemptClose,       │
 * │           { capture: true })                                         │
 * │         - console.log "Event listener nativo registrado"             │
 * │      4. console.log "Modal guard ativo"                              │
 * │    → Chamado por: init()                                             │
 * │                                                                       │
 * │ 4. init()                                                             │
 * │    → Entry point de inicialização do guard                          │
 * │    → returns void (side effect: patch + bind)                       │
 * │    → Fluxo:                                                           │
 * │      1. try-catch:                                                   │
 * │         a. console.log "Inicializando proteção"                      │
 * │         b. Se window.$: patchReportViewerPlugin(window.$)            │
 * │         c. bindModalGuard()                                          │
 * │         d. console.log "Proteção ativa"                              │
 * │      2. console.warn erro                                            │
 * │    → Chamado por: auto-init (DOMContentLoaded ou readyState ready)  │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ AUTO-INITIALIZATION ────────────────────────────────────────────────┐
 * │ if (document.readyState === 'loading')                                │
 * │   document.addEventListener('DOMContentLoaded', init)                 │
 * │ else                                                                  │
 * │   init()                                                              │
 * │ → Garante que init() executa quando DOM está pronto                  │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO - FECHAMENTO BLOQUEADO E ADIADO:
 * 1. Usuário abre modal de relatório → modal-viagem-novo.carregarRelatorioNoModal()
 * 2. Criar ReportViewer: $.fn.telerik_ReportViewer({ renderingBegin: ..., renderingEnd: ... })
 * 3. patchReportViewerPlugin interceptou e wrapeou callbacks
 * 4. Telerik dispara renderingBegin callback
 * 5. Wrapped callback: isReportViewerLoading = true (BLOQUEIA), _pendingCloseAfterRender = false
 * 6. console.log "renderingBegin - BLOQUEANDO fechamento"
 * 7. Usuário tenta fechar modal (clica X ou btnFechar)
 * 8. Bootstrap dispara hide.bs.modal event
 * 9. bindModalGuard listener intercepta (capture phase + jQuery delegation)
 * 10. onAttemptClose verifica: isReportViewerLoading == true
 * 11. Bloquear: e.preventDefault() + e.stopImmediatePropagation()
 * 12. Agendar: _pendingCloseAfterRender = true
 * 13. Toast: Alerta.Alerta("Relatório em processamento", "Aguarde...")
 * 14. console.log "Tentativa de fechar BLOQUEADA"
 * 15. return false (cancela event)
 * 16. Modal permanece aberto, relatório continua renderizando
 * 17. Telerik completa renderização → dispara renderingEnd callback
 * 18. Wrapped callback: isReportViewerLoading = false (DESBLOQUEIA)
 * 19. Verificar: _pendingCloseAfterRender == true
 * 20. console.log "Executando fechamento pendente"
 * 21. setTimeout(() => closeModalViagens(), 100) (delay 100ms)
 * 22. closeModalViagens(): bootstrap.Modal.hide() ou $('#modalViagens').modal('hide')
 * 23. console.log "Modal fechado"
 * 24. Modal fecha automaticamente após renderização completa
 *
 * 📌 CALLBACKS TELERIK REPORTVIEWER (4 wrapeados):
 * - renderingBegin: dispara quando ReportViewer inicia renderização
 *   - Wrapeado para: isReportViewerLoading = true, _pendingCloseAfterRender = false
 * - renderingEnd: dispara quando ReportViewer completa renderização
 *   - Wrapeado para: isReportViewerLoading = false, se pending → closeModalViagens()
 * - ready: dispara quando ReportViewer está pronto (alternativa a renderingEnd)
 *   - Wrapeado para: mesma lógica que renderingEnd
 * - error: dispara quando ReportViewer encontra erro
 *   - Wrapeado para: isReportViewerLoading = false, _pendingCloseAfterRender = false (cancela pending)
 *
 * 📌 EVENT LISTENERS (dupla proteção):
 * - jQuery delegation: $(document).on('hide.bs.modal', '#modalViagens', onAttemptClose)
 *   - Funciona para eventos delegados, flexível
 * - Native capture: el.addEventListener('hide.bs.modal', onAttemptClose, { capture: true })
 *   - Capture phase: executa ANTES de bubble phase, garante interceptação precoce
 * - Ambos registrados para garantir que pelo menos um funcione
 *
 * 📌 FALLBACKS CLOSE (3 métodos tentados em ordem):
 * 1. Bootstrap 5: bootstrap.Modal.getOrCreateInstance(el).hide()
 *    - Método oficial Bootstrap 5, preferido
 * 2. Bootstrap 4/jQuery: $('#modalViagens').modal('hide')
 *    - Compatibilidade com Bootstrap 4 e jQuery
 * 3. Manual: el.classList.remove('show') + el.setAttribute('aria-hidden', 'true')
 *    - Fallback último caso (sem Bootstrap ou jQuery)
 *
 * 📌 TIMING (setTimeout 100ms):
 * - Necessário porque renderingEnd/ready podem disparar ligeiramente antes de cleanup completo
 * - 100ms garante que Telerik finalizou todos os processos antes de close
 * - Se fechar imediatamente: pode causar erros de estado inconsistente
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - IIFE pattern: (function() { ... })() para evitar poluir scope global
 * - 'use strict': modo estrito JavaScript (previne erros comuns)
 * - $.fn._trv_patched flag: previne double-patching se script carregado múltiplas vezes
 * - Console.log com emoji prefixes facilita debug visual no console
 * - onAttemptClose return false: garante cancelamento do event (redundante com preventDefault)
 * - e.stopImmediatePropagation(): previne outros listeners de executar
 * - Alerta.Alerta: toast amarelo (aviso), não bloqueia UI (não é modal)
 * - originalCallback.apply(this, arguments): preserva context e parâmetros do callback original
 * - Capture phase: fase mais precoce do event propagation (antes de bubble)
 * - window.$ check: garante que jQuery está carregado antes de patchear
 * - try-catch em todas as funções: erros não quebram a proteção
 * - flags initialized com ternary: window.flag === true ? true : false (garante boolean limpo)
 * - DOMContentLoaded vs readyState ready: compatibilidade com diferentes timing de script load
 *
 * 🔌 VERSÃO: 2.0 (refatorado após Lote 192, adiciona comprehensive header)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */

(function ()
{
    'use strict';

    // ================================================================
    // FLAGS GLOBAIS
    // ================================================================
    window.isReportViewerLoading = window.isReportViewerLoading === true ? true : false;
    window._pendingCloseAfterRender = window._pendingCloseAfterRender === true ? true : false;

    // ================================================================
    // FUNÇÃO: Fechar modal programaticamente
    // ================================================================
    function closeModalViagens()
    {
        try
        {
            const el = document.getElementById('modalViagens');
            if (!el) return;

            console.log('[CloseGuard] 🚪 Fechando modal programaticamente...');

            // Tentar Bootstrap 5
            if (window.bootstrap && bootstrap.Modal && bootstrap.Modal.getOrCreateInstance)
            {
                bootstrap.Modal.getOrCreateInstance(el).hide();
            }
            // Fallback para Bootstrap 4/jQuery
            else if (window.$ && $('#modalViagens').modal)
            {
                $('#modalViagens').modal('hide');
            }
            // Fallback manual
            else
            {
                el.classList.remove('show');
                el.setAttribute('aria-hidden', 'true');
            }

            console.log('[CloseGuard] ✅ Modal fechado');
        } catch (e)
        {
            console.warn('[CloseGuard] ❌ Erro ao fechar modal:', e);
        }
    }

    // ================================================================
    // PATCH: Plugin Telerik ReportViewer
    // ================================================================
    // Intercepta a criação do viewer para envolver os callbacks
    function patchReportViewerPlugin($)
    {
        if (!$ || !$.fn || !$.fn.telerik_ReportViewer || $.fn._trv_patched)
        {
            return;
        }

        console.log('[CloseGuard] 🔧 Aplicando patch no Telerik ReportViewer...');

        const original = $.fn.telerik_ReportViewer;

        $.fn.telerik_ReportViewer = function (options)
        {
            try
            {
                if (options && typeof options === 'object')
                {
                    // Guardar callbacks originais
                    const originalRenderingBegin = options.renderingBegin;
                    const originalRenderingEnd = options.renderingEnd;
                    const originalReady = options.ready;
                    const originalError = options.error;

                    // WRAP: renderingBegin
                    options.renderingBegin = function ()
                    {
                        console.log('[CloseGuard] 🎬 renderingBegin - BLOQUEANDO fechamento');
                        window.isReportViewerLoading = true;
                        window._pendingCloseAfterRender = false;

                        if (typeof originalRenderingBegin === 'function')
                        {
                            originalRenderingBegin.apply(this, arguments);
                        }
                    };

                    // WRAP: renderingEnd
                    options.renderingEnd = function ()
                    {
                        console.log('[CloseGuard] 🎬 renderingEnd - DESBLOQUEANDO');
                        window.isReportViewerLoading = false;

                        // Se havia tentativa de fechar pendente, fechar agora
                        if (window._pendingCloseAfterRender)
                        {
                            console.log('[CloseGuard] 🚪 Executando fechamento pendente...');
                            window._pendingCloseAfterRender = false;
                            setTimeout(() => closeModalViagens(), 100);
                        }

                        if (typeof originalRenderingEnd === 'function')
                        {
                            originalRenderingEnd.apply(this, arguments);
                        }
                    };

                    // WRAP: ready
                    options.ready = function ()
                    {
                        console.log('[CloseGuard] ✅ ready - DESBLOQUEANDO');
                        window.isReportViewerLoading = false;

                        // Se havia tentativa de fechar pendente, fechar agora
                        if (window._pendingCloseAfterRender)
                        {
                            console.log('[CloseGuard] 🚪 Executando fechamento pendente...');
                            window._pendingCloseAfterRender = false;
                            setTimeout(() => closeModalViagens(), 100);
                        }

                        if (typeof originalReady === 'function')
                        {
                            originalReady.apply(this, arguments);
                        }
                    };

                    // WRAP: error
                    options.error = function ()
                    {
                        console.log('[CloseGuard] ❌ error - DESBLOQUEANDO');
                        window.isReportViewerLoading = false;
                        window._pendingCloseAfterRender = false;

                        if (typeof originalError === 'function')
                        {
                            originalError.apply(this, arguments);
                        }
                    };
                }
            } catch (e)
            {
                console.warn('[CloseGuard] ❌ Erro ao aplicar patch:', e);
            }

            return original.apply(this, arguments);
        };

        $.fn._trv_patched = true;
        console.log('[CloseGuard] ✅ Patch aplicado com sucesso');
    }

    // ================================================================
    // INTERCEPTAÇÃO: Tentativas de fechar modal
    // ================================================================
    function bindModalGuard()
    {
        function onAttemptClose(e)
        {
            try
            {
                if (window.isReportViewerLoading)
                {
                    console.log('[CloseGuard] 🛑 Tentativa de fechar BLOQUEADA - relatório renderizando');

                    // PREVENIR fechamento
                    if (e && e.preventDefault) e.preventDefault();
                    if (e && e.stopImmediatePropagation) e.stopImmediatePropagation();

                    // Marcar que quer fechar
                    window._pendingCloseAfterRender = true;

                    // Mostrar alerta ao usuário
                    if (window.Alerta && typeof window.Alerta.Alerta === 'function')
                    {
                        window.Alerta.Alerta(
                            'Relatório em processamento',
                            'Aguarde a renderização completa da Ficha de Viagem. O modal fechará automaticamente ao terminar.'
                        );
                    }

                    return false;
                }
            } catch (err)
            {
                console.warn('[CloseGuard] ❌ Erro em onAttemptClose:', err);
            }
        }

        // Bind via jQuery (delegação)
        if (window.$ && $(document).on)
        {
            $(document).off('hide.bs.modal', '#modalViagens', onAttemptClose);
            $(document).on('hide.bs.modal', '#modalViagens', onAttemptClose);
            console.log('[CloseGuard] ✅ Event listener jQuery registrado');
        }

        // Bind nativo (capture phase)
        const el = document.getElementById('modalViagens');
        if (el && el.addEventListener)
        {
            el.addEventListener('hide.bs.modal', onAttemptClose, { capture: true });
            console.log('[CloseGuard] ✅ Event listener nativo registrado');
        }

        console.log('[CloseGuard] 🛡️ Modal guard ativo');
    }

    // ================================================================
    // INICIALIZAÇÃO
    // ================================================================
    function init()
    {
        try
        {
            console.log('[CloseGuard] 🚀 Inicializando proteção...');

            if (window.$)
            {
                patchReportViewerPlugin(window.$);
            }

            bindModalGuard();

            console.log('[CloseGuard] ✅ Proteção ativa');
        } catch (e)
        {
            console.warn('[CloseGuard] ❌ Erro na inicialização:', e);
        }
    }

    // Executar quando DOM estiver pronto
    if (document.readyState === 'loading')
    {
        document.addEventListener('DOMContentLoaded', init);
    } else
    {
        init();
    }
})();
