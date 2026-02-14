/* ****************************************************************************************
 * ⚡ ARQUIVO: alerta.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Wrapper CORE para SweetAlertInterop + sistema unificado de tratamento
 *                   de erros JavaScript. Provê API simplificada para alertas, confirmações
 *                   e logging de erros com envio automático ao servidor.
 * 📥 ENTRADAS     : Chamadas de funções (Alerta.Erro, .Sucesso, .TratamentoErroComLinha, etc)
 * 📤 SAÍDAS       : SweetAlert modals, console logs, POST /api/LogErros/LogJavaScript
 * 🔗 CHAMADA POR  : TODO O SISTEMA FrotiX (referenciado em TODOS os arquivos JavaScript)
 * 🔄 CHAMA        : SweetAlertInterop.*, ErrorHandler.*, fetch /api/LogErros/LogJavaScript
 * 📦 DEPENDÊNCIAS : SweetAlertInterop (sweetalert_interop.js), ErrorHandler (error_handler.js)
 * 📝 OBSERVAÇÕES  : DEVE ser carregado APÓS SweetAlertInterop. Integração automática
 *                   com ErrorHandler via polling (max 50 tentativas x 100ms = 5s).
 *
 * 📋 ÍNDICE DE FUNÇÕES (20 funções principais + helpers):
 *
 * ┌─ FEEDBACKS BÁSICOS (window.Alerta.*) ─────────────────────────────────────────┐
 * │ 1.  Alerta.Erro(titulo, texto, confirm)                                        │
 * │     → SweetAlertInterop.ShowError() - Modal de erro vermelho                  │
 * │                                                                                 │
 * │ 2.  Alerta.Sucesso(titulo, texto, confirm)                                     │
 * │     → SweetAlertInterop.ShowSuccess() - Modal de sucesso verde                │
 * │                                                                                 │
 * │ 3.  Alerta.Info(titulo, texto, confirm)                                        │
 * │     → SweetAlertInterop.ShowInfo() - Modal informativo azul                   │
 * │                                                                                 │
 * │ 4.  Alerta.Warning(titulo, texto, confirm)                                     │
 * │     → SweetAlertInterop.ShowWarning() - Modal de aviso amarelo                │
 * │                                                                                 │
 * │ 5.  Alerta.Alerta(titulo, texto, confirm)                                      │
 * │     → Alias para Alerta.Warning (compatibilidade)                             │
 * │                                                                                 │
 * │ 6.  Alerta.Confirmar(titulo, texto, confirm, cancel)                           │
 * │     → SweetAlertInterop.ShowConfirm() - Modal confirmação 2 botões            │
 * │     → Retorna Promise<boolean> (true = confirmou, false = cancelou)           │
 * │                                                                                 │
 * │ 7.  Alerta.Confirmar3(titulo, texto, buttonTodos, buttonAtual, buttonCancel)   │
 * │     → SweetAlertInterop.ShowConfirm3() - Modal confirmação 3 botões           │
 * │     → Retorna Promise<"todos"|"atual"|false>                                  │
 * │                                                                                 │
 * │ 8.  Alerta.ValidacaoIAConfirmar(titulo, mensagem, confirm, cancel)             │
 * │     → SweetAlertInterop.ShowValidacaoIAConfirmar() - Modal IA c/ badge        │
 * │     → Para análises estatísticas (Z-Score, histórico). Fallback: Confirmar()  │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ TRATAMENTO DE ERROS (window.Alerta.TratamentoErroComLinha) ──────────────────┐
 * │ 9.  TratamentoErroComLinha(classeOuArquivo, metodo, erro)                      │
 * │     → Handler PRINCIPAL de erros do sistema                                   │
 * │     → Extrai mensagem via extrairMensagem()                                   │
 * │     → Prepara objeto erro (string → Error, object → enriquecido)              │
 * │     → Envia para SweetAlertInterop.ShowErrorUnexpected()                      │
 * │     → Envia log para servidor via _enviarLogParaServidor()                    │
 * │                                                                                 │
 * │ 10. extrairMensagem(erro) [helper interno]                                     │
 * │     → Extrai mensagem de erro de múltiplas fontes                             │
 * │     → Prioridades: erro/message/mensagem/msg → toString() → JSON.stringify()  │
 * │     → Fallback: "Erro sem mensagem específica"                                │
 * │                                                                                 │
 * │ 11. _enviarLogParaServidor(arquivo, metodo, erroObj)                           │
 * │     → POST /api/LogErros/LogJavaScript (silencioso, background)               │
 * │     → Payload: mensagem, arquivo, metodo, linha, coluna, stack, userAgent, url│
 * │     → Não bloqueia execução, nunca lança exceção                              │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ HELPER AJAX (window.criarErroAjax) ──────────────────────────────────────────┐
 * │ 12. criarErroAjax(jqXHR, textStatus, errorThrown, ajaxSettings)                │
 * │     → Converte erro jQuery AJAX para objeto compatível com TratamentoErro     │
 * │     → Extrai: status, statusText, responseText, url, method, headers          │
 * │     → Tenta parsear JSON response para mensagem do servidor                   │
 * │     → Mensagens amigáveis por HTTP code (400, 401, 404, 500, etc.)            │
 * │     → Retorna objeto enriquecido com .message, .erro, .stack, .tipoErro       │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ INTEGRAÇÃO ERRORHANDLER (auto-execução polling) ─────────────────────────────┐
 * │ 13. integrarErrorHandler() [IIFE]                                              │
 * │     → Aguarda ErrorHandler estar disponível (polling 100ms, max 5s)           │
 * │     → Chama tentarIntegrar() recursivamente até sucesso                       │
 * │                                                                                 │
 * │ 14. tentarIntegrar() [helper interno]                                          │
 * │     → Verifica typeof ErrorHandler !== 'undefined'                            │
 * │     → Se disponível: cria funções adicionais e expõe no Alerta.*              │
 * │     → Se não: retry setTimeout(100ms) até maxTentativas (50)                  │
 * │                                                                                 │
 * │ 15. Alerta.TratamentoErroComLinhaEnriquecido(arquivo, funcao, erro, contexto)  │
 * │     → Criada pela integração                                                  │
 * │     → Adiciona contextoManual ao erro antes de chamar TratamentoErroComLinha  │
 * │                                                                                 │
 * │ 16. Alerta.setContextoGlobal(contexto)                                         │
 * │     → Criada pela integração                                                  │
 * │     → Chama ErrorHandler.setContexto(contexto)                                │
 * │                                                                                 │
 * │ 17. Alerta.limparContextoGlobal()                                              │
 * │     → Criada pela integração                                                  │
 * │     → Chama ErrorHandler.limparContexto()                                     │
 * │                                                                                 │
 * │ 18. Alerta.obterLogErros()                                                     │
 * │     → Criada pela integração                                                  │
 * │     → Chama ErrorHandler.obterLog(), retorna array de erros                   │
 * │                                                                                 │
 * │ 19. Alerta.limparLogErros()                                                    │
 * │     → Criada pela integração                                                  │
 * │     → Chama ErrorHandler.limparLog()                                          │
 * │                                                                                 │
 * │ 20. Alerta.criarErroAjax(...)                                                  │
 * │     → Criada pela integração (alias para window.criarErroAjax)                │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ UTILITÁRIOS ──────────────────────────────────────────────────────────────────┐
 * │ 21. callIf(fn, ...args)                                                        │
 * │     → Helper seguro para chamar funções (try-catch interno)                   │
 * │     → Retorna resultado da função ou undefined em caso de erro                │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE TRATAMENTO DE ERRO:
 * 1. Código chama Alerta.TratamentoErroComLinha(arquivo, metodo, erro)
 * 2. Logs extensivos no console (debug)
 * 3. Detecta tipo do erro (string, Error, object, primitivo)
 * 4. Extrai mensagem via extrairMensagem() (múltiplas fontes)
 * 5. Prepara erroObj com message, erro, stack, name, propriedades extras
 * 6. Envia log para servidor via _enviarLogParaServidor() (POST background)
 * 7. Exibe modal SweetAlertInterop.ShowErrorUnexpected(arquivo, metodo, erroObj)
 *
 * 🌐 ENDPOINT AJAX:
 * - POST /api/LogErros/LogJavaScript
 *   Body: { mensagem, arquivo, metodo, linha, coluna, stack, userAgent, url, timestamp }
 *   Origem: CLIENT_JS
 *   Silencioso: não bloqueia nem exibe erro se falhar
 *
 * 📦 OBJETO ERRO ENRIQUECIDO (criarErroAjax):
 * {
 *   message: string,           // Mensagem principal
 *   erro: string,              // Mensagem alternativa
 *   status: number,            // HTTP status code
 *   statusText: string,        // HTTP status text
 *   responseText: string,      // Corpo da resposta
 *   url: string,               // URL do endpoint
 *   method: string,            // GET/POST/PUT/DELETE
 *   textStatus: string,        // jQuery status
 *   readyState: number,        // XMLHttpRequest state
 *   tipoErro: 'AJAX',         // Identificador
 *   headers: string,           // Response headers
 *   serverMessage: string,     // Mensagem do servidor (se JSON)
 *   responseJson: object,      // Response parseado (se JSON)
 *   mensagemAmigavel: string,  // Mensagem user-friendly por código HTTP
 *   stack: string              // Stack trace sintético
 * }
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Logging EXTENSIVO no console (para debug)
 * - Compatibilidade: window.TratamentoErroComLinha = window.Alerta.TratamentoErroComLinha
 * - Fallbacks: se SweetAlertInterop não disponível, log no console
 * - Integração automática com ErrorHandler (polling assíncrono)
 * - Suporte a contexto adicional via TratamentoErroComLinhaEnriquecido
 * - Mensagens amigáveis por HTTP code (0, 400, 401, 403, 404, 408, 500, 502, 503, 504)
 * - Extração inteligente de mensagens do servidor (JSON ou HTML)
 *
 * 📌 VERSÃO: 2.0 (Padrão FrotiX Simplificado)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 08/01/2026
 * 📌 DOCUMENTAÇÃO EXTERNA: Documentacao/JavaScript/alerta.js.md
 **************************************************************************************** */

(function initAlerta()
{
    window.Alerta = window.Alerta || {};

    function callIf(fn, ...args)
    {
        try { if (typeof fn === "function") return fn(...args); }
        catch (e) { console.error("[Alerta] erro ao chamar função:", e); }
    }

    // ---- Feedbacks básicos ----
    window.Alerta.Erro = window.Alerta.Erro || function (titulo, texto, confirm = "OK")
    {
        if (window.SweetAlertInterop?.ShowError)
        {
            return SweetAlertInterop.ShowError(titulo, texto, confirm);
        }
        console.error("SweetAlertInterop.ShowError não está disponível.", titulo, texto);
        return Promise.resolve();
    };

    window.Alerta.Sucesso = window.Alerta.Sucesso || function (titulo, texto, confirm = "OK")
    {
        if (window.SweetAlertInterop?.ShowSuccess)
        {
            return SweetAlertInterop.ShowSuccess(titulo, texto, confirm);
        }
        console.error("SweetAlertInterop.ShowSuccess não está disponível.");
        return Promise.resolve();
    };

    window.Alerta.Info = window.Alerta.Info || function (titulo, texto, confirm = "OK")
    {
        if (window.SweetAlertInterop?.ShowInfo)
        {
            return SweetAlertInterop.ShowInfo(titulo, texto, confirm);
        }
        console.error("SweetAlertInterop.ShowInfo não está disponível.");
        return Promise.resolve();
    };

    window.Alerta.Warning = window.Alerta.Warning || function (titulo, texto, confirm = "OK")
    {
        if (window.SweetAlertInterop?.ShowWarning)
        {
            return SweetAlertInterop.ShowWarning(titulo, texto, confirm);
        }
        console.error("SweetAlertInterop.ShowWarning não está disponível.");
        return Promise.resolve();
    };

    window.Alerta.Alerta = window.Alerta.Alerta || function (titulo, texto, confirm = "OK")
    {
        return callIf(window.Alerta.Warning, titulo, texto, confirm);
    };

    window.Alerta.Confirmar = window.Alerta.Confirmar || function (titulo, texto, confirm = "Sim", cancel = "Cancelar")
    {
        if (window.SweetAlertInterop?.ShowConfirm)
        {
            return SweetAlertInterop.ShowConfirm(titulo, texto, confirm, cancel);
        }
        console.error("SweetAlertInterop.ShowConfirm não está disponível.");
        return Promise.resolve(false);
    };

    window.Alerta.Confirmar3 = window.Alerta.Confirmar3 || function (titulo, texto, buttonTodos = "Todos", buttonAtual = "Atual", buttonCancel = "Cancelar")
    {
        if (window.SweetAlertInterop?.ShowConfirm3)
        {
            return SweetAlertInterop.ShowConfirm3(titulo, texto, buttonTodos, buttonAtual, buttonCancel);
        }
        console.error("SweetAlertInterop.ShowConfirm3 não está disponível.");
        return Promise.resolve(false);
    };

    // ===== VALIDAÇÃO IA - Alerta para análises inteligentes baseadas em estatísticas =====

    /**
     * Alerta de confirmação da validação IA (com análise estatística)
     * Usa o bonequinho padrão + badge de IA
     * IMPORTANTE: Use apenas para análises complexas com Z-Score e histórico do veículo.
     *             Para erros simples (data futura, km final < inicial), use Alerta.Erro
     * @param {string} titulo - Título do alerta
     * @param {string} mensagem - Mensagem com análise detalhada (suporta HTML e \n)
     * @param {string} confirm - Texto do botão de confirmação
     * @param {string} cancel - Texto do botão de cancelamento
     * @returns {Promise<boolean>} true se confirmou, false se cancelou
     */
    window.Alerta.ValidacaoIAConfirmar = window.Alerta.ValidacaoIAConfirmar || function (titulo, mensagem, confirm = "Confirmar", cancel = "Corrigir")
    {
        if (window.SweetAlertInterop?.ShowValidacaoIAConfirmar)
        {
            return SweetAlertInterop.ShowValidacaoIAConfirmar(titulo, mensagem, confirm, cancel);
        }
        // Fallback para confirmação padrão
        console.warn("SweetAlertInterop.ShowValidacaoIAConfirmar não disponível, usando fallback.");
        return window.Alerta.Confirmar(titulo, mensagem, confirm, cancel);
    };

    // ===== FUNÇÃO MELHORADA: Tratamento de Erros =====
    function _TratamentoErroComLinha(classeOuArquivo, metodo, erro)
    {
        console.log('=== TratamentoErroComLinha INICIADO ===');
        console.log('Classe/Arquivo:', classeOuArquivo);
        console.log('Método:', metodo);
        console.log('Erro recebido:', erro);
        console.log('Tipo do erro:', typeof erro);
        console.log('É Error?', erro instanceof Error);
        console.log('Nome do erro:', erro?.name);
        console.log('Construtor:', erro?.constructor?.name);

        // Log todas as propriedades do erro
        if (erro && typeof erro === 'object')
        {
            console.log('Propriedades do erro:', Object.keys(erro));
            try
            {
                console.log('Erro completo JSON:', JSON.stringify(erro, Object.getOwnPropertyNames(erro), 2));
            } catch (e)
            {
                console.log('Não foi possível serializar o erro');
            }
        }

        // Verificar se SweetAlertInterop está disponível
        if (!window.SweetAlertInterop?.ShowErrorUnexpected)
        {
            console.error("SweetAlertInterop.ShowErrorUnexpected não está disponível!");
            console.error("Erro:", classeOuArquivo, metodo, erro);
            return Promise.resolve();
        }

        // ===== FUNÇÃO AUXILIAR: EXTRAIR MENSAGEM =====
        function extrairMensagem(erro)
        {
            // Tentar propriedades comuns primeiro
            const propriedadesMsg = [
                'erro', 'message', 'mensagem', 'msg', 'error',
                'errorMessage', 'description', 'statusText', 'detail'
            ];

            for (const prop of propriedadesMsg)
            {
                if (erro[prop] && typeof erro[prop] === 'string' && erro[prop].trim())
                {
                    console.log(`✓ Mensagem encontrada em '${prop}':`, erro[prop]);
                    return erro[prop];
                }
            }

            // Se não encontrou, tentar toString() do erro
            if (erro.toString && typeof erro.toString === 'function')
            {
                const strErro = erro.toString();
                if (strErro && strErro !== '[object Object]')
                {
                    console.log('✓ Mensagem extraída via toString():', strErro);
                    return strErro;
                }
            }

            // Última tentativa: serializar o objeto
            try
            {
                const serializado = JSON.stringify(erro, null, 2);
                if (serializado && serializado !== '{}' && serializado !== 'null')
                {
                    console.log('✓ Mensagem serializada:', serializado);
                    return `Erro: ${serializado}`;
                }
            } catch (e)
            {
                console.error('Erro ao serializar:', e);
            }

            return 'Erro sem mensagem específica';
        }

        // ===== PREPARAR OBJETO DE ERRO =====
        let erroObj;

        if (typeof erro === 'string')
        {
            // String simples
            const tempError = new Error(erro);
            erroObj = {
                message: erro,
                erro: erro,
                stack: tempError.stack,
                name: 'Error'
            };
            console.log('✓ Erro string convertido para objeto');
        }
        else if (erro instanceof Error || erro?.constructor?.name === 'Error' ||
            erro?.constructor?.name?.endsWith('Error')) // SyntaxError, TypeError, etc
        {
            // Error nativo ou derivado
            const mensagem = erro.message || extrairMensagem(erro);

            erroObj = {
                message: mensagem,
                erro: mensagem,
                stack: erro.stack || new Error(mensagem).stack,
                name: erro.name || 'Error',
                // Preservar propriedades específicas de erro
                ...(erro.fileName && { arquivo: erro.fileName }),
                ...(erro.lineNumber && { linha: erro.lineNumber }),
                ...(erro.columnNumber && { coluna: erro.columnNumber })
            };
            console.log('✓ Erro Error object processado, mensagem:', mensagem);
        }
        else if (typeof erro === 'object' && erro !== null)
        {
            // Objeto genérico
            const mensagemExtraida = extrairMensagem(erro);

            erroObj = {
                message: mensagemExtraida,
                erro: mensagemExtraida,
                stack: erro.stack || new Error(mensagemExtraida).stack,
                name: erro.name || 'Error',
                // Preservar TODAS as propriedades originais
                ...erro
            };

            console.log('✓ Erro object processado, mensagem extraída:', mensagemExtraida);
        }
        else
        {
            // Fallback para outros tipos
            const errorStr = String(erro || 'Erro desconhecido');
            const tempError = new Error(errorStr);
            erroObj = {
                message: errorStr,
                erro: errorStr,
                stack: tempError.stack,
                name: 'Error'
            };
            console.log('✓ Erro fallback criado');
        }

        // Log final para debug
        console.log('📦 Objeto de erro final que será enviado:');
        console.log('  - message:', erroObj.message);
        console.log('  - erro:', erroObj.erro);
        console.log('  - name:', erroObj.name);
        console.log('  - stack presente?', !!erroObj.stack);
        console.log('  - Objeto completo:', erroObj);
        console.log('=== TratamentoErroComLinha ENVIANDO ===');

        // ===== ENVIAR LOG PARA O SERVIDOR (fetch silencioso) =====
        // Não bloqueia a exibição do SweetAlert, envia em background
        _enviarLogParaServidor(classeOuArquivo, metodo, erroObj);

        return SweetAlertInterop.ShowErrorUnexpected(classeOuArquivo, metodo, erroObj);
    }

    /**
     * Envia o erro para o servidor via POST /api/LogErros/LogJavaScript
     * Executa em background, silenciosamente (não bloqueia nem exibe erro adicional)
     * ORIGEM: CLIENT_JS
     * @param {string} arquivo - Nome do arquivo JS
     * @param {string} metodo - Nome da função/método
     * @param {object} erroObj - Objeto de erro preparado
     */
    function _enviarLogParaServidor(arquivo, metodo, erroObj)
    {
        try
        {
            // Preparar payload para o endpoint
            const payload = {
                mensagem: erroObj.message || erroObj.erro || 'Erro JavaScript',
                arquivo: arquivo || 'desconhecido.js',
                metodo: metodo || 'desconhecido',
                linha: erroObj.linha || erroObj.lineNumber || null,
                coluna: erroObj.coluna || erroObj.columnNumber || null,
                stack: erroObj.stack || null,
                userAgent: navigator.userAgent,
                url: window.location.href,
                timestamp: new Date().toISOString()
            };

            // Fetch silencioso - não bloqueia nem exibe erro
            fetch('/api/LogErros/LogJavaScript', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(payload)
            })
            .then(response => {
                if (response.ok)
                {
                    console.log('✅ [Alerta] Erro enviado para o servidor com sucesso');
                }
                else
                {
                    console.warn('⚠️ [Alerta] Falha ao enviar erro para servidor:', response.status);
                }
            })
            .catch(err => {
                // Silencioso - não propagamos erro de log
                console.warn('⚠️ [Alerta] Não foi possível enviar erro para servidor:', err.message);
            });
        }
        catch (ex)
        {
            // Silencioso - nunca deve atrapalhar o fluxo principal
            console.warn('⚠️ [Alerta] Exceção ao preparar envio de log:', ex.message);
        }
    }

    // Exportar a função
    window.Alerta.TratamentoErroComLinha = window.Alerta.TratamentoErroComLinha || _TratamentoErroComLinha;
    window.TratamentoErroComLinha = window.TratamentoErroComLinha || _TratamentoErroComLinha;

    console.log('[Alerta] Módulo inicializado com sucesso');
})();

// ============================================================================
// HELPER PARA ERROS AJAX
// ============================================================================

/**
 * Converte erro AJAX para objeto compatível com TratamentoErroComLinha
 * param {Object} jqXHR - Objeto jQuery XHR
 * param {string} textStatus - Status do erro
 * param {string} errorThrown - Exceção lançada
 * param {Object} ajaxSettings - Configurações do AJAX (use 'this' no callback)
 * returns {Object} Objeto de erro enriquecido
 * 
 * @example
 * $.ajax({
 *     url: "/api/endpoint",
 *     error: function(jqXHR, textStatus, errorThrown) {
 *         const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
 *         Alerta.TratamentoErroComLinha("meuArquivo.js", "minhaFuncao", erro);
 *     }
 * });
 */
window.criarErroAjax = function (jqXHR, textStatus, errorThrown, ajaxSettings = {}) 
{
    const erro = {
        message: errorThrown || textStatus || "Erro na requisição AJAX",
        erro: errorThrown || textStatus || "Erro na requisição",
        status: jqXHR.status,
        statusText: jqXHR.statusText,
        responseText: jqXHR.responseText,
        url: ajaxSettings.url || "URL não disponível",
        method: ajaxSettings.type || "GET",
        textStatus: textStatus,
        readyState: jqXHR.readyState,
        tipoErro: 'AJAX'
    };

    // Tentar obter headers
    try 
    {
        erro.headers = jqXHR.getAllResponseHeaders();
    }
    catch (e) 
    {
        // Headers não disponíveis
    }

    // Tentar extrair mensagem do servidor
    try 
    {
        const responseJson = JSON.parse(jqXHR.responseText);
        erro.serverMessage = responseJson.message || responseJson.error || responseJson.Message;
        erro.responseJson = responseJson;

        // Se o servidor enviou uma mensagem, usar ela como principal
        if (erro.serverMessage) 
        {
            erro.message = erro.serverMessage;
            erro.erro = erro.serverMessage;
        }
    }
    catch (e) 
    {
        // Resposta não é JSON - tentar extrair HTML ou texto
        if (jqXHR.responseText && jqXHR.responseText.length > 0) 
        {
            // Se for HTML, extrair apenas texto
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = jqXHR.responseText;
            const textoExtraido = tempDiv.textContent || tempDiv.innerText || "";

            // Limitar tamanho para não poluir o erro (primeiros 500 caracteres)
            if (textoExtraido.trim()) 
            {
                erro.serverMessage = textoExtraido.substring(0, 500);
            }
        }
    }

    // Criar stack trace sintético
    erro.stack = new Error(erro.message).stack;

    // Adicionar informações de timeout se aplicável
    if (textStatus === 'timeout') 
    {
        erro.message = `Timeout: A requisição para ${erro.url} demorou muito para responder`;
        erro.erro = erro.message;
    }

    // Adicionar informações de abort se aplicável
    if (textStatus === 'abort') 
    {
        erro.message = `Abort: A requisição para ${erro.url} foi cancelada`;
        erro.erro = erro.message;
    }

    // Mensagens amigáveis por código HTTP
    if (!erro.serverMessage) 
    {
        const mensagensPorStatus = {
            0: 'Sem conexão com o servidor',
            400: 'Requisição inválida',
            401: 'Não autorizado - faça login novamente',
            403: 'Acesso negado',
            404: 'Recurso não encontrado',
            408: 'Tempo de requisição esgotado',
            500: 'Erro interno do servidor',
            502: 'Gateway inválido',
            503: 'Serviço temporariamente indisponível',
            504: 'Gateway timeout'
        };

        const mensagemAmigavel = mensagensPorStatus[erro.status];
        if (mensagemAmigavel) 
        {
            erro.mensagemAmigavel = mensagemAmigavel;
        }
    }

    console.log('📡 [criarErroAjax] Erro AJAX enriquecido:', erro);

    return erro;
};

// ============================================================================
// INTEGRAÇÃO COM ERRORHANDLER
// ============================================================================

/**
 * Integração com ErrorHandler Unificado
 * Aguarda ErrorHandler estar disponível e cria funções de conveniência
 */
(function integrarErrorHandler()
{
    try {
        let tentativas = 0;
        const maxTentativas = 50; // 5 segundos (50 x 100ms)

        function tentarIntegrar()
        {
            try {
                tentativas++;

                if (typeof ErrorHandler !== 'undefined')
                {
                    console.log('✅ [Alerta] Integrado com ErrorHandler');

                    // Expor criarErroAjax também no namespace Alerta
                    window.Alerta.criarErroAjax = window.criarErroAjax;

                    // Criar função de conveniência para contexto adicional
                    window.Alerta.TratamentoErroComLinhaEnriquecido = function (arquivo, funcao, erro, contextoAdicional = {})
                    {
                        try {
                            // Se vier com contexto adicional, enriquecer o erro
                            if (contextoAdicional && Object.keys(contextoAdicional).length > 0)
                            {
                                // Se erro for objeto, adicionar contexto
                                if (typeof erro === 'object' && erro !== null)
                                {
                                    erro.contextoManual = contextoAdicional;
                                }
                                else
                                {
                                    // Se for string ou primitivo, criar objeto
                                    const mensagem = String(erro);
                                    erro = {
                                        message: mensagem,
                                        erro: mensagem,
                                        contextoManual: contextoAdicional,
                                        stack: new Error(mensagem).stack
                                    };
                                }
                            }

                            // Chamar o tratamento original
                            return window.Alerta.TratamentoErroComLinha(arquivo, funcao, erro);
                        } catch (erro) {
                            console.error('Erro em TratamentoErroComLinhaEnriquecido:', erro);
                            return Promise.resolve();
                        }
                    };

                    // Expor função para definir contexto global
                    window.Alerta.setContextoGlobal = function (contexto)
                    {
                        try {
                            if (ErrorHandler && ErrorHandler.setContexto)
                            {
                                ErrorHandler.setContexto(contexto);
                            }
                        } catch (erro) {
                            console.error('Erro em setContextoGlobal:', erro);
                        }
                    };

                    // Expor função para limpar contexto global
                    window.Alerta.limparContextoGlobal = function ()
                    {
                        try {
                            if (ErrorHandler && ErrorHandler.limparContexto)
                            {
                                ErrorHandler.limparContexto();
                            }
                        } catch (erro) {
                            console.error('Erro em limparContextoGlobal:', erro);
                        }
                    };

                    // Expor função para obter log de erros
                    window.Alerta.obterLogErros = function ()
                    {
                        try {
                            if (ErrorHandler && ErrorHandler.obterLog)
                            {
                                return ErrorHandler.obterLog();
                            }
                            return [];
                        } catch (erro) {
                            console.error('Erro em obterLogErros:', erro);
                            return [];
                        }
                    };

                    // Expor função para limpar log de erros
                    window.Alerta.limparLogErros = function ()
                    {
                        try {
                            if (ErrorHandler && ErrorHandler.limparLog)
                            {
                                ErrorHandler.limparLog();
                            }
                        } catch (erro) {
                            console.error('Erro em limparLogErros:', erro);
                        }
                    };

                    console.log('📋 [Alerta] Funções adicionais disponíveis:');
                    console.log('  - Alerta.criarErroAjax(jqXHR, textStatus, errorThrown, ajaxSettings)');
                    console.log('  - Alerta.TratamentoErroComLinhaEnriquecido(arquivo, funcao, erro, contexto)');
                    console.log('  - Alerta.setContextoGlobal(contexto)');
                    console.log('  - Alerta.limparContextoGlobal()');
                    console.log('  - Alerta.obterLogErros()');
                    console.log('  - Alerta.limparLogErros()');
                }
                else if (tentativas < maxTentativas)
                {
                    // Tentar novamente em 100ms
                    setTimeout(tentarIntegrar, 100);
                }
                else
                {
                    console.warn('⚠️ [Alerta] ErrorHandler não foi carregado após 5 segundos');
                    console.warn('   Certifique-se de que error_handler.js está sendo carregado');
                }
            } catch (erro) {
                console.error('Erro em tentarIntegrar:', erro);
            }
        }

        // Iniciar tentativas de integração
        tentarIntegrar();
    } catch (erro) {
        console.error('Erro em integrarErrorHandler:', erro);
    }
})();

// ============================================================================
// LOG FINAL
// ============================================================================

console.log('%c[Alerta] Sistema completo carregado',
    'background: #28a745; color: white; font-weight: bold; padding: 5px; border-radius: 3px;');
