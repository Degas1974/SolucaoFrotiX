# Repository/AlertasUsuarioRepository.cs

**Mudanca:** GRANDE | **+29** linhas | **-78** linhas

---

```diff
--- JANEIRO: Repository/AlertasUsuarioRepository.cs
+++ ATUAL: Repository/AlertasUsuarioRepository.cs
@@ -1,17 +1,15 @@
+using FrotiX.Data;
+using FrotiX.Models;
+using FrotiX.Repository.IRepository;
 using Microsoft.EntityFrameworkCore;
 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
-using FrotiX.Data;
-using FrotiX.Models;
-using FrotiX.Repository.IRepository;
-using FrotiX.Helpers;
 
 namespace FrotiX.Repository
 {
-
-    public class AlertasUsuarioRepository : Repository<AlertasUsuario>, IAlertasUsuarioRepository
+    public class AlertasUsuarioRepository :Repository<AlertasUsuario>, IAlertasUsuarioRepository
     {
         private new readonly FrotiXDbContext _db;
 
@@ -22,116 +20,57 @@
 
         public async Task<IEnumerable<AlertasUsuario>> ObterAlertasPorUsuarioAsync(string usuarioId)
         {
-            try
-            {
-
-                return await _db.Set<AlertasUsuario>()
-                    .Where(au => au.UsuarioId == usuarioId)
-                    .Include(au => au.AlertasFrotiX)
-                    .AsNoTracking()
-                    .ToListAsync();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "ObterAlertasPorUsuarioAsync", ex);
-                return new List<AlertasUsuario>();
-            }
+            return await _db.Set<AlertasUsuario>()
+                .Where(au => au.UsuarioId == usuarioId)
+                .Include(au => au.AlertasFrotiX)
+                .AsNoTracking()
+                .ToListAsync();
         }
 
         public async Task<IEnumerable<AlertasUsuario>> ObterUsuariosPorAlertaAsync(Guid alertaId)
         {
-            try
-            {
-
-                return await _db.Set<AlertasUsuario>()
-                    .Where(au => au.AlertasFrotiXId == alertaId)
-                    .AsNoTracking()
-                    .ToListAsync();
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "ObterUsuariosPorAlertaAsync", ex);
-                return new List<AlertasUsuario>();
-            }
+            return await _db.Set<AlertasUsuario>()
+                .Where(au => au.AlertasFrotiXId == alertaId)
+                .AsNoTracking()
+                .ToListAsync();
         }
 
-        public async Task<bool> UsuarioTemAlertaAsync(Guid alertaId, string usuarioId)
+        public async Task<bool> UsuarioTemAlertaAsync(Guid alertaId , string usuarioId)
         {
-            try
-            {
-
-                return await _db.Set<AlertasUsuario>()
-                    .AnyAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "UsuarioTemAlertaAsync", ex);
-                return false;
-            }
+            return await _db.Set<AlertasUsuario>()
+                .AnyAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);
         }
 
         public async Task RemoverAlertasDoUsuarioAsync(string usuarioId)
         {
-            try
+            var alertasUsuario = await _db.Set<AlertasUsuario>()
+                .Where(au => au.UsuarioId == usuarioId)
+                .ToListAsync();
+
+            if (alertasUsuario.Any())
             {
-
-                var alertasUsuario = await _db.Set<AlertasUsuario>()
-                    .Where(au => au.UsuarioId == usuarioId)
-                    .ToListAsync();
-
-                if (alertasUsuario.Any())
-                {
-
-                    _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
-
-                }
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "RemoverAlertasDoUsuarioAsync", ex);
-                throw;
+                _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
             }
         }
 
         public async Task RemoverUsuariosDoAlertaAsync(Guid alertaId)
         {
-            try
+            var alertasUsuario = await _db.Set<AlertasUsuario>()
+                .Where(au => au.AlertasFrotiXId == alertaId)
+                .ToListAsync();
+
+            if (alertasUsuario.Any())
             {
-
-                var alertasUsuario = await _db.Set<AlertasUsuario>()
-                    .Where(au => au.AlertasFrotiXId == alertaId)
-                    .ToListAsync();
-
-                if (alertasUsuario.Any())
-                {
-
-                    _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
-
-                }
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "RemoverUsuariosDoAlertaAsync", ex);
-                throw;
+                _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
             }
         }
 
         public new void Update(AlertasUsuario alertaUsuario)
         {
-            try
-            {
+            if (alertaUsuario == null)
+                throw new ArgumentNullException(nameof(alertaUsuario));
 
-                if (alertaUsuario == null)
-                    throw new ArgumentNullException(nameof(alertaUsuario));
-
-                _db.Set<AlertasUsuario>().Update(alertaUsuario);
-
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "Update", ex);
-                throw;
-            }
+            _db.Set<AlertasUsuario>().Update(alertaUsuario);
         }
     }
 }
```
