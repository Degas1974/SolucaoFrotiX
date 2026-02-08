# Pages/Combustivel/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Combustivel/Upsert.cshtml.cs
+++ ATUAL: Pages/Combustivel/Upsert.cshtml.cs
@@ -40,7 +40,7 @@
                 CombustivelObj = new Models.Combustivel();
                 if (id != Guid.Empty)
                 {
-                    CombustivelObj = _unitOfWork.Combustivel.GetFirstOrDefaultWithTracking(u => u.CombustivelId == id);
+                    CombustivelObj = _unitOfWork.Combustivel.GetFirstOrDefault(u => u.CombustivelId == id);
                     if (CombustivelObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    CombustivelObj = _unitOfWork.Combustivel.GetFirstOrDefaultWithTracking(u => u.CombustivelId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    CombustivelObj = _unitOfWork.Combustivel.GetFirstOrDefault(u => u.CombustivelId == id);
```
