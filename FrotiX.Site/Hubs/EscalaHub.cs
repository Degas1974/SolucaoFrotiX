/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EscalaHub.cs                                                                          ║
   ║ 📂 CAMINHO: Hubs/                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Hub SignalR para escala de motoristas em tempo real e serviço de monitoramento                 ║
   ║    em background (EscalaMonitorService) com atualização periódica.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • EscalaHub(ILogger<EscalaHub> logger, IServiceScopeFactory serviceScopeFactory)               ║
   ║    • OnConnectedAsync()                                                                           ║
   ║    • OnDisconnectedAsync(Exception exception)                                                     ║
   ║    • GetMotoristasVez()                                                                           ║
   ║    • GetEscalasDia(DateTime data)                                                                 ║
   ║    • NotificarAlteracaoStatus(Guid motoristaId, string novoStatus)                                ║
   ║    • NotificarNovaViagem(Guid motoristaId)                                                         ║
   ║    • EscalaMonitorService(...)                                                                    ║
   ║    • ExecuteAsync(CancellationToken stoppingToken)                                                ║
   ║    • Dispose()                                                                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: IUnitOfWork, IHubContext, BackgroundService                                       ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
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
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: EscalaHub                                                                         │
    // │ 📦 HERDA DE: Hub                                                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Disponibilizar atualização em tempo real da escala de motoristas via SignalR.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Clientes SignalR / Pipeline Hub
    // ➡️ CHAMA       : IUnitOfWork, Clients.*.SendAsync()
    //
    public class EscalaHub : Hub
    {
        private readonly ILogger<EscalaHub> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: EscalaHub (ctor)                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Program.cs                                                     │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Inicializar o hub com logger e fábrica de escopos de serviço.
        //
        // 📥 PARÂMETROS:
        // logger - Logger do hub de escala
        // serviceScopeFactory - Fábrica de escopos para resolver UnitOfWork
        //
        // Param logger: Logger do hub de escala.
        // Param serviceScopeFactory: Fábrica de escopos para resolver UnitOfWork.
        public EscalaHub(ILogger<EscalaHub> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnConnectedAsync                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : Clients.Caller.SendAsync(), base.OnConnectedAsync()                  │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Notificar o cliente sobre a conexão e registrar no pipeline padrão.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de conexão.
        //
        // Returns: Task de conexão do SignalR.
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnDisconnectedAsync                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : base.OnDisconnectedAsync()                                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Encerrar conexão do cliente seguindo o fluxo padrão.
        //
        // 📥 PARÂMETROS:
        // exception - Exceção gerada durante a desconexão (se houver).
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de desconexão.
        //
        // Param exception: Exceção gerada durante a desconexão (se houver).
        // Returns: Task de desconexão do SignalR.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMotoristasVez                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Cliente SignalR                                                     │
        // │    ➡️ CHAMA       : IUnitOfWork.EscalaDiaria.GetMotoristasVezAsync()                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Buscar motoristas da vez e enviar ao cliente conectado.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de consulta.
        //
        // Returns: Task de consulta e envio ao cliente.
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

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEscalasDia                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Cliente SignalR                                                     │
        // │    ➡️ CHAMA       : IUnitOfWork.EscalaDiaria.GetEscalasCompletasAsync()                  │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Buscar escalas do dia informado e calcular número de saídas.
        //
        // 📥 PARÂMETROS:
        // data - Data de referência para consulta.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de consulta.
        //
        // Param data: Data de referência para consulta.
        // Returns: Task de consulta e envio ao cliente.
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

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: NotificarAlteracaoStatus                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Serviços internos                                                    │
        // │    ➡️ CHAMA       : Clients.All.SendAsync(), GetMotoristasVez()                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Notificar alteração de status do motorista para todos os clientes.
        //
        // 📥 PARÂMETROS:
        // motoristaId - Identificador do motorista
        // novoStatus - Novo status do motorista
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de notificação.
        //
        // Param motoristaId: Identificador do motorista.
        // Param novoStatus: Novo status do motorista.
        // Returns: Task de notificação.
        public async Task NotificarAlteracaoStatus(Guid motoristaId, string novoStatus)
        {
            await Clients.All.SendAsync("StatusMotoristaAlterado", new { motoristaId, novoStatus });
            await GetMotoristasVez();
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: NotificarNovaViagem                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Serviços internos                                                    │
        // │    ➡️ CHAMA       : Clients.All.SendAsync(), GetMotoristasVez()                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Notificar nova viagem registrada e atualizar motoristas da vez.
        //
        // 📥 PARÂMETROS:
        // motoristaId - Identificador do motorista
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de notificação.
        //
        // Param motoristaId: Identificador do motorista.
        // Returns: Task de notificação.
        public async Task NotificarNovaViagem(Guid motoristaId)
        {
            await Clients.All.SendAsync("NovaViagemRegistrada", motoristaId);
            await GetMotoristasVez();
        }
    }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: EscalaMonitorService                                                               │
    // │ 📦 HERDA DE: BackgroundService                                                                │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Monitorar mudanças em viagens e notificar clientes periodicamente.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Host de serviços (BackgroundService)
    // ➡️ CHAMA       : IUnitOfWork, IHubContext<EscalaHub>
    //
    // ⚠️ ATENÇÃO:
    // Timer roda a cada 30s e pode gerar carga dependendo do volume de viagens.
    //
    public class EscalaMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext<EscalaHub> _hubContext;
        private readonly ILogger<EscalaMonitorService> _logger;
        private Timer _timer;

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: EscalaMonitorService (ctor)                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Host                                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Inicializar o serviço de monitoramento com os serviços necessários.
        //
        // 📥 PARÂMETROS:
        // serviceScopeFactory - Fábrica de escopos para obter UnitOfWork
        // hubContext - Contexto do hub para broadcast
        // logger - Logger do serviço
        //
        // Param serviceScopeFactory: Fábrica de escopos para obter UnitOfWork.
        // Param hubContext: Contexto do hub para broadcast.
        // Param logger: Logger do serviço.
        public EscalaMonitorService(
            IServiceScopeFactory serviceScopeFactory,
            IHubContext<EscalaHub> hubContext,
            ILogger<EscalaMonitorService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hubContext = hubContext;
            _logger = logger;
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ExecuteAsync                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : BackgroundService                                                   │
        // │    ➡️ CHAMA       : Timer (CheckForUpdates)                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Iniciar o timer periódico para verificar atualizações.
        //
        // 📥 PARÂMETROS:
        // stoppingToken - Token de cancelamento do host.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona do serviço.
        //
        // Param stoppingToken: Token de cancelamento do host.
        // Returns: Task de execução do serviço.
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(async (state) => await CheckForUpdates(), null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: CheckForUpdates                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Timer                                                               │
        // │    ➡️ CHAMA       : IUnitOfWork.Viagem, IHubContext<EscalaHub>                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Verificar viagens recentes e notificar clientes sobre mudanças.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de verificação.
        //
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

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Dispose                                                               │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Host/DI                                                             │
        // │    ➡️ CHAMA       : Timer.Dispose(), base.Dispose()                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Liberar recursos do timer ao encerrar o serviço.
        //
        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
