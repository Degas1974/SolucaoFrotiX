# Repository/RepactuacaoTerceirizacaoRepository.cs

**Mudanca:** GRANDE | **+46** linhas | **-20** linhas

---

```diff
--- JANEIRO: Repository/RepactuacaoTerceirizacaoRepository.cs
+++ ATUAL: Repository/RepactuacaoTerceirizacaoRepository.cs
@@ -1,42 +1,71 @@
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
     public class RepactuacaoTerceirizacaoRepository : Repository<RepactuacaoTerceirizacao>, IRepactuacaoTerceirizacaoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public RepactuacaoTerceirizacaoRepository(FrotiXDbContext db) : base(db)
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
+                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "RepactuacaoTerceirizacaoRepository", erro);
+                throw;
+            }
+            }
 
         public IEnumerable<SelectListItem> GetRepactuacaoTerceirizacaoListForDropDown()
-        {
-            return _db.RepactuacaoTerceirizacao
-                .Select(i => new SelectListItem()
-                {
-                    Text = i.ValorEncarregado.ToString(),
-                    Value = i.RepactuacaoContratoId.ToString()
-                });
-        }
+            {
+            try
+            {
 
-        public new void Update(RepactuacaoTerceirizacao RepactuacaoTerceirizacao)
-        {
-            var objFromDb = _db.RepactuacaoTerceirizacao.AsTracking().FirstOrDefault(s => s.RepactuacaoTerceirizacaoId == RepactuacaoTerceirizacao.RepactuacaoTerceirizacaoId);
+                return _db.RepactuacaoTerceirizacao
+                    .Select(i => new SelectListItem()
+                        {
+                        Text = i.ValorEncarregado.ToString(),
+                        Value = i.RepactuacaoContratoId.ToString()
+                        });
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "GetRepactuacaoTerceirizacaoListForDropDown", erro);
+                throw;
+            }
+            }
 
-            _db.Update(RepactuacaoTerceirizacao);
-            _db.SaveChanges();
+        public new void Update(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
+            {
+            try
+            {
+
+                if (repactuacaoTerceirizacao == null)
+                    throw new ArgumentNullException(nameof(repactuacaoTerceirizacao));
+
+                var objFromDb = _db.RepactuacaoTerceirizacao.FirstOrDefault(s => s.RepactuacaoTerceirizacaoId == repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId);
+
+                _db.Update(repactuacaoTerceirizacao);
+                _db.SaveChanges();
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "Update", erro);
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
            return _db.RepactuacaoTerceirizacao
                .Select(i => new SelectListItem()
                {
                    Text = i.ValorEncarregado.ToString(),
                    Value = i.RepactuacaoContratoId.ToString()
                });
        }
        public new void Update(RepactuacaoTerceirizacao RepactuacaoTerceirizacao)
        {
            var objFromDb = _db.RepactuacaoTerceirizacao.AsTracking().FirstOrDefault(s => s.RepactuacaoTerceirizacaoId == RepactuacaoTerceirizacao.RepactuacaoTerceirizacaoId);
            _db.Update(RepactuacaoTerceirizacao);
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
                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "RepactuacaoTerceirizacaoRepository", erro);
                throw;
            }
            }
            {
            try
            {
                return _db.RepactuacaoTerceirizacao
                    .Select(i => new SelectListItem()
                        {
                        Text = i.ValorEncarregado.ToString(),
                        Value = i.RepactuacaoContratoId.ToString()
                        });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "GetRepactuacaoTerceirizacaoListForDropDown", erro);
                throw;
            }
            }
        public new void Update(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
            {
            try
            {
                if (repactuacaoTerceirizacao == null)
                    throw new ArgumentNullException(nameof(repactuacaoTerceirizacao));
                var objFromDb = _db.RepactuacaoTerceirizacao.FirstOrDefault(s => s.RepactuacaoTerceirizacaoId == repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId);
                _db.Update(repactuacaoTerceirizacao);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "Update", erro);
                throw;
            }
            }
```
