# Repository/IRepository/IViewMediaConsumoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewMediaConsumoRepository.cs
+++ ATUAL: Repository/IRepository/IViewMediaConsumoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewMediaConsumoRepository : IRepository<ViewMediaConsumo>
     {
-    public interface IViewMediaConsumoRepository : IRepository<ViewMediaConsumo>
-        {
 
         IEnumerable<SelectListItem> GetViewMediaConsumoListForDropDown();
 
         void Update(ViewMediaConsumo viewMediaConsumo);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewMediaConsumoRepository : IRepository<ViewMediaConsumo>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewMediaConsumoRepository : IRepository<ViewMediaConsumo>
}
```
