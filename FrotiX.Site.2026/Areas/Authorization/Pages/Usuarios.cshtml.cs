/* ****************************************************************************************
 * ⚡ ARQUIVO: Usuarios.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel da página Usuarios. Controla apenas autorização e
 *                   renderização; a lógica de grid fica em usuarios.js.
 *
 * 📥 ENTRADAS     : Nenhuma (GET da página).
 *
 * 📤 SAÍDAS       : Renderização da página Usuarios.cshtml.
 *
 * 🔗 CHAMADA POR  : Motor Razor ao acessar /Authorization/Usuarios.
 *
 * 🔄 CHAMA        : Nenhum método interno (PageModel sem handlers).
 *
 * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Authorization, Razor Pages.
 *
 * 📝 OBSERVAÇÕES  : CRUD e comportamento do grid são definidos no arquivo usuarios.js.
 **************************************************************************************** */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Areas.Authorization.Pages
{
    /****************************************************************************************
     * ⚡ CLASSE: UsuariosModel (PageModel para Gerenciamento de Usuários)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Servir como PageModel para a página Usuarios.cshtml, que exibe
     *                   interface de gerenciamento de usuários com DataTable.
     *                   A lógica de negócio está implementada no arquivo usuarios.js
     *                   (frontend) e nos endpoints de API correspondentes.
     * 📥 ENTRADAS     : Nenhuma (classe vazia, toda lógica no frontend/API).
     * 📤 SAÍDAS       : Renderização da página Usuarios.cshtml com autorização obrigatória.
     * 🔗 CHAMADA POR  : ASP.NET Core Razor Pages engine quando rota /Authorization/Usuarios é acessada.
     * 🔄 CHAMA        : Nenhuma função (PageModel básico sem lógica).
     * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Authorization.
     ****************************************************************************************/
    [Authorize] // [DOC] Restringe acesso à página apenas para usuários autenticados
    public class UsuariosModel : PageModel
    {
        // [DOC] PageModel vazio - toda lógica de negócio está no arquivo usuarios.js
        // [DOC] e nos endpoints de API consumidos via AJAX
    }
}
