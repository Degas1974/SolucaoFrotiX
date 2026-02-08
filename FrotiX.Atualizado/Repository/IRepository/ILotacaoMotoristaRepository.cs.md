# Repository/IRepository/ILotacaoMotoristaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/ILotacaoMotoristaRepository.cs
+++ ATUAL: Repository/IRepository/ILotacaoMotoristaRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface ILotacaoMotoristaRepository : IRepository<LotacaoMotorista>
     {
-    public interface ILotacaoMotoristaRepository : IRepository<LotacaoMotorista>
-        {
 
         IEnumerable<SelectListItem> GetLotacaoMotoristaListForDropDown();
 
         void Update(LotacaoMotorista lotacaoMotorista);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface ILotacaoMotoristaRepository : IRepository<LotacaoMotorista>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface ILotacaoMotoristaRepository : IRepository<LotacaoMotorista>
}
```
