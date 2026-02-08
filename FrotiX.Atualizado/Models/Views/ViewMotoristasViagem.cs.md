# Models/Views/ViewMotoristasViagem.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewMotoristasViagem.cs
+++ ATUAL: Models/Views/ViewMotoristasViagem.cs
@@ -1,9 +1,10 @@
 using System;
 
 namespace FrotiX.Models.Views
+{
+
+    public class ViewMotoristasViagem
     {
-    public class ViewMotoristasViagem
-        {
 
         public Guid MotoristaId { get; set; }
 
@@ -16,6 +17,5 @@
         public string? TipoCondutor { get; set; }
 
         public byte[]? Foto { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewMotoristasViagem
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewMotoristasViagem
}
```
