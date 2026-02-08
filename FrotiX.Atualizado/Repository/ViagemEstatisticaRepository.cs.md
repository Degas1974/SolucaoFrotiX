# Repository/ViagemEstatisticaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/ViagemEstatisticaRepository.cs
+++ ATUAL: Repository/ViagemEstatisticaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -6,9 +5,11 @@
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
+using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
 {
+
     public class ViagemEstatisticaRepository : Repository<ViagemEstatistica>, IViagemEstatisticaRepository
     {
         private readonly FrotiXDbContext _context;
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
```
