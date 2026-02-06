/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ExibePDFPenalidade.cshtml.cs (Pages/Multa)                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para exibição do PDF da notificação de penalidade de multa de trânsito.                       ║
 * ║ Carrega a multa pelo ID e disponibiliza para visualização do documento de penalidade.                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES STATIC                                                                                       ║
 * ║ • multaId : Guid - ID da multa sendo visualizada                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • MultaObj : MultaViewModel - Contém entidade Multa                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega multa pelo ID para exibição do PDF da penalidade                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VALIDAÇÕES                                                                                                ║
 * ║ • ID vazio: redireciona para ListaPenalidade com mensagem de erro                                       ║
 * ║ • Multa não encontrada: redireciona para ListaPenalidade com mensagem                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • INotyfService - Notificações toast                                                                    ║
 * ║ • IWebHostEnvironment - Acesso a wwwroot                                                                ║
 * ║ • AppToast - Notificações visuais                                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Multa
{
    public class ExibePDFPenalidadeModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public static Guid multaId;

        public ExibePDFPenalidadeModel(
            IUnitOfWork unitOfWork ,
            INotyfService notyf ,
            IWebHostEnvironment hostingEnvironment
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _notyf = notyf;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ExibePDFPenalidade.cshtml.cs" , "ExibePDFPenalidadeModel" , error);
            }
        }

        [BindProperty]
        public MultaViewModel MultaObj
        {
            get; set;
        }

        private void SetViewModel()
        {
            try
            {
                MultaObj = new MultaViewModel { Multa = new Models.Multa() };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ExibePDFPenalidade.cshtml.cs" , "SetViewModel" , error);
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
                    multaId = id;
                    MultaObj.Multa = _unitOfWork.Multa.GetFirstOrDefault(m => m.MultaId == id);

                    if (MultaObj.Multa == null)
                    {
                        AppToast.show("Vermelho" , "Multa não encontrada." , 3000);
                        return RedirectToPage("./ListaPenalidade");
                    }
                }
                else
                {
                    AppToast.show("Vermelho" , "ID da multa inválido." , 3000);
                    return RedirectToPage("./ListaPenalidade");
                }

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ExibePDFPenalidade.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }
    }
}
