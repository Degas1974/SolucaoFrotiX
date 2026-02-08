# Pages/Manutencao/Upsert.cshtml.cs

**Mudanca:** MEDIA | **+5** linhas | **-23** linhas

---

```diff
--- JANEIRO: Pages/Manutencao/Upsert.cshtml.cs
+++ ATUAL: Pages/Manutencao/Upsert.cshtml.cs
@@ -11,7 +11,7 @@
 using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
-using MotoristaData = FrotiX.Models.DTO.MotoristaData;
+using MotoristaDataComFoto = FrotiX.Models.DTO.MotoristaDataComFoto;
 using VeiculoData = FrotiX.Models.DTO.VeiculoData;
 using VeiculoReservaData = FrotiX.Models.DTO.VeiculoReservaData;
 
@@ -61,7 +61,8 @@
         {
             try
             {
-                var ds = _cache.Get<List<MotoristaData>>(CacheKeys.Motoristas) ?? new List<MotoristaData>();
+
+                var ds = _cache.Get<List<MotoristaDataComFoto>>(CacheKeys.Motoristas) ?? new List<MotoristaDataComFoto>();
                 ViewData["dataMotorista"] = ds;
             }
             catch (Exception error)
@@ -75,7 +76,8 @@
         {
             try
             {
-                var ds = _cache.Get<List<VeiculoData>>(CacheKeys.Veiculos) ?? new List<VeiculoData>();
+
+                var ds = _cache.Get<List<VeiculoData>>(CacheKeys.VeiculosManutencao) ?? new List<VeiculoData>();
                 ViewData["dataVeiculo"] = ds;
             }
             catch (Exception error)
@@ -89,25 +91,8 @@
         {
             try
             {
-                var cachedData = _cache.Get(CacheKeys.VeiculosReserva);
-                List<VeiculoReservaData> ds;
-                if (cachedData is List<VeiculoReservaData> reservaData)
-                {
-                    ds = reservaData;
-                }
-                else if (cachedData is List<VeiculoData> veiculoData)
-                {
-                    ds = veiculoData
-                        .Select(v => new VeiculoReservaData(
-                            v.VeiculoId ,
-                            v.Descricao
-                        ))
-                        .ToList();
-                }
-                else
-                {
-                    ds = new List<VeiculoReservaData>();
-                }
+
+                var ds = _cache.Get<List<VeiculoReservaData>>(CacheKeys.VeiculosReserva) ?? new List<VeiculoReservaData>();
                 ViewData["dataVeiculoReserva"] = ds;
             }
             catch (Exception error)
@@ -138,7 +123,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    ManutencaoObj.Manutencao = _unitOfWork.Manutencao.GetFirstOrDefaultWithTracking(u => u.ManutencaoId == id);
+                    ManutencaoObj.Manutencao = _unitOfWork.Manutencao.GetFirstOrDefault(u => u.ManutencaoId == id);
                     if (ManutencaoObj?.Manutencao == null)
                         return NotFound();
                 }
```

### REMOVER do Janeiro

```csharp
using MotoristaData = FrotiX.Models.DTO.MotoristaData;
                var ds = _cache.Get<List<MotoristaData>>(CacheKeys.Motoristas) ?? new List<MotoristaData>();
                var ds = _cache.Get<List<VeiculoData>>(CacheKeys.Veiculos) ?? new List<VeiculoData>();
                var cachedData = _cache.Get(CacheKeys.VeiculosReserva);
                List<VeiculoReservaData> ds;
                if (cachedData is List<VeiculoReservaData> reservaData)
                {
                    ds = reservaData;
                }
                else if (cachedData is List<VeiculoData> veiculoData)
                {
                    ds = veiculoData
                        .Select(v => new VeiculoReservaData(
                            v.VeiculoId ,
                            v.Descricao
                        ))
                        .ToList();
                }
                else
                {
                    ds = new List<VeiculoReservaData>();
                }
                    ManutencaoObj.Manutencao = _unitOfWork.Manutencao.GetFirstOrDefaultWithTracking(u => u.ManutencaoId == id);
```


### ADICIONAR ao Janeiro

```csharp
using MotoristaDataComFoto = FrotiX.Models.DTO.MotoristaDataComFoto;
                var ds = _cache.Get<List<MotoristaDataComFoto>>(CacheKeys.Motoristas) ?? new List<MotoristaDataComFoto>();
                var ds = _cache.Get<List<VeiculoData>>(CacheKeys.VeiculosManutencao) ?? new List<VeiculoData>();
                var ds = _cache.Get<List<VeiculoReservaData>>(CacheKeys.VeiculosReserva) ?? new List<VeiculoReservaData>();
                    ManutencaoObj.Manutencao = _unitOfWork.Manutencao.GetFirstOrDefault(u => u.ManutencaoId == id);
```
