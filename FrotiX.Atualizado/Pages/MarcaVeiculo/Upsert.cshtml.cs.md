# Pages/MarcaVeiculo/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/MarcaVeiculo/Upsert.cshtml.cs
+++ ATUAL: Pages/MarcaVeiculo/Upsert.cshtml.cs
@@ -42,7 +42,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    MarcaVeiculoObj = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u => u.MarcaId == id);
+                    MarcaVeiculoObj = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u => u.MarcaId == id);
                     if (MarcaVeiculoObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    MarcaVeiculoObj = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u => u.MarcaId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    MarcaVeiculoObj = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u => u.MarcaId == id);
```
