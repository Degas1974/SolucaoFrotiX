# Repository/EmpenhoMultaRepository.cs

**Mudanca:** GRANDE | **+18** linhas | **-36** linhas

---

```diff
--- JANEIRO: Repository/EmpenhoMultaRepository.cs
+++ ATUAL: Repository/EmpenhoMultaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,61 +6,44 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class EmpenhoMultaRepository : Repository<EmpenhoMulta>, IEmpenhoMultaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public EmpenhoMultaRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetEmpenhoMultaListForDropDown()
-        {
-            try
             {
 
-                return _db.EmpenhoMulta
-                    .Join(_db.OrgaoAutuante,
-                        empenhomulta => empenhomulta.OrgaoAutuanteId,
-                        orgaoautuante => orgaoautuante.OrgaoAutuanteId,
-                        (empenhomulta, orgaoautuante) => new { empenhomulta, orgaoautuante })
-                    .OrderBy(o => o.empenhomulta.NotaEmpenho)
-                    .Select(i => new SelectListItem()
-                    {
+            return _db.EmpenhoMulta
+            .Join(_db.OrgaoAutuante,
+                empenhomulta => empenhomulta.OrgaoAutuanteId,
+                orgaoautuante => orgaoautuante.OrgaoAutuanteId,
+                (empenhomulta, orgaoautuante) => new { empenhomulta, orgaoautuante })
+            .OrderBy(o => o.empenhomulta.NotaEmpenho)
+            .Select(i => new SelectListItem()
+                {
 
-                        Text = i.empenhomulta.NotaEmpenho + " (" + i.orgaoautuante.Sigla + "/" + i.orgaoautuante.Nome + ")",
-                        Value = i.empenhomulta.EmpenhoMultaId.ToString()
-                    });
+                Text = i.empenhomulta.NotaEmpenho + "(" + i.orgaoautuante.Sigla + "/" + i.orgaoautuante.Nome + ")",
+                Value = i.empenhomulta.EmpenhoMultaId.ToString()
+                });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EmpenhoMultaRepository.cs", "GetEmpenhoMultaListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(EmpenhoMulta empenhomulta)
-        {
-            try
             {
 
-                var objFromDb = _db.EmpenhoMulta.AsTracking().FirstOrDefault(s => s.EmpenhoMultaId == empenhomulta.EmpenhoMultaId);
+            var objFromDb = _db.EmpenhoMulta.FirstOrDefault(s => s.EmpenhoMultaId == empenhomulta.EmpenhoMultaId);
 
-                _db.Update(empenhomulta);
+            _db.Update(empenhomulta);
+            _db.SaveChanges();
 
-                _db.SaveChanges();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EmpenhoMultaRepository.cs", "Update", ex);
-                throw;
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
        {
        }
        {
            try
                return _db.EmpenhoMulta
                    .Join(_db.OrgaoAutuante,
                        empenhomulta => empenhomulta.OrgaoAutuanteId,
                        orgaoautuante => orgaoautuante.OrgaoAutuanteId,
                        (empenhomulta, orgaoautuante) => new { empenhomulta, orgaoautuante })
                    .OrderBy(o => o.empenhomulta.NotaEmpenho)
                    .Select(i => new SelectListItem()
                    {
                        Text = i.empenhomulta.NotaEmpenho + " (" + i.orgaoautuante.Sigla + "/" + i.orgaoautuante.Nome + ")",
                        Value = i.empenhomulta.EmpenhoMultaId.ToString()
                    });
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoMultaRepository.cs", "GetEmpenhoMultaListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.EmpenhoMulta.AsTracking().FirstOrDefault(s => s.EmpenhoMultaId == empenhomulta.EmpenhoMultaId);
                _db.Update(empenhomulta);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoMultaRepository.cs", "Update", ex);
                throw;
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
            return _db.EmpenhoMulta
            .Join(_db.OrgaoAutuante,
                empenhomulta => empenhomulta.OrgaoAutuanteId,
                orgaoautuante => orgaoautuante.OrgaoAutuanteId,
                (empenhomulta, orgaoautuante) => new { empenhomulta, orgaoautuante })
            .OrderBy(o => o.empenhomulta.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.empenhomulta.NotaEmpenho + "(" + i.orgaoautuante.Sigla + "/" + i.orgaoautuante.Nome + ")",
                Value = i.empenhomulta.EmpenhoMultaId.ToString()
                });
            var objFromDb = _db.EmpenhoMulta.FirstOrDefault(s => s.EmpenhoMultaId == empenhomulta.EmpenhoMultaId);
            _db.Update(empenhomulta);
            _db.SaveChanges();
```
