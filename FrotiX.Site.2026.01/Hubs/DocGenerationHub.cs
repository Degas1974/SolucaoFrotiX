/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está documentado em:                                       ║
 * ║  📄 Documentacao/Hubs/DocGenerationHub.md                                ║
 * ║                                                                          ║
 * ║  Última atualização: 13/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrotiX.Hubs
{
    /// <summary>
    /// Hub SignalR para envio de progresso em tempo real durante geração de documentação
    /// </summary>
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
