# Pages/Unidade/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Unidade/Upsert.cshtml.cs
+++ ATUAL: Pages/Unidade/Upsert.cshtml.cs
@@ -41,7 +41,7 @@
                 UnidadeObj = new Models.Unidade();
                 if (id != Guid.Empty)
                 {
-                    UnidadeObj = _unitOfWork.Unidade.GetFirstOrDefaultWithTracking(u => u.UnidadeId == id);
+                    UnidadeObj = _unitOfWork.Unidade.GetFirstOrDefault(u => u.UnidadeId == id);
                     if (UnidadeObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    UnidadeObj = _unitOfWork.Unidade.GetFirstOrDefaultWithTracking(u => u.UnidadeId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    UnidadeObj = _unitOfWork.Unidade.GetFirstOrDefault(u => u.UnidadeId == id);
```
