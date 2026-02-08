# Repository/ViewViagensAgendaTodosMesesRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewViagensAgendaTodosMesesRepository.cs
+++ ATUAL: Repository/ViewViagensAgendaTodosMesesRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewViagensAgendaTodosMesesRepository : Repository<ViewViagensAgendaTodosMeses>, IViewViagensAgendaTodosMesesRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewViagensAgendaTodosMeses viewViagensAgendaTodosMeses)
             {
-            var objFromDb = _db.ViewViagensAgendaTodosMeses.AsTracking().FirstOrDefault(s => s.ViagemId == viewViagensAgendaTodosMeses.ViagemId);
+            var objFromDb = _db.ViewViagensAgendaTodosMeses.FirstOrDefault(s => s.ViagemId == viewViagensAgendaTodosMeses.ViagemId);
 
             _db.Update(viewViagensAgendaTodosMeses);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewViagensAgendaTodosMeses.AsTracking().FirstOrDefault(s => s.ViagemId == viewViagensAgendaTodosMeses.ViagemId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewViagensAgendaTodosMeses.FirstOrDefault(s => s.ViagemId == viewViagensAgendaTodosMeses.ViagemId);
```
