# Pages/Multa/UpsertEmpenhosMulta.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Multa/UpsertEmpenhosMulta.cshtml.cs
+++ ATUAL: Pages/Multa/UpsertEmpenhosMulta.cshtml.cs
@@ -64,7 +64,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    EmpenhoMultaObj.EmpenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefaultWithTracking(e =>
+                    EmpenhoMultaObj.EmpenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(e =>
                         e.EmpenhoMultaId == id
                     );
                     if (EmpenhoMultaObj == null)
```

### REMOVER do Janeiro

```csharp
                    EmpenhoMultaObj.EmpenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefaultWithTracking(e =>
```


### ADICIONAR ao Janeiro

```csharp
                    EmpenhoMultaObj.EmpenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(e =>
```
