# Repository/VeiculoAtaRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/VeiculoAtaRepository.cs
+++ ATUAL: Repository/VeiculoAtaRepository.cs
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
     public class VeiculoAtaRepository : Repository<VeiculoAta>, IVeiculoAtaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public VeiculoAtaRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown()
-        {
+            {
             return _db.VeiculoAta.Select(i => new SelectListItem()
-            {
+                {
 
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(VeiculoAta veiculoAta)
-        {
-            var objFromDb = _db.VeiculoAta.AsTracking().FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));
+            {
+            var objFromDb = _db.VeiculoAta.FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));
 
             _db.Update(veiculoAta);
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
            var objFromDb = _db.VeiculoAta.AsTracking().FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));
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
            var objFromDb = _db.VeiculoAta.FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));
            }
```
