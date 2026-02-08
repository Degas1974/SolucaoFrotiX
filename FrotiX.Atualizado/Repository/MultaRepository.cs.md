# Repository/MultaRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/MultaRepository.cs
+++ ATUAL: Repository/MultaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,35 +8,36 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class MultaRepository : Repository<Multa>, IMultaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public MultaRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetMultaListForDropDown()
-        {
+            {
             return _db.Multa
                 .OrderBy(o => o.NumInfracao)
                 .Select(i => new SelectListItem()
-                {
+                    {
                     Text = i.NumInfracao,
                     Value = i.MultaId.ToString()
-                });
-        }
+                    });
+            }
 
         public new void Update(Multa multa)
-        {
-            var objFromDb = _db.Multa.AsTracking().FirstOrDefault(s => s.MultaId == multa.MultaId);
+            {
+            var objFromDb = _db.Multa.FirstOrDefault(s => s.MultaId == multa.MultaId);
 
             _db.Update(multa);
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
            var objFromDb = _db.Multa.AsTracking().FirstOrDefault(s => s.MultaId == multa.MultaId);
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
            var objFromDb = _db.Multa.FirstOrDefault(s => s.MultaId == multa.MultaId);
            }
```
