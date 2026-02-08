/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Pendencias.cshtml.cs                                                                                 ║
 * ║ PageModel para gestão de abastecimentos pendentes de validação                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    Pendencias.cshtml.cs                                                                     ║
 * ║ @local      Pages/Abastecimento/                                                                     ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Lista e gestão de cupons pendentes de aprovação                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Abastecimento
{
    public class PendenciasModel : PageModel
    {
        public static IUnitOfWork _unitOfWork;

        public static void Initialize(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Pendencias.cshtml.cs", "Initialize", error);
                return;
            }
        }

        public PendenciasModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Pendencias.cshtml.cs", "PendenciasModel", error);
            }
        }

        public void OnGet()
        {
            try
            {
                Initialize(_unitOfWork);
                ViewData["lstVeiculos"] = new FrotiX.Pages.Abastecimento.ListaVeiculos(_unitOfWork).VeiculosList();
                ViewData["lstCombustivel"] = new FrotiX.Pages.Abastecimento.ListaCombustivel(_unitOfWork).CombustivelList();
                ViewData["lstMotorista"] = new FrotiX.Pages.Abastecimento.ListaMotorista(_unitOfWork).MotoristaList();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Pendencias.cshtml.cs", "OnGet", error);
                return;
            }
        }
    }
}
