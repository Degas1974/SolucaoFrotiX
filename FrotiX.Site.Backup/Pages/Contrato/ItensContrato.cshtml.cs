/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ItensContrato.cshtml.cs                                                                              ║
 * ║ PageModel para gestão de itens vinculados a contratos                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    ItensContrato.cshtml.cs                                                                  ║
 * ║ @local      Pages/Contrato/                                                                          ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Gestão de itens contratuais e preços unitários                                           ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Contrato
{
    public class ItensContratoModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;

        public ItensContratoModel(IUnitOfWork unitOfWork, INotyfService notyf)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _notyf = notyf;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContrato.cshtml.cs", "ItensContratoModel", error);
            }
        }

        [BindProperty]
        public ICPageViewModel ItensContratoObj { get; set; } = default!;

        public void OnGet()
        {
            try
            {
                ItensContratoObj = new ICPageViewModel
                {
                    ItensContrato = new ICPlaceholder()
                };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContrato.cshtml.cs", "OnGet", error);
            }
        }
    }
}
