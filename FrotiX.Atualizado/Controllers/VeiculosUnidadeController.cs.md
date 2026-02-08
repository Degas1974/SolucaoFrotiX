# Controllers/VeiculosUnidadeController.cs

**Mudanca:** MEDIA | **+7** linhas | **-6** linhas

---

```diff
--- JANEIRO: Controllers/VeiculosUnidadeController.cs
+++ ATUAL: Controllers/VeiculosUnidadeController.cs
@@ -4,28 +4,29 @@
 using System;
 using System.Collections.Generic;
 using System.Linq;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     [Route("api/[controller]")]
     [ApiController]
-    public class VeiculosUnidadeController : Controller
+    public class VeiculosUnidadeController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
 
-        public VeiculosUnidadeController(IUnitOfWork unitOfWork, ILogService log)
+        public VeiculosUnidadeController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs", "VeiculosUnidadeController", error);
+                Alerta.TratamentoErroComLinha(
+                    "VeiculosUnidadeController.cs" ,
+                    "VeiculosUnidadeController" ,
+                    error
+                );
             }
         }
 
@@ -51,9 +52,11 @@
                     {
                         v.VeiculoId ,
                         v.Placa ,
+
                         MarcaModelo = ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                         u.Sigla ,
                         CombustivelDescricao = c.Descricao ,
+
                         ContratoVeiculo = ct.AnoContrato
                             + "/"
                             + ct.NumeroContrato
@@ -74,7 +77,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs" , "Get" , error);
-
                 return Json(new
                 {
                     success = false ,
@@ -96,14 +98,12 @@
                     var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                         u.VeiculoId == model.VeiculoId
                     );
-
                     if (objFromDb != null)
                     {
 
                         objFromDb.UnidadeId = Guid.Empty;
                         _unitOfWork.Veiculo.Update(objFromDb);
                         _unitOfWork.Save();
-
                         return Json(
                             new
                             {
@@ -113,7 +113,6 @@
                         );
                     }
                 }
-
                 return Json(new
                 {
                     success = false ,
@@ -123,7 +122,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs" , "Delete" , error);
-
                 return Json(new
                 {
                     success = false ,
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
    public class VeiculosUnidadeController : Controller
        private readonly ILogService _log;
        public VeiculosUnidadeController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs", "VeiculosUnidadeController", error);
```


### ADICIONAR ao Janeiro

```csharp
    public class VeiculosUnidadeController :Controller
        public VeiculosUnidadeController(IUnitOfWork unitOfWork)
                Alerta.TratamentoErroComLinha(
                    "VeiculosUnidadeController.cs" ,
                    "VeiculosUnidadeController" ,
                    error
                );
```
