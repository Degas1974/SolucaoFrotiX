/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: AlertasBackgroundService.cs                                                             ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: BackgroundService que verifica alertas a cada minuto. Envia via SignalR (AlertasHub).  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: ExecuteAsync, VerificarAlertas, ObterIconePorTipo, ObterCorPorTipo, ObterTextoPorTipo    ║
   ║ 🔗 DEPS: IHubContext<AlertasHub>, IUnitOfWork | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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
    public class AlertasBackgroundService :BackgroundService
        {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<AlertasHub> _hubContext;
        private readonly ILogger<AlertasBackgroundService> _logger;
        private Timer _timer;

        public AlertasBackgroundService(
            IServiceProvider serviceProvider ,
            IHubContext<AlertasHub> hubContext ,
            ILogger<AlertasBackgroundService> logger)
            {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
            _logger = logger;
            }

        /***********************************************************************************
         * ⚡ FUNÇÃO: ExecuteAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar o BackgroundService e configurar timer para verificação
         *                   de alertas a cada minuto
         *
         * 📥 ENTRADAS     : stoppingToken [CancellationToken] - Token para parar o serviço
         *
         * 📤 SAÍDAS       : Task - Tarefa assíncrona do ciclo de vida do serviço
         *
         * ⬅️ CHAMADO POR  : Hosted Service Host (ASP.NET Core startup)
         *
         * ➡️ CHAMA        : VerificarAlertasAgendados() [linha 52]
         *
         * 📝 OBSERVAÇÕES  : Executa apenas uma vez durante startup. O Timer roda de forma
         *                   assíncrona em background criando callback a cada minuto.
         ***********************************************************************************/
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
            try
                {
                _logger.LogInformation("Serviço de Alertas FrotiX iniciado");

                // [UI] Verificar alertas a cada minuto
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

        /***********************************************************************************
         * ⚡ FUNÇÃO: VerificarAlertasAgendados
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Verificar alertas pendentes de notificação e enviá-los via SignalR
         *                   para usuários não notificados. Marcar como notificado após envio.
         *
         * 📥 ENTRADAS     : state [object] - Parâmetro do Timer (null)
         *
         * 📤 SAÍDAS       : void - Callback assíncrono do Timer
         *
         * ⬅️ CHAMADO POR  : Timer no ExecuteAsync() [linha 69] - a cada 1 minuto
         *
         * ➡️ CHAMA        : alertasRepo.GetAlertasParaNotificarAsync() [DB]
         *                   _hubContext.Clients.User().SendAsync() [SignalR]
         *                   VerificarAlertasExpirados() [linha 130]
         *                   unitOfWork.SaveAsync() [linha 118]
         *
         * 📝 OBSERVAÇÕES  : Usa DependencyInjection para resolver repositórios. Trata exceções
         *                   por alerta individual para evitar falha total. Envia notificação
         *                   apenas para usuários sem notificação (Notificado=false && Lido=false)
         ***********************************************************************************/
        private async void VerificarAlertasAgendados(object state)
            {
            try
                {
                using (var scope = _serviceProvider.CreateScope())
                    {
                    var alertasRepo = scope.ServiceProvider.GetRequiredService<IAlertasFrotiXRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // [DB] Buscar alertas para notificar (status Ativo=true, ainda não enviados)
                    var alertasParaNotificar = await alertasRepo.GetAlertasParaNotificarAsync();

                    foreach (var alerta in alertasParaNotificar)
                        {
                        try
                            {
                            // [LOGICA] Filtrar usuários que ainda não foram notificados ou leram alerta
                            // Criando lista de IDs para iterar
                            var usuariosNaoNotificados = alerta.AlertasUsuarios
                                .Where(au => !au.Notificado && !au.Lido)
                                .Select(au => au.UsuarioId)
                                .ToList();

                            if (usuariosNaoNotificados.Any())
                                {
                                // [AJAX] Enviar notificação via SignalR para cada usuário
                                foreach (var usuarioId in usuariosNaoNotificados)
                                    {
                                    // [DADOS] Montar objeto com dados do alerta para envio ao cliente
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

                                    // [REGRA] Marcar alerta como notificado para este usuário
                                    var alertaUsuario = alerta.AlertasUsuarios
                                        .FirstOrDefault(au => au.UsuarioId == usuarioId);

                                    if (alertaUsuario != null)
                                        {
                                        alertaUsuario.Notificado = true;
                                        }
                                    }

                                // [DB] Persistir marca de notificação
                                await unitOfWork.SaveAsync();

                                _logger.LogInformation($"Alerta {alerta.AlertasFrotiXId} notificado para {usuariosNaoNotificados.Count} usuários");
                                }
                            }
                        catch (Exception ex)
                            {
                            _logger.LogError(ex , $"Erro ao processar alerta {alerta.AlertasFrotiXId}");
                            }
                        }

                    // [REGRA] Verificar e desativar alertas que passaram data de expiração
                    await VerificarAlertasExpirados(unitOfWork , alertasRepo);
                    }
                }
            catch (Exception ex)
                {
                _logger.LogError(ex , "Erro ao verificar alertas agendados");
                Alerta.TratamentoErroComLinha("AlertasBackgroundService.cs" , "VerificarAlertasAgendados" , ex);
                }
            }

        /***********************************************************************************
         * ⚡ FUNÇÃO: VerificarAlertasExpirados
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar alertas que passaram data de expiração e desativar o campo
         *                   Ativo para remover da próxima verificação (soft delete)
         *
         * 📥 ENTRADAS     : unitOfWork [IUnitOfWork] - Unidade de trabalho para persistência
         *                   alertasRepo [IAlertasFrotiXRepository] - Repositório de alertas
         *
         * 📤 SAÍDAS       : Task - Operação assíncrona completa
         *
         * ⬅️ CHAMADO POR  : VerificarAlertasAgendados() [linha 170]
         *
         * ➡️ CHAMA        : unitOfWork.AlertasFrotiX.GetAllAsync() [DB]
         *                   alertasRepo.Update() [linha 155]
         *                   unitOfWork.SaveAsync() [linha 160]
         *
         * 📝 OBSERVAÇÕES  : Usa DateTime.Now para comparação. Verifica condições:
         *                   Ativo=true AND DataExpiracao.HasValue AND DataExpiracao < agora
         ***********************************************************************************/
        private async Task VerificarAlertasExpirados(IUnitOfWork unitOfWork , IAlertasFrotiXRepository alertasRepo)
            {
            try
                {
                var agora = DateTime.Now;

                // [LOGICA] Buscar alertas que passaram data de expiração
                // Filtar por: Ativo=true AND DataExpiracao != null AND DataExpiracao < agora
                var alertasExpirados = await unitOfWork.AlertasFrotiX.GetAllAsync(
                    a => a.Ativo &&
                         a.DataExpiracao.HasValue &&
                         a.DataExpiracao.Value < agora
                );

                // [LOGICA] Marcando cada alerta como inativo (soft delete)
                foreach (var alerta in alertasExpirados)
                    {
                    alerta.Ativo = false;
                    alertasRepo.Update(alerta);
                    }

                // [DB] Persistir desativação se houver alertas expirados
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

        /***********************************************************************************
         * ⚡ FUNÇÃO: StopAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Finalizar o BackgroundService de forma segura. Parar o Timer
         *                   e chamar base.StopAsync para limpeza de recursos
         *
         * 📥 ENTRADAS     : cancellationToken [CancellationToken] - Token de cancelamento
         *
         * 📤 SAÍDAS       : Task - Operação assíncrona de parada
         *
         * ⬅️ CHAMADO POR  : ASP.NET Core Host (durante shutdown da aplicação)
         *
         * ➡️ CHAMA        : _timer.Change() [linha 238]
         *                   _timer.Dispose() [linha 239]
         *                   base.StopAsync() [linha 241]
         *
         * 📝 OBSERVAÇÕES  : Parar o Timer evita tentativas de notificação após parada do app.
         *                   Dispor garante liberação de recursos.
         ***********************************************************************************/
        public override async Task StopAsync(CancellationToken cancellationToken)
            {
            try
                {
                _logger.LogInformation("Serviço de Alertas FrotiX está sendo finalizado");

                // [UI] Parar timer e liberar recursos
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

        /***********************************************************************************
         * ⚡ FUNÇÃO: ObterIconePorTipo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar classe CSS de ícone FontAwesome Duotone baseado no tipo
         *                   de alerta. Utilizado no objeto de notificação SignalR.
         *
         * 📥 ENTRADAS     : tipo [Models.TipoAlerta] - Tipo de alerta (enum)
         *
         * 📤 SAÍDAS       : string - Classe CSS FontAwesome duotone (ex: "fa-duotone fa-calendar-check")
         *
         * ⬅️ CHAMADO POR  : VerificarAlertasAgendados() [linha 141]
         *
         * ➡️ CHAMA        : Nenhuma dependência externa
         *
         * 📝 OBSERVAÇÕES  : Switch expression para mapeamento tipo→ícone. Retorna ícone
         *                   padrão "fa-duotone fa-circle-info" para tipos desconhecidos.
         ***********************************************************************************/
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

        /***********************************************************************************
         * ⚡ FUNÇÃO: ObterCorPorTipo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar código de cor hexadecimal baseado no tipo de alerta.
         *                   Utilizado para background do badge no frontend.
         *
         * 📥 ENTRADAS     : tipo [Models.TipoAlerta] - Tipo de alerta (enum)
         *
         * 📤 SAÍDAS       : string - Código hexadecimal de cor (ex: "#0ea5e9")
         *
         * ⬅️ CHAMADO POR  : VerificarAlertasAgendados() [linha 142]
         *
         * ➡️ CHAMA        : Nenhuma dependência externa
         *
         * 📝 OBSERVAÇÕES  : Switch expression para mapeamento tipo→cor. Retorna cor neutra
         *                   "#6c757d" (cinza) para tipos desconhecidos. Cores seguem padrão
         *                   Tailwind CSS expandido.
         ***********************************************************************************/
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

        /***********************************************************************************
         * ⚡ FUNÇÃO: ObterTextoPorTipo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar texto legível em português para exibição em badge
         *                   do alerta no frontend
         *
         * 📥 ENTRADAS     : tipo [Models.TipoAlerta] - Tipo de alerta (enum)
         *
         * 📤 SAÍDAS       : string - Texto em português (ex: "Agendamento", "Manutenção")
         *
         * ⬅️ CHAMADO POR  : VerificarAlertasAgendados() [linha 143]
         *
         * ➡️ CHAMA        : Nenhuma dependência externa
         *
         * 📝 OBSERVAÇÕES  : Switch expression para mapeamento tipo→texto. Retorna texto
         *                   padrão "Diversos" para tipos desconhecidos.
         ***********************************************************************************/
        private string ObterTextoPorTipo(Models.TipoAlerta tipo)
            {
            return tipo switch
                {
                    Models.TipoAlerta.Agendamento => "Agendamento",
                    Models.TipoAlerta.Manutencao => "Manutenção",
                    Models.TipoAlerta.Motorista => "Motorista",
                    Models.TipoAlerta.Veiculo => "Veículo",
                    Models.TipoAlerta.Anuncio => "Anúncio",
                    _ => "Diversos"
                    };
            }
        }
    }
