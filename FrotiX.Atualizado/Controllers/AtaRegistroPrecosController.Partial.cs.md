# Controllers/AtaRegistroPrecosController.Partial.cs

**Mudanca:** MEDIA | **+9** linhas | **-11** linhas

---

```diff
--- JANEIRO: Controllers/AtaRegistroPrecosController.Partial.cs
+++ ATUAL: Controllers/AtaRegistroPrecosController.Partial.cs
@@ -2,26 +2,23 @@
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Linq;
-using FrotiX.Helpers;
 
 namespace FrotiX.Controllers
 {
 
-    public partial class AtaRegistroPrecosController : ControllerBase
+    public partial class AtaRegistroPrecosController :ControllerBase
     {
-
         [Route("VerificarDependencias")]
         [HttpGet]
         public IActionResult VerificarDependencias(Guid id)
         {
             try
             {
-
                 if (id == Guid.Empty)
                 {
                     return BadRequest(new
                     {
-                        success = false,
+                        success = false ,
                         message = "ID inválido"
                     });
                 }
@@ -33,23 +30,22 @@
 
                 return Ok(new
                 {
-                    success = true,
-                    possuiDependencias,
-                    itens = itensCount,
+                    success = true ,
+                    possuiDependencias ,
+                    itens = itensCount ,
                     veiculos = veiculosCount
                 });
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "AtaRegistroPrecosController.Partial.cs", "VerificarDependencias");
                 Alerta.TratamentoErroComLinha(
-                    "AtaRegistroPrecosController.Partial.cs",
-                    "VerificarDependencias",
+                    "AtaRegistroPrecosController.Partial.cs" ,
+                    "VerificarDependencias" ,
                     error
                 );
-                return StatusCode(500, new
+                return StatusCode(500 , new
                 {
-                    success = false,
+                    success = false ,
                     message = "Erro ao verificar dependências"
                 });
             }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
    public partial class AtaRegistroPrecosController : ControllerBase
                        success = false,
                    success = true,
                    possuiDependencias,
                    itens = itensCount,
                _log.Error(error.Message, error, "AtaRegistroPrecosController.Partial.cs", "VerificarDependencias");
                    "AtaRegistroPrecosController.Partial.cs",
                    "VerificarDependencias",
                return StatusCode(500, new
                    success = false,
```


### ADICIONAR ao Janeiro

```csharp
    public partial class AtaRegistroPrecosController :ControllerBase
                        success = false ,
                    success = true ,
                    possuiDependencias ,
                    itens = itensCount ,
                    "AtaRegistroPrecosController.Partial.cs" ,
                    "VerificarDependencias" ,
                return StatusCode(500 , new
                    success = false ,
```
