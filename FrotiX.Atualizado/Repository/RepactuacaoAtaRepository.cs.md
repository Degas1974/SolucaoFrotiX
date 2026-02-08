# Repository/RepactuacaoAtaRepository.cs

**Mudanca:** GRANDE | **+46** linhas | **-20** linhas

---

```diff
--- JANEIRO: Repository/RepactuacaoAtaRepository.cs
+++ ATUAL: Repository/RepactuacaoAtaRepository.cs
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
     public class RepactuacaoAtaRepository : Repository<RepactuacaoAta>, IRepactuacaoAtaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public RepactuacaoAtaRepository(FrotiXDbContext db) : base(db)
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
+                Alerta.TratamentoErroComLinha("RepactuacaoAtaRepository.cs", "RepactuacaoAtaRepository", erro);
+                throw;
+            }
+            }
 
         public IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown()
-        {
-            return _db.RepactuacaoAta
-                .OrderBy(o => o.Descricao)
-                .Select(i => new SelectListItem()
-                {
-                    Text = i.Descricao,
-                    Value = i.RepactuacaoAtaId.ToString()
-                });
-        }
+            {
+            try
+            {
+
+                return _db.RepactuacaoAta
+                    .OrderBy(o => o.Descricao)
+                    .Select(i => new SelectListItem()
+                        {
+                        Text = i.Descricao,
+                        Value = i.RepactuacaoAtaId.ToString()
+                        });
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoAtaRepository.cs", "GetRepactuacaoAtaListForDropDown", erro);
+                throw;
+            }
+            }
 
         public new void Update(RepactuacaoAta repactuacaoitemveiculoata)
-        {
-            var objFromDb = _db.RepactuacaoAta.AsTracking().FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);
+            {
+            try
+            {
 
-            _db.Update(repactuacaoitemveiculoata);
-            _db.SaveChanges();
+                if (repactuacaoitemveiculoata == null)
+                    throw new ArgumentNullException(nameof(repactuacaoitemveiculoata));
+
+                var objFromDb = _db.RepactuacaoAta.FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);
+
+                _db.Update(repactuacaoitemveiculoata);
+                _db.SaveChanges();
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoAtaRepository.cs", "Update", erro);
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
            return _db.RepactuacaoAta
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao,
                    Value = i.RepactuacaoAtaId.ToString()
                });
        }
        {
            var objFromDb = _db.RepactuacaoAta.AsTracking().FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);
            _db.Update(repactuacaoitemveiculoata);
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
                Alerta.TratamentoErroComLinha("RepactuacaoAtaRepository.cs", "RepactuacaoAtaRepository", erro);
                throw;
            }
            }
            {
            try
            {
                return _db.RepactuacaoAta
                    .OrderBy(o => o.Descricao)
                    .Select(i => new SelectListItem()
                        {
                        Text = i.Descricao,
                        Value = i.RepactuacaoAtaId.ToString()
                        });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoAtaRepository.cs", "GetRepactuacaoAtaListForDropDown", erro);
                throw;
            }
            }
            {
            try
            {
                if (repactuacaoitemveiculoata == null)
                    throw new ArgumentNullException(nameof(repactuacaoitemveiculoata));
                var objFromDb = _db.RepactuacaoAta.FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);
                _db.Update(repactuacaoitemveiculoata);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoAtaRepository.cs", "Update", erro);
                throw;
            }
            }
```
