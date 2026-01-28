// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: CustomReportSourceResolver.cs                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Resolver customizado para relatórios Telerik Reporting.                      ║
// ║ Mapeia IDs de relatório para arquivos .trdp/.trdx no diretório Reports.      ║
// ║                                                                              ║
// ║ FUNCIONALIDADES:                                                             ║
// ║ - Resolve caminho físico do relatório a partir do reportId                   ║
// ║ - Adiciona extensão .trdp automaticamente se não especificada                ║
// ║ - Passa parâmetros do frontend para o relatório                              ║
// ║                                                                              ║
// ║ DIRETÓRIO DE RELATÓRIOS:                                                     ║
// ║ - {ContentRoot}/Reports/*.trdp                                               ║
// ║ - {ContentRoot}/Reports/*.trdx                                               ║
// ║                                                                              ║
// ║ USO:                                                                         ║
// ║ - Registrado como IReportSourceResolver no container DI                      ║
// ║ - Usado pelo ReportsController para renderização de relatórios               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 14                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using Telerik.Reporting;
using Telerik.Reporting.Services;

namespace FrotiX.Services
{
    /// <summary>
    /// Resolver customizado para relatórios Telerik (.trdp/.trdx).
    /// </summary>
    public class CustomReportSourceResolver :IReportSourceResolver
    {
        private readonly IWebHostEnvironment _environment;

        public CustomReportSourceResolver(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public ReportSource Resolve(string reportId , OperationOrigin operationOrigin , IDictionary<string , object> currentParameterValues)
        {
            // Caminho onde seus relatórios .trdp ou .trdx estão salvos
            var reportsPath = Path.Combine(_environment.ContentRootPath , "Reports");
            var reportPath = Path.Combine(reportsPath , reportId);

            // Adiciona extensão se não tiver
            if (!reportPath.EndsWith(".trdp") && !reportPath.EndsWith(".trdx"))
                reportPath += ".trdp";

            if (!File.Exists(reportPath))
                throw new FileNotFoundException($"Relatório não encontrado: {reportId}");

            var reportPackageSource = new UriReportSource
            {
                Uri = reportPath
            };

            // CRÍTICO: Passar os parâmetros recebidos do front-end para o relatório
            if (currentParameterValues != null)
            {
                foreach (var param in currentParameterValues)
                {
                    reportPackageSource.Parameters.Add(param.Key, param.Value);
                }
            }

            return reportPackageSource;
        }
    }
}
