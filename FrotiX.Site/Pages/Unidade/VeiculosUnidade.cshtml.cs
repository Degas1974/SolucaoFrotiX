/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: VeiculosUnidade.cshtml.cs (Pages/Unidade)                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para visualização dos veículos vinculados a uma unidade específica.                            ║
 * ║ Exibe grid de veículos filtrado pela unidade selecionada.                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES ESTÁTICAS                                                                                   ║
 * ║ • unidadeId : Guid - ID da unidade para filtrar veículos (usado pelo Controller)                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ BIND PROPERTIES                                                                                          ║
 * ║ • UnidadeObj : Unidade - Entidade da unidade selecionada                                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id)      : Carrega unidade e armazena ID estático para filtro                                    ║
 * ║ • OnPostSubmit() : Método placeholder (retorna Page)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                             ║
 * ║ • IUnitOfWork (Unidade)                                                                                  ║
 * ║ • INotyfService - Notificações toast                                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace FrotiX.Pages.Unidade
{
    public class VeiculosUnidadeModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IndexModel> _logger;
        private readonly INotyfService _notyf;

        public VeiculosUnidadeModel(IUnitOfWork unitOfWork , ILogger<IndexModel> logger , INotyfService notyf)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _notyf = notyf;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculosUnidade.cshtml.cs" , "VeiculosUnidadeModel" , error);
            }
        }

        public static Guid unidadeId;

        [BindProperty]
        public Models.Unidade UnidadeObj
        {
            get; set;
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                UnidadeObj = new Models.Unidade();
                if (id != Guid.Empty)
                {
                    UnidadeObj = _unitOfWork.Unidade.GetFirstOrDefault(u => u.UnidadeId == id);
                    if (UnidadeObj == null)
                    {
                        return NotFound();
                    }
                }
                unidadeId = UnidadeObj.UnidadeId;
                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculosUnidade.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }

        public IActionResult OnPostSubmit()
        {
            try
            {
                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculosUnidade.cshtml.cs" , "OnPostSubmit" , error);
                return Page();
            }
        }
    }
}
