// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MailSettings.cs                                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Configurações de envio de e-mail (SMTP).                                     ║
// ║ Mapeado da seção "MailSettings" do appsettings.json.                         ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - Mail: Endereço de e-mail remetente                                         ║
// ║ - DisplayName: Nome de exibição do remetente                                 ║
// ║ - Password: Senha ou app password do e-mail                                  ║
// ║ - Host: Servidor SMTP (ex: smtp.gmail.com)                                   ║
// ║ - Port: Porta SMTP (ex: 587 para TLS)                                        ║
// ║                                                                              ║
// ║ REGISTRO EM Program.cs:                                                      ║
// ║ services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));  ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 13                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Settings
    {
    /// <summary>
    /// Configurações de envio de e-mail SMTP.
    /// </summary>
    public class MailSettings
        {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        }
    }


