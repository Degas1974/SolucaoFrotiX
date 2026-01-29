/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : ReportsController.cs                                            ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Controller base para integração com Telerik Reporting Services.              ║
║ Herda de ReportsControllerBase para fornecer endpoints de relatórios.        ║
║ Endpoint: /api/reports                                                       ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ CONSTRUTOR                                                                   ║
║ - ReportsController(IReportServiceConfiguration) : Recebe configuração       ║
║   via injeção de dependência do Startup/Program.cs                           ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DEPENDENCIAS                                                                 ║
║ - Telerik.Reporting.Services : Serviços de relatórios Telerik                ║
║ - Telerik.Reporting.Services.AspNetCore : Integração ASP.NET Core            ║
║ - IReportServiceConfiguration : Configuração injetada                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS HERDADOS                                                           ║
║ - GET /api/reports : Lista relatórios disponíveis                            ║
║ - POST /api/reports : Processa relatório                                     ║
║ - Demais endpoints padrão do ReportsControllerBase                           ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 19          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

namespace FrotiX.Controllers
{
    [Route("api/reports")]
    public class ReportsController :ReportsControllerBase
    {
        // Construtor NOVO - usando injeção de dependência
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
            : base(reportServiceConfiguration)
        {
            // Não precisa de código aqui - a configuração vem do Startup/Program.cs
        }
    }
}
