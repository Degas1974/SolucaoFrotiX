# Models/Views/ViewMotoristaVez.cs

**Mudanca:** PEQUENA | **+2** linhas | **-2** linhas

---

```diff
--- JANEIRO: Models/Views/ViewMotoristaVez.cs
+++ ATUAL: Models/Views/ViewMotoristaVez.cs
@@ -9,27 +9,41 @@
 
 namespace FrotiX.Models
 {
+
     public class ViewMotoristasVez
     {
+
         public Guid MotoristaId { get; set; }
+
         public string NomeMotorista { get; set; }
+
         public string? Ponto { get; set; }
+
         public byte[]? Foto { get; set; }
+
         public DateTime DataEscala { get; set; }
+
         public int NumeroSaidas { get; set; }
+
         public string StatusMotorista { get; set; }
+
         public string? Lotacao { get; set; }
+
         public string? VeiculoDescricao { get; set; }
+
         public string? Placa { get; set; }
+
         public string HoraInicio { get; set; }
+
         public string HoraFim { get; set; }
 
         public string GetStatusClass()
         {
+
             return StatusMotorista?.ToLower() switch
             {
-                "disponÃ­vel" or "disponivel" => "text-success",
-                "em serviÃ§o" or "em servico" => "text-warning",
+                "disponível" or "disponivel" => "text-success",
+                "em serviço" or "em servico" => "text-warning",
                 _ => "text-secondary"
             };
         }
```

### REMOVER do Janeiro

```csharp
                "disponÃ­vel" or "disponivel" => "text-success",
                "em serviÃ§o" or "em servico" => "text-warning",
```


### ADICIONAR ao Janeiro

```csharp
                "disponível" or "disponivel" => "text-success",
                "em serviço" or "em servico" => "text-warning",
```
