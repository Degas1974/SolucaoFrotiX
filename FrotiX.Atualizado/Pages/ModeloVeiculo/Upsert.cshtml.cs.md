# Pages/ModeloVeiculo/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/ModeloVeiculo/Upsert.cshtml.cs
+++ ATUAL: Pages/ModeloVeiculo/Upsert.cshtml.cs
@@ -67,7 +67,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    ModeloVeiculoObj.ModeloVeiculo = _unitOfWork.ModeloVeiculo.GetFirstOrDefaultWithTracking(u => u.ModeloId == id);
+                    ModeloVeiculoObj.ModeloVeiculo = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u => u.ModeloId == id);
                     if (ModeloVeiculoObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    ModeloVeiculoObj.ModeloVeiculo = _unitOfWork.ModeloVeiculo.GetFirstOrDefaultWithTracking(u => u.ModeloId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    ModeloVeiculoObj.ModeloVeiculo = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u => u.ModeloId == id);
```
