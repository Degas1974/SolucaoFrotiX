# Repository/RepactuacaoVeiculoRepository.cs

**Mudanca:** GRANDE | **+44** linhas | **-18** linhas

---

```diff
--- JANEIRO: Repository/RepactuacaoVeiculoRepository.cs
+++ ATUAL: Repository/RepactuacaoVeiculoRepository.cs
@@ -1,49 +1,79 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
 using FrotiX.Data;
+using FrotiX.Helpers;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
 {
+
     public class RepactuacaoVeiculoRepository : Repository<RepactuacaoVeiculo>, IRepactuacaoVeiculoRepository
     {
         private new readonly FrotiXDbContext _db;
 
         public RepactuacaoVeiculoRepository(FrotiXDbContext db) : base(db)
         {
-            _db = db;
+            try
+            {
+                _db = db ?? throw new ArgumentNullException(nameof(db));
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoVeiculoRepository.cs", "RepactuacaoVeiculoRepository", erro);
+                throw;
+            }
         }
 
         public IEnumerable<SelectListItem> GetRepactuacaoVeiculoListForDropDown()
         {
-            return _db.RepactuacaoVeiculo
-                .OrderBy(o => o.RepactuacaoVeiculoId)
-                .Select(i => new SelectListItem()
-                {
-                    Text = i.Valor.ToString(),
-                    Value = i.RepactuacaoVeiculoId.ToString()
-                });
+            try
+            {
+
+                return _db.RepactuacaoVeiculo
+                    .OrderBy(o => o.RepactuacaoVeiculoId)
+                    .Select(i => new SelectListItem()
+                    {
+                        Text = i.Valor.ToString(),
+                        Value = i.RepactuacaoVeiculoId.ToString()
+                    });
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RepactuacaoVeiculoRepository.cs", "GetRepactuacaoVeiculoListForDropDown", erro);
+                throw;
+            }
         }
 
         public new void Update(RepactuacaoVeiculo repactuacaoVeiculo)
         {
-            var objFromDb = _db.RepactuacaoVeiculo.AsTracking().FirstOrDefault(s =>
-                s.RepactuacaoVeiculoId == repactuacaoVeiculo.RepactuacaoVeiculoId
-            );
+            try
+            {
 
-            if (objFromDb != null)
+                if (repactuacaoVeiculo == null)
+                    throw new ArgumentNullException(nameof(repactuacaoVeiculo));
+
+                var objFromDb = _db.RepactuacaoVeiculo.FirstOrDefault(s =>
+                    s.RepactuacaoVeiculoId == repactuacaoVeiculo.RepactuacaoVeiculoId
+                );
+
+                if (objFromDb != null)
+                {
+                    objFromDb.Valor = repactuacaoVeiculo.Valor;
+                    objFromDb.Observacao = repactuacaoVeiculo.Observacao;
+                    objFromDb.VeiculoId = repactuacaoVeiculo.VeiculoId;
+                    objFromDb.RepactuacaoContratoId = repactuacaoVeiculo.RepactuacaoContratoId;
+                }
+
+                _db.SaveChanges();
+            }
+            catch (Exception erro)
             {
-                objFromDb.Valor = repactuacaoVeiculo.Valor;
-                objFromDb.Observacao = repactuacaoVeiculo.Observacao;
-                objFromDb.VeiculoId = repactuacaoVeiculo.VeiculoId;
-                objFromDb.RepactuacaoContratoId = repactuacaoVeiculo.RepactuacaoContratoId;
+                Alerta.TratamentoErroComLinha("RepactuacaoVeiculoRepository.cs", "Update", erro);
+                throw;
             }
-
-            _db.SaveChanges();
         }
     }
 }
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            _db = db;
            return _db.RepactuacaoVeiculo
                .OrderBy(o => o.RepactuacaoVeiculoId)
                .Select(i => new SelectListItem()
                {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoVeiculoId.ToString()
                });
            var objFromDb = _db.RepactuacaoVeiculo.AsTracking().FirstOrDefault(s =>
                s.RepactuacaoVeiculoId == repactuacaoVeiculo.RepactuacaoVeiculoId
            );
            if (objFromDb != null)
                objFromDb.Valor = repactuacaoVeiculo.Valor;
                objFromDb.Observacao = repactuacaoVeiculo.Observacao;
                objFromDb.VeiculoId = repactuacaoVeiculo.VeiculoId;
                objFromDb.RepactuacaoContratoId = repactuacaoVeiculo.RepactuacaoContratoId;
            _db.SaveChanges();
```


### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
            try
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoVeiculoRepository.cs", "RepactuacaoVeiculoRepository", erro);
                throw;
            }
            try
            {
                return _db.RepactuacaoVeiculo
                    .OrderBy(o => o.RepactuacaoVeiculoId)
                    .Select(i => new SelectListItem()
                    {
                        Text = i.Valor.ToString(),
                        Value = i.RepactuacaoVeiculoId.ToString()
                    });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoVeiculoRepository.cs", "GetRepactuacaoVeiculoListForDropDown", erro);
                throw;
            }
            try
            {
                if (repactuacaoVeiculo == null)
                    throw new ArgumentNullException(nameof(repactuacaoVeiculo));
                var objFromDb = _db.RepactuacaoVeiculo.FirstOrDefault(s =>
                    s.RepactuacaoVeiculoId == repactuacaoVeiculo.RepactuacaoVeiculoId
                );
                if (objFromDb != null)
                {
                    objFromDb.Valor = repactuacaoVeiculo.Valor;
                    objFromDb.Observacao = repactuacaoVeiculo.Observacao;
                    objFromDb.VeiculoId = repactuacaoVeiculo.VeiculoId;
                    objFromDb.RepactuacaoContratoId = repactuacaoVeiculo.RepactuacaoContratoId;
                }
                _db.SaveChanges();
            }
            catch (Exception erro)
                Alerta.TratamentoErroComLinha("RepactuacaoVeiculoRepository.cs", "Update", erro);
                throw;
```
