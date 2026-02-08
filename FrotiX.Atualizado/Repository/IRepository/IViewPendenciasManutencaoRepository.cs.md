# Repository/IRepository/IViewPendenciasManutencaoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewPendenciasManutencaoRepository.cs
+++ ATUAL: Repository/IRepository/IViewPendenciasManutencaoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewPendenciasManutencaoRepository : IRepository<ViewPendenciasManutencao>
     {
-    public interface IViewPendenciasManutencaoRepository : IRepository<ViewPendenciasManutencao>
-        {
 
         IEnumerable<SelectListItem> GetViewPendenciasManutencaoListForDropDown();
 
         void Update(ViewPendenciasManutencao viewPendenciasManutencao);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewPendenciasManutencaoRepository : IRepository<ViewPendenciasManutencao>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewPendenciasManutencaoRepository : IRepository<ViewPendenciasManutencao>
}
```
