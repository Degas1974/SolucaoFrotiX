# Repository/ViewMediaConsumoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewMediaConsumoRepository.cs
+++ ATUAL: Repository/ViewMediaConsumoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewMediaConsumoRepository : Repository<ViewMediaConsumo>, IViewMediaConsumoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewMediaConsumo viewMediaConsumo)
             {
-            var objFromDb = _db.ViewMediaConsumo.AsTracking().FirstOrDefault(s => s.VeiculoId == viewMediaConsumo.VeiculoId);
+            var objFromDb = _db.ViewMediaConsumo.FirstOrDefault(s => s.VeiculoId == viewMediaConsumo.VeiculoId);
 
             _db.Update(viewMediaConsumo);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewMediaConsumo.AsTracking().FirstOrDefault(s => s.VeiculoId == viewMediaConsumo.VeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewMediaConsumo.FirstOrDefault(s => s.VeiculoId == viewMediaConsumo.VeiculoId);
```
