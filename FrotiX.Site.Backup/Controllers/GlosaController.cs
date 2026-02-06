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

/****************************************************************************************
 * ⚡ CONTROLLER: GlosaController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar glosas de notas fiscais (descontos por descumprimento contratual)
 *                   Endpoints API para grids Syncfusion EJ2 com paginação server-side
 *                   Exportação de relatórios em Excel (ClosedXML)
 * 📥 ENTRADAS     : DataManagerRequest (Syncfusion), Filtros (contratoId, ano, mês)
 * 📤 SAÍDAS       : JSON formato DataResult (Syncfusion), Arquivos Excel (.xlsx)
 * 🔗 CHAMADA POR  : JavaScript (Syncfusion Grid) das páginas de Glosas via AJAX
 * 🔄 CHAMA        : IGlosaService (lógica de negócio), ClosedXML (exportação Excel)
 * 📦 DEPENDÊNCIAS : ASP.NET Core, Syncfusion EJ2, ClosedXML, IGlosaService
 *
 * 📊 ENDPOINTS:
 *    - GET /glosa/resumo: Lista resumo de glosas (agregadas)
 *    - GET /glosa/detalhes: Lista detalhada de glosas (linha a linha)
 *    - POST /glosa/exportar-excel: Exporta dados para Excel
 *
 * ⚡ PERFORMANCE:
 *    - DataOperations (Syncfusion): Paginação, filtro e ordenação no servidor
 *    - IQueryable: Evita carregar dados desnecessários na memória
 *
 * 💡 CONCEITOS:
 *    - Glosa: Desconto aplicado em nota fiscal por descumprimento de contrato
 *    - Resumo: Totalizadores por período/contrato
 *    - Detalhes: Glosas individualizadas por nota fiscal
 ****************************************************************************************/
[ApiController]
[Route("glosa")]
public class GlosaController :ControllerBase
{
    private readonly IGlosaService _service;

    /****************************************************************************************
     * ⚡ FUNÇÃO: GlosaController (Construtor)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Injetar serviço de glosas (lógica de negócio)
     * 📥 ENTRADAS     : [IGlosaService] service
     * 📤 SAÍDAS       : Instância configurada
     * 🔗 CHAMADA POR  : ASP.NET Core DI
     ****************************************************************************************/
    public GlosaController(IGlosaService service)
    {
        try
        {
            _service = service;
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("GlosaController.cs" , "GlosaController" , error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: Resumo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Endpoint Syncfusion EJ2 Grid - Resumo de glosas agregadas
     *                   Suporta paginação, filtros e ordenação server-side
     * 📥 ENTRADAS     : [DataManagerRequest] dm - Parâmetros Syncfusion (filtro, ordenação, paginação)
     *                   [Guid] contratoId, [int] ano, [int] mes - Filtros de período
     * 📤 SAÍDAS       : [IActionResult] JSON formato DataResult (Result, Count)
     * 🔗 CHAMADA POR  : Syncfusion Grid JavaScript via AJAX GET
     * 🔄 CHAMA        : IGlosaService.ListarResumo(), DataOperations (Syncfusion)
     *
     * 📊 OPERAÇÕES SYNCFUSION:
     *    - PerformSearching: Busca textual
     *    - PerformFiltering: Filtros por coluna
     *    - PerformSorting: Ordenação
     *    - PerformSkip/Take: Paginação
     ****************************************************************************************/
    // aceita /glosa/resumo e /glosa/resumo/
    [HttpGet("resumo")]
    [HttpGet("resumo/")]
    public IActionResult Resumo(
        [FromQuery] DataManagerRequest dm ,
        [FromQuery] Guid contratoId ,
        [FromQuery] int ano ,
        [FromQuery] int mes
    )
    {
        try
        {
            // [DOC] Busca dados do serviço e converte para IQueryable (performance)
            var data = _service.ListarResumo(contratoId , mes , ano).AsQueryable();

            var ops = new DataOperations();
            IEnumerable result = data;

            // [DOC] Search opcional (busca textual em múltiplas colunas)
            if (dm.Search != null && dm.Search.Count > 0)
                result = ops.PerformSearching(result , dm.Search);

            // [DOC] Where - Filtros por coluna
            var whereOperator = (dm.Where != null && dm.Where.Count > 0) ? dm.Where[0].Operator : "and";
            result = ops.PerformFiltering(result , dm.Where , whereOperator);

            // sort
            result = ops.PerformSorting(result , dm.Sorted);

            // total antes de paginar
            var count = result.Cast<object>().Count();

            // paginação
            if (dm.Skip != 0)
                result = ops.PerformSkip(result , dm.Skip);
            if (dm.Take != 0)
                result = ops.PerformTake(result , dm.Take);

            return new JsonResult(new DataResult { Result = result , Count = count });
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("GlosaController.cs" , "Resumo" , error);
            return StatusCode(500);
        }
    }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Detalhes (GRID)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
    [HttpGet("detalhes")]
    [HttpGet("detalhes/")]
    public IActionResult Detalhes(
        [FromQuery] DataManagerRequest dm ,
        [FromQuery] Guid contratoId ,
        [FromQuery] int ano ,
        [FromQuery] int mes
    )
    {
        try
        {
            var data = _service.ListarDetalhes(contratoId , mes , ano).AsQueryable();

            var ops = new DataOperations();
            IEnumerable result = data;

            if (dm.Search != null && dm.Search.Count > 0)
                result = ops.PerformSearching(result , dm.Search);

            var whereOperator = (dm.Where != null && dm.Where.Count > 0) ? dm.Where[0].Operator : "and";
            result = ops.PerformFiltering(result , dm.Where , whereOperator);

            result = ops.PerformSorting(result , dm.Sorted);

            var count = result.Cast<object>().Count();

            if (dm.Skip != 0)
                result = ops.PerformSkip(result , dm.Skip);
            if (dm.Take != 0)
                result = ops.PerformTake(result , dm.Take);

            return new JsonResult(new DataResult { Result = result , Count = count });
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("GlosaController.cs" , "Detalhes" , error);
            return StatusCode(500);
        }
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
            var resumo = _service.ListarResumo(contratoId, mes, ano).ToList();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Resumo");
            var table = ws.Cell(1, 1).InsertTable(resumo, true);
            table.Theme = XLTableTheme.TableStyleMedium2;

            FormatCurrencyColumns(
                ws,
                table,
                "PrecoDiario",
                "PrecoTotalMensal",
                "Glosa",
                "ValorParaAteste"
            );
            ws.Columns().AdjustToContents();

            return BuildExcelFileResult(wb , $"Glosa_Resumo_{ano}-{mes:00}.xlsx");
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("GlosaController.cs" , "ExportResumo" , error);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Exportar detalhes de glosa em Excel.
    /// </summary>
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
                wsResumo ,
                tbResumo ,
                "PrecoDiario" ,
                "PrecoTotalMensal" ,
                "Glosa" ,
                "ValorParaAteste"
            );
            wsResumo.Columns().AdjustToContents();

            var wsDet = wb.Worksheets.Add("Detalhes");
            var tbDet = wsDet.Cell(1 , 1).InsertTable(detalhes , true);
            tbDet.Theme = XLTableTheme.TableStyleMedium2;
            FormatDateColumns(
                wsDet ,
                tbDet ,
                "DataSolicitacao" ,
                "DataDisponibilidade" ,
                "DataRecolhimento" ,
                "DataDevolucao"
            );
            wsDet.Columns().AdjustToContents();

            return BuildExcelFileResult(wb , $"Glosa_{ano}-{mes:00}.xlsx");
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("GlosaController.cs" , "ExportAmbos" , error);
            return StatusCode(500);
        }
    }

    private static IActionResult BuildExcelFileResult(XLWorkbook wb, string fileName)
    {
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return new FileContentResult(
            ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        )
        {
            FileDownloadName = fileName
        };
    }

    private static void FormatCurrencyColumns(
        IXLWorksheet ws,
        IXLTable table,
        params string[] headerNames)
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

    private static void FormatDateColumns(
        IXLWorksheet ws,
        IXLTable table,
        params string[] headerNames)
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