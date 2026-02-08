# Models/Views/ViewLavagem.cs

**Mudanca:** MEDIA | **+4** linhas | **-6** linhas

---

```diff
--- JANEIRO: Models/Views/ViewLavagem.cs
+++ ATUAL: Models/Views/ViewLavagem.cs
@@ -10,9 +10,10 @@
 using Microsoft.AspNetCore.Http;
 
 namespace FrotiX.Models
+{
+
+    public class ViewLavagem
     {
-    public class ViewLavagem
-        {
 
         public Guid LavagemId { get; set; }
 
@@ -24,17 +25,12 @@
 
         public string? Data { get; set; }
 
-        public string? HorarioInicio { get; set; }
-
-        public string? HorarioFim { get; set; }
-
-        public int? DuracaoMinutos { get; set; }
+        public string? Horario { get; set; }
 
         public string? Lavadores { get; set; }
 
         public string? DescricaoVeiculo { get; set; }
 
         public string? Nome { get; set; }
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public class ViewLavagem
        {
        public string? HorarioInicio { get; set; }
        public string? HorarioFim { get; set; }
        public int? DuracaoMinutos { get; set; }
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public class ViewLavagem
        public string? Horario { get; set; }
}
```
