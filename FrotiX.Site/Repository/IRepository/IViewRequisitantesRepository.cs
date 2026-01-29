// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewRequisitantesRepository.cs                                 ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewRequisitantes, consultando SQL View          ║
// ║ consolidada de requisitantes com dados de setor e lotação.                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewRequisitantesListForDropDown() → DropDown de requisitantes          ║
// ║ • Update() → Não aplicável (view somente leitura)                            ║
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
    /// Interface do repositório de ViewRequisitantes. Estende IRepository&lt;ViewRequisitantes&gt;.
    /// </summary>
    public interface IViewRequisitantesRepository : IRepository<ViewRequisitantes>
        {

        IEnumerable<SelectListItem> GetViewRequisitantesListForDropDown();

        void Update(ViewRequisitantes viewRequisitantes);

        }
    }


