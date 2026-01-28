// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ReCaptchaSettings.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Configurações do Google reCAPTCHA para proteção de formulários.              ║
// ║ Mapeado da seção "ReCaptcha" do appsettings.json.                            ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - Key: Chave pública do site (usada no frontend)                             ║
// ║ - Secret: Chave secreta (usada na validação backend)                         ║
// ║                                                                              ║
// ║ USO:                                                                         ║
// ║ - Formulários de login, registro, recuperação de senha                       ║
// ║ - Proteção contra bots e ataques de força bruta                              ║
// ║                                                                              ║
// ║ REGISTRO EM Program.cs:                                                      ║
// ║ services.Configure<ReCaptchaSettings>(Configuration.GetSection("ReCaptcha"));║
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
    /// Configurações do Google reCAPTCHA.
    /// </summary>
    public class ReCaptchaSettings
        {
        public string Key { get; set; }
        public string Secret { get; set; }
        }
    }


