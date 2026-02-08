# Repository/ItemVeiculoContratoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ItemVeiculoContratoRepository.cs
+++ ATUAL: Repository/ItemVeiculoContratoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class ItemVeiculoContratoRepository : Repository<ItemVeiculoContrato>, IItemVeiculoContratoRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -32,12 +32,10 @@
 
         public new void Update(ItemVeiculoContrato itemveiculocontrato)
         {
-            var objFromDb = _db.ItemVeiculoContrato.AsTracking().FirstOrDefault(s => s.ItemVeiculoId == itemveiculocontrato.ItemVeiculoId);
+            var objFromDb = _db.ItemVeiculoContrato.FirstOrDefault(s => s.ItemVeiculoId == itemveiculocontrato.ItemVeiculoId);
 
             _db.Update(itemveiculocontrato);
             _db.SaveChanges();
-
         }
-
     }
 }
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ItemVeiculoContrato.AsTracking().FirstOrDefault(s => s.ItemVeiculoId == itemveiculocontrato.ItemVeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ItemVeiculoContrato.FirstOrDefault(s => s.ItemVeiculoId == itemveiculocontrato.ItemVeiculoId);
```
