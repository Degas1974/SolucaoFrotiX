# Repository/ViewControleAcessoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewControleAcessoRepository.cs
+++ ATUAL: Repository/ViewControleAcessoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewControleAcessoRepository : Repository<ViewControleAcesso>, IViewControleAcessoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewControleAcesso viewControleAcesso)
             {
-            var objFromDb = _db.ViewControleAcesso.AsTracking().FirstOrDefault(s => s.RecursoId == viewControleAcesso.RecursoId);
+            var objFromDb = _db.ViewControleAcesso.FirstOrDefault(s => s.RecursoId == viewControleAcesso.RecursoId);
 
             _db.Update(viewControleAcesso);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewControleAcesso.AsTracking().FirstOrDefault(s => s.RecursoId == viewControleAcesso.RecursoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewControleAcesso.FirstOrDefault(s => s.RecursoId == viewControleAcesso.RecursoId);
```
