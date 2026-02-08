/* ****************************************************************************************
 * ⚡ ARQUIVO: api-client.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Cliente HTTP centralizado para comunicação com API. Wrapper sobre
 *                   jQuery.ajax e fetch nativo, com tratamento de erros padronizado e
 *                   headers default (Content-Type: application/json).
 * 📥 ENTRADAS     : URL endpoint (string), params/data (Object), options (Object fetch)
 * 📤 SAÍDAS       : Promises resolvidas com response.json() ou rejeitadas com erro,
 *                   Alerta.TratamentoErroComLinha em caso de erro
 * 🔗 CHAMADA POR  : Serviços de agendamento (services/*.js), componentes (components/*.js)
 * 🔄 CHAMA        : $.ajax (jQuery), fetch nativo, criarErroAjax (helper externo),
 *                   Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery ($.ajax), fetch API nativa, criarErroAjax function, Alerta.js
 * 📝 OBSERVAÇÕES  : Exporta window.ApiClient (instância global). Todos os métodos async
 *                   retornam Promises. Métodos GET/POST/PUT/DELETE usam jQuery.ajax,
 *                   método fetch() usa fetch nativo. Todos têm try-catch completo.
 *
 * 📋 ÍNDICE DE MÉTODOS DA CLASSE (6 métodos + constructor):
 *
 * ┌─ CONSTRUCTOR ─────────────────────────────────────────────────────────────┐
 * │ 1. constructor(baseUrl = '')                                             │
 * │    → Inicializa this.baseUrl (string, default vazio)                    │
 * │    → Inicializa this.defaultHeaders:                                     │
 * │      { 'Content-Type': 'application/json; charset=utf-8' }              │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MÉTODOS HTTP (jQuery.ajax) ─────────────────────────────────────────────┐
 * │ 2. async get(url, params = {})                                          │
 * │    → Retorna new Promise wrapping $.ajax                                │
 * │    → Config: type='GET', data=params, dataType='json'                   │
 * │    → success: resolve(data)                                             │
 * │    → error: reject(criarErroAjax(jqXHR, textStatus, errorThrown))      │
 * │    → try-catch: Alerta.TratamentoErroComLinha("api-client.js", "get")  │
 * │                                                                          │
 * │ 3. async post(url, data = {})                                           │
 * │    → Retorna new Promise wrapping $.ajax                                │
 * │    → Config: type='POST', contentType=defaultHeaders, dataType='json'  │
 * │    → data: JSON.stringify(data)                                         │
 * │    → success: resolve(response)                                         │
 * │    → error: reject(criarErroAjax(...))                                 │
 * │    → try-catch: Alerta.TratamentoErroComLinha("api-client.js", "post") │
 * │                                                                          │
 * │ 4. async put(url, data = {})                                            │
 * │    → Retorna new Promise wrapping $.ajax                                │
 * │    → Config: type='PUT', contentType, dataType='json'                  │
 * │    → data: JSON.stringify(data)                                         │
 * │    → success: resolve(response)                                         │
 * │    → error: reject(criarErroAjax(...))                                 │
 * │    → try-catch: Alerta.TratamentoErroComLinha("api-client.js", "put")  │
 * │                                                                          │
 * │ 5. async delete(url, params = {})                                       │
 * │    → Retorna new Promise wrapping $.ajax                                │
 * │    → Config: type='DELETE', data=params, dataType='json'               │
 * │    → success: resolve(response)                                         │
 * │    → error: reject(criarErroAjax(...))                                 │
 * │    → try-catch: Alerta.TratamentoErroComLinha("delete")                │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MÉTODO FETCH NATIVO ────────────────────────────────────────────────────┐
 * │ 6. async fetch(url, options = {})                                       │
 * │    → Usa fetch nativo (window.fetch)                                    │
 * │    → Config: url = baseUrl + url                                        │
 * │    →         headers = { ...defaultHeaders, ...options.headers }       │
 * │    → Se !response.ok: throw Error(`HTTP ${status}: ${statusText}`)     │
 * │    → Retorna await response.json()                                      │
 * │    → try-catch: Alerta.TratamentoErroComLinha("api-client.js", "fetch")│
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE USO TÍPICO (GET):
 * 1. Chamada: ApiClient.get('/api/viagens', { status: 'Aberta' })
 * 2. $.ajax({ url: baseUrl + '/api/viagens', type: 'GET', data: { status: 'Aberta' } })
 * 3. success: resolve(data) → Promise resolvida com dados JSON
 * 4. error: criarErroAjax → reject(erro) → Promise rejeitada
 * 5. try-catch externo: Alerta.TratamentoErroComLinha + throw
 *
 * 🔄 FLUXO DE USO TÍPICO (POST):
 * 1. Chamada: ApiClient.post('/api/viagens', { titulo: "Nova viagem" })
 * 2. $.ajax({ url, type: 'POST', data: JSON.stringify({...}), contentType: 'application/json' })
 * 3. success: resolve(response) → Promise resolvida
 * 4. error: reject(criarErroAjax) → Promise rejeitada
 *
 * 🔄 FLUXO DE USO TÍPICO (fetch):
 * 1. Chamada: ApiClient.fetch('/api/viagens', { method: 'POST', body: JSON.stringify({...}) })
 * 2. fetch(baseUrl + url, { ...options, headers: merged })
 * 3. Se !ok: throw Error
 * 4. Retorna response.json()
 * 5. try-catch: Alerta.TratamentoErroComLinha + throw
 *
 * 📌 DIFERENÇAS ENTRE MÉTODOS AJAX E FETCH:
 * - get/post/put/delete: usam jQuery.ajax (wrapper Promise)
 * - fetch: usa fetch nativo (sem jQuery)
 * - fetch permite options customizadas completas (headers, body, mode, etc.)
 * - ajax methods: data vai como JSON.stringify no body (POST/PUT)
 * - ajax methods: params vai como query string (GET/DELETE)
 * - fetch: opções completas de configuração (method, body, headers, etc.)
 *
 * 📌 TRATAMENTO DE ERROS:
 * - jQuery.ajax error callback: usa criarErroAjax(jqXHR, textStatus, errorThrown, this)
 * - fetch: throw new Error se !response.ok
 * - Todos os métodos: try-catch com Alerta.TratamentoErroComLinha
 * - Erros são re-thrown após logging (throw error)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - baseUrl: pode ser vazio (usar URLs absolutas) ou raiz da API (ex: "/api")
 * - defaultHeaders: aplicado a todos os requests (Content-Type JSON)
 * - fetch merge headers: { ...defaultHeaders, ...options.headers } (override possível)
 * - Todos os métodos são async: retornam Promises
 * - criarErroAjax: função externa (não definida neste arquivo)
 * - window.ApiClient: instância global criada com baseUrl vazio (line 193)
 * - Uso típico: ApiClient.get('/url') ou new ApiClient('/api').get('/viagens')
 * - JSON.stringify automático em POST/PUT
 * - dataType='json' força parse automático em GET/DELETE (jQuery)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Cliente HTTP para comunicação com API
 */
class ApiClient
{
    constructor(baseUrl = '')
    {
        this.baseUrl = baseUrl;
        this.defaultHeaders = {
            'Content-Type': 'application/json; charset=utf-8'
        };
    }

    /**
     * Requisição GET
     * param {string} url - URL do endpoint
     * param {Object} params - Parâmetros da query string
     * returns {Promise} Promise com resposta
     */
    async get(url, params = {})
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: this.baseUrl + url,
                    type: 'GET',
                    data: params,
                    dataType: 'json',
                    success: function (data)
                    {
                        resolve(data);
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("api-client.js", "get", error);
            throw error;
        }
    }

    /**
     * Requisição POST
     * param {string} url - URL do endpoint
     * param {Object} data - Dados a enviar
     * returns {Promise} Promise com resposta
     */
    async post(url, data = {})
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: this.baseUrl + url,
                    type: 'POST',
                    contentType: this.defaultHeaders['Content-Type'],
                    dataType: 'json',
                    data: JSON.stringify(data),
                    success: function (response)
                    {
                        resolve(response);
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("api-client.js", "post", error);
            throw error;
        }
    }

    /**
     * Requisição PUT
     * param {string} url - URL do endpoint
     * param {Object} data - Dados a enviar
     * returns {Promise} Promise com resposta
     */
    async put(url, data = {})
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: this.baseUrl + url,
                    type: 'PUT',
                    contentType: this.defaultHeaders['Content-Type'],
                    dataType: 'json',
                    data: JSON.stringify(data),
                    success: function (response)
                    {
                        resolve(response);
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("api-client.js", "put", error);
            throw error;
        }
    }

    /**
     * Requisição DELETE
     * param {string} url - URL do endpoint
     * param {Object} params - Parâmetros
     * returns {Promise} Promise com resposta
     */
    async delete(url, params = {})
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: this.baseUrl + url,
                    type: 'DELETE',
                    data: params,
                    dataType: 'json',
                    success: function (response)
                    {
                        resolve(response);
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("api-client.js", "delete", error);
            throw error;
        }
    }

    /**
     * Requisição com fetch nativo
     * param {string} url - URL do endpoint
     * param {Object} options - Opções do fetch
     * returns {Promise} Promise com resposta
     */
    async fetch(url, options = {})
    {
        try
        {
            const response = await fetch(this.baseUrl + url, {
                ...options,
                headers: {
                    ...this.defaultHeaders,
                    ...(options.headers || {})
                }
            });

            if (!response.ok)
            {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }

            return await response.json();
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("api-client.js", "fetch", error);
            throw error;
        }
    }
}

// Instância global
window.ApiClient = new ApiClient();
