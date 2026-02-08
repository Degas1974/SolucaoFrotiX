# Pages/Lavador/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Lavador/Upsert.cshtml.cs
+++ ATUAL: Pages/Lavador/Upsert.cshtml.cs
@@ -95,7 +95,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    LavadorObj.Lavador = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u => u.LavadorId == id);
+                    LavadorObj.Lavador = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == id);
                     if (LavadorObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    LavadorObj.Lavador = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u => u.LavadorId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    LavadorObj.Lavador = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == id);
```
