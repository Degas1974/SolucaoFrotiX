/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Index.cshtml.cs                                                                                       ║
 * ║ PageModel para listagem de Empenhos - exibe grid com todos os empenhos cadastrados                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    Index.cshtml.cs                                                                           ║
 * ║ @local      Pages/Empenho/                                                                            ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Listagem de empenhos do sistema de gestão de frotas                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Empenho
{
    public class IndexModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "IndexModel" , error);
            }
        }

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
