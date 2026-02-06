/* ****************************************************************************************
 * ⚡ ARQUIVO: Register.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel de registro de novos usuários, com criação de conta,
 *                   validação de domínio e login automático.
 *
 * 📥 ENTRADAS     : Input.Ponto, Input.NomeCompleto, Input.Email, Input.Senha,
 *                   Input.ConfirmacaoSenha, returnUrl.
 *
 * 📤 SAÍDAS       : IActionResult (LocalRedirect ou Page) e mensagens de erro.
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/Register).
 *
 * 🔄 CHAMA        : UserManager.CreateAsync(), SignInManager.SignInAsync().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, ILogger, AspNetUsers.
 *
 * 📝 OBSERVAÇÕES  : Confirmação de email está comentada; valida domínio com ValidateDomainAtEnd.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: RegisterModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para registro de novos usuários no FrotiX. Cria conta,
 *                   valida domínio de email e realiza login automático.
 *
 * 📥 ENTRADAS     : Input.Ponto, Input.NomeCompleto, Input.Email, Input.Senha,
 *                   Input.ConfirmacaoSenha, returnUrl
 *
 * 📤 SAÍDAS       : IActionResult - LocalRedirect para LoginFrotiX ou Page() com erros
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/Register)
 *
 * 🔄 CHAMA        : UserManager.CreateAsync(), SignInManager.SignInAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, ILogger, AspNetUsers
 *
 * 📝 OBSERVAÇÕES  : Confirmação de email está comentada; valida domínio via
 *                   ValidateDomainAtEnd.
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
using FrotiX.Models;
using FrotiX.Services;
using FrotiX.Validations;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class RegisterModel : PageModel
        {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: RegisterModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar dependências de Identity e envio de email.
         *
         * 📥 ENTRADAS     : [UserManager<IdentityUser>] userManager - Gerenciador de usuários
         *                   [SignInManager<IdentityUser>] signInManager - Gerenciador de autenticação
         *                   [ILogger<RegisterModel>] logger - Logger para auditoria
         *                   [IEmailSender] emailSender - Serviço de envio de email
         *
         * 📤 SAÍDAS       : Instância configurada de RegisterModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger, IEmailSender
         ****************************************************************************************/
        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<RegisterModel> logger,
            IEmailSender emailSender)
            {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnGet
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar página de registro e capturar URL de retorno
         * 📥 ENTRADAS     : [string] returnUrl - URL para redirecionar após registro (opcional)
         * 📤 SAÍDAS       : void - Armazena ReturnUrl para uso posterior
         * 🔗 CHAMADA POR  : Framework ASP.NET Core quando a página é acessada via GET
         * 🔄 CHAMA        : Nenhuma função
         * 📦 DEPENDÊNCIAS : ASP.NET Core Razor Pages
         ****************************************************************************************/
        public void OnGet(string returnUrl = null)
            {
            try
                {
                // [DOC] Armazena URL de retorno para redirecionamento após registro bem-sucedido
                ReturnUrl = returnUrl;
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, "Erro ao inicializar página de registro");
                TempData["Erro"] = "Erro ao carregar página de registro. Tente novamente.";
                }
            }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar registro de novo usuário no sistema FrotiX
         * 📥 ENTRADAS     : [string] returnUrl - URL de retorno após registro
         *                   [InputModel] Input - Dados do formulário (Ponto, Email, Nome, Senha)
         * 📤 SAÍDAS       : [IActionResult] - Redirect para LoginFrotiX ou Page() com erros
         * 🔗 CHAMADA POR  : Formulário de registro (POST)
         * 🔄 CHAMA        : UserManager.CreateAsync(), SignInManager.SignInAsync()
         * 📦 DEPENDÊNCIAS : ASP.NET Identity, Logger, AspNetUsers Model
         * --------------------------------------------------------------------------------------
         * [DOC] Valida domínio @camara.leg.br via atributo ValidateDomainAtEnd
         * [DOC] Cria usuário AspNetUsers com Ponto como username
         * [DOC] Faz login automático após registro bem-sucedido
         * [DOC] Email confirmation desabilitado (código comentado)
         ****************************************************************************************/
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
            {
            try
                {
                returnUrl = returnUrl ?? Url.Content("~/");

                if (ModelState.IsValid)
                    {
                    // [DOC] Cria objeto AspNetUsers com dados do formulário
                    var user = new AspNetUsers
                        {
                        UserName = Input.Ponto,
                        Email = Input.Email,
                        NomeCompleto = Input.NomeCompleto,
                        Ponto = Input.Ponto
                        };

                    // [DOC] Cria usuário no Identity com senha criptografada
                    var result = await _userManager.CreateAsync(user, Input.Senha);

                    if (result.Succeeded)
                        {
                        _logger.LogInformation("User created a new account with password.");

                        // [DOC] Confirmação de email desabilitada - seria necessário configurar IEmailSender
                        // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        // var callbackUrl = Url.Page("/Account/ConfirmEmail", null, new {userId = user.Id, code}, Request.Scheme);
                        //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // [DOC] Faz login automático sem persistir cookie (isPersistent: false)
                        await _signInManager.SignInAsync(user, false);
                        return LocalRedirect("/Identity/Account/LoginFrotiX");
                        }

                    // [DOC] Se criação falhar, adiciona erros ao ModelState para exibir no form
                    foreach (var error in result.Errors)
                        {
                        ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                // [DOC] Se chegou aqui, houve erro de validação ou falha na criação
                return Page();
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, "Erro ao registrar novo usuário");
                TempData["Erro"] = "Erro ao processar registro. Verifique os dados e tente novamente.";
                return Page();
                }
            }

        /****************************************************************************************
         * ⚡ CLASSE: InputModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Model de entrada para formulário de registro.
         *
         * 📥 ENTRADAS     : Ponto, NomeCompleto, Email, Senha, ConfirmacaoSenha
         *
         * 📤 SAÍDAS       : Objeto validado via Data Annotations
         *
         * 🔗 CHAMADA POR  : Razor Pages Model Binding
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, ValidateDomainAtEnd
         ****************************************************************************************/
        public class InputModel
            {
            [Required]
            [Display(Name = "Ponto")]
            public string Ponto { get; set; }

            [Required]
            [Display(Name = "Nome Completo")]
            public string NomeCompleto { get; set; }

            [Required]
            [EmailAddress]
            [ValidateDomainAtEnd(domainValue: "@camara.leg.br")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            //[StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Senha { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmação de Senha")]
            [Compare("Senha", ErrorMessage = "A senha e a confirmação não combinam.")]
            public string ConfirmacaoSenha { get; set; }

            //[Required]
            //[Display(Name = "I agree to terms & conditions")]
            //public bool AgreeToTerms { get; set; }

            //[Display(Name = "Sign up for newsletters")]
            //public bool SignUp { get; set; }
            }
        }
    }

