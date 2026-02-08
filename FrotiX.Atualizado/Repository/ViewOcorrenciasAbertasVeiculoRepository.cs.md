# Repository/ViewOcorrenciasAbertasVeiculoRepository.cs

**Mudanca:** PEQUENA | **+0** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/ViewOcorrenciasAbertasVeiculoRepository.cs
+++ ATUAL: Repository/ViewOcorrenciasAbertasVeiculoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -9,6 +8,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class ViewOcorrenciasAbertasVeiculoRepository : IViewOcorrenciasAbertasVeiculoRepository
     {
         private new readonly FrotiXDbContext _db;
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
```

