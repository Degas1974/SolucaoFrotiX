# Models/Cadastros/ItensContrato.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Models/Cadastros/ItensContrato.cs
+++ ATUAL: Models/Cadastros/ItensContrato.cs
@@ -10,6 +10,7 @@
 
 namespace FrotiX.Models
     {
+
     public class ItensContratoViewModel
         {
         public Guid ContratoId { get; set; }
@@ -17,7 +18,7 @@
 
         public IEnumerable<SelectListItem> ContratoList { get; set; }
 
-        }
+    }
 
     public class ItensContrato
         {
```

### REMOVER do Janeiro

```csharp
        }
```


### ADICIONAR ao Janeiro

```csharp
    }
```
