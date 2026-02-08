/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: DashboardMotoristas.cshtml.cs (Pages/Motorista)                                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para dashboard de motoristas. Exibe métricas e estatísticas relacionadas aos                  ║
 * ║ motoristas da frota (disponibilidade, viagens, CNH, etc.).                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Carrega lista de motoristas para dropdown de filtro                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VIEWDATA                                                                                                  ║
 * ║ • lstMotoristas : Lista de motoristas via ListaMotorista helper                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • FrotiX.Helpers.ListaMotorista - Helper para dropdown                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
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
