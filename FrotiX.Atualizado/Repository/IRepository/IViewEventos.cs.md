# Repository/IRepository/IViewEventos.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewEventos.cs
+++ ATUAL: Repository/IRepository/IViewEventos.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewEventosRepository : IRepository<ViewEventos>
     {
-    public interface IViewEventosRepository : IRepository<ViewEventos>
-        {
 
         IEnumerable<SelectListItem> GetViewEventosListForDropDown();
 
         void Update(ViewEventos viewEventos);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewEventosRepository : IRepository<ViewEventos>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewEventosRepository : IRepository<ViewEventos>
}
```
