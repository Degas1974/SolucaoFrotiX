# Controllers/TaxiLegController.cs

**Mudanca:** GRANDE | **+334** linhas | **-131** linhas

---

```diff
--- JANEIRO: Controllers/TaxiLegController.cs
+++ ATUAL: Controllers/TaxiLegController.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc;
@@ -20,46 +19,82 @@
 
     [Route("api/[controller]")]
     [ApiController]
-    public class TaxiLegController : Controller
+    public class TaxiLegController :Controller
     {
-        private readonly IWebHostEnvironment _hostingEnvironment;
+        private readonly ILogger<TaxiLegController> _logger;
+        private IWebHostEnvironment _hostingEnvironment;
         private readonly IUnitOfWork _unitOfWork;
         private readonly ICorridasTaxiLegRepository _corridasTaxiLegRepository;
-        private readonly ILogService _log;
-
-        public TaxiLegController(IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, ICorridasTaxiLegRepository corridasTaxiLegRepository, ILogService log)
+
+        private string ExtrairHora(IRow row , int cellIndex)
         {
             try
             {
+                var cell = row.GetCell(cellIndex);
+                if (cell != null)
+                {
+                    if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
+                    {
+                        return cell.DateCellValue.ToString("HH:mm");
+                    }
+                    else
+                    {
+                        string raw = cell.ToString().Trim();
+
+                        if (TimeSpan.TryParse(raw , out var ts))
+                            return ts.ToString(@"hh\:mm");
+                        else if (DateTime.TryParse(raw , out var dt))
+                            return dt.ToString("HH:mm");
+                    }
+                }
+                return "";
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("TaxiLegController.cs" , "ExtrairHora" , error);
+                return string.Empty;
+            }
+        }
+
+        public TaxiLegController(
+            ILogger<TaxiLegController> logger ,
+            IWebHostEnvironment hostingEnvironment ,
+            IUnitOfWork unitOfWork ,
+            ICorridasTaxiLegRepository corridasTaxiLegRepository
+        )
+        {
+            try
+            {
+                _logger = logger;
                 _hostingEnvironment = hostingEnvironment;
                 _unitOfWork = unitOfWork;
                 _corridasTaxiLegRepository = corridasTaxiLegRepository;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Constructor", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("TaxiLegController.cs" , "TaxiLegController" , error);
             }
         }
 
         [BindProperty]
-        public Models.CorridasTaxiLeg TaxiLegObj { get; set; }
-        public Models.CorridasCanceladasTaxiLeg TaxiLegCanceladasObj { get; set; }
+        public Models.CorridasTaxiLeg TaxiLegObj
+        {
+            get; set;
+        }
+        public Models.CorridasCanceladasTaxiLeg TaxiLegCanceladasObj
+        {
+            get; set;
+        }
 
         public IActionResult Index()
         {
             try
             {
-
-                _log.Info("TaxiLegController.Index: Acessando console de auditoria Taxi Leg.");
-
                 return View();
             }
-            catch (Exception ex)
-            {
-                _log.Error("TaxiLegController.Index", ex);
-                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Index", ex);
-
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("TaxiLegController.cs" , "Index" , error);
                 return View();
             }
         }
@@ -69,16 +104,15 @@
         {
             try
             {
-
-                _log.Info("TaxiLegController.Get: Checkpoint de conexão bem-sucedido.");
-
                 return Json(true);
             }
-            catch (Exception ex)
-            {
-                _log.Error("TaxiLegController.Get", ex);
-                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Get", ex);
-                return Json(new { success = false });
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("TaxiLegController.cs" , "Get" , error);
+                return Json(new
+                {
+                    success = false
+                });
             }
         }
 
@@ -88,12 +122,10 @@
         {
             try
             {
-
                 IFormFile file = Request.Form.Files[0];
-
                 string folderName = "DadosEditaveis/UploadExcel";
                 string webRootPath = _hostingEnvironment.WebRootPath;
-                string newPath = Path.Combine(webRootPath, folderName);
+                string newPath = Path.Combine(webRootPath , folderName);
 
                 if (!Directory.Exists(newPath))
                 {
@@ -104,14 +136,12 @@
 
                 if (file.Length > 0)
                 {
-
                     string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                     ISheet sheet;
-                    string fullPath = Path.Combine(newPath, file.FileName);
-
-                    using (var stream = new FileStream(fullPath, FileMode.Create))
+                    string fullPath = Path.Combine(newPath , file.FileName);
+
+                    using (var stream = new FileStream(fullPath , FileMode.Create))
                     {
-
                         file.CopyTo(stream);
                         stream.Position = 0;
 
@@ -122,17 +152,18 @@
                         }
                         else
                         {
-                            XSSFWorkbook xssfwb = new XSSFWorkbook(stream);
-                            sheet = xssfwb.GetSheetAt(0);
+                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
+                            sheet = hssfwb.GetSheetAt(0);
                         }
 
                         DateTime? primeiraDataAgenda = null;
                         for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                         {
                             IRow row = sheet.GetRow(i);
-                            if (row == null) continue;
-
-                            if (DateTime.TryParse(row.GetCell(7)?.ToString(), out var dataAgenda))
+                            if (row == null)
+                                continue;
+
+                            if (DateTime.TryParse(row.GetCell(7)?.ToString() , out var dataAgenda))
                             {
                                 primeiraDataAgenda = dataAgenda;
                                 break;
@@ -141,21 +172,29 @@
 
                         if (primeiraDataAgenda == null)
                         {
-
-                            _log.Warning("TaxiLegController.Import: Planilha sem datas válidas na coluna 7.");
-                            return Json(new { success = false, message = "Não foi possível determinar a data das corridas." });
+                            return Json(
+                                new
+                                {
+                                    success = false ,
+                                    message = "Não foi possível determinar a data das corridas." ,
+                                    data = (object)null ,
+                                }
+                            );
                         }
 
                         var ano = primeiraDataAgenda.Value.Year;
                         var mes = primeiraDataAgenda.Value.Month;
-
-                        if (_corridasTaxiLegRepository.ExisteCorridaNoMesAno(ano, mes))
-                        {
-                            _log.Warning($"TaxiLegController.Import: O mês {mes:D2}/{ano} já consta como importado.");
-                            return Json(new { success = false, message = $"O mês {mes:D2}/{ano} já foi importado!" });
-                        }
-
-                        _log.Info($"TaxiLegController.Import: Iniciando parsing de {sheet.LastRowNum} linhas para {mes:00}/{ano}.");
+                        if (_corridasTaxiLegRepository.ExisteCorridaNoMesAno(ano , mes))
+                        {
+                            return Json(
+                                new
+                                {
+                                    success = false ,
+                                    message = $"O mês {mes:D2}/{ano} já foi importado!" ,
+                                    data = (object)null ,
+                                }
+                            );
+                        }
 
                         for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                         {
@@ -286,7 +325,7 @@
 
                             TaxiLegObj.Glosa = TaxiLegObj.Espera > 15;
                             TaxiLegObj.ValorGlosa = (bool)TaxiLegObj.Glosa
-                                ? Math.Round((double)(TaxiLegObj.KmReal * 2.44), 2)
+                                ? Math.Round((double)(TaxiLegObj.KmReal * 2.44) , 2)
                                 : 0;
 
                             listaCorridas.Add(TaxiLegObj);
@@ -294,7 +333,6 @@
                         }
 
                         _unitOfWork.Save();
-                        _log.Info($"TaxiLegController.Import: Sucesso na importação de {listaCorridas.Count} registros de corridas.");
                     }
                 }
 
@@ -303,19 +341,18 @@
                 return Json(
                     new
                     {
-                        success = true,
-                        message = "Planilha Importada com Sucesso",
-                        data = listaCorridas,
+                        success = true ,
+                        message = "Planilha Importada com Sucesso" ,
+                        data = listaCorridas ,
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("TaxiLegController.Import", ex);
-                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Import", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("TaxiLegController.cs" , "Import" , error);
                 return Json(new
                 {
-                    success = false,
+                    success = false ,
                     message = "Erro ao importar planilha"
                 });
             }
@@ -327,33 +364,24 @@
         {
             try
             {
-
                 IFormFile file = Request.Form.Files[0];
-
                 string folderName = "DadosEditaveis/UploadExcel";
                 string webRootPath = _hostingEnvironment.WebRootPath;
-                string newPath = Path.Combine(webRootPath, folderName);
+                string newPath = Path.Combine(webRootPath , folderName);
                 StringBuilder sb = new StringBuilder();
-
                 if (!Directory.Exists(newPath))
                 {
                     Directory.CreateDirectory(newPath);
                 }
-
                 if (file.Length > 0)
                 {
-
-                    _log.Info($"TaxiLegController.ImportCanceladas: Iniciando importação de canceladas [{file.FileName}]");
                     string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                     ISheet sheet;
-                    string fullPath = Path.Combine(newPath, file.FileName);
-
-                    using (var stream = new FileStream(fullPath, FileMode.Create))
+                    string fullPath = Path.Combine(newPath , file.FileName);
+                    using (var stream = new FileStream(fullPath , FileMode.Create))
                     {
-
                         file.CopyTo(stream);
                         stream.Position = 0;
-
                         if (sFileExtension == ".xls")
                         {
                             HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
@@ -361,126 +389,291 @@
                         }
                         else
                         {
-                            XSSFWorkbook xssfwb = new XSSFWorkbook(stream);
-                            sheet = xssfwb.GetSheetAt(0);
+                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
+                            sheet = hssfwb.GetSheetAt(0);
                         }
 
                         IRow headerRow = sheet.GetRow(0);
                         int cellCount = headerRow.LastCellNum;
-                        sb.Append("<table id='tblImportacao' class='display' style='width: 100%'><thead><tr>");
+                        sb.Append(
+                            "<table id='tblImportacao' class='display' style='width: 100%'><thead><tr>"
+                        );
                         for (int j = 0; j < cellCount; j++)
                         {
-                            ICell cell = headerRow.GetCell(j);
+                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                             sb.Append("<th>" + cell.ToString() + "</th>");
                         }
-                        sb.Append("<th>Espera</th></tr></thead><tbody>");
-
+                        sb.Append("<th>" + "Espera" + "</th>");
+                        sb.Append("</tr></thead>");
+
+                        sb.AppendLine("<tbody><tr>");
                         for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                         {
                             IRow row = sheet.GetRow(i);
-                            if (row == null || row.Cells.All(d => d.CellType == CellType.Blank)) continue;
-
+                            if (row == null)
+                                continue;
+                            if (row.Cells.All(d => d.CellType == CellType.Blank))
+                                continue;
                             TaxiLegCanceladasObj = new CorridasCanceladasTaxiLeg();
-                            sb.Append("<tr>");
-
                             for (int j = row.FirstCellNum; j < cellCount; j++)
                             {
-                                string celulaValor = row.GetCell(j)?.ToString() ?? "N/A";
-                                sb.Append("<td>" + celulaValor + "</td>");
-
-                                switch (j)
-                                {
-                                    case 0: TaxiLegCanceladasObj.Origem = celulaValor; break;
-                                    case 1: TaxiLegCanceladasObj.Setor = celulaValor; break;
-                                    case 2: TaxiLegCanceladasObj.SetorExtra = celulaValor; break;
-                                    case 3: TaxiLegCanceladasObj.Unidade = celulaValor; break;
-                                    case 4: TaxiLegCanceladasObj.UnidadeExtra = celulaValor; break;
-                                    case 5:
-                                        int.TryParse(celulaValor, out int p);
-                                        TaxiLegCanceladasObj.QtdPassageiros = p > 0 ? p : 1;
-                                        break;
-                                    case 6: TaxiLegCanceladasObj.MotivoUso = celulaValor; break;
-                                    case 7:
-                                        if (DateTime.TryParse(celulaValor, out var dtAlt)) TaxiLegCanceladasObj.DataAgenda = dtAlt;
-                                        break;
-                                    case 8:
-                                        if (DateTime.TryParse(celulaValor, out var hrAlt)) TaxiLegCanceladasObj.HoraAgenda = hrAlt.ToShortTimeString();
-                                        break;
-                                    case 9:
-                                        if (DateTime.TryParse(celulaValor, out var hrCanc))
+                                if (j == 0)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.Origem = row.GetCell(j).ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 1)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.Setor = row.GetCell(j).ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 2)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.SetorExtra = row.GetCell(j).ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 3)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.Unidade = row.GetCell(j).ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 4)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.UnidadeExtra = row.GetCell(j)
+                                            .ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 5)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        try
                                         {
-                                            TaxiLegCanceladasObj.DataHoraCancelamento = hrCanc;
-                                            TaxiLegCanceladasObj.HoraCancelamento = hrCanc.ToShortTimeString();
+                                            TaxiLegCanceladasObj.QtdPassageiros = int.Parse(
+                                                row.GetCell(j).ToString()
+                                            );
                                         }
-                                        break;
-                                    case 10: TaxiLegCanceladasObj.TipoCancelamento = celulaValor; break;
-                                    case 11: TaxiLegCanceladasObj.MotivoCancelamento = celulaValor; break;
+                                        catch (Exception)
+                                        {
+                                            TaxiLegCanceladasObj.QtdPassageiros = 1;
+                                        }
+                                        sb.Append(
+                                            "<td>" + TaxiLegCanceladasObj.QtdPassageiros + "</td>"
+                                        );
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 6)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.MotivoUso = row.GetCell(j).ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 7)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.DataAgenda = Convert.ToDateTime(
+                                            row.GetCell(j).ToString()
+                                        );
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 8)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.HoraAgenda = DateTime
+                                            .Parse(row.GetCell(j).ToString())
+                                            .ToShortTimeString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 9)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.DataHoraCancelamento = DateTime.Parse(
+                                            row.GetCell(j).ToString()
+                                        );
+                                        TaxiLegCanceladasObj.HoraCancelamento = Convert
+                                            .ToDateTime(row.GetCell(j).ToString())
+                                            .ToShortTimeString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 10)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.TipoCancelamento = row.GetCell(j)
+                                            .ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
+                                }
+
+                                if (j == 11)
+                                {
+                                    if (row.GetCell(j) != null)
+                                    {
+                                        TaxiLegCanceladasObj.MotivoCancelamento = row.GetCell(j)
+                                            .ToString();
+                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                    }
+                                    else
+                                    {
+                                        sb.Append("<td> N/A </td>");
+                                    }
                                 }
                             }
 
-                            if (!string.IsNullOrEmpty(TaxiLegCanceladasObj.HoraAgenda) && !string.IsNullOrEmpty(TaxiLegCanceladasObj.HoraCancelamento))
+                            DateTime startTime = DateTime.Parse(TaxiLegCanceladasObj.HoraAgenda);
+                            DateTime endTime = DateTime.Parse(
+                                TaxiLegCanceladasObj.HoraCancelamento
+                            );
+
+                            TimeSpan span = endTime.Subtract(startTime);
+
+                            TaxiLegCanceladasObj.TempoEspera = (int?)span.TotalMinutes;
+
+                            if (TaxiLegCanceladasObj.TempoEspera < 0)
                             {
-                                DateTime startTime = DateTime.Parse(TaxiLegCanceladasObj.HoraAgenda);
-                                DateTime endTime = DateTime.Parse(TaxiLegCanceladasObj.HoraCancelamento);
-                                TimeSpan span = endTime.Subtract(startTime);
-
-                                if (span.TotalMinutes < 0)
-                                    TaxiLegCanceladasObj.TempoEspera = (int)span.Add(TimeSpan.FromDays(1)).TotalMinutes;
-                                else
-                                    TaxiLegCanceladasObj.TempoEspera = (int)span.TotalMinutes;
+                                DateTime startTimeAnterior = DateTime.Parse(
+                                    TaxiLegCanceladasObj.HoraAgenda
+                                );
+                                DateTime endTimeAnterior = DateTime.Parse("00:00:00");
+                                endTimeAnterior = endTimeAnterior.AddDays(1);
+
+                                TimeSpan spanAnterior = endTimeAnterior.Subtract(startTimeAnterior);
+
+                                DateTime startTimePosterior = DateTime.Parse("00:00:00");
+                                DateTime endTimePosterior = DateTime.Parse(
+                                    TaxiLegCanceladasObj.HoraCancelamento
+                                );
+
+                                TimeSpan spanPosterior = endTimePosterior.Subtract(
+                                    startTimePosterior
+                                );
+
+                                TaxiLegCanceladasObj.TempoEspera =
+                                    (int?)spanAnterior.TotalMinutes
+                                    + (int?)spanPosterior.TotalMinutes;
                             }
 
-                            sb.Append("<td>" + TaxiLegCanceladasObj.TempoEspera + "</td></tr>");
+                            sb.Append(
+                                "<td>" + TaxiLegCanceladasObj.TempoEspera.ToString() + "</td>"
+                            );
+                            sb.AppendLine("</tr>");
+
                             _unitOfWork.CorridasCanceladasTaxiLeg.Add(TaxiLegCanceladasObj);
-                        }
-
-                        _unitOfWork.Save();
-                        _log.Info($"TaxiLegController.ImportCanceladas: Sucesso na importação das corridas canceladas.");
+                            _unitOfWork.Save();
+                        }
+
                         sb.Append("</tbody></table>");
                     }
 
-                    return Json(new { success = true, message = "Planilha Importada com Sucesso", response = this.Content(sb.ToString()) });
+                    sb.Append("</tbody></table>");
+
+                    return Json(
+                        new
+                        {
+                            success = true ,
+                            message = "Planilha Importada com Sucesso" ,
+                            response = this.Content(sb.ToString()) ,
+                        }
+                    );
                 }
-
-                return Json(new { success = false, message = "Arquivo vazio ou inválido." });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("TaxiLegController.ImportCanceladas", ex);
-                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "ImportCanceladas", ex);
-                return Json(new { success = false, message = "Erro ao importar corridas canceladas" });
-            }
-        }
-
-        private string ExtrairHora(IRow row, int cellIndex)
-        {
-            try
-            {
-
-                var cell = row.GetCell(cellIndex);
-                if (cell == null) return "";
-
-                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
-                {
-                    return cell.DateCellValue.ToString("HH:mm");
+                else
+                {
+                    return Json(
+                        new
+                        {
+                            success = false ,
+                            message = "Planilha Não Importada" ,
+                            response = this.Content(sb.ToString()) ,
+                        }
+                    );
                 }
-
-                string valor = cell.ToString();
-                if (string.IsNullOrEmpty(valor)) return "";
-
-                if (DateTime.TryParse(valor, out var dt))
-                {
-                    return dt.ToString("HH:mm");
-                }
-
-                return valor;
-            }
-            catch (Exception ex)
-            {
-                _log.Error("TaxiLegController.ExtrairHora", ex);
-                return "";
-            }
-        }
-
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("TaxiLegController.cs" , "ImportCanceladas" , error);
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao importar corridas canceladas"
+                });
+            }
+        }
     }
 }
```
