# Repository/NotaFiscalRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/NotaFiscalRepository.cs
+++ ATUAL: Repository/NotaFiscalRepository.cs
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
     public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public NotaFiscalRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetNotaFiscalListForDropDown()
-        {
+            {
             return _db.NotaFiscal
             .OrderBy(o => o.NumeroNF)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.NumeroNF.ToString(),
                 Value = i.NotaFiscalId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(NotaFiscal notaFiscal)
-        {
-            var objFromDb = _db.NotaFiscal.AsTracking().FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);
+            {
+            var objFromDb = _db.NotaFiscal.FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);
 
             _db.Update(notaFiscal);
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
            }); ;
        }
        {
            var objFromDb = _db.NotaFiscal.AsTracking().FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);
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
            var objFromDb = _db.NotaFiscal.FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);
            }
```
