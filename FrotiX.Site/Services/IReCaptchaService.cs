/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Services/IReCaptchaService.cs                                  ║
 * ║  Descrição: Interface para validação de Google reCAPTCHA.                 ║
 * ║             Configs: Acessa ReCaptchaSettings (Key, Secret).             ║
 * ║             ValidateReCaptcha: Valida token recebido do frontend.        ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
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


