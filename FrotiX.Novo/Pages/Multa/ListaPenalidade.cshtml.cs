/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ListaPenalidade.cshtml.cs (Pages/Multa)                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para listagem de multas na fase de Penalidade. Esta é a segunda fase do ciclo de vida          ║
 * ║ de uma multa de trânsito, após a autuação ser confirmada e a penalidade aplicada.                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES ESTÁTICAS                                                                                    ║
 * ║ • _unitOfWork : Referência estática ao repositório (padrão do projeto)                                   ║
 * ║ • PDFAutuacao, PDFNotificacao : Arrays de bytes para armazenar PDFs                                      ║
 * ║ • MultaId : ID da multa atual (Guid)                                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • MultaObj : MultaViewModel para binding com a view                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • Initialize(unitOfWork) : Método estático para inicialização do repositório                             ║
 * ║ • OnGet() : Handler vazio - lógica está no JavaScript/Grid                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ FLUXO DE MULTAS                                                                                           ║
 * ║ • Autuação (ListaAutuacao) → Penalidade (este arquivo)                                                   ║
 * ║ • Status na Penalidade: À Pagar, Paga, Paga (Defin), Paga (Infrator), Enviada Secle, Arquivada           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • MultaViewModel - ViewModel de multas                                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
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
