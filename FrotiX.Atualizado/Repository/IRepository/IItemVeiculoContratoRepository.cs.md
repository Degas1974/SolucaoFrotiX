# Repository/IRepository/IItemVeiculoContratoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IItemVeiculoContratoRepository.cs
+++ ATUAL: Repository/IRepository/IItemVeiculoContratoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IItemVeiculoContratoRepository : IRepository<ItemVeiculoContrato>
     {
-    public interface IItemVeiculoContratoRepository : IRepository<ItemVeiculoContrato>
-        {
 
         IEnumerable<SelectListItem> GetItemVeiculoContratoListForDropDown();
 
         void Update(ItemVeiculoContrato itemveiculocontrato);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IItemVeiculoContratoRepository : IRepository<ItemVeiculoContrato>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IItemVeiculoContratoRepository : IRepository<ItemVeiculoContrato>
}
```
