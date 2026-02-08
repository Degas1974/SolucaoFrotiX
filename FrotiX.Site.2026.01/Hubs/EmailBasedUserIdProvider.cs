/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Data de Atualização: 23/01/2026
 * Tecnologias: .NET 10 (Preview), C#, SignalR
 * 
 * Descrição do Arquivo:
 * Provider de ID de usuário baseado em Email para SignalR.
 * =========================================================================================
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
