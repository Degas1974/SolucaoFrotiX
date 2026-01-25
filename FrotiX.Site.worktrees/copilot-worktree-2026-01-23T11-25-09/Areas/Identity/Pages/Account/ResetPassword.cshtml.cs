using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        /* > ---------------------------------------------------------------------------------------
         > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
         > ---------------------------------------------------------------------------------------
         > 🆔 **Nome:** ResetPassword.cshtml.cs
         > 📍 **Local:** Areas/Identity/Pages/Account
         > ❓ **Por que existo?** Gerencia o processo de redefinição de senha do usuário.
         > 🔗 **Relevância:** Alta (Recuperação de Acesso)
         > --------------------------------------------------------------------------------------- */

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ResetPasswordModel                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa dependências para o fluxo de redefinição de senha.             ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita operações do Identity para reset seguro de senha.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • userManager (UserManager<IdentityUser>): gerenciador de usuários.       ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara o fluxo de reset.                                  ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependência).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ResetPassword.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe o formulário de redefinição, exigindo token válido.                 ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Protege o fluxo garantindo que o reset só ocorra com token.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • code (string): token de redefinição recebido por e-mail.                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: página de reset ou erro de requisição.                   ║
        /// ║    • Significado: habilita o formulário com token.                           ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma externa (apenas validação de token).                            ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/ResetPassword).            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ResetPassword.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public IActionResult OnGet(string code = null)
        {
            try
            {
                if (code == null)
                {
                    return BadRequest("A code must be supplied for password reset.");
                }
                else
                {
                    Input = new InputModel
                    {
                        Code = code
                    };
                    return Page();
                }
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("ResetPassword.cshtml.cs", "OnGet", error);
                return BadRequest("Erro ao processar solicitação.");
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnPostAsync                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa a redefinição de senha, validando token e nova senha.            ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Conclui o reset mantendo sigilo sobre existência do usuário.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum (usa Input via Model Binding).                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: redireciona para confirmação ou retorna a página.        ║
        /// ║    • Significado: confirma sucesso ou exibe validações.                      ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _userManager.FindByEmailAsync() → busca usuário.                        ║
        /// ║    • _userManager.ResetPasswordAsync() → redefine senha.                     ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (POST /Identity/Account/ResetPassword).           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ResetPassword.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // [DADOS] Busca usuário
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    // [REGRA] Não revelar existência do usuário
                    // > 🧠 **Lógica:** Redireciona para confirmação mesmo se usuário não existir, por segurança (Enumeration Attack)
                    return RedirectToPage("./ResetPasswordConfirmation");
                }

                // [LOGICA] Resetar senha
                var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
                if (result.Succeeded)
                {
                    return RedirectToPage("./ResetPasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("ResetPassword.cshtml.cs", "OnPostAsync", error);
                return Page();
            }
        }
    }
}


