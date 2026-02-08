# Repository/TipoMultaRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/TipoMultaRepository.cs
+++ ATUAL: Repository/TipoMultaRepository.cs
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
     public class TipoMultaRepository : Repository<TipoMulta>, ITipoMultaRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public TipoMultaRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetTipoMultaListForDropDown()
-        {
+            {
             return _db.TipoMulta
                 .OrderBy(o => o.Artigo)
                 .Select(i => new SelectListItem()
-                {
+                    {
                     Text = i.Artigo + " - " + i.Descricao,
                     Value = i.TipoMultaId.ToString()
-                });
-        }
+                    });
+            }
 
         public new void Update(TipoMulta tipomulta)
-        {
-            var objFromDb = _db.TipoMulta.AsTracking().FirstOrDefault(s => s.TipoMultaId == tipomulta.TipoMultaId);
+            {
+            var objFromDb = _db.TipoMulta.FirstOrDefault(s => s.TipoMultaId == tipomulta.TipoMultaId);
 
             _db.Update(tipomulta);
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
            var objFromDb = _db.TipoMulta.AsTracking().FirstOrDefault(s => s.TipoMultaId == tipomulta.TipoMultaId);
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
            var objFromDb = _db.TipoMulta.FirstOrDefault(s => s.TipoMultaId == tipomulta.TipoMultaId);
            }
```
