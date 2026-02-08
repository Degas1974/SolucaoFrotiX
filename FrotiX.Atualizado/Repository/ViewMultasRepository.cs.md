# Repository/ViewMultasRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewMultasRepository.cs
+++ ATUAL: Repository/ViewMultasRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class viewMultasRepository : Repository<ViewMultas>, IviewMultasRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewMultas viewMultas)
             {
-            var objFromDb = _db.viewMultas.AsTracking().FirstOrDefault(s => s.MultaId == viewMultas.MultaId);
+            var objFromDb = _db.viewMultas.FirstOrDefault(s => s.MultaId == viewMultas.MultaId);
 
             _db.Update(viewMultas);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.viewMultas.AsTracking().FirstOrDefault(s => s.MultaId == viewMultas.MultaId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.viewMultas.FirstOrDefault(s => s.MultaId == viewMultas.MultaId);
```
