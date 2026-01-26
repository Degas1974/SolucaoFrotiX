/****************************************************************************************
 * ⚡ CLASSE: ConfirmEmailModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para confirmação de email após registro. Processa token
 *                   de confirmação enviado por email e valida email do usuário no sistema.
 *
 * 📥 ENTRADAS     : userId (string) - ID do usuário a confirmar
 *                   code (string) - Token de confirmação codificado em Base64Url
 *
 * 📤 SAÍDAS       : StatusMessage - Mensagem de sucesso ou erro
 *                   IActionResult - Page() ou RedirectToPage() ou NotFound()
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ConfirmEmail)
 *
 * 🔄 CHAMA        : UserManager.FindByIdAsync(), UserManager.ConfirmEmailAsync(),
 *                   WebEncoders.Base64UrlDecode()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, Microsoft.AspNetCore.WebUtilities
 *
 * 📝 OBSERVAÇÕES  : Aceita acesso anônimo ([AllowAnonymous]). Token é enviado
 *                   codificado em Base64Url e decodificado antes da validação.
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

namespace FrotiX.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
        {
        private readonly UserManager<IdentityUser> _userManager;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: ConfirmEmailModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa UserManager via injeção de dependência.
         *
         * 📥 ENTRADAS     : [UserManager<IdentityUser>] userManager - Gerenciador de usuários
         *
         * 📤 SAÍDAS       : Instância configurada de ConfirmEmailModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity
         ****************************************************************************************/
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
            {
            _userManager = userManager;
            }

        [TempData]
        public string StatusMessage { get; set; }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGetAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar confirmação de email via token. Valida userId e code,
         *                   decodifica token e confirma email do usuário no sistema Identity.
         *
         * 📥 ENTRADAS     : [string] userId - ID do usuário a confirmar
         *                   [string] code - Token de confirmação codificado em Base64Url
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - Page() se sucesso, RedirectToPage() se
         *                   parâmetros inválidos, NotFound() se usuário não existe
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ConfirmEmail?userId=X&code=Y)
         *
         * 🔄 CHAMA        : _userManager.FindByIdAsync(), _userManager.ConfirmEmailAsync(),
         *                   WebEncoders.Base64UrlDecode(), Encoding.UTF8.GetString()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, WebUtilities
         *
         * 📝 OBSERVAÇÕES  : Token é enviado codificado em Base64Url para segurança em URL.
         *                   Mensagens em inglês conforme padrão Identity.
         ****************************************************************************************/
        public async Task<IActionResult> OnGetAsync(string userId, string code)
            {
            try
            {
                // [DOC] Valida presença de parâmetros obrigatórios (userId e code)
                if (userId == null || code == null)
                    {
                    return RedirectToPage("/Index");
                    }

                // [DOC] Busca usuário por ID no sistema Identity
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    {
                    return NotFound($"Unable to load user with ID '{userId}'.");
                    }

                // [DOC] Decodifica token de Base64Url para string UTF-8
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

                // [DOC] Confirma email do usuário usando token decodificado
                var result = await _userManager.ConfirmEmailAsync(user, code);

                // [DOC] Define mensagem de status baseado no resultado (sucesso/erro)
                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error confirming your email: {ex.Message}";
                return Page();
            }
            }
        }
    }


