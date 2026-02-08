# Repository/IRepository/IViewNoFichaVistoriaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewNoFichaVistoriaRepository.cs
+++ ATUAL: Repository/IRepository/IViewNoFichaVistoriaRepository.cs
@@ -6,9 +6,10 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewNoFichaVistoriaRepository : IRepository<ViewNoFichaVistoria>
     {
-    public interface IViewNoFichaVistoriaRepository : IRepository<ViewNoFichaVistoria>
-        {
 
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewNoFichaVistoriaRepository : IRepository<ViewNoFichaVistoria>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewNoFichaVistoriaRepository : IRepository<ViewNoFichaVistoria>
}
```
