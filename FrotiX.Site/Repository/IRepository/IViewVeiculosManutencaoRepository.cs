// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewVeiculosManutencaoRepository.cs                            ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewVeiculosManutencao, consultando SQL View     ║
// ║ de veículos com histórico de manutenção consolidado.                         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewVeiculosManutencaoListForDropDown() → DropDown de veículos          ║
// ║ • Update() → Não aplicável (view somente leitura)                            ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de ViewVeiculosManutencao. Estende IRepository&lt;ViewVeiculosManutencao&gt;.
    /// </summary>
    public interface IViewVeiculosManutencaoRepository : IRepository<ViewVeiculosManutencao>
        {
        IEnumerable<SelectListItem> GetViewVeiculosManutencaoListForDropDown();

        void Update(ViewVeiculosManutencao viewVeiculosManutencao);
        }
    }

