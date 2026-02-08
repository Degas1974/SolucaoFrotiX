# Services/CacheWarmupService.cs

**Mudanca:** GRANDE | **+232** linhas | **-85** linhas

---

```diff
--- JANEIRO: Services/CacheWarmupService.cs
+++ ATUAL: Services/CacheWarmupService.cs
@@ -3,11 +3,13 @@
 using System.Linq;
 using System.Threading;
 using System.Threading.Tasks;
+using FrotiX.Helpers;
 using FrotiX.Infrastructure;
 using FrotiX.Models.DTO;
 using FrotiX.Repository.IRepository;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.Caching.Memory;
+using Microsoft.Extensions.Configuration;
 using Microsoft.Extensions.DependencyInjection;
 using Microsoft.Extensions.Hosting;
 using Microsoft.Extensions.Logging;
@@ -19,122 +21,281 @@
     private readonly ILogger<CacheWarmupService> _log;
     private CancellationTokenSource? _cts;
     private Task? _refreshLoop;
-
-    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(30);
-    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(10);
+    private Task? _warmupTask;
+
+    private readonly bool _warmupEnabled;
+    private readonly TimeSpan _warmupTimeout;
+    private readonly TimeSpan _ttl;
+    private readonly TimeSpan _refreshInterval;
+
+    private const string FotoFallback = "/images/barbudo.jpg";
 
     public CacheWarmupService(
         IServiceProvider sp,
         IMemoryCache cache,
-        ILogger<CacheWarmupService> log
+        ILogger<CacheWarmupService> log,
+        IConfiguration config
     )
     {
-        _sp = sp;
-        _cache = cache;
-        _log = log;
-    }
-
-    public async Task StartAsync(CancellationToken cancellationToken)
-    {
-        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
-
-        await WarmAsync(_cts.Token);
-
-        _refreshLoop = Task.Run(() => RefreshLoopAsync(_cts.Token), _cts.Token);
+        Console.WriteLine("[DIAG-CTOR] CacheWarmupService INICIO...");
+        try
+        {
+            _sp = sp;
+            _cache = cache;
+            _log = log;
+
+            var section = config.GetSection("CacheWarmup");
+
+            _warmupEnabled = section.GetValue("Enabled", true);
+
+            var warmupTimeoutSeconds = section.GetValue("WarmupTimeoutSeconds", 15);
+            var refreshIntervalMinutes = section.GetValue("RefreshIntervalMinutes", 5);
+            var cacheTtlMinutes = section.GetValue("CacheTtlMinutes", 10);
+
+            _warmupTimeout = TimeSpan.FromSeconds(Math.Max(1, warmupTimeoutSeconds));
+            _refreshInterval = TimeSpan.FromMinutes(Math.Max(1, refreshIntervalMinutes));
+            _ttl = TimeSpan.FromMinutes(Math.Max(1, cacheTtlMinutes));
+            Console.WriteLine("[DIAG-CTOR] CacheWarmupService FIM");
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", ".ctor", error);
+            throw;
+        }
+    }
+
+    public Task StartAsync(CancellationToken cancellationToken)
+    {
+        try
+        {
+            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
+
+            if (_warmupEnabled)
+            {
+                _warmupTask = Task.Run(() => WarmupWithTimeoutAsync(_cts.Token), _cts.Token);
+            }
+            else
+            {
+                _log.LogInformation("CacheWarmupService: warmup desabilitado por configuracao.");
+            }
+
+            _refreshLoop = Task.Run(() => RefreshLoopAsync(_cts.Token), _cts.Token);
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "StartAsync", error);
+            _log.LogError(error, "Falha ao iniciar CacheWarmupService.");
+        }
+
+        return Task.CompletedTask;
     }
 
     public async Task StopAsync(CancellationToken cancellationToken)
     {
-        if (_cts is not null)
-        {
-            _cts.Cancel();
-            try
+        try
+        {
+            if (_cts is not null)
             {
-                if (_refreshLoop is not null)
-                    await _refreshLoop;
+                _cts.Cancel();
+                try
+                {
+                    if (_warmupTask is not null)
+                        await _warmupTask;
+                    if (_refreshLoop is not null)
+                        await _refreshLoop;
+                }
+                catch
+                {
+                }
             }
-            catch
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "StopAsync", error);
+            _log.LogError(error, "Falha ao parar CacheWarmupService.");
+        }
+    }
+
+    private async Task RefreshLoopAsync(CancellationToken ct)
+    {
+        try
+        {
+            if (_refreshInterval <= TimeSpan.Zero)
             {
+                _log.LogWarning("CacheWarmupService: refresh desabilitado (intervalo <= 0).");
+                return;
             }
-        }
-    }
-
-    private async Task RefreshLoopAsync(CancellationToken ct)
-    {
-        var timer = new PeriodicTimer(_refreshInterval);
-        while (await timer.WaitForNextTickAsync(ct))
-            await WarmAsync(ct);
+
+            using var timer = new PeriodicTimer(_refreshInterval);
+            while (await timer.WaitForNextTickAsync(ct))
+                await WarmAsync(ct);
+        }
+        catch (OperationCanceledException) when (ct.IsCancellationRequested)
+        {
+            _log.LogInformation("CacheWarmupService: loop de refresh cancelado.");
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "RefreshLoopAsync", error);
+            _log.LogError(error, "Falha no loop de refresh do cache.");
+        }
+    }
+
+    private async Task WarmupWithTimeoutAsync(CancellationToken ct)
+    {
+        try
+        {
+            using var timeoutCts = new CancellationTokenSource(_warmupTimeout);
+            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
+                ct,
+                timeoutCts.Token
+            );
+
+            await WarmAsync(linkedCts.Token);
+        }
+        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
+        {
+            _log.LogWarning(
+                "CacheWarmupService: warmup cancelado por timeout ({timeout}s).",
+                _warmupTimeout.TotalSeconds
+            );
+        }
+        catch (OperationCanceledException)
+        {
+            _log.LogInformation("CacheWarmupService: warmup cancelado.");
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "WarmupWithTimeoutAsync", error);
+            _log.LogError(error, "Falha no warmup do cache.");
+        }
     }
 
     private async Task WarmAsync(CancellationToken ct)
     {
-        using var scope = _sp.CreateScope();
-        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
-
-        var motoristas = await uow
-            .ViewMotoristas.GetAllReducedIQueryable(
-                v => new { v.MotoristaId, Nome = v.MotoristaCondutor },
-                asNoTracking: true
-            )
-            .OrderBy(x => x.Nome)
-            .Select(x => new MotoristaData(x.MotoristaId, x.Nome ?? string.Empty))
-            .ToListAsync(ct);
-
-        Set(CacheKeys.Motoristas, motoristas);
-
-        var veiculos = await uow
-            .ViewVeiculosManutencao.GetAllReducedIQueryable(
-                v => new
+        try
+        {
+            using var scope = _sp.CreateScope();
+            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
+
+            var motoristasRaw = await uow
+                .ViewMotoristasViagem.GetAllReducedIQueryable(
+                    v => new { v.MotoristaId, v.MotoristaCondutor, v.Foto, v.Status },
+                    asNoTracking: true
+                )
+                .Where(m => m.Status == true)
+                .OrderBy(x => x.MotoristaCondutor)
+                .ToListAsync(ct);
+
+            var motoristas = motoristasRaw
+                .Select(m => new MotoristaDataComFoto(
+                    m.MotoristaId,
+                    m.MotoristaCondutor ?? string.Empty,
+                    ConverteFotoBase64(m.Foto)
+                ))
+                .ToList();
+
+            Set(CacheKeys.Motoristas, motoristas);
+
+            var veiculos = await uow
+                .ViewVeiculos.GetAllReducedIQueryable(
+                    v => new { v.VeiculoId, v.VeiculoCompleto, v.Status },
+                    asNoTracking: true
+                )
+                .Where(v => v.Status == true)
+                .OrderBy(x => x.VeiculoCompleto)
+                .Select(x => new VeiculoData(x.VeiculoId, x.VeiculoCompleto ?? string.Empty))
+                .ToListAsync(ct);
+
+            Set(CacheKeys.Veiculos, veiculos);
+
+            var veiculosManutencao = await uow
+                .ViewVeiculosManutencao.GetAllReducedIQueryable(
+                    v => new { v.VeiculoId, v.Descricao },
+                    asNoTracking: true
+                )
+                .OrderBy(x => x.Descricao)
+                .Select(x => new VeiculoData(x.VeiculoId, x.Descricao ?? string.Empty))
+                .ToListAsync(ct);
+
+            Set(CacheKeys.VeiculosManutencao, veiculosManutencao);
+
+            var veiculosReserva = await uow
+                .ViewVeiculosManutencaoReserva.GetAllReducedIQueryable(
+                    v => new { v.VeiculoId, v.Descricao },
+                    asNoTracking: true
+                )
+                .OrderBy(x => x.Descricao)
+                .Select(x => new VeiculoReservaData(x.VeiculoId, x.Descricao ?? string.Empty))
+                .ToListAsync(ct);
+
+            Set(CacheKeys.VeiculosReserva, veiculosReserva);
+
+            _log.LogInformation(
+                "Warm-up concluído: {m} motoristas, {v} veículos, {vm} manutenção, {vr} reserva",
+                motoristas.Count,
+                veiculos.Count,
+                veiculosManutencao.Count,
+                veiculosReserva.Count
+            );
+        }
+        catch (OperationCanceledException) when (ct.IsCancellationRequested)
+        {
+            _log.LogInformation("CacheWarmupService: warmup cancelado.");
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "WarmAsync", error);
+            _log.LogError(error, "Falha ao executar warmup do cache.");
+        }
+    }
+
+    private static string ConverteFotoBase64(byte[]? foto)
+    {
+        if (foto == null || foto.Length == 0)
+            return FotoFallback;
+
+        try
+        {
+            return $"data:image/jpeg;base64,{Convert.ToBase64String(foto)}";
+        }
+        catch
+        {
+            return FotoFallback;
+        }
+    }
+
+    private void Set<T>(string key, List<T> value)
+    {
+        try
+        {
+            _cache.Set(
+                key,
+                value,
+                new MemoryCacheEntryOptions
                 {
-                    v.VeiculoId,
-                    v.Descricao,
-                },
-                asNoTracking: true
-            )
-            .OrderBy(x => x.Descricao)
-            .Select(x => new VeiculoData(x.VeiculoId, x.Descricao ?? string.Empty))
-            .ToListAsync(ct);
-
-        Set(CacheKeys.Veiculos, veiculos);
-
-        var veiculosReserva = await uow
-            .ViewVeiculosManutencaoReserva.GetAllReducedIQueryable(
-                v => new
-                {
-                    v.VeiculoId,
-                    v.Descricao,
-                },
-                asNoTracking: true
-            )
-            .OrderBy(x => x.Descricao)
-            .Select(x => new VeiculoData(
-                x.VeiculoId,
-                x.Descricao ?? string.Empty
-            ))
-            .ToListAsync(ct);
-
-        Set(CacheKeys.VeiculosReserva, veiculosReserva);
-
-        _log.LogInformation(
-            "Warm-up concluído: {m} motoristas, {v} veículos",
-            motoristas.Count,
-            veiculos.Count
-        );
-    }
-
-    private void Set<T>(string key, List<T> value)
-    {
-        _cache.Set(
-            key,
-            value,
-            new MemoryCacheEntryOptions
-            {
-                AbsoluteExpirationRelativeToNow = _ttl,
-                Priority = CacheItemPriority.High,
-                Size = 1
-            }
-        );
-    }
-
-    public void Dispose() => _cts?.Dispose();
+                    AbsoluteExpirationRelativeToNow = _ttl,
+                    Priority = CacheItemPriority.High,
+                }
+            );
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "Set", error);
+            _log.LogError(error, "Falha ao gravar cache: {key}", key);
+        }
+    }
+
+    public void Dispose()
+    {
+        try
+        {
+            _cts?.Dispose();
+        }
+        catch (Exception error)
+        {
+            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "Dispose", error);
+            _log.LogError(error, "Falha ao liberar recursos do CacheWarmupService.");
+        }
+    }
 }
```
