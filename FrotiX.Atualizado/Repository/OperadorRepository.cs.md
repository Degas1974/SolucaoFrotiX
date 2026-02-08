# Repository/OperadorRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/OperadorRepository.cs
+++ ATUAL: Repository/OperadorRepository.cs
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
     public class OperadorRepository : Repository<Operador>, IOperadorRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public OperadorRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetOperadorListForDropDown()
-        {
+            {
             return _db.Operador
             .OrderBy(o => o.Nome)
             .Select(i => new SelectListItem()
-            {
+                {
                 Text = i.Nome,
                 Value = i.OperadorId.ToString()
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(Operador operador)
-        {
-            var objFromDb = _db.Operador.AsTracking().FirstOrDefault(s => s.OperadorId == operador.OperadorId);
+            {
+            var objFromDb = _db.Operador.FirstOrDefault(s => s.OperadorId == operador.OperadorId);
 
             _db.Update(operador);
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
            var objFromDb = _db.Operador.AsTracking().FirstOrDefault(s => s.OperadorId == operador.OperadorId);
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
            var objFromDb = _db.Operador.FirstOrDefault(s => s.OperadorId == operador.OperadorId);
            }
```
