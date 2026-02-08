# Repository/IRepository/IViewAbastecimentosRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewAbastecimentosRepository.cs
+++ ATUAL: Repository/IRepository/IViewAbastecimentosRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewAbastecimentosRepository : IRepository<ViewAbastecimentos>
     {
-    public interface IViewAbastecimentosRepository : IRepository<ViewAbastecimentos>
-        {
 
         IEnumerable<SelectListItem> GetViewAbastecimentosListForDropDown();
 
         void Update(ViewAbastecimentos viewAbastecimentos);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewAbastecimentosRepository : IRepository<ViewAbastecimentos>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewAbastecimentosRepository : IRepository<ViewAbastecimentos>
}
```
