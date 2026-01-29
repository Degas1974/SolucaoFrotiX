// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewAbastecimentosRepository.cs                                ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewAbastecimentos, consultando SQL View         ║
// ║ consolidada de abastecimentos para listagens e relatórios.                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewAbastecimentosListForDropDown() → DropDown de abastecimentos        ║
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
    /// Interface do repositório de ViewAbastecimentos. Estende IRepository&lt;ViewAbastecimentos&gt;.
    /// </summary>
    public interface IViewAbastecimentosRepository : IRepository<ViewAbastecimentos>
        {

        IEnumerable<SelectListItem> GetViewAbastecimentosListForDropDown();

        void Update(ViewAbastecimentos viewAbastecimentos);

        }
    }


