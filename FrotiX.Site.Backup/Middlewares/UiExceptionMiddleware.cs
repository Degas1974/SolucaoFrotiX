/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: UiExceptionMiddleware.cs                                                               ║
   ║ 📂 CAMINHO: /Middlewares                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Middleware para tratamento de exceções com resposta diferenciada:                               ║
   ║    JSON para requisições AJAX/API, redirect /Erro para requisições HTML.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [Invoke] : Intercepta e trata exceções.......... (HttpContext) -> Task                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Middlewares
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ CLASSE: UiExceptionMiddleware                                                      │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Middleware para tratamento de exceções com resposta diferenciada por tipo de       │
    // │    requisição. Verifica Accept header e X-Requested-With para decidir formato.        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Program.cs (pipeline de middlewares)                              │
    // │    ➡️ CHAMA       : /Erro (redirect HTML) ou JSON response (AJAX)                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public sealed class UiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public UiExceptionMiddleware(RequestDelegate next)
        {
            ArgumentNullException.ThrowIfNull(next);
            _next = next;
        }

        
        /// <summary>
        /// Status code 499: Client Closed Request (usado pelo nginx quando cliente desconecta)
        /// </summary>
        private const int StatusCodeClientClosedRequest = 499;

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Invoke                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Intercepta exceções e decide formato de resposta (JSON ou HTML).       │
        // │    - Se é desconexão de cliente: retorna 499 silenciosamente.                        │
        // │    - Se Accept contém application/json OU X-Requested-With é XMLHttpRequest:         │
        // │      Retorna JSON com detalhes do erro.                                              │
        // │    - Caso contrário: Redireciona para /Erro (que lerá TempData["UiError"]).          │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS: • http [HttpContext]: Contexto da requisição                              │
        // │ 📤 OUTPUTS: • [Task] - JSON response ou redirect                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        public async Task Invoke(HttpContext http)
        {
            try
            {
                // [LOGICA] Executar próximo middleware no pipeline
                await _next(http);
            }
            catch (Exception ex) when (IsClientDisconnectException(ex) || http.RequestAborted.IsCancellationRequested)
            {
                // [INFO] Cliente desconectou - isso é normal e esperado
                // Não tratamos como erro, apenas definimos o status 499 silenciosamente
                if (!http.Response.HasStarted)
                {
                    http.Response.StatusCode = StatusCodeClientClosedRequest;
                }
                return;
            }
            catch (OperationCanceledException) when (http.RequestAborted.IsCancellationRequested)
            {
                // [INFO] Requisição foi cancelada pelo cliente
                if (!http.Response.HasStarted)
                {
                    http.Response.StatusCode = StatusCodeClientClosedRequest;
                }
                return;
            }
            catch (Exception ex)
            {
                // [DADOS] Usa payload já registrado (se houver) ou monta um básico
                var payload =
                    http.Items.TryGetValue("UiError", out var v) && v is not null
                        ? v
                        : new
                        {
                            Origem = "Middleware",
                            Mensagem = ex.Message,
                            Stack = ex.StackTrace,
                        };

                // [LOGICA] Decide se a resposta deve ser JSON (AJAX/API) ou HTML (Razor/View)
                bool wantsJson = false;

                // [DADOS] Verificar Accept header
                var accept = http.Request.Headers["Accept"];
                if (accept.Count > 0)
                    wantsJson = accept.Any(h =>
                        h?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true
                    );

                // [DADOS] Verificar X-Requested-With para AJAX
                if (!wantsJson)
                {
                    var xrw = http.Request.Headers["X-Requested-With"];
                    if (xrw.Count > 0)
                        wantsJson = xrw.Any(h =>
                            string.Equals(h, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase)
                        );
                }

                // [UI] Definir status code de erro
                http.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (wantsJson)
                {
                    // [AJAX] Retornar resposta JSON para APIs
                    http.Response.ContentType = MediaTypeNames.Application.Json;

                    var problemJson = JsonSerializer.Serialize(
                        new
                        {
                            title = "Erro no servidor",
                            status = 500,
                            detail = payload,
                        }
                    );

                    await http.Response.WriteAsync(problemJson);
                    return;
                }
                else
                {
                    // [UI] HTML: redireciona para página de erro (que lerá TempData["UiError"])
                    http.Response.Redirect("/Erro");
                    return;
                }
            }
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: IsClientDisconnectException                                        │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Verifica se a exceção indica desconexão do cliente.                    │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS: • ex [Exception]: Exceção a ser verificada                                │
        // │ 📤 OUTPUTS: • [bool]: true se é desconexão de cliente                                │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        private static bool IsClientDisconnectException(Exception ex)
        {
            var current = ex;
            while (current != null)
            {
                // ConnectionResetException do ASP.NET Core
                if (current is ConnectionResetException)
                    return true;

                // ConnectionAbortedException do ASP.NET Core
                if (current.GetType().Name == "ConnectionAbortedException")
                    return true;

                // IOException com mensagens específicas de desconexão
                if (current is IOException ioEx)
                {
                    var message = ioEx.Message?.ToLowerInvariant() ?? "";
                    if (message.Contains("connection reset") ||
                        message.Contains("broken pipe") ||
                        message.Contains("an existing connection was forcibly closed") ||
                        message.Contains("the client has disconnected") ||
                        message.Contains("connection was aborted"))
                    {
                        return true;
                    }
                }

                // SocketException com códigos de desconexão
                if (current is System.Net.Sockets.SocketException socketEx)
                {
                    if (socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset ||
                        socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted ||
                        socketEx.SocketErrorCode == System.Net.Sockets.SocketError.Shutdown)
                    {
                        return true;
                    }
                }

                // Mensagens genéricas de desconexão
                var msg = current.Message?.ToLowerInvariant() ?? "";
                if (msg.Contains("the client has disconnected") ||
                    msg.Contains("connection reset by peer") ||
                    msg.Contains("an established connection was aborted"))
                {
                    return true;
                }

                current = current.InnerException;
            }

            return false;
        }
    }
}
