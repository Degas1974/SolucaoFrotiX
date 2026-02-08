# Repository/IRepository/IViewLotacoesRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewLotacoesRepository.cs
+++ ATUAL: Repository/IRepository/IViewLotacoesRepository.cs
@@ -6,9 +6,10 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewLotacoesRepository : IRepository<ViewLotacoes>
     {
-    public interface IViewLotacoesRepository : IRepository<ViewLotacoes>
-        {
 
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewLotacoesRepository : IRepository<ViewLotacoes>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewLotacoesRepository : IRepository<ViewLotacoes>
}
```
