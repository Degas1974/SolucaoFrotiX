# Repository/Repository.cs

**Mudanca:** MEDIA | **+0** linhas | **-24** linhas

---

```diff
--- JANEIRO: Repository/Repository.cs
+++ ATUAL: Repository/Repository.cs
@@ -64,14 +64,6 @@
             return dbSet.Find(id);
             }
 
-        public T GetWithTracking(object id)
-            {
-            if (id == null)
-                return null;
-
-            return dbSet.Find(id);
-            }
-
         public T GetFirstOrDefault(
             Expression<Func<T , bool>> filter = null ,
             string includeProperties = null
@@ -80,27 +72,6 @@
             try
                 {
                 return PrepareQuery(filter , includeProperties , asNoTracking: true).FirstOrDefault();
-                }
-            catch (InvalidOperationException ex) when (ex.Message.Contains("second operation"))
-                {
-
-                return null;
-                }
-            catch (Exception)
-                {
-
-                throw;
-                }
-            }
-
-        public T GetFirstOrDefaultWithTracking(
-            Expression<Func<T , bool>> filter = null ,
-            string includeProperties = null
-        )
-            {
-            try
-                {
-                return PrepareQuery(filter , includeProperties , asNoTracking: false).FirstOrDefault();
                 }
             catch (InvalidOperationException ex) when (ex.Message.Contains("second operation"))
                 {
```

### REMOVER do Janeiro

```csharp
        public T GetWithTracking(object id)
            {
            if (id == null)
                return null;
            return dbSet.Find(id);
            }
                }
            catch (InvalidOperationException ex) when (ex.Message.Contains("second operation"))
                {
                return null;
                }
            catch (Exception)
                {
                throw;
                }
            }
        public T GetFirstOrDefaultWithTracking(
            Expression<Func<T , bool>> filter = null ,
            string includeProperties = null
        )
            {
            try
                {
                return PrepareQuery(filter , includeProperties , asNoTracking: false).FirstOrDefault();
```

