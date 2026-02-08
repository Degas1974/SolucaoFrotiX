# Repository/IRepository/IMovimentacaoPatrimonioRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMovimentacaoPatrimonioRepository.cs
+++ ATUAL: Repository/IRepository/IMovimentacaoPatrimonioRepository.cs
@@ -9,13 +9,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMovimentacaoPatrimonioRepository : IRepository<MovimentacaoPatrimonio>
     {
-    public interface IMovimentacaoPatrimonioRepository : IRepository<MovimentacaoPatrimonio>
-        {
 
         IEnumerable<SelectListItem> GetMovimentacaoPatrimonioListForDropDown();
 
         void Update(MovimentacaoPatrimonio movimentacaoPatrimonio);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMovimentacaoPatrimonioRepository : IRepository<MovimentacaoPatrimonio>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMovimentacaoPatrimonioRepository : IRepository<MovimentacaoPatrimonio>
}
```
