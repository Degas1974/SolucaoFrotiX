/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: FrotiXLoggerProvider.cs                                                                ║
   ║ 📂 CAMINHO: /Logging                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Provider ILoggerProvider customizado que integra com LogService do FrotiX.                      ║
   ║    Captura logs ASP.NET Core (Warning+) e filtra categorias verbosas.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE CLASSES/FUNÇÕES (Entradas -> Saídas):                                                 ║
   ║ 1. [FrotiXLoggerProvider] : Provider de logging....... (ILogService) -> ILogger                    ║
   ║ 2. [FrotiXLogger]         : Logger customizado........ (categoryName) -> void (logs)               ║
   ║ 3. [AddFrotiXLogger]      : Extension de registro..... (ILoggingBuilder) -> ILoggingBuilder        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Concurrent;
using FrotiX.Services;
using Microsoft.Extensions.Logging;

namespace FrotiX.Logging;

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ CLASSE: FrotiXLoggerProvider                                                       │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
/// │    Provider de logging customizado que integra com o LogService do FrotiX.            │
/// │    Captura TODOS os logs do ASP.NET Core, incluindo erros de inicialização.           │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 📥 INPUTS (Entradas):                                                                 │
/// │    • logService [ILogService]: Serviço de log do FrotiX.                              │
/// │    • minimumLevel [LogLevel]: Nível mínimo (default: Warning).                        │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 📤 OUTPUTS (Saídas):                                                                  │
/// │    • [ILogger]: Logger para a categoria especificada.                                 │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🔗 RASTREABILIDADE:                                                                   │
/// │    ⬅️ CHAMADO POR : Program.cs (via AddFrotiXLogger)                                  │
/// │    ➡️ CHAMA       : FrotiXLogger.Log()                                                │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
public class FrotiXLoggerProvider : ILoggerProvider
{
    private readonly ILogService _logService;
    private readonly ConcurrentDictionary<string, FrotiXLogger> _loggers = new();
    private readonly LogLevel _minimumLevel;

    public FrotiXLoggerProvider(ILogService logService, LogLevel minimumLevel = LogLevel.Warning)
    {
        _logService = logService;
        _minimumLevel = minimumLevel;
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ FUNCIONALIDADE: CreateLogger                                                       │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Cria ou retorna logger cacheado para a categoria especificada.         │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 📥 INPUTS: • categoryName [string]: Nome da categoria (ex: "FrotiX.Controllers")     │
    /// │ 📤 OUTPUTS: • [ILogger]: Instância do FrotiXLogger                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new FrotiXLogger(name, _logService, _minimumLevel));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ CLASSE: FrotiXLogger                                                               │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
/// │    Logger customizado que envia logs para o LogService do FrotiX.                     │
/// │    Implementa filtros inteligentes para evitar logs verbosos.                         │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🔗 RASTREABILIDADE:                                                                   │
/// │    ⬅️ CHAMADO POR : FrotiXLoggerProvider.CreateLogger()                               │
/// │    ➡️ CHAMA       : ILogService.Error(), ILogService.Warning(), ILogService.Info()    │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
public class FrotiXLogger : ILogger
{
    private readonly string _categoryName;
    private readonly ILogService _logService;
    private readonly LogLevel _minimumLevel;

    public FrotiXLogger(string categoryName, ILogService logService, LogLevel minimumLevel)
    {
        _categoryName = categoryName;
        _logService = logService;
        _minimumLevel = minimumLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minimumLevel;
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ FUNCIONALIDADE: Log                                                                │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Processa e registra logs no LogService conforme nível e categoria.     │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 📥 INPUTS: logLevel, eventId, state, exception, formatter                            │
    /// │ 📤 OUTPUTS: void - Registra no LogService                                            │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // [LOGICA] Verificar se o nível está habilitado
        if (!IsEnabled(logLevel))
            return;

        try
        {
            // [DADOS] Formatar mensagem
            var message = formatter(state, exception);
            
            // [LOGICA] Ignorar logs muito verbosos ou internos
            if (ShouldIgnore(_categoryName, message))
                return;

            // [HELPER] Extrair nome do arquivo da categoria
            var arquivo = ExtractCategoryFile(_categoryName);

            // [LOGICA] Roteamento por nível de log
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    _logService.Error(
                        $"[{_categoryName}] {message}",
                        exception,
                        arquivo,
                        "ASP.NET Core"
                    );
                    break;

                case LogLevel.Warning:
                    _logService.Warning(
                        $"[{_categoryName}] {message}",
                        arquivo,
                        "ASP.NET Core"
                    );
                    break;

                case LogLevel.Information:
                    // [REGRA] Só loga INFO se for importante
                    if (IsImportantInfo(_categoryName, message))
                    {
                        _logService.Info(
                            $"[{_categoryName}] {message}",
                            arquivo,
                            "ASP.NET Core"
                        );
                    }
                    break;
            }
        }
        catch
        {
            // [SEGURANCA] Ignora erros no próprio logger para evitar loops
        }
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ FUNCIONALIDADE: ExtractCategoryFile                                                │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Extrai o nome do arquivo/classe da categoria de log.                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    private static string ExtractCategoryFile(string categoryName)
    {
        // [HELPER] Extrai o nome do arquivo/classe da categoria
        var parts = categoryName.Split('.');
        if (parts.Length > 0)
        {
            var lastPart = parts[^1];
            return $"{lastPart}.cs";
        }
        return categoryName;
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ FUNCIONALIDADE: ShouldIgnore                                                       │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Determina se um log deve ser ignorado (filtro de ruído).               │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    private static bool ShouldIgnore(string category, string message)
    {
        // [PERFORMANCE] Ignora categorias muito verbosas
        if (category.StartsWith("Microsoft.AspNetCore.Routing"))
            return true;
        if (category.StartsWith("Microsoft.AspNetCore.Mvc.Infrastructure"))
            return true;
        if (category.StartsWith("Microsoft.AspNetCore.StaticFiles"))
            return true;
        if (category.StartsWith("Microsoft.AspNetCore.Hosting.Diagnostics"))
            return true;
        if (category.StartsWith("Microsoft.EntityFrameworkCore.Query"))
            return true;
        if (category.StartsWith("Microsoft.EntityFrameworkCore.Database.Command"))
            return true;

        // [PERFORMANCE] Ignora mensagens específicas
        if (message.Contains("Executing endpoint"))
            return true;
        if (message.Contains("Request starting"))
            return true;
        if (message.Contains("Request finished"))
            return true;

        return false;
    }

    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ FUNCIONALIDADE: IsImportantInfo                                                    │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Determina se uma mensagem INFO é importante o suficiente para logar.   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    private static bool IsImportantInfo(string category, string message)
    {
        // [REGRA] Loga INFO apenas para categorias importantes
        if (category.StartsWith("FrotiX"))
            return true;
        if (message.Contains("started") || message.Contains("initialized"))
            return true;
        if (message.Contains("failed") || message.Contains("error"))
            return true;

        return false;
    }
}

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ CLASSE: FrotiXLoggerExtensions                                                     │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO: Extension methods para registrar o FrotiXLoggerProvider no DI.         │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🔗 RASTREABILIDADE:                                                                   │
/// │    ⬅️ CHAMADO POR : Program.cs                                                        │
/// │    ➡️ CHAMA       : ILoggingBuilder.AddProvider()                                     │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
public static class FrotiXLoggerExtensions
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ FUNCIONALIDADE: AddFrotiXLogger                                                    │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Registra o FrotiXLoggerProvider no pipeline de logging.                │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 📥 INPUTS: builder [ILoggingBuilder], logService [ILogService], minimumLevel         │
    /// │ 📤 OUTPUTS: [ILoggingBuilder] - Fluent API                                           │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public static ILoggingBuilder AddFrotiXLogger(this ILoggingBuilder builder, ILogService logService, LogLevel minimumLevel = LogLevel.Warning)
    {
        builder.AddProvider(new FrotiXLoggerProvider(logService, minimumLevel));
        return builder;
    }
}
