// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ILavadoresLavagemRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de LavadoresLavagem, gerenciando associação MxN     ║
// ║ entre lavadores e registros de lavagem de veículos.                          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetLavadoresLavagemListForDropDown() → DropDown de lavadores/lavagens      ║
// ║ • Update() → Atualização de lavador-lavagem                                  ║
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
    /// Interface do repositório de LavadoresLavagem. Estende IRepository&lt;LavadoresLavagem&gt;.
    /// </summary>
    public interface ILavadoresLavagemRepository : IRepository<LavadoresLavagem>
        {

        IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown();

        void Update(LavadoresLavagem lavadoresLavagem);

        }
    }


