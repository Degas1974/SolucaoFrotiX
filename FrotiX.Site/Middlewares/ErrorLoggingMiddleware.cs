/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ErrorLoggingMiddleware.cs                                                              ║
   ║ 📂 CAMINHO: /Middlewares                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Middleware ASP.NET que intercepta exceções não tratadas e erros HTTP (4xx/5xx).                 ║
   ║    Registra via ILogService com extração de arquivo/método/linha do stack trace.                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [InvokeAsync]       : Intercepta requisição.............. (HttpContext) -> Task                 ║
   ║ 2. [GetStatusMessage]  : Traduz código HTTP................. (int statusCode) -> string            ║
   ║ 3. [UseErrorLogging]   : Extension para registrar middleware (IApplicationBuilder) -> builder      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FrotiX.Services;
using Microsoft.AspNetCore.Builder;
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: InvokeAsync                                                        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Intercepta cada requisição HTTP e captura erros/exceções.              │
    // │    Registra erros HTTP (4xx/5xx) e exceções não tratadas no LogService.              │
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
            if (context.Response.StatusCode >= 400)
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
