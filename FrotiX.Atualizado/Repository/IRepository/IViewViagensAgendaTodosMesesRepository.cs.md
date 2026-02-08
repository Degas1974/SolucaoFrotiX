# Repository/IRepository/IViewViagensAgendaTodosMesesRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewViagensAgendaTodosMesesRepository.cs
+++ ATUAL: Repository/IRepository/IViewViagensAgendaTodosMesesRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewViagensAgendaTodosMesesRepository : IRepository<ViewViagensAgendaTodosMeses>
     {
-    public interface IViewViagensAgendaTodosMesesRepository : IRepository<ViewViagensAgendaTodosMeses>
-        {
 
         IEnumerable<SelectListItem> GetViewViagensAgendaTodosMesesListForDropDown();
 
         void Update(ViewViagensAgendaTodosMeses viewViagensAgendaMeses);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewViagensAgendaTodosMesesRepository : IRepository<ViewViagensAgendaTodosMeses>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewViagensAgendaTodosMesesRepository : IRepository<ViewViagensAgendaTodosMeses>
}
```
