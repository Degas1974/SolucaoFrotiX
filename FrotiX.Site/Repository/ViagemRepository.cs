// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViagemRepository.cs                                             ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Viagem. Herda Repository<Viagem>     ║
// ║ e implementa IViagemRepository. Gerencia operações CRUD e queries complexas  ║
// ║ relacionadas a viagens no sistema de gestão de frota.                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetViagemListForDropDown() → SelectList para dropdowns                     ║
// ║ • GetDistinctOrigensAsync() → Lista de origens únicas                        ║
// ║ • GetDistinctDestinosAsync() → Lista de destinos únicos                      ║
// ║ • CorrigirOrigemAsync() → Corrige origens em lote                            ║
// ║ • CorrigirDestinoAsync() → Corrige destinos em lote                          ║
// ║ • BuscarViagensRecorrenciaAsync() → Busca viagens de mesmo EventoId          ║
// ║ • GetViagensEventoPaginadoAsync() → Query otimizada usando ViewViagens       ║
// ║ • GetQuery() → IQueryable para composição de queries                         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ OTIMIZAÇÕES                                                                  ║
// ║ • Usa ViewViagens em vez de JOINs complexos para paginação                   ║
// ║ • Paginação server-side com COUNT separado                                   ║
// ║ • AsNoTracking() para queries read-only                                      ║
// ║ • Logs de performance com Stopwatch para diagnóstico                         ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<SelectListItem> GetViagemListForDropDown()
        {
            return _db.Viagem
                .OrderBy(o => o.DataInicial)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao ,
                    Value = i.ViagemId.ToString()
                });
        }

        public new void Update(Viagem viagem)
        {
            _db.Update(viagem);
            _db.SaveChanges();
        }

        public async Task<List<string>> GetDistinctOrigensAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Origem))
                .Select(v => v.Origem)
                .Distinct()
                .OrderBy(o => o)
                .ToListAsync();
        }

        public async Task<List<string>> GetDistinctDestinosAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Destino))
                .Select(v => v.Destino)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        public async Task CorrigirOrigemAsync(List<string> origensAntigas , string novaOrigem)
        {
            var viagens = await _db.Viagem
                .Where(v => origensAntigas.Contains(v.Origem))
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.Origem = novaOrigem;
            }

            await _db.SaveChangesAsync();
        }

        public async Task CorrigirDestinoAsync(List<string> destinosAntigos , string novoDestino)
        {
            var viagens = await _db.Viagem
                .Where(v => destinosAntigos.Contains(v.Destino))
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.Destino = novoDestino;
            }

            await _db.SaveChangesAsync();
        }

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

        /// <summary>
        /// ⚡ Query LINQ otimizada - usa ViewViagens em vez de JOINs complexos
        /// </summary>
        public async Task<(List<ViagemEventoDto> viagens, int totalItems)> GetViagensEventoPaginadoAsync(
            Guid eventoId ,
            int page ,
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
                        ViagemId = vv.ViagemId , // ✅ ADICIONADO!
                        EventoId = vv.EventoId ?? Guid.Empty ,
                        NoFichaVistoria = vv.NoFichaVistoria ?? 0 ,
                        NomeRequisitante = vv.NomeRequisitante ?? "" ,
                        NomeSetor = vv.NomeSetor ?? "" ,
                        NomeMotorista = vv.NomeMotorista ?? "" ,
                        DescricaoVeiculo = vv.DescricaoVeiculo ?? "" ,
                        CustoViagem = (decimal)(vv.CustoViagem ?? 0) ,
                        DataInicial = vv.DataInicial ?? DateTime.MinValue ,
                        HoraInicio = vv.HoraInicio ,
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
                Alerta.TratamentoErroComLinha("ViagemRepository.cs" , "GetViagensEventoPaginadoAsync" , error);
                throw;
            }
        }

        // ✅ CORREÇÃO: Usar Viagem em vez de T genérico
        /// <summary>
        /// Retorna IQueryable para permitir composição de queries sem materialização.
        /// Use para operações que só precisam de Count(), Min(), Max(), etc.
        /// </summary>
        public IQueryable<Viagem> GetQuery(Expression<Func<Viagem , bool>> filter = null)
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
