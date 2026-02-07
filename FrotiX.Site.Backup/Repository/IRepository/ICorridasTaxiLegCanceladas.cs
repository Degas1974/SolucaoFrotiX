/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: ICorridasTaxiLegCanceladas.cs                                                                       ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de CorridasCanceladasTaxiLeg, gerenciando corridas canceladas            ║
║              importadas do sistema TaxiLeg.                                                                    ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetCorridasCanceladasTaxiLegListForDropDown() → DropDown de canceladas                                    ║
║     • Update() → Atualização de corrida cancelada                                                               ║
║  🔗 DEPENDÊNCIAS: IRepository<CorridasCanceladasTaxiLeg>, SelectListItem                                       ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {

    // Interface do repositório de CorridasCanceladasTaxiLeg. Estende IRepository&lt;CorridasCanceladasTaxiLeg&gt;.

    public interface ICorridasCanceladasTaxiLegRepository : IRepository<CorridasCanceladasTaxiLeg>
        {

        IEnumerable<SelectListItem> GetCorridasCanceladasTaxiLegListForDropDown();

        void Update(CorridasCanceladasTaxiLeg corridasCanceladasTaxiLeg);

        }
    }


