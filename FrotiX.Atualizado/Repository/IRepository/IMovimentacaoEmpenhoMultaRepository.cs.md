# Repository/IRepository/IMovimentacaoEmpenhoMultaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMovimentacaoEmpenhoMultaRepository.cs
+++ ATUAL: Repository/IRepository/IMovimentacaoEmpenhoMultaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMovimentacaoEmpenhoMultaRepository : IRepository<MovimentacaoEmpenhoMulta>
     {
-    public interface IMovimentacaoEmpenhoMultaRepository : IRepository<MovimentacaoEmpenhoMulta>
-        {
 
         IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown();
 
         void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMovimentacaoEmpenhoMultaRepository : IRepository<MovimentacaoEmpenhoMulta>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMovimentacaoEmpenhoMultaRepository : IRepository<MovimentacaoEmpenhoMulta>
}
```
