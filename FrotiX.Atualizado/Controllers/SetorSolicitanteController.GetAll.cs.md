# Controllers/SetorSolicitanteController.GetAll.cs

**Mudanca:** MEDIA | **+0** linhas | **-6** linhas

---

```diff
--- JANEIRO: Controllers/SetorSolicitanteController.GetAll.cs
+++ ATUAL: Controllers/SetorSolicitanteController.GetAll.cs
@@ -32,7 +32,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetAll", error);
-                _log.Error("Erro ao listar setores solicitantes (Hierarquia)", error);
                 return Json(new { success = false, message = "Erro ao listar setores solicitantes" });
             }
         }
@@ -96,7 +95,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetById", error);
-                _log.Error($", errorErro ao buscar setor solicitante [ID: {id}]");
                 return Json(new { success = false, message = "Erro ao buscar setor solicitante" });
             }
         }
@@ -127,13 +125,13 @@
                         Sigla = model.Sigla,
                         Ramal = model.Ramal,
                         Status = model.Status,
+
                         SetorPaiId = !string.IsNullOrEmpty(model.SetorPaiId) && Guid.TryParse(model.SetorPaiId, out Guid paiId) && paiId != Guid.Empty
                             ? paiId
                             : (Guid?)null,
                         DataAlteracao = DateTime.Now
                     };
                     _unitOfWork.SetorSolicitante.Add(setor);
-                    _log.Info($"Criado novo setor solicitante: [Nome: {model.Nome}] [Sigla: {model.Sigla}]");
                 }
                 else
                 {
@@ -156,7 +154,6 @@
                     setor.DataAlteracao = DateTime.Now;
 
                     _unitOfWork.SetorSolicitante.Update(setor);
-                    _log.Info($"Atualizado setor solicitante: [ID: {model.SetorSolicitanteId}] [Nome: {model.Nome}]");
                 }
 
                 _unitOfWork.Save();
@@ -170,7 +167,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "Upsert", error);
-                _log.Error($", errorErro ao realizar Upsert de setor solicitante [Nome: {model?.Nome}]");
                 return Json(new { success = false, message = "Erro ao salvar setor solicitante" });
             }
         }
@@ -203,7 +199,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetSetoresPai", error);
-                _log.Error("Erro ao buscar lista de setores pai", error);
                 return Json(new List<object>());
             }
         }
```

### REMOVER do Janeiro

```csharp
                _log.Error("Erro ao listar setores solicitantes (Hierarquia)", error);
                _log.Error($", errorErro ao buscar setor solicitante [ID: {id}]");
                    _log.Info($"Criado novo setor solicitante: [Nome: {model.Nome}] [Sigla: {model.Sigla}]");
                    _log.Info($"Atualizado setor solicitante: [ID: {model.SetorSolicitanteId}] [Nome: {model.Nome}]");
                _log.Error($", errorErro ao realizar Upsert de setor solicitante [Nome: {model?.Nome}]");
                _log.Error("Erro ao buscar lista de setores pai", error);
```

