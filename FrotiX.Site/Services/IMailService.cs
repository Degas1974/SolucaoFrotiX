/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Services/IMailService.cs                                       ║
 * ║  Descrição: Interface para serviço de envio de e-mails.                   ║
 * ║             SendEmailAsync recebe MailRequest com destinatário, assunto, ║
 * ║             corpo e anexos. Implementado por MailService.                ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FrotiX.Models;
namespace FrotiX.Services
    {
    public interface IMailService
        {
        Task SendEmailAsync(MailRequest mailRequest);
        }
    }


