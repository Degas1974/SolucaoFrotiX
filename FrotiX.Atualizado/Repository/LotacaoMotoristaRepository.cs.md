# Repository/LotacaoMotoristaRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/LotacaoMotoristaRepository.cs
+++ ATUAL: Repository/LotacaoMotoristaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,32 +8,33 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class LotacaoMotoristaRepository : Repository<LotacaoMotorista>, ILotacaoMotoristaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public LotacaoMotoristaRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetLotacaoMotoristaListForDropDown()
-        {
+            {
             return _db.LotacaoMotorista
                 .Select(i => new SelectListItem()
-                {
-                });
-        }
+                    {
+                    });
+            }
 
         public new void Update(LotacaoMotorista lotacaoMotorista)
-        {
-            var objFromDb = _db.LotacaoMotorista.AsTracking().FirstOrDefault(lm => lm.LotacaoMotoristaId == lotacaoMotorista.LotacaoMotoristaId);
+            {
+            var objFromDb = _db.LotacaoMotorista.FirstOrDefault(lm => lm.LotacaoMotoristaId == lotacaoMotorista.LotacaoMotoristaId);
 
             _db.Update(lotacaoMotorista);
             _db.SaveChanges();
 
+            }
+
         }
-
     }
-}
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
{
    {
        {
        }
        {
                {
                });
        }
        {
            var objFromDb = _db.LotacaoMotorista.AsTracking().FirstOrDefault(lm => lm.LotacaoMotoristaId == lotacaoMotorista.LotacaoMotoristaId);
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
            {
                    {
                    });
            }
            {
            var objFromDb = _db.LotacaoMotorista.FirstOrDefault(lm => lm.LotacaoMotoristaId == lotacaoMotorista.LotacaoMotoristaId);
            }
```
