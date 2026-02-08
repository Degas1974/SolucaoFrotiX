# Repository/IRepository/IItensManutencaoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IItensManutencaoRepository.cs
+++ ATUAL: Repository/IRepository/IItensManutencaoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IItensManutencaoRepository : IRepository<ItensManutencao>
     {
-    public interface IItensManutencaoRepository : IRepository<ItensManutencao>
-        {
 
         IEnumerable<SelectListItem> GetItensManutencaoListForDropDown();
 
         void Update(ItensManutencao itensManutencao);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IItensManutencaoRepository : IRepository<ItensManutencao>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IItensManutencaoRepository : IRepository<ItensManutencao>
}
```
