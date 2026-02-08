# Controllers/ContratoController.Partial.cs

**Mudanca:** MEDIA | **+3** linhas | **-9** linhas

---

```diff
--- JANEIRO: Controllers/ContratoController.Partial.cs
+++ ATUAL: Controllers/ContratoController.Partial.cs
@@ -7,6 +7,7 @@
 
 namespace FrotiX.Controllers
 {
+
     public partial class ContratoController : Controller
     {
 
@@ -16,7 +17,6 @@
         {
             try
             {
-
                 bool statusBool = status == 1;
 
                 var result = (
@@ -37,16 +37,10 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ContratoController.Partial.cs", "ListaContratosPorStatus");
-                Alerta.TratamentoErroComLinha(
-                    "ContratoController.Partial.cs",
-                    "ListaContratosPorStatus",
-                    error
-                );
-                return StatusCode(500, new
+                Alerta.TratamentoErroComLinha("ContratoController.cs", "ListaContratosPorStatus", error);
+                return Json(new
                 {
-                    success = false,
-                    message = "Erro ao listar contratos"
+                    data = new List<object>()
                 });
             }
         }
```

### REMOVER do Janeiro

```csharp
                _log.Error(error.Message, error, "ContratoController.Partial.cs", "ListaContratosPorStatus");
                Alerta.TratamentoErroComLinha(
                    "ContratoController.Partial.cs",
                    "ListaContratosPorStatus",
                    error
                );
                return StatusCode(500, new
                    success = false,
                    message = "Erro ao listar contratos"
```


### ADICIONAR ao Janeiro

```csharp
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ListaContratosPorStatus", error);
                return Json(new
                    data = new List<object>()
```
