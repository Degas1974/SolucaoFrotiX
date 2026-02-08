# Repository/AbastecimentoRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/AbastecimentoRepository.cs
+++ ATUAL: Repository/AbastecimentoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,35 +8,34 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
-
+    {
     public class AbastecimentoRepository : Repository<Abastecimento>, IAbastecimentoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public AbastecimentoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetAbastecimentoListForDropDown()
-        {
-
+            {
             return _db.Abastecimento
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Litros.ToString(),
                 Value = i.AbastecimentoId.ToString()
-            });
-        }
+                }); ;
+            }
 
         public new void Update(Abastecimento abastecimento)
-        {
-
-            var objFromDb = _db.Abastecimento.AsTracking().FirstOrDefault(s => s.AbastecimentoId == abastecimento.AbastecimentoId);
+            {
+            var objFromDb = _db.Abastecimento.FirstOrDefault(s => s.AbastecimentoId == abastecimento.AbastecimentoId);
 
             _db.Update(abastecimento);
             _db.SaveChanges();
+
+            }
+
         }
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
            var objFromDb = _db.Abastecimento.AsTracking().FirstOrDefault(s => s.AbastecimentoId == abastecimento.AbastecimentoId);
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
            var objFromDb = _db.Abastecimento.FirstOrDefault(s => s.AbastecimentoId == abastecimento.AbastecimentoId);
            }
```
