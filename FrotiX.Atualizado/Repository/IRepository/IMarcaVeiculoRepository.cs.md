# Repository/IRepository/IMarcaVeiculoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMarcaVeiculoRepository.cs
+++ ATUAL: Repository/IRepository/IMarcaVeiculoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMarcaVeiculoRepository : IRepository<MarcaVeiculo>
     {
-    public interface IMarcaVeiculoRepository : IRepository<MarcaVeiculo>
-        {
 
         IEnumerable<SelectListItem> GetMarcaVeiculoListForDropDown();
 
         void Update(MarcaVeiculo marcaVeiculo);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMarcaVeiculoRepository : IRepository<MarcaVeiculo>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMarcaVeiculoRepository : IRepository<MarcaVeiculo>
}
```
