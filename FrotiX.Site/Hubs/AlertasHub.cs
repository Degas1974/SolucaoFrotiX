/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: AlertasHub.cs                                                                         ║
   ║ 📂 CAMINHO: Hubs/                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Hub SignalR para alertas em tempo real. Gerencia grupos por usuário (user_{id})                 ║
   ║    e eventos de conexão/desconexão.                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MarcarComoLido(string alertaId, string usuarioId)                                             ║
   ║    • OnConnectedAsync()                                                                           ║
   ║    • OnDisconnectedAsync(Exception exception)                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Microsoft.AspNetCore.SignalR                                                     ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FrotiX.Hubs
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: AlertasHub                                                                         │
    // │ 📦 HERDA DE: Hub                                                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Disponibilizar eventos SignalR para alertas em tempo real por usuário.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Clientes SignalR / Pipeline Hub
    // ➡️ CHAMA       : Clients.User().SendAsync(), Groups.AddToGroupAsync(), Groups.RemoveFromGroupAsync()
    
    
    public class AlertasHub :Hub
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MarcarComoLido                                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Cliente SignalR                                                     │
        // │    ➡️ CHAMA       : Clients.User().SendAsync()                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Notificar o cliente que um alerta foi marcado como lido.
        
        
        
        // 📥 PARÂMETROS:
        // alertaId - Identificador do alerta
        // usuarioId - Identificador do usuário alvo
        
        
        // Param alertaId: Identificador do alerta.
        // Param usuarioId: Identificador do usuário alvo.
        public async Task MarcarComoLido(string alertaId , string usuarioId)
        {
            try
            {
                await Clients.User(usuarioId).SendAsync("AlertaMarcadoComoLido" , alertaId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em MarcarComoLido: {ex.Message}");
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnConnectedAsync                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : Groups.AddToGroupAsync(), base.OnConnectedAsync()                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Registrar conexão do cliente e adicioná-lo ao grupo user_{id} quando identificado.
        
        
        
        // 📤 RETORNO:
        // Task - Operação assíncrona de conexão.
        
        
        // Returns: Task de conexão do SignalR.
        public override async Task OnConnectedAsync()
        {
            try
            {
                Console.WriteLine("=== CLIENTE CONECTANDO ===");

                var usuarioId = Context.UserIdentifier;
                Console.WriteLine($"UserIdentifier: {usuarioId ?? "NULL"}");

                var userName = Context.User?.Identity?.Name;
                Console.WriteLine($"User Name: {userName ?? "NULL"}");

                var isAuthenticated = Context.User?.Identity?.IsAuthenticated ?? false;
                Console.WriteLine($"IsAuthenticated: {isAuthenticated}");

                // Só adiciona ao grupo se houver usuário identificado
                if (!string.IsNullOrEmpty(usuarioId))
                {
                    var groupName = $"user_{usuarioId}";
                    Console.WriteLine($"Adicionando ao grupo: {groupName}");
                    await Groups.AddToGroupAsync(Context.ConnectionId , groupName);
                }
                else
                {
                    Console.WriteLine("AVISO: UserIdentifier está nulo!");
                }

                await base.OnConnectedAsync();
                Console.WriteLine("=== CONEXÃO BEM-SUCEDIDA ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERRO em OnConnectedAsync: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                throw; // Re-lançar para que o SignalR trate
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnDisconnectedAsync                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : Groups.RemoveFromGroupAsync(), base.OnDisconnectedAsync()             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Remover o cliente do grupo user_{id} ao desconectar.
        
        
        
        // 📥 PARÂMETROS:
        // exception - Exceção gerada durante a desconexão (se houver).
        
        
        
        // 📤 RETORNO:
        // Task - Operação assíncrona de desconexão.
        
        
        // Param exception: Exceção gerada durante a desconexão (se houver).
        // Returns: Task de desconexão do SignalR.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                Console.WriteLine("=== CLIENTE DESCONECTANDO ===");

                if (exception != null)
                {
                    Console.WriteLine($"Razão: {exception.Message}");
                }

                var usuarioId = Context.UserIdentifier;

                if (!string.IsNullOrEmpty(usuarioId))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId , $"user_{usuarioId}");
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERRO em OnDisconnectedAsync: {ex.Message}");
            }
        }
    }
}
