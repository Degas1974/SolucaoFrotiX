// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewOcorrenciasAbertasVeiculoRepository.cs                     ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewOcorrenciasAbertasVeiculo, consultando View  ║
// ║ de ocorrências abertas (não resolvidas) por veículo.                         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ OBSERVAÇÃO: Não herda IRepository genérico, define métodos próprios.          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS                                                                      ║
// ║ • GetAll() → Listagem de ocorrências abertas                                 ║
// ║ • GetFirstOrDefault() → Busca única por filtro                               ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FrotiX.Models;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de ViewOcorrenciasAbertasVeiculo. Não herda IRepository genérico.
    /// </summary>
    public interface IViewOcorrenciasAbertasVeiculoRepository
    {
        IEnumerable<ViewOcorrenciasAbertasVeiculo> GetAll(Expression<Func<ViewOcorrenciasAbertasVeiculo, bool>>? filter = null, string? includeProperties = null);
        ViewOcorrenciasAbertasVeiculo? GetFirstOrDefault(Expression<Func<ViewOcorrenciasAbertasVeiculo, bool>> filter, string? includeProperties = null);
    }
}
