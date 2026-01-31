/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Usuarios.cshtml.cs                                                                      ║
   ║ 📂 CAMINHO: /Areas/Authorization/Pages                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: PageModel para a página de gerenciamento de usuários (Usuarios.cshtml).               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: UsuariosModel                                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
