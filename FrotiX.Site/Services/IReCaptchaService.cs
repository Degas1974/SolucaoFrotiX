// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: IReCaptchaService.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Interface para serviço de validação Google reCAPTCHA.                        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - Configs: Acesso às configurações ReCaptchaSettings                         ║
// ║                                                                              ║
// ║ MÉTODOS:                                                                     ║
// ║ - ValidateReCaptcha(): Valida token do frontend com API Google               ║
// ║                                                                              ║
// ║ IMPLEMENTAÇÃO: ReCaptchaService                                              ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 14                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FrotiX.Settings;
namespace FrotiX.Services
    {
    /// <summary>
    /// Interface para validação Google reCAPTCHA.
    /// </summary>
    public interface IReCaptchaService
        {
        ReCaptchaSettings Configs { get; }
        bool ValidateReCaptcha(string token);
        }
    }


