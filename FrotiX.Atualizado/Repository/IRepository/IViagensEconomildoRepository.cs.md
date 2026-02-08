# Repository/IRepository/IViagensEconomildoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViagensEconomildoRepository.cs
+++ ATUAL: Repository/IRepository/IViagensEconomildoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViagensEconomildoRepository : IRepository<ViagensEconomildo>
     {
-    public interface IViagensEconomildoRepository : IRepository<ViagensEconomildo>
-        {
 
         IEnumerable<SelectListItem> GetViagensEconomildoListForDropDown();
 
         void Update(ViagensEconomildo viagensEconomildo);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViagensEconomildoRepository : IRepository<ViagensEconomildo>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViagensEconomildoRepository : IRepository<ViagensEconomildo>
}
```
