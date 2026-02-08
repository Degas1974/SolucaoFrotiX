# Repository/ViewEventosRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewEventosRepository.cs
+++ ATUAL: Repository/ViewEventosRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewEventosRepository : Repository<ViewEventos>, IViewEventosRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewEventos viewEventos)
             {
-            var objFromDb = _db.ViewEventos.AsTracking().FirstOrDefault(s => s.EventoId == viewEventos.EventoId);
+            var objFromDb = _db.ViewEventos.FirstOrDefault(s => s.EventoId == viewEventos.EventoId);
 
             _db.Update(viewEventos);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewEventos.AsTracking().FirstOrDefault(s => s.EventoId == viewEventos.EventoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewEventos.FirstOrDefault(s => s.EventoId == viewEventos.EventoId);
```
