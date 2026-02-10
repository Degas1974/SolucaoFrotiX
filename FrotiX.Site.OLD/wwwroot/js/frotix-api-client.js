/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ ARQUIVO: frotix-api-client.js                                                                       ║
   ║ CAMINHO: /wwwroot/js/                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ OBJETIVO: Cliente HTTP robusto com tratamento de erros, retry automatico e logging                 ║
   ║           Compativel com o padrao ApiResponse do backend                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ INDICE:                                                                                              ║
   ║ 1. [FrotiXApi.get] : Requisicao GET......................... (url, params) -> Promise               ║
   ║ 2. [FrotiXApi.post] : Requisicao POST....................... (url, data) -> Promise                 ║
   ║ 3. [FrotiXApi.put] : Requisicao PUT......................... (url, data) -> Promise                 ║
   ║ 4. [FrotiXApi.delete] : Requisicao DELETE................... (url, params) -> Promise              ║
   ║ 5. [FrotiXApi.request] : Requisicao generica com retry...... (method, url, data, attempt) -> Promise║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ DATA: 01/02/2026 | AUTOR: Claude Code | VERSAO: 1.0                                                 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

/**
 * Cliente API com tratamento robusto de erros
 * Resolve o problema de "Script error" capturando detalhes da falha
 */
var FrotiXApi = (function () {
    'use strict';

    var config = {
        baseUrl: '',
        timeout: 30000,
        retryAttempts: 2,
        retryDelay: 1000
    };

    /**
     * Classe customizada de erro da API
     */
    function ApiError(message, statusCode, requestId, details) {
        this.name = 'ApiError';
        this.message = message || 'Erro desconhecido';
        this.statusCode = statusCode || 0;
        this.requestId = requestId || generateRequestId();
        this.details = details || null;
        this.timestamp = new Date().toISOString();
    }
    ApiError.prototype = Object.create(Error.prototype);
    ApiError.prototype.constructor = ApiError;

    /**
     * Gera ID unico para requisicao
     */
    function generateRequestId() {
        try {
            return Math.random().toString(36).substring(2, 10);
        } catch (erro) {
            console.error('Erro em generateRequestId:', erro);
            return 'error-' + Date.now();
        }
    }

    /**
     * Sleep/delay async
     */
    function sleep(ms) {
        try {
            return new Promise(function (resolve) {
                setTimeout(resolve, ms);
            });
        } catch (erro) {
            console.error('Erro em sleep:', erro);
            return Promise.resolve();
        }
    }

    /**
     * Constroi URL com query params
     */
    function buildUrl(endpoint, params) {
        try {
            var url = config.baseUrl + endpoint;
            if (params && Object.keys(params).length > 0) {
                var queryString = Object.keys(params)
                    .filter(function (key) {
                        return params[key] !== null && params[key] !== undefined;
                    })
                    .map(function (key) {
                        return encodeURIComponent(key) + '=' + encodeURIComponent(params[key]);
                    })
                    .join('&');
                if (queryString) {
                    url += (url.indexOf('?') >= 0 ? '&' : '?') + queryString;
                }
            }
            return url;
        } catch (erro) {
            console.error('Erro em buildUrl:', erro);
            return config.baseUrl + endpoint;
        }
    }

    /**
     * Verifica se deve tentar novamente
     */
    function shouldRetry(error) {
        try {
            // Retry para erros de rede ou servidor sobrecarregado
            if (error instanceof TypeError) return true;
            if (error.name === 'AbortError') return true;
            if (error instanceof ApiError && [0, 502, 503, 504].indexOf(error.statusCode) >= 0) return true;
            return false;
        } catch (erro) {
            console.error('Erro em shouldRetry:', erro);
            return false;
        }
    }

    /**
     * Envia erro para o servidor para logging
     */
    function logErrorToServer(error) {
        try {
            var logData = {
                Tipo: 'HTTP-ERROR',
                Mensagem: error.message,
                StatusCode: error.statusCode,
                RequestId: error.requestId,
                Url: window.location.href,
                UserAgent: navigator.userAgent,
                Timestamp: error.timestamp,
                Detalhes: error.details ? JSON.stringify(error.details) : null
            };

            if (navigator.sendBeacon) {
                var blob = new Blob([JSON.stringify(logData)], { type: 'application/json' });
                navigator.sendBeacon('/api/LogErros/Client', blob);
            } else {
                fetch('/api/LogErros/Client', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(logData),
                    keepalive: true
                }).catch(function () { });
            }
        } catch (e) {
            if (console && console.warn) {
                console.warn('[FrotiXApi] Falha ao enviar log de erro:', e);
            }
        }
    }

    /**
     * Trata diferentes tipos de erro
     */
    function handleError(error, method, endpoint, requestId) {
        try {
            // Erro ja tratado
            if (error instanceof ApiError) {
                return error;
            }

            // Timeout/Abort
            if (error.name === 'AbortError') {
                return new ApiError(
                    'Requisicao cancelada: tempo limite excedido',
                    408,
                    requestId
                );
            }

            // Erro de rede (CORS, DNS, conexao)
            if (error instanceof TypeError && error.message && error.message.indexOf('fetch') >= 0) {
                return new ApiError(
                    'Erro de conexao: verifique sua internet ou tente novamente',
                    0,
                    requestId,
                    { originalError: error.message }
                );
            }

            // "Script error" - erro cross-origin sem detalhes
            if (error.message === 'Script error.' || error.message === 'Script error') {
                return new ApiError(
                    'Erro ao processar requisicao. Se persistir, contate o suporte.',
                    500,
                    requestId,
                    {
                        type: 'CrossOriginError',
                        hint: 'Erro de origem cruzada - detalhes nao disponiveis por seguranca'
                    }
                );
            }

            // Erro generico
            return new ApiError(
                error.message || 'Erro desconhecido',
                500,
                requestId,
                { originalError: error.toString ? error.toString() : String(error) }
            );
        } catch (erro) {
            console.error('Erro em handleError:', erro);
            return new ApiError('Erro critico ao processar erro', 500, requestId);
        }
    }

    /**
     * Funcao principal de requisicao com retry
     */
    function request(method, endpoint, data, attempt) {
        attempt = attempt || 1;
        var url = buildUrl(endpoint, method === 'GET' ? data : {});
        var requestId = generateRequestId();

        if (console && console.log) {
            console.log('[FrotiXApi] ' + method + ' ' + endpoint + ' (Tentativa ' + attempt + ', ID: ' + requestId + ')');
        }

        return new Promise(function (resolve, reject) {
            var options = {
                method: method,
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'Accept': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest',
                    'X-Request-Id': requestId
                },
                credentials: 'same-origin'
            };

            if (method !== 'GET' && data && Object.keys(data).length > 0) {
                options.body = JSON.stringify(data);
            }

            // Timeout via AbortController
            var controller = null;
            var timeoutId = null;

            if (window.AbortController) {
                controller = new AbortController();
                timeoutId = setTimeout(function () { controller.abort(); }, config.timeout);
                options.signal = controller.signal;
            }

            fetch(url, options)
                .then(function (response) {
                    if (timeoutId) clearTimeout(timeoutId);

                    // Captura header de request ID do servidor
                    var serverRequestId = response.headers.get('X-Request-Id') || requestId;

                    // Parse do response
                    var contentType = response.headers.get('Content-Type') || '';

                    if (contentType.indexOf('application/json') >= 0) {
                        return response.json().then(function (responseData) {
                            return { response: response, data: responseData, serverRequestId: serverRequestId };
                        });
                    } else {
                        return response.text().then(function (text) {
                            return { response: response, data: { success: false, message: text }, serverRequestId: serverRequestId };
                        });
                    }
                })
                .then(function (result) {
                    var response = result.response;
                    var responseData = result.data;
                    var serverRequestId = result.serverRequestId;

                    // Verifica status HTTP
                    if (!response.ok) {
                        throw new ApiError(
                            responseData.message || 'HTTP ' + response.status + ': ' + response.statusText,
                            response.status,
                            serverRequestId,
                            responseData.errorDetails
                        );
                    }

                    // Verifica success no body (padrao da API)
                    if (responseData.success === false) {
                        throw new ApiError(
                            responseData.message || 'Operacao nao realizada',
                            response.status,
                            responseData.requestId || serverRequestId,
                            responseData.errorDetails
                        );
                    }

                    if (console && console.log) {
                        console.log('[FrotiXApi] Sucesso: ' + endpoint);
                    }

                    resolve(responseData);
                })
                .catch(function (error) {
                    if (timeoutId) clearTimeout(timeoutId);

                    // Tratamento especifico por tipo de erro
                    var handledError = handleError(error, method, endpoint, requestId);

                    // Retry automatico para erros de rede
                    if (shouldRetry(error) && attempt < config.retryAttempts) {
                        if (console && console.warn) {
                            console.warn('[FrotiXApi] Retry ' + (attempt + 1) + '/' + config.retryAttempts + ' para ' + endpoint);
                        }

                        sleep(config.retryDelay * attempt).then(function () {
                            request(method, endpoint, data, attempt + 1)
                                .then(resolve)
                                .catch(reject);
                        });
                        return;
                    }

                    // Loga erro no servidor
                    logErrorToServer(handledError);

                    reject(handledError);
                });
        });
    }

    /**
     * Faz requisicao GET para a API
     */
    function get(endpoint, params) {
        try {
            return request('GET', endpoint, params || {});
        } catch (erro) {
            console.error('Erro em get:', erro);
            return Promise.reject(new ApiError('Erro ao iniciar requisicao GET', 0, generateRequestId()));
        }
    }

    /**
     * Faz requisicao POST para a API
     */
    function post(endpoint, data) {
        try {
            return request('POST', endpoint, data || {});
        } catch (erro) {
            console.error('Erro em post:', erro);
            return Promise.reject(new ApiError('Erro ao iniciar requisicao POST', 0, generateRequestId()));
        }
    }

    /**
     * Faz requisicao PUT para a API
     */
    function put(endpoint, data) {
        try {
            return request('PUT', endpoint, data || {});
        } catch (erro) {
            console.error('Erro em put:', erro);
            return Promise.reject(new ApiError('Erro ao iniciar requisicao PUT', 0, generateRequestId()));
        }
    }

    /**
     * Faz requisicao DELETE para a API
     */
    function del(endpoint, params) {
        try {
            return request('DELETE', endpoint, params || {});
        } catch (erro) {
            console.error('Erro em del:', erro);
            return Promise.reject(new ApiError('Erro ao iniciar requisicao DELETE', 0, generateRequestId()));
        }
    }

    // API publica
    return {
        get: get,
        post: post,
        put: put,
        'delete': del,
        request: request,
        config: config,
        ApiError: ApiError
    };
})();

// Expor globalmente
if (typeof window !== 'undefined') {
    window.FrotiXApi = FrotiXApi;
}
