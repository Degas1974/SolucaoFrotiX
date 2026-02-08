/* ****************************************************************************************
 * ⚡ ARQUIVO: global-toast.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema GLOBAL de notificações toast nativo (JavaScript puro, sem
 *                   dependências externas). Toast com gradientes, ícones FontAwesome,
 *                   barra de progresso animada e auto-close configurável.
 * 📥 ENTRADAS     : Chamadas window.AppToast.show(estilo, mensagem, duracaoMs)
 * 📤 SAÍDAS       : Toast visual no canto superior direito (configurável), animações CSS
 * 🔗 CHAMADA POR  : SignalR (reconexão), código geral do sistema (notificações)
 * 🔄 CHAMA        : requestAnimationFrame, setTimeout, DOM manipulation
 * 📦 DEPENDÊNCIAS : FontAwesome Duotone (ícones), NENHUMA biblioteca JavaScript
 * 📝 OBSERVAÇÕES  : IIFE auto-executável, singleton pattern (window.AppToast), Object.freeze,
 *                   barra de progresso com scaleX, ESC fecha toast, click no toast fecha
 *
 * 📋 ÍNDICE DE FUNÇÕES (9 funções + 1 event listener):
 *
 * ┌─ FUNÇÕES DE CONTAINER E HELPERS ───────────────────────────────────────────┐
 * │ 1. getContainer()                                                           │
 * │    → Retorna ou cria #app-toast-container (position fixed, z-index 100000) │
 * │    → Append to document.body no primeiro uso                               │
 * │                                                                             │
 * │ 2. sanitizeText(text)                                                       │
 * │    → HTML escaping (&, <, >, ", ')                                         │
 * │    → Retorna string segura para innerHTML                                 │
 * │                                                                             │
 * │ 3. clearTimers()                                                            │
 * │    → Cancela closeTimer (setTimeout) e animationFrameId (requestAnimationFrame)│
 * │    → Reset variáveis para null                                             │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE TOAST (show/close) ────────────────────────────────────────────┐
 * │ 4. close()                                                                  │
 * │    → Fecha toast atual com animação slideOutRight (0.4s)                   │
 * │    → Remove do DOM após animação, limpa timers                             │
 * │    → Reset currentToast = null                                             │
 * │                                                                             │
 * │ 5. show(estilo, mensagem, duracaoMs)                                        │
 * │    → Mostra toast com estilo (Verde/Vermelho/Amarelo)                      │
 * │    → Cria elemento DOM, adiciona ao container, anima entrada (slideInRight)│
 * │    → Inicia animação da barra de progresso via animateProgress()           │
 * │    → Auto-close após duracaoMs (default 3000ms)                            │
 * │    → Click no toast fecha, ESC fecha                                       │
 * │                                                                             │
 * │ 6. animateProgress(currentTime) [helper interno de show()]                 │
 * │    → Callback recursivo de requestAnimationFrame                           │
 * │    → Atualiza scaleX da barra de progresso (1 → 0)                         │
 * │    → progress = 1 - (elapsed / timeout)                                    │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE POSICIONAMENTO E ESTILO ───────────────────────────────────────┐
 * │ 7. setPosition(x, y)                                                        │
 * │    → Define posição do toast container                                     │
 * │    → x: 'Right'|'Left'|'Center' (horizontal)                               │
 * │    → y: 'Top'|'Bottom' (vertical)                                          │
 * │    → Atualiza container.style.cssText                                      │
 * │                                                                             │
 * │ 8. addStyles()                                                              │
 * │    → Adiciona <style id="app-toast-styles"> ao document.head               │
 * │    → Keyframes: @slideInRight, @slideOutRight                              │
 * │    → Hover effect: shadow + translateY(-2px)                               │
 * │    → Executado uma vez na inicialização (guard: document.getElementById)   │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EVENT LISTENERS GLOBAIS ──────────────────────────────────────────────────┐
 * │ 9. document keydown listener                                                │
 * │    → ESC key (e.key === 'Escape') fecha toast                              │
 * │    → Chama close()                                                         │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * 🎨 ESTILOS DISPONÍVEIS (STYLE_MAP):
 * - "Verde": gradient #16a34a → #22c55e, ícone fa-thumbs-up
 * - "Vermelho": gradient #dc2626 → #ef4444, ícone fa-face-eyes-xmarks
 * - "Amarelo": gradient #d97706 → #f59e0b, ícone fa-circle-radiation (fallback)
 *
 * 🔄 ESTRUTURA DO TOAST DOM:
 * <div class="app-toast-item" style="background: gradient; animation: slideInRight...">
 *   <div style="display:flex; padding:16px 20px;">
 *     <i class="fa-duotone fa-solid fa-{icon}" style="48px..."></i>
 *     <div style="flex:1;">
 *       <div style="font-weight:700; color:#fff;">{mensagem}</div>
 *       <div style="height:4px; background:rgba(255,255,255,0.3);">
 *         <div id="{progressId}" style="width:100%; transform:scaleX(1);"></div>
 *       </div>
 *     </div>
 *   </div>
 * </div>
 *
 * 📌 API PÚBLICA (window.AppToast):
 * - AppToast.show(estilo, mensagem, duracaoMs)
 * - AppToast.close()
 * - AppToast.setPosition(x, y)
 * - AppToast.version (string)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Singleton: window.AppToast já existe → warn + return
 * - Object.freeze: API pública imutável
 * - Barra de progresso: requestAnimationFrame (60fps) + scaleX
 * - Auto-close: setTimeout(close, duracaoMs)
 * - Animações: CSS @keyframes (slideInRight 0.4s, slideOutRight 0.4s)
 * - Container: position fixed, z-index 100000, pointer-events none (toast tem auto)
 * - Toast: min-width 380px, max-width 480px, border-radius 12px, shadow
 * - Logs console: %c styled logs (verde/vermelho)
 *
 * 🔌 VERSÃO: 5.0-final
 * 📌 ÚLTIMA ATUALIZAÇÃO: 08/01/2026
 * 📌 DOCUMENTAÇÃO EXTERNA: Documentacao/JavaScript/global-toast.js.md
 **************************************************************************************** */

(function ()
{
    'use strict';

    // Previne múltiplas inicializações
    if (window.AppToast)
    {
        console.warn('⚠️ AppToast já foi inicializado');
        return;
    }

    // ============================================
    // CONFIGURAÇÕES DE ESTILO
    // ============================================
    const STYLE_MAP = {
        "Verde": {
            gradient: "linear-gradient(135deg, #16a34a 0%, #22c55e 100%)",
            icon: '<i class="fa-duotone fa-solid fa-thumbs-up" style="font-size:48px;color:#fff;width:48px;height:48px;display:flex;align-items:center;justify-content:center;flex-shrink:0;" aria-hidden="true"></i>'
        },
        "Vermelho": {
            gradient: "linear-gradient(135deg, #dc2626 0%, #ef4444 100%)",
            icon: '<i class="fa-duotone fa-solid fa-face-eyes-xmarks" style="font-size:48px;color:#fff;width:48px;height:48px;display:flex;align-items:center;justify-content:center;flex-shrink:0;" aria-hidden="true"></i>'
        },
        "Amarelo": {
            gradient: "linear-gradient(135deg, #d97706 0%, #f59e0b 100%)",
            icon: '<i class="fa-duotone fa-solid fa-circle-radiation" style="font-size:48px;color:#fff;width:48px;height:48px;display:flex;align-items:center;justify-content:center;flex-shrink:0;" aria-hidden="true"></i>'
        }
    };

    // ============================================
    // VARIÁVEIS PRIVADAS
    // ============================================
    let container = null;
    let currentToast = null;
    let closeTimer = null;
    let animationFrameId = null;

    // ============================================
    // FUNÇÕES AUXILIARES
    // ============================================

    function getContainer()
    {
        try
        {
            if (!container)
            {
                container = document.createElement('div');
                container.id = 'app-toast-container';
                container.style.cssText = `
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    z-index: 100000;
                    pointer-events: none;
                `;
                document.body.appendChild(container);
            }
            return container;
        }
        catch (erro)
        {
            console.error('Erro em getContainer:', erro);
            return null;
        }
    }

    function sanitizeText(text)
    {
        try
        {
            return String(text || '')
                .replace(/&/g, '&amp;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;')
                .replace(/"/g, '&quot;')
                .replace(/'/g, '&#39;');
        }
        catch (erro)
        {
            console.error('Erro em sanitizeText:', erro);
            return '';
        }
    }

    function clearTimers()
    {
        try
        {
            if (closeTimer)
            {
                clearTimeout(closeTimer);
                closeTimer = null;
            }

            if (animationFrameId)
            {
                cancelAnimationFrame(animationFrameId);
                animationFrameId = null;
            }
        }
        catch (erro)
        {
            console.error('Erro em clearTimers:', erro);
        }
    }

    function close()
    {
        try
        {
            clearTimers();

            if (currentToast)
            {
                currentToast.style.animation = 'slideOutRight 0.4s ease forwards';

                setTimeout(() =>
                {
                    try
                    {
                        if (currentToast && currentToast.parentNode)
                        {
                            currentToast.parentNode.removeChild(currentToast);
                        }
                        currentToast = null;
                    }
                    catch (erro)
                    {
                        console.error('Erro no setTimeout de close:', erro);
                    }
                }, 400);
            }
        }
        catch (erro)
        {
            console.error('Erro em close:', erro);
        }
    }

    function show(estilo, mensagem, duracaoMs)
    {
        try
        {
            // Fecha toast anterior
            close();

            const timeout = Number.isFinite(duracaoMs) ? Math.max(0, duracaoMs) : 3000;
            const style = STYLE_MAP[estilo] || STYLE_MAP["Amarelo"];
            const text = sanitizeText(mensagem);
            const progressId = 'app-toast-progress-' + Date.now() + '-' + Math.random().toString(36).substr(2, 9);

            console.log(`%c[AppToast] Mostrando toast "${estilo}" por ${timeout}ms`, 'color: #4caf50; font-weight: bold;');

            // Cria elemento do toast
            const toast = document.createElement('div');
            toast.className = 'app-toast-item';
            toast.style.cssText = `
                background: ${style.gradient};
                min-width: 380px;
                max-width: 480px;
                border-radius: 12px;
                box-shadow: 0 8px 24px rgba(0,0,0,0.15);
                overflow: hidden;
                margin-bottom: 12px;
                pointer-events: auto;
                animation: slideInRight 0.4s ease forwards;
                cursor: pointer;
            `;

            toast.innerHTML = `
                <div style="display:flex;align-items:center;gap:16px;padding:16px 20px;">
                    ${style.icon}
                    <div style="flex:1;display:flex;flex-direction:column;gap:8px;">
                        <div style="font-size:16px;font-weight:700;line-height:1.4;color:#fff;">${text}</div>
                        <div style="position:relative;width:100%;height:4px;background:rgba(255,255,255,0.3);border-radius:4px;overflow:hidden;">
                            <div id="${progressId}" style="height:100%;width:100%;background:#fff;transform-origin:left;transform:scaleX(1);transition:none;"></div>
                        </div>
                    </div>
                </div>
            `;

            // Adiciona ao container
            const cont = getContainer();
            cont.appendChild(toast);
            currentToast = toast;

            // Anima barra de progresso
            if (timeout > 0)
            {
                const progressBar = document.getElementById(progressId);
                const startTime = performance.now();

                function animateProgress(currentTime)
                {
                    try
                    {
                        const elapsed = currentTime - startTime;
                        const progress = Math.max(0, 1 - (elapsed / timeout));

                        if (progressBar)
                        {
                            progressBar.style.transform = `scaleX(${progress})`;
                        }

                        if (progress > 0)
                        {
                            animationFrameId = requestAnimationFrame(animateProgress);
                        }
                        else
                        {
                            animationFrameId = null;
                        }
                    }
                    catch (erro)
                    {
                        console.error('Erro em animateProgress:', erro);
                    }
                }

                animationFrameId = requestAnimationFrame(animateProgress);

                // Fecha automaticamente após o timeout
                closeTimer = setTimeout(() =>
                {
                    try
                    {
                        console.log(`%c[AppToast] Fechando toast após ${timeout}ms`, 'color: #f44336; font-weight: bold;');
                        close();
                    }
                    catch (erro)
                    {
                        console.error('Erro no setTimeout de show:', erro);
                    }
                }, timeout);
            }

            // Clique no toast fecha
            toast.addEventListener('click', () =>
            {
                try
                {
                    console.log('[AppToast] Toast fechado por clique');
                    close();
                }
                catch (erro)
                {
                    console.error('Erro no click handler do toast:', erro);
                }
            });
        }
        catch (erro)
        {
            console.error('Erro em show:', erro);
        }
    }

    function setPosition(x, y)
    {
        try
        {
            const cont = getContainer();

            const horizontalPositions = {
                'Right': 'right: 20px; left: auto;',
                'Left': 'left: 20px; right: auto;',
                'Center': 'left: 50%; transform: translateX(-50%);'
            };

            const verticalPositions = {
                'Top': 'top: 20px; bottom: auto;',
                'Bottom': 'bottom: 20px; top: auto;'
            };

            cont.style.cssText = `
                position: fixed;
                z-index: 100000;
                pointer-events: none;
                ${horizontalPositions[x] || horizontalPositions['Right']}
                ${verticalPositions[y] || verticalPositions['Top']}
            `;
        }
        catch (erro)
        {
            console.error('Erro em setPosition:', erro);
        }
    }

    // ============================================
    // ADICIONA ANIMAÇÕES CSS
    // ============================================

    function addStyles()
    {
        try
        {
            if (!document.getElementById('app-toast-styles'))
            {
                const style = document.createElement('style');
                style.id = 'app-toast-styles';
                style.textContent = `
                    @keyframes slideInRight {
                        from {
                            opacity: 0;
                            transform: translateX(100%);
                        }
                        to {
                            opacity: 1;
                            transform: translateX(0);
                        }
                    }

                    @keyframes slideOutRight {
                        from {
                            opacity: 1;
                            transform: translateX(0);
                        }
                        to {
                            opacity: 0;
                            transform: translateX(100%);
                        }
                    }

                    .app-toast-item:hover {
                        box-shadow: 0 12px 32px rgba(0,0,0,0.2) !important;
                        transform: translateY(-2px);
                        transition: all 0.2s ease;
                    }
                `;
                document.head.appendChild(style);
            }
        }
        catch (erro)
        {
            console.error('Erro em addStyles:', erro);
        }
    }

    // ============================================
    // EVENT LISTENERS
    // ============================================

    // ESC fecha o toast
    document.addEventListener('keydown', (e) =>
    {
        try
        {
            if (e.key === 'Escape')
            {
                close();
            }
        }
        catch (erro)
        {
            console.error('Erro no event listener keydown (ESC):', erro);
        }
    });

    // Inicializa estilos
    addStyles();

    // ============================================
    // EXPORTA API PÚBLICA
    // ============================================

    window.AppToast = Object.freeze({
        show: show,
        close: close,
        setPosition: setPosition,
        version: '5.0-final'
    });

    console.log('%c✓ AppToast v5.0-final carregado', 'color: #4caf50; font-weight: bold;');

})();
