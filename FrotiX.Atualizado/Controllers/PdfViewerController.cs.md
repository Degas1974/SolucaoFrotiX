# Controllers/PdfViewerController.cs

**Mudanca:** GRANDE | **+10** linhas | **-27** linhas

---

```diff
--- JANEIRO: Controllers/PdfViewerController.cs
+++ ATUAL: Controllers/PdfViewerController.cs
@@ -5,38 +5,35 @@
 using System;
 using System.Collections.Generic;
 using System.IO;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers.API
 {
 
     [Route("api/[controller]")]
     [ApiController]
-    public class PdfViewerController : ControllerBase
+    public class PdfViewerController :ControllerBase
     {
         private readonly IWebHostEnvironment _hostingEnvironment;
-        private readonly ILogService _log;
-
-        public PdfViewerController(IWebHostEnvironment hostingEnvironment, ILogService log)
+
+        public PdfViewerController(IWebHostEnvironment hostingEnvironment)
         {
             try
             {
                 _hostingEnvironment = hostingEnvironment;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("PdfViewerController.cs", "PdfViewerController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "PdfViewerController" , error);
             }
         }
 
         [HttpPost("Load")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult Load([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer();
                 MemoryStream stream = new MemoryStream();
                 object jsonResult = new object();
@@ -45,7 +42,6 @@
                 {
                     if (bool.TryParse(jsonObject["isFileName"] , out bool isFileName) && isFileName)
                     {
-
                         string documentPath = jsonObject["document"].TrimStart('/');
                         string fullPath = Path.Combine(_hostingEnvironment.WebRootPath , documentPath);
 
@@ -56,8 +52,6 @@
                         }
                         else
                         {
-
-                            _log.Warning($"PdfViewerController.Load: Arquivo não encontrado em {fullPath}");
                             return Content(JsonConvert.SerializeObject(new
                             {
                                 error = "Arquivo não encontrado: " + fullPath
@@ -66,19 +60,16 @@
                     }
                     else
                     {
-
                         byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                         stream = new MemoryStream(bytes);
                     }
                 }
 
                 jsonResult = pdfviewer.Load(stream , jsonObject);
-                _log.Info("PdfViewerController.Load: PDF carregado com sucesso (Genérico).");
-                return Content(JsonConvert.SerializeObject(jsonResult));
-            }
-            catch (Exception error)
-            {
-                _log.Error("PdfViewerController.Load", error);
+                return Content(JsonConvert.SerializeObject(jsonResult));
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Load" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -89,18 +80,17 @@
 
         [HttpPost("Bookmarks")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult Bookmarks([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer();
                 object jsonResult = pdfviewer.GetBookmarks(jsonObject);
                 return Content(JsonConvert.SerializeObject(jsonResult));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.Bookmarks", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Bookmarks" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -111,18 +101,17 @@
 
         [HttpPost("RenderPdfPages")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult RenderPdfPages([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer();
                 object jsonResult = pdfviewer.GetPage(jsonObject);
                 return Content(JsonConvert.SerializeObject(jsonResult));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.RenderPdfPages", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderPdfPages" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -133,18 +122,17 @@
 
         [HttpPost("RenderPdfTexts")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult RenderPdfTexts([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer();
                 object jsonResult = pdfviewer.GetDocumentText(jsonObject);
                 return Content(JsonConvert.SerializeObject(jsonResult));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.RenderPdfTexts", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderPdfTexts" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -155,18 +143,17 @@
 
         [HttpPost("RenderThumbnailImages")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult RenderThumbnailImages([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer();
                 object result = pdfviewer.GetThumbnailImages(jsonObject);
                 return Content(JsonConvert.SerializeObject(result));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.RenderThumbnailImages", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderThumbnailImages" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -177,6 +164,7 @@
 
         [HttpPost("RenderAnnotationComments")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult RenderAnnotationComments([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -197,6 +185,7 @@
 
         [HttpPost("ExportAnnotations")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult ExportAnnotations([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -217,6 +206,7 @@
 
         [HttpPost("ImportAnnotations")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult ImportAnnotations([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -227,7 +217,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.ImportAnnotations", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportAnnotations" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -238,6 +227,7 @@
 
         [HttpPost("ExportFormFields")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult ExportFormFields([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -248,7 +238,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.ExportFormFields", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ExportFormFields" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -259,6 +248,7 @@
 
         [HttpPost("ImportFormFields")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult ImportFormFields([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -269,7 +259,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.ImportFormFields", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportFormFields" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -280,6 +269,7 @@
 
         [HttpPost("Unload")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult Unload([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -290,7 +280,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.Unload", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Unload" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -301,18 +290,17 @@
 
         [HttpPost("Download")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult Download([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
                 PdfRenderer pdfviewer = new PdfRenderer();
                 string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
-                _log.Info("PdfViewerController.Download: Documento exportado em Base64 para download.");
                 return Content(documentBase);
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.Download", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Download" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -323,6 +311,7 @@
 
         [HttpPost("PrintImages")]
         [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
+
         public IActionResult PrintImages([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -333,7 +322,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerController.PrintImages", error);
                 Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "PrintImages" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class PdfViewerController : ControllerBase
        private readonly ILogService _log;
        public PdfViewerController(IWebHostEnvironment hostingEnvironment, ILogService log)
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs", "PdfViewerController", error);
                            _log.Warning($"PdfViewerController.Load: Arquivo não encontrado em {fullPath}");
                _log.Info("PdfViewerController.Load: PDF carregado com sucesso (Genérico).");
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.Load", error);
                _log.Error("PdfViewerController.Bookmarks", error);
                _log.Error("PdfViewerController.RenderPdfPages", error);
                _log.Error("PdfViewerController.RenderPdfTexts", error);
                _log.Error("PdfViewerController.RenderThumbnailImages", error);
                _log.Error("PdfViewerController.ImportAnnotations", error);
                _log.Error("PdfViewerController.ExportFormFields", error);
                _log.Error("PdfViewerController.ImportFormFields", error);
                _log.Error("PdfViewerController.Unload", error);
                _log.Info("PdfViewerController.Download: Documento exportado em Base64 para download.");
                _log.Error("PdfViewerController.Download", error);
                _log.Error("PdfViewerController.PrintImages", error);
```


### ADICIONAR ao Janeiro

```csharp
    public class PdfViewerController :ControllerBase
        public PdfViewerController(IWebHostEnvironment hostingEnvironment)
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "PdfViewerController" , error);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
```
