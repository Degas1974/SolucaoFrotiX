# Repository/ViewAbastecimentosRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/ViewAbastecimentosRepository.cs
+++ ATUAL: Repository/ViewAbastecimentosRepository.cs
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
     public class ViewAbastecimentosRepository : Repository<ViewAbastecimentos>, IViewAbastecimentosRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public ViewAbastecimentosRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetViewAbastecimentosListForDropDown()
-        {
+            {
             return _db.ViewAbastecimentos
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
 
         public new void Update(ViewAbastecimentos viewAbastecimentos)
-        {
-            var objFromDb = _db.ViewAbastecimentos.AsTracking().FirstOrDefault(s => s.AbastecimentoId == viewAbastecimentos.AbastecimentoId);
+            {
+            var objFromDb = _db.ViewAbastecimentos.FirstOrDefault(s => s.AbastecimentoId == viewAbastecimentos.AbastecimentoId);
 
             _db.Update(viewAbastecimentos);
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
            var objFromDb = _db.ViewAbastecimentos.AsTracking().FirstOrDefault(s => s.AbastecimentoId == viewAbastecimentos.AbastecimentoId);
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
            var objFromDb = _db.ViewAbastecimentos.FirstOrDefault(s => s.AbastecimentoId == viewAbastecimentos.AbastecimentoId);
            }
```
