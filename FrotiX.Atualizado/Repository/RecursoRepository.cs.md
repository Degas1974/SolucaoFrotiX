# Repository/RecursoRepository.cs

**Mudanca:** MEDIA | **+12** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/RecursoRepository.cs
+++ ATUAL: Repository/RecursoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -7,37 +6,39 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
+using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class RecursoRepository : Repository<Recurso>, IRecursoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public RecursoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetRecursoListForDropDown()
-        {
+            {
             return _db.Recurso
             .OrderBy(o => o.Nome)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Nome,
                 Value = i.RecursoId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(Recurso recurso)
-        {
-            var objFromDb = _db.Recurso.AsTracking().FirstOrDefault(s => s.RecursoId == recurso.RecursoId);
+            {
+            var objFromDb = _db.Recurso.FirstOrDefault(s => s.RecursoId == recurso.RecursoId);
 
             _db.Update(recurso);
             _db.SaveChanges();
 
+            }
+
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
        }
        {
            {
            }); ;
        }
        {
            var objFromDb = _db.Recurso.AsTracking().FirstOrDefault(s => s.RecursoId == recurso.RecursoId);
}
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
    {
        {
            {
            }
            {
                {
                }); ;
            }
            {
            var objFromDb = _db.Recurso.FirstOrDefault(s => s.RecursoId == recurso.RecursoId);
            }
```
