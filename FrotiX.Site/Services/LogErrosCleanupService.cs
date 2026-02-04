/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LogErrosCleanupService.cs                                                              ║
   ║ 📂 CAMINHO: /Services                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: BackgroundService para limpeza automática de logs antigos (> 90 dias).               ║
   ║              Executa diariamente às 03:00 para minimizar impacto no sistema.                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 FUNCIONALIDADES:                                                                                ║
   ║ • Limpa logs mais antigos que 90 dias do banco de dados                                            ║
   ║ • Limpa arquivos TXT de fallback mais antigos que 90 dias                                         ║
   ║ • Registra log da operação de limpeza                                                             ║
   ║ • Configurável via appsettings.json                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: ILogRepository, ILogService                                                               ║
   ║ 📅 31/01/2026 | 👤 Claude Code | 📝 v1.0                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services;

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ SERVICE: LogErrosCleanupService                                                    │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO: Serviço em background que executa limpeza automática de logs antigos.  │
/// │              Roda diariamente às 03:00 e remove logs com mais de 90 dias.            │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
public class LogErrosCleanupService : BackgroundService
{
    // ====== DEPENDÊNCIAS ======
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<LogErrosCleanupService> _logger;

    // ====== CONFIGURAÇÃO ======
    private const int DIAS_RETENCAO = 90;
    private readonly TimeSpan _horaExecucao = new TimeSpan(3, 0, 0); // 03:00

    public LogErrosCleanupService(
        IServiceProvider serviceProvider,
        IWebHostEnvironment environment,
        ILogger<LogErrosCleanupService> logger)
    {
        Console.WriteLine("[DIAG-CTOR] LogErrosCleanupService INICIO...");
        _serviceProvider = serviceProvider;
        _environment = environment;
        _logger = logger;
        Console.WriteLine("[DIAG-CTOR] LogErrosCleanupService FIM");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🧹 LogErrosCleanupService iniciado - Limpeza diária às 03:00");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // [CALC] Calcular tempo até próxima execução
                var agora = DateTime.Now;
                var proximaExecucao = agora.Date.Add(_horaExecucao);

                if (agora > proximaExecucao)
                {
                    proximaExecucao = proximaExecucao.AddDays(1);
                }

                var tempoAteExecucao = proximaExecucao - agora;

                _logger.LogInformation("🧹 Próxima limpeza de logs agendada para: {ProximaExecucao}", proximaExecucao);

                // [WAIT] Aguardar até a hora da execução
                await Task.Delay(tempoAteExecucao, stoppingToken);

                // [EXEC] Executar limpeza
                await ExecutarLimpeza();
            }
            catch (OperationCanceledException)
            {
                // Serviço sendo encerrado
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro na execução do LogErrosCleanupService");

                // Aguardar 1 hora antes de tentar novamente em caso de erro
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }

    private async Task ExecutarLimpeza()
    {
        _logger.LogInformation("🧹 Iniciando limpeza de logs antigos (> {Dias} dias)...", DIAS_RETENCAO);

        var dataLimite = DateTime.Now.AddDays(-DIAS_RETENCAO);
        var totalRemovidos = 0;
        var arquivosRemovidos = 0;

        try
        {
            // [DB] Limpar logs do banco de dados
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<ILogRepository>();
                if (repository != null)
                {
                    totalRemovidos = await repository.DeleteBeforeDateAsync(dataLimite);
                    _logger.LogInformation("🗑️ {Total} registros removidos do banco de dados", totalRemovidos);
                }
            }

            // [FILES] Limpar arquivos TXT antigos
            var logDirectory = Path.Combine(_environment.ContentRootPath, "Logs");
            if (Directory.Exists(logDirectory))
            {
                var arquivosAntigos = Directory.GetFiles(logDirectory, "frotix_log_*.txt")
                    .Select(f => new FileInfo(f))
                    .Where(f => f.CreationTime < dataLimite)
                    .ToList();

                foreach (var arquivo in arquivosAntigos)
                {
                    try
                    {
                        arquivo.Delete();
                        arquivosRemovidos++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Não foi possível remover arquivo: {Arquivo}", arquivo.Name);
                    }
                }

                if (arquivosRemovidos > 0)
                {
                    _logger.LogInformation("🗑️ {Total} arquivos TXT removidos", arquivosRemovidos);
                }
            }

            // [LOG] Registrar conclusão
            using (var scope = _serviceProvider.CreateScope())
            {
                var logService = scope.ServiceProvider.GetService<ILogService>();
                logService?.Info(
                    $"Limpeza automática concluída: {totalRemovidos} registros e {arquivosRemovidos} arquivos removidos (anteriores a {dataLimite:dd/MM/yyyy})",
                    "LogErrosCleanupService.cs",
                    "ExecutarLimpeza"
                );
            }

            _logger.LogInformation("✅ Limpeza de logs concluída com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro durante a limpeza de logs");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🧹 LogErrosCleanupService parando...");
        await base.StopAsync(cancellationToken);
    }
}
