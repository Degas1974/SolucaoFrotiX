# Repository/IRepository/IRepactuacaoTerceirizacaoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRepactuacaoTerceirizacaoRepository.cs
+++ ATUAL: Repository/IRepository/IRepactuacaoTerceirizacaoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IRepactuacaoTerceirizacaoRepository : IRepository<RepactuacaoTerceirizacao>
     {
-    public interface IRepactuacaoTerceirizacaoRepository : IRepository<RepactuacaoTerceirizacao>
-        {
 
         IEnumerable<SelectListItem> GetRepactuacaoTerceirizacaoListForDropDown();
 
         void Update(RepactuacaoTerceirizacao RepactuacaoTerceirizacao);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IRepactuacaoTerceirizacaoRepository : IRepository<RepactuacaoTerceirizacao>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IRepactuacaoTerceirizacaoRepository : IRepository<RepactuacaoTerceirizacao>
}
```
