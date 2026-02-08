# Repository/EncarregadoContratoRepository.cs

**Mudanca:** MEDIA | **+2** linhas | **-13** linhas

---

```diff
--- JANEIRO: Repository/EncarregadoContratoRepository.cs
+++ ATUAL: Repository/EncarregadoContratoRepository.cs
@@ -1,9 +1,6 @@
-using Microsoft.EntityFrameworkCore;
-using System;
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
 {
@@ -19,18 +16,9 @@
 
         public new void Update(EncarregadoContrato encarregadoContrato)
         {
-            try
-            {
 
-                _db.EncarregadoContrato.Update(encarregadoContrato);
-
-                _db.SaveChanges();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EncarregadoContratoRepository.cs", "Update", ex);
-                throw;
-            }
+            _db.EncarregadoContrato.Update(encarregadoContrato);
+            _db.SaveChanges();
         }
     }
 }
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
using System;
using FrotiX.Helpers;
            try
            {
                _db.EncarregadoContrato.Update(encarregadoContrato);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EncarregadoContratoRepository.cs", "Update", ex);
                throw;
            }
```


### ADICIONAR ao Janeiro

```csharp
            _db.EncarregadoContrato.Update(encarregadoContrato);
            _db.SaveChanges();
```
