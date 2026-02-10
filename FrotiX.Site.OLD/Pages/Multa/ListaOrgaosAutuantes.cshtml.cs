/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ListaOrgaosAutuantes.cshtml.cs (Pages/Multa)                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para listagem de Órgãos Autuantes (entidades que aplicam multas de trânsito:                  ║
 * ║ DETRAN, PRF, PM, DER, etc.). Listagem é carregada via JavaScript/Grid.                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - dados carregados via AJAX                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Multa
{
    public class ListaOrgaosAutuantesModel :PageModel
    {
        public void OnGet()
        {
            try
            {
                // Método vazio - lógica no JavaScript
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaOrgaosAutuantes.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }
}
