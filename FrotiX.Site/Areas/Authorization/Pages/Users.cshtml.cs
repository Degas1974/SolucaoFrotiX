using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Areas.Authorization.Pages
{
    /****************************************************************************************
     * ⚡ CLASSE: UserModel (PageModel para Gerenciamento de Usuários)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Servir como PageModel para a página Users.cshtml, que gerencia
     *                   os usuários do sistema ASP.NET Identity (CRUD de usuários com
     *                   controle de email, telefone, confirmação e bloqueio).
     *                   A lógica de CRUD está implementada via API no endpoint /api/users
     *                   e a página consome via AJAX (DataTable editável).
     * 📥 ENTRADAS     : Nenhuma (classe vazia, toda lógica no frontend/API).
     * 📤 SAÍDAS       : Renderização da página Users.cshtml com autorização obrigatória.
     * 🔗 CHAMADA POR  : ASP.NET Core Razor Pages engine quando rota /Authorization/Users é acessada.
     * 🔄 CHAMA        : Nenhuma função (PageModel básico sem lógica).
     * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Authorization, ASP.NET Core Identity.
     ****************************************************************************************/
    [Authorize] // [DOC] Restringe acesso à página apenas para usuários autenticados
    public class UserModel : PageModel
    {
        // [DOC] PageModel vazio - toda lógica de negócio está no endpoint /api/users
        // [DOC] e no frontend (Users.cshtml com DataTable + AJAX)
    }
}
