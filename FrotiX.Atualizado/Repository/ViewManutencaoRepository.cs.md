# Repository/ViewManutencaoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewManutencaoRepository.cs
+++ ATUAL: Repository/ViewManutencaoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewManutencaoRepository : Repository<ViewManutencao>, IViewManutencaoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewManutencao viewManutencao)
             {
-            var objFromDb = _db.ViewManutencao.AsTracking().FirstOrDefault(s => s.ManutencaoId == viewManutencao.ManutencaoId);
+            var objFromDb = _db.ViewManutencao.FirstOrDefault(s => s.ManutencaoId == viewManutencao.ManutencaoId);
 
             _db.Update(viewManutencao);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewManutencao.AsTracking().FirstOrDefault(s => s.ManutencaoId == viewManutencao.ManutencaoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewManutencao.FirstOrDefault(s => s.ManutencaoId == viewManutencao.ManutencaoId);
```
