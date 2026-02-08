# Controllers/ViagemController.MetodosEstatisticas.cs

**Mudanca:** MEDIA | **+5** linhas | **-5** linhas

---

```diff
--- JANEIRO: Controllers/ViagemController.MetodosEstatisticas.cs
+++ ATUAL: Controllers/ViagemController.MetodosEstatisticas.cs
@@ -83,7 +83,7 @@
             try
             {
 
-                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
 
                 using (var scope = _serviceScopeFactory.CreateScope())
                 {
@@ -107,7 +107,7 @@
 
                     progresso.Total = datasUnicas.Count;
                     progresso.Mensagem = $"Processando estatísticas de {progresso.Total} datas...";
-                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
 
                     int contador = 0;
 
@@ -125,7 +125,7 @@
                                 : 0;
                             progresso.Mensagem = $"Processando data {contador} de {progresso.Total}... ({data:dd/MM/yyyy})";
 
-                            _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                            _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
 
                             if (contador % 10 == 0)
                             {
@@ -142,17 +142,18 @@
                     progresso.Concluido = true;
                     progresso.Percentual = 100;
                     progresso.Mensagem = $"Processamento concluído! Estatísticas de {contador} datas geradas.";
-                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                 }
             }
             catch (Exception error)
             {
+
                 Alerta.TratamentoErroComLinha("ViagemController.cs" , "ProcessarGeracaoEstatisticas" , error);
 
                 progresso.Erro = true;
                 progresso.Concluido = true;
                 progresso.Mensagem = $"Erro durante o processamento: {error.Message}";
-                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
+                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
             }
         }
 
@@ -166,7 +167,6 @@
 
                 if (_cache.TryGetValue(cacheKey , out ProgressoEstatisticas progresso))
                 {
-
                     return Json(new
                     {
                         success = true ,
@@ -214,6 +214,7 @@
             try
             {
                 var cacheKey = "ProgressoEstatisticas";
+
                 _cache.Remove(cacheKey);
 
                 return Json(new
```

### REMOVER do Janeiro

```csharp
                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
                            _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
                    _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
                _cache.Set(cacheKey, progresso, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), Size = 1 });
```


### ADICIONAR ao Janeiro

```csharp
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                            _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
```
