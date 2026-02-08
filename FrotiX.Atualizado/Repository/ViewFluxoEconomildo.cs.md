# Repository/ViewFluxoEconomildo.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewFluxoEconomildo.cs
+++ ATUAL: Repository/ViewFluxoEconomildo.cs
@@ -6,10 +6,10 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
     {
+
     public class ViewFluxoEconomildoRepository : Repository<ViewFluxoEconomildo>, IViewFluxoEconomildoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewFluxoEconomildo viewFluxoEconomildo)
             {
-            var objFromDb = _db.ViewFluxoEconomildo.AsTracking().FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildo.ViagemEconomildoId);
+            var objFromDb = _db.ViewFluxoEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildo.ViagemEconomildoId);
 
             _db.Update(viewFluxoEconomildo);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewFluxoEconomildo.AsTracking().FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildo.ViagemEconomildoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewFluxoEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildo.ViagemEconomildoId);
```
