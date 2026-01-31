/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: PageExceptionFilter.cs                                                                 ║
   ║ 📂 CAMINHO: Filters/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Filtros para capturar exceções em Razor Pages (.cshtml.cs), complementando o                    ║
   ║    GlobalExceptionFilter (Controllers). Inclui versão assíncrona dedicada.                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • PageExceptionFilter(ILogService logService, ILogger<PageExceptionFilter> logger)             ║
   ║    • OnPageHandlerSelected(PageHandlerSelectedContext context)                                    ║
   ║    • OnPageHandlerExecuting(PageHandlerExecutingContext context)                                  ║
   ║    • OnPageHandlerExecuted(PageHandlerExecutedContext context)                                    ║
   ║    • OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)                              ║
   ║    • OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next) ║
   ║    • AsyncPageExceptionFilter(ILogService logService, ILogger<AsyncPageExceptionFilter> logger)    ║
   ║    • OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)                              ║
   ║    • OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next) ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: ILogService, ILogger<>, Microsoft.AspNetCore.Mvc.RazorPages                        ║
   ║ 📅 ATUALIZAÇÃO: 30/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                     ║
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


// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
// │ 🎯 CLASSE: PageExceptionFilter                                                                   │
// │ 🔌 IMPLEMENTA: IPageFilter, IAsyncPageFilter                                                     │
// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯


// 🎯 OBJETIVO:
// Capturar exceções em Razor Pages e registrar detalhes com ILogService/ILogger.



// 🔗 RASTREABILIDADE:
// ⬅️ CHAMADO POR : Pipeline Razor Pages (IPageFilter/IAsyncPageFilter)
// ➡️ CHAMA       : ILogService.Error(), ILogger.LogError(), ExtractFileName(), ExtractLineNumber()


public class PageExceptionFilter : IPageFilter, IAsyncPageFilter
{
    private readonly ILogService _logService;
    private readonly ILogger<PageExceptionFilter> _logger;

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: PageExceptionFilter                                                           │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : DI / Program.cs / Startup                                             │
    // │    ➡️ CHAMA       : (injeção de dependências)                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Injetar serviços necessários para registro de exceções em Razor Pages.
    
    
    
    // 📥 PARÂMETROS:
    // logService - Serviço de log unificado do FrotiX
    // logger - Logger tipado para PageExceptionFilter
    
    
    // Param logService: Serviço de log unificado do FrotiX.
    // Param logger: Logger tipado para PageExceptionFilter.
    public PageExceptionFilter(ILogService logService, ILogger<PageExceptionFilter> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: OnPageHandlerSelected                                                        │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : Pipeline Razor Pages (IPageFilter)                                  │
    // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Callback de seleção do handler. Mantido para cumprir o contrato do filtro.
    
    
    
    // 📥 PARÂMETROS:
    // context - Contexto de seleção do handler da Razor Page.
    
    
    // Param context: Contexto de seleção do handler da Razor Page.
    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        // Não precisa fazer nada aqui
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: OnPageHandlerExecuting                                                       │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : Pipeline Razor Pages (IPageFilter)                                  │
    // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Callback pré-execução do handler. Mantido para cumprir o contrato do filtro.
    
    
    
    // 📥 PARÂMETROS:
    // context - Contexto de execução do handler da Razor Page.
    
    
    // Param context: Contexto de execução do handler da Razor Page.
    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        // Não precisa fazer nada aqui
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: OnPageHandlerExecuted                                                        │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : Pipeline Razor Pages (IPageFilter)                                  │
    // │    ➡️ CHAMA       : LogPageException()                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Capturar e registrar exceções geradas no handler da Razor Page.
    
    
    
    // 📥 PARÂMETROS:
    // context - Contexto de execução com possível exceção.
    
    
    // Param context: Contexto de execução com possível exceção.
    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        if (context.Exception != null && !context.ExceptionHandled)
        {
            LogPageException(context.Exception, context);
        }
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: OnPageHandlerSelectionAsync                                                   │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : Pipeline Razor Pages (IAsyncPageFilter)                              │
    // │    ➡️ CHAMA       : Task.CompletedTask                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Implementação assíncrona da seleção do handler (sem ação específica).
    
    
    
    // 📥 PARÂMETROS:
    // context - Contexto de seleção do handler da Razor Page.
    
    
    
    // 📤 RETORNO:
    // Task concluída imediatamente.
    
    
    // Param context: Contexto de seleção do handler da Razor Page.
    // Returns: Task concluída imediatamente.
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: OnPageHandlerExecutionAsync                                                   │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : Pipeline Razor Pages (IAsyncPageFilter)                              │
    // │    ➡️ CHAMA       : next(), LogPageException()                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Executar o handler e registrar exceções geradas na execução assíncrona.
    
    
    
    // 📥 PARÂMETROS:
    // context - Contexto de execução do handler da Razor Page
    // next - Delegate para executar o próximo estágio do pipeline
    
    
    
    // 📤 RETORNO:
    // Task que representa a execução assíncrona do handler.
    
    
    // Param context: Contexto de execução do handler da Razor Page.
    // Param next: Delegate para executar o próximo estágio do pipeline.
    // Returns: Task que representa a execução assíncrona do handler.
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


// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
// │ 🎯 CLASSE: AsyncPageExceptionFilter                                                              │
// │ 🔌 IMPLEMENTA: IAsyncPageFilter                                                                  │
// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯


// 🎯 OBJETIVO:
// Capturar exceções em Razor Pages usando fluxo assíncrono com try/catch.



// 🔗 RASTREABILIDADE:
// ⬅️ CHAMADO POR : Pipeline Razor Pages (IAsyncPageFilter)
// ➡️ CHAMA       : ILogService.Error(), ILogger.LogError()


public class AsyncPageExceptionFilter : IAsyncPageFilter
{
    private readonly ILogService _logService;
    private readonly ILogger<AsyncPageExceptionFilter> _logger;

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: AsyncPageExceptionFilter                                                    │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : DI / Program.cs / Startup                                             │
    // │    ➡️ CHAMA       : (injeção de dependências)                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Injetar serviços necessários para registrar exceções assíncronas em Razor Pages.
    
    
    
    // 📥 PARÂMETROS:
    // logService - Serviço de log unificado do FrotiX
    // logger - Logger tipado para AsyncPageExceptionFilter
    
    
    // Param logService: Serviço de log unificado do FrotiX.
    // Param logger: Logger tipado para AsyncPageExceptionFilter.
    public AsyncPageExceptionFilter(ILogService logService, ILogger<AsyncPageExceptionFilter> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: OnPageHandlerSelectionAsync                                                   │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : Pipeline Razor Pages (IAsyncPageFilter)                              │
    // │    ➡️ CHAMA       : Task.CompletedTask                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Implementação assíncrona da seleção do handler (sem ação específica).
    
    
    
    // 📥 PARÂMETROS:
    // context - Contexto de seleção do handler da Razor Page.
    
    
    
    // 📤 RETORNO:
    // Task concluída imediatamente.
    
    
    // Param context: Contexto de seleção do handler da Razor Page.
    // Returns: Task concluída imediatamente.
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ MÉTODO: OnPageHandlerExecutionAsync                                                   │
    // │ 🔗 RASTREABILIDADE:                                                                      │
    // │    ⬅️ CHAMADO POR : Pipeline Razor Pages (IAsyncPageFilter)                              │
    // │    ➡️ CHAMA       : next(), ILogService.Error(), ILogger.LogError()                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Executar o handler e registrar exceções assíncronas com log unificado.
    
    
    
    // 📥 PARÂMETROS:
    // context - Contexto de execução do handler da Razor Page
    // next - Delegate para executar o próximo estágio do pipeline
    
    
    
    // 📤 RETORNO:
    // Task que representa a execução assíncrona do handler.
    
    
    // Param context: Contexto de execução do handler da Razor Page.
    // Param next: Delegate para executar o próximo estágio do pipeline.
    // Returns: Task que representa a execução assíncrona do handler.
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
