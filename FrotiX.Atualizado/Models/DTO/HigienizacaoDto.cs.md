# Models/DTO/HigienizacaoDto.cs

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Models/DTO/HigienizacaoDto.cs
+++ ATUAL: Models/DTO/HigienizacaoDto.cs
@@ -2,22 +2,30 @@
 
 namespace FrotiX.Models.DTO
     {
+
     public class HigienizacaoDto
         {
+
         public string Tipo { get; set; }
+
         public List<string> AntigosValores { get; set; }
+
         public string NovosValores { get; set; }
         }
 
     public class CorrecaoOrigemDto
         {
+
         public List<string> Origens { get; set; }
+
         public string NovaOrigem { get; set; }
         }
 
     public class CorrecaoDestinoDto
         {
+
         public List<string> Destinos { get; set; }
+
         public string NovoDestino { get; set; }
         }
 
```
