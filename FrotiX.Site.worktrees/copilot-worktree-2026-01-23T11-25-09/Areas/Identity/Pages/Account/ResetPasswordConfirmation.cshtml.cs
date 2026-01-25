using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    /* > ---------------------------------------------------------------------------------------
     > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
     > ---------------------------------------------------------------------------------------
     > 🆔 **Nome:** ResetPasswordConfirmation.cshtml.cs
     > 📍 **Local:** Areas/Identity/Pages/Account
     > ❓ **Por que existo?** Confirma ao usuário que a senha foi redefinida com sucesso.
     > 🔗 **Relevância:** Baixa (Página Informativa)
     > --------------------------------------------------------------------------------------- */

    public class ResetPasswordConfirmationModel : PageModel
    {
        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza a view de confirmação de redefinição de senha.                  ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Informa o usuário sobre a conclusão do fluxo.                             ║
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
        /// ║    • Roteamento Razor Pages (GET /Identity/Account/ResetPasswordConfirmation).║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ResetPasswordConfirmation.cshtml                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public void OnGet()
        {
            try
            {
                // Método simples de exibição, mas mantendo padrão try-catch
            }
            catch (System.Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                FrotiX.Helpers.Alerta.TratamentoErroComLinha("ResetPasswordConfirmation.cshtml.cs", "OnGet", error);
            }
        }
    }
}


