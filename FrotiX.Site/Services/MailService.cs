// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MailService.cs                                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Implementação do serviço de envio de e-mails via SMTP.                       ║
// ║ Usa MailKit para conexão segura com servidores de e-mail.                    ║
// ║                                                                              ║
// ║ FUNCIONALIDADES:                                                             ║
// ║ - Envio de e-mail HTML assíncrono                                            ║
// ║ - Conexão TLS quando disponível (StartTlsWhenAvailable)                      ║
// ║ - Configuração via IOptions<MailSettings>                                    ║
// ║                                                                              ║
// ║ DEPENDÊNCIAS:                                                                ║
// ║ - MailKit: Biblioteca de e-mail cross-platform                               ║
// ║ - MimeKit: Criação de mensagens MIME                                         ║
// ║                                                                              ║
// ║ USO TÍPICO:                                                                  ║
// ║ - Envio de código de verificação de login                                    ║
// ║ - Recuperação de senha                                                       ║
// ║ - Notificações do sistema                                                    ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 15                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
    /// <summary>
    /// Implementação do serviço de envio de e-mails via SMTP/MailKit.
    /// </summary>
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

        public async Task SendEmailAsync(MailRequest mailRequest)
            {
            var email = new MimeMessage();


            //var nome = _settings.DisplayName;
            var nome = "FrotiX - Autenticação";
            var fromemail = _settings.Mail;

            email.Sender = MailboxAddress.Parse(_settings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            //MailboxAddress emailFrom = new MailboxAddress(_settings.DisplayName, _settings.Mail);
            MailboxAddress emailFrom = new MailboxAddress(nome, _settings.Mail);
            email.From.Add(emailFrom);

            email.Body = new TextPart("html") { Text = mailRequest.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_settings.Mail, _settings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            }
        }
    }


