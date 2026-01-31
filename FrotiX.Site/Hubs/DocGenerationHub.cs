/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DocGenerationHub.cs                                                                    ║
   ║ 📂 CAMINHO: /Hubs                                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Hub SignalR para progresso em tempo real durante geração de documentação.             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: 1.[OnConnectedAsync] 2.[OnDisconnectedAsync] 3.[SubscribeToJob] 4.[UnsubscribeFromJob]  ║
   ║ 🔗 DEPS: Microsoft.AspNetCore.SignalR, Alerta | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0               ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrotiX.Hubs
{
    
    // Hub SignalR para envio de progresso em tempo real durante geração de documentação
    
    public class DocGenerationHub : Hub
    {
        
        // Chamado quando um cliente se conecta ao hub
        
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

        
        // Chamado quando um cliente se desconecta do hub
        
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

        
        // Permite que o cliente se inscreva em um job específico
        
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

        
        // Permite que o cliente cancele a inscrição em um job
        
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
