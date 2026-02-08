# Controllers/OcorrenciaController.cs

**Mudanca:** GRANDE | **+44** linhas | **-58** linhas

---

```diff
--- JANEIRO: Controllers/OcorrenciaController.cs
+++ ATUAL: Controllers/OcorrenciaController.cs
@@ -18,23 +18,21 @@
     [Route("api/[controller]")]
     [ApiController]
     [IgnoreAntiforgeryToken]
-    public class OcorrenciaController : Controller
+    public class OcorrenciaController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-        private readonly IWebHostEnvironment _hostingEnv;
-
-        public OcorrenciaController(IUnitOfWork unitOfWork, IWebHostEnvironment env, ILogService log)
+        private IWebHostEnvironment hostingEnv;
+
+        public OcorrenciaController(IUnitOfWork unitOfWork , IWebHostEnvironment env)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _hostingEnv = env;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs", "OcorrenciaController", ex);
+                hostingEnv = env;
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciaController" , error);
             }
         }
 
@@ -51,7 +49,6 @@
         {
             try
             {
-
                 Guid? veiculoGuid = null, motoristaGuid = null;
                 if (!string.IsNullOrWhiteSpace(veiculoId) && Guid.TryParse(veiculoId , out var vg))
                     veiculoGuid = vg;
@@ -131,13 +128,11 @@
 
                 if (dataUnica.HasValue)
                 {
-
                     var dia = dataUnica.Value.Date;
                     q = q.Where(v => v.DataFinal.HasValue && v.DataFinal.Value.Date == dia);
                 }
                 else if (dtIni.HasValue && dtFim.HasValue)
                 {
-
                     var ini = dtIni.Value.Date;
                     var fim = dtFim.Value.Date;
                     q = q.Where(v =>
@@ -186,7 +181,6 @@
 
                 if (debug == "1")
                 {
-
                     var echo = new
                     {
                         recebido = new
@@ -215,10 +209,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.Get", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Get" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Get" , error);
                 return Json(new
                 {
                     data = new List<object>()
@@ -262,10 +255,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.Ocorrencias", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Ocorrencias" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "Ocorrencias" , error);
                 return Json(new
                 {
                     data = new List<object>()
@@ -311,10 +303,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.OcorrenciasVeiculos", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasVeiculos" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasVeiculos" , error);
                 return Json(new
                 {
                     data = new List<object>()
@@ -358,10 +349,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.OcorrenciasMotoristas", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasMotoristas" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasMotoristas" , error);
                 return Json(new
                 {
                     data = new List<object>()
@@ -437,10 +427,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.OcorrenciasStatus", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasStatus" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasStatus" , error);
                 return Json(new
                 {
                     data = new List<object>()
@@ -494,10 +483,9 @@
                     message = "Data inválida fornecida."
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.OcorrenciasData", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasData" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "OcorrenciasData" , error);
                 return Json(new
                 {
                     data = new List<object>()
@@ -515,13 +503,12 @@
                 return Json(new
                 {
                     success = false ,
-                    message = "Erro ao baixar ocorrência: Funcionalidade não ativada."
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.BaixarOcorrencia", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "BaixarOcorrencia" , ex);
+                    message = "Erro ao baixar ocorrência"
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "BaixarOcorrencia" , error);
                 return Json(new
                 {
                     success = false ,
@@ -531,6 +518,7 @@
         }
 
         [Route("SaveImage")]
+
         public void SaveImage(IList<IFormFile> UploadFiles)
         {
             try
@@ -543,18 +531,18 @@
                             .Parse(file.ContentDisposition)
                             .FileName.Trim('"');
                         filename =
-                            _hostingEnv.WebRootPath
+                            hostingEnv.WebRootPath
                             + "\\DadosEditaveis\\ImagensViagens"
                             + $@"\{filename}";
 
                         if (
                             !Directory.Exists(
-                                _hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
+                                hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
                             )
                         )
                         {
                             Directory.CreateDirectory(
-                                _hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
+                                hostingEnv.WebRootPath + "\\DadosEditaveis\\ImagensViagens"
                             );
                         }
 
@@ -565,16 +553,14 @@
                                 file.CopyTo(fs);
                                 fs.Flush();
                             }
-                            _log.Info($"OcorrenciaController.SaveImage: Arquivo {filename} salvo com sucesso.");
                             Response.StatusCode = 200;
                         }
                     }
                 }
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.SaveImage", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "SaveImage" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "SaveImage" , error);
                 Response.StatusCode = 204;
             }
         }
@@ -585,6 +571,7 @@
         {
             try
             {
+
                 return Json(
                     new
                     {
@@ -594,10 +581,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.EditaOcorrencia", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "EditaOcorrencia" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "EditaOcorrencia" , error);
                 return Json(new
                 {
                     success = false ,
@@ -612,7 +598,6 @@
         {
             try
             {
-                _log.Info($"OcorrenciaController.FechaItemOS: Tentativa de baixa de ItemManutencao {itensMmanutencao.ItemManutencaoId} via OS {itensMmanutencao.ManutencaoId}");
 
                 return new JsonResult(
                     new
@@ -622,10 +607,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("OcorrenciaController.FechaItemOS", ex );
-                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "FechaItemOS" , ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("OcorrenciaController.cs" , "FechaItemOS" , error);
                 return new JsonResult(new
                 {
                     success = false ,
```
