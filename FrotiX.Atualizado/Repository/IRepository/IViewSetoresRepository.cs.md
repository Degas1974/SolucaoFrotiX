# Repository/IRepository/IViewSetoresRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewSetoresRepository.cs
+++ ATUAL: Repository/IRepository/IViewSetoresRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewSetoresRepository : IRepository<ViewSetores>
     {
-    public interface IViewSetoresRepository : IRepository<ViewSetores>
-        {
 
         IEnumerable<SelectListItem> GetViewSetoresListForDropDown();
 
         void Update(ViewSetores viewSetores);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewSetoresRepository : IRepository<ViewSetores>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewSetoresRepository : IRepository<ViewSetores>
}
```
