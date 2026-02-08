# Repository/ViewLotacaoMotoristaRepository.cs

**Mudanca:** PEQUENA | **+0** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/ViewLotacaoMotoristaRepository.cs
+++ ATUAL: Repository/ViewLotacaoMotoristaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewLotacaoMotoristaRepository : Repository<ViewLotacaoMotorista>, IViewLotacaoMotoristaRepository
         {
         private new readonly FrotiXDbContext _db;
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
```

