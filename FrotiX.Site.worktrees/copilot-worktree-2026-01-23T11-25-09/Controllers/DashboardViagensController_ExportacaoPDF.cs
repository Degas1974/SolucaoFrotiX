using FrotiX.Data;
using FrotiX.Models;
using FrotiX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using FrotiX.Helpers;

namespace FrotiX.Controllers
{
    /* > ---------------------------------------------------------------------------------------
     > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
     > ---------------------------------------------------------------------------------------
     > 🆔 **Nome:** DashboardViagensController_ExportacaoPDF.cs
     > 📍 **Local:** Controllers
     > ❓ **Por que existo?** Extensão do Dashboard de Viagens para geração de relatórios PDF.
     >                      Processa exportação com gráficos, tabelas e KPIs.
     > 🔗 **Relevância:** Alta (Relatórios Gerenciais)
     > --------------------------------------------------------------------------------------- */

    [Authorize]
    public partial class DashboardViagensController : Controller
    {
        #region Exportação PDF

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExportarParaPDF                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exporta PDF via backend processando dados e KPIs no servidor.             ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Geração oficial de relatório gerencial de viagens.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): filtro início.                                  ║
        /// ║    • dataFim (DateTime?): filtro fim.                                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: arquivo PDF (Stream).                                    ║
        /// ║    • Consumidor: UI de Dashboard de Viagens.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • CriarPagina1Estatisticas()                                               ║
        /// ║    • CriarPagina2Rankings()                                                  ║
        /// ║    • CriarPagina3Complementos()                                              ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /ExportarParaPDF                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Viagens/DashboardViagens.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("ExportarParaPDF")]
        public async Task<IActionResult> ExportarParaPDF(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // [REGRA] Intervalo padrão quando datas não informadas
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                using (PdfDocument document = new PdfDocument())
                {
                    // [DADOS] Configuração do documento
                    document.PageSettings.Size = PdfPageSize.A4;
                    document.PageSettings.Margins.All = 40;

                    // [LOGICA] Geração modular por páginas
                    await CriarPagina1Estatisticas(document, dataInicio.Value, dataFim.Value);
                    await CriarPagina2Rankings(document, dataInicio.Value, dataFim.Value);
                    await CriarPagina3Complementos(document, dataInicio.Value, dataFim.Value);

                    // [DADOS] Salva PDF em memória
                    MemoryStream stream = new MemoryStream();
                    document.Save(stream);
                    stream.Position = 0;

                    // [DADOS] Nome do arquivo
                    string fileName = $"Dashboard_Viagens_{dataInicio.Value:dd-MM-yyyy}_a_{dataFim.Value:dd-MM-yyyy}.pdf";
                    return File(stream, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF");
                Alerta.TratamentoErroComLinha("DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF", ex);
                return Json(new { success = false, message = $"Erro ao gerar PDF: {ex.Message}" });
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExportarParaPDF (POST)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exporta PDF via frontend usando imagens Base64 dos gráficos.              ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Preserva layout visual do dashboard no relatório.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (ExportarDashboardParaPDFViewModel): dados e imagens Base64.      ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: arquivo PDF (Stream).                                    ║
        /// ║    • Consumidor: UI de Dashboard de Viagens.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • CriarPagina1ComCardsVisuais()                                            ║
        /// ║    • CriarPagina2ComGraficos()                                               ║
        /// ║    • CriarPagina3ComGraficos()                                               ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /ExportarParaPDF                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                               ║
        /// ║    • Arquivos relacionados: Pages/Viagens/DashboardViagens.cshtml             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpPost]
        [Route("ExportarParaPDF")]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> ExportarParaPDF([FromBody] ExportarDashboardParaPDFViewModel model)
        {
            try
            {
                // [REGRA] Valida payload
                if (model == null)
                {
                    return BadRequest("Dados inválidos");
                }

                // [LOGICA] Inicializa coleções quando nulas
                if (model.Graficos == null)
                    model.Graficos = new Dictionary<string, string>();

                if (model.Cards == null)
                    model.Cards = new Dictionary<string, string>();

                using (PdfDocument document = new PdfDocument())
                {
                    // [DADOS] Configuração do documento
                    document.PageSettings.Size = PdfPageSize.A4;
                    document.PageSettings.Margins.All = 40;

                    // [LOGICA] Geração modular por páginas
                    await CriarPagina1ComCardsVisuais(document, model.DataInicio, model.DataFim, model.Cards, model.Graficos);
                    await CriarPagina2ComGraficos(document, model.DataInicio, model.DataFim, model.Graficos);
                    await CriarPagina3ComGraficos(document, model.DataInicio, model.DataFim, model.Graficos);

                    // [DADOS] Salva PDF em memória
                    MemoryStream stream = new MemoryStream();
                    document.Save(stream);
                    stream.Position = 0;

                    return File(stream, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF_POST");
                Alerta.TratamentoErroComLinha("DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF_POST", ex);
                return StatusCode(500, new { success = false, message = $"Erro ao gerar PDF: {ex.Message}" });
            }
        }

        #region Métodos Privados - Criar Páginas com Gráficos e Cards

        private async Task CriarPagina1ComCardsVisuais(
            PdfDocument document,
            DateTime dataInicio,
            DateTime dataFim,
            Dictionary<string, string> cards,
            Dictionary<string, string> graficos)
        {
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            PdfColor primaryColor = new PdfColor(13, 110, 253);

            float yPosition = 0;
            float pageWidth = page.GetClientSize().Width;

            PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
                new PointF(0, 0),
                new PointF(pageWidth, 0),
                new PdfColor(13, 110, 253),
                new PdfColor(102, 126, 234)
            );

            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, pageWidth, 60));
            graphics.DrawString("DASHBOARD DE VIAGENS",
                titleFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(20, yPosition + 15));

            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
                regularFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(20, yPosition + 38));

            yPosition += 80;

            graphics.DrawString("ESTATÍSTICAS PRINCIPAIS",
                headerFont,
                new PdfSolidBrush(primaryColor),
                new PointF(0, yPosition));
            yPosition += 25;

            string[] cardIds = new string[]
            {
                "cardCustoTotal", "cardTotalViagens", "cardCustoMedio",
                "cardKmTotal", "cardKmMedio", "cardViagensFinalizadas",
                "cardViagensEmAndamento", "cardViagensAgendadas", "cardViagensCanceladas"
            };

            float cardWidth = (pageWidth - 20) / 3;
            float cardHeight = 70;
            float cardSpacing = 10;

            int row = 0, col = 0;
            foreach (string cardId in cardIds)
            {
                if (cards != null && cards.ContainsKey(cardId) && !string.IsNullOrEmpty(cards[cardId]))
                {
                    try
                    {
                        string base64Data = cards[cardId].Contains(",")
                            ? cards[cardId].Split(',')[1]
                            : cards[cardId];

                        byte[] imageBytes = Convert.FromBase64String(base64Data);

                        using (MemoryStream imageStream = new MemoryStream(imageBytes))
                        {
                            PdfBitmap image = new PdfBitmap(imageStream);

                            float xPos = col * (cardWidth + cardSpacing);
                            float yPos = yPosition + (row * (cardHeight + cardSpacing));

                            graphics.DrawImage(image, new RectangleF(xPos, yPos, cardWidth, cardHeight));
                        }
                    }
                    catch { }
                }

                col++;
                if (col >= 3)
                {
                    col = 0;
                    row++;
                }
            }

            yPosition += (3 * (cardHeight + cardSpacing)) + 20;

            if (graficos != null && graficos.ContainsKey("status") && !string.IsNullOrEmpty(graficos["status"]))
            {
                try
                {
                    graphics.DrawString("VIAGENS POR STATUS",
                        headerFont,
                        new PdfSolidBrush(primaryColor),
                        new PointF(0, yPosition));
                    yPosition += 25;

                    string base64Data = graficos["status"].Contains(",")
                        ? graficos["status"].Split(',')[1]
                        : graficos["status"];

                    byte[] imageBytes = Convert.FromBase64String(base64Data);

                    using (MemoryStream imageStream = new MemoryStream(imageBytes))
                    {
                        PdfBitmap image = new PdfBitmap(imageStream);
                        float graficoWidth = pageWidth * 0.8f;
                        float graficoHeight = 200;
                        float graficoX = (pageWidth - graficoWidth) / 2;

                        graphics.DrawImage(image, new RectangleF(graficoX, yPosition, graficoWidth, graficoHeight));
                    }
                }
                catch (Exception ex)
                {
                    Alerta.TratamentoErroComLinha("DashboardViagensController_ExportacaoPDF.cs", "CriarPagina1ComCardsVisuais_Grafico", ex);
                }
            }
        }

        private async Task CriarPagina2ComGraficos(
            PdfDocument document,
            DateTime dataInicio,
            DateTime dataFim,
            Dictionary<string, string> graficos)
        {
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            PdfColor primaryColor = new PdfColor(13, 110, 253);

            float yPosition = 0;
            float pageWidth = page.GetClientSize().Width;

            PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
                new PointF(0, 0),
                new PointF(pageWidth, 0),
                new PdfColor(13, 110, 253),
                new PdfColor(102, 126, 234)
            );

            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, pageWidth, 60));
            graphics.DrawString("RANKINGS E ANÁLISES",
                titleFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(20, yPosition + 15));

            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
                regularFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(20, yPosition + 38));

            yPosition += 80;

            yPosition = await DesenharGrafico(graphics, graficos, "motoristas", "VIAGENS POR MOTORISTA",
                yPosition, pageWidth, headerFont, primaryColor);

            yPosition = await DesenharGrafico(graphics, graficos, "veiculos", "VIAGENS POR VEÍCULO",
                yPosition, pageWidth, headerFont, primaryColor);

            yPosition = await DesenharGrafico(graphics, graficos, "finalidades", "VIAGENS POR FINALIDADE",
                yPosition, pageWidth, headerFont, primaryColor);
        }

        private async Task CriarPagina3ComGraficos(
            PdfDocument document,
            DateTime dataInicio,
            DateTime dataFim,
            Dictionary<string, string> graficos)
        {
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            PdfColor primaryColor = new PdfColor(13, 110, 253);

            float yPosition = 0;
            float pageWidth = page.GetClientSize().Width;

            PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
                new PointF(0, 0),
                new PointF(pageWidth, 0),
                new PdfColor(13, 110, 253),
                new PdfColor(102, 126, 234)
            );

            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, pageWidth, 60));
            graphics.DrawString("ANÁLISES COMPLEMENTARES",
                titleFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(20, yPosition + 15));

            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
                regularFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(20, yPosition + 38));

            yPosition += 80;

            yPosition = await DesenharGrafico(graphics, graficos, "requisitantes", "VIAGENS POR REQUISITANTE",
                yPosition, pageWidth, headerFont, primaryColor);

            yPosition = await DesenharGrafico(graphics, graficos, "setores", "VIAGENS POR SETOR",
                yPosition, pageWidth, headerFont, primaryColor);
        }

        private async Task<float> DesenharGrafico(
            PdfGraphics graphics,
            Dictionary<string, string> graficos,
            string chaveGrafico,
            string titulo,
            float yPosition,
            float pageWidth,
            PdfFont headerFont,
            PdfColor primaryColor)
        {
            if (graficos != null && graficos.ContainsKey(chaveGrafico) && !string.IsNullOrEmpty(graficos[chaveGrafico]))
            {
                try
                {
                    graphics.DrawString(titulo,
                        headerFont,
                        new PdfSolidBrush(primaryColor),
                        new PointF(0, yPosition));
                    yPosition += 25;

                    string base64Data = graficos[chaveGrafico].Contains(",")
                        ? graficos[chaveGrafico].Split(',')[1]
                        : graficos[chaveGrafico];

                    byte[] imageBytes = Convert.FromBase64String(base64Data);

                    using (MemoryStream imageStream = new MemoryStream(imageBytes))
                    {
                        PdfBitmap image = new PdfBitmap(imageStream);
                        float graficoWidth = pageWidth;
                        float graficoHeight = 180;

                        graphics.DrawImage(image, new RectangleF(0, yPosition, graficoWidth, graficoHeight));
                        yPosition += graficoHeight + 30;
                    }
                }
                catch { }
            }

            return yPosition;
        }

        #endregion Métodos Privados - Criar Páginas com Gráficos e Cards

        private async Task CriarPagina1Estatisticas(PdfDocument document, DateTime dataInicio, DateTime dataFim)
        {
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);

            PdfColor primaryColor = new PdfColor(13, 110, 253);
            PdfColor grayColor = new PdfColor(108, 117, 125);
            PdfColor lightGray = new PdfColor(248, 249, 250);

            float yPosition = 0;

            PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
                new PointF(0, 0),
                new PointF(page.GetClientSize().Width, 0),
                new PdfColor(13, 110, 253),
                new PdfColor(102, 126, 234)
            );

            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, page.GetClientSize().Width, 60));

            graphics.DrawString("DASHBOARD DE VIAGENS",
                titleFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(10, yPosition + 15));

            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
                regularFont,
                new PdfSolidBrush(new PdfColor(255, 255, 255)),
                new PointF(10, yPosition + 38));

            yPosition += 80;

            var viagens = await _context.Viagem
                .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                .ToListAsync();

            int totalViagens = viagens.Count;
            int viagensFinalizadas = viagens.Count(v => v.Status == "Realizada");
            int viagensEmAndamento = viagens.Count(v => v.Status == "Aberta");
            int viagensAgendadas = viagens.Count(v => v.Status == "Agendada");
            int viagensCanceladas = viagens.Count(v => v.Status == "Cancelada");

            decimal custoTotal = (decimal)(viagens.Sum(v => v.CustoCombustivel ?? 0) +
                                viagens.Sum(v => v.CustoLavador ?? 0) +
                                viagens.Sum(v => v.CustoMotorista ?? 0) +
                                viagens.Sum(v => v.CustoOperador ?? 0) +
                                viagens.Sum(v => v.CustoVeiculo ?? 0));

            decimal custoCombustivel = (decimal)viagens.Sum(v => v.CustoCombustivel ?? 0);
            decimal custoLavador = (decimal)viagens.Sum(v => v.CustoLavador ?? 0);
            decimal custoMotorista = (decimal)viagens.Sum(v => v.CustoMotorista ?? 0);
            decimal custoOperador = (decimal)viagens.Sum(v => v.CustoOperador ?? 0);
            decimal custoVeiculo = (decimal)viagens.Sum(v => v.CustoVeiculo ?? 0);

            decimal kmTotal = viagens.Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                                    .Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0));

            decimal custoMedioPorViagem = totalViagens > 0 ? custoTotal / totalViagens : 0;
            decimal kmMedioPorViagem = totalViagens > 0 ? kmTotal / totalViagens : 0;

            graphics.DrawString("ESTATÍSTICAS PRINCIPAIS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            PdfGrid gridEstatisticas = new PdfGrid();
            gridEstatisticas.Columns.Add(2);
            gridEstatisticas.Columns[0].Width = 250;
            gridEstatisticas.Columns[1].Width = page.GetClientSize().Width - 250;

            var estatisticas = new Dictionary<string, string>
            {
                { "Total de Viagens", totalViagens.ToString("N0") },
                { "Viagens Finalizadas", viagensFinalizadas.ToString("N0") },
                { "Viagens Em Andamento", viagensEmAndamento.ToString("N0") },
                { "Viagens Agendadas", viagensAgendadas.ToString("N0") },
                { "Viagens Canceladas", viagensCanceladas.ToString("N0") },
                { "Quilometragem Total", $"{kmTotal:N0} km" },
                { "Quilometragem Média por Viagem", $"{kmMedioPorViagem:N1} km" },
                { "Custo Médio por Viagem", $"R$ {custoMedioPorViagem:N2}" }
            };

            foreach (var stat in estatisticas)
            {
                PdfGridRow row = gridEstatisticas.Rows.Add();
                row.Cells[0].Value = stat.Key;
                row.Cells[1].Value = stat.Value;
                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
            }

            ApplyGridStyle(gridEstatisticas, primaryColor, lightGray);
            var result = gridEstatisticas.Draw(page, new PointF(0, yPosition));
            yPosition = result.Bounds.Bottom + 20;

            graphics.DrawString("CUSTOS DETALHADOS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            PdfGrid gridCustos = new PdfGrid();
            gridCustos.Columns.Add(2);
            gridCustos.Columns[0].Width = 250;
            gridCustos.Columns[1].Width = page.GetClientSize().Width - 250;

            var custos = new Dictionary<string, string>
            {
                { "Custo Total", $"R$ {custoTotal:N2}" },
                { "Custo com Combustível", $"R$ {custoCombustivel:N2}" },
                { "Custo com Lavador", $"R$ {custoLavador:N2}" },
                { "Custo com Motorista", $"R$ {custoMotorista:N2}" },
                { "Custo com Operador", $"R$ {custoOperador:N2}" },
                { "Custo com Veículo", $"R$ {custoVeiculo:N2}" }
            };

            foreach (var custo in custos)
            {
                PdfGridRow row = gridCustos.Rows.Add();
                row.Cells[0].Value = custo.Key;
                row.Cells[1].Value = custo.Value;
                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);

                if (custo.Key == "Custo Total")
                {
                    row.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(22, 163, 74));
                }
            }

            ApplyGridStyle(gridCustos, primaryColor, lightGray);
            result = gridCustos.Draw(page, new PointF(0, yPosition));
            yPosition = result.Bounds.Bottom + 20;

            graphics.DrawString("DISTRIBUIÇÃO POR STATUS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            PdfGrid gridStatus = new PdfGrid();
            gridStatus.Columns.Add(2);
            gridStatus.Columns[0].Width = 250;
            gridStatus.Columns[1].Width = page.GetClientSize().Width - 250;

            var statusData = new Dictionary<string, (int count, PdfColor color)>
            {
                { "Finalizadas", (viagensFinalizadas, new PdfColor(22, 163, 74)) },
                { "Em Andamento", (viagensEmAndamento, new PdfColor(13, 110, 253)) },
                { "Agendadas", (viagensAgendadas, new PdfColor(245, 158, 11)) },
                { "Canceladas", (viagensCanceladas, new PdfColor(220, 38, 38)) }
            };

            foreach (var status in statusData)
            {
                double percentual = totalViagens > 0 ? (double)status.Value.count / totalViagens * 100 : 0;

                PdfGridRow row = gridStatus.Rows.Add();
                row.Cells[0].Value = status.Key;
                row.Cells[1].Value = $"{status.Value.count:N0} ({percentual:N1}%)";
                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
                row.Cells[1].Style.TextBrush = new PdfSolidBrush(status.Value.color);
            }

            ApplyGridStyle(gridStatus, primaryColor, lightGray);
            result = gridStatus.Draw(page, new PointF(0, yPosition));

            string rodape = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm} | FrotiX - Sistema de Gestão de Frotas | Página 1/3";
            graphics.DrawString(rodape, smallFont, new PdfSolidBrush(grayColor),
                new PointF(0, page.GetClientSize().Height - 20));
        }

        private async Task CriarPagina2Rankings(PdfDocument document, DateTime dataInicio, DateTime dataFim)
        {
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);

            PdfColor primaryColor = new PdfColor(13, 110, 253);
            PdfColor grayColor = new PdfColor(108, 117, 125);
            PdfColor lightGray = new PdfColor(248, 249, 250);
            PdfColor cianoColor = new PdfColor(34, 211, 238);
            PdfColor laranjaColor = new PdfColor(217, 119, 6);
            PdfColor verdeColor = new PdfColor(22, 163, 74);

            float yPosition = 0;

            graphics.DrawString("RANKINGS E DETALHAMENTOS", titleFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 30;

            var viagens = await _context.Viagem
                .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                .Include(v => v.Motorista)
                .Include(v => v.Finalidade)
                .ToListAsync();

            var veiculosInfo = await _context.ViewVeiculos
                .Select(v => new { v.VeiculoId, v.Placa, v.MarcaModelo })
                .ToListAsync();

            graphics.DrawString("TOP 10 MOTORISTAS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            var topMotoristas = viagens
                .Where(v => v.Motorista != null)
                .GroupBy(v => new { v.MotoristaId, NomeMotorista = v.Motorista.Nome })
                .Select(g => new
                {
                    Motorista = g.Key.NomeMotorista ?? "Não informado",
                    TotalViagens = g.Count(),
                    CustoTotal = g.Sum(v => (v.CustoCombustivel ?? 0) + (v.CustoLavador ?? 0) +
                                           (v.CustoMotorista ?? 0) + (v.CustoOperador ?? 0) +
                                           (v.CustoVeiculo ?? 0))
                })
                .OrderByDescending(x => x.TotalViagens)
                .Take(10)
                .ToList();

            PdfGrid gridMotoristas = new PdfGrid();
            gridMotoristas.Columns.Add(3);
            gridMotoristas.Columns[0].Width = 250;
            gridMotoristas.Columns[1].Width = 100;
            gridMotoristas.Columns[2].Width = page.GetClientSize().Width - 350;

            PdfGridRow headerRow = gridMotoristas.Headers.Add(1)[0];
            headerRow.Cells[0].Value = "Motorista";
            headerRow.Cells[1].Value = "Viagens";
            headerRow.Cells[2].Value = "Custo Total";

            foreach (var motorista in topMotoristas)
            {
                PdfGridRow row = gridMotoristas.Rows.Add();
                row.Cells[0].Value = motorista.Motorista;
                row.Cells[1].Value = motorista.TotalViagens.ToString("N0");
                row.Cells[2].Value = $"R$ {motorista.CustoTotal:N2}";
            }

            ApplyGridStyle(gridMotoristas, cianoColor, lightGray);
            var result = gridMotoristas.Draw(page, new PointF(0, yPosition));
            yPosition = result.Bounds.Bottom + 20;

            graphics.DrawString("TOP 10 VEÍCULOS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            var topVeiculos = viagens
                .Where(v => v.VeiculoId != null)
                .GroupBy(v => v.VeiculoId)
                .Select(g => new
                {
                    VeiculoId = g.Key,
                    TotalViagens = g.Count(),
                    KmTotal = g.Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                               .Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                })
                .OrderByDescending(x => x.TotalViagens)
                .Take(10)
                .ToList();

            var topVeiculosComInfo = topVeiculos
                .Select(tv =>
                {
                    var veiculo = veiculosInfo.FirstOrDefault(vi => vi.VeiculoId == tv.VeiculoId);
                    return new
                    {
                        Veiculo = veiculo != null ?
                            $"{veiculo.Placa} - {veiculo.MarcaModelo}" :
                            "Veículo não encontrado",
                        tv.TotalViagens,
                        tv.KmTotal
                    };
                })
                .ToList();

            PdfGrid gridVeiculos = new PdfGrid();
            gridVeiculos.Columns.Add(3);
            gridVeiculos.Columns[0].Width = 250;
            gridVeiculos.Columns[1].Width = 100;
            gridVeiculos.Columns[2].Width = page.GetClientSize().Width - 350;

            headerRow = gridVeiculos.Headers.Add(1)[0];
            headerRow.Cells[0].Value = "Veículo";
            headerRow.Cells[1].Value = "Viagens";
            headerRow.Cells[2].Value = "KM Total";

            foreach (var veiculo in topVeiculosComInfo)
            {
                PdfGridRow row = gridVeiculos.Rows.Add();
                row.Cells[0].Value = veiculo.Veiculo;
                row.Cells[1].Value = veiculo.TotalViagens.ToString("N0");
                row.Cells[2].Value = $"{veiculo.KmTotal:N0} km";
            }

            ApplyGridStyle(gridVeiculos, laranjaColor, lightGray);
            result = gridVeiculos.Draw(page, new PointF(0, yPosition));
            yPosition = result.Bounds.Bottom + 20;

            graphics.DrawString("TOP 10 FINALIDADES", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            var topFinalidades = viagens
                .Where(v => v.Finalidade != null)
                .GroupBy(v => new { v.Finalidade, Descricao = v.Finalidade })
                .Select(g => new
                {
                    Finalidade = g.Key.Descricao ?? "Não informada",
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(10)
                .ToList();

            PdfGrid gridFinalidades = new PdfGrid();
            gridFinalidades.Columns.Add(2);
            gridFinalidades.Columns[0].Width = 350;
            gridFinalidades.Columns[1].Width = page.GetClientSize().Width - 350;

            headerRow = gridFinalidades.Headers.Add(1)[0];
            headerRow.Cells[0].Value = "Finalidade";
            headerRow.Cells[1].Value = "Total";

            foreach (var finalidade in topFinalidades)
            {
                PdfGridRow row = gridFinalidades.Rows.Add();
                row.Cells[0].Value = finalidade.Finalidade;
                row.Cells[1].Value = finalidade.Total.ToString("N0");
            }

            ApplyGridStyle(gridFinalidades, verdeColor, lightGray);
            result = gridFinalidades.Draw(page, new PointF(0, yPosition));

            string rodape = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm} | FrotiX - Sistema de Gestão de Frotas | Página 2/3";
            graphics.DrawString(rodape, smallFont, new PdfSolidBrush(grayColor),
                new PointF(0, page.GetClientSize().Height - 20));
        }

        private async Task CriarPagina3Complementos(PdfDocument document, DateTime dataInicio, DateTime dataFim)
        {
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);

            PdfColor primaryColor = new PdfColor(13, 110, 253);
            PdfColor grayColor = new PdfColor(108, 117, 125);
            PdfColor lightGray = new PdfColor(248, 249, 250);
            PdfColor greenColor = new PdfColor(22, 163, 74);
            PdfColor rosaColor = new PdfColor(236, 72, 153);
            PdfColor amareloColor = new PdfColor(245, 158, 11);

            float yPosition = 0;

            graphics.DrawString("DETALHAMENTOS COMPLEMENTARES", titleFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 30;

            graphics.DrawString("TOP 10 VIAGENS MAIS CARAS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            var viagensMaisCaras = await _context.ViewViagens
                .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                .OrderByDescending(v => v.CustoViagem)
                .Take(10)
                .ToListAsync();

            PdfGrid gridViagensCaras = new PdfGrid();
            gridViagensCaras.Columns.Add(7);
            gridViagensCaras.Columns[0].Width = 30;  
            gridViagensCaras.Columns[1].Width = 60;  
            gridViagensCaras.Columns[2].Width = 75;  
            gridViagensCaras.Columns[3].Width = 75;  
            gridViagensCaras.Columns[4].Width = 100; 
            gridViagensCaras.Columns[5].Width = 100; 
            gridViagensCaras.Columns[6].Width = 75;  

            PdfGridRow headerRow = gridViagensCaras.Headers.Add(1)[0];
            headerRow.Cells[0].Value = "#";
            headerRow.Cells[1].Value = "Nº Ficha";
            headerRow.Cells[2].Value = "Data Inicial";
            headerRow.Cells[3].Value = "Data Final";
            headerRow.Cells[4].Value = "Motorista";
            headerRow.Cells[5].Value = "Veículo";
            headerRow.Cells[6].Value = "Custo Total";

            int contador = 1;
            foreach (var viagem in viagensMaisCaras)
            {
                PdfGridRow row = gridViagensCaras.Rows.Add();
                row.Cells[0].Value = contador.ToString();
                row.Cells[1].Value = viagem.NoFichaVistoria?.ToString() ?? "N/A";
                row.Cells[2].Value = viagem.DataInicial?.ToString("dd/MM/yyyy") ?? "N/A";
                row.Cells[3].Value = viagem.DataFinal?.ToString("dd/MM/yyyy") ?? "N/A";
                row.Cells[4].Value = viagem.NomeMotorista ?? "Não informado";
                row.Cells[5].Value = viagem.DescricaoVeiculo ?? "Não informado";
                row.Cells[6].Value = $"R$ {(viagem.CustoViagem ?? 0):N2}";

                row.Cells[6].Style.TextBrush = new PdfSolidBrush(greenColor);
                row.Cells[6].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9, PdfFontStyle.Bold);

                contador++;
            }

            ApplyGridStyle(gridViagensCaras, primaryColor, lightGray);
            var result = gridViagensCaras.Draw(page, new PointF(0, yPosition));
            yPosition = result.Bounds.Bottom + 20;

            graphics.DrawString("TOP 10 REQUISITANTES", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            var viagens = await _context.Viagem
                .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim && v.RequisitanteId != null)
                .Include(v => v.Requisitante)
                .Include(v => v.SetorSolicitante)
                .ToListAsync();

            var topRequisitantes = viagens
                .Where(v => v.Requisitante != null && v.Requisitante.Nome != "Coordenação de Transportes (Ctran)")
                .GroupBy(v => new { v.RequisitanteId, Nome = v.Requisitante.Nome })
                .Select(g => new
                {
                    Requisitante = g.Key.Nome ?? "Não informado",
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(10)
                .ToList();

            PdfGrid gridRequisitantes = new PdfGrid();
            gridRequisitantes.Columns.Add(2);
            gridRequisitantes.Columns[0].Width = 400;
            gridRequisitantes.Columns[1].Width = page.GetClientSize().Width - 400;

            headerRow = gridRequisitantes.Headers.Add(1)[0];
            headerRow.Cells[0].Value = "Requisitante";
            headerRow.Cells[1].Value = "Total de Viagens";
            headerRow.Cells[0].Style.BackgroundBrush = new PdfSolidBrush(rosaColor);
            headerRow.Cells[1].Style.BackgroundBrush = new PdfSolidBrush(rosaColor);
            headerRow.Cells[0].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
            headerRow.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));

            foreach (var req in topRequisitantes)
            {
                PdfGridRow row = gridRequisitantes.Rows.Add();
                row.Cells[0].Value = req.Requisitante;
                row.Cells[1].Value = req.Total.ToString("N0");
            }

            ApplyGridStyle(gridRequisitantes, rosaColor, lightGray);
            result = gridRequisitantes.Draw(page, new PointF(0, yPosition));
            yPosition = result.Bounds.Bottom + 20;

            if (yPosition > page.GetClientSize().Height - 200)
            {
                page = document.Pages.Add();
                graphics = page.Graphics;
                yPosition = 0;
            }

            graphics.DrawString("TOP 10 SETORES", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
            yPosition += 20;

            var topSetores = viagens
                .Where(v => v.SetorSolicitante != null && v.SetorSolicitante.Nome != "Coordenação de Transportes")
                .GroupBy(v => new { v.SetorSolicitanteId, Nome = v.SetorSolicitante.Nome })
                .Select(g => new
                {
                    Setor = g.Key.Nome ?? "Não informado",
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(10)
                .ToList();

            PdfGrid gridSetores = new PdfGrid();
            gridSetores.Columns.Add(2);
            gridSetores.Columns[0].Width = 400;
            gridSetores.Columns[1].Width = page.GetClientSize().Width - 400;

            headerRow = gridSetores.Headers.Add(1)[0];
            headerRow.Cells[0].Value = "Setor";
            headerRow.Cells[1].Value = "Total de Viagens";
            headerRow.Cells[0].Style.BackgroundBrush = new PdfSolidBrush(amareloColor);
            headerRow.Cells[1].Style.BackgroundBrush = new PdfSolidBrush(amareloColor);
            headerRow.Cells[0].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
            headerRow.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));

            foreach (var setor in topSetores)
            {
                PdfGridRow row = gridSetores.Rows.Add();
                row.Cells[0].Value = setor.Setor;
                row.Cells[1].Value = setor.Total.ToString("N0");
            }

            ApplyGridStyle(gridSetores, amareloColor, lightGray);
            result = gridSetores.Draw(page, new PointF(0, yPosition));

            string rodape = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm} | FrotiX - Sistema de Gestão de Frotas | Página 3/3";
            graphics.DrawString(rodape, smallFont, new PdfSolidBrush(grayColor),
                new PointF(0, page.GetClientSize().Height - 20));
        }

        private void ApplyGridStyle(PdfGrid grid, PdfColor headerColor, PdfColor alternateRowColor)
        {
            if (grid.Headers.Count > 0)
            {
                PdfGridRow headerRow = grid.Headers[0];
                foreach (PdfGridCell cell in headerRow.Cells)
                {
                    if (cell.Style.BackgroundBrush == null)
                    {
                        cell.Style.BackgroundBrush = new PdfSolidBrush(headerColor);
                        cell.Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
                    }
                    else
                    {
                        cell.Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
                    }

                    cell.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
                    cell.Style.Borders.All = new PdfPen(new PdfColor(200, 200, 200), 0.5f);
                }
            }

            bool alternate = false;
            foreach (PdfGridRow row in grid.Rows)
            {
                foreach (PdfGridCell cell in row.Cells)
                {
                    if (alternate)
                    {
                        cell.Style.BackgroundBrush = new PdfSolidBrush(alternateRowColor);
                    }
                    
                    if (cell.Style.Font == null)
                        cell.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);

                    cell.Style.Borders.All = new PdfPen(new PdfColor(200, 200, 200), 0.5f);
                }
                alternate = !alternate;
            }
        }

        #endregion Exportação PDF
    }
}
