# Repository/IRepository/IVeiculoAtaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IVeiculoAtaRepository.cs
+++ ATUAL: Repository/IRepository/IVeiculoAtaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IVeiculoAtaRepository : IRepository<VeiculoAta>
     {
-    public interface IVeiculoAtaRepository : IRepository<VeiculoAta>
-        {
 
         IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown();
 
         void Update(VeiculoAta VeiculoAta);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IVeiculoAtaRepository : IRepository<VeiculoAta>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IVeiculoAtaRepository : IRepository<VeiculoAta>
}
```
