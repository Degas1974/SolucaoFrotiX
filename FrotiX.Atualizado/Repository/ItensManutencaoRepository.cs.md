# Repository/ItensManutencaoRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/ItensManutencaoRepository.cs
+++ ATUAL: Repository/ItensManutencaoRepository.cs
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
     public class ItensManutencaoRepository : Repository<ItensManutencao>, IItensManutencaoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public ItensManutencaoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetItensManutencaoListForDropDown()
-        {
+            {
             return _db.ItensManutencao
                 .OrderBy(o => o.DataItem)
                 .Select(i => new SelectListItem()
-                {
+                    {
                     Text = i.Resumo,
                     Value = i.ItemManutencaoId.ToString()
-                });
-        }
+                    });
+            }
 
         public new void Update(ItensManutencao itensManutencao)
-        {
-            var objFromDb = _db.ItensManutencao.AsTracking().FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);
+            {
+            var objFromDb = _db.ItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);
 
             _db.Update(itensManutencao);
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
            var objFromDb = _db.ItensManutencao.AsTracking().FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);
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
            var objFromDb = _db.ItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);
            }
```
