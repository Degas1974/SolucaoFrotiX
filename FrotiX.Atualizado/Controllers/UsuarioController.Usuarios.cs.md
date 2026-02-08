# Controllers/UsuarioController.Usuarios.cs

**Mudanca:** PEQUENA | **+1** linhas | **-3** linhas

---

```diff
--- JANEIRO: Controllers/UsuarioController.Usuarios.cs
+++ ATUAL: Controllers/UsuarioController.Usuarios.cs
@@ -17,9 +17,7 @@
         {
             try
             {
-
                 var usuarios = _unitOfWork.AspNetUsers.GetAll().OrderBy(u => u.NomeCompleto).ToList();
-
                 var result = new List<object>();
 
                 foreach (var u in usuarios)
@@ -86,9 +84,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UsuarioController", "GetAll");
                 Alerta.TratamentoErroComLinha("UsuarioController.Usuarios.cs", "GetAll", error);
-
                 return Json(new
                 {
                     success = false,
@@ -103,7 +99,6 @@
         {
             try
             {
-
                 if (string.IsNullOrEmpty(usuarioId))
                 {
                     return Json(new
@@ -136,9 +131,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro", error, "UsuarioController", "GetFoto");
-                Alerta.TratamentoErroComLinha("UsuarioController.Usuarios.cs", "GetFoto", error);
-
+                Alerta.TratamentoErroComLinha("UsuarioController.cs", "GetFoto", error);
                 return Json(new
                 {
                     success = false,
```

### REMOVER do Janeiro

```csharp
                _log.Error("Erro", error, "UsuarioController", "GetAll");
                _log.Error("Erro", error, "UsuarioController", "GetFoto");
                Alerta.TratamentoErroComLinha("UsuarioController.Usuarios.cs", "GetFoto", error);
```


### ADICIONAR ao Janeiro

```csharp
                Alerta.TratamentoErroComLinha("UsuarioController.cs", "GetFoto", error);
```
