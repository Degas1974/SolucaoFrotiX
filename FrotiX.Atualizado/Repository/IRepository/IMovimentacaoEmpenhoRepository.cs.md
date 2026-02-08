# Repository/IRepository/IMovimentacaoEmpenhoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IMovimentacaoEmpenhoRepository.cs
+++ ATUAL: Repository/IRepository/IMovimentacaoEmpenhoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IMovimentacaoEmpenhoRepository : IRepository<MovimentacaoEmpenho>
     {
-    public interface IMovimentacaoEmpenhoRepository : IRepository<MovimentacaoEmpenho>
-        {
 
         IEnumerable<SelectListItem> GetMovimentacaoEmpenhoListForDropDown();
 
         void Update(MovimentacaoEmpenho movimentacaoempenho);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IMovimentacaoEmpenhoRepository : IRepository<MovimentacaoEmpenho>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IMovimentacaoEmpenhoRepository : IRepository<MovimentacaoEmpenho>
}
```
