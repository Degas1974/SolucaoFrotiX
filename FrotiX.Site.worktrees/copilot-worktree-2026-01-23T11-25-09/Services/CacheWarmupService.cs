/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - CACHE WARMUP                                                            #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

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
using Microsoft.Extensions.Dependency Injection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// ╔══════════════════════════════════════════════════════════════════════════════╗
/// ║ 📌 NOME: CacheWarmupService                                                  ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 📝 DESCRIÇÃO:                                                                ║
/// ║    Serviço de background que pré-carrega dados críticos no cache na          ║
/// ║    inicialização do sistema e os mantém atualizados periodicamente.          ║
/// ║                                                                              ║
/// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
/// ║    PERFORMANCE CRÍTICA - Elimina queries pesadas do banco durante as         ║
/// ║    primeiras requisições. Carrega navigation menus, veículos, motoristas e   ║
/// ║    outros dados frequentes ANTES do primeiro acesso.                         ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 📊 CONFIGURAÇÃO:                                                             ║
/// ║    • TTL Cache: 30 minutos                                                   ║
/// ║    • Refresh: A cada 10 minutos                                              ║
/// ║    • Warm-up: Bloqueante na inicialização                                    ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
/// ║    • StartAsync() → Warm-up inicial bloqueante + inicia loop de refresh      ║
/// ║    • WarmAsync() → Carrega todos os dados no cache                           ║
/// ║    • RefreshLoopAsync() → Atualiza cache periodicamente                      ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 🔗 ESCOPO: INTERNA - Serviço de infraestrutura crítico para performance      ║
/// ║    • Arquivos relacionados: Startup.cs (configuração), IMemoryCache         ║
/// ╚══════════════════════════════════════════════════════════════════════════════╝
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

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: StartAsync                                                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Inicializa o serviço com warm-up BLOQUEANTE seguido de loop de refresh.   ║
    /// ║    Garante que o cache esteja pronto antes de aceitar requisições HTTP.      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • cancellationToken: Token de cancelamento do host                        ║
    /// ║                                                                              ║
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • Task: Tarefa assíncrona de inicialização                                ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        // 1) Warm-up BLOQUEANTE (garante cache pronto antes de atender requests)
        await WarmAsync(_cts.Token);

        // 2) Loop de refresh em background
        _refreshLoop = Task.Run(() => RefreshLoopAsync(_cts.Token), _cts.Token);
    }

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

    private async Task RefreshLoopAsync(CancellationToken ct)
    {
        var timer = new PeriodicTimer(_refreshInterval);
        while (await timer.WaitForNextTickAsync(ct))
            await WarmAsync(ct);
    }

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
                Size = 1 // Cada entrada conta como 1 unidade no SizeLimit
            }
        );
    }

    public void Dispose() => _cts?.Dispose();
}
