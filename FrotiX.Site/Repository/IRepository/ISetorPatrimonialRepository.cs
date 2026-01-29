// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ISetorPatrimonialRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de SetorPatrimonial, gerenciando setores do         ║
// ║ sistema de patrimônio para controle de bens.                                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetSetorListForDropDown() → DropDown de setores patrimoniais               ║
// ║ • Update() → Atualização de setor patrimonial                                ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Cadastros;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de SetorPatrimonial. Estende IRepository&lt;SetorPatrimonial&gt;.
    /// </summary>
    public interface ISetorPatrimonialRepository : IRepository<SetorPatrimonial>
        {

        IEnumerable<SelectListItem> GetSetorListForDropDown();

        void Update(SetorPatrimonial setor);

        }
    }


