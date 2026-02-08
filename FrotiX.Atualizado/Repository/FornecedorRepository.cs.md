# Repository/FornecedorRepository.cs

**Mudanca:** MEDIA | **+8** linhas | **-9** linhas

---

```diff
--- JANEIRO: Repository/FornecedorRepository.cs
+++ ATUAL: Repository/FornecedorRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -21,25 +21,22 @@
 
         public IEnumerable<SelectListItem> GetFornecedorListForDropDown()
         {
-
             return _db.Fornecedor
-            .Where(f => f.Status == true)
-            .OrderBy(o => o.DescricaoFornecedor)
-            .Select(i => new SelectListItem()
-            {
-                Text = i.DescricaoFornecedor,
-                Value = i.FornecedorId.ToString()
-            }); ;
+                .Where(f => f.Status == true)
+                .OrderBy(o => o.DescricaoFornecedor)
+                .Select(i => new SelectListItem()
+                {
+                    Text = i.DescricaoFornecedor,
+                    Value = i.FornecedorId.ToString()
+                });
         }
 
         public new void Update(Fornecedor fornecedor)
         {
-            var objFromDb = _db.Fornecedor.AsTracking().FirstOrDefault(s => s.FornecedorId == fornecedor.FornecedorId);
+            var objFromDb = _db.Fornecedor.FirstOrDefault(s => s.FornecedorId == fornecedor.FornecedorId);
 
             _db.Update(fornecedor);
             _db.SaveChanges();
-
         }
-
     }
 }
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            .Where(f => f.Status == true)
            .OrderBy(o => o.DescricaoFornecedor)
            .Select(i => new SelectListItem()
            {
                Text = i.DescricaoFornecedor,
                Value = i.FornecedorId.ToString()
            }); ;
            var objFromDb = _db.Fornecedor.AsTracking().FirstOrDefault(s => s.FornecedorId == fornecedor.FornecedorId);
```


### ADICIONAR ao Janeiro

```csharp
                .Where(f => f.Status == true)
                .OrderBy(o => o.DescricaoFornecedor)
                .Select(i => new SelectListItem()
                {
                    Text = i.DescricaoFornecedor,
                    Value = i.FornecedorId.ToString()
                });
            var objFromDb = _db.Fornecedor.FirstOrDefault(s => s.FornecedorId == fornecedor.FornecedorId);
```
