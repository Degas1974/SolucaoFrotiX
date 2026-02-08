# Models/Views/ViewVeiculosManutencaoReserva.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewVeiculosManutencaoReserva.cs
+++ ATUAL: Models/Views/ViewVeiculosManutencaoReserva.cs
@@ -1,10 +1,13 @@
 using System;
 
 namespace FrotiX.Models
+{
+
+    public class ViewVeiculosManutencaoReserva
     {
-    public class ViewVeiculosManutencaoReserva
-        {
+
         public String? Descricao { get; set; }
+
         public Guid VeiculoId { get; set; }
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewVeiculosManutencaoReserva
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewVeiculosManutencaoReserva
}
```
