/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ItensContrato.cshtml.cs (Pages/Contrato)                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para gestão de Itens Contratuais (veículos previstos no contrato de locação).                  ║
 * ║ Permite adicionar/editar tipos de veículos e quantidades por contrato.                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ BIND PROPERTIES                                                                                          ║
 * ║ • ItensContratoObj : ICPageViewModel - ViewModel com ICPlaceholder                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Inicializa ViewModel com placeholder vazio                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ OBSERVAÇÃO                                                                                               ║
 * ║ A lógica de CRUD é feita via AJAX no Controller - este PageModel apenas prepara visualização             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                             ║
 * ║ • IUnitOfWork (injetado para Controller)                                                                 ║
 * ║ • INotyfService - Notificações toast                                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
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
