# Services/LogErrosCleanupService.cs

**ARQUIVO NOVO** | 121 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
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

public class LogErrosCleanupService : BackgroundService
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<LogErrosCleanupService> _logger;

    private const int DIAS_RETENCAO = 90;
    private readonly TimeSpan _horaExecucao = new TimeSpan(3, 0, 0);

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

                var agora = DateTime.Now;
                var proximaExecucao = agora.Date.Add(_horaExecucao);

                if (agora > proximaExecucao)
                {
                    proximaExecucao = proximaExecucao.AddDays(1);
                }

                var tempoAteExecucao = proximaExecucao - agora;

                _logger.LogInformation("🧹 Próxima limpeza de logs agendada para: {ProximaExecucao}", proximaExecucao);

                await Task.Delay(tempoAteExecucao, stoppingToken);

                await ExecutarLimpeza();
            }
            catch (OperationCanceledException)
            {

                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro na execução do LogErrosCleanupService");

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

            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<ILogRepository>();
                if (repository != null)
                {
                    totalRemovidos = await repository.DeleteBeforeDateAsync(dataLimite);
                    _logger.LogInformation("🗑️ {Total} registros removidos do banco de dados", totalRemovidos);
                }
            }

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
```
