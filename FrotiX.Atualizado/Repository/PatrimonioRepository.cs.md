# Repository/PatrimonioRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/PatrimonioRepository.cs
+++ ATUAL: Repository/PatrimonioRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,35 +9,36 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class PatrimonioRepository : Repository<Patrimonio>, IPatrimonioRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public PatrimonioRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetPatrimonioListForDropDown()
-        {
+            {
             return _db.Patrimonio
             .OrderBy(o => o.NumeroSerie)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.NPR,
                 Value = i.PatrimonioId.ToString()
-            });
-        }
+                });
+            }
 
         public new void Update(Patrimonio patrimonio)
-        {
-            var objFromDb = _db.Patrimonio.AsTracking().FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);
+            {
+            var objFromDb = _db.Patrimonio.FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);
 
             _db.Update(patrimonio);
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
            var objFromDb = _db.Patrimonio.AsTracking().FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);
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
            var objFromDb = _db.Patrimonio.FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);
            }
```
