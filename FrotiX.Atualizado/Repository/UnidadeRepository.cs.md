# Repository/UnidadeRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/UnidadeRepository.cs
+++ ATUAL: Repository/UnidadeRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class UnidadeRepository : Repository<Unidade>, IUnidadeRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -34,7 +34,7 @@
 
         public new void Update(Unidade unidade)
         {
-            var objFromDb = _db.Unidade.AsTracking().FirstOrDefault(s => s.UnidadeId == unidade.UnidadeId);
+            var objFromDb = _db.Unidade.FirstOrDefault(s => s.UnidadeId == unidade.UnidadeId);
 
             _db.Update(unidade);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.Unidade.AsTracking().FirstOrDefault(s => s.UnidadeId == unidade.UnidadeId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.Unidade.FirstOrDefault(s => s.UnidadeId == unidade.UnidadeId);
```
