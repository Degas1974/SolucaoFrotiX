// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ErrorViewModel.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelos para tratamento de erros e envio de email.                          ║
// ║                                                                              ║
// ║ CLASSES:                                                                     ║
// ║ - ErrorViewModel: Modelo para página de erro padrão MVC                     ║
// ║   → RequestId: ID da requisição para rastreamento                           ║
// ║   → ShowRequestId: Computed property para exibição                          ║
// ║                                                                              ║
// ║ - EmailSender: Implementação placeholder de IEmailSender                    ║
// ║   → Lança NotImplementedException (usar MailService em vez disso)           ║
// ║                                                                              ║
// ║ NOTA: EmailSender é legado. O sistema usa MailService para emails.          ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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


