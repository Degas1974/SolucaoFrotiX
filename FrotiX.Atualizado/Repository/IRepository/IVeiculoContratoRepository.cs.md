# Repository/IRepository/IVeiculoContratoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IVeiculoContratoRepository.cs
+++ ATUAL: Repository/IRepository/IVeiculoContratoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IVeiculoContratoRepository : IRepository<VeiculoContrato>
     {
-    public interface IVeiculoContratoRepository : IRepository<VeiculoContrato>
-        {
 
         IEnumerable<SelectListItem> GetVeiculoContratoListForDropDown();
 
         void Update(VeiculoContrato VeiculoContrato);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IVeiculoContratoRepository : IRepository<VeiculoContrato>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IVeiculoContratoRepository : IRepository<VeiculoContrato>
}
```
