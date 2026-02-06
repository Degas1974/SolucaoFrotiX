// ====================================================================
// SUPRESSOR DE ERRO: Kendo e Syncfusion
// ====================================================================
// IMPORTANTE: Incluir ANTES de todos os outros scripts
// ====================================================================
// Suprime:
// - Kendo: erros "collapsible" e "toggle"
// - Syncfusion: erros "percentSign", "currencySign" (formatação antes do CLDR)
// ====================================================================

(function() {
    'use strict';

    // Guardar funções originais
    const originalError = console.error;
    const originalOnError = window.onerror;

    // Lista de erros conhecidos do Syncfusion relacionados a formatação
    const syncfusionFormatErrors = [
        'percentsign',
        'currencysign',
        'decimalseparator',
        'groupseparator',
        'format options',
        'type given must be invalid'
    ];

    // Função para verificar se é erro de formatação Syncfusion
    function isSyncfusionFormatError(msg) {
        const lowerMsg = msg.toLowerCase();
        return syncfusionFormatErrors.some(err => lowerMsg.includes(err));
    }

    // Sobrescrever console.error
    console.error = function(...args) {
        const msg = args.join(' ').toLowerCase();

        // Bloquear erros específicos do Kendo
        if (msg.includes('collapsible') ||
            (msg.includes('cannot read properties of undefined') && msg.includes('toggle'))) {
            console.warn('[SUPRIMIDO] Erro do Kendo ignorado:', ...args);
            return;
        }

        // Bloquear erros de formatação do Syncfusion
        if (isSyncfusionFormatError(msg)) {
            console.warn('[SUPRIMIDO] Erro de formatação Syncfusion ignorado:', ...args);
            return;
        }

        // Chamar original para outros erros
        originalError.apply(console, args);
    };

    // Sobrescrever window.onerror
    window.onerror = function(message, source, lineno, colno, error) {
        const msg = (message || '').toString().toLowerCase();
        const src = (source || '').toString().toLowerCase();

        // Bloquear erros do Kendo
        if (msg.includes('collapsible') ||
            (msg.includes('cannot read properties of undefined') && src.includes('kendo'))) {
            console.warn('[SUPRIMIDO] Erro global do Kendo ignorado:', message);
            return true; // Previne propagação
        }

        // Bloquear erros de formatação do Syncfusion (percentSign, etc.)
        if (isSyncfusionFormatError(msg) ||
            (src.includes('syncfusion') && msg.includes('cannot read properties of undefined')) ||
            (src.includes('ej2') && msg.includes('cannot read properties of undefined'))) {
            console.warn('[SUPRIMIDO] Erro global Syncfusion ignorado:', message);
            return true; // Previne propagação
        }

        // Chamar handler original
        if (originalOnError) {
            return originalOnError.apply(this, arguments);
        }

        return false;
    };

    // Também interceptar unhandledrejection para Promises
    window.addEventListener('unhandledrejection', function(event) {
        const reason = (event.reason || '').toString().toLowerCase();

        if (isSyncfusionFormatError(reason)) {
            console.warn('[SUPRIMIDO] Promise rejection Syncfusion ignorada:', event.reason);
            event.preventDefault();
            return;
        }
    });

    console.log('[SUPRESSOR] ✅ Ativo - erros do Kendo e Syncfusion serão suprimidos');
})();
