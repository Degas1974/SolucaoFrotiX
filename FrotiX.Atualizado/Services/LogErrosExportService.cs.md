# Services/LogErrosExportService.cs

**ARQUIVO NOVO** | 618 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using FrotiX.Repository.IRepository;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services;

public interface ILogErrosExportService
{
    Task<byte[]> ExportLogsToExcelAsync(LogQueryFilter filter);
    Task<byte[]> ExportLogsToCsvAsync(LogQueryFilter filter);
    Task<byte[]> ExportExecutiveReportPdfAsync(DateTime startDate, DateTime endDate);
    Task<byte[]> ExportExecutiveReportExcelAsync(DateTime startDate, DateTime endDate);
}

public class LogErrosExportService : ILogErrosExportService
{
    private readonly ILogRepository _repository;
    private readonly ILogger<LogErrosExportService> _logger;

    private static readonly string PrimaryColor = "#1e40af";

    public LogErrosExportService(ILogRepository repository, ILogger<LogErrosExportService> logger)
    {
        _repository = repository;
        _logger = logger;

        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> ExportLogsToExcelAsync(LogQueryFilter filter)
    {
        try
        {
            var logs = await _repository.GetForExportAsync(filter);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Logs de Erros");

            var headers = new[] { "ID", "Data/Hora", "Tipo", "Origem", "Nível", "Mensagem", "Arquivo", "Método", "Linha", "Usuário", "URL", "Resolvido" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e40af");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            int row = 2;
            foreach (var log in logs)
            {
                worksheet.Cell(row, 1).Value = log.Id;
                worksheet.Cell(row, 2).Value = log.DataHora;
                worksheet.Cell(row, 3).Value = log.Tipo;
                worksheet.Cell(row, 4).Value = log.Origem;
                worksheet.Cell(row, 5).Value = log.Nivel;
                worksheet.Cell(row, 6).Value = TruncateText(log.Mensagem, 200);
                worksheet.Cell(row, 7).Value = log.Arquivo;
                worksheet.Cell(row, 8).Value = log.Metodo;
                worksheet.Cell(row, 9).Value = log.Linha;
                worksheet.Cell(row, 10).Value = log.Usuario;
                worksheet.Cell(row, 11).Value = TruncateText(log.Url, 100);
                worksheet.Cell(row, 12).Value = log.Resolvido;

                var rowColor = GetRowColor(log.Tipo);
                if (!string.IsNullOrEmpty(rowColor))
                {
                    worksheet.Range(row, 1, row, 12).Style.Fill.BackgroundColor = XLColor.FromHtml(rowColor);
                }

                row++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.Column(6).Width = 50;
            worksheet.Column(11).Width = 40;

            worksheet.RangeUsed().SetAutoFilter();

            worksheet.SheetView.FreezeRows(1);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao exportar logs para Excel");
            throw;
        }
    }

    public async Task<byte[]> ExportLogsToCsvAsync(LogQueryFilter filter)
    {
        try
        {
            var logs = await _repository.GetForExportAsync(filter);

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);

            await writer.WriteLineAsync("ID;Data/Hora;Tipo;Origem;Nível;Mensagem;Arquivo;Método;Linha;Usuário;URL;Resolvido");

            foreach (var log in logs)
            {
                var line = $"{log.Id};" +
                    $"\"{log.DataHora}\";" +
                    $"\"{log.Tipo}\";" +
                    $"\"{log.Origem}\";" +
                    $"\"{log.Nivel}\";" +
                    $"\"{EscapeCsv(log.Mensagem)}\";" +
                    $"\"{log.Arquivo}\";" +
                    $"\"{log.Metodo}\";" +
                    $"\"{log.Linha}\";" +
                    $"\"{log.Usuario}\";" +
                    $"\"{EscapeCsv(log.Url)}\";" +
                    $"\"{log.Resolvido}\"";
                await writer.WriteLineAsync(line);
            }

            await writer.FlushAsync();
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao exportar logs para CSV");
            throw;
        }
    }

    public async Task<byte[]> ExportExecutiveReportPdfAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var report = await _repository.GetExecutiveReportAsync(startDate, endDate);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(c => ComposeHeader(c, startDate, endDate));

                    page.Content().Element(c => ComposeContent(c, report));

                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório PDF");
            throw;
        }
    }

    public async Task<byte[]> ExportExecutiveReportExcelAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var report = await _repository.GetExecutiveReportAsync(startDate, endDate);

            using var workbook = new XLWorkbook();

            var wsResumo = workbook.Worksheets.Add("Resumo Executivo");
            ComposeExcelSummary(wsResumo, report);

            var wsTopPages = workbook.Worksheets.Add("Top Páginas");
            ComposeExcelRanking(wsTopPages, "Top Páginas com Erros", report.TopPaginas);

            var wsTopErrors = workbook.Worksheets.Add("Top Erros");
            ComposeExcelRanking(wsTopErrors, "Erros Mais Frequentes", report.TopErros);

            var wsTopUsers = workbook.Worksheets.Add("Top Usuários");
            ComposeExcelRanking(wsTopUsers, "Usuários com Mais Erros", report.TopUsuarios);

            var wsTimeline = workbook.Worksheets.Add("Timeline");
            ComposeExcelTimeline(wsTimeline, report.ErrosPorDia);

            var wsPeakHours = workbook.Worksheets.Add("Horários de Pico");
            ComposeExcelPeakHours(wsPeakHours, report.HorariosPico);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório Excel");
            throw;
        }
    }

    private void ComposeHeader(IContainer container, DateTime startDate, DateTime endDate)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("FrotiX - Sistema de Gestão de Frotas")
                    .FontSize(18)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);

                col.Item().Text("Relatório Executivo de Logs de Erros")
                    .FontSize(14)
                    .SemiBold()
                    .FontColor(Colors.Grey.Darken2);

                col.Item().Text($"Período: {startDate:dd/MM/yyyy} a {endDate:dd/MM/yyyy}")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Medium);
            });

            row.ConstantItem(100).AlignRight().Column(col =>
            {
                col.Item().Text($"Gerado em:")
                    .FontSize(8)
                    .FontColor(Colors.Grey.Medium);
                col.Item().Text($"{DateTime.Now:dd/MM/yyyy HH:mm}")
                    .FontSize(9)
                    .Bold();
            });
        });

        container.PaddingTop(10).LineHorizontal(2).LineColor(Colors.Blue.Darken3);
    }

    private void ComposeContent(IContainer container, LogExecutiveReport report)
    {
        container.PaddingVertical(15).Column(column =>
        {

            column.Item().Element(c => ComposeKPIs(c, report));

            column.Item().PaddingVertical(10);

            column.Item().Element(c => ComposeComparison(c, report));

            column.Item().PaddingVertical(10);

            column.Item().Element(c => ComposeTopProblems(c, report));

            column.Item().PaddingVertical(10);

            column.Item().Element(c => ComposeDistribution(c, report));

            column.Item().PaddingVertical(10);

            column.Item().Element(c => ComposePeakHours(c, report));
        });
    }

    private void ComposeKPIs(IContainer container, LogExecutiveReport report)
    {
        container.Column(col =>
        {
            col.Item().Text("Resumo do Período")
                .FontSize(14)
                .Bold()
                .FontColor(Colors.Blue.Darken3);

            col.Item().PaddingTop(10).Row(row =>
            {

                row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                {
                    c.Item().Text("Total de Logs").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text(report.TotalLogs.ToString("N0")).FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                });

                row.ConstantItem(10);

                row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                {
                    c.Item().Text("Total de Erros").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text(report.TotalErros.ToString("N0")).FontSize(20).Bold().FontColor(Colors.Red.Darken2);
                });

                row.ConstantItem(10);

                row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                {
                    c.Item().Text("Média/Dia").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text(report.MediaErrosPorDia.ToString("N1")).FontSize(20).Bold().FontColor(Colors.Orange.Darken2);
                });

                row.ConstantItem(10);

                row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                {
                    c.Item().Text("Taxa Resolução").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text($"{report.TaxaResolucao:N1}%").FontSize(20).Bold().FontColor(Colors.Green.Darken2);
                });
            });
        });
    }

    private void ComposeComparison(IContainer container, LogExecutiveReport report)
    {
        container.Column(col =>
        {
            col.Item().Text("Comparativo com Período Anterior")
                .FontSize(14)
                .Bold()
                .FontColor(Colors.Blue.Darken3);

            col.Item().PaddingTop(10).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                        .Text("Métrica").FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                        .Text("Período Atual").FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                        .Text("Período Anterior").FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                        .Text("Variação").FontColor(Colors.White).Bold();
                });

                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Erros");
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(report.ComparativoErros.PeriodoAtual.ToString("N0"));
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(report.ComparativoErros.PeriodoAnterior.ToString("N0"));
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text($"{report.ComparativoErros.VariacaoPercentual:+0.0;-0.0;0}%")
                    .FontColor(report.ComparativoErros.VariacaoPercentual > 0 ? Colors.Red.Darken2 : Colors.Green.Darken2);

                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Warnings");
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(report.ComparativoWarnings.PeriodoAtual.ToString("N0"));
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(report.ComparativoWarnings.PeriodoAnterior.ToString("N0"));
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text($"{report.ComparativoWarnings.VariacaoPercentual:+0.0;-0.0;0}%")
                    .FontColor(report.ComparativoWarnings.VariacaoPercentual > 0 ? Colors.Red.Darken2 : Colors.Green.Darken2);
            });
        });
    }

    private void ComposeTopProblems(IContainer container, LogExecutiveReport report)
    {
        container.Column(col =>
        {
            col.Item().Text("Top 5 Problemas")
                .FontSize(14)
                .Bold()
                .FontColor(Colors.Blue.Darken3);

            col.Item().PaddingTop(10).Row(row =>
            {

                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Páginas com Mais Erros").FontSize(11).SemiBold();
                    c.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(25);
                            columns.RelativeColumn();
                            columns.ConstantColumn(50);
                        });

                        foreach (var item in report.TopPaginas.Take(5))
                        {
                            table.Cell().Padding(3).Text($"{item.Posicao}.");
                            table.Cell().Padding(3).Text(TruncateText(item.Label, 30));
                            table.Cell().Padding(3).AlignRight().Text(item.Count.ToString("N0"));
                        }
                    });
                });

                row.ConstantItem(20);

                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Erros Mais Frequentes").FontSize(11).SemiBold();
                    c.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(25);
                            columns.RelativeColumn();
                            columns.ConstantColumn(50);
                        });

                        foreach (var item in report.TopErros.Take(5))
                        {
                            table.Cell().Padding(3).Text($"{item.Posicao}.");
                            table.Cell().Padding(3).Text(TruncateText(item.Label, 30));
                            table.Cell().Padding(3).AlignRight().Text(item.Count.ToString("N0"));
                        }
                    });
                });
            });
        });
    }

    private void ComposeDistribution(IContainer container, LogExecutiveReport report)
    {
        container.Column(col =>
        {
            col.Item().Text("Distribuição por Tipo e Origem")
                .FontSize(14)
                .Bold()
                .FontColor(Colors.Blue.Darken3);

            col.Item().PaddingTop(10).Row(row =>
            {

                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Por Tipo").FontSize(11).SemiBold();
                    c.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(50);
                        });

                        foreach (var item in report.DistribuicaoPorTipo)
                        {
                            table.Cell().Padding(3).Text(item.Label);
                            table.Cell().Padding(3).AlignRight().Text(item.Count.ToString("N0"));
                            table.Cell().Padding(3).AlignRight().Text($"{item.Percentage:N1}%");
                        }
                    });
                });

                row.ConstantItem(20);

                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Por Origem").FontSize(11).SemiBold();
                    c.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(50);
                        });

                        foreach (var item in report.DistribuicaoPorOrigem)
                        {
                            table.Cell().Padding(3).Text(item.Label);
                            table.Cell().Padding(3).AlignRight().Text(item.Count.ToString("N0"));
                            table.Cell().Padding(3).AlignRight().Text($"{item.Percentage:N1}%");
                        }
                    });
                });
            });
        });
    }

    private void ComposePeakHours(IContainer container, LogExecutiveReport report)
    {
        container.Column(col =>
        {
            col.Item().Text("Horários de Pico")
                .FontSize(14)
                .Bold()
                .FontColor(Colors.Blue.Darken3);

            col.Item().PaddingTop(10).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    for (int i = 0; i < 12; i++)
                        columns.RelativeColumn();
                });

                foreach (var hour in report.HorariosPico.Where(h => h.Hora < 12).OrderBy(h => h.Hora))

... (+118 linhas)
```
