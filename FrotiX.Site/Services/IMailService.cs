// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: IMailService.cs                                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Interface para serviço de envio de e-mails.                                  ║
// ║                                                                              ║
// ║ MÉTODOS:                                                                     ║
// ║ - SendEmailAsync(): Envia e-mail assíncrono via MailRequest                  ║
// ║                                                                              ║
// ║ DEPENDÊNCIAS:                                                                ║
// ║ - MailRequest: Model com destinatário, assunto, corpo e anexos               ║
// ║ - MailSettings: Configurações SMTP                                           ║
// ║                                                                              ║
// ║ IMPLEMENTAÇÃO: MailService                                                   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 14                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FrotiX.Models;
namespace FrotiX.Services
    {
    /// <summary>
    /// Interface para serviço de envio de e-mails.
    /// </summary>
    public interface IMailService
        {
        Task SendEmailAsync(MailRequest mailRequest);
        }
    }


