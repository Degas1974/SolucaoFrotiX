/****************************************************************************************
 * ⚡ CLASSE: ForgotPasswordConfirmation (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para página de confirmação de recuperação de senha.
 *                   Classe vazia sem lógica - apenas exibe página estática.
 *
 * 📥 ENTRADAS     : Nenhuma
 *
 * 📤 SAÍDAS       : Nenhuma
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ForgotPasswordConfirmation)
 *
 * 🔄 CHAMA        : Nenhum
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Razor Pages
 *
 * 📝 OBSERVAÇÕES  : PageModel mínimo sem lógica. Toda apresentação está no .cshtml.
 ****************************************************************************************/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
        {
        /****************************************************************************************
         * ⚡ MÉTODO: OnGet
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Handler para GET da página de confirmação. Método vazio sem lógica.
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : void
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ForgotPasswordConfirmation)
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : Nenhuma
         ****************************************************************************************/
        public void OnGet()
            {
            // [DOC] Método vazio - página apenas exibe conteúdo estático
            }
        }
    }


