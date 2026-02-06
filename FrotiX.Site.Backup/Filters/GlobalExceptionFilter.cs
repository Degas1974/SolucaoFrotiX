/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: GlobalExceptionFilter.cs                                                               ║
   ║ 📂 CAMINHO: /Filters                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Filtro global IExceptionFilter para Controllers MVC e API.                                      ║
   ║    Extrai arquivo/método/linha do stack trace. Retorna JSON para AJAX/API.                         ║
   ║    Inclui AsyncExceptionFilter para operações assíncronas e TaskCanceledException.                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [OnException]        : Captura e trata exceções......... (ExceptionContext) -> void             ║
   ║ 2. [ExtractFileName]    : Extrai nome do arquivo........... (Exception) -> string                  ║
   ║ 3. [ExtractLineNumber]  : Extrai número da linha........... (Exception) -> int?                    ║
   ║ 4. [IsAjaxRequest]      : Verifica se é requisição AJAX.... (HttpRequest) -> bool                  ║
   ║ 5. [OnExceptionAsync]   : Trata exceções assíncronas....... (ExceptionContext) -> Task             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrotiX.Filters;


// ╭───────────────────────────────────────────────────────────────────────────────────────╮
// │ ⚡ CLASSE: GlobalExceptionFilter                                                      │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
// │    Filtro global para capturar exceções em Controllers MVC e API.                     │
// │    Registra detalhes completos do erro incluindo arquivo, método e linha.             │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🔗 RASTREABILIDADE:                                                                   │
// │    ⬅️ CHAMADO POR : Pipeline ASP.NET (registrado em Program.cs)                       │
// │    ➡️ CHAMA       : ILogService.Error(), ILogger.LogError()                           │
// ╰───────────────────────────────────────────────────────────────────────────────────────╯

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogService _logService;
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionFilter(
        ILogService logService,
        ILogger<GlobalExceptionFilter> logger,
        IWebHostEnvironment environment)
    {
        _logService = logService;
        _logger = logger;
        _environment = environment;
    }


    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: OnException                                                        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Captura exceções de Controllers e registra detalhes no LogService.     │
    // │    Para requisições AJAX/API retorna JSON com erro. HTML usa handler padrão.         │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • context [ExceptionContext]: Contexto da exceção capturada               │
    // │ 📤 OUTPUTS: • void - Define context.Result para AJAX ou deixa handler padrão         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        // [DADOS] Extrai informações detalhadas do erro
        var arquivo = ExtractFileName(exception);
        var metodo = exception.TargetSite?.Name ?? "Desconhecido";
        var linha = ExtractLineNumber(exception);
        var controller = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
        var action = context.RouteData.Values["action"]?.ToString() ?? "Unknown";

        // [DADOS] Monta mensagem detalhada
        var message = $"Erro em {controller}/{action}: {exception.Message}";

        // [DEBUG] Registra o erro no LogService
        _logService.Error(
            message,
            exception,
            arquivo,
            metodo,
            linha
        );

        // [DEBUG] Log via ILogger para console/debug
        _logger.LogError(exception, "Exceção capturada no controller {Controller}/{Action}", controller, action);

        // [LOGICA] Se for requisição AJAX/API, retorna JSON
        if (IsAjaxRequest(context.HttpContext.Request) || context.HttpContext.Request.Path.StartsWithSegments("/api"))
        {
            // [AJAX] Retorna resposta JSON estruturada
            context.Result = new JsonResult(new
            {
                success = false,
                error = _environment.IsDevelopment() ? exception.Message : "Ocorreu um erro interno. Por favor, tente novamente.",
                errorId = DateTime.Now.Ticks.ToString(),
                details = _environment.IsDevelopment() ? new
                {
                    arquivo,
                    metodo,
                    linha,
                    stackTrace = exception.StackTrace?.Split('\n').Take(5).ToArray()
                } : null
            })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
        // [UI] Para requisições normais, deixa o handler padrão tratar
    }


    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: ExtractFileName                                                    │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Extrai o nome do arquivo de origem da exceção do StackTrace.           │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • exception [Exception]: Exceção capturada                                │
    // │ 📤 OUTPUTS: • [string]: Nome do arquivo (ex: "Controllers/ViagemController.cs")      │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    private static string ExtractFileName(Exception exception)
    {
        try
        {
            // [HELPER] Primeiro tenta do TargetSite
            var declaringType = exception.TargetSite?.DeclaringType;
            if (declaringType != null)
            {
                var typeName = declaringType.Name;
                var namespaceParts = declaringType.Namespace?.Split('.') ?? Array.Empty<string>();
                var lastPart = namespaceParts.LastOrDefault() ?? "";

                // [DADOS] Retorna algo como "Controllers/ViagemController.cs"
                return $"{lastPart}/{typeName}.cs";
            }

            // [HELPER] Tenta extrair do StackTrace
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                // [LOGICA] Padrão: "at Namespace.Class.Method() in C:\Path\File.cs:line 123"
                var match = Regex.Match(exception.StackTrace, @"in (.+\.cs):line \d+");
                if (match.Success)
                {
                    var fullPath = match.Groups[1].Value;
                    return Path.GetFileName(fullPath);
                }
            }
        }
        catch { }

        return "Arquivo não identificado";
    }


    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: ExtractLineNumber                                                  │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Extrai o número da linha do erro do StackTrace.                        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • exception [Exception]: Exceção capturada                                │
    // │ 📤 OUTPUTS: • [int?]: Número da linha ou null se não encontrado                      │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    private static int? ExtractLineNumber(Exception exception)
    {
        try
        {
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                // [HELPER] Regex para extrair ":line 123"
                var match = Regex.Match(exception.StackTrace, @":line (\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out var line))
                {
                    return line;
                }
            }
        }
        catch { }

        return null;
    }


    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: IsAjaxRequest                                                      │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Verifica se a requisição é AJAX (XMLHttpRequest ou accept JSON).       │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • request [HttpRequest]: Requisição HTTP                                  │
    // │ 📤 OUTPUTS: • [bool]: true se é AJAX                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    private static bool IsAjaxRequest(HttpRequest request)
    {
        // [LOGICA] Verifica X-Requested-With ou Accept header
        return request.Headers["X-Requested-With"] == "XMLHttpRequest"
            || request.Headers.Accept.ToString().Contains("application/json");
    }
}


// ╭───────────────────────────────────────────────────────────────────────────────────────╮
// │ ⚡ CLASSE: AsyncExceptionFilter                                                       │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
// │    Filtro de exceção assíncrono para operações async.                                 │
// │    Trata TaskCanceledException e OperationCanceledException como warning.             │
// │───────────────────────────────────────────────────────────────────────────────────────│
// │ 🔗 RASTREABILIDADE:                                                                   │
// │    ⬅️ CHAMADO POR : Pipeline ASP.NET (registrado em Program.cs)                       │
// │    ➡️ CHAMA       : ILogService.Warning()                                             │
// ╰───────────────────────────────────────────────────────────────────────────────────────╯

public class AsyncExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogService _logService;
    private readonly ILogger<AsyncExceptionFilter> _logger;

    public AsyncExceptionFilter(ILogService logService, ILogger<AsyncExceptionFilter> logger)
    {
        _logService = logService;
        _logger = logger;
    }


    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ FUNCIONALIDADE: OnExceptionAsync                                                   │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Trata exceções de operações assíncronas. Tasks canceladas são          │
    // │    logadas como warning, outras exceções passam para GlobalExceptionFilter.          │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 📥 INPUTS: • context [ExceptionContext]: Contexto da exceção                         │
    // │ 📤 OUTPUTS: • [Task]: Task completada                                                │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;

        // [LOGICA] Para exceções de Task canceladas, apenas loga como warning
        if (exception is TaskCanceledException || exception is OperationCanceledException)
        {
            // [DEBUG] Registrar cancelamento como warning (não é erro)
            _logService.Warning(
                $"Operação cancelada: {context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}",
                exception.TargetSite?.DeclaringType?.Name + ".cs",
                exception.TargetSite?.Name
            );
            return Task.CompletedTask;
        }

        // [LOGICA] Para outras exceções, o GlobalExceptionFilter vai tratar
        return Task.CompletedTask;
    }
}
