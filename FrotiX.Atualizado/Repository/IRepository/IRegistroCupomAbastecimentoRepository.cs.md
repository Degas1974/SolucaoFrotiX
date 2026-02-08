# Repository/IRepository/IRegistroCupomAbastecimentoRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRegistroCupomAbastecimentoRepository.cs
+++ ATUAL: Repository/IRepository/IRegistroCupomAbastecimentoRepository.cs
@@ -6,13 +6,13 @@
 using Microsoft.AspNetCore.Mvc.Rendering;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IRegistroCupomAbastecimentoRepository : IRepository<RegistroCupomAbastecimento>
     {
-    public interface IRegistroCupomAbastecimentoRepository : IRepository<RegistroCupomAbastecimento>
-        {
 
         IEnumerable<SelectListItem> GetRegistroCupomAbastecimentoListForDropDown();
 
         void Update(RegistroCupomAbastecimento registroCupomAbastecimento);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IRegistroCupomAbastecimentoRepository : IRepository<RegistroCupomAbastecimento>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IRegistroCupomAbastecimentoRepository : IRepository<RegistroCupomAbastecimento>
}
```
