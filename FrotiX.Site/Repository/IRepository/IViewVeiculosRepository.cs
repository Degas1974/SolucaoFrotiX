// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewVeiculosRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewVeiculos, consultando SQL View consolidada   ║
// ║ de veículos com dados de contrato, marca, modelo e situação.                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewVeiculosListForDropDown() → DropDown de veículos consolidados        ║
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
    /// Interface do repositório de ViewVeiculos. Estende IRepository&lt;ViewVeiculos&gt;.
    /// </summary>
    public interface IViewVeiculosRepository : IRepository<ViewVeiculos>
        {

        IEnumerable<SelectListItem> GetViewVeiculosListForDropDown();

        void Update(ViewVeiculos viewVeiculos);

        }
    }


