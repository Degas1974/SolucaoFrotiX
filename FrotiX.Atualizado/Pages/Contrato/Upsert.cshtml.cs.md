# Pages/Contrato/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Contrato/Upsert.cshtml.cs
+++ ATUAL: Pages/Contrato/Upsert.cshtml.cs
@@ -71,7 +71,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    ContratoObj.Contrato = _unitOfWork.Contrato.GetFirstOrDefaultWithTracking(u =>
+                    ContratoObj.Contrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                         u.ContratoId == id
                     );
                     if (ContratoObj == null)
```

### REMOVER do Janeiro

```csharp
                    ContratoObj.Contrato = _unitOfWork.Contrato.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    ContratoObj.Contrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
```
