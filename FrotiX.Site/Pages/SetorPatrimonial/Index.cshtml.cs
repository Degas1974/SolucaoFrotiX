/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: Index.cshtml.cs (Pages/SetorPatrimonial)                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel mínimo para listagem de Setores Patrimoniais (responsáveis pelos bens patrimoniais).           ║
 * ║ Dados são carregados via AJAX no SetorController.                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Método vazio (dados carregados via AJAX)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ NOTA                                                                                                     ║
 * ║ Namespace incorreto (Pages.SecaoPatrimonial em vez de Pages.SetorPatrimonial) - manter para compatib.    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.SecaoPatrimonial
{
    public class IndexModel : PageModel
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
