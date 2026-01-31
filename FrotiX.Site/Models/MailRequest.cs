/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: MailRequest.cs                                                                          ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Modelo para requisição de envio de email no sistema.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MailRequest                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Nenhuma                                                                            ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
    {
    // ==================================================================================================
    // DTO
    // ==================================================================================================
    // Dados necessários para envio de email.
    // ==================================================================================================
    public class MailRequest
        {
        // Email de destino.
        public string ToEmail { get; set; }

        // Assunto do email.
        public string Subject { get; set; }

        // Corpo do email.
        public string Body { get; set; }
        }
    }

