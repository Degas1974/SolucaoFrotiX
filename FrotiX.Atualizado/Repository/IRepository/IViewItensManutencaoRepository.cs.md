# Repository/IRepository/IViewItensManutencaoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewItensManutencaoRepository.cs
+++ ATUAL: Repository/IRepository/IViewItensManutencaoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewItensManutencaoRepository : IRepository<ViewItensManutencao>
     {
-    public interface IViewItensManutencaoRepository : IRepository<ViewItensManutencao>
-        {
 
         IEnumerable<SelectListItem> GetViewItensManutencaoListForDropDown();
 
         void Update(ViewItensManutencao viewItensManutencao);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewItensManutencaoRepository : IRepository<ViewItensManutencao>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewItensManutencaoRepository : IRepository<ViewItensManutencao>
}
```
