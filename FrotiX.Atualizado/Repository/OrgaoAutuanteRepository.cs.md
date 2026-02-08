# Repository/OrgaoAutuanteRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/OrgaoAutuanteRepository.cs
+++ ATUAL: Repository/OrgaoAutuanteRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,35 +8,36 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class OrgaoAutuanteRepository : Repository<OrgaoAutuante>, IOrgaoAutuanteRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public OrgaoAutuanteRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetOrgaoAutuanteListForDropDown()
-        {
+            {
             return _db.OrgaoAutuante
                 .OrderBy(o => o.Nome)
                 .Select(i => new SelectListItem()
-                {
+                    {
                     Text = i.Nome + " (" + i.Sigla + ")",
                     Value = i.OrgaoAutuanteId.ToString()
-                });
-        }
+                    });
+            }
 
         public new void Update(OrgaoAutuante orgaoautuante)
-        {
-            var objFromDb = _db.OrgaoAutuante.AsTracking().FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);
+            {
+            var objFromDb = _db.OrgaoAutuante.FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);
 
             _db.Update(orgaoautuante);
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
                });
        }
        {
            var objFromDb = _db.OrgaoAutuante.AsTracking().FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
            {
                    {
                    });
            }
            {
            var objFromDb = _db.OrgaoAutuante.FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);
            }
```
