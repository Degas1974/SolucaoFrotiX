# Models/Views/ViewMediaConsumo.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewMediaConsumo.cs
+++ ATUAL: Models/Views/ViewMediaConsumo.cs
@@ -10,13 +10,13 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewMediaConsumo
     {
-    public class ViewMediaConsumo
-        {
 
         public Guid VeiculoId { get; set; }
 
         public decimal? ConsumoGeral { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewMediaConsumo
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewMediaConsumo
}
```
