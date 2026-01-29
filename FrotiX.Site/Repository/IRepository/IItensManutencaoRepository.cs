// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IItensManutencaoRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ItensManutencao, gerenciando tipos de itens de   ║
// ║ manutenção veicular (troca óleo, pneus, freios, bateria, etc.).             ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetItensManutencaoListForDropDown() → DropDown de itens de manutenção      ║
// ║ • Update() → Atualização de item manutenção                                  ║
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
    /// Interface do repositório de ItensManutencao. Estende IRepository&lt;ItensManutencao&gt;.
    /// </summary>
    public interface IItensManutencaoRepository : IRepository<ItensManutencao>
        {

        IEnumerable<SelectListItem> GetItensManutencaoListForDropDown();

        void Update(ItensManutencao itensManutencao);

        }
    }


