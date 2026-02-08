# Repository/LavadorRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/LavadorRepository.cs
+++ ATUAL: Repository/LavadorRepository.cs
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
     public class LavadorRepository : Repository<Lavador>, ILavadorRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public LavadorRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetLavadorListForDropDown()
-        {
+            {
             return _db.Lavador
             .OrderBy(o => o.Nome)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Nome,
                 Value = i.LavadorId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(Lavador lavador)
-        {
-            var objFromDb = _db.Lavador.AsTracking().FirstOrDefault(s => s.LavadorId == lavador.LavadorId);
+            {
+            var objFromDb = _db.Lavador.FirstOrDefault(s => s.LavadorId == lavador.LavadorId);
 
             _db.Update(lavador);
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
            var objFromDb = _db.Lavador.AsTracking().FirstOrDefault(s => s.LavadorId == lavador.LavadorId);
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
            var objFromDb = _db.Lavador.FirstOrDefault(s => s.LavadorId == lavador.LavadorId);
            }
```
