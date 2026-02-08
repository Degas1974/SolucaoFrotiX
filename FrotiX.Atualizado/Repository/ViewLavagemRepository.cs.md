# Repository/ViewLavagemRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewLavagemRepository.cs
+++ ATUAL: Repository/ViewLavagemRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewLavagemRepository : Repository<ViewLavagem>, IViewLavagemRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewLavagem viewLavagem)
             {
-            var objFromDb = _db.ViewLavagem.AsTracking().FirstOrDefault(s => s.LavagemId == viewLavagem.LavagemId);
+            var objFromDb = _db.ViewLavagem.FirstOrDefault(s => s.LavagemId == viewLavagem.LavagemId);
 
             _db.Update(viewLavagem);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewLavagem.AsTracking().FirstOrDefault(s => s.LavagemId == viewLavagem.LavagemId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewLavagem.FirstOrDefault(s => s.LavagemId == viewLavagem.LavagemId);
```
