# Controllers/ManutencaoController.cs

**Mudanca:** GRANDE | **+29** linhas | **-53** linhas

---

```diff
--- JANEIRO: Controllers/ManutencaoController.cs
+++ ATUAL: Controllers/ManutencaoController.cs
@@ -18,28 +18,29 @@
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
-    public class ManutencaoController : Controller
+    public class ManutencaoController :Controller
     {
         private readonly IWebHostEnvironment _hostingEnvironment;
         private readonly IUnitOfWork _unitOfWork;
         private readonly IMemoryCache _cache;
-        private readonly ILogService _log;
-
-        public ManutencaoController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment, IMemoryCache cache, ILogService log)
+
+        public ManutencaoController(
+            IUnitOfWork unitOfWork ,
+            IWebHostEnvironment hostingEnvironment ,
+            IMemoryCache cache
+        )
         {
             try
             {
                 _unitOfWork = unitOfWork;
                 _hostingEnvironment = hostingEnvironment;
                 _cache = cache;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("ManutencaoController.cs", "ManutencaoController", error);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ManutencaoController" , error);
             }
         }
 
@@ -49,12 +50,10 @@
             TimeSpan ttl
         )
         {
-
             return await _cache.GetOrCreateAsync(
                 key ,
                 async entry =>
                 {
-
                     entry.AbsoluteExpirationRelativeToNow = ttl;
                     return await factory();
                 }
@@ -70,7 +69,6 @@
             DateTime? dtFim
         )
         {
-
             bool filtrarStatus = !string.IsNullOrWhiteSpace(statusId) && statusId != "Todas";
             bool filtrarMesAno = mes.HasValue && ano.HasValue;
             bool filtrarPeriodo = dtIni.HasValue && dtFim.HasValue;
@@ -225,7 +223,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "Get");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "Get" , error);
                 return View();
             }
@@ -245,7 +242,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ApagaConexaoOcorrencia");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ApagaConexaoOcorrencia" , error);
                 return new JsonResult(new
                 {
@@ -268,7 +264,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ApagaConexaoPendencia");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ApagaConexaoPendencia" , error);
                 return new JsonResult(new
                 {
@@ -315,7 +310,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ApagaItens");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ApagaItens" , error);
                 return new JsonResult(new
                 {
@@ -363,7 +357,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ApagaLavagem");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ApagaLavagem" , error);
                 return View();
             }
@@ -422,7 +415,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "CancelaOS");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "CancelaOS" , error);
                 return new JsonResult(new
                 {
@@ -470,7 +462,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "FechaOS");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "FechaOS" , error);
                 return new JsonResult(new
                 {
@@ -535,9 +526,8 @@
                             }
                         }
                     }
-                    catch (Exception)
-                    {
-                        _log.Warning("Falha ao processar JSON de itens removidos na BaixaOS. Tentando fallback.", "ManutencaoController.cs", "BaixaOS");
+                    catch
+                    {
 
                         foreach (var id in itensRemovidosJson.Split(',', StringSplitOptions.RemoveEmptyEntries))
                         {
@@ -647,7 +637,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "BaixaOS");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs", "BaixaOS", error);
                 return new JsonResult(new
                 {
@@ -706,7 +695,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "InsereItemOS");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "InsereItemOS" , error);
                 return new JsonResult(new
                 {
@@ -740,7 +728,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "InsereLavadoresLavagem");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "InsereLavadoresLavagem" , error);
                 return View();
             }
@@ -755,8 +742,7 @@
 
                 var objLavagem = new Lavagem();
                 objLavagem.Data = lavagem.Data;
-                objLavagem.HorarioInicio = lavagem.HorarioInicio;
-                objLavagem.HorarioFim = lavagem.HorarioFim;
+                objLavagem.HorarioLavagem = lavagem.HorarioLavagem;
                 objLavagem.VeiculoId = lavagem.VeiculoId;
                 objLavagem.MotoristaId = lavagem.MotoristaId;
 
@@ -775,7 +761,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "InsereLavagem");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "InsereLavagem" , error);
                 return View();
             }
@@ -815,7 +800,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "InsereOS");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "InsereOS" , error);
                 return new JsonResult(new
                 {
@@ -868,7 +852,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ItensOS");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ItensOS" , error);
                 return View();
             }
@@ -886,8 +869,9 @@
                     {
                         vl.LavagemId ,
                         vl.Data ,
-                        HorarioInicio = DateTime.Parse(vl.HorarioInicio).ToString("HH:mm") ,
-                        HorarioFim = !string.IsNullOrEmpty(vl.HorarioFim) ? DateTime.Parse(vl.HorarioFim).ToString("HH:mm") : null ,
+                        Horario = !string.IsNullOrWhiteSpace(vl.Horario)
+                            ? (vl.Horario.Length >= 5 ? vl.Horario.Substring(0, 5) : vl.Horario)
+                            : null ,
                         vl.DescricaoVeiculo ,
                         vl.Nome ,
                         vl.Lavadores ,
@@ -928,7 +912,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaLavagemLavadores");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaLavagemLavadores" , error);
                 return View();
             }
@@ -947,8 +930,9 @@
                     {
                         vl.LavagemId ,
                         vl.Data ,
-                        HorarioInicio = DateTime.Parse(vl.HorarioInicio).ToString("HH:mm") ,
-                        HorarioFim = !string.IsNullOrEmpty(vl.HorarioFim) ? DateTime.Parse(vl.HorarioFim).ToString("HH:mm") : null ,
+                        Horario = !string.IsNullOrWhiteSpace(vl.Horario)
+                            ? (vl.Horario.Length >= 5 ? vl.Horario.Substring(0, 5) : vl.Horario)
+                            : null ,
                         vl.DescricaoVeiculo ,
                         vl.Nome ,
                         vl.Lavadores ,
@@ -962,7 +946,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaLavagemMotoristas");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaLavagemMotoristas" , error);
                 return View();
             }
@@ -981,8 +964,9 @@
                     {
                         vl.LavagemId ,
                         vl.Data ,
-                        HorarioInicio = DateTime.Parse(vl.HorarioInicio).ToString("HH:mm") ,
-                        HorarioFim = !string.IsNullOrEmpty(vl.HorarioFim) ? DateTime.Parse(vl.HorarioFim).ToString("HH:mm") : null ,
+                        Horario = !string.IsNullOrWhiteSpace(vl.Horario)
+                            ? (vl.Horario.Length >= 5 ? vl.Horario.Substring(0, 5) : vl.Horario)
+                            : null ,
                         vl.DescricaoVeiculo ,
                         vl.Nome ,
                         vl.Lavadores ,
@@ -996,7 +980,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaLavagemVeiculos");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaLavagemVeiculos" , error);
                 return View();
             }
@@ -1014,8 +997,9 @@
                     {
                         vl.LavagemId ,
                         vl.Data ,
-                        HorarioInicio = DateTime.Parse(vl.HorarioInicio).ToString("HH:mm") ,
-                        HorarioFim = !string.IsNullOrEmpty(vl.HorarioFim) ? DateTime.Parse(vl.HorarioFim).ToString("HH:mm") : null ,
+                        Horario = !string.IsNullOrWhiteSpace(vl.Horario)
+                            ? (vl.Horario.Length >= 5 ? vl.Horario.Substring(0, 5) : vl.Horario)
+                            : null ,
                         vl.DescricaoVeiculo ,
                         vl.Nome ,
                         vl.Lavadores ,
@@ -1029,7 +1013,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaLavagens");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaLavagens" , error);
                 return View();
             }
@@ -1048,8 +1031,9 @@
                     {
                         vl.LavagemId ,
                         vl.Data ,
-                        HorarioInicio = DateTime.Parse(vl.HorarioInicio).ToString("HH:mm") ,
-                        HorarioFim = !string.IsNullOrEmpty(vl.HorarioFim) ? DateTime.Parse(vl.HorarioFim).ToString("HH:mm") : null ,
+                        Horario = !string.IsNullOrWhiteSpace(vl.Horario)
+                            ? (vl.Horario.Length >= 5 ? vl.Horario.Substring(0, 5) : vl.Horario)
+                            : null ,
                         vl.DescricaoVeiculo ,
                         vl.Nome ,
                         vl.Lavadores ,
@@ -1063,7 +1047,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaLavagensData");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaLavagensData" , error);
                 return View();
             }
@@ -1078,7 +1061,7 @@
                 var objManutencacao = await _unitOfWork
                     .ViewManutencao.GetAllReducedIQueryable(
                         selector: vm => vm ,
-                        filter: manutencoesFilters(Guid.Empty, "Aberta", null, null, null, null)
+                        filter: manutencoesFilters("Aberta")
                     )
                     .AsNoTracking()
 
@@ -1091,7 +1074,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaManutencao");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaManutencao" , error);
                 return View();
             }
@@ -1126,7 +1108,6 @@
             }
             catch (Exception error)
             {
-                 _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaManutencaoData");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaManutencaoData" , error);
                 return View();
             }
@@ -1176,7 +1157,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaManutencaoIntervalo");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaManutencaoIntervalo" , error);
                 return View();
             }
@@ -1223,7 +1203,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaManutencaoStatus");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaManutencaoStatus" , error);
                 return View();
             }
@@ -1267,7 +1246,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ListaManutencaoVeiculo");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ListaManutencaoVeiculo" , error);
                 return View();
             }
@@ -1313,7 +1291,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "OcorrenciasVeiculosManutencao");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "OcorrenciasVeiculosManutencao" , error);
                 return Json(new { data = Array.Empty<object>() });
             }
@@ -1354,7 +1331,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "OcorrenciasVeiculosPendencias");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "OcorrenciasVeiculosPendencias" , error);
                 return View();
             }
@@ -1374,7 +1350,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "RecuperaLavador");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "RecuperaLavador" , error);
                 return View();
             }
@@ -1404,7 +1379,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "RecuperaUsuario");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "RecuperaUsuario" , error);
                 return View();
             }
@@ -1452,7 +1426,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "SaveImage");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "SaveImage" , error);
                 Response.StatusCode = 204;
             }
@@ -1480,7 +1453,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message , error , "ManutencaoController.cs" , "ZeraItensOS");
                 Alerta.TratamentoErroComLinha("ManutencaoController.cs" , "ZeraItensOS" , error);
                 return new JsonResult(new
                 {
```
