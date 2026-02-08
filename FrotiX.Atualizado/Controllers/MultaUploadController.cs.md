# Controllers/MultaUploadController.cs

**Mudanca:** MEDIA | **+5** linhas | **-16** linhas

---

```diff
--- JANEIRO: Controllers/MultaUploadController.cs
+++ ATUAL: Controllers/MultaUploadController.cs
@@ -9,24 +9,20 @@
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
-    public class MultaUploadController : ControllerBase
+    public class MultaUploadController :ControllerBase
     {
         private readonly IWebHostEnvironment _hostingEnvironment;
-        private readonly ILogService _log;
-
-        public MultaUploadController(IWebHostEnvironment hostingEnvironment , ILogService logService)
+
+        public MultaUploadController(IWebHostEnvironment hostingEnvironment)
         {
             try
             {
                 _hostingEnvironment = hostingEnvironment;
-                _log = logService;
-            }
-            catch (Exception error)
-            {
-                _log.Error("MultaUploadController.Constructor", error );
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "MultaUploadController" , error);
             }
         }
@@ -36,7 +32,6 @@
         {
             try
             {
-
                 if (UploadFiles == null || UploadFiles.Count == 0)
                 {
                     return Ok(new
@@ -54,7 +49,6 @@
 
                 if (!Directory.Exists(pastaMultas))
                 {
-
                     Directory.CreateDirectory(pastaMultas);
                 }
 
@@ -105,8 +99,6 @@
                     }
                     catch (Exception fileError)
                     {
-
-                        _log.Error("MultaUploadController.Save.ForEach", fileError );
                         Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Save.ForEach" , fileError);
                         uploadedFiles.Add(new
                         {
@@ -126,7 +118,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("MultaUploadController.Save", error );
                 Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Save" , error);
                 return Ok(new
                 {
@@ -144,7 +135,6 @@
         {
             try
             {
-
                 if (UploadFiles == null || UploadFiles.Count == 0)
                 {
 
@@ -171,12 +161,10 @@
                 {
                     try
                     {
-
                         string caminhoCompleto = Path.Combine(pastaMultas , file.FileName);
 
                         if (System.IO.File.Exists(caminhoCompleto))
                         {
-
                             System.IO.File.Delete(caminhoCompleto);
 
                             removedFiles.Add(new
@@ -188,7 +176,6 @@
                         }
                         else
                         {
-
                             removedFiles.Add(new
                             {
                                 name = file.FileName ,
@@ -200,8 +187,6 @@
                     }
                     catch (Exception fileError)
                     {
-
-                        _log.Error("MultaUploadController.Remove.ForEach", fileError );
                         Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Remove.ForEach" , fileError);
                         removedFiles.Add(new
                         {
@@ -220,7 +205,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("MultaUploadController.Remove", error );
                 Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Remove" , error);
                 return Ok(new
                 {
@@ -237,13 +221,11 @@
         {
             try
             {
-
                 var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                 string caminhoCompleto = Path.Combine(pastaMultas , fileName);
 
                 if (System.IO.File.Exists(caminhoCompleto))
                 {
-
                     System.IO.File.Delete(caminhoCompleto);
 
                     return Ok(new
@@ -265,7 +247,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("MultaUploadController.RemoveByFileName", error );
                 Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "RemoveByFileName" , error);
                 return Ok(new
                 {
@@ -283,12 +264,10 @@
         {
             try
             {
-
                 var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
 
                 if (!Directory.Exists(pastaMultas))
                 {
-
                     return Ok(new
                     {
                         files = new List<object>()
@@ -313,7 +292,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("MultaUploadController.GetFileList", error );
                 Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "GetFileList" , error);
                 return Ok(new
                 {
@@ -331,7 +309,6 @@
         {
             try
             {
-
                 var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                 var tempPath = Path.Combine(pastaMultas , "temp");
 
@@ -356,7 +333,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("MultaUploadController.Chunk", error );
                 Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Chunk" , error);
                 return Ok(new
                 {
@@ -374,7 +350,6 @@
         {
             try
             {
-
                 var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                 var tempPath = Path.Combine(pastaMultas , "temp");
 
@@ -397,7 +372,6 @@
                             {
                                 chunkStream.CopyTo(finalStream);
                             }
-
                             System.IO.File.Delete(chunkPath);
                         }
                     }
@@ -412,7 +386,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("MultaUploadController.MergeChunks", error );
                 Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "MergeChunks" , error);
                 return Ok(new
                 {
```

### REMOVER do Janeiro

```csharp
    public class MultaUploadController : ControllerBase
        private readonly ILogService _log;
        public MultaUploadController(IWebHostEnvironment hostingEnvironment , ILogService logService)
                _log = logService;
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.Constructor", error );
                        _log.Error("MultaUploadController.Save.ForEach", fileError );
                _log.Error("MultaUploadController.Save", error );
                        _log.Error("MultaUploadController.Remove.ForEach", fileError );
                _log.Error("MultaUploadController.Remove", error );
                _log.Error("MultaUploadController.RemoveByFileName", error );
                _log.Error("MultaUploadController.GetFileList", error );
                _log.Error("MultaUploadController.Chunk", error );
                _log.Error("MultaUploadController.MergeChunks", error );
```


### ADICIONAR ao Janeiro

```csharp
    public class MultaUploadController :ControllerBase
        public MultaUploadController(IWebHostEnvironment hostingEnvironment)
            }
            catch (Exception error)
            {
```
