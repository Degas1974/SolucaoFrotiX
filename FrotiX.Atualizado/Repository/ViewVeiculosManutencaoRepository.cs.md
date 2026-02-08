# Repository/ViewVeiculosManutencaoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewVeiculosManutencaoRepository.cs
+++ ATUAL: Repository/ViewVeiculosManutencaoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
@@ -8,6 +7,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewVeiculosManutencaoRepository : Repository<ViewVeiculosManutencao>, IViewVeiculosManutencaoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -30,7 +30,7 @@
 
         public new void Update(ViewVeiculosManutencao viewVeiculosManutencao)
             {
-            var objFromDb = _db.ViewVeiculosManutencao.AsTracking().FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencao.VeiculoId);
+            var objFromDb = _db.ViewVeiculosManutencao.FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencao.VeiculoId);
 
             _db.Update(viewVeiculosManutencao);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewVeiculosManutencao.AsTracking().FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencao.VeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewVeiculosManutencao.FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencao.VeiculoId);
```
