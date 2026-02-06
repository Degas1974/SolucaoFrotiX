/* ****************************************************************************************
 * ⚡ ARQUIVO: LoginFrotiX.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel customizado do login FrotiX, retornando JSON para
 *                   autenticação via AJAX.
 *
 * 📥 ENTRADAS     : Input.Ponto, Input.Password, Input.RememberMe, returnUrl.
 *
 * 📤 SAÍDAS       : JsonResult { isSuccess, returnUrl, message }.
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/LoginFrotiX).
 *
 * 🔄 CHAMA        : SignInManager.PasswordSignInAsync().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger, IUnitOfWork.
 *
 * 📝 OBSERVAÇÕES  : IUnitOfWork é injetado mas não utilizado no código atual.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: LoginFrotiX (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel customizado para login do FrotiX. Similar ao LoginModel
 *                   padrão, mas retorna JSON ao invés de Page/Redirect para suportar
 *                   login via AJAX. Integra com IUnitOfWork.
 *
 * 📥 ENTRADAS     : Input.Ponto (string) - Username/Ponto (formato p_xxxx)
 *                   Input.Password (string) - Senha
 *                   Input.RememberMe (bool) - Manter logado
 *                   returnUrl (string) - URL de retorno (opcional)
 *
 * 📤 SAÍDAS       : JsonResult - { isSuccess, returnUrl, message }
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/LoginFrotiX) via AJAX
 *
 * 🔄 CHAMA        : SignInManager.PasswordSignInAsync(), IUnitOfWork (injetado mas não usado)
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger, IUnitOfWork
 *
 * 📝 OBSERVAÇÕES  : Versão customizada que retorna JSON para suportar login assíncrono.
 *                   IUnitOfWork injetado mas não utilizado no código atual.
 ****************************************************************************************/
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

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class LoginFrotiX : PageModel
        {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: LoginFrotiX
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa dependências via injeção (SignInManager, Logger, UnitOfWork).
         *
         * 📥 ENTRADAS     : [SignInManager<IdentityUser>] signInManager - Gerenciador de autenticação
         *                   [ILogger<LoginModel>] logger - Logger para auditoria
         *                   [IUnitOfWork] unitOfWork - Acesso ao repositório (não usado atualmente)
         *
         * 📤 SAÍDAS       : Instância configurada de LoginFrotiX
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger, IUnitOfWork
         ****************************************************************************************/
        public LoginFrotiX(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, IUnitOfWork unitOfWork)
            {
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            }

        [BindProperty]
        public LoginFrotiXModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        /****************************************************************************************
         * ⚡ CLASSE: LoginFrotiXModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Model de entrada para formulário de login FrotiX. Mensagens de
         *                   erro customizadas em português e formato de Ponto específico.
         *
         * 📥 ENTRADAS     : Ponto (string) - Username no formato p_xxxx
         *                   Password (string) - Senha do usuário
         *                   RememberMe (bool) - Manter autenticação
         *
         * 📤 SAÍDAS       : Objeto validado via Data Annotations
         *
         * 🔗 CHAMADA POR  : Razor Pages Model Binding
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
         ****************************************************************************************/
        public class LoginFrotiXModel
            {
            [Required(ErrorMessage = "Insira o seu ponto! (p_xxxx)")]
            public string Ponto { get; set; }

            [Required(ErrorMessage = "A senha é obrigatória!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGetAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar GET da página de login FrotiX. Prepara página limpando
         *                   cookies externos e carregando esquemas de autenticação externa.
         *
         * 📥 ENTRADAS     : [string] returnUrl - URL de retorno após login (opcional)
         *
         * 📤 SAÍDAS       : [Task] - Operação assíncrona sem retorno
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/LoginFrotiX)
         *
         * 🔄 CHAMA        : HttpContext.SignOutAsync(), _signInManager.GetExternalAuthenticationSchemesAsync()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, Authentication
         *
         * 📝 OBSERVAÇÕES  : Idêntico ao LoginModel.OnGetAsync - código duplicado.
         ****************************************************************************************/
        public async Task OnGetAsync(string returnUrl = null)
            {
            try
            {
                // [DOC] Adiciona mensagem de erro ao ModelState se ErrorMessage estiver definido
                if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    }

                // [DOC] Define URL de retorno padrão se não fornecida
                returnUrl = returnUrl ?? Url.Content("~/");

                // [DOC] Limpa cookies de autenticação externa para garantir processo limpo
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                // [DOC] Carrega lista de provedores de autenticação externa
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ReturnUrl = returnUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar GET de LoginFrotiX");
                // [DOC] Falha silenciosa - página ainda é exibida
            }
            }


        /****************************************************************************************
         * ⚡ MÉTODO: OnPostAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar tentativa de login via AJAX. Valida credenciais e retorna
         *                   JSON com resultado (isSuccess, returnUrl, message) ao invés de
         *                   redirect/page tradicional.
         *
         * 📥 ENTRADAS     : [string] returnUrl - URL de retorno (opcional)
         *                   Input.Ponto (binding) - Username/Ponto
         *                   Input.Password (binding) - Senha
         *                   Input.RememberMe (binding) - Manter logado
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - JsonResult com { isSuccess, returnUrl?, message? }
         *
         * 🔗 CHAMADA POR  : Motor Razor (POST /Account/LoginFrotiX) via AJAX
         *
         * 🔄 CHAMA        : _signInManager.PasswordSignInAsync()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         *
         * 📝 OBSERVAÇÕES  : Retorna JSON para suportar login assíncrono. Hardcoded returnUrl
         *                   (/intel/analyticsdashboard) ao invés de usar parâmetro.
         ****************************************************************************************/
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
            {
            try
            {
                // [DOC] Define URL de retorno padrão se não fornecida
                returnUrl = returnUrl ?? Url.Content("~/");

                if (ModelState.IsValid)
                    {
                    // [DOC] Tenta autenticar usuário com Ponto e senha
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(Input.Ponto, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                    // [DOC] Login bem-sucedido - retorna JSON com URL de redirecionamento (HARDCODED)
                    if (result.Succeeded)
                        {
                        _logger.LogInformation("User logged in.");
                        return new JsonResult(
                            new
                                {
                                isSuccess = true,
                                returnUrl = "/intel/analyticsdashboard"
                                });
                        //return LocalRedirect("/intel/analyticsdashboard");
                        }

                    // [DOC] Requer autenticação de dois fatores - retorna JSON com URL de 2FA
                    if (result.RequiresTwoFactor)
                        {
                        return new JsonResult(
                            new
                                {
                                isSuccess = true,
                                returnUrl = "./LoginWith2fa"
                                });
                        //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }

                    // [DOC] Conta bloqueada - retorna JSON com URL de lockout
                    if (result.IsLockedOut)
                        {
                        _logger.LogWarning("User account locked out.");
                        return new JsonResult(
                            new
                                {
                                isSuccess = true,
                                returnUrl = "./Lockout"
                                });
                        //return RedirectToPage("./Lockout");
                        }
                    else
                        {
                        // [DOC] Credenciais inválidas - retorna JSON com falha
                        ModelState.AddModelError(string.Empty, "Login Inválido.");
                        return new JsonResult(
                            new
                                {
                                isSuccess = false
                                });
                        }
                    }

                // [DOC] ModelState inválido - extrai primeira mensagem de erro
                var errorMessage = "";
                foreach (var modelState in ViewData.ModelState.Values)
                    {
                    foreach (ModelError error in modelState.Errors)
                        {
                        errorMessage = error.ErrorMessage;
                        }
                    }

                // [DOC] Retorna JSON com mensagem de erro de validação
                return new JsonResult(
                            new
                                {
                                isSuccess = false,
                                message = errorMessage
                                });

                // If we got this far, something failed, redisplay form
                //return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar login FrotiX para Ponto: {Ponto}", Input?.Ponto);
                return new JsonResult(
                    new
                        {
                        isSuccess = false,
                        message = $"Erro ao processar login: {ex.Message}"
                        });
            }
            }
        }
    }

