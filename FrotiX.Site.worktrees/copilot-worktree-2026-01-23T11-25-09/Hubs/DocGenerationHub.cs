/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  HUBS SIGNALR - COMUNICAÇÃO EM TEMPO REAL                                           #
*  #   SUBMÓDULO: GERAÇÃO DE DOCUMENTAÇÃO                                                          #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrotiX.Hubs
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: DocGenerationHub                                                    ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Hub SignalR para comunicação em tempo real durante geração de             ║
    /// ║    documentação automática. Envia progresso, status e logs ao frontend.      ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA:                                                              ║
    /// ║    UX crítico. Mantém usuário informado durante processos longos de          ║
    /// ║    geração de docs (pode levar minutos). Evita timeout perception.          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📡 EVENTOS ENVIADOS AO CLIENTE:                                              ║
    /// ║    • Connected → Confirmação de conexão com ConnectionId                     ║
    /// ║    • SubscribedToJob → Confirmação de inscrição em job específico            ║
    /// ║    • ProgressUpdate → Atualização de progresso (%)                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 MÉTODOS CHAMÁVEIS PELO CLIENTE:                                           ║
    /// ║    • SubscribeToJob(jobId) → Inscrever-se em job específico                  ║
    /// ║    • UnsubscribeFromJob(jobId) → Cancelar inscrição                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: TEMPO REAL - Conexão WebSocket bidirecional                       ║
    /// ║    • Arquivos relacionados: DocGenerator (Service), _Layout.cshtml          ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class DocGenerationHub : Hub
    {
        /// <summary>
        /// Chamado quando um cliente se conecta ao hub
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();
                await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGenerationHub.cs", "OnConnectedAsync", error);
            }
        }

        /// <summary>
        /// Chamado quando um cliente se desconecta do hub
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGenerationHub.cs", "OnDisconnectedAsync", error);
            }
        }

        /// <summary>
        /// Permite que o cliente se inscreva em um job específico
        /// </summary>
        public async Task SubscribeToJob(string jobId)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"job_{jobId}");
                await Clients.Caller.SendAsync("SubscribedToJob", jobId);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGenerationHub.cs", "SubscribeToJob", error);
            }
        }

        /// <summary>
        /// Permite que o cliente cancele a inscrição em um job
        /// </summary>
        public async Task UnsubscribeFromJob(string jobId)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"job_{jobId}");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocGenerationHub.cs", "UnsubscribeFromJob", error);
            }
        }
    }
}
