# Repository/IRepository/IViewControleAcessoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewControleAcessoRepository.cs
+++ ATUAL: Repository/IRepository/IViewControleAcessoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewControleAcessoRepository : IRepository<ViewControleAcesso>
     {
-    public interface IViewControleAcessoRepository : IRepository<ViewControleAcesso>
-        {
 
         IEnumerable<SelectListItem> GetViewControleAcessoListForDropDown();
 
         void Update(ViewControleAcesso viewControleAcesso);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewControleAcessoRepository : IRepository<ViewControleAcesso>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewControleAcessoRepository : IRepository<ViewControleAcesso>
}
```
