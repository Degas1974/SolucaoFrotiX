/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ExibePDFComprovante.cshtml.cs (Pages/Multa)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para exibição do PDF do comprovante de pagamento de multa de trânsito.                        ║
 * ║ Carrega a multa pelo ID e disponibiliza para visualização do comprovante.                               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES STATIC                                                                                       ║
 * ║ • multaId : Guid - ID da multa sendo visualizada                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • MultaObj : MultaViewModel - Contém entidade Multa                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega multa pelo ID para exibição do comprovante                                        ║
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
    public class ExibePDFComprovanteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public static Guid multaId;

        public ExibePDFComprovanteModel(
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
                Alerta.TratamentoErroComLinha("ExibePDFComprovante.cshtml.cs" , "ExibePDFComprovanteModel" , error);
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
                Alerta.TratamentoErroComLinha("ExibePDFComprovante.cshtml.cs" , "SetViewModel" , error);
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
                Alerta.TratamentoErroComLinha("ExibePDFComprovante.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }
    }
}
