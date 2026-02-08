# Repository/IRepository/ILavadorContratoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ILavadorContratoRepository.cs
+++ ATUAL: Repository/IRepository/ILavadorContratoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ILavadorContratoRepository : IRepository<LavadorContrato>
     {
-    public interface ILavadorContratoRepository : IRepository<LavadorContrato>
-        {
 
         IEnumerable<SelectListItem> GetLavadorContratoListForDropDown();
 
         void Update(LavadorContrato LavadorContrato);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ILavadorContratoRepository : IRepository<LavadorContrato>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ILavadorContratoRepository : IRepository<LavadorContrato>
}
```
