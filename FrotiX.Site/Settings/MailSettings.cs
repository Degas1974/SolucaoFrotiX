/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Settings/MailSettings.cs                                       ║
 * ║  Descrição: DTO de configuração para envio de e-mails via SMTP.          ║
 * ║             Propriedades: Mail, DisplayName, Password, Host, Port.       ║
 * ║             Vinculado a appsettings.json seção "MailSettings".           ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Settings
    {
    public class MailSettings
        {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        }
    }


