# Repository/IRepository/ISetorPatrimonialRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ISetorPatrimonialRepository.cs
+++ ATUAL: Repository/IRepository/ISetorPatrimonialRepository.cs
@@ -9,13 +9,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ISetorPatrimonialRepository : IRepository<SetorPatrimonial>
     {
-    public interface ISetorPatrimonialRepository : IRepository<SetorPatrimonial>
-        {
 
         IEnumerable<SelectListItem> GetSetorListForDropDown();
 
         void Update(SetorPatrimonial setor);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ISetorPatrimonialRepository : IRepository<SetorPatrimonial>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ISetorPatrimonialRepository : IRepository<SetorPatrimonial>
}
```
