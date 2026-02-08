# Repository/IRepository/IViewContratoFornecedor.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewContratoFornecedor.cs
+++ ATUAL: Repository/IRepository/IViewContratoFornecedor.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewContratoFornecedorRepository : IRepository<ViewContratoFornecedor>
     {
-    public interface IViewContratoFornecedorRepository : IRepository<ViewContratoFornecedor>
-        {
 
         IEnumerable<SelectListItem> GetViewContratoFornecedorListForDropDown();
 
         void Update(ViewContratoFornecedor viewContratoFornecedor);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewContratoFornecedorRepository : IRepository<ViewContratoFornecedor>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewContratoFornecedorRepository : IRepository<ViewContratoFornecedor>
}
```
