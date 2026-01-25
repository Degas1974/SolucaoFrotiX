/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** Lockout.cshtml.cs
 > 📍 **Local:** Areas/Identity/Pages/Account
 > ❓ **Por que existo?** Gerencia a tela de bloqueio de conta e tentativa de desbloqueio.
 > 🔗 **Relevância:** Alta (Segurança)
 > ---------------------------------------------------------------------------------------
 */

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FrotiX.Helpers;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * **Injeção de Dependência:** `_signInManager`, `_logger`
        * **Identity:** `SignOutAsync`, `PasswordSignInAsync`

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Rotas de API:** GET/POST /Identity/Account/Lockout
        */
        private readonly ILogger<LogoutModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LockoutModel                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o PageModel da tela de bloqueio de conta.                      ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Prepara as dependências para o fluxo de segurança do lockout.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • signInManager (SignInManager<IdentityUser>): gerenciador de logins.     ║
        /// ║    • logger (ILogger<LogoutModel>): logger da página.                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara dependências do fluxo.                             ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependências).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Lockout.cshtml                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public LockoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGetAsync                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe a tela de bloqueio e força logout para segurança.                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Evita sessão ativa durante bloqueio e padroniza o fluxo de lockout.       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task: operação assíncrona sem retorno explícito.                        ║
        /// ║    • Significado: finaliza o GET com sessão encerrada.                       ║
        /// ║    • Consumidor: pipeline Razor Pages.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _signInManager.SignOutAsync() → encerra sessão.                         ║
        /// ║    • _logger.LogInformation() → registra evento.                             ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/Lockout).                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Lockout.cshtml                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task OnGetAsync()
        {
            // [REGRA] Forçar logout ao entrar na tela de bloqueio
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out.");
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnPostAsync                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Tenta autenticar para desbloqueio e redireciona conforme resultado.       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Controla o fluxo de retorno do usuário após lockout.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno após login.                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: redirecionamento ou reexibição da página.                ║
        /// ║    • Significado: define sucesso, 2FA, lockout ou erro.                      ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _signInManager.PasswordSignInAsync() → autentica usuário.               ║
        /// ║    • _logger.LogInformation()/LogWarning() → registra eventos.               ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (POST /Identity/Account/Lockout).                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Lockout.cshtml                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // [DADOS] Recuperar e-mail da ViewData (se disponível)
                var userName = ViewData["Email"]?.ToString();

                // [LOGICA] Tentativa de login
                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, true, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
                }
                if (result.IsLockedOut)
                {
                    // [REGRA] Se continuar bloqueado, manter na página
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}


