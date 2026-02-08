# Controllers/AdministracaoController.cs

**Mudanca:** GRANDE | **+8** linhas | **-45** linhas

---

```diff
--- JANEIRO: Controllers/AdministracaoController.cs
+++ ATUAL: Controllers/AdministracaoController.cs
@@ -7,35 +7,22 @@
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.EntityFrameworkCore;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
+
     [Authorize]
     [ApiController]
     public class AdministracaoController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
         private readonly FrotiXDbContext _context;
-        private readonly ILogService _log;
         private const decimal KM_MAXIMO_POR_VIAGEM = 2000;
 
-        public AdministracaoController(IUnitOfWork unitOfWork, FrotiXDbContext context, ILogService logService)
-        {
-            try
-            {
-                _unitOfWork = unitOfWork;
-                _context = context;
-                _log = logService;
-
-                _log.Info("AdministracaoController inicializado", "AdministracaoController.cs", "Constructor");
-            }
-            catch (Exception error)
-            {
-                Console.WriteLine($"Erro no construtor de AdministracaoController: {error.Message}");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "Constructor", error);
-            }
+        public AdministracaoController(IUnitOfWork unitOfWork, FrotiXDbContext context)
+        {
+            _unitOfWork = unitOfWork;
+            _context = context;
         }
 
         [HttpGet]
@@ -44,7 +31,6 @@
         {
             try
             {
-
                 if (!dataInicio.HasValue || !dataFim.HasValue)
                 {
                     dataFim = DateTime.Now.Date;
@@ -89,8 +75,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter resumo geral da frota", ex, "AdministracaoController.cs", "ObterResumoGeralFrota");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterResumoGeralFrota", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message, dados = new { veiculosAtivos = 0, motoristasAtivos = 0, viagensRealizadas = 0, totalKm = 0 } });
             }
         }
@@ -101,7 +85,6 @@
         {
             try
             {
-
                 if (!dataInicio.HasValue || !dataFim.HasValue)
                 {
                     dataFim = DateTime.Now.Date;
@@ -156,8 +139,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter estatísticas de normalização", ex, "AdministracaoController.cs", "ObterEstatisticasNormalizacao");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEstatisticasNormalizacao", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message });
             }
         }
@@ -168,9 +149,9 @@
         {
             try
             {
+
                 try
                 {
-
                     var distribuicao = await _context.VeiculoPadraoViagem
                         .AsNoTracking()
                         .Where(v => !string.IsNullOrEmpty(v.TipoUso))
@@ -215,8 +196,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error(ex.Message, ex, "AdministracaoController.cs", "ObterDistribuicaoTipoUso");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterDistribuicaoTipoUso", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
             }
         }
@@ -268,8 +247,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter heatmap de viagens", ex, "AdministracaoController.cs", "ObterHeatmapViagens");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterHeatmapViagens", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message });
             }
         }
@@ -306,6 +283,7 @@
 
                 if (!viagensComVeiculo.Any())
                 {
+
                     var totalViagensNoPeriodo = await _context.Viagem
                         .AsNoTracking()
                         .CountAsync(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFimAjustada);
@@ -316,12 +294,10 @@
                                         v.DataInicial <= dataFimAjustada &&
                                         v.VeiculoId != null);
 
-                    return Ok(new
-                    {
+                    return Ok(new {
                         sucesso = true,
                         dados = new List<object>(),
-                        debug = new
-                        {
+                        debug = new {
                             totalViagensNoPeriodo,
                             viagensComVeiculoCount,
                             dataInicio = dataInicio?.ToString("yyyy-MM-dd"),
@@ -355,8 +331,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter Top 10 veículos por KM", ex, "AdministracaoController.cs", "ObterTop10VeiculosPorKm");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterTop10VeiculosPorKm", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message, stack = ex.StackTrace, dados = new List<object>() });
             }
         }
@@ -403,12 +377,10 @@
                                         v.DataInicial <= dataFimAjustada &&
                                         v.MotoristaId != null);
 
-                    return Ok(new
-                    {
+                    return Ok(new {
                         sucesso = true,
                         dados = new List<object>(),
-                        debug = new
-                        {
+                        debug = new {
                             totalViagensNoPeriodo,
                             viagensComMotoristaCount
                         }
@@ -438,8 +410,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter Top 10 motoristas por KM", ex, "AdministracaoController.cs", "ObterTop10MotoristasPorKm");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterTop10MotoristasPorKm", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
             }
         }
@@ -486,8 +456,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter custo por finalidade", ex, "AdministracaoController.cs", "ObterCustoPorFinalidade");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterCustoPorFinalidade", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
             }
         }
@@ -579,8 +547,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter comparativo próprios vs terceirizados", ex, "AdministracaoController.cs", "ObterComparativoPropiosTerceirizados");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterComparativoPropiosTerceirizados", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message });
             }
         }
@@ -661,8 +627,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter eficiência da frota", ex, "AdministracaoController.cs", "ObterEficienciaFrota");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEficienciaFrota", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
             }
         }
@@ -721,8 +685,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao obter evolução mensal de custos", ex, "AdministracaoController.cs", "ObterEvolucaoMensalCustos");
-                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEvolucaoMensalCustos", ex);
                 return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using FrotiX.Services;
        private readonly ILogService _log;
        public AdministracaoController(IUnitOfWork unitOfWork, FrotiXDbContext context, ILogService logService)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _context = context;
                _log = logService;
                _log.Info("AdministracaoController inicializado", "AdministracaoController.cs", "Constructor");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Erro no construtor de AdministracaoController: {error.Message}");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "Constructor", error);
            }
                _log.Error("Erro ao obter resumo geral da frota", ex, "AdministracaoController.cs", "ObterResumoGeralFrota");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterResumoGeralFrota", ex);
                _log.Error("Erro ao obter estatísticas de normalização", ex, "AdministracaoController.cs", "ObterEstatisticasNormalizacao");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEstatisticasNormalizacao", ex);
                _log.Error(ex.Message, ex, "AdministracaoController.cs", "ObterDistribuicaoTipoUso");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterDistribuicaoTipoUso", ex);
                _log.Error("Erro ao obter heatmap de viagens", ex, "AdministracaoController.cs", "ObterHeatmapViagens");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterHeatmapViagens", ex);
                    return Ok(new
                    {
                        debug = new
                        {
                _log.Error("Erro ao obter Top 10 veículos por KM", ex, "AdministracaoController.cs", "ObterTop10VeiculosPorKm");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterTop10VeiculosPorKm", ex);
                    return Ok(new
                    {
                        debug = new
                        {
                _log.Error("Erro ao obter Top 10 motoristas por KM", ex, "AdministracaoController.cs", "ObterTop10MotoristasPorKm");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterTop10MotoristasPorKm", ex);
                _log.Error("Erro ao obter custo por finalidade", ex, "AdministracaoController.cs", "ObterCustoPorFinalidade");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterCustoPorFinalidade", ex);
                _log.Error("Erro ao obter comparativo próprios vs terceirizados", ex, "AdministracaoController.cs", "ObterComparativoPropiosTerceirizados");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterComparativoPropiosTerceirizados", ex);
                _log.Error("Erro ao obter eficiência da frota", ex, "AdministracaoController.cs", "ObterEficienciaFrota");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEficienciaFrota", ex);
                _log.Error("Erro ao obter evolução mensal de custos", ex, "AdministracaoController.cs", "ObterEvolucaoMensalCustos");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEvolucaoMensalCustos", ex);
```


### ADICIONAR ao Janeiro

```csharp
        public AdministracaoController(IUnitOfWork unitOfWork, FrotiXDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
                    return Ok(new {
                        debug = new {
                    return Ok(new {
                        debug = new {
```
