/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: ErrorViewModel.cs                                                                       ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: ViewModel de erro e stub de envio de email para Identity UI.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: ErrorViewModel, EmailSender                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Identity.UI.Services                                                                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FrotiX.Models
    {
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: expor identificador de requisição em telas de erro.
    // ==================================================================================================
    public class ErrorViewModel
        {
        // Identificador da requisição.
        public string RequestId { get; set; }

        // Indica se o RequestId deve ser exibido.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }

    // ==================================================================================================
    // SERVIÇO STUB
    // ==================================================================================================
    // Implementação mínima de envio de email para Identity UI.
    // ==================================================================================================
    public class EmailSender : IEmailSender
        {
        // ⚠️ ATENÇÃO: stub sem implementação de provedor de email.
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
            throw new NotImplementedException("No email provider is implemented by default, please Google on how to add one, like SendGrid.");
            }
        }
    }

