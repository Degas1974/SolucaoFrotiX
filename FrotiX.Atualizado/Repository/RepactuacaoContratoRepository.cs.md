# Repository/RepactuacaoContratoRepository.cs

**Mudanca:** GRANDE | **+46** linhas | **-20** linhas

---

```diff
--- JANEIRO: Repository/RepactuacaoContratoRepository.cs
+++ ATUAL: Repository/RepactuacaoContratoRepository.cs
@@ -1,43 +1,72 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
 using FrotiX.Data;
+using FrotiX.Helpers;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class RepactuacaoContratoRepository : Repository<RepactuacaoContrato>, IRepactuacaoContratoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public RepactuacaoContratoRepository(FrotiXDbContext db) : base(db)
-        {
-            _db = db;
-        }
+            {
+            try
+            {
+                _db = db ?? throw new ArgumentNullException(nameof(db));
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoContratoRepository.cs", "RepactuacaoContratoRepository", erro);
+                throw;
+            }
+            }
 
         public IEnumerable<SelectListItem> GetRepactuacaoContratoListForDropDown()
-        {
-            return _db.RepactuacaoContrato
-                .OrderBy(o => o.Descricao)
-                .Select(i => new SelectListItem()
-                {
-                    Text = i.Descricao,
-                    Value = i.RepactuacaoContratoId.ToString()
-                });
-        }
+            {
+            try
+            {
+
+                return _db.RepactuacaoContrato
+                    .OrderBy(o => o.Descricao)
+                    .Select(i => new SelectListItem()
+                        {
+                        Text = i.Descricao,
+                        Value = i.RepactuacaoContratoId.ToString()
+                        });
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoContratoRepository.cs", "GetRepactuacaoContratoListForDropDown", erro);
+                throw;
+            }
+            }
 
         public new void Update(RepactuacaoContrato RepactuacaoContrato)
-        {
-            var objFromDb = _db.RepactuacaoContrato.AsTracking().FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);
+            {
+            try
+            {
 
-            _db.Update(RepactuacaoContrato);
-            _db.SaveChanges();
+                if (RepactuacaoContrato == null)
+                    throw new ArgumentNullException(nameof(RepactuacaoContrato));
+
+                var objFromDb = _db.RepactuacaoContrato.FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);
+
+                _db.Update(RepactuacaoContrato);
+                _db.SaveChanges();
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoContratoRepository.cs", "Update", erro);
+                throw;
+            }
+            }
 
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
            _db = db;
        }
        {
            return _db.RepactuacaoContrato
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao,
                    Value = i.RepactuacaoContratoId.ToString()
                });
        }
        {
            var objFromDb = _db.RepactuacaoContrato.AsTracking().FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);
            _db.Update(RepactuacaoContrato);
            _db.SaveChanges();
}
```


### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
    {
        {
            {
            try
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoContratoRepository.cs", "RepactuacaoContratoRepository", erro);
                throw;
            }
            }
            {
            try
            {
                return _db.RepactuacaoContrato
                    .OrderBy(o => o.Descricao)
                    .Select(i => new SelectListItem()
                        {
                        Text = i.Descricao,
                        Value = i.RepactuacaoContratoId.ToString()
                        });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoContratoRepository.cs", "GetRepactuacaoContratoListForDropDown", erro);
                throw;
            }
            }
            {
            try
            {
                if (RepactuacaoContrato == null)
                    throw new ArgumentNullException(nameof(RepactuacaoContrato));
                var objFromDb = _db.RepactuacaoContrato.FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);
                _db.Update(RepactuacaoContrato);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoContratoRepository.cs", "Update", erro);
                throw;
            }
            }
```
