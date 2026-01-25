/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** Usuarios.cshtml.cs
 > 📍 **Local:** Areas/Authorization/Pages
 > ❓ **Por que existo?** Gerencia a exibição da lista de Usuários do sistema.
 > 🔗 **Relevância:** Alta (Segurança/Administração)
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
    public class UsuariosModel : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * **Injeção de Dependência:** `_log`

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Rotas de API:** GET /Authorization/Usuarios
        */
        private readonly ILogService _log;

        public UsuariosModel(ILogService log)
        {
            _log = log;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGet                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa a página de listagem de Usuários (Versão PT-BR).               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Interface alternativa para gestão de usuários (verificar obsolescência).  ║
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
        /// ║    • Roteamento ASP.NET Core (/Authorization/Usuarios)                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Usuarios.cshtml, usuarios.js                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void OnGet()
        {
            try
            {
                _log.Info("Acesso à página de Gerenciamento de Usuários");
                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Inicialização
                // Carrega dados iniciais se necessário (atualmente vazio)
                // ═══════════════════════════════════════════════════════════════
            }
            catch (Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                _log.Error("Erro ao carregar Gerenciamento de Usuários", error);
                Alerta.TratamentoErroComLinha("Usuarios.cshtml.cs", "OnGet", error);
            }
        }
    }
}
