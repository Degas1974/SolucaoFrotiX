/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: CacheWarmupService.cs                                                                   ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: IHostedService que pré-carrega cache IMemoryCache no startup. Refresh a cada 5min.     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 ÍNDICE: StartAsync, StopAsync, WarmAsync, RefreshLoopAsync, WarmupWithTimeoutAsync              ║
   ║ 🔗 DEPS: IMemoryCache, IUnitOfWork, CacheKeys | 📅 04/02/2026 | 👤 Copilot | 📝 v3.0                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

// Services/CacheWarmupService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Infrastructure;
using FrotiX.Models.DTO;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public sealed class CacheWarmupService : IHostedService, IDisposable
{
    private readonly IServiceProvider _sp;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheWarmupService> _log;
    private CancellationTokenSource? _cts;
    private Task? _refreshLoop;
    private Task? _warmupTask;

    private readonly bool _warmupEnabled;
    private readonly TimeSpan _warmupTimeout;
    private readonly TimeSpan _ttl;
    private readonly TimeSpan _refreshInterval;
    
    // Fallback foto padrão para motoristas sem foto
    private const string FotoFallback = "/images/barbudo.jpg";

    public CacheWarmupService(
        IServiceProvider sp,
        IMemoryCache cache,
        ILogger<CacheWarmupService> log,
        IConfiguration config
    )
    {
        try
        {
            _sp = sp;
            _cache = cache;
            _log = log;

            var section = config.GetSection("CacheWarmup");

            _warmupEnabled = section.GetValue("Enabled", true);

            var warmupTimeoutSeconds = section.GetValue("WarmupTimeoutSeconds", 15);
            var refreshIntervalMinutes = section.GetValue("RefreshIntervalMinutes", 5); // Default: 5 minutos
            var cacheTtlMinutes = section.GetValue("CacheTtlMinutes", 10); // Default: 10 minutos (2x refresh)

            _warmupTimeout = TimeSpan.FromSeconds(Math.Max(1, warmupTimeoutSeconds));
            _refreshInterval = TimeSpan.FromMinutes(Math.Max(1, refreshIntervalMinutes));
            _ttl = TimeSpan.FromMinutes(Math.Max(1, cacheTtlMinutes));
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", ".ctor", error);
            throw;
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: StartAsync
     * --------------------------------------------------------------------------------------
    * 🎯 OBJETIVO     : Iniciar o IHostedService sem bloquear o startup e iniciar
    *                   o loop de refresh em background
     *
     * 📥 ENTRADAS     : cancellationToken [CancellationToken] - Token para parar serviço
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona do ciclo de vida
     *
     * ⬅️ CHAMADO POR  : ASP.NET Core Host (durante startup)
     *
    * ➡️ CHAMA        : WarmupWithTimeoutAsync() [linha 122]
    *                   RefreshLoopAsync() [linha 147]
     *
     * 📝 OBSERVAÇÕES  : Warm-up NAO BLOQUEANTE permite que Kestrel escute mesmo se
     *                   o banco estiver lento ou indisponivel.
     ***********************************************************************************/
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // 1) Warm-up NAO BLOQUEANTE (permite Kestrel iniciar)
            if (_warmupEnabled)
            {
                _warmupTask = Task.Run(() => WarmupWithTimeoutAsync(_cts.Token), _cts.Token);
            }
            else
            {
                _log.LogInformation("CacheWarmupService: warmup desabilitado por configuracao.");
            }

            // 2) Loop de refresh em background
            _refreshLoop = Task.Run(() => RefreshLoopAsync(_cts.Token), _cts.Token);
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "StartAsync", error);
            _log.LogError(error, "Falha ao iniciar CacheWarmupService.");
        }

        return Task.CompletedTask;
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: StopAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Parar o IHostedService de forma segura. Cancelar loop de refresh
     *                   e aguardar finalização da tarefa em background
     *
     * 📥 ENTRADAS     : cancellationToken [CancellationToken] - Token de cancelamento
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona de parada
     *
     * ⬅️ CHAMADO POR  : ASP.NET Core Host (durante shutdown da aplicação)
     *
     * ➡️ CHAMA        : _cts.Cancel() [linha 91]
     *
     * 📝 OBSERVAÇÕES  : Aguarda finalização segura do _refreshLoop. Ignora exceções
     *                   de cancelamento (esperadas e normais).
     ***********************************************************************************/
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_cts is not null)
            {
                _cts.Cancel();
                try
                {
                    if (_warmupTask is not null)
                        await _warmupTask;
                    if (_refreshLoop is not null)
                        await _refreshLoop;
                }
                catch
                { /* ignore */
                }
            }
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "StopAsync", error);
            _log.LogError(error, "Falha ao parar CacheWarmupService.");
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: RefreshLoopAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Loop infinito que executa WarmAsync periodicamente (a cada 5min)
     *                   para manter cache atualizado com dados novos do banco
     *
     * 📥 ENTRADAS     : ct [CancellationToken] - Token para parar o loop
     *
     * 📤 SAÍDAS       : Task - Tarefa assíncrona nunca completa (até cancelamento)
     *
     * ⬅️ CHAMADO POR  : StartAsync() [linha 74] - Task.Run em background
     *
     * ➡️ CHAMA        : WarmAsync() [linha 107]
     *                   PeriodicTimer.WaitForNextTickAsync() [Intervalo: 5min]
     *
     * 📝 OBSERVAÇÕES  : Usa PeriodicTimer (C# 11+) para scheduling. TTL do cache: 10min,
     *                   Refresh: 5min → cache sempre fresco (overlap de 5min de cobertura)
     ***********************************************************************************/
    private async Task RefreshLoopAsync(CancellationToken ct)
    {
        try
        {
            if (_refreshInterval <= TimeSpan.Zero)
            {
                _log.LogWarning("CacheWarmupService: refresh desabilitado (intervalo <= 0).");
                return;
            }

            using var timer = new PeriodicTimer(_refreshInterval);
            while (await timer.WaitForNextTickAsync(ct))
                await WarmAsync(ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            _log.LogInformation("CacheWarmupService: loop de refresh cancelado.");
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "RefreshLoopAsync", error);
            _log.LogError(error, "Falha no loop de refresh do cache.");
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: WarmupWithTimeoutAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar warm-up com timeout para evitar travar o startup.
     *
     * 📥 ENTRADAS     : ct [CancellationToken] - Token para cancelamento global
     *
     * 📤 SAÍDAS       : Task - Operacao assincrona finalizada
     *
     * ⬅️ CHAMADO POR  : StartAsync() [linha 86]
     *
     * ➡️ CHAMA        : WarmAsync() [linha 158]
     *
     * 📝 OBSERVAÇÕES  : Em caso de timeout, apenas registra log e segue.
     ***********************************************************************************/
    private async Task WarmupWithTimeoutAsync(CancellationToken ct)
    {
        try
        {
            using var timeoutCts = new CancellationTokenSource(_warmupTimeout);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                ct,
                timeoutCts.Token
            );

            await WarmAsync(linkedCts.Token);
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            _log.LogWarning(
                "CacheWarmupService: warmup cancelado por timeout ({timeout}s).",
                _warmupTimeout.TotalSeconds
            );
        }
        catch (OperationCanceledException)
        {
            _log.LogInformation("CacheWarmupService: warmup cancelado.");
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "WarmupWithTimeoutAsync", error);
            _log.LogError(error, "Falha no warmup do cache.");
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: WarmAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Buscar dados do banco (motoristas c/ foto, veículos) e popular cache
     *                   com expiração e prioridade HIGH. Core do sistema de cache.
     *
     * 📥 ENTRADAS     : ct [CancellationToken] - Token para cancelamento
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona completa
     *
     * ⬅️ CHAMADO POR  : StartAsync() [linha 71] - Bloqueante no startup
     *                   RefreshLoopAsync() [linha 107] - A cada 5min
     *
     * ➡️ CHAMA        : _sp.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>()
     *                   unitOfWork.ViewMotoristasViagem [DB] - Motoristas COM FOTO
     *                   unitOfWork.ViewVeiculos [DB] - Veículos (VeiculoCompleto)
     *                   unitOfWork.ViewVeiculosManutencao [DB] - Veículos Manutenção
     *                   unitOfWork.ViewVeiculosManutencaoReserva [DB] - Veículos Reserva
     *                   Set() - Persiste em cache
     *
     * 📝 OBSERVAÇÕES  : Usa DependencyInjection via scope para resolver UnitOfWork.
     *                   Cria DTOs (MotoristaDataComFoto, VeiculoData) para minimizar footprint.
     *                   AsNoTracking() crucial para performance (sem rastreamento EF).
     ***********************************************************************************/
    private async Task WarmAsync(CancellationToken ct)
    {
        try
        {
            using var scope = _sp.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            // =======================
            // MOTORISTAS COM FOTO
            // =======================
            // [DB] Buscar motoristas com ID, Nome e Foto (sem tracking EF)
            // LOGICA: Inclui foto para exibir avatar nos dropdowns
            var motoristasRaw = await uow
                .ViewMotoristasViagem.GetAllReducedIQueryable(
                    v => new { v.MotoristaId, v.MotoristaCondutor, v.Foto, v.Status },
                    asNoTracking: true
                )
                .Where(m => m.Status == true)
                .OrderBy(x => x.MotoristaCondutor)
                .ToListAsync(ct);

            var motoristas = motoristasRaw
                .Select(m => new MotoristaDataComFoto(
                    m.MotoristaId,
                    m.MotoristaCondutor ?? string.Empty,
                    ConverteFotoBase64(m.Foto)
                ))
                .ToList();

            Set(CacheKeys.Motoristas, motoristas);

            // =======================
            // VEÍCULOS (ViewVeiculos.VeiculoCompleto)
            // =======================
            // [DB] Buscar veículos com VeiculoCompleto (Placa + Marca/Modelo)
            // LOGICA: Cache principal para todas as telas de seleção de veículos
            var veiculos = await uow
                .ViewVeiculos.GetAllReducedIQueryable(
                    v => new { v.VeiculoId, v.VeiculoCompleto, v.Status },
                    asNoTracking: true
                )
                .Where(v => v.Status == true)
                .OrderBy(x => x.VeiculoCompleto)
                .Select(x => new VeiculoData(x.VeiculoId, x.VeiculoCompleto ?? string.Empty))
                .ToListAsync(ct);

            Set(CacheKeys.Veiculos, veiculos);

            // =======================
            // VEÍCULOS MANUTENÇÃO
            // =======================
            // [DB] Buscar veículos para telas de manutenção
            var veiculosManutencao = await uow
                .ViewVeiculosManutencao.GetAllReducedIQueryable(
                    v => new { v.VeiculoId, v.Descricao },
                    asNoTracking: true
                )
                .OrderBy(x => x.Descricao)
                .Select(x => new VeiculoData(x.VeiculoId, x.Descricao ?? string.Empty))
                .ToListAsync(ct);

            Set(CacheKeys.VeiculosManutencao, veiculosManutencao);

            // =======================
            // VEÍCULOS RESERVA
            // =======================
            // [DB] Buscar veículos reserva para fallback em caso de indisponibilidade
            var veiculosReserva = await uow
                .ViewVeiculosManutencaoReserva.GetAllReducedIQueryable(
                    v => new { v.VeiculoId, v.Descricao },
                    asNoTracking: true
                )
                .OrderBy(x => x.Descricao)
                .Select(x => new VeiculoReservaData(x.VeiculoId, x.Descricao ?? string.Empty))
                .ToListAsync(ct);

            Set(CacheKeys.VeiculosReserva, veiculosReserva);

            _log.LogInformation(
                "Warm-up concluído: {m} motoristas, {v} veículos, {vm} manutenção, {vr} reserva",
                motoristas.Count,
                veiculos.Count,
                veiculosManutencao.Count,
                veiculosReserva.Count
            );
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            _log.LogInformation("CacheWarmupService: warmup cancelado.");
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "WarmAsync", error);
            _log.LogError(error, "Falha ao executar warmup do cache.");
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: ConverteFotoBase64 (Helper privado)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Converter byte[] de foto para Data URI Base64.
     *
     * 📥 ENTRADAS     : byte[]? foto - Bytes da imagem ou null
     *
     * 📤 SAÍDAS       : string - Data URI Base64 ou fallback
     ***********************************************************************************/
    private static string ConverteFotoBase64(byte[]? foto)
    {
        if (foto == null || foto.Length == 0)
            return FotoFallback;

        try
        {
            return $"data:image/jpeg;base64,{Convert.ToBase64String(foto)}";
        }
        catch
        {
            return FotoFallback;
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: Set<T>
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Genérica para persistir lista em cache com configuração padrão
     *                   (TTL 10min, Prioridade HIGH)
     *
     * 📥 ENTRADAS     : key [string] - Chave de cache (ex: CacheKeys.Motoristas)
     *                   value [List<T>] - Lista a armazenar em cache
     *
     * 📤 SAÍDAS       : void - Cache atualizado
     *
     * ⬅️ CHAMADO POR  : WarmAsync()
     *
     * ➡️ CHAMA        : _cache.Set() [IMemoryCache]
     *
     * 📝 OBSERVAÇÕES  : Genérica para facilitar reuso. TTL e Prioridade são constantes
     *                   (_ttl e CacheItemPriority.High). HIGH prioridade evita eviction
     *                   quando memória fica pressão.
     ***********************************************************************************/
    private void Set<T>(string key, List<T> value)
    {
        try
        {
            _cache.Set(
                key,
                value,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _ttl,
                    Priority = CacheItemPriority.High,
                }
            );
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "Set", error);
            _log.LogError(error, "Falha ao gravar cache: {key}", key);
        }
    }

    public void Dispose()
    {
        try
        {
            _cts?.Dispose();
        }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("CacheWarmupService.cs", "Dispose", error);
            _log.LogError(error, "Falha ao liberar recursos do CacheWarmupService.");
        }
    }
}
