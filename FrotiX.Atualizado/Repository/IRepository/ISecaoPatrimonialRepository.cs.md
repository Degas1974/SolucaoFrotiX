# Repository/IRepository/ISecaoPatrimonialRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ISecaoPatrimonialRepository.cs
+++ ATUAL: Repository/IRepository/ISecaoPatrimonialRepository.cs
@@ -9,13 +9,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ISecaoPatrimonialRepository : IRepository<SecaoPatrimonial>
     {
-    public interface ISecaoPatrimonialRepository : IRepository<SecaoPatrimonial>
-        {
 
         IEnumerable<SelectListItem> GetSecaoListForDropDown();
 
         void Update(SecaoPatrimonial secao);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ISecaoPatrimonialRepository : IRepository<SecaoPatrimonial>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ISecaoPatrimonialRepository : IRepository<SecaoPatrimonial>
}
```
