# Repository/IRepository/IViewManutencaoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewManutencaoRepository.cs
+++ ATUAL: Repository/IRepository/IViewManutencaoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewManutencaoRepository : IRepository<ViewManutencao>
     {
-    public interface IViewManutencaoRepository : IRepository<ViewManutencao>
-        {
 
         IEnumerable<SelectListItem> GetViewManutencaoListForDropDown();
 
         void Update(ViewManutencao viewManutencao);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewManutencaoRepository : IRepository<ViewManutencao>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewManutencaoRepository : IRepository<ViewManutencao>
}
```
