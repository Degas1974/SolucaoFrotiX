# Pages/AtaRegistroPrecos/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/AtaRegistroPrecos/Upsert.cshtml.cs
+++ ATUAL: Pages/AtaRegistroPrecos/Upsert.cshtml.cs
@@ -76,7 +76,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    AtaObj.AtaRegistroPrecos = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefaultWithTracking(u => u.AtaId == id);
+                    AtaObj.AtaRegistroPrecos = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u => u.AtaId == id);
                     if (AtaObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    AtaObj.AtaRegistroPrecos = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefaultWithTracking(u => u.AtaId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    AtaObj.AtaRegistroPrecos = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u => u.AtaId == id);
```
