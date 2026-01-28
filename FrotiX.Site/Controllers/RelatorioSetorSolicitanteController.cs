/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: RelatorioSetorSolicitanteController.cs                           ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: RelatorioSetorSolicitante
     * 🎯 OBJETIVO: Gerar relatório de Setores Solicitantes usando Stimulsoft Reporting
     * 📋 ROTAS: /SetorSolicitante/RelatorioSetorSolicitante/*
     * 🔗 ENTIDADES: SetorSolicitante (via template .mrt)
     * 📦 DEPENDÊNCIAS: Stimulsoft.Report, Stimulsoft.Report.Mvc
     ****************************************************************************************/
    [Route("SetorSolicitante/RelatorioSetorSolicitante")]
    public class RelatorioSetorSolicitanteController : Controller
    {
        // [DOC] Construtor estático: inicializa licença Stimulsoft antes de qualquer uso
        static RelatorioSetorSolicitanteController()
        {
            try
            {
                // [DOC] Licença Stimulsoft hardcoded (requer validação periódica)
                Stimulsoft.Base.StiLicense.Key =
                    "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";
                //Stimulsoft.Base.StiLicense.LoadFromFile("https://localhost:44340/licenses/stimulsoftlicense.key");
                //Stimulsoft.Base.StiLicense.LoadFromStream(stream);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "RelatorioSetorSolicitanteController",
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Index
         * 🎯 OBJETIVO: Renderizar página do relatório de Setores Solicitantes
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: View
         * 🔗 CHAMADA POR: Menu de relatórios
         * 🔄 CHAMA: View (Razor Page com viewer Stimulsoft)
         ****************************************************************************************/
        [IgnoreAntiforgeryToken]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "Index",
                    error
                );
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetReport
         * 🎯 OBJETIVO: Carregar e renderizar template .mrt do relatório de setores
         * 📥 ENTRADAS: Nenhuma (dados vêm do template)
         * 📤 SAÍDAS: StiNetCoreViewer.GetReportResult (stream PDF/HTML)
         * 🔗 CHAMADA POR: Viewer Stimulsoft (JavaScript no frontend)
         * 🔄 CHAMA: SetoresSolicitantes.mrt
         ****************************************************************************************/
        [Route("GetReport")]
        public IActionResult GetReport()
        {
            try
            {
                StiReport report = new StiReport();
                report.Dictionary.DataStore.Clear();
                // [DOC] Carrega template .mrt da pasta Reports
                report.Load(StiNetCoreHelper.MapPath(this, "Reports/SetoresSolicitantes.mrt"));
                //report.Load(StiNetCoreHelper.MapPath(this, "Reports/Viagens.mrt"));
                //report["@p_ViagemId"] = "4D220794-ED4B-454B-DCD8-08D9A2F6C6C2";
                //report.DataSources["DataSource1"].Parameters["@p_ViagemId"].Value = "4D220794-ED4B-454B-DCD8-08D9A2F6C6C2";

                return StiNetCoreViewer.GetReportResult(this, report);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "GetReport",
                    error
                );
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ViewerEvent
         * 🎯 OBJETIVO: Processar eventos do viewer Stimulsoft (print, export, zoom, etc)
         * 📥 ENTRADAS: Request do viewer (POST automático)
         * 📤 SAÍDAS: StiNetCoreViewer.ViewerEventResult (resposta do evento)
         * 🔗 CHAMADA POR: Viewer Stimulsoft (JavaScript - eventos de UI)
         * 🔄 CHAMA: StiNetCoreViewer.ViewerEventResult()
         ****************************************************************************************/
        [Route("ViewerEvent")]
        public IActionResult ViewerEvent()
        {
            try
            {
                // [DOC] Processa eventos: PrintReport, ExportReport, GetPages, etc
                return StiNetCoreViewer.ViewerEventResult(this);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "RelatorioSetorSolicitanteController.cs",
                    "ViewerEvent",
                    error
                );
                return View(); // padronizado
            }
        }
    }
}
