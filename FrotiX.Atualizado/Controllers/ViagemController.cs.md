# Controllers/ViagemController.cs

**Mudanca:** GRANDE | **+271** linhas | **-221** linhas

---

```diff
--- JANEIRO: Controllers/ViagemController.cs
+++ ATUAL: Controllers/ViagemController.cs
@@ -32,7 +32,6 @@
     {
         private readonly FrotiXDbContext _context;
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
         private IWebHostEnvironment hostingEnv;
         private readonly IViagemRepository _viagemRepo;
         private readonly MotoristaFotoService _fotoService;
@@ -43,15 +42,12 @@
 
         [ActivatorUtilitiesConstructor]
         public ViagemController(
-            FrotiXDbContext context,
-            IUnitOfWork unitOfWork,
-            IViagemRepository viagemRepo,
-            IWebHostEnvironment webHostEnvironment,
-            MotoristaFotoService fotoService,
-            IMemoryCache cache,
-            IServiceScopeFactory serviceScopeFactory,
-            IViagemEstatisticaRepository viagemEstatisticaRepository,
-            ILogService log
+            FrotiXDbContext context ,
+            IUnitOfWork unitOfWork ,
+            IViagemRepository viagemRepo ,
+            IWebHostEnvironment webHostEnvironment ,
+            MotoristaFotoService fotoService ,
+            IMemoryCache cache , IServiceScopeFactory serviceScopeFactory , IViagemEstatisticaRepository viagemEstatisticaRepository
         )
         {
             try
@@ -63,13 +59,12 @@
                 _cache = cache;
                 _serviceScopeFactory = serviceScopeFactory;
                 _context = context;
-                _log = log;
-                _viagemEstatisticaService = new ViagemEstatisticaService(_context, viagemEstatisticaRepository, unitOfWork);
-                _veiculoEstatisticaService = new VeiculoEstatisticaService(_context, cache);
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("ViagemController.cs", "ViagemController", error);
+                _viagemEstatisticaService = new ViagemEstatisticaService(_context , viagemEstatisticaRepository , unitOfWork);
+                _veiculoEstatisticaService = new VeiculoEstatisticaService(_context , cache);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemController" , error);
             }
         }
 
@@ -134,13 +129,16 @@
 
         [HttpPost]
         [Route("UploadFichaVistoria")]
+
         public async Task<IActionResult> UploadFichaVistoria(
-            IFormFile arquivo,
+            IFormFile arquivo ,
             [FromForm] string viagemId
         )
         {
             try
             {
+                Console.WriteLine($"Recebido viagemId: {viagemId}");
+
                 if (arquivo == null || arquivo.Length == 0)
                 {
                     return Json(new
@@ -190,16 +188,8 @@
                     viagem.FichaVistoria = ms.ToArray();
                 }
 
-                var nomeArquivo = arquivo.FileName?.ToLowerInvariant() ?? "";
-                var ehFichaPadrao = nomeArquivo.Contains("fichaamarelanova") ||
-                                    nomeArquivo.Contains("ficha_amarela") ||
-                                    nomeArquivo.Contains("fichapadrao");
-                viagem.TemFichaVistoriaReal = !ehFichaPadrao;
-
                 _unitOfWork.Viagem.Update(viagem);
                 _unitOfWork.Save();
-
-                _log.Info($"Ficha de vistoria atualizada para a viagem ID: {viagemId}. Arquivo: {nomeArquivo}", "ViagemController", "UploadFichaVistoria");
 
                 var base64 = Convert.ToBase64String(viagem.FichaVistoria);
                 return Json(
@@ -213,8 +203,8 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "UploadFichaVistoria");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "UploadFichaVistoria" , error);
+                Console.WriteLine($"Erro ao salvar ficha: {error.Message}");
                 return Json(
                     new
                     {
@@ -226,10 +216,12 @@
         }
 
         [Route("ExisteFichaParaData")]
+
         public JsonResult ExisteFichaParaData(string data)
         {
             try
             {
+
                 if (!DateTime.TryParseExact(data , "yyyy-MM-dd" , CultureInfo.InvariantCulture , DateTimeStyles.None , out DateTime dataConvertida))
                 {
                     return Json(false);
@@ -243,7 +235,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ExisteFichaParaData");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ExisteFichaParaData" , error);
                 return Json(false);
             }
@@ -251,6 +242,7 @@
 
         [Route("VerificaFichaExiste")]
         [HttpGet]
+
         public IActionResult VerificaFichaExiste(int noFichaVistoria)
         {
             try
@@ -274,15 +266,15 @@
 
                 return Json(new { success = true , data = new { existe = false } });
             }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "ViagemController", "VerificaFichaExiste");
-                return Json(new { success = false , message = error.Message });
+            catch (Exception ex)
+            {
+                return Json(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet]
         [Route("ObterFichaVistoria")]
+
         public IActionResult ObterFichaVistoria(string viagemId)
         {
             try
@@ -328,7 +320,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ObterFichaVistoria");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterFichaVistoria" , error);
                 return Json(new
                 {
@@ -340,6 +331,7 @@
 
         [Route("MontaDescricaoSemFormato")]
         [HttpPost]
+
         public IActionResult MontaDescricaoSemFormato()
         {
             try
@@ -349,11 +341,10 @@
                 foreach (var viagem in objViagens)
                 {
                     viagem.DescricaoSemFormato = Servicos.ConvertHtml(viagem.Descricao);
+
                     _unitOfWork.Viagem.Update(viagem);
-                }
-                _unitOfWork.Save();
-
-                _log.Info("Sincronização de descrições sem formato concluída.", "ViagemController", "MontaDescricaoSemFormato");
+                    _unitOfWork.Save();
+                }
 
                 return Json(new
                 {
@@ -362,7 +353,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "MontaDescricaoSemFormato");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "MontaDescricaoSemFormato" , error);
                 return Json(new
                 {
@@ -374,6 +364,7 @@
 
         [HttpGet]
         [Route("FotoMotorista")]
+
         public IActionResult FotoMotorista(Guid id)
         {
             try
@@ -421,7 +412,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "FotoMotorista");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "FotoMotorista" , error);
                 return Json(new
                 {
@@ -431,6 +421,7 @@
         }
 
         [HttpGet("PegarStatusViagem")]
+
         public IActionResult PegarStatusViagem(Guid viagemId)
         {
             try
@@ -444,13 +435,13 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "PegarStatusViagem");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegarStatusViagem" , error);
                 return StatusCode(500 , $"Erro ao verificar status da viagem: {error.Message}");
             }
         }
 
         [HttpGet("ListaDistintos")]
+
         public IActionResult ListaDistintos()
         {
             try
@@ -485,7 +476,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ListaDistintos");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ListaDistintos" , error);
                 return StatusCode(500 , $"Erro ao obter origens/destinos: {error.Message}");
             }
@@ -511,6 +501,7 @@
 
         [HttpPost]
         [Route("Unificar")]
+
         public IActionResult Unificar([FromBody] UnificacaoRequest request)
         {
             try
@@ -600,8 +591,6 @@
                     _unitOfWork.Save();
                 }
 
-                _log.Info($"Unificação de endereços concluída. Novo Valor: {request.NovoValor}", "ViagemController", "Unificar");
-
                 return Ok(new
                 {
                     mensagem = "Unificação realizada com sucesso."
@@ -609,7 +598,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "Unificar");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "Unificar" , error);
                 return StatusCode(500 , new
                 {
@@ -620,6 +608,7 @@
         }
 
         [HttpGet]
+
         public IActionResult Get(
             string veiculoId = null ,
             string motoristaId = null ,
@@ -687,7 +676,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "Get");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "Get" , error);
                 return Json(new
                 {
@@ -699,6 +687,7 @@
 
         [HttpPost]
         [Route("AplicarCorrecaoOrigem")]
+
         public async Task<IActionResult> AplicarCorrecaoOrigem([FromBody] CorrecaoOrigemDto dto)
         {
             try
@@ -707,14 +696,10 @@
                     return BadRequest();
 
                 await _viagemRepo.CorrigirOrigemAsync(dto.Origens , dto.NovaOrigem);
-
-                _log.Info($"Correção de Origem aplicada: {dto.NovaOrigem}", "ViagemController", "AplicarCorrecaoOrigem");
-
                 return Ok();
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AplicarCorrecaoOrigem");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AplicarCorrecaoOrigem" , error);
                 return StatusCode(500);
             }
@@ -722,6 +707,7 @@
 
         [HttpPost]
         [Route("AplicarCorrecaoDestino")]
+
         public async Task<IActionResult> AplicarCorrecaoDestino([FromBody] CorrecaoDestinoDto dto)
         {
             try
@@ -730,14 +716,10 @@
                     return BadRequest();
 
                 await _viagemRepo.CorrigirDestinoAsync(dto.Destinos , dto.NovoDestino);
-
-                _log.Info($"Correção de Destino aplicada: {dto.NovoDestino}", "ViagemController", "AplicarCorrecaoDestino");
-
                 return Ok();
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AplicarCorrecaoDestino");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AplicarCorrecaoDestino" , error);
                 return StatusCode(500);
             }
@@ -771,6 +753,7 @@
 
         [Route("FluxoFiltrado")]
         [HttpGet]
+
         public IActionResult FluxoFiltrado(string veiculoId , string motoristaId , string dataFluxo)
         {
             try
@@ -841,7 +824,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "FluxoFiltrado");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoFiltrado" , error);
                 return Json(new
                 {
@@ -852,6 +834,7 @@
         }
 
         [Route("UpdateStatusEvento")]
+
         public JsonResult UpdateStatusEvento(Guid Id)
         {
             try
@@ -883,9 +866,6 @@
                             type = 0;
                         }
                         _unitOfWork.Evento.Update(objFromDb);
-                        _unitOfWork.Save();
-
-                        _log.Warning(Description, "ViagemController", "UpdateStatusEvento");
                     }
                     return Json(
                         new
@@ -903,7 +883,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "UpdateStatusEvento");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "UpdateStatusEvento" , error);
                 return Json(new
                 {
@@ -914,6 +893,7 @@
 
         [Route("ApagaEvento")]
         [HttpGet]
+
         public IActionResult ApagaEvento(string eventoId)
         {
             try
@@ -939,9 +919,6 @@
 
                     _unitOfWork.Evento.Remove(objFromDb);
                     _unitOfWork.Save();
-
-                    _log.Warning($"Evento removido: {objFromDb.Nome}", "ViagemController", "ApagaEvento");
-
                     return Json(new
                     {
                         success = true ,
@@ -957,7 +934,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ApagaEvento");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ApagaEvento" , error);
                 return Json(new
                 {
@@ -969,6 +945,7 @@
 
         [Route("ViagemEventos")]
         [HttpGet]
+
         public IActionResult ViagemEventos()
         {
             try
@@ -984,7 +961,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ViagemEventos");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemEventos" , error);
                 return Json(new
                 {
@@ -996,6 +972,7 @@
 
         [Route("Fluxo")]
         [HttpGet]
+
         public IActionResult Fluxo()
         {
             try
@@ -1023,7 +1000,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "Fluxo");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "Fluxo" , error);
                 return Json(new
                 {
@@ -1035,6 +1011,7 @@
 
         [Route("FluxoVeiculos")]
         [HttpGet]
+
         public IActionResult FluxoVeiculos(string Id)
         {
             try
@@ -1063,7 +1040,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "FluxoVeiculos");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoVeiculos" , error);
                 return Json(new
                 {
@@ -1075,6 +1051,7 @@
 
         [Route("FluxoMotoristas")]
         [HttpGet]
+
         public IActionResult FluxoMotoristas(string Id)
         {
             try
@@ -1103,7 +1080,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "FluxoMotoristas");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoMotoristas" , error);
                 return Json(new
                 {
@@ -1115,6 +1091,7 @@
 
         [Route("FluxoData")]
         [HttpGet]
+
         public IActionResult FluxoData(string Id)
         {
             try
@@ -1146,7 +1123,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "FluxoData");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "FluxoData" , error);
                 return Json(new
                 {
@@ -1158,6 +1134,7 @@
 
         [Route("ApagaFluxoEconomildo")]
         [HttpPost]
+
         public IActionResult ApagaFluxoEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
         {
             try
@@ -1187,8 +1164,6 @@
                 _unitOfWork.ViagensEconomildo.Remove(objFromDb);
                 _unitOfWork.Save();
 
-                _log.Warning($"Registro de fluxo Economildo removido ID: {objFromDb.ViagemEconomildoId}", "ViagemController", "ApagaFluxoEconomildo");
-
                 return Json(new
                 {
                     success = true,
@@ -1197,7 +1172,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ApagaFluxoEconomildo");
                 Alerta.TratamentoErroComLinha("ViagemController.cs", "ApagaFluxoEconomildo", error);
                 return Json(new
                 {
@@ -1209,6 +1183,7 @@
 
         [Route("MyUploader")]
         [HttpPost]
+
         public IActionResult MyUploader(IFormFile MyUploader , [FromForm] string ViagemId)
         {
             try
@@ -1227,8 +1202,6 @@
                     _unitOfWork.Viagem.Update(viagemObj);
                     _unitOfWork.Save();
 
-                    _log.Info($"Ficha de Vistoria via MyUploader salva para Viagem ID: {ViagemId}", "ViagemController", "MyUploader");
-
                     return new ObjectResult(new
                     {
                         status = "success"
@@ -1241,7 +1214,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "MyUploader");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "MyUploader" , error);
                 return new ObjectResult(new
                 {
@@ -1252,6 +1224,7 @@
 
         [Route("CalculaCustoViagens")]
         [HttpPost]
+
         public IActionResult CalculaCustoViagens()
         {
             try
@@ -1272,8 +1245,6 @@
 
                 Task.Run(async () => await ProcessarCalculoCustoViagens());
 
-                _log.Warning("Processamento de cálculo de custos iniciado pelo usuário.", "ViagemController", "CalculaCustoViagens");
-
                 return Json(new
                 {
                     success = true ,
@@ -1282,7 +1253,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "CalculaCustoViagens");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "CalculaCustoViagens" , error);
                 return Json(new
                 {
@@ -1309,7 +1279,7 @@
             try
             {
 
-                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
 
                 using (var scope = _serviceScopeFactory.CreateScope())
                 {
@@ -1331,7 +1301,7 @@
 
                     progresso.Total = objViagens.Count;
                     progresso.Mensagem = $"Processando {progresso.Total} viagens...";
-                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
 
                     int contador = 0;
 
@@ -1368,7 +1338,7 @@
                                 : 0;
                             progresso.Mensagem = $"Processando viagem {contador} de {progresso.Total}...";
 
-                            _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                            _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
 
                             if (contador % 10 == 0)
                             {
@@ -1385,25 +1355,23 @@
                     progresso.Concluido = true;
                     progresso.Percentual = 100;
                     progresso.Mensagem = $"Processamento concluído! {contador} viagens atualizadas.";
-                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
-
-                    _log.Info("Cálculo de custos em lote finalizado com sucesso.", "ViagemController", "ProcessarCalculoCustoViagens");
-                }
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "ViagemController", "ProcessarCalculoCustoViagens");
+                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
+                }
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ProcessarCalculoCustoViagens" , error);
 
                 progresso.Erro = true;
                 progresso.Concluido = true;
                 progresso.Mensagem = $"Erro durante o processamento: {error.Message}";
-                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
             }
         }
 
         [Route("ObterProgressoCalculoCusto")]
         [HttpGet]
+
         public IActionResult ObterProgressoCalculoCusto()
         {
             try
@@ -1443,7 +1411,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ObterProgressoCalculoCusto");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterProgressoCalculoCusto" , error);
                 return Json(new
                 {
@@ -1455,6 +1422,7 @@
 
         [Route("LimparProgressoCalculoCusto")]
         [HttpPost]
+
         public IActionResult LimparProgressoCalculoCusto()
         {
             try
@@ -1470,7 +1438,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "LimparProgressoCalculoCusto");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "LimparProgressoCalculoCusto" , error);
                 return Json(new
                 {
@@ -1482,6 +1449,7 @@
 
         [Route("ViagemVeiculos")]
         [HttpGet]
+
         public IActionResult ViagemVeiculos(Guid Id)
         {
             try
@@ -1494,7 +1462,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ViagemVeiculos");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemVeiculos" , error);
                 return StatusCode(500);
             }
@@ -1502,6 +1469,7 @@
 
         [Route("ViagemMotoristas")]
         [HttpGet]
+
         public IActionResult ViagemMotoristas(Guid Id)
         {
             try
@@ -1514,7 +1482,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ViagemMotoristas");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemMotoristas" , error);
                 return StatusCode(500);
             }
@@ -1522,6 +1489,7 @@
 
         [Route("ViagemStatus")]
         [HttpGet]
+
         public IActionResult ViagemStatus(string Id)
         {
             try
@@ -1534,7 +1502,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ViagemStatus");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemStatus" , error);
                 return StatusCode(500);
             }
@@ -1542,6 +1509,7 @@
 
         [Route("ViagemSetores")]
         [HttpGet]
+
         public IActionResult ViagemSetores(Guid Id)
         {
             try
@@ -1554,7 +1522,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ViagemSetores");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemSetores" , error);
                 return StatusCode(500);
             }
@@ -1562,6 +1529,7 @@
 
         [Route("ViagemData")]
         [HttpGet]
+
         public IActionResult ViagemData(string Id)
         {
             try
@@ -1574,7 +1542,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ViagemData");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ViagemData" , error);
                 return StatusCode(500);
             }
@@ -1582,6 +1549,7 @@
 
         [Route("Ocorrencias")]
         [HttpGet]
+
         public IActionResult Ocorrencias(Guid Id)
         {
             try
@@ -1594,7 +1562,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "Ocorrencias");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "Ocorrencias" , error);
                 return StatusCode(500);
             }
@@ -1602,6 +1569,7 @@
 
         [Route("Cancelar")]
         [HttpPost]
+
         public IActionResult Cancelar(ViagemID id)
         {
             try
@@ -1609,17 +1577,14 @@
                 var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id.ViagemId);
                 if (objFromDb != null)
                 {
-                    System.Security.Claims.ClaimsPrincipal currentUser = this.User;
-                    var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
+                    ClaimsPrincipal currentUser = this.User;
+                    var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                     objFromDb.UsuarioIdCancelamento = currentUserID;
                     objFromDb.DataCancelamento = DateTime.Now;
 
                     objFromDb.Status = "Cancelada";
                     _unitOfWork.Viagem.Update(objFromDb);
                     _unitOfWork.Save();
-
-                    _log.Warning($"Viagem cancelada. ID: {id.ViagemId}, Usuário: {currentUserID}", "ViagemController", "Cancelar");
-
                     return Json(new
                     {
                         success = true ,
@@ -1634,7 +1599,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "Cancelar");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "Cancelar" , error);
                 return Json(new
                 {
@@ -1646,6 +1610,7 @@
 
         [HttpGet]
         [Route("PegaFicha")]
+
         public JsonResult PegaFicha([FromQuery] Guid id)
         {
             try
@@ -1671,7 +1636,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "PegaFicha");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaFicha" , error);
                 return Json(false);
             }
@@ -1679,6 +1643,7 @@
 
         [Route("AdicionarViagensEconomildo")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarViagensEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
         {
             try
@@ -1686,8 +1651,6 @@
                 _unitOfWork.ViagensEconomildo.Add(viagensEconomildo);
                 _unitOfWork.Save();
 
-                _log.Info($"Viagem Economildo adicionada. ID: {viagensEconomildo.ViagemEconomildoId}", "ViagemController", "AdicionarViagensEconomildo");
-
                 return Json(new
                 {
                     success = true ,
@@ -1696,7 +1659,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AdicionarViagensEconomildo");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarViagensEconomildo" , error);
                 return Json(new
                 {
@@ -1708,6 +1670,7 @@
 
         [Route("ExisteDataEconomildo")]
         [Consumes("application/json")]
+
         public JsonResult ExisteDataEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
         {
             try
@@ -1743,7 +1706,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ExisteDataEconomildo");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ExisteDataEconomildo" , error);
                 return Json(new
                 {
@@ -1755,6 +1717,7 @@
 
         [HttpGet]
         [Route("PegaFichaModal")]
+
         public JsonResult PegaFichaModal(Guid id)
         {
             try
@@ -1771,7 +1734,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "PegaFichaModal");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaFichaModal" , error);
                 return Json(false);
             }
@@ -1779,6 +1741,7 @@
 
         [HttpGet]
         [Route("PegaCategoria")]
+
         public JsonResult PegaCategoria(Guid id)
         {
             try
@@ -1792,7 +1755,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "PegaCategoria");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "PegaCategoria" , error);
                 return Json(false);
             }
@@ -1800,6 +1762,7 @@
 
         [Route("AdicionarViagensEconomildoLote")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarViagensEconomildoLote([FromBody] List<ViagensEconomildo> viagens)
         {
             try
@@ -1820,8 +1783,6 @@
 
                 _unitOfWork.Save();
 
-                _log.Info($"{viagens.Count} viagens Economildo adicionadas em lote.", "ViagemController", "AdicionarViagensEconomildoLote");
-
                 return Json(new
                 {
                     success = true ,
@@ -1830,7 +1791,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AdicionarViagensEconomildoLote");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarViagensEconomildoLote" , error);
                 return Json(new
                 {
@@ -1841,6 +1801,7 @@
         }
 
         [Route("BuscarViagemEconomildo")]
+
         public JsonResult BuscarViagemEconomildo(Guid id)
         {
             try
@@ -1878,7 +1839,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "BuscarViagemEconomildo");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "BuscarViagemEconomildo" , error);
                 return Json(new
                 {
@@ -1890,6 +1850,7 @@
 
         [Route("AtualizarViagemEconomildo")]
         [Consumes("application/json")]
+
         public JsonResult AtualizarViagemEconomildo([FromBody] ViagensEconomildo viagem)
         {
             try
@@ -1929,8 +1890,6 @@
                 _unitOfWork.ViagensEconomildo.Update(objFromDb);
                 _unitOfWork.Save();
 
-                _log.Info($"Viagem Economildo atualizada. ID: {viagem.ViagemEconomildoId}", "ViagemController", "AtualizarViagemEconomildo");
-
                 return Json(new
                 {
                     success = true ,
@@ -1939,7 +1898,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AtualizarViagemEconomildo");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AtualizarViagemEconomildo" , error);
                 return Json(new
                 {
@@ -1961,6 +1919,7 @@
 
         [Route("AdicionarEvento")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarEvento([FromBody] Evento evento)
         {
             try
@@ -1977,8 +1936,6 @@
 
                 _unitOfWork.Evento.Add(evento);
                 _unitOfWork.Save();
-
-                _log.Info($"Novo evento adicionado: {evento.Nome}", "ViagemController", "AdicionarEvento");
 
                 return Json(
                     new
@@ -1992,7 +1949,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AdicionarEvento");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarEvento" , error);
                 return Json(new
                 {
@@ -2004,6 +1960,7 @@
 
         [Route("AdicionarRequisitante")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarRequisitante([FromBody] Requisitante requisitante)
         {
             try
@@ -2025,14 +1982,12 @@
                 requisitante.Status = true;
                 requisitante.DataAlteracao = DateTime.Now;
 
-                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
-                var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
+                ClaimsPrincipal currentUser = this.User;
+                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                 requisitante.UsuarioIdAlteracao = currentUserID;
 
                 _unitOfWork.Requisitante.Add(requisitante);
                 _unitOfWork.Save();
-
-                _log.Info($"Novo requisitante adicionado: {requisitante.Nome}", "ViagemController", "AdicionarRequisitante");
 
                 return Json(
                     new
@@ -2045,7 +2000,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AdicionarRequisitante");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarRequisitante" , error);
                 return Json(new
                 {
@@ -2057,6 +2011,7 @@
 
         [Route("AdicionarSetor")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarSetor([FromBody] SetorSolicitante setorSolicitante)
         {
             try
@@ -2107,14 +2062,12 @@
                 setorSolicitante.Status = true;
                 setorSolicitante.DataAlteracao = DateTime.Now;
 
-                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
-                var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
+                ClaimsPrincipal currentUser = this.User;
+                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                 setorSolicitante.UsuarioIdAlteracao = currentUserID;
 
                 _unitOfWork.SetorSolicitante.Add(setorSolicitante);
                 _unitOfWork.Save();
-
-                _log.Info($"Novo setor adicionado: {setorSolicitante.Nome}", "ViagemController", "AdicionarSetor");
 
                 return Json(
                     new
@@ -2127,7 +2080,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "AdicionarSetor");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "AdicionarSetor" , error);
                 return Json(new
                 {
@@ -2138,6 +2090,7 @@
         }
 
         [Route("ListaViagensEvento")]
+
         public async Task<IActionResult> ListaViagensEvento(
             Guid Id ,
             int page = 1 ,
@@ -2153,10 +2106,11 @@
                 if (useCache && _cache.TryGetValue(cacheKey , out var cachedResult))
                 {
                     swCache.Stop();
-                    _log.Info($"ListaViagensEvento - Cache Hit: {Id}", "ViagemController", "ListaViagensEvento");
+                    Console.WriteLine($"[CACHE HIT] {swCache.ElapsedMilliseconds}ms");
                     return Ok(cachedResult);
                 }
                 swCache.Stop();
+                Console.WriteLine($"[CACHE MISS] {swCache.ElapsedMilliseconds}ms");
 
                 var (viagens, totalItems) = await _unitOfWork.Viagem.GetViagensEventoPaginadoAsync(
                     Id ,
@@ -2180,8 +2134,7 @@
                 {
                     var cacheOptions = new MemoryCacheEntryOptions()
                         .SetSlidingExpiration(TimeSpan.FromMinutes(5))
-                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
-                        .SetSize(1);
+                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                     _cache.Set(cacheKey , result , cacheOptions);
                 }
 
@@ -2201,143 +2154,244 @@
 
         [Route("ObterTotalCustoViagensEvento")]
         [HttpGet]
+
         public async Task<IActionResult> ObterTotalCustoViagensEvento(Guid Id)
         {
             try
             {
                 if (Id == Guid.Empty)
                 {
-                    return BadRequest(new
-                    {
-                        success = false ,
-                        message = "ID do evento inválido"
-                    });
-                }
-
-                var evento = _unitOfWork.Evento.GetFirstOrDefault(e => e.EventoId == Id);
+                    return await Task.FromResult(
+                        BadRequest(new
+                        {
+                            success = false ,
+                            message = "ID do evento inválido"
+                        })
+                    );
+                }
+
+                var evento = await Task.Run(() =>
+                    _unitOfWork.Evento.GetFirstOrDefault(e => e.EventoId == Id)
+                );
                 if (evento == null)
                 {
-                    return NotFound(new
-                    {
-                        success = false ,
-                        message = "Evento não encontrado"
-                    });
-                }
-
-                var viagens = _unitOfWork.ViewViagens.GetAll(filter: x =>
-                    x.EventoId == Id && x.Status == "Realizada"
-                ).ToList();
+                    return await Task.FromResult(
+                        NotFound(new
+                        {
+                            success = false ,
+                            message = "Evento não encontrado"
+                        })
+                    );
+                }
+
+                var viagens = await Task.Run(() =>
+                    _unitOfWork
+                        .ViewViagens.GetAll(filter: x =>
+                            x.EventoId == Id && x.Status == "Realizada"
+                        )
+                        .ToList()
+                );
 
                 var estatisticas = new
                 {
-                    TotalViagens = viagens.Count ,
-                    ViagensComCusto = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).ToList() ,
-                    ViagensSemCusto = viagens.Where(v => !v.CustoViagem.HasValue || v.CustoViagem == 0).ToList() ,
+                    TotalViagens = viagens.Count() ,
+                    ViagensComCusto = viagens
+                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
+                        .ToList() ,
+                    ViagensSemCusto = viagens
+                        .Where(v => !v.CustoViagem.HasValue || v.CustoViagem == 0)
+                        .ToList() ,
                 };
 
-                decimal custoTotal = estatisticas.ViagensComCusto.Sum(v => (decimal)(v.CustoViagem ?? 0));
-                decimal custoMedio = estatisticas.ViagensComCusto.Count > 0 ? custoTotal / estatisticas.ViagensComCusto.Count : 0;
-
-                decimal? custoMinimo = estatisticas.ViagensComCusto.Count > 0 ? (decimal)estatisticas.ViagensComCusto.Min(v => v.CustoViagem ?? 0) : (decimal?)null;
-                decimal? custoMaximo = estatisticas.ViagensComCusto.Count > 0 ? (decimal)estatisticas.ViagensComCusto.Max(v => v.CustoViagem ?? 0) : (decimal?)null;
+                decimal custoTotal = estatisticas.ViagensComCusto.Sum(v =>
+                    (decimal)(v.CustoViagem ?? 0)
+                );
+                decimal custoMedio =
+                    estatisticas.ViagensComCusto.Count > 0
+                        ? custoTotal / estatisticas.ViagensComCusto.Count
+                        : 0;
+
+                decimal? custoMinimo =
+                    estatisticas.ViagensComCusto.Count > 0
+                        ? (decimal)estatisticas.ViagensComCusto.Min(v => v.CustoViagem ?? 0)
+                        : (decimal?)null;
+
+                decimal? custoMaximo =
+                    estatisticas.ViagensComCusto.Count > 0
+                        ? (decimal)estatisticas.ViagensComCusto.Max(v => v.CustoViagem ?? 0)
+                        : (decimal?)null;
 
                 var culturaBR = new System.Globalization.CultureInfo("pt-BR");
 
-                _log.Info($"Consulta de custos para evento: {evento.Nome}", "ViagemController", "ObterTotalCustoViagensEvento");
-
-                return Ok(new
-                {
-                    success = true ,
-                    eventoId = Id ,
-                    nomeEvento = evento?.Nome ,
-                    totalViagens = estatisticas.TotalViagens ,
-                    totalViagensComCusto = estatisticas.ViagensComCusto.Count ,
-                    viagensSemCusto = estatisticas.ViagensSemCusto.Count ,
-                    percentualComCusto = estatisticas.TotalViagens > 0 ? (estatisticas.ViagensComCusto.Count * 100.0 / estatisticas.TotalViagens) : 0 ,
-                    totalCusto = custoTotal ,
-                    custoMedio = custoMedio ,
-                    custoMinimo = custoMinimo ?? 0 ,
-                    custoMaximo = custoMaximo ?? 0 ,
-                    totalCustoFormatado = custoTotal.ToString("C" , culturaBR) ,
-                    custoMedioFormatado = custoMedio.ToString("C" , culturaBR) ,
-                    custoMinimoFormatado = custoMinimo?.ToString("C" , culturaBR) ?? "R$ 0,00" ,
-                    custoMaximoFormatado = custoMaximo?.ToString("C" , culturaBR) ?? "R$ 0,00" ,
-                    dataConsulta = DateTime.Now ,
-                });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "ViagemController", "ObterTotalCustoViagensEvento");
+                return await Task.FromResult(
+                    Ok(
+                        new
+                        {
+                            success = true ,
+                            eventoId = Id ,
+                            nomeEvento = evento?.Nome ,
+                            totalViagens = estatisticas.TotalViagens ,
+                            totalViagensComCusto = estatisticas.ViagensComCusto.Count ,
+                            viagensSemCusto = estatisticas.ViagensSemCusto.Count ,
+                            percentualComCusto = estatisticas.TotalViagens > 0
+                                ? (
+                                    estatisticas.ViagensComCusto.Count
+                                    * 100.0
+                                    / estatisticas.TotalViagens
+                                )
+                                : 0 ,
+                            totalCusto = custoTotal ,
+                            custoMedio = custoMedio ,
+                            custoMinimo = custoMinimo ?? 0 ,
+                            custoMaximo = custoMaximo ?? 0 ,
+                            totalCustoFormatado = custoTotal.ToString("C" , culturaBR) ,
+                            custoMedioFormatado = custoMedio.ToString("C" , culturaBR) ,
+                            custoMinimoFormatado = custoMinimo?.ToString("C" , culturaBR)
+                                ?? "R$ 0,00" ,
+                            custoMaximoFormatado = custoMaximo?.ToString("C" , culturaBR)
+                                ?? "R$ 0,00" ,
+                            dataConsulta = DateTime.Now ,
+                        }
+                    )
+                );
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterTotalCustoViagensEvento" , error);
-                return StatusCode(500 , new { success = false , message = "Erro ao processar a solicitação" , error = error.Message });
+                var errorDetails = new
+                {
+                    Message = error.Message ,
+                    StackTrace = error.StackTrace ,
+                    InnerException = error.InnerException?.Message ,
+                };
+
+                return await Task.FromResult(
+                    StatusCode(
+                        500 ,
+                        new
+                        {
+                            success = false ,
+                            message = "Erro ao processar a solicitação" ,
+                            error = errorDetails.Message ,
+                        }
+                    )
+                );
             }
         }
 
         [Route("EstatisticasViagensEvento")]
         [HttpGet]
+
         public IActionResult EstatisticasViagensEvento(Guid Id)
         {
             try
             {
-                var viagens = _unitOfWork.ViewViagens.GetAll(filter: x => x.EventoId == Id && x.Status == "Realizada").ToList();
+                var viagens = _unitOfWork
+                    .ViewViagens.GetAll(filter: x => x.EventoId == Id && x.Status == "Realizada")
+                    .ToList();
 
                 var estatisticas = new
                 {
                     success = true ,
                     totalViagens = viagens.Count() ,
-                    viagensComCusto = viagens.Count(v => v.CustoViagem.HasValue && v.CustoViagem > 0) ,
-                    viagensSemCusto = viagens.Count(v => !v.CustoViagem.HasValue || v.CustoViagem == 0) ,
-                    custoTotal = viagens.Where(v => v.CustoViagem.HasValue).Sum(v => v.CustoViagem.Value) ,
-                    custoMedio = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).Select(v => v.CustoViagem.Value).DefaultIfEmpty(0).Average() ,
-                    custoMinimo = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).Select(v => v.CustoViagem.Value).DefaultIfEmpty(0).Min() ,
-                    custoMaximo = viagens.Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0).Select(v => v.CustoViagem.Value).DefaultIfEmpty(0).Max() ,
-                    motoristas = viagens.Where(v => !string.IsNullOrEmpty(v.NomeMotorista)).GroupBy(v => v.NomeMotorista).Select(g => new { nome = g.Key , viagens = g.Count() , custoTotal = g.Sum(v => v.CustoViagem ?? 0) , }).OrderByDescending(m => m.viagens).ToList() ,
+                    viagensComCusto = viagens.Count(v =>
+                        v.CustoViagem.HasValue && v.CustoViagem > 0
+                    ) ,
+                    viagensSemCusto = viagens.Count(v =>
+                        !v.CustoViagem.HasValue || v.CustoViagem == 0
+                    ) ,
+                    custoTotal = viagens
+                        .Where(v => v.CustoViagem.HasValue)
+                        .Sum(v => v.CustoViagem.Value) ,
+                    custoMedio = viagens
+                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
+                        .Select(v => v.CustoViagem.Value)
+                        .DefaultIfEmpty(0)
+                        .Average() ,
+                    custoMinimo = viagens
+                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
+                        .Select(v => v.CustoViagem.Value)
+                        .DefaultIfEmpty(0)
+                        .Min() ,
+                    custoMaximo = viagens
+                        .Where(v => v.CustoViagem.HasValue && v.CustoViagem > 0)
+                        .Select(v => v.CustoViagem.Value)
+                        .DefaultIfEmpty(0)
+                        .Max() ,
+                    motoristas = viagens
+                        .Where(v => !string.IsNullOrEmpty(v.NomeMotorista))
+                        .GroupBy(v => v.NomeMotorista)
+                        .Select(g => new
+                        {
+                            nome = g.Key ,
+                            viagens = g.Count() ,
+                            custoTotal = g.Sum(v => v.CustoViagem ?? 0) ,
+                        })
+                        .OrderByDescending(m => m.viagens)
+                        .ToList() ,
                 };
 
                 return Ok(estatisticas);
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "EstatisticasViagensEvento");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "EstatisticasViagensEvento" , error);
-                return Ok(new { success = false , error = error.Message });
+                return Ok(new
+                {
+                    success = false ,
+                    error = error.Message
+                });
             }
         }
 
         [HttpPost]
+
         public IActionResult SaveImage(IList<IFormFile> UploadFiles)
         {
             try
             {
-                if (UploadFiles == null || UploadFiles.Count == 0 || UploadFiles[0] == null)
+                if (UploadFiles == null || UploadFiles.Count == 0)
                 {
                     return BadRequest("Nenhum arquivo foi enviado");
                 }
 
                 foreach (IFormFile file in UploadFiles)
                 {
-                    if (file?.Length > 0)
-                    {
-                        string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
+                    if (file.Length > 0)
+                    {
+                        string filename = ContentDispositionHeaderValue
+                            .Parse(file.ContentDisposition)
+                            .FileName.Trim('"');
+
                         filename = Path.GetFileName(filename);
-                        string folderPath = Path.Combine(hostingEnv.WebRootPath , "DadosEditaveis" , "ImagensViagens");
-
-                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
+
+                        string folderPath = Path.Combine(
+                            hostingEnv.WebRootPath ,
+                            "DadosEditaveis" ,
+                            "ImagensViagens"
+                        );
+
+                        if (!Directory.Exists(folderPath))
+                        {
+                            Directory.CreateDirectory(folderPath);
+                        }
 
                         string fullPath = Path.Combine(folderPath , filename);
+
                         using (var stream = new FileStream(fullPath , FileMode.Create))
                         {
                             file.CopyTo(stream);
                         }
-                        _log.Info($"Arquivo salvo: {filename}", "ViagemController", "SaveImage");
                     }
                 }
 
-                return Ok(new { message = "Imagem(ns) salva(s) com sucesso!" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "ViagemController", "SaveImage");
+                return Ok(new
+                {
+                    message = "Imagem(ns) salva(s) com sucesso!"
+                });
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "SaveImage" , error);
                 return StatusCode(500);
             }
@@ -2345,42 +2399,68 @@
 
         [HttpGet]
         [Route("EstatisticasVeiculo")]
+
         public async Task<IActionResult> GetEstatisticasVeiculo([FromQuery] Guid veiculoId)
         {
             try
             {
                 if (veiculoId == Guid.Empty)
                 {
-                    return Json(new { success = false , message = "VeiculoId é obrigatório" });
+                    return Json(new
+                    {
+                        success = false,
+                        message = "VeiculoId é obrigatório"
+                    });
                 }
 
                 var estatisticas = await _veiculoEstatisticaService.ObterEstatisticasAsync(veiculoId);
-                return Json(new { success = true , data = estatisticas });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "ViagemController", "GetEstatisticasVeiculo");
-                Alerta.TratamentoErroComLinha("ViagemController.cs" , "GetEstatisticasVeiculo" , error);
-                return Json(new { success = false , message = "Erro ao obter estatísticas do veículo" });
+
+                return Json(new
+                {
+                    success = true,
+                    data = estatisticas
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemController.cs", "GetEstatisticasVeiculo", error);
+                return Json(new
+                {
+                    success = false,
+                    message = "Erro ao obter estatísticas do veículo"
+                });
             }
         }
 
         [HttpPost]
         [Route("FinalizaViagem")]
         [Consumes("application/json")]
+
         public async Task<IActionResult> FinalizaViagemAsync([FromBody] FinalizacaoViagem viagem)
         {
             try
             {
+
                 if (viagem.DataFinal.HasValue && viagem.DataFinal.Value.Date > DateTime.Today)
                 {
-                    return Json(new { success = false , message = "A Data Final não pode ser superior à data atual." });
-                }
-
-                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == viagem.ViagemId);
+                    return Json(new
+                    {
+                        success = false ,
+                        message = "A Data Final não pode ser superior à data atual."
+                    });
+                }
+
+                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
+                    v.ViagemId == viagem.ViagemId
+                );
+
                 if (objViagem == null)
                 {
-                    return Json(new { success = false , message = "Viagem não encontrada" });
+                    return Json(new
+                    {
+                        success = false ,
+                        message = "Viagem não encontrada"
+                    });
                 }
 
                 objViagem.DataFinal = viagem.DataFinal;
@@ -2392,8 +2472,8 @@
                 objViagem.StatusDocumento = viagem.StatusDocumento;
                 objViagem.StatusCartaoAbastecimento = viagem.StatusCartaoAbastecimento;
 
-                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
-                var currentUserID = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
+                ClaimsPrincipal currentUser = this.User;
+                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                 var currentUserName = currentUser.Identity?.Name ?? "Sistema";
                 objViagem.UsuarioIdFinalizacao = currentUserID;
                 objViagem.DataFinalizacao = DateTime.Now;
@@ -2405,6 +2485,7 @@
                 {
                     foreach (var ocDto in viagem.Ocorrencias)
                     {
+
                         if (!string.IsNullOrWhiteSpace(ocDto.Resumo))
                         {
                             var ocorrencia = new OcorrenciaViagem
@@ -2426,7 +2507,10 @@
                     }
                 }
 
-                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == objViagem.VeiculoId);
+                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
+                    v.VeiculoId == objViagem.VeiculoId
+                );
+
                 if (veiculo != null)
                 {
                     veiculo.Quilometragem = viagem.KmFinal;
@@ -2437,24 +2521,37 @@
 
                 await _viagemEstatisticaService.AtualizarEstatisticasDiaAsync((DateTime)objViagem.DataInicial);
 
-                _log.Info($"Viagem finalizada. ID: {objViagem.ViagemId}. Ocorrências: {ocorrenciasCriadas}", "ViagemController", "FinalizaViagem");
-
                 var mensagem = "Viagem finalizada com sucesso";
-                if (ocorrenciasCriadas > 0) mensagem += $" ({ocorrenciasCriadas} ocorrência(s) registrada(s))";
-
-                return Json(new { success = true , message = mensagem , type = 0 , ocorrenciasCriadas = ocorrenciasCriadas });
-            }
-            catch (Exception error)
-            {
-                _log.Error("Erro", error, "ViagemController", "FinalizaViagem");
+                if (ocorrenciasCriadas > 0)
+                {
+                    mensagem += $" ({ocorrenciasCriadas} ocorrência(s) registrada(s))";
+                }
+
+                return Json(
+                    new
+                    {
+                        success = true ,
+                        message = mensagem ,
+                        type = 0 ,
+                        ocorrenciasCriadas = ocorrenciasCriadas
+                    }
+                );
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "FinalizaViagem" , error);
-                return Json(new { success = false , message = "Erro ao finalizar viagem: " + error.Message });
+                return Json(new
+                {
+                    success = false ,
+                    message = "Erro ao finalizar viagem: " + error.Message
+                });
             }
         }
 
         [Route("AjustaViagem")]
         [HttpPost]
         [Consumes("application/json")]
+
         public IActionResult AjustaViagem([FromBody] AjusteViagem viagem)
         {
             try
@@ -2527,6 +2624,7 @@
 
         [HttpPost]
         [Route("FluxoServerSide")]
+
         public IActionResult FluxoServerSide()
         {
             try
@@ -2541,8 +2639,7 @@
 
                 var veiculoIdStr = Request.Form["veiculoId"].FirstOrDefault();
                 var motoristaIdStr = Request.Form["motoristaId"].FirstOrDefault();
-                var dataFluxoDeStr = Request.Form["dataFluxoDe"].FirstOrDefault();
-                var dataFluxoAteStr = Request.Form["dataFluxoAte"].FirstOrDefault();
+                var dataFluxoStr = Request.Form["dataFluxo"].FirstOrDefault();
 
                 var query = _unitOfWork.ViewFluxoEconomildo.GetAll().AsQueryable();
 
@@ -2561,19 +2658,11 @@
                     query = query.Where(v => v.MotoristaId == motGuid);
                 }
 
-                if (!string.IsNullOrEmpty(dataFluxoDeStr))
-                {
-                    if (DateTime.TryParse(dataFluxoDeStr , out var dataConvDe))
-                    {
-                        query = query.Where(v => v.Data.HasValue && v.Data.Value.Date >= dataConvDe.Date);
-                    }
-                }
-
-                if (!string.IsNullOrEmpty(dataFluxoAteStr))
-                {
-                    if (DateTime.TryParse(dataFluxoAteStr , out var dataConvAte))
-                    {
-                        query = query.Where(v => v.Data.HasValue && v.Data.Value.Date <= dataConvAte.Date);
+                if (!string.IsNullOrEmpty(dataFluxoStr))
+                {
+                    if (DateTime.TryParse(dataFluxoStr , out var dataConv))
+                    {
+                        query = query.Where(v => v.Data == dataConv);
                     }
                 }
 
@@ -2631,6 +2720,7 @@
         }
 
         [HttpGet("ObterDocumento")]
+
         public IActionResult ObterDocumento(Guid viagemId)
         {
             try
@@ -2647,7 +2737,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ObterDocumento");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterDocumento" , error);
                 return StatusCode(500);
             }
@@ -2655,6 +2744,7 @@
 
         [HttpGet]
         [Route("ObterDadosMobile")]
+
         public IActionResult ObterDadosMobile(Guid viagemId)
         {
             try
@@ -2741,7 +2831,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ObterDadosMobile");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterDadosMobile" , error);
                 return Json(new
                 {
@@ -2753,6 +2842,7 @@
 
         [HttpGet]
         [Route("ObterImagemOcorrencia")]
+
         public IActionResult ObterImagemOcorrencia(Guid ocorrenciaId)
         {
             try
@@ -2800,7 +2890,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ObterImagemOcorrencia");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterImagemOcorrencia" , error);
                 return Json(new
                 {
@@ -2812,6 +2901,7 @@
 
         [HttpGet]
         [Route("ObterOcorrenciasViagem")]
+
         public IActionResult ObterOcorrenciasViagem(Guid viagemId)
         {
             try
@@ -2849,7 +2939,6 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "ViagemController", "ObterOcorrenciasViagem");
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterOcorrenciasViagem" , error);
                 return Json(new
                 {
@@ -2879,7 +2968,10 @@
             };
         }
 
-        public int Order { get; set; }
+        public int Order
+        {
+            get; set;
+        }
 
         public void OnAuthorization(AuthorizationFilterContext context)
         {
```
