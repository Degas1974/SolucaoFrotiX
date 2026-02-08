# Repository/IRepository/IRecursoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRecursoRepository.cs
+++ ATUAL: Repository/IRepository/IRecursoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IRecursoRepository : IRepository<Recurso>
     {
-    public interface IRecursoRepository : IRepository<Recurso>
-        {
 
         IEnumerable<SelectListItem> GetRecursoListForDropDown();
 
         void Update(Recurso recurso);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IRecursoRepository : IRepository<Recurso>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IRecursoRepository : IRepository<Recurso>
}
```
