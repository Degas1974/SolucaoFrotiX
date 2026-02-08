# Models/Cadastros/LavadorContrato.cs

**Mudanca:** MEDIA | **+5** linhas | **-5** linhas

---

```diff
--- JANEIRO: Models/Cadastros/LavadorContrato.cs
+++ ATUAL: Models/Cadastros/LavadorContrato.cs
@@ -9,22 +9,25 @@
 using FrotiX.Validations;
 
 namespace FrotiX.Models
+{
+
+    public class LavadorContratoViewModel
     {
-    public class LavadorContratoViewModel
-        {
+
         public Guid LavadorId { get; set; }
+
         public Guid ContratoId { get; set; }
+
         public LavadorContrato LavadorContrato { get; set; }
-        }
+    }
 
     public class LavadorContrato
-        {
+    {
 
         [Key, Column(Order = 0)]
         public Guid LavadorId { get; set; }
 
         [Key, Column(Order = 1)]
         public Guid ContratoId { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class LavadorContratoViewModel
        {
        }
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class LavadorContratoViewModel
    }
    {
}
```
