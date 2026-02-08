# Repository/IRepository/IViewViagensAgendaRepository.cs

**Mudanca:** MEDIA | **+3** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IViewViagensAgendaRepository.cs
+++ ATUAL: Repository/IRepository/IViewViagensAgendaRepository.cs
@@ -7,13 +7,13 @@
 using System.Threading.Tasks;
 
 namespace FrotiX.Repository.IRepository
+{
+
+    public interface IViewViagensAgendaRepository : IRepository<ViewViagensAgenda>
     {
-    public interface IViewViagensAgendaRepository : IRepository<ViewViagensAgenda>
-        {
 
         IEnumerable<SelectListItem> GetViewViagensAgendaListForDropDown();
 
         void Update(ViewViagensAgenda viewViagensAgenda);
-
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    public interface IViewViagensAgendaRepository : IRepository<ViewViagensAgenda>
        {
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    public interface IViewViagensAgendaRepository : IRepository<ViewViagensAgenda>
}
```
