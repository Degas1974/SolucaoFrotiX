/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ListaTiposMulta.cshtml.cs (Pages/Multa)                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para listagem de tipos de multa (infrações de trânsito com artigo, código DENATRAN,           ║
 * ║ desdobramento e descrição). Dados carregados via DataTables AJAX.                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - dados carregados via JavaScript/AJAX                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Multa
{
    public class ListaTiposMultaModel : PageModel
    {
        public void OnGet()
        {
            try
            {
                // Página apenas exibe lista via DataTables AJAX
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaTiposMulta.cshtml.cs", "OnGet", error);
            }
        }
    }
}
