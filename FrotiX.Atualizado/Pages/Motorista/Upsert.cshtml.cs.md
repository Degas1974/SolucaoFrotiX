# Pages/Motorista/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Motorista/Upsert.cshtml.cs
+++ ATUAL: Pages/Motorista/Upsert.cshtml.cs
@@ -100,7 +100,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    MotoristaObj.Motorista = _unitOfWork.Motorista.GetFirstOrDefaultWithTracking(u =>
+                    MotoristaObj.Motorista = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                         u.MotoristaId == id
                     );
 
```

### REMOVER do Janeiro

```csharp
                    MotoristaObj.Motorista = _unitOfWork.Motorista.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    MotoristaObj.Motorista = _unitOfWork.Motorista.GetFirstOrDefault(u =>
```
