# Services/AlertasBackgroundService.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Services/AlertasBackgroundService.cs
+++ ATUAL: Services/AlertasBackgroundService.cs
@@ -78,6 +78,7 @@
 
                                 foreach (var usuarioId in usuariosNaoNotificados)
                                     {
+
                                     await _hubContext.Clients.User(usuarioId).SendAsync("NovoAlerta" , new
                                         {
                                         alertaId = alerta.AlertasFrotiXId ,
@@ -205,7 +206,7 @@
                     Models.TipoAlerta.Motorista => "Motorista",
                     Models.TipoAlerta.Veiculo => "Veículo",
                     Models.TipoAlerta.Anuncio => "Anúncio",
-                    _ => "Aniversário"
+                    _ => "Diversos"
                     };
             }
         }
```

### REMOVER do Janeiro

```csharp
                    _ => "Aniversário"
```


### ADICIONAR ao Janeiro

```csharp
                    _ => "Diversos"
```
