/****************************************************************************************
 * ⚡ CLASSE: ForgotPasswordModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para recuperação de senha. Gera token de reset, envia
 *                   email com link de recuperação e redireciona para página de confirmação.
 *
 * 📥 ENTRADAS     : Input.Email (string) - Email do usuário para recuperação
 *
 * 📤 SAÍDAS       : IActionResult - RedirectToPage ou Page()
 *                   Email com link de reset (via IEmailSender)
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/ForgotPassword)
 *
 * 🔄 CHAMA        : UserManager.FindByEmailAsync(), UserManager.IsEmailConfirmedAsync(),
 *                   UserManager.GeneratePasswordResetTokenAsync(), IEmailSender.SendEmailAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, ILogger
 *
 * 📝 OBSERVAÇÕES  : Por segurança, não revela se usuário existe ou email não confirmado.
 *                   OnGet executa SignOut (comportamento questionável - ver linha 39-44).
 ****************************************************************************************/
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
        {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: ForgotPasswordModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa dependências via injeção (SignInManager, UserManager,
         *                   EmailSender e Logger).
         *
         * 📥 ENTRADAS     : [SignInManager<IdentityUser>] signInManager - Gerenciador de autenticação
         *                   [ILogger<LogoutModel>] logger - Logger para auditoria
         *                   [UserManager<IdentityUser>] userManager - Gerenciador de usuários
         *                   [IEmailSender] emailSender - Serviço de envio de email
         *
         * 📤 SAÍDAS       : Instância configurada de ForgotPasswordModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, ILogger
         ****************************************************************************************/
        public ForgotPasswordModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, UserManager<IdentityUser> userManager, IEmailSender emailSender)
            {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            }

        [BindProperty]
        public InputModel Input { get; set; }

        /****************************************************************************************
         * ⚡ CLASSE: InputModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Model de entrada para formulário de recuperação de senha.
         *
         * 📥 ENTRADAS     : Email (string) - Email do usuário para recuperação
         *
         * 📤 SAÍDAS       : Objeto validado via Data Annotations
         *
         * 🔗 CHAMADA POR  : Razor Pages Model Binding
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
         ****************************************************************************************/
        public class InputModel
            {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGet
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar GET da página de recuperação de senha. Executa logout
         *                   do usuário atual (comportamento questionável - provavelmente
         *                   código copiado do LogoutModel por engano).
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : [Task] - Operação assíncrona sem retorno
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/ForgotPassword)
         *
         * 🔄 CHAMA        : _signInManager.SignOutAsync(), _logger.LogInformation()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         *
         * 📝 OBSERVAÇÕES  : ATENÇÃO: Este método executa SignOut, o que provavelmente não
         *                   é o comportamento desejado em uma página de recuperação de senha.
         *                   Parece ter sido copiado do LogoutModel. Revisar se necessário.
         ****************************************************************************************/
        public async Task OnGet()
            {
            try
            {
                // [DOC] ATENÇÃO: SignOut aqui é questionável - revisar se necessário
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar GET de ForgotPassword");
                // [DOC] Falha silenciosa - página ainda é exibida mesmo com erro
            }
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnPostAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar solicitação de recuperação de senha. Valida email, gera
         *                   token de reset, envia email com link de recuperação e redireciona
         *                   para página de confirmação.
         *
         * 📥 ENTRADAS     : Input.Email (binding) - Email do usuário para recuperação
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - RedirectToPage("ForgotPasswordConfirmation")
         *                   ou Page() em caso de validação falhar
         *
         * 🔗 CHAMADA POR  : Motor Razor (POST /Account/ForgotPassword)
         *
         * 🔄 CHAMA        : _userManager.FindByEmailAsync(), _userManager.IsEmailConfirmedAsync(),
         *                   _userManager.GeneratePasswordResetTokenAsync(), _emailSender.SendEmailAsync()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, HtmlEncoder
         *
         * 📝 OBSERVAÇÕES  : Por segurança, sempre redireciona para confirmação mesmo se usuário
         *                   não existir ou email não confirmado (evita enumeration attack).
         ****************************************************************************************/
        public async Task<IActionResult> OnPostAsync()
            {
            try
            {
                // [DOC] Valida model (Email obrigatório e formato válido)
                if (ModelState.IsValid)
                    {
                    // [DOC] Busca usuário por email
                    var user = await _userManager.FindByEmailAsync(Input.Email);

                    // [DOC] Por segurança, não revela se usuário existe ou se email não está confirmado
                    // Sempre redireciona para confirmação para evitar enumeration attack
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                        {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToPage("./ForgotPasswordConfirmation");
                        }

                    // [DOC] Gera token único de reset de senha para este usuário
                    // For more information on how to enable account confirmation and password reset please
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // [DOC] Constrói URL de callback com token para página de reset
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { code },
                        protocol: Request.Scheme);

                    // [DOC] Envia email com link de recuperação (link codificado para segurança)
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return RedirectToPage("./ForgotPasswordConfirmation");
                    }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar recuperação de senha para email: {Email}", Input?.Email);
                TempData["Erro"] = $"Erro ao processar solicitação: {ex.Message}";
                return Page();
            }
            }
        }
    }


