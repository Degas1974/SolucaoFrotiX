# Repository/IRepository/IRequisitanteRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRequisitanteRepository.cs
+++ ATUAL: Repository/IRepository/IRequisitanteRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IRequisitanteRepository : IRepository<Requisitante>
     {
-    public interface IRequisitanteRepository : IRepository<Requisitante>
-        {
 
         IEnumerable<SelectListItem> GetRequisitanteListForDropDown();
 
         void Update(Requisitante requisitante);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IRequisitanteRepository : IRepository<Requisitante>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IRequisitanteRepository : IRepository<Requisitante>
}
```
