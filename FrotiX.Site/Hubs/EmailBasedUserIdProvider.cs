/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Hubs/EmailBasedUserIdProvider.cs                               ║
 * ║  Descrição: Implementação de IUserIdProvider para SignalR.               ║
 * ║             Extrai ID do usuário via Claims na ordem: Email, Name,        ║
 * ║             NameIdentifier. Permite envio direcionado de mensagens.      ║
 * ║  Data: 28/01/2026 | LOTE: 21                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
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
