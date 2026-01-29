/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ErrorViewModel.cs                                                                       ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: ViewModel para exibição de erros e serviço stub de envio de email.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: ErrorViewModel (RequestId, ShowRequestId), EmailSender (IEmailSender stub)              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: Microsoft.AspNetCore.Identity.UI.Services                                                  ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FrotiX.Models
    {
    public class ErrorViewModel
        {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }

    public class EmailSender : IEmailSender
        {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
            throw new NotImplementedException("No email provider is implemented by default, please Google on how to add one, like SendGrid.");
            }
        }
    }


