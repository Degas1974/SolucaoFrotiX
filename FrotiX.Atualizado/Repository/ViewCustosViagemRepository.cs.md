# Repository/ViewCustosViagemRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewCustosViagemRepository.cs
+++ ATUAL: Repository/ViewCustosViagemRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewCustosViagemRepository : Repository<ViewCustosViagem>, IViewCustosViagemRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewCustosViagem viewCustosViagem)
             {
-            var objFromDb = _db.ViewViagens.AsTracking().FirstOrDefault(s => s.ViagemId == viewCustosViagem.ViagemId);
+            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewCustosViagem.ViagemId);
 
             _db.Update(viewCustosViagem);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewViagens.AsTracking().FirstOrDefault(s => s.ViagemId == viewCustosViagem.ViagemId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewCustosViagem.ViagemId);
```
