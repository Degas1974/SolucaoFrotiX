# Controllers/AbastecimentoController.Pendencias.cs

**Mudanca:** GRANDE | **+46** linhas | **-70** linhas

---

```diff
--- JANEIRO: Controllers/AbastecimentoController.Pendencias.cs
+++ ATUAL: Controllers/AbastecimentoController.Pendencias.cs
@@ -6,14 +6,13 @@
 using System.Globalization;
 using System.Linq;
 using System.Text.Json.Serialization;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     public partial class AbastecimentoController : ControllerBase
     {
+
         public class PendenciaDTO
         {
             [JsonPropertyName("abastecimentoPendenteId")]
@@ -133,8 +132,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro ao listar pendências de abastecimento", error, "AbastecimentoController.Pendencias.cs", "ListarPendencias");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ListarPendencias", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ListarPendencias", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -165,8 +163,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro ao contabilizar pendências", error, "AbastecimentoController.Pendencias.cs", "ContarPendencias");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ContarPendencias", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ContarPendencias", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -215,8 +212,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($"Erro ao recuperar pendência {id}", error, "AbastecimentoController.Pendencias.cs", "ObterPendencia");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ObterPendencia", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ObterPendencia", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -227,6 +223,7 @@
         {
             try
             {
+
                 ModelState.Remove("Veiculo");
                 ModelState.Remove("Motorista");
                 ModelState.Remove("Combustivel");
@@ -252,6 +249,7 @@
 
                     if (existenteAbastecimento != null)
                     {
+
                         pendencia.Status = 1;
                         _unitOfWork.AbastecimentoPendente.Update(pendencia);
                         _unitOfWork.Save();
@@ -292,8 +290,6 @@
 
                 double consumoFinal = request.Litros > 0 ? kmRodado / request.Litros.Value : 0;
 
-                _log.Info($"Pendência {request.AbastecimentoPendenteId} resolvida com sucesso", "AbastecimentoController.Pendencias.cs", "ResolverPendencia");
-
                 return Ok(new
                 {
                     success = true,
@@ -302,8 +298,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($"Erro ao resolver pendência {request.AbastecimentoPendenteId}", error, "AbastecimentoController.Pendencias.cs", "ResolverPendencia");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ResolverPendencia", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ResolverPendencia", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -314,6 +309,7 @@
         {
             try
             {
+
                 ModelState.Remove("Veiculo");
                 ModelState.Remove("Motorista");
                 ModelState.Remove("Combustivel");
@@ -376,8 +372,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($"Erro ao salvar alterações da pendência {request.AbastecimentoPendenteId}", error, "AbastecimentoController.Pendencias.cs", "SalvarPendencia");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "SalvarPendencia", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "SalvarPendencia", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -388,6 +383,7 @@
         {
             try
             {
+
                 ModelState.Remove("Veiculo");
                 ModelState.Remove("Motorista");
                 ModelState.Remove("Combustivel");
@@ -402,8 +398,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($"Erro no fluxo Salvar+Importar pendência {request.AbastecimentoPendenteId}", error, "AbastecimentoController.Pendencias.cs", "SalvarEImportarPendencia");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "SalvarEImportarPendencia", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "SalvarEImportarPendencia", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -414,6 +409,7 @@
         {
             try
             {
+
                 ModelState.Remove("Veiculo");
                 ModelState.Remove("Motorista");
                 ModelState.Remove("Combustivel");
@@ -433,8 +429,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($"Erro ao excluir pendência {request.AbastecimentoPendenteId}", error, "AbastecimentoController.Pendencias.cs", "ExcluirPendencia");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ExcluirPendencia", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ExcluirPendencia", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -463,14 +458,11 @@
 
                 _unitOfWork.Save();
 
-                _log.Info($"{quantidade} pendências excluídas manualmente", "AbastecimentoController.Pendencias.cs", "ExcluirTodasPendencias");
-
                 return Ok(new { success = true, message = $"{quantidade} pendência(s) excluída(s) com sucesso" });
             }
             catch (Exception error)
             {
-                _log.Error("Erro ao excluir todas as pendências", error, "AbastecimentoController.Pendencias.cs", "ExcluirTodasPendencias");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ExcluirTodasPendencias", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ExcluirTodasPendencias", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -481,6 +473,7 @@
         {
             try
             {
+
                 ModelState.Remove("Veiculo");
                 ModelState.Remove("Motorista");
                 ModelState.Remove("Combustivel");
@@ -500,11 +493,11 @@
 
                 if (pendencia.CampoCorrecao == "KmAnterior")
                 {
-                    pendencia.KmAnterior = (int?)pendencia.ValorSugerido;
+                    pendencia.KmAnterior = pendencia.ValorSugerido;
                 }
                 else if (pendencia.CampoCorrecao == "Km")
                 {
-                    pendencia.Km = (int?)pendencia.ValorSugerido;
+                    pendencia.Km = pendencia.ValorSugerido;
                 }
 
                 pendencia.KmRodado = (pendencia.Km ?? 0) - (pendencia.KmAnterior ?? 0);
@@ -522,8 +515,6 @@
                 _unitOfWork.AbastecimentoPendente.Update(pendencia);
                 _unitOfWork.Save();
 
-                _log.Info($"Sugestão aplicada na pendência {request.AbastecimentoPendenteId}", "AbastecimentoController.Pendencias.cs", "AplicarSugestao");
-
                 return Ok(new
                 {
                     success = true,
@@ -534,8 +525,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($"Erro ao aplicar sugestão na pendência {request.AbastecimentoPendenteId}", error, "AbastecimentoController.Pendencias.cs", "AplicarSugestao");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "AplicarSugestao", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AplicarSugestao", error);
                 return StatusCode(500, new { success = false, message = error.Message });
             }
         }
@@ -544,13 +534,13 @@
         {
             return tipo switch
             {
-                "autorizacao" => "fa-duotone fa-ban",
-                "motorista" => "fa-duotone fa-user-xmark",
-                "veiculo" => "fa-duotone fa-car-burst",
-                "litros" => "fa-duotone fa-gas-pump",
-                "km" => "fa-duotone fa-gauge-high",
-                "data" => "fa-duotone fa-calendar-xmark",
-                _ => "fa-duotone fa-circle-xmark"
+                "autorizacao" => "fa-ban",
+                "motorista" => "fa-user-xmark",
+                "veiculo" => "fa-car-burst",
+                "litros" => "fa-gas-pump",
+                "km" => "fa-gauge-high",
+                "data" => "fa-calendar-xmark",
+                _ => "fa-circle-xmark"
             };
         }
 
@@ -647,50 +637,41 @@
 
         private (bool success, string message) SalvarPendenciaInterno(EditarPendenciaRequest request)
         {
-            try
-            {
-                var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
-                    p.AbastecimentoPendenteId == Guid.Parse(request.AbastecimentoPendenteId));
-
-                if (pendencia == null)
-                {
-                    return (false, "Pendência não encontrada");
-                }
-
-                pendencia.AutorizacaoQCard = request.AutorizacaoQCard;
-                pendencia.Placa = request.Placa;
-                pendencia.CodMotorista = request.CodMotorista;
-                pendencia.Litros = request.Litros;
-                pendencia.ValorUnitario = request.ValorUnitario;
-                pendencia.KmAnterior = request.KmAnterior;
-                pendencia.Km = request.Km;
-                pendencia.KmRodado = (request.Km ?? 0) - (request.KmAnterior ?? 0);
-
-                if (!string.IsNullOrEmpty(request.VeiculoId))
-                    pendencia.VeiculoId = Guid.Parse(request.VeiculoId);
-
-                if (!string.IsNullOrEmpty(request.MotoristaId))
-                    pendencia.MotoristaId = Guid.Parse(request.MotoristaId);
-
-                if (!string.IsNullOrEmpty(request.CombustivelId))
-                    pendencia.CombustivelId = Guid.Parse(request.CombustivelId);
-
-                if (DateTime.TryParse(request.DataHora, out DateTime dataHora))
-                {
-                    pendencia.DataHora = dataHora;
-                }
-
-                _unitOfWork.AbastecimentoPendente.Update(pendencia);
-                _unitOfWork.Save();
-
-                return (true, "Pendência atualizada");
-            }
-            catch (Exception error)
-            {
-                _log.Error($"Erro interno ao salvar pendência {request.AbastecimentoPendenteId}", error, "AbastecimentoController.Pendencias.cs", "SalvarPendenciaInterno");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "SalvarPendenciaInterno", error);
-                throw;
-            }
+            var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
+                p.AbastecimentoPendenteId == Guid.Parse(request.AbastecimentoPendenteId));
+
+            if (pendencia == null)
+            {
+                return (false, "Pendência não encontrada");
+            }
+
+            pendencia.AutorizacaoQCard = request.AutorizacaoQCard;
+            pendencia.Placa = request.Placa;
+            pendencia.CodMotorista = request.CodMotorista;
+            pendencia.Litros = request.Litros;
+            pendencia.ValorUnitario = request.ValorUnitario;
+            pendencia.KmAnterior = request.KmAnterior;
+            pendencia.Km = request.Km;
+            pendencia.KmRodado = (request.Km ?? 0) - (request.KmAnterior ?? 0);
+
+            if (!string.IsNullOrEmpty(request.VeiculoId))
+                pendencia.VeiculoId = Guid.Parse(request.VeiculoId);
+
+            if (!string.IsNullOrEmpty(request.MotoristaId))
+                pendencia.MotoristaId = Guid.Parse(request.MotoristaId);
+
+            if (!string.IsNullOrEmpty(request.CombustivelId))
+                pendencia.CombustivelId = Guid.Parse(request.CombustivelId);
+
+            if (DateTime.TryParse(request.DataHora, out DateTime dataHora))
+            {
+                pendencia.DataHora = dataHora;
+            }
+
+            _unitOfWork.AbastecimentoPendente.Update(pendencia);
+            _unitOfWork.Save();
+
+            return (true, "Pendência atualizada");
         }
 
         private void AtualizarMediaConsumoVeiculo(Guid veiculoId)
@@ -715,8 +696,7 @@
             }
             catch (Exception error)
             {
-                _log.Error($"Erro ao atualizar média de consumo do veículo {veiculoId}", error, "AbastecimentoController.Pendencias.cs", "AtualizarMediaConsumoVeiculo");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "AtualizarMediaConsumoVeiculo", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AtualizarMediaConsumoVeiculo", error);
             }
         }
     }
```
