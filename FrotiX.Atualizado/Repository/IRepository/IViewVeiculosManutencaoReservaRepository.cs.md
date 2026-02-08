# Repository/IRepository/IViewVeiculosManutencaoReservaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewVeiculosManutencaoReservaRepository.cs
+++ ATUAL: Repository/IRepository/IViewVeiculosManutencaoReservaRepository.cs
@@ -3,11 +3,13 @@
 using System.Collections.Generic;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewVeiculosManutencaoReservaRepository : IRepository<ViewVeiculosManutencaoReserva>
     {
-    public interface IViewVeiculosManutencaoReservaRepository : IRepository<ViewVeiculosManutencaoReserva>
-        {
+
         IEnumerable<SelectListItem> GetViewVeiculosManutencaoReservaListForDropDown();
 
         void Update(ViewVeiculosManutencaoReserva viewVeiculosManutencaoReserva);
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewVeiculosManutencaoReservaRepository : IRepository<ViewVeiculosManutencaoReserva>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewVeiculosManutencaoReservaRepository : IRepository<ViewVeiculosManutencaoReserva>
}
```
