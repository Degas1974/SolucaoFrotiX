# Repository/IRepository/IPatrimonioRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IPatrimonioRepository.cs
+++ ATUAL: Repository/IRepository/IPatrimonioRepository.cs
@@ -9,13 +9,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IPatrimonioRepository : IRepository<Patrimonio>
     {
-    public interface IPatrimonioRepository : IRepository<Patrimonio>
-        {
 
         IEnumerable<SelectListItem> GetPatrimonioListForDropDown();
 
         void Update(Patrimonio patrimonio);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IPatrimonioRepository : IRepository<Patrimonio>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IPatrimonioRepository : IRepository<Patrimonio>
}
```
