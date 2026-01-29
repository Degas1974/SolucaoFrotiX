/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/ErrorViewModel.cs                                       ║
 * ║  Descrição: ViewModel para exibição de erros e serviço de envio de email ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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


