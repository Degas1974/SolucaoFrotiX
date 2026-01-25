using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _sender;

        /* > ---------------------------------------------------------------------------------------
         > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
         > ---------------------------------------------------------------------------------------
         > 🆔 **Nome:** RegisterConfirmation.cshtml.cs
         > 📍 **Local:** Areas/Identity/Pages/Account
         > ❓ **Por que existo?** Exibe a confirmação de registro do usuário (e link de ativação em ambiente de desenvolvimento).
         > 🔗 **Relevância:** Média (Feedback de Registro)
         > --------------------------------------------------------------------------------------- */

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RegisterConfirmationModel                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa dependências para confirmação de registro.                     ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite buscar usuário e gerar link de confirmação.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • userManager (UserManager<IdentityUser>): gerenciador de usuários.       ║
        /// ║    • sender (IEmailSender): serviço de envio de e-mail.                      ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara fluxo de confirmação.                              ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependências).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: RegisterConfirmation.cshtml                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGetAsync                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe a confirmação de registro e, em dev, gera link de ativação.         ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Informa o usuário e facilita testes de confirmação de conta.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • email (string): e-mail do usuário recém-registrado.                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: página de confirmação ou redirecionamento.               ║
        /// ║    • Significado: define visibilidade do link e exibe a view.                ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _userManager.FindByEmailAsync() → busca usuário.                        ║
        /// ║    • _userManager.GenerateEmailConfirmationTokenAsync() → token.            ║
        /// ║    • _userManager.GetUserIdAsync() → id do usuário.                          ║
        /// ║    • Url.Page() → monta URL de confirmação.                                  ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/RegisterConfirmation).    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: RegisterConfirmation.cshtml                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string email)
        {
            try
            {
                if (email == null)
                {
                    return RedirectToPage("/Index");
                }

                // [DADOS] Buscar usuário
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound($"Unable to load user with email '{email}'.");
                }

                Email = email;
                
                // [LOGICA] Verificar se deve exibir link (Dev mode)
                // > 🧠 **Lógica:** Exibe link de confirmação direta apenas para facilitar testes
                DisplayConfirmAccountLink = true;
                
                if (DisplayConfirmAccountLink)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    EmailConfirmationUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code },
                        protocol: Request.Scheme);
                }

                return Page();
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("RegisterConfirmation.cshtml.cs", "OnGetAsync", error);
                return RedirectToPage("/Index");
            }
        }
    }
}


