# Controllers/LoginController.cs

**Mudanca:** GRANDE | **+23** linhas | **-16** linhas

---

```diff
--- JANEIRO: Controllers/LoginController.cs
+++ ATUAL: Controllers/LoginController.cs
@@ -1,5 +1,4 @@
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.Extensions.Logging;
@@ -8,45 +7,47 @@
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
-    public class LoginController : Controller
+    public class LoginController :Controller
     {
-        private readonly ILogger<LoginController> _logger;
+        private readonly ILogger<AbastecimentoController> _logger;
         private IWebHostEnvironment _hostingEnvironment;
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public LoginController(ILogger<LoginController> logger, IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, ILogService log)
+        public LoginController(
+            ILogger<AbastecimentoController> logger ,
+            IWebHostEnvironment hostingEnvironment ,
+            IUnitOfWork unitOfWork
+        )
         {
             try
             {
                 _logger = logger;
                 _hostingEnvironment = hostingEnvironment;
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("LoginController.cs", "LoginController", error);
+                Alerta.TratamentoErroComLinha("LoginController.cs" , "LoginController" , error);
             }
         }
 
         [BindProperty]
-        public Models.Abastecimento AbastecimentoObj { get; set; }
+        public Models.Abastecimento AbastecimentoObj
+        {
+            get; set;
+        }
 
         public IActionResult Index()
         {
             try
             {
-
                 return View();
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LoginController.cs", "Index");
-                Alerta.TratamentoErroComLinha("LoginController.cs", "Index", error);
+                Alerta.TratamentoErroComLinha("LoginController.cs" , "Index" , error);
                 return View();
             }
         }
@@ -56,12 +57,10 @@
         {
             try
             {
-
                 return View();
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LoginController.cs", "Get");
                 Alerta.TratamentoErroComLinha("LoginController.cs" , "Get" , error);
                 return View();
             }
@@ -78,19 +77,24 @@
                 ClaimsPrincipal currentUser = User;
                 var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
 
-                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == currentUserID);
+                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u =>
+                    u.Id == currentUserID
+                );
 
                 usuarioCorrenteNome = objUsuario.NomeCompleto;
                 usuarioCorrentePonto = objUsuario.Ponto;
                 Settings.GlobalVariables.gPontoUsuario = objUsuario.Ponto;
 
-                return Ok(new { nome = usuarioCorrenteNome, ponto = usuarioCorrentePonto });
+                return Json(new
+                {
+                    nome = usuarioCorrenteNome ,
+                    ponto = usuarioCorrentePonto
+                });
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "LoginController.cs", "RecuperaUsuarioAtual");
-                Alerta.TratamentoErroComLinha("LoginController.cs", "RecuperaUsuarioAtual", error);
-                return BadRequest();
+                Alerta.TratamentoErroComLinha("LoginController.cs" , "RecuperaUsuarioAtual" , error);
+                return View();
             }
         }
     }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class LoginController : Controller
        private readonly ILogger<LoginController> _logger;
        private readonly ILogService _log;
        public LoginController(ILogger<LoginController> logger, IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("LoginController.cs", "LoginController", error);
        public Models.Abastecimento AbastecimentoObj { get; set; }
                _log.Error(error.Message, error, "LoginController.cs", "Index");
                Alerta.TratamentoErroComLinha("LoginController.cs", "Index", error);
                _log.Error(error.Message, error, "LoginController.cs", "Get");
                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == currentUserID);
                return Ok(new { nome = usuarioCorrenteNome, ponto = usuarioCorrentePonto });
                _log.Error(error.Message, error, "LoginController.cs", "RecuperaUsuarioAtual");
                Alerta.TratamentoErroComLinha("LoginController.cs", "RecuperaUsuarioAtual", error);
                return BadRequest();
```


### ADICIONAR ao Janeiro

```csharp
    public class LoginController :Controller
        private readonly ILogger<AbastecimentoController> _logger;
        public LoginController(
            ILogger<AbastecimentoController> logger ,
            IWebHostEnvironment hostingEnvironment ,
            IUnitOfWork unitOfWork
        )
                Alerta.TratamentoErroComLinha("LoginController.cs" , "LoginController" , error);
        public Models.Abastecimento AbastecimentoObj
        {
            get; set;
        }
                Alerta.TratamentoErroComLinha("LoginController.cs" , "Index" , error);
                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u =>
                    u.Id == currentUserID
                );
                return Json(new
                {
                    nome = usuarioCorrenteNome ,
                    ponto = usuarioCorrentePonto
                });
                Alerta.TratamentoErroComLinha("LoginController.cs" , "RecuperaUsuarioAtual" , error);
                return View();
```
