// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IOcorrenciaViagemRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de OcorrenciaViagem, gerenciando ocorrências        ║
// ║ registradas durante viagens (acidentes, avarias, atrasos).                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ OBSERVAÇÃO: Não herda IRepository<T>, define CRUD próprio para tabela MxN.   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS                                                                      ║
// ║ • GetAll() → Listagem com filtro e includes                                 ║
// ║ • GetFirstOrDefault() → Busca única                                         ║
// ║ • Add(), Remove(), Update() → CRUD padrão                                   ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FrotiX.Models;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de OcorrenciaViagem. Não herda IRepository genérico.
    /// </summary>
    public interface IOcorrenciaViagemRepository
    {
        IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem, bool>>? filter = null, string? includeProperties = null);
        OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem, bool>> filter, string? includeProperties = null);
        void Add(OcorrenciaViagem entity);
        void Remove(OcorrenciaViagem entity);
        void Update(OcorrenciaViagem entity);
    }
}
