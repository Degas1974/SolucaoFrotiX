# Models/Views/ViewMotoristaFluxo.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewMotoristaFluxo.cs
+++ ATUAL: Models/Views/ViewMotoristaFluxo.cs
@@ -10,13 +10,13 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewMotoristaFluxo
     {
-    public class ViewMotoristaFluxo
-        {
 
         public string? MotoristaId { get; set; }
 
         public string? NomeMotorista { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewMotoristaFluxo
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewMotoristaFluxo
}
```
