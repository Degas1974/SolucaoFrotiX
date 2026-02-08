/* ****************************************************************************************
 * ⚡ ARQUIVO: frotix-error-logger.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema GLOBAL de captura e envio de erros JavaScript para servidor.
 *                   Captura window.onerror, Promise rejections, erros em try-catch manual.
 *                   Envia automaticamente para API via fetch POST (background silencioso).
 *                   Dedupe de erros (100ms debounce) + cache de erros recentes (100 máx).
 * 📥 ENTRADAS     : Erros globais (window.onerror), Promise rejections (unhandledrejection),
 *                   chamadas manuais (TratamentoErroComLinha, logError, safeAsync, safeSync)
 * 📤 SAÍDAS       : POST /api/LogErros/LogJavaScript (JSON: mensagem, arquivo, metodo, linha,
 *                   coluna, stack, url, userAgent, timestamp), console logs, SweetAlert2 modals
 * 🔗 CHAMADA POR  : window.onerror (automático), unhandledrejection listener (automático),
 *                   try-catch blocks em todos os arquivos JavaScript (TratamentoErroComLinha),
 *                   código geral (logError, logWarning, safeAsync, safeSync)
 * 🔄 CHAMA        : fetch API, window.Alerta.TratamentoErroComLinha (se existe),
 *                   window.SweetAlertInterop.ShowErrorUnexpected (fallback), Swal.fire (SweetAlert2),
 *                   alert (fallback nativo)
 * 📦 DEPENDÊNCIAS : Fetch API (nativo), SweetAlert2 (opcional para modal), window.Alerta (opcional)
 * 📝 OBSERVAÇÕES  : IIFE com auto-inicialização (DOMContentLoaded), singleton (window.FrotiXErrorLogger),
 *                   dedupe via Map (100ms debounce), cache auto-limpeza (>100 erros ou >5s antigos),
 *                   stack trace parsing multi-browser (Chrome/Edge/Firefox/Safari), não bloqueia
 *                   propagação de erros (return false em window.onerror)
 *
 * 📋 ÍNDICE DE FUNÇÕES (16 funções + 2 event listeners + 1 conditional listener):
 *
 * ┌─ FUNÇÕES AUXILIARES (Stack/Parse/Dedupe) ─────────────────────────────────────┐
 * │ 1. parseStackTrace(stack)                                                      │
 * │    → Extrai arquivo, linha, coluna, função de stack trace                     │
 * │    → Suporta Chrome/Edge (at func (url:10:20)), Firefox (func@url:10:20),     │
 * │      Safari (func@url:10:20), Chrome sem nome (at url:10:20)                  │
 * │    → Retorna { arquivo, linha, coluna, funcao } ou valores null               │
 * │                                                                                 │
 * │ 2. extractFileName(url)                                                        │
 * │    → Extrai nome do arquivo de URL completa (remove query string, pega last)  │
 * │    → Exemplo: "http://site.com/js/file.js?v=1" → "file.js"                    │
 * │                                                                                 │
 * │ 3. isDuplicateError(errorKey)                                                  │
 * │    → Verifica se erro já foi logado nos últimos 100ms (debounce)              │
 * │    → Auto-limpeza do Map quando >100 entradas ou >5s antigas                  │
 * │    → Retorna true se duplicado, false se novo                                 │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÃO DE ENVIO PARA SERVIDOR ────────────────────────────────────────────────┐
 * │ 4. sendErrorToServer(errorData)                                                │
 * │    → Envia erro para API via fetch POST (background, silencioso)              │
 * │    → Endpoint: /api/LogErros/LogJavaScript                                     │
 * │    → Headers: Content-Type: application/json, X-Requested-With: XMLHttpRequest│
 * │    → Dedupe antes de enviar (isDuplicateError)                                │
 * │    → Não loga erro de rede (evita loop infinito)                              │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ HANDLERS DE ERRO GLOBAL (window.onerror/Promise) ─────────────────────────────┐
 * │ 5. globalErrorHandler(message, source, lineno, colno, error)                   │
 * │    → Atribuído a window.onerror na init()                                      │
 * │    → Captura erros de script não tratados                                      │
 * │    → Parseia stack trace, monta errorData, chama sendErrorToServer()           │
 * │    → Retorna false (não impede propagação do erro)                             │
 * │                                                                                 │
 * │ 6. unhandledRejectionHandler(event)                                            │
 * │    → Event listener para 'unhandledrejection' (Promise rejects não tratadas)  │
 * │    → Extrai event.reason, parseia stack, monta errorData, envia               │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ API PÚBLICA (window.FrotiXErrorLogger) ────────────────────────────────────────┐
 * │ 7. TratamentoErroComLinha(classe, metodo, erro)                                │
 * │    → Método PRINCIPAL para try-catch manual em todo código FrotiX             │
 * │    → Parseia stack, monta errorData, envia para servidor                       │
 * │    → Chama window.Alerta.TratamentoErroComLinha (se existe) para modal visual │
 * │    → Fallback: SweetAlertInterop.ShowErrorUnexpected ou console.error          │
 * │                                                                                 │
 * │ 8. logError(message, arquivo, metodo, details)                                 │
 * │    → Log manual de erro com mensagem customizada                               │
 * │    → details.stack opcional                                                    │
 * │    → Envia para servidor via sendErrorToServer()                               │
 * │                                                                                 │
 * │ 9. logWarning(message, arquivo, metodo)                                        │
 * │    → Log de warning (console.warn apenas, NÃO envia para servidor)            │
 * │                                                                                 │
 * │ 10. safeAsync(fn, classe, metodo)                                              │
 * │    → Wrapper para funções async com try-catch automático                       │
 * │    → Chama TratamentoErroComLinha em caso de erro, re-lança o erro            │
 * │    → Uso: await FrotiXErrorLogger.safeAsync(async () => {...}, 'Class', 'method')│
 * │                                                                                 │
 * │ 11. safeSync(fn, classe, metodo)                                               │
 * │    → Wrapper para funções síncronas com try-catch automático                   │
 * │    → Chama TratamentoErroComLinha em caso de erro, re-lança o erro            │
 * │                                                                                 │
 * │ 12. setEnabled(enabled)                                                        │
 * │    → Habilita/desabilita envio de erros para servidor (CONFIG.enabled)        │
 * │                                                                                 │
 * │ 13. setApiEndpoint(endpoint)                                                   │
 * │    → Configura endpoint da API (default: /api/LogErros/LogJavaScript)         │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE INICIALIZAÇÃO ──────────────────────────────────────────────────────┐
 * │ 14. init()                                                                      │
 * │    → Registra window.onerror = globalErrorHandler                              │
 * │    → Registra window.addEventListener('unhandledrejection', unhandledRejectionHandler)│
 * │    → Chamado em DOMContentLoaded ou imediatamente se DOM já pronto            │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE COMPATIBILIDADE (window.Alerta alias) ──────────────────────────────┐
 * │ 15. window.Alerta.TratamentoErroComLinha(classe, metodo, erro)                 │
 * │    → Wrapper de compatibilidade para código legacy FrotiX                      │
 * │    → Chama FrotiXErrorLogger.TratamentoErroComLinha()                          │
 * │    → Se SweetAlert2 (Swal) disponível: mostra modal com stack trace (5 linhas)│
 * │    → Fallback: alert() nativo                                                  │
 * │                                                                                 │
 * │ 16. TratamentoErroComLinha(classe, metodo, erro) [função global]               │
 * │    → Alias global para window.Alerta.TratamentoErroComLinha                   │
 * │    → Permite chamada direta sem namespace                                      │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EVENT LISTENERS GLOBAIS ───────────────────────────────────────────────────────┐
 * │ 17. window.onerror = globalErrorHandler                                         │
 * │    → Captura erros de script não tratados                                      │
 * │                                                                                 │
 * │ 18. window 'unhandledrejection' listener                                        │
 * │    → Captura Promise rejections não tratadas                                   │
 * │                                                                                 │
 * │ 19. document 'DOMContentLoaded' listener (condicional)                          │
 * │    → Chama init() quando DOM pronto (se document.readyState === 'loading')    │
 * │    → Se DOM já pronto, chama init() imediatamente                              │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * 📌 AJAX ENDPOINT:
 * - POST /api/LogErros/LogJavaScript
 *   → Body (JSON): { mensagem, arquivo, metodo, linha, coluna, stack, url, userAgent, timestamp }
 *   → Response: void (silencioso, erros de rede não são re-logados)
 *
 * 🎨 CONFIGURAÇÕES (CONFIG):
 * - apiEndpoint: '/api/LogErros/LogJavaScript'
 * - maxStackLines: 15 (não usado atualmente)
 * - debounceTime: 100ms (intervalo mínimo entre logs iguais)
 * - enabled: true (pode ser desabilitado via setEnabled(false))
 *
 * 🔄 ESTRUTURA DE errorData (enviado para servidor):
 * {
 *   mensagem: string,        // Mensagem de erro
 *   arquivo: string,         // Nome do arquivo (extraído de stack ou source)
 *   metodo: string,          // Nome da função (extraído de stack ou fornecido)
 *   linha: number,           // Linha do erro
 *   coluna: number,          // Coluna do erro
 *   stack: string,           // Stack trace completo (primeiras maxStackLines linhas)
 *   url: string,             // window.location.href
 *   userAgent: string,       // navigator.userAgent
 *   timestamp: string        // ISO 8601 (new Date().toISOString())
 * }
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - IIFE pattern: (function() { ... })() - auto-executável
 * - Singleton: window.FrotiXErrorLogger (API pública imutável)
 * - Dedupe: Map com errorKey = "${mensagem}-${arquivo}-${linha}", 100ms window
 * - Cache: Max 100 erros recentes, auto-limpa >5s antigos
 * - Stack trace parsing: 4 padrões regex (Chrome/Edge com/sem nome, Firefox, Safari)
 * - SweetAlert2: Modal opcional com stack trace (5 linhas), HTML formatado com emojis
 * - Não bloqueia propagação: window.onerror retorna false
 * - safeAsync/safeSync: re-lançam o erro após logar (preservam comportamento original)
 *
 * 🔌 VERSÃO: 1.0.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 08/01/2026
 * 📌 DOCUMENTAÇÃO EXTERNA: Documentacao/JavaScript/frotix-error-logger.js.md
 **************************************************************************************** */

window.FrotiXErrorLogger = (function () {
    'use strict';

    // ========== CONFIGURAÇÃO ==========
    const CONFIG = {
        apiEndpoint: '/api/LogErros/LogJavaScript',
        maxStackLines: 15,
        debounceTime: 100, // ms entre logs iguais
        enabled: true
    };

    // Cache para evitar logs duplicados
    const recentErrors = new Map();

    // ========== FUNÇÕES AUXILIARES ==========

    /**
     * Extrai informações do stack trace
     */
    function parseStackTrace(stack) {
        if (!stack) return { arquivo: null, linha: null, coluna: null, funcao: null };

        try {
            // Padrões de stack trace
            const patterns = [
                // Chrome/Edge: "at functionName (http://...file.js:10:20)"
                /at\s+(.+?)\s+\((.+?):(\d+):(\d+)\)/,
                // Chrome/Edge sem nome de função: "at http://...file.js:10:20"
                /at\s+(.+?):(\d+):(\d+)/,
                // Firefox: "functionName@http://...file.js:10:20"
                /(.+?)@(.+?):(\d+):(\d+)/,
                // Safari: "functionName@http://...file.js:10:20"
                /(\w+)@(.+):(\d+):(\d+)/
            ];

            const lines = stack.split('\n');

            for (const line of lines) {
                for (const pattern of patterns) {
                    const match = line.match(pattern);
                    if (match) {
                        if (match.length === 5) {
                            // Com nome de função
                            return {
                                funcao: match[1].trim(),
                                arquivo: extractFileName(match[2]),
                                linha: parseInt(match[3], 10),
                                coluna: parseInt(match[4], 10)
                            };
                        } else if (match.length === 4) {
                            // Sem nome de função
                            return {
                                funcao: null,
                                arquivo: extractFileName(match[1]),
                                linha: parseInt(match[2], 10),
                                coluna: parseInt(match[3], 10)
                            };
                        }
                    }
                }
            }
        } catch (e) {
            console.warn('FrotiXErrorLogger: Erro ao parsear stack trace', e);
        }

        return { arquivo: null, linha: null, coluna: null, funcao: null };
    }

    /**
     * Extrai nome do arquivo de uma URL completa
     */
    function extractFileName(url) {
        if (!url) return null;
        try {
            // Remove query string
            const cleanUrl = url.split('?')[0];
            // Pega última parte do path
            const parts = cleanUrl.split('/');
            return parts[parts.length - 1] || cleanUrl;
        } catch {
            return url;
        }
    }

    /**
     * Verifica se é erro duplicado recente
     */
    function isDuplicateError(errorKey) {
        try {
            const now = Date.now();
            const lastTime = recentErrors.get(errorKey);

            if (lastTime && (now - lastTime) < CONFIG.debounceTime) {
                return true;
            }

            recentErrors.set(errorKey, now);

            // Limpa cache antigo
            if (recentErrors.size > 100) {
                const oldEntries = Array.from(recentErrors.entries())
                    .filter(([, time]) => (now - time) > 5000);
                oldEntries.forEach(([key]) => recentErrors.delete(key));
            }

            return false;
        } catch (erro) {
            console.error('Erro em isDuplicateError:', erro);
            return false;
        }
    }

    /**
     * Envia erro para o servidor
     */
    async function sendErrorToServer(errorData) {
        if (!CONFIG.enabled) return;

        const errorKey = `${errorData.mensagem}-${errorData.arquivo}-${errorData.linha}`;
        if (isDuplicateError(errorKey)) return;

        try {
            const response = await fetch(CONFIG.apiEndpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: JSON.stringify(errorData)
            });

            if (!response.ok) {
                console.warn('FrotiXErrorLogger: Falha ao enviar erro para servidor', response.status);
            }
        } catch (e) {
            // Não loga erro de rede para evitar loop infinito
            console.warn('FrotiXErrorLogger: Erro de rede ao enviar log', e.message);
        }
    }

    // ========== HANDLERS DE ERRO GLOBAL ==========

    /**
     * Handler para window.onerror (erros de script)
     */
    function globalErrorHandler(message, source, lineno, colno, error) {
        try {
            const stackInfo = error ? parseStackTrace(error.stack) : {};

            const errorData = {
                mensagem: message || 'Erro JavaScript desconhecido',
                arquivo: extractFileName(source) || stackInfo.arquivo,
                metodo: stackInfo.funcao,
                linha: lineno || stackInfo.linha,
                coluna: colno || stackInfo.coluna,
                stack: error?.stack || null,
                url: window.location.href,
                userAgent: navigator.userAgent,
                timestamp: new Date().toISOString()
            };

            sendErrorToServer(errorData);
        } catch (erro) {
            console.error('Erro em globalErrorHandler:', erro);
        }

        // Não impede propagação do erro
        return false;
    }

    /**
     * Handler para Promise rejections não tratadas
     */
    function unhandledRejectionHandler(event) {
        try {
            const error = event.reason;
            const stackInfo = error instanceof Error ? parseStackTrace(error.stack) : {};

            const errorData = {
                mensagem: error instanceof Error ? error.message : String(error),
                arquivo: stackInfo.arquivo || 'Promise',
                metodo: stackInfo.funcao || 'unhandledRejection',
                linha: stackInfo.linha,
                coluna: stackInfo.coluna,
                stack: error instanceof Error ? error.stack : null,
                url: window.location.href,
                userAgent: navigator.userAgent,
                timestamp: new Date().toISOString()
            };

            sendErrorToServer(errorData);
        } catch (erro) {
            console.error('Erro em unhandledRejectionHandler:', erro);
        }
    }

    // ========== API PÚBLICA ==========

    /**
     * Wrapper para tratamento de erros em funções
     * Uso: FrotiXErrorLogger.TratamentoErroComLinha('Classe', 'metodo', erro)
     */
    function TratamentoErroComLinha(classe, metodo, erro) {
        try {
            const stackInfo = erro instanceof Error ? parseStackTrace(erro.stack) : {};

            const errorData = {
                mensagem: erro instanceof Error ? erro.message : String(erro),
                arquivo: stackInfo.arquivo || classe,
                metodo: metodo || stackInfo.funcao,
                linha: stackInfo.linha,
                coluna: stackInfo.coluna,
                stack: erro instanceof Error ? erro.stack : null,
                url: window.location.href,
                userAgent: navigator.userAgent,
                timestamp: new Date().toISOString()
            };

            sendErrorToServer(errorData);

            // Exibe alerta visual se disponível
            if (window.Alerta && window.Alerta.TratamentoErroComLinha) {
                window.Alerta.TratamentoErroComLinha(classe, metodo, erro);
            } else if (window.SweetAlertInterop && window.SweetAlertInterop.ShowErrorUnexpected) {
                window.SweetAlertInterop.ShowErrorUnexpected(classe, metodo, erro);
            } else {
                console.error(`[FrotiX Error] ${classe}.${metodo}:`, erro);
            }
        } catch (erroInterno) {
            console.error('Erro em TratamentoErroComLinha:', erroInterno);
        }
    }

    /**
     * Log manual de erro
     */
    function logError(message, arquivo, metodo, details) {
        try {
            const errorData = {
                mensagem: message,
                arquivo: arquivo || extractFileName(window.location.pathname),
                metodo: metodo,
                linha: null,
                coluna: null,
                stack: details?.stack || null,
                url: window.location.href,
                userAgent: navigator.userAgent,
                timestamp: new Date().toISOString()
            };

            sendErrorToServer(errorData);
        } catch (erro) {
            console.error('Erro em logError:', erro);
        }
    }

    /**
     * Log de warning
     */
    function logWarning(message, arquivo, metodo) {
        try {
            console.warn(`[FrotiX Warning] ${arquivo || ''}.${metodo || ''}: ${message}`);
            // Warnings não são enviados ao servidor por padrão
        } catch (erro) {
            console.error('Erro em logWarning:', erro);
        }
    }

    /**
     * Wrapper para try-catch em funções async
     * Uso: await FrotiXErrorLogger.safeAsync(async () => { ... }, 'Classe', 'metodo')
     */
    async function safeAsync(fn, classe, metodo) {
        try {
            return await fn();
        } catch (erro) {
            TratamentoErroComLinha(classe, metodo, erro);
            throw erro; // Re-lança para manter comportamento esperado
        }
    }

    /**
     * Wrapper para try-catch em funções síncronas
     * Uso: FrotiXErrorLogger.safeSync(() => { ... }, 'Classe', 'metodo')
     */
    function safeSync(fn, classe, metodo) {
        try {
            return fn();
        } catch (erro) {
            TratamentoErroComLinha(classe, metodo, erro);
            throw erro;
        }
    }

    /**
     * Habilita/desabilita o logger
     */
    function setEnabled(enabled) {
        try {
            CONFIG.enabled = enabled;
        } catch (erro) {
            console.error('Erro em setEnabled:', erro);
        }
    }

    /**
     * Configura endpoint da API
     */
    function setApiEndpoint(endpoint) {
        try {
            CONFIG.apiEndpoint = endpoint;
        } catch (erro) {
            console.error('Erro em setApiEndpoint:', erro);
        }
    }

    // ========== INICIALIZAÇÃO ==========

    function init() {
        try {
            // Registra handlers globais
            window.onerror = globalErrorHandler;
            window.addEventListener('unhandledrejection', unhandledRejectionHandler);

            console.log('[FrotiXErrorLogger] Inicializado com sucesso');
        } catch (erro) {
            console.error('Erro em init:', erro);
        }
    }

    // Inicializa quando o DOM estiver pronto
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // ========== EXPÕE API PÚBLICA ==========

    return {
        TratamentoErroComLinha,
        logError,
        logWarning,
        safeAsync,
        safeSync,
        setEnabled,
        setApiEndpoint
    };

})();

// ========== ALIAS GLOBAL PARA COMPATIBILIDADE ==========

// Mantém compatibilidade com o padrão existente no FrotiX
window.Alerta = window.Alerta || {};
window.Alerta.TratamentoErroComLinha = window.Alerta.TratamentoErroComLinha || function (classe, metodo, erro) {
    try {
        FrotiXErrorLogger.TratamentoErroComLinha(classe, metodo, erro);

        // Se SweetAlert2 estiver disponível, mostra modal de erro
        if (typeof Swal !== 'undefined') {
            const stackInfo = erro instanceof Error && erro.stack ?
                erro.stack.split('\n').slice(0, 5).join('<br>') : '';

            Swal.fire({
                icon: 'error',
                title: 'Erro Sem Tratamento',
                html: `
                    <div style="text-align: left; font-size: 14px;">
                        <p><b>📚 Classe:</b> ${classe}</p>
                        <p><b>🖋️ Método:</b> ${metodo}</p>
                        <p><b>❌ Erro:</b> ${erro?.message || erro}</p>
                        ${stackInfo ? `<details><summary>Stack Trace</summary><pre style="font-size: 11px; overflow-x: auto;">${stackInfo}</pre></details>` : ''}
                    </div>
                `,
                confirmButtonText: 'OK',
                confirmButtonColor: '#dc3545'
            });
        } else {
            // Fallback para alert nativo
            alert(`Erro em ${classe}.${metodo}: ${erro?.message || erro}`);
        }
    } catch (erroInterno) {
        console.error('Erro em window.Alerta.TratamentoErroComLinha:', erroInterno);
    }
};

// Função global para uso em try-catch
function TratamentoErroComLinha(classe, metodo, erro) {
    try {
        window.Alerta.TratamentoErroComLinha(classe, metodo, erro);
    } catch (erroInterno) {
        console.error('Erro em TratamentoErroComLinha (global):', erroInterno);
    }
}
