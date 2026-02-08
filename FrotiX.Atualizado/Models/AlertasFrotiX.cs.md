# Models/AlertasFrotiX.cs

**Mudanca:** PEQUENA | **+2** linhas | **-2** linhas

---

```diff
--- JANEIRO: Models/AlertasFrotiX.cs
+++ ATUAL: Models/AlertasFrotiX.cs
@@ -5,8 +5,10 @@
 
 namespace FrotiX.Models
 {
+
     public class AlertasFrotiX
     {
+
         [Key]
         public Guid AlertasFrotiXId
         {
@@ -185,6 +187,7 @@
 
     public class AlertasUsuario
     {
+
         [Key]
         public Guid AlertasUsuarioId
         {
@@ -262,8 +265,8 @@
         [Display(Name = "Anúncio")]
         Anuncio = 5,
 
-        [Display(Name = "Aniversário")]
-        Aniversario = 6
+        [Display(Name = "Diversos")]
+        Diversos = 6
     }
 
     public enum PrioridadeAlerta
```

### REMOVER do Janeiro

```csharp
        [Display(Name = "Aniversário")]
        Aniversario = 6
```


### ADICIONAR ao Janeiro

```csharp
        [Display(Name = "Diversos")]
        Diversos = 6
```
