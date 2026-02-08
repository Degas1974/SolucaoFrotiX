# Repository/RequisitanteRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/RequisitanteRepository.cs
+++ ATUAL: Repository/RequisitanteRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class RequisitanteRepository : Repository<Requisitante>, IRequisitanteRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -34,7 +34,7 @@
 
         public new void Update(Requisitante requisitante)
         {
-            var objFromDb = _db.Requisitante.AsTracking().FirstOrDefault(s =>
+            var objFromDb = _db.Requisitante.FirstOrDefault(s =>
                 s.RequisitanteId == requisitante.RequisitanteId
             );
 
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.Requisitante.AsTracking().FirstOrDefault(s =>
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.Requisitante.FirstOrDefault(s =>
```
