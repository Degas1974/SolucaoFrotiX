# Repository/ManutencaoRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/ManutencaoRepository.cs
+++ ATUAL: Repository/ManutencaoRepository.cs
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
     public class ManutencaoRepository : Repository<Manutencao>, IManutencaoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public ManutencaoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetManutencaoListForDropDown()
-        {
+            {
             return _db.Manutencao
             .OrderBy(o => o.ResumoOS)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.ResumoOS,
                 Value = i.ManutencaoId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(Manutencao manutencao)
-        {
-            var objFromDb = _db.Manutencao.AsTracking().FirstOrDefault(s => s.ManutencaoId == manutencao.ManutencaoId);
+            {
+            var objFromDb = _db.Manutencao.FirstOrDefault(s => s.ManutencaoId == manutencao.ManutencaoId);
 
             _db.Update(manutencao);
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
            var objFromDb = _db.Manutencao.AsTracking().FirstOrDefault(s => s.ManutencaoId == manutencao.ManutencaoId);
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
            var objFromDb = _db.Manutencao.FirstOrDefault(s => s.ManutencaoId == manutencao.ManutencaoId);
            }
```
