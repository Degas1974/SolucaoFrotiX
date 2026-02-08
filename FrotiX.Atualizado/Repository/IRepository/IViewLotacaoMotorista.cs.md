# Repository/IRepository/IViewLotacaoMotorista.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewLotacaoMotorista.cs
+++ ATUAL: Repository/IRepository/IViewLotacaoMotorista.cs
@@ -6,9 +6,10 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewLotacaoMotoristaRepository : IRepository<ViewLotacaoMotorista>
     {
-    public interface IViewLotacaoMotoristaRepository : IRepository<ViewLotacaoMotorista>
-        {
 
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewLotacaoMotoristaRepository : IRepository<ViewLotacaoMotorista>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewLotacaoMotoristaRepository : IRepository<ViewLotacaoMotorista>
}
```
