# Repository/CorridasTaxiLegCanceladasRepository.cs

**Mudanca:** GRANDE | **+14** linhas | **-32** linhas

---

```diff
--- JANEIRO: Repository/CorridasTaxiLegCanceladasRepository.cs
+++ ATUAL: Repository/CorridasTaxiLegCanceladasRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,55 +6,36 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
-
+    {
     public class CorridasCanceladasTaxiLegRepository : Repository<CorridasCanceladasTaxiLeg>, ICorridasCanceladasTaxiLegRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public CorridasCanceladasTaxiLegRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetCorridasCanceladasTaxiLegListForDropDown()
-        {
-            try
             {
-
-                return _db.CorridasCanceladasTaxiLeg
-                    .Select(i => new SelectListItem()
-                    {
-                        Text = i.MotivoCancelamento,
-                        Value = i.CorridaCanceladaId.ToString()
-                    });
+            return _db.CorridasCanceladasTaxiLeg
+            .Select(i => new SelectListItem()
+                {
+                Text = i.MotivoCancelamento,
+                Value = i.CorridaCanceladaId.ToString()
+                });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CorridasTaxiLegCanceladasRepository.cs", "GetCorridasCanceladasTaxiLegListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(CorridasCanceladasTaxiLeg corridasCanceladasTaxiLeg)
-        {
-            try
             {
+            var objFromDb = _db.CorridasCanceladasTaxiLeg.FirstOrDefault(s => s.CorridaCanceladaId == corridasCanceladasTaxiLeg.CorridaCanceladaId);
 
-                var objFromDb = _db.CorridasCanceladasTaxiLeg.AsTracking().FirstOrDefault(s => s.CorridaCanceladaId == corridasCanceladasTaxiLeg.CorridaCanceladaId);
+            _db.Update(corridasCanceladasTaxiLeg);
+            _db.SaveChanges();
 
-                _db.Update(corridasCanceladasTaxiLeg);
+            }
 
-                _db.SaveChanges();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CorridasTaxiLegCanceladasRepository.cs", "Update", ex);
-                throw;
-            }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
using FrotiX.Helpers;
{
    {
        {
        }
        {
            try
                return _db.CorridasCanceladasTaxiLeg
                    .Select(i => new SelectListItem()
                    {
                        Text = i.MotivoCancelamento,
                        Value = i.CorridaCanceladaId.ToString()
                    });
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegCanceladasRepository.cs", "GetCorridasCanceladasTaxiLegListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.CorridasCanceladasTaxiLeg.AsTracking().FirstOrDefault(s => s.CorridaCanceladaId == corridasCanceladasTaxiLeg.CorridaCanceladaId);
                _db.Update(corridasCanceladasTaxiLeg);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegCanceladasRepository.cs", "Update", ex);
                throw;
            }
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
            return _db.CorridasCanceladasTaxiLeg
            .Select(i => new SelectListItem()
                {
                Text = i.MotivoCancelamento,
                Value = i.CorridaCanceladaId.ToString()
                });
            var objFromDb = _db.CorridasCanceladasTaxiLeg.FirstOrDefault(s => s.CorridaCanceladaId == corridasCanceladasTaxiLeg.CorridaCanceladaId);
            _db.Update(corridasCanceladasTaxiLeg);
            _db.SaveChanges();
            }
```
