# Repository/ViewContratoFornecedorRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Repository/ViewContratoFornecedorRepository.cs
+++ ATUAL: Repository/ViewContratoFornecedorRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -10,6 +9,7 @@
 
 namespace FrotiX.Repository
     {
+
     public class ViewContratoFornecedorRepository : Repository<ViewContratoFornecedor>, IViewContratoFornecedorRepository
         {
         private new readonly FrotiXDbContext _db;
@@ -32,7 +32,7 @@
 
         public new void Update(ViewContratoFornecedor viewContratoFornecedor)
             {
-            var objFromDb = _db.ViewContratoFornecedor.AsTracking().FirstOrDefault(s => s.ContratoId == viewContratoFornecedor.ContratoId);
+            var objFromDb = _db.ViewContratoFornecedor.FirstOrDefault(s => s.ContratoId == viewContratoFornecedor.ContratoId);
 
             _db.Update(viewContratoFornecedor);
             _db.SaveChanges();
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
            var objFromDb = _db.ViewContratoFornecedor.AsTracking().FirstOrDefault(s => s.ContratoId == viewContratoFornecedor.ContratoId);
```


### ADICIONAR ao Janeiro

```csharp
            var objFromDb = _db.ViewContratoFornecedor.FirstOrDefault(s => s.ContratoId == viewContratoFornecedor.ContratoId);
```
