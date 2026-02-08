# Controllers/MultaPdfViewerController.cs

**Mudanca:** MEDIA | **+5** linhas | **-25** linhas

---

```diff
--- JANEIRO: Controllers/MultaPdfViewerController.cs
+++ ATUAL: Controllers/MultaPdfViewerController.cs
@@ -1,4 +1,3 @@
-using FrotiX.Services;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.Extensions.Caching.Memory;
@@ -10,29 +9,24 @@
 
 namespace FrotiX.Controllers
 {
-
     [ApiController]
     [Route("api/[controller]")]
-    public class MultaPdfViewerController : Controller
+    public class MultaPdfViewerController :Controller
     {
         private readonly IWebHostEnvironment _env;
         private readonly IMemoryCache _cache;
-        private readonly ILogService _logService;
 
         public MultaPdfViewerController(
             IWebHostEnvironment env ,
-            IMemoryCache cache ,
-            ILogService logService)
+            IMemoryCache cache)
         {
             try
             {
                 _env = env;
                 _cache = cache;
-                _logService = logService;
-            }
-            catch (Exception error)
-            {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "MultaPdfViewerController (Construtor)");
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "MultaPdfViewerController" , error);
             }
         }
@@ -41,13 +35,11 @@
         {
             try
             {
-
                 var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath , "wwwroot");
                 return Path.Combine(root , "DadosEditaveis" , "Multas");
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ResolveFolder");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ResolveFolder" , error);
                 return string.Empty;
             }
@@ -57,7 +49,6 @@
         {
             try
             {
-
                 if (json == null)
                     return new MemoryStream();
 
@@ -70,7 +61,6 @@
 
                 if (isFileName)
                 {
-
                     var folder = ResolveFolder();
                     var path = Path.Combine(folder , Path.GetFileName(document));
 
@@ -87,7 +77,6 @@
                 }
                 else
                 {
-
                     byte[] bytes;
                     try
                     {
@@ -102,7 +91,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ResolveDocumentStream");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ResolveDocumentStream" , error);
                 return new MemoryStream();
             }
@@ -113,7 +101,6 @@
         {
             try
             {
-
                 var viewer = new PdfRenderer(_cache);
 
                 var stream = ResolveDocumentStream(json);
@@ -125,7 +112,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Load");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Load" , error);
                 return StatusCode(500 , new
                 {
@@ -146,7 +132,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderPdfPages");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderPdfPages" , error);
                 return StatusCode(500 , new
                 {
@@ -167,7 +152,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderPdfTexts");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderPdfTexts" , error);
                 return StatusCode(500 , new
                 {
@@ -188,7 +172,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderThumbnailImages");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderThumbnailImages" , error);
                 return StatusCode(500 , new
                 {
@@ -209,7 +192,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Bookmarks");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Bookmarks" , error);
                 return StatusCode(500 , new
                 {
@@ -230,7 +212,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderAnnotationComments");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderAnnotationComments" , error);
                 return StatusCode(500 , new
                 {
@@ -250,7 +231,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Unload");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Unload" , error);
                 return StatusCode(500 , new
                 {
@@ -270,7 +250,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ExportAnnotations");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ExportAnnotations" , error);
                 return StatusCode(500 , new
                 {
@@ -303,7 +282,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ImportAnnotations");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ImportAnnotations" , error);
                 return StatusCode(500 , new
                 {
@@ -323,7 +301,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ExportFormFields");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ExportFormFields" , error);
                 return StatusCode(500 , new
                 {
@@ -348,7 +325,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ImportFormFields");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ImportFormFields" , error);
                 return StatusCode(500 , new
                 {
@@ -368,7 +344,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Download");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Download" , error);
                 return StatusCode(500 , new
                 {
@@ -389,7 +364,6 @@
             }
             catch (Exception error)
             {
-                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "PrintImages");
                 Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "PrintImages" , error);
                 return StatusCode(500 , new
                 {
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class MultaPdfViewerController : Controller
        private readonly ILogService _logService;
            IMemoryCache cache ,
            ILogService logService)
                _logService = logService;
            }
            catch (Exception error)
            {
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "MultaPdfViewerController (Construtor)");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ResolveFolder");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ResolveDocumentStream");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Load");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderPdfPages");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderPdfTexts");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderThumbnailImages");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Bookmarks");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderAnnotationComments");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Unload");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ExportAnnotations");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ImportAnnotations");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ExportFormFields");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ImportFormFields");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Download");
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "PrintImages");
```


### ADICIONAR ao Janeiro

```csharp
    public class MultaPdfViewerController :Controller
            IMemoryCache cache)
            }
            catch (Exception error)
            {
```
