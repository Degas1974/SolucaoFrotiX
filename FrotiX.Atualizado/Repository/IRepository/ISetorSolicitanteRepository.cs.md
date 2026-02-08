# Repository/IRepository/ISetorSolicitanteRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ISetorSolicitanteRepository.cs
+++ ATUAL: Repository/IRepository/ISetorSolicitanteRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ISetorSolicitanteRepository : IRepository<SetorSolicitante>
     {
-    public interface ISetorSolicitanteRepository : IRepository<SetorSolicitante>
-        {
 
         IEnumerable<SelectListItem> GetSetorSolicitanteListForDropDown();
 
         void Update(SetorSolicitante setorSolicitante);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ISetorSolicitanteRepository : IRepository<SetorSolicitante>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ISetorSolicitanteRepository : IRepository<SetorSolicitante>
}
```
