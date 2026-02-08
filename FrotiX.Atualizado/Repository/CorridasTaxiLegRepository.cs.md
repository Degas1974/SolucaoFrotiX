# Repository/CorridasTaxiLegRepository.cs

**Mudanca:** GRANDE | **+19** linhas | **-44** linhas

---

```diff
--- JANEIRO: Repository/CorridasTaxiLegRepository.cs
+++ ATUAL: Repository/CorridasTaxiLegRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,74 +6,49 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class CorridasTaxiLegRepository : Repository<CorridasTaxiLeg>, ICorridasTaxiLegRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
-        public CorridasTaxiLegRepository(FrotiXDbContext db) : base(db)
-        {
+        public CorridasTaxiLegRepository(FrotiXDbContext db)
+            : base(db)
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetCorridasTaxiLegListForDropDown()
-        {
-            try
             {
 
-                return _db.CorridasTaxiLeg.Select(i => new SelectListItem()
+            return _db.CorridasTaxiLeg.Select(i => new SelectListItem()
                 {
-                    Text = i.DescUnidade,
-                    Value = i.CorridaId.ToString(),
+                Text = i.DescUnidade,
+                Value = i.CorridaId.ToString(),
                 });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "GetCorridasTaxiLegListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(CorridasTaxiLeg corridasTaxiLeg)
-        {
-            try
             {
 
-                var objFromDb = _db.CorridasTaxiLeg.AsTracking().FirstOrDefault(s =>
-                    s.CorridaId == corridasTaxiLeg.CorridaId
-                );
+            var objFromDb = _db.CorridasTaxiLeg.FirstOrDefault(s =>
+                s.CorridaId == corridasTaxiLeg.CorridaId
+            );
 
-                _db.Update(corridasTaxiLeg);
-
-                _db.SaveChanges();
+            _db.Update(corridasTaxiLeg);
+            _db.SaveChanges();
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "Update", ex);
-                throw;
-            }
-        }
 
         public bool ExisteCorridaNoMesAno(int ano, int mes)
-        {
-            try
             {
 
-                return _db.CorridasTaxiLeg.Any(x =>
-                    x.DataAgenda.HasValue
-                    && x.DataAgenda.Value.Year == ano
-                    && x.DataAgenda.Value.Month == mes
-                );
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "ExisteCorridaNoMesAno", ex);
-                return false;
+            return _db.CorridasTaxiLeg.Any(x =>
+                x.DataAgenda.HasValue
+                && x.DataAgenda.Value.Year == ano
+                && x.DataAgenda.Value.Month == mes
+            );
             }
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
        public CorridasTaxiLegRepository(FrotiXDbContext db) : base(db)
        {
        }
        {
            try
                return _db.CorridasTaxiLeg.Select(i => new SelectListItem()
                    Text = i.DescUnidade,
                    Value = i.CorridaId.ToString(),
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "GetCorridasTaxiLegListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.CorridasTaxiLeg.AsTracking().FirstOrDefault(s =>
                    s.CorridaId == corridasTaxiLeg.CorridaId
                );
                _db.Update(corridasTaxiLeg);
                _db.SaveChanges();
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "Update", ex);
                throw;
            }
        }
        {
            try
                return _db.CorridasTaxiLeg.Any(x =>
                    x.DataAgenda.HasValue
                    && x.DataAgenda.Value.Year == ano
                    && x.DataAgenda.Value.Month == mes
                );
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "ExisteCorridaNoMesAno", ex);
                return false;
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
        public CorridasTaxiLegRepository(FrotiXDbContext db)
            : base(db)
            {
            }
            return _db.CorridasTaxiLeg.Select(i => new SelectListItem()
                Text = i.DescUnidade,
                Value = i.CorridaId.ToString(),
            var objFromDb = _db.CorridasTaxiLeg.FirstOrDefault(s =>
                s.CorridaId == corridasTaxiLeg.CorridaId
            );
            _db.Update(corridasTaxiLeg);
            _db.SaveChanges();
            return _db.CorridasTaxiLeg.Any(x =>
                x.DataAgenda.HasValue
                && x.DataAgenda.Value.Year == ano
                && x.DataAgenda.Value.Month == mes
            );
```
