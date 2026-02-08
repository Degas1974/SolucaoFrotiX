# Repository/IRepository/IViewEmpenhosRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewEmpenhosRepository.cs
+++ ATUAL: Repository/IRepository/IViewEmpenhosRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewEmpenhosRepository : IRepository<ViewEmpenhos>
     {
-    public interface IViewEmpenhosRepository : IRepository<ViewEmpenhos>
-        {
 
         IEnumerable<SelectListItem> GetViewEmpenhosListForDropDown();
 
         void Update(ViewEmpenhos viewEmpenhos);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewEmpenhosRepository : IRepository<ViewEmpenhos>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewEmpenhosRepository : IRepository<ViewEmpenhos>
}
```
