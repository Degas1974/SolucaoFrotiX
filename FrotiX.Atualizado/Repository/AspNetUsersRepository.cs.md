# Repository/AspNetUsersRepository.cs

**Mudanca:** GRANDE | **+14** linhas | **-32** linhas

---

```diff
--- JANEIRO: Repository/AspNetUsersRepository.cs
+++ ATUAL: Repository/AspNetUsersRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,57 +6,38 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
-{
-
+    {
     public class AspNetUsersRepository : Repository<AspNetUsers>, IAspNetUsersRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public AspNetUsersRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetAspNetUsersListForDropDown()
-        {
-            try
             {
-
-                return _db.AspNetUsers
-                    .Where(e => (bool)e.Status)
-                    .OrderBy(o => o.NomeCompleto)
-                    .Select(i => new SelectListItem()
+            return _db.AspNetUsers
+                .Where(e => (bool)e.Status)
+                .OrderBy(o => o.NomeCompleto)
+                .Select(i => new SelectListItem()
                     {
-                        Text = i.NomeCompleto,
-                        Value = i.Id.ToString()
+                    Text = i.NomeCompleto,
+                    Value = i.Id.ToString()
                     });
             }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AspNetUsersRepository.cs", "GetAspNetUsersListForDropDown", ex);
-                return new List<SelectListItem>();
-            }
-        }
 
         public new void Update(AspNetUsers aspNetUsers)
-        {
-            try
             {
+            var objFromDb = _db.AspNetUsers.FirstOrDefault(s => s.Id == aspNetUsers.Id);
 
-                var objFromDb = _db.AspNetUsers.AsTracking().FirstOrDefault(s => s.Id == aspNetUsers.Id);
+            _db.Update(aspNetUsers);
+            _db.SaveChanges();
 
-                _db.Update(aspNetUsers);
+            }
 
-                _db.SaveChanges();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AspNetUsersRepository.cs", "Update", ex);
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
                return _db.AspNetUsers
                    .Where(e => (bool)e.Status)
                    .OrderBy(o => o.NomeCompleto)
                    .Select(i => new SelectListItem()
                        Text = i.NomeCompleto,
                        Value = i.Id.ToString()
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AspNetUsersRepository.cs", "GetAspNetUsersListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }
        {
            try
                var objFromDb = _db.AspNetUsers.AsTracking().FirstOrDefault(s => s.Id == aspNetUsers.Id);
                _db.Update(aspNetUsers);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AspNetUsersRepository.cs", "Update", ex);
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
            return _db.AspNetUsers
                .Where(e => (bool)e.Status)
                .OrderBy(o => o.NomeCompleto)
                .Select(i => new SelectListItem()
                    Text = i.NomeCompleto,
                    Value = i.Id.ToString()
            var objFromDb = _db.AspNetUsers.FirstOrDefault(s => s.Id == aspNetUsers.Id);
            _db.Update(aspNetUsers);
            _db.SaveChanges();
            }
```
