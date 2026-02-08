# Controllers/ViagemEventoController.UpdateStatus.cs

**Mudanca:** PEQUENA | **+2** linhas | **-3** linhas

---

```diff
--- JANEIRO: Controllers/ViagemEventoController.UpdateStatus.cs
+++ ATUAL: Controllers/ViagemEventoController.UpdateStatus.cs
@@ -1,7 +1,6 @@
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc;
 using System;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -39,9 +38,10 @@
                     message = evento.Status == "1" ? "Evento ativado com sucesso" : "Evento inativado com sucesso"
                 });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("ViagemEventoController.UpdateStatus.cs", "UpdateStatusEvento", ex);
+
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "UpdateStatusEvento", error);
                 return Json(new
                 {
                     success = false,
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("ViagemEventoController.UpdateStatus.cs", "UpdateStatusEvento", ex);
```


### ADICIONAR ao Janeiro

```csharp
            catch (Exception error)
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "UpdateStatusEvento", error);
```
