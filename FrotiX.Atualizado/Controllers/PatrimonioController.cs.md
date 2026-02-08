# Controllers/PatrimonioController.cs

**Mudanca:** GRANDE | **+41** linhas | **-55** linhas

---

```diff
--- JANEIRO: Controllers/PatrimonioController.cs
+++ ATUAL: Controllers/PatrimonioController.cs
@@ -7,8 +7,6 @@
 using System.Linq;
 using System.Security.Claims;
 using System.Threading.Tasks;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -19,23 +17,14 @@
     {
         private readonly IUnitOfWork _unitOfWork;
         private readonly IMemoryCache _cache;
-        private readonly ILogService _log;
 
         private static readonly HashSet<string> _processandoRequests = new HashSet<string>();
         private static readonly object _lockObject = new object();
 
-        public PatrimonioController(IUnitOfWork unitOfWork, IMemoryCache cache, ILogService log)
-        {
-            try
-            {
-                _unitOfWork = unitOfWork;
-                _cache = cache;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "PatrimonioController", error);
-            }
+        public PatrimonioController(IUnitOfWork unitOfWork , IMemoryCache cache)
+        {
+            _unitOfWork = unitOfWork;
+            _cache = cache;
         }
 
         [HttpGet]
@@ -43,7 +32,6 @@
         {
             try
             {
-
                 var query = _unitOfWork.ViewPatrimonioConferencia.GetAll().AsQueryable();
 
                 if (!string.IsNullOrWhiteSpace(marca))
@@ -101,8 +89,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.Get", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "Get", ex);
                 return Json(
                     new
                     {
@@ -120,14 +106,12 @@
         {
             try
             {
-
                 var movimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(m =>
                     m.MovimentacaoPatrimonioId == id
                 );
 
                 if (movimentacao == null)
                 {
-
                     return Json(new
                     {
                         success = false ,
@@ -181,8 +165,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.GetMovimentacao", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "GetMovimentacao", ex);
                 return Json(
                     new
                     {
@@ -197,8 +179,11 @@
         [Route("CreateMovimentacao")]
         public IActionResult CreateMovimentacao([FromBody] MovimentacaoPatrimonioDto dto)
         {
-
             var requestId = Guid.NewGuid().ToString().Substring(0 , 8);
+            Console.WriteLine($"[{requestId}] === INÍCIO CreateMovimentacao ===");
+            Console.WriteLine(
+                $"[{requestId}] Dados recebidos: PatrimonioId={dto.PatrimonioId}, Data={dto.DataMovimentacao}"
+            );
 
             var requestKey =
                 $"{dto.PatrimonioId}_{dto.DataMovimentacao?.ToString("yyyyMMddHHmmss")}";
@@ -207,7 +192,7 @@
             {
                 if (_processandoRequests.Contains(requestKey))
                 {
-
+                    Console.WriteLine($"[{requestId}] Requisição duplicada detectada. Rejeitando.");
                     return Json(
                         new
                         {
@@ -224,9 +209,13 @@
 
                 ClaimsPrincipal currentUser = this.User;
                 var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
+                Console.WriteLine($"[{requestId}] Usuário: {currentUserID}");
+
+                Console.WriteLine($"[{requestId}] Iniciando validações...");
 
                 if (dto.PatrimonioId == Guid.Empty)
                 {
+                    Console.WriteLine($"[{requestId}] Erro: Patrimônio não selecionado");
                     return Json(new
                     {
                         success = false ,
@@ -236,6 +225,7 @@
 
                 if (!dto.DataMovimentacao.HasValue)
                 {
+                    Console.WriteLine($"[{requestId}] Erro: Data não informada");
                     return Json(
                         new
                         {
@@ -247,6 +237,7 @@
 
                 if (!dto.SetorDestinoId.HasValue || dto.SetorDestinoId == Guid.Empty)
                 {
+                    Console.WriteLine($"[{requestId}] Erro: Setor destino não informado");
                     return Json(
                         new
                         {
@@ -258,6 +249,7 @@
 
                 if (!dto.SecaoDestinoId.HasValue || dto.SecaoDestinoId == Guid.Empty)
                 {
+                    Console.WriteLine($"[{requestId}] Erro: Seção destino não informada");
                     return Json(
                         new
                         {
@@ -267,6 +259,10 @@
                     );
                 }
 
+                Console.WriteLine($"[{requestId}] Validações OK");
+
+                Console.WriteLine($"[{requestId}] Buscando patrimônio {dto.PatrimonioId}...");
+
                 Patrimonio patrimonio = null;
                 try
                 {
@@ -276,12 +272,13 @@
                 }
                 catch (Exception findEx)
                 {
-                    _log.Error("PatrimonioController.CreateMovimentacao.FindPatrimonio", findEx);
+                    Console.WriteLine($"[{requestId}] Erro ao buscar patrimônio: {findEx.Message}");
                     throw;
                 }
 
                 if (patrimonio == null)
                 {
+                    Console.WriteLine($"[{requestId}] Patrimônio não encontrado");
                     return Json(new
                     {
                         success = false ,
@@ -289,11 +286,16 @@
                     });
                 }
 
+                Console.WriteLine(
+                    $"[{requestId}] Patrimônio encontrado: NPR={patrimonio.NPR}, SetorAtual={patrimonio.SetorId}, SecaoAtual={patrimonio.SecaoId}"
+                );
+
                 var setorOrigemId = patrimonio.SetorId;
                 var secaoOrigemId = patrimonio.SecaoId;
 
                 if (dto.SecaoDestinoId == secaoOrigemId && dto.SetorDestinoId == setorOrigemId)
                 {
+                    Console.WriteLine($"[{requestId}] Erro: Destino igual à origem");
                     return Json(
                         new
                         {
@@ -303,6 +305,7 @@
                     );
                 }
 
+                Console.WriteLine($"[{requestId}] Criando objeto movimentação...");
                 var movimentacao = new MovimentacaoPatrimonio
                 {
                     MovimentacaoPatrimonioId = Guid.NewGuid() ,
@@ -314,21 +317,31 @@
                     SecaoDestinoId = dto.SecaoDestinoId.Value ,
                     ResponsavelMovimentacao = currentUserID ,
                 };
-
+                Console.WriteLine(
+                    $"[{requestId}] Movimentação criada com ID: {movimentacao.MovimentacaoPatrimonioId}"
+                );
+
+                Console.WriteLine($"[{requestId}] Atualizando patrimônio para novo destino...");
                 patrimonio.SetorId = dto.SetorDestinoId.Value;
                 patrimonio.SecaoId = dto.SecaoDestinoId.Value;
                 patrimonio.Status = dto.StatusPatrimonio;
 
                 try
                 {
+                    Console.WriteLine($"[{requestId}] Adicionando movimentação ao contexto...");
                     _unitOfWork.MovimentacaoPatrimonio.Add(movimentacao);
+
+                    Console.WriteLine($"[{requestId}] Marcando patrimônio como modificado...");
                     _unitOfWork.Patrimonio.Update(patrimonio);
+
+                    Console.WriteLine($"[{requestId}] Chamando Save() - ÚNICA VEZ");
                     _unitOfWork.Save();
-                    _log.Info($"PatrimonioController.CreateMovimentacao: Patrimônio {patrimonio.NPR} movido para Setor {dto.SetorDestinoId} / Seção {dto.SecaoDestinoId}.");
+                    Console.WriteLine($"[{requestId}] Save() completado com sucesso");
                 }
                 catch (Exception saveEx)
                 {
-                    _log.Error("PatrimonioController.CreateMovimentacao.Save", saveEx);
+                    Console.WriteLine($"[{requestId}] ERRO no Save(): {saveEx.Message}");
+                    Console.WriteLine($"[{requestId}] StackTrace: {saveEx.StackTrace}");
                     throw;
                 }
 
@@ -342,11 +355,14 @@
                     } ,
                 };
 
+                Console.WriteLine($"[{requestId}] Preparando resposta JSON de sucesso");
+                Console.WriteLine($"[{requestId}] === FIM CreateMovimentacao (SUCESSO) ===");
+
                 return Json(response);
             }
             catch (InvalidOperationException ioEx) when (ioEx.Message.Contains("second operation"))
             {
-                _log.Error("PatrimonioController.CreateMovimentacao.Concurrency", ioEx);
+                Console.WriteLine($"[{requestId}] ERRO de concorrência: {ioEx.Message}");
                 return Json(
                     new
                     {
@@ -357,8 +373,10 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.CreateMovimentacao", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "CreateMovimentacao", ex);
+                Console.WriteLine($"[{requestId}] ERRO geral: {ex.Message}");
+                Console.WriteLine($"[{requestId}] StackTrace: {ex.StackTrace}");
+                Console.WriteLine($"[{requestId}] === FIM CreateMovimentacao (ERRO) ===");
+
                 return Json(
                     new
                     {
@@ -373,6 +391,7 @@
                 lock (_lockObject)
                 {
                     _processandoRequests.Remove(requestKey);
+                    Console.WriteLine($"[{requestId}] Request removido da lista de processamento");
                 }
             }
         }
@@ -446,7 +465,6 @@
 
                 _unitOfWork.MovimentacaoPatrimonio.Update(movimentacao);
                 _unitOfWork.Save();
-                _log.Info($"PatrimonioController.UpdateMovimentacao: Movimentação {movimentacao.MovimentacaoPatrimonioId} do Patrimônio {patrimonio.NPR} atualizada.");
 
                 return Json(
                     new
@@ -458,8 +476,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.UpdateMovimentacao", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "UpdateMovimentacao", ex);
                 return Json(
                     new
                     {
@@ -500,7 +516,6 @@
 
                 _unitOfWork.MovimentacaoPatrimonio.Remove(movimentacao);
                 _unitOfWork.Save();
-                _log.Info($"PatrimonioController.DeleteMovimentacaoPatrimonio: Movimentação {dto.MovimentacaoPatrimonioId} removida.");
 
                 return Json(new
                 {
@@ -510,8 +525,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.DeleteMovimentacaoPatrimonio", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "DeleteMovimentacaoPatrimonio", ex);
                 return Json(
                     new
                     {
@@ -656,8 +669,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.MovimentacaoPatrimonioGrid", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "MovimentacaoPatrimonioGrid", ex);
                 return Json(
                     new
                     {
@@ -693,8 +704,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.GetResponsaveisMovimentacoes", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "GetResponsaveisMovimentacoes", ex);
                 return Json(new
                 {
                     success = false ,
@@ -744,8 +753,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.GetSetoresSecoesHierarquicos", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "GetSetoresSecoesHierarquicos", ex);
                 return Json(new
                 {
                     success = false ,
@@ -775,8 +782,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaPatrimonios", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaPatrimonios", ex);
                 return Json(
                     new
                     {
@@ -840,8 +845,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.GetSingle", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "GetSingle", ex);
                 return Json(
                     new
                     {
@@ -873,8 +876,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaSetores", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaSetores", ex);
                 return Json(
                     new
                     {
@@ -916,8 +917,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaSecoes", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaSecoes", ex);
                 return Json(
                     new
                     {
@@ -950,8 +949,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaMarcas", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaMarcas", ex);
                 return Json(new
                 {
                     success = false ,
@@ -992,8 +989,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaModelos", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaModelos", ex);
                 return Json(new
                 {
                     success = false ,
@@ -1044,8 +1039,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaMarcasModelos", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaMarcasModelos", ex);
                 return Json(new
                 {
                     success = false ,
@@ -1095,8 +1088,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaSetoresSecoes", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaSetoresSecoes", ex);
                 return Json(new
                 {
                     success = false ,
@@ -1129,8 +1120,6 @@
             }
             catch (Exception ex)
             {
-                _log.Error("PatrimonioController.ListaSituacoes", ex);
-                Alerta.TratamentoErroComLinha("PatrimonioController.cs", "ListaSituacoes", ex);
                 return Json(new
                 {
                     success = false ,
```
