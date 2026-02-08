# Controllers/SetorSolicitanteController.UpdateStatus.cs

**Mudanca:** PEQUENA | **+0** linhas | **-3** linhas

---

```diff
--- JANEIRO: Controllers/SetorSolicitanteController.UpdateStatus.cs
+++ ATUAL: Controllers/SetorSolicitanteController.UpdateStatus.cs
@@ -31,9 +31,6 @@
                 _unitOfWork.SetorSolicitante.Update(setor);
                 _unitOfWork.Save();
 
-                var statusMsg = setor.Status ? "Ativo" : "Inativo";
-                _log.Info($"Atualizado Status do Setor Solicitante [Nome: {setor.Nome}] ({statusMsg})");
-
                 return Json(new
                 {
                     success = true,
@@ -44,7 +41,6 @@
             catch (Exception error)
             {
                 Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "UpdateStatus", error);
-                _log.Error($", errorErro ao alternar status do setor solicitante [ID: {id}]");
                 return Json(new { success = false, message = "Erro ao alterar status do setor" });
             }
         }
```

### REMOVER do Janeiro

```csharp
                var statusMsg = setor.Status ? "Ativo" : "Inativo";
                _log.Info($"Atualizado Status do Setor Solicitante [Nome: {setor.Nome}] ({statusMsg})");
                _log.Error($", errorErro ao alternar status do setor solicitante [ID: {id}]");
```

