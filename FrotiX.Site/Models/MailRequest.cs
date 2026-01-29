/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MailRequest.cs                                                                          ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Modelo para requisição de envio de email no sistema.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: ToEmail, Subject, Body                                                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: Nenhuma | 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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


