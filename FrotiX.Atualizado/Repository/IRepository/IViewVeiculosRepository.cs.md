# Repository/IRepository/IViewVeiculosRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewVeiculosRepository.cs
+++ ATUAL: Repository/IRepository/IViewVeiculosRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewVeiculosRepository : IRepository<ViewVeiculos>
     {
-    public interface IViewVeiculosRepository : IRepository<ViewVeiculos>
-        {
 
         IEnumerable<SelectListItem> GetViewVeiculosListForDropDown();
 
         void Update(ViewVeiculos viewVeiculos);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewVeiculosRepository : IRepository<ViewVeiculos>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewVeiculosRepository : IRepository<ViewVeiculos>
}
```
