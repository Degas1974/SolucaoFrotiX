/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ReportsController.cs                                                                    ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Controller base para Telerik Reporting Services. Herda de ReportsControllerBase.       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: (Endpoints herdados) GET/POST /api/reports - lista e processa relatórios                 ║
   ║ 🔗 DEPS: Telerik.Reporting.Services.AspNetCore | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0               ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
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
            System.Diagnostics.Debug.WriteLine("🔧 ReportsController inicializado!");
        }
    }
}
