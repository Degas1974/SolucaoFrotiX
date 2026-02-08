# Repository/CustoMensalItensContratoRepository.cs

**Mudanca:** GRANDE | **+13** linhas | **-34** linhas

---

```diff
--- JANEIRO: Repository/CustoMensalItensContratoRepository.cs
+++ ATUAL: Repository/CustoMensalItensContratoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,59 +6,37 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
-
+    {
     public class CustoMensalItensContratoRepository : Repository<CustoMensalItensContrato>, ICustoMensalItensContratoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public CustoMensalItensContratoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetCustoMensalItensContratoListForDropDown()
-        {
-            try
             {
-
-                return _db.CustoMensalItensContrato
-                    .OrderBy(o => o.Ano)
-                    .Select(i => new SelectListItem()
+            return _db.CustoMensalItensContrato
+                .OrderBy(o => o.Ano)
+                .Select(i => new SelectListItem()
                     {
-                        Text = i.Ano.ToString(),
-                        Value = i.NotaFiscalId.ToString()
+                    Text = i.Ano.ToString(),
+                    Value = i.NotaFiscalId.ToString()
                     });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CustoMensalItensContratoRepository.cs", "GetCustoMensalItensContratoListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(CustoMensalItensContrato customensalitenscontrato)
-        {
-            try
             {
+            var objFromDb = _db.CustoMensalItensContrato.FirstOrDefault(s => (s.NotaFiscalId == customensalitenscontrato.NotaFiscalId) && (s.Ano == customensalitenscontrato.Ano) && (s.Mes == customensalitenscontrato.Mes));
 
-                var objFromDb = _db.CustoMensalItensContrato.AsTracking().FirstOrDefault(s =>
-                    (s.NotaFiscalId == customensalitenscontrato.NotaFiscalId) &&
-                    (s.Ano == customensalitenscontrato.Ano) &&
-                    (s.Mes == customensalitenscontrato.Mes));
+            _db.Update(customensalitenscontrato);
+            _db.SaveChanges();
 
-                _db.Update(customensalitenscontrato);
+            }
 
-                _db.SaveChanges();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CustoMensalItensContratoRepository.cs", "Update", ex);
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
                return _db.CustoMensalItensContrato
                    .OrderBy(o => o.Ano)
                    .Select(i => new SelectListItem()
                        Text = i.Ano.ToString(),
                        Value = i.NotaFiscalId.ToString()
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CustoMensalItensContratoRepository.cs", "GetCustoMensalItensContratoListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.CustoMensalItensContrato.AsTracking().FirstOrDefault(s =>
                    (s.NotaFiscalId == customensalitenscontrato.NotaFiscalId) &&
                    (s.Ano == customensalitenscontrato.Ano) &&
                    (s.Mes == customensalitenscontrato.Mes));
                _db.Update(customensalitenscontrato);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CustoMensalItensContratoRepository.cs", "Update", ex);
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
            return _db.CustoMensalItensContrato
                .OrderBy(o => o.Ano)
                .Select(i => new SelectListItem()
                    Text = i.Ano.ToString(),
                    Value = i.NotaFiscalId.ToString()
            var objFromDb = _db.CustoMensalItensContrato.FirstOrDefault(s => (s.NotaFiscalId == customensalitenscontrato.NotaFiscalId) && (s.Ano == customensalitenscontrato.Ano) && (s.Mes == customensalitenscontrato.Mes));
            _db.Update(customensalitenscontrato);
            _db.SaveChanges();
            }
```
