// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewSetoresRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewSetores, consultando SQL View consolidada    ║
// ║ de setores solicitantes com dados estruturais.                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewSetoresListForDropDown() → DropDown de setores                      ║
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
    /// Interface do repositório de ViewSetores. Estende IRepository&lt;ViewSetores&gt;.
    /// </summary>
    public interface IViewSetoresRepository : IRepository<ViewSetores>
        {

        IEnumerable<SelectListItem> GetViewSetoresListForDropDown();

        void Update(ViewSetores viewSetores);

        }
    }


