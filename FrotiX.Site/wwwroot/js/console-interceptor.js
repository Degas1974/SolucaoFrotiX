/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: console-interceptor.js                                                                 ║
   ║ 📂 CAMINHO: /wwwroot/js/                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interceptador global de console.log/warn/error/info para registro automático         ║
   ║              de TODOS os logs do navegador no sistema de logs do FrotiX.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [interceptConsole] : Intercepta métodos do console.... () -> void                               ║
   ║ 2. [sendConsoleLogToServer] : Envia log para servidor.... (type, args) -> void                     ║
   ║ 3. [formatConsoleArgs] : Formata argumentos do console.... (args) -> string                        ║
   ║ 4. [getStackInfo] : Extrai info do stack trace........... () -> object                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 RASTREABILIDADE:                                                                                 ║
   ║    ⬅️ CHAMADO POR : Auto-executado no carregamento da página                                        ║
   ║    ➡️ CHAMA       : POST /api/LogErros/LogConsole                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📝 OBSERVAÇÕES:                                                                                     ║
   ║    • Mantém funcionalidade original do console (console.log continua funcionando)                  ║
   ║    • Envia logs assincronamente sem bloquear execução                                              ║
   ║    • Deduplicação automática para evitar envios duplicados                                         ║
   ║    • Extrai arquivo/linha do stack trace quando possível                                           ║
   ║ 📅 31/01/2026 | 👤 Claude Code | 📝 v1.0                                                            ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

(function () {
    'use strict';

    // [DADOS] Cache para deduplicação (evita enviar mesmo log múltiplas vezes)
    const logCache = new Map();
    const CACHE_DURATION = 100; // ms

    // [DADOS] Referências originais do console
    const originalConsole = {
        log: console.log,
        warn: console.warn,
        error: console.error,
        info: console.info,
        debug: console.debug
    };

    /**
     * ╭───────────────────────────────────────────────────────────────────────────────────────╮
     * │ ⚡ FUNCIONALIDADE: interceptConsole                                                   │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 🎯 DESCRIÇÃO: Intercepta todos os métodos do console (log, warn, error, info, debug)  │
     * │              mantendo funcionalidade original e enviando para servidor.               │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 📥 INPUTS: Nenhum - executado automaticamente                                         │
     * │ 📤 OUTPUTS: void - Sobrescreve métodos do console                                     │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 🔗 RASTREABILIDADE:                                                                    │
     * │    ⬅️ CHAMADO POR : IIFE auto-executável                                              │
     * │    ➡️ CHAMA       : sendConsoleLogToServer() para cada tipo de log                     │
     * ╰───────────────────────────────────────────────────────────────────────────────────────╯
     */
    function interceptConsole() {
        try {
            // [LOGICA] Interceptar console.log
            console.log = function (...args) {
                originalConsole.log.apply(console, args);
                sendConsoleLogToServer('INFO', args);
            };

            // [LOGICA] Interceptar console.warn
            console.warn = function (...args) {
                originalConsole.warn.apply(console, args);
                sendConsoleLogToServer('WARN', args);
            };

            // [LOGICA] Interceptar console.error
            console.error = function (...args) {
                originalConsole.error.apply(console, args);
                sendConsoleLogToServer('ERROR', args);
            };

            // [LOGICA] Interceptar console.info
            console.info = function (...args) {
                originalConsole.info.apply(console, args);
                sendConsoleLogToServer('INFO', args);
            };

            // [LOGICA] Interceptar console.debug (apenas em modo DEBUG)
            console.debug = function (...args) {
                originalConsole.debug.apply(console, args);
                sendConsoleLogToServer('DEBUG', args);
            };

            originalConsole.log('✅ [FrotiX] Interceptador de console ativado - Logs serão registrados automaticamente');
        } catch (error) {
            originalConsole.error('[FrotiX] Erro ao inicializar interceptador de console:', error);
        }
    }

    /**
     * ╭───────────────────────────────────────────────────────────────────────────────────────╮
     * │ ⚡ FUNCIONALIDADE: sendConsoleLogToServer                                             │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 🎯 DESCRIÇÃO: Envia log do console para o servidor via API REST.                      │
     * │              Inclui deduplicação e extração de stack trace.                           │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 📥 INPUTS:                                                                             │
     * │    • type [string]: Tipo do log (INFO, WARN, ERROR, DEBUG)                            │
     * │    • args [Array]: Argumentos passados para console.*                                 │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 📤 OUTPUTS: void - Envia requisição assíncrona para servidor                          │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 🔗 RASTREABILIDADE:                                                                    │
     * │    ⬅️ CHAMADO POR : console.log/warn/error/info/debug interceptados                    │
     * │    ➡️ CHAMA       : POST /api/LogErros/LogConsole                                      │
     * ╰───────────────────────────────────────────────────────────────────────────────────────╯
     */
    function sendConsoleLogToServer(type, args) {
        try {
            // [DADOS] Formatar argumentos em string
            const message = formatConsoleArgs(args);

            // [REGRA] Não enviar logs vazios
            if (!message || message.trim().length === 0) {
                return;
            }

            // [PERFORMANCE] Deduplicação - não enviar mesmo log em 100ms
            const cacheKey = `${type}:${message}`;
            const now = Date.now();
            if (logCache.has(cacheKey)) {
                const lastTime = logCache.get(cacheKey);
                if (now - lastTime < CACHE_DURATION) {
                    return; // Log duplicado, ignora
                }
            }
            logCache.set(cacheKey, now);

            // [HELPER] Extrair informações do stack trace
            const stackInfo = getStackInfo();

            // [DADOS] Montar payload
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

            // [AJAX] Enviar para servidor (assíncrono, não bloqueia)
            fetch('/api/LogErros/LogConsole', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            }).catch(function (error) {
                // [SEGURANCA] Silenciar erros do próprio interceptador para evitar loops
                // Apenas loga no console original
                originalConsole.error('[FrotiX] Erro ao enviar log do console para servidor:', error);
            });

        } catch (error) {
            // [SEGURANCA] Nunca deixar o interceptador quebrar a aplicação
            originalConsole.error('[FrotiX] Erro no interceptador de console:', error);
        }
    }

    /**
     * ╭───────────────────────────────────────────────────────────────────────────────────────╮
     * │ ⚡ FUNCIONALIDADE: formatConsoleArgs                                                   │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 🎯 DESCRIÇÃO: Converte argumentos do console em string formatada.                     │
     * │              Suporta strings, objetos, arrays, erros, etc.                            │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 📥 INPUTS: args [Array] - Argumentos do console.log/warn/error                        │
     * │ 📤 OUTPUTS: [string] - Mensagem formatada                                             │
     * ╰───────────────────────────────────────────────────────────────────────────────────────╯
     */
    function formatConsoleArgs(args) {
        try {
            // [DADOS] Converter cada argumento em string
            return Array.from(args).map(function (arg) {
                if (arg === null) return 'null';
                if (arg === undefined) return 'undefined';
                if (typeof arg === 'string') return arg;
                if (typeof arg === 'number' || typeof arg === 'boolean') return String(arg);

                // [DADOS] Objetos e arrays -> JSON
                if (typeof arg === 'object') {
                    try {
                        // [REGRA] Se for Error, extrair mensagem e stack
                        if (arg instanceof Error) {
                            return `Error: ${arg.message}\nStack: ${arg.stack || 'N/A'}`;
                        }
                        // [DADOS] Outros objetos -> JSON limitado (evita circular reference)
                        return JSON.stringify(arg, null, 2).substring(0, 1000); // Limita a 1000 chars
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

    /**
     * ╭───────────────────────────────────────────────────────────────────────────────────────╮
     * │ ⚡ FUNCIONALIDADE: getStackInfo                                                        │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 🎯 DESCRIÇÃO: Extrai arquivo, método, linha e coluna do stack trace atual.            │
     * │              Ignora linhas do próprio interceptador.                                  │
     * │───────────────────────────────────────────────────────────────────────────────────────│
     * │ 📥 INPUTS: Nenhum - captura stack atual                                               │
     * │ 📤 OUTPUTS: {arquivo, metodo, linha, coluna, stack} - Informações do stack            │
     * ╰───────────────────────────────────────────────────────────────────────────────────────╯
     */
    function getStackInfo() {
        try {
            // [DADOS] Capturar stack trace
            const stack = new Error().stack || '';

            // [DADOS] Parsear stack trace
            const lines = stack.split('\n');

            // [LOGICA] Procurar primeira linha que NÃO seja do console-interceptor
            for (let i = 0; i < lines.length; i++) {
                const line = lines[i];

                // [REGRA] Ignorar linhas do próprio interceptador
                if (line.includes('console-interceptor.js')) continue;
                if (line.includes('at console.')) continue;
                if (line.includes('at sendConsoleLogToServer')) continue;
                if (line.includes('at interceptConsole')) continue;

                // [DADOS] Extrair informações usando regex
                // Formato Chrome: "at funcao (arquivo.js:123:45)"
                // Formato Firefox: "funcao@arquivo.js:123:45"
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

            // [DADOS] Se não conseguiu parsear, retorna apenas stack
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

    // [LOGICA] Inicializar interceptador quando script carregar
    interceptConsole();

    // [DADOS] Limpar cache periodicamente (evita vazamento de memória)
    setInterval(function () {
        const now = Date.now();
        for (const [key, time] of logCache.entries()) {
            if (now - time > CACHE_DURATION * 10) {
                logCache.delete(key);
            }
        }
    }, 30000); // A cada 30 segundos

})();
