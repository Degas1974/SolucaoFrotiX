# Repository/IRepository/IViewEmpenhoMultaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewEmpenhoMultaRepository.cs
+++ ATUAL: Repository/IRepository/IViewEmpenhoMultaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewEmpenhoMultaRepository : IRepository<ViewEmpenhoMulta>
     {
-    public interface IViewEmpenhoMultaRepository : IRepository<ViewEmpenhoMulta>
-        {
 
         IEnumerable<SelectListItem> GetViewEmpenhoMultaListForDropDown();
 
         void Update(ViewEmpenhoMulta viewEmpenhos);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewEmpenhoMultaRepository : IRepository<ViewEmpenhoMulta>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewEmpenhoMultaRepository : IRepository<ViewEmpenhoMulta>
}
```
