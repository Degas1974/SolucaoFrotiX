# Repository/EventoRepository.cs

**Mudanca:** GRANDE | **+108** linhas | **-108** linhas

---

```diff
--- JANEIRO: Repository/EventoRepository.cs
+++ ATUAL: Repository/EventoRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -8,155 +7,160 @@
 using FrotiX.Repository.IRepository;
 using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc.Rendering;
+using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
 {
-    public class EventoRepository : Repository<Evento>, IEventoRepository
+
+    public class EventoRepository :Repository<Evento>, IEventoRepository
     {
         private new readonly FrotiXDbContext _db;
 
         public EventoRepository(FrotiXDbContext db) : base(db)
         {
-            _db = db;
+        _db = db;
         }
 
         public IEnumerable<SelectListItem> GetEventoListForDropDown()
         {
-            return _db.Evento
-                .OrderBy(o => o.Nome)
-                .Select(i => new SelectListItem()
-                {
-                    Text = i.Nome,
-                    Value = i.EventoId.ToString()
-                });
+        return _db.Evento
+            .OrderBy(o => o.Nome)
+            .Select(i => new SelectListItem()
+            {
+                Text = i.Nome ,
+                Value = i.EventoId.ToString()
+            });
         }
 
         public new void Update(Evento evento)
         {
-            _db.Update(evento);
-            _db.SaveChanges();
+        _db.Update(evento);
+        _db.SaveChanges();
         }
 
         public async Task<(List<EventoListDto> eventos, int totalItems)> GetEventosPaginadoAsync(
-            int page,
-            int pageSize,
+            int page ,
+            int pageSize ,
             string filtroStatus = null
         )
         {
-            try
+        try
+        {
+        var swTotal = System.Diagnostics.Stopwatch.StartNew();
+
+        Console.WriteLine("=== INÍCIO GetEventosPaginadoAsync ===");
+
+        var swEventos = System.Diagnostics.Stopwatch.StartNew();
+
+        var query = from e in _db.Evento
+                    join r in _db.Requisitante on e.RequisitanteId equals r.RequisitanteId into reqJoin
+                    from req in reqJoin.DefaultIfEmpty()
+                    join s in _db.SetorSolicitante on e.SetorSolicitanteId equals s.SetorSolicitanteId into setorJoin
+                    from setor in setorJoin.DefaultIfEmpty()
+                    select new
+                    {
+                        e.EventoId ,
+                        e.Nome ,
+                        e.Descricao ,
+                        e.DataInicial ,
+                        e.DataFinal ,
+                        e.QtdParticipantes ,
+                        e.Status ,
+
+                        NomeRequisitante = req != null ? req.Nome : "" ,
+
+                        NomeSetor = setor != null ? setor.Nome : ""
+                    };
+
+        if (!string.IsNullOrEmpty(filtroStatus))
+        {
+        query = query.Where(x => x.Status == filtroStatus);
+        }
+
+        var totalItems = await query.CountAsync();
+
+        if (totalItems == 0)
+        {
+        Console.WriteLine("=== FIM (sem dados) ===\n");
+        return (new List<EventoListDto>(), 0);
+        }
+
+        var eventos = await query
+            .OrderByDescending(x => x.DataInicial)
+            .Skip((page - 1) * pageSize)
+            .Take(pageSize)
+            .AsNoTracking()
+            .ToListAsync();
+
+        swEventos.Stop();
+        Console.WriteLine($"[QUERY EVENTOS] {eventos.Count}/{totalItems} registros - {swEventos.ElapsedMilliseconds}ms");
+
+        var swCustos = System.Diagnostics.Stopwatch.StartNew();
+
+        var eventoIds = eventos.Select(x => x.EventoId).ToList();
+
+        var custosPorEvento = await _db.Viagem
+            .Where(v => eventoIds.Contains(v.EventoId.Value))
+            .GroupBy(v => v.EventoId.Value)
+            .Select(g => new
             {
-                var swTotal = System.Diagnostics.Stopwatch.StartNew();
+                EventoId = g.Key ,
 
-                Console.WriteLine("=== INÍCIO GetEventosPaginadoAsync ===");
+                CustoTotal = (decimal)(
+                    g.Sum(v => (double)(v.CustoCombustivel ?? 0)) +
+                    g.Sum(v => (double)(v.CustoMotorista ?? 0)) +
+                    g.Sum(v => (double)(v.CustoVeiculo ?? 0)) +
+                    g.Sum(v => (double)(v.CustoOperador ?? 0)) +
+                    g.Sum(v => (double)(v.CustoLavador ?? 0))
+                )
+            })
+            .AsNoTracking()
+            .ToListAsync();
 
-                var swEventos = System.Diagnostics.Stopwatch.StartNew();
+        var custosDict = custosPorEvento.ToDictionary(x => x.EventoId , x => x.CustoTotal);
 
-                var query = from e in _db.Evento
-                            join r in _db.Requisitante on e.RequisitanteId equals r.RequisitanteId into reqJoin
-                            from req in reqJoin.DefaultIfEmpty()
-                            join s in _db.SetorSolicitante on e.SetorSolicitanteId equals s.SetorSolicitanteId into setorJoin
-                            from setor in setorJoin.DefaultIfEmpty()
-                            select new
-                            {
-                                e.EventoId,
-                                e.Nome,
-                                e.Descricao,
-                                e.DataInicial,
-                                e.DataFinal,
-                                e.QtdParticipantes,
-                                e.Status,
-                                NomeRequisitante = req != null ? req.Nome : "",
-                                NomeSetor = setor != null ? setor.Nome : ""
-                            };
+        swCustos.Stop();
+        Console.WriteLine($"[CUSTOS] {custosPorEvento.Count} eventos com custos - {swCustos.ElapsedMilliseconds}ms");
 
-                if (!string.IsNullOrEmpty(filtroStatus))
-                {
-                    query = query.Where(x => x.Status == filtroStatus);
-                }
+        var swFormato = System.Diagnostics.Stopwatch.StartNew();
 
-                var totalItems = await query.CountAsync();
+        var result = eventos.Select(x =>
+        {
+        var custo = custosDict.ContainsKey(x.EventoId) ? custosDict[x.EventoId] : 0;
 
-                if (totalItems == 0)
-                {
-                    Console.WriteLine("=== FIM (sem dados) ===\n");
-                    return (new List<EventoListDto>(), 0);
-                }
+        return new EventoListDto
+        {
+            EventoId = x.EventoId ,
+            Nome = x.Nome ,
+            Descricao = x.Descricao ,
+            DataInicial = x.DataInicial ,
+            DataFinal = x.DataFinal ,
+            QtdParticipantes = (x.QtdParticipantes ?? 0).ToString().PadLeft(3 , '0') ,
+            Status = x.Status ,
+            NomeRequisitante = x.NomeRequisitante ,
+            NomeRequisitanteHTML = Servicos.ConvertHtml(x.NomeRequisitante ?? "") ,
+            NomeSetor = x.NomeSetor ,
+            CustoViagem = string.Format("R$ {0:N2}" , custo) ,
+            CustoViagemNaoFormatado = custo
+        };
+        }).ToList();
 
-                var eventos = await query
-                    .OrderByDescending(x => x.DataInicial)
-                    .Skip((page - 1) * pageSize)
-                    .Take(pageSize)
-                    .AsNoTracking()
-                    .ToListAsync();
+        swFormato.Stop();
+        Console.WriteLine($"[FORMATO] {result.Count} registros - {swFormato.ElapsedMilliseconds}ms");
 
-                swEventos.Stop();
-                Console.WriteLine($"[QUERY EVENTOS] {eventos.Count}/{totalItems} registros - {swEventos.ElapsedMilliseconds}ms");
+        swTotal.Stop();
+        Console.WriteLine($"[TOTAL REPOSITORY] {swTotal.ElapsedMilliseconds}ms");
+        Console.WriteLine("=== FIM GetEventosPaginadoAsync ===\n");
 
-                var swCustos = System.Diagnostics.Stopwatch.StartNew();
-
-                var eventoIds = eventos.Select(x => x.EventoId).ToList();
-
-                var custosPorEvento = await _db.Viagem
-                    .Where(v => eventoIds.Contains(v.EventoId.Value))
-                    .GroupBy(v => v.EventoId.Value)
-                    .Select(g => new
-                    {
-                        EventoId = g.Key,
-                        CustoTotal = (decimal)(
-                            g.Sum(v => (double)(v.CustoCombustivel ?? 0)) +
-                            g.Sum(v => (double)(v.CustoMotorista ?? 0)) +
-                            g.Sum(v => (double)(v.CustoVeiculo ?? 0)) +
-                            g.Sum(v => (double)(v.CustoOperador ?? 0)) +
-                            g.Sum(v => (double)(v.CustoLavador ?? 0))
-                        )
-                    })
-                    .AsNoTracking()
-                    .ToListAsync();
-
-                var custosDict = custosPorEvento.ToDictionary(x => x.EventoId, x => x.CustoTotal);
-
-                swCustos.Stop();
-                Console.WriteLine($"[CUSTOS] {custosPorEvento.Count} eventos com custos - {swCustos.ElapsedMilliseconds}ms");
-
-                var swFormato = System.Diagnostics.Stopwatch.StartNew();
-
-                var result = eventos.Select(x =>
-                {
-                    var custo = custosDict.ContainsKey(x.EventoId) ? custosDict[x.EventoId] : 0;
-
-                    return new EventoListDto
-                    {
-                        EventoId = x.EventoId,
-                        Nome = x.Nome,
-                        Descricao = x.Descricao,
-                        DataInicial = x.DataInicial,
-                        DataFinal = x.DataFinal,
-                        QtdParticipantes = (x.QtdParticipantes ?? 0).ToString().PadLeft(3, '0'),
-                        Status = x.Status,
-                        NomeRequisitante = x.NomeRequisitante,
-                        NomeRequisitanteHTML = Servicos.ConvertHtml(x.NomeRequisitante ?? ""),
-                        NomeSetor = x.NomeSetor,
-                        CustoViagem = string.Format("R$ {0:N2}", custo),
-                        CustoViagemNaoFormatado = custo
-                    };
-                }).ToList();
-
-                swFormato.Stop();
-                Console.WriteLine($"[FORMATO] {result.Count} registros - {swFormato.ElapsedMilliseconds}ms");
-
-                swTotal.Stop();
-                Console.WriteLine($"[TOTAL REPOSITORY] {swTotal.ElapsedMilliseconds}ms");
-                Console.WriteLine("=== FIM GetEventosPaginadoAsync ===\n");
-
-                return (result, totalItems);
-            }
-            catch (Exception error)
-            {
-                Console.WriteLine($"[ERRO REPOSITORY] {error.Message}");
-                Console.WriteLine($"[STACK] {error.StackTrace}");
-                Alerta.TratamentoErroComLinha("EventoRepository.cs", "GetEventosPaginadoAsync", error);
-                throw;
-            }
+        return (result, totalItems);
+        }
+        catch (Exception error)
+        {
+        Console.WriteLine($"[ERRO REPOSITORY] {error.Message}");
+        Console.WriteLine($"[STACK] {error.StackTrace}");
+        Alerta.TratamentoErroComLinha("EventoRepository.cs" , "GetEventosPaginadoAsync" , error);
+        throw;
+        }
         }
     }
 }
```
