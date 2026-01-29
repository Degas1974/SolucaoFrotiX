// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewMotoristasViagemRepository.cs                              ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewMotoristasViagem, consultando SQL View       ║
// ║ consolidada de motoristas disponíveis para viagem com escalas.               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewMotoristasViagemListForDropDown() → DropDown de motoristas          ║
// ║ • Update() → Não aplicável (view somente leitura)                            ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using FrotiX.Models.Views;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de ViewMotoristasViagem. Estende IRepository&lt;ViewMotoristasViagem&gt;.
    /// </summary>
    public interface IViewMotoristasViagemRepository : IRepository<ViewMotoristasViagem>
        {

        IEnumerable<SelectListItem> GetViewMotoristasViagemListForDropDown();

        void Update(ViewMotoristasViagem viewMotoristasViagem);

        }
    }


