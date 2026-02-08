# Repository/ViewProcuraFichaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewProcuraFichaRepository.cs
+++ ATUAL: Repository/ViewProcuraFichaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewProcuraFichaRepository : Repository<ViewProcuraFicha>, IViewProcuraFichaRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewProcuraFicha viewProcuraFicha)
             {
-            var objFromDb = _db.ViewProcuraFicha.AsTracking().FirstOrDefault(s => s.VeiculoId == viewProcuraFicha.VeiculoId);
+            var objFromDb = _db.ViewProcuraFicha.FirstOrDefault(s => s.VeiculoId == viewProcuraFicha.VeiculoId);
 
             _db.Update(viewProcuraFicha);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewProcuraFicha.AsTracking().FirstOrDefault(s => s.VeiculoId == viewProcuraFicha.VeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewProcuraFicha.FirstOrDefault(s => s.VeiculoId == viewProcuraFicha.VeiculoId);
```
