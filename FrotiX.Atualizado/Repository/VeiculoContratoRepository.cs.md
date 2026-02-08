# Repository/VeiculoContratoRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/VeiculoContratoRepository.cs
+++ ATUAL: Repository/VeiculoContratoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,32 +8,33 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class VeiculoContratoRepository : Repository<VeiculoContrato>, IVeiculoContratoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public VeiculoContratoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetVeiculoContratoListForDropDown()
-        {
+            {
             return _db.VeiculoContrato.Select(i => new SelectListItem()
-            {
+                {
 
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(VeiculoContrato veiculoContrato)
-        {
-            var objFromDb = _db.VeiculoContrato.AsTracking().FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));
+            {
+            var objFromDb = _db.VeiculoContrato.FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));
 
             _db.Update(veiculoContrato);
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
            var objFromDb = _db.VeiculoContrato.AsTracking().FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));
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
            var objFromDb = _db.VeiculoContrato.FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));
            }
```
