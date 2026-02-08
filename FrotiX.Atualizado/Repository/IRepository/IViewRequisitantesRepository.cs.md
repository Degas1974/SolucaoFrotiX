# Repository/IRepository/IViewRequisitantesRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewRequisitantesRepository.cs
+++ ATUAL: Repository/IRepository/IViewRequisitantesRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewRequisitantesRepository : IRepository<ViewRequisitantes>
     {
-    public interface IViewRequisitantesRepository : IRepository<ViewRequisitantes>
-        {
 
         IEnumerable<SelectListItem> GetViewRequisitantesListForDropDown();
 
         void Update(ViewRequisitantes viewRequisitantes);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewRequisitantesRepository : IRepository<ViewRequisitantes>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewRequisitantesRepository : IRepository<ViewRequisitantes>
}
```
