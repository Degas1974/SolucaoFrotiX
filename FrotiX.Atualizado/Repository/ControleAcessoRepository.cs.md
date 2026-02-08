# Repository/ControleAcessoRepository.cs

**Mudanca:** GRANDE | **+13** linhas | **-31** linhas

---

```diff
--- JANEIRO: Repository/ControleAcessoRepository.cs
+++ ATUAL: Repository/ControleAcessoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,55 +6,39 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class ControleAcessoRepository : Repository<ControleAcesso>, IControleAcessoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public ControleAcessoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetControleAcessoListForDropDown()
-        {
-            try
             {
 
-                return _db.ControleAcesso
-                    .Select(i => new SelectListItem()
-                    {
-                        Text = i.RecursoId.ToString(),
-                        Value = i.UsuarioId.ToString()
-                    });
+            return _db.ControleAcesso
+            .Select(i => new SelectListItem()
+                {
+                Text = i.RecursoId.ToString(),
+                Value = i.UsuarioId.ToString()
+                });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("ControleAcessoRepository.cs", "GetControleAcessoListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(ControleAcesso controleAcesso)
-        {
-            try
             {
 
-                var objFromDb = _db.ControleAcesso.AsTracking().FirstOrDefault(s => s.RecursoId == controleAcesso.RecursoId);
+            var objFromDb = _db.ControleAcesso.FirstOrDefault(s => s.RecursoId == controleAcesso.RecursoId);
 
-                _db.Update(controleAcesso);
+            _db.Update(controleAcesso);
+            _db.SaveChanges();
 
-                _db.SaveChanges();
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("ControleAcessoRepository.cs", "Update", ex);
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
                return _db.ControleAcesso
                    .Select(i => new SelectListItem()
                    {
                        Text = i.RecursoId.ToString(),
                        Value = i.UsuarioId.ToString()
                    });
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ControleAcessoRepository.cs", "GetControleAcessoListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.ControleAcesso.AsTracking().FirstOrDefault(s => s.RecursoId == controleAcesso.RecursoId);
                _db.Update(controleAcesso);
                _db.SaveChanges();
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ControleAcessoRepository.cs", "Update", ex);
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
            return _db.ControleAcesso
            .Select(i => new SelectListItem()
                {
                Text = i.RecursoId.ToString(),
                Value = i.UsuarioId.ToString()
                });
            var objFromDb = _db.ControleAcesso.FirstOrDefault(s => s.RecursoId == controleAcesso.RecursoId);
            _db.Update(controleAcesso);
            _db.SaveChanges();
```
