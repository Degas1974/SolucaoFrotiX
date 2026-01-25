/*
    ═══════════════════════════════════════════════════════════════════════════════
    📄 DOCUMENTAÇÃO COMPLETA DISPONÍVEL
    ═══════════════════════════════════════════════════════════════════════════════

    📍 Localização: Documentacao/JavaScript/frotix.js.md
    📅 Última Atualização: 16/01/2026 20:30
    📋 Versão: 2.2

    Este arquivo contém utilitários globais, sistemas de UI (spinner, ripple, loading),
    funções de formatação e validação global anti-espaços iniciais.

    NOVO em 2.2: Validação keydown que BLOQUEIA espaço antes de ser digitado
    (inspirada na técnica do campo email do modal de requisitante).

    Técnica anterior (input + trimStart) NÃO funcionava com Syncfusion.
    Nova técnica (keydown + preventDefault) funciona em TODOS os inputs nativos.

    Para entender completamente a funcionalidade, consulte a documentação acima.
    ═══════════════════════════════════════════════════════════════════════════════
*/

/**
 * Corta as bordas transparentes de uma imagem PNG e redimensiona para largura e altura desejadas
 * param {HTMLImageElement} img - Elemento de imagem já carregado
 * param {number} targetWidth - Largura final desejada após o trim e resize
 * param {number} targetHeight - Altura final desejada após o trim e resize
 * returns {HTMLCanvasElement} - Canvas com imagem cortada e redimensionada
 */

function trimTransparentPNG(img, targetWidth, targetHeight)
{
    const tempCanvas = document.createElement('canvas');
    const ctx = tempCanvas.getContext('2d');

    tempCanvas.width = img.width;
    tempCanvas.height = img.height;
    ctx.drawImage(img, 0, 0);

    const imageData = ctx.getImageData(0, 0, tempCanvas.width, tempCanvas.height);
    const pixels = imageData.data;
    const w = imageData.width;
    const h = imageData.height;

    let top = null, bottom = null, left = null, right = null;

    for (let y = 0; y < h; y++)
    {
        for (let x = 0; x < w; x++)
        {
            const alpha = pixels[(y * w + x) * 4 + 3];
            if (alpha > 0)
            {
                if (top === null) top = y;
                bottom = y;
                if (left === null || x < left) left = x;
                if (right === null || x > right) right = x;
            }
        }
    }

    const trimmedWidth = right - left + 1;
    const trimmedHeight = bottom - top + 1;
    const trimmedData = ctx.getImageData(left, top, trimmedWidth, trimmedHeight);

    const resultCanvas = document.createElement('canvas');
    resultCanvas.width = targetWidth;
    resultCanvas.height = targetHeight;
    const resultCtx = resultCanvas.getContext('2d');

    // Criar canvas temporário com a imagem cortada
    const trimmedCanvas = document.createElement('canvas');
    trimmedCanvas.width = trimmedWidth;
    trimmedCanvas.height = trimmedHeight;
    trimmedCanvas.getContext('2d').putImageData(trimmedData, 0, 0);

    // Redimensionar para tamanho final desejado
    resultCtx.drawImage(trimmedCanvas, 0, 0, targetWidth, targetHeight);

    return resultCanvas;
}

// Módulo global para controlar o Spinner - Padrão FrotiX com Logo Pulsando
(function ()
{
    const KEY = 'ftx:spin:next';

    // Core com padrão FrotiX (logo pulsando + barra de progresso)
    window.FtxSpin = {
        _el: null,
        show(msg)
        {
            if (this._el) { this.setMsg(msg); this._el.style.display = 'flex'; return; }
            const ov = document.createElement('div');
            ov.className = 'ftx-spin-overlay';
            ov.innerHTML = `
        <div class="ftx-spin-box" role="status" aria-live="assertive" style="text-align: center; min-width: 300px;">
          <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
          <div class="ftx-loading-bar"></div>
          <div class="ftx-loading-text ftx-spin-msg">${msg || 'Carregando…'}</div>
          <div class="ftx-loading-subtext">Por favor, aguarde...</div>
        </div>`;
            document.body.appendChild(ov);
            this._el = ov;
        },
        hide() { if (this._el) this._el.style.display = 'none'; },
        setMsg(msg)
        {
            const t = this._el && this._el.querySelector('.ftx-spin-msg');
            if (t && msg) t.textContent = msg;
        }
    };

    // 1) Ao clicar em qualquer <a data-ftx-spin>, mostra e marca intenção para a próxima página
    document.addEventListener('click', function (ev)
    {
        const a = ev.target.closest && ev.target.closest('a[data-ftx-spin]');
        if (!a) return;
        const msg = a.getAttribute('data-ftx-spin') || 'Carregando…';
        try { sessionStorage.setItem(KEY, msg); } catch (e) { }
        // não impedir a navegação — apenas mostra já
        FtxSpin.show(msg);
    }, true);

    // 2) Se chegamos a uma nova página com intenção pendente, reabrir o spinner o quanto antes
    (function autoOpenOnLoad()
    {
        let msg = null;
        try { msg = sessionStorage.getItem(KEY); } catch (e) { }
        if (!msg) return;
        try { sessionStorage.removeItem(KEY); } catch (e) { }
        // Se body ainda não existe, aguarde o DOM mínimo
        if (!document.body)
        {
            document.addEventListener('DOMContentLoaded', function () { FtxSpin.show(msg); }, { once: true });
        } else
        {
            FtxSpin.show(msg);
        }
    })();
})();

/*!
* Global Busy Submit v1.0
* - Aplica spinner + trava de duplo clique em botões de submit
* - Por padrío, habilita em TODOS os forms (opt-out com data-auto-spinner="off")
*/
(function ()
{
    "use strict";

    var SUBMIT_SELECTOR = "button[type=submit], input[type=submit]";

    function isFormEnabled(form)
    {
        // Por padrío, ligado. Desligue com data-auto-spinner="off"
        return (form.dataset.autoSpinner || "on").toLowerCase() !== "off";
    }

    function getSubmitter(evt, form)
    {
        if (evt && evt.submitter) return evt.submitter; // padrío moderno
        if (form._lastClickedSubmit && form.contains(form._lastClickedSubmit)) return form._lastClickedSubmit;
        // botão padrío por atributo
        var explicitDefault = form.querySelector("[data-default-submit]");
        if (explicitDefault) return explicitDefault;
        // primeiro submit do form
        return form.querySelector(SUBMIT_SELECTOR);
    }

    function htmlEscape(s)
    {
        return String(s).replace(/[&<>"']/g, function (ch)
        {
            return ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" })[ch];
        });
    }

    function lockButton(btn)
    {
        if (!btn || btn.dataset.busy === "true" || btn.hasAttribute("data-no-busy")) return false;

        // manter largura para evitar "pulo" opcional
        if (btn.dataset.busyKeepWidth === "true")
        {
            var rect = btn.getBoundingClientRect();
            btn.style.minWidth = rect.width + "px";
        }

        btn.dataset.busy = "true";
        btn.setAttribute("disabled", "disabled");
        btn.setAttribute("aria-busy", "true");

        var text = btn.dataset.spinnerText || "Salvando...";
        var iconClass = btn.dataset.spinnerIcon || "fa-solid fa-spinner fa-spin me-2";

        if (btn.tagName === "BUTTON")
        {
            if (!btn.dataset.originalHtml) btn.dataset.originalHtml = btn.innerHTML;
            // troca conteúdo: ícone + texto
            btn.innerHTML = '<i class="' + htmlEscape(iconClass) + '"></i>' + htmlEscape(text);
        } else if (btn.tagName === "INPUT")
        {
            // para <input type="submit"> não há innerHTML; troca apenas o value
            if (!btn.dataset.originalValue) btn.dataset.originalValue = btn.value;
            btn.value = text;
            // dica: prefira <button> se quiser ícone
        }
        return true;
    }

    function maybeValid(form)
    {
        // 1) HTML5
        if (typeof form.checkValidity === "function" && !form.checkValidity()) return false;

        // 2) jQuery Validate (se presente)
        var $ = window.jQuery;
        if ($ && typeof $.fn.valid === "function")
        {
            var $form = $(form);
            if ($form.data("validator") && !$form.valid()) return false;
        }
        return true;
    }

    function onReady(fn)
    {
        if (document.readyState === "loading")
        {
            document.addEventListener("DOMContentLoaded", fn);
        } else
        {
            fn();
        }
    }

    onReady(function ()
    {
        // Rastreia o último botão submit clicado (cobre Enter/submitter ausente)
        document.addEventListener("click", function (ev)
        {
            var target = ev.target && ev.target.closest ? ev.target.closest(SUBMIT_SELECTOR) : null;
            if (!target) return;
            var form = target.form;
            if (!form || !isFormEnabled(form)) return;
            form._lastClickedSubmit = target;
        }, true); // capture = true para pegar mesmo com delegação em libs

        // Handler global de submit (não impede o envio)
        document.addEventListener("submit", function (ev)
        {
            var form = ev.target;
            if (!(form instanceof HTMLFormElement)) return;
            if (!isFormEnabled(form)) return;

            // só mostra spinner se o form estiver realmente válido
            if (!maybeValid(form)) return;

            var submitter = getSubmitter(ev, form);
            if (submitter) lockButton(submitter);
            // Não chamar preventDefault(); deixamos o post seguir normalmente
        }, false);
    });
})();

function formatarDataBR(raw)
{
    const s = (raw ?? '').toString().trim();
    if (!s) return '';

    const m = moment(s,
        [
            moment.ISO_8601,           // 2025-09-18T10:32:00[Z]
            "DD/MM/YYYY",
            "D/M/YYYY",
            "DD/MM/YYYY HH:mm",
            "D/M/YYYY H:mm",
            "YYYY-MM-DD",
            "x"                        // unix ms
        ],
        true                         // strict
    );

    return m.isValid() ? m.format("DD/MM/YYYY") : '';
}

function formatarHora(raw, preferVazioSeSemHora = false)
{
    const s = (raw ?? '').toString().trim();
    if (!s) return '';

    // extrai ticks do formato .NET /Date(1695004800000)/
    const ticks = +((s.match(/\d+/) || [])[0]);
    const candidato = s.startsWith('/Date(') ? ticks : s;

    const m = moment(candidato, [
        moment.ISO_8601,            // 2025-09-18T10:32:00Z / 2025-09-18T10:32
        "DD/MM/YYYY HH:mm:ss",
        "DD/MM/YYYY H:mm:ss",
        "DD/MM/YYYY HH:mm",
        "DD/MM/YYYY H:mm",
        "YYYY-MM-DD HH:mm:ss",
        "YYYY-MM-DD HH:mm",
        "YYYY-MM-DD",               // se vier só a data
        "x"                         // unix ms
    ], true); // strict

    if (!m.isValid()) return '';

    // opcional: se veio só data e você quer vazio em vez de 00:00
    const temHora = /[T\s]\d{1,2}:\d{2}/.test(s);
    if (preferVazioSeSemHora && !temHora) return '';

    return m.format("HH:mm");
}

// ============================================================================
// NOTA: Tooltips Syncfusion são gerenciados exclusivamente por:
// wwwroot/js/syncfusion_tooltips.js (carregado via _ScriptsBasePlugins.cshtml)
// NÃO DUPLICAR a inicialização de ejTooltip aqui.
// ============================================================================


/**
 * Remove acentos e caracteres inválidos para nomes de arquivo
 * Substitui espaços por underscore
 */
function tiraAcento(texto)
{
    if (!texto || typeof texto !== 'string') return '';

    try
    {
        let resultado = texto
            // Normaliza e remove acentos
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')

            // Caracteres especiais comuns
            .replace(/[àáâãäåæ]/gi, 'a')
            .replace(/[èéêë]/gi, 'e')
            .replace(/[ìíîï]/gi, 'i')
            .replace(/[òóôõöø]/gi, 'o')
            .replace(/[ùúûü]/gi, 'u')
            .replace(/[ýÿ]/gi, 'y')
            .replace(/[ñ]/gi, 'n')
            .replace(/[ç]/gi, 'c')
            .replace(/[ß]/g, 'ss')
            .replace(/[œ]/gi, 'oe')
            .replace(/[æ]/gi, 'ae')
            .replace(/[ð]/gi, 'd')
            .replace(/[þ]/gi, 'th')

            // Remove caracteres inválidos para nomes de arquivo
            // Windows: < > : " / \ | ? *
            // Também remove caracteres de controle
            .replace(/[<>:"\/\\|?*\x00-\x1F\x7F]/g, '')

            // Remove caracteres especiais, mantendo apenas alfanuméricos, underscore, hífen e ponto
            .replace(/[^\w\s.\-]/g, '')

            // Substitui espaços por underscore
            .replace(/\s+/g, '_')

            // Remove múltiplos underscores/hífens/pontos consecutivos
            .replace(/_{2,}/g, '_')
            .replace(/-{2,}/g, '-')
            .replace(/\.{2,}/g, '.')

            // Remove underscore/hífen no início e fim
            .replace(/^[_\-]+|[_\-]+$/g, '');

        // Limita tamanho (255 caracteres)
        return resultado.length > 255 ? resultado.substring(0, 255) : resultado;

    } catch (error)
    {
        console.error('Erro em tiraAcento:', error);
        return '';
    }
}

// Exemplos:
// tiraAcento("Açúcar & Café.pdf")        → "Acucar_Cafe.pdf"
// tiraAcento("São Paulo/Rio")            → "Sao_PauloRio"
// tiraAcento("Relatório 2024: análise")  → "Relatorio_2024_analise"

/* ================================================================
   EFEITO RIPPLE - PADRÃO FROTIX
   Adicionar ao final do frotix.js
   ================================================================ */

/**
 * Cria o efeito ripple no ponto exato do clique
 * @param {MouseEvent} event - Evento de clique
 * @param {HTMLElement} element - Elemento que receberá o ripple
 */
function createRipple(event, element)
{
    try
    {
        // Não criar ripple em elementos desabilitados
        if (element.disabled || element.classList.contains('disabled') || element.getAttribute('aria-disabled') === 'true')
        {
            return;
        }

        // Não criar ripple se o elemento tem a classe no-ripple
        if (element.classList.contains('no-ripple'))
        {
            return;
        }

        // Remove ripples anteriores para evitar acúmulo
        const existingRipples = element.querySelectorAll('.ftx-ripple-circle');
        existingRipples.forEach(ripple =>
        {
            if (ripple.dataset.removing !== 'true')
            {
                ripple.remove();
            }
        });

        // Calcula posição do clique relativa ao elemento
        const rect = element.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;

        // Verifica se é botão laranja (ripple externo maior)
        const isOrangeButton = element.classList.contains('btn-fundo-laranja') ||
                               element.classList.contains('btn-header-orange');

        // Tamanho: 1.0x para todos os botões
        const multiplier = 1.0;
        const size = Math.max(rect.width, rect.height) * multiplier;
        const duration = isOrangeButton ? 900 : 600;

        // Cria o elemento do ripple com estilos inline (garante funcionamento)
        const ripple = document.createElement('span');
        ripple.className = 'ftx-ripple-circle';

        // Gradiente mais intenso para botões laranja
        const gradient = isOrangeButton
            ? 'radial-gradient(circle, rgba(255,255,255,0.95) 0%, rgba(255,255,255,0.6) 30%, rgba(255,255,255,0.3) 50%, transparent 70%)'
            : 'radial-gradient(circle, rgba(255,255,255,0.5) 0%, rgba(255,255,255,0.2) 40%, transparent 70%)';

        ripple.style.cssText = `
            position: absolute;
            border-radius: 50%;
            pointer-events: none;
            transform: scale(0);
            animation: ftxRippleAnim ${duration}ms ease-out forwards;
            background: ${gradient};
            width: ${size}px;
            height: ${size}px;
            left: ${x}px;
            top: ${y}px;
            margin-left: -${size / 2}px;
            margin-top: -${size / 2}px;
            z-index: 9999;
        `;

        // Adiciona o ripple ao elemento
        element.appendChild(ripple);

        // Remove o ripple após a animação
        setTimeout(() =>
        {
            ripple.dataset.removing = 'true';
            ripple.remove();
        }, duration);

    } catch (error)
    {
        console.warn('Erro ao criar ripple:', error);
    }
}

/**
 * Inicializa o sistema de ripple com delegação de eventos
 * Funciona automaticamente com elementos dinâmicos (DataTables, modais, etc.)
 */
(function initRippleSystem()
{
    'use strict';

    // Seletor de elementos que receberão ripple automaticamente
    const RIPPLE_SELECTOR = [
        '.btn',
        '[class*="btn-"]',
        'button[type="button"]',
        'button[type="submit"]',
        '.btn-icon-28',
        '.btn-acao-ftx',
        '.ftx-ripple'
    ].join(',');

    // Handler do clique com delegação
    function handleRippleClick(event)
    {
        // Encontra o elemento mais próximo que deve ter ripple
        const target = event.target.closest(RIPPLE_SELECTOR);

        if (target)
        {
            createRipple(event, target);
        }
    }

    // Registra o listener no document para capturar todos os cliques
    // Usa capture:true para garantir que pegamos o evento antes de qualquer stopPropagation
    document.addEventListener('click', handleRippleClick, true);

    // Expõe a função globalmente para uso manual se necessário
    window.createRipple = createRipple;

    // Log de inicialização (pode remover em produção)
    // console.log('✨ FrotiX Ripple System initialized');
})();

/**
 * Função auxiliar para adicionar ripple manualmente a um elemento
 * Útil para elementos criados dinamicamente que não correspondem ao seletor padrão
 * 
 * Exemplo de uso:
 * addRippleToElement(document.getElementById('meuBotaoCustomizado'));
 * 
 * @param {HTMLElement} element - Elemento que receberá o ripple
 * @param {string} variant - 'light' (padrão), 'dark', 'subtle' ou 'intense'
 */
function addRippleToElement(element, variant = 'light')
{
    if (!element) return;

    // Adiciona as classes necessárias
    element.classList.add('ftx-ripple');

    if (variant === 'dark')
    {
        element.classList.add('ftx-ripple-dark');
    }
    else if (variant === 'subtle')
    {
        element.classList.add('ftx-ripple-subtle');
    }
    else if (variant === 'intense')
    {
        element.classList.add('ftx-ripple-intense');
    }
    else
    {
        element.classList.add('ftx-ripple-light');
    }

    // Garante que o elemento tem position:relative e overflow:hidden
    const computedStyle = window.getComputedStyle(element);
    if (computedStyle.position === 'static')
    {
        element.style.position = 'relative';
    }
    element.style.overflow = 'hidden';
}

/**
 * Remove o efeito ripple de um elemento
 * @param {HTMLElement} element - Elemento a ter o ripple removido
 */
function removeRippleFromElement(element)
{
    if (!element) return;

    element.classList.remove('ftx-ripple', 'ftx-ripple-light', 'ftx-ripple-dark', 'ftx-ripple-subtle', 'ftx-ripple-intense');

    // Remove qualquer ripple em andamento
    const ripples = element.querySelectorAll('.ftx-ripple-circle');
    ripples.forEach(r => r.remove());
}

// Expõe funções auxiliares globalmente
window.addRippleToElement = addRippleToElement;
window.removeRippleFromElement = removeRippleFromElement;

/* ================================================================
   SISTEMA DATA-FTX-LOADING - SPINNER EM BOTÕES/LINKS AO CLICAR
   Padrão FrotiX: Adiciona spinner automático em elementos com data-ftx-loading
   ================================================================ */

(function initFtxLoadingSystem() {
    'use strict';

    // Configuração do sistema
    const FTX_LOADING_CONFIG = {
        spinnerClass: 'fa-duotone fa-spinner-third fa-spin',
        loadingClass: 'ftx-btn-loading',
        timeout: 30000, // 30 segundos para timeout
        disableOnClick: true
    };

    // Seletor de elementos que terão loading automático
    const LOADING_SELECTOR = '[data-ftx-loading]';

    /**
     * Aplica o estado de loading em um elemento
     * @param {HTMLElement} element - Botão ou link
     */
    function applyLoading(element) {
        if (!element || element.classList.contains(FTX_LOADING_CONFIG.loadingClass)) {
            return false;
        }

        // Salva estado original
        const icon = element.querySelector('i[class*="fa-"]');
        if (icon) {
            element.dataset.ftxOriginalIcon = icon.className;
            icon.className = FTX_LOADING_CONFIG.spinnerClass;
        } else {
            // Se não tem ícone, adiciona um spinner no início
            const spinner = document.createElement('i');
            spinner.className = FTX_LOADING_CONFIG.spinnerClass + ' me-1';
            spinner.dataset.ftxTempSpinner = 'true';
            element.insertBefore(spinner, element.firstChild);
        }

        // Salva largura para evitar "pulo"
        const rect = element.getBoundingClientRect();
        element.style.minWidth = rect.width + 'px';

        // Marca como loading
        element.classList.add(FTX_LOADING_CONFIG.loadingClass);

        // Desabilita para evitar duplo clique
        if (FTX_LOADING_CONFIG.disableOnClick) {
            if (element.tagName === 'BUTTON' || element.tagName === 'INPUT') {
                element.disabled = true;
            }
            element.style.pointerEvents = 'none';
        }

        // Timeout para restaurar (caso a página não navegue)
        element._ftxLoadingTimeout = setTimeout(() => {
            resetLoading(element);
        }, FTX_LOADING_CONFIG.timeout);

        return true;
    }

    /**
     * Remove o estado de loading de um elemento
     * @param {HTMLElement} element - Botão ou link
     */
    function resetLoading(element) {
        if (!element || !element.classList.contains(FTX_LOADING_CONFIG.loadingClass)) {
            return;
        }

        // Limpa timeout
        if (element._ftxLoadingTimeout) {
            clearTimeout(element._ftxLoadingTimeout);
            element._ftxLoadingTimeout = null;
        }

        // Remove spinner temporário
        const tempSpinner = element.querySelector('[data-ftx-temp-spinner]');
        if (tempSpinner) {
            tempSpinner.remove();
        }

        // Restaura ícone original
        const icon = element.querySelector('i[class*="fa-spin"]');
        if (icon && element.dataset.ftxOriginalIcon) {
            icon.className = element.dataset.ftxOriginalIcon;
            delete element.dataset.ftxOriginalIcon;
        }

        // Remove classe e estilos
        element.classList.remove(FTX_LOADING_CONFIG.loadingClass);
        element.style.minWidth = '';
        element.style.pointerEvents = '';

        // Reabilita
        if (element.tagName === 'BUTTON' || element.tagName === 'INPUT') {
            element.disabled = false;
        }
    }

    /**
     * Handler de clique com delegação
     */
    function handleLoadingClick(event) {
        const target = event.target.closest(LOADING_SELECTOR);
        
        if (!target) return;

        // Evita duplo clique
        if (target.classList.contains(FTX_LOADING_CONFIG.loadingClass)) {
            event.preventDefault();
            event.stopPropagation();
            return;
        }

        // Aplica loading
        applyLoading(target);
    }

    // Registra listener global com delegação
    document.addEventListener('click', handleLoadingClick, true);

    // Reseta loading quando página volta do cache (bfcache)
    window.addEventListener('pageshow', function(event) {
        if (event.persisted) {
            // Página veio do cache, reseta todos os loadings
            document.querySelectorAll('.' + FTX_LOADING_CONFIG.loadingClass).forEach(resetLoading);
        }
    });

    // Expõe funções globalmente
    window.FtxLoading = {
        apply: applyLoading,
        reset: resetLoading,
        resetAll: function() {
            document.querySelectorAll('.' + FTX_LOADING_CONFIG.loadingClass).forEach(resetLoading);
        }
    };

    // console.log('🔄 FrotiX Loading System initialized');
})();

/* ================================================================
   CSS INLINE PARA LOADING (caso não esteja no frotix.css)
   ================================================================ */
(function injectLoadingStyles() {
    const styleId = 'ftx-loading-styles';
    if (document.getElementById(styleId)) return;

    const css = `
        /* FrotiX Loading State */
        .ftx-btn-loading {
            position: relative;
            cursor: wait !important;
            opacity: 0.85;
        }
        
        .ftx-btn-loading i.fa-spin {
            animation: ftx-spin 0.8s linear infinite;
        }
        
        @keyframes ftx-spin {
            from { transform: rotate(0deg); }
            to { transform: rotate(360deg); }
        }
    `;

    const style = document.createElement('style');
    style.id = styleId;
    style.textContent = css;
    document.head.appendChild(style);
})();

/*!
 * FrotiX Nav Menu Click Enhancement v1.0
 * - Permite expandir/recolher submenus clicando no nome do item, não apenas na setinha
 * - Reforça o comportamento esperado pelo usuário
 */
(function() {
    'use strict';

    document.addEventListener('DOMContentLoaded', function() {
        try {
            // Aguarda o menu ser inicializado pelo SmartAdmin
            setTimeout(function() {
                initNavMenuClickEnhancement();
            }, 500);
        } catch (e) {
            console.error('Erro ao inicializar enhancement do nav-menu:', e);
        }
    });

    function initNavMenuClickEnhancement() {
        var navMenu = document.getElementById('js-nav-menu');
        if (!navMenu) return;

        // Adicionar evento de clique em todos os links do menu que têm submenus
        navMenu.querySelectorAll('li').forEach(function(li) {
            var submenu = li.querySelector(':scope > ul');
            var link = li.querySelector(':scope > a');

            // Se não tem submenu ou link, pular
            if (!submenu || !link) return;

            // Se o link já é uma navegação real (não #), não fazer nada
            var href = link.getAttribute('href');
            if (href && href !== '#' && href !== 'javascript:void(0)') return;

            // Adicionar cursor pointer para indicar que é clicável
            link.style.cursor = 'pointer';

            // Evento de clique no link inteiro (incluindo texto e ícone)
            link.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();

                // Toggle da visibilidade do submenu
                var isOpen = li.classList.contains('open');
                var collapseSign = link.querySelector('.collapse-sign');

                if (isOpen) {
                    // Fechar
                    $(submenu).slideUp(300, function() {
                        li.classList.remove('open');
                        link.setAttribute('aria-expanded', 'false');
                        if (collapseSign) {
                            collapseSign.innerHTML = '<em class="fal fa-angle-down"></em>';
                        }
                    });
                } else {
                    // Abrir
                    $(submenu).slideDown(300, function() {
                        li.classList.add('open');
                        link.setAttribute('aria-expanded', 'true');
                        if (collapseSign) {
                            collapseSign.innerHTML = '<em class="fal fa-angle-up"></em>';
                        }
                    });

                    // Modo accordion: fechar outros menus do mesmo nível
                    var parent = li.parentElement;
                    if (parent) {
                        parent.querySelectorAll(':scope > li.open').forEach(function(openLi) {
                            if (openLi !== li) {
                                var openSubmenu = openLi.querySelector(':scope > ul');
                                var openLink = openLi.querySelector(':scope > a');
                                var openCollapseSign = openLink ? openLink.querySelector('.collapse-sign') : null;

                                if (openSubmenu) {
                                    $(openSubmenu).slideUp(300, function() {
                                        openLi.classList.remove('open');
                                        if (openLink) openLink.setAttribute('aria-expanded', 'false');
                                        if (openCollapseSign) {
                                            openCollapseSign.innerHTML = '<em class="fal fa-angle-down"></em>';
                                        }
                                    });
                                }
                            }
                        });
                    }
                }
            });
        });

        console.log('FrotiX Nav Menu Click Enhancement inicializado');
    }
})();

// ===============================================================
// HELPER: Acesso ao ComboBox de Requisitantes (Telerik)
// ===============================================================
/**
 * Obtém instância do Telerik ComboBox de Requisitantes (Agenda Principal)
 * Função helper para compatibilidade após migração de Syncfusion para Telerik
 * @returns {kendo.ui.ComboBox|null} Instância do ComboBox ou null
 */
window.getRequisitanteCombo = function() {
    try {
        const input = $("input[name='lstRequisitante']");
        if (input.length === 0) {
            console.warn("ComboBox lstRequisitante não encontrado no DOM");
            return null;
        }

        const combo = input.data("kendoComboBox");
        if (!combo) {
            console.warn("Instância kendoComboBox não encontrada");
            return null;
        }

        return combo;
    } catch (error) {
        console.error("Erro ao acessar ComboBox de Requisitantes:", error);
        return null;
    }
};

/**
 * Obtém instância do Telerik ComboBox de Requisitantes (Modal Evento)
 * @returns {kendo.ui.ComboBox|null} Instância do ComboBox ou null
 */
window.getRequisitanteEventoCombo = function() {
    try {
        const input = $("input[name='lstRequisitanteEvento']");
        if (input.length === 0) {
            console.warn("ComboBox lstRequisitanteEvento não encontrado no DOM");
            return null;
        }

        const combo = input.data("kendoComboBox");
        if (!combo) {
            console.warn("Instância kendoComboBox não encontrada");
            return null;
        }

        return combo;
    } catch (error) {
        console.error("Erro ao acessar ComboBox de Requisitantes (Modal Evento):", error);
        return null;
    }
};

// ================================================================
// VALIDAÇÃO GLOBAL: BLOQUEAR ESPAÇOS NO INÍCIO DE INPUTS DE TEXTO
// ================================================================
/**
 * Valida e previne entrada de dados iniciando com espaço em TODOS os inputs de texto
 * Aplicado automaticamente em todo o sistema
 * Usa a MESMA TECNOLOGIA do campo de email do modal de requisitante
 *
 * TÉCNICA: event.keydown + preventDefault (BLOQUEIA A TECLA)
 * Diferente da abordagem anterior que usava .trimStart() após digitar,
 * esta técnica PREVINE que o espaço seja digitado em primeiro lugar.
 *
 * Adicionado em: 16/01/2026
 * Atualizado em: 16/01/2026 - Substituído por técnica keydown (mais eficaz)
 */

// Aplicação DIRETA via addEventListener (mesma técnica do campo email)
(function() {
    function aplicarValidacaoAntiEspacos() {
        const seletores = 'input[type="text"], input[type="search"], input[type="email"], textarea';
        const campos = document.querySelectorAll(seletores);
        let contadorAplicado = 0;

        campos.forEach(function(campo) {
            // Evitar aplicar múltiplas vezes
            if (campo.dataset.validacaoEspacosAplicada) return;
            campo.dataset.validacaoEspacosAplicada = 'true';

            // KEYPRESS: Bloqueia espaço quando campo vazio
            campo.addEventListener('keypress', function(e) {
                if ((!campo.value || campo.value.length === 0) && e.charCode === 32) {
                    e.preventDefault();
                    return false;
                }
            });

            // INPUT: Remove espaço se digitado no início
            campo.addEventListener('input', function(e) {
                if (campo.value && campo.value[0] === ' ') {
                    campo.value = campo.value.trimStart();
                }
            });

            // PASTE: Remove espaços colados
            campo.addEventListener('paste', function(e) {
                setTimeout(() => {
                    if (campo.value && campo.value[0] === ' ') {
                        campo.value = campo.value.trimStart();
                    }
                }, 10);
            });

            // BLUR: Trim completo
            campo.addEventListener('blur', function(e) {
                if (campo.value && campo.value !== campo.value.trim()) {
                    campo.value = campo.value.trim();
                }
            });

            contadorAplicado++;
        });

        if (contadorAplicado > 0) {
            console.log(`✅ Validação anti-espaços aplicada em ${contadorAplicado} campos`);
        }
    }

    // Aplicar quando DOM carregar
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', aplicarValidacaoAntiEspacos);
    } else {
        aplicarValidacaoAntiEspacos();
    }

    // Reaplicar a cada 2 segundos para campos dinâmicos
    setInterval(aplicarValidacaoAntiEspacos, 2000);
})();
