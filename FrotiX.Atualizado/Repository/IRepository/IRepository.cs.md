# Repository/IRepository/IRepository.cs

**Mudanca:** GRANDE | **+20** linhas | **-25** linhas

---

```diff
--- JANEIRO: Repository/IRepository/IRepository.cs
+++ ATUAL: Repository/IRepository/IRepository.cs
@@ -5,59 +5,52 @@
 using System.Threading.Tasks;
 
 namespace FrotiX.Repository.IRepository
-    {
+{
 
     public interface IRepository<T>
         where T : class
-        {
+    {
 
         T Get(object id);
 
-        T GetWithTracking(object id);
-
         T GetFirstOrDefault(
-            Expression<Func<T , bool>> filter = null ,
-            string includeProperties = null
-        );
-
-        T GetFirstOrDefaultWithTracking(
-            Expression<Func<T , bool>> filter = null ,
+            Expression<Func<T, bool>> filter = null,
             string includeProperties = null
         );
 
         Task<T> GetFirstOrDefaultAsync(
-            Expression<Func<T , bool>> filter = null ,
+            Expression<Func<T, bool>> filter = null,
             string includeProperties = null
         );
 
         IEnumerable<T> GetAll(
-            Expression<Func<T , bool>> filter = null ,
-            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
-            string includeProperties = null ,
+            Expression<Func<T, bool>> filter = null,
+            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
+            string includeProperties = null,
             bool asNoTracking = true
         );
 
         Task<IEnumerable<T>> GetAllAsync(
-            Expression<Func<T , bool>> filter = null ,
-            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
-            string includeProperties = null ,
-            bool asNoTracking = true ,
+            Expression<Func<T, bool>> filter = null,
+            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
+            string includeProperties = null,
+            bool asNoTracking = true,
             int? take = null
         );
 
         IEnumerable<TResult> GetAllReduced<TResult>(
-            Expression<Func<T , TResult>> selector ,
-            Expression<Func<T , bool>> filter = null ,
-            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
-            string includeProperties = null ,
+            Expression<Func<T, TResult>> selector,
+            Expression<Func<T, bool>> filter = null,
+            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
+            string includeProperties = null,
             bool asNoTracking = true
         );
 
         IQueryable<TResult> GetAllReducedIQueryable<TResult>(
-            Expression<Func<T , TResult>> selector ,
-            Expression<Func<T , bool>> filter = null ,
-            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
-            string includeProperties = null ,
+            Expression<Func<T, TResult>> selector,
+            Expression<Func<T, bool>> filter = null,
+            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
+            string includeProperties = null,
             bool asNoTracking = true
         );
 
@@ -70,5 +63,5 @@
         void Remove(object id);
 
         void Remove(T entity);
-        }
     }
+}
```

### REMOVER do Janeiro

```csharp
    {
        {
        T GetWithTracking(object id);
            Expression<Func<T , bool>> filter = null ,
            string includeProperties = null
        );
        T GetFirstOrDefaultWithTracking(
            Expression<Func<T , bool>> filter = null ,
            Expression<Func<T , bool>> filter = null ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true ,
            Expression<Func<T , TResult>> selector ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            Expression<Func<T , TResult>> selector ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
        }
```


### ADICIONAR ao Janeiro

```csharp
{
    {
            Expression<Func<T, bool>> filter = null,
            Expression<Func<T, bool>> filter = null,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool asNoTracking = true,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
}
```
