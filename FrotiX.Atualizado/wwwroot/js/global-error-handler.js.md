# wwwroot/js/global-error-handler.js

**ARQUIVO NOVO** | 170 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```javascript
(function () {
    'use strict';

    var errorCache = {};
    var CACHE_DURATION_MS = 5000;

    function generateRequestId() {
        try {
            return Math.random().toString(36).substring(2, 10);
        } catch (erro) {
            return 'error-' + Date.now();
        }
    }

    function isDuplicate(key) {
        try {
            var now = Date.now();
            if (errorCache[key] && (now - errorCache[key]) < CACHE_DURATION_MS) {
                return true;
            }
            errorCache[key] = now;
            return false;
        } catch (erro) {
            return false;
        }
    }

    function sendErrorToServer(errorInfo) {
        try {

            var cacheKey = errorInfo.tipo + ':' + errorInfo.mensagem + ':' + (errorInfo.linha || 0);
            if (isDuplicate(cacheKey)) {
                return;
            }

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

            if (navigator.sendBeacon) {
                var blob = new Blob([payload], { type: 'application/json' });
                navigator.sendBeacon('/api/LogErros/Client', blob);
            } else {

                fetch('/api/LogErros/Client', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: payload,
                    keepalive: true
                }).catch(function () { });
            }
        } catch (e) {

            if (console && console.warn) {
                console.warn('[FrotiX] Falha ao enviar log de erro:', e);
            }
        }
    }

    function handleGlobalError(message, source, lineno, colno, error) {
        try {
            var requestId = generateRequestId();

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

            if (console && console.error) {
                console.error('[FrotiX] Erro Global Capturado:', {
                    requestId: requestId,
                    message: errorInfo.mensagem,
                    source: source,
                    line: lineno,
                    isScriptError: isScriptError
                });
            }

            sendErrorToServer(errorInfo);

            return false;
        } catch (erro) {

            return false;
        }
    }

    function handleUnhandledRejection(event) {
        try {
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

            if (reason) {
                if (reason instanceof Error) {
                    errorInfo.mensagem = reason.message || 'Promise rejeitada sem tratamento';
                    errorInfo.stack = reason.stack || null;

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

            if (console && console.error) {
                console.error('[FrotiX] Promise Rejeitada:', {
                    requestId: requestId,
                    reason: reason
                });
            }

            sendErrorToServer(errorInfo);
        } catch (erro) {

        }
    }

    function init() {
        try {

            window.onerror = handleGlobalError;

            window.addEventListener('unhandledrejection', handleUnhandledRejection);

            if (console && console.log) {
                console.log('[FrotiX] Global Error Handler ativado - Erros serao capturados automaticamente');
            }
        } catch (erro) {
            if (console && console.error) {
                console.error('[FrotiX] Falha ao inicializar Global Error Handler:', erro);
            }
        }
    }

    setInterval(function () {
        try {
            var now = Date.now();
            for (var key in errorCache) {
                if (errorCache.hasOwnProperty(key) && (now - errorCache[key]) > CACHE_DURATION_MS * 2) {
                    delete errorCache[key];
                }
            }
        } catch (erro) {
            if (console && console.warn) {
                console.warn('[FrotiX] Erro ao limpar cache de erros:', erro);
            }
        }
    }, 30000);

    init();

})();
```
