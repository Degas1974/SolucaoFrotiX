/* ****************************************************************************************
 * ⚡ ARQUIVO: error_handler.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema unificado de tratamento de erros JavaScript com integração
 *                   Alerta.TratamentoErroComLinha. Captura erros globais, promises não
 *                   tratadas, enriquece com contexto e registra métricas em localStorage.
 * 📥 ENTRADAS     : Eventos window.error, unhandledrejection, chamadas ErrorHandler.capturar()
 * 📤 SAÍDAS       : Logs console, chamadas Alerta.TratamentoErroComLinha, metrics em localStorage
 * 🔗 CHAMADA POR  : Auto-execução (IIFE), global error handlers, código que chama ErrorHandler.capturar()
 * 🔄 CHAMA        : Alerta.TratamentoErroComLinha, console.*, localStorage API
 * 📦 DEPENDÊNCIAS : Alerta.js (window.Alerta), localStorage, navigator.userAgent
 * 📝 OBSERVAÇÕES  : IIFE auto-executável (initErrorHandler), expõe window.ErrorHandler,
 *                   tratamento AJAX manual (não global), log limitado a 50 erros
 *
 * 📋 ÍNDICE DE FUNÇÕES (11 funções principais + 2 event listeners):
 *
 * ┌─ ErrorHandler METHODS (window.ErrorHandler.*) ────────────────────────────┐
 * │ 1. capturar(origem, error, contexto)                                       │
 * │    → Handler central que enriquece e envia para Alerta.TratamentoErroComLinha│
 * │    → Log com console.group, extrai arquivo/função, cria erro enriquecido  │
 * │    → Parâmetros: origem (string), error (Error|Object), contexto (object) │
 * │                                                                             │
 * │ 2. criarErroEnriquecido(error, errorInfo, contexto)                        │
 * │    → Cria objeto Error enriquecido com contexto, origem, timestamp        │
 * │    → Preserva stack trace, adiciona detalhes AJAX se aplicável            │
 * │    → Retorna Error object com propriedades extras                         │
 * │                                                                             │
 * │ 3. extrairArquivo(error, contexto)                                         │
 * │    → Extrai nome do arquivo de origem do erro                             │
 * │    → Prioridades: contexto.filename → error.fileName → stack regex        │
 * │    → Fallback: 'agendamento_viagem.js'                                    │
 * │                                                                             │
 * │ 4. extrairFuncao(error, origem)                                            │
 * │    → Extrai nome da função do stack trace                                 │
 * │    → Regex: /at\s+(\w+)/ na segunda linha do stack                        │
 * │    → Fallback: origem (string)                                            │
 * │                                                                             │
 * │ 5. registrarMetrica(origem, errorInfo)                                     │
 * │    → Salva erro em localStorage (se DEBUG_MODE ativo)                     │
 * │    → Mantém apenas últimos 50 erros (FIFO)                                │
 * │    → Chave localStorage: 'erros_log'                                      │
 * │                                                                             │
 * │ 6. setContexto(contexto)                                                   │
 * │    → Define contexto atual (merge com contextoAtual)                      │
 * │    → Usado para enriquecer erros subsequentes                             │
 * │                                                                             │
 * │ 7. limparContexto()                                                        │
 * │    → Limpa contextoAtual (reset para {})                                  │
 * │                                                                             │
 * │ 8. obterLog()                                                              │
 * │    → Retorna array de erros do localStorage (parse 'erros_log')           │
 * │    → Fallback: [] em caso de erro                                         │
 * │                                                                             │
 * │ 9. limparLog()                                                             │
 * │    → Remove 'erros_log' do localStorage                                   │
 * │    → Log: "✅ Log de erros limpo"                                          │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ GLOBAL EVENT LISTENERS ───────────────────────────────────────────────────┐
 * │ 10. window 'error' listener                                                │
 * │     → Captura erros JavaScript globais não tratados                       │
 * │     → Filtra erros de bibliotecas externas (filename check)               │
 * │     → Chama ErrorHandler.capturar('global', error, contexto)              │
 * │                                                                             │
 * │ 11. window 'unhandledrejection' listener                                   │
 * │     → Captura Promise rejections não tratadas                             │
 * │     → event.preventDefault() para não logar no console nativo             │
 * │     → Chama ErrorHandler.capturar('promise', error, contexto)             │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE CAPTURA DE ERRO:
 * 1. Erro ocorre (global, promise, manual capturar())
 * 2. ErrorHandler.capturar() é chamado
 * 3. Enriquece erro com contexto, stack, timestamp, userAgent, URL
 * 4. Extrai arquivo e função (extrairArquivo, extrairFuncao)
 * 5. Cria erro enriquecido (criarErroEnriquecido)
 * 6. Envia para Alerta.TratamentoErroComLinha(arquivo, funcao, erro)
 * 7. Registra métrica em localStorage (se DEBUG_MODE)
 *
 * 📌 ERROR INFO STRUCTURE:
 * {
 *   origem: string,           // 'global', 'promise', 'ajax', etc.
 *   mensagem: string,          // error.message ou fallback
 *   stack: string,             // stack trace
 *   timestamp: ISO string,     // new Date().toISOString()
 *   userAgent: string,         // navigator.userAgent
 *   url: string,               // window.location.href
 *   contexto: object,          // contextoAtual + contexto param
 *   tipoRequisicao?: string,   // 'GET', 'POST', etc. (se AJAX)
 *   urlRequisicao?: string,    // URL do endpoint (se AJAX)
 *   statusCode?: number        // HTTP status (se AJAX)
 * }
 *
 * 📌 ENRICHED ERROR PROPERTIES:
 * - error.contexto (object)
 * - error.origem (string)
 * - error.timestamp (ISO string)
 * - error.detalhes.url (se AJAX)
 * - error.detalhes.method (se AJAX)
 * - error.detalhes.status (se AJAX)
 * - error.detalhes.statusText (se AJAX)
 * - error.detalhes.responseText (se AJAX)
 * - error.detalhes.serverMessage (se AJAX)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - AJAX error handling é MANUAL (não global), usar criarErroAjax() do alerta.js
 * - Filtro de bibliotecas externas (filename check em window.error)
 * - Debug mode: window.DEBUG_MODE habilita localStorage logging
 * - Limite de 50 erros em localStorage (FIFO queue)
 * - Exposto globalmente: window.ErrorHandler, window.AgendamentoViagens.errorHandler
 *
 * 🛡️ VERSÃO: Sem handler global de AJAX (tratamento manual)
 **************************************************************************************** */

(function initErrorHandler() 
{
    'use strict';

    /**
     * ErrorHandler Central - Unifica todos os tratamentos de erro
     * Integra com o sistema existente de Alerta.TratamentoErroComLinha
     */
    const ErrorHandler = {
        /**
         * Contexto adicional para enriquecer o erro
         */
        contextoAtual: {},

        /**
         * Handler central que enriquece e envia para Alerta.TratamentoErroComLinha
         * param {string} origem - Origem do erro (global, ajax, promise, etc)
         * param {Error|Object} error - Objeto de erro
         * param {Object} contexto - Contexto adicional
         */
        capturar: function (origem, error, contexto = {}) 
        {
            try 
            {
                console.group(`🔴 [ErrorHandler] Erro capturado - Origem: ${origem}`);

                // Construir informações detalhadas do erro
                const errorInfo = {
                    origem: origem,
                    mensagem: error.message || error.reason || error.erro || 'Erro desconhecido',
                    stack: error.stack || '',
                    timestamp: new Date().toISOString(),
                    userAgent: navigator.userAgent,
                    url: window.location.href,
                    contexto: { ...this.contextoAtual, ...contexto }
                };

                // Adicionar propriedades específicas do tipo de origem
                if (origem === 'ajax' && contexto.url) 
                {
                    errorInfo.tipoRequisicao = contexto.method || 'GET';
                    errorInfo.urlRequisicao = contexto.url;
                    errorInfo.statusCode = contexto.status;
                }

                // Logar informações completas no console
                console.log('📍 Detalhes do erro:', errorInfo);
                console.groupEnd();

                // Determinar o arquivo e função de origem
                let arquivo = this.extrairArquivo(error, contexto);
                let funcao = this.extrairFuncao(error, origem);

                // Criar objeto de erro enriquecido para Alerta.TratamentoErroComLinha
                const errorEnriquecido = this.criarErroEnriquecido(error, errorInfo, contexto);

                // Enviar para o sistema Alerta.TratamentoErroComLinha
                if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha) 
                {
                    console.log('📤 Enviando para Alerta.TratamentoErroComLinha');
                    Alerta.TratamentoErroComLinha(arquivo, funcao, errorEnriquecido);
                }
                else 
                {
                    console.error('❌ Alerta.TratamentoErroComLinha não disponível!');
                }

                // Registrar métrica
                this.registrarMetrica(origem, errorInfo);
            }
            catch (err) 
            {
                // Fallback caso o handler falhe
                console.error('❌ ERRO CRÍTICO no ErrorHandler:', err);
                console.error('Erro original:', error);
            }
        },

        /**
         * Cria objeto de erro enriquecido mantendo todas as propriedades
         */
        criarErroEnriquecido: function (error, errorInfo, contexto)
        {
            try {
                // Base do erro
                let errorEnriquecido;

                if (error instanceof Error)
                {
                    errorEnriquecido = error;
                }
                else if (typeof error === 'object' && error !== null)
                {
                    errorEnriquecido = new Error(errorInfo.mensagem);
                    // Copiar todas as propriedades do erro original
                    Object.assign(errorEnriquecido, error);
                }
                else
                {
                    errorEnriquecido = new Error(String(error));
                }

                // Enriquecer com contexto
                errorEnriquecido.contexto = errorInfo.contexto;
                errorEnriquecido.origem = errorInfo.origem;
                errorEnriquecido.timestamp = errorInfo.timestamp;

                // Adicionar detalhes específicos de AJAX
                if (contexto.url)
                {
                    errorEnriquecido.detalhes = errorEnriquecido.detalhes || {};
                    errorEnriquecido.detalhes.url = contexto.url;
                    errorEnriquecido.detalhes.method = contexto.method;
                    errorEnriquecido.detalhes.status = contexto.status;
                    errorEnriquecido.detalhes.statusText = contexto.statusText;
                    errorEnriquecido.detalhes.responseText = contexto.responseText;
                    errorEnriquecido.detalhes.serverMessage = contexto.serverMessage;
                }

                // Preservar stack
                if (!errorEnriquecido.stack && error.stack)
                {
                    errorEnriquecido.stack = error.stack;
                }

                return errorEnriquecido;
            } catch (erro) {
                console.error('Erro em criarErroEnriquecido:', erro);
                return new Error(errorInfo?.mensagem || 'Erro desconhecido');
            }
        },

        /**
         * Extrai arquivo do erro ou contexto
         */
        extrairArquivo: function (error, contexto)
        {
            try {
                // Prioridade 1: Arquivo do contexto
                if (contexto.filename) return contexto.filename;

                // Prioridade 2: Arquivo do erro
                if (error.fileName) return error.fileName;
                if (error.arquivo) return error.arquivo;
                if (error.detalhes?.arquivo) return error.detalhes.arquivo;

                // Prioridade 3: Extrair do stack
                if (error.stack)
                {
                    const match = error.stack.match(/(?:https?:)?\/\/[^\/]+\/(?:.*\/)?([\w\-_.]+\.(?:js|ts|jsx|tsx))/);
                    if (match) return match[1];
                }

                return 'agendamento_viagem.js';
            } catch (erro) {
                console.error('Erro em extrairArquivo:', erro);
                return 'agendamento_viagem.js';
            }
        },

        /**
         * Extrai função do erro
         */
        extrairFuncao: function (error, origem)
        {
            try {
                // Tentar extrair do stack
                if (error.stack)
                {
                    const lines = error.stack.split('\n');
                    if (lines.length > 1)
                    {
                        const match = lines[1].match(/at\s+(\w+)/);
                        if (match) return match[1];
                    }
                }

                return origem;
            } catch (erro) {
                console.error('Erro em extrairFuncao:', erro);
                return origem;
            }
        },

        /**
         * Registra métrica de erro (para análise futura)
         */
        registrarMetrica: function (origem, errorInfo) 
        {
            try 
            {
                // Salvar no localStorage para análise
                if (window.DEBUG_MODE && localStorage) 
                {
                    const erros = JSON.parse(localStorage.getItem('erros_log') || '[]');
                    erros.push({
                        origem,
                        info: errorInfo,
                        timestamp: Date.now()
                    });

                    // Manter apenas os últimos 50 erros
                    if (erros.length > 50) erros.shift();

                    localStorage.setItem('erros_log', JSON.stringify(erros));
                }
            }
            catch (error) 
            {
                // Falha silenciosa
            }
        },

        /**
         * Define contexto atual
         */
        setContexto: function (contexto)
        {
            try {
                this.contextoAtual = { ...this.contextoAtual, ...contexto };
            } catch (erro) {
                console.error('Erro em setContexto:', erro);
            }
        },

        /**
         * Limpa contexto
         */
        limparContexto: function ()
        {
            try {
                this.contextoAtual = {};
            } catch (erro) {
                console.error('Erro em limparContexto:', erro);
            }
        },

        /**
         * Obtém log de erros
         */
        obterLog: function () 
        {
            try 
            {
                return JSON.parse(localStorage.getItem('erros_log') || '[]');
            }
            catch (error) 
            {
                return [];
            }
        },

        /**
         * Limpa log de erros
         */
        limparLog: function () 
        {
            try 
            {
                localStorage.removeItem('erros_log');
                console.log('✅ Log de erros limpo');
            }
            catch (error) 
            {
                // Falha silenciosa
            }
        }
    };

    // ============================================================================
    // HANDLERS GLOBAIS INTEGRADOS
    // ============================================================================

    /**
     * Handler de erros JavaScript globais
     */
    window.addEventListener('error', function (event) 
    {
        try 
        {
            // Prevenir que erros de terceiros quebrem a aplicação
            if (event.filename && !event.filename.includes('agendamento_viagem.js')) 
            {
                console.warn('⚠️ Erro de biblioteca externa:', event.message);
                return;
            }

            // Construir contexto do erro
            const contexto = {
                filename: event.filename,
                lineno: event.lineno,
                colno: event.colno,
                tipo: 'JavaScript Error'
            };

            // Enviar para o handler central
            ErrorHandler.capturar('global', event.error || new Error(event.message), contexto);
        }
        catch (error) 
        {
            console.error('Erro no handler global de erros:', error);
        }
    });

    /**
     * Handler de Promises não capturadas
     */
    window.addEventListener('unhandledrejection', function (event) 
    {
        try 
        {
            // Prevenir que o erro seja jogado no console
            event.preventDefault();

            // Construir contexto
            const contexto = {
                promise: event.promise,
                tipo: 'Unhandled Promise Rejection'
            };

            // Criar objeto de erro
            const error = event.reason instanceof Error
                ? event.reason
                : new Error(String(event.reason));

            // Enviar para o handler central
            ErrorHandler.capturar('promise', error, contexto);
        }
        catch (error) 
        {
            console.error('Erro no handler de unhandledrejection:', error);
        }
    });

    // ============================================================================
    // ❌ HANDLER GLOBAL DE AJAX REMOVIDO
    // ============================================================================
    // 
    // O tratamento de erros AJAX é feito manualmente em cada $.ajax()
    // usando o helper criarErroAjax() disponível em alerta.js
    //
    // Padrío de uso:
    // $.ajax({
    //     error: function (jqXHR, textStatus, errorThrown) {
    //         const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
    //         Alerta.TratamentoErroComLinha("arquivo.js", "funcao", erro);
    //     }
    // });
    //
    // ============================================================================

    // ============================================================================
    // EXPOSIÇÃO GLOBAL
    // ============================================================================

    // Tornar ErrorHandler disponível globalmente
    window.ErrorHandler = ErrorHandler;

    // Adicionar ao namespace AgendamentoViagens se existir
    if (window.AgendamentoViagens) 
    {
        window.AgendamentoViagens.errorHandler = ErrorHandler;
    }

    // ============================================================================
    // LOG DE INICIALIZAÇÃO
    // ============================================================================

    console.log('%c🛡️ ErrorHandler Unificado Inicializado',
        'background: #dc3545; color: white; font-weight: bold; padding: 5px;');

    console.log('%c📡 AJAX: Tratamento manual com criarErroAjax()',
        'background: #007bff; color: white; padding: 3px;');

    if (window.DEBUG_MODE) 
    {
        console.log('📊 Para ver log de erros: ErrorHandler.obterLog()');
        console.log('🧹 Para limpar log: ErrorHandler.limparLog()');
    }

})();
