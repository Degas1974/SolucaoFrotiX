# Services/ListaCacheService.cs

**ARQUIVO NOVO** | 174 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Infrastructure;
using FrotiX.Models.DTO;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services
{

    public class ListaCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ListaCacheService> _log;

        private static readonly TimeSpan FallbackTtl = TimeSpan.FromMinutes(5);

        private const string FotoFallback = "/images/barbudo.jpg";

        public ListaCacheService(
            IMemoryCache cache,
            IUnitOfWork unitOfWork,
            ILogger<ListaCacheService> log)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public async Task<List<MotoristaDataComFoto>> GetMotoristasAsync()
        {
            try
            {

                if (_cache.TryGetValue(CacheKeys.Motoristas, out List<MotoristaDataComFoto>? cached) && cached != null)
                {
                    return cached;
                }

                _log.LogWarning("Cache miss para {CacheKey}. Carregando do banco...", CacheKeys.Motoristas);

                var motoristas = await _unitOfWork
                    .ViewMotoristasViagem.GetAllReducedIQueryable(
                        v => new { v.MotoristaId, v.MotoristaCondutor, v.Foto, v.Status },
                        asNoTracking: true
                    )
                    .Where(m => m.Status == true)
                    .OrderBy(m => m.MotoristaCondutor)
                    .ToListAsync();

                var lista = motoristas.Select(m => new MotoristaDataComFoto(
                    m.MotoristaId,
                    m.MotoristaCondutor ?? string.Empty,
                    ConverteFotoBase64(m.Foto)
                )).ToList();

                _cache.Set(CacheKeys.Motoristas, lista, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = FallbackTtl,
                    Priority = CacheItemPriority.High
                });

                return lista;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaCacheService.cs", "GetMotoristasAsync", error);
                _log.LogError(error, "Erro ao obter motoristas do cache/DB");
                return new List<MotoristaDataComFoto>();
            }
        }

        public async Task<List<VeiculoData>> GetVeiculosAsync()
        {
            try
            {

                if (_cache.TryGetValue(CacheKeys.Veiculos, out List<VeiculoData>? cached) && cached != null)
                {
                    return cached;
                }

                _log.LogWarning("Cache miss para {CacheKey}. Carregando do banco...", CacheKeys.Veiculos);

                var veiculos = await _unitOfWork
                    .ViewVeiculos.GetAllReducedIQueryable(
                        v => new { v.VeiculoId, v.VeiculoCompleto, v.Status },
                        asNoTracking: true
                    )
                    .Where(v => v.Status == true)
                    .OrderBy(v => v.VeiculoCompleto)
                    .Select(v => new VeiculoData(v.VeiculoId, v.VeiculoCompleto ?? string.Empty))
                    .ToListAsync();

                _cache.Set(CacheKeys.Veiculos, veiculos, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = FallbackTtl,
                    Priority = CacheItemPriority.High
                });

                return veiculos;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaCacheService.cs", "GetVeiculosAsync", error);
                _log.LogError(error, "Erro ao obter veículos do cache/DB");
                return new List<VeiculoData>();
            }
        }

        public async Task<List<VeiculoData>> GetVeiculosManutencaoAsync()
        {
            try
            {

                if (_cache.TryGetValue(CacheKeys.VeiculosManutencao, out List<VeiculoData>? cached) && cached != null)
                {
                    return cached;
                }

                _log.LogWarning("Cache miss para {CacheKey}. Carregando do banco...", CacheKeys.VeiculosManutencao);

                var veiculos = await _unitOfWork
                    .ViewVeiculosManutencao.GetAllReducedIQueryable(
                        v => new { v.VeiculoId, v.Descricao },
                        asNoTracking: true
                    )
                    .OrderBy(v => v.Descricao)
                    .Select(v => new VeiculoData(v.VeiculoId, v.Descricao ?? string.Empty))
                    .ToListAsync();

                _cache.Set(CacheKeys.VeiculosManutencao, veiculos, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = FallbackTtl,
                    Priority = CacheItemPriority.High
                });

                return veiculos;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaCacheService.cs", "GetVeiculosManutencaoAsync", error);
                _log.LogError(error, "Erro ao obter veículos manutenção do cache/DB");
                return new List<VeiculoData>();
            }
        }

        public async Task<List<VeiculoReservaData>> GetVeiculosReservaAsync()
        {
            try
            {

                if (_cache.TryGetValue(CacheKeys.VeiculosReserva, out List<VeiculoReservaData>? cached) && cached != null)
                {
                    return cached;
                }

                _log.LogWarning("Cache miss para {CacheKey}. Carregando do banco...", CacheKeys.VeiculosReserva);

                var veiculos = await _unitOfWork
                    .ViewVeiculosManutencaoReserva.GetAllReducedIQueryable(
                        v => new { v.VeiculoId, v.Descricao },
                        asNoTracking: true
                    )
                    .OrderBy(v => v.Descricao)
                    .Select(v => new VeiculoReservaData(v.VeiculoId, v.Descricao ?? string.Empty))
                    .ToListAsync();

                _cache.Set(CacheKeys.VeiculosReserva, veiculos, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = FallbackTtl,
                    Priority = CacheItemPriority.High
                });

                return veiculos;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaCacheService.cs", "GetVeiculosReservaAsync", error);
                _log.LogError(error, "Erro ao obter veículos reserva do cache/DB");
                return new List<VeiculoReservaData>();
            }
        }

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
    }
}
```
