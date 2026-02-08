# Controllers/AgendaController.cs

**Mudanca:** GRANDE | **+330** linhas | **-276** linhas

---

```diff
--- JANEIRO: Controllers/AgendaController.cs
+++ ATUAL: Controllers/AgendaController.cs
@@ -10,31 +10,28 @@
 using System;
 using System.Collections.Generic;
 using System.Linq;
+using System.Linq.Expressions;
 using System.Security.Claims;
 using System.Threading.Tasks;
-using FrotiX.Helpers;
 
 namespace FrotiX.Controllers
 {
-
-[Route("api/[controller]")]
+    [Route("api/[controller]")]
     [ApiController]
     public class AgendaController : ControllerBase
     {
         private readonly FrotiXDbContext _context;
-        private readonly ILogger<AgendaController> _logger;
+        private readonly ILogger<AbastecimentoController> _logger;
         private readonly IUnitOfWork _unitOfWork;
         private readonly IWebHostEnvironment _hostingEnvironment;
         private readonly ViagemEstatisticaService _viagemEstatisticaService;
-        private readonly ILogService _log;
 
         public AgendaController(
-            ILogger<AgendaController> logger,
-            IWebHostEnvironment hostingEnvironment,
-            IUnitOfWork unitOfWork,
-            FrotiXDbContext context,
-            IViagemEstatisticaRepository viagemEstatisticaRepository,
-            ILogService logService
+            ILogger<AbastecimentoController> logger ,
+            IWebHostEnvironment hostingEnvironment ,
+            IUnitOfWork unitOfWork ,
+            FrotiXDbContext context ,
+            IViagemEstatisticaRepository viagemEstatisticaRepository
         )
         {
             try
@@ -43,12 +40,11 @@
                 _hostingEnvironment = hostingEnvironment;
                 _unitOfWork = unitOfWork;
                 _context = context;
-                _viagemEstatisticaService = new ViagemEstatisticaService(context, viagemEstatisticaRepository, unitOfWork);
-                _log = logService;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "AgendaController", error);
+                _viagemEstatisticaService = new ViagemEstatisticaService(context , viagemEstatisticaRepository , unitOfWork);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "AgendaController" , error);
             }
         }
 
@@ -57,7 +53,6 @@
         {
             try
             {
-
                 _logger.LogInformation("[TesteView] Testando acesso à ViewViagensAgenda");
 
                 var count = _context.ViewViagensAgenda.Count();
@@ -83,15 +78,16 @@
                     mensagem = "Teste concluído com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao testar view de agenda", ex, "AgendaController.cs", "TesteView");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "TesteView", ex);
+            catch (Exception error)
+            {
+                _logger.LogError(error, "[TesteView] Erro ao testar view");
                 return StatusCode(500, new
                 {
                     sucesso = false,
                     mensagem = "Erro no teste",
-                    erro = ex.Message
+                    erro = error.Message,
+                    innerException = error.InnerException?.Message,
+                    stack = error.StackTrace
                 });
             }
         }
@@ -193,15 +189,16 @@
                     mensagem = "Diagnóstico concluído"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro no diagnóstico de agenda", ex, "AgendaController.cs", "DiagnosticoAgenda");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "DiagnosticoAgenda", ex);
+            catch (Exception error)
+            {
+                _logger.LogError(error, "[DiagnosticoAgenda] Erro no diagnóstico");
                 return StatusCode(500, new
                 {
                     sucesso = false,
                     mensagem = "Erro no diagnóstico",
-                    erro = ex.Message
+                    erro = error.Message,
+                    innerException = error.InnerException?.Message,
+                    stack = error.StackTrace
                 });
             }
         }
@@ -223,6 +220,20 @@
                     .Where(v => v.DataInicial.HasValue
                         && v.DataInicial >= startMenos3
                         && v.DataInicial < endMenos3)
+                    .Select(v => new
+                    {
+                        v.ViagemId ,
+                        v.Titulo ,
+                        v.Start ,
+                        v.DataInicial ,
+                        v.CorEvento ,
+                        v.CorTexto ,
+                        v.Descricao ,
+                        v.Placa ,
+                        v.NomeMotorista ,
+                        v.NomeEventoFull ,
+                        v.Finalidade
+                    })
                     .ToList();
 
                 _logger.LogInformation($"[TesteCarregaViagens] Registros encontrados: {viagensRaw.Count}");
@@ -230,7 +241,9 @@
                 var viagens = viagensRaw
                     .Select(v =>
                     {
+
                         var startDate = v.Start ?? v.DataInicial ?? DateTime.Now;
+
                         var endDate = startDate.AddHours(1);
 
                         return new
@@ -241,7 +254,9 @@
                             end = endDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                             backgroundColor = v.CorEvento ?? "#808080",
                             textColor = v.CorTexto ?? "#FFFFFF",
+
                             descricao = v.Descricao ?? "",
+
                             placa = v.Placa,
                             motorista = v.NomeMotorista,
                             evento = v.NomeEventoFull,
@@ -252,8 +267,7 @@
 
                 _logger.LogInformation($"[TesteCarregaViagens] Viagens processadas: {viagens.Count}");
 
-                return Ok(new
-                {
+                return Ok(new {
                     data = viagens,
                     debug = new
                     {
@@ -265,15 +279,16 @@
                     }
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao testar carregamento de viagens", ex, "AgendaController.cs", "TesteCarregaViagens");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "TesteCarregaViagens", ex);
+            catch (Exception error)
+            {
+                _logger.LogError(error, "[TesteCarregaViagens] ERRO");
                 return StatusCode(500, new
                 {
                     sucesso = false,
                     mensagem = "Erro no teste",
-                    erro = ex.Message
+                    erro = error.Message,
+                    innerException = error.InnerException?.Message,
+                    stack = error.StackTrace
                 });
             }
         }
@@ -289,18 +304,23 @@
                 }
 
                 var viagens = await _unitOfWork.Viagem.GetAllReducedIQueryable(
+
                     selector: v => new
                     {
-                        v.ViagemId,
-                        v.DataInicial,
-                        v.RecorrenciaViagemId,
+                        v.ViagemId ,
+                        v.DataInicial ,
+                        v.RecorrenciaViagemId ,
                         v.Status
-                    },
+                    } ,
+
                     filter: v =>
                         (v.RecorrenciaViagemId == id || v.ViagemId == id) &&
-                        v.Status != "Cancelada",
-                    orderBy: q => q.OrderBy(v => v.DataInicial),
-                    includeProperties: null,
+                        v.Status != "Cancelada" ,
+
+                    orderBy: q => q.OrderBy(v => v.DataInicial) ,
+
+                    includeProperties: null ,
+
                     asNoTracking: true
                 )
                 .Take(100)
@@ -310,18 +330,32 @@
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao buscar viagens da série de recorrência", ex, "AgendaController.cs", "BuscarViagensRecorrencia");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "BuscarViagensRecorrencia", ex);
-                return StatusCode(500, "Erro interno");
+                _logger.LogError(ex , "Erro ao buscar viagens recorrentes");
+                return StatusCode(500 , "Erro interno");
             }
         }
 
         public class ViagemRecorrenciaDto
         {
-            public Guid ViagemId { get; set; }
-            public DateTime DataInicial { get; set; }
-            public Guid? RecorrenciaViagemId { get; set; }
-            public string Status { get; set; }
+            public Guid ViagemId
+            {
+                get; set;
+            }
+
+            public DateTime DataInicial
+            {
+                get; set;
+            }
+
+            public Guid? RecorrenciaViagemId
+            {
+                get; set;
+            }
+
+            public string Status
+            {
+                get; set;
+            }
         }
 
         [HttpPost("Agendamento")]
@@ -340,7 +374,7 @@
                 {
                     return BadRequest(new
                     {
-                        success = false,
+                        success = false ,
                         message = "A Data Final não pode ser superior à data atual."
                     });
                 }
@@ -404,6 +438,7 @@
                     viagem.DatasSelecionadas = null;
 
                     viagem.StatusAgendamento = true;
+
                     viagem.FoiAgendamento = false;
 
                     var primeiraDataSelecionada = DatasSelecionadasAdicao.FirstOrDefault();
@@ -414,21 +449,21 @@
 
                         DateTime DataInicialCompleta;
                         DateTime.TryParseExact(
-                            (DataInicial + " " + HoraInicio),
-                            new string[] { "dd/MM/yyyy HH:mm" },
-                            System.Globalization.CultureInfo.InvariantCulture,
-                            System.Globalization.DateTimeStyles.None,
+                            (DataInicial + " " + HoraInicio) ,
+                            new string[] { "dd/MM/yyyy HH:mm" } ,
+                            System.Globalization.CultureInfo.InvariantCulture ,
+                            System.Globalization.DateTimeStyles.None ,
                             out DataInicialCompleta
                         );
 
-                        AtualizarDadosAgendamento(novaViagem, viagem);
+                        AtualizarDadosAgendamento(novaViagem , viagem);
                         novaViagem.DataInicial = primeiraDataSelecionada;
                         novaViagem.HoraInicio = new DateTime(
-                            DataInicialCompleta.Year,
-                            DataInicialCompleta.Month,
-                            DataInicialCompleta.Day,
-                            DataInicialCompleta.Hour,
-                            DataInicialCompleta.Minute,
+                            DataInicialCompleta.Year ,
+                            DataInicialCompleta.Month ,
+                            DataInicialCompleta.Day ,
+                            DataInicialCompleta.Hour ,
+                            DataInicialCompleta.Minute ,
                             DataInicialCompleta.Second
                         );
 
@@ -461,24 +496,25 @@
 
                                 DataInicial = dataSelecionada.ToString("dd/MM/yyyy");
                                 DateTime.TryParseExact(
-                                    (DataInicial + " " + HoraInicio),
-                                    new string[] { "dd/MM/yyyy HH:mm" },
-                                    System.Globalization.CultureInfo.InvariantCulture,
-                                    System.Globalization.DateTimeStyles.None,
+                                    (DataInicial + " " + HoraInicio) ,
+                                    new string[] { "dd/MM/yyyy HH:mm" } ,
+                                    System.Globalization.CultureInfo.InvariantCulture ,
+                                    System.Globalization.DateTimeStyles.None ,
                                     out DataInicialCompleta
                                 );
 
-                                AtualizarDadosAgendamento(novaViagemRecorrente, viagem);
+                                AtualizarDadosAgendamento(novaViagemRecorrente , viagem);
                                 novaViagemRecorrente.DataInicial = dataSelecionada;
                                 novaViagemRecorrente.HoraInicio = new DateTime(
-                                    DataInicialCompleta.Year,
-                                    DataInicialCompleta.Month,
-                                    DataInicialCompleta.Day,
-                                    DataInicialCompleta.Hour,
-                                    DataInicialCompleta.Minute,
+                                    DataInicialCompleta.Year ,
+                                    DataInicialCompleta.Month ,
+                                    DataInicialCompleta.Day ,
+                                    DataInicialCompleta.Hour ,
+                                    DataInicialCompleta.Minute ,
                                     DataInicialCompleta.Second
                                 );
                                 novaViagemRecorrente.RecorrenciaViagemId = viagemIdRecorrente;
+
                                 novaViagemRecorrente.UsuarioIdAgendamento = currentUserID;
                                 novaViagemRecorrente.DataAgendamento = DateTime.Now;
 
@@ -513,10 +549,9 @@
                             }
                             catch (Exception error)
                             {
-                                _log.Error(error.Message, error, "AgendaController.cs", "AgendamentoAsync.RecorrenciaDatas");
                                 Alerta.TratamentoErroComLinha(
-                                    "AgendaController.cs",
-                                    "AgendamentoAsync.RecorrenciaDatas",
+                                    "AgendaController.cs" ,
+                                    "Agendamento.foreach" ,
                                     error
                                 );
                             }
@@ -553,21 +588,21 @@
                                         break;
                                 }
 
-                                for (int i = 1; i < DatasSelecionadasAdicao.Count(); i++)
+                                for (int i = 1 ; i < DatasSelecionadasAdicao.Count() ; i++)
                                 {
                                     try
                                     {
                                         proximaData = proximaData.AddDays(incremento);
 
                                         Viagem novaViagemPeriodo = new Viagem();
-                                        AtualizarDadosAgendamento(novaViagemPeriodo, viagem);
+                                        AtualizarDadosAgendamento(novaViagemPeriodo , viagem);
                                         novaViagemPeriodo.DataInicial = proximaData;
                                         novaViagemPeriodo.HoraInicio = new DateTime(
-                                            proximaData.Year,
-                                            proximaData.Month,
-                                            proximaData.Day,
-                                            DataInicialCompleta.Hour,
-                                            DataInicialCompleta.Minute,
+                                            proximaData.Year ,
+                                            proximaData.Month ,
+                                            proximaData.Day ,
+                                            DataInicialCompleta.Hour ,
+                                            DataInicialCompleta.Minute ,
                                             DataInicialCompleta.Second
                                         );
                                         novaViagemPeriodo.RecorrenciaViagemId = viagemIdRecorrente;
@@ -606,10 +641,9 @@
                                     }
                                     catch (Exception error)
                                     {
-                                        _log.Error(error.Message, error, "AgendaController.cs", "Agendamento.intervalo.for");
                                         Alerta.TratamentoErroComLinha(
-                                            "AgendaController.cs",
-                                            "Agendamento.intervalo.for",
+                                            "AgendaController.cs" ,
+                                            "Agendamento.intervalo.for" ,
                                             error
                                         );
                                     }
@@ -617,10 +651,9 @@
                             }
                             catch (Exception error)
                             {
-                                _log.Error(error.Message, error, "AgendaController.cs", "Agendamento.intervalo");
                                 Alerta.TratamentoErroComLinha(
-                                    "AgendaController.cs",
-                                    "Agendamento.intervalo",
+                                    "AgendaController.cs" ,
+                                    "Agendamento.intervalo" ,
                                     error
                                 );
                             }
@@ -630,14 +663,14 @@
                     novaViagem.OperacaoBemSucedida = true;
                     return Ok(new
                     {
-                        novaViagem,
+                        novaViagem ,
                         success = true
                     });
                 }
 
                 if (isNew == false)
                 {
-                    var agendamentoAtual = _unitOfWork.Viagem.GetFirstOrDefaultWithTracking(vg =>
+                    var agendamentoAtual = _unitOfWork.Viagem.GetFirstOrDefault(vg =>
                         vg.ViagemId == viagem.ViagemId
                     );
                     if (agendamentoAtual == null)
@@ -645,7 +678,7 @@
                         return NotFound(
                             new
                             {
-                                success = false,
+                                success = false ,
                                 message = "Agendamento não encontrado"
                             }
                         );
@@ -667,7 +700,7 @@
                     if (viagem.HoraInicio.HasValue && dataOriginal.HasValue)
                     {
                         agendamentoAtual.HoraInicio = CombineHourKeepingDate(
-                            dataOriginal,
+                            dataOriginal ,
                             viagem.HoraInicio
                         );
                     }
@@ -696,14 +729,17 @@
 
                     if (isTransformacaoParaViagem)
                     {
+
                         if (!string.IsNullOrEmpty(viagem.UsuarioIdCriacao))
                         {
+
                             if (string.IsNullOrEmpty(agendamentoAtual.UsuarioIdCriacao))
                             {
                                 agendamentoAtual.UsuarioIdCriacao = viagem.UsuarioIdCriacao;
                                 agendamentoAtual.DataCriacao = viagem.DataCriacao;
                             }
                         }
+
                         else if (string.IsNullOrEmpty(agendamentoAtual.UsuarioIdCriacao))
                         {
                             agendamentoAtual.UsuarioIdCriacao = currentUserID;
@@ -713,7 +749,7 @@
 
                     if (viagem.KmFinal != null && viagem.KmFinal != 0)
                     {
-                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(vcl =>
+                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(vcl =>
                             vcl.VeiculoId == viagem.VeiculoId
                         );
                         if (veiculo != null && veiculo.Quilometragem < viagem.KmFinal)
@@ -734,10 +770,10 @@
                     return Ok(
                         new
                         {
-                            success = true,
-                            message = "Agendamento Atualizado com Sucesso",
-                            viagemId = agendamentoAtual.ViagemId,
-                            objViagem = agendamentoAtual,
+                            success = true ,
+                            message = "Agendamento Atualizado com Sucesso" ,
+                            viagemId = agendamentoAtual.ViagemId ,
+                            objViagem = agendamentoAtual ,
                         }
                     );
                 }
@@ -745,7 +781,7 @@
                 if ((viagem.ViagemId == Guid.Empty))
                 {
                     Viagem objViagem = new Viagem();
-                    AtualizarDadosAgendamento(objViagem, viagem);
+                    AtualizarDadosAgendamento(objViagem , viagem);
 
                     objViagem.UsuarioIdAgendamento = currentUserID;
                     objViagem.DataAgendamento = DateTime.Now;
@@ -755,7 +791,7 @@
 
                     if (viagem.KmFinal != null && viagem.KmFinal != 0)
                     {
-                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(v =>
+                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                             v.VeiculoId == viagem.VeiculoId
                         );
                         if (veiculo.Quilometragem < viagem.KmFinal)
@@ -769,10 +805,10 @@
                     return Ok(
                         new
                         {
-                            success = true,
-                            message = "Agendamento inserido com sucesso",
-                            viagemId = objViagem.ViagemId,
-                            objViagem,
+                            success = true ,
+                            message = "Agendamento inserido com sucesso" ,
+                            viagemId = objViagem.ViagemId ,
+                            objViagem ,
                         }
                     );
                 }
@@ -780,20 +816,19 @@
                 return Ok(
                     new
                     {
-                        success = true,
-                        message = "Operação realizada com sucesso",
-                        viagemId = viagem.ViagemId,
-                    }
-                );
-            }
-            catch (Exception ex)
-            {
-                _log.Error("Erro no processamento do agendamento", ex, "AgendaController.cs", "AgendamentoAsync");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "AgendamentoAsync", ex);
+                        success = true ,
+                        message = "Operação realizada com sucesso" ,
+                        viagemId = viagem.ViagemId ,
+                    }
+                );
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "Agendamento" , error);
                 return BadRequest(new
                 {
-                    success = false,
-                    mensagem = ex.Message
+                    success = false ,
+                    mensagem = error.Message
                 });
             }
         }
@@ -803,7 +838,7 @@
         {
             try
             {
-                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefaultWithTracking(v =>
+                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                     v.ViagemId == viagem.ViagemId
                 );
                 if (objFromDb == null)
@@ -811,14 +846,13 @@
                     return NotFound(
                         new
                         {
-                            success = false,
+                            success = false ,
                             message = "Agendamento não encontrado"
                         }
                     );
                 }
 
                 var itensManutencao = _context.ItensManutencao
-                    .AsTracking()
                     .Where(i => i.ViagemId.HasValue && i.ViagemId.Value == viagem.ViagemId)
                     .ToList();
 
@@ -832,19 +866,18 @@
                 _unitOfWork.Save();
                 return Ok(new
                 {
-                    success = true,
+                    success = true ,
                     message = "Agendamento apagado com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao apagar agendamento", ex, "AgendaController.cs", "ApagaAgendamento");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "ApagaAgendamento", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    error = "Erro interno do servidor",
-                    message = ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "ApagaAgendamento" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    error = "Erro interno do servidor" ,
+                    message = error.Message
                 });
             }
         }
@@ -858,7 +891,7 @@
                 {
                     return BadRequest(new
                     {
-                        success = false,
+                        success = false ,
                         message = "RecorrenciaViagemId é obrigatório"
                     });
                 }
@@ -874,7 +907,7 @@
                 {
                     return NotFound(new
                     {
-                        success = false,
+                        success = false ,
                         message = "Nenhum agendamento recorrente encontrado"
                     });
                 }
@@ -901,19 +934,18 @@
 
                 return Ok(new
                 {
-                    success = true,
+                    success = true ,
                     message = $"{viagemIds.Count} agendamento(s) recorrente(s) foram excluídos com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao apagar agendamentos recorrentes", ex, "AgendaController.cs", "ApagaAgendamentosRecorrentes");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "ApagaAgendamentosRecorrentes", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    error = "Erro interno do servidor",
-                    message = ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "ApagaAgendamentosRecorrentes" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    error = "Erro interno do servidor" ,
+                    message = error.Message
                 });
             }
         }
@@ -928,7 +960,7 @@
         {
             try
             {
-                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefaultWithTracking(v =>
+                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                     v.ViagemId == viagem.ViagemId
                 );
                 if (objFromDb == null)
@@ -936,7 +968,7 @@
                     return NotFound(
                         new
                         {
-                            success = false,
+                            success = false ,
                             message = "Agendamento não encontrado"
                         }
                     );
@@ -961,24 +993,23 @@
                 _unitOfWork.Save();
                 return Ok(new
                 {
-                    success = true,
+                    success = true ,
                     message = "Agendamento cancelado com sucesso"
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao cancelar agendamento", ex, "AgendaController.cs", "CancelaAgendamento");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "CancelaAgendamento", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "CancelaAgendamento" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
                     error = "Erro interno do servidor"
                 });
             }
         }
 
         [HttpGet("CarregaViagens")]
-        public ActionResult CarregaViagens(DateTime start, DateTime end)
+        public ActionResult CarregaViagens(DateTime start , DateTime end)
         {
             try
             {
@@ -990,12 +1021,28 @@
                     .Where(v => v.DataInicial.HasValue
                         && v.DataInicial >= startMenos3
                         && v.DataInicial < endMenos3)
+                    .Select(v => new
+                    {
+                        v.ViagemId ,
+                        v.Titulo ,
+                        v.Start ,
+                        v.DataInicial ,
+                        v.CorEvento ,
+                        v.CorTexto ,
+                        v.Descricao ,
+                        v.Placa ,
+                        v.NomeMotorista ,
+                        v.NomeEventoFull ,
+                        v.Finalidade
+                    })
                     .ToList();
 
                 var viagens = viagensRaw
                     .Select(v =>
                     {
+
                         var startDate = v.Start ?? v.DataInicial ?? DateTime.Now;
+
                         var endDate = startDate.AddHours(1);
 
                         return new
@@ -1006,7 +1053,9 @@
                             end = endDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                             backgroundColor = v.CorEvento ?? "#808080",
                             textColor = v.CorTexto ?? "#FFFFFF",
+
                             descricao = v.Descricao ?? "",
+
                             placa = v.Placa,
                             motorista = v.NomeMotorista,
                             evento = v.NomeEventoFull,
@@ -1017,42 +1066,35 @@
 
                 return Ok(new { data = viagens });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao carregar viagens", ex, "AgendaController.cs", "CarregaViagens");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "CarregaViagens", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    error = "Erro interno do servidor",
-                    mensagemDetalhada = ex.Message,
-                    stackTrace = ex.StackTrace,
-                    innerException = ex.InnerException?.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "CarregaViagens" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    error = "Erro interno do servidor" ,
+                    mensagemDetalhada = error.Message ,
+                    stackTrace = error.StackTrace ,
+                    innerException = error.InnerException?.Message
                 });
             }
         }
 
         [HttpGet("GetDatasViagem")]
         public IActionResult GetDatasViagem(
-            Guid viagemId,
-            Guid recorrenciaViagemId = default(Guid),
+            Guid viagemId ,
+            Guid recorrenciaViagemId = default(Guid) ,
             bool editarProximos = false
         )
         {
             try
             {
-                var objViagens = _unitOfWork.Viagem.GetAllReduced(selector: v => new
-                {
-                    v.DataInicial,
-                    v.RecorrenciaViagemId,
-                    v.ViagemId,
-                });
-
                 List<DateTime> datasOrdenadas;
 
                 if (recorrenciaViagemId == Guid.Empty)
                 {
-                    datasOrdenadas = objViagens
+                    datasOrdenadas = _context.Viagem
+                        .AsNoTracking()
                         .Where(v => v.ViagemId == viagemId || v.RecorrenciaViagemId == viagemId)
                         .Select(v => v.DataInicial)
                         .Where(d => d.HasValue)
@@ -1062,13 +1104,16 @@
                 }
                 else if (editarProximos)
                 {
-                    var dataAtual = objViagens
-                        .FirstOrDefault(v => v.ViagemId == viagemId)
-                        ?.DataInicial;
+                    var dataAtual = _context.Viagem
+                        .AsNoTracking()
+                        .Where(v => v.ViagemId == viagemId)
+                        .Select(v => v.DataInicial)
+                        .FirstOrDefault();
 
                     if (dataAtual.HasValue)
                     {
-                        datasOrdenadas = objViagens
+                        datasOrdenadas = _context.Viagem
+                            .AsNoTracking()
                             .Where(v =>
                                 v.RecorrenciaViagemId == recorrenciaViagemId
                                 && v.DataInicial >= dataAtual
@@ -1084,7 +1129,7 @@
                         return BadRequest(
                             new
                             {
-                                sucesso = false,
+                                sucesso = false ,
                                 mensagem = "Registro de viagem não encontrado."
                             }
                         );
@@ -1092,7 +1137,8 @@
                 }
                 else
                 {
-                    datasOrdenadas = objViagens
+                    datasOrdenadas = _context.Viagem
+                        .AsNoTracking()
                         .Where(v =>
                             v.RecorrenciaViagemId == recorrenciaViagemId
                             || v.ViagemId == viagemId
@@ -1107,14 +1153,13 @@
 
                 return Ok(datasOrdenadas);
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao obter datas da viagem", ex, "AgendaController.cs", "GetDatasViagem");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "GetDatasViagem", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "GetDatasViagem" , error);
                 return BadRequest(new
                 {
-                    sucesso = false,
-                    mensagem = ex.Message
+                    sucesso = false ,
+                    mensagem = error.Message
                 });
             }
         }
@@ -1124,6 +1169,7 @@
         {
             try
             {
+
                 var objViagem = _unitOfWork
                     .Viagem.GetAll()
                     .Where(v => v.ViagemId == viagemId)
@@ -1139,16 +1185,15 @@
 
                 return Ok(objViagem);
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao obter agendamento", ex, "AgendaController.cs", "ObterAgendamento");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamento", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "ObterAgendamento" , error);
                 return StatusCode(
-                    500,
+                    500 ,
                     new
                     {
-                        mensagem = "Erro interno ao obter o agendamento",
-                        detalhes = ex.Message
+                        mensagem = "Erro interno ao obter o agendamento" ,
+                        erro = error.Message
                     }
                 );
             }
@@ -1169,18 +1214,21 @@
                 return NotFound(
                     new
                     {
-                        sucesso = false,
+                        sucesso = false ,
                         mensagem = "Registro de viagem não encontrado."
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao obter agendamento para edição", ex, "AgendaController.cs", "ObterAgendamentoEdicao");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentoEdicao", ex);
-                return StatusCode(500, new
-                {
-                    sucesso = false,
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AgendaController.cs" ,
+                    "ObterAgendamentoEdicao" ,
+                    error
+                );
+                return StatusCode(500 , new
+                {
+                    sucesso = false ,
                     error = "Erro interno do servidor"
                 });
             }
@@ -1199,18 +1247,21 @@
                 return NotFound(
                     new
                     {
-                        sucesso = false,
+                        sucesso = false ,
                         mensagem = "Registro de viagem não encontrado."
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao obter agendamento edição inicial", ex, "AgendaController.cs", "ObterAgendamentoEdicaoInicial");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentoEdicaoInicial", ex);
-                return StatusCode(500, new
-                {
-                    sucesso = false,
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AgendaController.cs" ,
+                    "ObterAgendamentoEdicaoInicial" ,
+                    error
+                );
+                return StatusCode(500 , new
+                {
+                    sucesso = false ,
                     error = "Erro interno do servidor"
                 });
             }
@@ -1231,18 +1282,21 @@
                 return NotFound(
                     new
                     {
-                        sucesso = false,
+                        sucesso = false ,
                         mensagem = "Registro de viagem não encontrado."
                     }
                 );
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao obter agendamentos para exclusão", ex, "AgendaController.cs", "ObterAgendamentoExclusao");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentoExclusao", ex);
-                return StatusCode(500, new
-                {
-                    sucesso = false,
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AgendaController.cs" ,
+                    "ObterAgendamentoExclusao" ,
+                    error
+                );
+                return StatusCode(500 , new
+                {
+                    sucesso = false ,
                     error = "Erro interno do servidor"
                 });
             }
@@ -1250,7 +1304,7 @@
 
         [HttpGet("ObterAgendamentosRecorrentes")]
         public async Task<IActionResult> ObterAgendamentosRecorrentes(
-            string RecorrenciaViagemId,
+            string RecorrenciaViagemId ,
             string DataInicialRecorrencia
         )
         {
@@ -1269,15 +1323,21 @@
                 }
                 return Ok(objViagens);
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao obter agendamentos recorrentes", ex, "AgendaController.cs", "ObterAgendamentosRecorrentes");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentosRecorrentes", ex);
-                return StatusCode(500, new
-                {
-                    mensagem = "Erro interno ao obter os agendamentos recorrentes",
-                    erro = ex.Message,
-                });
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AgendaController.cs" ,
+                    "ObterAgendamentosRecorrentes" ,
+                    error
+                );
+                return StatusCode(
+                    500 ,
+                    new
+                    {
+                        mensagem = "Erro interno ao obter os agendamentos recorrentes" ,
+                        erro = error.Message ,
+                    }
+                );
             }
         }
 
@@ -1290,20 +1350,25 @@
 
                 if (objUsuario == null)
                 {
-                    return Ok(new { data = "" });
+                    return Ok(new
+                    {
+                        data = ""
+                    });
                 }
                 else
                 {
-                    return Ok(new { data = objUsuario.NomeCompleto });
-                }
-            }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao recuperar usuário", ex, "AgendaController.cs", "RecuperaUsuario");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "RecuperaUsuario", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
+                    return Ok(new
+                    {
+                        data = objUsuario.NomeCompleto
+                    });
+                }
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "RecuperaUsuario" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
                     error = "Erro interno do servidor"
                 });
             }
@@ -1320,24 +1385,26 @@
                     data = viagemObj
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao recuperar viagem", ex, "AgendaController.cs", "RecuperaViagem");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "RecuperaViagem", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    error = ex.Message,
-                    stackTrace = ex.StackTrace,
-                    innerException = ex.InnerException?.Message,
-                });
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "RecuperaViagem" , error);
+                return StatusCode(
+                    500 ,
+                    new
+                    {
+                        success = false ,
+                        error = error.Message ,
+                        stackTrace = error.StackTrace ,
+                        innerException = error.InnerException?.Message ,
+                    }
+                );
             }
         }
 
         [HttpGet("VerificarAgendamento")]
         public IActionResult VerificarAgendamento(
-            string data,
-            Guid viagemIdRecorrente = default,
+            string data ,
+            Guid viagemIdRecorrente = default ,
             string horaInicio = null
         )
         {
@@ -1348,17 +1415,17 @@
                     return BadRequest(
                         new
                         {
-                            sucesso = false,
-                            mensagem = "A data é obrigatória para verificar o agendamento.",
+                            sucesso = false ,
+                            mensagem = "A data é obrigatória para verificar o agendamento." ,
                         }
                     );
                 }
 
-                if (!DateTime.TryParse(data, out DateTime dataAgendamento))
+                if (!DateTime.TryParse(data , out DateTime dataAgendamento))
                 {
                     return BadRequest(new
                     {
-                        sucesso = false,
+                        sucesso = false ,
                         mensagem = "Data inválida."
                     });
                 }
@@ -1366,7 +1433,7 @@
                 TimeSpan? horaAgendamento = null;
                 if (
                     !string.IsNullOrEmpty(horaInicio)
-                    && TimeSpan.TryParse(horaInicio, out TimeSpan parsedHora)
+                    && TimeSpan.TryParse(horaInicio , out TimeSpan parsedHora)
                 )
                 {
                     horaAgendamento = parsedHora;
@@ -1395,23 +1462,21 @@
                     existe = existeAgendamento
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao verificar disponibilidade de agendamento", ex, "AgendaController.cs", "VerificarAgendamento");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "VerificarAgendamento", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AgendaController.cs" , "VerificarAgendamento" , error);
                 return BadRequest(new
                 {
-                    sucesso = false,
-                    mensagem = ex.Message
-                });
-            }
-        }
-
-        private void AtualizarDadosAgendamento(Viagem objViagem, AgendamentoViagem viagem)
-        {
-            try
-            {
-
+                    sucesso = false ,
+                    mensagem = error.Message
+                });
+            }
+        }
+
+        private void AtualizarDadosAgendamento(Viagem objViagem , AgendamentoViagem viagem)
+        {
+            try
+            {
                 objViagem.DataInicial = viagem.DataInicial;
                 objViagem.HoraInicio = viagem.HoraInicio;
                 objViagem.Finalidade = viagem.Finalidade;
@@ -1459,15 +1524,18 @@
                     descricao = Servicos.ConvertHtml(descricao);
                 objViagem.DescricaoSemFormato = descricao;
             }
-            catch (Exception ex)
-            {
-                _log.Error("Erro ao atualizar dados do agendamento", ex, "AgendaController.cs", "AtualizarDadosAgendamento");
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "AtualizarDadosAgendamento", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AgendaController.cs" ,
+                    "AtualizarDadosAgendamento" ,
+                    error
+                );
                 return;
             }
         }
 
-        private static DateTime? CombineHourKeepingDate(DateTime? baseDate, DateTime? newTime)
+        private static DateTime? CombineHourKeepingDate(DateTime? baseDate , DateTime? newTime)
         {
             try
             {
@@ -1478,19 +1546,22 @@
                 var t = newTime.Value;
 
                 return new DateTime(
-                    d.Year,
-                    d.Month,
-                    d.Day,
-                    t.Hour,
-                    t.Minute,
-                    0,
+                    d.Year ,
+                    d.Month ,
+                    d.Day ,
+                    t.Hour ,
+                    t.Minute ,
+                    0 ,
                     DateTimeKind.Unspecified
                 );
             }
-            catch (Exception ex)
-            {
-
-                Alerta.TratamentoErroComLinha("AgendaController.cs", "CombineHourKeepingDate", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AgendaController.cs" ,
+                    "CombineHourKeepingDate" ,
+                    error
+                );
                 return default(DateTime?);
             }
         }
```
