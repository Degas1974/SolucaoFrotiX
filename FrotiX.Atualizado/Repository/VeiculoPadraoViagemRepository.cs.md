# Repository/VeiculoPadraoViagemRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/VeiculoPadraoViagemRepository.cs
+++ ATUAL: Repository/VeiculoPadraoViagemRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,6 +8,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class VeiculoPadraoViagemRepository : Repository<VeiculoPadraoViagem>, IVeiculoPadraoViagemRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -20,7 +20,7 @@
 
         public new void Update(VeiculoPadraoViagem veiculoPadraoViagem)
         {
-            var objFromDb = _db.VeiculoPadraoViagem.AsTracking().FirstOrDefault(s => s.VeiculoId == veiculoPadraoViagem.VeiculoId);
+            var objFromDb = _db.VeiculoPadraoViagem.FirstOrDefault(s => s.VeiculoId == veiculoPadraoViagem.VeiculoId);
 
             if (objFromDb != null)
             {
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.VeiculoPadraoViagem.AsTracking().FirstOrDefault(s => s.VeiculoId == veiculoPadraoViagem.VeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.VeiculoPadraoViagem.FirstOrDefault(s => s.VeiculoId == veiculoPadraoViagem.VeiculoId);
```
