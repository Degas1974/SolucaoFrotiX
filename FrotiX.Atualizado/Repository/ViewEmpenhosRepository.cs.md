# Repository/ViewEmpenhosRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewEmpenhosRepository.cs
+++ ATUAL: Repository/ViewEmpenhosRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewEmpenhosRepository : Repository<ViewEmpenhos>, IViewEmpenhosRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewEmpenhos viewEmpenhos)
             {
-            var objFromDb = _db.ViewEmpenhos.AsTracking().FirstOrDefault(s => s.EmpenhoId == viewEmpenhos.EmpenhoId);
+            var objFromDb = _db.ViewEmpenhos.FirstOrDefault(s => s.EmpenhoId == viewEmpenhos.EmpenhoId);
 
             _db.Update(viewEmpenhos);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewEmpenhos.AsTracking().FirstOrDefault(s => s.EmpenhoId == viewEmpenhos.EmpenhoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewEmpenhos.FirstOrDefault(s => s.EmpenhoId == viewEmpenhos.EmpenhoId);
```
