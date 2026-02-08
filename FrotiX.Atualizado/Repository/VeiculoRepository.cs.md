# Repository/VeiculoRepository.cs

**Mudanca:** MEDIA | **+10** linhas | **-11** linhas

---

```diff
--- JANEIRO: Repository/VeiculoRepository.cs
+++ ATUAL: Repository/VeiculoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,26 +8,27 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public VeiculoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetVeiculoListForDropDown()
-        {
+            {
             return _db.Veiculo
             .OrderBy(o => o.Placa)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Placa,
                 Value = i.VeiculoId.ToString()
-            });
-        }
+                }); ;
+            }
 
         public IEnumerable<SelectListItem> GetVeiculoCompletoListForDropDown()
         {
@@ -43,11 +43,11 @@
         }
 
         public new void Update(Veiculo veiculo)
-        {
+            {
 
             _db.Update(veiculo);
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
            }
```
