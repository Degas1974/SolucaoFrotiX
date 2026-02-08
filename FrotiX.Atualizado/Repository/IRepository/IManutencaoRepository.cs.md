# Repository/IRepository/IManutencaoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IManutencaoRepository.cs
+++ ATUAL: Repository/IRepository/IManutencaoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IManutencaoRepository : IRepository<Manutencao>
     {
-    public interface IManutencaoRepository : IRepository<Manutencao>
-        {
 
         IEnumerable<SelectListItem> GetManutencaoListForDropDown();
 
         void Update(Manutencao manutencao);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IManutencaoRepository : IRepository<Manutencao>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IManutencaoRepository : IRepository<Manutencao>
}
```
