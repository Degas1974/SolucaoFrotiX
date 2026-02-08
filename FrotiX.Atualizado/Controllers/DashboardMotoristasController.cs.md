# Controllers/DashboardMotoristasController.cs

**Mudanca:** GRANDE | **+82** linhas | **-149** linhas

---

```diff
--- JANEIRO: Controllers/DashboardMotoristasController.cs
+++ ATUAL: Controllers/DashboardMotoristasController.cs
@@ -7,67 +7,46 @@
 using System.Linq;
 using System.Threading.Tasks;
 using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
-
     [Authorize]
     public class DashboardMotoristasController : Controller
     {
         private readonly FrotiXDbContext _context;
-        private readonly ILogService _log;
-
-        public DashboardMotoristasController(FrotiXDbContext context, ILogService log)
-        {
-            try
-            {
-                _context = context;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                _log?.Error(ex.Message, ex, "DashboardMotoristasController.cs", "Constructor");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "Constructor", ex);
-            }
+
+        public DashboardMotoristasController(FrotiXDbContext context)
+        {
+            _context = context;
         }
 
         private (DateTime dataInicio, DateTime dataFim) ObterPeriodo(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
         {
-            try
-            {
-
-                if (ano.HasValue)
-                {
-                    if (mes.HasValue)
-                    {
-                        var inicio = new DateTime(ano.Value, mes.Value, 1);
-                        var fim = inicio.AddMonths(1).AddSeconds(-1);
-                        return (inicio, fim);
-                    }
-                    else
-                    {
-                        var inicio = new DateTime(ano.Value, 1, 1);
-                        var fim = new DateTime(ano.Value, 12, 31, 23, 59, 59);
-                        return (inicio, fim);
-                    }
-                }
-                else if (dataInicio.HasValue && dataFim.HasValue)
-                {
-                    return (dataInicio.Value, dataFim.Value);
+            if (ano.HasValue)
+            {
+                if (mes.HasValue)
+                {
+                    var inicio = new DateTime(ano.Value, mes.Value, 1);
+                    var fim = inicio.AddMonths(1).AddSeconds(-1);
+                    return (inicio, fim);
                 }
                 else
                 {
-                    var fim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
-                    var inicio = fim.AddDays(-30);
+                    var inicio = new DateTime(ano.Value, 1, 1);
+                    var fim = new DateTime(ano.Value, 12, 31, 23, 59, 59);
                     return (inicio, fim);
                 }
             }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterPeriodo");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterPeriodo", ex);
-                return (DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(1).AddSeconds(-1));
+            else if (dataInicio.HasValue && dataFim.HasValue)
+            {
+                return (dataInicio.Value, dataFim.Value);
+            }
+            else
+            {
+
+                var fim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
+                var inicio = fim.AddDays(-30);
+                return (inicio, fim);
             }
         }
 
@@ -77,7 +56,6 @@
         {
             try
             {
-
                 var hoje = DateTime.Now;
                 var anoAtual = hoje.Year;
                 var mesAtual = hoje.Month;
@@ -90,7 +68,6 @@
 
                 if (!anosViagens.Any())
                 {
-
                     anosViagens = await _context.Viagem
                         .Where(v => v.DataInicial.HasValue && v.MotoristaId.HasValue)
                         .Select(v => new { Ano = v.DataInicial.Value.Year, Mes = v.DataInicial.Value.Month })
@@ -132,8 +109,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterAnosMesesDisponiveis");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterAnosMesesDisponiveis", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -144,7 +119,6 @@
         {
             try
             {
-
                 var hoje = DateTime.Now;
                 var anoAtual = hoje.Year;
                 var mesAtual = hoje.Month;
@@ -158,7 +132,6 @@
 
                 if (!meses.Any())
                 {
-
                     meses = await _context.Viagem
                         .Where(v => v.DataInicial.HasValue && v.MotoristaId.HasValue)
                         .Where(v => v.DataInicial.Value.Year == ano)
@@ -178,8 +151,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterMesesPorAno");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterMesesPorAno", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -205,8 +176,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterListaMotoristas");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterListaMotoristas", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -217,6 +186,7 @@
         {
             try
             {
+
                 if (ano.HasValue)
                 {
                     if (mes.HasValue)
@@ -232,6 +202,7 @@
                 }
                 else if (!dataInicio.HasValue || !dataFim.HasValue)
                 {
+
                     var ultimoMes = await _context.EstatisticaGeralMensal
                         .AsNoTracking()
                         .OrderByDescending(e => e.Ano)
@@ -257,6 +228,7 @@
 
                 if (ano.HasValue && mes.HasValue)
                 {
+
                     var estatGeral = await _context.EstatisticaGeralMensal
                         .AsNoTracking()
                         .Where(e => e.Ano == ano && e.Mes == mes)
@@ -264,6 +236,7 @@
 
                     if (estatGeral != null)
                     {
+
                         var cnhStats = await _context.Motorista
                             .AsNoTracking()
                             .Where(m => m.Status == true && m.DataVencimentoCNH.HasValue)
@@ -300,87 +273,76 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterEstatisticasGerais");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterEstatisticasGerais", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
 
         private async Task<IActionResult> ObterEstatisticasGeraisFallback(DateTime? dataInicio, DateTime? dataFim, DateTime hoje)
         {
-            try
-            {
-                var statsMotoristas = await _context.Motorista
-                    .AsNoTracking()
-                    .GroupBy(m => 1)
-                    .Select(g => new
-                    {
-                        total = g.Count(),
-                        ativos = g.Count(m => m.Status == true),
-                        inativos = g.Count(m => m.Status == false),
-                        efetivos = g.Count(m => m.Status == true && (m.EfetivoFerista == "Efetivo" || m.EfetivoFerista == null || m.EfetivoFerista == "")),
-                        feristas = g.Count(m => m.Status == true && m.EfetivoFerista == "Ferista"),
-                        cobertura = g.Count(m => m.Status == true && m.EfetivoFerista == "Cobertura"),
-                        cnhVencidas = g.Count(m => m.Status == true && m.DataVencimentoCNH.HasValue && m.DataVencimentoCNH.Value < hoje),
-                        cnhVencendo = g.Count(m => m.Status == true && m.DataVencimentoCNH.HasValue && m.DataVencimentoCNH.Value >= hoje && m.DataVencimentoCNH.Value <= hoje.AddDays(30))
-                    })
-                    .FirstOrDefaultAsync();
-
-                var viagensStats = await _context.Viagem
-                    .AsNoTracking()
-                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
-                    .Where(v => v.MotoristaId.HasValue)
-                    .Select(v => new { v.KmInicial, v.KmFinal, v.Minutos })
-                    .ToListAsync();
-
-                var totalViagens = viagensStats.Count;
-                var kmTotal = viagensStats
-                    .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
-                    .Where(v => v.KmFinal.Value >= v.KmInicial.Value)
-                    .Where(v => (v.KmFinal.Value - v.KmInicial.Value) <= 2000m)
-                    .Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0));
-                var horasTotais = viagensStats.Sum(v => v.Minutos ?? 0) / 60.0;
-
-                var multasStats = await _context.Multa
-                    .AsNoTracking()
-                    .Where(m => m.Data >= dataInicio && m.Data <= dataFim)
-                    .GroupBy(m => 1)
-                    .Select(g => new { total = g.Count(), valorTotal = g.Sum(m => m.ValorAteVencimento ?? 0) })
-                    .FirstOrDefaultAsync();
-
-                var abastecimentos = await _context.Abastecimento
-                    .AsNoTracking()
-                    .Where(a => a.DataHora >= dataInicio && a.DataHora <= dataFim)
-                    .Where(a => a.MotoristaId != Guid.Empty)
-                    .CountAsync();
-
-                return Json(new
-                {
-                    success = true,
-                    totalMotoristas = statsMotoristas?.total ?? 0,
-                    motoristasAtivos = statsMotoristas?.ativos ?? 0,
-                    motoristasInativos = statsMotoristas?.inativos ?? 0,
-                    efetivos = statsMotoristas?.efetivos ?? 0,
-                    feristas = statsMotoristas?.feristas ?? 0,
-                    cobertura = statsMotoristas?.cobertura ?? 0,
-                    cnhVencidas = statsMotoristas?.cnhVencidas ?? 0,
-                    cnhVencendo30Dias = statsMotoristas?.cnhVencendo ?? 0,
-                    totalViagens,
-                    kmTotal,
-                    horasTotais = Math.Round(horasTotais, 1),
-                    totalMultas = multasStats?.total ?? 0,
-                    valorTotalMultas = Math.Round(multasStats?.valorTotal ?? 0, 2),
-                    abastecimentos,
-                    periodoInicio = dataInicio?.ToString("yyyy-MM-dd"),
-                    periodoFim = dataFim?.ToString("yyyy-MM-dd")
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterEstatisticasGeraisFallback");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterEstatisticasGeraisFallback", ex);
-                return Json(new { success = false, message = ex.Message });
-            }
+            var statsMotoristas = await _context.Motorista
+                .AsNoTracking()
+                .GroupBy(m => 1)
+                .Select(g => new
+                {
+                    total = g.Count(),
+                    ativos = g.Count(m => m.Status == true),
+                    inativos = g.Count(m => m.Status == false),
+                    efetivos = g.Count(m => m.Status == true && (m.EfetivoFerista == "Efetivo" || m.EfetivoFerista == null || m.EfetivoFerista == "")),
+                    feristas = g.Count(m => m.Status == true && m.EfetivoFerista == "Ferista"),
+                    cobertura = g.Count(m => m.Status == true && m.EfetivoFerista == "Cobertura"),
+                    cnhVencidas = g.Count(m => m.Status == true && m.DataVencimentoCNH.HasValue && m.DataVencimentoCNH.Value < hoje),
+                    cnhVencendo = g.Count(m => m.Status == true && m.DataVencimentoCNH.HasValue && m.DataVencimentoCNH.Value >= hoje && m.DataVencimentoCNH.Value <= hoje.AddDays(30))
+                })
+                .FirstOrDefaultAsync();
+
+            var viagensStats = await _context.Viagem
+                .AsNoTracking()
+                .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
+                .Where(v => v.MotoristaId.HasValue)
+                .Select(v => new { v.KmInicial, v.KmFinal, v.Minutos })
+                .ToListAsync();
+
+            var totalViagens = viagensStats.Count;
+            var kmTotal = viagensStats
+                .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
+                .Where(v => v.KmFinal.Value >= v.KmInicial.Value)
+                .Where(v => (v.KmFinal.Value - v.KmInicial.Value) <= 2000m)
+                .Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0));
+            var horasTotais = viagensStats.Sum(v => v.Minutos ?? 0) / 60.0;
+
+            var multasStats = await _context.Multa
+                .AsNoTracking()
+                .Where(m => m.Data >= dataInicio && m.Data <= dataFim)
+                .GroupBy(m => 1)
+                .Select(g => new { total = g.Count(), valorTotal = g.Sum(m => m.ValorAteVencimento ?? 0) })
+                .FirstOrDefaultAsync();
+
+            var abastecimentos = await _context.Abastecimento
+                .AsNoTracking()
+                .Where(a => a.DataHora >= dataInicio && a.DataHora <= dataFim)
+                .Where(a => a.MotoristaId != Guid.Empty)
+                .CountAsync();
+
+            return Json(new
+            {
+                success = true,
+                totalMotoristas = statsMotoristas?.total ?? 0,
+                motoristasAtivos = statsMotoristas?.ativos ?? 0,
+                motoristasInativos = statsMotoristas?.inativos ?? 0,
+                efetivos = statsMotoristas?.efetivos ?? 0,
+                feristas = statsMotoristas?.feristas ?? 0,
+                cobertura = statsMotoristas?.cobertura ?? 0,
+                cnhVencidas = statsMotoristas?.cnhVencidas ?? 0,
+                cnhVencendo30Dias = statsMotoristas?.cnhVencendo ?? 0,
+                totalViagens,
+                kmTotal,
+                horasTotais = Math.Round(horasTotais, 1),
+                totalMultas = multasStats?.total ?? 0,
+                valorTotalMultas = Math.Round(multasStats?.valorTotal ?? 0, 2),
+                abastecimentos,
+                periodoInicio = dataInicio?.ToString("yyyy-MM-dd"),
+                periodoFim = dataFim?.ToString("yyyy-MM-dd")
+            });
         }
 
         [HttpGet]
@@ -445,6 +407,7 @@
                 }
                 else
                 {
+
                     var viagens = await _context.Viagem
                         .Where(v => v.MotoristaId == motoristaId)
                         .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
@@ -535,8 +498,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterDadosMotorista");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterDadosMotorista", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -547,6 +508,7 @@
         {
             try
             {
+
                 if (ano.HasValue && mes.HasValue)
                 {
                     var ranking = await _context.RankingMotoristasMensal
@@ -586,8 +548,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterTop10PorViagens");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterTop10PorViagens", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -598,6 +558,7 @@
         {
             try
             {
+
                 if (ano.HasValue && mes.HasValue)
                 {
                     var ranking = await _context.RankingMotoristasMensal
@@ -644,8 +605,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterTop10PorKm");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterTop10PorKm", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -677,8 +636,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterDistribuicaoPorTipo");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterDistribuicaoPorTipo", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -706,8 +663,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterDistribuicaoPorStatus");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterDistribuicaoPorStatus", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -770,8 +725,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterEvolucaoViagens");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterEvolucaoViagens", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -782,6 +735,7 @@
         {
             try
             {
+
                 if (ano.HasValue && mes.HasValue)
                 {
                     var ranking = await _context.RankingMotoristasMensal
@@ -826,8 +780,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterTop10PorHoras");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterTop10PorHoras", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -838,6 +790,7 @@
         {
             try
             {
+
                 if (ano.HasValue && mes.HasValue)
                 {
                     var ranking = await _context.RankingMotoristasMensal
@@ -881,8 +834,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterTop10PorAbastecimentos");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterTop10PorAbastecimentos", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -893,6 +844,7 @@
         {
             try
             {
+
                 if (ano.HasValue && mes.HasValue)
                 {
                     var ranking = await _context.RankingMotoristasMensal
@@ -938,8 +890,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterMotoristasComMaisMultas");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterMotoristasComMaisMultas", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -990,8 +940,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterDistribuicaoPorTempoEmpresa");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterDistribuicaoPorTempoEmpresa", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -1036,8 +984,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterMotoristasComCnhProblema");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterMotoristasComCnhProblema", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -1048,6 +994,7 @@
         {
             try
             {
+
                 if (ano.HasValue && mes.HasValue)
                 {
                     var ranking = await _context.RankingMotoristasMensal
@@ -1121,8 +1068,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterTop10Performance");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterTop10Performance", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -1226,8 +1171,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterHeatmapViagens");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterHeatmapViagens", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -1238,6 +1181,7 @@
         {
             try
             {
+
                 int anoFiltro, mesFiltro;
                 if (ano.HasValue && mes.HasValue)
                 {
@@ -1327,8 +1271,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterPosicaoMotorista");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterPosicaoMotorista", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -1351,10 +1293,8 @@
 
                 return File(foto, "image/jpeg");
             }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardMotoristasController.cs", "ObterFotoMotorista");
-                Alerta.TratamentoErroComLinha("DashboardMotoristasController.cs", "ObterFotoMotorista", ex);
+            catch
+            {
                 return NotFound();
             }
         }
```
