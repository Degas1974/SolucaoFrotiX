using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FrotiX.Models;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FrotiX.Repository.IRepository;
using System.Security.Claims;
using FrotiX.Services;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginFrotiX : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /* > ---------------------------------------------------------------------------------------
         > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
         > ---------------------------------------------------------------------------------------
         > 🆔 **Nome:** LoginFrotiX.cshtml.cs
         > 📍 **Local:** Areas/Identity/Pages/Account
         > ❓ **Por que existo?** Gerencia o processo de login customizado do sistema FrotiX,
         >    autenticando usuários via Ponto e Senha.
         > 🔗 **Relevância:** Alta (Ponto de entrada principal do sistema)
         > --------------------------------------------------------------------------------------- */

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LoginFrotiX                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa dependências do login customizado FrotiX.                      ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Disponibiliza acesso a autenticação, dados e log centralizado.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • signInManager (SignInManager<IdentityUser>): gerenciador de logins.     ║
        /// ║    • logger (ILogger<LoginModel>): logger da página.                         ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a repositórios do sistema.             ║
        /// ║    • log (ILogService): serviço de log FrotiX.                               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara o fluxo de autenticação customizada.              ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependências).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: LoginFrotiX.cshtml                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public LoginFrotiX(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, IUnitOfWork unitOfWork, ILogService log)
        {
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _log = log;
        }

        [BindProperty]
        public LoginFrotiXModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class LoginFrotiXModel
        {
            [Required(ErrorMessage = "Insira o seu ponto! (p_xxxx)")]
            public string Ponto { get; set; }

            [Required(ErrorMessage = "A senha � obrigat�ria!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGetAsync                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Prepara a tela de login, limpa cookies e configura URL de retorno.        ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante estado limpo antes da autenticação.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno pós-login.                           ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task: operação assíncrona sem retorno explícito.                        ║
        /// ║    • Significado: prepara o GET da página.                                   ║
        /// ║    • Consumidor: pipeline Razor Pages.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • HttpContext.SignOutAsync() → limpa cookie externo.                      ║
        /// ║    • _signInManager.GetExternalAuthenticationSchemesAsync() → provedores.    ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/LoginFrotiX).              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: LoginFrotiX.cshtml                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task OnGetAsync(string returnUrl = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }

                returnUrl = returnUrl ?? Url.Content("~/");

                // Clear the existing external cookie to ensure a clean login process
                // [REGRA] Limpeza preventiva de cookies
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                // > ?? **Conex�o:** Busca esquemas de autentica��o externa (se houver)
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ReturnUrl = returnUrl;
            }
            catch (Exception error)
            {
                // ??? Blindagem Padronizada FrotiX
                _log.Error("Erro ao carregar tela de LoginFrotiX", error);
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("LoginFrotiX.cshtml.cs", "OnGetAsync", error);
            }
        }


        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnPostAsync                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa login via AJAX, valida credenciais e retorna JSON.               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Mantém o fluxo de autenticação customizada com resposta estruturada.      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno pós-login.                           ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status e rota de destino.                       ║
        /// ║    • Significado: sinaliza sucesso, 2FA, lockout ou erro.                    ║
        /// ║    • Consumidor: front-end de login FrotiX.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _signInManager.PasswordSignInAsync() → autentica usuário.               ║
        /// ║    • _log.Info()/Warning()/Error() → registra eventos.                       ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (POST /Identity/Account/LoginFrotiX).             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: LoginFrotiX.cshtml                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    // [LOGICA] Autentica��o via Ponto
                    // > ?? **L�gica:** Tenta login com senha e bloqueio ativado em caso de falhas
                    var result = await _signInManager.PasswordSignInAsync(Input.Ponto, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                    
                    if (result.Succeeded)
                    {
                        _log.Info($"Usu�rio logged in via Ponto: {Input.Ponto}");
                        // [REGRA] Retorno AJAX Sucesso
                        return new JsonResult(
                            new
                            {
                                isSuccess = true,
                                returnUrl = "/intel/analyticsdashboard"
                            });
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return new JsonResult(
                            new
                            {
                                isSuccess = true,
                                returnUrl = "./LoginWith2fa"
                            });
                    }
                    if (result.IsLockedOut)
                    {
                        _log.Warning($"Conta bloqueada para o ponto: {Input.Ponto}");
                        return new JsonResult(
                            new
                            {
                                isSuccess = true,
                                returnUrl = "./Lockout"
                            });
                    }
                    else
                    {
                        _log.Warning($"Tentativa de login falhou para o ponto: {Input.Ponto}");
                        ModelState.AddModelError(string.Empty, "Login Inv�lido.");
                        return new JsonResult(
                            new
                            {
                                isSuccess = false
                            });
                    }
                }

                var errorMessage = "";

                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorMessage = error.ErrorMessage;
                    }
                }

                // [REGRA] Retorno AJAX Erro de Valida��o
                return new JsonResult(
                            new
                            {
                                isSuccess = false,
                                message = errorMessage
                            });
            }
            catch (Exception error)
            {
                // ??? Blindagem Padronizada FrotiX
                _log.Error($", errorErro ao processar login via Ponto: {Input?.Ponto}");
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("LoginFrotiX.cshtml.cs", "OnPostAsync", error);
                return new JsonResult(new { isSuccess = false, message = "Erro interno ao realizar login." });
            }
        }
    }
}


