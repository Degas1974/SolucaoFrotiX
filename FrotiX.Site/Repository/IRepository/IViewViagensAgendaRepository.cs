// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewViagensAgendaRepository.cs                                 ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewViagensAgenda, consultando SQL View de       ║
// ║ viagens agendadas para exibição em calendário/agenda.                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewViagensAgendaListForDropDown() → DropDown de viagens agendadas      ║
// ║ • Update() → Não aplicável (view somente leitura)                            ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de ViewViagensAgenda. Estende IRepository&lt;ViewViagensAgenda&gt;.
    /// </summary>
    public interface IViewViagensAgendaRepository : IRepository<ViewViagensAgenda>
        {

        IEnumerable<SelectListItem> GetViewViagensAgendaListForDropDown();

        void Update(ViewViagensAgenda viewViagensAgenda);

        }
    }


