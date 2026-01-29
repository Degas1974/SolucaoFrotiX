// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : IRepository.cs                                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ FINALIDADE : Interface genérica do padrão Repository para EF Core.           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO FUNCIONAL                                                          ║
// ║ Interface IRepository<T> que define contrato genérico para CRUD:             ║
// ║ • Get(id): Busca por chave primária                                          ║
// ║ • GetFirstOrDefault/Async: Primeiro registro com filtro opcional             ║
// ║ • GetAll/Async: Lista com filtro, ordenação, includes, AsNoTracking, take    ║
// ║ • GetAllReduced: Projeção Select<TResult> materializada (ToList)             ║
// ║ • GetAllReducedIQueryable: Projeção como IQueryable (lazy)                   ║
// ║ • Add/AddAsync, Update, Remove: Operações de persistência                    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ IMPLEMENTAÇÃO: Repository<T> em Repository.cs                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE        : 24 — Repository/IRepository                                    ║
// ║ DATA        : 29/01/2026                                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

// IRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Contrato genérico de repositório para entidades EF Core.
    /// Mantém apenas operações genéricas (sem acoplamento a tipos do domínio).
    /// </summary>
    public interface IRepository<T>
        where T : class
        {
        /// <summary>Obtém uma entidade pela chave primária (chave simples).</summary>
        T Get(object id);

        /// <summary>Obtém a primeira entidade que satisfaz o filtro.</summary>
        T GetFirstOrDefault(
            Expression<Func<T , bool>> filter = null ,
            string includeProperties = null
        );

        /// <summary>Obtém a primeira entidade que satisfaz o filtro (assíncrono).</summary>
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T , bool>> filter = null ,
            string includeProperties = null
        );

        /// <summary>Retorna um conjunto materializado de entidades.</summary>
        IEnumerable<T> GetAll(
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true
        );

        /// <summary>Retorna um conjunto materializado de entidades (assíncrono).</summary>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true ,
            int? take = null
        );

        /// <summary>
        /// ✅ Versão materializada (compat com páginas antigas).
        /// Projeta e já materializa a lista (ToList()).
        /// </summary>
        IEnumerable<TResult> GetAllReduced<TResult>(
            Expression<Func<T , TResult>> selector ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true
        );

        /// <summary>
        /// Retorna um IQueryable projetado (DTO/lista leve), sem materializar.
        /// O EF Core traduz a expressão para SQL parametrizado.
        /// </summary>
        IQueryable<TResult> GetAllReducedIQueryable<TResult>(
            Expression<Func<T , TResult>> selector ,
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            string includeProperties = null ,
            bool asNoTracking = true
        );

        /// <summary>Adiciona a entidade ao contexto.</summary>
        void Add(T entity);

        /// <summary>Adiciona a entidade ao contexto (assíncrono).</summary>
        Task AddAsync(T entity);

        /// <summary>Atualiza a entidade no contexto.</summary>
        void Update(T entity);

        /// <summary>Remove a entidade pela chave (chave simples).</summary>
        void Remove(object id);

        /// <summary>Remove a entidade informada.</summary>
        void Remove(T entity);
        }
    }
