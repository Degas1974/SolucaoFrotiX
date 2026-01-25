/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** ForgotPassword.cshtml.cs
 > 📍 **Local:** Areas/Identity/Pages/Account
 > ❓ **Por que existo?** Processa o pedido de recuperação de senha.
 > 🔗 **Relevância:** Alta (Segurança)
 > ---------------------------------------------------------------------------------------
 */

using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FrotiX.Helpers;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * **Injeção de Dependência:** `_signInManager`, `_userManager`, `_emailSender`
        * **Ações:** `SignOutAsync`, `FindByEmailAsync`, `GeneratePasswordResetTokenAsync`, `SendEmailAsync`

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Rotas de API:** GET/POST /Identity/Account/ForgotPassword
        */
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ForgotPasswordModel                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o PageModel de recuperação de senha.                           ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita o fluxo seguro de reset de senha via Identity.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • signInManager (SignInManager<IdentityUser>): gerenciador de logins.     ║
        /// ║    • logger (ILogger<LogoutModel>): logger da página.                        ║
        /// ║    • userManager (UserManager<IdentityUser>): gerenciador de usuários.       ║
        /// ║    • emailSender (IEmailSender): serviço de envio de e-mails.                ║
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
        /// ║    • Arquivos relacionados: ForgotPassword.cshtml                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public ForgotPasswordModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa o GET e garante que o usuário esteja deslogado.                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Evita sessão ativa durante o fluxo de reset de senha.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task: operação assíncrona sem retorno explícito.                        ║
        /// ║    • Significado: finaliza a execução do GET.                                ║
        /// ║    • Consumidor: pipeline Razor Pages.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _signInManager.SignOutAsync() → encerra sessão.                         ║
        /// ║    • _logger.LogInformation() → registra evento.                             ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/ForgotPassword).          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ForgotPassword.cshtml                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task OnGet()
        {
            // [REGRA] Deslogar antes de iniciar recuperação de senha
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out.");
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnPostAsync                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa o POST e envia e-mail de recuperação de senha.                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Dispara o fluxo seguro de reset via token do Identity.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum (usa Input via Model Binding).                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: redireciona para confirmação ou retorna a página.        ║
        /// ║    • Significado: garante sigilo sobre existência do usuário.                ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _userManager.FindByEmailAsync() → busca usuário.                        ║
        /// ║    • _userManager.IsEmailConfirmedAsync() → valida e-mail.                   ║
        /// ║    • _userManager.GeneratePasswordResetTokenAsync() → gera token.           ║
        /// ║    • _emailSender.SendEmailAsync() → envia e-mail.                           ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (POST /Identity/Account/ForgotPassword).         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ForgotPassword.cshtml                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // [DADOS] Buscar usuário e verificar confirmação de e-mail
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // [REGRA] Não revelar se usuário existe ou não (Security)
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // [LOGICA] Gerar token e URL de reset
                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                // [AJAX] Enviar e-mail
                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}


