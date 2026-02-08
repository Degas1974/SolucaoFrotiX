# Repository/IRepository/IViewFluxoEconomildoDataRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewFluxoEconomildoDataRepository.cs
+++ ATUAL: Repository/IRepository/IViewFluxoEconomildoDataRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewFluxoEconomildoDataRepository : IRepository<ViewFluxoEconomildoData>
     {
-    public interface IViewFluxoEconomildoDataRepository : IRepository<ViewFluxoEconomildoData>
-        {
 
         IEnumerable<SelectListItem> GetViewFluxoEconomildoDataListForDropDown();
 
         void Update(ViewFluxoEconomildoData viewFluxoEconomildoData);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewFluxoEconomildoDataRepository : IRepository<ViewFluxoEconomildoData>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewFluxoEconomildoDataRepository : IRepository<ViewFluxoEconomildoData>
}
```
