/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Hubs/AlertasHub.cs                                             ║
 * ║  Descrição: SignalR Hub para sistema de alertas em tempo real.            ║
 * ║             Gerencia grupos de usuário (user_{id}). Métodos:             ║
 * ║             MarcarComoLido, OnConnectedAsync, OnDisconnectedAsync.        ║
 * ║             Envia AlertaMarcadoComoLido para clientes.                    ║
 * ║  Data: 28/01/2026 | LOTE: 21                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FrotiX.Hubs
{
    public class AlertasHub :Hub
    {
        // Método que pode ser chamado do cliente para marcar alerta como lido
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

        // Método chamado quando um cliente conecta
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

        // Método chamado quando um cliente desconecta
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
