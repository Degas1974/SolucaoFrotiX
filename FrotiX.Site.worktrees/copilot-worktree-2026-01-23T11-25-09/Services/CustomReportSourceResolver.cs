/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - TELERIK REPORT RESOLVER                                                 #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using Telerik.Reporting;
using Telerik.Reporting.Services;

namespace FrotiX.Services
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: CustomReportSourceResolver                                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Resolvedor customizado para localizar e carregar relatórios Telerik.      ║
    /// ║    Gerencia o caminho dos arquivos .trdp/.trdx e repassa parâmetros.         ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Integração crítica com Telerik Reporting. Permite que relatórios sejam   ║
    /// ║    carregados dinamicamente com parâmetros personalizados.                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • Resolve() → Localiza arquivo de relatório e cria UriReportSource        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INTERNA - Infraestrutura de relatórios                            ║
    /// ║    • Arquivos relacionados: Reports/*.trdp, ReportsController.cs            ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class CustomReportSourceResolver : IReportSourceResolver
    {
        private readonly IWebHostEnvironment _environment;

        public CustomReportSourceResolver(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Resolve                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Localiza o arquivo de relatório e cria fonte com parâmetros injetados.    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • reportId: Nome/ID do relatório                                          ║
        /// ║    • operationOrigin: Origem da operação Telerik                             ║
        /// ║    • currentParameterValues: Parâmetros do relatório                         ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ReportSource: Fonte do relatório configurada                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public ReportSource Resolve(string reportId, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)
        {
            // [LOGICA] Caminho onde os relatórios .trdp ou .trdx estão salvos
            var reportsPath = Path.Combine(_environment.ContentRootPath, "Reports");
            var reportPath = Path.Combine(reportsPath, reportId);

            // [REGRA] Adiciona extensão padrão se não tiver
            if (!reportPath.EndsWith(".trdp") && !reportPath.EndsWith(".trdx"))
                reportPath += ".trdp";

            // [REGRA] Validação de existência do arquivo
            if (!File.Exists(reportPath))
                throw new FileNotFoundException($"Relatório não encontrado: {reportId}");

            var reportPackageSource = new UriReportSource
            {
                Uri = reportPath
            };

            // [DADOS] CRÍTICO: Passar os parâmetros recebidos do front-end para o relatório
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
