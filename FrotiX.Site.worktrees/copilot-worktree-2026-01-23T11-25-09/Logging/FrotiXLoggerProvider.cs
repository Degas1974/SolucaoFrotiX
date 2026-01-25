using System;
using System.Collections.Concurrent;
using FrotiX.Services;
using Microsoft.Extensions.Logging;

namespace FrotiX.Logging;

/// <summary>
/// ╔══════════════════════════════════════════════════════════════════════════════╗
/// ║                                                                              ║
/// ║  📋 ARQUIVO: FrotiXLoggerProvider.cs (Logger Provider Customizado)          ║
/// ║                                                                              ║
/// ║  DESCRIÇÃO:                                                                  ║
/// ║  Provider de logging customizado que integra ASP.NET Core ILogger com       ║
/// ║  LogService do FrotiX (persistência em banco de dados).                     ║
/// ║                                                                              ║
/// ║  ARQUITETURA:                                                                ║
/// ║  1. FrotiXLoggerProvider: Factory de loggers (ILoggerProvider).             ║
/// ║  2. FrotiXLogger: Logger customizado por categoria (ILogger).               ║
/// ║  3. FrotiXLoggerExtensions: Extension method para registro no DI.           ║
/// ║                                                                              ║
/// ║  FLUXO DE INTEGRAÇÃO:                                                        ║
/// ║  ASP.NET Core ILogger → FrotiXLogger → LogService → Banco de Dados          ║
/// ║                                                                              ║
/// ║  FUNCIONALIDADES:                                                            ║
/// ║  - Captura TODOS os logs do ASP.NET Core (incluindo inicialização).         ║
/// ║  - Filtra logs verbosos (Routing, StaticFiles, EF Queries).                 ║
/// ║  - Mapeia LogLevel para métodos do LogService (Error/Warning/Info).         ║
/// ║  - Cache de loggers por categoria (ConcurrentDictionary).                   ║
/// ║  - Extração inteligente de arquivo/classe da categoria.                     ║
/// ║                                                                              ║
/// ║  NÍVEIS DE LOG SUPORTADOS:                                                   ║
/// ║  - Critical/Error → LogService.Error() (persiste no BD).                    ║
/// ║  - Warning → LogService.Warning() (persiste no BD).                         ║
/// ║  - Information → LogService.Info() (apenas logs importantes).               ║
/// ║  - Debug/Trace → Ignorados (muito verbosos).                                ║
/// ║                                                                              ║
/// ║  FILTROS APLICADOS:                                                          ║
/// ║  - Ignora: Routing, StaticFiles, MVC Infrastructure, EF Queries.            ║
/// ║  - Ignora: "Executing endpoint", "Request starting/finished".               ║
/// ║  - Loga INFO apenas para: FrotiX.*, "started", "initialized", "failed".     ║
/// ║                                                                              ║
/// ║  REGISTRO NO PROGRAM.CS:                                                     ║
/// ║  builder.Logging.AddFrotiXLogger(logService, LogLevel.Warning);             ║
/// ║                                                                              ║
/// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
/// ║                                                                              ║
/// ╚══════════════════════════════════════════════════════════════════════════════╝
/// </summary>

/// <summary>
/// ╭──────────────────────────────────────────────────────────────────────────────
/// │ CLASSE: FrotiXLoggerProvider (Factory de Loggers Customizados)
/// │──────────────────────────────────────────────────────────────────────────────
/// │ DESCRIÇÃO:
/// │    Provider que cria instâncias de FrotiXLogger para cada categoria.
/// │    Implementa ILoggerProvider (interface do ASP.NET Core).
/// │
/// │ PROPRIEDADES:
/// │    - _logService: Serviço de log do FrotiX (injetado via DI).
/// │    - _loggers: Cache thread-safe de loggers por categoria (ConcurrentDictionary).
/// │    - _minimumLevel: Nível mínimo de log (default: Warning).
/// │
/// │ MÉTODOS:
/// │    - CreateLogger(): Cria ou retorna logger existente para uma categoria.
/// │    - Dispose(): Limpa cache de loggers (chamado ao desligar app).
/// │
/// │ CACHE DE LOGGERS:
/// │    - Usa ConcurrentDictionary para evitar criar múltiplos loggers para mesma categoria.
/// │    - Thread-safe (múltiplas threads podem solicitar loggers simultaneamente).
/// │    - GetOrAdd: Se logger existe, retorna; senão, cria novo.
/// │
/// │ CATEGORIAS COMUNS:
/// │    - "FrotiX.Controllers.ViagemController"
/// │    - "Microsoft.AspNetCore.Hosting.Diagnostics"
/// │    - "Microsoft.EntityFrameworkCore.Database.Command"
/// │
/// │ REGISTRO NO DI:
/// │    ILoggingBuilder.AddProvider(new FrotiXLoggerProvider(logService, LogLevel.Warning));
/// │──────────────────────────────────────────────────────────────────────────────
/// </summary>
public class FrotiXLoggerProvider : ILoggerProvider
{
    private readonly ILogService _logService;
    private readonly ConcurrentDictionary<string, FrotiXLogger> _loggers = new();
    private readonly LogLevel _minimumLevel;

    /// <summary>
    /// Construtor que recebe LogService e nível mínimo de log.
    /// </summary>
    /// <param name="logService">Serviço de log do FrotiX (persistência em BD).</param>
    /// <param name="minimumLevel">Nível mínimo de log (default: Warning).</param>
    public FrotiXLoggerProvider(ILogService logService, LogLevel minimumLevel = LogLevel.Warning)
    {
        _logService = logService;
        _minimumLevel = minimumLevel;
    }

    /// <summary>
    /// Cria ou retorna logger existente para uma categoria.
    /// Usa ConcurrentDictionary para cache thread-safe.
    /// </summary>
    public ILogger CreateLogger(string categoryName)
    {
        // [CACHE] - GetOrAdd: Retorna logger existente ou cria novo (thread-safe)
        return _loggers.GetOrAdd(categoryName, name => new FrotiXLogger(name, _logService, _minimumLevel));
    }

    /// <summary>
    /// Limpa cache de loggers (chamado ao desligar aplicação).
    /// </summary>
    public void Dispose()
    {
        _loggers.Clear();
    }
}

/// <summary>
/// ╭──────────────────────────────────────────────────────────────────────────────
/// │ CLASSE: FrotiXLogger (Logger Customizado por Categoria)
/// │──────────────────────────────────────────────────────────────────────────────
/// │ DESCRIÇÃO:
/// │    Logger customizado que envia logs filtrados para LogService do FrotiX.
/// │    Implementa ILogger (interface do ASP.NET Core).
/// │
/// │ PROPRIEDADES:
/// │    - _categoryName: Nome da categoria (ex: "FrotiX.Controllers.ViagemController").
/// │    - _logService: Serviço de log do FrotiX (persistência em BD).
/// │    - _minimumLevel: Nível mínimo de log (definido no Provider).
/// │
/// │ MÉTODOS PRINCIPAIS:
/// │    - Log<TState>(): Método principal de logging (chamado pelo ASP.NET Core).
/// │    - IsEnabled(): Verifica se nível de log está habilitado.
/// │    - BeginScope(): Não implementado (retorna null).
/// │
/// │ MÉTODOS AUXILIARES (PRIVATE STATIC):
/// │    - ExtractCategoryFile(): Extrai nome do arquivo da categoria.
/// │    - ShouldIgnore(): Filtra logs verbosos (Routing, StaticFiles, EF Queries).
/// │    - IsImportantInfo(): Define se INFO deve ser logado (apenas FrotiX.* e eventos importantes).
/// │
/// │ MAPEAMENTO DE NÍVEIS:
/// │    - Critical/Error → LogService.Error() → Tabela Log (Nivel = "Error").
/// │    - Warning → LogService.Warning() → Tabela Log (Nivel = "Warning").
/// │    - Information → LogService.Info() → Tabela Log (Nivel = "Info") - APENAS logs importantes.
/// │    - Debug/Trace → IGNORADOS (muito verbosos).
/// │
/// │ FILTROS DE CATEGORIA (ShouldIgnore):
/// │    ❌ Microsoft.AspNetCore.Routing.*
/// │    ❌ Microsoft.AspNetCore.Mvc.Infrastructure.*
/// │    ❌ Microsoft.AspNetCore.StaticFiles.*
/// │    ❌ Microsoft.AspNetCore.Hosting.Diagnostics.*
/// │    ❌ Microsoft.EntityFrameworkCore.Query.*
/// │    ❌ Microsoft.EntityFrameworkCore.Database.Command.*
/// │
/// │ FILTROS DE MENSAGEM (ShouldIgnore):
/// │    ❌ "Executing endpoint"
/// │    ❌ "Request starting"
/// │    ❌ "Request finished"
/// │
/// │ CRITÉRIOS PARA INFO (IsImportantInfo):
/// │    ✅ Categoria começa com "FrotiX"
/// │    ✅ Mensagem contém "started", "initialized", "failed", "error"
/// │
/// │ TRATAMENTO DE ERROS:
/// │    - Try-catch no método Log() para evitar loops infinitos (erro no logger).
/// │    - Exceções no logger são silenciadas (catch vazio).
/// │
/// │ EXTRAÇÃO DE ARQUIVO (ExtractCategoryFile):
/// │    - "FrotiX.Controllers.ViagemController" → "ViagemController.cs"
/// │    - "Microsoft.AspNetCore.Hosting.Diagnostics" → "Diagnostics.cs"
/// │    - Usa Split('.') e pega última parte (^1).
/// │──────────────────────────────────────────────────────────────────────────────
/// </summary>
public class FrotiXLogger : ILogger
{
    private readonly string _categoryName;
    private readonly ILogService _logService;
    private readonly LogLevel _minimumLevel;

    /// <summary>
    /// Construtor que recebe categoria, LogService e nível mínimo.
    /// </summary>
    public FrotiXLogger(string categoryName, ILogService logService, LogLevel minimumLevel)
    {
        _categoryName = categoryName;
        _logService = logService;
        _minimumLevel = minimumLevel;
    }

    /// <summary>
    /// Não implementado (ASP.NET Core usa para agrupamento de logs).
    /// </summary>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    /// <summary>
    /// Verifica se nível de log está habilitado (>= _minimumLevel).
    /// </summary>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minimumLevel;
    }

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────
    /// │ MÉTODO: Log<TState> (Método Principal de Logging)
    /// │──────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Método chamado pelo ASP.NET Core para logar mensagens.
    /// │    Filtra logs verbosos e mapeia para LogService.
    /// │
    /// │ FLUXO:
    /// │    1. Verifica se LogLevel está habilitado (IsEnabled).
    /// │    2. Formata mensagem usando formatter (state + exception).
    /// │    3. Aplica filtros (ShouldIgnore).
    /// │    4. Extrai nome do arquivo da categoria (ExtractCategoryFile).
    /// │    5. Mapeia LogLevel para método do LogService:
    /// │       - Critical/Error → LogService.Error()
    /// │       - Warning → LogService.Warning()
    /// │       - Information → LogService.Info() (apenas logs importantes)
    /// │    6. Ignora exceções no logger (catch vazio para evitar loops).
    /// │
    /// │ PARÂMETROS:
    /// │    - logLevel: Nível do log (Trace/Debug/Info/Warning/Error/Critical).
    /// │    - eventId: ID do evento (não usado).
    /// │    - state: Estado do log (geralmente mensagem ou objeto).
    /// │    - exception: Exceção associada (pode ser null).
    /// │    - formatter: Função que formata state+exception em string.
    /// │──────────────────────────────────────────────────────────────────────────
    /// </summary>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // [ETAPA 1] - Verifica se LogLevel está habilitado (>= _minimumLevel)
        if (!IsEnabled(logLevel))
            return;

        try
        {
            // [ETAPA 2] - Formata mensagem usando formatter (state + exception)
            var message = formatter(state, exception);

            // [ETAPA 3] - Aplica filtros (ignora logs verbosos)
            if (ShouldIgnore(_categoryName, message))
                return;

            // [ETAPA 4] - Extrai nome do arquivo da categoria (última parte do namespace)
            var arquivo = ExtractCategoryFile(_categoryName);

            // [ETAPA 5] - Mapeia LogLevel para método do LogService
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    // [ERRO/CRÍTICO] - Persiste no BD via LogService.Error()
                    _logService.Error(
                        $"[{_categoryName}] {message}",
                        exception,
                        arquivo,
                        "ASP.NET Core"
                    );
                    break;

                case LogLevel.Warning:
                    // [WARNING] - Persiste no BD via LogService.Warning()
                    _logService.Warning(
                        $"[{_categoryName}] {message}",
                        arquivo,
                        "ASP.NET Core"
                    );
                    break;

                case LogLevel.Information:
                    // [INFO] - Só loga se for importante (FrotiX.* ou eventos críticos)
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
            // [PROTEÇÃO] - Ignora erros no próprio logger para evitar loops infinitos
        }
    }

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────
    /// │ MÉTODO: ExtractCategoryFile (Extração de Nome de Arquivo da Categoria)
    /// │──────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Extrai o nome do arquivo/classe da categoria (última parte do namespace).
    /// │
    /// │ EXEMPLOS:
    /// │    - "FrotiX.Controllers.ViagemController" → "ViagemController.cs"
    /// │    - "Microsoft.AspNetCore.Hosting.Diagnostics" → "Diagnostics.cs"
    /// │    - "Program" → "Program.cs"
    /// │
    /// │ LÓGICA:
    /// │    1. Split('.') separa namespace em partes.
    /// │    2. Pega última parte (^1 = índice negativo C# 8.0+).
    /// │    3. Adiciona extensão ".cs".
    /// │
    /// │ USO:
    /// │    Usado para popular campo "Arquivo" na tabela Log do banco de dados.
    /// │──────────────────────────────────────────────────────────────────────────
    /// </summary>
    private static string ExtractCategoryFile(string categoryName)
    {
        // [ETAPA 1] - Split por '.' e pega última parte (nome da classe)
        var parts = categoryName.Split('.');
        if (parts.Length > 0)
        {
            var lastPart = parts[^1]; // C# 8.0+ Index operator (equivalente a parts[parts.Length - 1])
            return $"{lastPart}.cs";
        }
        return categoryName;
    }

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────
    /// │ MÉTODO: ShouldIgnore (Filtro de Logs Verbosos)
    /// │──────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Determina se log deve ser IGNORADO (filtro de ruído).
    /// │    Evita poluir banco de dados com logs irrelevantes.
    /// │
    /// │ CATEGORIAS IGNORADAS (StartsWith):
    /// │    ❌ Microsoft.AspNetCore.Routing (endpoints, routing interno)
    /// │    ❌ Microsoft.AspNetCore.Mvc.Infrastructure (MVC pipeline)
    /// │    ❌ Microsoft.AspNetCore.StaticFiles (arquivos estáticos: CSS, JS, imagens)
    /// │    ❌ Microsoft.AspNetCore.Hosting.Diagnostics (diagnostics HTTP)
    /// │    ❌ Microsoft.EntityFrameworkCore.Query (queries SQL geradas)
    /// │    ❌ Microsoft.EntityFrameworkCore.Database.Command (comandos SQL executados)
    /// │
    /// │ MENSAGENS IGNORADAS (Contains):
    /// │    ❌ "Executing endpoint" (log de execução de endpoint)
    /// │    ❌ "Request starting" (log de início de requisição)
    /// │    ❌ "Request finished" (log de fim de requisição)
    /// │
    /// │ RETORNO:
    /// │    - true: Log deve ser IGNORADO (não persiste no BD).
    /// │    - false: Log deve ser LOGADO (persiste no BD).
    /// │──────────────────────────────────────────────────────────────────────────
    /// </summary>
    private static bool ShouldIgnore(string category, string message)
    {
        // [FILTRO 1] - Ignora categorias muito verbosas (ASP.NET Core interno)
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

        // [FILTRO 2] - Ignora mensagens específicas (logs de pipeline HTTP)
        if (message.Contains("Executing endpoint"))
            return true;
        if (message.Contains("Request starting"))
            return true;
        if (message.Contains("Request finished"))
            return true;

        return false; // NÃO ignora (deve logar)
    }

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────
    /// │ MÉTODO: IsImportantInfo (Filtro de Logs INFO Importantes)
    /// │──────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Determina se log INFO deve ser LOGADO (filtro de logs importantes).
    /// │    Evita logar INFO muito verboso (apenas eventos críticos).
    /// │
    /// │ CRITÉRIOS PARA LOGAR INFO:
    /// │    ✅ Categoria começa com "FrotiX" (logs do sistema FrotiX).
    /// │    ✅ Mensagem contém "started" (inicialização de serviços).
    /// │    ✅ Mensagem contém "initialized" (inicialização completa).
    /// │    ✅ Mensagem contém "failed" (falha em inicialização).
    /// │    ✅ Mensagem contém "error" (erro em INFO, raro mas possível).
    /// │
    /// │ EXEMPLOS LOGADOS:
    /// │    ✅ "[FrotiX.Services.EscalaService] Serviço inicializado com sucesso"
    /// │    ✅ "[Microsoft.Hosting.Lifetime] Application started"
    /// │    ✅ "[Microsoft.EntityFrameworkCore] Database initialized"
    /// │
    /// │ EXEMPLOS IGNORADOS:
    /// │    ❌ "[Microsoft.AspNetCore.Routing] Endpoint matched"
    /// │    ❌ "[Microsoft.EntityFrameworkCore.Query] Executing query"
    /// │
    /// │ RETORNO:
    /// │    - true: Log INFO deve ser LOGADO (persiste no BD).
    /// │    - false: Log INFO deve ser IGNORADO (não persiste no BD).
    /// │──────────────────────────────────────────────────────────────────────────
    /// </summary>
    private static bool IsImportantInfo(string category, string message)
    {
        // [CRITÉRIO 1] - Loga INFO apenas para categorias FrotiX (namespace do sistema)
        if (category.StartsWith("FrotiX"))
            return true;

        // [CRITÉRIO 2] - Loga INFO para eventos importantes (inicialização, falhas)
        if (message.Contains("started") || message.Contains("initialized"))
            return true;
        if (message.Contains("failed") || message.Contains("error"))
            return true;

        return false; // NÃO loga INFO (ignora)
    }
}

/// <summary>
/// ╭──────────────────────────────────────────────────────────────────────────────
/// │ CLASSE: FrotiXLoggerExtensions (Extension Methods para Registro do Logger)
/// │──────────────────────────────────────────────────────────────────────────────
/// │ DESCRIÇÃO:
/// │    Extension methods para facilitar registro do FrotiXLoggerProvider no DI.
/// │
/// │ MÉTODO:
/// │    - AddFrotiXLogger(): Registra FrotiXLoggerProvider no ILoggingBuilder.
/// │
/// │ USO EM PROGRAM.CS:
/// │    builder.Logging.AddFrotiXLogger(logService, LogLevel.Warning);
/// │
/// │ PARÂMETROS:
/// │    - builder: ILoggingBuilder do ASP.NET Core.
/// │    - logService: ILogService do FrotiX (injetado).
/// │    - minimumLevel: Nível mínimo de log (default: Warning).
/// │
/// │ BENEFÍCIOS:
/// │    - Sintaxe fluente (builder.Logging.AddFrotiXLogger(...)).
/// │    - Encapsula criação do Provider.
/// │    - Integra com ASP.NET Core logging pipeline.
/// │──────────────────────────────────────────────────────────────────────────────
/// </summary>
public static class FrotiXLoggerExtensions
{
    /// <summary>
    /// Registra FrotiXLoggerProvider no ILoggingBuilder.
    /// </summary>
    /// <param name="builder">ILoggingBuilder do ASP.NET Core.</param>
    /// <param name="logService">ILogService do FrotiX (persistência em BD).</param>
    /// <param name="minimumLevel">Nível mínimo de log (default: Warning).</param>
    /// <returns>ILoggingBuilder para encadeamento (fluent syntax).</returns>
    public static ILoggingBuilder AddFrotiXLogger(this ILoggingBuilder builder, ILogService logService, LogLevel minimumLevel = LogLevel.Warning)
    {
        // [REGISTRO] - Adiciona FrotiXLoggerProvider ao pipeline de logging do ASP.NET Core
        builder.AddProvider(new FrotiXLoggerProvider(logService, minimumLevel));
        return builder;
    }
}
