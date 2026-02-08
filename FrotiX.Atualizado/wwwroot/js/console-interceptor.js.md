# wwwroot/js/console-interceptor.js

**ARQUIVO NOVO** | 164 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```javascript
(function () {
    'use strict';

    const logCache = new Map();
    const CACHE_DURATION = 100;

    const originalConsole = {
        log: console.log,
        warn: console.warn,
        error: console.error,
        info: console.info,
        debug: console.debug
    };

    function interceptConsole() {
        try {

            console.log = function (...args) {
                originalConsole.log.apply(console, args);
                sendConsoleLogToServer('INFO', args);
            };

            console.warn = function (...args) {
                originalConsole.warn.apply(console, args);
                sendConsoleLogToServer('WARN', args);
            };

            console.error = function (...args) {
                originalConsole.error.apply(console, args);
                sendConsoleLogToServer('ERROR', args);
            };

            console.info = function (...args) {
                originalConsole.info.apply(console, args);
                sendConsoleLogToServer('INFO', args);
            };

            console.debug = function (...args) {
                originalConsole.debug.apply(console, args);
                sendConsoleLogToServer('DEBUG', args);
            };

            originalConsole.log('✅ [FrotiX] Interceptador de console ativado - Logs serão registrados automaticamente');
        } catch (error) {
            originalConsole.error('[FrotiX] Erro ao inicializar interceptador de console:', error);
        }
    }

    function sendConsoleLogToServer(type, args) {
        try {

            const message = formatConsoleArgs(args);

            if (!message || message.trim().length === 0) {
                return;
            }

            const cacheKey = `${type}:${message}`;
            const now = Date.now();
            if (logCache.has(cacheKey)) {
                const lastTime = logCache.get(cacheKey);
                if (now - lastTime < CACHE_DURATION) {
                    return;
                }
            }
            logCache.set(cacheKey, now);

            const stackInfo = getStackInfo();

            const payload = {
                Tipo: type,
                Mensagem: message,
                Arquivo: stackInfo.arquivo,
                Metodo: stackInfo.metodo,
                Linha: stackInfo.linha,
                Coluna: stackInfo.coluna,
                Stack: stackInfo.stack,
                Url: window.location.href,
                UserAgent: navigator.userAgent,
                Timestamp: new Date().toISOString()
            };

            fetch('/api/LogErros/LogConsole', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            }).catch(function (error) {

                originalConsole.error('[FrotiX] Erro ao enviar log do console para servidor:', error);
            });

        } catch (error) {

            originalConsole.error('[FrotiX] Erro no interceptador de console:', error);
        }
    }

    function formatConsoleArgs(args) {
        try {

            return Array.from(args).map(function (arg) {
                if (arg === null) return 'null';
                if (arg === undefined) return 'undefined';
                if (typeof arg === 'string') return arg;
                if (typeof arg === 'number' || typeof arg === 'boolean') return String(arg);

                if (typeof arg === 'object') {
                    try {

                        if (arg instanceof Error) {
                            return `Error: ${arg.message}\nStack: ${arg.stack || 'N/A'}`;
                        }

                        return JSON.stringify(arg, null, 2).substring(0, 1000);
                    } catch (e) {
                        return '[Objeto não serializável]';
                    }
                }

                return String(arg);
            }).join(' ');
        } catch (error) {
            return '[Erro ao formatar argumentos]';
        }
    }

    function getStackInfo() {
        try {

            const stack = new Error().stack || '';

            const lines = stack.split('\n');

            for (let i = 0; i < lines.length; i++) {
                const line = lines[i];

                if (line.includes('console-interceptor.js')) continue;
                if (line.includes('at console.')) continue;
                if (line.includes('at sendConsoleLogToServer')) continue;
                if (line.includes('at interceptConsole')) continue;

                const chromeMatch = line.match(/at\s+(?:(.+?)\s+\()?(.+?):(\d+):(\d+)/);
                const firefoxMatch = line.match(/(.+?)@(.+?):(\d+):(\d+)/);

                if (chromeMatch) {
                    return {
                        arquivo: chromeMatch[2] || null,
                        metodo: chromeMatch[1] || null,
                        linha: parseInt(chromeMatch[3]) || null,
                        coluna: parseInt(chromeMatch[4]) || null,
                        stack: stack
                    };
                }

                if (firefoxMatch) {
                    return {
                        arquivo: firefoxMatch[2] || null,
                        metodo: firefoxMatch[1] || null,
                        linha: parseInt(firefoxMatch[3]) || null,
                        coluna: parseInt(firefoxMatch[4]) || null,
                        stack: stack
                    };
                }
            }

            return {
                arquivo: null,
                metodo: null,
                linha: null,
                coluna: null,
                stack: stack
            };
        } catch (error) {
            return {
                arquivo: null,
                metodo: null,
                linha: null,
                coluna: null,
                stack: null
            };
        }
    }

    interceptConsole();

    setInterval(function () {
        try {
            const now = Date.now();
            for (const [key, time] of logCache.entries()) {
                if (now - time > CACHE_DURATION * 10) {
                    logCache.delete(key);
                }
            }
        } catch (error) {
            originalConsole.error('[FrotiX] Erro ao limpar cache de logs:', error);
        }
    }, 30000);

})();
```
