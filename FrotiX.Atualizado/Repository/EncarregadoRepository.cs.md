# Repository/EncarregadoRepository.cs

**Mudanca:** MEDIA | **+2** linhas | **-13** linhas

---

```diff
--- JANEIRO: Repository/EncarregadoRepository.cs
+++ ATUAL: Repository/EncarregadoRepository.cs
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
 
         public new void Update(Encarregado encarregado)
         {
-            try
-            {
 
-                _db.Encarregado.Update(encarregado);
-
-                _db.SaveChanges();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EncarregadoRepository.cs", "Update", ex);
-                throw;
-            }
+            _db.Encarregado.Update(encarregado);
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
                _db.Encarregado.Update(encarregado);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EncarregadoRepository.cs", "Update", ex);
                throw;
            }
```


### ADICIONAR ao Janeiro

```csharp
            _db.Encarregado.Update(encarregado);
            _db.SaveChanges();
```
