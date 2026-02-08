# Repository/SecaoPatrimonialRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/SecaoPatrimonialRepository.cs
+++ ATUAL: Repository/SecaoPatrimonialRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,35 +9,36 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class SecaoPatrimonialRepository : Repository<SecaoPatrimonial>, ISecaoPatrimonialRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public SecaoPatrimonialRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetSecaoListForDropDown()
-        {
+            {
             return _db.SecaoPatrimonial
             .OrderBy(o => o.NomeSecao)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.NomeSecao + "/" + i.SetorId.ToString(),
                 Value = i.SecaoId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(SecaoPatrimonial secao)
-        {
-            var objFromDb = _db.SecaoPatrimonial.AsTracking().FirstOrDefault(s => s.SecaoId == secao.SecaoId);
+            {
+            var objFromDb = _db.SecaoPatrimonial.FirstOrDefault(s => s.SecaoId == secao.SecaoId);
 
             _db.Update(secao);
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
            var objFromDb = _db.SecaoPatrimonial.AsTracking().FirstOrDefault(s => s.SecaoId == secao.SecaoId);
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
            var objFromDb = _db.SecaoPatrimonial.FirstOrDefault(s => s.SecaoId == secao.SecaoId);
            }
```
