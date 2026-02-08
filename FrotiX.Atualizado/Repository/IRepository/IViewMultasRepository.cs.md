# Repository/IRepository/IViewMultasRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewMultasRepository.cs
+++ ATUAL: Repository/IRepository/IViewMultasRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IviewMultasRepository : IRepository<ViewMultas>
     {
-    public interface IviewMultasRepository : IRepository<ViewMultas>
-        {
 
         IEnumerable<SelectListItem> GetviewMultasListForDropDown();
 
         void Update(ViewMultas viewMultas);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IviewMultasRepository : IRepository<ViewMultas>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IviewMultasRepository : IRepository<ViewMultas>
}
```
