# Repository/IRepository/IRepactuacaoAtaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRepactuacaoAtaRepository.cs
+++ ATUAL: Repository/IRepository/IRepactuacaoAtaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IRepactuacaoAtaRepository : IRepository<RepactuacaoAta>
     {
-    public interface IRepactuacaoAtaRepository : IRepository<RepactuacaoAta>
-        {
 
         IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown();
 
         void Update(RepactuacaoAta repactuacaoitemveiculoata);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IRepactuacaoAtaRepository : IRepository<RepactuacaoAta>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IRepactuacaoAtaRepository : IRepository<RepactuacaoAta>
}
```
