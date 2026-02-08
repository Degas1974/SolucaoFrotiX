# Repository/IRepository/IOperadorRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IOperadorRepository.cs
+++ ATUAL: Repository/IRepository/IOperadorRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IOperadorRepository : IRepository<Operador>
     {
-    public interface IOperadorRepository : IRepository<Operador>
-        {
 
         IEnumerable<SelectListItem> GetOperadorListForDropDown();
 
         void Update(Operador operador);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IOperadorRepository : IRepository<Operador>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IOperadorRepository : IRepository<Operador>
}
```
