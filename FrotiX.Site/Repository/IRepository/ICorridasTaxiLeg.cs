// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ICorridasTaxiLeg.cs                                             ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de CorridasTaxiLeg, gerenciando corridas de táxi    ║
// ║ importadas do sistema TaxiLeg integrado.                                     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetCorridasTaxiLegListForDropDown() → DropDown de corridas                 ║
// ║ • Update() → Atualização de corrida                                          ║
// ║ • ExisteCorridaNoMesAno() → Verifica existência de corridas no período       ║
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
    /// Interface do repositório de CorridasTaxiLeg. Estende IRepository&lt;CorridasTaxiLeg&gt;.
    /// </summary>
    public interface ICorridasTaxiLegRepository : IRepository<CorridasTaxiLeg>
        {
        IEnumerable<SelectListItem> GetCorridasTaxiLegListForDropDown();

        void Update(CorridasTaxiLeg corridasTaxiLeg);

        bool ExisteCorridaNoMesAno(int ano, int mes);
        }
    }


