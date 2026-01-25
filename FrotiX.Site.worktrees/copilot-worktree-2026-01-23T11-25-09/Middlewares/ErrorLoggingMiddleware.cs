using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FrotiX.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrotiX.Middlewares;

/// <summary>
/// ╔══════════════════════════════════════════════════════════════════════════════╗
/// ║                                                                              ║
/// ║  🛡️ ARQUIVO: ErrorLoggingMiddleware.cs (Middleware de Captura de Erros)    ║
/// ║                                                                              ║
/// ║  DESCRIÇÃO:                                                                  ║
/// ║  Middleware global para capturar e registrar TODOS os erros HTTP.           ║
/// ║  Intercepta exceções não tratadas E erros de status HTTP (4xx/5xx).         ║
/// ║                                                                              ║
/// ║  FUNCIONALIDADES:                                                            ║
/// ║  1. Captura exceções não tratadas (try-catch no pipeline).                  ║
/// ║  2. Registra erros HTTP 4xx/5xx (StatusCode >= 400).                        ║
/// ║  3. Extrai informações de StackTrace (arquivo, método, linha).              ║
/// ║  4. Persiste logs via ILogService (banco de dados).                         ║
/// ║  5. Re-lança exceções para handler padrão do ASP.NET Core.                  ║
/// ║                                                                              ║
/// ║  TIPOS DE LOG:                                                               ║
/// ║  - LogService.Error(): Exceções não tratadas (com StackTrace).              ║
/// ║  - LogService.HttpError(): Erros de status HTTP (4xx/5xx).                  ║
/// ║                                                                              ║
/// ║  STATUS CODES SUPORTADOS:                                                    ║
/// ║  - 400-499: Client Errors (Bad Request, Unauthorized, Forbidden, NotFound). ║
/// ║  - 500-599: Server Errors (Internal Server Error, Bad Gateway, etc.).       ║
/// ║                                                                              ║
/// ║  REGISTRO NO PIPELINE (Program.cs):                                          ║
/// ║  app.UseErrorLogging(); // ANTES de UseEndpoints()                          ║
/// ║                                                                              ║
/// ║  INTEGRAÇÃO:                                                                 ║
/// ║  - ILogger<ErrorLoggingMiddleware>: Log no console/debug (ASP.NET Core).    ║
/// ║  - ILogService: Persistência em banco de dados (FrotiX).                    ║
/// ║                                                                              ║
/// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
/// ║                                                                              ║
/// ╚══════════════════════════════════════════════════════════════════════════════╝
/// </summary>

/// <summary>
/// ╭──────────────────────────────────────────────────────────────────────────────
/// │ CLASSE: ErrorLoggingMiddleware (Middleware de Captura de Erros)
/// │──────────────────────────────────────────────────────────────────────────────
/// │ DESCRIÇÃO:
/// │    Middleware que intercepta TODA requisição HTTP no pipeline.
/// │    Captura exceções não tratadas E erros de status HTTP (4xx/5xx).
/// │
/// │ PROPRIEDADES:
/// │    - _next: Próximo middleware no pipeline (RequestDelegate).
/// │    - _logger: Logger do ASP.NET Core (ILogger).
/// │
/// │ MÉTODO PRINCIPAL:
/// │    - InvokeAsync(): Chamado pelo ASP.NET Core para cada requisição HTTP.
/// │
/// │ FLUXO DE EXECUÇÃO:
/// │    1. Try: Chama próximo middleware (_next).
/// │    2. Verifica StatusCode >= 400 (erros HTTP).
/// │    3. Catch: Captura exceções não tratadas.
/// │    4. Extrai arquivo/método/linha do StackTrace.
/// │    5. Loga via ILogService (banco de dados).
/// │    6. Re-lança exceção para handler padrão (UseExceptionHandler).
/// │
/// │ INJEÇÃO DE DEPENDÊNCIAS:
/// │    - _next e _logger: Injetados no construtor (Scoped/Singleton).
/// │    - ILogService: Injetado no método InvokeAsync (Scoped por requisição).
/// │
/// │ REGISTRO NO PIPELINE:
/// │    app.UseErrorLogging(); // Program.cs
/// │──────────────────────────────────────────────────────────────────────────────
/// </summary>
public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;

    /// <summary>
    /// Construtor que recebe próximo middleware e logger (injeção de dependências).
    /// </summary>
    public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────
    /// │ MÉTODO: InvokeAsync (Método Principal do Middleware)
    /// │──────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Método chamado pelo ASP.NET Core para CADA requisição HTTP.
    /// │    Intercepta exceções e erros de status HTTP.
    /// │
    /// │ FLUXO:
    /// │    1. Try: Chama próximo middleware (_next).
    /// │    2. Verifica StatusCode >= 400 (4xx/5xx).
    /// │       → Extrai path, method, statusCode.
    /// │       → Loga via LogService.HttpError().
    /// │    3. Catch: Captura exceções não tratadas.
    /// │       → Extrai arquivo, método, linha do StackTrace (Regex).
    /// │       → Loga via LogService.Error() + HttpError(500).
    /// │       → Re-lança exceção (throw;) para handler padrão.
    /// │
    /// │ PARÂMETROS:
    /// │    - context: HttpContext da requisição (Request/Response).
    /// │    - logService: ILogService injetado por requisição (Scoped).
    /// │
    /// │ EXTRAÇÃO DE STACKTRACE:
    /// │    - TargetSite.DeclaringType.FullName → Arquivo/Classe.
    /// │    - TargetSite.Name → Método.
    /// │    - Regex @":line (\d+)" → Número da linha.
    /// │
    /// │ DUPLO LOG PARA EXCEÇÕES:
    /// │    1. LogService.Error(): Log detalhado com StackTrace completo.
    /// │    2. LogService.HttpError(500): Log HTTP simplificado (path + mensagem).
    /// │
    /// │ RE-THROW EXCEPTION:
    /// │    - Throw; re-lança exceção original (sem alterar StackTrace).
    /// │    - Permite UseExceptionHandler() processar (página de erro amigável).
    /// │──────────────────────────────────────────────────────────────────────────
    /// </summary>
    public async Task InvokeAsync(HttpContext context, ILogService logService)
    {
        try
        {
            // [ETAPA 1] - Chama próximo middleware no pipeline (await assíncrono)
            await _next(context);

            // [ETAPA 2] - Verifica StatusCode >= 400 (erros HTTP 4xx/5xx)
            if (context.Response.StatusCode >= 400)
            {
                var statusCode = context.Response.StatusCode;
                var path = context.Request.Path.Value ?? "";
                var method = context.Request.Method;
                var message = GetStatusMessage(statusCode); // Mensagem amigável do status

                // [LOG HTTP] - Registra erro HTTP no banco de dados (LogService.HttpError)
                logService.HttpError(statusCode, path, method, message);
            }
        }
        catch (Exception ex)
        {
            // [ETAPA 3] - Captura exceções não tratadas (try-catch global)

            // [LOG CONSOLE] - Log no console/debug do ASP.NET Core (ILogger)
            _logger.LogError(ex, "Erro não tratado na requisição");

            // [ETAPA 4] - Extrai informações do StackTrace (arquivo, método, linha)
            var arquivo = ex.TargetSite?.DeclaringType?.FullName ?? "Desconhecido";
            var metodo = ex.TargetSite?.Name ?? "Desconhecido";
            int? linha = null;

            // [ETAPA 4.1] - Tenta extrair linha do StackTrace via Regex ":line (\d+)"
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                var match = System.Text.RegularExpressions.Regex.Match(ex.StackTrace, @":line (\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out var l))
                {
                    linha = l;
                }
            }

            // [ETAPA 5] - Registra erro DETALHADO no banco de dados (LogService.Error)
            logService.Error(
                $"Exceção não tratada: {ex.Message}",
                ex,
                arquivo,
                metodo,
                linha
            );

            // [ETAPA 6] - Registra erro HTTP 500 no banco de dados (LogService.HttpError)
            // Duplo log: Error() para detalhes técnicos, HttpError() para análise HTTP
            logService.HttpError(
                500,
                context.Request.Path.Value ?? "",
                context.Request.Method,
                ex.Message
            );

            // [ETAPA 7] - Re-lança exceção para handler padrão do ASP.NET (UseExceptionHandler)
            // Mantém StackTrace original (throw; sem parâmetro)
            throw;
        }
    }

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────
    /// │ MÉTODO: GetStatusMessage (Mensagens Amigáveis para Status HTTP)
    /// │──────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Mapeia códigos de status HTTP para mensagens amigáveis em português.
    /// │    Usado em logs para facilitar identificação do erro.
    /// │
    /// │ STATUS CODES SUPORTADOS (15 códigos):
    /// │    - 400: Bad Request (Requisição inválida)
    /// │    - 401: Unauthorized (Não autorizado)
    /// │    - 403: Forbidden (Acesso negado)
    /// │    - 404: Not Found (Página não encontrada)
    /// │    - 405: Method Not Allowed (Método não permitido)
    /// │    - 408: Request Timeout (Tempo esgotado)
    /// │    - 409: Conflict (Conflito de dados)
    /// │    - 415: Unsupported Media Type (Tipo de mídia não suportado)
    /// │    - 422: Unprocessable Entity (Entidade não processável)
    /// │    - 429: Too Many Requests (Muitas requisições)
    /// │    - 500: Internal Server Error (Erro interno do servidor)
    /// │    - 501: Not Implemented (Não implementado)
    /// │    - 502: Bad Gateway (Gateway inválido)
    /// │    - 503: Service Unavailable (Serviço indisponível)
    /// │    - 504: Gateway Timeout (Timeout do gateway)
    /// │
    /// │ FALLBACK:
    /// │    - Outros códigos: "HTTP Error {statusCode}" (genérico).
    /// │
    /// │ USO:
    /// │    var message = GetStatusMessage(404); // "Not Found - Página não encontrada"
    /// │──────────────────────────────────────────────────────────────────────────
    /// </summary>
    private static string GetStatusMessage(int statusCode)
    {
        return statusCode switch
        {
            // [4XX - CLIENT ERRORS] - Erros do lado do cliente
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

            // [5XX - SERVER ERRORS] - Erros do lado do servidor
            500 => "Internal Server Error - Erro interno do servidor",
            501 => "Not Implemented - Não implementado",
            502 => "Bad Gateway - Gateway inválido",
            503 => "Service Unavailable - Serviço indisponível",
            504 => "Gateway Timeout - Timeout do gateway",

            // [FALLBACK] - Outros códigos não mapeados
            _ => $"HTTP Error {statusCode}"
        };
    }
}

/// <summary>
/// ╭──────────────────────────────────────────────────────────────────────────────
/// │ CLASSE: ErrorLoggingMiddlewareExtensions (Extension Methods para Registro)
/// │──────────────────────────────────────────────────────────────────────────────
/// │ DESCRIÇÃO:
/// │    Extension methods para facilitar registro do middleware no pipeline.
/// │
/// │ MÉTODO:
/// │    - UseErrorLogging(): Registra ErrorLoggingMiddleware no IApplicationBuilder.
/// │
/// │ USO EM PROGRAM.CS:
/// │    app.UseErrorLogging(); // Antes de UseEndpoints() e UseRouting()
/// │
/// │ ORDEM RECOMENDADA NO PIPELINE:
/// │    1. app.UseExceptionHandler("/Error"); // Handler de exceções padrão
/// │    2. app.UseErrorLogging();             // Nosso middleware de log
/// │    3. app.UseRouting();                  // Roteamento
/// │    4. app.UseEndpoints(...);             // Endpoints MVC/API
/// │
/// │ BENEFÍCIOS:
/// │    - Sintaxe fluente (app.UseErrorLogging()).
/// │    - Encapsula UseMiddleware<ErrorLoggingMiddleware>().
/// │    - Integra com pipeline do ASP.NET Core.
/// │──────────────────────────────────────────────────────────────────────────────
/// </summary>
public static class ErrorLoggingMiddlewareExtensions
{
    /// <summary>
    /// Registra ErrorLoggingMiddleware no pipeline do ASP.NET Core.
    /// </summary>
    /// <param name="builder">IApplicationBuilder do ASP.NET Core.</param>
    /// <returns>IApplicationBuilder para encadeamento (fluent syntax).</returns>
    public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
    {
        // [REGISTRO] - Adiciona ErrorLoggingMiddleware ao pipeline HTTP
        return builder.UseMiddleware<ErrorLoggingMiddleware>();
    }
}
