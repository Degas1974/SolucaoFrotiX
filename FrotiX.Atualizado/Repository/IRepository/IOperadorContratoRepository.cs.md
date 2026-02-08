# Repository/IRepository/IOperadorContratoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IOperadorContratoRepository.cs
+++ ATUAL: Repository/IRepository/IOperadorContratoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IOperadorContratoRepository : IRepository<OperadorContrato>
     {
-    public interface IOperadorContratoRepository : IRepository<OperadorContrato>
-        {
 
         IEnumerable<SelectListItem> GetOperadorContratoListForDropDown();
 
         void Update(OperadorContrato OperadorContrato);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IOperadorContratoRepository : IRepository<OperadorContrato>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IOperadorContratoRepository : IRepository<OperadorContrato>
}
```
