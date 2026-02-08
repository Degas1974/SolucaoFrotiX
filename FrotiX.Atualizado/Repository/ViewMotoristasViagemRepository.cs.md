# Repository/ViewMotoristasViagemRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewMotoristasViagemRepository.cs
+++ ATUAL: Repository/ViewMotoristasViagemRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Models.Views;
@@ -11,6 +10,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewMotoristasViagemRepository : Repository<ViewMotoristasViagem>, IViewMotoristasViagemRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -33,7 +33,7 @@
 
         public new void Update(ViewMotoristasViagem viewMotoristasviagem)
             {
-            var objFromDb = _db.ViewMotoristasViagem.AsTracking().FirstOrDefault(s => s.MotoristaId == viewMotoristasviagem.MotoristaId);
+            var objFromDb = _db.ViewMotoristasViagem.FirstOrDefault(s => s.MotoristaId == viewMotoristasviagem.MotoristaId);
 
             _db.Update(viewMotoristasviagem);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewMotoristasViagem.AsTracking().FirstOrDefault(s => s.MotoristaId == viewMotoristasviagem.MotoristaId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewMotoristasViagem.FirstOrDefault(s => s.MotoristaId == viewMotoristasviagem.MotoristaId);
```
