# Models/Cadastros/Lavagem.cs

**Mudanca:** MEDIA | **+2** linhas | **-4** linhas

---

```diff
--- JANEIRO: Models/Cadastros/Lavagem.cs
+++ ATUAL: Models/Cadastros/Lavagem.cs
@@ -9,8 +9,10 @@
 
 namespace FrotiX.Models
 {
+
     public class Lavagem
     {
+
         [Key]
         public Guid LavagemId { get; set; }
 
@@ -18,11 +20,8 @@
         [Display(Name = "Data")]
         public DateTime? Data { get; set; }
 
-        [Display(Name = "Horário Início")]
-        public DateTime? HorarioInicio { get; set; }
-
-        [Display(Name = "Horário Fim")]
-        public DateTime? HorarioFim { get; set; }
+        [Display(Name = "Horário da Lavagem")]
+        public DateTime? HorarioLavagem { get; set; }
 
         [Display(Name = "Veículo Lavado")]
         public Guid VeiculoId { get; set; }
```

### REMOVER do Janeiro

```csharp
        [Display(Name = "Horário Início")]
        public DateTime? HorarioInicio { get; set; }
        [Display(Name = "Horário Fim")]
        public DateTime? HorarioFim { get; set; }
```


### ADICIONAR ao Janeiro

```csharp
        [Display(Name = "Horário da Lavagem")]
        public DateTime? HorarioLavagem { get; set; }
```
