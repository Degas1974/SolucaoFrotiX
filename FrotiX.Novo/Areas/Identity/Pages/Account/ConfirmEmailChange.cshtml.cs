/* ****************************************************************************************
 * ⚡ ARQUIVO: ConfirmEmailChange.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para confirmar alteração de email, validando token e
 *                   atualizando email/username do usuário no Identity.
 *
 * 📥 ENTRADAS     : userId (string), email (string), code (string) via query string.
 *
 * 📤 SAÍDAS       : StatusMessage e IActionResult (Page/Redirect/NotFound).
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ConfirmEmailChange).
 *
 * 🔄 CHAMA        : UserManager.ChangeEmailAsync(), SetUserNameAsync(),
 *                   SignInManager.RefreshSignInAsync().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, WebUtilities, Razor Pages.
 *
 * 📝 OBSERVAÇÕES  : Sistema usa email como username; ambos são atualizados.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: ConfirmEmailChangeModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para confirmação de alteração de email. Processa token
 *                   de confirmação, altera email do usuário e atualiza username
 *                   (email = username no sistema).
 *
 * 📥 ENTRADAS     : userId (string) - ID do usuário
 *                   email (string) - Novo email a ser confirmado
 *                   code (string) - Token de confirmação codificado em Base64Url
 *
 * 📤 SAÍDAS       : StatusMessage - Mensagem de sucesso ou erro
 *                   IActionResult - Page() ou RedirectToPage() ou NotFound()
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ConfirmEmailChange)
 *
 * 🔄 CHAMA        : UserManager.FindByIdAsync(), UserManager.ChangeEmailAsync(),
 *                   UserManager.SetUserNameAsync(), SignInManager.RefreshSignInAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, Microsoft.AspNetCore.WebUtilities
 *
 * 📝 OBSERVAÇÕES  : Após alterar email, também altera username (email=username).
 *                   Atualiza sessão com RefreshSignInAsync().
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
        {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: ConfirmEmailChangeModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa UserManager e SignInManager via injeção de dependência.
         *
         * 📥 ENTRADAS     : [UserManager<IdentityUser>] userManager - Gerenciador de usuários
         *                   [SignInManager<IdentityUser>] signInManager - Gerenciador de autenticação
         *
         * 📤 SAÍDAS       : Instância configurada de ConfirmEmailChangeModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity
         ****************************************************************************************/
        public ConfirmEmailChangeModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
            {
            _userManager = userManager;
            _signInManager = signInManager;
            }

        [TempData]
        public string StatusMessage { get; set; }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGetAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar confirmação de alteração de email via token. Valida
         *                   parâmetros, altera email, atualiza username e atualiza sessão.
         *
         * 📥 ENTRADAS     : [string] userId - ID do usuário
         *                   [string] email - Novo email a ser confirmado
         *                   [string] code - Token de confirmação codificado em Base64Url
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - Page() ou RedirectToPage() ou NotFound()
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ConfirmEmailChange?userId=X&email=Y&code=Z)
         *
         * 🔄 CHAMA        : _userManager.FindByIdAsync(), _userManager.ChangeEmailAsync(),
         *                   _userManager.SetUserNameAsync(), _signInManager.RefreshSignInAsync()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, WebUtilities
         *
         * 📝 OBSERVAÇÕES  : Sistema usa email como username, por isso ambos são atualizados.
         *                   RefreshSignInAsync() garante que sessão reflete novo email.
         ****************************************************************************************/
        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
            {
            try
            {
                // [DOC] Valida presença de todos os parâmetros obrigatórios
                if (userId == null || email == null || code == null)
                    {
                    return RedirectToPage("/Index");
                    }

                // [DOC] Busca usuário por ID
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    {
                    return NotFound($"Unable to load user with ID '{userId}'.");
                    }

                // [DOC] Decodifica token de Base64Url para string UTF-8
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

                // [DOC] Altera email do usuário usando token decodificado
                var result = await _userManager.ChangeEmailAsync(user, email, code);
                if (!result.Succeeded)
                    {
                    StatusMessage = "Error changing email.";
                    return Page();
                    }

                // [DOC] Como email = username no sistema, atualiza username também
                var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
                if (!setUserNameResult.Succeeded)
                    {
                    StatusMessage = "Error changing user name.";
                    return Page();
                    }

                // [DOC] Atualiza sessão do usuário com novo email/username
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Thank you for confirming your email change.";
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error confirming email change: {ex.Message}";
                return Page();
            }
            }
        }
    }

