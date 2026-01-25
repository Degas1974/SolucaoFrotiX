/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** Roles.cshtml.cs
 > 📍 **Local:** Areas/Authorization/Pages
 > ❓ **Por que existo?** Gerencia a exibição e lógica da página de Roles (Perfis de Acesso).
 > 🔗 **Relevância:** Alta (Segurança/Controle de Acesso)
 > ---------------------------------------------------------------------------------------
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrotiX.Helpers;
using FrotiX.Services;
using System;

namespace FrotiX.Areas.Authorization.Pages
{
    [Authorize]
    public class RoleModel : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * **Injeção de Dependência:** `_log`

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Rotas de API:** GET /Authorization/Roles
        */
        private readonly ILogService _log;

        public RoleModel(ILogService log)
        {
            _log = log;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa a página de gerenciamento de Perfis de Acesso (Roles).         ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante que a view seja renderizada corretamente para o usuário auth.     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: Renderiza a View associada (@page).                               ║
        /// ║    • Significado: Fluxo padrão de Razor Pages.                               ║
        /// ║    • Consumidor: Sistema de Roteamento ASP.NET Core.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma lógica complexa nesta Action.                                   ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento ASP.NET Core (/Authorization/Roles)                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Roles.cshtml                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void OnGet()
        {
            try
            {
                _log.Info("Acesso à página de Gerenciamento de Perfis (Roles)");
                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Inicialização
                // Carrega dados iniciais se necessário (atualmente vazio)
                // ═══════════════════════════════════════════════════════════════
            }
            catch (Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                _log.Error("Erro ao carregar Gerenciamento de Perfis (Roles)", error);
                Alerta.TratamentoErroComLinha("Roles.cshtml.cs", "OnGet", error);
            }
        }
    }
}
