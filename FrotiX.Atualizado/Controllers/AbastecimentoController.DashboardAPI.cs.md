# Controllers/AbastecimentoController.DashboardAPI.cs

**Mudanca:** GRANDE | **+76** linhas | **-92** linhas

---

```diff
--- JANEIRO: Controllers/AbastecimentoController.DashboardAPI.cs
+++ ATUAL: Controllers/AbastecimentoController.DashboardAPI.cs
@@ -6,8 +6,6 @@
 using System;
 using System.Collections.Generic;
 using System.Linq;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -173,6 +171,7 @@
                     litrosPorMes,
                     consumoPorMes,
                     totais,
+
                     filtroAplicado = new
                     {
                         ano = ano ?? 0,
@@ -184,8 +183,7 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "AbastecimentoController.DashboardAPI.cs", "DashboardDados");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDados", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardDados", error);
                 return StatusCode(500, new { message = "Erro ao carregar dados do dashboard" });
             }
         }
@@ -233,6 +231,7 @@
 
                 var resultado = new
                 {
+
                     anosDisponiveis = _unitOfWork.ViewAbastecimentos.GetAll()
                         .Where(a => a.DataHora.HasValue)
                         .Select(a => a.DataHora.Value.Year)
@@ -312,14 +311,14 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "AbastecimentoController.DashboardAPI.cs", "DashboardDadosPeriodo");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDadosPeriodo", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardDadosPeriodo", error);
                 return StatusCode(500, new { message = "Erro ao carregar dados do dashboard por período" });
             }
         }
 
         private IActionResult DashboardDadosFallback(int? ano, int? mes)
         {
+
             if ((!ano.HasValue || ano == 0) && (!mes.HasValue || mes == 0))
             {
                 var ultimaData = _unitOfWork.ViewAbastecimentos.GetAll()
@@ -372,6 +371,7 @@
 
             var resultado = new
             {
+
                 anosDisponiveis = _unitOfWork.ViewAbastecimentos.GetAll()
                     .Where(a => a.DataHora.HasValue)
                     .Select(a => a.DataHora.Value.Year)
@@ -380,15 +380,15 @@
                     .ToList(),
 
                 resumoPorAno = (from a in _unitOfWork.ViewAbastecimentos.GetAll()
-                                where a.DataHora.HasValue
-                                group a by a.DataHora.Value.Year into g
-                                orderby g.Key descending
-                                select new
-                                {
-                                    ano = g.Key,
-                                    valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
-                                    litros = g.Sum(a => ParseDecimal(a.Litros))
-                                })
+                               where a.DataHora.HasValue
+                               group a by a.DataHora.Value.Year into g
+                               orderby g.Key descending
+                               select new
+                               {
+                                   ano = g.Key,
+                                   valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
+                                   litros = g.Sum(a => ParseDecimal(a.Litros))
+                               })
                                .Take(3)
                                .OrderBy(r => r.ano)
                                .ToList(),
@@ -464,12 +464,12 @@
         {
             try
             {
+
                 var temDadosEstatisticos = _context.EstatisticaAbastecimentoMensal
                     .Any(e => e.Ano == ano);
 
                 if (!temDadosEstatisticos)
                 {
-                    _log.Warning($"Dados estatísticos não encontrados para o ano {ano}. Iniciando Fallback.");
                     return DashboardMensalFallback(ano, mes);
                 }
 
@@ -483,6 +483,9 @@
                     .Where(e => e.Ano == ano);
 
                 var queryTipo = _context.EstatisticaAbastecimentoTipoVeiculo
+                    .Where(e => e.Ano == ano);
+
+                var queryVeiculo = _context.EstatisticaAbastecimentoVeiculo
                     .Where(e => e.Ano == ano);
 
                 if (mes.HasValue && mes > 0)
@@ -496,6 +499,7 @@
                 var estatMensal = queryMensal.ToList();
                 var estatComb = queryComb.ToList();
                 var estatCat = queryCat.ToList();
+                var estatTipo = queryTipo.ToList();
 
                 var valorTotal = estatMensal.Sum(e => e.ValorTotal ?? 0);
                 var litrosTotal = estatMensal.Sum(e => e.LitrosTotal ?? 0);
@@ -599,8 +603,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardMensal para ano={ano}, mes={mes}");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardMensal", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardMensal", error);
                 return StatusCode(500, new { message = "Erro ao carregar dados mensais" });
             }
         }
@@ -723,12 +726,12 @@
         {
             try
             {
+
                 var temDadosEstatisticos = _context.EstatisticaAbastecimentoVeiculo
                     .Any(e => e.Ano == ano);
 
                 if (!temDadosEstatisticos)
                 {
-                    _log.Warning($"Dados estatísticos de veículo não encontrados para o ano {ano}. Iniciando Fallback.");
                     return DashboardVeiculoFallback(ano, mes, veiculoId, tipoVeiculo);
                 }
 
@@ -737,6 +740,7 @@
 
                 if (mes.HasValue && mes > 0)
                 {
+
                     var veiculosComAbastecimentoNoMes = _context.EstatisticaAbastecimentoVeiculoMensal
                         .Where(e => e.Ano == ano && e.Mes == mes.Value)
                         .Select(e => e.VeiculoId)
@@ -759,6 +763,7 @@
                 }
                 else
                 {
+
                     modelosDisponiveis = _context.EstatisticaAbastecimentoTipoVeiculo
                         .Where(e => e.Ano == ano && !string.IsNullOrEmpty(e.TipoVeiculo))
                         .Select(e => e.TipoVeiculo)
@@ -800,6 +805,7 @@
 
                 if (veiculoId.HasValue && veiculoId.Value != Guid.Empty)
                 {
+
                     var veiculoMensalTodos = _context.EstatisticaAbastecimentoVeiculoMensal
                         .Where(e => e.Ano == ano && e.VeiculoId == veiculoId.Value)
                         .ToList();
@@ -851,6 +857,7 @@
                 }
                 else if (!string.IsNullOrEmpty(tipoVeiculo))
                 {
+
                     descricaoVeiculo = tipoVeiculo;
                     categoriaVeiculo = tipoVeiculo;
 
@@ -893,6 +900,7 @@
                 }
                 else
                 {
+
                     var todosMesesData = _context.EstatisticaAbastecimentoMensal
                         .Where(e => e.Ano == ano)
                         .ToList();
@@ -951,8 +959,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardVeiculo para ano={ano}, mes={mes}");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardVeiculo", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardVeiculo", error);
                 return StatusCode(500, new { message = "Erro ao carregar dados por veículo" });
             }
         }
@@ -1042,8 +1049,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardDadosVeiculoPeriodo para {dataInicio} até {dataFim}");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDadosVeiculoPeriodo", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardDadosVeiculoPeriodo", error);
                 return StatusCode(500, new { message = "Erro ao carregar dados por veículo por período" });
             }
         }
@@ -1211,8 +1217,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardDetalhes");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardDetalhes", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardDetalhes", error);
                 return StatusCode(500, new { message = "Erro ao carregar detalhes" });
             }
         }
@@ -1223,6 +1228,7 @@
         {
             try
             {
+
                 var queryHeatmap = _context.HeatmapAbastecimentoMensal
                     .Where(h => h.VeiculoId == null && h.TipoVeiculo == null);
 
@@ -1236,6 +1242,7 @@
 
                 if (heatmapData.Any())
                 {
+
                     var diasSemana = new[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };
 
                     var agrupado = heatmapData
@@ -1271,13 +1278,11 @@
                     });
                 }
 
-                _log.Warning($"Dados de Heatmap de Hora não encontrados para ano={ano}. Iniciando Fallback.");
                 return DashboardHeatmapHoraFallback(ano, mes);
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardHeatmapHora");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardHeatmapHora", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardHeatmapHora", error);
                 return StatusCode(500, new { message = "Erro ao carregar mapa de calor" });
             }
         }
@@ -1337,6 +1342,7 @@
         {
             try
             {
+
                 var estatCat = _context.EstatisticaAbastecimentoCategoria
                     .Where(e => e.Ano == ano)
                     .ToList();
@@ -1364,13 +1370,11 @@
                     });
                 }
 
-                _log.Warning($"Dados de Heatmap de Categoria não encontrados para ano={ano}. Iniciando Fallback.");
                 return DashboardHeatmapCategoriaFallback(ano);
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardHeatmapCategoria");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardHeatmapCategoria", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardHeatmapCategoria", error);
                 return StatusCode(500, new { message = "Erro ao carregar mapa de calor" });
             }
         }
@@ -1423,6 +1427,7 @@
         {
             try
             {
+
                 if (string.IsNullOrEmpty(placa) && string.IsNullOrEmpty(tipoVeiculo))
                 {
                     return Ok(new { xLabels = new string[0], yLabels = new string[0], data = new object[0] });
@@ -1441,13 +1446,19 @@
 
                 var dados = query.ToList();
 
+                if (!dados.Any())
+                {
+                    return Ok(new { xLabels = new string[0], yLabels = new string[0], data = new object[0] });
+                }
+
                 var agrupado = dados
                     .GroupBy(a => new { DiaSemana = (int)a.DataHora.Value.DayOfWeek, Hora = a.DataHora.Value.Hour })
                     .Select(g => new
                     {
                         diaSemana = g.Key.DiaSemana,
                         hora = g.Key.Hora,
-                        valor = g.Sum(a => ParseDecimal(a.ValorTotal))
+                        valor = g.Sum(a => ParseDecimal(a.ValorTotal)),
+                        quantidade = g.Count()
                     })
                     .ToList();
 
@@ -1477,9 +1488,8 @@
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardHeatmapVeiculo");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardHeatmapVeiculo", error);
-                return StatusCode(500, new { message = "Erro ao carregar mapa de calor de veículo" });
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardHeatmapVeiculo", error);
+                return StatusCode(500, new { message = "Erro ao carregar mapa de calor do veículo" });
             }
         }
 
@@ -1489,6 +1499,7 @@
         {
             try
             {
+
                 var mesesEstatisticos = _context.EstatisticaAbastecimentoMensal
                     .Where(e => e.Ano == ano && e.ValorTotal > 0)
                     .Select(e => e.Mes)
@@ -1512,8 +1523,7 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "AbastecimentoController.DashboardAPI.cs", "DashboardMesesDisponiveis");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardMesesDisponiveis", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardMesesDisponiveis", error);
                 return StatusCode(500, new { message = "Erro ao carregar meses disponíveis" });
             }
         }
@@ -1622,7 +1632,7 @@
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardViagensVeiculo", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardViagensVeiculo", error);
                 return StatusCode(500, new { message = "Erro ao carregar viagens do veículo" });
             }
         }
@@ -1722,7 +1732,7 @@
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardAbastecimentosUnidade", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardAbastecimentosUnidade", error);
                 return StatusCode(500, new { message = "Erro ao carregar abastecimentos da unidade" });
             }
         }
@@ -1738,9 +1748,10 @@
                     return BadRequest(new { message = "Categoria é obrigatória" });
                 }
 
-                var veiculosDaCategoria = _unitOfWork.ViewVeiculos.GetAll(v => v.Categoria == categoria)
-                                                    .Select(v => v.VeiculoId)
-                                                    .ToList();
+                var veiculosDaCategoria = _unitOfWork.ViewVeiculos.GetAll()
+                    .Where(v => v.Categoria == categoria)
+                    .Select(v => v.VeiculoId)
+                    .ToList();
 
                 if (!veiculosDaCategoria.Any())
                 {
@@ -1819,80 +1830,70 @@
             }
             catch (Exception error)
             {
-                _log.Error($", errorErro ao carregar DashboardAbastecimentosCategoria para categoria={categoria}");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "DashboardAbastecimentosCategoria", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DashboardAbastecimentosCategoria", error);
                 return StatusCode(500, new { message = "Erro ao carregar abastecimentos da categoria" });
             }
         }
 
         private static decimal ParseDecimal(string? valor)
         {
-            try
-            {
-                if (string.IsNullOrEmpty(valor))
-                    return 0;
-
-                var valorLimpo = valor
-                    .Replace("R$", "")
-                    .Replace(" ", "")
-                    .Trim();
-
-                if (string.IsNullOrEmpty(valorLimpo))
-                    return 0;
-
-                bool temVirgula = valorLimpo.Contains(',');
-                bool temPonto = valorLimpo.Contains('.');
-
-                if (temVirgula && temPonto)
-                {
-
-                    int posVirgula = valorLimpo.LastIndexOf(',');
-                    int posPonto = valorLimpo.LastIndexOf('.');
-
-                    if (posVirgula > posPonto)
-                    {
-
-                        valorLimpo = valorLimpo.Replace(".", "").Replace(",", ".");
-                    }
-                    else
-                    {
-
-                        valorLimpo = valorLimpo.Replace(",", "");
-                    }
-                }
-                else if (temVirgula)
-                {
-
-                    valorLimpo = valorLimpo.Replace(",", ".");
-                }
-                else if (temPonto)
-                {
-
-                    var partes = valorLimpo.Split('.');
-                    bool ehSeparadorMilhar = partes.Length > 1 && partes.Skip(1).All(p => p.Length == 3);
-
-                    if (ehSeparadorMilhar)
-                    {
-
-                        valorLimpo = valorLimpo.Replace(".", "");
-                    }
-
-                }
-
-                if (decimal.TryParse(valorLimpo, System.Globalization.NumberStyles.Any,
-                    System.Globalization.CultureInfo.InvariantCulture, out decimal result))
-                {
-                    return result;
-                }
-
+            if (string.IsNullOrEmpty(valor))
                 return 0;
-            }
-            catch (Exception error)
-            {
-
-                Alerta.TratamentoErroComLinha("AbastecimentoController.DashboardAPI.cs", "ParseDecimal", error);
+
+            var valorLimpo = valor
+                .Replace("R$", "")
+                .Replace(" ", "")
+                .Trim();
+
+            if (string.IsNullOrEmpty(valorLimpo))
                 return 0;
-            }
+
+            bool temVirgula = valorLimpo.Contains(',');
+            bool temPonto = valorLimpo.Contains('.');
+
+            if (temVirgula && temPonto)
+            {
+
+                int posVirgula = valorLimpo.LastIndexOf(',');
+                int posPonto = valorLimpo.LastIndexOf('.');
+
+                if (posVirgula > posPonto)
+                {
+
+                    valorLimpo = valorLimpo.Replace(".", "").Replace(",", ".");
+                }
+                else
+                {
+
+                    valorLimpo = valorLimpo.Replace(",", "");
+                }
+            }
+            else if (temVirgula)
+            {
+
+                valorLimpo = valorLimpo.Replace(",", ".");
+            }
+            else if (temPonto)
+            {
+
+                var partes = valorLimpo.Split('.');
+                bool ehSeparadorMilhar = partes.Length > 1 && partes.Skip(1).All(p => p.Length == 3);
+
+                if (ehSeparadorMilhar)
+                {
+
+                    valorLimpo = valorLimpo.Replace(".", "");
+                }
+
+            }
+
+            if (decimal.TryParse(valorLimpo, System.Globalization.NumberStyles.Any,
+                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
+            {
+                return result;
+            }
+
+            return 0;
         }
 
     }
```
