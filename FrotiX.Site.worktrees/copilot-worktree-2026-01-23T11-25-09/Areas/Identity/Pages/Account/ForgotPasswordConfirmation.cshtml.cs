/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** ForgotPasswordConfirmation.cshtml.cs
 > 📍 **Local:** Areas/Identity/Pages/Account
 > ❓ **Por que existo?** Exibe confirmação de envio de e-mail de recuperação.
 > 🔗 **Relevância:** Média (Informativo)
 > ---------------------------------------------------------------------------------------
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrotiX.Helpers;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * Nenhuma dependência externa.

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Rotas de API:** GET /Identity/Account/ForgotPasswordConfirmation
        */
        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe a confirmação de que o e-mail de recuperação foi enviado.           ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Informa o usuário sem expor detalhes sensíveis.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: renderiza a página de confirmação.                                ║
        /// ║    • Significado: feedback visual do fluxo.                                  ║
        /// ║    • Consumidor: pipeline Razor Pages.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas renderização da view).                                  ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/ForgotPasswordConfirmation).║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ForgotPasswordConfirmation.cshtml                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        // > ⚙️ **OnGet:** Exibe a página.
        public void OnGet()
        {
            try
            {
                // > 🧠 **Lógica:** Apenas exibe a View estática.
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                Alerta.TratamentoErroComLinha("ForgotPasswordConfirmation.cshtml.cs", "OnGet", error);
            }
        }
    }
}


