/* ****************************************************************************************
 * ⚡ ARQUIVO: ajax-helper.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Utilitário para criar objetos de erro enriquecidos a partir de falhas
 *                   de requisições jQuery AJAX. Converte jqXHR error callback em Error
 *                   object estruturado com informações detalhadas (status HTTP, URL,
 *                   método, response text/JSON, timestamp, mensagem amigável para usuário).
 *                   Mapeia códigos de status HTTP e textStatus para mensagens em pt-BR.
 * 📥 ENTRADAS     : jqXHR (jQuery XMLHttpRequest object), textStatus (string: timeout/error/
 *                   abort/parsererror), errorThrown (string), ajaxSettings (Object `this`
 *                   do $.ajax com url e type)
 * 📤 SAÍDAS       : Error object com propriedade .ajax contendo {status, statusText,
 *                   errorThrown, url, method, responseText, responseJSON, timestamp,
 *                   mensagemUsuario, mensagem completa formatada}
 * 🔗 CHAMADA POR  : Todos os $.ajax error callbacks em services (agendamento.service.js,
 *                   viagem.service.js, evento.service.js, etc.), api-client.js, event
 *                   handlers, componentes
 * 🔄 CHAMA        : JSON.parse (try-catch), new Date().toISOString(), new Error(),
 *                   string templates (template literals)
 * 📦 DEPENDÊNCIAS : Nenhuma (pure JavaScript, não depende de jQuery runtime)
 * 📝 OBSERVAÇÕES  : Exporta window.criarErroAjax (função global). Try-catch completo com
 *                   fallback para erro básico. Tenta parsear responseText como JSON
 *                   (silenciosamente falha se não for JSON). Mensagens amigáveis em pt-BR
 *                   para 8 status codes + 4 textStatus types. Usado em conjunto com
 *                   Alerta.TratamentoErroComLinha para logging padronizado.
 *
 * 📋 ÍNDICE DE FUNÇÕES (1 função global window.*):
 *
 * ┌─ FUNÇÃO PRINCIPAL ────────────────────────────────────────────────────┐
 * │ window.criarErroAjax(jqXHR, textStatus, errorThrown, ajaxSettings)   │
 * │ → Cria Error object enriquecido com informações AJAX                 │
 * │ → Estrutura do objeto erro.ajax:                                     │
 * │   • status: jqXHR.status || 0                                        │
 * │   • statusText: jqXHR.statusText || textStatus || 'Erro desconhecido'│
 * │   • errorThrown: errorThrown || 'Sem detalhes'                       │
 * │   • url: ajaxSettings?.url || 'URL não disponível'                   │
 * │   • method: ajaxSettings?.type || 'GET'                              │
 * │   • responseText: jqXHR.responseText || ''                           │
 * │   • responseJSON: parsed JSON ou null (try-catch)                    │
 * │   • timestamp: new Date().toISOString()                              │
 * │   • mensagemUsuario: mensagem pt-BR baseada em status/textStatus     │
 * │ → Switch por erro.status:                                            │
 * │   • 0: "Sem conexão com o servidor. Verifique sua internet."        │
 * │   • 400: "Dados inválidos enviados ao servidor."                    │
 * │   • 401: "Sessão expirada. Por favor, faça login novamente."        │
 * │   • 403: "Você não tem permissão para esta operação."               │
 * │   • 404: "Recurso não encontrado no servidor."                      │
 * │   • 500: "Erro interno do servidor. Tente novamente mais tarde."    │
 * │   • 503: "Servidor temporariamente indisponível."                   │
 * │   • default: switch por textStatus:                                  │
 * │     - 'timeout': "A operação demorou muito. Tente novamente."       │
 * │     - 'abort': "Operação cancelada."                                │
 * │     - 'parsererror': "Erro ao processar resposta do servidor."      │
 * │     - outros: "Erro ao comunicar com servidor (${status})."         │
 * │ → Constrói mensagemCompleta:                                         │
 * │   "Status: {status} - {statusText}\nURL: {url}\nMétodo: {method}\n  │
 * │    Mensagem: {mensagemUsuario}"                                      │
 * │ → Cria Error object: new Error(mensagemCompleta)                     │
 * │ → Adiciona propriedade: errorObj.ajax = erro (objeto completo)       │
 * │ → returns errorObj                                                   │
 * │ → try-catch outer: se falhar, retorna Error("Erro AJAX: {textStatus}")│
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE CRIAÇÃO DE ERRO:
 * 1. $.ajax error callback dispara: function(jqXHR, textStatus, errorThrown)
 * 2. Chama criarErroAjax(jqXHR, textStatus, errorThrown, this)
 * 3. Cria objeto erro com todos os campos básicos
 * 4. Try-catch: tenta parsear jqXHR.responseText como JSON
 *    a. Se sucesso: erro.responseJSON = parsed object
 *    b. Se falha: erro.responseJSON = null (silencioso)
 * 5. Switch por erro.status para definir mensagemUsuario
 * 6. Se não match: switch por textStatus (timeout, abort, parsererror)
 * 7. Constrói mensagemCompleta com join('\n')
 * 8. Cria Error object com mensagem completa
 * 9. Adiciona errorObj.ajax = erro (objeto detalhado)
 * 10. Retorna errorObj
 * 11. Chamador usa: Alerta.TratamentoErroComLinha("arquivo.js", "funcao", errorObj)
 *
 * 🔄 EXEMPLO DE USO (pattern padrão):
 * $.ajax({
 *     url: "/api/Viagem/Salvar",
 *     type: "POST",
 *     data: JSON.stringify(dados),
 *     contentType: "application/json",
 *     success: function(response) {
 *         console.log("Sucesso:", response);
 *     },
 *     error: function(jqXHR, textStatus, errorThrown) {
 *         const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
 *         Alerta.TratamentoErroComLinha("viagem.service.js", "salvar", erro);
 *         // erro.ajax.mensagemUsuario pode ser exibido ao usuário
 *         // erro.ajax.status, erro.ajax.url, etc. para debugging
 *     }
 * });
 *
 * 📌 ESTRUTURA DO OBJETO ERRO RETORNADO:
 * Error {
 *   message: "Status: 500 - Internal Server Error\nURL: /api/Viagem/Salvar\n
 *             Método: POST\nMensagem: Erro interno do servidor...",
 *   ajax: {
 *     status: 500,
 *     statusText: "Internal Server Error",
 *     errorThrown: "Internal Server Error",
 *     url: "/api/Viagem/Salvar",
 *     method: "POST",
 *     responseText: "{\"error\":\"Database connection failed\"}",
 *     responseJSON: { error: "Database connection failed" },
 *     timestamp: "2026-02-01T12:34:56.789Z",
 *     mensagemUsuario: "Erro interno do servidor. Tente novamente mais tarde."
 *   }
 * }
 *
 * 📌 STATUS CODES MAPEADOS (8 codes):
 * - 0: Sem conexão (network error, CORS, servidor offline)
 * - 400: Bad Request (dados inválidos)
 * - 401: Unauthorized (sessão expirada, não autenticado)
 * - 403: Forbidden (sem permissão)
 * - 404: Not Found (endpoint não existe)
 * - 500: Internal Server Error (erro no backend)
 * - 503: Service Unavailable (servidor sobrecarregado)
 * - outros: mensagem genérica com status code
 *
 * 📌 TEXT STATUS MAPEADOS (4 types):
 * - 'timeout': requisição excedeu timeout configurado
 * - 'abort': requisição cancelada programaticamente
 * - 'parsererror': resposta não é JSON válido (quando dataType='json')
 * - 'error': erro genérico (status HTTP não-2xx)
 *
 * 📌 PARSING DE JSON:
 * - Tenta JSON.parse(jqXHR.responseText)
 * - Se sucesso: responseJSON = parsed object (ex: {error: "msg"})
 * - Se falha: responseJSON = null (resposta HTML, texto, ou vazio)
 * - Try-catch silencioso (não loga, apenas ignora)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Optional chaining: ajaxSettings?.url (suporta ajaxSettings=undefined)
 * - Fallbacks em todos os campos: || 'default value'
 * - Timestamp ISO 8601: "2026-02-01T12:34:56.789Z" (UTC)
 * - mensagemUsuario útil para exibir ao usuário final (não técnica)
 * - mensagemCompleta útil para logs e debugging (técnica)
 * - errorObj é Error nativo (instanceof Error === true)
 * - errorObj.ajax contém todos os detalhes estruturados
 * - Usado em TODOS os $.ajax error callbacks do projeto
 * - Pattern comum: criarErroAjax + Alerta.TratamentoErroComLinha
 * - Não depende de jQuery para funcionar (apenas para tipo jqXHR)
 * - Try-catch outer protege contra falhas na própria função
 * - Fallback error: new Error("Erro AJAX: {textStatus}")
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Cria objeto de erro detalhado a partir de falha AJAX
 * param {jqXHR} jqXHR - Objeto jQuery XHR
 * param {string} textStatus - Status textual (timeout, error, abort, parsererror)
 * param {string} errorThrown - Erro lançado
 * param {Object} ajaxSettings - Configurações da chamada AJAX (this)
 * returns {Object} Objeto de erro enriquecido
 */
window.criarErroAjax = function (jqXHR, textStatus, errorThrown, ajaxSettings)
{
    try
    {
        const erro = {
            // Informações básicas
            status: jqXHR.status || 0,
            statusText: jqXHR.statusText || textStatus || 'Erro desconhecido',
            errorThrown: errorThrown || 'Sem detalhes',

            // Informações da requisição
            url: ajaxSettings?.url || 'URL não disponível',
            method: ajaxSettings?.type || 'GET',

            // Response
            responseText: jqXHR.responseText || '',
            responseJSON: null,

            // Timestamp
            timestamp: new Date().toISOString(),

            // Mensagem amigável
            mensagemUsuario: ''
        };

        // Tentar parsear JSON da resposta
        try
        {
            if (jqXHR.responseText)
            {
                erro.responseJSON = JSON.parse(jqXHR.responseText);
            }
        } catch (e)
        {
            // Não é JSON, tudo bem
        }

        // Definir mensagem amigável baseada no status
        switch (erro.status)
        {
            case 0:
                erro.mensagemUsuario = 'Sem conexão com o servidor. Verifique sua internet.';
                break;
            case 400:
                erro.mensagemUsuario = 'Dados inválidos enviados ao servidor.';
                break;
            case 401:
                erro.mensagemUsuario = 'Sessão expirada. Por favor, faça login novamente.';
                break;
            case 403:
                erro.mensagemUsuario = 'Você não tem permissão para esta operação.';
                break;
            case 404:
                erro.mensagemUsuario = 'Recurso não encontrado no servidor.';
                break;
            case 500:
                erro.mensagemUsuario = 'Erro interno do servidor. Tente novamente mais tarde.';
                break;
            case 503:
                erro.mensagemUsuario = 'Servidor temporariamente indisponível.';
                break;
            default:
                if (textStatus === 'timeout')
                {
                    erro.mensagemUsuario = 'A operação demorou muito. Tente novamente.';
                } else if (textStatus === 'abort')
                {
                    erro.mensagemUsuario = 'Operação cancelada.';
                } else if (textStatus === 'parsererror')
                {
                    erro.mensagemUsuario = 'Erro ao processar resposta do servidor.';
                } else
                {
                    erro.mensagemUsuario = `Erro ao comunicar com o servidor (${erro.status}).`;
                }
        }

        // Construir mensagem de erro completa
        const mensagemCompleta = [
            `Status: ${erro.status} - ${erro.statusText}`,
            `URL: ${erro.url}`,
            `Método: ${erro.method}`,
            `Mensagem: ${erro.mensagemUsuario}`
        ].join('\n');

        // Retornar objeto Error com propriedades adicionais
        const errorObj = new Error(mensagemCompleta);
        errorObj.ajax = erro;

        return errorObj;

    } catch (e)
    {
        // Se falhar ao criar o erro, retornar erro básico
        return new Error(`Erro AJAX: ${textStatus || 'Desconhecido'}`);
    }
};

/**
 * Exemplo de uso:
 * 
 * $.ajax({
 *     url: "/api/endpoint",
 *     type: "POST",
 *     success: function(data) {
 *         // ...
 *     },
 *     error: function(jqXHR, textStatus, errorThrown) {
 *         const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
 *         Alerta.TratamentoErroComLinha("arquivo.js", "nomeFuncao", erro);
 *     }
 * });
 */
