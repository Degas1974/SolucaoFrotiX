// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewOcorrenciasViagemRepository.cs                             ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewOcorrenciasViagem, consultando SQL View      ║
// ║ consolidada de ocorrências relacionadas a viagens.                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ OBSERVAÇÃO: Não herda IRepository genérico, define métodos próprios.          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS                                                                      ║
// ║ • GetAll() → Listagem com filtro e includes                                 ║
// ║ • GetFirstOrDefault() → Busca única por filtro                               ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FrotiX.Models;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de ViewOcorrenciasViagem. Não herda IRepository genérico.
    /// </summary>
    public interface IViewOcorrenciasViagemRepository
    {
        IEnumerable<ViewOcorrenciasViagem> GetAll(Expression<Func<ViewOcorrenciasViagem, bool>>? filter = null, string? includeProperties = null);
        ViewOcorrenciasViagem? GetFirstOrDefault(Expression<Func<ViewOcorrenciasViagem, bool>> filter, string? includeProperties = null);
    }
}
