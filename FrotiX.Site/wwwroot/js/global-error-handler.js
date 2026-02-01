/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ ARQUIVO: global-error-handler.js                                                                    ║
   ║ CAMINHO: /wwwroot/js/                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ OBJETIVO: Capturar TODOS os erros globais do JavaScript, incluindo:                                 ║
   ║           - window.onerror (erros de sintaxe e runtime)                                             ║
   ║           - unhandledrejection (Promises rejeitadas sem catch)                                      ║
   ║           - "Script error" de scripts cross-origin                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ INDICE DE FUNCOES:                                                                                  ║
   ║ 1. [handleGlobalError] : Processa erros globais................. (message, source, ...) -> void     ║
   ║ 2. [handleUnhandledRejection] : Processa promises rejeitadas.... (event) -> void                    ║
   ║ 3. [sendErrorToServer] : Envia erro para API.................... (errorInfo) -> void                ║
   ║ 4. [generateRequestId] : Gera ID unico.......................... () -> string                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ RASTREABILIDADE:                                                                                    ║
   ║    CHAMA: POST /api/LogErros/Client                                                                 ║
   ║ DATA: 01/02/2026 | AUTOR: Claude Code | VERSAO: 1.0                                                 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

(function () {
    'use strict';

    // Cache para deduplicacao (evita enviar mesmo erro multiplas vezes)
    var errorCache = {};
    var CACHE_DURATION_MS = 5000; // 5 segundos

    /**
     * Gera ID unico para requisicao
     */
    function generateRequestId() {
        return Math.random().toString(36).substring(2, 10);
    }

    /**
     * Verifica se o erro e duplicado (dentro do periodo de cache)
     */
    function isDuplicate(key) {
        var now = Date.now();
        if (errorCache[key] && (now - errorCache[key]) < CACHE_DURATION_MS) {
            return true;
        }
        errorCache[key] = now;
        return false;
    }

    /**
     * Envia erro para o servidor via API
     */
    function sendErrorToServer(errorInfo) {
        try {
            // Gera chave para deduplicacao
            var cacheKey = errorInfo.tipo + ':' + errorInfo.mensagem + ':' + (errorInfo.linha || 0);
            if (isDuplicate(cacheKey)) {
                return;
            }

            // Prepara payload
            var payload = JSON.stringify({
                Tipo: errorInfo.tipo,
                Mensagem: errorInfo.mensagem,
                StatusCode: errorInfo.statusCode || 0,
                RequestId: errorInfo.requestId,
                Arquivo: errorInfo.arquivo,
                Metodo: errorInfo.metodo,
                Linha: errorInfo.linha,
                Coluna: errorInfo.coluna,
                Stack: errorInfo.stack,
                Url: errorInfo.url || window.location.href,
                UserAgent: navigator.userAgent,
                Timestamp: new Date().toISOString()
            });

            // Usa sendBeacon para garantir envio mesmo ao fechar pagina
            if (navigator.sendBeacon) {
                var blob = new Blob([payload], { type: 'application/json' });
                navigator.sendBeacon('/api/LogErros/Client', blob);
            } else {
                // Fallback para fetch (fire-and-forget)
                fetch('/api/LogErros/Client', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: payload,
                    keepalive: true
                }).catch(function () { /* Silenciar erros do log */ });
            }
        } catch (e) {
            // Nunca deixar o handler de erros quebrar a pagina
            if (console && console.warn) {
                console.warn('[FrotiX] Falha ao enviar log de erro:', e);
            }
        }
    }

    /**
     * Handler para window.onerror
     * Captura erros de sintaxe, runtime e "Script error" de scripts cross-origin
     */
    function handleGlobalError(message, source, lineno, colno, error) {
        var requestId = generateRequestId();

        // Tratamento especial para "Script error" (erros cross-origin)
        var isScriptError = (message === 'Script error.' || message === 'Script error');

        var errorInfo = {
            tipo: 'GLOBAL-ERROR',
            requestId: requestId,
            mensagem: isScriptError
                ? 'Erro de script externo (CORS) - Detalhes nao disponiveis por seguranca do navegador. Verifique se todos os scripts CDN possuem crossorigin="anonymous".'
                : (message || 'Erro desconhecido'),
            arquivo: source || 'Desconhecido',
            metodo: null,
            linha: lineno || null,
            coluna: colno || null,
            stack: error && error.stack ? error.stack : null,
            url: window.location.href,
            statusCode: 0
        };

        // Log no console para debug
        if (console && console.error) {
            console.error('[FrotiX] Erro Global Capturado:', {
                requestId: requestId,
                message: errorInfo.mensagem,
                source: source,
                line: lineno,
                isScriptError: isScriptError
            });
        }

        // Envia para o servidor
        sendErrorToServer(errorInfo);

        // Retorna false para permitir que o erro continue propagando
        // (permite que outros handlers tambem capturem)
        return false;
    }

    /**
     * Handler para unhandledrejection
     * Captura Promises rejeitadas que nao foram tratadas com .catch()
     */
    function handleUnhandledRejection(event) {
        var requestId = generateRequestId();
        var reason = event.reason;

        var errorInfo = {
            tipo: 'UNHANDLED-PROMISE',
            requestId: requestId,
            mensagem: 'Promise rejeitada sem tratamento',
            arquivo: null,
            metodo: null,
            linha: null,
            coluna: null,
            stack: null,
            url: window.location.href,
            statusCode: 0
        };

        // Extrai informacoes do reason
        if (reason) {
            if (reason instanceof Error) {
                errorInfo.mensagem = reason.message || 'Promise rejeitada sem tratamento';
                errorInfo.stack = reason.stack || null;

                // Tenta extrair arquivo/linha do stack
                if (reason.stack) {
                    var match = reason.stack.match(/at\s+(?:.+?\s+\()?(.+?):(\d+):(\d+)/);
                    if (match) {
                        errorInfo.arquivo = match[1];
                        errorInfo.linha = parseInt(match[2], 10);
                        errorInfo.coluna = parseInt(match[3], 10);
                    }
                }
            } else if (typeof reason === 'string') {
                errorInfo.mensagem = reason;
            } else if (typeof reason === 'object') {
                try {
                    errorInfo.mensagem = JSON.stringify(reason).substring(0, 500);
                } catch (e) {
                    errorInfo.mensagem = 'Promise rejeitada com objeto nao serializavel';
                }
            }
        }

        // Log no console para debug
        if (console && console.error) {
            console.error('[FrotiX] Promise Rejeitada:', {
                requestId: requestId,
                reason: reason
            });
        }

        // Envia para o servidor
        sendErrorToServer(errorInfo);
    }

    /**
     * Inicializa os handlers globais
     */
    function init() {
        // Handler para erros globais de JavaScript
        window.onerror = handleGlobalError;

        // Handler para Promises rejeitadas sem catch
        window.addEventListener('unhandledrejection', handleUnhandledRejection);

        // Log de inicializacao
        if (console && console.log) {
            console.log('[FrotiX] Global Error Handler ativado - Erros serao capturados automaticamente');
        }
    }

    // Limpa cache de erros periodicamente
    setInterval(function () {
        var now = Date.now();
        for (var key in errorCache) {
            if (errorCache.hasOwnProperty(key) && (now - errorCache[key]) > CACHE_DURATION_MS * 2) {
                delete errorCache[key];
            }
        }
    }, 30000); // A cada 30 segundos

    // Inicializa quando o script carrega
    init();

})();
