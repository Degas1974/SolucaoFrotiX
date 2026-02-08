/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MailSettings.cs                                                                         ║
   ║ 📂 CAMINHO: /Settings                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTO de configuração SMTP. Vinculado a appsettings.json seção "MailSettings".           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Mail, DisplayName, Password, Host, Port                                                  ║
   ║ 🔗 DEPS: Nenhuma (POCO) | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                      ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
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


