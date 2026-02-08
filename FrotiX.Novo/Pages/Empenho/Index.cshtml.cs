// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Index.cshtml.cs (Empenho)                                          ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para listagem de empenhos orçamentários.                          ║
// ║ Controla saldos disponíveis para despesas com a frota.                      ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • Injeção de IUnitOfWork                                                    ║
// ║ • OnGet vazio - dados carregados via AJAX                                   ║
// ║ • Grid com EmpenhoController endpoints                                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Empenho
{
    public class IndexModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

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

            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }
}
