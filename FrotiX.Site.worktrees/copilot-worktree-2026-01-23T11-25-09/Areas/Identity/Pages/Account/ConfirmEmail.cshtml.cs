/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** ConfirmEmail.cshtml.cs
 > 📍 **Local:** Areas/Identity/Pages/Account
 > ❓ **Por que existo?** Processa a confirmação de e-mail do usuário.
 > 🔗 **Relevância:** Alta (Segurança/Cadastro)
 > ---------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using FrotiX.Helpers;

namespace FrotiX.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * **Injeção de Dependência:** `_userManager`
        * **Identity:** `ConfirmEmailAsync`, `FindByIdAsync`
        * **Utils:** `WebEncoders`, `Encoding`

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Rotas de API:** GET /Identity/Account/ConfirmEmail
        */
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ConfirmEmailModel                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o PageModel responsável pela confirmação de e-mail do usuário.║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante acesso ao Identity para validar e concluir o fluxo de confirmação║
        /// ║    de e-mail com segurança.                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • userManager (UserManager<IdentityUser>): gerenciador de usuários.       ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: habilita operações do Identity neste contexto.             ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependência).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ConfirmEmail.cshtml                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGetAsync                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa a confirmação de e-mail a partir dos parâmetros da URL.          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Conclui o fluxo de verificação de conta exigido pelo Identity.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • userId (string): identificador do usuário a confirmar.                  ║
        /// ║    • code (string): token de confirmação gerado pelo Identity.               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: página de status ou redirecionamento.                    ║
        /// ║    • Significado: indica sucesso/erro na confirmação.                        ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _userManager.FindByIdAsync() → busca usuário.                           ║
        /// ║    • _userManager.ConfirmEmailAsync() → confirma e-mail.                      ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/ConfirmEmail).            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ConfirmEmail.cshtml                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            // [REGRA] Validar parâmetros obrigatórios
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            // [DADOS] Buscar usuário
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            // [LOGICA] Decodificar e confirmar
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            // [UI] Definir mensagem de status
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}


