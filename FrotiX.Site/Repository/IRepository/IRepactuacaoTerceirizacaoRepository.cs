// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRepactuacaoTerceirizacaoRepository.cs                          ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de RepactuacaoTerceirizacao, gerenciando            ║
// ║ repactuações de valores de veículos terceirizados (reajustes).               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRepactuacaoTerceirizacaoListForDropDown() → DropDown de repactuações    ║
// ║ • Update() → Atualização de repactuação de terceirização                     ║
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
    /// Interface do repositório de RepactuacaoTerceirizacao. Estende IRepository&lt;RepactuacaoTerceirizacao&gt;.
    /// </summary>
    public interface IRepactuacaoTerceirizacaoRepository : IRepository<RepactuacaoTerceirizacao>
        {

        IEnumerable<SelectListItem> GetRepactuacaoTerceirizacaoListForDropDown();

        void Update(RepactuacaoTerceirizacao RepactuacaoTerceirizacao);

        }
    }


