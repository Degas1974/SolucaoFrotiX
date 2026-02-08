# Repository/ViewItensManutencaoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewItensManutencaoRepository.cs
+++ ATUAL: Repository/ViewItensManutencaoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewItensManutencaoRepository : Repository<ViewItensManutencao>, IViewItensManutencaoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewItensManutencao viewItensManutencao)
             {
-            var objFromDb = _db.ViewItensManutencao.AsTracking().FirstOrDefault(s => s.ItemManutencaoId == viewItensManutencao.ItemManutencaoId);
+            var objFromDb = _db.ViewItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewItensManutencao.ItemManutencaoId);
 
             _db.Update(viewItensManutencao);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewItensManutencao.AsTracking().FirstOrDefault(s => s.ItemManutencaoId == viewItensManutencao.ItemManutencaoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewItensManutencao.ItemManutencaoId);
```
