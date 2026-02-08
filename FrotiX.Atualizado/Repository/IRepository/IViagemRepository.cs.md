# Repository/IRepository/IViagemRepository.cs

**Mudanca:** MEDIA | **+5** linhas | **-5** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViagemRepository.cs
+++ ATUAL: Repository/IRepository/IViagemRepository.cs
@@ -9,25 +9,30 @@
 
 namespace FrotiX.Repository.IRepository
 {
+
     public interface IViagemRepository : IRepository<Viagem>
     {
+
         IEnumerable<SelectListItem> GetViagemListForDropDown();
 
         void Update(Viagem viagem);
 
         Task<List<string>> GetDistinctOrigensAsync();
+
         Task<List<string>> GetDistinctDestinosAsync();
-        Task CorrigirOrigemAsync(List<string> origensAntigas , string novaOrigem);
-        Task CorrigirDestinoAsync(List<string> destinosAntigos , string novoDestino);
+
+        Task CorrigirOrigemAsync(List<string> origensAntigas, string novaOrigem);
+
+        Task CorrigirDestinoAsync(List<string> destinosAntigos, string novoDestino);
 
         Task<List<Viagem>> BuscarViagensRecorrenciaAsync(Guid id);
 
         Task<(List<ViagemEventoDto> viagens, int totalItems)> GetViagensEventoPaginadoAsync(
-            Guid eventoId ,
-            int page ,
+            Guid eventoId,
+            int page,
             int pageSize
         );
 
-        IQueryable<Viagem> GetQuery(Expression<Func<Viagem , bool>> filter = null);
+        IQueryable<Viagem> GetQuery(Expression<Func<Viagem, bool>> filter = null);
     }
 }
```

### REMOVER do Janeiro

```csharp
        Task CorrigirOrigemAsync(List<string> origensAntigas , string novaOrigem);
        Task CorrigirDestinoAsync(List<string> destinosAntigos , string novoDestino);
            Guid eventoId ,
            int page ,
        IQueryable<Viagem> GetQuery(Expression<Func<Viagem , bool>> filter = null);
```


### ADICIONAR ao Janeiro

```csharp
        Task CorrigirOrigemAsync(List<string> origensAntigas, string novaOrigem);
        Task CorrigirDestinoAsync(List<string> destinosAntigos, string novoDestino);
            Guid eventoId,
            int page,
        IQueryable<Viagem> GetQuery(Expression<Func<Viagem, bool>> filter = null);
```
