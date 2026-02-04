/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LogErrosAlertService.cs                                                                ║
   ║ 📂 CAMINHO: /Services                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: BackgroundService que monitora logs de erros e dispara alertas via SignalR.           ║
   ║              Verifica thresholds configurados e detecta anomalias em tempo real.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 FUNCIONALIDADES:                                                                                ║
   ║ • Monitora erros por minuto/hora                                                                   ║
   ║ • Detecta picos de erros (anomalias)                                                              ║
   ║ • Alerta sobre erros críticos não resolvidos                                                       ║
   ║ • Notifica administradores via SignalR                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: IHubContext<AlertasHub>, ILogRepository                                                   ║
   ║ 📅 31/01/2026 | 👤 Claude Code | 📝 v1.0                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ SERVICE: LogErrosAlertService                                                      │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO: Serviço em background que monitora logs de erros e envia alertas       │
/// │              em tempo real via SignalR quando thresholds são ultrapassados.          │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
public class LogErrosAlertService : BackgroundService
{
    // ====== DEPENDÊNCIAS ======
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<AlertasHub> _hubContext;
    private readonly ILogger<LogErrosAlertService> _logger;

    // ====== CONFIGURAÇÃO ======
    private readonly LogAlertConfig _config;
    private Timer? _checkTimer;

    // ====== CACHE DE ALERTAS ENVIADOS (evita spam) ======
    private readonly ConcurrentDictionary<string, DateTime> _alertasSent = new();
    private const int ALERT_COOLDOWN_MINUTES = 5;

    // ====== MÉTRICAS ======
    private DateTime _ultimaVerificacao = DateTime.MinValue;

    public LogErrosAlertService(
        IServiceProvider serviceProvider,
        IHubContext<AlertasHub> hubContext,
        ILogger<LogErrosAlertService> logger)
    {
        _serviceProvider = serviceProvider;
        _hubContext = hubContext;
        _logger = logger;

        // [CONFIG] Thresholds padrão
        _config = new LogAlertConfig
        {
            ThresholdErrosPorHora = 100,
            ThresholdErrosPorMinuto = 20,
            ThresholdErrosCriticos = 5,
            ThresholdMesmoErro = 10,
            AlertarNovosErros = true,
            AlertarPicos = true
        };
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: ExecuteAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Inicializar o BackgroundService e configurar Timer para verificação
     *                   periódica de alertas de erros (a cada 30 segundos)
     *
     * 📥 ENTRADAS     : stoppingToken [CancellationToken] - Token para parar serviço
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona do ciclo de vida
     *
     * ⬅️ CHAMADO POR  : ASP.NET Core Host (durante startup)
     *
     * ➡️ CHAMA        : VerificarAlertas() [callback Timer, linha 88]
     *
     * 📝 OBSERVAÇÕES  : Timer roda a cada 30s com delay inicial de 10s para não
     *                   saturar no startup. Monitoramento contínuo em background.
     ***********************************************************************************/
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("🔔 LogErrosAlertService iniciado - Monitorando logs em tempo real");

            // [TIMER] Verificar a cada 30 segundos
            _checkTimer = new Timer(
                VerificarAlertas,
                null,
                TimeSpan.FromSeconds(10), // Delay inicial
                TimeSpan.FromSeconds(30)  // Intervalo
            );

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao iniciar LogErrosAlertService");
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: VerificarAlertas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Verificar thresholds de erros, detectar anomalias e erros críticos
     *                   não resolvidos, disparando alertas quando necessário
     *
     * 📥 ENTRADAS     : state [object?] - Parâmetro do Timer (null)
     *
     * 📤 SAÍDAS       : void - Callback assíncrono do Timer
     *
     * ⬅️ CHAMADO POR  : Timer no ExecuteAsync() [linha 102] - a cada 30 segundos
     *
     * ➡️ CHAMA        : repository.CheckThresholdsAsync() [linha 130]
     *                   repository.DetectAnomaliesAsync() [linha 137]
     *                   repository.GetUnresolvedCriticalAsync() [linha 155]
     *                   EnviarAlertaSeNecessario() [linhas 134, 151, 168]
     *
     * 📝 OBSERVAÇÕES  : Verifica 3 tipos de anomalias: thresholds, anomalias estatísticas
     *                   e erros críticos não resolvidos. Try-catch abrangente.
     ***********************************************************************************/
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

            // [CHECK] Verificar thresholds
            var alertas = await repository.CheckThresholdsAsync(_config);

            foreach (var alerta in alertas)
            {
                await EnviarAlertaSeNecessario(alerta);
            }

            // [CHECK] Verificar anomalias
            var anomalias = await repository.DetectAnomaliesAsync(1, 2.0); // Último dia, threshold 2.0
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

            // [CHECK] Erros críticos não resolvidos
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

    /***********************************************************************************
     * ⚡ FUNÇÃO: EnviarAlertaSeNecessario
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Verificar cooldown e descartar duplicatas, depois enviar alerta
     *                   via SignalR para todos os clientes conectados
     *
     * 📥 ENTRADAS     : alerta [LogThresholdAlert] - Detalhes do alerta a enviar
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona de envio
     *
     * ⬅️ CHAMADO POR  : VerificarAlertas() [linhas 154, 171, 188]
     *
     * ➡️ CHAMA        : _alertasSent.TryGetValue() [cache check]
     *                   GetTituloAlerta() [linha 200]
     *                   GetIconeAlerta() [linha 201]
     *                   GetCorAlerta() [linha 202]
     *                   _hubContext.Clients.All.SendAsync() [SignalR broadcast]
     *                   LimparCacheAntigo() [linha 219]
     *
     * 📝 OBSERVAÇÕES  : Implementa cooldown de 5 minutos para evitar spam. Cache
     *                   limpeza automática de entradas obsoletas (> 10 min).
     ***********************************************************************************/
    private async Task EnviarAlertaSeNecessario(LogThresholdAlert alerta)
    {
        try
        {
            // [CACHE] Verificar cooldown para evitar spam
            var alertaKey = $"{alerta.Tipo}_{alerta.Descricao?.GetHashCode()}";
            if (_alertasSent.TryGetValue(alertaKey, out var lastSent))
            {
                if ((DateTime.Now - lastSent).TotalMinutes < ALERT_COOLDOWN_MINUTES)
                {
                    return; // Ainda em cooldown
                }
            }

            // [SIGNALR] Enviar alerta para todos os usuários conectados
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

            // Enviar para todos os clientes (broadcast)
            await _hubContext.Clients.All.SendAsync("AlertaLogErro", alertaPayload);

            // [LOG] Registrar envio
            _logger.LogWarning("🚨 Alerta de Log enviado: {Tipo} - {Descricao}", alerta.Tipo, alerta.Descricao);

            // [CACHE] Atualizar cache
            _alertasSent[alertaKey] = DateTime.Now;

            // [CLEANUP] Limpar entradas antigas do cache
            LimparCacheAntigo();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar alerta via SignalR");
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: GetTituloAlerta
     * ⚡ FUNÇÃO: GetIconeAlerta
     * ⚡ FUNÇÃO: GetCorAlerta
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Retornar título, ícone e cor do alerta baseado no tipo de
     *                   anomalia/threshold detectada. Usados na construção do payload
     *
     * 📥 ENTRADAS     : alerta [LogThresholdAlert] - tipo do alerta (enum)
     *
     * 📤 SAÍDAS       : string - Título legível, classe CSS FontAwesome, ou código HEX
     *
     * ⬅️ CHAMADO POR  : EnviarAlertaSeNecessario() [linhas 237-239]
     *
     * ➡️ CHAMA        : Switch expression (nenhuma dependência)
     *
     * 📝 OBSERVAÇÕES  : Simples mapeia tipo → apresentação visual. Usam switch expression.
     *                   GetTituloAlerta inclui emoji descritivos. GetCorAlerta baseado
     *                   em severidade (high=vermelho, medium=laranja, low=amarelo).
     ***********************************************************************************/
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

    /***********************************************************************************
     * ⚡ FUNÇÃO: LimparCacheAntigo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Remover entradas do cache de cooldown com idade > 10 minutos
     *                   para evitar crescimento infinito do dicionário
     *
     * 📥 ENTRADAS     : Nenhuma (opera sobre _alertasSent)
     *
     * 📤 SAÍDAS       : void - Cache atualizado
     *
     * ⬅️ CHAMADO POR  : EnviarAlertaSeNecessario() [linha 257]
     *
     * ➡️ CHAMA        : _alertasSent.Where/Select/ToList(), TryRemove()
     *
     * 📝 OBSERVAÇÕES  : Executa a cada alerta enviado. Remove chaves com
     *                   lastSent > ALERT_COOLDOWN_MINUTES * 2 (10 min).
     ***********************************************************************************/
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

    /***********************************************************************************
     * ⚡ FUNÇÃO: StopAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Finalizar o BackgroundService de forma segura. Parar Timer
     *                   e chamar base.StopAsync
     *
     * 📥 ENTRADAS     : cancellationToken [CancellationToken] - Token de cancelamento
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona de parada
     *
     * ⬅️ CHAMADO POR  : ASP.NET Core Host (durante shutdown)
     *
     * ➡️ CHAMA        : _checkTimer.Change(), _checkTimer.Dispose()
     *                   base.StopAsync()
     *
     * 📝 OBSERVAÇÕES  : Parar Timer evita verificações pendentes após shutdown.
     ***********************************************************************************/
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔔 LogErrosAlertService parando...");

        _checkTimer?.Change(Timeout.Infinite, 0);
        _checkTimer?.Dispose();

        await base.StopAsync(cancellationToken);
    }
}
