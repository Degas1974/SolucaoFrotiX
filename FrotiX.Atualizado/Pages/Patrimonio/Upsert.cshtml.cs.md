# Pages/Patrimonio/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Patrimonio/Upsert.cshtml.cs
+++ ATUAL: Pages/Patrimonio/Upsert.cshtml.cs
@@ -73,7 +73,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    PatrimonioObj.Patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefaultWithTracking(u =>
+                    PatrimonioObj.Patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefault(u =>
                         u.PatrimonioId == id
                     );
                     if (PatrimonioObj == null)
```

### REMOVER do Janeiro

```csharp
                    PatrimonioObj.Patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    PatrimonioObj.Patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefault(u =>
```
