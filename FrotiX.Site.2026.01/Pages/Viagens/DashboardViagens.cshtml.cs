/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DashboardViagens.cshtml.cs                                                                          ║
 * ║ PageModel para dashboard e visualizações                                                            ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    DashboardViagens.cshtml.cs                                                              ║
 * ║ @local      Pages/Viagens/                                                                                ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Gestão de viagens                                                                       ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using FrotiX.Helpers;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Viagens
{
    public class DashboardViagensModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardViagensModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
            ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
            ViewData["lstSetor"] = new ListaSetores(_unitOfWork).SetoresList();
            ViewData["lstRequisitante"] = new ListaRequisitante(_unitOfWork).RequisitantesList();
            ViewData["lstFinalidade"] = new ListaFinalidade(_unitOfWork).FinalidadesList();
            ViewData["lstEvento"] = new ListaEvento(_unitOfWork).EventosList();
        }
    }
}

