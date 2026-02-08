# Repository/ViewPendenciasManutencaoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewPendenciasManutencaoRepository.cs
+++ ATUAL: Repository/ViewPendenciasManutencaoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewPendenciasManutencaoRepository : Repository<ViewPendenciasManutencao>, IViewPendenciasManutencaoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewPendenciasManutencao viewPendenciasManutencao)
             {
-            var objFromDb = _db.ViewPendenciasManutencao.AsTracking().FirstOrDefault(s => s.ItemManutencaoId == viewPendenciasManutencao.ItemManutencaoId);
+            var objFromDb = _db.ViewPendenciasManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewPendenciasManutencao.ItemManutencaoId);
 
             _db.Update(viewPendenciasManutencao);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewPendenciasManutencao.AsTracking().FirstOrDefault(s => s.ItemManutencaoId == viewPendenciasManutencao.ItemManutencaoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewPendenciasManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewPendenciasManutencao.ItemManutencaoId);
```
