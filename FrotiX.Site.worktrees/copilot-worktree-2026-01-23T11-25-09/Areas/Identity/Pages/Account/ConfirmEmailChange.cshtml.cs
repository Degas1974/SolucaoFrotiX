/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** ConfirmEmailChange.cshtml.cs
 > 📍 **Local:** Areas/Identity/Pages/Account
 > ❓ **Por que existo?** Processa a confirmação da troca de e-mail do usuário.
 > 🔗 **Relevância:** Alta (Segurança/Cadastro)
 > ---------------------------------------------------------------------------------------
 */

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
using FrotiX.Helpers;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * **Injeção de Dependência:** `_userManager`, `_signInManager`
        * **Identity:** `ChangeEmailAsync`, `SetUserNameAsync`, `RefreshSignInAsync`

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Rotas de API:** GET /Identity/Account/ConfirmEmailChange
        */
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ConfirmEmailChangeModel                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o PageModel para confirmação de troca de e-mail.               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita operações do Identity para validar e concluir a alteração de     ║
        /// ║    e-mail com segurança.                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • userManager (UserManager<IdentityUser>): gerenciador de usuários.       ║
        /// ║    • signInManager (SignInManager<IdentityUser>): gerenciador de login.      ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara dependências do Identity.                          ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependências).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ConfirmEmailChange.cshtml                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public ConfirmEmailChangeModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGetAsync                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa a confirmação da troca de e-mail e sincroniza o username.        ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Mantém a integridade da identidade do usuário após mudança de e-mail.     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • userId (string): identificador do usuário.                              ║
        /// ║    • email (string): novo e-mail confirmado.                                 ║
        /// ║    • code (string): token de validação gerado pelo Identity.                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: página de status ou redirecionamento.                    ║
        /// ║    • Significado: indica sucesso/erro na alteração de e-mail.                ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _userManager.FindByIdAsync() → busca usuário.                           ║
        /// ║    • _userManager.ChangeEmailAsync() → altera e-mail.                         ║
        /// ║    • _userManager.SetUserNameAsync() → sincroniza username.                  ║
        /// ║    • _signInManager.RefreshSignInAsync() → atualiza sessão.                  ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/ConfirmEmailChange).      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ConfirmEmailChange.cshtml                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            // [REGRA] Validar parâmetros obrigatórios
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            // [DADOS] Buscar usuário
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            // [LOGICA] Alterar e-mail
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Error changing email.";
                return Page();
            }

            // [DADOS] Atualizar UserName (pois é o mesmo que o email)
            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Error changing user name.";
                return Page();
            }

            // [UI] Refresh no login e mensagem de sucesso
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
        }
    }
}


