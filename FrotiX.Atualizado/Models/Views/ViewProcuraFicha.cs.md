# Models/Views/ViewProcuraFicha.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewProcuraFicha.cs
+++ ATUAL: Models/Views/ViewProcuraFicha.cs
@@ -10,9 +10,10 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewProcuraFicha
     {
-    public class ViewProcuraFicha
-        {
 
         public Guid MotoristaId { get; set; }
 
@@ -27,6 +28,5 @@
         public string? HoraFim { get; set; }
 
         public int? NoFichaVistoria { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewProcuraFicha
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewProcuraFicha
}
```
