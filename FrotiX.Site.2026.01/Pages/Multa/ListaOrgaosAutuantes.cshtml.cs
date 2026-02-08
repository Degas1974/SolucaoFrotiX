/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ListaOrgaosAutuantes.cshtml.cs                                                                        ║
 * ║ PageModel para listagem de órgãos autuantes                                                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    ListaOrgaosAutuantes.cshtml.cs                                                            ║
 * ║ @local      Pages/Multa/                                                                              ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Grid de órgãos autuantes (DETRAN, PRF, etc)                                              ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
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
