/* ****************************************************************************************
 * ⚡ ARQUIVO: Logout.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para logout de usuários com SignOut e redirecionamento.
 *
 * 📥 ENTRADAS     : returnUrl (string) no POST.
 *
 * 📤 SAÍDAS       : IActionResult (Redirect/Page) e mensagens de log.
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/Logout).
 *
 * 🔄 CHAMA        : SignInManager.SignOutAsync(), Logger.LogInformation().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger, Razor Pages.
 *
 * 📝 OBSERVAÇÕES  : OnGet realiza SignOut automaticamente; OnPost aceita returnUrl.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: LogoutModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para logout de usuários. Executa SignOut tanto em GET
 *                   quanto em POST, loga evento e exibe página de confirmação.
 *
 * 📥 ENTRADAS     : returnUrl (string) - URL de retorno após logout (apenas POST)
 *
 * 📤 SAÍDAS       : IActionResult (POST) - LocalRedirect ou Page()
 *                   void (GET) - Apenas executa SignOut e exibe página
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /Account/Logout)
 *
 * 🔄 CHAMA        : SignInManager.SignOutAsync(), Logger.LogInformation()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
 *
 * 📝 OBSERVAÇÕES  : OnGet executa SignOut automaticamente (sem confirmação). OnPost
 *                   suporta returnUrl para redirecionamento pós-logout.
 ****************************************************************************************/
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FrotiX.Models;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class LogoutModel : PageModel
        {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: LogoutModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa SignInManager e Logger via injeção de dependência.
         *
         * 📥 ENTRADAS     : [SignInManager<IdentityUser>] signInManager - Gerenciador de autenticação
         *                   [ILogger<LogoutModel>] logger - Logger para auditoria
         *
         * 📤 SAÍDAS       : Instância configurada de LogoutModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         ****************************************************************************************/
        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
            {
            _signInManager = signInManager;
            _logger = logger;
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGet
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar GET de logout. Executa SignOut automaticamente sem
         *                   confirmação e exibe página de logout.
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : [Task] - Operação assíncrona sem retorno
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /Account/Logout)
         *
         * 🔄 CHAMA        : _signInManager.SignOutAsync(), _logger.LogInformation()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         *
         * 📝 OBSERVAÇÕES  : SignOut imediato sem confirmação.
         ****************************************************************************************/
        public async Task OnGet()
            {
            try
            {
                // [DOC] Executa logout do usuário atual
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar GET de Logout");
                // [DOC] Falha silenciosa - página ainda é exibida
            }
            }

        /****************************************************************************************
         * ⚡ MÉTODO: OnPost
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar POST de logout. Executa SignOut e redireciona para
         *                   returnUrl se fornecido, ou exibe página de logout.
         *
         * 📥 ENTRADAS     : [string] returnUrl - URL de retorno após logout (opcional)
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - LocalRedirect ou Page()
         *
         * 🔗 CHAMADA POR  : Motor Razor (POST /Account/Logout)
         *
         * 🔄 CHAMA        : _signInManager.SignOutAsync(), _logger.LogInformation()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, ILogger
         *
         * 📝 OBSERVAÇÕES  : Suporta redirecionamento pós-logout via returnUrl.
         ****************************************************************************************/
        public async Task<IActionResult> OnPost(string returnUrl = null)
            {
            try
            {
                // [DOC] Executa logout do usuário atual
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");

                // [DOC] Redireciona para returnUrl se fornecido, senão exibe página de logout
                if (returnUrl != null)
                    {
                    return LocalRedirect(returnUrl);
                    }
                else
                    {
                    return Page();
                    }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar POST de Logout");
                TempData["Erro"] = $"Erro ao processar logout: {ex.Message}";
                return Page();
            }
            }
        }
    }

