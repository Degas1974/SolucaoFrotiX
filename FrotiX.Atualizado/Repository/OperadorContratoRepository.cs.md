# Repository/OperadorContratoRepository.cs

**Mudanca:** MEDIA | **+11** linhas | **-12** linhas

---

```diff
--- JANEIRO: Repository/OperadorContratoRepository.cs
+++ ATUAL: Repository/OperadorContratoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,32 +8,33 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository
-{
+    {
+
     public class OperadorContratoRepository : Repository<OperadorContrato>, IOperadorContratoRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public OperadorContratoRepository(FrotiXDbContext db) : base(db)
-        {
+            {
             _db = db;
-        }
+            }
 
         public IEnumerable<SelectListItem> GetOperadorContratoListForDropDown()
-        {
+            {
             return _db.OperadorContrato.Select(i => new SelectListItem()
-            {
+                {
 
-            }); ;
-        }
+                }); ;
+            }
 
         public new void Update(OperadorContrato operadorContrato)
-        {
-            var objFromDb = _db.OperadorContrato.AsTracking().FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));
+            {
+            var objFromDb = _db.OperadorContrato.FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));
 
             _db.Update(operadorContrato);
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
            var objFromDb = _db.OperadorContrato.AsTracking().FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));
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
            var objFromDb = _db.OperadorContrato.FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));
            }
```
