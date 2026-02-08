# Repository/IRepository/IViagemEstatisticaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViagemEstatisticaRepository.cs
+++ ATUAL: Repository/IRepository/IViagemEstatisticaRepository.cs
@@ -5,11 +5,13 @@
 
 namespace FrotiX.Repository.IRepository
 {
+
     public interface IViagemEstatisticaRepository : IRepository<ViagemEstatistica>
     {
+
         Task<ViagemEstatistica> ObterPorDataAsync(DateTime dataReferencia);
 
-        Task<List<ViagemEstatistica>> ObterPorPeriodoAsync(DateTime dataInicio , DateTime dataFim);
+        Task<List<ViagemEstatistica>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
 
         Task<bool> ExisteParaDataAsync(DateTime dataReferencia);
 
```

### REMOVER do Janeiro

```csharp
        Task<List<ViagemEstatistica>> ObterPorPeriodoAsync(DateTime dataInicio , DateTime dataFim);
```


### ADICIONAR ao Janeiro

```csharp
        Task<List<ViagemEstatistica>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
```
