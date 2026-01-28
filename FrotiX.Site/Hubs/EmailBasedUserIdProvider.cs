// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EmailBasedUserIdProvider.cs                                         ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Provider customizado para identificação de usuários no SignalR.              ║
// ║ Define o UserIdentifier usado por Clients.User(id).                          ║
// ║                                                                              ║
// ║ ORDEM DE BUSCA:                                                              ║
// ║ 1. ClaimTypes.Email (preferencial)                                           ║
// ║ 2. ClaimTypes.Name (fallback)                                                ║
// ║ 3. ClaimTypes.NameIdentifier (último recurso)                                ║
// ║                                                                              ║
// ║ REGISTRO EM Program.cs:                                                      ║
// ║ services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();          ║
// ║                                                                              ║
// ║ USO:                                                                         ║
// ║ - AlertasHub: Envio de notificações para usuário específico                  ║
// ║ - Context.UserIdentifier retorna o email do usuário                          ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 12                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FrotiX.Hubs
{
    /// <summary>
    /// Provider que define UserIdentifier como email do usuário.
    /// </summary>
    public class EmailBasedUserIdProvider :IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Tenta pegar o email do usuário autenticado
            var email = connection.User?.FindFirst(ClaimTypes.Email)?.Value;

            // Se não encontrar email, tenta o Name
            if (string.IsNullOrEmpty(email))
            {
                email = connection.User?.FindFirst(ClaimTypes.Name)?.Value;
            }

            // Se ainda não encontrar, tenta o NameIdentifier
            if (string.IsNullOrEmpty(email))
            {
                email = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return email;
        }
    }
}
