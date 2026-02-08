# Pages/Viagens/Upsert.cshtml.cs

**Mudanca:** PEQUENA | **+1** linhas | **-0** linhas

---

```diff
--- JANEIRO: Pages/Viagens/Upsert.cshtml.cs
+++ ATUAL: Pages/Viagens/Upsert.cshtml.cs
@@ -961,6 +961,7 @@
                 }
                 else
                 {
+                    ViagemObj.Viagem.Status = "Realizada";
 
                 }
 
```

### ADICIONAR ao Janeiro

```csharp
                    ViagemObj.Viagem.Status = "Realizada";
```
