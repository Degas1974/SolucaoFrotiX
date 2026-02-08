# Models/Views/ViewFluxoEconomildoData.cs

**Mudanca:** PEQUENA | **+2** linhas | **-2** linhas

---

```diff
--- JANEIRO: Models/Views/ViewFluxoEconomildoData.cs
+++ ATUAL: Models/Views/ViewFluxoEconomildoData.cs
@@ -10,9 +10,10 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewFluxoEconomildoData
     {
-    public class ViewFluxoEconomildoData
-        {
 
         public Guid VeiculoId { get; set; }
 
```

### REMOVER do Janeiro

```csharp
    public class ViewFluxoEconomildoData
        {
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewFluxoEconomildoData
```
