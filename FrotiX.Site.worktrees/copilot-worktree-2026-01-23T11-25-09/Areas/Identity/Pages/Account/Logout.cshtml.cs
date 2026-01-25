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

        /* > ---------------------------------------------------------------------------------------
         > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
         > ---------------------------------------------------------------------------------------
         > 🆔 **Nome:** Logout.cshtml.cs
         > 📍 **Local:** Areas/Identity/Pages/Account
         > ❓ **Por que existo?** Encerra a sessão do usuário no sistema.
         > 🔗 **Relevância:** Alta (Segurança e Ciclo de Vida da Sessão)
         > --------------------------------------------------------------------------------------- */

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LogoutModel                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa dependências do fluxo de logout.                              ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante controle de sessão e logging de saída do usuário.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • signInManager (SignInManager<IdentityUser>): gerenciador de logins.     ║
        /// ║    • logger (ILogger<LogoutModel>): logger da página.                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara o fluxo de logout.                                ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependências).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Logout.cshtml                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Realiza logout via GET (uso por link direto).                             ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Encerra sessão e registra a saída do usuário.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task: operação assíncrona sem retorno explícito.                        ║
        /// ║    • Significado: finaliza a execução do GET.                                ║
        /// ║    • Consumidor: pipeline Razor Pages.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _signInManager.SignOutAsync() → encerra sessão.                         ║
        /// ║    • _logger.LogInformation() → registra evento.                             ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/Logout).                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Logout.cshtml                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task OnGet()
        {
            try
            {
                // [REGRA] Realizar logout
                await _signInManager.SignOutAsync();

                _logger.LogInformation("User logged out.");
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Logout.cshtml.cs", "OnGet", error);
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnPost                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Realiza logout via POST e redireciona para a URL informada.               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante saída segura e redirecionamento controlado.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno após logout.                         ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: redirecionamento ou reexibição da página.                ║
        /// ║    • Significado: encerra sessão e mantém fluxo de navegação.                ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _signInManager.SignOutAsync() → encerra sessão.                         ║
        /// ║    • _logger.LogInformation() → registra evento.                             ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (POST /Identity/Account/Logout).                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Logout.cshtml                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            try
            {
                // [REGRA] Realizar logout seguro
                await _signInManager.SignOutAsync();

                _logger.LogInformation("User logged out.");

                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    // > 🧠 **Lógica:** Se não houver URL de retorno, volta para a página atual (agora deslogado)
                    return Page();
                }
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("Logout.cshtml.cs", "OnPost", error);
                return RedirectToPage(); // Tenta recarregar em caso de erro
            }
        }
    }
}


