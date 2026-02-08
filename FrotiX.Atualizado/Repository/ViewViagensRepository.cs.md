# Repository/ViewViagensRepository.cs

**Mudanca:** PEQUENA | **+2** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewViagensRepository.cs
+++ ATUAL: Repository/ViewViagensRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -8,9 +7,11 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
+using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
 {
+
     public class ViewViagensRepository : Repository<ViewViagens>, IViewViagensRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -59,7 +60,7 @@
 
         public new void Update(ViewViagens viewViagens)
         {
-            var objFromDb = _db.ViewViagens.AsTracking().FirstOrDefault(s => s.ViagemId == viewViagens.ViagemId);
+            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewViagens.ViagemId);
 
             _db.Update(viewViagens);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewViagens.AsTracking().FirstOrDefault(s => s.ViagemId == viewViagens.ViagemId);
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewViagens.FirstOrDefault(s => s.ViagemId == viewViagens.ViagemId);
```
