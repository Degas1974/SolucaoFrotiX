# Repository/IRepository/IViewOcorrencia.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewOcorrencia.cs
+++ ATUAL: Repository/IRepository/IViewOcorrencia.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewOcorrenciaRepository : IRepository<ViewOcorrencia>
     {
-    public interface IViewOcorrenciaRepository : IRepository<ViewOcorrencia>
-        {
 
         IEnumerable<SelectListItem> GetViewOcorrenciaListForDropDown();
 
         void Update(ViewOcorrencia viewOcorrencia);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewOcorrenciaRepository : IRepository<ViewOcorrencia>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewOcorrenciaRepository : IRepository<ViewOcorrencia>
}
```
