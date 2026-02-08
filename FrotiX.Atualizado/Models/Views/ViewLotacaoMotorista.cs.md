# Models/Views/ViewLotacaoMotorista.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewLotacaoMotorista.cs
+++ ATUAL: Models/Views/ViewLotacaoMotorista.cs
@@ -10,9 +10,10 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewLotacaoMotorista
     {
-    public class ViewLotacaoMotorista
-        {
 
         public Guid UnidadeId { get; set; }
 
@@ -31,5 +32,5 @@
         public string? DataFim { get; set; }
 
         public string? MotoristaCobertura { get; set; }
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewLotacaoMotorista
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewLotacaoMotorista
}
```
