# Repository/IRepository/IViewMotoristaFluxo.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewMotoristaFluxo.cs
+++ ATUAL: Repository/IRepository/IViewMotoristaFluxo.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewMotoristaFluxoRepository : IRepository<ViewMotoristaFluxo>
     {
-    public interface IViewMotoristaFluxoRepository : IRepository<ViewMotoristaFluxo>
-        {
 
         IEnumerable<SelectListItem> GetViewMotoristaFluxoListForDropDown();
 
         void Update(ViewMotoristaFluxo viewMotoristaFluxo);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewMotoristaFluxoRepository : IRepository<ViewMotoristaFluxo>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewMotoristaFluxoRepository : IRepository<ViewMotoristaFluxo>
}
```
