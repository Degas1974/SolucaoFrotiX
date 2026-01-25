/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - ENVIO DE E-MAIL                                                         #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using FrotiX.Models;
using FrotiX.Settings;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace FrotiX.Services
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: MailService                                                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Serviço de envio de e-mails usando MailKit/MimeKit via SMTP.              ║
    /// ║    Utilizado para autenticação 2FA, notificações e alertas do sistema.       ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Sistema crítico de comunicação. Envia códigos de autenticação, alertas   ║
    /// ║    de vencimento de CNH, notificações de manutenção e outros e-mails.        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • SendEmailAsync() → Envia e-mail via SMTP                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INTERNA - Serviço de infraestrutura                               ║
    /// ║    • Arquivos relacionados: appsettings.json (MailSettings), IMailService   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class MailService : IMailService
    {
        public class EmailSettings
        {
            public string EmailId { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public bool UseSSL { get; set; }
        }

        private readonly MailSettings _settings;
        
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _settings = mailSettings.Value;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SendEmailAsync                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Envia e-mail HTML via SMTP com autenticação TLS.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • mailRequest: Objeto com ToEmail, Subject, Body                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task: Tarefa assíncrona de envio                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            // [DADOS] Criação da mensagem MIME
            var email = new MimeMessage();

            // [REGRA] Configuração do remetente (FrotiX - Autenticação)
            var nome = "FrotiX - Autenticação";
            var fromemail = _settings.Mail;

            email.Sender = MailboxAddress.Parse(_settings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            MailboxAddress emailFrom = new MailboxAddress(nome, _settings.Mail);
            email.From.Add(emailFrom);

            // [DADOS] Corpo do e-mail em HTML
            email.Body = new TextPart("html") { Text = mailRequest.Body };

            // [AJAX] Envio via SMTP com TLS
            using var smtp = new SmtpClient();
            smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_settings.Mail, _settings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}


