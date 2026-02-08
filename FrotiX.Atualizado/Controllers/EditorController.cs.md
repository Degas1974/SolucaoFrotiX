# Controllers/EditorController.cs

**Mudanca:** MEDIA | **+4** linhas | **-18** linhas

---

```diff
--- JANEIRO: Controllers/EditorController.cs
+++ ATUAL: Controllers/EditorController.cs
@@ -1,30 +1,16 @@
+using FrotiX.Helpers;
+
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.IO;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("Editor")]
-    public class EditorController : Controller
+    public class EditorController :Controller
     {
-        private readonly ILogService _logService;
-
-        public EditorController(ILogService logService)
-        {
-            try
-            {
-                _logService = logService;
-            }
-            catch (Exception ex)
-            {
-
-                Console.WriteLine($"Erro crítico no construtor do EditorController: {ex.Message}");
-            }
-        }
 
         [HttpPost("DownloadImagemDocx")]
         public IActionResult DownloadImagemDocx(IFormFile docx)
@@ -38,13 +24,14 @@
                 var bytes = memory.ToArray();
 
                 var imagem = SfdtHelper.SalvarImagemDeDocx(bytes);
-                System.IO.File.WriteAllBytes("wwwroot/uploads/Editor.png", imagem);
+
+                System.IO.File.WriteAllBytes("wwwroot/uploads/Editor.png" , imagem);
+
                 return Ok();
             }
             catch (Exception error)
             {
-                _logService?.Error(error.Message, error, "EditorController.cs", "DownloadImagemDocx");
-                Alerta.TratamentoErroComLinha("EditorController.cs", "DownloadImagemDocx", error);
+                Alerta.TratamentoErroComLinha("EditorController.cs" , "DownloadImagemDocx" , error);
                 return StatusCode(500);
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
    public class EditorController : Controller
        private readonly ILogService _logService;
        public EditorController(ILogService logService)
        {
            try
            {
                _logService = logService;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro crítico no construtor do EditorController: {ex.Message}");
            }
        }
                System.IO.File.WriteAllBytes("wwwroot/uploads/Editor.png", imagem);
                _logService?.Error(error.Message, error, "EditorController.cs", "DownloadImagemDocx");
                Alerta.TratamentoErroComLinha("EditorController.cs", "DownloadImagemDocx", error);
```


### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
    public class EditorController :Controller
                System.IO.File.WriteAllBytes("wwwroot/uploads/Editor.png" , imagem);
                Alerta.TratamentoErroComLinha("EditorController.cs" , "DownloadImagemDocx" , error);
```
