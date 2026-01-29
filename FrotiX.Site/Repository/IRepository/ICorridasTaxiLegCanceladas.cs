// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ICorridasTaxiLegCanceladas.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de CorridasCanceladasTaxiLeg, gerenciando corridas  ║
// ║ canceladas importadas do sistema TaxiLeg.                                    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetCorridasCanceladasTaxiLegListForDropDown() → DropDown de canceladas     ║
// ║ • Update() → Atualização de corrida cancelada                                ║
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
    /// Interface do repositório de CorridasCanceladasTaxiLeg. Estende IRepository&lt;CorridasCanceladasTaxiLeg&gt;.
    /// </summary>
    public interface ICorridasCanceladasTaxiLegRepository : IRepository<CorridasCanceladasTaxiLeg>
        {

        IEnumerable<SelectListItem> GetCorridasCanceladasTaxiLegListForDropDown();

        void Update(CorridasCanceladasTaxiLeg corridasCanceladasTaxiLeg);

        }
    }


