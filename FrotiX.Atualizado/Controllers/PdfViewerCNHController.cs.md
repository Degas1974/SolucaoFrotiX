# Controllers/PdfViewerCNHController.cs

**Mudanca:** MEDIA | **+2** linhas | **-24** linhas

---

```diff
--- JANEIRO: Controllers/PdfViewerCNHController.cs
+++ ATUAL: Controllers/PdfViewerCNHController.cs
@@ -7,8 +7,6 @@
 using System;
 using System.Collections.Generic;
 using System.IO;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -16,18 +14,16 @@
     [Route("api/[controller]")]
     [ApiController]
     [IgnoreAntiforgeryToken]
-    public class PdfViewerCNHController : Controller
+    public class PdfViewerCNHController :Controller
     {
         private readonly IMemoryCache _cache;
         private readonly IWebHostEnvironment _hostingEnvironment;
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
         public PdfViewerCNHController(
             IWebHostEnvironment hostingEnvironment ,
             IMemoryCache cache ,
-            IUnitOfWork unitOfWork ,
-            ILogService logService
+            IUnitOfWork unitOfWork
         )
         {
             try
@@ -35,7 +31,6 @@
                 _hostingEnvironment = hostingEnvironment;
                 _cache = cache;
                 _unitOfWork = unitOfWork;
-                _log = logService;
             }
             catch (Exception error)
             {
@@ -47,12 +42,10 @@
         {
             try
             {
-
                 return View();
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.PdfViewerFeatures", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PdfViewerFeatures" , error);
                 return View();
             }
@@ -60,11 +53,11 @@
 
         [HttpPost]
         [Route("Load")]
+
         public IActionResult Load([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer(_cache);
                 MemoryStream stream = new MemoryStream();
                 object jsonResult = new object();
@@ -73,7 +66,6 @@
                 {
                     if (bool.Parse(jsonObject["isFileName"]))
                     {
-
                         string documentPath = GetDocumentPath(jsonObject["document"]);
                         if (!string.IsNullOrEmpty(documentPath))
                         {
@@ -87,19 +79,16 @@
                     }
                     else
                     {
-
                         byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                         stream = new MemoryStream(bytes);
                     }
                 }
 
                 jsonResult = pdfviewer.Load(stream , jsonObject);
-                _log.Info("PdfViewerCNHController.Load: Documento carregado no visualizador.");
                 return Content(JsonConvert.SerializeObject(jsonResult));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.Load", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Load" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -110,18 +99,17 @@
 
         [HttpPost]
         [Route("RenderPdfPages")]
+
         public IActionResult RenderPdfPages([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer(_cache);
                 object jsonResult = pdfviewer.GetPage(jsonObject);
                 return Content(JsonConvert.SerializeObject(jsonResult));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.RenderPdfPages", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderPdfPages" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -132,18 +120,17 @@
 
         [HttpPost]
         [Route("RenderAnnotationComments")]
+
         public IActionResult RenderAnnotationComments([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer(_cache);
                 object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
                 return Content(JsonConvert.SerializeObject(jsonResult));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.RenderAnnotationComments", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderAnnotationComments" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -154,18 +141,17 @@
 
         [HttpPost]
         [Route("Unload")]
+
         public IActionResult Unload([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer(_cache);
                 pdfviewer.ClearCache(jsonObject);
                 return Content("Document cache is cleared");
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.Unload", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Unload" , error);
                 return Content("Erro ao limpar cache");
             }
@@ -173,18 +159,17 @@
 
         [HttpPost]
         [Route("RenderThumbnailImages")]
+
         public IActionResult RenderThumbnailImages([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
-
                 PdfRenderer pdfviewer = new PdfRenderer(_cache);
                 object result = pdfviewer.GetThumbnailImages(jsonObject);
                 return Content(JsonConvert.SerializeObject(result));
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.RenderThumbnailImages", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderThumbnailImages" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -195,6 +180,7 @@
 
         [HttpPost]
         [Route("Bookmarks")]
+
         public IActionResult Bookmarks([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -205,7 +191,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.Bookmarks", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Bookmarks" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -216,18 +201,17 @@
 
         [HttpPost]
         [Route("Download")]
+
         public IActionResult Download([FromBody] Dictionary<string , string> jsonObject)
         {
             try
             {
                 PdfRenderer pdfviewer = new PdfRenderer(_cache);
                 string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
-                _log.Info("PdfViewerCNHController.Download: Download de PDF (CNH) realizado.");
                 return Content(documentBase);
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.Download", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Download" , error);
                 return Content(string.Empty);
             }
@@ -235,6 +219,7 @@
 
         [HttpPost]
         [Route("PrintImages")]
+
         public IActionResult PrintImages([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -245,7 +230,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.PrintImages", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PrintImages" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -256,6 +240,7 @@
 
         [HttpPost]
         [Route("ExportAnnotations")]
+
         public IActionResult ExportAnnotations([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -266,7 +251,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.ExportAnnotations", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ExportAnnotations" , error);
                 return Content(string.Empty);
             }
@@ -274,6 +258,7 @@
 
         [HttpPost]
         [Route("ImportAnnotations")]
+
         public IActionResult ImportAnnotations([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -298,7 +283,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.ImportAnnotations", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ImportAnnotations" , error);
                 return Content(string.Empty);
             }
@@ -306,6 +290,7 @@
 
         [HttpPost]
         [Route("ExportFormFields")]
+
         public IActionResult ExportFormFields([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -316,7 +301,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.ExportFormFields", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ExportFormFields" , error);
                 return Content(string.Empty);
             }
@@ -324,6 +308,7 @@
 
         [HttpPost]
         [Route("ImportFormFields")]
+
         public IActionResult ImportFormFields([FromBody] Dictionary<string , string> jsonObject)
         {
             try
@@ -334,7 +319,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.ImportFormFields", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ImportFormFields" , error);
                 return Content(JsonConvert.SerializeObject(new
                 {
@@ -364,7 +348,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.GetDocumentPath", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "GetDocumentPath" , error);
                 return string.Empty;
             }
@@ -372,6 +355,7 @@
 
         [HttpPost]
         [Route("GetDocument")]
+
         public string GetDocument(Guid id)
         {
             try
@@ -390,7 +374,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("PdfViewerCNHController.GetDocument", error);
                 Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "GetDocument" , error);
                 return string.Empty;
             }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
    public class PdfViewerCNHController : Controller
        private readonly ILogService _log;
            IUnitOfWork unitOfWork ,
            ILogService logService
                _log = logService;
                _log.Error("PdfViewerCNHController.PdfViewerFeatures", error);
                _log.Info("PdfViewerCNHController.Load: Documento carregado no visualizador.");
                _log.Error("PdfViewerCNHController.Load", error);
                _log.Error("PdfViewerCNHController.RenderPdfPages", error);
                _log.Error("PdfViewerCNHController.RenderAnnotationComments", error);
                _log.Error("PdfViewerCNHController.Unload", error);
                _log.Error("PdfViewerCNHController.RenderThumbnailImages", error);
                _log.Error("PdfViewerCNHController.Bookmarks", error);
                _log.Info("PdfViewerCNHController.Download: Download de PDF (CNH) realizado.");
                _log.Error("PdfViewerCNHController.Download", error);
                _log.Error("PdfViewerCNHController.PrintImages", error);
                _log.Error("PdfViewerCNHController.ExportAnnotations", error);
                _log.Error("PdfViewerCNHController.ImportAnnotations", error);
                _log.Error("PdfViewerCNHController.ExportFormFields", error);
                _log.Error("PdfViewerCNHController.ImportFormFields", error);
                _log.Error("PdfViewerCNHController.GetDocumentPath", error);
                _log.Error("PdfViewerCNHController.GetDocument", error);
```


### ADICIONAR ao Janeiro

```csharp
    public class PdfViewerCNHController :Controller
            IUnitOfWork unitOfWork
```
