/* ****************************************************************************************
 * ⚡ ARQUIVO: kendo-error-suppressor.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Suprime erros CONHECIDOS e ESPERADOS do Kendo UI (collapsible,
 *                   toggle) que poluem console sem impacto real. Intercepta console.error
 *                   e window.onerror ANTES de todos os outros scripts para bloquear
 *                   propagação de erros específicos do Kendo.
 * 📥 ENTRADAS     : Chamadas console.error(), erros globais window.onerror
 * 📤 SAÍDAS       : console.warn para erros suprimidos, console.error original para outros,
 *                   return true em window.onerror para prevenir propagação
 * 🔗 CHAMADA POR  : console.error (override global), window.onerror (override global),
 *                   qualquer código que logue erro ou lance exceção não tratada
 * 🔄 CHAMA        : console.error original, console.warn, window.onerror original (se existe)
 * 📦 DEPENDÊNCIAS : Vanilla JavaScript (sem bibliotecas), deve ser carregado PRIMEIRO
 * 📝 OBSERVAÇÕES  : IIFE auto-executável, CRITICAL: incluir ANTES de todos os outros scripts
 *                   (especialmente antes de kendo.all.min.js), guarda funções originais
 *                   (originalError, originalOnError), pattern matching em lowercase,
 *                   suprime apenas erros específicos (collapsible + toggle undefined),
 *                   não afeta outros erros
 *
 * 📋 ÍNDICE DE FUNÇÕES (2 overrides + 1 log inicial):
 *
 * ┌─ OVERRIDES GLOBAIS (console.error/window.onerror) ──────────────────────────────┐
 * │ 1. console.error override                                                        │
 * │    → Intercepta TODAS as chamadas console.error()                               │
 * │    → Pattern matching: msg.includes('collapsible') OU                           │
 * │      (msg.includes('cannot read properties of undefined') AND msg.includes('toggle'))│
 * │    → Se match: console.warn('[SUPRIMIDO] Erro do Kendo ignorado:', ...args)    │
 * │    → Se não match: chama originalError.apply(console, args)                     │
 * │                                                                                  │
 * │ 2. window.onerror override                                                      │
 * │    → Intercepta TODOS os erros globais não tratados                             │
 * │    → Pattern matching: msg.includes('collapsible') OU                           │
 * │      (msg.includes('cannot read properties of undefined') AND src.includes('kendo'))│
 * │    → Se match: console.warn('[SUPRIMIDO] Erro global do Kendo ignorado:', message)│
 * │      e return true (previne propagação)                                         │
 * │    → Se não match: chama originalOnError.apply(this, arguments) ou return false│
 * └──────────────────────────────────────────────────────────────────────────────────┘
 *
 * 🎯 ERROS SUPRIMIDOS (2 padrões):
 * 1. Palavra-chave "collapsible" (qualquer erro mencionando collapsible)
 * 2. "cannot read properties of undefined" + "toggle" (erro específico de Kendo)
 * 3. "cannot read properties of undefined" + source contém "kendo" (variante)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - IIFE pattern: (function() { ... })() - auto-executável
 * - CRITICAL: Este arquivo DEVE ser carregado PRIMEIRO, antes de kendo.all.min.js
 * - Guarda funções originais: originalError, originalOnError (const, não podem ser modificados)
 * - Lowercase matching: msg.toLowerCase() para case-insensitive comparison
 * - Não afeta outros erros: apenas erros Kendo específicos são suprimidos
 * - console.warn: erros suprimidos são logados como warning para debug
 * - window.onerror return true: previne propagação do erro (não aparece no console)
 * - window.onerror return false: permite propagação (comportamento padrão)
 *
 * 🔌 VERSÃO: 1.0.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 08/01/2026
 * 📌 DOCUMENTAÇÃO EXTERNA: Documentacao/JavaScript/kendo-error-suppressor.js.md
 **************************************************************************************** */

(function() {
    'use strict';
    
    // Guardar funções originais
    const originalError = console.error;
    const originalOnError = window.onerror;
    
    // Sobrescrever console.error
    console.error = function(...args) {
        try {
            const msg = args.join(' ').toLowerCase();

            // Bloquear erros específicos do Kendo
            if (msg.includes('collapsible') ||
                (msg.includes('cannot read properties of undefined') && msg.includes('toggle'))) {
                console.warn('[SUPRIMIDO] Erro do Kendo ignorado:', ...args);
                return;
            }

            // Chamar original para outros erros
            originalError.apply(console, args);
        } catch (erro) {
            // Se falhar o supressor, chama original para não perder o erro
            originalError.apply(console, args);
        }
    };
    
    // Sobrescrever window.onerror
    window.onerror = function(message, source, lineno, colno, error) {
        try {
            const msg = (message || '').toString().toLowerCase();
            const src = (source || '').toString().toLowerCase();

            // Bloquear erros do Kendo
            if (msg.includes('collapsible') ||
                (msg.includes('cannot read properties of undefined') && src.includes('kendo'))) {
                console.warn('[SUPRIMIDO] Erro global do Kendo ignorado:', message);
                return true; // Previne propagação
            }

            // Chamar handler original
            if (originalOnError) {
                return originalOnError.apply(this, arguments);
            }

            return false;
        } catch (erro) {
            // Se falhar o supressor, chama original ou retorna false
            if (originalOnError) {
                return originalOnError.apply(this, arguments);
            }
            return false;
        }
    };
    
    console.log('[SUPRESSOR] ✅ Ativo - erros do Kendo serão suprimidos');
})();
