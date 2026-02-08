# Repository/IRepository/IUnidadeRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IUnidadeRepository.cs
+++ ATUAL: Repository/IRepository/IUnidadeRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IUnidadeRepository : IRepository<Unidade>
     {
-    public interface IUnidadeRepository : IRepository<Unidade>
-        {
 
         IEnumerable<SelectListItem> GetUnidadeListForDropDown();
 
         void Update(Unidade unidade);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IUnidadeRepository : IRepository<Unidade>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IUnidadeRepository : IRepository<Unidade>
}
```
