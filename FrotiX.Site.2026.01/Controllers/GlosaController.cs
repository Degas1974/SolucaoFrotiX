using ClosedXML.Excel;
using FrotiX.Services;
using FrotiX.Helpers;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Blazor.Data;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GlosaController                                                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para processamento e gerenciamento de Glosas de Faturamento.          ║
    /// ║    Opera integração com IGlosaService e Grids Syncfusion.                    ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Permite auditoria e contestação de valores cobrados, integrando           ║
    /// ║    dados contratuais e eventos operacionais para calcular descontos.         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /glosa                                                       ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [ApiController]
    [Route("glosa")]
    public class GlosaController : ControllerBase
    {
        private readonly IGlosaService _service;
        private readonly ILogService _logService;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GlosaController (Construtor)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de Glosas com serviço de regra de negócio.       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • service (IGlosaService): Regras de cálculo de glosa.                    ║
        /// ║    • logService (ILogService): Serviço de log centralizado.                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public GlosaController(IGlosaService service, ILogService logService)
        {
            try
            {
                _service = service;
                _logService = logService;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GlosaController.cs", "GlosaController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Resumo (GRID)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados resumidos para grid Syncfusion (server-side operations).    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dm (DataManagerRequest): Filtros/Paginação do grid.                     ║
        /// ║    • contratoId (Guid): ID do contrato.                                      ║
        /// ║    • ano (int): Ano de referência.                                           ║
        /// ║    • mes (int): Mês de referência.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON { Result, Count } para o grid.                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("resumo")]
        [HttpGet("resumo/")]
        public IActionResult Resumo(
            [FromQuery] DataManagerRequest dm,
            [FromQuery] Guid contratoId,
            [FromQuery] int ano,
            [FromQuery] int mes
        )
        {
            try
            {
                // [DADOS] Carrega resumo de glosas por contrato/mês/ano.
                var data = _service.ListarResumo(contratoId, mes, ano).AsQueryable();

                var ops = new DataOperations();
                IEnumerable result = data;

                // search (opcional)
                // [FILTRO] Pesquisa textual global do DataManager.
                if (dm.Search != null && dm.Search.Count > 0)
                    result = ops.PerformSearching(result, dm.Search);

                // where
                // [FILTRO] Filtros estruturados com operador and/or.
                var whereOperator = (dm.Where != null && dm.Where.Count > 0) ? dm.Where[0].Operator : "and";
                result = ops.PerformFiltering(result, dm.Where, whereOperator);

                // sort
                // [ORDENACAO] Ordenação das colunas solicitadas.
                result = ops.PerformSorting(result, dm.Sorted);

                // total antes de paginar
                // [PAGINACAO] Total de registros antes do Skip/Take.
                var count = result.Cast<object>().Count();

                // paginação
                // [PAGINACAO] Aplicação de Skip/Take.
                if (dm.Skip != 0)
                    result = ops.PerformSkip(result, dm.Skip);
                if (dm.Take != 0)
                    result = ops.PerformTake(result, dm.Take);

                return new JsonResult(new DataResult { Result = result, Count = count });
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "GlosaController.cs", "Resumo");
                Alerta.TratamentoErroComLinha("GlosaController.cs", "Resumo", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Detalhes (GRID)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados detalhados para grid Syncfusion.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dm (DataManagerRequest): Filtros/Paginação do grid.                     ║
        /// ║    • contratoId (Guid): ID do contrato.                                      ║
        /// ║    • ano (int): Ano de referência.                                           ║
        /// ║    • mes (int): Mês de referência.                                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("detalhes")]
        [HttpGet("detalhes/")]
        public IActionResult Detalhes(
            [FromQuery] DataManagerRequest dm,
            [FromQuery] Guid contratoId,
            [FromQuery] int ano,
            [FromQuery] int mes
        )
        {
            try
            {
                // [DADOS] Carrega detalhes de glosas por contrato/mês/ano.
                var data = _service.ListarDetalhes(contratoId, mes, ano).AsQueryable();

                var ops = new DataOperations();
                IEnumerable result = data;

                // [FILTRO] Pesquisa textual global do DataManager.
                if (dm.Search != null && dm.Search.Count > 0)
                    result = ops.PerformSearching(result, dm.Search);

                // [FILTRO] Filtros estruturados com operador and/or.
                var whereOperator = (dm.Where != null && dm.Where.Count > 0) ? dm.Where[0].Operator : "and";
                result = ops.PerformFiltering(result, dm.Where, whereOperator);

                // [ORDENACAO] Ordenação das colunas solicitadas.
                result = ops.PerformSorting(result, dm.Sorted);

                // [PAGINACAO] Total de registros antes do Skip/Take.
                var count = result.Cast<object>().Count();

                // [PAGINACAO] Aplicação de Skip/Take.
                if (dm.Skip != 0)
                    result = ops.PerformSkip(result, dm.Skip);
                if (dm.Take != 0)
                    result = ops.PerformTake(result, dm.Take);

                return new JsonResult(new DataResult { Result = result, Count = count });
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "GlosaController.cs", "Detalhes");
                Alerta.TratamentoErroComLinha("GlosaController.cs", "Detalhes", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExportResumo (Excel)                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Gera arquivo Excel (.xlsx) com o resumo de glosas.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • contratoId (Guid): Contrato.                                            ║
        /// ║    • mes (int): Mês.                                                         ║
        /// ║    • ano (int): Ano.                                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("export/resumo")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public IActionResult ExportResumo(
            [FromQuery] Guid contratoId,
            [FromQuery] int mes,
            [FromQuery] int ano
        )
        {
            try
            {
                // [DADOS] Coleta dados de resumo para exportação.
                var resumo = _service.ListarResumo(contratoId, mes, ano).ToList();

                // [ARQUIVO] Monta workbook e worksheet de resumo.
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Resumo");
                var table = ws.Cell(1, 1).InsertTable(resumo, true);
                table.Theme = XLTableTheme.TableStyleMedium2;

                // [FORMATO] Ajusta colunas monetárias.
                FormatCurrencyColumns(
                    ws,
                    table,
                    "PrecoDiario",
                    "PrecoTotalMensal",
                    "Glosa",
                    "ValorParaAteste"
                );
                ws.Columns().AdjustToContents();

                // [ARQUIVO] Retorna arquivo XLSX para download.
                return BuildExcelFileResult(wb, $"Glosa_Resumo_{ano}-{mes:00}.xlsx");
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "GlosaController.cs", "ExportResumo");
                Alerta.TratamentoErroComLinha("GlosaController.cs", "ExportResumo", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExportDetalhes (Excel)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Gera arquivo Excel (.xlsx) com os detalhes de glosas.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • contratoId (Guid): Contrato.                                            ║
        /// ║    • mes (int): Mês.                                                         ║
        /// ║    • ano (int): Ano.                                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("export/detalhes")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public IActionResult ExportDetalhes(
            [FromQuery] Guid contratoId,
            [FromQuery] int mes,
            [FromQuery] int ano
        )
        {
            try
            {
                // [DADOS] Coleta dados detalhados para exportação.
                var detalhes = _service.ListarDetalhes(contratoId, mes, ano).ToList();

                // [ARQUIVO] Monta workbook e worksheet de detalhes.
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Detalhes");
                var table = ws.Cell(1, 1).InsertTable(detalhes, true);
                table.Theme = XLTableTheme.TableStyleMedium2;

                // [FORMATO] Ajusta colunas de data.
                FormatDateColumns(
                    ws,
                    table,
                    "DataSolicitacao",
                    "DataDisponibilidade",
                    "DataRecolhimento",
                    "DataDevolucao"
                );
                ws.Columns().AdjustToContents();

                // [ARQUIVO] Retorna arquivo XLSX para download.
                return BuildExcelFileResult(wb, $"Glosa_Detalhes_{ano}-{mes:00}.xlsx");
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "GlosaController.cs", "ExportDetalhes");
                Alerta.TratamentoErroComLinha("GlosaController.cs", "ExportDetalhes", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExportAmbos (Excel Completo)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Gera arquivo XLSX com duas abas (Resumo e Detalhes).                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • contratoId (Guid): Contrato.                                            ║
        /// ║    • mes (int): Mês.                                                         ║
        /// ║    • ano (int): Ano.                                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("export")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public IActionResult ExportAmbos(
            [FromQuery] Guid contratoId,
            [FromQuery] int mes,
            [FromQuery] int ano
        )
        {
            try
            {
                // [DADOS] Coleta resumo e detalhes para exportação completa.
                var resumo = _service.ListarResumo(contratoId, mes, ano).ToList();
                var detalhes = _service.ListarDetalhes(contratoId, mes, ano).ToList();

                // [ARQUIVO] Monta workbook com duas abas.
                using var wb = new XLWorkbook();

                // [ARQUIVO] Aba de resumo.
                var wsResumo = wb.Worksheets.Add("Resumo");
                var tbResumo = wsResumo.Cell(1, 1).InsertTable(resumo, true);
                tbResumo.Theme = XLTableTheme.TableStyleMedium2;
                // [FORMATO] Ajusta colunas monetárias do resumo.
                FormatCurrencyColumns(
                    wsResumo,
                    tbResumo,
                    "PrecoDiario",
                    "PrecoTotalMensal",
                    "Glosa",
                    "ValorParaAteste"
                );
                wsResumo.Columns().AdjustToContents();

                // [ARQUIVO] Aba de detalhes.
                var wsDet = wb.Worksheets.Add("Detalhes");
                var tbDet = wsDet.Cell(1, 1).InsertTable(detalhes, true);
                tbDet.Theme = XLTableTheme.TableStyleMedium2;
                // [FORMATO] Ajusta colunas de datas dos detalhes.
                FormatDateColumns(
                    wsDet,
                    tbDet,
                    "DataSolicitacao",
                    "DataDisponibilidade",
                    "DataRecolhimento",
                    "DataDevolucao"
                );
                wsDet.Columns().AdjustToContents();

                // [ARQUIVO] Retorna arquivo XLSX para download.
                return BuildExcelFileResult(wb, $"Glosa_{ano}-{mes:00}.xlsx");
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "GlosaController.cs", "ExportAmbos");
                Alerta.TratamentoErroComLinha("GlosaController.cs", "ExportAmbos", error);
                return StatusCode(500);
            }
        }

        // ===================== HELPERS =====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: BuildExcelFileResult                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Converte o workbook em FileContentResult para download.                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private static FileContentResult BuildExcelFileResult(XLWorkbook wb, string fileName)
        {
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return new FileContentResult(
                ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            )
            {
                FileDownloadName = fileName,
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FormatCurrencyColumns                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Aplica formatação monetária (R$) nas colunas especificadas.               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private static void FormatCurrencyColumns(
            IXLWorksheet ws,
            IXLTable table,
            params string[] headerNames
        )
        {
            var headers =
                headerNames
                    ?.Where(h => !string.IsNullOrWhiteSpace(h))
                    .Select(h => h.Trim())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var cell in table.HeadersRow().Cells())
                if (headers.Contains(cell.GetString().Trim()))
                    ws.Column(cell.Address.ColumnNumber).Style.NumberFormat.Format = "R$ #,##0.00";
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FormatDateColumns                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Aplica formatação de data (dd/MM/yyyy) nas colunas especificadas.         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private static void FormatDateColumns(
            IXLWorksheet ws,
            IXLTable table,
            params string[] headerNames
        )
        {
            var headers =
                headerNames
                    ?.Where(h => !string.IsNullOrWhiteSpace(h))
                    .Select(h => h.Trim())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var cell in table.HeadersRow().Cells())
                if (headers.Contains(cell.GetString().Trim()))
                    ws.Column(cell.Address.ColumnNumber).Style.DateFormat.Format = "dd/MM/yyyy";
        }
    }
}
