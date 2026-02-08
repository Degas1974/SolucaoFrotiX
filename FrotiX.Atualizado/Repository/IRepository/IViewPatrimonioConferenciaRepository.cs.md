# Repository/IRepository/IViewPatrimonioConferenciaRepository.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewPatrimonioConferenciaRepository.cs
+++ ATUAL: Repository/IRepository/IViewPatrimonioConferenciaRepository.cs
@@ -2,7 +2,8 @@
 
 namespace FrotiX.Repository.IRepository
 {
-    public interface IViewPatrimonioConferenciaRepository :IRepository<ViewPatrimonioConferencia>
+
+    public interface IViewPatrimonioConferenciaRepository : IRepository<ViewPatrimonioConferencia>
     {
 
     }
```

### REMOVER do Janeiro

```csharp
    public interface IViewPatrimonioConferenciaRepository :IRepository<ViewPatrimonioConferencia>
```


### ADICIONAR ao Janeiro

```csharp
    public interface IViewPatrimonioConferenciaRepository : IRepository<ViewPatrimonioConferencia>
```
