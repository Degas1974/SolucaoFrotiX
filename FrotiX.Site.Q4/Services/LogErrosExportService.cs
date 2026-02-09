/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LogErrosExportService.cs                                                               ║
   ║ 📂 CAMINHO: /Services                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Serviço para exportação de logs e relatórios gerenciais em Excel e PDF.              ║
   ║              Utiliza ClosedXML para Excel e QuestPDF para geração de PDFs profissionais.          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 FUNCIONALIDADES:                                                                                ║
   ║ • ExportToExcel: Exporta logs filtrados para planilha Excel (.xlsx)                               ║
   ║ • ExportToCSV: Exporta logs filtrados para CSV                                                    ║
   ║ • ExportExecutiveReportPdf: Gera relatório gerencial em PDF                                       ║
   ║ • ExportExecutiveReportExcel: Gera relatório gerencial em Excel                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: ILogRepository, ClosedXML, QuestPDF                                                      ║
   ║ 📅 31/01/2026 | 👤 Claude Code | 📝 v1.0                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ SERVICE: LogErrosExportService                                                     │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO: Serviço para exportação de logs e relatórios em Excel e PDF.           │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
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

    // Cores do FrotiX
    private static readonly string PrimaryColor = "#1e40af";

    public LogErrosExportService(ILogRepository repository, ILogger<LogErrosExportService> logger)
    {
        _repository = repository;
        _logger = logger;

        // Configurar licença do QuestPDF (Community)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    // ====== EXPORTAR LOGS PARA EXCEL ======
    public async Task<byte[]> ExportLogsToExcelAsync(LogQueryFilter filter)
    {
        try
        {
            var logs = await _repository.GetForExportAsync(filter);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Logs de Erros");

            // Cabeçalhos
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

            // Dados
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

                // Colorir linha baseado no tipo
                var rowColor = GetRowColor(log.Tipo);
                if (!string.IsNullOrEmpty(rowColor))
                {
                    worksheet.Range(row, 1, row, 12).Style.Fill.BackgroundColor = XLColor.FromHtml(rowColor);
                }

                row++;
            }

            // Ajustar largura das colunas
            worksheet.Columns().AdjustToContents();
            worksheet.Column(6).Width = 50; // Mensagem
            worksheet.Column(11).Width = 40; // URL

            // Filtros
            worksheet.RangeUsed().SetAutoFilter();

            // Congelar cabeçalho
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

    // ====== EXPORTAR LOGS PARA CSV ======
    public async Task<byte[]> ExportLogsToCsvAsync(LogQueryFilter filter)
    {
        try
        {
            var logs = await _repository.GetForExportAsync(filter);

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);

            // Cabeçalho
            await writer.WriteLineAsync("ID;Data/Hora;Tipo;Origem;Nível;Mensagem;Arquivo;Método;Linha;Usuário;URL;Resolvido");

            // Dados
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

    // ====== RELATÓRIO EXECUTIVO PDF ======
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

                    // Cabeçalho
                    page.Header().Element(c => ComposeHeader(c, startDate, endDate));

                    // Conteúdo
                    page.Content().Element(c => ComposeContent(c, report));

                    // Rodapé
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

    // ====== RELATÓRIO EXECUTIVO EXCEL ======
    public async Task<byte[]> ExportExecutiveReportExcelAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var report = await _repository.GetExecutiveReportAsync(startDate, endDate);

            using var workbook = new XLWorkbook();

            // Aba 1: Resumo Executivo
            var wsResumo = workbook.Worksheets.Add("Resumo Executivo");
            ComposeExcelSummary(wsResumo, report);

            // Aba 2: Top Páginas
            var wsTopPages = workbook.Worksheets.Add("Top Páginas");
            ComposeExcelRanking(wsTopPages, "Top Páginas com Erros", report.TopPaginas);

            // Aba 3: Top Erros
            var wsTopErrors = workbook.Worksheets.Add("Top Erros");
            ComposeExcelRanking(wsTopErrors, "Erros Mais Frequentes", report.TopErros);

            // Aba 4: Top Usuários
            var wsTopUsers = workbook.Worksheets.Add("Top Usuários");
            ComposeExcelRanking(wsTopUsers, "Usuários com Mais Erros", report.TopUsuarios);

            // Aba 5: Timeline
            var wsTimeline = workbook.Worksheets.Add("Timeline");
            ComposeExcelTimeline(wsTimeline, report.ErrosPorDia);

            // Aba 6: Horários de Pico
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

    // ====== COMPOSIÇÃO DO PDF ======
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
            // Seção 1: KPIs
            column.Item().Element(c => ComposeKPIs(c, report));

            column.Item().PaddingVertical(10);

            // Seção 2: Comparativo
            column.Item().Element(c => ComposeComparison(c, report));

            column.Item().PaddingVertical(10);

            // Seção 3: Top 5 Problemas
            column.Item().Element(c => ComposeTopProblems(c, report));

            column.Item().PaddingVertical(10);

            // Seção 4: Distribuição
            column.Item().Element(c => ComposeDistribution(c, report));

            column.Item().PaddingVertical(10);

            // Seção 5: Horários de Pico
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
                // Total de Logs
                row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                {
                    c.Item().Text("Total de Logs").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text(report.TotalLogs.ToString("N0")).FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                });

                row.ConstantItem(10);

                // Total de Erros
                row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                {
                    c.Item().Text("Total de Erros").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text(report.TotalErros.ToString("N0")).FontSize(20).Bold().FontColor(Colors.Red.Darken2);
                });

                row.ConstantItem(10);

                // Média por Dia
                row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                {
                    c.Item().Text("Média/Dia").FontSize(9).FontColor(Colors.Grey.Darken1);
                    c.Item().Text(report.MediaErrosPorDia.ToString("N1")).FontSize(20).Bold().FontColor(Colors.Orange.Darken2);
                });

                row.ConstantItem(10);

                // Taxa de Resolução
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

                // Cabeçalho
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

                // Erros
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text("Erros");
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(report.ComparativoErros.PeriodoAtual.ToString("N0"));
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(report.ComparativoErros.PeriodoAnterior.ToString("N0"));
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text($"{report.ComparativoErros.VariacaoPercentual:+0.0;-0.0;0}%")
                    .FontColor(report.ComparativoErros.VariacaoPercentual > 0 ? Colors.Red.Darken2 : Colors.Green.Darken2);

                // Warnings
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
                // Top Páginas
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

                // Top Erros
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
                // Por Tipo
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

                // Por Origem
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

                // Primeira linha (00h - 11h)
                foreach (var hour in report.HorariosPico.Where(h => h.Hora < 12).OrderBy(h => h.Hora))
                {
                    var bgColor = hour.IsPico ? Colors.Red.Lighten3 : Colors.Grey.Lighten3;
                    table.Cell().Background(bgColor).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(3).AlignCenter()
                        .Column(c =>
                        {
                            c.Item().Text(hour.HoraFormatada).FontSize(8).Bold();
                            c.Item().Text(hour.TotalErros.ToString()).FontSize(9);
                        });
                }

                // Segunda linha (12h - 23h)
                foreach (var hour in report.HorariosPico.Where(h => h.Hora >= 12).OrderBy(h => h.Hora))
                {
                    var bgColor = hour.IsPico ? Colors.Red.Lighten3 : Colors.Grey.Lighten3;
                    table.Cell().Background(bgColor).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(3).AlignCenter()
                        .Column(c =>
                        {
                            c.Item().Text(hour.HoraFormatada).FontSize(8).Bold();
                            c.Item().Text(hour.TotalErros.ToString()).FontSize(9);
                        });
                }
            });

            col.Item().PaddingTop(5).Text("* Células em vermelho indicam horários de pico")
                .FontSize(8).Italic().FontColor(Colors.Grey.Darken1);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            col.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Text("FrotiX - Sistema de Gestão de Frotas")
                    .FontSize(8)
                    .FontColor(Colors.Grey.Medium);

                row.RelativeItem().AlignRight().Text(x =>
                {
                    x.Span("Página ").FontSize(8).FontColor(Colors.Grey.Medium);
                    x.CurrentPageNumber().FontSize(8);
                    x.Span(" de ").FontSize(8).FontColor(Colors.Grey.Medium);
                    x.TotalPages().FontSize(8);
                });
            });
        });
    }

    // ====== COMPOSIÇÃO DO EXCEL ======
    private void ComposeExcelSummary(IXLWorksheet ws, LogExecutiveReport report)
    {
        // Título
        ws.Cell("A1").Value = "RELATÓRIO EXECUTIVO - LOGS DE ERROS";
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A1").Style.Font.FontSize = 16;
        ws.Range("A1:D1").Merge();

        ws.Cell("A2").Value = $"Período: {report.DataInicio:dd/MM/yyyy} a {report.DataFim:dd/MM/yyyy}";
        ws.Cell("A3").Value = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}";

        // KPIs
        ws.Cell("A5").Value = "INDICADORES CHAVE";
        ws.Cell("A5").Style.Font.Bold = true;
        ws.Cell("A5").Style.Fill.BackgroundColor = XLColor.FromHtml(PrimaryColor);
        ws.Cell("A5").Style.Font.FontColor = XLColor.White;

        ws.Cell("A6").Value = "Total de Logs:";
        ws.Cell("B6").Value = report.TotalLogs;
        ws.Cell("A7").Value = "Total de Erros:";
        ws.Cell("B7").Value = report.TotalErros;
        ws.Cell("A8").Value = "Média por Dia:";
        ws.Cell("B8").Value = report.MediaErrosPorDia;
        ws.Cell("A9").Value = "Taxa de Resolução:";
        ws.Cell("B9").Value = $"{report.TaxaResolucao}%";

        // Comparativo
        ws.Cell("A11").Value = "COMPARATIVO COM PERÍODO ANTERIOR";
        ws.Cell("A11").Style.Font.Bold = true;
        ws.Cell("A11").Style.Fill.BackgroundColor = XLColor.FromHtml(PrimaryColor);
        ws.Cell("A11").Style.Font.FontColor = XLColor.White;

        ws.Cell("A12").Value = "Métrica";
        ws.Cell("B12").Value = "Atual";
        ws.Cell("C12").Value = "Anterior";
        ws.Cell("D12").Value = "Variação";
        ws.Range("A12:D12").Style.Font.Bold = true;

        ws.Cell("A13").Value = "Erros";
        ws.Cell("B13").Value = report.ComparativoErros.PeriodoAtual;
        ws.Cell("C13").Value = report.ComparativoErros.PeriodoAnterior;
        ws.Cell("D13").Value = $"{report.ComparativoErros.VariacaoPercentual:+0.0;-0.0;0}%";

        ws.Cell("A14").Value = "Warnings";
        ws.Cell("B14").Value = report.ComparativoWarnings.PeriodoAtual;
        ws.Cell("C14").Value = report.ComparativoWarnings.PeriodoAnterior;
        ws.Cell("D14").Value = $"{report.ComparativoWarnings.VariacaoPercentual:+0.0;-0.0;0}%";

        // Distribuição
        ws.Cell("A16").Value = "DISTRIBUIÇÃO POR TIPO";
        ws.Cell("A16").Style.Font.Bold = true;
        ws.Cell("A16").Style.Fill.BackgroundColor = XLColor.FromHtml(PrimaryColor);
        ws.Cell("A16").Style.Font.FontColor = XLColor.White;

        int row = 17;
        ws.Cell($"A{row}").Value = "Tipo";
        ws.Cell($"B{row}").Value = "Quantidade";
        ws.Cell($"C{row}").Value = "Percentual";
        ws.Range($"A{row}:C{row}").Style.Font.Bold = true;
        row++;

        foreach (var item in report.DistribuicaoPorTipo)
        {
            ws.Cell($"A{row}").Value = item.Label;
            ws.Cell($"B{row}").Value = item.Count;
            ws.Cell($"C{row}").Value = $"{item.Percentage}%";
            row++;
        }

        ws.Columns().AdjustToContents();
    }

    private void ComposeExcelRanking(IXLWorksheet ws, string title, List<LogRankingItem> ranking)
    {
        ws.Cell("A1").Value = title.ToUpper();
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A1").Style.Font.FontSize = 14;

        ws.Cell("A3").Value = "Posição";
        ws.Cell("B3").Value = "Item";
        ws.Cell("C3").Value = "Detalhes";
        ws.Cell("D3").Value = "Quantidade";
        ws.Cell("E3").Value = "Percentual";
        ws.Range("A3:E3").Style.Font.Bold = true;
        ws.Range("A3:E3").Style.Fill.BackgroundColor = XLColor.FromHtml(PrimaryColor);
        ws.Range("A3:E3").Style.Font.FontColor = XLColor.White;

        int row = 4;
        foreach (var item in ranking)
        {
            ws.Cell($"A{row}").Value = item.Posicao;
            ws.Cell($"B{row}").Value = item.Label;
            ws.Cell($"C{row}").Value = item.SubLabel ?? "";
            ws.Cell($"D{row}").Value = item.Count;
            ws.Cell($"E{row}").Value = $"{item.Percentage}%";
            row++;
        }

        ws.Columns().AdjustToContents();
        ws.Column(2).Width = 50;
        ws.Column(3).Width = 40;
    }

    private void ComposeExcelTimeline(IXLWorksheet ws, List<LogTimelineItem> timeline)
    {
        ws.Cell("A1").Value = "TIMELINE DE ERROS POR DIA";
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A1").Style.Font.FontSize = 14;

        ws.Cell("A3").Value = "Data";
        ws.Cell("B3").Value = "Total";
        ws.Cell("C3").Value = "Erros";
        ws.Cell("D3").Value = "Warnings";
        ws.Cell("E3").Value = "Info";
        ws.Range("A3:E3").Style.Font.Bold = true;
        ws.Range("A3:E3").Style.Fill.BackgroundColor = XLColor.FromHtml(PrimaryColor);
        ws.Range("A3:E3").Style.Font.FontColor = XLColor.White;

        int row = 4;
        foreach (var item in timeline.OrderBy(t => t.Data))
        {
            ws.Cell($"A{row}").Value = item.Label;
            ws.Cell($"B{row}").Value = item.Total;
            ws.Cell($"C{row}").Value = item.Erros;
            ws.Cell($"D{row}").Value = item.Warnings;
            ws.Cell($"E{row}").Value = item.Info;
            row++;
        }

        ws.Columns().AdjustToContents();
    }

    private void ComposeExcelPeakHours(IXLWorksheet ws, List<LogHourAnalysis> peakHours)
    {
        ws.Cell("A1").Value = "ANÁLISE DE HORÁRIOS DE PICO";
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A1").Style.Font.FontSize = 14;

        ws.Cell("A3").Value = "Hora";
        ws.Cell("B3").Value = "Total Erros";
        ws.Cell("C3").Value = "Média";
        ws.Cell("D3").Value = "Pico?";
        ws.Range("A3:D3").Style.Font.Bold = true;
        ws.Range("A3:D3").Style.Fill.BackgroundColor = XLColor.FromHtml(PrimaryColor);
        ws.Range("A3:D3").Style.Font.FontColor = XLColor.White;

        int row = 4;
        foreach (var item in peakHours.OrderBy(h => h.Hora))
        {
            ws.Cell($"A{row}").Value = item.HoraFormatada;
            ws.Cell($"B{row}").Value = item.TotalErros;
            ws.Cell($"C{row}").Value = Math.Round(item.MediaErros, 1);
            ws.Cell($"D{row}").Value = item.IsPico ? "SIM" : "Não";

            if (item.IsPico)
            {
                ws.Range($"A{row}:D{row}").Style.Fill.BackgroundColor = XLColor.FromHtml("#fee2e2");
            }

            row++;
        }

        ws.Columns().AdjustToContents();
    }

    // ====== HELPERS ======
    private static string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
    }

    private static string EscapeCsv(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Replace("\"", "\"\"").Replace("\n", " ").Replace("\r", " ");
    }

    private static string GetRowColor(string tipo)
    {
        return tipo?.ToUpper() switch
        {
            "ERROR" or "ERROR-JS" => "#fee2e2", // Light red
            "WARN" or "WARNING" => "#fef3c7", // Light yellow
            "HTTP-ERROR" => "#fce7f3", // Light pink
            _ => ""
        };
    }
}
