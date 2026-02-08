# Repository/ViewViagensAgendaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewViagensAgendaRepository.cs
+++ ATUAL: Repository/ViewViagensAgendaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -11,6 +10,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewViagensAgendaRepository
         : Repository<ViewViagensAgenda>,
             IViewViagensAgendaRepository
@@ -36,7 +36,7 @@
 
         public new void Update(ViewViagensAgenda viewViagensAgenda)
             {
-            var objFromDb = _db.ViewViagensAgenda.AsTracking().FirstOrDefault(s =>
+            var objFromDb = _db.ViewViagensAgenda.FirstOrDefault(s =>
                 s.ViagemId == viewViagensAgenda.ViagemId
             );
 
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewViagensAgenda.AsTracking().FirstOrDefault(s =>
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewViagensAgenda.FirstOrDefault(s =>
```
