# Pages/Encarregado/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Encarregado/Upsert.cshtml.cs
+++ ATUAL: Pages/Encarregado/Upsert.cshtml.cs
@@ -97,7 +97,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    EncarregadoObj.Encarregado = _unitOfWork.Encarregado.GetFirstOrDefaultWithTracking(u =>
+                    EncarregadoObj.Encarregado = _unitOfWork.Encarregado.GetFirstOrDefault(u =>
                         u.EncarregadoId == id
                     );
                     if (EncarregadoObj == null)
```

### REMOVER do Janeiro

```csharp
                    EncarregadoObj.Encarregado = _unitOfWork.Encarregado.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    EncarregadoObj.Encarregado = _unitOfWork.Encarregado.GetFirstOrDefault(u =>
```
