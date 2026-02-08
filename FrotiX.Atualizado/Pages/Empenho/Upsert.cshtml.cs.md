# Pages/Empenho/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Empenho/Upsert.cshtml.cs
+++ ATUAL: Pages/Empenho/Upsert.cshtml.cs
@@ -70,7 +70,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    EmpenhoObj.Empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
+                    EmpenhoObj.Empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                         u.EmpenhoId == id
                     );
                     if (EmpenhoObj == null)
```

### REMOVER do Janeiro

```csharp
                    EmpenhoObj.Empenho = _unitOfWork.Empenho.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    EmpenhoObj.Empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
```
