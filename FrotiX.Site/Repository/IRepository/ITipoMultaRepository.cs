// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ITipoMultaRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de TipoMulta, gerenciando tipos de infrações de     ║
// ║ trânsito (excesso velocidade, estacionamento irregular, etc.).              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetTipoMultaListForDropDown() → DropDown de tipos de multa                 ║
// ║ • Update() → Atualização de tipo multa                                       ║
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
    /// Interface do repositório de TipoMulta. Estende IRepository&lt;TipoMulta&gt;.
    /// </summary>
    public interface ITipoMultaRepository : IRepository<TipoMulta>
        {

        IEnumerable<SelectListItem> GetTipoMultaListForDropDown();

        void Update(TipoMulta tipomulta);

        }
    }


