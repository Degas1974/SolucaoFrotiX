/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Lockout.cshtml.cs                                                                       ║
   ║ 📂 CAMINHO: /Areas/Identity/Pages/Account                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: PageModel para página de bloqueio/desbloqueio de conta.                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: LockoutModel                                                                            ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

/****************************************************************************************
 * ⚡ CLASSE: LockoutModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para página de desbloqueio de conta. Permite usuário
 *                   desbloquear tela bloqueada por múltiplas tentativas de login falhas.
 *
 * 📥 ENTRADAS     : Input.Password (string) - Senha para desbloquear
 *                   returnUrl (string) - URL de retorno após desbloqueio bem-sucedido
 *
 * 📤 SAÍDAS       : IActionResult - LocalRedirect, RedirectToPage ou Page()
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/Lockout)
 *
 * 🔄 CHAMA        : SignInManager.SignOutAsync(), SignInManager.PasswordSignInAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
 *
 * 📝 OBSERVAÇÕES  : OnGetAsync executa SignOut (comportamento questionável). ViewData["Email"]
 *                   é usado para obter userName (requer população prévia por middleware/filtro).
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
    public class LockoutModel : PageModel
        {
        private readonly ILogger<LogoutModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        [BindProperty]
        public InputModel Input { get; set; }

        /****************************************************************************************
         * ⚡ CLASSE: InputModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Model de entrada para formulário de desbloqueio.
         *
         * 📥 ENTRADAS     : Password (string) - Senha do usuário para desbloqueio
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
            [DataType(DataType.Password)]
            public string Password { get; set; }
            }

        /****************************************************************************************
         * ⚡ CONSTRUTOR: LockoutModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa SignInManager e Logger via injeção de dependência.
         *
         * 📥 ENTRADAS     : [SignInManager<IdentityUser>] signInManager - Gerenciador de autenticação
         *                   [ILogger<LogoutModel>] logger - Logger para auditoria
         *
         * 📤 SAÍDAS       : Instância configurada de LockoutModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         ****************************************************************************************/
        public LockoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
            {
            _signInManager = signInManager;
            _logger = logger;
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGetAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar GET da página de lockout. Executa SignOut do usuário
         *                   atual (comportamento questionável - provavelmente copiado de
         *                   LogoutModel por engano).
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : [Task] - Operação assíncrona sem retorno
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/Lockout)
         *
         * 🔄 CHAMA        : _signInManager.SignOutAsync(), _logger.LogInformation()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         *
         * 📝 OBSERVAÇÕES  : ATENÇÃO: SignOut em página de Lockout é questionável. Revisar lógica.
         ****************************************************************************************/
        public async Task OnGetAsync()
            {
            try
            {
                // [DOC] ATENÇÃO: SignOut em página de lockout parece incorreto - revisar
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar GET de Lockout");
                // [DOC] Falha silenciosa - página ainda é exibida
            }
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnPostAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar tentativa de desbloqueio de conta. Valida senha, tenta
         *                   login e redireciona conforme resultado (sucesso, 2FA, lockout).
         *
         * 📥 ENTRADAS     : [string] returnUrl - URL de retorno após desbloqueio (opcional)
         *                   Input.Password (binding) - Senha para desbloqueio
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - LocalRedirect, RedirectToPage ou Page()
         *
         * 🔗 CHAMADA POR  : Motor Razor (POST /Account/Lockout)
         *
         * 🔄 CHAMA        : _signInManager.PasswordSignInAsync()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         *
         * 📝 OBSERVAÇÕES  : userName obtido de ViewData["Email"] - requer população prévia.
         *                   lockoutOnFailure: true - novas falhas podem estender lockout.
         ****************************************************************************************/
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
            {
            try
            {
                // [DOC] Define URL de retorno padrão (homepage) se não fornecida
                returnUrl = returnUrl ?? Url.Content("~/");

                if (ModelState.IsValid)
                    {
                    // [DOC] Obtém userName de ViewData (deve ser populado por middleware/filtro)
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var userName = ViewData["Email"]?.ToString();

                    // [DOC] Tenta autenticar com senha fornecida (RememberMe=true, lockoutOnFailure=true)
                    var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, true, lockoutOnFailure: true);

                    // [DOC] Login bem-sucedido - redireciona para URL de retorno
                    if (result.Succeeded)
                        {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                        }

                    // [DOC] Requer autenticação de dois fatores
                    if (result.RequiresTwoFactor)
                        {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
                        }

                    // [DOC] Conta ainda bloqueada - redireciona para mesma página
                    if (result.IsLockedOut)
                        {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                        }
                    else
                        {
                        // [DOC] Senha incorreta - exibe erro
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                        }
                    }

                // [DOC] ModelState inválido - reexibe formulário com erros
                // If we got this far, something failed, redisplay form
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar desbloqueio para usuário: {UserName}", ViewData["Email"]);
                TempData["Erro"] = $"Erro ao processar desbloqueio: {ex.Message}";
                return Page();
            }
            }
        }
    }


