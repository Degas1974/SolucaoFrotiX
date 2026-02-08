# Services/ViagemEstatisticaService.cs

**Mudanca:** GRANDE | **+22** linhas | **-22** linhas

---

```diff
--- JANEIRO: Services/ViagemEstatisticaService.cs
+++ ATUAL: Services/ViagemEstatisticaService.cs
@@ -10,7 +10,6 @@
 
 namespace FrotiX.Services
 {
-
     public class ViagemEstatisticaService
     {
         private readonly FrotiXDbContext _context;
@@ -18,8 +17,8 @@
         private readonly IUnitOfWork _unitOfWork;
 
         public ViagemEstatisticaService(
-            FrotiXDbContext context,
-            IViagemEstatisticaRepository repository,
+            FrotiXDbContext context ,
+            IViagemEstatisticaRepository repository ,
             IUnitOfWork unitOfWork)
         {
             _context = context;
@@ -31,7 +30,6 @@
         {
             try
             {
-
                 var dataReferencia = data.Date;
 
                 var estatisticaExistente = await _repository.ObterPorDataAsync(dataReferencia);
@@ -41,7 +39,7 @@
                 if (estatisticaExistente != null)
                 {
 
-                    AtualizarEstatistica(estatisticaExistente, novaEstatistica);
+                    AtualizarEstatistica(estatisticaExistente , novaEstatistica);
                     await _context.SaveChangesAsync();
                     return estatisticaExistente;
                 }
@@ -56,11 +54,11 @@
             }
             catch (Exception ex)
             {
-                throw new Exception($"Erro ao obter estatísticas: {ex.Message}", ex);
-            }
-        }
-
-        public async Task<List<ViagemEstatistica>> ObterEstatisticasPeriodoAsync(DateTime dataInicio, DateTime dataFim)
+                throw new Exception($"Erro ao obter estatísticas: {ex.Message}" , ex);
+            }
+        }
+
+        public async Task<List<ViagemEstatistica>> ObterEstatisticasPeriodoAsync(DateTime dataInicio , DateTime dataFim)
         {
             try
             {
@@ -75,7 +73,7 @@
             }
             catch (Exception ex)
             {
-                throw new Exception($"Erro ao obter estatísticas do período: {ex.Message}", ex);
+                throw new Exception($"Erro ao obter estatísticas do período: {ex.Message}" , ex);
             }
         }
 
@@ -126,23 +124,21 @@
 
             if (viagensComKm.Any())
             {
-
                 estatistica.QuilometragemTotal = viagensComKm.Sum(v =>
                     (v.KmFinal ?? 0) - (v.KmInicial ?? 0));
-
                 estatistica.QuilometragemMedia = estatistica.QuilometragemTotal / viagensComKm.Count;
             }
 
             var viagensPorStatus = viagens
                 .GroupBy(v => v.Status)
-                .Select(g => new { status = g.Key, quantidade = g.Count() })
+                .Select(g => new { status = g.Key , quantidade = g.Count() })
                 .ToList();
             estatistica.ViagensPorStatusJson = JsonSerializer.Serialize(viagensPorStatus);
 
             var viagensPorMotorista = viagens
                 .Where(v => v.Motorista != null)
                 .GroupBy(v => v.Motorista.Nome)
-                .Select(g => new { motorista = g.Key, quantidade = g.Count() })
+                .Select(g => new { motorista = g.Key , quantidade = g.Count() })
                 .OrderByDescending(x => x.quantidade)
                 .Take(10)
                 .ToList();
@@ -151,7 +147,7 @@
             var viagensPorVeiculo = viagens
                 .Where(v => v.Veiculo != null)
                 .GroupBy(v => v.Veiculo.Placa)
-                .Select(g => new { veiculo = g.Key, quantidade = g.Count() })
+                .Select(g => new { veiculo = g.Key , quantidade = g.Count() })
                 .OrderByDescending(x => x.quantidade)
                 .Take(10)
                 .ToList();
@@ -160,7 +156,7 @@
             var viagensPorFinalidade = viagens
                 .Where(v => !string.IsNullOrEmpty(v.Finalidade))
                 .GroupBy(v => v.Finalidade)
-                .Select(g => new { finalidade = g.Key, quantidade = g.Count() })
+                .Select(g => new { finalidade = g.Key , quantidade = g.Count() })
                 .OrderByDescending(x => x.quantidade)
                 .ToList();
             estatistica.ViagensPorFinalidadeJson = JsonSerializer.Serialize(viagensPorFinalidade);
@@ -168,7 +164,7 @@
             var viagensPorRequisitante = viagens
                 .Where(v => v.Requisitante != null)
                 .GroupBy(v => v.Requisitante.Nome)
-                .Select(g => new { requisitante = g.Key, quantidade = g.Count() })
+                .Select(g => new { requisitante = g.Key , quantidade = g.Count() })
                 .OrderByDescending(x => x.quantidade)
                 .Take(10)
                 .ToList();
@@ -177,7 +173,7 @@
             var viagensPorSetor = viagens
                 .Where(v => v.SetorSolicitante != null)
                 .GroupBy(v => v.SetorSolicitante.Nome)
-                .Select(g => new { setor = g.Key, quantidade = g.Count() })
+                .Select(g => new { setor = g.Key , quantidade = g.Count() })
                 .OrderByDescending(x => x.quantidade)
                 .Take(10)
                 .ToList();
@@ -188,7 +184,7 @@
                 .GroupBy(v => v.Motorista.Nome)
                 .Select(g => new
                 {
-                    motorista = g.Key,
+                    motorista = g.Key ,
                     custoTotal = g.Sum(v => (v.CustoMotorista ?? 0))
                 })
                 .OrderByDescending(x => x.custoTotal)
@@ -201,7 +197,7 @@
                 .GroupBy(v => v.Veiculo.Placa)
                 .Select(g => new
                 {
-                    veiculo = g.Key,
+                    veiculo = g.Key ,
                     custoTotal = g.Sum(v => (v.CustoVeiculo ?? 0))
                 })
                 .OrderByDescending(x => x.custoTotal)
@@ -218,7 +214,7 @@
                 .GroupBy(v => v.Veiculo.Placa)
                 .Select(g => new
                 {
-                    veiculo = g.Key,
+                    veiculo = g.Key ,
                     kmTotal = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                 })
                 .OrderByDescending(x => x.kmTotal)
@@ -239,9 +235,8 @@
             return estatistica;
         }
 
-        private void AtualizarEstatistica(ViagemEstatistica existente, ViagemEstatistica nova)
-        {
-
+        private void AtualizarEstatistica(ViagemEstatistica existente , ViagemEstatistica nova)
+        {
             existente.TotalViagens = nova.TotalViagens;
             existente.ViagensFinalizadas = nova.ViagensFinalizadas;
             existente.ViagensEmAndamento = nova.ViagensEmAndamento;
@@ -266,7 +261,6 @@
             existente.CustosPorVeiculoJson = nova.CustosPorVeiculoJson;
             existente.KmPorVeiculoJson = nova.KmPorVeiculoJson;
             existente.CustosPorTipoJson = nova.CustosPorTipoJson;
-
             existente.DataAtualizacao = DateTime.Now;
         }
 
@@ -277,16 +271,19 @@
                 var dataReferencia = data.Date;
 
                 var novaEstatistica = await CalcularEstatisticasAsync(dataReferencia);
+
                 var estatisticaExistente = await _repository.ObterPorDataAsync(dataReferencia);
 
                 if (estatisticaExistente != null)
                 {
-                    AtualizarEstatistica(estatisticaExistente, novaEstatistica);
+
+                    AtualizarEstatistica(estatisticaExistente , novaEstatistica);
                     await _context.SaveChangesAsync();
                     return estatisticaExistente;
                 }
                 else
                 {
+
                     novaEstatistica.DataCriacao = DateTime.Now;
                     await _repository.AddAsync(novaEstatistica);
                     await _context.SaveChangesAsync();
@@ -295,7 +292,7 @@
             }
             catch (Exception ex)
             {
-                throw new Exception($"Erro ao recalcular estatísticas: {ex.Message}", ex);
+                throw new Exception($"Erro ao recalcular estatísticas: {ex.Message}" , ex);
             }
         }
 
@@ -308,7 +305,7 @@
             }
             catch (Exception ex)
             {
-                throw new Exception($"Erro ao atualizar estatísticas do dia: {ex.Message}", ex);
+                throw new Exception($"Erro ao atualizar estatísticas do dia: {ex.Message}" , ex);
             }
         }
     }
```

### REMOVER do Janeiro

```csharp
            FrotiXDbContext context,
            IViagemEstatisticaRepository repository,
                    AtualizarEstatistica(estatisticaExistente, novaEstatistica);
                throw new Exception($"Erro ao obter estatísticas: {ex.Message}", ex);
            }
        }
        public async Task<List<ViagemEstatistica>> ObterEstatisticasPeriodoAsync(DateTime dataInicio, DateTime dataFim)
                throw new Exception($"Erro ao obter estatísticas do período: {ex.Message}", ex);
                .Select(g => new { status = g.Key, quantidade = g.Count() })
                .Select(g => new { motorista = g.Key, quantidade = g.Count() })
                .Select(g => new { veiculo = g.Key, quantidade = g.Count() })
                .Select(g => new { finalidade = g.Key, quantidade = g.Count() })
                .Select(g => new { requisitante = g.Key, quantidade = g.Count() })
                .Select(g => new { setor = g.Key, quantidade = g.Count() })
                    motorista = g.Key,
                    veiculo = g.Key,
                    veiculo = g.Key,
        private void AtualizarEstatistica(ViagemEstatistica existente, ViagemEstatistica nova)
        {
                    AtualizarEstatistica(estatisticaExistente, novaEstatistica);
                throw new Exception($"Erro ao recalcular estatísticas: {ex.Message}", ex);
                throw new Exception($"Erro ao atualizar estatísticas do dia: {ex.Message}", ex);
```


### ADICIONAR ao Janeiro

```csharp
            FrotiXDbContext context ,
            IViagemEstatisticaRepository repository ,
                    AtualizarEstatistica(estatisticaExistente , novaEstatistica);
                throw new Exception($"Erro ao obter estatísticas: {ex.Message}" , ex);
            }
        }
        public async Task<List<ViagemEstatistica>> ObterEstatisticasPeriodoAsync(DateTime dataInicio , DateTime dataFim)
                throw new Exception($"Erro ao obter estatísticas do período: {ex.Message}" , ex);
                .Select(g => new { status = g.Key , quantidade = g.Count() })
                .Select(g => new { motorista = g.Key , quantidade = g.Count() })
                .Select(g => new { veiculo = g.Key , quantidade = g.Count() })
                .Select(g => new { finalidade = g.Key , quantidade = g.Count() })
                .Select(g => new { requisitante = g.Key , quantidade = g.Count() })
                .Select(g => new { setor = g.Key , quantidade = g.Count() })
                    motorista = g.Key ,
                    veiculo = g.Key ,
                    veiculo = g.Key ,
        private void AtualizarEstatistica(ViagemEstatistica existente , ViagemEstatistica nova)
        {
                    AtualizarEstatistica(estatisticaExistente , novaEstatistica);
                throw new Exception($"Erro ao recalcular estatísticas: {ex.Message}" , ex);
                throw new Exception($"Erro ao atualizar estatísticas do dia: {ex.Message}" , ex);
```
