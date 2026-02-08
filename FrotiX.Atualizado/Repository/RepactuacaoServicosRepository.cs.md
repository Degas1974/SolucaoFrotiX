# Repository/RepactuacaoServicosRepository.cs

**Mudanca:** GRANDE | **+46** linhas | **-20** linhas

---

```diff
--- JANEIRO: Repository/RepactuacaoServicosRepository.cs
+++ ATUAL: Repository/RepactuacaoServicosRepository.cs
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
     public class RepactuacaoServicosRepository : Repository<RepactuacaoServicos>, IRepactuacaoServicosRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public RepactuacaoServicosRepository(FrotiXDbContext db) : base(db)
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
+                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "RepactuacaoServicosRepository", erro);
+                throw;
+            }
+            }
 
         public IEnumerable<SelectListItem> GetRepactuacaoServicosListForDropDown()
-        {
-            return _db.RepactuacaoServicos
-                .Select(i => new SelectListItem()
-                {
-                    Text = i.Valor.ToString(),
-                    Value = i.RepactuacaoContratoId.ToString()
-                });
-        }
+            {
+            try
+            {
 
-        public new void Update(RepactuacaoServicos RepactuacaoServicos)
-        {
-            var objFromDb = _db.RepactuacaoServicos.AsTracking().FirstOrDefault(s => s.RepactuacaoServicoId == RepactuacaoServicos.RepactuacaoServicoId);
+                return _db.RepactuacaoServicos
+                    .Select(i => new SelectListItem()
+                        {
+                        Text = i.Valor.ToString(),
+                        Value = i.RepactuacaoContratoId.ToString()
+                        });
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "GetRepactuacaoServicosListForDropDown", erro);
+                throw;
+            }
+            }
 
-            _db.Update(RepactuacaoServicos);
-            _db.SaveChanges();
+        public new void Update(RepactuacaoServicos repactuacaoServicos)
+            {
+            try
+            {
+
+                if (repactuacaoServicos == null)
+                    throw new ArgumentNullException(nameof(repactuacaoServicos));
+
+                var objFromDb = _db.RepactuacaoServicos.FirstOrDefault(s => s.RepactuacaoServicoId == repactuacaoServicos.RepactuacaoServicoId);
+
+                _db.Update(repactuacaoServicos);
+                _db.SaveChanges();
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "Update", erro);
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
            return _db.RepactuacaoServicos
                .Select(i => new SelectListItem()
                {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoContratoId.ToString()
                });
        }
        public new void Update(RepactuacaoServicos RepactuacaoServicos)
        {
            var objFromDb = _db.RepactuacaoServicos.AsTracking().FirstOrDefault(s => s.RepactuacaoServicoId == RepactuacaoServicos.RepactuacaoServicoId);
            _db.Update(RepactuacaoServicos);
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
                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "RepactuacaoServicosRepository", erro);
                throw;
            }
            }
            {
            try
            {
                return _db.RepactuacaoServicos
                    .Select(i => new SelectListItem()
                        {
                        Text = i.Valor.ToString(),
                        Value = i.RepactuacaoContratoId.ToString()
                        });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "GetRepactuacaoServicosListForDropDown", erro);
                throw;
            }
            }
        public new void Update(RepactuacaoServicos repactuacaoServicos)
            {
            try
            {
                if (repactuacaoServicos == null)
                    throw new ArgumentNullException(nameof(repactuacaoServicos));
                var objFromDb = _db.RepactuacaoServicos.FirstOrDefault(s => s.RepactuacaoServicoId == repactuacaoServicos.RepactuacaoServicoId);
                _db.Update(repactuacaoServicos);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "Update", erro);
                throw;
            }
            }
```
