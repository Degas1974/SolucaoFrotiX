# Controllers/DashboardViagensController.cs

**Mudanca:** GRANDE | **+175** linhas | **-236** linhas

---

```diff
--- JANEIRO: Controllers/DashboardViagensController.cs
+++ ATUAL: Controllers/DashboardViagensController.cs
@@ -16,7 +16,6 @@
 using Syncfusion.Pdf.Grid;
 using System.Text.Json;
 using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -26,27 +25,18 @@
     {
         private readonly FrotiXDbContext _context;
         private readonly UserManager<IdentityUser> _userManager;
-        private readonly ILogService _log;
 
         private const decimal KM_MAXIMO_POR_VIAGEM = 2000m;
 
-        public DashboardViagensController(FrotiXDbContext context, UserManager<IdentityUser> userManager, ILogService logService)
-        {
-            try
-            {
-                _context = context;
-                _userManager = userManager;
-                _log = logService;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "Constructor", ex);
-            }
+        public DashboardViagensController(FrotiXDbContext context , UserManager<IdentityUser> userManager)
+        {
+            _context = context;
+            _userManager = userManager;
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterEstatisticasGerais")]
-        public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio, DateTime? dataFim)
+        public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio , DateTime? dataFim)
         {
             try
             {
@@ -58,7 +48,6 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                     .ToListAsync();
 
@@ -99,7 +88,6 @@
                 var dataFimAnterior = dataInicio.Value.AddSeconds(-1);
 
                 var viagensAnteriores = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicioAnterior && v.DataInicial <= dataFimAnterior)
                     .ToListAsync();
 
@@ -134,53 +122,54 @@
 
                 return Json(new
                 {
-                    success = true,
-                    totalViagens,
-                    viagensFinalizadas,
-                    viagensAgendadas,
-                    viagensCanceladas,
-                    viagensEmAndamento,
-                    custoTotal = Math.Round(custoTotal, 2),
-                    custoCombustivel = Math.Round(custoCombustivel, 2),
-                    custoLavador = Math.Round(custoLavador, 2),
-                    custoMotorista = Math.Round(custoMotorista, 2),
-                    custoOperador = Math.Round(custoOperador, 2),
-                    custoVeiculo = Math.Round(custoVeiculo, 2),
-                    kmTotal,
-                    kmMedioPorViagem = Math.Round(kmMedioPorViagem, 2),
-                    custoMedioPorViagem = Math.Round(custoMedioPorViagem, 2),
-                    viagensComKm = viagens.Count(v => v.KmInicial.HasValue && v.KmFinal.HasValue),
-                    viagensKmValido = viagensParaMedia,
+                    success = true ,
+
+                    totalViagens ,
+                    viagensFinalizadas ,
+                    viagensAgendadas ,
+                    viagensCanceladas ,
+                    viagensEmAndamento ,
+                    custoTotal = Math.Round(custoTotal , 2) ,
+                    custoCombustivel = Math.Round(custoCombustivel , 2) ,
+                    custoLavador = Math.Round(custoLavador , 2) ,
+                    custoMotorista = Math.Round(custoMotorista , 2) ,
+                    custoOperador = Math.Round(custoOperador , 2) ,
+                    custoVeiculo = Math.Round(custoVeiculo , 2) ,
+                    kmTotal ,
+                    kmMedioPorViagem = Math.Round(kmMedioPorViagem , 2) ,
+                    custoMedioPorViagem = Math.Round(custoMedioPorViagem , 2) ,
+
+                    viagensComKm = viagens.Count(v => v.KmInicial.HasValue && v.KmFinal.HasValue) ,
+                    viagensKmValido = viagensParaMedia ,
+
                     periodoAnterior = new
                     {
-                        totalViagens = totalViagensAnteriores,
-                        viagensFinalizadas = viagensFinalizadasAnterior,
-                        viagensAgendadas = viagensAgendadasAnterior,
-                        viagensCanceladas = viagensCanceladasAnterior,
-                        viagensEmAndamento = viagensEmAndamentoAnterior,
-                        custoTotal = Math.Round(custoTotalAnterior, 2),
-                        custoCombustivel = Math.Round(custoCombustivelAnterior, 2),
-                        custoLavador = Math.Round(custoLavadorAnterior, 2),
-                        custoMotorista = Math.Round(custoMotoristaAnterior, 2),
-                        custoOperador = Math.Round(custoOperadorAnterior, 2),
-                        custoVeiculo = Math.Round(custoVeiculoAnterior, 2),
-                        custoMedioPorViagem = Math.Round(custoMedioPorViagemAnterior, 2),
-                        kmTotal = kmTotalAnterior,
-                        kmMedioPorViagem = Math.Round(kmMedioPorViagemAnterior, 2)
+                        totalViagens = totalViagensAnteriores ,
+                        viagensFinalizadas = viagensFinalizadasAnterior ,
+                        viagensAgendadas = viagensAgendadasAnterior ,
+                        viagensCanceladas = viagensCanceladasAnterior ,
+                        viagensEmAndamento = viagensEmAndamentoAnterior ,
+                        custoTotal = Math.Round(custoTotalAnterior , 2) ,
+                        custoCombustivel = Math.Round(custoCombustivelAnterior , 2) ,
+                        custoLavador = Math.Round(custoLavadorAnterior , 2) ,
+                        custoMotorista = Math.Round(custoMotoristaAnterior , 2) ,
+                        custoOperador = Math.Round(custoOperadorAnterior , 2) ,
+                        custoVeiculo = Math.Round(custoVeiculoAnterior , 2) ,
+                        custoMedioPorViagem = Math.Round(custoMedioPorViagemAnterior , 2) ,
+                        kmTotal = kmTotalAnterior ,
+                        kmMedioPorViagem = Math.Round(kmMedioPorViagemAnterior , 2)
                     }
                 });
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterEstatisticasGerais");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterEstatisticasGerais", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterViagensPorDia")]
-        public async Task<IActionResult> ObterViagensPorDia(DateTime? dataInicio, DateTime? dataFim)
+        public async Task<IActionResult> ObterViagensPorDia(DateTime? dataInicio , DateTime? dataFim)
         {
             try
             {
@@ -191,49 +180,46 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim && v.DataInicial.HasValue)
                     .ToListAsync();
 
-                var diasSemana = new Dictionary<DayOfWeek, (string Nome, int Ordem)>
-                {
-                    { DayOfWeek.Monday, ("Segunda", 1) },
-                    { DayOfWeek.Tuesday, ("Terça", 2) },
-                    { DayOfWeek.Wednesday, ("Quarta", 3) },
-                    { DayOfWeek.Thursday, ("Quinta", 4) },
-                    { DayOfWeek.Friday, ("Sexta", 5) },
-                    { DayOfWeek.Saturday, ("Sábado", 6) },
-                    { DayOfWeek.Sunday, ("Domingo", 7) }
+                var diasSemana = new Dictionary<DayOfWeek , (string Nome, int Ordem)>
+                {
+                    { DayOfWeek.Monday , ("Segunda" , 1) } ,
+                    { DayOfWeek.Tuesday , ("Terça" , 2) } ,
+                    { DayOfWeek.Wednesday , ("Quarta" , 3) } ,
+                    { DayOfWeek.Thursday , ("Quinta" , 4) } ,
+                    { DayOfWeek.Friday , ("Sexta" , 5) } ,
+                    { DayOfWeek.Saturday , ("Sábado" , 6) } ,
+                    { DayOfWeek.Sunday , ("Domingo" , 7) }
                 };
 
                 var viagensPorDiaSemana = viagens
                     .GroupBy(v => v.DataInicial.Value.DayOfWeek)
                     .Select(g => new
                     {
-                        diaSemana = diasSemana[g.Key].Nome,
-                        ordem = diasSemana[g.Key].Ordem,
-                        total = g.Count(),
-                        finalizadas = g.Count(v => v.Status == "Realizada"),
-                        agendadas = g.Count(v => v.Status == "Agendada"),
-                        canceladas = g.Count(v => v.Status == "Cancelada"),
+                        diaSemana = diasSemana[g.Key].Nome ,
+                        ordem = diasSemana[g.Key].Ordem ,
+                        total = g.Count() ,
+                        finalizadas = g.Count(v => v.Status == "Realizada") ,
+                        agendadas = g.Count(v => v.Status == "Agendada") ,
+                        canceladas = g.Count(v => v.Status == "Cancelada") ,
                         emAndamento = g.Count(v => v.Status == "Aberta")
                     })
                     .OrderBy(x => x.ordem)
                     .ToList();
 
-                return Json(new { success = true, data = viagensPorDiaSemana });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorDia");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorDia", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = viagensPorDiaSemana });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterViagensPorStatus")]
-        public async Task<IActionResult> ObterViagensPorStatus(DateTime? dataInicio, DateTime? dataFim)
+        public async Task<IActionResult> ObterViagensPorStatus(DateTime? dataInicio , DateTime? dataFim)
         {
             try
             {
@@ -244,7 +230,6 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                     .ToListAsync();
 
@@ -252,25 +237,23 @@
                     .GroupBy(v => v.Status ?? "Não Informado")
                     .Select(g => new
                     {
-                        status = g.Key,
+                        status = g.Key ,
                         total = g.Count()
                     })
                     .OrderByDescending(x => x.total)
                     .ToList();
 
-                return Json(new { success = true, data = viagensPorStatus });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorStatus");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorStatus", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = viagensPorStatus });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterViagensPorMotorista")]
-        public async Task<IActionResult> ObterViagensPorMotorista(DateTime? dataInicio, DateTime? dataFim, int top = 10)
+        public async Task<IActionResult> ObterViagensPorMotorista(DateTime? dataInicio , DateTime? dataFim , int top = 10)
         {
             try
             {
@@ -281,11 +264,10 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
-                var motoristaDict = new Dictionary<string, int>();
+                var motoristaDict = new Dictionary<string , int>();
 
                 foreach (var est in estatisticas)
                 {
@@ -294,7 +276,7 @@
                         try
                         {
                             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
-                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorMotoristaJson, options);
+                            var lista = JsonSerializer.Deserialize<List<Dictionary<string , JsonElement>>>(est.ViagensPorMotoristaJson , options);
 
                             if (lista != null)
                             {
@@ -329,24 +311,22 @@
                 }
 
                 var dados = motoristaDict
-                    .Select(kv => new { motorista = kv.Key, totalViagens = kv.Value })
+                    .Select(kv => new { motorista = kv.Key , totalViagens = kv.Value })
                     .OrderByDescending(x => x.totalViagens)
                     .Take(top)
                     .ToList();
 
-                return Json(new { success = true, data = dados });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorMotorista");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorMotorista", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterViagensPorSetor")]
-        public async Task<IActionResult> ObterViagensPorSetor(DateTime? dataInicio, DateTime? dataFim, int top = 6)
+        public async Task<IActionResult> ObterViagensPorSetor(DateTime? dataInicio , DateTime? dataFim , int top = 6)
         {
             try
             {
@@ -357,24 +337,22 @@
                 }
 
                 var setoresDb = await _context.SetorSolicitante
-                    .AsNoTracking()
                     .Where(s => s.Status == true && !string.IsNullOrEmpty(s.Nome))
-                    .Select(s => new { s.Nome, s.Sigla })
+                    .Select(s => new { s.Nome , s.Sigla })
                     .ToListAsync();
 
                 var dictNomeParaSigla = setoresDb
                     .GroupBy(s => s.Nome.Trim().ToUpper())
                     .ToDictionary(
-                        g => g.Key,
+                        g => g.Key ,
                         g => !string.IsNullOrEmpty(g.First().Sigla) ? g.First().Sigla.Trim() : g.First().Nome.Trim()
                     );
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
-                var setorDict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
+                var setorDict = new Dictionary<string , int>(StringComparer.OrdinalIgnoreCase);
 
                 foreach (var est in estatisticas)
                 {
@@ -383,7 +361,7 @@
                         try
                         {
                             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
-                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorSetorJson, options);
+                            var lista = JsonSerializer.Deserialize<List<Dictionary<string , JsonElement>>>(est.ViagensPorSetorJson , options);
 
                             if (lista != null)
                             {
@@ -402,6 +380,7 @@
 
                                     if (!string.IsNullOrEmpty(setor) && totalViagens > 0)
                                     {
+
                                         var chaveSetor = setor.Trim().ToUpper();
                                         var siglaOuNome = dictNomeParaSigla.ContainsKey(chaveSetor)
                                             ? dictNomeParaSigla[chaveSetor]
@@ -432,24 +411,22 @@
                 }
 
                 var dados = setorDict
-                    .Select(kv => new { setor = kv.Key, totalViagens = kv.Value })
+                    .Select(kv => new { setor = kv.Key , totalViagens = kv.Value })
                     .OrderByDescending(x => x.totalViagens)
                     .Take(top)
                     .ToList();
 
-                return Json(new { success = true, data = dados, viagensCtran = viagensCtran });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorSetor");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorSetor", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados , viagensCtran = viagensCtran });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterCustosPorMotorista")]
-        public async Task<IActionResult> ObterCustosPorMotorista(DateTime? dataInicio, DateTime? dataFim, int top = 10)
+        public async Task<IActionResult> ObterCustosPorMotorista(DateTime? dataInicio , DateTime? dataFim , int top = 10)
         {
             try
             {
@@ -460,11 +437,11 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
-                var motoristaDict = new Dictionary<string, decimal>();
+                var motoristaDict = new Dictionary<string , decimal>();
+                var erros = new List<string>();
 
                 foreach (var est in estatisticas)
                 {
@@ -473,7 +450,7 @@
                         try
                         {
                             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
-                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.CustosPorMotoristaJson, options);
+                            var lista = JsonSerializer.Deserialize<List<Dictionary<string , JsonElement>>>(est.CustosPorMotoristaJson , options);
 
                             if (lista != null)
                             {
@@ -494,7 +471,7 @@
                         }
                         catch (Exception ex)
                         {
-                            _log.Error($"Erro ao deserializar custos: {ex.Message}", ex, "DashboardViagensController.cs", "ObterCustosPorMotorista");
+                            erros.Add($"Erro deserializar: {ex.Message}");
                         }
                     }
                 }
@@ -502,8 +479,8 @@
                 var dados = motoristaDict
                     .Select(kv => new
                     {
-                        motorista = kv.Key,
-                        custoTotal = Math.Round(kv.Value, 2)
+                        motorista = kv.Key ,
+                        custoTotal = Math.Round(kv.Value , 2)
                     })
                     .OrderByDescending(x => x.custoTotal)
                     .Take(top)
@@ -511,26 +488,25 @@
 
                 return Json(new
                 {
-                    success = true,
-                    data = dados,
+                    success = true ,
+                    data = dados ,
                     debug = new
                     {
-                        totalEstatisticas = estatisticas.Count,
-                        totalMotoristas = motoristaDict.Count
+                        totalEstatisticas = estatisticas.Count ,
+                        totalMotoristas = motoristaDict.Count ,
+                        erros = erros
                     }
                 });
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorMotorista");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorMotorista", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterCustosPorVeiculo")]
-        public async Task<IActionResult> ObterCustosPorVeiculo(DateTime? dataInicio, DateTime? dataFim, int top = 10)
+        public async Task<IActionResult> ObterCustosPorVeiculo(DateTime? dataInicio , DateTime? dataFim , int top = 10)
         {
             try
             {
@@ -541,7 +517,6 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim && v.VeiculoId != null)
                     .Where(v => v.Status == "Realizada")
                     .Include(v => v.Veiculo)
@@ -552,34 +527,32 @@
                 var custosPorVeiculo = viagens
                     .GroupBy(v => new
                     {
-                        v.VeiculoId,
-                        v.Veiculo.Placa,
+                        v.VeiculoId ,
+                        v.Veiculo.Placa ,
                         Descricao = v.Veiculo.ModeloVeiculo != null && v.Veiculo.ModeloVeiculo.MarcaVeiculo != null
                             ? $"{v.Veiculo.ModeloVeiculo.MarcaVeiculo.DescricaoMarca} {v.Veiculo.ModeloVeiculo.DescricaoModelo} - {v.Veiculo.Placa}"
                             : v.Veiculo.Placa ?? "Não informado"
                     })
                     .Select(g => new
                     {
-                        veiculoId = g.Key.VeiculoId,
-                        veiculo = g.Key.Descricao ?? "Não informado",
+                        veiculoId = g.Key.VeiculoId ,
+                        veiculo = g.Key.Descricao ?? "Não informado" ,
                         custoTotal = Math.Round(
                             g.Sum(v => v.CustoCombustivel ?? 0) +
                             g.Sum(v => v.CustoLavador ?? 0) +
                             g.Sum(v => v.CustoMotorista ?? 0) +
                             g.Sum(v => v.CustoOperador ?? 0) +
-                            g.Sum(v => v.CustoVeiculo ?? 0), 2)
+                            g.Sum(v => v.CustoVeiculo ?? 0) , 2)
                     })
                     .OrderByDescending(x => x.custoTotal)
                     .Take(top)
                     .ToList();
 
-                return Json(new { success = true, data = custosPorVeiculo });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorVeiculo");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorVeiculo", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = custosPorVeiculo });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
@@ -596,7 +569,6 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                     .Where(v => v.Status == "Realizada")
                     .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
@@ -624,16 +596,21 @@
                     veiculo = v.Veiculo != null && v.Veiculo.ModeloVeiculo != null && v.Veiculo.ModeloVeiculo.MarcaVeiculo != null
                         ? "(" + v.Veiculo.Placa + ") - " + v.Veiculo.ModeloVeiculo.MarcaVeiculo.DescricaoMarca + "/" + v.Veiculo.ModeloVeiculo.DescricaoModelo
                         : v.Veiculo != null ? v.Veiculo.Placa : "Não informado",
+
                     kmRodado = v.KmInicial.HasValue && v.KmFinal.HasValue
                         ? v.KmFinal.Value - v.KmInicial.Value
                         : 0m,
+
                     minutos = v.Minutos ?? 0,
+
                     finalidade = v.Finalidade ?? "-",
+
                     custoCombustivel = Math.Round(v.CustoCombustivel ?? 0d, 2),
                     custoVeiculo = Math.Round(v.CustoVeiculo ?? 0d, 2),
                     custoMotorista = Math.Round(v.CustoMotorista ?? 0d, 2),
                     custoOperador = Math.Round(v.CustoOperador ?? 0d, 2),
                     custoLavador = Math.Round(v.CustoLavador ?? 0d, 2),
+
                     custoTotal = Math.Round(
                         (v.CustoCombustivel ?? 0d) +
                         (v.CustoLavador ?? 0d) +
@@ -646,15 +623,13 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterTop10ViagensMaisCaras");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterTop10ViagensMaisCaras", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterCustosPorDia")]
-        public async Task<IActionResult> ObterCustosPorDia(DateTime? dataInicio, DateTime? dataFim)
+        public async Task<IActionResult> ObterCustosPorDia(DateTime? dataInicio , DateTime? dataFim)
         {
             try
             {
@@ -665,34 +640,31 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .OrderBy(v => v.DataReferencia)
                     .ToListAsync();
 
                 var dados = estatisticas.Select(e => new
                 {
-                    data = e.DataReferencia.ToString("yyyy-MM-dd"),
-                    combustivel = Math.Round(e.CustoCombustivel, 2),
-                    motorista = Math.Round(e.CustoMotorista, 2),
-                    operador = Math.Round(e.CustoOperador, 2),
-                    lavador = Math.Round(e.CustoLavador, 2),
-                    veiculo = Math.Round(e.CustoVeiculo, 2)
+                    data = e.DataReferencia.ToString("yyyy-MM-dd") ,
+                    combustivel = Math.Round(e.CustoCombustivel , 2) ,
+                    motorista = Math.Round(e.CustoMotorista , 2) ,
+                    operador = Math.Round(e.CustoOperador , 2) ,
+                    lavador = Math.Round(e.CustoLavador , 2) ,
+                    veiculo = Math.Round(e.CustoVeiculo , 2)
                 }).ToList();
 
-                return Json(new { success = true, data = dados });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorDia");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorDia", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterCustosPorTipo")]
-        public async Task<IActionResult> ObterCustosPorTipo(DateTime? dataInicio, DateTime? dataFim)
+        public async Task<IActionResult> ObterCustosPorTipo(DateTime? dataInicio , DateTime? dataFim)
         {
             try
             {
@@ -703,41 +675,38 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
                 var totais = new
                 {
-                    custoCombustivel = Math.Round(estatisticas.Sum(e => e.CustoCombustivel), 2),
-                    custoMotorista = Math.Round(estatisticas.Sum(e => e.CustoMotorista), 2),
-                    custoOperador = Math.Round(estatisticas.Sum(e => e.CustoOperador), 2),
-                    custoLavador = Math.Round(estatisticas.Sum(e => e.CustoLavador), 2),
-                    custoVeiculo = Math.Round(estatisticas.Sum(e => e.CustoVeiculo), 2)
+                    custoCombustivel = Math.Round(estatisticas.Sum(e => e.CustoCombustivel) , 2) ,
+                    custoMotorista = Math.Round(estatisticas.Sum(e => e.CustoMotorista) , 2) ,
+                    custoOperador = Math.Round(estatisticas.Sum(e => e.CustoOperador) , 2) ,
+                    custoLavador = Math.Round(estatisticas.Sum(e => e.CustoLavador) , 2) ,
+                    custoVeiculo = Math.Round(estatisticas.Sum(e => e.CustoVeiculo) , 2)
                 };
 
                 var dados = new[]
                 {
-                    new { tipo = "Combustível", custo = totais.custoCombustivel },
-                    new { tipo = "Motorista", custo = totais.custoMotorista },
-                    new { tipo = "Operador", custo = totais.custoOperador },
-                    new { tipo = "Lavador", custo = totais.custoLavador },
-                    new { tipo = "Veículo", custo = totais.custoVeiculo }
+                    new { tipo = "Combustível" , custo = totais.custoCombustivel } ,
+                    new { tipo = "Motorista" , custo = totais.custoMotorista } ,
+                    new { tipo = "Operador" , custo = totais.custoOperador } ,
+                    new { tipo = "Lavador" , custo = totais.custoLavador } ,
+                    new { tipo = "Veículo" , custo = totais.custoVeiculo }
                 }.Where(x => x.custo > 0).ToList();
 
-                return Json(new { success = true, data = dados });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustosPorTipo");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustosPorTipo", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterViagensPorVeiculo")]
-        public async Task<IActionResult> ObterViagensPorVeiculo(DateTime? dataInicio, DateTime? dataFim, int top = 10)
+        public async Task<IActionResult> ObterViagensPorVeiculo(DateTime? dataInicio , DateTime? dataFim , int top = 10)
         {
             try
             {
@@ -748,11 +717,10 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
-                var veiculoDict = new Dictionary<string, int>();
+                var veiculoDict = new Dictionary<string , int>();
 
                 foreach (var est in estatisticas)
                 {
@@ -761,7 +729,7 @@
                         try
                         {
                             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
-                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorVeiculoJson, options);
+                            var lista = JsonSerializer.Deserialize<List<Dictionary<string , JsonElement>>>(est.ViagensPorVeiculoJson , options);
 
                             if (lista != null)
                             {
@@ -795,24 +763,22 @@
                 }
 
                 var dados = veiculoDict
-                    .Select(kv => new { veiculo = kv.Key, totalViagens = kv.Value })
+                    .Select(kv => new { veiculo = kv.Key , totalViagens = kv.Value })
                     .OrderByDescending(x => x.totalViagens)
                     .Take(top)
                     .ToList();
 
-                return Json(new { success = true, data = dados });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorVeiculo");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorVeiculo", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterViagensPorFinalidade")]
-        public async Task<IActionResult> ObterViagensPorFinalidade(DateTime? dataInicio, DateTime? dataFim)
+        public async Task<IActionResult> ObterViagensPorFinalidade(DateTime? dataInicio , DateTime? dataFim)
         {
             try
             {
@@ -823,11 +789,10 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
-                var finalidadeDict = new Dictionary<string, int>();
+                var finalidadeDict = new Dictionary<string , int>();
 
                 foreach (var est in estatisticas)
                 {
@@ -836,7 +801,7 @@
                         try
                         {
                             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
-                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorFinalidadeJson, options);
+                            var lista = JsonSerializer.Deserialize<List<Dictionary<string , JsonElement>>>(est.ViagensPorFinalidadeJson , options);
 
                             if (lista != null)
                             {
@@ -868,24 +833,22 @@
                 }
 
                 var dados = finalidadeDict
-                    .Select(kv => new { finalidade = kv.Key, total = kv.Value })
+                    .Select(kv => new { finalidade = kv.Key , total = kv.Value })
                     .OrderByDescending(x => x.total)
                     .Take(15)
                     .ToList();
 
-                return Json(new { success = true, data = dados });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorFinalidade");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorFinalidade", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterKmPorVeiculo")]
-        public async Task<IActionResult> ObterKmPorVeiculo(DateTime? dataInicio, DateTime? dataFim, int top = 10)
+        public async Task<IActionResult> ObterKmPorVeiculo(DateTime? dataInicio , DateTime? dataFim , int top = 10)
         {
             try
             {
@@ -896,11 +859,10 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
-                var kmDict = new Dictionary<string, decimal>();
+                var kmDict = new Dictionary<string , decimal>();
 
                 foreach (var est in estatisticas)
                 {
@@ -909,7 +871,7 @@
                         try
                         {
                             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
-                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.KmPorVeiculoJson, options);
+                            var lista = JsonSerializer.Deserialize<List<Dictionary<string , JsonElement>>>(est.KmPorVeiculoJson , options);
 
                             if (lista != null)
                             {
@@ -943,24 +905,22 @@
                 }
 
                 var dados = kmDict
-                    .Select(kv => new { veiculo = kv.Key, kmTotal = Math.Round(kv.Value, 0) })
+                    .Select(kv => new { veiculo = kv.Key , kmTotal = Math.Round(kv.Value , 0) })
                     .OrderByDescending(x => x.kmTotal)
                     .Take(top)
                     .ToList();
 
-                return Json(new { success = true, data = dados });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterKmPorVeiculo");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterKmPorVeiculo", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("api/DashboardViagens/ObterViagensPorRequisitante")]
-        public async Task<IActionResult> ObterViagensPorRequisitante(DateTime? dataInicio, DateTime? dataFim, int top = 6)
+        public async Task<IActionResult> ObterViagensPorRequisitante(DateTime? dataInicio , DateTime? dataFim , int top = 6)
         {
             try
             {
@@ -971,11 +931,10 @@
                 }
 
                 var estatisticas = await _context.ViagemEstatistica
-                    .AsNoTracking()
                     .Where(v => v.DataReferencia >= dataInicio.Value.Date && v.DataReferencia <= dataFim.Value.Date)
                     .ToListAsync();
 
-                var requisitanteDict = new Dictionary<string, int>();
+                var requisitanteDict = new Dictionary<string , int>();
 
                 foreach (var est in estatisticas)
                 {
@@ -984,7 +943,7 @@
                         try
                         {
                             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
-                            var lista = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(est.ViagensPorRequisitanteJson, options);
+                            var lista = JsonSerializer.Deserialize<List<Dictionary<string , JsonElement>>>(est.ViagensPorRequisitanteJson , options);
 
                             if (lista != null)
                             {
@@ -1032,10 +991,10 @@
                     if (string.IsNullOrEmpty(nomeCompleto))
                         return nomeCompleto;
 
-                    var partes = nomeCompleto.Split(new[] { '(', '-' }, StringSplitOptions.RemoveEmptyEntries);
+                    var partes = nomeCompleto.Split(new[] { '(' , '-' } , StringSplitOptions.RemoveEmptyEntries);
                     var nome = partes[0].Trim();
 
-                    var nomes = nome.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
+                    var nomes = nome.Split(new[] { ' ' } , StringSplitOptions.RemoveEmptyEntries);
                     if (nomes.Length <= 2)
                         return nome;
 
@@ -1043,18 +1002,16 @@
                 }
 
                 var dados = requisitanteDict
-                    .Select(kv => new { requisitante = PegarDoisPrimeirosNomes(kv.Key), totalViagens = kv.Value })
+                    .Select(kv => new { requisitante = PegarDoisPrimeirosNomes(kv.Key) , totalViagens = kv.Value })
                     .OrderByDescending(x => x.totalViagens)
                     .Take(top)
                     .ToList();
 
-                return Json(new { success = true, data = dados, viagensCtran = viagensCtran });
-            }
-            catch (Exception ex)
-            {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterViagensPorRequisitante");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterViagensPorRequisitante", ex);
-                return Json(new { success = false, message = ex.Message });
+                return Json(new { success = true , data = dados , viagensCtran = viagensCtran });
+            }
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
@@ -1071,7 +1028,6 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                     .Where(v => v.HoraInicio.HasValue)
                     .Where(v => v.Status == "Realizada" || v.Status == "Agendada" || v.Status == "Aberta")
@@ -1087,6 +1043,7 @@
 
                 foreach (var v in viagens)
                 {
+
                     int diaIndex = v.DiaSemana == 0 ? 6 : v.DiaSemana - 1;
                     int horaIndex = Math.Clamp(v.Hora, 0, 23);
 
@@ -1125,8 +1082,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterHeatmapViagens");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterHeatmapViagens", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -1144,7 +1099,6 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                     .Where(v => v.Status == "Realizada")
                     .Where(v => v.KmInicial.HasValue && v.KmFinal.HasValue)
@@ -1183,8 +1137,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterTop10VeiculosPorKm");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterTop10VeiculosPorKm", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
@@ -1202,7 +1154,6 @@
                 }
 
                 var viagens = await _context.Viagem
-                    .AsNoTracking()
                     .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
                     .Where(v => v.Status == "Realizada")
                     .Where(v => !string.IsNullOrEmpty(v.Finalidade))
@@ -1235,8 +1186,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "DashboardViagensController.cs", "ObterCustoMedioPorFinalidade");
-                Alerta.TratamentoErroComLinha("DashboardViagensController.cs", "ObterCustoMedioPorFinalidade", ex);
                 return Json(new { success = false, message = ex.Message });
             }
         }
```
