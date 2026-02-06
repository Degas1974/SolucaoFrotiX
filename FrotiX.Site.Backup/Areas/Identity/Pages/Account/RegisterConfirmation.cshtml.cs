/* ****************************************************************************************
 * ⚡ ARQUIVO: RegisterConfirmation.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para confirmação de registro, gerando link de validação
 *                   e exibindo instruções ao usuário.
 *
 * 📥 ENTRADAS     : email (string) via query string.
 *
 * 📤 SAÍDAS       : IActionResult (Page/Redirect/NotFound) e EmailConfirmationUrl.
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/RegisterConfirmation).
 *
 * 🔄 CHAMA        : UserManager.FindByEmailAsync(), GenerateEmailConfirmationTokenAsync().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, WebEncoders, ILogger.
 *
 * 📝 OBSERVAÇÕES  : Exibe link direto quando IEmailSender não está configurado.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: RegisterConfirmationModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Processar confirmação de registro, gerando link de validação
 *                   e exibindo instruções ao usuário.
 *
 * 📥 ENTRADAS     : email (string) - Email do usuário recém-registrado
 *
 * 📤 SAÍDAS       : IActionResult - Página com link de confirmação ou redirect
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET /Account/RegisterConfirmation)
 *
 * 🔄 CHAMA        : UserManager.FindByEmailAsync(), GenerateEmailConfirmationTokenAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, ILogger, WebEncoders
 *
 * 📝 OBSERVAÇÕES  : Exibe link direto de confirmação quando IEmailSender
 *                   não está configurado no ambiente.
 ****************************************************************************************/
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace FrotiX.Areas.Identity.Pages.Account
    {
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
        {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly ILogger<RegisterConfirmationModel> _logger;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: RegisterConfirmationModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar dependências para validação e envio de email.
         *
         * 📥 ENTRADAS     : [UserManager<IdentityUser>] userManager - Gerenciador de usuários
         *                   [IEmailSender] sender - Serviço de envio de email
         *                   [ILogger<RegisterConfirmationModel>] logger - Logger para auditoria
         *
         * 📤 SAÍDAS       : Instância configurada de RegisterConfirmationModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, IEmailSender, ILogger
         ****************************************************************************************/
        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender, ILogger<RegisterConfirmationModel> logger)
            {
            _userManager = userManager;
            _sender = sender;
            _logger = logger;
            }

        // [DOC] Email do usuário recém-registrado (query string)
        public string Email { get; set; }

        // [DOC] Indica se deve exibir link direto de confirmação (sem IEmailSender)
        public bool DisplayConfirmAccountLink { get; set; }

        // [DOC] URL completa para confirmar o email do usuário
        public string EmailConfirmationUrl { get; set; }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnGetAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar link de confirmação de email para novo usuário registrado
         * 📥 ENTRADAS     : [string] email - Email do usuário registrado
         * 📤 SAÍDAS       : [IActionResult] - Página com link de confirmação ou erro
         * 🔗 CHAMADA POR  : Framework ASP.NET Core após redirecionamento do Register
         * 🔄 CHAMA        : UserManager.FindByEmailAsync(), GenerateEmailConfirmationTokenAsync()
         * 📦 DEPENDÊNCIAS : ASP.NET Identity, WebEncoders
         * --------------------------------------------------------------------------------------
         * [DOC] Valida se email foi fornecido, busca usuário no Identity
         * [DOC] Gera token de confirmação e cria URL completa para confirmação
         * [DOC] DisplayConfirmAccountLink=true pois IEmailSender não está configurado
         ****************************************************************************************/
        public async Task<IActionResult> OnGetAsync(string email)
            {
            try
                {
                // [DOC] Valida se email foi fornecido na query string
                if (email == null)
                    {
                    _logger.LogWarning("Tentativa de acessar confirmação de registro sem email");
                    return RedirectToPage("/Index");
                    }

                // [DOC] Busca usuário pelo email fornecido
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    {
                    _logger.LogWarning($"Usuário com email '{email}' não encontrado para confirmação");
                    return NotFound($"Unable to load user with email '{email}'.");
                    }

                Email = email;

                // [DOC] Exibe link de confirmação diretamente (IEmailSender não configurado)
                // [DOC] Em produção, remover isso e configurar envio real de email
                DisplayConfirmAccountLink = true;

                if (DisplayConfirmAccountLink)
                    {
                    var userId = await _userManager.GetUserIdAsync(user);

                    // [DOC] Gera token de confirmação criptografado
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // [DOC] Cria URL completa para confirmação de email
                    EmailConfirmationUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code },
                        protocol: Request.Scheme);
                    }

                return Page();
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, $"Erro ao processar confirmação de registro para email: {email}");
                TempData["Erro"] = "Erro ao processar confirmação. Contate o suporte.";
                return RedirectToPage("/Index");
                }
            }
        }
    }

