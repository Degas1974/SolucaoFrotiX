# Repository/EmpenhoRepository.cs

**Mudanca:** GRANDE | **+18** linhas | **-36** linhas

---

```diff
--- JANEIRO: Repository/EmpenhoRepository.cs
+++ ATUAL: Repository/EmpenhoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,62 +6,45 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class EmpenhoRepository : Repository<Empenho>, IEmpenhoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public EmpenhoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetEmpenhoListForDropDown()
-        {
-            try
             {
 
-                return _db.Empenho
-                    .Join(_db.Contrato,
-                        empenho => empenho.ContratoId,
-                        contrato => contrato.ContratoId,
-                        (empenho, contrato) => new { empenho, contrato })
-                    .OrderBy(o => o.empenho.NotaEmpenho)
-                    .Select(i => new SelectListItem()
-                    {
+            return _db.Empenho
+            .Join(_db.Contrato,
+                empenho => empenho.ContratoId,
+                contrato => contrato.ContratoId,
+                (empenho, contrato) => new { empenho, contrato })
+            .OrderBy(o => o.empenho.NotaEmpenho)
+            .Select(i => new SelectListItem()
+                {
 
-                        Text = i.empenho.NotaEmpenho + " (" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
+                Text = i.empenho.NotaEmpenho + "(" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
 
-                        Value = i.contrato.ContratoId.ToString()
-                    });
+                Value = i.contrato.ContratoId.ToString()
+                });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EmpenhoRepository.cs", "GetEmpenhoListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(Empenho empenho)
-        {
-            try
             {
 
-                var objFromDb = _db.Empenho.AsTracking().FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);
+            var objFromDb = _db.Empenho.FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);
 
-                _db.Update(empenho);
+            _db.Update(empenho);
+            _db.SaveChanges();
 
-                _db.SaveChanges();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("EmpenhoRepository.cs", "Update", ex);
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
                return _db.Empenho
                    .Join(_db.Contrato,
                        empenho => empenho.ContratoId,
                        contrato => contrato.ContratoId,
                        (empenho, contrato) => new { empenho, contrato })
                    .OrderBy(o => o.empenho.NotaEmpenho)
                    .Select(i => new SelectListItem()
                    {
                        Text = i.empenho.NotaEmpenho + " (" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
                        Value = i.contrato.ContratoId.ToString()
                    });
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoRepository.cs", "GetEmpenhoListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.Empenho.AsTracking().FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);
                _db.Update(empenho);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoRepository.cs", "Update", ex);
                throw;
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
            return _db.Empenho
            .Join(_db.Contrato,
                empenho => empenho.ContratoId,
                contrato => contrato.ContratoId,
                (empenho, contrato) => new { empenho, contrato })
            .OrderBy(o => o.empenho.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.empenho.NotaEmpenho + "(" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
                Value = i.contrato.ContratoId.ToString()
                });
            var objFromDb = _db.Empenho.FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);
            _db.Update(empenho);
            _db.SaveChanges();
```
