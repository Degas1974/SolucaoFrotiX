# Repository/ViewFluxoEconomildoData.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewFluxoEconomildoData.cs
+++ ATUAL: Repository/ViewFluxoEconomildoData.cs
@@ -6,10 +6,10 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
     {
+
     public class ViewFluxoEconomildoDataRepository : Repository<ViewFluxoEconomildoData>, IViewFluxoEconomildoDataRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewFluxoEconomildoData viewFluxoEconomildoData)
             {
-            var objFromDb = _db.ViewFluxoEconomildoData.AsTracking().FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildoData.ViagemEconomildoId);
+            var objFromDb = _db.ViewFluxoEconomildoData.FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildoData.ViagemEconomildoId);
 
             _db.Update(viewFluxoEconomildoData);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewFluxoEconomildoData.AsTracking().FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildoData.ViagemEconomildoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewFluxoEconomildoData.FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildoData.ViagemEconomildoId);
```
