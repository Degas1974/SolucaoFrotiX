/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  HUBS SIGNALR - PROVEDOR DE USER ID                                                 #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FrotiX.Hubs
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: EmailBasedUserIdProvider                                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Provedor customizado de User ID para SignalR. Usa EMAIL do usuário como  ║
    /// ║    identificador único ao invés do NameIdentifier padrão.                    ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA:                                                              ║
    /// ║    Permite enviar notificações SignalR direcionadas a usuário específico    ║
    /// ║    usando o email como chave. Essencial para alertas personalizados.        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔍 LÓGICA DE FALLBACK:                                                       ║
    /// ║    1º Tenta: ClaimTypes.Email                                                ║
    /// ║    2º Tenta: ClaimTypes.Name                                                 ║
    /// ║    3º Tenta: ClaimTypes.NameIdentifier                                       ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INFRAESTRUTURA SignalR - Registrado no Startup.cs                 ║
    /// ║    • Uso: services.AddSignalR().AddUserIdProvider<EmailBasedUserIdProvider>()║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class EmailBasedUserIdProvider : IUserIdProvider
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetUserId                                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Extrai o User ID (email) das Claims do usuário autenticado.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETRO:                                                                ║
        /// ║    • connection: Contexto da conexão SignalR                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • string: Email do usuário ou null se não encontrar                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public string GetUserId(HubConnectionContext connection)
        {
            // [DADOS] 1ª Tentativa: Claim de Email
            var email = connection.User?.FindFirst(ClaimTypes.Email)?.Value;

            // [REGRA] Fallback 1: Se não encontrar email, tenta Name
            if (string.IsNullOrEmpty(email))
            {
                email = connection.User?.FindFirst(ClaimTypes.Name)?.Value;
            }

            // [REGRA] Fallback 2: Se ainda não encontrar, tenta NameIdentifier (GUID)
            if (string.IsNullOrEmpty(email))
            {
                email = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return email;
        }
    }
}
