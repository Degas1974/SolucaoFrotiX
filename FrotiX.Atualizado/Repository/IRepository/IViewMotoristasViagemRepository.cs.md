# Repository/IRepository/IViewMotoristasViagemRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewMotoristasViagemRepository.cs
+++ ATUAL: Repository/IRepository/IViewMotoristasViagemRepository.cs
@@ -7,13 +7,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewMotoristasViagemRepository : IRepository<ViewMotoristasViagem>
     {
-    public interface IViewMotoristasViagemRepository : IRepository<ViewMotoristasViagem>
-        {
 
         IEnumerable<SelectListItem> GetViewMotoristasViagemListForDropDown();
 
         void Update(ViewMotoristasViagem viewMotoristasViagem);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewMotoristasViagemRepository : IRepository<ViewMotoristasViagem>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewMotoristasViagemRepository : IRepository<ViewMotoristasViagem>
}
```
