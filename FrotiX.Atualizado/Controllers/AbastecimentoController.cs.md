# Controllers/AbastecimentoController.cs

**Mudanca:** GRANDE | **+460** linhas | **-243** linhas

---

```diff
--- JANEIRO: Controllers/AbastecimentoController.cs
+++ ATUAL: Controllers/AbastecimentoController.cs
@@ -17,30 +17,26 @@
 using System.Text;
 using System.Transactions;
 using Microsoft.AspNetCore.Mvc.Infrastructure;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("api/[controller]")]
     [ApiController]
-    public partial class AbastecimentoController : ControllerBase
+    public partial class AbastecimentoController :ControllerBase
     {
         private readonly ILogger<AbastecimentoController> _logger;
         private readonly IWebHostEnvironment _hostingEnvironment;
         private readonly IUnitOfWork _unitOfWork;
         private readonly IHubContext<ImportacaoHub> _hubContext;
         private readonly FrotiXDbContext _context;
-        private readonly ILogService _log;
 
         public AbastecimentoController(
-            ILogger<AbastecimentoController> logger,
-            IWebHostEnvironment hostingEnvironment,
-            IUnitOfWork unitOfWork,
+            ILogger<AbastecimentoController> logger ,
+            IWebHostEnvironment hostingEnvironment ,
+            IUnitOfWork unitOfWork ,
             IHubContext<ImportacaoHub> hubContext,
-            FrotiXDbContext context,
-            ILogService log
+            FrotiXDbContext context
         )
         {
             try
@@ -50,29 +46,31 @@
                 _unitOfWork = unitOfWork;
                 _hubContext = hubContext;
                 _context = context;
-                _log = log;
-
-                _log.Info("AbastecimentoController inicializado", "AbastecimentoController.cs", "Constructor");
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Constructor", ex);
-            }
-        }
-
-        public Models.Abastecimento AbastecimentoObj { get; set; }
-
-        [HttpGet("HealthCheck")]
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "AbastecimentoController" ,
+                    error
+                );
+            }
+        }
+
+        public Models.Abastecimento AbastecimentoObj
+        {
+            get; set;
+        }
+
         public IActionResult Index()
         {
             try
             {
-                return Ok(new { status = "Online", service = "Abastecimento" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro no HealthCheck de Abastecimento", error, "AbastecimentoController.cs", "Index");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Index", error);
+                return StatusCode(200);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Index" , error);
                 return StatusCode(500);
             }
         }
@@ -82,142 +80,183 @@
         {
             try
             {
+
                 var dados = _unitOfWork
                     .ViewAbastecimentos.GetAll()
                     .OrderByDescending(va => va.DataHora)
-                    .Take(1000)
                     .ToList();
 
-                return Ok(new { data = dados });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro ao carregar listagem de abastecimentos", error, "AbastecimentoController.cs", "Get");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Get", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("AbastecimentoVeiculos")]
+                return Ok(new
+                {
+                    data = dados
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Get" , error);
+                return StatusCode(500);
+            }
+        }
+
+        [Route("AbastecimentoVeiculos")]
+        [HttpGet]
         public IActionResult AbastecimentoVeiculos(Guid Id)
         {
             try
             {
+
                 var dados = _unitOfWork
                     .ViewAbastecimentos.GetAll()
                     .Where(va => va.VeiculoId == Id)
                     .OrderByDescending(va => va.DataHora)
                     .ToList();
 
-                return Ok(new { data = dados });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao filtrar abastecimentos por veículo {Id}", error, "AbastecimentoController.cs", "AbastecimentoVeiculos");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoVeiculos", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("AbastecimentoCombustivel")]
+                return Ok(new
+                {
+                    data = dados
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "AbastecimentoVeiculos" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("AbastecimentoCombustivel")]
+        [HttpGet]
         public IActionResult AbastecimentoCombustivel(Guid Id)
         {
             try
             {
+
                 var dados = _unitOfWork
                     .ViewAbastecimentos.GetAll()
                     .Where(va => va.CombustivelId == Id)
                     .OrderByDescending(va => va.DataHora)
                     .ToList();
 
-                return Ok(new { data = dados });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao filtrar abastecimentos por combustível {Id}", error, "AbastecimentoController.cs", "AbastecimentoCombustivel");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoCombustivel", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("AbastecimentoUnidade")]
+                return Ok(new
+                {
+                    data = dados
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "AbastecimentoCombustivel" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("AbastecimentoUnidade")]
+        [HttpGet]
         public IActionResult AbastecimentoUnidade(Guid Id)
         {
             try
             {
+
                 var dados = _unitOfWork
                     .ViewAbastecimentos.GetAll()
                     .Where(va => va.UnidadeId == Id)
                     .OrderByDescending(va => va.DataHora)
                     .ToList();
 
-                return Ok(new { data = dados });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao filtrar abastecimentos por unidade {Id}", error, "AbastecimentoController.cs", "AbastecimentoUnidade");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoUnidade", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("AbastecimentoMotorista")]
+                return Ok(new
+                {
+                    data = dados
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "AbastecimentoUnidade" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("AbastecimentoMotorista")]
+        [HttpGet]
         public IActionResult AbastecimentoMotorista(Guid Id)
         {
             try
             {
+
                 var dados = _unitOfWork
                     .ViewAbastecimentos.GetAll()
                     .Where(va => va.MotoristaId == Id)
                     .OrderByDescending(va => va.DataHora)
                     .ToList();
 
-                return Ok(new { data = dados });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao filtrar abastecimentos por motorista {Id}", error, "AbastecimentoController.cs", "AbastecimentoMotorista");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoMotorista", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("AbastecimentoData")]
+                return Ok(new
+                {
+                    data = dados
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "AbastecimentoMotorista" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("AbastecimentoData")]
+        [HttpGet]
         public IActionResult AbastecimentoData(string dataAbastecimento)
         {
             try
             {
+
                 var dados = _unitOfWork
                     .ViewAbastecimentos.GetAll()
                     .Where(va => va.Data == dataAbastecimento)
                     .OrderByDescending(va => va.DataHora)
                     .ToList();
 
-                return Ok(new { data = dados });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao filtrar abastecimentos por data {dataAbastecimento}", error, "AbastecimentoController.cs", "AbastecimentoData");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoData", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpPost("Import")]
+                return Ok(new
+                {
+                    data = dados
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "AbastecimentoData" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("Import")]
+        [HttpPost]
         public ActionResult Import()
         {
             try
             {
-
                 IFormFile file = Request.Form.Files[0];
                 string folderName = "DadosEditaveis/UploadExcel";
                 string webRootPath = _hostingEnvironment.WebRootPath;
-                string newPath = Path.Combine(webRootPath, folderName);
+                string newPath = Path.Combine(webRootPath , folderName);
                 StringBuilder sb = new StringBuilder();
 
                 if (!Directory.Exists(newPath))
                 {
-
                     Directory.CreateDirectory(newPath);
                 }
 
@@ -226,11 +265,10 @@
 
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
 
@@ -247,12 +285,23 @@
 
                         IRow headerRow = sheet.GetRow(0);
                         int cellCount = headerRow.LastCellNum;
-                        sb.Append("<table id='tblImportacao' class='display' style='width: 100%'><thead><tr>");
+                        sb.Append(
+                            "<table id='tblImportacao' class='display' style='width: 100%'><thead><tr>"
+                        );
 
                         for (int j = 0; j < cellCount; j++)
                         {
                             NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
-                            if (j == 5 || j == 7 || j == 10 || j == 11 || j == 12 || j == 13 || j == 14 || j == 15)
+                            if (
+                                j == 5
+                                || j == 7
+                                || j == 10
+                                || j == 11
+                                || j == 12
+                                || j == 13
+                                || j == 14
+                                || j == 15
+                            )
                             {
                                 sb.Append("<th>" + cell.ToString() + "</th>");
                             }
@@ -265,125 +314,240 @@
                         try
                         {
 
-                            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 30, 30)))
+                            using (
+                                TransactionScope scope = new TransactionScope(
+                                    TransactionScopeOption.RequiresNew ,
+                                    new TimeSpan(0 , 30 , 30)
+                                )
+                            )
                             {
                                 sb.AppendLine("<tbody><tr>");
 
                                 for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                                 {
                                     IRow row = sheet.GetRow(i);
-                                    if (row == null) continue;
-
-                                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
-
-                                    AbastecimentoObj = new Models.Abastecimento();
-                                    AbastecimentoObj.AbastecimentoId = Guid.NewGuid();
+                                    if (row == null)
+                                        continue;
+
+                                    if (row.Cells.All(d => d.CellType == CellType.Blank))
+                                        continue;
+
+                                    AbastecimentoObj = new Abastecimento();
 
                                     for (int j = row.FirstCellNum; j < cellCount; j++)
                                     {
                                         if (row.GetCell(j) != null)
                                         {
 
-                                            if (i == 1 && j == 0)
+                                            if (i == 1)
                                             {
-                                                var dataStr = row.GetCell(j).ToString();
-                                                var dataDt = Convert.ToDateTime(dataStr);
-                                                var objFromDb = _unitOfWork.Abastecimento.GetFirstOrDefault(u => u.DataHora == dataDt);
-                                                if (objFromDb != null)
+                                                if (j == 0)
                                                 {
-
-                                                    return Ok(new { success = false, message = "Os registros para o dia " + dataStr + " já foram importados!" });
+                                                    var objFromDb =
+                                                        _unitOfWork.Abastecimento.GetFirstOrDefault(
+                                                            u =>
+                                                                u.DataHora
+                                                                == Convert.ToDateTime(
+                                                                    row.GetCell(j).ToString()
+                                                                )
+                                                        );
+                                                    if (objFromDb != null)
+                                                    {
+                                                        return Ok(
+                                                            new
+                                                            {
+                                                                success = false ,
+                                                                message = "Os registros para o dia "
+                                                                    + Convert.ToDateTime(
+                                                                        row.GetCell(j).ToString()
+                                                                    )
+                                                                    + " já foram importados!" ,
+                                                            }
+                                                        );
+                                                    }
                                                 }
                                             }
 
                                             if (j == 7)
                                             {
-                                                AbastecimentoObj.DataHora = Convert.ToDateTime(row.GetCell(j).ToString());
-                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                                AbastecimentoObj.DataHora = Convert.ToDateTime(
+                                                    row.GetCell(j).ToString()
+                                                );
+                                                sb.Append(
+                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
+                                                );
                                             }
 
                                             if (j == 5)
                                             {
                                                 string placaVeiculo = row.GetCell(j).ToString();
-                                                var veiculoObj = _unitOfWork.Veiculo.GetFirstOrDefault(m => m.Placa == placaVeiculo);
+
+                                                var veiculoObj =
+                                                    _unitOfWork.Veiculo.GetFirstOrDefault(m =>
+                                                        m.Placa == placaVeiculo
+                                                    );
                                                 if (veiculoObj != null)
                                                 {
-                                                    AbastecimentoObj.VeiculoId = veiculoObj.VeiculoId;
-                                                    sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                                    AbastecimentoObj.VeiculoId =
+                                                        veiculoObj.VeiculoId;
+                                                    sb.Append(
+                                                        "<td>" + row.GetCell(j).ToString() + "</td>"
+                                                    );
                                                 }
                                                 else
                                                 {
-
-                                                    return Ok(new { success = false, message = "Não foi encontrado o veículo de placa: " + placaVeiculo });
+                                                    return Ok(
+                                                        new
+                                                        {
+                                                            success = false ,
+                                                            message =
+                                                                "Não foi encontrado o veículo de placa: "
+                                                                + placaVeiculo ,
+                                                        }
+                                                    );
                                                 }
                                             }
 
                                             if (j == 10)
                                             {
-                                                string motorista = row.GetCell(j).ToString().Replace(".", "");
-                                                var motoristaObj = _unitOfWork.Motorista.GetFirstOrDefault(m => m.Nome == motorista);
+                                                string motorista = row.GetCell(j).ToString();
+                                                motorista = motorista.Replace("." , "");
+
+                                                var motoristaObj =
+                                                    _unitOfWork.Motorista.GetFirstOrDefault(m =>
+                                                        m.Nome == motorista
+                                                    );
                                                 if (motoristaObj != null)
                                                 {
-                                                    AbastecimentoObj.MotoristaId = motoristaObj.MotoristaId;
-                                                    sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                                    AbastecimentoObj.MotoristaId =
+                                                        motoristaObj.MotoristaId;
+                                                    sb.Append(
+                                                        "<td>" + row.GetCell(j).ToString() + "</td>"
+                                                    );
                                                 }
                                                 else
                                                 {
-
-                                                    return Ok(new { success = false, message = "Não foi encontrado o(a) motorista: " + motorista });
+                                                    return Ok(
+                                                        new
+                                                        {
+                                                            success = false ,
+                                                            message =
+                                                                "Não foi encontrado o(a) motorista: "
+                                                                + motorista ,
+                                                        }
+                                                    );
                                                 }
                                             }
 
                                             if (j == 12)
                                             {
-                                                AbastecimentoObj.Hodometro = Convert.ToInt32(row.GetCell(j).ToString());
-                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                                AbastecimentoObj.Hodometro = Convert.ToInt32(
+                                                    row.GetCell(j).ToString()
+                                                );
+                                                sb.Append(
+                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
+                                                );
                                             }
 
                                             if (j == 11)
                                             {
-                                                AbastecimentoObj.KmRodado = Convert.ToInt32(row.GetCell(12).ToString()) - Convert.ToInt32(row.GetCell(11).ToString());
-                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                                AbastecimentoObj.KmRodado =
+                                                    Convert.ToInt32(row.GetCell(12).ToString())
+                                                    - Convert.ToInt32(row.GetCell(11).ToString());
+                                                sb.Append(
+                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
+                                                );
                                             }
 
                                             if (j == 13)
                                             {
                                                 if (row.GetCell(j).ToString() == "GASOLINA")
                                                 {
-                                                    AbastecimentoObj.CombustivelId = Guid.Parse("F668F660-8380-4DF3-90CD-787DB06FE734");
+                                                    AbastecimentoObj.CombustivelId = Guid.Parse(
+                                                        "F668F660-8380-4DF3-90CD-787DB06FE734"
+                                                    );
                                                 }
                                                 else
                                                 {
-                                                    AbastecimentoObj.CombustivelId = Guid.Parse("A69AA86A-9162-4242-AB9A-8B184E04C4DA");
+                                                    AbastecimentoObj.CombustivelId = Guid.Parse(
+                                                        "A69AA86A-9162-4242-AB9A-8B184E04C4DA"
+                                                    );
                                                 }
-                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
+                                                sb.Append(
+                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
+                                                );
                                             }
 
                                             if (j == 14)
                                             {
-                                                AbastecimentoObj.ValorUnitario = Convert.ToDouble(row.GetCell(j).ToString());
-                                                sb.Append("<td>" + Math.Round((double)AbastecimentoObj.ValorUnitario, 2).ToString("0.00") + "</td>");
+                                                AbastecimentoObj.ValorUnitario = Convert.ToDouble(
+                                                    row.GetCell(j).ToString()
+                                                );
+                                                sb.Append(
+                                                    "<td>"
+                                                        + Math.Round(
+                                                                (double)
+                                                                    AbastecimentoObj.ValorUnitario ,
+                                                                2
+                                                            )
+                                                            .ToString("0.00")
+                                                        + "</td>"
+                                                );
                                             }
 
                                             if (j == 15)
                                             {
-                                                AbastecimentoObj.Litros = Convert.ToDouble(row.GetCell(j).ToString());
-                                                sb.Append("<td>" + Math.Round((double)AbastecimentoObj.Litros, 2).ToString("0.00") + "</td>");
+                                                AbastecimentoObj.Litros = Convert.ToDouble(
+                                                    row.GetCell(j).ToString()
+                                                );
+                                                sb.Append(
+                                                    "<td>"
+                                                        + Math.Round(
+                                                                (double)AbastecimentoObj.Litros ,
+                                                                2
+                                                            )
+                                                            .ToString("0.00")
+                                                        + "</td>"
+                                                );
                                             }
                                         }
                                     }
 
-                                    sb.Append("<td>" + Math.Round(((double)AbastecimentoObj.KmRodado / (double)AbastecimentoObj.Litros), 2).ToString("0.00") + "</td>");
-
-                                    var mediaveiculo = _unitOfWork.ViewMediaConsumo.GetFirstOrDefault(v => v.VeiculoId == AbastecimentoObj.VeiculoId);
+                                    sb.Append(
+                                        "<td>"
+                                            + Math.Round(
+                                                    (
+                                                        (double)AbastecimentoObj.KmRodado
+                                                        / (double)AbastecimentoObj.Litros
+                                                    ) ,
+                                                    2
+                                                )
+                                                .ToString("0.00")
+                                            + "</td>"
+                                    );
+
+                                    var mediaveiculo =
+                                        _unitOfWork.ViewMediaConsumo.GetFirstOrDefault(v =>
+                                            v.VeiculoId == AbastecimentoObj.VeiculoId
+                                        );
                                     if (mediaveiculo != null)
                                     {
                                         sb.Append("<td>" + mediaveiculo.ConsumoGeral + "</td>");
                                     }
                                     else
                                     {
-
-                                        sb.Append("<td>" + Math.Round(((double)AbastecimentoObj.KmRodado / (double)AbastecimentoObj.Litros), 2).ToString("0.00") + "</td>");
+                                        sb.Append(
+                                            "<td>"
+                                                + Math.Round(
+                                                        (
+                                                            (double)AbastecimentoObj.KmRodado
+                                                            / (double)AbastecimentoObj.Litros
+                                                        ) ,
+                                                        2
+                                                    )
+                                                    .ToString("0.00")
+                                                + "</td>"
+                                        );
                                     }
 
                                     sb.AppendLine("</tr>");
@@ -393,92 +557,111 @@
                                 }
 
                                 sb.Append("</tbody></table>");
-
                                 scope.Complete();
                             }
                         }
                         catch (Exception error)
                         {
-
-                            _log.Error("Falha crítica durante transação de importação", error, "AbastecimentoController.cs", "Import.TransactionScope");
-                            Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Import.TransactionScope", error);
+                            Alerta.TratamentoErroComLinha(
+                                "AbastecimentoController.cs" ,
+                                "Import.TransactionScope" ,
+                                error
+                            );
                             throw;
                         }
                     }
                 }
 
-                _log.Info($"Planilha importada com sucesso: {file.FileName}", "AbastecimentoController.cs", "Import");
-
-                return Ok(new
-                {
-                    success = true,
-                    message = "Planilha Importada com Sucesso",
-                    response = sb.ToString(),
-                });
-            }
-            catch (Exception error)
-            {
-
-                _log.Error("Erro fatal no processo de importação legado", error, "AbastecimentoController.cs", "Import");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Import", error);
-
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("MotoristaList")]
+                return Ok(
+                    new
+                    {
+                        success = true ,
+                        message = "Planilha Importada com Sucesso" ,
+                        response = sb.ToString() ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Import" , error);
+                return StatusCode(500);
+            }
+        }
+
+        [Route("MotoristaList")]
+        [HttpGet]
         public IActionResult MotoristaList()
         {
             try
             {
+
                 var result = _unitOfWork.ViewMotoristas.GetAll().OrderBy(vm => vm.Nome).ToList();
-                return Ok(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro ao carregar lista de motoristas", error, "AbastecimentoController.cs", "MotoristaList");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "MotoristaList", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("UnidadeList")]
+
+                return Ok(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "MotoristaList" , error);
+                return StatusCode(500);
+            }
+        }
+
+        [Route("UnidadeList")]
+        [HttpGet]
         public IActionResult UnidadeList()
         {
             try
             {
+
                 var result = _unitOfWork.Unidade.GetAll().OrderBy(u => u.Descricao).ToList();
-                return Ok(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro ao carregar lista de unidades", error, "AbastecimentoController.cs", "UnidadeList");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "UnidadeList", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("CombustivelList")]
+
+                return Ok(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "UnidadeList" , error);
+                return StatusCode(500);
+            }
+        }
+
+        [Route("CombustivelList")]
+        [HttpGet]
         public IActionResult CombustivelList()
         {
             try
             {
+
                 var result = _unitOfWork.Combustivel.GetAll().OrderBy(u => u.Descricao).ToList();
-                return Ok(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro ao carregar lista de combustíveis", error, "AbastecimentoController.cs", "CombustivelList");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "CombustivelList", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("VeiculoList")]
+
+                return Ok(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "CombustivelList" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("VeiculoList")]
+        [HttpGet]
         public IActionResult VeiculoList()
         {
             try
             {
+
                 var result = (
                     from v in _unitOfWork.Veiculo.GetAll()
                     join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
@@ -486,24 +669,31 @@
                     orderby v.Placa
                     select new
                     {
-                        v.VeiculoId,
-                        PlacaMarcaModelo = v.Placa + " - " + ma.DescricaoMarca + "/" + m.DescricaoModelo,
+                        v.VeiculoId ,
+                        PlacaMarcaModelo = v.Placa
+                            + " - "
+                            + ma.DescricaoMarca
+                            + "/"
+                            + m.DescricaoModelo ,
                     }
                 )
-                .OrderBy(v => v.PlacaMarcaModelo)
-                .ToList();
-
-                return Ok(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro ao carregar lista de veículos", error, "AbastecimentoController.cs", "VeiculoList");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "VeiculoList", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpPost("AtualizaQuilometragem")]
+                    .OrderBy(v => v.PlacaMarcaModelo)
+                    .ToList();
+
+                return Ok(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "VeiculoList" , error);
+                return StatusCode(500);
+            }
+        }
+
+        [Route("AtualizaQuilometragem")]
+        [HttpPost]
         public IActionResult AtualizaQuilometragem([FromBody] Dictionary<string, object> payload)
         {
             try
@@ -511,16 +701,17 @@
 
                 if (!payload.TryGetValue("AbastecimentoId", out var abastecimentoIdObj))
                 {
-
                     return BadRequest(new { success = false, message = "AbastecimentoId é obrigatório" });
                 }
 
                 var abastecimentoId = Guid.Parse(abastecimentoIdObj.ToString());
-                var objAbastecimento = _unitOfWork.Abastecimento.GetFirstOrDefault(a => a.AbastecimentoId == abastecimentoId);
+
+                var objAbastecimento = _unitOfWork.Abastecimento.GetFirstOrDefault(a =>
+                    a.AbastecimentoId == abastecimentoId
+                );
 
                 if (objAbastecimento == null)
                 {
-
                     return NotFound(new { success = false, message = "Abastecimento não encontrado" });
                 }
 
@@ -528,7 +719,6 @@
                 {
                     if (int.TryParse(kmRodadoObj.ToString(), out var kmRodado))
                     {
-
                         objAbastecimento.KmRodado = kmRodado;
                     }
                 }
@@ -536,116 +726,144 @@
                 _unitOfWork.Abastecimento.Update(objAbastecimento);
                 _unitOfWork.Save();
 
-                _log.Info($"KM de abastecimento {abastecimentoId} atualizado para {objAbastecimento.KmRodado}", "AbastecimentoController.cs", "AtualizaQuilometragem");
-
                 return Ok(new { success = true, message = "Quilometragem atualizada com sucesso" });
             }
             catch (Exception error)
             {
-
-                _log.Error("Erro ao atualizar quilometragem", error, "AbastecimentoController.cs", "AtualizaQuilometragem");
-
                 Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AtualizaQuilometragem", error);
-
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
 
-        [HttpPost("EditaKm")]
+        [Route("EditaKm")]
+        [HttpPost]
         public IActionResult EditaKm([FromBody] Dictionary<string, object> payload)
         {
 
             return AtualizaQuilometragem(payload);
         }
 
-        [HttpGet("ListaRegistroCupons")]
+        [Route("ListaRegistroCupons")]
+        [HttpGet]
         public IActionResult ListaRegistroCupons(string IDapi)
         {
             try
             {
+
                 var result = (
                     from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                     orderby rc.DataRegistro descending
                     select new
                     {
-                        DataRegistro = rc.DataRegistro != null ? rc.DataRegistro.Value.ToShortDateString() : "",
-                        rc.RegistroCupomId,
+                        DataRegistro = rc.DataRegistro?.ToShortDateString() ,
+                        rc.RegistroCupomId ,
                     }
                 ).ToList();
 
-                return Ok(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro ao carregar lista de registro de cupons", error, "AbastecimentoController.cs", "ListaRegistroCupons");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ListaRegistroCupons", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("PegaRegistroCupons")]
+                return Ok(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "ListaRegistroCupons" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("PegaRegistroCupons")]
+        [HttpGet]
         public IActionResult PegaRegistroCupons(string IDapi)
         {
             try
             {
-                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc => rc.RegistroCupomId == Guid.Parse(IDapi));
-                return Ok(new { RegistroPDF = objRegistro.RegistroPDF });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao recuperar PDF do cupom {IDapi}", error, "AbastecimentoController.cs", "PegaRegistroCupons");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "PegaRegistroCupons", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("PegaRegistroCuponsData")]
+
+                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc =>
+                    rc.RegistroCupomId == Guid.Parse(IDapi)
+                );
+
+                return Ok(new
+                {
+                    RegistroPDF = objRegistro.RegistroPDF
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "PegaRegistroCupons" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("PegaRegistroCuponsData")]
+        [HttpGet]
         public IActionResult PegaRegistroCuponsData(string id)
         {
             try
             {
+
                 var result = (
                     from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                     where rc.DataRegistro == DateTime.Parse(id)
                     orderby rc.DataRegistro descending
                     select new
                     {
-                        DataRegistro = rc.DataRegistro != null ? rc.DataRegistro.Value.ToShortDateString() : "",
-                        rc.RegistroCupomId,
+                        DataRegistro = rc.DataRegistro?.ToShortDateString() ,
+                        rc.RegistroCupomId ,
                     }
                 ).ToList();
 
-                return Ok(new { data = result });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao filtrar registros de cupons por data {id}", error, "AbastecimentoController.cs", "PegaRegistroCuponsData");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "PegaRegistroCuponsData", error);
-                return StatusCode(500);
-            }
-        }
-
-        [HttpGet("DeleteRegistro")]
+                return Ok(new
+                {
+                    data = result
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "PegaRegistroCuponsData" ,
+                    error
+                );
+                return StatusCode(500);
+            }
+        }
+
+        [Route("DeleteRegistro")]
+        [HttpGet]
         public IActionResult DeleteRegistro(string IDapi)
         {
             try
             {
-                var guid = Guid.Parse(IDapi);
-                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc => rc.RegistroCupomId == guid);
-
-                if (objRegistro != null)
-                {
-                    _unitOfWork.RegistroCupomAbastecimento.Remove(objRegistro);
-                    _unitOfWork.Save();
-                    _log.Warning($"Registro de cupom {IDapi} excluído permanentemente", "AbastecimentoController.cs", "DeleteRegistro");
-                }
-
-                return Ok(new { success = true, message = "Registro excluído com sucesso!" });
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro ao excluir registro de cupom {IDapi}", error, "AbastecimentoController.cs", "DeleteRegistro");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DeleteRegistro", error);
+
+                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc =>
+                    rc.RegistroCupomId == Guid.Parse(IDapi)
+                );
+
+                _unitOfWork.RegistroCupomAbastecimento.Remove(objRegistro);
+                _unitOfWork.Save();
+
+                return Ok(new
+                {
+                    success = true ,
+                    message = "Registro excluído com sucesso!"
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AbastecimentoController.cs" ,
+                    "DeleteRegistro" ,
+                    error
+                );
                 return StatusCode(500);
             }
         }
```
