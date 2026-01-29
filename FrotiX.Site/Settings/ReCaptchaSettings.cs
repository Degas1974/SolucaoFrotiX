/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Settings/ReCaptchaSettings.cs                                  ║
 * ║  Descrição: DTO de configuração para Google reCAPTCHA. Propriedades:     ║
 * ║             Key (chave pública do site) e Secret (chave secreta).        ║
 * ║             Vinculado a appsettings.json seção "ReCaptchaSettings".      ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
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


