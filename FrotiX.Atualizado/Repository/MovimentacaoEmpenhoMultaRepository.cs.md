# Repository/MovimentacaoEmpenhoMultaRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/MovimentacaoEmpenhoMultaRepository.cs
+++ ATUAL: Repository/MovimentacaoEmpenhoMultaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,34 +8,35 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class MovimentacaoEmpenhoMultaRepository : Repository<MovimentacaoEmpenhoMulta>, IMovimentacaoEmpenhoMultaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public MovimentacaoEmpenhoMultaRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown()
-        {
+            {
             return _db.MovimentacaoEmpenhoMulta
             .OrderBy(o => o.DataMovimentacao)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.DataMovimentacao + "(" + i.Valor + ")",
                 Value = i.MovimentacaoId.ToString()
-            });
-        }
+                });
+            }
 
         public new void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta)
-        {
-            var objFromDb = _db.MovimentacaoEmpenhoMulta.AsTracking().FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);
+            {
+            var objFromDb = _db.MovimentacaoEmpenhoMulta.FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);
 
             _db.Update(movimentacaoempenhomulta);
             _db.SaveChanges();
 
+            }
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
            var objFromDb = _db.MovimentacaoEmpenhoMulta.AsTracking().FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);
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
            var objFromDb = _db.MovimentacaoEmpenhoMulta.FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);
            }
```
