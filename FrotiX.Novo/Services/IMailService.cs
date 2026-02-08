/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IMailService.cs                                                                         ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interface de envio de e-mails. Implementado por MailService.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: SendEmailAsync(MailRequest) -> Task                                                      ║
   ║ 🔗 DEPS: FrotiX.Models.MailRequest | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
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


