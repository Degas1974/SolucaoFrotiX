# Models/Views/ViewFluxoEconomildo.cs

**Mudanca:** PEQUENA | **+2** linhas | **-2** linhas

---

```diff
--- JANEIRO: Models/Views/ViewFluxoEconomildo.cs
+++ ATUAL: Models/Views/ViewFluxoEconomildo.cs
@@ -10,9 +10,10 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewFluxoEconomildo
     {
-    public class ViewFluxoEconomildo
-        {
 
         public Guid VeiculoId { get; set; }
 
```

### REMOVER do Janeiro

```csharp
    public class ViewFluxoEconomildo
        {
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewFluxoEconomildo
```
