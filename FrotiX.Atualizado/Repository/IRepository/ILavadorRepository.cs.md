# Repository/IRepository/ILavadorRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ILavadorRepository.cs
+++ ATUAL: Repository/IRepository/ILavadorRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ILavadorRepository : IRepository<Lavador>
     {
-    public interface ILavadorRepository : IRepository<Lavador>
-        {
 
         IEnumerable<SelectListItem> GetLavadorListForDropDown();
 
         void Update(Lavador lavador);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ILavadorRepository : IRepository<Lavador>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ILavadorRepository : IRepository<Lavador>
}
```
