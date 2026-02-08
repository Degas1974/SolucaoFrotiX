# Repository/IRepository/IMotoristaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMotoristaRepository.cs
+++ ATUAL: Repository/IRepository/IMotoristaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMotoristaRepository : IRepository<Motorista>
     {
-    public interface IMotoristaRepository : IRepository<Motorista>
-        {
 
         IEnumerable<SelectListItem> GetMotoristaListForDropDown();
 
         void Update(Motorista motorista);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMotoristaRepository : IRepository<Motorista>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMotoristaRepository : IRepository<Motorista>
}
```
