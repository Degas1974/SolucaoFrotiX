# Services/LogErrosAlertService.cs

**ARQUIVO NOVO** | 205 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Hubs;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services;

public class LogErrosAlertService : BackgroundService
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<AlertasHub> _hubContext;
    private readonly ILogger<LogErrosAlertService> _logger;

    private readonly LogAlertConfig _config;
    private Timer? _checkTimer;

    private readonly ConcurrentDictionary<string, DateTime> _alertasSent = new();
    private const int ALERT_COOLDOWN_MINUTES = 5;

    private DateTime _ultimaVerificacao = DateTime.MinValue;

    public LogErrosAlertService(
        IServiceProvider serviceProvider,
        IHubContext<AlertasHub> hubContext,
        ILogger<LogErrosAlertService> logger)
    {
        Console.WriteLine("[DIAG-CTOR] LogErrosAlertService INICIO...");
        _serviceProvider = serviceProvider;
        _hubContext = hubContext;
        _logger = logger;

        _config = new LogAlertConfig
        {
            ThresholdErrosPorHora = 100,
            ThresholdErrosPorMinuto = 20,
            ThresholdErrosCriticos = 5,
            ThresholdMesmoErro = 10,
            AlertarNovosErros = true,
            AlertarPicos = true
        };
        Console.WriteLine("[DIAG-CTOR] LogErrosAlertService FIM");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("🔔 LogErrosAlertService iniciado - Monitorando logs em tempo real");

            _checkTimer = new Timer(
                VerificarAlertas,
                null,
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(30)
            );

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao iniciar LogErrosAlertService");
        }
    }

    private async void VerificarAlertas(object? state)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<ILogRepository>();

            if (repository == null)
            {
                _logger.LogWarning("ILogRepository não disponível para verificação de alertas");
                return;
            }

            var alertas = await repository.CheckThresholdsAsync(_config);

            foreach (var alerta in alertas)
            {
                await EnviarAlertaSeNecessario(alerta);
            }

            var anomalias = await repository.DetectAnomaliesAsync(1, 2.0);
            foreach (var anomalia in anomalias.Where(a => a.Severidade == "high"))
            {
                var alertaAnomalia = new LogThresholdAlert
                {
                    Tipo = "ANOMALIA",
                    Descricao = anomalia.Mensagem,
                    ValorAtual = anomalia.CountErros,
                    Threshold = (int)anomalia.Media,
                    PercentualExcedido = anomalia.ZScore * 100,
                    Severidade = "high",
                    DetectadoEm = anomalia.DataHora
                };
                await EnviarAlertaSeNecessario(alertaAnomalia);
            }

            var criticosNaoResolvidos = await repository.GetUnresolvedCriticalAsync(10);
            if (criticosNaoResolvidos.Count >= _config.ThresholdErrosCriticos)
            {
                var alertaCritico = new LogThresholdAlert
                {
                    Tipo = "CRITICOS_NAO_RESOLVIDOS",
                    Descricao = $"{criticosNaoResolvidos.Count} erros críticos aguardando resolução",
                    ValorAtual = criticosNaoResolvidos.Count,
                    Threshold = _config.ThresholdErrosCriticos,
                    PercentualExcedido = (double)criticosNaoResolvidos.Count / _config.ThresholdErrosCriticos * 100 - 100,
                    Severidade = "high",
                    DetectadoEm = DateTime.Now
                };
                await EnviarAlertaSeNecessario(alertaCritico);
            }

            _ultimaVerificacao = DateTime.Now;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na verificação de alertas de logs");
        }
    }

    private async Task EnviarAlertaSeNecessario(LogThresholdAlert alerta)
    {
        try
        {

            var alertaKey = $"{alerta.Tipo}_{alerta.Descricao?.GetHashCode()}";
            if (_alertasSent.TryGetValue(alertaKey, out var lastSent))
            {
                if ((DateTime.Now - lastSent).TotalMinutes < ALERT_COOLDOWN_MINUTES)
                {
                    return;
                }
            }

            var alertaPayload = new
            {
                id = Guid.NewGuid().ToString(),
                tipo = alerta.Tipo,
                titulo = GetTituloAlerta(alerta),
                descricao = alerta.Descricao,
                valorAtual = alerta.ValorAtual,
                threshold = alerta.Threshold,
                severidade = alerta.Severidade,
                icone = GetIconeAlerta(alerta),
                cor = GetCorAlerta(alerta),
                dataHora = alerta.DetectadoEm,
                origem = "LogErros"
            };

            await _hubContext.Clients.All.SendAsync("AlertaLogErro", alertaPayload);

            _logger.LogWarning("🚨 Alerta de Log enviado: {Tipo} - {Descricao}", alerta.Tipo, alerta.Descricao);

            _alertasSent[alertaKey] = DateTime.Now;

            LimparCacheAntigo();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar alerta via SignalR");
        }
    }

    private string GetTituloAlerta(LogThresholdAlert alerta)
    {
        return alerta.Tipo switch
        {
            "ERROS_POR_HORA" => "⚠️ Alto Volume de Erros/Hora",
            "ERROS_POR_MINUTO" => "🚨 Pico de Erros/Minuto",
            "ERROS_CRITICOS" => "❌ Erros Críticos Detectados",
            "MESMO_ERRO" => "🔁 Erro Repetido Múltiplas Vezes",
            "ANOMALIA" => "📈 Anomalia Detectada",
            "CRITICOS_NAO_RESOLVIDOS" => "⏰ Erros Críticos Pendentes",
            _ => "⚡ Alerta de Log"
        };
    }

    private string GetIconeAlerta(LogThresholdAlert alerta)
    {
        return alerta.Tipo switch
        {
            "ERROS_POR_HORA" => "fa-duotone fa-clock",
            "ERROS_POR_MINUTO" => "fa-duotone fa-bolt",
            "ERROS_CRITICOS" => "fa-duotone fa-circle-exclamation",
            "MESMO_ERRO" => "fa-duotone fa-repeat",
            "ANOMALIA" => "fa-duotone fa-chart-line-up",
            "CRITICOS_NAO_RESOLVIDOS" => "fa-duotone fa-hourglass-clock",
            _ => "fa-duotone fa-bell"
        };
    }

    private string GetCorAlerta(LogThresholdAlert alerta)
    {
        return alerta.Severidade switch
        {
            "high" => "#dc3545",
            "medium" => "#fd7e14",
            "low" => "#ffc107",
            _ => "#6c757d"
        };
    }

    private void LimparCacheAntigo()
    {
        var agora = DateTime.Now;
        var keysParaRemover = _alertasSent
            .Where(kv => (agora - kv.Value).TotalMinutes > ALERT_COOLDOWN_MINUTES * 2)
            .Select(kv => kv.Key)
            .ToList();

        foreach (var key in keysParaRemover)
        {
            _alertasSent.TryRemove(key, out _);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔔 LogErrosAlertService parando...");

        _checkTimer?.Change(Timeout.Infinite, 0);
        _checkTimer?.Dispose();

        await base.StopAsync(cancellationToken);
    }
}
```
