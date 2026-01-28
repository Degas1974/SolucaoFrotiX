/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: Index.cshtml.cs (Pages/MovimentacaoPatrimonio)                                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel mínimo para listagem de Movimentações de Patrimônio (transferências entre setores/seções).     ║
 * ║ Dados são carregados via AJAX no MovimentacaoPatrimonioController.                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Método vazio (dados carregados via AJAX)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ OBSERVAÇÃO                                                                                               ║
 * ║ O grid usa Syncfusion EJ2 Grid com dados via MovimentacaoPatrimonioController.GetAll()                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.MovimentacaoPatrimonio
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
