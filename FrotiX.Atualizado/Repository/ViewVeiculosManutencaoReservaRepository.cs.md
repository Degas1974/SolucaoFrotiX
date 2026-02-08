# Repository/ViewVeiculosManutencaoReservaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewVeiculosManutencaoReservaRepository.cs
+++ ATUAL: Repository/ViewVeiculosManutencaoReservaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
@@ -8,6 +7,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewVeiculosManutencaoReservaRepository : Repository<ViewVeiculosManutencaoReserva>, IViewVeiculosManutencaoReservaRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -30,7 +30,7 @@
 
         public new void Update(ViewVeiculosManutencaoReserva viewVeiculosManutencaoReserva)
             {
-            var objFromDb = _db.ViewVeiculosManutencaoReserva.AsTracking().FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencaoReserva.VeiculoId);
+            var objFromDb = _db.ViewVeiculosManutencaoReserva.FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencaoReserva.VeiculoId);
 
             _db.Update(viewVeiculosManutencaoReserva);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewVeiculosManutencaoReserva.AsTracking().FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencaoReserva.VeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewVeiculosManutencaoReserva.FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencaoReserva.VeiculoId);
```
