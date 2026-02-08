# Repository/ContratoRepository.cs

**Mudanca:** GRANDE | **+19** linhas | **-30** linhas

---

```diff
--- JANEIRO: Repository/ContratoRepository.cs
+++ ATUAL: Repository/ContratoRepository.cs
@@ -1,54 +1,43 @@
-using Microsoft.EntityFrameworkCore;
-using System;
-using System.Collections.Generic;
 using System.Linq;
-using System.Threading.Tasks;
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
+using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class ContratoRepository : Repository<Contrato>, IContratoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
-        public ContratoRepository(FrotiXDbContext db) : base(db)
-        {
+        public ContratoRepository(FrotiXDbContext db)
+            : base(db)
+            {
             _db = db;
-        }
+            }
 
         public IQueryable<SelectListItem> GetDropDown(string? tipoContrato = null)
-        {
-            try
             {
 
-                var temTipo = !string.IsNullOrWhiteSpace(tipoContrato);
+            var temTipo = !string.IsNullOrWhiteSpace(tipoContrato);
 
-                return _db.Set<Contrato>()
-                    .AsNoTracking()
-                    .Where(c => c.Status && (!temTipo || c.TipoContrato == tipoContrato))
+            return _db.Set<Contrato>()
+                .AsNoTracking()
+                .Where(c => c.Status && (!temTipo || c.TipoContrato == tipoContrato))
 
-                    .OrderByDescending(c => c.AnoContrato)
-                    .ThenByDescending(c => c.NumeroContrato)
-                    .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor)
-                    .Select(c => new SelectListItem
+                .OrderByDescending(c => c.AnoContrato)
+                .ThenByDescending(c => c.NumeroContrato)
+                .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor)
+                .Select(c => new SelectListItem
                     {
-                        Value = c.ContratoId.ToString(),
+                    Value = c.ContratoId.ToString(),
 
-                        Text = temTipo
-                            ? $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}"
-                            : $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor} ({c.TipoContrato})"
+                    Text = temTipo
+                        ? $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}"
+                        : $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor} ({c.TipoContrato})",
                     });
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("ContratoRepository.cs", "GetDropDown", ex);
-                return Enumerable.Empty<SelectListItem>().AsQueryable();
             }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrotiX.Helpers;
{
    {
        public ContratoRepository(FrotiXDbContext db) : base(db)
        {
        }
        {
            try
                var temTipo = !string.IsNullOrWhiteSpace(tipoContrato);
                return _db.Set<Contrato>()
                    .AsNoTracking()
                    .Where(c => c.Status && (!temTipo || c.TipoContrato == tipoContrato))
                    .OrderByDescending(c => c.AnoContrato)
                    .ThenByDescending(c => c.NumeroContrato)
                    .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor)
                    .Select(c => new SelectListItem
                        Value = c.ContratoId.ToString(),
                        Text = temTipo
                            ? $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}"
                            : $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor} ({c.TipoContrato})"
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ContratoRepository.cs", "GetDropDown", ex);
                return Enumerable.Empty<SelectListItem>().AsQueryable();
}
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
    {
        {
        public ContratoRepository(FrotiXDbContext db)
            : base(db)
            {
            }
            var temTipo = !string.IsNullOrWhiteSpace(tipoContrato);
            return _db.Set<Contrato>()
                .AsNoTracking()
                .Where(c => c.Status && (!temTipo || c.TipoContrato == tipoContrato))
                .OrderByDescending(c => c.AnoContrato)
                .ThenByDescending(c => c.NumeroContrato)
                .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor)
                .Select(c => new SelectListItem
                    Value = c.ContratoId.ToString(),
                    Text = temTipo
                        ? $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}"
                        : $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor} ({c.TipoContrato})",
```
