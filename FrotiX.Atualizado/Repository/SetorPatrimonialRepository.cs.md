# Repository/SetorPatrimonialRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/SetorPatrimonialRepository.cs
+++ ATUAL: Repository/SetorPatrimonialRepository.cs
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
     public class SetorPatrimonialRepository : Repository<SetorPatrimonial>, ISetorPatrimonialRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public SetorPatrimonialRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetSetorListForDropDown()
-        {
+            {
             return _db.SetorPatrimonial
             .OrderBy(o => o.NomeSetor)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.NomeSetor,
                 Value = i.SetorId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(SetorPatrimonial setor)
-        {
-            var objFromDb = _db.SetorPatrimonial.AsTracking().FirstOrDefault(s => s.SetorId == setor.SetorId);
+            {
+            var objFromDb = _db.SetorPatrimonial.FirstOrDefault(s => s.SetorId == setor.SetorId);
 
             _db.Update(setor);
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
            var objFromDb = _db.SetorPatrimonial.AsTracking().FirstOrDefault(s => s.SetorId == setor.SetorId);
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
            var objFromDb = _db.SetorPatrimonial.FirstOrDefault(s => s.SetorId == setor.SetorId);
            }
```
