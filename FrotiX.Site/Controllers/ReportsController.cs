/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ReportsController.cs                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Reports API
     * 🎯 OBJETIVO: Fornecer endpoints para Telerik Reporting (herda ReportsControllerBase)
     * 📋 ROTAS: /api/reports/* (rotas herdadas da base)
     * 🔗 ENTIDADES: Nenhuma (renderização de relatórios Telerik)
     * 📦 DEPENDÊNCIAS: Telerik.Reporting.Services.AspNetCore, IReportServiceConfiguration
     ****************************************************************************************/
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
