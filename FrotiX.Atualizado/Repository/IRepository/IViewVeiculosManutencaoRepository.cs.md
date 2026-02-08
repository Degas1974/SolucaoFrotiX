# Repository/IRepository/IViewVeiculosManutencaoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewVeiculosManutencaoRepository.cs
+++ ATUAL: Repository/IRepository/IViewVeiculosManutencaoRepository.cs
@@ -3,11 +3,13 @@
 using System.Collections.Generic;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewVeiculosManutencaoRepository : IRepository<ViewVeiculosManutencao>
     {
-    public interface IViewVeiculosManutencaoRepository : IRepository<ViewVeiculosManutencao>
-        {
+
         IEnumerable<SelectListItem> GetViewVeiculosManutencaoListForDropDown();
 
         void Update(ViewVeiculosManutencao viewVeiculosManutencao);
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewVeiculosManutencaoRepository : IRepository<ViewVeiculosManutencao>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewVeiculosManutencaoRepository : IRepository<ViewVeiculosManutencao>
}
```
