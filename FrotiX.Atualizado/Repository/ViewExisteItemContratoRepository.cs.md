# Repository/ViewExisteItemContratoRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewExisteItemContratoRepository.cs
+++ ATUAL: Repository/ViewExisteItemContratoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewExisteItemContratoRepository : Repository<ViewExisteItemContrato>, IViewExisteItemContratoRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewExisteItemContrato viewExisteItemContrato)
             {
-            var objFromDb = _db.ViewExisteItemContrato.AsTracking().FirstOrDefault(v => v.RepactuacaoContratoId == viewExisteItemContrato.RepactuacaoContratoId);
+            var objFromDb = _db.ViewExisteItemContrato.FirstOrDefault(v => v.RepactuacaoContratoId == viewExisteItemContrato.RepactuacaoContratoId);
 
             _db.Update(viewExisteItemContrato);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewExisteItemContrato.AsTracking().FirstOrDefault(v => v.RepactuacaoContratoId == viewExisteItemContrato.RepactuacaoContratoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewExisteItemContrato.FirstOrDefault(v => v.RepactuacaoContratoId == viewExisteItemContrato.RepactuacaoContratoId);
```
