# Pages/Operador/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Operador/Upsert.cshtml.cs
+++ ATUAL: Pages/Operador/Upsert.cshtml.cs
@@ -97,7 +97,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    OperadorObj.Operador = _unitOfWork.Operador.GetFirstOrDefaultWithTracking(u =>
+                    OperadorObj.Operador = _unitOfWork.Operador.GetFirstOrDefault(u =>
                         u.OperadorId == id
                     );
                     if (OperadorObj == null)
```

### REMOVER do Janeiro

```csharp
                    OperadorObj.Operador = _unitOfWork.Operador.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    OperadorObj.Operador = _unitOfWork.Operador.GetFirstOrDefault(u =>
```
