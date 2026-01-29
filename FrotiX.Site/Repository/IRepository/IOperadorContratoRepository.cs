// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IOperadorContratoRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de OperadorContrato, gerenciando associação MxN     ║
// ║ entre operadores e contratos de frota.                                       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetOperadorContratoListForDropDown() → DropDown de operadores/contratos    ║
// ║ • Update() → Atualização de operador-contrato                                ║
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
    /// Interface do repositório de OperadorContrato. Estende IRepository&lt;OperadorContrato&gt;.
    /// </summary>
    public interface IOperadorContratoRepository : IRepository<OperadorContrato>
        {

        IEnumerable<SelectListItem> GetOperadorContratoListForDropDown();

        void Update(OperadorContrato OperadorContrato);

        }
    }


