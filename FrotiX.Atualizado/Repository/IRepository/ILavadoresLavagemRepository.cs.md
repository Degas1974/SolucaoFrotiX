# Repository/IRepository/ILavadoresLavagemRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ILavadoresLavagemRepository.cs
+++ ATUAL: Repository/IRepository/ILavadoresLavagemRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ILavadoresLavagemRepository : IRepository<LavadoresLavagem>
     {
-    public interface ILavadoresLavagemRepository : IRepository<LavadoresLavagem>
-        {
 
         IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown();
 
         void Update(LavadoresLavagem lavadoresLavagem);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ILavadoresLavagemRepository : IRepository<LavadoresLavagem>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ILavadoresLavagemRepository : IRepository<LavadoresLavagem>
}
```
