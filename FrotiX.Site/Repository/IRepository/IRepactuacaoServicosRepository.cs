// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRepactuacaoServicosRepository.cs                               ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de RepactuacaoServicos, gerenciando repactuações de ║
// ║ valores de serviços contratados (manutenção, lavagem, abastecimento).        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRepactuacaoServicosListForDropDown() → DropDown de repactuações         ║
// ║ • Update() → Atualização de repactuação de serviços                          ║
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
    /// Interface do repositório de RepactuacaoServicos. Estende IRepository&lt;RepactuacaoServicos&gt;.
    /// </summary>
    public interface IRepactuacaoServicosRepository : IRepository<RepactuacaoServicos>
        {

        IEnumerable<SelectListItem> GetRepactuacaoServicosListForDropDown();

        void Update(RepactuacaoServicos RepactuacaoServicos);

        }
    }


