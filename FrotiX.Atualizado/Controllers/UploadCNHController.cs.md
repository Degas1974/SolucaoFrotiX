# Controllers/UploadCNHController.cs

**Mudanca:** MEDIA | **+6** linhas | **-14** linhas

---

```diff
--- JANEIRO: Controllers/UploadCNHController.cs
+++ ATUAL: Controllers/UploadCNHController.cs
@@ -1,5 +1,4 @@
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Http.Features;
@@ -14,68 +13,57 @@
     [Route("api/[controller]")]
     [ApiController]
     [IgnoreAntiforgeryToken]
-    public partial class UploadCNHController : Controller
+    public partial class UploadCNHController :Controller
     {
-        private readonly IWebHostEnvironment hostingEnv;
+        private IWebHostEnvironment hostingEnv;
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public UploadCNHController(IWebHostEnvironment env, IUnitOfWork unitOfWork, ILogService log)
+        public UploadCNHController(IWebHostEnvironment env , IUnitOfWork unitOfWork)
         {
             try
             {
                 this.hostingEnv = env;
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("UploadCNHController.cs", "UploadCNHController", error);
+                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "UploadCNHController" , error);
             }
         }
 
         [AcceptVerbs("Post")]
         [HttpPost]
         [Route("Save")]
-        public IActionResult Save(IList<IFormFile> UploadFiles, [FromQuery] Guid motoristaId)
+        public IActionResult Save(IList<IFormFile> UploadFiles , [FromQuery] Guid motoristaId)
         {
             try
             {
-
                 if (UploadFiles != null && motoristaId != Guid.Empty)
                 {
                     foreach (var file in UploadFiles)
                     {
-
                         var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                             u.MotoristaId == motoristaId
                         );
 
                         if (objFromDb != null)
                         {
-
                             using (var target = new MemoryStream())
                             {
                                 file.CopyTo(target);
                                 objFromDb.CNHDigital = target.ToArray();
                             }
-
                             _unitOfWork.Motorista.Update(objFromDb);
                             _unitOfWork.Save();
-
-                            _log.Info($"Upload de CNH realizado com sucesso para o Motorista ID: {motoristaId} (Arquivo: {file.FileName})", "UploadCNHController", "Save");
                         }
                     }
                 }
-
                 return Content("");
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UploadCNHController", "Save");
                 Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "Save" , error);
                 Response.StatusCode = 500;
-
                 return Content("");
             }
         }
@@ -83,39 +71,31 @@
         [AcceptVerbs("Post")]
         [HttpPost]
         [Route("Remove")]
-        public IActionResult Remove(IList<IFormFile> UploadFiles, [FromQuery] Guid motoristaId)
+        public IActionResult Remove(IList<IFormFile> UploadFiles , [FromQuery] Guid motoristaId)
         {
             try
             {
-
                 if (motoristaId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                         u.MotoristaId == motoristaId
                     );
 
                     if (objFromDb != null)
                     {
-
                         objFromDb.CNHDigital = null;
                         _unitOfWork.Motorista.Update(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Info($"Arquivo de CNH removido com sucesso para o Motorista ID: {motoristaId}", "UploadCNHController", "Remove");
                     }
                 }
-
                 return Content("");
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UploadCNHController", "Remove");
                 Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "Remove" , error);
                 Response.Clear();
                 Response.StatusCode = 500;
                 Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = error.Message;
-
                 return Content("");
             }
         }
@@ -127,14 +107,11 @@
         {
             try
             {
-
                 return View();
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UploadCNHController", "UploadFeatures");
                 Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "UploadFeatures" , error);
-
                 return View();
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public partial class UploadCNHController : Controller
        private readonly IWebHostEnvironment hostingEnv;
        private readonly ILogService _log;
        public UploadCNHController(IWebHostEnvironment env, IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("UploadCNHController.cs", "UploadCNHController", error);
        public IActionResult Save(IList<IFormFile> UploadFiles, [FromQuery] Guid motoristaId)
                            _log.Info($"Upload de CNH realizado com sucesso para o Motorista ID: {motoristaId} (Arquivo: {file.FileName})", "UploadCNHController", "Save");
                _log.Error("Erro", error, "UploadCNHController", "Save");
        public IActionResult Remove(IList<IFormFile> UploadFiles, [FromQuery] Guid motoristaId)
                        _log.Info($"Arquivo de CNH removido com sucesso para o Motorista ID: {motoristaId}", "UploadCNHController", "Remove");
                _log.Error("Erro", error, "UploadCNHController", "Remove");
                _log.Error("Erro", error, "UploadCNHController", "UploadFeatures");
```


### ADICIONAR ao Janeiro

```csharp
    public partial class UploadCNHController :Controller
        private IWebHostEnvironment hostingEnv;
        public UploadCNHController(IWebHostEnvironment env , IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "UploadCNHController" , error);
        public IActionResult Save(IList<IFormFile> UploadFiles , [FromQuery] Guid motoristaId)
        public IActionResult Remove(IList<IFormFile> UploadFiles , [FromQuery] Guid motoristaId)
```
