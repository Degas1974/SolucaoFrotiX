# Repository/ViewRequisitantesRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewRequisitantesRepository.cs
+++ ATUAL: Repository/ViewRequisitantesRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewRequisitantesRepository : Repository<ViewRequisitantes>, IViewRequisitantesRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewRequisitantes viewRequisitantes)
             {
-            var objFromDb = _db.ViewRequisitantes.AsTracking().FirstOrDefault(s => s.RequisitanteId == viewRequisitantes.RequisitanteId);
+            var objFromDb = _db.ViewRequisitantes.FirstOrDefault(s => s.RequisitanteId == viewRequisitantes.RequisitanteId);
 
             _db.Update(viewRequisitantes);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewRequisitantes.AsTracking().FirstOrDefault(s => s.RequisitanteId == viewRequisitantes.RequisitanteId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewRequisitantes.FirstOrDefault(s => s.RequisitanteId == viewRequisitantes.RequisitanteId);
```
