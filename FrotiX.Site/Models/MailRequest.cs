// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MailRequest.cs                                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para requisição de envio de email.                                   ║
// ║ Usado pelo MailService para estruturar mensagens.                           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - ToEmail: Endereço de email do destinatário                                ║
// ║ - Subject: Assunto do email                                                 ║
// ║ - Body: Corpo do email (pode ser HTML)                                      ║
// ║                                                                              ║
// ║ USO: MailService.SendEmailAsync(MailRequest request)                        ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
    {
    public class MailRequest
        {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        }
    }


