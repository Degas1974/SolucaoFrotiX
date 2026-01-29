// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewLotacoesRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewLotacoes, consultando SQL View consolidada   ║
// ║ de lotações de motoristas com dados de setor e período.                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ OBSERVAÇÃO: Interface vazia, herda apenas métodos do IRepository genérico.   ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de ViewLotacoes. Estende IRepository&lt;ViewLotacoes&gt;.
    /// </summary>
    public interface IViewLotacoesRepository : IRepository<ViewLotacoes>
        {

        }
    }


