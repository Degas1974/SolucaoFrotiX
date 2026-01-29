// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ISetorSolicitanteRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de SetorSolicitante, gerenciando setores que        ║
// ║ solicitam viagens e serviços de transporte.                                  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetSetorSolicitanteListForDropDown() → DropDown de setores solicitantes    ║
// ║ • Update() → Atualização de setor solicitante                                ║
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
    /// Interface do repositório de SetorSolicitante. Estende IRepository&lt;SetorSolicitante&gt;.
    /// </summary>
    public interface ISetorSolicitanteRepository : IRepository<SetorSolicitante>
        {

        IEnumerable<SelectListItem> GetSetorSolicitanteListForDropDown();

        void Update(SetorSolicitante setorSolicitante);

        }
    }


