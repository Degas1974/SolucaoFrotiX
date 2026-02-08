# Models/Views/ViewVeiculosManutencao.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Models/Views/ViewVeiculosManutencao.cs
+++ ATUAL: Models/Views/ViewVeiculosManutencao.cs
@@ -1,10 +1,13 @@
 using System;
 
 namespace FrotiX.Models
+{
+
+    public class ViewVeiculosManutencao
     {
-    public class ViewVeiculosManutencao
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
    public class ViewVeiculosManutencao
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewVeiculosManutencao
}
```
