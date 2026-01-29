// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IOperadorRepository.cs                                          ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Operador, gerenciando operadores de SIGA/SISME   ║
// ║ que registram operações no sistema de frota.                                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetOperadorListForDropDown() → DropDown de operadores                      ║
// ║ • Update() → Atualização de operador                                         ║
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
    /// Interface do repositório de Operador. Estende IRepository&lt;Operador&gt;.
    /// </summary>
    public interface IOperadorRepository : IRepository<Operador>
        {

        IEnumerable<SelectListItem> GetOperadorListForDropDown();

        void Update(Operador operador);

        }
    }


