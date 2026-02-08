# Pages/SetorPatrimonial/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/SetorPatrimonial/Upsert.cshtml.cs
+++ ATUAL: Pages/SetorPatrimonial/Upsert.cshtml.cs
@@ -66,7 +66,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    SetorObj = _unitOfWork.SetorPatrimonial.GetFirstOrDefaultWithTracking(u => u.SetorId == id);
+                    SetorObj = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u => u.SetorId == id);
                     if (SetorObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    SetorObj = _unitOfWork.SetorPatrimonial.GetFirstOrDefaultWithTracking(u => u.SetorId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    SetorObj = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u => u.SetorId == id);
```
