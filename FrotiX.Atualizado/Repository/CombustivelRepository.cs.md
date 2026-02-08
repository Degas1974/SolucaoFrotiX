# Repository/CombustivelRepository.cs

**Mudanca:** GRANDE | **+13** linhas | **-31** linhas

---

```diff
--- JANEIRO: Repository/CombustivelRepository.cs
+++ ATUAL: Repository/CombustivelRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,57 +6,41 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class CombustivelRepository : Repository<Combustivel>, ICombustivelRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public CombustivelRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetCombustivelListForDropDown()
-        {
-            try
             {
 
-                return _db.Combustivel
-                    .Where(e => e.Status)
-                    .OrderBy(o => o.Descricao)
-                    .Select(i => new SelectListItem()
+            return _db.Combustivel
+                .Where(e => e.Status)
+                .OrderBy(o => o.Descricao)
+                .Select(i => new SelectListItem()
                     {
-                        Text = i.Descricao,
-                        Value = i.CombustivelId.ToString()
+                    Text = i.Descricao,
+                    Value = i.CombustivelId.ToString()
                     });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CombustivelRepository.cs", "GetCombustivelListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(Combustivel combustivel)
-        {
-            try
             {
 
-                var objFromDb = _db.Combustivel.AsTracking().FirstOrDefault(s => s.CombustivelId == combustivel.CombustivelId);
+            var objFromDb = _db.Combustivel.FirstOrDefault(s => s.CombustivelId == combustivel.CombustivelId);
 
-                _db.Update(combustivel);
+            _db.Update(combustivel);
+            _db.SaveChanges();
 
-                _db.SaveChanges();
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("CombustivelRepository.cs", "Update", ex);
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
                return _db.Combustivel
                    .Where(e => e.Status)
                    .OrderBy(o => o.Descricao)
                    .Select(i => new SelectListItem()
                        Text = i.Descricao,
                        Value = i.CombustivelId.ToString()
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CombustivelRepository.cs", "GetCombustivelListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.Combustivel.AsTracking().FirstOrDefault(s => s.CombustivelId == combustivel.CombustivelId);
                _db.Update(combustivel);
                _db.SaveChanges();
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CombustivelRepository.cs", "Update", ex);
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
            return _db.Combustivel
                .Where(e => e.Status)
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    Text = i.Descricao,
                    Value = i.CombustivelId.ToString()
            var objFromDb = _db.Combustivel.FirstOrDefault(s => s.CombustivelId == combustivel.CombustivelId);
            _db.Update(combustivel);
            _db.SaveChanges();
```
