# Controllers/SecaoController.cs

**Mudanca:** MEDIA | **+3** linhas | **-10** linhas

---

```diff
--- JANEIRO: Controllers/SecaoController.cs
+++ ATUAL: Controllers/SecaoController.cs
@@ -1,5 +1,4 @@
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -10,21 +9,19 @@
 
     [Route("api/[controller]")]
     [ApiController]
-    public class SecaoController : Controller
+    public class SecaoController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public SecaoController(IUnitOfWork unitOfWork, ILogService log)
+        public SecaoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("SecaoController.cs", "SecaoController", error);
+                Alerta.TratamentoErroComLinha("SecaoController.cs" , "SecaoController" , error);
             }
         }
 
@@ -61,7 +58,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SecaoController.cs" , "ListaSecoes" , error);
-                _log.Error("Erro ao listar seções patrimoniais", error);
                 return Json(
                     new
                     {
@@ -79,7 +75,6 @@
         {
             try
             {
-
                 if (!setorSelecionado.HasValue || setorSelecionado == Guid.Empty)
                 {
                     return Json(new
@@ -105,7 +100,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SecaoController.cs" , "ListaSecoes" , error);
-                _log.Error($", errorErro ao carregar combo de seções para o setor [ID: {setorSelecionado}]");
                 return Json(
                     new
                     {
@@ -122,10 +116,8 @@
         {
             try
             {
-
                 if (Id != Guid.Empty)
                 {
-
                     var objFromDb = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u =>
                         u.SecaoId == Id
                     );
@@ -133,7 +125,6 @@
                     int type = 0;
                     if (objFromDb != null)
                     {
-
                         if (objFromDb.Status == true)
                         {
                             objFromDb.Status = false;
@@ -154,9 +145,7 @@
                         }
                         _unitOfWork.SecaoPatrimonial.Update(objFromDb);
                         _unitOfWork.Save();
-                        _log.Info(Description);
                     }
-
                     return Json(
                         new
                         {
@@ -166,7 +155,6 @@
                         }
                     );
                 }
-
                 return Json(new
                 {
                     success = false
@@ -179,7 +167,6 @@
                     "UpdateStatusSecao" ,
                     error
                 );
-                _log.Error($", errorErro ao alternar status da seção [ID: {Id}]");
                 return new JsonResult(new
                 {
                     sucesso = false
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class SecaoController : Controller
        private readonly ILogService _log;
        public SecaoController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("SecaoController.cs", "SecaoController", error);
                _log.Error("Erro ao listar seções patrimoniais", error);
                _log.Error($", errorErro ao carregar combo de seções para o setor [ID: {setorSelecionado}]");
                        _log.Info(Description);
                _log.Error($", errorErro ao alternar status da seção [ID: {Id}]");
```


### ADICIONAR ao Janeiro

```csharp
    public class SecaoController :Controller
        public SecaoController(IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha("SecaoController.cs" , "SecaoController" , error);
```
