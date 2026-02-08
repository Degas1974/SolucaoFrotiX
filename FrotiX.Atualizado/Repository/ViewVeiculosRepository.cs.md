# Repository/ViewVeiculosRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewVeiculosRepository.cs
+++ ATUAL: Repository/ViewVeiculosRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewVeiculosRepository : Repository<ViewVeiculos>, IViewVeiculosRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewVeiculos viewVeiculos)
             {
-            var objFromDb = _db.ViewVeiculos.AsTracking().FirstOrDefault(s => s.VeiculoId == viewVeiculos.VeiculoId);
+            var objFromDb = _db.ViewVeiculos.FirstOrDefault(s => s.VeiculoId == viewVeiculos.VeiculoId);
 
             _db.Update(viewVeiculos);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewVeiculos.AsTracking().FirstOrDefault(s => s.VeiculoId == viewVeiculos.VeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewVeiculos.FirstOrDefault(s => s.VeiculoId == viewVeiculos.VeiculoId);
```
