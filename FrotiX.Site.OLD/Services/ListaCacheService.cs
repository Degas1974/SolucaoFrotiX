/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ListaCacheService.cs                                                                    ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Serviço centralizado para obtenção de listas de Motoristas e Veículos.                 ║
   ║              Implementa padrão cache-aside com fallback para DB.                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS:                                                                                         ║
   ║    • GetMotoristasAsync() - Retorna lista de motoristas COM FOTO (cache ou DB)                      ║
   ║    • GetVeiculosAsync() - Retorna lista de veículos (ViewVeiculos.VeiculoCompleto)                  ║
   ║    • GetVeiculosManutencaoAsync() - Retorna lista de veículos para manutenção                       ║
   ║    • GetVeiculosReservaAsync() - Retorna lista de veículos reserva                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: IMemoryCache, IUnitOfWork, CacheKeys | 📅 04/02/2026 | 👤 Copilot | 📝 v1.0                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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
    /***********************************************************************************
     * ⚡ CLASSE: ListaCacheService
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Serviço centralizado para listas de Motoristas e Veículos com
     *                   padrão cache-aside (tenta cache, fallback DB, popula cache).
     *
     * 📥 ENTRADAS     : IMemoryCache, IUnitOfWork, ILogger
     *
     * 📤 SAÍDAS       : Listas tipadas de MotoristaDataComFoto, VeiculoData
     *
     * ⬅️ CHAMADO POR  : Pages/*, Controllers/*, APIs
     *
     * ➡️ CHAMA        : IUnitOfWork.ViewMotoristasViagem, IUnitOfWork.ViewVeiculos,
     *                   IUnitOfWork.ViewVeiculosManutencao, IUnitOfWork.ViewVeiculosManutencaoReserva
     *
     * 📝 OBSERVAÇÕES  : Cache populado pelo CacheWarmupService no startup.
     *                   Este serviço garante fallback se cache estiver vazio.
     ***********************************************************************************/
    public class ListaCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ListaCacheService> _log;

        // TTL padrão para fallback (quando cache não foi populado pelo warmup)
        private static readonly TimeSpan FallbackTtl = TimeSpan.FromMinutes(5);
        
        // Fallback foto padrão
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

        /***********************************************************************************
         * ⚡ MÉTODO: GetMotoristasAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter lista de motoristas COM FOTO do cache ou DB.
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : List<MotoristaDataComFoto> - Lista de motoristas com foto Base64
         *
         * ⬅️ CHAMADO POR  : Pages/Viagens/*, Pages/TaxiLeg/*, Pages/Manutencao/*
         *
         * ➡️ CHAMA        : IUnitOfWork.ViewMotoristasViagem.GetAllReducedIQueryable()
         *
         * 📝 OBSERVAÇÕES  : Converte foto byte[] para Data URI Base64.
         *                   Usa fallback "/images/barbudo.jpg" se foto ausente.
         ***********************************************************************************/
        public async Task<List<MotoristaDataComFoto>> GetMotoristasAsync()
        {
            try
            {
                // 1. Tenta obter do cache
                if (_cache.TryGetValue(CacheKeys.Motoristas, out List<MotoristaDataComFoto>? cached) && cached != null)
                {
                    return cached;
                }

                // 2. Fallback: carrega do banco
                _log.LogWarning("Cache miss para {CacheKey}. Carregando do banco...", CacheKeys.Motoristas);

                var motoristas = await _unitOfWork
                    .ViewMotoristasViagem.GetAllReducedIQueryable(
                        v => new { v.MotoristaId, v.MotoristaCondutor, v.Foto, v.Status },
                        asNoTracking: true
                    )
                    .Where(m => m.Status == true)
                    .OrderBy(m => m.MotoristaCondutor)
                    .ToListAsync();

                // 3. Converte para DTO com foto Base64
                var lista = motoristas.Select(m => new MotoristaDataComFoto(
                    m.MotoristaId,
                    m.MotoristaCondutor ?? string.Empty,
                    ConverteFotoBase64(m.Foto)
                )).ToList();

                // 4. Popula cache para próximas requisições
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

        /***********************************************************************************
         * ⚡ MÉTODO: GetVeiculosAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter lista de veículos (ViewVeiculos.VeiculoCompleto) do cache ou DB.
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : List<VeiculoData> - Lista de veículos com descrição completa
         *
         * ⬅️ CHAMADO POR  : Pages/Viagens/*, Pages/TaxiLeg/*
         *
         * ➡️ CHAMA        : IUnitOfWork.ViewVeiculos.GetAllReducedIQueryable()
         *
         * 📝 OBSERVAÇÕES  : Usa ViewVeiculos.VeiculoCompleto como descrição (Placa + Marca/Modelo).
         ***********************************************************************************/
        public async Task<List<VeiculoData>> GetVeiculosAsync()
        {
            try
            {
                // 1. Tenta obter do cache
                if (_cache.TryGetValue(CacheKeys.Veiculos, out List<VeiculoData>? cached) && cached != null)
                {
                    return cached;
                }

                // 2. Fallback: carrega do banco
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

                // 3. Popula cache para próximas requisições
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

        /***********************************************************************************
         * ⚡ MÉTODO: GetVeiculosManutencaoAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter lista de veículos para manutenção (ViewVeiculosManutencao).
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : List<VeiculoData> - Lista de veículos para manutenção
         *
         * ⬅️ CHAMADO POR  : Pages/Manutencao/Upsert
         *
         * ➡️ CHAMA        : IUnitOfWork.ViewVeiculosManutencao.GetAllReducedIQueryable()
         ***********************************************************************************/
        public async Task<List<VeiculoData>> GetVeiculosManutencaoAsync()
        {
            try
            {
                // 1. Tenta obter do cache
                if (_cache.TryGetValue(CacheKeys.VeiculosManutencao, out List<VeiculoData>? cached) && cached != null)
                {
                    return cached;
                }

                // 2. Fallback: carrega do banco
                _log.LogWarning("Cache miss para {CacheKey}. Carregando do banco...", CacheKeys.VeiculosManutencao);

                var veiculos = await _unitOfWork
                    .ViewVeiculosManutencao.GetAllReducedIQueryable(
                        v => new { v.VeiculoId, v.Descricao },
                        asNoTracking: true
                    )
                    .OrderBy(v => v.Descricao)
                    .Select(v => new VeiculoData(v.VeiculoId, v.Descricao ?? string.Empty))
                    .ToListAsync();

                // 3. Popula cache para próximas requisições
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

        /***********************************************************************************
         * ⚡ MÉTODO: GetVeiculosReservaAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter lista de veículos reserva (ViewVeiculosManutencaoReserva).
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : List<VeiculoReservaData> - Lista de veículos reserva
         *
         * ⬅️ CHAMADO POR  : Pages/Manutencao/Upsert
         *
         * ➡️ CHAMA        : IUnitOfWork.ViewVeiculosManutencaoReserva.GetAllReducedIQueryable()
         ***********************************************************************************/
        public async Task<List<VeiculoReservaData>> GetVeiculosReservaAsync()
        {
            try
            {
                // 1. Tenta obter do cache
                if (_cache.TryGetValue(CacheKeys.VeiculosReserva, out List<VeiculoReservaData>? cached) && cached != null)
                {
                    return cached;
                }

                // 2. Fallback: carrega do banco
                _log.LogWarning("Cache miss para {CacheKey}. Carregando do banco...", CacheKeys.VeiculosReserva);

                var veiculos = await _unitOfWork
                    .ViewVeiculosManutencaoReserva.GetAllReducedIQueryable(
                        v => new { v.VeiculoId, v.Descricao },
                        asNoTracking: true
                    )
                    .OrderBy(v => v.Descricao)
                    .Select(v => new VeiculoReservaData(v.VeiculoId, v.Descricao ?? string.Empty))
                    .ToListAsync();

                // 3. Popula cache para próximas requisições
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

        /***********************************************************************************
         * ⚡ MÉTODO: ConverteFotoBase64 (Helper privado)
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
    }
}
