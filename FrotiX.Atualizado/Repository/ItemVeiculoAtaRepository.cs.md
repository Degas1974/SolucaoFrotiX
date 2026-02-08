# Repository/ItemVeiculoAtaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ItemVeiculoAtaRepository.cs
+++ ATUAL: Repository/ItemVeiculoAtaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class ItemVeiculoAtaRepository : Repository<ItemVeiculoAta>, IItemVeiculoAtaRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -32,12 +32,10 @@
 
         public new void Update(ItemVeiculoAta itemveiculoata)
         {
-            var objFromDb = _db.ItemVeiculoAta.AsTracking().FirstOrDefault(s => s.ItemVeiculoAtaId == itemveiculoata.ItemVeiculoAtaId);
+            var objFromDb = _db.ItemVeiculoAta.FirstOrDefault(s => s.ItemVeiculoAtaId == itemveiculoata.ItemVeiculoAtaId);
 
             _db.Update(itemveiculoata);
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
            var objFromDb = _db.ItemVeiculoAta.AsTracking().FirstOrDefault(s => s.ItemVeiculoAtaId == itemveiculoata.ItemVeiculoAtaId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ItemVeiculoAta.FirstOrDefault(s => s.ItemVeiculoAtaId == itemveiculoata.ItemVeiculoAtaId);
```
