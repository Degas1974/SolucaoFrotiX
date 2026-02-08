# Repository/ViewNoFichaVistoriaRepository.cs

**Mudanca:** PEQUENA | **+0** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/ViewNoFichaVistoriaRepository.cs
+++ ATUAL: Repository/ViewNoFichaVistoriaRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewNoFichaVistoriaRepository : Repository<ViewNoFichaVistoria>, IViewNoFichaVistoriaRepository
         {
         private new readonly FrotiXDbContext _db;
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
```

