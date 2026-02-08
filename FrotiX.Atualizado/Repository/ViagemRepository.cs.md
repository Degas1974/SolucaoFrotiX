# Repository/ViagemRepository.cs

**Mudanca:** GRANDE | **+18** linhas | **-20** linhas

---

```diff
--- JANEIRO: Repository/ViagemRepository.cs
+++ ATUAL: Repository/ViagemRepository.cs
@@ -1,8 +1,8 @@
-using Microsoft.EntityFrameworkCore;
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc.Rendering;
+using Microsoft.EntityFrameworkCore;
 using NPOI.SS.Formula.Functions;
 using System;
 using System.Collections.Generic;
@@ -12,6 +12,7 @@
 
 namespace FrotiX.Repository
 {
+
     public class ViagemRepository : Repository<Viagem>, IViagemRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -27,7 +28,7 @@
                 .OrderBy(o => o.DataInicial)
                 .Select(i => new SelectListItem()
                 {
-                    Text = i.Descricao,
+                    Text = i.Descricao ,
                     Value = i.ViagemId.ToString()
                 });
         }
@@ -58,10 +59,9 @@
                 .ToListAsync();
         }
 
-        public async Task CorrigirOrigemAsync(List<string> origensAntigas, string novaOrigem)
+        public async Task CorrigirOrigemAsync(List<string> origensAntigas , string novaOrigem)
         {
             var viagens = await _db.Viagem
-                .AsTracking()
                 .Where(v => origensAntigas.Contains(v.Origem))
                 .ToListAsync();
 
@@ -73,10 +73,9 @@
             await _db.SaveChangesAsync();
         }
 
-        public async Task CorrigirDestinoAsync(List<string> destinosAntigos, string novoDestino)
+        public async Task CorrigirDestinoAsync(List<string> destinosAntigos , string novoDestino)
         {
             var viagens = await _db.Viagem
-                .AsTracking()
                 .Where(v => destinosAntigos.Contains(v.Destino))
                 .ToListAsync();
 
@@ -106,8 +105,8 @@
         }
 
         public async Task<(List<ViagemEventoDto> viagens, int totalItems)> GetViagensEventoPaginadoAsync(
-            Guid eventoId,
-            int page,
+            Guid eventoId ,
+            int page ,
             int pageSize
         )
         {
@@ -143,16 +142,16 @@
                     .Where(vv => viagemIds.Contains(vv.ViagemId))
                     .Select(vv => new ViagemEventoDto
                     {
-                        ViagemId = vv.ViagemId,
-                        EventoId = vv.EventoId ?? Guid.Empty,
-                        NoFichaVistoria = vv.NoFichaVistoria ?? 0,
-                        NomeRequisitante = vv.NomeRequisitante ?? "",
-                        NomeSetor = vv.NomeSetor ?? "",
-                        NomeMotorista = vv.NomeMotorista ?? "",
-                        DescricaoVeiculo = vv.DescricaoVeiculo ?? "",
-                        CustoViagem = (decimal)(vv.CustoViagem ?? 0),
-                        DataInicial = vv.DataInicial ?? DateTime.MinValue,
-                        HoraInicio = vv.HoraInicio,
+                        ViagemId = vv.ViagemId ,
+                        EventoId = vv.EventoId ?? Guid.Empty ,
+                        NoFichaVistoria = vv.NoFichaVistoria ?? 0 ,
+                        NomeRequisitante = vv.NomeRequisitante ?? "" ,
+                        NomeSetor = vv.NomeSetor ?? "" ,
+                        NomeMotorista = vv.NomeMotorista ?? "" ,
+                        DescricaoVeiculo = vv.DescricaoVeiculo ?? "" ,
+                        CustoViagem = (decimal)(vv.CustoViagem ?? 0) ,
+                        DataInicial = vv.DataInicial ?? DateTime.MinValue ,
+                        HoraInicio = vv.HoraInicio ,
                         Placa = vv.Placa ?? ""
                     })
                     .AsNoTracking()
@@ -174,12 +173,12 @@
             catch (Exception error)
             {
                 Console.WriteLine($"[ERRO SQL] {error.Message}");
-                Alerta.TratamentoErroComLinha("ViagemRepository.cs", "GetViagensEventoPaginadoAsync", error);
+                Alerta.TratamentoErroComLinha("ViagemRepository.cs" , "GetViagensEventoPaginadoAsync" , error);
                 throw;
             }
         }
 
-        public IQueryable<Viagem> GetQuery(Expression<Func<Viagem, bool>> filter = null)
+        public IQueryable<Viagem> GetQuery(Expression<Func<Viagem , bool>> filter = null)
         {
             IQueryable<Viagem> query = dbSet;
 
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
                    Text = i.Descricao,
        public async Task CorrigirOrigemAsync(List<string> origensAntigas, string novaOrigem)
                .AsTracking()
        public async Task CorrigirDestinoAsync(List<string> destinosAntigos, string novoDestino)
                .AsTracking()
            Guid eventoId,
            int page,
                        ViagemId = vv.ViagemId,
                        EventoId = vv.EventoId ?? Guid.Empty,
                        NoFichaVistoria = vv.NoFichaVistoria ?? 0,
                        NomeRequisitante = vv.NomeRequisitante ?? "",
                        NomeSetor = vv.NomeSetor ?? "",
                        NomeMotorista = vv.NomeMotorista ?? "",
                        DescricaoVeiculo = vv.DescricaoVeiculo ?? "",
                        CustoViagem = (decimal)(vv.CustoViagem ?? 0),
                        DataInicial = vv.DataInicial ?? DateTime.MinValue,
                        HoraInicio = vv.HoraInicio,
                Alerta.TratamentoErroComLinha("ViagemRepository.cs", "GetViagensEventoPaginadoAsync", error);
        public IQueryable<Viagem> GetQuery(Expression<Func<Viagem, bool>> filter = null)
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
                    Text = i.Descricao ,
        public async Task CorrigirOrigemAsync(List<string> origensAntigas , string novaOrigem)
        public async Task CorrigirDestinoAsync(List<string> destinosAntigos , string novoDestino)
            Guid eventoId ,
            int page ,
                        ViagemId = vv.ViagemId ,
                        EventoId = vv.EventoId ?? Guid.Empty ,
                        NoFichaVistoria = vv.NoFichaVistoria ?? 0 ,
                        NomeRequisitante = vv.NomeRequisitante ?? "" ,
                        NomeSetor = vv.NomeSetor ?? "" ,
                        NomeMotorista = vv.NomeMotorista ?? "" ,
                        DescricaoVeiculo = vv.DescricaoVeiculo ?? "" ,
                        CustoViagem = (decimal)(vv.CustoViagem ?? 0) ,
                        DataInicial = vv.DataInicial ?? DateTime.MinValue ,
                        HoraInicio = vv.HoraInicio ,
                Alerta.TratamentoErroComLinha("ViagemRepository.cs" , "GetViagensEventoPaginadoAsync" , error);
        public IQueryable<Viagem> GetQuery(Expression<Func<Viagem , bool>> filter = null)
```
