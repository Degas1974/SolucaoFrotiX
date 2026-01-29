/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: PageExceptionFilter.cs                                                                  ║
   ║ 📂 CAMINHO: /Filters                                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Filtro IPageFilter/IAsyncPageFilter para capturar exceções em Razor Pages (.cshtml.cs).         ║
   ║    Complementa GlobalExceptionFilter (Controllers). Inclui AsyncPageExceptionFilter (async).        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ ── PageExceptionFilter ──                                                                          ║
   ║ 1. [OnPageHandlerSelected]      : Callback seleção handler............ (context) -> void           ║
   ║ 2. [OnPageHandlerExecuting]     : Callback pré-execução............... (context) -> void           ║
   ║ 3. [OnPageHandlerExecuted]      : Captura exceção e loga.............. (context) -> void           ║
   ║ 4. [OnPageHandlerSelectionAsync]: Versão async da seleção............. (context) -> Task           ║
   ║ 5. [OnPageHandlerExecutionAsync]: Executa e captura exceção async..... (ctx,next) -> Task          ║
   ║ 6. [LogPageException]           : Extrai detalhes e loga via ILogService (ex,ctx)  -> void         ║
   ║ 7. [ExtractFileName]            : Extrai nome arquivo do stack........ (ex,fallback) -> string     ║
   ║ 8. [ExtractLineNumber]          : Extrai número linha do stack........ (ex) -> int?                ║
   ║ ── AsyncPageExceptionFilter ──                                                                     ║
   ║ 9. [OnPageHandlerSelectionAsync]: Callback async seleção.............. (context) -> Task           ║
   ║10. [OnPageHandlerExecutionAsync]: Try/catch com re-throw.............. (ctx,next) -> Task          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: ILogService, ILogger<PageExceptionFilter>                                         ║
   ║ 📅 ATUALIZAÇÃO: 29/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Filters;

/// <summary>
/// Filtro para capturar exceções em Razor Pages (.cshtml/.cshtml.cs)
/// Complementa o GlobalExceptionFilter que só funciona para Controllers
/// </summary>
public class PageExceptionFilter : IPageFilter, IAsyncPageFilter
{
    private readonly ILogService _logService;
    private readonly ILogger<PageExceptionFilter> _logger;

    public PageExceptionFilter(ILogService logService, ILogger<PageExceptionFilter> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        // Não precisa fazer nada aqui
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        // Não precisa fazer nada aqui
    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        if (context.Exception != null && !context.ExceptionHandled)
        {
            LogPageException(context.Exception, context);
        }
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        return next().ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                var exception = task.Exception.InnerException ?? task.Exception;
                LogPageException(exception, context);
            }
        });
    }

    private void LogPageException(Exception exception, FilterContext context)
    {
        try
        {
            var pagePath = context.ActionDescriptor.DisplayName ?? "Unknown Page";
            var arquivo = ExtractFileName(exception, pagePath);
            var metodo = exception.TargetSite?.Name ?? "OnGet/OnPost";
            var linha = ExtractLineNumber(exception);

            var message = $"Erro em Razor Page {pagePath}: {exception.Message}";

            _logService.Error(
                message,
                exception,
                arquivo,
                metodo,
                linha
            );

            _logger.LogError(exception, "Exceção em Razor Page {Page}", pagePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar exceção de página");
        }
    }

    private static string ExtractFileName(Exception exception, string fallback)
    {
        try
        {
            // Tenta do TargetSite
            var declaringType = exception.TargetSite?.DeclaringType;
            if (declaringType != null)
            {
                return $"{declaringType.Name}.cs";
            }

            // Tenta do StackTrace
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                var match = Regex.Match(exception.StackTrace, @"in (.+\.cs):line \d+");
                if (match.Success)
                {
                    return Path.GetFileName(match.Groups[1].Value);
                }

                // Tenta padrão de Razor Pages
                var razorMatch = Regex.Match(exception.StackTrace, @"Pages[/\\](.+\.cshtml)");
                if (razorMatch.Success)
                {
                    return razorMatch.Groups[1].Value;
                }
            }

            // Extrai do DisplayName
            var parts = fallback.Split('/');
            return parts.LastOrDefault() ?? fallback;
        }
        catch
        {
            return fallback;
        }
    }

    private static int? ExtractLineNumber(Exception exception)
    {
        try
        {
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
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
}

/// <summary>
/// Filtro assíncrono para Razor Pages
/// </summary>
public class AsyncPageExceptionFilter : IAsyncPageFilter
{
    private readonly ILogService _logService;
    private readonly ILogger<AsyncPageExceptionFilter> _logger;

    public AsyncPageExceptionFilter(ILogService logService, ILogger<AsyncPageExceptionFilter> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            var pagePath = context.ActionDescriptor.DisplayName ?? "Unknown";
            
            _logService.Error(
                $"Exceção async em {pagePath}: {ex.Message}",
                ex,
                $"{pagePath}.cshtml.cs",
                context.HandlerMethod?.Name ?? "Handler"
            );

            _logger.LogError(ex, "Exceção async em Razor Page {Page}", pagePath);
            
            throw; // Re-lança para tratamento padrão
        }
    }
}
