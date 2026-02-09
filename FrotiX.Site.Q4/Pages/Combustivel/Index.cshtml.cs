/* ****************************************************************************************
 * ⚡ ARQUIVO: Pages/Combustivel/Index.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel (code-behind) da página de listagem de Tipos de Combustível.
 *                   Inicializa a página Index.cshtml sem lógica específica (lógica no frontend).
 * 📥 ENTRADAS     : GET request para rota /Combustivel, método OnGet() chamado pelo ASP.NET Core
 * 📤 SAÍDAS       : Renderização de Index.cshtml, sem ViewData ou Model específico (PageModel vazio)
 * 🔗 CHAMADA POR  : ASP.NET Core Razor Pages pipeline ao acessar /Combustivel, Index.cshtml
 * 🔄 CHAMA        : Alerta.TratamentoErroComLinha (tratamento de erros global - fail-safe)
 * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Mvc.RazorPages (PageModel base class), Alerta.cs (helper)
 * 📝 OBSERVAÇÕES  : PageModel minimalista - toda lógica CRUD está no JavaScript (combustivel.js)
 *                   e Controller (/api/Combustivel). OnGet() vazio com try-catch preventivo.
 *                   31 linhas apenas. Padrão comum em FrotiX para páginas simples de listagem.
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Combustivel
{
    public class IndexModel :PageModel
    {
        public void OnGet()
        {
            try
            {

            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }
}
