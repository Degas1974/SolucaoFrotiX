/*
 ╔══════════════════════════════════════════════════════════════════════════╗
 ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO                                            ║
 ║  Arquivo: ResetPasswordConfirmation.cshtml.cs                            ║
 ║  Caminho: /Areas/Identity/Pages/Account/ResetPasswordConfirmation.cshtml.cs║
 ║  Documentado em: 2026-01-26                                              ║
 ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
        {
        private readonly ILogger<ResetPasswordConfirmationModel> _logger;

        public ResetPasswordConfirmationModel(ILogger<ResetPasswordConfirmationModel> logger)
            {
            _logger = logger;
            }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnGet
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Exibir página de confirmação de reset de senha
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : void - Apenas exibe a página
         * 🔗 CHAMADA POR  : Framework ASP.NET Core após redirecionamento do ResetPassword
         * 🔄 CHAMA        : Nenhuma função
         * 📦 DEPENDÊNCIAS : ASP.NET Core Razor Pages
         * --------------------------------------------------------------------------------------
         * [DOC] Página de confirmação estática sem lógica adicional
         ****************************************************************************************/
        public void OnGet()
            {
            try
                {
                // [DOC] Página estática de confirmação - sem lógica necessária
                _logger.LogInformation("Página de confirmação de reset de senha acessada");
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, "Erro ao carregar confirmação de reset");
                }
            }
        }
    }


