// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IPlacaBronzeRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de PlacaBronze, gerenciando dados de placas de      ║
// ║ veículos no sistema bronze (importação/sincronização).                       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetPlacaBronzeListForDropDown() → DropDown de placas bronze                ║
// ║ • Update() → Atualização de placa bronze                                     ║
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
    /// Interface do repositório de PlacaBronze. Estende IRepository&lt;PlacaBronze&gt;.
    /// </summary>
    public interface IPlacaBronzeRepository : IRepository<PlacaBronze>
        {

        IEnumerable<SelectListItem> GetPlacaBronzeListForDropDown();

        void Update(PlacaBronze placaBronze);

        }
    }


