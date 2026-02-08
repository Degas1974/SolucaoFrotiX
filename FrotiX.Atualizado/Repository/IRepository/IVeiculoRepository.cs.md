# Repository/IRepository/IVeiculoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IVeiculoRepository.cs
+++ ATUAL: Repository/IRepository/IVeiculoRepository.cs
@@ -6,15 +6,15 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IVeiculoRepository : IRepository<Veiculo>
     {
-    public interface IVeiculoRepository : IRepository<Veiculo>
-        {
 
         IEnumerable<SelectListItem> GetVeiculoListForDropDown();
 
         void Update(Veiculo veiculo);
 
         IEnumerable<SelectListItem> GetVeiculoCompletoListForDropDown();
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IVeiculoRepository : IRepository<Veiculo>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IVeiculoRepository : IRepository<Veiculo>
}
```
