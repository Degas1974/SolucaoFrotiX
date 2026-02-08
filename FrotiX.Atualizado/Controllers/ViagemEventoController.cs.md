# Controllers/ViagemEventoController.cs

**Mudanca:** GRANDE | **+215** linhas | **-260** linhas

---

```diff
--- JANEIRO: Controllers/ViagemEventoController.cs
+++ ATUAL: Controllers/ViagemEventoController.cs
@@ -23,48 +23,49 @@
     public partial class ViagemEventoController : Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-        private readonly IWebHostEnvironment _hostingEnv;
+        private IWebHostEnvironment hostingEnv;
+        private readonly IWebHostEnvironment webHostEnvironment;
 
         public ViagemEventoController(
             IUnitOfWork unitOfWork,
             IWebHostEnvironment env,
-            ILogService log
+            IWebHostEnvironment webHostEnvironment
         )
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _hostingEnv = env;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemEventoController", ex);
-            }
-        }
-
-        [HttpGet]
+                hostingEnv = env;
+                this.webHostEnvironment = webHostEnvironment;
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "ViagemEventoController.cs",
+                    "ViagemEventoController",
+                    error
+                );
+            }
+        }
+
+        [HttpGet]
+
         public IActionResult Get(string Id)
         {
             try
             {
-
-                _log.Info("ViagemEventoController.Get: Solicitando lista de eventos básicos.");
                 return Json(
                     new
                     {
-
                         data = _unitOfWork
                             .ViewViagens.GetAll()
                             .Where(v => v.Finalidade == "Evento" && v.StatusAgendamento == false),
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.Get", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Get", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Get", error);
                 return Json(new
                 {
                     success = false,
@@ -75,41 +76,38 @@
 
         [Route("ViagemEventos")]
         [HttpGet]
+
         public IActionResult ViagemEventos()
         {
             try
             {
-
-                _log.Info("ViagemEventoController.ViagemEventos: Solicitando lista de eventos agendados.");
                 return Json(
                     new
                     {
-
                         data = _unitOfWork
                             .ViewViagens.GetAll()
                             .Where(v => v.Finalidade == "Evento" && v.StatusAgendamento == false),
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ViagemEventos", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemEventos", ex);
-                return Json(new
-                {
-                    success = false,
-                    message = "Erro ao carregar dados"
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemEventos", error);
+                return Json(new
+                {
+                    success = false,
+                    message = "Erro ao carregar eventos"
                 });
             }
         }
 
         [Route("Fluxo")]
         [HttpGet]
+
         public IActionResult Fluxo()
         {
             try
             {
-                _log.Info("ViagemEventoController.Fluxo: Listando fluxo geral Economildo.");
                 var result = (
                     from vf in _unitOfWork.ViewFluxoEconomildo.GetAll()
                     select new
@@ -132,10 +130,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.Fluxo", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Fluxo", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Fluxo", error);
                 return Json(new
                 {
                     success = false,
@@ -146,12 +143,11 @@
 
         [Route("FluxoVeiculos")]
         [HttpGet]
+
         public IActionResult FluxoVeiculos(string Id)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.FluxoVeiculos: Filtrando fluxo para Veículo {Id}");
                 var result = (
                     from vf in _unitOfWork.ViewFluxoEconomildo.GetAll()
                     where vf.VeiculoId == Guid.Parse(Id)
@@ -175,10 +171,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.FluxoVeiculos", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoVeiculos", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoVeiculos", error);
                 return Json(new
                 {
                     success = false,
@@ -189,12 +184,11 @@
 
         [Route("FluxoMotoristas")]
         [HttpGet]
+
         public IActionResult FluxoMotoristas(string Id)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.FluxoMotoristas: Filtrando fluxo para Motorista {Id}");
                 var result = (
                     from vf in _unitOfWork.ViewFluxoEconomildo.GetAll()
                     where vf.MotoristaId == Guid.Parse(Id)
@@ -218,10 +212,9 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.FluxoMotoristas", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoMotoristas", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoMotoristas", error);
                 return Json(new
                 {
                     success = false,
@@ -232,12 +225,11 @@
 
         [Route("FluxoData")]
         [HttpGet]
+
         public IActionResult FluxoData(string Id)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.FluxoData: Filtrando fluxo para Data {Id}");
                 var dataFluxo = DateTime.Parse(Id);
 
                 var result = (
@@ -263,32 +255,29 @@
                     data = result
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.FluxoData", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoData", ex);
-                return Json(new
-                {
-                    success = false,
-                    message = "Erro ao carregar fluxo de data"
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoData", error);
+                return Json(new
+                {
+                    success = false,
+                    message = "Erro ao carregar fluxo por data"
                 });
             }
         }
 
         [Route("ApagaFluxoEconomildo")]
         [HttpPost]
+
         public IActionResult ApagaFluxoEconomildo(ViagensEconomildo viagensEconomildo)
         {
             try
             {
-
                 var objFromDb = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(v =>
                     v.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId
                 );
-
                 _unitOfWork.ViagensEconomildo.Remove(objFromDb);
                 _unitOfWork.Save();
-
                 return Json(new
                 {
                     success = true,
@@ -308,13 +297,13 @@
 
         [Route("MyUploader")]
         [HttpPost]
+
         public IActionResult MyUploader(IFormFile MyUploader, [FromForm] string ViagemId)
         {
             try
             {
                 if (MyUploader != null)
                 {
-
                     var viagemObj = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                         v.ViagemId == Guid.Parse(ViagemId)
                     );
@@ -349,12 +338,11 @@
 
         [Route("CalculaCustoViagens")]
         [HttpPost]
+
         public IActionResult CalculaCustoViagens()
         {
             try
             {
-
-                _log.Info("ViagemEventoController.CalculaCustoViagens: Iniciando recálculo de custos para viagens realizadas.");
                 var objViagens = _unitOfWork.Viagem.GetAll(v =>
                     v.StatusAgendamento == false
                     && v.Status == "Realizada"
@@ -370,7 +358,6 @@
 
                 foreach (var viagem in objViagens)
                 {
-
                     if (viagem.MotoristaId != null)
                     {
                         int minutos = -1;
@@ -381,7 +368,6 @@
                         );
                         viagem.Minutos = minutos;
                     }
-
                     if (viagem.VeiculoId != null)
                     {
                         viagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(viagem, _unitOfWork);
@@ -393,17 +379,15 @@
                 }
 
                 _unitOfWork.Save();
-                _log.Info($"ViagemEventoController.CalculaCustoViagens: Sucesso no recálculo ({objViagens.Count()} registros).");
 
                 return Json(new
                 {
                     success = true
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.CalculaCustoViagens", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "CalculaCustoViagens", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "CalculaCustoViagens", error);
                 return Json(new
                 {
                     success = false,
@@ -414,26 +398,23 @@
 
         [Route("ViagemVeiculos")]
         [HttpGet]
+
         public IActionResult ViagemVeiculos(Guid Id)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.ViagemVeiculos: Obtendo viagens para Veículo {Id}");
                 return Json(
                     new
                     {
-
                         data = _unitOfWork
                             .ViewViagens.GetAll()
                             .Where(vv => vv.VeiculoId == Id && vv.StatusAgendamento == false),
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ViagemVeiculos", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemVeiculos", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemVeiculos", error);
                 return Json(new
                 {
                     success = false,
@@ -444,26 +425,23 @@
 
         [Route("ViagemMotoristas")]
         [HttpGet]
+
         public IActionResult ViagemMotoristas(Guid Id)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.ViagemMotoristas: Obtendo viagens para Motorista {Id}");
                 return Json(
                     new
                     {
-
                         data = _unitOfWork
                             .ViewViagens.GetAll()
                             .Where(vv => vv.MotoristaId == Id && vv.StatusAgendamento == false),
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ViagemMotoristas", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemMotoristas", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemMotoristas", error);
                 return Json(new
                 {
                     success = false,
@@ -474,26 +452,23 @@
 
         [Route("ViagemStatus")]
         [HttpGet]
+
         public IActionResult ViagemStatus(string Id)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.ViagemStatus: Filtrando por status {Id}");
                 return Json(
                     new
                     {
-
                         data = _unitOfWork
                             .ViewViagens.GetAll()
                             .Where(vv => vv.Status == Id && vv.StatusAgendamento == false),
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ViagemStatus", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemStatus", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemStatus", error);
                 return Json(new
                 {
                     success = false,
@@ -504,26 +479,23 @@
 
         [Route("ViagemSetores")]
         [HttpGet]
+
         public IActionResult ViagemSetores(Guid Id)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.ViagemSetores: Filtrando por Setor {Id}");
                 return Json(
                     new
                     {
-
                         data = _unitOfWork
                             .ViewViagens.GetAll()
                             .Where(vv => vv.SetorSolicitanteId == Id && vv.StatusAgendamento == false),
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ViagemSetores", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemSetores", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemSetores", error);
                 return Json(new
                 {
                     success = false,
@@ -534,19 +506,16 @@
 
         [Route("ViagemData")]
         [HttpGet]
+
         public IActionResult ViagemData(string Id)
         {
             try
             {
-
                 if (DateTime.TryParse(Id, out DateTime parsedDate))
                 {
-
-                    _log.Info($"ViagemEventoController.ViagemData: Filtrando por Data {parsedDate:dd/MM/yyyy}");
                     return Json(
                         new
                         {
-
                             data = _unitOfWork
                                 .ViewViagens.GetAll()
                                 .Where(vv =>
@@ -561,10 +530,9 @@
                     message = "Data inválida fornecida."
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ViagemData", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemData", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemData", error);
                 return Json(new
                 {
                     success = false,
@@ -575,16 +543,14 @@
 
         [Route("Ocorrencias")]
         [HttpGet]
+
         public IActionResult Ocorrencias(Guid Id)
         {
             try
             {
-
-                _log.Info("ViagemEventoController.Ocorrencias: Listando viagens com ocorrências registradas.");
                 return Json(
                     new
                     {
-
                         data = _unitOfWork
                             .ViewViagens.GetAll()
                             .Where(vv =>
@@ -593,10 +559,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.Ocorrencias", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Ocorrencias", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Ocorrencias", error);
                 return Json(new
                 {
                     success = false,
@@ -607,21 +572,17 @@
 
         [Route("Cancelar")]
         [HttpPost]
+
         public IActionResult Cancelar(ViagemID id)
         {
             try
             {
-
                 var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id.ViagemId);
                 if (objFromDb != null)
                 {
-
                     objFromDb.Status = "Cancelada";
                     _unitOfWork.Viagem.Update(objFromDb);
                     _unitOfWork.Save();
-
-                    _log.Info($"ViagemEventoController.Cancelar: Viagem {id.ViagemId} cancelada com sucesso.");
-
                     return Json(new
                     {
                         success = true,
@@ -631,13 +592,12 @@
                 return Json(new
                 {
                     success = false,
-                    message = "Erro ao cancelar Viagem: Registro não encontrado"
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.Cancelar", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Cancelar", ex);
+                    message = "Erro ao cancelar Viagem"
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Cancelar", error);
                 return Json(new
                 {
                     success = false,
@@ -648,6 +608,7 @@
 
         [HttpGet]
         [Route("PegaFicha")]
+
         public JsonResult PegaFicha(Guid id)
         {
             try
@@ -669,36 +630,32 @@
                     return Json(false);
                 }
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.PegaFicha", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaFicha", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaFicha", error);
                 return Json(false);
             }
         }
 
         [Route("AdicionarViagensEconomildo")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarViagensEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
         {
             try
             {
-
                 _unitOfWork.ViagensEconomildo.Add(viagensEconomildo);
                 _unitOfWork.Save();
 
-                _log.Info($"ViagemEventoController.AdicionarViagensEconomildo: Sucesso ao adicionar registro Economildo.");
-
                 return Json(new
                 {
                     success = true,
                     message = "Viagem Adicionada com Sucesso!"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.AdicionarViagensEconomildo", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarViagensEconomildo", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarViagensEconomildo", error);
                 return Json(new
                 {
                     success = false,
@@ -709,11 +666,11 @@
 
         [Route("ExisteDataEconomildo")]
         [Consumes("application/json")]
+
         public JsonResult ExisteDataEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
         {
             try
             {
-
                 if (viagensEconomildo.Data != null)
                 {
                     var existeData = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(u =>
@@ -740,10 +697,9 @@
                     message = ""
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ExisteDataEconomildo", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ExisteDataEconomildo", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ExisteDataEconomildo", error);
                 return Json(new
                 {
                     success = false,
@@ -754,11 +710,11 @@
 
         [HttpGet]
         [Route("PegaFichaModal")]
+
         public JsonResult PegaFichaModal(Guid id)
         {
             try
             {
-
                 var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
                 if (objFromDb.FichaVistoria != null)
                 {
@@ -769,16 +725,16 @@
                 }
                 return Json(false);
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.PegaFichaModal", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaFichaModal", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaFichaModal", error);
                 return Json(false);
             }
         }
 
         [HttpGet]
         [Route("PegaCategoria")]
+
         public JsonResult PegaCategoria(Guid id)
         {
             try
@@ -790,10 +746,9 @@
                 }
                 return Json(false);
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.PegaCategoria", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaCategoria", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaCategoria", error);
                 return Json(false);
             }
         }
@@ -810,6 +765,7 @@
 
         [Route("AdicionarEvento")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarEvento([FromBody] Evento evento)
         {
             try
@@ -826,7 +782,6 @@
 
                 _unitOfWork.Evento.Add(evento);
                 _unitOfWork.Save();
-                _log.Info($"ViagemEventoController.AdicionarEvento: Novo evento '{evento.Nome}' cadastrado via modal.");
 
                 return Json(
                     new
@@ -837,10 +792,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.AdicionarEvento", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarEvento", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarEvento", error);
                 return Json(new
                 {
                     success = false,
@@ -851,11 +805,11 @@
 
         [Route("AdicionarRequisitante")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarRequisitante([FromBody] Requisitante requisitante)
         {
             try
             {
-
                 var existeRequisitante = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                     (u.Ponto == requisitante.Ponto) || (u.Nome == requisitante.Nome)
                 );
@@ -879,7 +833,6 @@
 
                 _unitOfWork.Requisitante.Add(requisitante);
                 _unitOfWork.Save();
-                _log.Info($"ViagemEventoController.AdicionarRequisitante: Novo requisitante '{requisitante.Nome}' cadastrado via modal.");
 
                 return Json(
                     new
@@ -890,10 +843,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.AdicionarRequisitante", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarRequisitante", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarRequisitante", error);
                 return Json(new
                 {
                     success = false,
@@ -904,11 +856,11 @@
 
         [Route("AdicionarSetor")]
         [Consumes("application/json")]
+
         public JsonResult AdicionarSetor([FromBody] SetorSolicitante setorSolicitante)
         {
             try
             {
-
                 if (setorSolicitante.Sigla != null)
                 {
                     var existeSigla = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
@@ -961,7 +913,6 @@
 
                 _unitOfWork.SetorSolicitante.Add(setorSolicitante);
                 _unitOfWork.Save();
-                _log.Info($"ViagemEventoController.AdicionarSetor: Novo setor '{setorSolicitante.Nome}' cadastrado via modal.");
 
                 return Json(
                     new
@@ -971,10 +922,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.AdicionarSetor", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarSetor", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarSetor", error);
                 return Json(new
                 {
                     success = false,
@@ -984,6 +934,7 @@
         }
 
         [Route("SaveImage")]
+
         public void SaveImage(IList<IFormFile> UploadFiles)
         {
             try
@@ -999,7 +950,7 @@
                         filename = Path.GetFileName(filename);
 
                         string folderPath = Path.Combine(
-                            _hostingEnv.WebRootPath,
+                            hostingEnv.WebRootPath,
                             "DadosEditaveis",
                             "ImagensViagens"
                         );
@@ -1018,28 +969,25 @@
                                 file.CopyTo(fs);
                                 fs.Flush();
                             }
-                            _log.Info($"ViagemEventoController.SaveImage: Arquivo '{filename}' salvo com sucesso.");
                             Response.StatusCode = 200;
                         }
                     }
                 }
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.SaveImage", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "SaveImage", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "SaveImage", error);
                 Response.StatusCode = 204;
             }
         }
 
         [Route("FinalizaViagem")]
         [Consumes("application/json")]
+
         public IActionResult FinalizaViagem([FromBody] FinalizacaoViagem viagem)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.FinalizaViagem: Processando finalização da viagem {viagem.ViagemId}.");
                 var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                     v.ViagemId == viagem.ViagemId
                 );
@@ -1067,7 +1015,6 @@
                 _unitOfWork.Veiculo.Update(veiculo);
 
                 _unitOfWork.Save();
-                _log.Info($"ViagemEventoController.FinalizaViagem: Viagem {viagem.ViagemId} finalizada com sucesso. Km Final: {viagem.KmFinal}.");
 
                 return Json(
                     new
@@ -1078,10 +1025,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.FinalizaViagem", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FinalizaViagem", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FinalizaViagem", error);
                 return Json(new
                 {
                     success = false,
@@ -1092,12 +1038,11 @@
 
         [Route("AjustaViagem")]
         [Consumes("application/json")]
+
         public IActionResult AjustaViagem([FromBody] AjusteViagem viagem)
         {
             try
             {
-
-                _log.Info($"ViagemEventoController.AjustaViagem: Solicitado ajuste para a viagem {viagem.ViagemId}.");
                 var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                     v.ViagemId == viagem.ViagemId
                 );
@@ -1128,9 +1073,8 @@
                 objViagem.CustoLavador = 0;
 
                 _unitOfWork.Viagem.Update(objViagem);
+
                 _unitOfWork.Save();
-
-                _log.Info($"ViagemEventoController.AjustaViagem: Ajustes aplicados com sucesso na viagem {viagem.ViagemId}.");
 
                 return Json(
                     new
@@ -1141,10 +1085,9 @@
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.AjustaViagem", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AjustaViagem", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AjustaViagem", error);
                 return Json(new
                 {
                     success = false,
@@ -1191,13 +1134,25 @@
             }
         }
 
+        public class Objfile
+        {
+            public string file
+            {
+                get; set;
+            }
+            public string viagemid
+            {
+                get; set;
+            }
+        }
+
         [Route("ObterPorId")]
         [HttpGet]
+
         public IActionResult ObterPorId(Guid id)
         {
             try
             {
-
                 if (id == Guid.Empty)
                 {
                     return Json(new
@@ -1235,10 +1190,9 @@
                     }
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ObterPorId", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ObterPorId", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ObterPorId", error);
                 return Json(new
                 {
                     success = false,
@@ -1250,49 +1204,47 @@
         [Route("FileUpload")]
         [HttpPost]
         [RequestSizeLimit(valueCountLimit: 1999483648)]
+
         public JsonResult FileUpload(Objfile objFile)
         {
             try
             {
-
-                if (string.IsNullOrEmpty(objFile.viagemid))
+                if (objFile.viagemid == "")
                 {
                     return Json(false);
                 }
 
-                _log.Info($"ViagemEventoController.FileUpload: Recebido arquivo para a viagem {objFile.viagemid}.");
+                String viagemid = objFile.viagemid;
                 var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
-                    v.ViagemId == Guid.Parse(objFile.viagemid)
-                );
-
-                if (objViagem == null) return Json(false);
+                    v.ViagemId == Guid.Parse(viagemid)
+                );
+
+                string base64 = objFile.file;
+                int tamanho = objFile.file.Length;
 
                 _unitOfWork.Viagem.Update(objViagem);
-                _unitOfWork.Save();
-
-                return Json(objFile.viagemid);
-            }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.FileUpload", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FileUpload", ex);
+
+                return Json(viagemid);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FileUpload", error);
                 return Json(false);
             }
         }
 
         [Route("ObterDetalhamentoCustosViagem")]
         [HttpGet("ObterDetalhamentoCustosViagem")]
+
         public async Task<IActionResult> ObterDetalhamentoCustosViagem(Guid viagemId)
         {
             try
             {
 
-                _log.Info($"ViagemEventoController.ObterDetalhamentoCustosViagem: Solicitado detalhamento para id {viagemId}.");
                 var viagem = await _unitOfWork.Viagem.GetFirstOrDefaultAsync(
-                    v => v.ViagemId == viagemId,
+                    v => v.ViagemId == viagemId ,
                     includeProperties: "Requisitante,Motorista,Veiculo"
                 );
-
                 if (viagem == null)
                 {
                     return Json(new { success = false, message = "Viagem não encontrada" });
@@ -1302,8 +1254,10 @@
                 if (viagem.DataInicial.HasValue && viagem.HoraInicio.HasValue &&
                     viagem.DataFinal.HasValue && viagem.HoraFim.HasValue)
                 {
+
                     var dataHoraInicio = viagem.DataInicial.Value.Date + viagem.HoraInicio.Value.TimeOfDay;
                     var dataHoraFim = viagem.DataFinal.Value.Date + viagem.HoraFim.Value.TimeOfDay;
+
                     var diferenca = dataHoraFim - dataHoraInicio;
                     tempoTotalHoras = diferenca.TotalHours;
                 }
@@ -1333,124 +1287,115 @@
             }
             catch (Exception ex)
             {
-                _log.Error("ViagemEventoController.ObterDetalhamentoCustosViagem", ex);
-                return Json(new
-                {
+                return Json(new {
                     success = false,
                     message = $"Erro ao obter detalhamento: {ex.Message}"
                 });
             }
         }
 
-        public class Objfile
-        {
-            public string file { get; set; }
-            public string viagemid { get; set; }
-        }
-
         [Route("ObterDetalhamentoCustos")]
         [HttpGet]
+
         public IActionResult ObterDetalhamentoCustos(Guid eventoId)
         {
-            try
-            {
-
-                if (eventoId == Guid.Empty)
-                {
-                    return Json(new
-                    {
-                        success = false,
-                        message = "ID do evento inválido"
-                    });
-                }
-
-                var viagens = _unitOfWork.Viagem
-                    .GetAll()
-                    .Where(v => v.EventoId == eventoId)
-                    .ToList();
-
-                if (!viagens.Any())
-                {
-                    return Json(new
-                    {
-                        success = false,
-                        message = "Nenhuma viagem encontrada para este evento"
-                    });
-                }
-
-                double tempoTotalHoras = 0;
-                DateTime? primeiraDataInicial = null;
-                DateTime? ultimaDataFinal = null;
-
-                foreach (var viagem in viagens)
-                {
-                    if (viagem.DataInicial.HasValue && viagem.DataFinal.HasValue)
-                    {
-
-                        if (!primeiraDataInicial.HasValue || viagem.DataInicial.Value < primeiraDataInicial.Value)
-                        {
-                            primeiraDataInicial = viagem.DataInicial.Value;
-                        }
-
-                        if (!ultimaDataFinal.HasValue || viagem.DataFinal.Value > ultimaDataFinal.Value)
-                        {
-                            ultimaDataFinal = viagem.DataFinal.Value;
-                        }
-
-                        var dataHoraInicial = viagem.DataInicial.Value.Date;
-                        var dataHoraFinal = viagem.DataFinal.Value.Date;
-
-                        if (viagem.HoraInicio.HasValue)
-                        {
-                            dataHoraInicial = dataHoraInicial.Add(viagem.HoraInicio.Value.TimeOfDay);
-                        }
-
-                        if (viagem.HoraFim.HasValue)
-                        {
-                            dataHoraFinal = dataHoraFinal.Add(viagem.HoraFim.Value.TimeOfDay);
-                        }
-
-                        var duracao = dataHoraFinal - dataHoraInicial;
-                        tempoTotalHoras += duracao.TotalHours;
-                    }
-                }
-
-                var custoMotorista = viagens.Sum(v => v.CustoMotorista ?? 0);
-                var custoVeiculo = viagens.Sum(v => v.CustoVeiculo ?? 0);
-                var custoCombustivel = viagens.Sum(v => v.CustoCombustivel ?? 0);
-
-                return Json(new
-                {
-                    success = true,
-                    data = new
-                    {
-                        TempoTotalHoras = Math.Round(tempoTotalHoras, 2),
-                        DataInicial = primeiraDataInicial,
-                        HoraInicial = viagens.Where(v => v.DataInicial == primeiraDataInicial)
-                                             .Select(v => v.HoraInicio)
-                                             .FirstOrDefault(),
-                        DataFinal = ultimaDataFinal,
-                        HoraFinal = viagens.Where(v => v.DataFinal == ultimaDataFinal)
-                                           .Select(v => v.HoraFim)
-                                           .FirstOrDefault(),
-                        CustoMotorista = custoMotorista,
-                        CustoVeiculo = custoVeiculo,
-                        CustoCombustivel = custoCombustivel,
-                        CustoTotal = custoMotorista + custoVeiculo + custoCombustivel,
-                        QuantidadeViagens = viagens.Count
-                    }
-                });
-            }
-            catch (Exception ex)
-            {
-                _log.Error("ViagemEventoController.ObterDetalhamentoCustos", ex);
-                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ObterDetalhamentoCustos", ex);
-                return Json(new
-                {
-                    success = false,
-                    message = "Erro ao buscar detalhamento de custos"
-                });
-            }
+        try
+        {
+        if (eventoId == Guid.Empty)
+        {
+        return Json(new
+        {
+            success = false ,
+            message = "ID do evento inválido"
+        });
+        }
+
+        var viagens = _unitOfWork.Viagem
+            .GetAll()
+            .Where(v => v.EventoId == eventoId)
+            .ToList();
+
+        if (!viagens.Any())
+        {
+        return Json(new
+        {
+            success = false ,
+            message = "Nenhuma viagem encontrada para este evento"
+        });
+        }
+
+        double tempoTotalHoras = 0;
+        DateTime? primeiraDataInicial = null;
+        DateTime? ultimaDataFinal = null;
+
+        foreach (var viagem in viagens)
+        {
+        if (viagem.DataInicial.HasValue && viagem.DataFinal.HasValue)
+        {
+
+        if (!primeiraDataInicial.HasValue || viagem.DataInicial.Value < primeiraDataInicial.Value)
+        {
+        primeiraDataInicial = viagem.DataInicial.Value;
+        }
+
+        if (!ultimaDataFinal.HasValue || viagem.DataFinal.Value > ultimaDataFinal.Value)
+        {
+        ultimaDataFinal = viagem.DataFinal.Value;
+        }
+
+        var dataHoraInicial = viagem.DataInicial.Value.Date;
+        var dataHoraFinal = viagem.DataFinal.Value.Date;
+
+        if (viagem.HoraInicio.HasValue)
+        {
+            dataHoraInicial = dataHoraInicial.Add(viagem.HoraInicio.Value.TimeOfDay);
+        }
+
+        if (viagem.HoraFim.HasValue)
+        {
+            dataHoraFinal = dataHoraFinal.Add(viagem.HoraFim.Value.TimeOfDay);
+        }
+
+        var duracao = dataHoraFinal - dataHoraInicial;
+        tempoTotalHoras += duracao.TotalHours;
+        }
+        }
+
+        var custoMotorista = viagens.Sum(v => v.CustoMotorista ?? 0);
+        var custoVeiculo = viagens.Sum(v => v.CustoVeiculo ?? 0);
+        var custoCombustivel = viagens.Sum(v => v.CustoCombustivel ?? 0);
+
+        return Json(new
+        {
+            success = true ,
+            data = new
+            {
+                TempoTotalHoras = Math.Round(tempoTotalHoras , 2) ,
+                DataInicial = primeiraDataInicial ,
+                HoraInicial = viagens.Where(v => v.DataInicial == primeiraDataInicial)
+                                     .Select(v => v.HoraInicio)
+                                     .FirstOrDefault() ,
+                DataFinal = ultimaDataFinal ,
+                HoraFinal = viagens.Where(v => v.DataFinal == ultimaDataFinal)
+                                   .Select(v => v.HoraFim)
+                                   .FirstOrDefault() ,
+                CustoMotorista = custoMotorista ,
+                CustoVeiculo = custoVeiculo ,
+                CustoCombustivel = custoCombustivel ,
+                CustoTotal = custoMotorista + custoVeiculo + custoCombustivel ,
+                QuantidadeViagens = viagens.Count
+            }
+        });
+        }
+        catch (Exception error)
+        {
+        Alerta.TratamentoErroComLinha("ViagemEventoController.cs" , "ObterDetalhamentoCustos" , error);
+        return Json(new
+        {
+            success = false ,
+            message = "Erro ao buscar detalhamento de custos"
+        });
+        }
         }
     }
 }
```
