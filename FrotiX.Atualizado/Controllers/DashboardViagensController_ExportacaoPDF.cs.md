# Controllers/DashboardViagensController_ExportacaoPDF.cs

**Mudanca:** GRANDE | **+283** linhas | **-242** linhas

---

```diff
--- JANEIRO: Controllers/DashboardViagensController_ExportacaoPDF.cs
+++ ATUAL: Controllers/DashboardViagensController_ExportacaoPDF.cs
@@ -1,6 +1,7 @@
+using FrotiX.Helpers;
+
 using FrotiX.Data;
 using FrotiX.Models;
-using FrotiX.ViewModels;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Identity;
 using Microsoft.AspNetCore.Mvc;
@@ -14,7 +15,6 @@
 using Syncfusion.Pdf;
 using Syncfusion.Pdf.Graphics;
 using Syncfusion.Pdf.Grid;
-using FrotiX.Helpers;
 
 namespace FrotiX.Controllers
 {
@@ -25,7 +25,7 @@
 
         [HttpGet]
         [Route("ExportarParaPDF")]
-        public async Task<IActionResult> ExportarParaPDF(DateTime? dataInicio, DateTime? dataFim)
+        public async Task<IActionResult> ExportarParaPDF(DateTime? dataInicio , DateTime? dataFim)
         {
             try
             {
@@ -42,114 +42,153 @@
                     document.PageSettings.Size = PdfPageSize.A4;
                     document.PageSettings.Margins.All = 40;
 
-                    await CriarPagina1Estatisticas(document, dataInicio.Value, dataFim.Value);
-                    await CriarPagina2Rankings(document, dataInicio.Value, dataFim.Value);
-                    await CriarPagina3Complementos(document, dataInicio.Value, dataFim.Value);
+                    await CriarPagina1Estatisticas(document , dataInicio.Value , dataFim.Value);
+
+                    await CriarPagina2Rankings(document , dataInicio.Value , dataFim.Value);
+
+                    await CriarPagina3Complementos(document , dataInicio.Value , dataFim.Value);
 
                     MemoryStream stream = new MemoryStream();
                     document.Save(stream);
                     stream.Position = 0;
 
                     string fileName = $"Dashboard_Viagens_{dataInicio.Value:dd-MM-yyyy}_a_{dataFim.Value:dd-MM-yyyy}.pdf";
-                    return File(stream, "application/pdf", fileName);
+                    return File(stream , "application/pdf" , fileName);
                 }
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF");
-                Alerta.TratamentoErroComLinha("DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF", ex);
-                return Json(new { success = false, message = $"Erro ao gerar PDF: {ex.Message}" });
+                return Json(new { success = false , message = $"Erro ao gerar PDF: {ex.Message}" });
             }
         }
 
         [HttpPost]
         [Route("ExportarParaPDF")]
         [RequestSizeLimit(104857600)]
-        public async Task<IActionResult> ExportarParaPDF([FromBody] ExportarDashboardParaPDFViewModel model)
+        public async Task<IActionResult> ExportarParaPDF([FromBody] FrotiX.ViewModels.ExportarDashboardParaPDFViewModel model)
         {
             try
             {
+                Console.WriteLine("🔍 ===== INÍCIO ExportarParaPDF POST =====");
+                Console.WriteLine($"📅 DataInicio: {model?.DataInicio}");
+                Console.WriteLine($"📅 DataFim: {model?.DataFim}");
+                Console.WriteLine($"📊 Graficos.Count: {model?.Graficos?.Count ?? 0}");
+                Console.WriteLine($"🎨 Cards.Count: {model?.Cards?.Count ?? 0}");
 
                 if (model == null)
                 {
+                    Console.WriteLine("❌ Model é null!");
                     return BadRequest("Dados inválidos");
                 }
 
                 if (model.Graficos == null)
-                    model.Graficos = new Dictionary<string, string>();
+                {
+                    Console.WriteLine("⚠️ Graficos é null, criando dicionário vazio");
+                    model.Graficos = new Dictionary<string , string>();
+                }
 
                 if (model.Cards == null)
-                    model.Cards = new Dictionary<string, string>();
-
+                {
+                    Console.WriteLine("⚠️ Cards é null, criando dicionário vazio");
+                    model.Cards = new Dictionary<string , string>();
+                }
+
+                Console.WriteLine("📊 Gráficos recebidos:");
+                foreach (var grafico in model.Graficos)
+                {
+                    var tamanhoKB = (grafico.Value?.Length ?? 0) / 1024;
+                    Console.WriteLine($" - {grafico.Key}: {tamanhoKB} KB");
+                }
+
+                Console.WriteLine("🎨 Cards recebidos:");
+                foreach (var card in model.Cards)
+                {
+                    var tamanhoKB = (card.Value?.Length ?? 0) / 1024;
+                    Console.WriteLine($" - {card.Key}: {tamanhoKB} KB");
+                }
+
+                Console.WriteLine("📄 Criando documento PDF...");
                 using (PdfDocument document = new PdfDocument())
                 {
 
                     document.PageSettings.Size = PdfPageSize.A4;
                     document.PageSettings.Margins.All = 40;
 
-                    await CriarPagina1ComCardsVisuais(document, model.DataInicio, model.DataFim, model.Cards, model.Graficos);
-                    await CriarPagina2ComGraficos(document, model.DataInicio, model.DataFim, model.Graficos);
-                    await CriarPagina3ComGraficos(document, model.DataInicio, model.DataFim, model.Graficos);
-
+                    Console.WriteLine("📄 Criando Página 1 (Cards + Status)...");
+                    await CriarPagina1ComCardsVisuais(document , model.DataInicio , model.DataFim , model.Cards , model.Graficos);
+
+                    Console.WriteLine("📄 Criando Página 2 (Rankings)...");
+                    await CriarPagina2ComGraficos(document , model.DataInicio , model.DataFim , model.Graficos);
+
+                    Console.WriteLine("📄 Criando Página 3 (Complementos)...");
+                    await CriarPagina3ComGraficos(document , model.DataInicio , model.DataFim , model.Graficos);
+
+                    Console.WriteLine("💾 Salvando PDF...");
                     MemoryStream stream = new MemoryStream();
                     document.Save(stream);
                     stream.Position = 0;
 
-                    return File(stream, "application/pdf");
+                    Console.WriteLine($"✅ PDF gerado com sucesso! Tamanho: {stream.Length} bytes");
+                    Console.WriteLine("🔍 ===== FIM ExportarParaPDF POST =====");
+
+                    return File(stream , "application/pdf");
                 }
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF_POST");
-                Alerta.TratamentoErroComLinha("DashboardViagensController_ExportacaoPDF.cs", "ExportarParaPDF_POST", ex);
-                return StatusCode(500, new { success = false, message = $"Erro ao gerar PDF: {ex.Message}" });
+                Console.WriteLine("❌ ===== ERRO ExportarParaPDF POST =====");
+                Console.WriteLine($"❌ Mensagem: {ex.Message}");
+                Console.WriteLine($"❌ Stack: {ex.StackTrace}");
+                return StatusCode(500 , new { success = false , message = $"Erro ao gerar PDF: {ex.Message}" });
             }
         }
 
         private async Task CriarPagina1ComCardsVisuais(
-            PdfDocument document,
-            DateTime dataInicio,
-            DateTime dataFim,
-            Dictionary<string, string> cards,
-            Dictionary<string, string> graficos)
+            PdfDocument document ,
+            DateTime dataInicio ,
+            DateTime dataFim ,
+            Dictionary<string , string> cards ,
+            Dictionary<string , string> graficos)
         {
             PdfPage page = document.Pages.Add();
             PdfGraphics graphics = page.Graphics;
 
-            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
-            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
-
-            PdfColor primaryColor = new PdfColor(13, 110, 253);
+            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica , 16 , PdfFontStyle.Bold);
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 10);
+
+            PdfColor primaryColor = new PdfColor(13 , 110 , 253);
 
             float yPosition = 0;
             float pageWidth = page.GetClientSize().Width;
 
             PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
-                new PointF(0, 0),
-                new PointF(pageWidth, 0),
-                new PdfColor(13, 110, 253),
-                new PdfColor(102, 126, 234)
+                new PointF(0 , 0) ,
+                new PointF(pageWidth , 0) ,
+                new PdfColor(13 , 110 , 253) ,
+                new PdfColor(102 , 126 , 234)
             );
 
-            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, pageWidth, 60));
-            graphics.DrawString("DASHBOARD DE VIAGENS",
-                titleFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(20, yPosition + 15));
-
-            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
-                regularFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(20, yPosition + 38));
+            graphics.DrawRectangle(gradientBrush , new RectangleF(0 , yPosition , pageWidth , 60));
+            graphics.DrawString("DASHBOARD DE VIAGENS" ,
+                titleFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(20 , yPosition + 15));
+
+            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}" ,
+                regularFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(20 , yPosition + 38));
 
             yPosition += 80;
 
-            graphics.DrawString("ESTATÍSTICAS PRINCIPAIS",
-                headerFont,
-                new PdfSolidBrush(primaryColor),
-                new PointF(0, yPosition));
+            graphics.DrawString("ESTATÍSTICAS PRINCIPAIS" ,
+                headerFont ,
+                new PdfSolidBrush(primaryColor) ,
+                new PointF(0 , yPosition));
             yPosition += 25;
+
+            Console.WriteLine($"🎨 Total de cards recebidos: {cards?.Count ?? 0}");
 
             string[] cardIds = new string[]
             {
@@ -169,6 +208,8 @@
                 {
                     try
                     {
+                        Console.WriteLine($"🎨 Desenhando card: {cardId}");
+
                         string base64Data = cards[cardId].Contains(",")
                             ? cards[cardId].Split(',')[1]
                             : cards[cardId];
@@ -182,10 +223,19 @@
                             float xPos = col * (cardWidth + cardSpacing);
                             float yPos = yPosition + (row * (cardHeight + cardSpacing));
 
-                            graphics.DrawImage(image, new RectangleF(xPos, yPos, cardWidth, cardHeight));
+                            graphics.DrawImage(image , new RectangleF(xPos , yPos , cardWidth , cardHeight));
+
+                            Console.WriteLine($"✅ Card {cardId} desenhado em ({xPos}, {yPos}) - {cardWidth}x{cardHeight}");
                         }
                     }
-                    catch { }
+                    catch (Exception ex)
+                    {
+                        Console.WriteLine($"❌ Erro ao desenhar card {cardId}: {ex.Message}");
+                    }
+                }
+                else
+                {
+                    Console.WriteLine($"⚠️ Card {cardId} não encontrado ou vazio");
                 }
 
                 col++;
@@ -202,10 +252,12 @@
             {
                 try
                 {
-                    graphics.DrawString("VIAGENS POR STATUS",
-                        headerFont,
-                        new PdfSolidBrush(primaryColor),
-                        new PointF(0, yPosition));
+                    Console.WriteLine("📊 Desenhando gráfico de Status...");
+
+                    graphics.DrawString("VIAGENS POR STATUS" ,
+                        headerFont ,
+                        new PdfSolidBrush(primaryColor) ,
+                        new PointF(0 , yPosition));
                     yPosition += 25;
 
                     string base64Data = graficos["status"].Contains(",")
@@ -221,127 +273,130 @@
                         float graficoHeight = 200;
                         float graficoX = (pageWidth - graficoWidth) / 2;
 
-                        graphics.DrawImage(image, new RectangleF(graficoX, yPosition, graficoWidth, graficoHeight));
+                        graphics.DrawImage(image , new RectangleF(graficoX , yPosition , graficoWidth , graficoHeight));
+                        Console.WriteLine($"✅ Gráfico Status desenhado");
                     }
                 }
                 catch (Exception ex)
                 {
-                    Alerta.TratamentoErroComLinha("DashboardViagensController_ExportacaoPDF.cs", "CriarPagina1ComCardsVisuais_Grafico", ex);
+                    Console.WriteLine($"❌ Erro ao desenhar gráfico Status: {ex.Message}");
                 }
             }
         }
 
         private async Task CriarPagina2ComGraficos(
-            PdfDocument document,
-            DateTime dataInicio,
-            DateTime dataFim,
-            Dictionary<string, string> graficos)
+            PdfDocument document ,
+            DateTime dataInicio ,
+            DateTime dataFim ,
+            Dictionary<string , string> graficos)
         {
             PdfPage page = document.Pages.Add();
             PdfGraphics graphics = page.Graphics;
 
-            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
-            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
-
-            PdfColor primaryColor = new PdfColor(13, 110, 253);
+            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica , 16 , PdfFontStyle.Bold);
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 10);
+
+            PdfColor primaryColor = new PdfColor(13 , 110 , 253);
 
             float yPosition = 0;
             float pageWidth = page.GetClientSize().Width;
 
             PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
-                new PointF(0, 0),
-                new PointF(pageWidth, 0),
-                new PdfColor(13, 110, 253),
-                new PdfColor(102, 126, 234)
+                new PointF(0 , 0) ,
+                new PointF(pageWidth , 0) ,
+                new PdfColor(13 , 110 , 253) ,
+                new PdfColor(102 , 126 , 234)
             );
 
-            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, pageWidth, 60));
-            graphics.DrawString("RANKINGS E ANÁLISES",
-                titleFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(20, yPosition + 15));
-
-            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
-                regularFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(20, yPosition + 38));
+            graphics.DrawRectangle(gradientBrush , new RectangleF(0 , yPosition , pageWidth , 60));
+            graphics.DrawString("RANKINGS E ANÁLISES" ,
+                titleFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(20 , yPosition + 15));
+
+            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}" ,
+                regularFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(20 , yPosition + 38));
 
             yPosition += 80;
 
-            yPosition = await DesenharGrafico(graphics, graficos, "motoristas", "VIAGENS POR MOTORISTA",
-                yPosition, pageWidth, headerFont, primaryColor);
-
-            yPosition = await DesenharGrafico(graphics, graficos, "veiculos", "VIAGENS POR VEÍCULO",
-                yPosition, pageWidth, headerFont, primaryColor);
-
-            yPosition = await DesenharGrafico(graphics, graficos, "finalidades", "VIAGENS POR FINALIDADE",
-                yPosition, pageWidth, headerFont, primaryColor);
+            yPosition = await DesenharGrafico(graphics , graficos , "motoristas" , "VIAGENS POR MOTORISTA" ,
+                yPosition , pageWidth , headerFont , primaryColor);
+
+            yPosition = await DesenharGrafico(graphics , graficos , "veiculos" , "VIAGENS POR VEÍCULO" ,
+                yPosition , pageWidth , headerFont , primaryColor);
+
+            yPosition = await DesenharGrafico(graphics , graficos , "finalidades" , "VIAGENS POR FINALIDADE" ,
+                yPosition , pageWidth , headerFont , primaryColor);
         }
 
         private async Task CriarPagina3ComGraficos(
-            PdfDocument document,
-            DateTime dataInicio,
-            DateTime dataFim,
-            Dictionary<string, string> graficos)
+            PdfDocument document ,
+            DateTime dataInicio ,
+            DateTime dataFim ,
+            Dictionary<string , string> graficos)
         {
             PdfPage page = document.Pages.Add();
             PdfGraphics graphics = page.Graphics;
 
-            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
-            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
-
-            PdfColor primaryColor = new PdfColor(13, 110, 253);
+            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica , 16 , PdfFontStyle.Bold);
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 10);
+
+            PdfColor primaryColor = new PdfColor(13 , 110 , 253);
 
             float yPosition = 0;
             float pageWidth = page.GetClientSize().Width;
 
             PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
-                new PointF(0, 0),
-                new PointF(pageWidth, 0),
-                new PdfColor(13, 110, 253),
-                new PdfColor(102, 126, 234)
+                new PointF(0 , 0) ,
+                new PointF(pageWidth , 0) ,
+                new PdfColor(13 , 110 , 253) ,
+                new PdfColor(102 , 126 , 234)
             );
 
-            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, pageWidth, 60));
-            graphics.DrawString("ANÁLISES COMPLEMENTARES",
-                titleFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(20, yPosition + 15));
-
-            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
-                regularFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(20, yPosition + 38));
+            graphics.DrawRectangle(gradientBrush , new RectangleF(0 , yPosition , pageWidth , 60));
+            graphics.DrawString("ANÁLISES COMPLEMENTARES" ,
+                titleFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(20 , yPosition + 15));
+
+            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}" ,
+                regularFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(20 , yPosition + 38));
 
             yPosition += 80;
 
-            yPosition = await DesenharGrafico(graphics, graficos, "requisitantes", "VIAGENS POR REQUISITANTE",
-                yPosition, pageWidth, headerFont, primaryColor);
-
-            yPosition = await DesenharGrafico(graphics, graficos, "setores", "VIAGENS POR SETOR",
-                yPosition, pageWidth, headerFont, primaryColor);
+            yPosition = await DesenharGrafico(graphics , graficos , "requisitantes" , "VIAGENS POR REQUISITANTE" ,
+                yPosition , pageWidth , headerFont , primaryColor);
+
+            yPosition = await DesenharGrafico(graphics , graficos , "setores" , "VIAGENS POR SETOR" ,
+                yPosition , pageWidth , headerFont , primaryColor);
         }
 
         private async Task<float> DesenharGrafico(
-            PdfGraphics graphics,
-            Dictionary<string, string> graficos,
-            string chaveGrafico,
-            string titulo,
-            float yPosition,
-            float pageWidth,
-            PdfFont headerFont,
+            PdfGraphics graphics ,
+            Dictionary<string , string> graficos ,
+            string chaveGrafico ,
+            string titulo ,
+            float yPosition ,
+            float pageWidth ,
+            PdfFont headerFont ,
             PdfColor primaryColor)
         {
             if (graficos != null && graficos.ContainsKey(chaveGrafico) && !string.IsNullOrEmpty(graficos[chaveGrafico]))
             {
                 try
                 {
-                    graphics.DrawString(titulo,
-                        headerFont,
-                        new PdfSolidBrush(primaryColor),
-                        new PointF(0, yPosition));
+                    Console.WriteLine($"📊 Desenhando gráfico: {chaveGrafico}");
+
+                    graphics.DrawString(titulo ,
+                        headerFont ,
+                        new PdfSolidBrush(primaryColor) ,
+                        new PointF(0 , yPosition));
                     yPosition += 25;
 
                     string base64Data = graficos[chaveGrafico].Contains(",")
@@ -356,50 +411,59 @@
                         float graficoWidth = pageWidth;
                         float graficoHeight = 180;
 
-                        graphics.DrawImage(image, new RectangleF(0, yPosition, graficoWidth, graficoHeight));
+                        graphics.DrawImage(image , new RectangleF(0 , yPosition , graficoWidth , graficoHeight));
                         yPosition += graficoHeight + 30;
+
+                        Console.WriteLine($"✅ Gráfico {chaveGrafico} desenhado");
                     }
                 }
-                catch { }
+                catch (Exception ex)
+                {
+                    Console.WriteLine($"❌ Erro ao desenhar gráfico {chaveGrafico}: {ex.Message}");
+                }
+            }
+            else
+            {
+                Console.WriteLine($"⚠️ Gráfico {chaveGrafico} não encontrado ou vazio");
             }
 
             return yPosition;
         }
 
-        private async Task CriarPagina1Estatisticas(PdfDocument document, DateTime dataInicio, DateTime dataFim)
+        private async Task CriarPagina1Estatisticas(PdfDocument document , DateTime dataInicio , DateTime dataFim)
         {
             PdfPage page = document.Pages.Add();
             PdfGraphics graphics = page.Graphics;
 
-            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
-            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
-            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
-
-            PdfColor primaryColor = new PdfColor(13, 110, 253);
-            PdfColor grayColor = new PdfColor(108, 117, 125);
-            PdfColor lightGray = new PdfColor(248, 249, 250);
+            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica , 16 , PdfFontStyle.Bold);
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 10);
+            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica , 8);
+
+            PdfColor primaryColor = new PdfColor(13 , 110 , 253);
+            PdfColor grayColor = new PdfColor(108 , 117 , 125);
+            PdfColor lightGray = new PdfColor(248 , 249 , 250);
 
             float yPosition = 0;
 
             PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
-                new PointF(0, 0),
-                new PointF(page.GetClientSize().Width, 0),
-                new PdfColor(13, 110, 253),
-                new PdfColor(102, 126, 234)
+                new PointF(0 , 0) ,
+                new PointF(page.GetClientSize().Width , 0) ,
+                new PdfColor(13 , 110 , 253) ,
+                new PdfColor(102 , 126 , 234)
             );
 
-            graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, page.GetClientSize().Width, 60));
-
-            graphics.DrawString("DASHBOARD DE VIAGENS",
-                titleFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(10, yPosition + 15));
-
-            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
-                regularFont,
-                new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                new PointF(10, yPosition + 38));
+            graphics.DrawRectangle(gradientBrush , new RectangleF(0 , yPosition , page.GetClientSize().Width , 60));
+
+            graphics.DrawString("DASHBOARD DE VIAGENS" ,
+                titleFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(10 , yPosition + 15));
+
+            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}" ,
+                regularFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(10 , yPosition + 38));
 
             yPosition += 80;
 
@@ -431,7 +495,7 @@
             decimal custoMedioPorViagem = totalViagens > 0 ? custoTotal / totalViagens : 0;
             decimal kmMedioPorViagem = totalViagens > 0 ? kmTotal / totalViagens : 0;
 
-            graphics.DrawString("ESTATÍSTICAS PRINCIPAIS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("ESTATÍSTICAS PRINCIPAIS" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             PdfGrid gridEstatisticas = new PdfGrid();
@@ -439,7 +503,7 @@
             gridEstatisticas.Columns[0].Width = 250;
             gridEstatisticas.Columns[1].Width = page.GetClientSize().Width - 250;
 
-            var estatisticas = new Dictionary<string, string>
+            var estatisticas = new Dictionary<string , string>
             {
                 { "Total de Viagens", totalViagens.ToString("N0") },
                 { "Viagens Finalizadas", viagensFinalizadas.ToString("N0") },
@@ -456,14 +520,14 @@
                 PdfGridRow row = gridEstatisticas.Rows.Add();
                 row.Cells[0].Value = stat.Key;
                 row.Cells[1].Value = stat.Value;
-                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
-            }
-
-            ApplyGridStyle(gridEstatisticas, primaryColor, lightGray);
-            var result = gridEstatisticas.Draw(page, new PointF(0, yPosition));
+                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 10 , PdfFontStyle.Bold);
+            }
+
+            ApplyGridStyle(gridEstatisticas , primaryColor , lightGray);
+            var result = gridEstatisticas.Draw(page , new PointF(0 , yPosition));
             yPosition = result.Bounds.Bottom + 20;
 
-            graphics.DrawString("CUSTOS DETALHADOS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("CUSTOS DETALHADOS" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             PdfGrid gridCustos = new PdfGrid();
@@ -471,7 +535,7 @@
             gridCustos.Columns[0].Width = 250;
             gridCustos.Columns[1].Width = page.GetClientSize().Width - 250;
 
-            var custos = new Dictionary<string, string>
+            var custos = new Dictionary<string , string>
             {
                 { "Custo Total", $"R$ {custoTotal:N2}" },
                 { "Custo com Combustível", $"R$ {custoCombustivel:N2}" },
@@ -486,19 +550,19 @@
                 PdfGridRow row = gridCustos.Rows.Add();
                 row.Cells[0].Value = custo.Key;
                 row.Cells[1].Value = custo.Value;
-                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
+                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 10 , PdfFontStyle.Bold);
 
                 if (custo.Key == "Custo Total")
                 {
-                    row.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(22, 163, 74));
-                }
-            }
-
-            ApplyGridStyle(gridCustos, primaryColor, lightGray);
-            result = gridCustos.Draw(page, new PointF(0, yPosition));
+                    row.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(22 , 163 , 74));
+                }
+            }
+
+            ApplyGridStyle(gridCustos , primaryColor , lightGray);
+            result = gridCustos.Draw(page , new PointF(0 , yPosition));
             yPosition = result.Bounds.Bottom + 20;
 
-            graphics.DrawString("DISTRIBUIÇÃO POR STATUS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("DISTRIBUIÇÃO POR STATUS" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             PdfGrid gridStatus = new PdfGrid();
@@ -506,7 +570,7 @@
             gridStatus.Columns[0].Width = 250;
             gridStatus.Columns[1].Width = page.GetClientSize().Width - 250;
 
-            var statusData = new Dictionary<string, (int count, PdfColor color)>
+            var statusData = new Dictionary<string , (int count, PdfColor color)>
             {
                 { "Finalizadas", (viagensFinalizadas, new PdfColor(22, 163, 74)) },
                 { "Em Andamento", (viagensEmAndamento, new PdfColor(13, 110, 253)) },
@@ -521,38 +585,38 @@
                 PdfGridRow row = gridStatus.Rows.Add();
                 row.Cells[0].Value = status.Key;
                 row.Cells[1].Value = $"{status.Value.count:N0} ({percentual:N1}%)";
-                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
+                row.Cells[1].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 10 , PdfFontStyle.Bold);
                 row.Cells[1].Style.TextBrush = new PdfSolidBrush(status.Value.color);
             }
 
-            ApplyGridStyle(gridStatus, primaryColor, lightGray);
-            result = gridStatus.Draw(page, new PointF(0, yPosition));
+            ApplyGridStyle(gridStatus , primaryColor , lightGray);
+            result = gridStatus.Draw(page , new PointF(0 , yPosition));
 
             string rodape = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm} | FrotiX - Sistema de Gestão de Frotas | Página 1/3";
-            graphics.DrawString(rodape, smallFont, new PdfSolidBrush(grayColor),
-                new PointF(0, page.GetClientSize().Height - 20));
+            graphics.DrawString(rodape , smallFont , new PdfSolidBrush(grayColor) ,
+                new PointF(0 , page.GetClientSize().Height - 20));
         }
 
-        private async Task CriarPagina2Rankings(PdfDocument document, DateTime dataInicio, DateTime dataFim)
+        private async Task CriarPagina2Rankings(PdfDocument document , DateTime dataInicio , DateTime dataFim)
         {
             PdfPage page = document.Pages.Add();
             PdfGraphics graphics = page.Graphics;
 
-            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
-            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
-            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
-
-            PdfColor primaryColor = new PdfColor(13, 110, 253);
-            PdfColor grayColor = new PdfColor(108, 117, 125);
-            PdfColor lightGray = new PdfColor(248, 249, 250);
-            PdfColor cianoColor = new PdfColor(34, 211, 238);
-            PdfColor laranjaColor = new PdfColor(217, 119, 6);
-            PdfColor verdeColor = new PdfColor(22, 163, 74);
+            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica , 14 , PdfFontStyle.Bold);
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 9);
+            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica , 8);
+
+            PdfColor primaryColor = new PdfColor(13 , 110 , 253);
+            PdfColor grayColor = new PdfColor(108 , 117 , 125);
+            PdfColor lightGray = new PdfColor(248 , 249 , 250);
+            PdfColor cianoColor = new PdfColor(34 , 211 , 238);
+            PdfColor laranjaColor = new PdfColor(217 , 119 , 6);
+            PdfColor verdeColor = new PdfColor(22 , 163 , 74);
 
             float yPosition = 0;
 
-            graphics.DrawString("RANKINGS E DETALHAMENTOS", titleFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("RANKINGS E DETALHAMENTOS" , titleFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 30;
 
             var viagens = await _context.Viagem
@@ -562,19 +626,19 @@
                 .ToListAsync();
 
             var veiculosInfo = await _context.ViewVeiculos
-                .Select(v => new { v.VeiculoId, v.Placa, v.MarcaModelo })
+                .Select(v => new { v.VeiculoId , v.Placa , v.MarcaModelo })
                 .ToListAsync();
 
-            graphics.DrawString("TOP 10 MOTORISTAS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("TOP 10 MOTORISTAS" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             var topMotoristas = viagens
                 .Where(v => v.Motorista != null)
-                .GroupBy(v => new { v.MotoristaId, NomeMotorista = v.Motorista.Nome })
+                .GroupBy(v => new { v.MotoristaId , NomeMotorista = v.Motorista.Nome })
                 .Select(g => new
                 {
-                    Motorista = g.Key.NomeMotorista ?? "Não informado",
-                    TotalViagens = g.Count(),
+                    Motorista = g.Key.NomeMotorista ?? "Não informado" ,
+                    TotalViagens = g.Count() ,
                     CustoTotal = g.Sum(v => (v.CustoCombustivel ?? 0) + (v.CustoLavador ?? 0) +
                                            (v.CustoMotorista ?? 0) + (v.CustoOperador ?? 0) +
                                            (v.CustoVeiculo ?? 0))
@@ -602,11 +666,11 @@
                 row.Cells[2].Value = $"R$ {motorista.CustoTotal:N2}";
             }
 
-            ApplyGridStyle(gridMotoristas, cianoColor, lightGray);
-            var result = gridMotoristas.Draw(page, new PointF(0, yPosition));
+            ApplyGridStyle(gridMotoristas , cianoColor , lightGray);
+            var result = gridMotoristas.Draw(page , new PointF(0 , yPosition));
             yPosition = result.Bounds.Bottom + 20;
 
-            graphics.DrawString("TOP 10 VEÍCULOS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("TOP 10 VEÍCULOS" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             var topVeiculos = viagens
@@ -614,8 +678,8 @@
                 .GroupBy(v => v.VeiculoId)
                 .Select(g => new
                 {
-                    VeiculoId = g.Key,
-                    TotalViagens = g.Count(),
+                    VeiculoId = g.Key ,
+                    TotalViagens = g.Count() ,
                     KmTotal = g.Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
                                .Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                 })
@@ -631,8 +695,8 @@
                     {
                         Veiculo = veiculo != null ?
                             $"{veiculo.Placa} - {veiculo.MarcaModelo}" :
-                            "Veículo não encontrado",
-                        tv.TotalViagens,
+                            "Veículo não encontrado" ,
+                        tv.TotalViagens ,
                         tv.KmTotal
                     };
                 })
@@ -657,19 +721,19 @@
                 row.Cells[2].Value = $"{veiculo.KmTotal:N0} km";
             }
 
-            ApplyGridStyle(gridVeiculos, laranjaColor, lightGray);
-            result = gridVeiculos.Draw(page, new PointF(0, yPosition));
+            ApplyGridStyle(gridVeiculos , laranjaColor , lightGray);
+            result = gridVeiculos.Draw(page , new PointF(0 , yPosition));
             yPosition = result.Bounds.Bottom + 20;
 
-            graphics.DrawString("TOP 10 FINALIDADES", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("TOP 10 FINALIDADES" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             var topFinalidades = viagens
                 .Where(v => v.Finalidade != null)
-                .GroupBy(v => new { v.Finalidade, Descricao = v.Finalidade })
+                .GroupBy(v => new { v.Finalidade , Descricao = v.Finalidade })
                 .Select(g => new
                 {
-                    Finalidade = g.Key.Descricao ?? "Não informada",
+                    Finalidade = g.Key.Descricao ?? "Não informada" ,
                     Total = g.Count()
                 })
                 .OrderByDescending(x => x.Total)
@@ -692,37 +756,37 @@
                 row.Cells[1].Value = finalidade.Total.ToString("N0");
             }
 
-            ApplyGridStyle(gridFinalidades, verdeColor, lightGray);
-            result = gridFinalidades.Draw(page, new PointF(0, yPosition));
+            ApplyGridStyle(gridFinalidades , verdeColor , lightGray);
+            result = gridFinalidades.Draw(page , new PointF(0 , yPosition));
 
             string rodape = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm} | FrotiX - Sistema de Gestão de Frotas | Página 2/3";
-            graphics.DrawString(rodape, smallFont, new PdfSolidBrush(grayColor),
-                new PointF(0, page.GetClientSize().Height - 20));
+            graphics.DrawString(rodape , smallFont , new PdfSolidBrush(grayColor) ,
+                new PointF(0 , page.GetClientSize().Height - 20));
         }
 
-        private async Task CriarPagina3Complementos(PdfDocument document, DateTime dataInicio, DateTime dataFim)
+        private async Task CriarPagina3Complementos(PdfDocument document , DateTime dataInicio , DateTime dataFim)
         {
             PdfPage page = document.Pages.Add();
             PdfGraphics graphics = page.Graphics;
 
-            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
-            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
-            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
-
-            PdfColor primaryColor = new PdfColor(13, 110, 253);
-            PdfColor grayColor = new PdfColor(108, 117, 125);
-            PdfColor lightGray = new PdfColor(248, 249, 250);
-            PdfColor greenColor = new PdfColor(22, 163, 74);
-            PdfColor rosaColor = new PdfColor(236, 72, 153);
-            PdfColor amareloColor = new PdfColor(245, 158, 11);
+            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica , 14 , PdfFontStyle.Bold);
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 9);
+            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica , 8);
+
+            PdfColor primaryColor = new PdfColor(13 , 110 , 253);
+            PdfColor grayColor = new PdfColor(108 , 117 , 125);
+            PdfColor lightGray = new PdfColor(248 , 249 , 250);
+            PdfColor greenColor = new PdfColor(22 , 163 , 74);
+            PdfColor rosaColor = new PdfColor(236 , 72 , 153);
+            PdfColor amareloColor = new PdfColor(245 , 158 , 11);
 
             float yPosition = 0;
 
-            graphics.DrawString("DETALHAMENTOS COMPLEMENTARES", titleFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("DETALHAMENTOS COMPLEMENTARES" , titleFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 30;
 
-            graphics.DrawString("TOP 10 VIAGENS MAIS CARAS", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("TOP 10 VIAGENS MAIS CARAS" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             var viagensMaisCaras = await _context.ViewViagens
@@ -763,30 +827,29 @@
                 row.Cells[6].Value = $"R$ {(viagem.CustoViagem ?? 0):N2}";
 
                 row.Cells[6].Style.TextBrush = new PdfSolidBrush(greenColor);
-                row.Cells[6].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9, PdfFontStyle.Bold);
+                row.Cells[6].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 9 , PdfFontStyle.Bold);
 
                 contador++;
             }
 
-            ApplyGridStyle(gridViagensCaras, primaryColor, lightGray);
-            var result = gridViagensCaras.Draw(page, new PointF(0, yPosition));
+            ApplyGridStyle(gridViagensCaras , primaryColor , lightGray);
+            var result = gridViagensCaras.Draw(page , new PointF(0 , yPosition));
             yPosition = result.Bounds.Bottom + 20;
 
-            graphics.DrawString("TOP 10 REQUISITANTES", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("TOP 10 REQUISITANTES" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             var viagens = await _context.Viagem
                 .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim && v.RequisitanteId != null)
                 .Include(v => v.Requisitante)
-                .Include(v => v.SetorSolicitante)
                 .ToListAsync();
 
             var topRequisitantes = viagens
                 .Where(v => v.Requisitante != null && v.Requisitante.Nome != "Coordenação de Transportes (Ctran)")
-                .GroupBy(v => new { v.RequisitanteId, Nome = v.Requisitante.Nome })
+                .GroupBy(v => new { v.RequisitanteId , Nome = v.Requisitante.Nome })
                 .Select(g => new
                 {
-                    Requisitante = g.Key.Nome ?? "Não informado",
+                    Requisitante = g.Key.Nome ?? "Não informado" ,
                     Total = g.Count()
                 })
                 .OrderByDescending(x => x.Total)
@@ -801,10 +864,11 @@
             headerRow = gridRequisitantes.Headers.Add(1)[0];
             headerRow.Cells[0].Value = "Requisitante";
             headerRow.Cells[1].Value = "Total de Viagens";
+
             headerRow.Cells[0].Style.BackgroundBrush = new PdfSolidBrush(rosaColor);
             headerRow.Cells[1].Style.BackgroundBrush = new PdfSolidBrush(rosaColor);
-            headerRow.Cells[0].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
-            headerRow.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
+            headerRow.Cells[0].Style.TextBrush = new PdfSolidBrush(new PdfColor(255 , 255 , 255));
+            headerRow.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(255 , 255 , 255));
 
             foreach (var req in topRequisitantes)
             {
@@ -813,26 +877,27 @@
                 row.Cells[1].Value = req.Total.ToString("N0");
             }
 
-            ApplyGridStyle(gridRequisitantes, rosaColor, lightGray);
-            result = gridRequisitantes.Draw(page, new PointF(0, yPosition));
+            ApplyGridStyle(gridRequisitantes , rosaColor , lightGray);
+            result = gridRequisitantes.Draw(page , new PointF(0 , yPosition));
             yPosition = result.Bounds.Bottom + 20;
 
             if (yPosition > page.GetClientSize().Height - 200)
             {
+
                 page = document.Pages.Add();
                 graphics = page.Graphics;
                 yPosition = 0;
             }
 
-            graphics.DrawString("TOP 10 SETORES", headerFont, new PdfSolidBrush(primaryColor), new PointF(0, yPosition));
+            graphics.DrawString("TOP 10 SETORES" , headerFont , new PdfSolidBrush(primaryColor) , new PointF(0 , yPosition));
             yPosition += 20;
 
             var topSetores = viagens
                 .Where(v => v.SetorSolicitante != null && v.SetorSolicitante.Nome != "Coordenação de Transportes")
-                .GroupBy(v => new { v.SetorSolicitanteId, Nome = v.SetorSolicitante.Nome })
+                .GroupBy(v => new { v.SetorSolicitanteId , Nome = v.SetorSolicitante.Nome })
                 .Select(g => new
                 {
-                    Setor = g.Key.Nome ?? "Não informado",
+                    Setor = g.Key.Nome ?? "Não informado" ,
                     Total = g.Count()
                 })
                 .OrderByDescending(x => x.Total)
@@ -847,10 +912,11 @@
             headerRow = gridSetores.Headers.Add(1)[0];
             headerRow.Cells[0].Value = "Setor";
             headerRow.Cells[1].Value = "Total de Viagens";
+
             headerRow.Cells[0].Style.BackgroundBrush = new PdfSolidBrush(amareloColor);
             headerRow.Cells[1].Style.BackgroundBrush = new PdfSolidBrush(amareloColor);
-            headerRow.Cells[0].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
-            headerRow.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
+            headerRow.Cells[0].Style.TextBrush = new PdfSolidBrush(new PdfColor(255 , 255 , 255));
+            headerRow.Cells[1].Style.TextBrush = new PdfSolidBrush(new PdfColor(255 , 255 , 255));
 
             foreach (var setor in topSetores)
             {
@@ -859,33 +925,26 @@
                 row.Cells[1].Value = setor.Total.ToString("N0");
             }
 
-            ApplyGridStyle(gridSetores, amareloColor, lightGray);
-            result = gridSetores.Draw(page, new PointF(0, yPosition));
+            ApplyGridStyle(gridSetores , amareloColor , lightGray);
+            result = gridSetores.Draw(page , new PointF(0 , yPosition));
 
             string rodape = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm} | FrotiX - Sistema de Gestão de Frotas | Página 3/3";
-            graphics.DrawString(rodape, smallFont, new PdfSolidBrush(grayColor),
-                new PointF(0, page.GetClientSize().Height - 20));
+            graphics.DrawString(rodape , smallFont , new PdfSolidBrush(grayColor) ,
+                new PointF(0 , page.GetClientSize().Height - 20));
         }
 
-        private void ApplyGridStyle(PdfGrid grid, PdfColor headerColor, PdfColor alternateRowColor)
+        private void ApplyGridStyle(PdfGrid grid , PdfColor headerColor , PdfColor alternateRowColor)
         {
+
             if (grid.Headers.Count > 0)
             {
                 PdfGridRow headerRow = grid.Headers[0];
                 foreach (PdfGridCell cell in headerRow.Cells)
                 {
-                    if (cell.Style.BackgroundBrush == null)
-                    {
-                        cell.Style.BackgroundBrush = new PdfSolidBrush(headerColor);
-                        cell.Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
-                    }
-                    else
-                    {
-                        cell.Style.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
-                    }
-
-                    cell.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
-                    cell.Style.Borders.All = new PdfPen(new PdfColor(200, 200, 200), 0.5f);
+                    cell.Style.BackgroundBrush = new PdfSolidBrush(headerColor);
+                    cell.Style.TextBrush = new PdfSolidBrush(new PdfColor(255 , 255 , 255));
+                    cell.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 10 , PdfFontStyle.Bold);
+                    cell.Style.Borders.All = new PdfPen(new PdfColor(200 , 200 , 200) , 0.5f);
                 }
             }
 
@@ -898,11 +957,8 @@
                     {
                         cell.Style.BackgroundBrush = new PdfSolidBrush(alternateRowColor);
                     }
-
-                    if (cell.Style.Font == null)
-                        cell.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
-
-                    cell.Style.Borders.All = new PdfPen(new PdfColor(200, 200, 200), 0.5f);
+                    cell.Style.Borders.All = new PdfPen(new PdfColor(200 , 200 , 200) , 0.5f);
+                    cell.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 9);
                 }
                 alternate = !alternate;
             }
```
