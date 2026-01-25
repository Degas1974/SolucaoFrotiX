// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   🚗 ViagemRepository.cs | Repository/ | 2026-01-20                                               ║
// ║   CORE DE VIAGENS. Métodos avançados: normalização Origem/Destino, recorrência, paginação        ║
// ║   ✅ Update SEM código morto. ⚠️ Quebra Unit of Work. Performance logging (Stopwatch)            ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FrotiX.Repository
{
    public class ViagemRepository : Repository<Viagem>, IViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        public ViagemRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Dropdown - OrderBy: DataInicial, Text: Descricao
        public IEnumerable<SelectListItem> GetViagemListForDropDown()
        {
            return _db.Viagem
                .OrderBy(o => o.DataInicial)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao,
                    Value = i.ViagemId.ToString()
                });
        }

        // [ETAPA] Update - ✅ SEM código morto objFromDb (como VeiculoRepository)
        // ⚠️ Quebra Unit of Work (SaveChanges direto)
        public new void Update(Viagem viagem)
        {
            _db.Update(viagem);
            _db.SaveChanges();
        }

        // [ETAPA] GetDistinctOrigensAsync - Retorna lista única de origens (normalização)
        public async Task<List<string>> GetDistinctOrigensAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Origem))
                .Select(v => v.Origem)
                .Distinct()
                .OrderBy(o => o)
                .ToListAsync();
        }

        // [ETAPA] GetDistinctDestinosAsync - Retorna lista única de destinos (normalização)
        public async Task<List<string>> GetDistinctDestinosAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Destino))
                .Select(v => v.Destino)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        // [ETAPA] CorrigirOrigemAsync - Normalização: substitui múltiplas origens antigas por nova
        // ⚠️ Quebra Unit of Work (SaveChangesAsync direto)
        public async Task CorrigirOrigemAsync(List<string> origensAntigas, string novaOrigem)
        {
            var viagens = await _db.Viagem
                .AsTracking()
                .Where(v => origensAntigas.Contains(v.Origem))
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.Origem = novaOrigem;
            }

            await _db.SaveChangesAsync();
        }

        // [ETAPA] CorrigirDestinoAsync - Normalização: substitui múltiplos destinos antigos por novo
        // ⚠️ Quebra Unit of Work (SaveChangesAsync direto)
        public async Task CorrigirDestinoAsync(List<string> destinosAntigos, string novoDestino)
        {
            var viagens = await _db.Viagem
                .AsTracking()
                .Where(v => destinosAntigos.Contains(v.Destino))
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.Destino = novoDestino;
            }

            await _db.SaveChangesAsync();
        }

        // [ETAPA] BuscarViagensRecorrenciaAsync - Busca todas viagens do mesmo evento (recorrência)
        public async Task<List<Viagem>> BuscarViagensRecorrenciaAsync(Guid id)
        {
            var viagemOriginal = await _db.Viagem.FindAsync(id);
            if (viagemOriginal == null)
                return new List<Viagem>();

            if (viagemOriginal.EventoId.HasValue)
            {
                return await _db.Viagem
                    .Where(v => v.EventoId == viagemOriginal.EventoId.Value)
                    .OrderBy(v => v.DataInicial)
                    .ToListAsync();
            }

            return new List<Viagem> { viagemOriginal };
        }

        // [ETAPA] GetViagensEventoPaginadoAsync - ⚡ PAGINAÇÃO OTIMIZADA DE VIAGENS DE EVENTO
        // Usa ViewViagens (JOIN pré-calculado) + paginação em 2 etapas (IDs → dados completos)
        // ✅ Performance logging com Stopwatch, AsNoTracking, try-catch
        /// <summary>
        /// ⚡ Query LINQ otimizada - usa ViewViagens em vez de JOINs complexos
        /// </summary>
        public async Task<(List<ViagemEventoDto> viagens, int totalItems)> GetViagensEventoPaginadoAsync(
            Guid eventoId,
            int page,
            int pageSize
        )
        {
            try
            {
                var swTotal = System.Diagnostics.Stopwatch.StartNew();
                var swCount = System.Diagnostics.Stopwatch.StartNew();

                // COUNT otimizado na tabela Viagem
                var totalItems = await _db.Viagem
                    .Where(v => v.EventoId == eventoId && v.Status == "Realizada")
                    .CountAsync();

                swCount.Stop();
                Console.WriteLine($"[SQL COUNT] {totalItems} registros - {swCount.ElapsedMilliseconds}ms");

                if (totalItems == 0)
                {
                    return (new List<ViagemEventoDto>(), 0);
                }

                var swQuery = System.Diagnostics.Stopwatch.StartNew();

                // Buscar IDs das viagens paginadas
                var viagemIds = await _db.Viagem
                    .Where(v => v.EventoId == eventoId && v.Status == "Realizada")
                    .OrderByDescending(v => v.DataInicial)
                    .ThenByDescending(v => v.HoraInicio)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(v => v.ViagemId)
                    .ToListAsync();

                // Buscar dados completos da ViewViagens apenas para os IDs paginados
                var viagens = await _db.ViewViagens
                    .Where(vv => viagemIds.Contains(vv.ViagemId))
                    .Select(vv => new ViagemEventoDto
                    {
                        ViagemId = vv.ViagemId, // ✅ ADICIONADO!
                        EventoId = vv.EventoId ?? Guid.Empty,
                        NoFichaVistoria = vv.NoFichaVistoria ?? 0,
                        NomeRequisitante = vv.NomeRequisitante ?? "",
                        NomeSetor = vv.NomeSetor ?? "",
                        NomeMotorista = vv.NomeMotorista ?? "",
                        DescricaoVeiculo = vv.DescricaoVeiculo ?? "",
                        CustoViagem = (decimal)(vv.CustoViagem ?? 0),
                        DataInicial = vv.DataInicial ?? DateTime.MinValue,
                        HoraInicio = vv.HoraInicio,
                        Placa = vv.Placa ?? ""
                    })
                    .AsNoTracking()
                    .ToListAsync();

                // Reordenar no lado do cliente (já são poucos registros)
                viagens = viagens
                    .OrderByDescending(v => v.DataInicial)
                    .ThenByDescending(v => v.HoraInicio)
                    .ToList();

                swQuery.Stop();
                Console.WriteLine($"[SQL QUERY] {viagens.Count} registros - {swQuery.ElapsedMilliseconds}ms");

                swTotal.Stop();
                Console.WriteLine($"[TOTAL] {swTotal.ElapsedMilliseconds}ms\n");

                return (viagens, totalItems);
            }
            catch (Exception error)
            {
                Console.WriteLine($"[ERRO SQL] {error.Message}");
                Alerta.TratamentoErroComLinha("ViagemRepository.cs", "GetViagensEventoPaginadoAsync", error);
                throw;
            }
        }

        // [ETAPA] GetQuery - Retorna IQueryable (composição de queries sem materialização)
        // Útil para operações agregadas (Count, Min, Max, Sum) sem carregar registros em memória
        // ✅ CORREÇÃO: Usar Viagem em vez de T genérico
        /// <summary>
        /// Retorna IQueryable para permitir composição de queries sem materialização.
        /// Use para operações que só precisam de Count(), Min(), Max(), etc.
        /// </summary>
        public IQueryable<Viagem> GetQuery(Expression<Func<Viagem, bool>> filter = null)
        {
            IQueryable<Viagem> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }
    }
}
