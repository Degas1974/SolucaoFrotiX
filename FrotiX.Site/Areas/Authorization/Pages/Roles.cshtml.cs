using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Areas.Authorization.Pages
{
    /****************************************************************************************
     * ⚡ CLASSE: RoleModel (PageModel para Gerenciamento de Roles)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Servir como PageModel para a página Roles.cshtml, que gerencia
     *                   os perfis de acesso (Roles) do sistema ASP.NET Identity.
     *                   A lógica de CRUD está implementada via API no endpoint /api/roles
     *                   e a página consome via AJAX (DataTable editável).
     * 📥 ENTRADAS     : Nenhuma (classe vazia, toda lógica no frontend/API).
     * 📤 SAÍDAS       : Renderização da página Roles.cshtml com autorização obrigatória.
     * 🔗 CHAMADA POR  : ASP.NET Core Razor Pages engine quando rota /Authorization/Roles é acessada.
     * 🔄 CHAMA        : Nenhuma função (PageModel básico sem lógica).
     * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Authorization, ASP.NET Core Identity.
     ****************************************************************************************/
    [Authorize] // [DOC] Restringe acesso à página apenas para usuários autenticados
    public class RoleModel : PageModel
    {
        // [DOC] PageModel vazio - toda lógica de negócio está no endpoint /api/roles
        // [DOC] e no frontend (Roles.cshtml com DataTable + AJAX)
    }
}
