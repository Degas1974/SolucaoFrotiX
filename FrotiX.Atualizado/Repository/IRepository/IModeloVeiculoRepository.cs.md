# Repository/IRepository/IModeloVeiculoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IModeloVeiculoRepository.cs
+++ ATUAL: Repository/IRepository/IModeloVeiculoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IModeloVeiculoRepository : IRepository<ModeloVeiculo>
     {
-    public interface IModeloVeiculoRepository : IRepository<ModeloVeiculo>
-        {
 
         IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown();
 
         void Update(ModeloVeiculo modeloVeiculo);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IModeloVeiculoRepository : IRepository<ModeloVeiculo>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IModeloVeiculoRepository : IRepository<ModeloVeiculo>
}
```
