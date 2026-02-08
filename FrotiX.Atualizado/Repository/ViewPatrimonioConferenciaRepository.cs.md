# Repository/ViewPatrimonioConferenciaRepository.cs

**Mudanca:** PEQUENA | **+0** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/ViewPatrimonioConferenciaRepository.cs
+++ ATUAL: Repository/ViewPatrimonioConferenciaRepository.cs
@@ -1,10 +1,10 @@
-using Microsoft.EntityFrameworkCore;
 using FrotiX.Data;
 using FrotiX.Models.Views;
 using FrotiX.Repository.IRepository;
 
 namespace FrotiX.Repository
 {
+
     public class ViewPatrimonioConferenciaRepository :Repository<ViewPatrimonioConferencia>, IViewPatrimonioConferenciaRepository
     {
         private new readonly FrotiXDbContext _db;
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
```

