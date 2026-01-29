// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewVeiculosManutencaoReservaRepository.cs                     ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewVeiculosManutencaoReserva, consultando View  ║
// ║ de veículos reserva disponíveis para substituição em manutenção.             ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewVeiculosManutencaoReservaListForDropDown() → DropDown de reservas   ║
// ║ • Update() → Não aplicável (view somente leitura)                            ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de ViewVeiculosManutencaoReserva. Estende IRepository&lt;ViewVeiculosManutencaoReserva&gt;.
    /// </summary>
    public interface IViewVeiculosManutencaoReservaRepository : IRepository<ViewVeiculosManutencaoReserva>
        {
        IEnumerable<SelectListItem> GetViewVeiculosManutencaoReservaListForDropDown();

        void Update(ViewVeiculosManutencaoReserva viewVeiculosManutencaoReserva);
        }
    }

