/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ErrorLoggingMiddleware.cs                                                              ║
   ║ 📂 CAMINHO: /Middlewares                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Middleware ASP.NET que intercepta exceções não tratadas e erros HTTP (4xx/5xx).                 ║
   ║    Registra via ILogService com extração de arquivo/método/linha do stack trace.                   ║
   ║    Trata ConnectionResetException (desconexão de cliente) como evento normal, não erro.            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [InvokeAsync]               : Intercepta requisição.............. (HttpContext) -> Task         ║
   ║ 2. [IsClientDisconnectException]: Verifica desconexão cliente....... (Exception) -> bool           ║
   ║ 3. [GetStatusMessage]          : Traduz código HTTP................. (int statusCode) -> string    ║
   ║ 4. [UseErrorLogging]           : Extension para registrar middleware (IApplicationBuilder) -> builder║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrotiX.Middlewares;


// ╭───────────────────────────────────────────────────────────────────────────────────────╮
// │ ⚡ CLASSE: ErrorLoggingMiddleware                                                     │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
// │    Middleware para capturar e registrar erros HTTP em toda a aplicação.               │
// │    Intercepta exceções não tratadas e erros de status HTTP (4xx e 5xx).               │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🔗 RASTREABILIDADE:                                                                   │
// │    ⬅️ CHAMADO POR : Program.cs (via UseErrorLogging)                                  │
// │    ➡️ CHAMA       : ILogService.Error(), ILogService.HttpError(), ILogService.Warning()│
// ╰───────────────────────────────────────────────────────────────────────────────────────╯

public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;

    public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    
    /// <summary>
    /// Status code 499: Client Closed Request (usado pelo nginx quando cliente desconecta)
    /// </summary>
    private const int StatusCodeClientClosedRequest = 499;

    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: InvokeAsync                                                        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Intercepta cada requisição HTTP e captura erros/exceções.              │
    // │    Registra erros HTTP (4xx/5xx) e exceções não tratadas no LogService.              │
    // │    Trata ConnectionResetException como evento normal (cliente desconectou).          │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • context [HttpContext], logService [ILogService]                         │
    // │ 📤 OUTPUTS: • [Task] - Continua pipeline ou re-lança exceção                         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public async Task InvokeAsync(HttpContext context, ILogService logService)
    {
        try
        {
            // [LOGICA] Executar próximo middleware no pipeline
            await _next(context);

            // [LOGICA] Registrar erros de status HTTP (4xx e 5xx)
            // Ignora 499 (Client Closed Request) pois é tratado separadamente
            if (context.Response.StatusCode >= 400 && context.Response.StatusCode != StatusCodeClientClosedRequest)
            {
                // [DADOS] Extrair informações da requisição
                var statusCode = context.Response.StatusCode;
                var path = context.Request.Path.Value ?? "";
                var method = context.Request.Method;
                var message = GetStatusMessage(statusCode);

                // [DEBUG] Registrar erro HTTP
                logService.HttpError(statusCode, path, method, message);
            }
        }
        catch (Exception ex) when (IsClientDisconnectException(ex) || context.RequestAborted.IsCancellationRequested)
        {
            // [INFO] Cliente desconectou durante a requisição - isso é normal e esperado
            // Não é um erro crítico, apenas log informativo em nível Debug
            _logger.LogDebug(
                "Cliente desconectou durante requisição: {Method} {Path}",
                context.Request.Method,
                context.Request.Path.Value ?? "/"
            );

            // [LOGICA] Define status 499 (Client Closed Request) se a resposta ainda não foi iniciada
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodeClientClosedRequest;
            }

            // [INFO] Registra como informação, não como erro
            logService.Info(
                $"Cliente desconectou: {context.Request.Method} {context.Request.Path.Value ?? "/"}",
                "ErrorLoggingMiddleware",
                "InvokeAsync"
            );

            // [LOGICA] Não re-lança a exceção - é um evento esperado
            return;
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // [INFO] Requisição foi cancelada pelo cliente
            _logger.LogDebug(
                "Requisição cancelada pelo cliente: {Method} {Path}",
                context.Request.Method,
                context.Request.Path.Value ?? "/"
            );

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodeClientClosedRequest;
            }

            // [LOGICA] Não re-lança - cancelamento é esperado
            return;
        }
        catch (Exception ex)
        {
            // [DEBUG] Log via ILogger padrão
            _logger.LogError(ex, "Erro não tratado na requisição");

            // [DADOS] Extrair informações do erro
            var arquivo = ex.TargetSite?.DeclaringType?.FullName ?? "Desconhecido";
            var metodo = ex.TargetSite?.Name ?? "Desconhecido";
            int? linha = null;

            // [HELPER] Tenta extrair linha do StackTrace
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                var match = System.Text.RegularExpressions.Regex.Match(ex.StackTrace, @":line (\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out var l))
                {
                    linha = l;
                }
            }

            // [DEBUG] Registrar exceção detalhada
            logService.Error(
                $"Exceção não tratada: {ex.Message}",
                ex,
                arquivo,
                metodo,
                linha
            );

            // [DEBUG] Registrar também como erro HTTP 500
            logService.HttpError(
                500,
                context.Request.Path.Value ?? "",
                context.Request.Method,
                ex.Message
            );

            // [LOGICA] Re-lança a exceção para o handler padrão do ASP.NET
            throw;
        }
    }

    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: IsClientDisconnectException                                        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Verifica se a exceção indica desconexão do cliente.                    │
    // │    Detecta ConnectionResetException, IOException com ECONNRESET, etc.                │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • ex [Exception]: Exceção a ser verificada                                │
    // │ 📤 OUTPUTS: • [bool]: true se é desconexão de cliente                                │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    private static bool IsClientDisconnectException(Exception ex)
    {
        // [LOGICA] Verifica a exceção e todas as inner exceptions
        var current = ex;
        while (current != null)
        {
            // [CHECK] ConnectionResetException do ASP.NET Core
            if (current is ConnectionResetException)
                return true;

            // [CHECK] ConnectionAbortedException do ASP.NET Core
            if (current.GetType().Name == "ConnectionAbortedException")
                return true;

            // [CHECK] IOException com mensagens específicas de desconexão
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

            // [CHECK] SocketException com códigos de desconexão
            if (current is System.Net.Sockets.SocketException socketEx)
            {
                // ECONNRESET (104 no Linux, 10054 no Windows)
                if (socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset ||
                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted ||
                    socketEx.SocketErrorCode == System.Net.Sockets.SocketError.Shutdown)
                {
                    return true;
                }
            }

            // [CHECK] Mensagens genéricas de desconexão
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: GetStatusMessage                                                   │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Traduz código HTTP para mensagem legível em português.                 │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • statusCode [int]: Código HTTP (400, 404, 500, etc)                      │
    // │ 📤 OUTPUTS: • [string]: Mensagem descritiva do erro                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    private static string GetStatusMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request - Requisição inválida",
            401 => "Unauthorized - Não autorizado",
            403 => "Forbidden - Acesso negado",
            404 => "Not Found - Página não encontrada",
            405 => "Method Not Allowed - Método não permitido",
            408 => "Request Timeout - Tempo esgotado",
            409 => "Conflict - Conflito de dados",
            415 => "Unsupported Media Type - Tipo de mídia não suportado",
            422 => "Unprocessable Entity - Entidade não processável",
            429 => "Too Many Requests - Muitas requisições",
            499 => "Client Closed Request - Cliente desconectou",
            500 => "Internal Server Error - Erro interno do servidor",
            501 => "Not Implemented - Não implementado",
            502 => "Bad Gateway - Gateway inválido",
            503 => "Service Unavailable - Serviço indisponível",
            504 => "Gateway Timeout - Timeout do gateway",
            _ => $"HTTP Error {statusCode}"
        };
    }
}


// ╭───────────────────────────────────────────────────────────────────────────────────────╮
// │ ⚡ CLASSE: ErrorLoggingMiddlewareExtensions                                           │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🎯 DESCRIÇÃO: Extension method para facilitar o registro do middleware no pipeline.  │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🔗 RASTREABILIDADE:                                                                   │
// │    ⬅️ CHAMADO POR : Program.cs                                                        │
// │    ➡️ CHAMA       : IApplicationBuilder.UseMiddleware()                            │
// ╰───────────────────────────────────────────────────────────────────────────────────────╯

public static class ErrorLoggingMiddlewareExtensions
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: UseErrorLogging                                                    │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Registra o ErrorLoggingMiddleware no pipeline de requisições.          │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • builder [IApplicationBuilder]                                           │
    // │ 📤 OUTPUTS: • [IApplicationBuilder] - Fluent API                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorLoggingMiddleware>();
    }
}
