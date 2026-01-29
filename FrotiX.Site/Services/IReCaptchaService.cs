/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IReCaptchaService.cs                                                                    ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interface para validação Google reCAPTCHA. Implementado por ReCaptchaService.          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Configs (ReCaptchaSettings), ValidateReCaptcha(token) -> bool                            ║
   ║ 🔗 DEPS: ReCaptchaSettings | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                   ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FrotiX.Settings;
namespace FrotiX.Services
    {
    public interface IReCaptchaService
        {
        ReCaptchaSettings Configs { get; }
        bool ValidateReCaptcha(string token);
        }
    }


