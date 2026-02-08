# Repository/IRepository/ITipoMultaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ITipoMultaRepository.cs
+++ ATUAL: Repository/IRepository/ITipoMultaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ITipoMultaRepository : IRepository<TipoMulta>
     {
-    public interface ITipoMultaRepository : IRepository<TipoMulta>
-        {
 
         IEnumerable<SelectListItem> GetTipoMultaListForDropDown();
 
         void Update(TipoMulta tipomulta);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ITipoMultaRepository : IRepository<TipoMulta>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ITipoMultaRepository : IRepository<TipoMulta>
}
```
