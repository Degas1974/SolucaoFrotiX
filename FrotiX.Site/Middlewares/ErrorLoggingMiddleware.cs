/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Middlewares/ErrorLoggingMiddleware.cs                          ║
 * ║  Descrição: Middleware ASP.NET que intercepta exceções não tratadas e    ║
 * ║             erros HTTP (4xx/5xx). Registra via ILogService com extração  ║
 * ║             de arquivo/método/linha do stack trace. Extension method     ║
 * ║             UseErrorLogging para facilitar registro no pipeline.         ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FrotiX.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrotiX.Middlewares;

/// <summary>
/// Middleware para capturar e registrar erros HTTP em toda a aplicação
/// Intercepta exceções não tratadas e erros de status HTTP
/// </summary>
public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;

    public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ILogService logService)
    {
        try
        {
            await _next(context);

            // Registra erros de status HTTP (4xx e 5xx)
            if (context.Response.StatusCode >= 400)
            {
                var statusCode = context.Response.StatusCode;
                var path = context.Request.Path.Value ?? "";
                var method = context.Request.Method;
                var message = GetStatusMessage(statusCode);

                logService.HttpError(statusCode, path, method, message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado na requisição");

            // Extrai informações do erro
            var arquivo = ex.TargetSite?.DeclaringType?.FullName ?? "Desconhecido";
            var metodo = ex.TargetSite?.Name ?? "Desconhecido";
            int? linha = null;

            // Tenta extrair linha do StackTrace
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                var match = System.Text.RegularExpressions.Regex.Match(ex.StackTrace, @":line (\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out var l))
                {
                    linha = l;
                }
            }

            logService.Error(
                $"Exceção não tratada: {ex.Message}",
                ex,
                arquivo,
                metodo,
                linha
            );

            // Registra também como erro HTTP 500
            logService.HttpError(
                500,
                context.Request.Path.Value ?? "",
                context.Request.Method,
                ex.Message
            );

            // Re-lança a exceção para o handler padrão do ASP.NET
            throw;
        }
    }

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

/// <summary>
/// Extension method para facilitar o registro do middleware no pipeline
/// </summary>
public static class ErrorLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorLoggingMiddleware>();
    }
}
