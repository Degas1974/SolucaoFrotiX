/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: ICorridasTaxiLeg.cs                                                                                 ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de CorridasTaxiLeg, gerenciando corridas de táxi importadas do sistema   ║
║              TaxiLeg integrado.                                                                                ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetCorridasTaxiLegListForDropDown() → DropDown de corridas                                                ║
║     • Update() → Atualização de corrida                                                                         ║
║     • ExisteCorridaNoMesAno() → Verifica existência de corridas no período                                      ║
║  🔗 DEPENDÊNCIAS: IRepository<CorridasTaxiLeg>, SelectListItem                                                 ║
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

    // Interface do repositório de CorridasTaxiLeg. Estende IRepository&lt;CorridasTaxiLeg&gt;.

    public interface ICorridasTaxiLegRepository : IRepository<CorridasTaxiLeg>
        {
        IEnumerable<SelectListItem> GetCorridasTaxiLegListForDropDown();

        void Update(CorridasTaxiLeg corridasTaxiLeg);

        bool ExisteCorridaNoMesAno(int ano, int mes);
        }
    }


