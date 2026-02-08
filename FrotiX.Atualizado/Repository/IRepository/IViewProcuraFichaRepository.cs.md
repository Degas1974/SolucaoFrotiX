# Repository/IRepository/IViewProcuraFichaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewProcuraFichaRepository.cs
+++ ATUAL: Repository/IRepository/IViewProcuraFichaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewProcuraFichaRepository : IRepository<ViewProcuraFicha>
     {
-    public interface IViewProcuraFichaRepository : IRepository<ViewProcuraFicha>
-        {
 
         IEnumerable<SelectListItem> GetViewProcuraFichaListForDropDown();
 
         void Update(ViewProcuraFicha viewProcuraFicha);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewProcuraFichaRepository : IRepository<ViewProcuraFicha>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewProcuraFichaRepository : IRepository<ViewProcuraFicha>
}
```
