# Repository/IRepository/IViewAtaFornecedor.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewAtaFornecedor.cs
+++ ATUAL: Repository/IRepository/IViewAtaFornecedor.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewAtaFornecedorRepository : IRepository<ViewAtaFornecedor>
     {
-    public interface IViewAtaFornecedorRepository : IRepository<ViewAtaFornecedor>
-        {
 
         IEnumerable<SelectListItem> GetViewAtaFornecedorListForDropDown();
 
         void Update(ViewAtaFornecedor viewAtaFornecedor);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewAtaFornecedorRepository : IRepository<ViewAtaFornecedor>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewAtaFornecedorRepository : IRepository<ViewAtaFornecedor>
}
```
