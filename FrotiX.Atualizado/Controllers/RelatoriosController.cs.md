# Controllers/RelatoriosController.cs

**Mudanca:** GRANDE | **+276** linhas | **-381** linhas

---

```diff
--- JANEIRO: Controllers/RelatoriosController.cs
+++ ATUAL: Controllers/RelatoriosController.cs
@@ -8,31 +8,21 @@
 using FrotiX.Services.Pdf;
 using Microsoft.AspNetCore.Mvc;
 
-namespace FrotiX.Controllers
-{
-
-    [ApiController]
-    [Route("api/[controller]")]
-    public class RelatoriosController : Controller
+namespace FrotiX.Controllers;
+
+[ApiController]
+[Route("api/[controller]")]
+public class RelatoriosController : Controller
 {
     private readonly FrotiXDbContext _context;
     private readonly IUnitOfWork _unitOfWork;
     private readonly RelatorioEconomildoPdfService _pdfService;
-    private readonly ILogService _log;
-
-    public RelatoriosController(FrotiXDbContext context, IUnitOfWork unitOfWork, ILogService log)
-    {
-        try
-        {
-            _context = context;
-            _unitOfWork = unitOfWork;
-            _log = log;
-            _pdfService = new RelatorioEconomildoPdfService();
-        }
-        catch (Exception ex)
-        {
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "RelatoriosController", ex);
-        }
+
+    public RelatoriosController(FrotiXDbContext context, IUnitOfWork unitOfWork)
+    {
+        _context = context;
+        _unitOfWork = unitOfWork;
+        _pdfService = new RelatorioEconomildoPdfService();
     }
 
     [HttpGet]
@@ -45,10 +35,7 @@
     {
         try
         {
-
             var filtro = new FiltroEconomildoDto { Mob = mob, Mes = mes, Ano = ano };
-
-            _log.Info($"RelatoriosController.ExportarEconomildo: Gerando relatório {tipo} (MOB: {mob ?? "Todos"}, Mês: {mes ?? 0}, Ano: {ano ?? 0})");
 
             byte[] pdfBytes = tipo switch
             {
@@ -68,446 +55,334 @@
         }
         catch (Exception ex)
         {
-
-            _log.Error("RelatoriosController.ExportarEconomildo", ex);
             Alerta.TratamentoErroComLinha("RelatoriosController.cs", "ExportarEconomildo", ex);
-
             return BadRequest($"Erro ao gerar PDF: {ex.Message}");
         }
     }
 
     private List<ViagensEconomildo> BuscarViagensEconomildo(FiltroEconomildoDto filtro, bool ignorarMob = false)
     {
+        var query = _context.ViagensEconomildo.AsQueryable();
+
+        if (!ignorarMob && !string.IsNullOrEmpty(filtro.Mob))
+        {
+            query = query.Where(v => v.MOB == filtro.Mob);
+        }
+
+        if (filtro.Mes.HasValue && filtro.Mes.Value > 0)
+        {
+            query = query.Where(v => v.Data.HasValue && v.Data.Value.Month == filtro.Mes.Value);
+        }
+
+        if (filtro.Ano.HasValue && filtro.Ano.Value > 0)
+        {
+            query = query.Where(v => v.Data.HasValue && v.Data.Value.Year == filtro.Ano.Value);
+        }
+
+        return query.ToList();
+    }
+
+    private byte[] GerarHeatmapViagens(FiltroEconomildoDto filtro)
+    {
+        var dados = MontarDadosHeatmap(filtro, usarPassageiros: false);
+        dados.Titulo = "Mapa de Calor - Distribuição de Viagens";
+        dados.Subtitulo = $"{filtro.NomeVeiculo} | Análise por Dia da Semana e Horário";
+        dados.UnidadeLegenda = "viagens";
+        return _pdfService.GerarHeatmapViagens(dados);
+    }
+
+    private byte[] GerarHeatmapPassageiros(FiltroEconomildoDto filtro)
+    {
+        var dados = MontarDadosHeatmap(filtro, usarPassageiros: true);
+        dados.Titulo = "Mapa de Calor - Distribuição de Passageiros";
+        dados.Subtitulo = $"{filtro.NomeVeiculo} | Análise por Dia da Semana e Horário";
+        dados.UnidadeLegenda = "passageiros";
+        return _pdfService.GerarHeatmapPassageiros(dados);
+    }
+
+    private HeatmapDto MontarDadosHeatmap(FiltroEconomildoDto filtro, bool usarPassageiros)
+    {
+        var viagens = BuscarViagensEconomildo(filtro);
+
+        var valores = new int[7, 24];
+        int valorMaximo = 0;
+        string diaPico = "";
+        int horaPico = 0;
+        var totaisPorDia = new int[7];
+
+        foreach (var viagem in viagens)
+        {
+            if (!viagem.Data.HasValue) continue;
+
+            var diaSemana = (int)viagem.Data.Value.DayOfWeek;
+            diaSemana = diaSemana == 0 ? 6 : diaSemana - 1;
+
+            var hora = ExtrairHora(viagem.HoraInicio);
+            if (hora < 0) continue;
+
+            var quantidade = usarPassageiros ? (viagem.QtdPassageiros ?? 1) : 1;
+
+            valores[diaSemana, hora] += quantidade;
+            totaisPorDia[diaSemana] += quantidade;
+
+            if (valores[diaSemana, hora] > valorMaximo)
+            {
+                valorMaximo = valores[diaSemana, hora];
+                diaPico = ObterNomeDiaAbreviado(diaSemana);
+                horaPico = hora;
+            }
+        }
+
+        var diasNomes = new[] { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };
+        var indiceDiaMaisMovimentado = Array.IndexOf(totaisPorDia, totaisPorDia.Max());
+
+        int maxManha = 0, horaInicioManha = 10;
+        for (int h = 6; h <= 12; h++)
+        {
+            int totalHora = 0;
+            for (int d = 0; d < 7; d++) totalHora += valores[d, h];
+            if (totalHora > maxManha) { maxManha = totalHora; horaInicioManha = h; }
+        }
+
+        int primeiraHora = 23, ultimaHora = 0;
+        for (int h = 0; h < 24; h++)
+            for (int d = 0; d < 7; d++)
+                if (valores[d, h] > 0)
+                {
+                    if (h < primeiraHora) primeiraHora = h;
+                    if (h > ultimaHora) ultimaHora = h;
+                }
+
+        return new HeatmapDto
+        {
+            Valores = valores,
+            ValorMaximo = valorMaximo,
+            DiaPico = diaPico,
+            HoraPico = horaPico,
+            HorarioPicoManha = $"{horaInicioManha}h - {Math.Min(horaInicioManha + 2, 12)}h",
+            DiaMaisMovimentado = diasNomes[indiceDiaMaisMovimentado],
+            PeriodoOperacao = primeiraHora <= ultimaHora ? $"{primeiraHora:00}h - {ultimaHora:00}h" : "—",
+            Filtro = filtro
+        };
+    }
+
+    private byte[] GerarUsuariosMes(FiltroEconomildoDto filtro)
+    {
+        var viagens = BuscarViagensEconomildo(filtro);
+
+        var usuariosPorMes = viagens
+            .Where(v => v.Data.HasValue)
+            .GroupBy(v => v.Data!.Value.Month)
+            .Select(g => new ItemGraficoDto
+            {
+                Label = ObterNomeMes(g.Key),
+                Valor = g.Sum(v => v.QtdPassageiros ?? 0)
+            })
+            .OrderBy(x => ObterNumeroMes(x.Label))
+            .ToList();
+
+        var total = usuariosPorMes.Sum(d => d.Valor);
+        foreach (var item in usuariosPorMes)
+            item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
+
+        var dados = new GraficoBarrasDto
+        {
+            Titulo = "Usuários por Mês",
+            Subtitulo = filtro.NomeVeiculo,
+            EixoX = "Mês",
+            EixoY = "Usuários",
+            Dados = usuariosPorMes,
+            Filtro = filtro
+        };
+
+        return _pdfService.GerarUsuariosMes(dados);
+    }
+
+    private byte[] GerarUsuariosTurno(FiltroEconomildoDto filtro)
+    {
+        var viagens = BuscarViagensEconomildo(filtro);
+
+        var manha = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Manhã").Sum(v => v.QtdPassageiros ?? 0);
+        var tarde = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Tarde").Sum(v => v.QtdPassageiros ?? 0);
+        var noite = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Noite").Sum(v => v.QtdPassageiros ?? 0);
+
+        var total = manha + tarde + noite;
+
+        var dados = new GraficoPizzaDto
+        {
+            Titulo = "Usuários por Turno",
+            Subtitulo = filtro.NomeVeiculo,
+            Dados = new List<ItemGraficoDto>
+            {
+                new() { Label = "Manhã", Valor = manha, Percentual = total > 0 ? (double)manha / total * 100 : 0 },
+                new() { Label = "Tarde", Valor = tarde, Percentual = total > 0 ? (double)tarde / total * 100 : 0 },
+                new() { Label = "Noite", Valor = noite, Percentual = total > 0 ? (double)noite / total * 100 : 0 }
+            },
+            Filtro = filtro
+        };
+
+        return _pdfService.GerarUsuariosTurno(dados);
+    }
+
+    private byte[] GerarComparativoMob(FiltroEconomildoDto filtro)
+    {
+        var viagensTodos = BuscarViagensEconomildo(filtro, ignorarMob: true);
+
+        var comparativoMob = viagensTodos
+            .Where(v => v.Data.HasValue)
+            .GroupBy(v => v.Data!.Value.Month)
+            .Select(g => new
+            {
+                mesNum = g.Key,
+                mes = ObterNomeMes(g.Key),
+                rodoviaria = g.Where(v => v.MOB == "Rodoviaria").Sum(v => v.QtdPassageiros ?? 0),
+                pgr = g.Where(v => v.MOB == "PGR").Sum(v => v.QtdPassageiros ?? 0),
+                cefor = g.Where(v => v.MOB == "Cefor").Sum(v => v.QtdPassageiros ?? 0)
+            })
+            .OrderBy(x => x.mesNum)
+            .ToList();
+
+        var labels = comparativoMob.Select(x => x.mes).ToList();
+
+        var series = new List<SerieGraficoDto>
+        {
+            new() { Nome = "PGR", Cor = "#3b82f6", Valores = comparativoMob.Select(x => x.pgr).ToList() },
+            new() { Nome = "Rodoviária", Cor = "#f97316", Valores = comparativoMob.Select(x => x.rodoviaria).ToList() },
+            new() { Nome = "Cefor", Cor = "#8b5cf6", Valores = comparativoMob.Select(x => x.cefor).ToList() }
+        };
+
+        var dados = new GraficoComparativoDto
+        {
+            Titulo = "Comparativo Mensal por MOB",
+            Subtitulo = $"Ano: {filtro.Ano ?? DateTime.Now.Year}",
+            Labels = labels,
+            Series = series,
+            Filtro = filtro
+        };
+
+        return _pdfService.GerarComparativoMob(dados);
+    }
+
+    private byte[] GerarUsuariosDiaSemana(FiltroEconomildoDto filtro)
+    {
+        var viagens = BuscarViagensEconomildo(filtro);
+
+        var usuariosPorDiaSemana = viagens
+            .Where(v => v.Data.HasValue)
+            .GroupBy(v => v.Data!.Value.DayOfWeek)
+            .Where(g => g.Key != DayOfWeek.Saturday && g.Key != DayOfWeek.Sunday)
+            .Select(g => new ItemGraficoDto
+            {
+                Label = ObterNomeDiaSemana(g.Key),
+                Valor = g.Sum(v => v.QtdPassageiros ?? 0)
+            })
+            .OrderBy(x => OrdemDiaSemana(x.Label))
+            .ToList();
+
+        var total = usuariosPorDiaSemana.Sum(d => d.Valor);
+        foreach (var item in usuariosPorDiaSemana)
+            item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
+
+        var dados = new GraficoBarrasDto
+        {
+            Titulo = "Usuários por Dia da Semana",
+            Subtitulo = filtro.NomeVeiculo,
+            EixoX = "Dia",
+            EixoY = "Usuários",
+            Dados = usuariosPorDiaSemana,
+            Filtro = filtro
+        };
+
+        return _pdfService.GerarUsuariosDiaSemana(dados);
+    }
+
+    private byte[] GerarDistribuicaoHorario(FiltroEconomildoDto filtro)
+    {
+        var viagens = BuscarViagensEconomildo(filtro);
+
+        var usuariosPorHora = viagens
+            .Where(v => !string.IsNullOrEmpty(v.HoraInicio))
+            .GroupBy(v => ExtrairHora(v.HoraInicio))
+            .Where(g => g.Key >= 0)
+            .Select(g => new ItemGraficoDto
+            {
+                Label = g.Key.ToString("00") + ":00",
+                Valor = g.Sum(v => v.QtdPassageiros ?? 0)
+            })
+            .OrderBy(x => int.Parse(x.Label.Substring(0, 2)))
+            .ToList();
+
+        var total = usuariosPorHora.Sum(d => d.Valor);
+        foreach (var item in usuariosPorHora)
+            item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
+
+        var dados = new GraficoBarrasDto
+        {
+            Titulo = "Distribuição por Horário",
+            Subtitulo = filtro.NomeVeiculo,
+            EixoX = "Horário",
+            EixoY = "Usuários",
+            Dados = usuariosPorHora,
+            Filtro = filtro
+        };
+
+        return _pdfService.GerarDistribuicaoHorario(dados);
+    }
+
+    private byte[] GerarTopVeiculos(FiltroEconomildoDto filtro)
+    {
+        var viagens = BuscarViagensEconomildo(filtro);
+
+        var topVeiculos = viagens
+            .Where(v => v.VeiculoId != Guid.Empty)
+            .GroupBy(v => v.VeiculoId)
+            .Select(g => new
+            {
+                veiculoId = g.Key,
+                total = g.Count()
+            })
+            .OrderByDescending(x => x.total)
+            .Take(10)
+            .ToList();
+
+        var veiculoIds = topVeiculos.Select(v => v.veiculoId).ToList();
+        var veiculos = _unitOfWork.ViewVeiculos
+            .GetAll(v => veiculoIds.Contains(v.VeiculoId))
+            .ToDictionary(v => v.VeiculoId, v => v.Placa ?? "S/N");
+
+        var dadosVeiculos = topVeiculos
+            .Select(v => new ItemGraficoDto
+            {
+                Label = veiculos.ContainsKey(v.veiculoId) ? veiculos[v.veiculoId] : "S/N",
+                Valor = v.total
+            })
+            .ToList();
+
+        var total = dadosVeiculos.Sum(d => d.Valor);
+        foreach (var item in dadosVeiculos)
+            item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
+
+        var dados = new GraficoBarrasDto
+        {
+            Titulo = "Top 10 Veículos",
+            Subtitulo = filtro.NomeVeiculo,
+            EixoX = "Veículo",
+            EixoY = "Viagens",
+            Dados = dadosVeiculos,
+            Filtro = filtro
+        };
+
+        return _pdfService.GerarTopVeiculos(dados);
+    }
+
+    private string ClassificarTurno(string? horaInicio)
+    {
         try
         {
-
-            var query = _context.ViagensEconomildo.AsQueryable();
-
-            if (!ignorarMob && !string.IsNullOrEmpty(filtro.Mob))
-            {
-
-                query = query.Where(v => v.MOB == filtro.Mob);
-            }
-
-            if (filtro.Mes.HasValue && filtro.Mes.Value > 0)
-            {
-
-                query = query.Where(v => v.Data.HasValue && v.Data.Value.Month == filtro.Mes.Value);
-            }
-
-            if (filtro.Ano.HasValue && filtro.Ano.Value > 0)
-            {
-
-                query = query.Where(v => v.Data.HasValue && v.Data.Value.Year == filtro.Ano.Value);
-            }
-
-            return query.ToList();
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.BuscarViagensEconomildo", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "BuscarViagensEconomildo", ex);
-            return new List<ViagensEconomildo>();
-        }
-    }
-
-    private byte[] GerarHeatmapViagens(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var dados = MontarDadosHeatmap(filtro, usarPassageiros: false);
-
-            dados.Titulo = "Mapa de Calor - Distribuição de Viagens";
-            dados.Subtitulo = $"{filtro.NomeVeiculo} | Análise por Dia da Semana e Horário";
-            dados.UnidadeLegenda = "viagens";
-
-            return _pdfService.GerarHeatmapViagens(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarHeatmapViagens", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarHeatmapViagens", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private byte[] GerarHeatmapPassageiros(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var dados = MontarDadosHeatmap(filtro, usarPassageiros: true);
-
-            dados.Titulo = "Mapa de Calor - Distribuição de Passageiros";
-            dados.Subtitulo = $"{filtro.NomeVeiculo} | Análise por Dia da Semana e Horário";
-            dados.UnidadeLegenda = "passageiros";
-
-            return _pdfService.GerarHeatmapPassageiros(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarHeatmapPassageiros", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarHeatmapPassageiros", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private HeatmapDto MontarDadosHeatmap(FiltroEconomildoDto filtro, bool usarPassageiros)
-    {
-        try
-        {
-
-            var viagens = BuscarViagensEconomildo(filtro);
-
-            var valores = new int[7, 24];
-            int valorMaximo = 0;
-            string diaPico = "";
-            int horaPico = 0;
-            var totaisPorDia = new int[7];
-
-            foreach (var viagem in viagens)
-            {
-                if (!viagem.Data.HasValue) continue;
-
-                var diaSemana = (int)viagem.Data.Value.DayOfWeek;
-                diaSemana = diaSemana == 0 ? 6 : diaSemana - 1;
-
-                var hora = ExtrairHora(viagem.HoraInicio);
-                if (hora < 0) continue;
-
-                var quantidade = usarPassageiros ? (viagem.QtdPassageiros ?? 1) : 1;
-
-                valores[diaSemana, hora] += quantidade;
-                totaisPorDia[diaSemana] += quantidade;
-
-                if (valores[diaSemana, hora] > valorMaximo)
-                {
-                    valorMaximo = valores[diaSemana, hora];
-                    diaPico = ObterNomeDiaAbreviado(diaSemana);
-                    horaPico = hora;
-                }
-            }
-
-            var diasNomes = new[] { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };
-            var indiceDiaMaisMovimentado = Array.IndexOf(totaisPorDia, totaisPorDia.Max());
-
-            int maxManha = 0, horaInicioManha = 10;
-            for (int h = 6; h <= 12; h++)
-            {
-                int totalHora = 0;
-                for (int d = 0; d < 7; d++) totalHora += valores[d, h];
-                if (totalHora > maxManha) { maxManha = totalHora; horaInicioManha = h; }
-            }
-
-            int primeiraHora = 23, ultimaHora = 0;
-            for (int h = 0; h < 24; h++)
-                for (int d = 0; d < 7; d++)
-                    if (valores[d, h] > 0)
-                    {
-                        if (h < primeiraHora) primeiraHora = h;
-                        if (h > ultimaHora) ultimaHora = h;
-                    }
-
-            return new HeatmapDto
-            {
-                Valores = valores,
-                ValorMaximo = valorMaximo,
-                DiaPico = diaPico,
-                HoraPico = horaPico,
-                HorarioPicoManha = $"{horaInicioManha}h - {Math.Min(horaInicioManha + 2, 12)}h",
-                DiaMaisMovimentado = diasNomes[indiceDiaMaisMovimentado],
-                PeriodoOperacao = primeiraHora <= ultimaHora ? $"{primeiraHora:00}h - {ultimaHora:00}h" : "—",
-                Filtro = filtro
-            };
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.MontarDadosHeatmap", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "MontarDadosHeatmap", ex);
-            return new HeatmapDto { Valores = new int[7, 24], Filtro = filtro };
-        }
-    }
-
-    private byte[] GerarUsuariosMes(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var viagens = BuscarViagensEconomildo(filtro);
-
-            var usuariosPorMes = viagens
-                .Where(v => v.Data.HasValue)
-                .GroupBy(v => v.Data!.Value.Month)
-                .Select(g => new ItemGraficoDto
-                {
-                    Label = ObterNomeMes(g.Key),
-                    Valor = g.Sum(v => v.QtdPassageiros ?? 0)
-                })
-                .OrderBy(x => ObterNumeroMes(x.Label))
-                .ToList();
-
-            var total = usuariosPorMes.Sum(d => d.Valor);
-            foreach (var item in usuariosPorMes)
-                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
-
-            var dados = new GraficoBarrasDto
-            {
-                Titulo = "Usuários por Mês",
-                Subtitulo = filtro.NomeVeiculo,
-                EixoX = "Mês",
-                EixoY = "Usuários",
-                Dados = usuariosPorMes,
-                Filtro = filtro
-            };
-
-            return _pdfService.GerarUsuariosMes(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarUsuariosMes", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarUsuariosMes", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private byte[] GerarUsuariosTurno(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var viagens = BuscarViagensEconomildo(filtro);
-
-            var manha = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Manhã").Sum(v => v.QtdPassageiros ?? 0);
-            var tarde = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Tarde").Sum(v => v.QtdPassageiros ?? 0);
-            var noite = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Noite").Sum(v => v.QtdPassageiros ?? 0);
-
-            var total = manha + tarde + noite;
-
-            var dados = new GraficoPizzaDto
-            {
-                Titulo = "Usuários por Turno",
-                Subtitulo = filtro.NomeVeiculo,
-                Dados = new List<ItemGraficoDto>
-                {
-                    new() { Label = "Manhã", Valor = manha, Percentual = total > 0 ? (double)manha / total * 100 : 0 },
-                    new() { Label = "Tarde", Valor = tarde, Percentual = total > 0 ? (double)tarde / total * 100 : 0 },
-                    new() { Label = "Noite", Valor = noite, Percentual = total > 0 ? (double)noite / total * 100 : 0 }
-                },
-                Filtro = filtro
-            };
-
-            return _pdfService.GerarUsuariosTurno(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarUsuariosTurno", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarUsuariosTurno", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private byte[] GerarComparativoMob(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var viagensTodos = BuscarViagensEconomildo(filtro, ignorarMob: true);
-
-            var comparativoMob = viagensTodos
-                .Where(v => v.Data.HasValue)
-                .GroupBy(v => v.Data!.Value.Month)
-                .Select(g => new
-                {
-                    mesNum = g.Key,
-                    mes = ObterNomeMes(g.Key),
-                    rodoviaria = g.Where(v => v.MOB == "Rodoviaria").Sum(v => v.QtdPassageiros ?? 0),
-                    pgr = g.Where(v => v.MOB == "PGR").Sum(v => v.QtdPassageiros ?? 0),
-                    cefor = g.Where(v => v.MOB == "Cefor").Sum(v => v.QtdPassageiros ?? 0)
-                })
-                .OrderBy(x => x.mesNum)
-                .ToList();
-
-            var labels = comparativoMob.Select(x => x.mes).ToList();
-
-            var series = new List<SerieGraficoDto>
-            {
-                new() { Nome = "PGR", Cor = "#3b82f6", Valores = comparativoMob.Select(x => x.pgr).ToList() },
-                new() { Nome = "Rodoviária", Cor = "#f97316", Valores = comparativoMob.Select(x => x.rodoviaria).ToList() },
-                new() { Nome = "Cefor", Cor = "#8b5cf6", Valores = comparativoMob.Select(x => x.cefor).ToList() }
-            };
-
-            var dados = new GraficoComparativoDto
-            {
-                Titulo = "Comparativo Mensal por MOB",
-                Subtitulo = $"Ano: {filtro.Ano ?? DateTime.Now.Year}",
-                Labels = labels,
-                Series = series,
-                Filtro = filtro
-            };
-
-            return _pdfService.GerarComparativoMob(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarComparativoMob", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarComparativoMob", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private byte[] GerarUsuariosDiaSemana(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var viagens = BuscarViagensEconomildo(filtro);
-
-            var usuariosPorDiaSemana = viagens
-                .Where(v => v.Data.HasValue)
-                .GroupBy(v => v.Data!.Value.DayOfWeek)
-                .Where(g => g.Key != DayOfWeek.Saturday && g.Key != DayOfWeek.Sunday)
-                .Select(g => new ItemGraficoDto
-                {
-                    Label = ObterNomeDiaSemana(g.Key),
-                    Valor = g.Sum(v => v.QtdPassageiros ?? 0)
-                })
-                .OrderBy(x => OrdemDiaSemana(x.Label))
-                .ToList();
-
-            var total = usuariosPorDiaSemana.Sum(d => d.Valor);
-            foreach (var item in usuariosPorDiaSemana)
-                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
-
-            var dados = new GraficoBarrasDto
-            {
-                Titulo = "Usuários por Dia da Semana",
-                Subtitulo = filtro.NomeVeiculo,
-                EixoX = "Dia",
-                EixoY = "Usuários",
-                Dados = usuariosPorDiaSemana,
-                Filtro = filtro
-            };
-
-            return _pdfService.GerarUsuariosDiaSemana(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarUsuariosDiaSemana", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarUsuariosDiaSemana", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private byte[] GerarDistribuicaoHorario(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var viagens = BuscarViagensEconomildo(filtro);
-
-            var usuariosPorHora = viagens
-                .Where(v => !string.IsNullOrEmpty(v.HoraInicio))
-                .GroupBy(v => ExtrairHora(v.HoraInicio))
-                .Where(g => g.Key >= 0)
-                .Select(g => new ItemGraficoDto
-                {
-                    Label = g.Key.ToString("00") + ":00",
-                    Valor = g.Sum(v => v.QtdPassageiros ?? 0)
-                })
-                .OrderBy(x => int.Parse(x.Label.Substring(0, 2)))
-                .ToList();
-
-            var total = usuariosPorHora.Sum(d => d.Valor);
-            foreach (var item in usuariosPorHora)
-                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
-
-            var dados = new GraficoBarrasDto
-            {
-                Titulo = "Distribuição por Horário",
-                Subtitulo = filtro.NomeVeiculo,
-                EixoX = "Horário",
-                EixoY = "Usuários",
-                Dados = usuariosPorHora,
-                Filtro = filtro
-            };
-
-            return _pdfService.GerarDistribuicaoHorario(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarDistribuicaoHorario", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarDistribuicaoHorario", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private byte[] GerarTopVeiculos(FiltroEconomildoDto filtro)
-    {
-        try
-        {
-
-            var viagens = BuscarViagensEconomildo(filtro);
-
-            var topVeiculos = viagens
-                .Where(v => v.VeiculoId != Guid.Empty)
-                .GroupBy(v => v.VeiculoId)
-                .Select(g => new
-                {
-                    veiculoId = g.Key,
-                    total = g.Count()
-                })
-                .OrderByDescending(x => x.total)
-                .Take(10)
-                .ToList();
-
-            var veiculoIds = topVeiculos.Select(v => v.veiculoId).ToList();
-            var veiculos = _unitOfWork.ViewVeiculos
-                .GetAll(v => veiculoIds.Contains(v.VeiculoId))
-                .ToDictionary(v => v.VeiculoId, v => v.Placa ?? "S/N");
-
-            var dadosVeiculos = topVeiculos
-                .Select(v => new ItemGraficoDto
-                {
-                    Label = veiculos.ContainsKey(v.veiculoId) ? veiculos[v.veiculoId] : "S/N",
-                    Valor = v.total
-                })
-                .ToList();
-
-            var total = dadosVeiculos.Sum(d => d.Valor);
-            foreach (var item in dadosVeiculos)
-                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;
-
-            var dados = new GraficoBarrasDto
-            {
-                Titulo = "Top 10 Veículos",
-                Subtitulo = filtro.NomeVeiculo,
-                EixoX = "Veículo",
-                EixoY = "Viagens",
-                Dados = dadosVeiculos,
-                Filtro = filtro
-            };
-
-            return _pdfService.GerarTopVeiculos(dados);
-        }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.GerarTopVeiculos", ex);
-            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarTopVeiculos", ex);
-            return Array.Empty<byte>();
-        }
-    }
-
-    private string ClassificarTurno(string? horaInicio)
-    {
-        try
-        {
-
             if (string.IsNullOrEmpty(horaInicio)) return "Manhã";
 
             if (TimeSpan.TryParse(horaInicio, out var hora))
             {
-
                 if (hora.Hours >= 6 && hora.Hours < 12) return "Manhã";
                 if (hora.Hours >= 12 && hora.Hours < 18) return "Tarde";
                 return "Noite";
@@ -515,9 +390,8 @@
 
             return "Manhã";
         }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.ClassificarTurno", ex);
+        catch
+        {
             return "Manhã";
         }
     }
@@ -526,20 +400,17 @@
     {
         try
         {
-
             if (string.IsNullOrEmpty(horaStr)) return -1;
 
             if (TimeSpan.TryParse(horaStr, out var hora))
             {
-
                 return hora.Hours;
             }
 
             return -1;
         }
-        catch (Exception ex)
-        {
-            _log.Error("RelatoriosController.ExtrairHora", ex);
+        catch
+        {
             return -1;
         }
     }
@@ -579,4 +450,3 @@
     };
 
 }
-}
```
