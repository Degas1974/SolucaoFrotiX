# Repository/IRepository/IRepactuacaoServicosRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRepactuacaoServicosRepository.cs
+++ ATUAL: Repository/IRepository/IRepactuacaoServicosRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IRepactuacaoServicosRepository : IRepository<RepactuacaoServicos>
     {
-    public interface IRepactuacaoServicosRepository : IRepository<RepactuacaoServicos>
-        {
 
         IEnumerable<SelectListItem> GetRepactuacaoServicosListForDropDown();
 
         void Update(RepactuacaoServicos RepactuacaoServicos);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IRepactuacaoServicosRepository : IRepository<RepactuacaoServicos>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IRepactuacaoServicosRepository : IRepository<RepactuacaoServicos>
}
```
