# Pages/SetorSolicitante/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/SetorSolicitante/Upsert.cshtml.cs
+++ ATUAL: Pages/SetorSolicitante/Upsert.cshtml.cs
@@ -43,7 +43,7 @@
                 SetorSolicitanteObj = new Models.SetorSolicitante();
                 if (id != Guid.Empty)
                 {
-                    SetorSolicitanteObj = _unitOfWork.SetorSolicitante.GetFirstOrDefaultWithTracking(u =>
+                    SetorSolicitanteObj = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                         u.SetorSolicitanteId == id
                     );
                     if (SetorSolicitanteObj == null)
```

### REMOVER do Janeiro

```csharp
                    SetorSolicitanteObj = _unitOfWork.SetorSolicitante.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    SetorSolicitanteObj = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
```
