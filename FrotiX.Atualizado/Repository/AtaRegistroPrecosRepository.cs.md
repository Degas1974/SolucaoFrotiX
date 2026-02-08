# Repository/AtaRegistroPrecosRepository.cs

**Mudanca:** GRANDE | **+19** linhas | **-37** linhas

---

```diff
--- JANEIRO: Repository/AtaRegistroPrecosRepository.cs
+++ ATUAL: Repository/AtaRegistroPrecosRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,61 +6,49 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class AtaRegistroPrecosRepository : Repository<AtaRegistroPrecos>, IAtaRegistroPrecosRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public AtaRegistroPrecosRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetAtaListForDropDown(int status)
-        {
-            try
             {
 
-                return _db.AtaRegistroPrecos
-                    .Where(s => s.Status == Convert.ToBoolean(status))
-                    .Join(_db.Fornecedor,
-                        ataregistroprecos => ataregistroprecos.FornecedorId,
-                        fornecedor => fornecedor.FornecedorId,
-                        (ataregistroprecos, fornecedor) => new { ataregistroprecos, fornecedor })
-                    .OrderByDescending(o => o.ataregistroprecos.AnoAta + "/" + o.ataregistroprecos.NumeroAta + " - " + o.fornecedor.DescricaoFornecedor)
-                    .Select(i => new SelectListItem()
-                    {
-                        Text = i.ataregistroprecos.AnoAta + "/" + i.ataregistroprecos.NumeroAta + " - " + i.fornecedor.DescricaoFornecedor,
-                        Value = i.ataregistroprecos.AtaId.ToString()
-                    });
+            return _db.AtaRegistroPrecos
+            .Where(s => s.Status == Convert.ToBoolean(status))
+
+            .Join(_db.Fornecedor,
+                ataregistroprecos => ataregistroprecos.FornecedorId,
+                fornecedor => fornecedor.FornecedorId,
+                (ataregistroprecos, fornecedor) => new { ataregistroprecos, fornecedor })
+
+            .OrderByDescending(o => o.ataregistroprecos.AnoAta + "/" + o.ataregistroprecos.NumeroAta + " - " + o.fornecedor.DescricaoFornecedor)
+            .Select(i => new SelectListItem()
+                {
+
+                Text = i.ataregistroprecos.AnoAta + "/" + i.ataregistroprecos.NumeroAta + " - " + i.fornecedor.DescricaoFornecedor,
+                Value = i.ataregistroprecos.AtaId.ToString()
+                });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AtaRegistroPrecosRepository.cs", "GetAtaListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(AtaRegistroPrecos ataRegistroPrecos)
-        {
-            try
             {
 
-                var objFromDb = _db.AtaRegistroPrecos.AsTracking().FirstOrDefault(s => s.AtaId == ataRegistroPrecos.AtaId);
+            var objFromDb = _db.AtaRegistroPrecos.FirstOrDefault(s => s.AtaId == ataRegistroPrecos.AtaId);
 
-                _db.Update(ataRegistroPrecos);
+            _db.Update(ataRegistroPrecos);
 
-                _db.SaveChanges();
+            _db.SaveChanges();
+
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AtaRegistroPrecosRepository.cs", "Update", ex);
-                throw;
-            }
+
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
                return _db.AtaRegistroPrecos
                    .Where(s => s.Status == Convert.ToBoolean(status))
                    .Join(_db.Fornecedor,
                        ataregistroprecos => ataregistroprecos.FornecedorId,
                        fornecedor => fornecedor.FornecedorId,
                        (ataregistroprecos, fornecedor) => new { ataregistroprecos, fornecedor })
                    .OrderByDescending(o => o.ataregistroprecos.AnoAta + "/" + o.ataregistroprecos.NumeroAta + " - " + o.fornecedor.DescricaoFornecedor)
                    .Select(i => new SelectListItem()
                    {
                        Text = i.ataregistroprecos.AnoAta + "/" + i.ataregistroprecos.NumeroAta + " - " + i.fornecedor.DescricaoFornecedor,
                        Value = i.ataregistroprecos.AtaId.ToString()
                    });
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosRepository.cs", "GetAtaListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.AtaRegistroPrecos.AsTracking().FirstOrDefault(s => s.AtaId == ataRegistroPrecos.AtaId);
                _db.Update(ataRegistroPrecos);
                _db.SaveChanges();
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosRepository.cs", "Update", ex);
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
            return _db.AtaRegistroPrecos
            .Where(s => s.Status == Convert.ToBoolean(status))
            .Join(_db.Fornecedor,
                ataregistroprecos => ataregistroprecos.FornecedorId,
                fornecedor => fornecedor.FornecedorId,
                (ataregistroprecos, fornecedor) => new { ataregistroprecos, fornecedor })
            .OrderByDescending(o => o.ataregistroprecos.AnoAta + "/" + o.ataregistroprecos.NumeroAta + " - " + o.fornecedor.DescricaoFornecedor)
            .Select(i => new SelectListItem()
                {
                Text = i.ataregistroprecos.AnoAta + "/" + i.ataregistroprecos.NumeroAta + " - " + i.fornecedor.DescricaoFornecedor,
                Value = i.ataregistroprecos.AtaId.ToString()
                });
            var objFromDb = _db.AtaRegistroPrecos.FirstOrDefault(s => s.AtaId == ataRegistroPrecos.AtaId);
            _db.Update(ataRegistroPrecos);
            _db.SaveChanges();
```
