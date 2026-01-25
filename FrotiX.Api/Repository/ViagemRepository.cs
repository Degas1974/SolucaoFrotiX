using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FrotiXApi.Repository
{
    public class ViagemRepository : Repository<Viagem>, IViagemRepository
    {
        private readonly FrotiXDbContext _db;

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

        public void Update(Viagem viagem)
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
