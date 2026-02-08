# Repository/ViewAtaFornecedorRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/ViewAtaFornecedorRepository.cs
+++ ATUAL: Repository/ViewAtaFornecedorRepository.cs
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
     public class ViewAtaFornecedorRepository : Repository<ViewAtaFornecedor>, IViewAtaFornecedorRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public ViewAtaFornecedorRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetViewAtaFornecedorListForDropDown()
-        {
+            {
             return _db.ViewAtaFornecedor
             .OrderBy(o => o.AtaVeiculo)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.AtaVeiculo.ToString(),
                 Value = i.AtaId.ToString()
-            }); ; ;
-        }
+                }); ; ;
+            }
 
         public new void Update(ViewAtaFornecedor viewAtaFornecedor)
-        {
-            var objFromDb = _db.ViewAtaFornecedor.AsTracking().FirstOrDefault(s => s.AtaId == viewAtaFornecedor.AtaId);
+            {
+            var objFromDb = _db.ViewAtaFornecedor.FirstOrDefault(s => s.AtaId == viewAtaFornecedor.AtaId);
 
             _db.Update(viewAtaFornecedor);
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
            }); ; ;
        }
        {
            var objFromDb = _db.ViewAtaFornecedor.AsTracking().FirstOrDefault(s => s.AtaId == viewAtaFornecedor.AtaId);
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
                }); ; ;
            }
            {
            var objFromDb = _db.ViewAtaFornecedor.FirstOrDefault(s => s.AtaId == viewAtaFornecedor.AtaId);
            }
```
