# Services/MotoristaFotoService.cs

**Mudanca:** PEQUENA | **+1** linhas | **-1** linhas

---

```diff
--- JANEIRO: Services/MotoristaFotoService.cs
+++ ATUAL: Services/MotoristaFotoService.cs
@@ -38,7 +38,7 @@
 
             fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(resized)}";
 
-            _cache.Set(cacheKey, fotoBase64, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), Size = 1 });
+            _cache.Set(cacheKey, fotoBase64, TimeSpan.FromHours(1));
             return fotoBase64;
             }
 
```

### REMOVER do Janeiro

```csharp
            _cache.Set(cacheKey, fotoBase64, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), Size = 1 });
```


### ADICIONAR ao Janeiro

```csharp
            _cache.Set(cacheKey, fotoBase64, TimeSpan.FromHours(1));
```
