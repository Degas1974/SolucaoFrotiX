# Repository/ViewMotoristaFluxo.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewMotoristaFluxo.cs
+++ ATUAL: Repository/ViewMotoristaFluxo.cs
@@ -6,10 +6,10 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
     {
+
     public class ViewMotoristaFluxoRepository : Repository<ViewMotoristaFluxo>, IViewMotoristaFluxoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewMotoristaFluxo viewMotoristaFluxo)
             {
-            var objFromDb = _db.ViewMotoristaFluxo.AsTracking().FirstOrDefault(s => s.NomeMotorista == viewMotoristaFluxo.NomeMotorista);
+            var objFromDb = _db.ViewMotoristaFluxo.FirstOrDefault(s => s.NomeMotorista == viewMotoristaFluxo.NomeMotorista);
 
             _db.Update(viewMotoristaFluxo);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewMotoristaFluxo.AsTracking().FirstOrDefault(s => s.NomeMotorista == viewMotoristaFluxo.NomeMotorista);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewMotoristaFluxo.FirstOrDefault(s => s.NomeMotorista == viewMotoristaFluxo.NomeMotorista);
```
