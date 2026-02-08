# Repository/ViagensEconomildoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViagensEconomildoRepository.cs
+++ ATUAL: Repository/ViagensEconomildoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViagensEconomildoRepository : Repository<ViagensEconomildo>, IViagensEconomildoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -31,7 +31,7 @@
 
         public new void Update(ViagensEconomildo viagensEconomildo)
             {
-            var objFromDb = _db.ViagensEconomildo.AsTracking().FirstOrDefault(s => s.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId);
+            var objFromDb = _db.ViagensEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId);
 
             _db.Update(viagensEconomildo);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViagensEconomildo.AsTracking().FirstOrDefault(s => s.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViagensEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId);
```
