/* ****************************************************************************************
 * ⚡ ARQUIVO: Pages/Usuarios/Index.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para gestão completa de usuários (CRUD) com DataTable
 *                   server-side, sistema crítico de controle de acesso do FrotiX
 *
 * 📥 ENTRADAS     : Query parameters filtros (textBusca, apenasMotoristasAtivos,
 *                   perfilFiltro), requisições AJAX DataTable via GET
 *
 * 📤 SAÍDAS       : Page renderizada com DataTable populado via API, modais
 *                   para novo/editar usuário, toast notificações
 *
 * 🔗 CHAMADA POR  : GET /Usuarios/Index (navegação menu Admin > Usuários)
 *
 * 🔄 CHAMA        : Nenhuma operação crítica neste PageModel (dados via API)
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core PageModel, System.Net.Http
 *
 * 📝 OBSERVAÇÕES  : PageModel minimalista - operações CRUD delegadas para
 *                   Controllers API (API/UsuariosController.cs).
 *                   Carregamento de dados: GET /api/Usuarios/GetAll (server-side)
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        /************************************************************************************
         * ⚡ FUNÇÃO: OnGet
         * ------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar requisição GET inicial da página de usuários
         *
         * 📥 ENTRADAS     : Nenhuma (parâmetros vem via rota /Usuarios/Index)
         *
         * 📤 SAÍDAS       : void (renderiza Index.cshtml com PageModel vazio)
         *
         * ⬅️ CHAMADO POR  : ASP.NET Core runtime ao acessar rota /Usuarios/Index
         *
         * ➡️ CHAMA        : Nenhuma operação (UI carrega dados via JavaScript/AJAX)
         *
         * 📝 OBSERVAÇÕES  : Função minimalista - lógica pura delegada para JavaScript.
         *                   Dados carregados assincronamente via carregarGrid() em
         *                   usuarios-index.js após DOM ready
         ************************************************************************************/
        public void OnGet()
        {
            try
            {
                // [UI] Página carrega dados via API/DataTables após renderização
                // Nenhuma lógica necessária no servidor (padrão SPA-lite do FrotiX)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs", "OnGet", error);
            }
        }
    }
}
