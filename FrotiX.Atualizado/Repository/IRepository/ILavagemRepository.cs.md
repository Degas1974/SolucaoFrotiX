# Repository/IRepository/ILavagemRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ILavagemRepository.cs
+++ ATUAL: Repository/IRepository/ILavagemRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ILavagemRepository : IRepository<Lavagem>
     {
-    public interface ILavagemRepository : IRepository<Lavagem>
-        {
 
         IEnumerable<SelectListItem> GetLavagemListForDropDown();
 
         void Update(Lavagem lavagem);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ILavagemRepository : IRepository<Lavagem>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ILavagemRepository : IRepository<Lavagem>
}
```
