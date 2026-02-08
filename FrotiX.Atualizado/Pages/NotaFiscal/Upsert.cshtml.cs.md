# Pages/NotaFiscal/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/NotaFiscal/Upsert.cshtml.cs
+++ ATUAL: Pages/NotaFiscal/Upsert.cshtml.cs
@@ -65,7 +65,7 @@
 
                 if (id != Guid.Empty)
                 {
-                    NotaFiscalObj.NotaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefaultWithTracking(u =>
+                    NotaFiscalObj.NotaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                         u.NotaFiscalId == id
                     );
                     if (NotaFiscalObj.NotaFiscal == null)
```

### REMOVER do Janeiro

```csharp
                    NotaFiscalObj.NotaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefaultWithTracking(u =>
```


### ADICIONAR ao Janeiro

```csharp
                    NotaFiscalObj.NotaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
```
