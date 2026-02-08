# Repository/PlacaBronzeRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/PlacaBronzeRepository.cs
+++ ATUAL: Repository/PlacaBronzeRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class PlacaBronzeRepository : Repository<PlacaBronze>, IPlacaBronzeRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -34,7 +34,7 @@
 
         public new void Update(PlacaBronze placaBronze)
         {
-            var objFromDb = _db.PlacaBronze.AsTracking().FirstOrDefault(s =>
+            var objFromDb = _db.PlacaBronze.FirstOrDefault(s =>
                 s.PlacaBronzeId == placaBronze.PlacaBronzeId
             );
 
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.PlacaBronze.AsTracking().FirstOrDefault(s =>
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.PlacaBronze.FirstOrDefault(s =>
```
