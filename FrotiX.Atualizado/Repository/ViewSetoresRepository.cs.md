# Repository/ViewSetoresRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewSetoresRepository.cs
+++ ATUAL: Repository/ViewSetoresRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewSetoresRepository : Repository<ViewSetores>, IViewSetoresRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewSetores viewSetores)
             {
-            var objFromDb = _db.ViewSetores.AsTracking().FirstOrDefault(s => s.SetorSolicitanteId == viewSetores.SetorSolicitanteId);
+            var objFromDb = _db.ViewSetores.FirstOrDefault(s => s.SetorSolicitanteId == viewSetores.SetorSolicitanteId);
 
             _db.Update(viewSetores);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewSetores.AsTracking().FirstOrDefault(s => s.SetorSolicitanteId == viewSetores.SetorSolicitanteId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewSetores.FirstOrDefault(s => s.SetorSolicitanteId == viewSetores.SetorSolicitanteId);
```
