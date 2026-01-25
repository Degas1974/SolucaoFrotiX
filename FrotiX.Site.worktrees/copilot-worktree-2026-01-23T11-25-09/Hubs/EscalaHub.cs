/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  HUBS SIGNALR - GESTÃO DE ESCALAS EM TEMPO REAL                                     #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using FrotiX.Repository.IRepository;
using FrotiX.Models;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace FrotiX.Hubs
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: EscalaHub                                                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Hub SignalR para comunicação em tempo real sobre ESCALAS de motoristas.  ║
    /// ║    Notifica mudanças em escalas, motoristas da vez, conflitos, etc.         ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA CRÍTICA:                                                      ║
    /// ║    OPERAÇÃO EM TEMPO REAL. Quando um despachante altera uma escala, todos  ║
    /// ║    os usuários conectados veem a mudança instantaneamente. Evita conflitos. ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📡 EVENTOS ENVIADOS:                                                         ║
    /// ║    • Connected → Confirmação de conexão                                      ║
    /// ║    • AtualizarMotoristasVez → Lista de próximos motoristas                   ║
    /// ║    • EscalaAtualizada → Notificação de mudança em escala                     ║
    /// ║    • ConflitoDete ctado → Alerta de conflito de horário                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 MÉTODOS CHAMÁVEIS:                                                        ║
    /// ║    • GetMotoristasVez() → Busca próximos N motoristas disponíveis            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: TEMPO REAL - Múltiplos usuários conectados simultaneamente        ║
    /// ║    • Arquivos relacionados: EscalaController, Pages/Escala/*.cshtml         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class EscalaHub : Hub
    {
        private readonly ILogger<EscalaHub> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EscalaHub(ILogger<EscalaHub> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetMotoristasVez()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var motoristasVez = await unitOfWork.EscalaDiaria.GetMotoristasVezAsync(5);
                    await Clients.Caller.SendAsync("AtualizarMotoristasVez", motoristasVez);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar motoristas da vez");
            }
        }

        public async Task GetEscalasDia(DateTime data)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var escalas = await unitOfWork.EscalaDiaria.GetEscalasCompletasAsync(data);

                    var dia = data.Date;
                    var proximoDia = dia.AddDays(1);

                    foreach (var escala in escalas)
                    {
                        if (escala.MotoristaId.HasValue)
                        {
                            var viagens = await unitOfWork.Viagem
                                .GetAllAsync(v => v.MotoristaId == escala.MotoristaId.Value &&
                                                 v.DataFinalizacao >= dia &&
                                                 v.DataFinalizacao < proximoDia &&
                                                 v.Status == "Realizada");
                            escala.NumeroSaidas = viagens.Count();
                        }
                    }
                    
                    await Clients.Caller.SendAsync("AtualizarEscalasDia", escalas);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar escalas do dia");
            }
        }

        public async Task NotificarAlteracaoStatus(Guid motoristaId, string novoStatus)
        {
            await Clients.All.SendAsync("StatusMotoristaAlterado", new { motoristaId, novoStatus });
            await GetMotoristasVez();
        }

        public async Task NotificarNovaViagem(Guid motoristaId)
        {
            await Clients.All.SendAsync("NovaViagemRegistrada", motoristaId);
            await GetMotoristasVez();
        }
    }

    // Serviço em background para monitorar mudanças na tabela Viagem
    public class EscalaMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext<EscalaHub> _hubContext;
        private readonly ILogger<EscalaMonitorService> _logger;
        private Timer _timer;

        public EscalaMonitorService(
            IServiceScopeFactory serviceScopeFactory,
            IHubContext<EscalaHub> hubContext,
            ILogger<EscalaMonitorService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(async (state) => await CheckForUpdates(), null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private async Task CheckForUpdates()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    
                    // Verificar viagens recentes (últimos 30 segundos)
                    var agoraMenos30 = DateTime.Now.AddSeconds(-30);

                    var viagensRecentes = await unitOfWork.Viagem
                        .GetAllAsync(v => v.DataCriacao >= agoraMenos30 ||
                                         v.DataFinalizacao >= agoraMenos30 ||
                                         v.DataCancelamento >= agoraMenos30);


                    if (viagensRecentes.Any())
                    {
                        // Atualizar motoristas da vez
                        var motoristasVez = await unitOfWork.EscalaDiaria.GetMotoristasVezAsync(5);
                        await _hubContext.Clients.All.SendAsync("AtualizarMotoristasVez", motoristasVez);

                        // Notificar sobre mudanças nas viagens
                        foreach (var viagem in viagensRecentes)
                        {
                            var status = viagem.Status;
                            var dataRef = viagem.DataFinalizacao ?? viagem.DataInicial ?? DateTime.Now;

                            await _hubContext.Clients.All.SendAsync("ViagemAtualizada", new
                            {
                                viagemId = viagem.ViagemId,
                                motoristaId = viagem.MotoristaId,
                                status,
                                dataViagem = dataRef
                            });

                            if (viagem.MotoristaId is not Guid motoristaId || motoristaId == Guid.Empty)
                                continue;

                            if (status == "Realizada")
                            {
                                await unitOfWork.EscalaDiaria.AtualizarStatusMotoristaAsync(
                                    motoristaId, "Disponível", dataRef);
                                await unitOfWork.SaveAsync();
                                
                                await _hubContext.Clients.All.SendAsync("StatusMotoristaAlterado", new
                                {
                                    motoristaId,
                                    novoStatus = "Disponível"
                                });
                            }
                            else if (status == "Em Andamento")
                            {
                                await unitOfWork.EscalaDiaria.AtualizarStatusMotoristaAsync(
                                    motoristaId, "Em Viagem", dataRef);
                                await unitOfWork.SaveAsync();
                                
                                await _hubContext.Clients.All.SendAsync("StatusMotoristaAlterado", new
                                {
                                    motoristaId,
                                    novoStatus = "Em Viagem"
                                });
                            }
                        }

                        // Atualizar escalas completas

                        var hoje = DateTime.Today;
                        var amanha = hoje.AddDays(1);
                        var escalasHoje = await unitOfWork.EscalaDiaria.GetEscalasCompletasAsync(hoje);

                        // Adicionar número de viagens realizadas
                        foreach (var escala in escalasHoje)
                        {
                            if (escala.MotoristaId.HasValue)
                            {
                                var viagensMotorista = await unitOfWork.Viagem
                                    .GetAllAsync(v => v.MotoristaId == escala.MotoristaId.Value &&
                                                     v.DataFinalizacao >= hoje &&
                                                     v.DataFinalizacao < amanha &&
                                                     v.Status == "Realizada");
                                escala.NumeroSaidas = viagensMotorista.Count();
                            }
                        }
                        
                        await _hubContext.Clients.All.SendAsync("AtualizarEscalasDia", escalasHoje);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar atualizações de viagens");
            }
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
