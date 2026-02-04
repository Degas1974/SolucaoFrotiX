/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: CacheWarmupService.cs                                                                   ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: IHostedService que pré-carrega cache IMemoryCache no startup. Refresh a cada 10min.    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: StartAsync, StopAsync, WarmupAsync, RefreshLoopAsync (TTL: 30min, Refresh: 10min)        ║
   ║ 🔗 DEPS: IMemoryCache, IUnitOfWork, CacheKeys | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

// Services/CacheWarmupService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Infrastructure;
using FrotiX.Models.DTO;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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

    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(30);
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(10);

    public CacheWarmupService(
        IServiceProvider sp,
        IMemoryCache cache,
        ILogger<CacheWarmupService> log
    )
    {
        _sp = sp;
        _cache = cache;
        _log = log;
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: StartAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Iniciar o IHostedService. Bloqueia para warm-up do cache,
     *                   depois inicia loop de refresh em background
     *
     * 📥 ENTRADAS     : cancellationToken [CancellationToken] - Token para parar serviço
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona do ciclo de vida
     *
     * ⬅️ CHAMADO POR  : ASP.NET Core Host (durante startup)
     *
     * ➡️ CHAMA        : WarmAsync() [linha 57]
     *                   RefreshLoopAsync() [linha 58]
     *
     * 📝 OBSERVAÇÕES  : Warm-up BLOQUEANTE garante cache pronto antes de requests.
     *                   Loop de refresh roda assincronamente em background.
     ***********************************************************************************/
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        // 1) Warm-up BLOQUEANTE (garante cache pronto antes de atender requests)
        await WarmAsync(_cts.Token);

        // 2) Loop de refresh em background
        _refreshLoop = Task.Run(() => RefreshLoopAsync(_cts.Token), _cts.Token);
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
        if (_cts is not null)
        {
            _cts.Cancel();
            try
            {
                if (_refreshLoop is not null)
                    await _refreshLoop;
            }
            catch
            { /* ignore */
            }
        }
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: RefreshLoopAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Loop infinito que executa WarmAsync periodicamente (a cada 10min)
     *                   para manter cache atualizado com dados novos do banco
     *
     * 📥 ENTRADAS     : ct [CancellationToken] - Token para parar o loop
     *
     * 📤 SAÍDAS       : Task - Tarefa assíncrona nunca completa (até cancelamento)
     *
     * ⬅️ CHAMADO POR  : StartAsync() [linha 74] - Task.Run em background
     *
     * ➡️ CHAMA        : WarmAsync() [linha 107]
     *                   PeriodicTimer.WaitForNextTickAsync() [Intervalo: 10min]
     *
     * 📝 OBSERVAÇÕES  : Usa PeriodicTimer (C# 11+) para scheduling. TTL do cache: 30min,
     *                   Refresh: 10min → cache sempre fresco (overlap de 20min de cobertura)
     ***********************************************************************************/
    private async Task RefreshLoopAsync(CancellationToken ct)
    {
        var timer = new PeriodicTimer(_refreshInterval);
        while (await timer.WaitForNextTickAsync(ct))
            await WarmAsync(ct);
    }

    /***********************************************************************************
     * ⚡ FUNÇÃO: WarmAsync
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Buscar dados do banco (motoristas, veículos) e popular cache
     *                   com expiração e prioridade HIGH. Core do sistema de cache.
     *
     * 📥 ENTRADAS     : ct [CancellationToken] - Token para cancelamento
     *
     * 📤 SAÍDAS       : Task - Operação assíncrona completa
     *
     * ⬅️ CHAMADO POR  : StartAsync() [linha 71] - Bloqueante no startup
     *                   RefreshLoopAsync() [linha 107] - A cada 10min
     *
     * ➡️ CHAMA        : _sp.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>()
     *                   unitOfWork.ViewMotoristas.GetAllReducedIQueryable() [DB]
     *                   unitOfWork.ViewVeiculosManutencao.GetAllReducedIQueryable() [DB]
     *                   unitOfWork.ViewVeiculosManutencaoReserva.GetAllReducedIQueryable() [DB]
     *                   Set() [linha 149] - Persiste em cache
     *
     * 📝 OBSERVAÇÕES  : Usa DependencyInjection via scope para resolver UnitOfWork.
     *                   Cria DTOs (MotoristaData, VeiculoData) para minimizar footprint.
     *                   AsNoTracking() crucial para performance (sem rastreamento EF).
     ***********************************************************************************/
    private async Task WarmAsync(CancellationToken ct)
    {
        using var scope = _sp.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        // =======================
        // MOTORISTAS
        // =======================
        var motoristas = await uow
            .ViewMotoristas.GetAllReducedIQueryable(
                v => new { v.MotoristaId, Nome = v.MotoristaCondutor },
                asNoTracking: true
            )
            .OrderBy(x => x.Nome) // ordena por campo simples -> SQL ok
            .Select(x => new MotoristaData(x.MotoristaId, x.Nome ?? string.Empty))
            .ToListAsync(ct);

        Set(CacheKeys.Motoristas, motoristas);

        // =======================
        // VEÍCULOS
        // =======================
        var veiculos = await uow
            .ViewVeiculosManutencao.GetAllReducedIQueryable(
                v => new
                {
                    v.VeiculoId, // pode ser Guid
                    v.Descricao,
                },
                asNoTracking: true
            )
            .OrderBy(x => x.Descricao)
            .Select(x => new VeiculoData(x.VeiculoId, x.Descricao ?? string.Empty))
            .ToListAsync(ct);

        Set(CacheKeys.Veiculos, veiculos);

        // =======================
        // VEÍCULOS RESERVA (se usar)
        // =======================
        // Se você removeu essa lista, apague este bloco e a chave.
        var veiculosReserva = await uow
            .ViewVeiculosManutencaoReserva.GetAllReducedIQueryable(
                v => new
                {
                    v.VeiculoId, // pode ser Guid
                    v.Descricao,
                },
                asNoTracking: true
            )
            .OrderBy(x => x.Descricao)
            .Select(x => new VeiculoData(
                x.VeiculoId, // Remove ?? Guid.Empty
                x.Descricao ?? string.Empty // Mantém para string nullable
            ))
            .ToListAsync(ct);

        // comente esta linha se não usar reserva
        Set(CacheKeys.VeiculosReserva, veiculosReserva);

        _log.LogInformation(
            "Warm-up concluído: {m} motoristas, {v} veículos",
            motoristas.Count,
            veiculos.Count
        );
    }

    private void Set<T>(string key, List<T> value)
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

    public void Dispose() => _cts?.Dispose();
}
