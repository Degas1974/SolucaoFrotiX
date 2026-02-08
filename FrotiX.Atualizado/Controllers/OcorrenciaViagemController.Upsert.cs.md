# Controllers/OcorrenciaViagemController.Upsert.cs

**Mudanca:** MEDIA | **+8** linhas | **-11** linhas

---

```diff
--- JANEIRO: Controllers/OcorrenciaViagemController.Upsert.cs
+++ ATUAL: Controllers/OcorrenciaViagemController.Upsert.cs
@@ -1,6 +1,5 @@
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
 using FrotiX.TextNormalization;
 using Microsoft.AspNetCore.Mvc;
 using System;
@@ -18,7 +17,6 @@
         {
             try
             {
-
                 if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                 {
                     return new JsonResult(new
@@ -34,7 +32,6 @@
 
                 if (ocorrencia == null)
                 {
-
                     return new JsonResult(new
                     {
                         success = false,
@@ -64,31 +61,29 @@
                 _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                 _unitOfWork.Save();
 
-                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaUpsert: Ocorrência {dto.OcorrenciaViagemId} baixada na tela de Upsert.");
-
                 return new JsonResult(new
                 {
                     success = true,
                     message = "Ocorrência baixada com sucesso"
                 });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaUpsert", ex);
-                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Upsert.cs", "BaixarOcorrenciaUpsert", ex);
+                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "BaixarOcorrenciaUpsert", error);
                 return new JsonResult(new
                 {
                     success = false,
-                    message = "Erro ao baixar ocorrência: " + ex.Message
+                    message = "Erro ao baixar ocorrência: " + error.Message
                 });
             }
         }
 
-        public class BaixarOcorrenciaUpsertDTO
-        {
-            public Guid OcorrenciaViagemId { get; set; }
-            public string? SolucaoOcorrencia { get; set; }
-        }
+    }
 
+    public class BaixarOcorrenciaUpsertDTO
+    {
+        public Guid OcorrenciaViagemId { get; set; }
+        public string? SolucaoOcorrencia { get; set; }
     }
+
 }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
                _log.Info($"OcorrenciaViagemController.BaixarOcorrenciaUpsert: Ocorrência {dto.OcorrenciaViagemId} baixada na tela de Upsert.");
            catch (Exception ex)
                _log.Error("OcorrenciaViagemController.BaixarOcorrenciaUpsert", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Upsert.cs", "BaixarOcorrenciaUpsert", ex);
                    message = "Erro ao baixar ocorrência: " + ex.Message
        public class BaixarOcorrenciaUpsertDTO
        {
            public Guid OcorrenciaViagemId { get; set; }
            public string? SolucaoOcorrencia { get; set; }
        }
```


### ADICIONAR ao Janeiro

```csharp
            catch (Exception error)
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "BaixarOcorrenciaUpsert", error);
                    message = "Erro ao baixar ocorrência: " + error.Message
    }
    public class BaixarOcorrenciaUpsertDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
        public string? SolucaoOcorrencia { get; set; }
```
