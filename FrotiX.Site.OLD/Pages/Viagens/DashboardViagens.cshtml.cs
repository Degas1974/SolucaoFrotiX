/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: DashboardViagens.cshtml.cs (Pages/Viagens)                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para dashboard de viagens. Carrega múltiplas listas para dropdowns de filtro:                 ║
 * ║ motoristas, veículos, setores, requisitantes, finalidades e eventos.                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Carrega todas as listas para dropdown via ViewData                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VIEWDATA                                                                                                  ║
 * ║ • lstMotorista : Lista de motoristas via ListaMotorista helper                                          ║
 * ║ • lstVeiculos : Lista de veículos via ListaVeiculos helper                                              ║
 * ║ • lstSetor : Lista de setores via ListaSetores helper                                                   ║
 * ║ • lstRequisitante : Lista de requisitantes via ListaRequisitante helper                                 ║
 * ║ • lstFinalidade : Lista de finalidades via ListaFinalidade helper                                       ║
 * ║ • lstEvento : Lista de eventos via ListaEvento helper                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • FrotiX.Helpers.* - Helpers para dropdown (ListaMotorista, ListaVeiculos, etc.)                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
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
