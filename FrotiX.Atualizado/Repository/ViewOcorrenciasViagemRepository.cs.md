# Repository/ViewOcorrenciasViagemRepository.cs

**Mudanca:** PEQUENA | **+0** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/ViewOcorrenciasViagemRepository.cs
+++ ATUAL: Repository/ViewOcorrenciasViagemRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,6 +8,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class ViewOcorrenciasViagemRepository : IViewOcorrenciasViagemRepository
     {
         private new readonly FrotiXDbContext _db;
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
```

