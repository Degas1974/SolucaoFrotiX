# Repository/OcorrenciaViagemRepository.cs

**Mudanca:** GRANDE | **+17** linhas | **-17** linhas

---

```diff
--- JANEIRO: Repository/OcorrenciaViagemRepository.cs
+++ ATUAL: Repository/OcorrenciaViagemRepository.cs
@@ -1,4 +1,3 @@
-using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -6,20 +5,21 @@
 using FrotiX.Data;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
+using Microsoft.EntityFrameworkCore;
 
 namespace FrotiX.Repository
-{
+    {
 
     public class OcorrenciaViagemRepository : IOcorrenciaViagemRepository
-    {
+        {
         private new readonly FrotiXDbContext _db;
 
         public OcorrenciaViagemRepository(FrotiXDbContext db)
-        {
+            {
             _db = db;
-        }
+            }
 
-        public IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem, bool>>? filter = null, string? includeProperties = null)
+        public IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem , bool>>? filter = null , string? includeProperties = null)
         {
             IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;
 
@@ -28,16 +28,16 @@
 
             if (!string.IsNullOrEmpty(includeProperties))
             {
-                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
+                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
                 {
                     query = query.Include(prop.Trim());
                 }
             }
 
             return query.ToList();
-        }
+            }
 
-        public OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem, bool>> filter, string? includeProperties = null)
+        public OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem , bool>> filter , string? includeProperties = null)
         {
             IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;
 
@@ -45,28 +45,28 @@
 
             if (!string.IsNullOrEmpty(includeProperties))
             {
-                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
+                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
                 {
                     query = query.Include(prop.Trim());
                 }
             }
 
             return query.FirstOrDefault();
-        }
+            }
 
         public void Add(OcorrenciaViagem entity)
-        {
+            {
             _db.OcorrenciaViagem.Add(entity);
-        }
+            }
 
         public void Remove(OcorrenciaViagem entity)
-        {
+            {
             _db.OcorrenciaViagem.Remove(entity);
-        }
+            }
 
         public new void Update(OcorrenciaViagem entity)
-        {
+            {
             _db.OcorrenciaViagem.Update(entity);
+            }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
{
    {
        {
        }
        public IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem, bool>>? filter = null, string? includeProperties = null)
                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        }
        public OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem, bool>> filter, string? includeProperties = null)
                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        }
        {
        }
        {
        }
        {
}
```


### ADICIONAR ao Janeiro

```csharp
using Microsoft.EntityFrameworkCore;
    {
        {
            {
            }
        public IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem , bool>>? filter = null , string? includeProperties = null)
                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
            }
        public OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem , bool>> filter , string? includeProperties = null)
                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
            }
            {
            }
            {
            }
            {
            }
```
