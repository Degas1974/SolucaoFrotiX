# Repository/LavadoresLavagemRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/LavadoresLavagemRepository.cs
+++ ATUAL: Repository/LavadoresLavagemRepository.cs
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
     public class LavadoresLavagemRepository : Repository<LavadoresLavagem>, ILavadoresLavagemRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public LavadoresLavagemRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown()
-        {
+            {
             return _db.LavadoresLavagem
             .OrderBy(o => o.LavagemId)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.LavadorId.ToString(),
                 Value = i.LavagemId.ToString()
-            }); ; ;
-        }
+                }); ; ;
+            }
 
         public new void Update(LavadoresLavagem lavadoresLavagem)
-        {
-            var objFromDb = _db.LavadoresLavagem.AsTracking().FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);
+            {
+            var objFromDb = _db.LavadoresLavagem.FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);
 
             _db.Update(lavadoresLavagem);
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
            var objFromDb = _db.LavadoresLavagem.AsTracking().FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);
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
            var objFromDb = _db.LavadoresLavagem.FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);
            }
```
