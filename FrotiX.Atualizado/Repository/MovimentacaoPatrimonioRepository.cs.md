# Repository/MovimentacaoPatrimonioRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/MovimentacaoPatrimonioRepository.cs
+++ ATUAL: Repository/MovimentacaoPatrimonioRepository.cs
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
     public class MovimentacaoPatrimonioRepository : Repository<MovimentacaoPatrimonio>, IMovimentacaoPatrimonioRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public MovimentacaoPatrimonioRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetMovimentacaoPatrimonioListForDropDown()
-        {
+            {
             return _db.MovimentacaoPatrimonio
             .OrderBy(o => o.PatrimonioId)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.DataMovimentacao.ToString(),
                 Value = i.MovimentacaoPatrimonioId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(MovimentacaoPatrimonio movimentacaoPatrimonio)
-        {
-            var objFromDb = _db.MovimentacaoPatrimonio.AsTracking().FirstOrDefault(s => s.MovimentacaoPatrimonioId == movimentacaoPatrimonio.MovimentacaoPatrimonioId);
+            {
+            var objFromDb = _db.MovimentacaoPatrimonio.FirstOrDefault(s => s.MovimentacaoPatrimonioId == movimentacaoPatrimonio.MovimentacaoPatrimonioId);
 
             _db.Update(movimentacaoPatrimonio);
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
            var objFromDb = _db.MovimentacaoPatrimonio.AsTracking().FirstOrDefault(s => s.MovimentacaoPatrimonioId == movimentacaoPatrimonio.MovimentacaoPatrimonioId);
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
            var objFromDb = _db.MovimentacaoPatrimonio.FirstOrDefault(s => s.MovimentacaoPatrimonioId == movimentacaoPatrimonio.MovimentacaoPatrimonioId);
            }
```
