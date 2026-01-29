/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/MailRequest.cs                                          ║
 * ║  Descrição: Modelo para requisição de envio de email                     ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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


