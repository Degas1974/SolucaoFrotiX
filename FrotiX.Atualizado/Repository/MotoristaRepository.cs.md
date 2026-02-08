# Repository/MotoristaRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/MotoristaRepository.cs
+++ ATUAL: Repository/MotoristaRepository.cs
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
     public class MotoristaRepository : Repository<Motorista>, IMotoristaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public MotoristaRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetMotoristaListForDropDown()
-        {
+            {
             return _db.Motorista
             .OrderBy(o => o.Nome)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Nome,
                 Value = i.MotoristaId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(Motorista motorista)
-        {
-            var objFromDb = _db.Motorista.AsTracking().FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);
+            {
+            var objFromDb = _db.Motorista.FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);
 
             _db.Update(motorista);
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
            var objFromDb = _db.Motorista.AsTracking().FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);
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
            var objFromDb = _db.Motorista.FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);
            }
```
