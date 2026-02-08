# Repository/ModeloVeiculoRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/ModeloVeiculoRepository.cs
+++ ATUAL: Repository/ModeloVeiculoRepository.cs
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
     public class ModeloVeiculoRepository : Repository<ModeloVeiculo>, IModeloVeiculoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public ModeloVeiculoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown()
-        {
+            {
             return _db.ModeloVeiculo
             .OrderBy(o => o.DescricaoModelo)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.DescricaoModelo,
                 Value = i.ModeloId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(ModeloVeiculo modeloVeiculo)
-        {
-            var objFromDb = _db.ModeloVeiculo.AsTracking().FirstOrDefault(s => s.ModeloId == modeloVeiculo.ModeloId);
+            {
+            var objFromDb = _db.ModeloVeiculo.FirstOrDefault(s => s.ModeloId == modeloVeiculo.ModeloId);
 
             _db.Update(modeloVeiculo);
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
            var objFromDb = _db.ModeloVeiculo.AsTracking().FirstOrDefault(s => s.ModeloId == modeloVeiculo.ModeloId);
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
            var objFromDb = _db.ModeloVeiculo.FirstOrDefault(s => s.ModeloId == modeloVeiculo.ModeloId);
            }
```
