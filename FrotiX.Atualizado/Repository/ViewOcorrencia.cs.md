# Repository/ViewOcorrencia.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewOcorrencia.cs
+++ ATUAL: Repository/ViewOcorrencia.cs
@@ -6,10 +6,10 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
-using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
     {
+
     public class ViewOcorrenciaRepository : Repository<ViewOcorrencia>, IViewOcorrenciaRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewOcorrencia viewOcorrencia)
             {
-            var objFromDb = _db.ViewOcorrencia.AsTracking().FirstOrDefault(s => s.ViagemId == viewOcorrencia.ViagemId);
+            var objFromDb = _db.ViewOcorrencia.FirstOrDefault(s => s.ViagemId == viewOcorrencia.ViagemId);
 
             _db.Update(viewOcorrencia);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewOcorrencia.AsTracking().FirstOrDefault(s => s.ViagemId == viewOcorrencia.ViagemId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewOcorrencia.FirstOrDefault(s => s.ViagemId == viewOcorrencia.ViagemId);
```
