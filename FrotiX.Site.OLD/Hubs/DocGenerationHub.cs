/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DocGenerationHub.cs                                                                   ║
   ║ 📂 CAMINHO: Hubs/                                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Hub SignalR para progresso em tempo real durante geração de documentação.                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OnConnectedAsync()                                                                           ║
   ║    • OnDisconnectedAsync(Exception? exception)                                                    ║
   ║    • SubscribeToJob(string jobId)                                                                 ║
   ║    • UnsubscribeFromJob(string jobId)                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Microsoft.AspNetCore.SignalR, Alerta                                              ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrotiX.Hubs
{
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: DocGenerationHub                                                                  │
    // │ 📦 HERDA DE: Hub                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Enviar progresso em tempo real durante geração de documentação via SignalR.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Clientes SignalR / Pipeline Hub
    // ➡️ CHAMA       : Clients.Caller.SendAsync(), Groups.AddToGroupAsync(), Groups.RemoveFromGroupAsync()
    //
    public class DocGenerationHub : Hub
    {
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnConnectedAsync                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : base.OnConnectedAsync(), Clients.Caller.SendAsync()                  │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Registrar a conexão e notificar o cliente com o ConnectionId.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de conexão.
        //
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

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnDisconnectedAsync                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : base.OnDisconnectedAsync()                                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Executar rotina padrão ao desconectar o cliente do hub.
        //
        // 📥 PARÂMETROS:
        // exception - Exceção gerada durante a desconexão (se houver).
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de desconexão.
        //
        // Param exception: Exceção gerada durante a desconexão (se houver).
        // Returns: Task de desconexão do SignalR.
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

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SubscribeToJob                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Cliente SignalR                                                     │
        // │    ➡️ CHAMA       : Groups.AddToGroupAsync(), Clients.Caller.SendAsync()                 │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Inscrever a conexão atual no grupo do job e confirmar inscrição.
        //
        // 📥 PARÂMETROS:
        // jobId - Identificador do job.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de inscrição.
        //
        // Param jobId: Identificador do job.
        // Returns: Task da inscrição no grupo.
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

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: UnsubscribeFromJob                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Cliente SignalR                                                     │
        // │    ➡️ CHAMA       : Groups.RemoveFromGroupAsync()                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Remover a conexão atual do grupo do job.
        //
        // 📥 PARÂMETROS:
        // jobId - Identificador do job.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de cancelamento de inscrição.
        //
        // Param jobId: Identificador do job.
        // Returns: Task do cancelamento de inscrição.
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
