# Repository/ViewMotoristasRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewMotoristasRepository.cs
+++ ATUAL: Repository/ViewMotoristasRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewMotoristasRepository : Repository<ViewMotoristas>, IViewMotoristasRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewMotoristas viewMotoristas)
             {
-            var objFromDb = _db.ViewMotoristas.AsTracking().FirstOrDefault(s => s.MotoristaId == viewMotoristas.MotoristaId);
+            var objFromDb = _db.ViewMotoristas.FirstOrDefault(s => s.MotoristaId == viewMotoristas.MotoristaId);
 
             _db.Update(viewMotoristas);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewMotoristas.AsTracking().FirstOrDefault(s => s.MotoristaId == viewMotoristas.MotoristaId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewMotoristas.FirstOrDefault(s => s.MotoristaId == viewMotoristas.MotoristaId);
```
