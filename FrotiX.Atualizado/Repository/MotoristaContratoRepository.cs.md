# Repository/MotoristaContratoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/MotoristaContratoRepository.cs
+++ ATUAL: Repository/MotoristaContratoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class MotoristaContratoRepository : Repository<MotoristaContrato>, IMotoristaContratoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -29,7 +29,7 @@
 
         public new void Update(MotoristaContrato motoristaContrato)
             {
-            var objFromDb = _db.MotoristaContrato.AsTracking().FirstOrDefault(s => (s.MotoristaId == motoristaContrato.MotoristaId) && (s.ContratoId == motoristaContrato.ContratoId));
+            var objFromDb = _db.MotoristaContrato.FirstOrDefault(s => (s.MotoristaId == motoristaContrato.MotoristaId) && (s.ContratoId == motoristaContrato.ContratoId));
 
             _db.Update(motoristaContrato);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.MotoristaContrato.AsTracking().FirstOrDefault(s => (s.MotoristaId == motoristaContrato.MotoristaId) && (s.ContratoId == motoristaContrato.ContratoId));
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.MotoristaContrato.FirstOrDefault(s => (s.MotoristaId == motoristaContrato.MotoristaId) && (s.ContratoId == motoristaContrato.ContratoId));
```
