# Repository/AlertasFrotiXRepository.cs

**Mudanca:** PEQUENA | **+0** linhas | **-3** linhas

---

```diff
--- JANEIRO: Repository/AlertasFrotiXRepository.cs
+++ ATUAL: Repository/AlertasFrotiXRepository.cs
@@ -9,7 +9,6 @@
 
 namespace FrotiX.Repository
 {
-
     public class AlertasFrotiXRepository : Repository<AlertasFrotiX>, IAlertasFrotiXRepository
     {
         private new readonly FrotiXDbContext _db;
@@ -23,7 +22,6 @@
         {
             try
             {
-
                 return await _db.AlertasFrotiX
                     .Include(a => a.AlertasUsuarios)
 
@@ -42,7 +40,6 @@
         {
             try
             {
-
                 return await _db.AlertasFrotiX
                     .Include(a => a.AlertasUsuarios)
                     .Where(a => a.AlertasUsuarios.Any(au => au.Lido))
@@ -60,7 +57,6 @@
         {
             try
             {
-
                 return await _db.AlertasUsuario
                     .Where(au => au.UsuarioId == usuarioId && !au.Lido && !au.Apagado)
                     .Join(_db.AlertasFrotiX,
@@ -81,14 +77,11 @@
         {
             try
             {
-
                 var alertaUsuario = await _db.AlertasUsuario
-                    .AsTracking()
                     .FirstOrDefaultAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);
 
                 if (alertaUsuario != null)
                 {
-
                     alertaUsuario.Lido = true;
                     alertaUsuario.DataLeitura = DateTime.Now;
                     await _db.SaveChangesAsync();
@@ -108,13 +101,11 @@
         {
             try
             {
-
                 _db.AlertasFrotiX.Add(alerta);
                 await _db.SaveChangesAsync();
 
                 if (usuariosIds == null || !usuariosIds.Any())
                 {
-
                     var todosUsuarios = await _db.AspNetUsers.Select(u => u.Id).ToListAsync();
                     usuariosIds = todosUsuarios;
                 }
@@ -170,7 +161,6 @@
             try
             {
                 var alertaUsuario = await _db.AlertasUsuario
-                    .AsTracking()
                     .FirstOrDefaultAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);
 
                 if (alertaUsuario != null && !alertaUsuario.Lido)
@@ -195,7 +185,6 @@
             try
             {
                 var alerta = await _db.AlertasFrotiX
-                    .AsTracking()
                     .FirstOrDefaultAsync(a => a.AlertasFrotiXId == alertaId);
 
                 if (alerta != null)
```

### REMOVER do Janeiro

```csharp
                    .AsTracking()
                    .AsTracking()
                    .AsTracking()
```

