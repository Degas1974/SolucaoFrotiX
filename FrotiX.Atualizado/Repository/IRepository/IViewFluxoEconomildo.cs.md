# Repository/IRepository/IViewFluxoEconomildo.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewFluxoEconomildo.cs
+++ ATUAL: Repository/IRepository/IViewFluxoEconomildo.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewFluxoEconomildoRepository : IRepository<ViewFluxoEconomildo>
     {
-    public interface IViewFluxoEconomildoRepository : IRepository<ViewFluxoEconomildo>
-        {
 
         IEnumerable<SelectListItem> GetViewFluxoEconomildoListForDropDown();
 
         void Update(ViewFluxoEconomildo viewFluxoEconomildo);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewFluxoEconomildoRepository : IRepository<ViewFluxoEconomildo>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewFluxoEconomildoRepository : IRepository<ViewFluxoEconomildo>
}
```
