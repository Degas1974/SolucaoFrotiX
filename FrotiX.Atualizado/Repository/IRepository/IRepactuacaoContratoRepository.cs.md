# Repository/IRepository/IRepactuacaoContratoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRepactuacaoContratoRepository.cs
+++ ATUAL: Repository/IRepository/IRepactuacaoContratoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IRepactuacaoContratoRepository : IRepository<RepactuacaoContrato>
     {
-    public interface IRepactuacaoContratoRepository : IRepository<RepactuacaoContrato>
-        {
 
         IEnumerable<SelectListItem> GetRepactuacaoContratoListForDropDown();
 
         void Update(RepactuacaoContrato RepactuacaoContrato);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IRepactuacaoContratoRepository : IRepository<RepactuacaoContrato>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IRepactuacaoContratoRepository : IRepository<RepactuacaoContrato>
}
```
