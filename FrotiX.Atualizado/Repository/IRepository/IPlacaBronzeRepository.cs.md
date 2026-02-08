# Repository/IRepository/IPlacaBronzeRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IPlacaBronzeRepository.cs
+++ ATUAL: Repository/IRepository/IPlacaBronzeRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IPlacaBronzeRepository : IRepository<PlacaBronze>
     {
-    public interface IPlacaBronzeRepository : IRepository<PlacaBronze>
-        {
 
         IEnumerable<SelectListItem> GetPlacaBronzeListForDropDown();
 
         void Update(PlacaBronze placaBronze);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IPlacaBronzeRepository : IRepository<PlacaBronze>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IPlacaBronzeRepository : IRepository<PlacaBronze>
}
```
