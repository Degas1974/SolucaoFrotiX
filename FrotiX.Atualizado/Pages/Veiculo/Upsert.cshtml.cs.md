# Pages/Veiculo/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Veiculo/Upsert.cshtml.cs
+++ ATUAL: Pages/Veiculo/Upsert.cshtml.cs
@@ -101,7 +101,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    VeiculoObj.Veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(u => u.VeiculoId == id);
+                    VeiculoObj.Veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.VeiculoId == id);
 
                     if (VeiculoObj == null)
                     {
```

### REMOVER do Janeiro

```csharp
                    VeiculoObj.Veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(u => u.VeiculoId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    VeiculoObj.Veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.VeiculoId == id);
```
