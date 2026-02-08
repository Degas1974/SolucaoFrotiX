# Controllers/NotaFiscalController.Partial.cs

**Mudanca:** MEDIA | **+8** linhas | **-17** linhas

---

```diff
--- JANEIRO: Controllers/NotaFiscalController.Partial.cs
+++ ATUAL: Controllers/NotaFiscalController.Partial.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Linq;
@@ -18,7 +17,6 @@
         {
             try
             {
-
                 if (model == null)
                 {
                     return Json(new { success = false, message = "Dados inválidos" });
@@ -49,12 +47,8 @@
                 var empenho = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                 if (empenho != null)
                 {
-
-                    double valorDebitado = (model.ValorNF ?? 0) - (model.ValorGlosa ?? 0);
-                    empenho.SaldoFinal = empenho.SaldoFinal - valorDebitado;
+                    empenho.SaldoFinal = empenho.SaldoFinal - (model.ValorNF - model.ValorGlosa);
                     _unitOfWork.Empenho.Update(empenho);
-
-                    _log.Info($"NotaFiscalController.Insere: NF {model.NumeroNF} cadastrada. Valor líquido debitado do empenho {empenho.EmpenhoId}: {valorDebitado:C}");
                 }
 
                 _unitOfWork.NotaFiscal.Add(model);
@@ -63,18 +57,17 @@
                 return Json(new
                 {
                     success = true,
-                    message = "Nota Fiscal cadastrada with sucesso!",
+                    message = "Nota Fiscal cadastrada com sucesso!",
                     notaFiscalId = model.NotaFiscalId
                 });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("NotaFiscalController.Insere", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.Partial.cs", "Insere", ex);
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Insere", error);
                 return Json(new
                 {
                     success = false,
-                    message = "Erro ao cadastrar Nota Fiscal: " + ex.Message
+                    message = "Erro ao cadastrar Nota Fiscal: " + error.Message
                 });
             }
         }
@@ -86,7 +79,6 @@
         {
             try
             {
-
                 if (model == null || model.NotaFiscalId == Guid.Empty)
                 {
                     return Json(new { success = false, message = "Dados inválidos" });
@@ -113,7 +105,6 @@
 
                 if (objFromDb == null)
                 {
-
                     return Json(new { success = false, message = "Nota Fiscal não encontrada" });
                 }
 
@@ -132,7 +123,6 @@
                         {
                             empenhoAntigo.SaldoFinal = empenhoAntigo.SaldoFinal + valorAntigoLiquido;
                             _unitOfWork.Empenho.Update(empenhoAntigo);
-                            _log.Info($"NotaFiscalController.Edita: Estornando {valorAntigoLiquido:C} para empenho antigo {empenhoAntigo.EmpenhoId} devido a troca de empenho na NF {model.NumeroNF}");
                         }
 
                         var empenhoNovo = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
@@ -140,7 +130,6 @@
                         {
                             empenhoNovo.SaldoFinal = empenhoNovo.SaldoFinal - valorNovoLiquido;
                             _unitOfWork.Empenho.Update(empenhoNovo);
-                            _log.Info($"NotaFiscalController.Edita: Debitando {valorNovoLiquido:C} do novo empenho {empenhoNovo.EmpenhoId} para a NF {model.NumeroNF}");
                         }
                     }
                     else
@@ -151,7 +140,6 @@
                         {
                             empenho.SaldoFinal = empenho.SaldoFinal - diferencaValor;
                             _unitOfWork.Empenho.Update(empenho);
-                            _log.Info($"NotaFiscalController.Edita: Ajustando saldo do empenho {empenho.EmpenhoId} em {diferencaValor:C} devido a alteração na NF {model.NumeroNF}");
                         }
                     }
                 }
@@ -171,7 +159,6 @@
                         empenhoNovo.SaldoFinal = empenhoNovo.SaldoFinal - valorNovoLiquido;
                         _unitOfWork.Empenho.Update(empenhoNovo);
                     }
-                    _log.Info($"NotaFiscalController.Edita: Troca de empenho da NF {model.NumeroNF} sem alteração de valor. Valor: {valorNovoLiquido:C}");
                 }
 
                 objFromDb.NumeroNF = model.NumeroNF;
@@ -196,14 +183,13 @@
                     message = "Nota Fiscal atualizada com sucesso!"
                 });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("NotaFiscalController.Edita", ex);
-                Alerta.TratamentoErroComLinha("NotaFiscalController.Partial.cs", "Edita", ex);
+                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Edita", error);
                 return Json(new
                 {
                     success = false,
-                    message = "Erro ao atualizar Nota Fiscal: " + ex.Message
+                    message = "Erro ao atualizar Nota Fiscal: " + error.Message
                 });
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
                    double valorDebitado = (model.ValorNF ?? 0) - (model.ValorGlosa ?? 0);
                    empenho.SaldoFinal = empenho.SaldoFinal - valorDebitado;
                    _log.Info($"NotaFiscalController.Insere: NF {model.NumeroNF} cadastrada. Valor líquido debitado do empenho {empenho.EmpenhoId}: {valorDebitado:C}");
                    message = "Nota Fiscal cadastrada with sucesso!",
            catch (Exception ex)
                _log.Error("NotaFiscalController.Insere", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.Partial.cs", "Insere", ex);
                    message = "Erro ao cadastrar Nota Fiscal: " + ex.Message
                            _log.Info($"NotaFiscalController.Edita: Estornando {valorAntigoLiquido:C} para empenho antigo {empenhoAntigo.EmpenhoId} devido a troca de empenho na NF {model.NumeroNF}");
                            _log.Info($"NotaFiscalController.Edita: Debitando {valorNovoLiquido:C} do novo empenho {empenhoNovo.EmpenhoId} para a NF {model.NumeroNF}");
                            _log.Info($"NotaFiscalController.Edita: Ajustando saldo do empenho {empenho.EmpenhoId} em {diferencaValor:C} devido a alteração na NF {model.NumeroNF}");
                    _log.Info($"NotaFiscalController.Edita: Troca de empenho da NF {model.NumeroNF} sem alteração de valor. Valor: {valorNovoLiquido:C}");
            catch (Exception ex)
                _log.Error("NotaFiscalController.Edita", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.Partial.cs", "Edita", ex);
                    message = "Erro ao atualizar Nota Fiscal: " + ex.Message
```


### ADICIONAR ao Janeiro

```csharp
                    empenho.SaldoFinal = empenho.SaldoFinal - (model.ValorNF - model.ValorGlosa);
                    message = "Nota Fiscal cadastrada com sucesso!",
            catch (Exception error)
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Insere", error);
                    message = "Erro ao cadastrar Nota Fiscal: " + error.Message
            catch (Exception error)
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Edita", error);
                    message = "Erro ao atualizar Nota Fiscal: " + error.Message
```
