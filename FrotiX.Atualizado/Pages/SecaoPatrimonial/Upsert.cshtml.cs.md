# Pages/SecaoPatrimonial/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/SecaoPatrimonial/Upsert.cshtml.cs
+++ ATUAL: Pages/SecaoPatrimonial/Upsert.cshtml.cs
@@ -66,7 +66,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    SecaoObj = _unitOfWork.SecaoPatrimonial.GetFirstOrDefaultWithTracking(u => u.SecaoId == id);
+                    SecaoObj = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u => u.SecaoId == id);
                     if (SecaoObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    SecaoObj = _unitOfWork.SecaoPatrimonial.GetFirstOrDefaultWithTracking(u => u.SecaoId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    SecaoObj = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u => u.SecaoId == id);
```
