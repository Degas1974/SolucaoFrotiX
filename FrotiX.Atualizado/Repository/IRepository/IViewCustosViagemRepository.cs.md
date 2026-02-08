# Repository/IRepository/IViewCustosViagemRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewCustosViagemRepository.cs
+++ ATUAL: Repository/IRepository/IViewCustosViagemRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewCustosViagemRepository : IRepository<ViewCustosViagem>
     {
-    public interface IViewCustosViagemRepository : IRepository<ViewCustosViagem>
-        {
 
         IEnumerable<SelectListItem> GetViewCustosViagemListForDropDown();
 
         void Update(ViewCustosViagem ViewCustosViagem);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewCustosViagemRepository : IRepository<ViewCustosViagem>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewCustosViagemRepository : IRepository<ViewCustosViagem>
}
```
