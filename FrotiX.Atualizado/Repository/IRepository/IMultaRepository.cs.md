# Repository/IRepository/IMultaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMultaRepository.cs
+++ ATUAL: Repository/IRepository/IMultaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMultaRepository : IRepository<Multa>
     {
-    public interface IMultaRepository : IRepository<Multa>
-        {
 
         IEnumerable<SelectListItem> GetMultaListForDropDown();
 
         void Update(Multa multa);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMultaRepository : IRepository<Multa>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMultaRepository : IRepository<Multa>
}
```
