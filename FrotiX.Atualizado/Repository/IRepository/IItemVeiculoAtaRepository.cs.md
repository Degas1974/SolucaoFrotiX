# Repository/IRepository/IItemVeiculoAtaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IItemVeiculoAtaRepository.cs
+++ ATUAL: Repository/IRepository/IItemVeiculoAtaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IItemVeiculoAtaRepository : IRepository<ItemVeiculoAta>
     {
-    public interface IItemVeiculoAtaRepository : IRepository<ItemVeiculoAta>
-        {
 
         IEnumerable<SelectListItem> GetItemVeiculoAtaListForDropDown();
 
         void Update(ItemVeiculoAta itemveiculoata);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IItemVeiculoAtaRepository : IRepository<ItemVeiculoAta>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IItemVeiculoAtaRepository : IRepository<ItemVeiculoAta>
}
```
