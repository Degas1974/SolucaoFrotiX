# Services/VeiculoEstatisticaService.cs

**Mudanca:** GRANDE | **+74** linhas | **-87** linhas

---

```diff
--- JANEIRO: Services/VeiculoEstatisticaService.cs
+++ ATUAL: Services/VeiculoEstatisticaService.cs
@@ -6,7 +6,6 @@
 using FrotiX.Models.DTO;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.Caching.Memory;
-using FrotiX.Helpers;
 
 namespace FrotiX.Services
 {
@@ -30,7 +29,6 @@
         {
             try
             {
-
                 var cacheKey = $"VeiculoEstatistica_{veiculoId}";
 
                 if (_cache.TryGetValue(cacheKey, out EstatisticaVeiculoDto cached))
@@ -42,11 +40,7 @@
 
                 if (estatisticas != null)
                 {
-                    _cache.Set(cacheKey, estatisticas, new MemoryCacheEntryOptions
-                    {
-                        AbsoluteExpirationRelativeToNow = CACHE_DURATION,
-                        Size = 1
-                    });
+                    _cache.Set(cacheKey, estatisticas, CACHE_DURATION);
                 }
 
                 return estatisticas;
@@ -60,105 +54,97 @@
 
         private async Task<EstatisticaVeiculoDto> CalcularEstatisticasAsync(Guid veiculoId)
         {
-            try
-            {
-
-                var veiculoBase = await _context.Veiculo
-                    .AsNoTracking()
-                    .Where(v => v.VeiculoId == veiculoId)
-                    .FirstOrDefaultAsync();
-
-                if (veiculoBase == null)
-                {
-                    return CriarEstatisticasVazias(veiculoId);
-                }
-
-                string descricao = "";
-                if (veiculoBase.MarcaId.HasValue)
-                {
-                    var marca = await _context.MarcaVeiculo.FindAsync(veiculoBase.MarcaId.Value);
-                    if (marca != null) descricao = marca.DescricaoMarca ?? "";
-                }
-                if (veiculoBase.ModeloId.HasValue)
-                {
-                    var modelo = await _context.ModeloVeiculo.FindAsync(veiculoBase.ModeloId.Value);
-                    if (modelo != null) descricao += " " + (modelo.DescricaoModelo ?? "");
-                }
-                descricao = descricao.Trim();
-
-                var veiculo = new { veiculoBase.VeiculoId, veiculoBase.Placa, Descricao = descricao };
-
-                var viagens = await _context.Viagem
-                    .AsNoTracking()
-                    .Where(v =>
-                        v.VeiculoId == veiculoId &&
-                        v.Status == "Realizada" &&
-                        v.KmInicial.HasValue && v.KmInicial > 0 &&
-                        v.KmFinal.HasValue && v.KmFinal > 0 &&
-                        v.KmFinal > v.KmInicial &&
-                        v.DataInicial.HasValue &&
-                        v.DataFinal.HasValue)
-                    .OrderByDescending(v => v.DataFinal)
-                    .Take(QUANTIDADE_VIAGENS_HISTORICO)
-                    .Select(v => new ViagemDados
-                    {
-                        KmRodado = v.KmFinal.Value - v.KmInicial.Value,
-                        DuracaoMinutos = CalcularDuracaoMinutos(v.DataInicial, v.HoraInicio, v.DataFinal, v.HoraFim),
-                        DataFinal = v.DataFinal.Value
-                    })
-                    .ToListAsync();
-
-                viagens = viagens
-                    .Where(v => v.DuracaoMinutos >= 1 && v.DuracaoMinutos <= 1440)
-                    .ToList();
-
-                if (!viagens.Any())
-                {
-                    return new EstatisticaVeiculoDto
-                    {
-                        VeiculoId = veiculoId,
-                        Placa = veiculo.Placa,
-                        Descricao = veiculo.Descricao,
-                        TotalViagens = 0
-                    };
-                }
-
-                var kms = viagens.Select(v => (double)v.KmRodado).OrderBy(k => k).ToList();
-                var duracoes = viagens.Select(v => (double)v.DuracaoMinutos).OrderBy(d => d).ToList();
-
-                var estatisticas = new EstatisticaVeiculoDto
+
+            var veiculoBase = await _context.Veiculo
+                .AsNoTracking()
+                .Where(v => v.VeiculoId == veiculoId)
+                .FirstOrDefaultAsync();
+
+            if (veiculoBase == null)
+            {
+                return CriarEstatisticasVazias(veiculoId);
+            }
+
+            string descricao = "";
+            if (veiculoBase.MarcaId.HasValue)
+            {
+                var marca = await _context.MarcaVeiculo.FindAsync(veiculoBase.MarcaId.Value);
+                if (marca != null) descricao = marca.DescricaoMarca ?? "";
+            }
+            if (veiculoBase.ModeloId.HasValue)
+            {
+                var modelo = await _context.ModeloVeiculo.FindAsync(veiculoBase.ModeloId.Value);
+                if (modelo != null) descricao += " " + (modelo.DescricaoModelo ?? "");
+            }
+            descricao = descricao.Trim();
+
+            var veiculo = new { veiculoBase.VeiculoId, veiculoBase.Placa, Descricao = descricao };
+
+            var viagens = await _context.Viagem
+                .AsNoTracking()
+                .Where(v =>
+                    v.VeiculoId == veiculoId &&
+                    v.Status == "Realizada" &&
+                    v.KmInicial.HasValue && v.KmInicial > 0 &&
+                    v.KmFinal.HasValue && v.KmFinal > 0 &&
+                    v.KmFinal > v.KmInicial &&
+                    v.DataInicial.HasValue &&
+                    v.DataFinal.HasValue)
+                .OrderByDescending(v => v.DataFinal)
+                .Take(QUANTIDADE_VIAGENS_HISTORICO)
+                .Select(v => new ViagemDados
+                {
+                    KmRodado = v.KmFinal.Value - v.KmInicial.Value,
+                    DuracaoMinutos = CalcularDuracaoMinutos(v.DataInicial, v.HoraInicio, v.DataFinal, v.HoraFim),
+                    DataFinal = v.DataFinal.Value
+                })
+                .ToListAsync();
+
+            viagens = viagens
+                .Where(v => v.DuracaoMinutos >= 1 && v.DuracaoMinutos <= 1440)
+                .ToList();
+
+            if (!viagens.Any())
+            {
+                return new EstatisticaVeiculoDto
                 {
                     VeiculoId = veiculoId,
                     Placa = veiculo.Placa,
                     Descricao = veiculo.Descricao,
-                    TotalViagens = viagens.Count,
-
-                    KmMedio = kms.Average(),
-                    KmMediano = CalcularMediana(kms),
-                    KmDesvioPadrao = CalcularDesvioPadrao(kms),
-                    KmMinimo = (int)kms.Min(),
-                    KmMaximo = (int)kms.Max(),
-                    KmPercentil95 = CalcularPercentil(kms, 0.95),
-                    KmPercentil99 = CalcularPercentil(kms, 0.99),
-
-                    DuracaoMediaMinutos = duracoes.Average(),
-                    DuracaoMedianaMinutos = CalcularMediana(duracoes),
-                    DuracaoDesvioPadraoMinutos = CalcularDesvioPadrao(duracoes),
-                    DuracaoMinimaMinutos = (int)duracoes.Min(),
-                    DuracaoMaximaMinutos = (int)duracoes.Max(),
-                    DuracaoPercentil95Minutos = CalcularPercentil(duracoes, 0.95),
-
-                    DataViagemMaisAntiga = viagens.Min(v => v.DataFinal),
-                    DataViagemMaisRecente = viagens.Max(v => v.DataFinal)
+                    TotalViagens = 0
                 };
-
-                return estatisticas;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("VeiculoEstatisticaService.cs", "CalcularEstatisticasAsync", ex);
-                throw;
-            }
+            }
+
+            var kms = viagens.Select(v => (double)v.KmRodado).OrderBy(k => k).ToList();
+            var duracoes = viagens.Select(v => (double)v.DuracaoMinutos).OrderBy(d => d).ToList();
+
+            var estatisticas = new EstatisticaVeiculoDto
+            {
+                VeiculoId = veiculoId,
+                Placa = veiculo.Placa,
+                Descricao = veiculo.Descricao,
+                TotalViagens = viagens.Count,
+
+                KmMedio = kms.Average(),
+                KmMediano = CalcularMediana(kms),
+                KmDesvioPadrao = CalcularDesvioPadrao(kms),
+                KmMinimo = (int)kms.Min(),
+                KmMaximo = (int)kms.Max(),
+                KmPercentil95 = CalcularPercentil(kms, 0.95),
+                KmPercentil99 = CalcularPercentil(kms, 0.99),
+
+                DuracaoMediaMinutos = duracoes.Average(),
+                DuracaoMedianaMinutos = CalcularMediana(duracoes),
+                DuracaoDesvioPadraoMinutos = CalcularDesvioPadrao(duracoes),
+                DuracaoMinimaMinutos = (int)duracoes.Min(),
+                DuracaoMaximaMinutos = (int)duracoes.Max(),
+                DuracaoPercentil95Minutos = CalcularPercentil(duracoes, 0.95),
+
+                DataViagemMaisAntiga = viagens.Min(v => v.DataFinal),
+                DataViagemMaisRecente = viagens.Max(v => v.DataFinal)
+            };
+
+            return estatisticas;
         }
 
         private static int CalcularDuracaoMinutos(DateTime? dataInicial, DateTime? horaInicio, DateTime? dataFinal, DateTime? horaFim)
```
