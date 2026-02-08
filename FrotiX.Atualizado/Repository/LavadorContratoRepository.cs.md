# Repository/LavadorContratoRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/LavadorContratoRepository.cs
+++ ATUAL: Repository/LavadorContratoRepository.cs
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
     public class LavadorContratoRepository : Repository<LavadorContrato>, ILavadorContratoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public LavadorContratoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetLavadorContratoListForDropDown()
-        {
+            {
             return _db.LavadorContrato.Select(i => new SelectListItem()
-            {
+                {
 
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(LavadorContrato lavadorContrato)
-        {
-            var objFromDb = _db.LavadorContrato.AsTracking().FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));
+            {
+            var objFromDb = _db.LavadorContrato.FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));
 
             _db.Update(lavadorContrato);
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
            }); ;
        }
        {
            var objFromDb = _db.LavadorContrato.AsTracking().FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));
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
                }); ;
            }
            {
            var objFromDb = _db.LavadorContrato.FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));
            }
```
