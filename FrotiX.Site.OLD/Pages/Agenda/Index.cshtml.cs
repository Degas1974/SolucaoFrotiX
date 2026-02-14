/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está completamente documentado em:                         ║
 * ║  📄 Documentacao/Pages/Agenda - Index.md                                 ║
 * ║                                                                          ║
 * ║  Última atualização: 08/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using FrotiX.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace FrotiX.Pages.Agenda
{
    public class IndexModel :PageModel
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
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "Initialize" , error);
            }
        }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "IndexModel" , error);
            }
        }

        public void OnGet()
        {
            try
            {
                // Inicializar dados para os controles da página
                ViewData["dataCombustivel"] = new Helpers.ListaNivelCombustivel(_unitOfWork).NivelCombustivelList();
                ViewData["dataMotorista"] = new Helpers.ListaMotorista(_unitOfWork).MotoristaList();
                ViewData["dataVeiculo"] = new Helpers.ListaVeiculos(_unitOfWork).VeiculosList();
                ViewData["dataSetor"] = new Helpers.ListaSetores(_unitOfWork).SetoresList();
                ViewData["dataRequisitante"] = new Helpers.ListaRequisitante(_unitOfWork).RequisitantesList();
                ViewData["dataFinalidade"] = new Helpers.ListaFinalidade(_unitOfWork).FinalidadesList();
                ViewData["dataEvento"] = new Helpers.ListaEvento(_unitOfWork).EventosList();
                ViewData["dataSetorEvento"] = new Helpers.ListaSetoresEvento(_unitOfWork).SetoresEventoList();
                ViewData["dataPeriodo"] = new Helpers.ListaPeriodos().PeriodosList();
                ViewData["dataRecorrente"] = new Helpers.ListaRecorrente().RecorrenteList();

                var listaOrigem = _unitOfWork.Viagem.GetAllReduced(selector: v => v.Origem)
                    .Where(o => o != null)
                    .Distinct()
                    .OrderBy(o => o)
                    .ToList();
                ViewData["ListaOrigem"] = listaOrigem;

                var listaDestino = _unitOfWork.Viagem.GetAllReduced(selector: v => v.Destino)
                    .Where(d => d != null)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();
                ViewData["ListaDestino"] = listaDestino;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "OnGet" , error);
            }
        }
    }
}
