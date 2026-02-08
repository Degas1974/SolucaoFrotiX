# Pages/Requisitante/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Requisitante/Upsert.cshtml.cs
+++ ATUAL: Pages/Requisitante/Upsert.cshtml.cs
@@ -72,7 +72,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    RequisitanteObj.Requisitante = _unitOfWork.Requisitante.GetFirstOrDefaultWithTracking(u =>
+                    RequisitanteObj.Requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                         u.RequisitanteId == id
                     );
                     if (RequisitanteObj == null)
```

### REMOVER do Janeiro

```csharp
                    RequisitanteObj.Requisitante = _unitOfWork.Requisitante.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    RequisitanteObj.Requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
```
