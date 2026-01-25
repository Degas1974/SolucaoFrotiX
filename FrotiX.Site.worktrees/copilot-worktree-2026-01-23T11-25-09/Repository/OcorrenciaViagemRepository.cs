// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   ⚠️ OcorrenciaViagemRepository.cs | Repository/ | 2026-01-20                                     ║
// ║   Ocorrências de viagem. ✅ NÃO herda Repository<T> - Implementação direta                       ║
// ║   ✅ Update() não chama SaveChanges (Unit of Work correto)                                       ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    // [ETAPA] ✅ NÃO herda Repository<T> - implementa IOcorrenciaViagemRepository diretamente
    public class OcorrenciaViagemRepository : IOcorrenciaViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        public OcorrenciaViagemRepository(FrotiXDbContext db)
        {
            _db = db;
        }

        // [ETAPA] GetAll - Suporta filtros + Include de navegações
        // [ETAPA] GetAll - Suporta filtros + Include de navegações
        public IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop.Trim());
                }
            }

            return query.ToList();
        }

        // [ETAPA] GetFirstOrDefault - Filtro obrigatório + Include de navegações
        public OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem, bool>> filter, string? includeProperties = null)
        {
            IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop.Trim());
                }
            }

            return query.FirstOrDefault();
        }

        // [ETAPA] Add - Adiciona entidade ao contexto
        public void Add(OcorrenciaViagem entity)
        {
            _db.OcorrenciaViagem.Add(entity);
        }

        // [ETAPA] Remove - Remove entidade do contexto
        public void Remove(OcorrenciaViagem entity)
        {
            _db.OcorrenciaViagem.Remove(entity);
        }

        // [ETAPA] Update - ✅ NÃO chama SaveChanges (Unit of Work correto!)
        public new void Update(OcorrenciaViagem entity)
        {
            _db.OcorrenciaViagem.Update(entity);
        }
    }
}
