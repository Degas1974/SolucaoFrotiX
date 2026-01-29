// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewViagensAgendaTodosMesesRepository.cs                       ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewViagensAgendaTodosMeses, consultando View    ║
// ║ de viagens recorrentes para todos os meses do ano.                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewViagensAgendaTodosMesesListForDropDown() → DropDown de viagens      ║
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
    /// Interface do repositório de ViewViagensAgendaTodosMeses. Estende IRepository&lt;ViewViagensAgendaTodosMeses&gt;.
    /// </summary>
    public interface IViewViagensAgendaTodosMesesRepository : IRepository<ViewViagensAgendaTodosMeses>
        {

        IEnumerable<SelectListItem> GetViewViagensAgendaTodosMesesListForDropDown();

        void Update(ViewViagensAgendaTodosMeses viewViagensAgendaMeses);

        }
    }


