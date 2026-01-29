/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EmailBasedUserIdProvider.cs                                                            ║
   ║ 📂 CAMINHO: /Hubs                                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: IUserIdProvider para SignalR. Extrai ID via Claims: Email > Name > NameIdentifier.    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: 1.[GetUserId] : Extrai ID do usuário de HubConnectionContext                            ║
   ║ 🔗 DEPS: Microsoft.AspNetCore.SignalR, System.Security.Claims | 📅 29/01/2026 | 👤 Copilot | v2.0  ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FrotiX.Hubs
{
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
