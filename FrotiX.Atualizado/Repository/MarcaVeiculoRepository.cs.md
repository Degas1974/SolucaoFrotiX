# Repository/MarcaVeiculoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/MarcaVeiculoRepository.cs
+++ ATUAL: Repository/MarcaVeiculoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class MarcaVeiculoRepository : Repository<MarcaVeiculo>, IMarcaVeiculoRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -34,7 +34,7 @@
 
         public new void Update(MarcaVeiculo marcaVeiculo)
         {
-            var objFromDb = _db.MarcaVeiculo.AsTracking().FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);
+            var objFromDb = _db.MarcaVeiculo.FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);
 
             _db.Update(marcaVeiculo);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.MarcaVeiculo.AsTracking().FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.MarcaVeiculo.FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);
```
