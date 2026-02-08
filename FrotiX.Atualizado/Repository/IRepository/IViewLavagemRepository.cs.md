# Repository/IRepository/IViewLavagemRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewLavagemRepository.cs
+++ ATUAL: Repository/IRepository/IViewLavagemRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewLavagemRepository : IRepository<ViewLavagem>
     {
-    public interface IViewLavagemRepository : IRepository<ViewLavagem>
-        {
 
         IEnumerable<SelectListItem> GetViewLavagemListForDropDown();
 
         void Update(ViewLavagem viewLavagem);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewLavagemRepository : IRepository<ViewLavagem>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewLavagemRepository : IRepository<ViewLavagem>
}
```
