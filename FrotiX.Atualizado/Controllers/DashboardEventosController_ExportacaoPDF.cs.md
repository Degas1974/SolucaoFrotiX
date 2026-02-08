# Controllers/DashboardEventosController_ExportacaoPDF.cs

**Mudanca:** GRANDE | **+246** linhas | **-285** linhas

---

```diff
--- JANEIRO: Controllers/DashboardEventosController_ExportacaoPDF.cs
+++ ATUAL: Controllers/DashboardEventosController_ExportacaoPDF.cs
@@ -55,362 +55,325 @@
             catch (Exception ex)
             {
                 _log.Error(ex.Message, ex, "DashboardEventosController_ExportacaoPDF.cs", "ExportarParaPDF");
-                Alerta.TratamentoErroComLinha("DashboardEventosController_ExportacaoPDF.cs", "ExportarParaPDF", ex);
                 return Json(new { success = false, message = $"Erro ao gerar PDF: {ex.Message}" });
             }
         }
 
-        private async Task CriarPagina1Estatisticas(PdfDocument document, DateTime dataInicio, DateTime dataFim)
-        {
-            try
-            {
-
-                PdfPage page = document.Pages.Add();
-                PdfGraphics graphics = page.Graphics;
-
-                PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
-                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-                PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
-                PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
-
-                PdfColor grayColor = new PdfColor(108, 117, 125);
-
-                float yPosition = 0;
-
-                PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
-                    new PointF(0, 0),
-                    new PointF(page.GetClientSize().Width, 0),
-                    new PdfColor(102, 126, 234),
-                    new PdfColor(118, 75, 162)
-                );
-
-                graphics.DrawRectangle(gradientBrush, new RectangleF(0, yPosition, page.GetClientSize().Width, 60));
-
-                graphics.DrawString("DASHBOARD DE EVENTOS",
-                    titleFont,
-                    new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                    new PointF(page.GetClientSize().Width / 2, yPosition + 18),
-                    new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Top));
-
-                graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}",
-                    regularFont,
-                    new PdfSolidBrush(new PdfColor(255, 255, 255)),
-                    new PointF(page.GetClientSize().Width / 2, yPosition + 38),
-                    new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Top));
-
-                yPosition += 80;
-
-                var eventos = await _context.Evento
-                    .Include(e => e.SetorSolicitante)
-                    .Include(e => e.Requisitante)
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
-                    .ToListAsync();
-
-                var totalEventos = eventos.Count;
-                var eventosAtivos = eventos.Count(e => e.Status == "Ativo" || e.Status == "Em Andamento");
-                var eventosConcluidos = eventos.Count(e => e.Status == "Concluído" || e.Status == "Finalizado");
-                var eventosCancelados = eventos.Count(e => e.Status == "Cancelado");
-                var totalParticipantes = eventos.Sum(e => e.QtdParticipantes ?? 0);
-                var mediaParticipantes = totalEventos > 0 ? (double)totalParticipantes / totalEventos : 0;
-
-                float cardWidth = (page.GetClientSize().Width - 30) / 4;
-                float cardHeight = 70;
-                float xPosition = 0;
-
-                DesenharCardKPI(graphics, xPosition, yPosition, cardWidth, cardHeight,
-                    "TOTAL DE EVENTOS", totalEventos.ToString(),
-                    new PdfColor(13, 110, 253), regularFont, headerFont);
-
-                xPosition += cardWidth + 10;
-
-                DesenharCardKPI(graphics, xPosition, yPosition, cardWidth, cardHeight,
-                    "ATIVOS", eventosAtivos.ToString(),
-                    new PdfColor(22, 163, 74), regularFont, headerFont);
-
-                xPosition += cardWidth + 10;
-
-                DesenharCardKPI(graphics, xPosition, yPosition, cardWidth, cardHeight,
-                    "CONCLUÍDOS", eventosConcluidos.ToString(),
-                    new PdfColor(23, 162, 184), regularFont, headerFont);
-
-                xPosition += cardWidth + 10;
-
-                DesenharCardKPI(graphics, xPosition, yPosition, cardWidth, cardHeight,
-                    "CANCELADOS", eventosCancelados.ToString(),
-                    new PdfColor(220, 53, 69), regularFont, headerFont);
-
-                yPosition += cardHeight + 30;
-
-                xPosition = 0;
-                cardWidth = (page.GetClientSize().Width - 10) / 2;
-
-                DesenharCardKPI(graphics, xPosition, yPosition, cardWidth, cardHeight,
-                    "TOTAL PARTICIPANTES", totalParticipantes.ToString("N0"),
-                    new PdfColor(157, 78, 221), regularFont, headerFont);
-
-                xPosition += cardWidth + 10;
-
-                DesenharCardKPI(graphics, xPosition, yPosition, cardWidth, cardHeight,
-                    "MÉDIA POR EVENTO", mediaParticipantes.ToString("N1") + " part.",
-                    new PdfColor(245, 158, 11), regularFont, headerFont);
-
-                yPosition += cardHeight + 30;
-
-                graphics.DrawString("DISTRIBUIÇÃO POR STATUS", headerFont, new PdfSolidBrush(grayColor), new PointF(0, yPosition));
-                yPosition += 25;
-
-                var eventosPorStatus = eventos
-                    .GroupBy(e => e.Status ?? "Sem Status")
-                    .Select(g => new
-                    {
-                        Status = g.Key,
-                        Quantidade = g.Count(),
-                        Participantes = g.Sum(e => e.QtdParticipantes ?? 0),
-                        Percentual = totalEventos > 0 ? (double)g.Count() / totalEventos * 100 : 0
-                    })
-                    .OrderByDescending(x => x.Quantidade)
-                    .ToList();
-
-                PdfGrid gridStatus = new PdfGrid();
-                gridStatus.DataSource = eventosPorStatus.Select(e => new
-                {
-                    Status = e.Status,
-                    Quantidade = e.Quantidade.ToString(),
-                    Participantes = e.Participantes.ToString("N0"),
-                    Percentual = e.Percentual.ToString("N1") + "%"
-                }).ToList();
-
-                ApplyGridStyle(gridStatus, regularFont);
-                PdfGridLayoutResult resultStatus = gridStatus.Draw(page, new PointF(0, yPosition));
-                yPosition = resultStatus.Bounds.Bottom + 20;
-
-                graphics.DrawString($"© {DateTime.Now.Year} FrotiX - Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}",
-                    smallFont,
-                    new PdfSolidBrush(grayColor),
-                    new PointF(page.GetClientSize().Width / 2, page.GetClientSize().Height + 20),
-                    new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Top));
+        private async Task CriarPagina1Estatisticas(PdfDocument document , DateTime dataInicio , DateTime dataFim)
+        {
+            PdfPage page = document.Pages.Add();
+            PdfGraphics graphics = page.Graphics;
+
+            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica , 16 , PdfFontStyle.Bold);
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 10);
+            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica , 8);
+
+            PdfColor primaryColor = new PdfColor(102 , 126 , 234);
+            PdfColor grayColor = new PdfColor(108 , 117 , 125);
+            PdfColor lightGray = new PdfColor(248 , 249 , 250);
+
+            float yPosition = 0;
+
+            PdfLinearGradientBrush gradientBrush = new PdfLinearGradientBrush(
+                new PointF(0 , 0) ,
+                new PointF(page.GetClientSize().Width , 0) ,
+                new PdfColor(102 , 126 , 234) ,
+                new PdfColor(118 , 75 , 162)
+            );
+
+            graphics.DrawRectangle(gradientBrush , new RectangleF(0 , yPosition , page.GetClientSize().Width , 60));
+
+            graphics.DrawString("DASHBOARD DE EVENTOS" ,
+                titleFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(page.GetClientSize().Width / 2 , yPosition + 18) ,
+                new PdfStringFormat(PdfTextAlignment.Center , PdfVerticalAlignment.Top));
+
+            graphics.DrawString($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}" ,
+                regularFont ,
+                new PdfSolidBrush(new PdfColor(255 , 255 , 255)) ,
+                new PointF(page.GetClientSize().Width / 2 , yPosition + 38) ,
+                new PdfStringFormat(PdfTextAlignment.Center , PdfVerticalAlignment.Top));
+
+            yPosition += 80;
+
+            var eventos = await _context.Evento
+                .Include(e => e.SetorSolicitante)
+                .Include(e => e.Requisitante)
+                .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+                .ToListAsync();
+
+            var totalEventos = eventos.Count;
+            var eventosAtivos = eventos.Count(e => e.Status == "Ativo" || e.Status == "Em Andamento");
+            var eventosConcluidos = eventos.Count(e => e.Status == "Concluído" || e.Status == "Finalizado");
+            var eventosCancelados = eventos.Count(e => e.Status == "Cancelado");
+            var totalParticipantes = eventos.Sum(e => e.QtdParticipantes ?? 0);
+            var mediaParticipantes = totalEventos > 0 ? (double)totalParticipantes / totalEventos : 0;
+
+            float cardWidth = (page.GetClientSize().Width - 30) / 4;
+            float cardHeight = 70;
+            float xPosition = 0;
+
+            DesenharCardKPI(graphics , xPosition , yPosition , cardWidth , cardHeight ,
+                "TOTAL DE EVENTOS" , totalEventos.ToString() ,
+                new PdfColor(13 , 110 , 253) , regularFont , headerFont);
+
+            xPosition += cardWidth + 10;
+
+            DesenharCardKPI(graphics , xPosition , yPosition , cardWidth , cardHeight ,
+                "ATIVOS" , eventosAtivos.ToString() ,
+                new PdfColor(22 , 163 , 74) , regularFont , headerFont);
+
+            xPosition += cardWidth + 10;
+
+            DesenharCardKPI(graphics , xPosition , yPosition , cardWidth , cardHeight ,
+                "CONCLUÍDOS" , eventosConcluidos.ToString() ,
+                new PdfColor(23 , 162 , 184) , regularFont , headerFont);
+
+            xPosition += cardWidth + 10;
+
+            DesenharCardKPI(graphics , xPosition , yPosition , cardWidth , cardHeight ,
+                "CANCELADOS" , eventosCancelados.ToString() ,
+                new PdfColor(220 , 53 , 69) , regularFont , headerFont);
+
+            yPosition += cardHeight + 30;
+
+            xPosition = 0;
+            cardWidth = (page.GetClientSize().Width - 10) / 2;
+
+            DesenharCardKPI(graphics , xPosition , yPosition , cardWidth , cardHeight ,
+                "TOTAL PARTICIPANTES" , totalParticipantes.ToString("N0") ,
+                new PdfColor(157 , 78 , 221) , regularFont , headerFont);
+
+            xPosition += cardWidth + 10;
+
+            DesenharCardKPI(graphics , xPosition , yPosition , cardWidth , cardHeight ,
+                "MÉDIA POR EVENTO" , mediaParticipantes.ToString("N1") + " part." ,
+                new PdfColor(245 , 158 , 11) , regularFont , headerFont);
+
+            yPosition += cardHeight + 30;
+
+            graphics.DrawString("DISTRIBUIÇÃO POR STATUS" , headerFont , new PdfSolidBrush(grayColor) , new PointF(0 , yPosition));
+            yPosition += 25;
+
+            var eventosPorStatus = eventos
+                .GroupBy(e => e.Status ?? "Sem Status")
+                .Select(g => new
+                {
+                    Status = g.Key ,
+                    Quantidade = g.Count() ,
+                    Participantes = g.Sum(e => e.QtdParticipantes ?? 0) ,
+                    Percentual = totalEventos > 0 ? (double)g.Count() / totalEventos * 100 : 0
+                })
+                .OrderByDescending(x => x.Quantidade)
+                .ToList();
+
+            PdfGrid gridStatus = new PdfGrid();
+            gridStatus.DataSource = eventosPorStatus.Select(e => new
+            {
+                Status = e.Status ,
+                Quantidade = e.Quantidade.ToString() ,
+                Participantes = e.Participantes.ToString("N0") ,
+                Percentual = e.Percentual.ToString("N1") + "%"
+            }).ToList();
+
+            ApplyGridStyle(gridStatus , regularFont);
+            PdfGridLayoutResult resultStatus = gridStatus.Draw(page , new PointF(0 , yPosition));
+            yPosition = resultStatus.Bounds.Bottom + 20;
+
+            graphics.DrawString($"© {DateTime.Now.Year} FrotiX - Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}" ,
+                smallFont ,
+                new PdfSolidBrush(grayColor) ,
+                new PointF(page.GetClientSize().Width / 2 , page.GetClientSize().Height + 20) ,
+                new PdfStringFormat(PdfTextAlignment.Center , PdfVerticalAlignment.Top));
+        }
+
+        private async Task CriarPagina2SetoresRequisitantes(PdfDocument document , DateTime dataInicio , DateTime dataFim)
+        {
+            PdfPage page = document.Pages.Add();
+            PdfGraphics graphics = page.Graphics;
+
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 10);
+            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica , 8);
+
+            PdfColor grayColor = new PdfColor(108 , 117 , 125);
+
+            float yPosition = 0;
+
+            graphics.DrawString("ESTATÍSTICAS POR SETOR E REQUISITANTE" , headerFont , new PdfSolidBrush(grayColor) , new PointF(0 , yPosition));
+            yPosition += 30;
+
+            var eventos = await _context.Evento
+                .Include(e => e.SetorSolicitante)
+                .Include(e => e.Requisitante)
+                .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+                .ToListAsync();
+
+            graphics.DrawString("TOP 10 SETORES SOLICITANTES" , headerFont , new PdfSolidBrush(grayColor) , new PointF(0 , yPosition));
+            yPosition += 25;
+
+            var eventosPorSetor = eventos
+                .GroupBy(e => e.SetorSolicitante != null ? e.SetorSolicitante.Nome : "Sem Setor")
+                .Select(g => new
+                {
+                    Setor = g.Key ,
+                    Quantidade = g.Count() ,
+                    Participantes = g.Sum(e => e.QtdParticipantes ?? 0) ,
+                    Concluidos = g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado") ,
+                    TaxaConclusao = g.Count() > 0 ? (double)g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado") / g.Count() * 100 : 0
+                })
+                .OrderByDescending(x => x.Quantidade)
+                .Take(10)
+                .ToList();
+
+            PdfGrid gridSetores = new PdfGrid();
+            gridSetores.DataSource = eventosPorSetor.Select(e => new
+            {
+                Setor = e.Setor.Length > 30 ? e.Setor.Substring(0 , 27) + "..." : e.Setor ,
+                Eventos = e.Quantidade.ToString() ,
+                Participantes = e.Participantes.ToString("N0") ,
+                Concluídos = e.Concluidos.ToString() ,
+                Taxa = e.TaxaConclusao.ToString("N1") + "%"
+            }).ToList();
+
+            ApplyGridStyle(gridSetores , regularFont);
+            PdfGridLayoutResult resultSetores = gridSetores.Draw(page , new PointF(0 , yPosition));
+            yPosition = resultSetores.Bounds.Bottom + 30;
+
+            graphics.DrawString("TOP 10 REQUISITANTES" , headerFont , new PdfSolidBrush(grayColor) , new PointF(0 , yPosition));
+            yPosition += 25;
+
+            var eventosPorRequisitante = eventos
+                .GroupBy(e => e.Requisitante != null ? e.Requisitante.Nome : "Sem Requisitante")
+                .Select(g => new
+                {
+                    Requisitante = g.Key ,
+                    Quantidade = g.Count() ,
+                    Participantes = g.Sum(e => e.QtdParticipantes ?? 0)
+                })
+                .OrderByDescending(x => x.Quantidade)
+                .Take(10)
+                .ToList();
+
+            PdfGrid gridRequisitantes = new PdfGrid();
+            gridRequisitantes.DataSource = eventosPorRequisitante.Select(e => new
+            {
+                Requisitante = e.Requisitante.Length > 40 ? e.Requisitante.Substring(0 , 37) + "..." : e.Requisitante ,
+                Eventos = e.Quantidade.ToString() ,
+                Participantes = e.Participantes.ToString("N0")
+            }).ToList();
+
+            ApplyGridStyle(gridRequisitantes , regularFont);
+            PdfGridLayoutResult resultRequisitantes = gridRequisitantes.Draw(page , new PointF(0 , yPosition));
+
+            graphics.DrawString($"© {DateTime.Now.Year} FrotiX - Página 2" ,
+                smallFont ,
+                new PdfSolidBrush(grayColor) ,
+                new PointF(page.GetClientSize().Width / 2 , page.GetClientSize().Height + 20) ,
+                new PdfStringFormat(PdfTextAlignment.Center , PdfVerticalAlignment.Top));
+        }
+
+        private async Task CriarPagina3Top10Eventos(PdfDocument document , DateTime dataInicio , DateTime dataFim)
+        {
+            PdfPage page = document.Pages.Add();
+            PdfGraphics graphics = page.Graphics;
+
+            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica , 12 , PdfFontStyle.Bold);
+            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica , 10);
+            PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica , 8);
+
+            PdfColor grayColor = new PdfColor(108 , 117 , 125);
+
+            float yPosition = 0;
+
+            graphics.DrawString("TOP 10 MAIORES EVENTOS" , headerFont , new PdfSolidBrush(grayColor) , new PointF(0 , yPosition));
+            yPosition += 30;
+
+            var topEventos = await _context.Evento
+                .Include(e => e.SetorSolicitante)
+                .Include(e => e.Requisitante)
+                .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
+                .OrderByDescending(e => e.QtdParticipantes)
+                .Take(10)
+                .ToListAsync();
+
+            PdfGrid gridEventos = new PdfGrid();
+            gridEventos.DataSource = topEventos.Select(e => new
+            {
+                Evento = e.Nome.Length > 35 ? e.Nome.Substring(0 , 32) + "..." : e.Nome ,
+                Data = e.DataInicial.HasValue ? e.DataInicial.Value.ToString("dd/MM/yyyy") : "" ,
+                Participantes = (e.QtdParticipantes ?? 0).ToString("N0") ,
+                Setor = e.SetorSolicitante != null ? (e.SetorSolicitante.Nome.Length > 25 ? e.SetorSolicitante.Nome.Substring(0 , 22) + "..." : e.SetorSolicitante.Nome) : "N/A" ,
+                Status = e.Status ?? "N/A"
+            }).ToList();
+
+            ApplyGridStyle(gridEventos , regularFont);
+            gridEventos.Columns[0].Width = 150;
+            gridEventos.Columns[1].Width = 70;
+            gridEventos.Columns[2].Width = 80;
+            gridEventos.Columns[3].Width = 110;
+            gridEventos.Columns[4].Width = 70;
+
+            PdfGridLayoutResult resultEventos = gridEventos.Draw(page , new PointF(0 , yPosition));
+
+            graphics.DrawString($"© {DateTime.Now.Year} FrotiX - Página 3" ,
+                smallFont ,
+                new PdfSolidBrush(grayColor) ,
+                new PointF(page.GetClientSize().Width / 2 , page.GetClientSize().Height + 20) ,
+                new PdfStringFormat(PdfTextAlignment.Center , PdfVerticalAlignment.Top));
+        }
+
+        private void DesenharCardKPI(PdfGraphics graphics , float x , float y , float width , float height ,
+            string label , string value , PdfColor borderColor , PdfFont labelFont , PdfFont valueFont)
+        {
+
+            graphics.DrawRectangle(new PdfSolidBrush(new PdfColor(255 , 255 , 255)) , new RectangleF(x , y , width , height));
+
+            graphics.DrawRectangle(new PdfSolidBrush(borderColor) , new RectangleF(x , y , 4 , height));
+
+            graphics.DrawRectangle(new PdfPen(new PdfColor(220 , 220 , 220)) , new RectangleF(x , y , width , height));
+
+            graphics.DrawString(label , labelFont , new PdfSolidBrush(new PdfColor(108 , 117 , 125)) ,
+                new PointF(x + 15 , y + 15));
+
+            graphics.DrawString(value , valueFont , new PdfSolidBrush(new PdfColor(17 , 24 , 39)) ,
+                new PointF(x + 15 , y + 35));
+        }
+
+        private void ApplyGridStyle(PdfGrid grid , PdfFont font)
+        {
+
+            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
+            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(102 , 126 , 234));
+            headerStyle.TextBrush = new PdfSolidBrush(new PdfColor(255 , 255 , 255));
+            headerStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 10 , PdfFontStyle.Bold);
+
+            foreach (PdfGridColumn column in grid.Columns)
+            {
+                column.Format = new PdfStringFormat(PdfTextAlignment.Left , PdfVerticalAlignment.Middle);
             }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController_ExportacaoPDF.cs", "CriarPagina1Estatisticas");
-                Alerta.TratamentoErroComLinha("DashboardEventosController_ExportacaoPDF.cs", "CriarPagina1Estatisticas", ex);
+
+            grid.Headers[0].ApplyStyle(headerStyle);
+            grid.Headers[0].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica , 10 , PdfFontStyle.Bold);
+
+            for (int i = 0 ; i < grid.Rows.Count ; i++)
+            {
+                PdfGridRow row = grid.Rows[i];
+                row.Style.Font = font;
+                row.Style.BackgroundBrush = i % 2 == 0 ?
+                    new PdfSolidBrush(new PdfColor(255 , 255 , 255)) :
+                    new PdfSolidBrush(new PdfColor(248 , 249 , 250));
+                row.Style.TextBrush = new PdfSolidBrush(new PdfColor(0 , 0 , 0));
+
+                foreach (PdfGridCell cell in row.Cells)
+                {
+                    cell.Style.Borders.All = new PdfPen(new PdfColor(220 , 220 , 220) , 0.5f);
+                }
             }
-        }
-
-        private async Task CriarPagina2SetoresRequisitantes(PdfDocument document, DateTime dataInicio, DateTime dataFim)
-        {
-            try
-            {
-
-                PdfPage page = document.Pages.Add();
-                PdfGraphics graphics = page.Graphics;
-
-                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-                PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
-                PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
-
-                PdfColor grayColor = new PdfColor(108, 117, 125);
-
-                float yPosition = 0;
-
-                graphics.DrawString("ESTATÍSTICAS POR SETOR E REQUISITANTE", headerFont, new PdfSolidBrush(grayColor), new PointF(0, yPosition));
-                yPosition += 30;
-
-                var eventos = await _context.Evento
-                    .Include(e => e.SetorSolicitante)
-                    .Include(e => e.Requisitante)
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
-                    .ToListAsync();
-
-                graphics.DrawString("TOP 10 SETORES SOLICITANTES", headerFont, new PdfSolidBrush(grayColor), new PointF(0, yPosition));
-                yPosition += 25;
-
-                var eventosPorSetor = eventos
-                    .GroupBy(e => e.SetorSolicitante != null ? e.SetorSolicitante.Nome : "Sem Setor")
-                    .Select(g => new
-                    {
-                        Setor = g.Key,
-                        Quantidade = g.Count(),
-                        Participantes = g.Sum(e => e.QtdParticipantes ?? 0),
-                        Concluidos = g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado"),
-                        TaxaConclusao = g.Count() > 0 ? (double)g.Count(e => e.Status == "Concluído" || e.Status == "Finalizado") / g.Count() * 100 : 0
-                    })
-                    .OrderByDescending(x => x.Quantidade)
-                    .Take(10)
-                    .ToList();
-
-                PdfGrid gridSetores = new PdfGrid();
-                gridSetores.DataSource = eventosPorSetor.Select(e => new
-                {
-                    Setor = e.Setor.Length > 30 ? e.Setor.Substring(0, 27) + "..." : e.Setor,
-                    Eventos = e.Quantidade.ToString(),
-                    Participantes = e.Participantes.ToString("N0"),
-                    Concluídos = e.Concluidos.ToString(),
-                    Taxa = e.TaxaConclusao.ToString("N1") + "%"
-                }).ToList();
-
-                ApplyGridStyle(gridSetores, regularFont);
-                PdfGridLayoutResult resultSetores = gridSetores.Draw(page, new PointF(0, yPosition));
-                yPosition = resultSetores.Bounds.Bottom + 30;
-
-                graphics.DrawString("TOP 10 REQUISITANTES", headerFont, new PdfSolidBrush(grayColor), new PointF(0, yPosition));
-                yPosition += 25;
-
-                var eventosPorRequisitante = eventos
-                    .GroupBy(e => e.Requisitante != null ? e.Requisitante.Nome : "Sem Requisitante")
-                    .Select(g => new
-                    {
-                        Requisitante = g.Key,
-                        Quantidade = g.Count(),
-                        Participantes = g.Sum(e => e.QtdParticipantes ?? 0)
-                    })
-                    .OrderByDescending(x => x.Quantidade)
-                    .Take(10)
-                    .ToList();
-
-                PdfGrid gridRequisitantes = new PdfGrid();
-                gridRequisitantes.DataSource = eventosPorRequisitante.Select(e => new
-                {
-                    Requisitante = e.Requisitante.Length > 40 ? e.Requisitante.Substring(0, 37) + "..." : e.Requisitante,
-                    Eventos = e.Quantidade.ToString(),
-                    Participantes = e.Participantes.ToString("N0")
-                }).ToList();
-
-                ApplyGridStyle(gridRequisitantes, regularFont);
-                PdfGridLayoutResult resultRequisitantes = gridRequisitantes.Draw(page, new PointF(0, yPosition));
-
-                graphics.DrawString($"© {DateTime.Now.Year} FrotiX - Página 2",
-                    smallFont,
-                    new PdfSolidBrush(grayColor),
-                    new PointF(page.GetClientSize().Width / 2, page.GetClientSize().Height + 20),
-                    new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Top));
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController_ExportacaoPDF.cs", "CriarPagina2SetoresRequisitantes");
-                Alerta.TratamentoErroComLinha("DashboardEventosController_ExportacaoPDF.cs", "CriarPagina2SetoresRequisitantes", ex);
-            }
-        }
-
-        private async Task CriarPagina3Top10Eventos(PdfDocument document, DateTime dataInicio, DateTime dataFim)
-        {
-            try
-            {
-                PdfPage page = document.Pages.Add();
-                PdfGraphics graphics = page.Graphics;
-
-                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
-                PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
-                PdfFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
-
-                PdfColor grayColor = new PdfColor(108, 117, 125);
-
-                float yPosition = 0;
-
-                graphics.DrawString("TOP 10 MAIORES EVENTOS", headerFont, new PdfSolidBrush(grayColor), new PointF(0, yPosition));
-                yPosition += 30;
-
-                var topEventos = await _context.Evento
-                    .Include(e => e.SetorSolicitante)
-                    .Include(e => e.Requisitante)
-                    .Where(e => e.DataInicial >= dataInicio && e.DataInicial <= dataFim)
-                    .OrderByDescending(e => e.QtdParticipantes)
-                    .Take(10)
-                    .ToListAsync();
-
-                PdfGrid gridEventos = new PdfGrid();
-                gridEventos.DataSource = topEventos.Select(e => new
-                {
-                    Evento = e.Nome.Length > 35 ? e.Nome.Substring(0, 32) + "..." : e.Nome,
-                    Data = e.DataInicial.HasValue ? e.DataInicial.Value.ToString("dd/MM/yyyy") : "",
-                    Participantes = (e.QtdParticipantes ?? 0).ToString("N0"),
-                    Setor = e.SetorSolicitante != null ? (e.SetorSolicitante.Nome.Length > 25 ? e.SetorSolicitante.Nome.Substring(0, 22) + "..." : e.SetorSolicitante.Nome) : "N/A",
-                    Status = e.Status ?? "N/A"
-                }).ToList();
-
-                ApplyGridStyle(gridEventos, regularFont);
-                gridEventos.Columns[0].Width = 150;
-                gridEventos.Columns[1].Width = 70;
-                gridEventos.Columns[2].Width = 80;
-                gridEventos.Columns[3].Width = 110;
-                gridEventos.Columns[4].Width = 70;
-
-                PdfGridLayoutResult resultEventos = gridEventos.Draw(page, new PointF(0, yPosition));
-
-                graphics.DrawString($"© {DateTime.Now.Year} FrotiX - Página 3",
-                    smallFont,
-                    new PdfSolidBrush(grayColor),
-                    new PointF(page.GetClientSize().Width / 2, page.GetClientSize().Height + 20),
-                    new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Top));
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController_ExportacaoPDF.cs", "CriarPagina3Top10Eventos");
-                Alerta.TratamentoErroComLinha("DashboardEventosController_ExportacaoPDF.cs", "CriarPagina3Top10Eventos", ex);
-            }
-        }
-
-        private void DesenharCardKPI(PdfGraphics graphics, float x, float y, float width, float height,
-            string label, string value, PdfColor borderColor, PdfFont labelFont, PdfFont valueFont)
-        {
-            try
-            {
-                graphics.DrawRectangle(new PdfSolidBrush(new PdfColor(255, 255, 255)), new RectangleF(x, y, width, height));
-                graphics.DrawRectangle(new PdfSolidBrush(borderColor), new RectangleF(x, y, 4, height));
-                graphics.DrawRectangle(new PdfPen(new PdfColor(220, 220, 220)), new RectangleF(x, y, width, height));
-
-                graphics.DrawString(label, labelFont, new PdfSolidBrush(new PdfColor(108, 117, 125)),
-                    new PointF(x + 15, y + 15));
-
-                graphics.DrawString(value, valueFont, new PdfSolidBrush(new PdfColor(17, 24, 39)),
-                    new PointF(x + 15, y + 35));
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController_ExportacaoPDF.cs", "DesenharCardKPI");
-                Alerta.TratamentoErroComLinha("DashboardEventosController_ExportacaoPDF.cs", "DesenharCardKPI", ex);
-            }
-        }
-
-        private void ApplyGridStyle(PdfGrid grid, PdfFont font)
-        {
-            try
-            {
-                PdfGridCellStyle headerStyle = new PdfGridCellStyle();
-                headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(102, 126, 234));
-                headerStyle.TextBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));
-                headerStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
-
-                foreach (PdfGridColumn column in grid.Columns)
-                {
-                    column.Format = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
-                }
-
-                grid.Headers[0].ApplyStyle(headerStyle);
-                grid.Headers[0].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
-
-                for (int i = 0; i < grid.Rows.Count; i++)
-                {
-                    PdfGridRow row = grid.Rows[i];
-                    row.Style.Font = font;
-                    row.Style.BackgroundBrush = i % 2 == 0 ?
-                        new PdfSolidBrush(new PdfColor(255, 255, 255)) :
-                        new PdfSolidBrush(new PdfColor(248, 249, 250));
-                    row.Style.TextBrush = new PdfSolidBrush(new PdfColor(0, 0, 0));
-
-                    foreach (PdfGridCell cell in row.Cells)
-                    {
-                        cell.Style.Borders.All = new PdfPen(new PdfColor(220, 220, 220), 0.5f);
-                    }
-                }
-
-                grid.Style.CellPadding = new PdfPaddings(5, 5, 5, 5);
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardEventosController_ExportacaoPDF.cs", "ApplyGridStyle");
-                Alerta.TratamentoErroComLinha("DashboardEventosController_ExportacaoPDF.cs", "ApplyGridStyle", ex);
-            }
+
+            grid.Style.CellPadding = new PdfPaddings(5 , 5 , 5 , 5);
         }
 
     }
```
