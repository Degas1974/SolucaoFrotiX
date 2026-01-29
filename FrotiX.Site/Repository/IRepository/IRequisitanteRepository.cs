// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRequisitanteRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Requisitante, gerenciando pessoas/setores que    ║
// ║ solicitam viagens e serviços de transporte.                                  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRequisitanteListForDropDown() → DropDown de requisitantes               ║
// ║ • Update() → Atualização de requisitante                                     ║
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
    /// Interface do repositório de Requisitante. Estende IRepository&lt;Requisitante&gt;.
    /// </summary>
    public interface IRequisitanteRepository : IRepository<Requisitante>
        {

        IEnumerable<SelectListItem> GetRequisitanteListForDropDown();

        void Update(Requisitante requisitante);

        }
    }


