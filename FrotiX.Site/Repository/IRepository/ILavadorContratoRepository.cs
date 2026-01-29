// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ILavadorContratoRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de LavadorContrato, gerenciando associação MxN      ║
// ║ entre lavadores e contratos de lavagem de frota.                             ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetLavadorContratoListForDropDown() → DropDown de lavadores/contratos      ║
// ║ • Update() → Atualização de lavador-contrato                                 ║
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
    /// Interface do repositório de LavadorContrato. Estende IRepository&lt;LavadorContrato&gt;.
    /// </summary>
    public interface ILavadorContratoRepository : IRepository<LavadorContrato>
        {

        IEnumerable<SelectListItem> GetLavadorContratoListForDropDown();

        void Update(LavadorContrato LavadorContrato);

        }
    }


