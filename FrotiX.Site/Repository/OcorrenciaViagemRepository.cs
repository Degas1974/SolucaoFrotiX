// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : OcorrenciaViagemRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade OcorrenciaViagem. Gerencia           ║
// ║ ocorrências e problemas registrados durante viagens.                         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetAll() → Lista completa com filtro e includes opcionais                  ║
// ║ • GetFirstOrDefault() → Primeira ocorrência que atende filtro                ║
// ║ • Add() → Adiciona nova ocorrência                                           ║
// ║ • Remove() → Remove ocorrência existente                                    ║
// ║ • Update() → Atualiza entidade OcorrenciaViagem                             ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
{
    public class OcorrenciaViagemRepository : IOcorrenciaViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        public OcorrenciaViagemRepository(FrotiXDbContext db)
        {
            _db = db;
        }

        public IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem , bool>>? filter = null , string? includeProperties = null)
        {
            IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop.Trim());
                }
            }

            return query.ToList();
        }

        public OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem , bool>> filter , string? includeProperties = null)
        {
            IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop.Trim());
                }
            }

            return query.FirstOrDefault();
        }

        public void Add(OcorrenciaViagem entity)
        {
            _db.OcorrenciaViagem.Add(entity);
        }

        public void Remove(OcorrenciaViagem entity)
        {
            _db.OcorrenciaViagem.Remove(entity);
        }

        public new void Update(OcorrenciaViagem entity)
        {
            _db.OcorrenciaViagem.Update(entity);
        }
    }
}
