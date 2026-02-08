# Models/Views/ViewNoFichaVistoria.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewNoFichaVistoria.cs
+++ ATUAL: Models/Views/ViewNoFichaVistoria.cs
@@ -10,11 +10,11 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewNoFichaVistoria
     {
-    public class ViewNoFichaVistoria
-        {
 
         public int? NoFichaVistoria { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewNoFichaVistoria
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewNoFichaVistoria
}
```
