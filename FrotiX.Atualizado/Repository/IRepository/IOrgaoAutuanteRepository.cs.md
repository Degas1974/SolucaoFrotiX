# Repository/IRepository/IOrgaoAutuanteRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IOrgaoAutuanteRepository.cs
+++ ATUAL: Repository/IRepository/IOrgaoAutuanteRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IOrgaoAutuanteRepository : IRepository<OrgaoAutuante>
     {
-    public interface IOrgaoAutuanteRepository : IRepository<OrgaoAutuante>
-        {
 
         IEnumerable<SelectListItem> GetOrgaoAutuanteListForDropDown();
 
         void Update(OrgaoAutuante orgaoautuante);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IOrgaoAutuanteRepository : IRepository<OrgaoAutuante>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IOrgaoAutuanteRepository : IRepository<OrgaoAutuante>
}
```
