/* ****************************************************************************************
 * ⚡ ARQUIVO: ResetPassword.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para redefinição de senha via token de recuperação.
 *
 * 📥 ENTRADAS     : Input.Email, Input.Password, Input.ConfirmPassword, Input.Code.
 *
 * 📤 SAÍDAS       : IActionResult (RedirectToPage ou Page) e mensagens de erro.
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/ResetPassword).
 *
 * 🔄 CHAMA        : UserManager.FindByEmailAsync(), ResetPasswordAsync().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger, Razor Pages.
 *
 * 📝 OBSERVAÇÕES  : Não revela existência do usuário por segurança.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: ResetPasswordModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Processar redefinição de senha com token. Valida código,
 *                   atualiza credenciais e redireciona para confirmação.
 *
 * 📥 ENTRADAS     : Input.Email, Input.Password, Input.ConfirmPassword, Input.Code
 *
 * 📤 SAÍDAS       : IActionResult - RedirectToPage ou Page()
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/ResetPassword)
 *
 * 🔄 CHAMA        : UserManager.FindByEmailAsync(), ResetPasswordAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
 ****************************************************************************************/
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
        {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResetPasswordModel> _logger;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: ResetPasswordModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar dependências de Identity e logging.
         *
         * 📥 ENTRADAS     : [UserManager<IdentityUser>] userManager - Gerenciador de usuários
         *                   [ILogger<ResetPasswordModel>] logger - Logger para auditoria
         *
         * 📤 SAÍDAS       : Instância configurada de ResetPasswordModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         ****************************************************************************************/
        public ResetPasswordModel(UserManager<IdentityUser> userManager, ILogger<ResetPasswordModel> logger)
            {
            _userManager = userManager;
            _logger = logger;
            }

        [BindProperty]
        public InputModel Input { get; set; }

        /****************************************************************************************
         * ⚡ CLASSE: InputModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Model de entrada para redefinição de senha.
         *
         * 📥 ENTRADAS     : Email, Password, ConfirmPassword, Code
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnGet
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Validar código de recuperação e inicializar formulário
         * 📥 ENTRADAS     : [string] code - Código de recuperação enviado por email
         * 📤 SAÍDAS       : [IActionResult] - Página com formulário ou BadRequest
         * 🔗 CHAMADA POR  : Framework ASP.NET Core via link de recuperação
         * 🔄 CHAMA        : Nenhuma função
         * 📦 DEPENDÊNCIAS : ASP.NET Core Razor Pages
         * --------------------------------------------------------------------------------------
         * [DOC] Valida presença do código de recuperação antes de exibir formulário
         * [DOC] Armazena código no InputModel para envio junto com nova senha
         ****************************************************************************************/
        public IActionResult OnGet(string code = null)
            {
            try
                {
                // [DOC] Valida se código de recuperação foi fornecido na URL
                if (code == null)
                    {
                    _logger.LogWarning("Tentativa de acessar reset de senha sem código");
                    return BadRequest("A code must be supplied for password reset.");
                    }

                // [DOC] Inicializa InputModel com código para manter no hidden field
                Input = new InputModel
                    {
                    Code = code
                    };
                return Page();
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, "Erro ao carregar página de reset de senha");
                TempData["Erro"] = "Erro ao carregar formulário. Tente novamente.";
                return BadRequest("Erro ao processar solicitação.");
                }
            }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar redefinição de senha com token de recuperação
         * 📥 ENTRADAS     : [InputModel] Input - Email, nova senha e código de recuperação
         * 📤 SAÍDAS       : [IActionResult] - Redirect para confirmação ou Page() com erros
         * 🔗 CHAMADA POR  : Formulário de reset de senha (POST)
         * 🔄 CHAMA        : UserManager.FindByEmailAsync(), ResetPasswordAsync()
         * 📦 DEPENDÊNCIAS : ASP.NET Identity
         * --------------------------------------------------------------------------------------
         * [DOC] Não revela se usuário existe ou não por segurança
         * [DOC] Valida código e email antes de permitir reset
         * [DOC] Redireciona sempre para confirmação (mesmo se usuário não existir)
         ****************************************************************************************/
        public async Task<IActionResult> OnPostAsync()
            {
            try
                {
                // [DOC] Valida dados do formulário (senha, confirmação, email)
                if (!ModelState.IsValid)
                    {
                    return Page();
                    }

                // [DOC] Busca usuário pelo email fornecido
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                    {
                    // [DOC] Por segurança, não revela que usuário não existe
                    // [DOC] Sempre redireciona para confirmação
                    _logger.LogWarning($"Tentativa de reset de senha para email inexistente: {Input.Email}");
                    return RedirectToPage("./ResetPasswordConfirmation");
                    }

                // [DOC] Executa reset de senha validando código de recuperação
                var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

                if (result.Succeeded)
                    {
                    _logger.LogInformation($"Senha redefinida com sucesso para usuário: {Input.Email}");
                    return RedirectToPage("./ResetPasswordConfirmation");
                    }

                // [DOC] Se reset falhar (código inválido/expirado), exibe erros
                foreach (var error in result.Errors)
                    {
                    ModelState.AddModelError(string.Empty, error.Description);
                    }

                return Page();
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, $"Erro ao resetar senha para email: {Input.Email}");
                TempData["Erro"] = "Erro ao redefinir senha. Tente novamente ou solicite novo código.";
                return Page();
                }
            }
        }
    }


