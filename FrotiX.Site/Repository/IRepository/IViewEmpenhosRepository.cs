// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewEmpenhosRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewEmpenhos, consultando SQL View consolidada   ║
// ║ de empenhos com saldos e movimentações.                                       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewEmpenhosListForDropDown() → DropDown de empenhos consolidados       ║
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
    /// Interface do repositório de ViewEmpenhos. Estende IRepository&lt;ViewEmpenhos&gt;.
    /// </summary>
    public interface IViewEmpenhosRepository : IRepository<ViewEmpenhos>
        {

        IEnumerable<SelectListItem> GetViewEmpenhosListForDropDown();

        void Update(ViewEmpenhos viewEmpenhos);

        }
    }


