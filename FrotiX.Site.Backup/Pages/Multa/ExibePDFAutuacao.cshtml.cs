/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ExibePDFAutuacao.cshtml.cs (Pages/Multa)                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para exibição de PDF de Autuação de multas. Carrega a multa pelo ID                           ║
 * ║ e exibe o documento PDF associado (notificação de autuação de trânsito).                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES ESTÁTICAS                                                                                    ║
 * ║ • multaId : ID da multa atual                                                                            ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • MultaObj : MultaViewModel - Contém a entidade Multa                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega multa pelo ID e valida existência                                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VALIDAÇÕES                                                                                                ║
 * ║ • ID vazio ou inválido: exibe AppToast vermelho e redireciona para ListaAutuacao                        ║
 * ║ • Multa não encontrada: idem                                                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • INotyfService - Notificações                                                                           ║
 * ║ • AppToast - Feedback visual                                                                             ║
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
    public class ExibePDFAutuacaoModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public static Guid multaId;

        public ExibePDFAutuacaoModel(
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
                Alerta.TratamentoErroComLinha("ExibePDFAutuacao.cshtml.cs" , "ExibePDFAutuacaoModel" , error);
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
                Alerta.TratamentoErroComLinha("ExibePDFAutuacao.cshtml.cs" , "SetViewModel" , error);
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
                        return RedirectToPage("./ListaAutuacao");
                    }
                }
                else
                {
                    AppToast.show("Vermelho" , "ID da multa inválido." , 3000);
                    return RedirectToPage("./ListaAutuacao");
                }

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ExibePDFAutuacao.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }
    }
}
