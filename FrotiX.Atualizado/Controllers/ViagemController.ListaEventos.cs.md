# Controllers/ViagemController.ListaEventos.cs

**Mudanca:** GRANDE | **+27** linhas | **-9** linhas

---

```diff
--- JANEIRO: Controllers/ViagemController.ListaEventos.cs
+++ ATUAL: Controllers/ViagemController.ListaEventos.cs
@@ -18,7 +18,9 @@
         public IActionResult ListaEventos(
             int draw = 1,
             int start = 0,
-            int length = 25)
+            int length = 25,
+            int orderColumn = 1,
+            string orderDir = "desc")
         {
             var sw = System.Diagnostics.Stopwatch.StartNew();
 
@@ -29,11 +31,25 @@
 
                 Console.WriteLine($"[ListaEventos] Total de eventos: {totalRecords}");
 
-                var eventos = _context.Evento
+                IQueryable<Evento> query = _context.Evento
                     .Include(e => e.SetorSolicitante)
                     .Include(e => e.Requisitante)
-                    .AsNoTracking()
-                    .OrderBy(e => e.Nome)
+                    .AsNoTracking();
+
+                query = orderColumn switch
+                {
+                    0 => orderDir == "asc" ? query.OrderBy(e => e.Nome) : query.OrderByDescending(e => e.Nome),
+                    1 => orderDir == "asc" ? query.OrderBy(e => e.DataInicial) : query.OrderByDescending(e => e.DataInicial),
+                    2 => orderDir == "asc" ? query.OrderBy(e => e.DataFinal) : query.OrderByDescending(e => e.DataFinal),
+                    3 => orderDir == "asc" ? query.OrderBy(e => e.QtdParticipantes) : query.OrderByDescending(e => e.QtdParticipantes),
+                    4 => orderDir == "asc" ? query.OrderBy(e => e.SetorSolicitante.Nome) : query.OrderByDescending(e => e.SetorSolicitante.Nome),
+
+                    6 => orderDir == "asc" ? query.OrderBy(e => e.Status) : query.OrderByDescending(e => e.Status),
+
+                    _ => orderDir == "asc" ? query.OrderBy(e => e.DataInicial) : query.OrderByDescending(e => e.DataInicial)
+                };
+
+                var eventos = query
                     .Skip(start)
                     .Take(length)
                     .ToList();
@@ -43,7 +59,7 @@
                 var eventoIds = eventos.Select(e => e.EventoId).ToList();
 
                 var viagensDict = _context.Viagem
-                    .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value))
+                    .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value) && v.Status == "Realizada")
                     .AsNoTracking()
                     .GroupBy(v => v.EventoId)
                     .Select(g => new
@@ -94,18 +110,26 @@
                         custoViagem = custoViagem,
                         viagensCount = viagensCount
                     };
-                })
-                .ToList();
+                });
+
+                if (orderColumn == 5)
+                {
+                    resultado = orderDir == "asc"
+                        ? resultado.OrderBy(r => r.custoViagem)
+                        : resultado.OrderByDescending(r => r.custoViagem);
+                }
+
+                var resultadoFinal = resultado.ToList();
 
                 sw.Stop();
-                Console.WriteLine($"[ListaEventos] ✅ TOTAL: {sw.ElapsedMilliseconds}ms - Página {(start / length) + 1} ({resultado.Count} de {totalRecords} eventos)");
+                Console.WriteLine($"[ListaEventos] ✅ TOTAL: {sw.ElapsedMilliseconds}ms - Página {(start / length) + 1} ({resultadoFinal.Count} de {totalRecords} eventos) - Ordenado por coluna {orderColumn} ({orderDir})");
 
                 return Json(new
                 {
                     draw = draw,
                     recordsTotal = totalRecords,
                     recordsFiltered = totalRecords,
-                    data = resultado
+                    data = resultadoFinal
                 });
             }
             catch (Exception error)
```

### REMOVER do Janeiro

```csharp
            int length = 25)
                var eventos = _context.Evento
                    .AsNoTracking()
                    .OrderBy(e => e.Nome)
                    .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value))
                })
                .ToList();
                Console.WriteLine($"[ListaEventos] ✅ TOTAL: {sw.ElapsedMilliseconds}ms - Página {(start / length) + 1} ({resultado.Count} de {totalRecords} eventos)");
                    data = resultado
```


### ADICIONAR ao Janeiro

```csharp
            int length = 25,
            int orderColumn = 1,
            string orderDir = "desc")
                IQueryable<Evento> query = _context.Evento
                    .AsNoTracking();
                query = orderColumn switch
                {
                    0 => orderDir == "asc" ? query.OrderBy(e => e.Nome) : query.OrderByDescending(e => e.Nome),
                    1 => orderDir == "asc" ? query.OrderBy(e => e.DataInicial) : query.OrderByDescending(e => e.DataInicial),
                    2 => orderDir == "asc" ? query.OrderBy(e => e.DataFinal) : query.OrderByDescending(e => e.DataFinal),
                    3 => orderDir == "asc" ? query.OrderBy(e => e.QtdParticipantes) : query.OrderByDescending(e => e.QtdParticipantes),
                    4 => orderDir == "asc" ? query.OrderBy(e => e.SetorSolicitante.Nome) : query.OrderByDescending(e => e.SetorSolicitante.Nome),
                    6 => orderDir == "asc" ? query.OrderBy(e => e.Status) : query.OrderByDescending(e => e.Status),
                    _ => orderDir == "asc" ? query.OrderBy(e => e.DataInicial) : query.OrderByDescending(e => e.DataInicial)
                };
                var eventos = query
                    .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value) && v.Status == "Realizada")
                });
                if (orderColumn == 5)
                {
                    resultado = orderDir == "asc"
                        ? resultado.OrderBy(r => r.custoViagem)
                        : resultado.OrderByDescending(r => r.custoViagem);
                }
                var resultadoFinal = resultado.ToList();
                Console.WriteLine($"[ListaEventos] ✅ TOTAL: {sw.ElapsedMilliseconds}ms - Página {(start / length) + 1} ({resultadoFinal.Count} de {totalRecords} eventos) - Ordenado por coluna {orderColumn} ({orderDir})");
                    data = resultadoFinal
```
