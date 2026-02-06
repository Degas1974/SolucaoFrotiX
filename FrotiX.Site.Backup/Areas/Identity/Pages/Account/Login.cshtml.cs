/* ****************************************************************************************
 * ⚡ ARQUIVO: Login.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel de autenticação, processando login via Ponto/senha e
 *                   suportando 2FA, lockout e autenticação externa.
 *
 * 📥 ENTRADAS     : Input.Ponto, Input.Password, Input.RememberMe, returnUrl.
 *
 * 📤 SAÍDAS       : IActionResult (RedirectToPage/LocalRedirect/Page).
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/Login).
 *
 * 🔄 CHAMA        : SignInManager.PasswordSignInAsync(), GetExternalAuthenticationSchemesAsync(),
 *                   HttpContext.SignOutAsync().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger, Razor Pages.
 *
 * 📝 OBSERVAÇÕES  : Integração Redecamara; redirecionamento hardcoded após login.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: LoginModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para autenticação de usuários. Processa login via Ponto
 *                   (username customizado) e senha da Redecamara, suportando autenticação
 *                   externa, 2FA e lockout.
 *
 * 📥 ENTRADAS     : Input.Ponto (string) - Username/Ponto do usuário
 *                   Input.Password (string) - Senha da Redecamara
 *                   Input.RememberMe (bool) - Manter logado
 *                   returnUrl (string) - URL de retorno após login
 *
 * 📤 SAÍDAS       : IActionResult - LocalRedirect, RedirectToPage ou Page()
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/Login)
 *
 * 🔄 CHAMA        : SignInManager.PasswordSignInAsync(), SignInManager.GetExternalAuthenticationSchemesAsync(),
 *                   HttpContext.SignOutAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
 *
 * 📝 OBSERVAÇÕES  : Integração customizada com Redecamara. Redireciona para
 *                   /intel/analyticsdashboard após login bem-sucedido (hardcoded).
 ****************************************************************************************/
using System;
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

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class LoginModel : PageModel
        {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: LoginModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa SignInManager e Logger via injeção de dependência.
         *
         * 📥 ENTRADAS     : [SignInManager<IdentityUser>] signInManager - Gerenciador de autenticação
         *                   [ILogger<LoginModel>] logger - Logger para auditoria
         *
         * 📤 SAÍDAS       : Instância configurada de LoginModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         ****************************************************************************************/
        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
            {
            _signInManager = signInManager;
            _logger = logger;
            }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        /****************************************************************************************
         * ⚡ CLASSE: InputModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Model de entrada para formulário de login. Customizado para usar
         *                   "Ponto" ao invés de "Email" como username.
         *
         * 📥 ENTRADAS     : Ponto (string) - Username customizado (integração Redecamara)
         *                   Password (string) - Senha do usuário
         *                   RememberMe (bool) - Manter autenticação por 30 dias
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
            public string Ponto { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGetAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar GET da página de login. Prepara página limpando cookies
         *                   externos, carregando esquemas de autenticação externa e exibindo
         *                   mensagens de erro se existirem.
         *
         * 📥 ENTRADAS     : [string] returnUrl - URL de retorno após login (opcional)
         *
         * 📤 SAÍDAS       : [Task] - Operação assíncrona sem retorno
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/Login)
         *
         * 🔄 CHAMA        : HttpContext.SignOutAsync(), _signInManager.GetExternalAuthenticationSchemesAsync()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, Authentication
         *
         * 📝 OBSERVAÇÕES  : Limpa cookies de autenticação externa para garantir processo limpo.
         ****************************************************************************************/
        public async Task OnGetAsync(string returnUrl = null)
            {
            try
            {
                // [DOC] Adiciona mensagem de erro ao ModelState se ErrorMessage estiver definido (via TempData)
                if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    }

                // [DOC] Define URL de retorno padrão (homepage) se não fornecida
                returnUrl = returnUrl ?? Url.Content("~/");

                // [DOC] Limpa cookies de autenticação externa para garantir processo limpo
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                // [DOC] Carrega lista de provedores de autenticação externa (Google, Facebook, etc.)
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ReturnUrl = returnUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar GET de Login");
                // [DOC] Falha silenciosa - página ainda é exibida
            }
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnPostAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar tentativa de login. Valida credenciais via Ponto e senha,
         *                   suporta 2FA e lockout, e redireciona conforme resultado.
         *
         * 📥 ENTRADAS     : [string] returnUrl - URL de retorno após login (opcional)
         *                   Input.Ponto (binding) - Username/Ponto
         *                   Input.Password (binding) - Senha
         *                   Input.RememberMe (binding) - Manter logado
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - LocalRedirect, RedirectToPage ou Page()
         *
         * 🔗 CHAMADA POR  : Motor Razor (POST /Account/Login)
         *
         * 🔄 CHAMA        : _signInManager.PasswordSignInAsync()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         *
         * 📝 OBSERVAÇÕES  : Redireciona para /intel/analyticsdashboard (hardcoded) ao invés
         *                   de usar returnUrl. lockoutOnFailure=true habilita bloqueio após
         *                   múltiplas tentativas falhas.
         ****************************************************************************************/
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
            {
            try
            {
                // [DOC] Define URL de retorno padrão (homepage) se não fornecida
                returnUrl = returnUrl ?? Url.Content("~/");

                if (ModelState.IsValid)
                    {
                    // [DOC] Tenta autenticar usuário com Ponto (username) e senha
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(Input.Ponto, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                    // [DOC] Login bem-sucedido - redireciona para dashboard (HARDCODED - ignora returnUrl)
                    if (result.Succeeded)
                        {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect("/intel/analyticsdashboard");
                        }

                    // [DOC] Requer autenticação de dois fatores
                    if (result.RequiresTwoFactor)
                        {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }

                    // [DOC] Conta bloqueada por tentativas falhas excessivas
                    if (result.IsLockedOut)
                        {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                        }
                    else
                        {
                        // [DOC] Credenciais inválidas - exibe erro
                        ModelState.AddModelError(string.Empty, "Login Inválido.");
                        return Page();
                        }
                    }

                // [DOC] ModelState inválido - reexibe formulário com erros
                // If we got this far, something failed, redisplay form
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar login para Ponto: {Ponto}", Input?.Ponto);
                TempData["Erro"] = $"Erro ao processar login: {ex.Message}";
                return Page();
            }
            }
        }
    }

