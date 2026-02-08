# Repository/IRepository/IViewGlosaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewGlosaRepository.cs
+++ ATUAL: Repository/IRepository/IViewGlosaRepository.cs
@@ -3,10 +3,10 @@
 using FrotiX.Repository.IRepository;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewGlosaRepository : IRepository<ViewGlosa>
     {
 
-    public interface IViewGlosaRepository : IRepository<ViewGlosa>
-        {
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewGlosaRepository : IRepository<ViewGlosa>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewGlosaRepository : IRepository<ViewGlosa>
}
```
