/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ListaPenalidade.cshtml.cs                                                                             ║
 * ║ PageModel para listagem de penalidades                                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    ListaPenalidade.cshtml.cs                                                                 ║
 * ║ @local      Pages/Multa/                                                                              ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Grid de penalidades para gerenciamento de multas                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Multa
{
    public class ListaPenalidade :PageModel
    {
        public static IUnitOfWork _unitOfWork;
        public static byte[] PDFAutuacao;
        public static byte[] PDFNotificacao;
        public static Guid MultaId;

        [BindProperty]
        public Models.MultaViewModel MultaObj
        {
            get; set;
        }

        public static void Initialize(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml.cs" , "Initialize" , error);
                return;
            }
        }

        public ListaPenalidade(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml.cs" , "ListaPenalidade" , error);
            }
        }

        public void OnGet()
        {
            try
            {
                // Método vazio - lógica está no Initialize
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }
}
