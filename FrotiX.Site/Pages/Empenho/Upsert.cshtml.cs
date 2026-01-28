// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Upsert.cshtml.cs (Empenho)                                         ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para criação/edição de empenhos orçamentários.                    ║
// ║ Vincula empenhos a contratos ou atas de registro de preços.                 ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • Injeção de IUnitOfWork, ILogger, IWebHostEnvironment, INotyfService       ║
// ║ • [BindProperty] EmpenhoObj - EmpenhoViewModel                              ║
// ║ • SetViewModel - Inicializa com dropdowns (ContratoList, AtaList)           ║
// ║                                                                              ║
// ║ HANDLERS:                                                                     ║
// ║ • OnGet(id) - Carrega empenho existente ou novo                             ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace FrotiX.Pages.Empenho
{
    public class UpsertModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly INotyfService _notyf;

        public static Guid empenhoId;

        public UpsertModel(
            IUnitOfWork unitOfWork ,
            ILogger<IndexModel> logger ,
            IWebHostEnvironment hostingEnvironment ,
            INotyfService notyf
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _notyf = notyf;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "UpsertModel" , error);
            }
        }

        [BindProperty]
        public EmpenhoViewModel EmpenhoObj
        {
            get; set;
        }

        private void SetViewModel()
        {
            try
            {
                EmpenhoObj = new EmpenhoViewModel
                {
                    ContratoList = _unitOfWork.Contrato.GetDropDown("") ,
                    AtaList = _unitOfWork.AtaRegistroPrecos.GetAtaListForDropDown(1) ,
                    Empenho = new Models.Empenho() ,
                };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "SetViewModel" , error);
                return;
            }
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                SetViewModel();

                if (id != Guid.Empty)
                {
                    EmpenhoObj.Empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                        u.EmpenhoId == id
                    );
                    if (EmpenhoObj == null)
                    {
                        return NotFound();
                    }
                }
                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }
    }
}
