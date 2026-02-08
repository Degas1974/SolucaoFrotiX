# Repository/RegistroCupomAbastecimentoRepository.cs

**Mudanca:** GRANDE | **+46** linhas | **-20** linhas

---

```diff
--- JANEIRO: Repository/RegistroCupomAbastecimentoRepository.cs
+++ ATUAL: Repository/RegistroCupomAbastecimentoRepository.cs
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
     public class RegistroCupomAbastecimentoRepository : Repository<RegistroCupomAbastecimento>, IRegistroCupomAbastecimentoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public RegistroCupomAbastecimentoRepository(FrotiXDbContext db) : base(db)
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
+                Alerta.TratamentoErroComLinha("RegistroCupomAbastecimentoRepository.cs", "RegistroCupomAbastecimentoRepository", erro);
+                throw;
+            }
+            }
 
         public IEnumerable<SelectListItem> GetRegistroCupomAbastecimentoListForDropDown()
-        {
-            return _db.RegistroCupomAbastecimento
-                .OrderBy(o => o.DataRegistro)
-                .Select(i => new SelectListItem()
-                {
-                    Text = i.RegistroPDF,
-                    Value = i.RegistroCupomId.ToString()
-                });
-        }
+            {
+            try
+            {
+
+                return _db.RegistroCupomAbastecimento
+                    .OrderBy(o => o.DataRegistro)
+                    .Select(i => new SelectListItem()
+                        {
+                        Text = i.RegistroPDF,
+                        Value = i.RegistroCupomId.ToString()
+                        });
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RegistroCupomAbastecimentoRepository.cs", "GetRegistroCupomAbastecimentoListForDropDown", erro);
+                throw;
+            }
+            }
 
         public new void Update(RegistroCupomAbastecimento registroCupomAbastecimento)
-        {
-            var objFromDb = _db.RegistroCupomAbastecimento.AsTracking().FirstOrDefault(s => s.RegistroCupomId == registroCupomAbastecimento.RegistroCupomId);
+            {
+            try
+            {
 
-            _db.Update(registroCupomAbastecimento);
-            _db.SaveChanges();
+                if (registroCupomAbastecimento == null)
+                    throw new ArgumentNullException(nameof(registroCupomAbastecimento));
+
+                var objFromDb = _db.RegistroCupomAbastecimento.FirstOrDefault(s => s.RegistroCupomId == registroCupomAbastecimento.RegistroCupomId);
+
+                _db.Update(registroCupomAbastecimento);
+                _db.SaveChanges();
+            }
+            catch (Exception erro)
+            {
+                Alerta.TratamentoErroComLinha("RegistroCupomAbastecimentoRepository.cs", "Update", erro);
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
            return _db.RegistroCupomAbastecimento
                .OrderBy(o => o.DataRegistro)
                .Select(i => new SelectListItem()
                {
                    Text = i.RegistroPDF,
                    Value = i.RegistroCupomId.ToString()
                });
        }
        {
            var objFromDb = _db.RegistroCupomAbastecimento.AsTracking().FirstOrDefault(s => s.RegistroCupomId == registroCupomAbastecimento.RegistroCupomId);
            _db.Update(registroCupomAbastecimento);
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
                Alerta.TratamentoErroComLinha("RegistroCupomAbastecimentoRepository.cs", "RegistroCupomAbastecimentoRepository", erro);
                throw;
            }
            }
            {
            try
            {
                return _db.RegistroCupomAbastecimento
                    .OrderBy(o => o.DataRegistro)
                    .Select(i => new SelectListItem()
                        {
                        Text = i.RegistroPDF,
                        Value = i.RegistroCupomId.ToString()
                        });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RegistroCupomAbastecimentoRepository.cs", "GetRegistroCupomAbastecimentoListForDropDown", erro);
                throw;
            }
            }
            {
            try
            {
                if (registroCupomAbastecimento == null)
                    throw new ArgumentNullException(nameof(registroCupomAbastecimento));
                var objFromDb = _db.RegistroCupomAbastecimento.FirstOrDefault(s => s.RegistroCupomId == registroCupomAbastecimento.RegistroCupomId);
                _db.Update(registroCupomAbastecimento);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RegistroCupomAbastecimentoRepository.cs", "Update", erro);
                throw;
            }
            }
```
