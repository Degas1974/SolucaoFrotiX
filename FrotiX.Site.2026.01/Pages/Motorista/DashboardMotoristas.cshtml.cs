/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DashboardMotoristas.cshtml.cs                                                                         ║
 * ║ PageModel para Dashboard de Motoristas - métricas e indicadores de motoristas                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    DashboardMotoristas.cshtml.cs                                                             ║
 * ║ @local      Pages/Motorista/                                                                          ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Dashboard com KPIs de motoristas e lista para seleção                                    ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using FrotiX.Helpers;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Motorista
{
    public class DashboardMotoristasModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardMotoristasModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            ViewData["lstMotoristas"] = new ListaMotorista(_unitOfWork).MotoristaList();
        }
    }
}
