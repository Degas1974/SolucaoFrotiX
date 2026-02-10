/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: CustomReportSourceResolver.cs                                                           ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Implementação de IReportSourceResolver para Telerik Reports. Resolve .trdp/.trdx.      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Resolve() - localiza relatório e passa parâmetros do frontend                            ║
   ║ 🔗 DEPS: Telerik.Reporting.Services | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                          ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Reporting;
using Telerik.Reporting.Services;

namespace FrotiX.Services
{
    public class CustomReportSourceResolver :IReportSourceResolver
    {
        private readonly IWebHostEnvironment _environment;

        public CustomReportSourceResolver(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /***********************************************************************************
         * ⚡ FUNÇÃO: Resolve
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Localizar arquivo de relatório Telerik (.trdp/.trdx) e retornar
         *                   UriReportSource com parâmetros passados do frontend
         *
         * 📥 ENTRADAS     : reportId [string] - Nome do relatório (com ou sem extensão)
         *                   operationOrigin [OperationOrigin] - Origem da operação
         *                   currentParameterValues [IDictionary] - Parâmetros do frontend
         *
         * 📤 SAÍDAS       : ReportSource - UriReportSource configurado com parâmetros
         *
         * ⬅️ CHAMADO POR  : Telerik ReportViewer (via IReportSourceResolver)
         *
         * ➡️ CHAMA        : Path.Combine() [sistema arquivos]
         *                   File.Exists() [validação]
         *                   UriReportSource() [Telerik.Reporting]
         *
         * 📝 OBSERVAÇÕES  : Busca em /Reports. Adiciona .trdp se extensão ausente.
         *                   CRÍTICO: Passa parâmetros recebidos ao relatório.
         *                   Lança FileNotFoundException se arquivo não encontrado.
         ***********************************************************************************/
        public ReportSource Resolve(string reportId , OperationOrigin operationOrigin , IDictionary<string , object> currentParameterValues)
        {
            try
            {
                // [DADOS] Construir caminho absoluto para o relatório
                // Busca na pasta /Reports dentro do diretório raiz da aplicação
                var reportsPath = Path.Combine(_environment.ContentRootPath , "Reports");
                var reportPath = Path.Combine(reportsPath , reportId);

                // [VALIDACAO] Adicionar extensão padrão se não houver .trdp ou .trdx
                if (!reportPath.EndsWith(".trdp") && !reportPath.EndsWith(".trdx"))
                    reportPath += ".trdp";

                // [VALIDACAO] Verificar se arquivo existe no disco
                if (!File.Exists(reportPath))
                    throw new FileNotFoundException($"Relatório não encontrado: {reportId}");

                // [DADOS] Criar source de  URI para Telerik (aponta para arquivo local)
                var reportPackageSource = new UriReportSource
                {
                    Uri = reportPath
                };

                // [REGRA] CRÍTICO: Passar os parâmetros recebidos do front-end para o relatório
                // Estes parâmetros serão acessíveis no template .trdp do relatório
                if (currentParameterValues != null)
                {
                    foreach (var param in currentParameterValues)
                    {
                        reportPackageSource.Parameters.Add(param.Key, param.Value);
                    }
                }

                return reportPackageSource;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao resolver relatório '{reportId}': {ex.Message}", ex);
            }
        }
    }
}
