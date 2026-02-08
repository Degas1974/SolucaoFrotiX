# Repository/IRepository/IViewMotoristasRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewMotoristasRepository.cs
+++ ATUAL: Repository/IRepository/IViewMotoristasRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewMotoristasRepository : IRepository<ViewMotoristas>
     {
-    public interface IViewMotoristasRepository : IRepository<ViewMotoristas>
-        {
 
         IEnumerable<SelectListItem> GetViewMotoristasListForDropDown();
 
         void Update(ViewMotoristas viewMotoristas);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewMotoristasRepository : IRepository<ViewMotoristas>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewMotoristasRepository : IRepository<ViewMotoristas>
}
```
