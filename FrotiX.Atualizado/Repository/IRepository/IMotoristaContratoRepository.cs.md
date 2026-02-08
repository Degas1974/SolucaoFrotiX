# Repository/IRepository/IMotoristaContratoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMotoristaContratoRepository.cs
+++ ATUAL: Repository/IRepository/IMotoristaContratoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMotoristaContratoRepository : IRepository<MotoristaContrato>
     {
-    public interface IMotoristaContratoRepository : IRepository<MotoristaContrato>
-        {
 
         IEnumerable<SelectListItem> GetMotoristaContratoListForDropDown();
 
         void Update(MotoristaContrato MotoristaContrato);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMotoristaContratoRepository : IRepository<MotoristaContrato>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMotoristaContratoRepository : IRepository<MotoristaContrato>
}
```
