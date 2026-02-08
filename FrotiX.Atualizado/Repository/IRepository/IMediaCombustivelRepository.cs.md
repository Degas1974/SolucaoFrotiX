# Repository/IRepository/IMediaCombustivelRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMediaCombustivelRepository.cs
+++ ATUAL: Repository/IRepository/IMediaCombustivelRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMediaCombustivelRepository : IRepository<MediaCombustivel>
     {
-    public interface IMediaCombustivelRepository : IRepository<MediaCombustivel>
-        {
 
         IEnumerable<SelectListItem> GetMediaCombustivelListForDropDown();
 
         void Update(MediaCombustivel mediacombustivel);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMediaCombustivelRepository : IRepository<MediaCombustivel>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMediaCombustivelRepository : IRepository<MediaCombustivel>
}
```
