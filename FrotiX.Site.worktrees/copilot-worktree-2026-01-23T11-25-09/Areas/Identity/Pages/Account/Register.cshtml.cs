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

        /* > ---------------------------------------------------------------------------------------
         > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
         > ---------------------------------------------------------------------------------------
         > 🆔 **Nome:** Register.cshtml.cs
         > 📍 **Local:** Areas/Identity/Pages/Account
         > ❓ **Por que existo?** Gerencia o registro de novos usuários no sistema.
         > 🔗 **Relevância:** Alta (Criação de Contas)
         > --------------------------------------------------------------------------------------- */

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RegisterModel                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa dependências do fluxo de registro de usuários.                 ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Disponibiliza acesso ao Identity e envio de e-mail.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • userManager (UserManager<IdentityUser>): gerenciador de usuários.       ║
        /// ║    • signInManager (SignInManager<IdentityUser>): gerenciador de logins.     ║
        /// ║    • logger (ILogger<RegisterModel>): logger da página.                      ║
        /// ║    • emailSender (IEmailSender): serviço de envio de e-mail.                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara o fluxo de cadastro.                               ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependências).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Register.cshtml                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
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

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Prepara a tela de registro definindo a URL de retorno.                    ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Mantém a navegação coerente após o cadastro.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno pós-registro.                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: renderiza a página de cadastro.                                   ║
        /// ║    • Significado: prepara o GET da página.                                   ║
        /// ║    • Consumidor: pipeline Razor Pages.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma lógica externa.                                                 ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/Register).                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Register.cshtml                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public void OnGet(string returnUrl = null)
        {
            try
            {
                ReturnUrl = returnUrl;
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Register.cshtml.cs", "OnGet", error);
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnPostAsync                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa o cadastro, valida dados e cria o usuário no Identity.           ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante criação segura de contas e fluxo de login pós-cadastro.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno pós-registro.                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: redireciona para login ou retorna a página com erros.    ║
        /// ║    • Significado: confirma criação ou exibe validações.                      ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _userManager.CreateAsync() → cria usuário.                              ║
        /// ║    • _signInManager.SignInAsync() → login pós-cadastro.                      ║
        /// ║    • _logger.LogInformation() → registra evento.                             ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (POST /Identity/Account/Register).               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Register.cshtml                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");
                if (ModelState.IsValid)
                {
                    // [DADOS] Criação da entidade de usuário customizada
                    // > 🧠 **Lógica:** Mapeia InputModel para a entidade AspNetUsers customizada
                    var user = new AspNetUsers
                    {
                        UserName = Input.Ponto,
                        Email = Input.Email,
                        NomeCompleto = Input.NomeCompleto,
                        Ponto = Input.Ponto
                    };

                    // [LOGICA] Criação no Identity
                    // > 📡 **Conexão:** Banco de Dados (Criação de Usuário)
                    var result = await _userManager.CreateAsync(user, Input.Senha);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        // [REGRA] Login automático pós-cadastro e redirecionamento para tela de login
                        await _signInManager.SignInAsync(user, false);
                        return LocalRedirect("/Identity/Account/LoginFrotiX");
                    }

                    // [UI] Adicionar erros do Identity ao ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                // If we got this far, something failed, redisplay form
                return Page();
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Register.cshtml.cs", "OnPostAsync", error);
                return Page();
            }
        }

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


