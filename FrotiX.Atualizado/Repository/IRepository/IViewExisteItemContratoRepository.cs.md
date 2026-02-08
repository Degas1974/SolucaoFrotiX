# Repository/IRepository/IViewExisteItemContratoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewExisteItemContratoRepository.cs
+++ ATUAL: Repository/IRepository/IViewExisteItemContratoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewExisteItemContratoRepository : IRepository<ViewExisteItemContrato>
     {
-    public interface IViewExisteItemContratoRepository : IRepository<ViewExisteItemContrato>
-        {
 
         IEnumerable<SelectListItem> GetViewExisteItemContratoListForDropDown();
 
         void Update(ViewExisteItemContrato viewExisteItemContrato);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewExisteItemContratoRepository : IRepository<ViewExisteItemContrato>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewExisteItemContratoRepository : IRepository<ViewExisteItemContrato>
}
```
