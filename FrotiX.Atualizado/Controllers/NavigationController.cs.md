# Controllers/NavigationController.cs

**Mudanca:** GRANDE | **+323** linhas | **-349** linhas

---

```diff
--- JANEIRO: Controllers/NavigationController.cs
+++ ATUAL: Controllers/NavigationController.cs
@@ -2,7 +2,6 @@
 using FrotiX.Models;
 using FrotiX.Models.FontAwesome;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.Rendering;
@@ -28,7 +27,6 @@
         private readonly IUnitOfWork _unitOfWork;
         private readonly IWebHostEnvironment _env;
         private readonly IMemoryCache _cache;
-        private readonly ILogService _log;
 
         private string NavJsonPath => Path.Combine(_env.ContentRootPath, "nav.json");
         private string NavJsonBackupPath => Path.Combine(_env.ContentRootPath, "nav.json.bak");
@@ -37,18 +35,16 @@
         private const string CacheKeyFontAwesomeIcons = "FontAwesomeIcons";
         private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
 
-        public NavigationController(IUnitOfWork unitOfWork, IWebHostEnvironment env, IMemoryCache cache, ILogService log)
+        public NavigationController(IUnitOfWork unitOfWork, IWebHostEnvironment env, IMemoryCache cache)
         {
             try
             {
                 _unitOfWork = unitOfWork;
                 _env = env;
                 _cache = cache;
-                _log = log;
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.Constructor", error);
+            }
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha("NavigationController.cs", "NavigationController", error);
             }
         }
@@ -61,24 +57,22 @@
             {
 
                 var jsonText = System.IO.File.ReadAllText(NavJsonPath);
-
                 var navigation = NavigationBuilder.FromJson(jsonText);
 
-                var treeData = TransformToTreeData(navigation.Lists , null);
+                var treeData = TransformToTreeData(navigation.Lists, null);
 
                 return Json(new
                 {
-                    success = true ,
+                    success = true,
                     data = treeData
                 });
             }
             catch (Exception error)
             {
-                _log.Error("NavigationController.GetTree", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "GetTree" , error);
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "GetTree", error);
                 return Json(new
                 {
-                    success = false ,
+                    success = false,
                     message = "Erro ao carregar navegação: " + error.Message
                 });
             }
@@ -93,39 +87,38 @@
 
                 if (System.IO.File.Exists(NavJsonPath))
                 {
-                    System.IO.File.Copy(NavJsonPath , NavJsonBackupPath , true);
+                    System.IO.File.Copy(NavJsonPath, NavJsonBackupPath, true);
                 }
 
                 var navigation = new
                 {
-                    version = "0.9" ,
+                    version = "0.9",
                     lists = TransformFromTreeData(items)
                 };
 
                 var options = new JsonSerializerOptions
                 {
-                    WriteIndented = true ,
-                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase ,
+                    WriteIndented = true,
+                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                     Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                 };
-                var jsonText = JsonSerializer.Serialize(navigation , options);
-                System.IO.File.WriteAllText(NavJsonPath , jsonText);
+                var jsonText = JsonSerializer.Serialize(navigation, options);
+                System.IO.File.WriteAllText(NavJsonPath, jsonText);
 
                 SincronizarRecursos(items);
 
                 return Json(new
                 {
-                    success = true ,
+                    success = true,
                     message = "Navegação salva com sucesso!"
                 });
             }
             catch (Exception error)
             {
-                _log.Error("NavigationController.SaveTree", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "SaveTree" , error);
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "SaveTree", error);
                 return Json(new
                 {
-                    success = false ,
+                    success = false,
                     message = "Erro ao salvar navegação: " + error.Message
                 });
             }
@@ -143,25 +136,24 @@
 
                 if (recursoExistente != null)
                 {
-
                     return Json(new
                     {
-                        success = false ,
+                        success = false,
                         message = "Já existe um recurso com este Nome de Menu!"
                     });
                 }
 
                 var recurso = new Recurso
                 {
-                    RecursoId = Guid.NewGuid() ,
-                    Nome = !string.IsNullOrEmpty(item.Title) ? item.Title : "Novo Item" ,
-                    NomeMenu = !string.IsNullOrEmpty(item.NomeMenu) ? item.NomeMenu : $"menu_{Guid.NewGuid():N}" ,
-                    Descricao = $"Menu: {item.NomeMenu}" ,
-                    Ordem = GetNextOrdem() ,
-                    Icon = !string.IsNullOrEmpty(item.Icon) ? item.Icon : "fa-regular fa-folder" ,
-                    Href = !string.IsNullOrEmpty(item.Href) ? item.Href : "javascript:void(0);" ,
-                    Ativo = true ,
-                    Nivel = 0 ,
+                    RecursoId = Guid.NewGuid(),
+                    Nome = !string.IsNullOrEmpty(item.Title) ? item.Title : "Novo Item",
+                    NomeMenu = !string.IsNullOrEmpty(item.NomeMenu) ? item.NomeMenu : $"menu_{Guid.NewGuid():N}",
+                    Descricao = $"Menu: {item.NomeMenu}",
+                    Ordem = GetNextOrdem(),
+                    Icon = !string.IsNullOrEmpty(item.Icon) ? item.Icon : "fa-regular fa-folder",
+                    Href = !string.IsNullOrEmpty(item.Href) ? item.Href : "javascript:void(0);",
+                    Ativo = true,
+                    Nivel = 0,
                     HasChild = false
                 };
                 _unitOfWork.Recurso.Add(recurso);
@@ -173,24 +165,24 @@
 
                 return Json(new
                 {
-                    success = true ,
-                    recursoId = recurso.RecursoId ,
+                    success = true,
+                    recursoId = recurso.RecursoId,
                     message = "Item adicionado com sucesso!"
                 });
             }
             catch (Exception error)
             {
-                _log.Error("NavigationController.AddItem", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "AddItem" , error);
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "AddItem", error);
                 return Json(new
                 {
-                    success = false ,
+                    success = false,
                     message = "Erro ao adicionar item: " + error.Message
                 });
             }
         }
 
         [HttpPost]
+
         [Route("UpdateItem")]
         public IActionResult UpdateItem([FromBody] NavigationItemDTO item)
         {
@@ -202,7 +194,6 @@
 
                 if (recurso != null)
                 {
-
                     recurso.Nome = item.Title;
                     recurso.NomeMenu = item.NomeMenu;
                     _unitOfWork.Recurso.Update(recurso);
@@ -211,23 +202,23 @@
 
                 return Json(new
                 {
-                    success = true ,
+                    success = true,
                     message = "Item atualizado com sucesso!"
                 });
             }
             catch (Exception error)
             {
-                _log.Error("NavigationController.UpdateItem", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "UpdateItem" , error);
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "UpdateItem", error);
                 return Json(new
                 {
-                    success = false ,
+                    success = false,
                     message = "Erro ao atualizar item: " + error.Message
                 });
             }
         }
 
         [HttpPost]
+
         [Route("DeleteItem")]
         public IActionResult DeleteItem([FromBody] DeleteNavigationItemRequest request)
         {
@@ -253,17 +244,16 @@
 
                 return Json(new
                 {
-                    success = true ,
+                    success = true,
                     message = "Item removido com sucesso!"
                 });
             }
             catch (Exception error)
             {
-                _log.Error("NavigationController.DeleteItem", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "DeleteItem" , error);
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "DeleteItem", error);
                 return Json(new
                 {
-                    success = false ,
+                    success = false,
                     message = "Erro ao remover item: " + error.Message
                 });
             }
@@ -288,19 +278,19 @@
                     return acesso?.Acesso == true;
                 }).ToList();
 
-                var arvore = MontarArvoreRecursiva(recursosComAcesso , null);
-
-                return Json(new { success = true , data = arvore });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.GetTreeFromDb", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "GetTreeFromDb" , error);
-                return Json(new { success = false , message = error.Message });
+                var arvore = MontarArvoreRecursiva(recursosComAcesso, null);
+
+                return Json(new { success = true, data = arvore });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "GetTreeFromDb", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
         [HttpGet]
+
         [Route("GetTreeAdmin")]
         public IActionResult GetTreeAdmin()
         {
@@ -310,19 +300,19 @@
                     .OrderBy(r => r.Ordem)
                     .ToList();
 
-                var arvore = MontarArvoreRecursiva(todosRecursos , null);
-
-                return Json(new { success = true , data = arvore });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.GetTreeAdmin", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "GetTreeAdmin" , error);
-                return Json(new { success = false , message = error.Message });
+                var arvore = MontarArvoreRecursiva(todosRecursos, null);
+
+                return Json(new { success = true, data = arvore });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "GetTreeAdmin", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
         [HttpGet]
+
         [Route("DebugTreeAdmin")]
         public IActionResult DebugTreeAdmin()
         {
@@ -331,35 +321,34 @@
                 var todosRecursos = _unitOfWork.Recurso.GetAll().ToList();
                 var totalRecursos = todosRecursos.Count;
                 var recursosRaiz = todosRecursos.Where(r => r.ParentId == null).ToList();
-                var arvore = MontarArvoreRecursiva(todosRecursos , null);
+                var arvore = MontarArvoreRecursiva(todosRecursos, null);
 
                 return Json(new
                 {
-                    success = true ,
-                    totalRecursosNoBanco = totalRecursos ,
-                    totalRecursosRaiz = recursosRaiz.Count ,
-                    totalItensNaArvore = arvore.Count ,
+                    success = true,
+                    totalRecursosNoBanco = totalRecursos,
+                    totalRecursosRaiz = recursosRaiz.Count,
+                    totalItensNaArvore = arvore.Count,
                     primeiros5Recursos = todosRecursos.Take(5).Select(r => new
                     {
-                        r.RecursoId ,
-                        r.Nome ,
-                        r.NomeMenu ,
-                        r.ParentId ,
-                        r.Ordem ,
+                        r.RecursoId,
+                        r.Nome,
+                        r.NomeMenu,
+                        r.ParentId,
+                        r.Ordem,
                         r.Ativo
-                    }) ,
-                    recursosRaizNomes = recursosRaiz.Select(r => r.Nome).ToList() ,
+                    }),
+                    recursosRaizNomes = recursosRaiz.Select(r => r.Nome).ToList(),
                     arvoreGerada = arvore
                 });
             }
             catch (Exception error)
             {
-                _log.Error("NavigationController.DebugTreeAdmin", error );
                 return Json(new
                 {
-                    success = false ,
-                    message = error.Message ,
-                    stackTrace = error.StackTrace ,
+                    success = false,
+                    message = error.Message,
+                    stackTrace = error.StackTrace,
                     innerException = error.InnerException?.Message
                 });
             }
@@ -409,13 +398,12 @@
                 List<RecursoTreeDTO>? items = null;
                 try
                 {
-                    items = JsonSerializer.Deserialize<List<RecursoTreeDTO>>(jsonBody , options);
+                    items = JsonSerializer.Deserialize<List<RecursoTreeDTO>>(jsonBody, options);
                 }
                 catch (JsonException ex)
                 {
                     Log($"❌ Erro ao deserializar JSON: {ex.Message}");
-                    _log.Error("NavigationController.SaveTreeToDb.JsonDeserialize", ex );
-                    return Json(new { success = false , message = "Erro ao processar JSON: " + ex.Message });
+                    return Json(new { success = false, message = "Erro ao processar JSON: " + ex.Message });
                 }
 
                 Log($"Recebido {items?.Count ?? 0} itens para salvar");
@@ -423,7 +411,7 @@
                 if (items == null || items.Count == 0)
                 {
                     Log("❌ ERRO: items é NULL ou vazio!");
-                    return Json(new { success = false , message = "Lista de itens é nula ou vazia. Verifique o JSON enviado." });
+                    return Json(new { success = false, message = "Lista de itens é nula ou vazia. Verifique o JSON enviado." });
                 }
 
                 foreach (var item in items.Take(3))
@@ -436,13 +424,13 @@
                 var updates = new List<RecursoUpdate>();
                 var processedIds = new HashSet<Guid>();
                 Log("Coletando atualizações...");
-                ColetarAtualizacoes(items , null , 0 , 0 , updates , processedIds);
+                ColetarAtualizacoes(items, null, 0, 0, updates, processedIds);
                 Log($"Total de atualizações coletadas: {updates.Count}");
 
                 if (updates.Count == 0)
                 {
                     Log("⚠️ Nenhuma atualização encontrada. Nada a salvar.");
-                    return Json(new { success = false , message = "Nenhuma alteração detectada na árvore." });
+                    return Json(new { success = false, message = "Nenhuma alteração detectada na árvore." });
                 }
 
                 var recursoIds = updates.Select(u => u.RecursoId).ToList();
@@ -458,7 +446,7 @@
                 for (int i = 0; i < updates.Count; i++)
                 {
                     var update = updates[i];
-                    if (recursosDict.TryGetValue(update.RecursoId , out var recurso))
+                    if (recursosDict.TryGetValue(update.RecursoId, out var recurso))
                     {
                         recurso.Ordem = -(i + 1);
                         db.Entry(recurso).State = EntityState.Modified;
@@ -474,7 +462,7 @@
                 Log("FASE 2: Aplicando valores finais...");
                 foreach (var update in updates)
                 {
-                    if (recursosDict.TryGetValue(update.RecursoId , out var recurso))
+                    if (recursosDict.TryGetValue(update.RecursoId, out var recurso))
                     {
                         recurso.ParentId = update.ParentId;
                         recurso.Nivel = update.Nivel;
@@ -499,10 +487,10 @@
                 if (totalRows == 0)
                 {
                     Log("⚠️ Nenhuma linha foi alterada nas duas fases.");
-                    return Json(new { success = false , message = "Nenhuma alteração foi persistida." });
-                }
-
-                return Json(new { success = true , message = $"Navegação salva com sucesso! ({totalRows} registros atualizados)" });
+                    return Json(new { success = false, message = "Nenhuma alteração foi persistida." });
+                }
+
+                return Json(new { success = true, message = $"Navegação salva com sucesso! ({totalRows} registros atualizados)" });
             }
             catch (Exception error)
             {
@@ -520,9 +508,8 @@
                 Log($"❌ ERRO: {errorMessage}");
                 Log($"StackTrace: {error.StackTrace}");
 
-                _log.Error("NavigationController.SaveTreeToDb", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "SaveTreeToDb" , error);
-                return Json(new { success = false , message = errorMessage });
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "SaveTreeToDb", error);
+                return Json(new { success = false, message = errorMessage });
             }
         }
 
@@ -634,7 +621,7 @@
             {
                 if (!System.IO.File.Exists(NavJsonPath))
                 {
-                    return Json(new { success = false , message = "Arquivo nav.json não encontrado!" });
+                    return Json(new { success = false, message = "Arquivo nav.json não encontrado!" });
                 }
 
                 var jsonText = System.IO.File.ReadAllText(NavJsonPath);
@@ -644,14 +631,14 @@
                 int atualizados = 0;
                 int criados = 0;
 
-                ProcessarItensParaMigracao(navigation.Lists , null , 0 , ref ordem , ref atualizados , ref criados);
+                ProcessarItensParaMigracao(navigation.Lists, null, 0, ref ordem, ref atualizados, ref criados);
                 _unitOfWork.Save();
 
                 return Json(new
                 {
-                    success = true ,
-                    message = $"Migração concluída! {criados} recursos criados, {atualizados} atualizados." ,
-                    criados ,
+                    success = true,
+                    message = $"Migração concluída! {criados} recursos criados, {atualizados} atualizados.",
+                    criados,
                     atualizados
                 });
             }
@@ -667,13 +654,13 @@
                         mensagem += " | Inner2: " + error.InnerException.InnerException.Message;
                     }
                 }
-                _log.Error("NavigationController.MigrateFromJson", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "MigrateFromJson" , error);
-                return Json(new { success = false , message = mensagem });
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "MigrateFromJson", error);
+                return Json(new { success = false, message = mensagem });
             }
         }
 
         [HttpPost]
+
         [Route("SaveRecurso")]
         public IActionResult SaveRecurso([FromBody] RecursoTreeDTO dto)
         {
@@ -682,7 +669,7 @@
                 Recurso recurso;
                 bool isNew = false;
 
-                if (!string.IsNullOrEmpty(dto.Id) && Guid.TryParse(dto.Id , out var recursoId))
+                if (!string.IsNullOrEmpty(dto.Id) && Guid.TryParse(dto.Id, out var recursoId))
                 {
                     recurso = _unitOfWork.Recurso.GetFirstOrDefault(r => r.RecursoId == recursoId);
                     if (recurso == null)
@@ -732,7 +719,7 @@
                 recurso.Nivel = dto.Nivel;
                 recurso.Ativo = dto.Ativo;
                 recurso.HasChild = temFilhos;
-                recurso.ParentId = Guid.TryParse(dto.ParentId , out var parentId) ? parentId : null;
+                recurso.ParentId = Guid.TryParse(dto.ParentId, out var parentId) ? parentId : null;
 
                 if (isNew)
                 {
@@ -751,40 +738,40 @@
 
                 return Json(new
                 {
-                    success = true ,
-                    recursoId = recurso.RecursoId ,
+                    success = true,
+                    recursoId = recurso.RecursoId,
                     message = isNew ? "Recurso criado com sucesso!" : "Recurso atualizado com sucesso!"
                 });
             }
             catch (Exception error)
             {
-                _log.Error("NavigationController.SaveRecurso", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "SaveRecurso" , error);
-                return Json(new { success = false , message = error.Message });
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "SaveRecurso", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
         [HttpPost]
+
         [Route("DeleteRecurso")]
         public IActionResult DeleteRecurso([FromBody] DeleteRecursoRequest request)
         {
             try
             {
-                if (!Guid.TryParse(request.RecursoId , out var recursoId))
-                {
-                    return Json(new { success = false , message = "ID do recurso inválido!" });
+                if (!Guid.TryParse(request.RecursoId, out var recursoId))
+                {
+                    return Json(new { success = false, message = "ID do recurso inválido!" });
                 }
 
                 var recurso = _unitOfWork.Recurso.GetFirstOrDefault(r => r.RecursoId == recursoId);
                 if (recurso == null)
                 {
-                    return Json(new { success = false , message = "Recurso não encontrado!" });
+                    return Json(new { success = false, message = "Recurso não encontrado!" });
                 }
 
                 var temFilhos = _unitOfWork.Recurso.GetAll(r => r.ParentId == recursoId).Any();
                 if (temFilhos)
                 {
-                    return Json(new { success = false , message = "Não é possível excluir recurso que possui subitens!" });
+                    return Json(new { success = false, message = "Não é possível excluir recurso que possui subitens!" });
                 }
 
                 var controlesAcesso = _unitOfWork.ControleAcesso.GetAll(ca => ca.RecursoId == recursoId);
@@ -796,57 +783,57 @@
                 _unitOfWork.Recurso.Remove(recurso);
                 _unitOfWork.Save();
 
-                return Json(new { success = true , message = "Recurso removido com sucesso!" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.DeleteRecurso", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "DeleteRecurso" , error);
-                return Json(new { success = false , message = error.Message });
+                return Json(new { success = true, message = "Recurso removido com sucesso!" });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "DeleteRecurso", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
         [HttpGet]
+
         [Route("GetUsuariosAcesso")]
         public IActionResult GetUsuariosAcesso(string recursoId)
         {
             try
             {
-                if (!Guid.TryParse(recursoId , out var recId))
-                {
-                    return Json(new { success = false , message = "ID do recurso inválido!" });
+                if (!Guid.TryParse(recursoId, out var recId))
+                {
+                    return Json(new { success = false, message = "ID do recurso inválido!" });
                 }
 
                 var usuarios = _unitOfWork.AspNetUsers.GetAll(u => u.Status == true)
                     .OrderBy(u => u.NomeCompleto)
                     .Select(u => new
                     {
-                        UsuarioId = u.Id ,
-                        Nome = u.NomeCompleto ?? u.UserName ,
+                        UsuarioId = u.Id,
+                        Nome = u.NomeCompleto ?? u.UserName,
                         Acesso = _unitOfWork.ControleAcesso
                             .GetFirstOrDefault(ca => ca.UsuarioId == u.Id && ca.RecursoId == recId)?.Acesso ?? false
                     })
                     .ToList();
 
-                return Json(new { success = true , data = usuarios });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.GetUsuariosAcesso", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "GetUsuariosAcesso" , error);
-                return Json(new { success = false , message = error.Message });
+                return Json(new { success = true, data = usuarios });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "GetUsuariosAcesso", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
         [HttpPost]
+
         [Route("UpdateAcesso")]
         public IActionResult UpdateAcesso([FromBody] UpdateAcessoRequest request)
         {
             try
             {
-                if (!Guid.TryParse(request.RecursoId , out var recursoId))
-                {
-                    return Json(new { success = false , message = "ID do recurso inválido!" });
+                if (!Guid.TryParse(request.RecursoId, out var recursoId))
+                {
+                    return Json(new { success = false, message = "ID do recurso inválido!" });
                 }
 
                 var controle = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
@@ -856,8 +843,8 @@
                 {
                     controle = new ControleAcesso
                     {
-                        UsuarioId = request.UsuarioId ,
-                        RecursoId = recursoId ,
+                        UsuarioId = request.UsuarioId,
+                        RecursoId = recursoId,
                         Acesso = request.Acesso
                     };
                     _unitOfWork.ControleAcesso.Add(controle);
@@ -870,25 +857,25 @@
 
                 _unitOfWork.Save();
 
-                return Json(new { success = true , message = "Acesso atualizado!" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.UpdateAcesso", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "UpdateAcesso" , error);
-                return Json(new { success = false , message = error.Message });
+                return Json(new { success = true, message = "Acesso atualizado!" });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "UpdateAcesso", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
         [HttpPost]
+
         [Route("HabilitarAcessoTodosUsuarios")]
         public IActionResult HabilitarAcessoTodosUsuarios([FromBody] HabilitarAcessoRequest request)
         {
             try
             {
-                if (!Guid.TryParse(request.RecursoId , out var recursoId))
-                {
-                    return Json(new { success = false , message = "ID do recurso inválido!" });
+                if (!Guid.TryParse(request.RecursoId, out var recursoId))
+                {
+                    return Json(new { success = false, message = "ID do recurso inválido!" });
                 }
 
                 var todosUsuarios = _unitOfWork.AspNetUsers.GetAll();
@@ -904,8 +891,8 @@
 
                         var novoControle = new ControleAcesso
                         {
-                            UsuarioId = usuario.Id ,
-                            RecursoId = recursoId ,
+                            UsuarioId = usuario.Id,
+                            RecursoId = recursoId,
                             Acesso = true
                         };
                         _unitOfWork.ControleAcesso.Add(novoControle);
@@ -914,17 +901,16 @@
 
                 _unitOfWork.Save();
 
-                return Json(new { success = true , message = "Acesso habilitado para todos os usuários!" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.HabilitarAcessoTodosUsuarios", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "HabilitarAcessoTodosUsuarios" , error);
-                return Json(new { success = false , message = error.Message });
-            }
-        }
-
-        private List<RecursoTreeDTO> MontarArvoreRecursiva(List<Recurso> recursos , Guid? parentId)
+                return Json(new { success = true, message = "Acesso habilitado para todos os usuários!" });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "HabilitarAcessoTodosUsuarios", error);
+                return Json(new { success = false, message = error.Message });
+            }
+        }
+
+        private List<RecursoTreeDTO> MontarArvoreRecursiva(List<Recurso> recursos, Guid? parentId)
         {
 
             return recursos
@@ -936,21 +922,21 @@
                 .Select(r =>
                 {
                     var dto = RecursoTreeDTO.FromRecurso(r);
-                    dto.Items = MontarArvoreRecursiva(recursos , r.RecursoId);
+                    dto.Items = MontarArvoreRecursiva(recursos, r.RecursoId);
                     dto.HasChild = dto.Items != null && dto.Items.Any();
                     return dto;
                 })
                 .ToList();
         }
 
-        private void AtualizarRecursosRecursivamente(List<RecursoTreeDTO> items , Guid? parentId , int nivel , double ordemBase)
+        private void AtualizarRecursosRecursivamente(List<RecursoTreeDTO> items, Guid? parentId, int nivel, double ordemBase)
         {
             for (int i = 0; i < items.Count; i++)
             {
                 var item = items[i];
                 double ordemAtual = ordemBase + i;
 
-                if (Guid.TryParse(item.Id , out var recursoId))
+                if (Guid.TryParse(item.Id, out var recursoId))
                 {
                     var recurso = _unitOfWork.Recurso.GetFirstOrDefault(r => r.RecursoId == recursoId);
                     if (recurso != null)
@@ -982,7 +968,7 @@
 
                             double ordemBaseFilhos = ordemAtual * 100;
                             Console.WriteLine($"[AtualizarRecursos] Processando {item.Items.Count} filhos de '{recurso.Nome}'");
-                            AtualizarRecursosRecursivamente(item.Items , recursoId , nivel + 1 , ordemBaseFilhos);
+                            AtualizarRecursosRecursivamente(item.Items, recursoId, nivel + 1, ordemBaseFilhos);
                         }
                     }
                     else
@@ -997,7 +983,7 @@
             }
         }
 
-        private void ProcessarItensParaMigracao(List<ListItem> items , Guid? parentId , int nivel , ref int ordem , ref int atualizados , ref int criados , double ordemPai = 0)
+        private void ProcessarItensParaMigracao(List<ListItem> items, Guid? parentId, int nivel, ref int ordem, ref int atualizados, ref int criados, double ordemPai = 0)
         {
             if (items == null) return;
 
@@ -1014,79 +1000,78 @@
                     var recurso = _unitOfWork.Recurso.GetFirstOrDefault(r => r.NomeMenu == nomeMenuBusca);
                     bool isNew = false;
 
-                    double ordemCalculada;
-                    if (nivel == 0)
-                    {
-                        ordemCalculada = indiceLocal;
-                    }
-                    else
-                    {
-                        ordemCalculada = (ordemPai * 100) + indiceLocal;
-                    }
-
-                    if (recurso == null)
+                double ordemCalculada;
+                if (nivel == 0)
+                {
+                    ordemCalculada = indiceLocal;
+                }
+                else
+                {
+                    ordemCalculada = (ordemPai * 100) + indiceLocal;
+                }
+
+                if (recurso == null)
                     {
 
                         recurso = new Recurso
                         {
-                            RecursoId = Guid.NewGuid() ,
-                            Nome = item.Title ?? item.NomeMenu ?? "Sem Nome" ,
-                            NomeMenu = nomeMenuBusca ?? $"menu_{Guid.NewGuid():N}" ,
-                            Descricao = $"Menu: {nomeMenuBusca}" ,
-                            Ordem = ordemCalculada ,
-                            ParentId = parentId ,
-                            Icon = item.Icon ?? "fa-duotone fa-folder" ,
-                            Href = item.Href ?? "javascript:void(0);" ,
-                            Ativo = true ,
-                            Nivel = nivel ,
-                            HasChild = item.HasChild
-                        };
-                        _unitOfWork.Recurso.Add(recurso);
-                        isNew = true;
-                        criados++;
-                    }
-                    else
-                    {
-
-                        recurso.ParentId = parentId;
-                        recurso.Icon = item.Icon ?? "fa-regular fa-folder";
-                        recurso.Href = item.Href ?? "javascript:void(0);";
-                        recurso.Nivel = nivel;
-                        recurso.Ativo = true;
-                        recurso.Ordem = ordemCalculada;
-                        recurso.HasChild = item.HasChild;
-                        _unitOfWork.Recurso.Update(recurso);
-                        atualizados++;
-                    }
-
+                            RecursoId = Guid.NewGuid(),
+                            Nome = item.Title ?? item.NomeMenu ?? "Sem Nome",
+                            NomeMenu = nomeMenuBusca ?? $"menu_{Guid.NewGuid():N}",
+                            Descricao = $"Menu: {nomeMenuBusca}",
+                        Ordem = ordemCalculada,
+                        ParentId = parentId,
+                        Icon = item.Icon ?? "fa-duotone fa-folder",
+                        Href = item.Href ?? "javascript:void(0);",
+                        Ativo = true,
+                        Nivel = nivel,
+                        HasChild = item.HasChild
+                    };
+                    _unitOfWork.Recurso.Add(recurso);
+                    isNew = true;
+                    criados++;
+                }
+                else
+                {
+
+                    recurso.ParentId = parentId;
+                    recurso.Icon = item.Icon ?? "fa-regular fa-folder";
+                    recurso.Href = item.Href ?? "javascript:void(0);";
+                    recurso.Nivel = nivel;
+                    recurso.Ativo = true;
+                    recurso.Ordem = ordemCalculada;
+                    recurso.HasChild = item.HasChild;
+                    _unitOfWork.Recurso.Update(recurso);
+                    atualizados++;
+                }
+
+                _unitOfWork.Save();
+
+                if (isNew)
+                {
+                    CriarControleAcessoParaTodosUsuarios(recurso.RecursoId);
                     _unitOfWork.Save();
-
-                    if (isNew)
-                    {
-                        CriarControleAcessoParaTodosUsuarios(recurso.RecursoId);
-                        _unitOfWork.Save();
-                    }
-
-                    if (item.Items?.Any() == true)
-                    {
-                        ProcessarItensParaMigracao(item.Items , recurso.RecursoId , nivel + 1 , ref ordem , ref atualizados , ref criados , ordemCalculada);
-                    }
-
+                }
+
+                if (item.Items?.Any() == true)
+                {
+                    ProcessarItensParaMigracao(item.Items, recurso.RecursoId, nivel + 1, ref ordem, ref atualizados, ref criados, ordemCalculada);
+                }
+
+                indiceLocal++;
+                ordem++;
+                }
+                catch (Exception ex)
+                {
+
+                    Console.WriteLine($"Erro ao migrar item '{item.NomeMenu ?? item.Title}': {ex.Message}");
                     indiceLocal++;
                     ordem++;
                 }
-                catch (Exception ex)
-                {
-
-                    Console.WriteLine($"Erro ao migrar item '{item.NomeMenu ?? item.Title}': {ex.Message}");
-                    _log.Error($"Erro ao migrar item '{item.NomeMenu ?? item.Title}'", ex, "NavigationController.cs", "ProcessarItensParaMigracao");
-                    indiceLocal++;
-                    ordem++;
-                }
-            }
-        }
-
-        private List<NavigationTreeItem> TransformToTreeData(List<ListItem> items , string parentId)
+            }
+        }
+
+        private List<NavigationTreeItem> TransformToTreeData(List<ListItem> items, string parentId)
         {
             var result = new List<NavigationTreeItem>();
             int index = 0;
@@ -1099,21 +1084,21 @@
 
                 var treeItem = new NavigationTreeItem
                 {
-                    Id = id ,
-                    Text = item.NomeMenu ?? item.Title ,
-                    Title = item.Title ,
-                    NomeMenu = item.NomeMenu ,
-                    Href = item.Href ,
-                    Icon = item.Icon ,
-                    IconCss = item.Icon ,
-                    ParentId = parentId ,
-                    HasChild = item.Items != null && item.Items.Count > 0 ,
+                    Id = id,
+                    Text = item.NomeMenu ?? item.Title,
+                    Title = item.Title,
+                    NomeMenu = item.NomeMenu,
+                    Href = item.Href,
+                    Icon = item.Icon,
+                    IconCss = item.Icon,
+                    ParentId = parentId,
+                    HasChild = item.Items != null && item.Items.Count > 0,
                     Expanded = true
                 };
 
                 if (item.Items != null && item.Items.Count > 0)
                 {
-                    treeItem.Items = TransformToTreeData(item.Items , id);
+                    treeItem.Items = TransformToTreeData(item.Items, id);
                 }
 
                 result.Add(treeItem);
@@ -1163,30 +1148,30 @@
             if (string.IsNullOrEmpty(text)) return text;
 
             return text
-                .Replace("á" , "&aacute;")
-                .Replace("à" , "&agrave;")
-                .Replace("ã" , "&atilde;")
-                .Replace("â" , "&acirc;")
-                .Replace("é" , "&eacute;")
-                .Replace("ê" , "&ecirc;")
-                .Replace("í" , "&iacute;")
-                .Replace("ó" , "&oacute;")
-                .Replace("ô" , "&ocirc;")
-                .Replace("õ" , "&otilde;")
-                .Replace("ú" , "&uacute;")
-                .Replace("ç" , "&ccedil;")
-                .Replace("Á" , "&Aacute;")
-                .Replace("À" , "&Agrave;")
-                .Replace("Ã" , "&Atilde;")
-                .Replace("Â" , "&Acirc;")
-                .Replace("É" , "&Eacute;")
-                .Replace("Ê" , "&Ecirc;")
-                .Replace("Í" , "&Iacute;")
-                .Replace("Ó" , "&Oacute;")
-                .Replace("Ô" , "&Ocirc;")
-                .Replace("Õ" , "&Otilde;")
-                .Replace("Ú" , "&Uacute;")
-                .Replace("Ç" , "&Ccedil;");
+                .Replace("á", "&aacute;")
+                .Replace("à", "&agrave;")
+                .Replace("ã", "&atilde;")
+                .Replace("â", "&acirc;")
+                .Replace("é", "&eacute;")
+                .Replace("ê", "&ecirc;")
+                .Replace("í", "&iacute;")
+                .Replace("ó", "&oacute;")
+                .Replace("ô", "&ocirc;")
+                .Replace("õ", "&otilde;")
+                .Replace("ú", "&uacute;")
+                .Replace("ç", "&ccedil;")
+                .Replace("Á", "&Aacute;")
+                .Replace("À", "&Agrave;")
+                .Replace("Ã", "&Atilde;")
+                .Replace("Â", "&Acirc;")
+                .Replace("É", "&Eacute;")
+                .Replace("Ê", "&Ecirc;")
+                .Replace("Í", "&Iacute;")
+                .Replace("Ó", "&Oacute;")
+                .Replace("Ô", "&Ocirc;")
+                .Replace("Õ", "&Otilde;")
+                .Replace("Ú", "&Uacute;")
+                .Replace("Ç", "&Ccedil;");
         }
 
         private void SincronizarRecursos(List<NavigationTreeItem> items)
@@ -1203,11 +1188,11 @@
 
                         recurso = new Recurso
                         {
-                            RecursoId = Guid.NewGuid() ,
-                            Nome = item.Title ?? item.Text ,
-                            NomeMenu = item.NomeMenu ,
-                            Descricao = $"Menu: {item.NomeMenu}" ,
-                            Ordem = GetNextOrdem() ,
+                            RecursoId = Guid.NewGuid(),
+                            Nome = item.Title ?? item.Text,
+                            NomeMenu = item.NomeMenu,
+                            Descricao = $"Menu: {item.NomeMenu}",
+                            Ordem = GetNextOrdem(),
                             HasChild = item.HasChild
                         };
                         _unitOfWork.Recurso.Add(recurso);
@@ -1250,8 +1235,8 @@
                 {
                     var novoControle = new ControleAcesso
                     {
-                        UsuarioId = usuario.Id ,
-                        RecursoId = recursoId ,
+                        UsuarioId = usuario.Id,
+                        RecursoId = recursoId,
                         Acesso = true
                     };
                     _unitOfWork.ControleAcesso.Add(novoControle);
@@ -1260,34 +1245,33 @@
         }
 
         [HttpGet]
+
         [Route("GetIconesFontAwesomeHierarquico")]
         public IActionResult GetIconesFontAwesomeHierarquico()
         {
             try
             {
 
-                if (_cache.TryGetValue(CacheKeyFontAwesomeIcons , out List<object> cachedIcons))
-                {
-                    return Json(new { success = true , data = cachedIcons });
+                if (_cache.TryGetValue(CacheKeyFontAwesomeIcons, out List<object> cachedIcons))
+                {
+                    return Json(new { success = true, data = cachedIcons });
                 }
 
                 var icons = LoadFontAwesomeIconsFromJson();
 
                 var cacheOptions = new MemoryCacheEntryOptions
                 {
-                    AbsoluteExpirationRelativeToNow = CacheDuration ,
-                    Priority = CacheItemPriority.Normal ,
-                    Size = 1
+                    AbsoluteExpirationRelativeToNow = CacheDuration,
+                    Priority = CacheItemPriority.Normal
                 };
-                _cache.Set(CacheKeyFontAwesomeIcons , icons , cacheOptions);
-
-                return Json(new { success = true , data = icons });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.GetIconesFontAwesomeHierarquico", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "GetIconesFontAwesomeHierarquico" , error);
-                return Json(new { success = false , message = error.Message });
+                _cache.Set(CacheKeyFontAwesomeIcons, icons, cacheOptions);
+
+                return Json(new { success = true, data = icons });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "GetIconesFontAwesomeHierarquico", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
@@ -1314,21 +1298,21 @@
                     .OrderBy(i => i.Label)
                     .Select(i => new
                     {
-                        id = i.Id ,
-                        text = i.Label ,
-                        name = i.Name ,
-                        parentId = catId ,
+                        id = i.Id,
+                        text = i.Label,
+                        name = i.Name,
+                        parentId = catId,
                         keywords = i.Keywords
                     })
                     .ToList<object>();
 
                 result.Add(new
                 {
-                    id = catId ,
-                    text = categoria.Categoria ,
-                    isCategory = true ,
-                    hasChild = sortedIcons.Count > 0 ,
-                    expanded = false ,
+                    id = catId,
+                    text = categoria.Categoria,
+                    isCategory = true,
+                    hasChild = sortedIcons.Count > 0,
+                    expanded = false,
                     child = sortedIcons
                 });
             }
@@ -1337,6 +1321,7 @@
         }
 
         [HttpGet]
+
         [Route("GetPaginasHierarquico")]
         public IActionResult GetPaginasHierarquico()
         {
@@ -1344,34 +1329,32 @@
             {
                 const string cacheKey = "PaginasHierarquicas";
 
-                if (_cache.TryGetValue(cacheKey , out List<object> cachedPages))
-                {
-                    return Json(new { success = true , data = cachedPages });
+                if (_cache.TryGetValue(cacheKey, out List<object> cachedPages))
+                {
+                    return Json(new { success = true, data = cachedPages });
                 }
 
                 var paginas = LoadPaginasFromFileSystem();
 
                 var cacheOptions = new MemoryCacheEntryOptions
                 {
-                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) ,
-                    Priority = CacheItemPriority.Normal ,
-                    Size = 1
+                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
+                    Priority = CacheItemPriority.Normal
                 };
-                _cache.Set(cacheKey , paginas , cacheOptions);
-
-                return Json(new { success = true , data = paginas });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.GetPaginasHierarquico", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "GetPaginasHierarquico" , error);
-                return Json(new { success = false , message = error.Message });
+                _cache.Set(cacheKey, paginas, cacheOptions);
+
+                return Json(new { success = true, data = paginas });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "GetPaginasHierarquico", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
         private List<object> LoadPaginasFromFileSystem()
         {
-            var pagesPath = Path.Combine(_env.ContentRootPath , "Pages");
+            var pagesPath = Path.Combine(_env.ContentRootPath, "Pages");
 
             if (!Directory.Exists(pagesPath))
             {
@@ -1382,7 +1365,7 @@
 
             var moduleDirs = Directory.GetDirectories(pagesPath)
                 .Select(d => new DirectoryInfo(d))
-                .Where(d => !d.Name.StartsWith("_") && !d.Name.Equals("Shared" , StringComparison.OrdinalIgnoreCase))
+                .Where(d => !d.Name.StartsWith("_") && !d.Name.Equals("Shared", StringComparison.OrdinalIgnoreCase))
                 .OrderBy(d => d.Name)
                 .ToList();
 
@@ -1391,7 +1374,7 @@
                 var moduleName = moduleDir.Name;
                 var moduleId = $"module_{moduleName}";
 
-                var pageFiles = moduleDir.GetFiles("*.cshtml" , SearchOption.TopDirectoryOnly)
+                var pageFiles = moduleDir.GetFiles("*.cshtml", SearchOption.TopDirectoryOnly)
                     .Where(f => !f.Name.StartsWith("_"))
                     .OrderBy(f => f.Name)
                     .ToList();
@@ -1408,23 +1391,23 @@
 
                     return new
                     {
-                        id = pageId ,
-                        text = pageName ,
-                        displayText = $"({moduloAmigavel}) {pageName}" ,
-                        paginaRef = paginaRef ,
-                        pageName = pageName ,
-                        moduleName = moduleName ,
+                        id = pageId,
+                        text = pageName,
+                        displayText = $"({moduloAmigavel}) {pageName}",
+                        paginaRef = paginaRef,
+                        pageName = pageName,
+                        moduleName = moduleName,
                         parentId = moduleId
                     };
                 }).ToList<object>();
 
                 result.Add(new
                 {
-                    id = moduleId ,
-                    text = GetFriendlyModuleName(moduleName) ,
-                    isCategory = true ,
-                    hasChild = children.Count > 0 ,
-                    expanded = false ,
+                    id = moduleId,
+                    text = GetFriendlyModuleName(moduleName),
+                    isCategory = true,
+                    hasChild = children.Count > 0,
+                    expanded = false,
                     child = children
                 });
             }
@@ -1436,23 +1419,23 @@
         {
             return pageName switch
             {
-                "Index" => "Listar" ,
-                "Upsert" => "Criar/Editar" ,
-                "UploadCNH" => "Upload CNH" ,
-                "UploadCRLV" => "Upload CRLV" ,
-                "UploadPDF" => "Upload PDF" ,
-                "DashboardAbastecimento" => "Dashboard" ,
-                "DashboardVeiculos" => "Dashboard" ,
-                "DashboardMotoristas" => "Dashboard" ,
-                "DashboardViagens" => "Dashboard" ,
-                "DashboardLavagem" => "Dashboard" ,
-                "DashboardEventos" => "Dashboard" ,
-                "DashboardEconomildo" => "Dashboard Economildo" ,
-                "DashboardAdministracao" => "Dashboard" ,
-                "PBI" => "Power BI" ,
-                "PBILotacaoMotorista" => "Power BI - Lotação" ,
-                "PBILavagem" => "Power BI - Lavagem" ,
-                "PBITaxiLeg" => "Power BI - Taxi Leg" ,
+                "Index" => "Listar",
+                "Upsert" => "Criar/Editar",
+                "UploadCNH" => "Upload CNH",
+                "UploadCRLV" => "Upload CRLV",
+                "UploadPDF" => "Upload PDF",
+                "DashboardAbastecimento" => "Dashboard",
+                "DashboardVeiculos" => "Dashboard",
+                "DashboardMotoristas" => "Dashboard",
+                "DashboardViagens" => "Dashboard",
+                "DashboardLavagem" => "Dashboard",
+                "DashboardEventos" => "Dashboard",
+                "DashboardEconomildo" => "Dashboard Economildo",
+                "DashboardAdministracao" => "Dashboard",
+                "PBI" => "Power BI",
+                "PBILotacaoMotorista" => "Power BI - Lotação",
+                "PBILavagem" => "Power BI - Lavagem",
+                "PBITaxiLeg" => "Power BI - Taxi Leg",
                 _ => pageName
             };
         }
@@ -1461,22 +1444,23 @@
         {
             return moduleName switch
             {
-                "Administracao" => "Administração" ,
-                "AlertasFrotiX" => "Alertas FrotiX" ,
-                "AtaRegistroPrecos" => "Ata de Registro de Preços" ,
-                "Combustivel" => "Combustível" ,
-                "Manutencao" => "Manutenção" ,
-                "MovimentacaoPatrimonio" => "Movimentação de Patrimônio" ,
-                "SecaoPatrimonial" => "Seções Patrimoniais" ,
-                "SetorPatrimonial" => "Setores Patrimoniais" ,
-                "SetorSolicitante" => "Setores Solicitantes" ,
-                "Usuarios" => "Usuários" ,
-                "Veiculo" => "Veículos" ,
+                "Administracao" => "Administração",
+                "AlertasFrotiX" => "Alertas FrotiX",
+                "AtaRegistroPrecos" => "Ata de Registro de Preços",
+                "Combustivel" => "Combustível",
+                "Manutencao" => "Manutenção",
+                "MovimentacaoPatrimonio" => "Movimentação de Patrimônio",
+                "SecaoPatrimonial" => "Seções Patrimoniais",
+                "SetorPatrimonial" => "Setores Patrimoniais",
+                "SetorSolicitante" => "Setores Solicitantes",
+                "Usuarios" => "Usuários",
+                "Veiculo" => "Veículos",
                 _ => moduleName
             };
         }
 
         [HttpGet]
+
         [Route("GetNavigationMenu")]
         public async Task<IActionResult> GetNavigationMenu()
         {
@@ -1489,16 +1473,15 @@
                 {
 
                     var htmlString = await RenderViewComponentToStringAsync(viewResult);
-                    return Json(new { success = true , html = htmlString });
-                }
-
-                return Json(new { success = false , message = "Erro ao renderizar menu de navegação" });
-            }
-            catch (Exception error)
-            {
-                _log.Error("NavigationController.GetNavigationMenu", error );
-                Alerta.TratamentoErroComLinha("NavigationController.cs" , "GetNavigationMenu" , error);
-                return Json(new { success = false , message = error.Message });
+                    return Json(new { success = true, html = htmlString });
+                }
+
+                return Json(new { success = false, message = "Erro ao renderizar menu de navegação" });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("NavigationController.cs", "GetNavigationMenu", error);
+                return Json(new { success = false, message = error.Message });
             }
         }
 
```
