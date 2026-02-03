/* ****************************************************************************************
 * ⚡ ARQUIVO: Pages/MarcaVeiculo/Index.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel (code-behind) da página de listagem de Marcas de Veículos.
 *                   Inicializa a página Index.cshtml sem lógica específica (lógica no frontend).
 * 📥 ENTRADAS     : GET request para rota /MarcaVeiculo, método OnGet() chamado pelo ASP.NET Core
 * 📤 SAÍDAS       : Renderização de Index.cshtml, sem ViewData ou Model específico (PageModel vazio)
 * 🔗 CHAMADA POR  : ASP.NET Core Razor Pages pipeline ao acessar /MarcaVeiculo, Index.cshtml
 * 🔄 CHAMA        : Alerta.TratamentoErroComLinha (tratamento de erros global - fail-safe)
 * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Mvc.RazorPages (PageModel base class), Alerta.cs (helper)
 * 📝 OBSERVAÇÕES  : PageModel minimalista - toda lógica CRUD está no JavaScript (marcaveiculo.js)
 *                   e Controller (/api/MarcaVeiculo). OnGet() vazio com try-catch preventivo.
 *                   31 linhas apenas. Padrão comum em FrotiX para páginas simples de listagem.
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.MarcaVeiculo
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
