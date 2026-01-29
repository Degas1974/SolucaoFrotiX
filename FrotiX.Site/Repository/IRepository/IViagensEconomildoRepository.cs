// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViagensEconomildoRepository.cs                                 ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViagensEconomildo, gerenciando viagens do app    ║
// ║ mobile Economildo integradas ao sistema principal.                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViagensEconomildoListForDropDown() → DropDown de viagens Economildo     ║
// ║ • Update() → Atualização de viagem Economildo                                ║
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
    /// Interface do repositório de ViagensEconomildo. Estende IRepository&lt;ViagensEconomildo&gt;.
    /// </summary>
    public interface IViagensEconomildoRepository : IRepository<ViagensEconomildo>
        {

        IEnumerable<SelectListItem> GetViagensEconomildoListForDropDown();

        void Update(ViagensEconomildo viagensEconomildo);

        }
    }


