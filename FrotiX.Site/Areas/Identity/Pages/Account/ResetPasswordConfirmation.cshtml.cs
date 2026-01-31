/* ****************************************************************************************
 * ⚡ ARQUIVO: ResetPasswordConfirmation.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel da confirmação de reset de senha (página estática).
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : Renderização da página ResetPasswordConfirmation.cshtml.
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ResetPasswordConfirmation).
 *
 * 🔄 CHAMA        : Nenhum método interno.
 *
 * 📦 DEPENDÊNCIAS : Razor Pages, ILogger.
 *
 * 📝 OBSERVAÇÕES  : Utiliza logger apenas para auditoria do acesso.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: ResetPasswordConfirmationModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Exibir página de confirmação após reset de senha.
 *
 * 📥 ENTRADAS     : Nenhuma
 *
 * 📤 SAÍDAS       : Renderização da página
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ResetPasswordConfirmation)
 *
 * 🔄 CHAMA        : Nenhum
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Razor Pages, ILogger
 ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ CONSTRUTOR: ResetPasswordConfirmationModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar logger para auditoria.
         *
         * 📥 ENTRADAS     : [ILogger<ResetPasswordConfirmationModel>] logger - Logger para auditoria
         *
         * 📤 SAÍDAS       : Instância configurada de ResetPasswordConfirmationModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ILogger
         ****************************************************************************************/
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


