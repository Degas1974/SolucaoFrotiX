# Models/Views/ViewLotacoes.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewLotacoes.cs
+++ ATUAL: Models/Views/ViewLotacoes.cs
@@ -10,9 +10,10 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewLotacoes
     {
-    public class ViewLotacoes
-        {
 
         public Guid LotacaoMotoristaId { get; set; }
 
@@ -29,5 +30,5 @@
         public string? DataInicio { get; set; }
 
         public bool Lotado { get; set; }
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewLotacoes
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewLotacoes
}
```
