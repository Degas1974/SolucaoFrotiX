# Controllers/ItensContratoController.cs

**Mudanca:** GRANDE | **+1** linhas | **-33** linhas

---

```diff
--- JANEIRO: Controllers/ItensContratoController.cs
+++ ATUAL: Controllers/ItensContratoController.cs
@@ -5,24 +5,20 @@
 using System;
 using System.Collections.Generic;
 using System.Linq;
-using FrotiX.Helpers;
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
     public partial class ItensContratoController : ControllerBase
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
-
-        public ItensContratoController(IUnitOfWork unitOfWork, ILogService log)
+
+        public ItensContratoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
@@ -36,7 +32,6 @@
         {
             try
             {
-
                 var contratos = _unitOfWork.Contrato.GetAll(
                     filter: c => c.Status == status,
                     includeProperties: "Fornecedor"
@@ -55,7 +50,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "ListaContratos");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "ListaContratos", error);
                 return Ok(new { success = false, message = "Erro ao carregar contratos" });
             }
@@ -67,7 +61,6 @@
         {
             try
             {
-
                 var atas = _unitOfWork.AtaRegistroPrecos.GetAll(
                     filter: a => a.Status == status,
                     includeProperties: "Fornecedor"
@@ -85,7 +78,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "ListaAtas");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "ListaAtas", error);
                 return Ok(new { success = false, message = "Erro ao carregar atas" });
             }
@@ -97,7 +89,6 @@
         {
             try
             {
-
                 var contrato = _unitOfWork.Contrato.GetFirstOrDefault(
                     filter: c => c.ContratoId == id,
                     includeProperties: "Fornecedor"
@@ -105,7 +96,6 @@
 
                 if (contrato == null)
                 {
-
                     return Ok(new { success = false, message = "Contrato não encontrado" });
                 }
 
@@ -143,7 +133,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetContratoDetalhes");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetContratoDetalhes", error);
                 return Ok(new { success = false, message = "Erro ao carregar detalhes do contrato" });
             }
@@ -155,7 +144,6 @@
         {
             try
             {
-
                 var ata = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(
                     filter: a => a.AtaId == id,
                     includeProperties: "Fornecedor"
@@ -163,7 +151,6 @@
 
                 if (ata == null)
                 {
-
                     return Ok(new { success = false, message = "Ata não encontrada" });
                 }
 
@@ -185,7 +172,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetAtaDetalhes");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetAtaDetalhes", error);
                 return Ok(new { success = false, message = "Erro ao carregar detalhes da ata" });
             }
@@ -262,7 +248,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosContrato", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -293,7 +278,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosDisponiveis");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosDisponiveis", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -333,7 +317,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetItensContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetItensContrato", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -378,7 +361,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirVeiculoContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirVeiculoContrato", error);
                 return Ok(new { success = false, message = "Erro ao incluir veículo no contrato" });
             }
@@ -415,7 +397,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverVeiculoContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverVeiculoContrato", error);
                 return Ok(new { success = false, message = "Erro ao remover veículo do contrato" });
             }
@@ -446,7 +427,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetEncarregadosContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetEncarregadosContrato", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -473,7 +453,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetEncarregadosDisponiveis");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetEncarregadosDisponiveis", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -507,7 +486,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirEncarregadoContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirEncarregadoContrato", error);
                 return Ok(new { success = false, message = "Erro ao incluir encarregado no contrato" });
             }
@@ -536,7 +514,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverEncarregadoContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverEncarregadoContrato", error);
                 return Ok(new { success = false, message = "Erro ao remover encarregado do contrato" });
             }
@@ -567,7 +544,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetOperadoresContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetOperadoresContrato", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -594,7 +570,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetOperadoresDisponiveis");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetOperadoresDisponiveis", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -628,7 +603,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirOperadorContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirOperadorContrato", error);
                 return Ok(new { success = false, message = "Erro ao incluir operador no contrato" });
             }
@@ -657,7 +631,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverOperadorContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverOperadorContrato", error);
                 return Ok(new { success = false, message = "Erro ao remover operador do contrato" });
             }
@@ -689,7 +662,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetMotoristasContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetMotoristasContrato", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -716,7 +688,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetMotoristasDisponiveis");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetMotoristasDisponiveis", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -750,7 +721,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirMotoristaContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirMotoristaContrato", error);
                 return Ok(new { success = false, message = "Erro ao incluir motorista no contrato" });
             }
@@ -779,7 +749,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverMotoristaContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverMotoristaContrato", error);
                 return Ok(new { success = false, message = "Erro ao remover motorista do contrato" });
             }
@@ -810,7 +779,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetLavadoresContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetLavadoresContrato", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -837,7 +805,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetLavadoresDisponiveis");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetLavadoresDisponiveis", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -871,7 +838,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirLavadorContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirLavadorContrato", error);
                 return Ok(new { success = false, message = "Erro ao incluir lavador no contrato" });
             }
@@ -900,7 +866,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverLavadorContrato");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverLavadorContrato", error);
                 return Ok(new { success = false, message = "Erro ao remover lavador do contrato" });
             }
@@ -940,7 +905,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosAta");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosAta", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -971,7 +935,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosDisponiveisAta");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosDisponiveisAta", error);
                 return Ok(new { success = false, data = new List<object>() });
             }
@@ -1005,7 +968,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirVeiculoAta");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirVeiculoAta", error);
                 return Ok(new { success = false, message = "Erro ao incluir veículo na ata" });
             }
@@ -1042,7 +1004,6 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverVeiculoAta");
                 Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverVeiculoAta", error);
                 return Ok(new { success = false, message = "Erro ao remover veículo da ata" });
             }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
        private readonly ILogService _log;
        public ItensContratoController(IUnitOfWork unitOfWork, ILogService log)
                _log = log;
                _log.Error(error.Message, error, "ItensContratoController.cs", "ListaContratos");
                _log.Error(error.Message, error, "ItensContratoController.cs", "ListaAtas");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetContratoDetalhes");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetAtaDetalhes");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosDisponiveis");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetItensContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirVeiculoContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverVeiculoContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetEncarregadosContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetEncarregadosDisponiveis");
                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirEncarregadoContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverEncarregadoContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetOperadoresContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetOperadoresDisponiveis");
                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirOperadorContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverOperadorContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetMotoristasContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetMotoristasDisponiveis");
                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirMotoristaContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverMotoristaContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetLavadoresContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetLavadoresDisponiveis");
                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirLavadorContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverLavadorContrato");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosAta");
                _log.Error(error.Message, error, "ItensContratoController.cs", "GetVeiculosDisponiveisAta");
                _log.Error(error.Message, error, "ItensContratoController.cs", "IncluirVeiculoAta");
                _log.Error(error.Message, error, "ItensContratoController.cs", "RemoverVeiculoAta");
```


### ADICIONAR ao Janeiro

```csharp
        public ItensContratoController(IUnitOfWork unitOfWork)
```
