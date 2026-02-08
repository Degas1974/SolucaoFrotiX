# Repository/SetorSolicitanteRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/SetorSolicitanteRepository.cs
+++ ATUAL: Repository/SetorSolicitanteRepository.cs
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
     public class SetorSolicitanteRepository : Repository<SetorSolicitante>, ISetorSolicitanteRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public SetorSolicitanteRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetSetorSolicitanteListForDropDown()
-        {
+            {
             return _db.SetorSolicitante
             .OrderBy(o => o.Nome)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Nome,
                 Value = i.SetorSolicitanteId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(SetorSolicitante setorSolicitante)
-        {
-            var objFromDb = _db.SetorSolicitante.AsTracking().FirstOrDefault(s => s.SetorSolicitanteId == setorSolicitante.SetorSolicitanteId);
+            {
+            var objFromDb = _db.SetorSolicitante.FirstOrDefault(s => s.SetorSolicitanteId == setorSolicitante.SetorSolicitanteId);
 
             _db.Update(setorSolicitante);
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
            var objFromDb = _db.SetorSolicitante.AsTracking().FirstOrDefault(s => s.SetorSolicitanteId == setorSolicitante.SetorSolicitanteId);
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
                }); ;
            }
            {
            var objFromDb = _db.SetorSolicitante.FirstOrDefault(s => s.SetorSolicitanteId == setorSolicitante.SetorSolicitanteId);
            }
```
