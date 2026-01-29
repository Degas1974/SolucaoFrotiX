// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IMotoristaContratoRepository.cs                                 ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de MotoristaContrato, gerenciando associação MxN    ║
// ║ entre motoristas terceirizados e contratos de transporte.                    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetMotoristaContratoListForDropDown() → DropDown de motoristas/contratos   ║
// ║ • Update() → Atualização de motorista-contrato                               ║
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
    /// Interface do repositório de MotoristaContrato. Estende IRepository&lt;MotoristaContrato&gt;.
    /// </summary>
    public interface IMotoristaContratoRepository : IRepository<MotoristaContrato>
        {

        IEnumerable<SelectListItem> GetMotoristaContratoListForDropDown();

        void Update(MotoristaContrato MotoristaContrato);

        }
    }


