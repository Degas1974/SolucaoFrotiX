# Repository/ViewEmpenhoMultaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewEmpenhoMultaRepository.cs
+++ ATUAL: Repository/ViewEmpenhoMultaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewEmpenhoMultaRepository : Repository<ViewEmpenhoMulta>, IViewEmpenhoMultaRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewEmpenhoMulta viewEmpenhoMulta)
             {
-            var objFromDb = _db.ViewEmpenhoMulta.AsTracking().FirstOrDefault(s => s.EmpenhoMultaId == viewEmpenhoMulta.EmpenhoMultaId);
+            var objFromDb = _db.ViewEmpenhoMulta.FirstOrDefault(s => s.EmpenhoMultaId == viewEmpenhoMulta.EmpenhoMultaId);
 
             _db.Update(viewEmpenhoMulta);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewEmpenhoMulta.AsTracking().FirstOrDefault(s => s.EmpenhoMultaId == viewEmpenhoMulta.EmpenhoMultaId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewEmpenhoMulta.FirstOrDefault(s => s.EmpenhoMultaId == viewEmpenhoMulta.EmpenhoMultaId);
```
