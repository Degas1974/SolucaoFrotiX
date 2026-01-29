// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRecursoRepository.cs                                           ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Recurso, gerenciando fontes de recursos          ║
// ║ orçamentários utilizados nos contratos e empenhos.                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRecursoListForDropDown() → DropDown de recursos                         ║
// ║ • Update() → Atualização de recurso                                          ║
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
    /// Interface do repositório de Recurso. Estende IRepository&lt;Recurso&gt;.
    /// </summary>
    public interface IRecursoRepository : IRepository<Recurso>
        {

        IEnumerable<SelectListItem> GetRecursoListForDropDown();

        void Update(Recurso recurso);

        }
    }


