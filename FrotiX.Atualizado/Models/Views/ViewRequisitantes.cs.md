# Models/Views/ViewRequisitantes.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewRequisitantes.cs
+++ ATUAL: Models/Views/ViewRequisitantes.cs
@@ -10,13 +10,13 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewRequisitantes
     {
-    public class ViewRequisitantes
-        {
 
         public Guid RequisitanteId { get; set; }
 
         public string? Requisitante { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewRequisitantes
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewRequisitantes
}
```
