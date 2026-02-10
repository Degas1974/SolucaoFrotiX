/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ReCaptchaSettings.cs                                                                    ║
   ║ 📂 CAMINHO: /Settings                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTO de configuração Google reCAPTCHA. Vinculado a appsettings.json.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Key (pública), Secret (secreta)                                                          ║
   ║ 🔗 DEPS: Nenhuma (POCO) | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                      ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Settings
    {
    public class ReCaptchaSettings
        {
        public string Key { get; set; }
        public string Secret { get; set; }
        }
    }


