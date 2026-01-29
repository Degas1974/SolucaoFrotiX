// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IOrgaoAutuanteRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de OrgaoAutuante, gerenciando órgãos responsáveis   ║
// ║ por aplicação de multas (DETRAN, PRF, Guarda Municipal, etc.).               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetOrgaoAutuanteListForDropDown() → DropDown de órgãos autuantes           ║
// ║ • Update() → Atualização de órgão autuante                                   ║
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
    /// Interface do repositório de OrgaoAutuante. Estende IRepository&lt;OrgaoAutuante&gt;.
    /// </summary>
    public interface IOrgaoAutuanteRepository : IRepository<OrgaoAutuante>
        {

        IEnumerable<SelectListItem> GetOrgaoAutuanteListForDropDown();

        void Update(OrgaoAutuante orgaoautuante);

        }
    }


