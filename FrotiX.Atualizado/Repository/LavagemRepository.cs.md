# Repository/LavagemRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/LavagemRepository.cs
+++ ATUAL: Repository/LavagemRepository.cs
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
     public class LavagemRepository : Repository<Lavagem>, ILavagemRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public LavagemRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetLavagemListForDropDown()
-        {
+            {
             return _db.Lavagem
             .OrderBy(o => o.Data)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Data.ToString(),
                 Value = i.LavagemId.ToString()
-            }); ; ;
-        }
+                }); ; ;
+            }
 
         public new void Update(Lavagem lavagem)
-        {
-            var objFromDb = _db.Lavagem.AsTracking().FirstOrDefault(s => s.LavagemId == lavagem.LavagemId);
+            {
+            var objFromDb = _db.Lavagem.FirstOrDefault(s => s.LavagemId == lavagem.LavagemId);
 
             _db.Update(lavagem);
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
            }); ; ;
        }
        {
            var objFromDb = _db.Lavagem.AsTracking().FirstOrDefault(s => s.LavagemId == lavagem.LavagemId);
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
                }); ; ;
            }
            {
            var objFromDb = _db.Lavagem.FirstOrDefault(s => s.LavagemId == lavagem.LavagemId);
            }
```
