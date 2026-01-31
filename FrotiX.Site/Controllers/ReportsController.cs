/* ****************************************************************************************
 * ⚡ ARQUIVO: ReportsController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor o endpoint base do Telerik Reporting Services.
 *
 * 📥 ENTRADAS     : Requisições GET/POST do serviço de relatórios.
 *
 * 📤 SAÍDAS       : Respostas geradas pelo ReportsControllerBase.
 *
 * 🔗 CHAMADA POR  : Componentes Telerik Reporting.
 *
 * 🔄 CHAMA        : ReportsControllerBase (Telerik).
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: ReportsController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Servir endpoints herdados do Telerik Reporting Services.
     *
     * 📥 ENTRADAS     : Requisições do viewer/reporting.
     *
     * 📤 SAÍDAS       : Respostas do serviço de relatórios.
     ****************************************************************************************/
    [Route("api/reports")]
    public class ReportsController :ReportsControllerBase
    {
        // Construtor NOVO - usando injeção de dependência
        /****************************************************************************************
         * ⚡ FUNÇÃO: ReportsController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar a configuração do serviço de relatórios.
         *
         * 📥 ENTRADAS     : reportServiceConfiguration.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
            : base(reportServiceConfiguration)
        {
            System.Diagnostics.Debug.WriteLine("🔧 ReportsController inicializado!");
        }
    }
}
