# Repository/IRepository/INotaFiscalRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/INotaFiscalRepository.cs
+++ ATUAL: Repository/IRepository/INotaFiscalRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface INotaFiscalRepository : IRepository<NotaFiscal>
     {
-    public interface INotaFiscalRepository : IRepository<NotaFiscal>
-        {
 
         IEnumerable<SelectListItem> GetNotaFiscalListForDropDown();
 
         void Update(NotaFiscal notaFiscal);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface INotaFiscalRepository : IRepository<NotaFiscal>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface INotaFiscalRepository : IRepository<NotaFiscal>
}
```
