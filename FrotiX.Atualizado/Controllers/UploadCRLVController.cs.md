# Controllers/UploadCRLVController.cs

**Mudanca:** MEDIA | **+6** linhas | **-14** linhas

---

```diff
--- JANEIRO: Controllers/UploadCRLVController.cs
+++ ATUAL: Controllers/UploadCRLVController.cs
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
-    public partial class UploadCRLVController : Controller
+    public partial class UploadCRLVController :Controller
     {
-        private readonly IWebHostEnvironment hostingEnv;
+        private IWebHostEnvironment hostingEnv;
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public UploadCRLVController(IWebHostEnvironment env, IUnitOfWork unitOfWork, ILogService log)
+        public UploadCRLVController(IWebHostEnvironment env , IUnitOfWork unitOfWork)
         {
             try
             {
                 this.hostingEnv = env;
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("UploadCRLVController.cs", "UploadCRLVController", error);
+                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "UploadCRLVController" , error);
             }
         }
 
         [AcceptVerbs("Post")]
         [HttpPost]
         [Route("Save")]
-        public IActionResult Save(IList<IFormFile> UploadFiles, [FromQuery] Guid veiculoId)
+        public IActionResult Save(IList<IFormFile> UploadFiles , [FromQuery] Guid veiculoId)
         {
             try
             {
-
                 if (UploadFiles != null && veiculoId != Guid.Empty)
                 {
                     foreach (var file in UploadFiles)
                     {
-
                         var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                             u.VeiculoId == veiculoId
                         );
 
                         if (objFromDb != null)
                         {
-
                             using (var target = new MemoryStream())
                             {
                                 file.CopyTo(target);
                                 objFromDb.CRLV = target.ToArray();
                             }
-
                             _unitOfWork.Veiculo.Update(objFromDb);
                             _unitOfWork.Save();
-
-                            _log.Info($"Upload de CRLV realizado com sucesso para o Veículo ID: {veiculoId} (Arquivo: {file.FileName})", "UploadCRLVController", "Save");
                         }
                     }
                 }
-
                 return Content("");
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UploadCRLVController", "Save");
                 Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "Save" , error);
                 Response.StatusCode = 500;
-
                 return Content("");
             }
         }
@@ -83,39 +71,31 @@
         [AcceptVerbs("Post")]
         [HttpPost]
         [Route("Remove")]
-        public IActionResult Remove(IList<IFormFile> UploadFiles, [FromQuery] Guid veiculoId)
+        public IActionResult Remove(IList<IFormFile> UploadFiles , [FromQuery] Guid veiculoId)
         {
             try
             {
-
                 if (veiculoId != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                         u.VeiculoId == veiculoId
                     );
 
                     if (objFromDb != null)
                     {
-
                         objFromDb.CRLV = null;
                         _unitOfWork.Veiculo.Update(objFromDb);
                         _unitOfWork.Save();
-
-                        _log.Info($"Arquivo de CRLV removido com sucesso para o Veículo ID: {veiculoId}", "UploadCRLVController", "Remove");
                     }
                 }
-
                 return Content("");
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UploadCRLVController", "Remove");
                 Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "Remove" , error);
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
-                _log.Error("Erro", error, "UploadCRLVController", "UploadFeatures");
                 Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "UploadFeatures" , error);
-
                 return View();
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public partial class UploadCRLVController : Controller
        private readonly IWebHostEnvironment hostingEnv;
        private readonly ILogService _log;
        public UploadCRLVController(IWebHostEnvironment env, IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs", "UploadCRLVController", error);
        public IActionResult Save(IList<IFormFile> UploadFiles, [FromQuery] Guid veiculoId)
                            _log.Info($"Upload de CRLV realizado com sucesso para o Veículo ID: {veiculoId} (Arquivo: {file.FileName})", "UploadCRLVController", "Save");
                _log.Error("Erro", error, "UploadCRLVController", "Save");
        public IActionResult Remove(IList<IFormFile> UploadFiles, [FromQuery] Guid veiculoId)
                        _log.Info($"Arquivo de CRLV removido com sucesso para o Veículo ID: {veiculoId}", "UploadCRLVController", "Remove");
                _log.Error("Erro", error, "UploadCRLVController", "Remove");
                _log.Error("Erro", error, "UploadCRLVController", "UploadFeatures");
```


### ADICIONAR ao Janeiro

```csharp
    public partial class UploadCRLVController :Controller
        private IWebHostEnvironment hostingEnv;
        public UploadCRLVController(IWebHostEnvironment env , IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "UploadCRLVController" , error);
        public IActionResult Save(IList<IFormFile> UploadFiles , [FromQuery] Guid veiculoId)
        public IActionResult Remove(IList<IFormFile> UploadFiles , [FromQuery] Guid veiculoId)
```
