# Models/DTO/LookupsDto.cs

**Mudanca:** PEQUENA | **+1** linhas | **-0** linhas

---

```diff
--- JANEIRO: Models/DTO/LookupsDto.cs
+++ ATUAL: Models/DTO/LookupsDto.cs
@@ -2,7 +2,10 @@
 
 namespace FrotiX.Models.DTO
     {
+
     public sealed record MotoristaData(Guid MotoristaId, string Nome);
+
+    public sealed record MotoristaDataComFoto(Guid MotoristaId, string Nome, string? FotoBase64);
 
     public sealed record VeiculoData(Guid VeiculoId, string Descricao);
 
```

### ADICIONAR ao Janeiro

```csharp
    public sealed record MotoristaDataComFoto(Guid MotoristaId, string Nome, string? FotoBase64);
```
