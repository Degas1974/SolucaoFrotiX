# Models/Views/ViewPendenciasManutencao.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewPendenciasManutencao.cs
+++ ATUAL: Models/Views/ViewPendenciasManutencao.cs
@@ -10,9 +10,10 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewPendenciasManutencao
     {
-    public class ViewPendenciasManutencao
-        {
 
         public Guid ItemManutencaoId { get; set; }
 
@@ -39,6 +40,5 @@
         public string? Nome { get; set; }
 
         public string? ImagemOcorrencia { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewPendenciasManutencao
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewPendenciasManutencao
}
```
