# Extensions/IdentityExtensions.cs

**Mudanca:** GRANDE | **+22** linhas | **-40** linhas

---

```diff
--- JANEIRO: Extensions/IdentityExtensions.cs
+++ ATUAL: Extensions/IdentityExtensions.cs
@@ -7,23 +7,24 @@
 using Microsoft.AspNetCore.Identity;
 using FrotiX.Data;
 using FrotiX.Models;
-using System;
-using FrotiX.Helpers;
 
 namespace FrotiX.Extensions
+{
+
+    public static class IdentityExtensions
     {
-    public static class IdentityExtensions
-        {
+
         [DebuggerStepThrough]
         private static bool HasRole(this ClaimsPrincipal principal, params string[] roles)
-            {
+        {
+
             if (principal == null)
                 return default;
 
             var claims = principal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToSafeList();
 
             return claims?.Any() == true && claims.Intersect(roles ?? new string[] { }).Any();
-            }
+        }
 
         [DebuggerStepThrough]
         public static IEnumerable<ListItem> AuthorizeFor(this IEnumerable<ListItem> source, ClaimsPrincipal identity)
@@ -43,53 +44,38 @@
 
         [DebuggerStepThrough]
         public static async Task<IdentityResult> UpdateAsync<T>(this ApplicationDbContext context, T model, string id) where T : class
+        {
+
+            var entity = await context.FindAsync<T>(id);
+
+            if (entity == null)
             {
-            try
-            {
-                var entity = await context.FindAsync<T>(id);
+                return IdentityResult.Failed();
+            }
 
-                if (entity == null)
-                    {
-                    return IdentityResult.Failed();
-                    }
+            context.Entry((object)entity).CurrentValues.SetValues(model);
 
-                context.Entry((object)entity).CurrentValues.SetValues(model);
+            await context.SaveChangesAsync();
 
-                await context.SaveChangesAsync();
-
-                return IdentityResult.Success;
-            }
-            catch (Exception ex)
-            {
-               Alerta.TratamentoErroComLinha("IdentityExtensions.cs", "UpdateAsync", ex);
-
-               return IdentityResult.Failed(new IdentityError { Description = "Erro interno ao atualizar registro via Extension." });
-            }
-            }
+            return IdentityResult.Success;
+        }
 
         [DebuggerStepThrough]
         public static async Task<IdentityResult> DeleteAsync<T>(this ApplicationDbContext context, string id) where T : class
+        {
+
+            var entity = await context.FindAsync<T>(id);
+
+            if (entity == null)
             {
-            try
-            {
-                var entity = await context.FindAsync<T>(id);
+                return IdentityResult.Failed();
+            }
 
-                if (entity == null)
-                    {
-                    return IdentityResult.Failed();
-                    }
+            context.Remove((object)entity);
 
-                context.Remove((object)entity);
+            await context.SaveChangesAsync();
 
-                await context.SaveChangesAsync();
-
-                return IdentityResult.Success;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("IdentityExtensions.cs", "DeleteAsync", ex);
-                return IdentityResult.Failed(new IdentityError { Description = "Erro interno ao excluir registro via Extension." });
-            }
-            }
+            return IdentityResult.Success;
         }
     }
+}
```

### REMOVER do Janeiro

```csharp
using System;
using FrotiX.Helpers;
    public static class IdentityExtensions
        {
            {
            }
            try
            {
                var entity = await context.FindAsync<T>(id);
                if (entity == null)
                    {
                    return IdentityResult.Failed();
                    }
                context.Entry((object)entity).CurrentValues.SetValues(model);
                await context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
               Alerta.TratamentoErroComLinha("IdentityExtensions.cs", "UpdateAsync", ex);
               return IdentityResult.Failed(new IdentityError { Description = "Erro interno ao atualizar registro via Extension." });
            }
            }
            try
            {
                var entity = await context.FindAsync<T>(id);
                if (entity == null)
                    {
                    return IdentityResult.Failed();
                    }
                context.Remove((object)entity);
                await context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("IdentityExtensions.cs", "DeleteAsync", ex);
                return IdentityResult.Failed(new IdentityError { Description = "Erro interno ao excluir registro via Extension." });
            }
            }
```


### ADICIONAR ao Janeiro

```csharp
{
    public static class IdentityExtensions
        {
        }
        {
            var entity = await context.FindAsync<T>(id);
            if (entity == null)
                return IdentityResult.Failed();
            }
            context.Entry((object)entity).CurrentValues.SetValues(model);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        {
            var entity = await context.FindAsync<T>(id);
            if (entity == null)
                return IdentityResult.Failed();
            }
            context.Remove((object)entity);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
}
```
