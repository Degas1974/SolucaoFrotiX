# Pages/PlacaBronze/Upsert.cshtml.cs

**Mudanca:** MEDIA | **+5** linhas | **-5** linhas

---

```diff
--- JANEIRO: Pages/PlacaBronze/Upsert.cshtml.cs
+++ ATUAL: Pages/PlacaBronze/Upsert.cshtml.cs
@@ -35,13 +35,13 @@
 
             if (id != null)
             {
-                PlacaBronzeObj.PlacaBronze = _unitOfWork.PlacaBronze.GetFirstOrDefaultWithTracking(u => u.PlacaBronzeId == id);
+                PlacaBronzeObj.PlacaBronze = _unitOfWork.PlacaBronze.GetFirstOrDefault(u => u.PlacaBronzeId == id);
                 if (PlacaBronzeObj.PlacaBronze == null)
                 {
                     return NotFound();
                 }
 
-                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(v => v.PlacaBronzeId == id);
+                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.PlacaBronzeId == id);
                 if (veiculo != null)
                 {
                      PlacaBronzeObj.VeiculoId = veiculo.VeiculoId;
@@ -65,7 +65,7 @@
             }
             else
             {
-                var objFromDb = _unitOfWork.PlacaBronze.GetWithTracking(PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
+                var objFromDb = _unitOfWork.PlacaBronze.Get(PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
                 objFromDb.DescricaoPlaca = PlacaBronzeObj.PlacaBronze.DescricaoPlaca;
                 objFromDb.Status = PlacaBronzeObj.PlacaBronze.Status;
                 _unitOfWork.PlacaBronze.Update(objFromDb);
@@ -73,7 +73,7 @@
 
             _unitOfWork.Save();
 
-             var currentVeiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(v => v.PlacaBronzeId == PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
+             var currentVeiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.PlacaBronzeId == PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
 
              if (currentVeiculo != null && currentVeiculo.VeiculoId != PlacaBronzeObj.VeiculoId)
              {
@@ -83,7 +83,7 @@
 
              if (PlacaBronzeObj.VeiculoId != Guid.Empty)
              {
-                 var newVeiculo = _unitOfWork.Veiculo.GetWithTracking(PlacaBronzeObj.VeiculoId);
+                 var newVeiculo = _unitOfWork.Veiculo.Get(PlacaBronzeObj.VeiculoId);
                  if (newVeiculo != null)
                  {
                      newVeiculo.PlacaBronzeId = PlacaBronzeObj.PlacaBronze.PlacaBronzeId;
```

### REMOVER do Janeiro

```csharp
                PlacaBronzeObj.PlacaBronze = _unitOfWork.PlacaBronze.GetFirstOrDefaultWithTracking(u => u.PlacaBronzeId == id);
                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(v => v.PlacaBronzeId == id);
                var objFromDb = _unitOfWork.PlacaBronze.GetWithTracking(PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
             var currentVeiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(v => v.PlacaBronzeId == PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
                 var newVeiculo = _unitOfWork.Veiculo.GetWithTracking(PlacaBronzeObj.VeiculoId);
```


### ADICIONAR ao Janeiro

```csharp
                PlacaBronzeObj.PlacaBronze = _unitOfWork.PlacaBronze.GetFirstOrDefault(u => u.PlacaBronzeId == id);
                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.PlacaBronzeId == id);
                var objFromDb = _unitOfWork.PlacaBronze.Get(PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
             var currentVeiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.PlacaBronzeId == PlacaBronzeObj.PlacaBronze.PlacaBronzeId);
                 var newVeiculo = _unitOfWork.Veiculo.Get(PlacaBronzeObj.VeiculoId);
```
