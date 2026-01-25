using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FrotiX.Hubs;
using FrotiX.Repository.IRepository;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: AlertasBackgroundService                                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Serviço em background que monitora e notifica alertas agendados via      ║
    /// ║    SignalR. Executa verificações periódicas a cada minuto.                   ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Sistema crítico de notificações em tempo real para CNH vencidas,         ║
    /// ║    CRLV expirado, manutenções pendentes e outros alertas do sistema.        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • ExecuteAsync() → Inicia o serviço e timer periódico                     ║
    /// ║    • VerificarAlertasAgendados() → Busca e notifica alertas pendentes       ║
    /// ║    • VerificarAlertasExpirados() → Marca alertas vencidos como expirados    ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INTERNA - Serviço de infraestrutura do sistema                    ║
    /// ║    • Arquivos relacionados: AlertasHub.cs, AlertasFrotiX (Model)            ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class AlertasBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<AlertasHub> _hubContext;
        private readonly ILogger<AlertasBackgroundService> _logger;
        private Timer _timer;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AlertasBackgroundService (Construtor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o serviço de background de alertas.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • serviceProvider (IServiceProvider): Provider para criar scopes          ║
        /// ║    • hubContext (IHubContext<AlertasHub>): Contexto SignalR para notificar   ║
        /// ║    • logger (ILogger): Logger para registro de eventos                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public AlertasBackgroundService(
            IServiceProvider serviceProvider,
            IHubContext<AlertasHub> hubContext,
            ILogger<AlertasBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
            _logger = logger;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExecuteAsync                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Método principal do BackgroundService que inicia o timer de verificação.  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • stoppingToken (CancellationToken): Token de cancelamento                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task: Tarefa assíncrona                                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
            try
                {
                _logger.LogInformation("Serviço de Alertas FrotiX iniciado");

                // Verificar alertas a cada minuto
                _timer = new Timer(
                    VerificarAlertasAgendados ,
                    null ,
                    TimeSpan.Zero ,
                    TimeSpan.FromMinutes(1)
                );

                await Task.CompletedTask;
                }
            catch (Exception ex)
                {
                _logger.LogError(ex , "Erro ao iniciar serviço de alertas");
                Alerta.TratamentoErroComLinha("AlertasBackgroundService.cs" , "ExecuteAsync" , ex);
                }
            }

        private async void VerificarAlertasAgendados(object state)
            {
            try
                {
                using (var scope = _serviceProvider.CreateScope())
                    {
                    var alertasRepo = scope.ServiceProvider.GetRequiredService<IAlertasFrotiXRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // Buscar alertas para notificar
                    var alertasParaNotificar = await alertasRepo.GetAlertasParaNotificarAsync();

                    foreach (var alerta in alertasParaNotificar)
                        {
                        try
                            {
                            // Obter usuários não notificados
                            var usuariosNaoNotificados = alerta.AlertasUsuarios
                                .Where(au => !au.Notificado && !au.Lido)
                                .Select(au => au.UsuarioId)
                                .ToList();

                            if (usuariosNaoNotificados.Any())
                                {
                                // Enviar notificação via SignalR
                                foreach (var usuarioId in usuariosNaoNotificados)
                                    {
                                    await _hubContext.Clients.User(usuarioId).SendAsync("NovoAlerta" , new
                                        {
                                        alertaId = alerta.AlertasFrotiXId ,
                                        titulo = alerta.Titulo ,
                                        descricao = alerta.Descricao ,
                                        tipo = alerta.TipoAlerta ,
                                        prioridade = alerta.Prioridade ,
                                        iconeCss = ObterIconePorTipo(alerta.TipoAlerta) ,
                                        corBadge = ObterCorPorTipo(alerta.TipoAlerta) ,
                                        textoBadge = ObterTextoPorTipo(alerta.TipoAlerta) ,
                                        dataInsercao = alerta.DataInsercao
                                        });

                                    // Marcar como notificado
                                    var alertaUsuario = alerta.AlertasUsuarios
                                        .FirstOrDefault(au => au.UsuarioId == usuarioId);

                                    if (alertaUsuario != null)
                                        {
                                        alertaUsuario.Notificado = true;
                                        }
                                    }

                                // Salvar alterações
                                await unitOfWork.SaveAsync();

                                _logger.LogInformation($"Alerta {alerta.AlertasFrotiXId} notificado para {usuariosNaoNotificados.Count} usuários");
                                }
                            }
                        catch (Exception ex)
                            {
                            _logger.LogError(ex , $"Erro ao processar alerta {alerta.AlertasFrotiXId}");
                            }
                        }

                    // Verificar e desativar alertas expirados
                    await VerificarAlertasExpirados(unitOfWork , alertasRepo);
                    }
                }
            catch (Exception ex)
                {
                _logger.LogError(ex , "Erro ao verificar alertas agendados");
                Alerta.TratamentoErroComLinha("AlertasBackgroundService.cs" , "VerificarAlertasAgendados" , ex);
                }
            }

        private async Task VerificarAlertasExpirados(IUnitOfWork unitOfWork , IAlertasFrotiXRepository alertasRepo)
            {
            try
                {
                var agora = DateTime.Now;

                var alertasExpirados = await unitOfWork.AlertasFrotiX.GetAllAsync(
                    a => a.Ativo &&
                         a.DataExpiracao.HasValue &&
                         a.DataExpiracao.Value < agora
                );

                foreach (var alerta in alertasExpirados)
                    {
                    alerta.Ativo = false;
                    alertasRepo.Update(alerta);
                    }

                if (alertasExpirados.Any())
                    {
                    await unitOfWork.SaveAsync();
                    _logger.LogInformation($"{alertasExpirados.Count()} alertas foram desativados por expiração");
                    }
                }
            catch (Exception ex)
                {
                _logger.LogError(ex , "Erro ao verificar alertas expirados");
                Alerta.TratamentoErroComLinha("AlertasBackgroundService.cs" , "VerificarAlertasExpirados" , ex);
                }
            }

        public override async Task StopAsync(CancellationToken cancellationToken)
            {
            try
                {
                _logger.LogInformation("Serviço de Alertas FrotiX está sendo finalizado");

                _timer?.Change(Timeout.Infinite , 0);
                _timer?.Dispose();

                await base.StopAsync(cancellationToken);
                }
            catch (Exception ex)
                {
                _logger.LogError(ex , "Erro ao parar serviço de alertas");
                Alerta.TratamentoErroComLinha("AlertasBackgroundService.cs" , "StopAsync" , ex);
                }
            }

        private string ObterIconePorTipo(Models.TipoAlerta tipo)
            {
            return tipo switch
                {
                    Models.TipoAlerta.Agendamento => "fa-duotone fa-calendar-check",
                    Models.TipoAlerta.Manutencao => "fa-duotone fa-screwdriver-wrench",
                    Models.TipoAlerta.Motorista => "fa-duotone fa-id-card-clip",
                    Models.TipoAlerta.Veiculo => "fa-duotone fa-car-bus",
                    Models.TipoAlerta.Anuncio => "fa-duotone fa-bullhorn",
                    _ => "fa-duotone fa-circle-info"
                    };
            }

        private string ObterCorPorTipo(Models.TipoAlerta tipo)
            {
            return tipo switch
                {
                    Models.TipoAlerta.Agendamento => "#0ea5e9",
                    Models.TipoAlerta.Manutencao => "#f59e0b",
                    Models.TipoAlerta.Motorista => "#14b8a6",
                    Models.TipoAlerta.Veiculo => "#7c3aed",
                    Models.TipoAlerta.Anuncio => "#dc2626",
                    _ => "#6c757d"
                    };
            }

        private string ObterTextoPorTipo(Models.TipoAlerta tipo)
            {
            return tipo switch
                {
                    Models.TipoAlerta.Agendamento => "Agendamento",
                    Models.TipoAlerta.Manutencao => "Manutenção",
                    Models.TipoAlerta.Motorista => "Motorista",
                    Models.TipoAlerta.Veiculo => "Veículo",
                    Models.TipoAlerta.Anuncio => "Anúncio",
                    _ => "Aniversário"
                    };
            }
        }
    }
