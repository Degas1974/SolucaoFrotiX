# Pages/Fornecedor/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/Fornecedor/Upsert.cshtml.cs
+++ ATUAL: Pages/Fornecedor/Upsert.cshtml.cs
@@ -40,7 +40,7 @@
                 FornecedorObj = new Models.Fornecedor();
                 if (id != Guid.Empty)
                 {
-                    FornecedorObj = _unitOfWork.Fornecedor.GetFirstOrDefaultWithTracking(u => u.FornecedorId == id);
+                    FornecedorObj = _unitOfWork.Fornecedor.GetFirstOrDefault(u => u.FornecedorId == id);
                     if (FornecedorObj == null)
                     {
                         return NotFound();
```

### REMOVER do Janeiro

```csharp
                    FornecedorObj = _unitOfWork.Fornecedor.GetFirstOrDefaultWithTracking(u => u.FornecedorId == id);
```


### ADICIONAR ao Janeiro

```csharp
                    FornecedorObj = _unitOfWork.Fornecedor.GetFirstOrDefault(u => u.FornecedorId == id);
```
