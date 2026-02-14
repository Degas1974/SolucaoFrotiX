// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: GerarEstatisticasViagens.cshtml.cs                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para geração de estatísticas de viagens.                          ║
// ║ Processa e gera relatórios estatísticos consolidados.                       ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • Injeção de IUnitOfWork e IWebHostEnvironment                              ║
// ║ • Processamento de métricas via ViagemController                            ║
// ║ • Gera estatísticas por período, motorista, veículo                         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Administracao
{
    public class GerarEstatisticasViagensModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GerarEstatisticasViagensModel(IUnitOfWork unitOfWork , IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GerarEstatisticasViagens.cshtml.cs" , "GerarEstatisticasViagensModel" , error);
            }
        }
    }
}
