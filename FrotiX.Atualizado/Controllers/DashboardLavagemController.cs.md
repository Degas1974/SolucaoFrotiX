# Controllers/DashboardLavagemController.cs

**Mudanca:** GRANDE | **+22** linhas | **-135** linhas

---

```diff
--- JANEIRO: Controllers/DashboardLavagemController.cs
+++ ATUAL: Controllers/DashboardLavagemController.cs
@@ -9,30 +9,19 @@
 using System.Linq;
 using System.Threading.Tasks;
 using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
-
     [Authorize]
     public class DashboardLavagemController : Controller
     {
         private readonly FrotiXDbContext _context;
         private readonly UserManager<IdentityUser> _userManager;
-        private readonly ILogService _log;
-
-        public DashboardLavagemController(FrotiXDbContext context, UserManager<IdentityUser> userManager, ILogService log)
-        {
-            try
-            {
-                _context = context;
-                _userManager = userManager;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "DashboardLavagemController", ex);
-            }
+
+        public DashboardLavagemController(FrotiXDbContext context, UserManager<IdentityUser> userManager)
+        {
+            _context = context;
+            _userManager = userManager;
         }
 
         [HttpGet]
@@ -98,8 +87,8 @@
                     .FirstOrDefault();
 
                 var horarioPico = lavagens
-                    .Where(l => l.HorarioInicio.HasValue)
-                    .GroupBy(l => l.HorarioInicio.Value.Hour)
+                    .Where(l => l.HorarioLavagem.HasValue)
+                    .GroupBy(l => l.HorarioLavagem.Value.Hour)
                     .Select(g => new { Hora = $"{g.Key:D2}:00", Quantidade = g.Count() })
                     .OrderByDescending(x => x.Quantidade)
                     .FirstOrDefault();
@@ -131,8 +120,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EstatisticasGerais");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EstatisticasGerais", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -143,7 +130,6 @@
         {
             try
             {
-
                 if (!dataInicio.HasValue || !dataFim.HasValue)
                 {
                     dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
@@ -167,8 +153,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorDiaSemana");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorDiaSemana", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -179,31 +163,28 @@
         {
             try
             {
-
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var lavagens = await _context.Lavagem
-                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim && l.HorarioInicio.HasValue)
+                if (!dataInicio.HasValue || !dataFim.HasValue)
+                {
+                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                    dataInicio = dataFim.Value.AddDays(-30);
+                }
+
+                var lavagens = await _context.Lavagem
+                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim && l.HorarioLavagem.HasValue)
                     .ToListAsync();
 
                 var resultado = Enumerable.Range(0, 24)
                     .Select(h => new
                     {
                         hora = $"{h:D2}:00",
-                        quantidade = lavagens.Count(l => l.HorarioInicio.Value.Hour == h)
-                    })
-                    .ToList();
-
-                return Json(new { success = true, data = resultado });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorHorario");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorHorario", ex);
+                        quantidade = lavagens.Count(l => l.HorarioLavagem.Value.Hour == h)
+                    })
+                    .ToList();
+
+                return Json(new { success = true, data = resultado });
+            }
+            catch (Exception ex)
+            {
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -238,8 +219,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EvolucaoMensal");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EvolucaoMensal", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -281,8 +260,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "TopLavadores");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "TopLavadores", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -329,8 +306,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "TopVeiculos");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "TopVeiculos", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -367,8 +342,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "TopMotoristas");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "TopMotoristas", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -386,11 +359,11 @@
                 }
 
                 var lavagens = await _context.Lavagem
-                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim && l.Data.HasValue && l.HorarioInicio.HasValue)
+                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim && l.Data.HasValue && l.HorarioLavagem.HasValue)
                     .ToListAsync();
 
                 var resultado = lavagens
-                    .GroupBy(l => new { Dia = (int)l.Data.Value.DayOfWeek, Hora = l.HorarioInicio.Value.Hour })
+                    .GroupBy(l => new { Dia = (int)l.Data.Value.DayOfWeek, Hora = l.HorarioLavagem.Value.Hour })
                     .Select(g => new
                     {
                         dia = g.Key.Dia,
@@ -403,8 +376,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "HeatmapDiaHora");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "HeatmapDiaHora", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -447,94 +418,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorContrato");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorContrato", ex);
-                return Json(new { success = false, message = ex.Message });
-            }
-        }
-
-        [HttpGet]
-        [Route("api/DashboardLavagem/DuracaoLavagens")]
-        public async Task<IActionResult> DuracaoLavagens(DateTime? dataInicio, DateTime? dataFim)
-        {
-            try
-            {
-                if (!dataInicio.HasValue || !dataFim.HasValue)
-                {
-                    dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    dataInicio = dataFim.Value.AddDays(-30);
-                }
-
-                var lavagens = await _context.Lavagem
-                    .Include(l => l.Veiculo)
-                    .Where(l => l.Data >= dataInicio && l.Data <= dataFim
-                        && l.HorarioInicio.HasValue && l.HorarioFim.HasValue)
-                    .ToListAsync();
-
-                var lavagensDuracao = lavagens
-                    .Select(l => new
-                    {
-                        l.LavagemId,
-                        Categoria = l.Veiculo?.PlacaBronzeId != null ? "PM" : (l.Veiculo?.Categoria ?? "Outros"),
-                        DuracaoMinutos = (l.HorarioFim.Value - l.HorarioInicio.Value).TotalMinutes
-                    })
-                    .Where(l => l.DuracaoMinutos > 0 && l.DuracaoMinutos < 480)
-                    .ToList();
-
-                var distribuicao = new[]
-                {
-                    new { faixa = "0-15 min", min = 0, max = 15 },
-                    new { faixa = "15-30 min", min = 15, max = 30 },
-                    new { faixa = "30-45 min", min = 30, max = 45 },
-                    new { faixa = "45-60 min", min = 45, max = 60 },
-                    new { faixa = "60+ min", min = 60, max = 999 }
-                }
-                .Select(f => new
-                {
-                    faixa = f.faixa,
-                    quantidade = lavagensDuracao.Count(l => l.DuracaoMinutos >= f.min && l.DuracaoMinutos < f.max)
-                })
-                .ToList();
-
-                var duracaoPorCategoria = lavagensDuracao
-                    .GroupBy(l => l.Categoria)
-                    .Select(g => new
-                    {
-                        categoria = g.Key,
-                        mediaMinutos = Math.Round(g.Average(l => l.DuracaoMinutos), 1),
-                        quantidade = g.Count()
-                    })
-                    .OrderByDescending(x => x.quantidade)
-                    .ToList();
-
-                var duracaoMedia = lavagensDuracao.Any()
-                    ? Math.Round(lavagensDuracao.Average(l => l.DuracaoMinutos), 1)
-                    : 0;
-                var duracaoMinima = lavagensDuracao.Any()
-                    ? Math.Round(lavagensDuracao.Min(l => l.DuracaoMinutos), 1)
-                    : 0;
-                var duracaoMaxima = lavagensDuracao.Any()
-                    ? Math.Round(lavagensDuracao.Max(l => l.DuracaoMinutos), 1)
-                    : 0;
-
-                return Json(new
-                {
-                    success = true,
-                    estatisticas = new
-                    {
-                        totalComDuracao = lavagensDuracao.Count,
-                        duracaoMedia,
-                        duracaoMinima,
-                        duracaoMaxima
-                    },
-                    distribuicao,
-                    duracaoPorCategoria
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "DuracaoLavagens");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "DuracaoLavagens", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -575,8 +458,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "LavagensPorCategoria");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "LavagensPorCategoria", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -624,8 +505,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EstatisticasPorLavador");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EstatisticasPorLavador", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -674,8 +553,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardLavagemController.cs", "EstatisticasPorVeiculo");
-                Alerta.TratamentoErroComLinha("DashboardLavagemController.cs", "EstatisticasPorVeiculo", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
```
