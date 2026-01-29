// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IAspNetUsersRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de AspNetUsers, gerenciando acesso a usuários do    ║
// ║ ASP.NET Identity para operações específicas do sistema.                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetAspNetUsersListForDropDown() → DropDown de usuários                     ║
// ║ • Update() → Atualização de usuário                                          ║
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
    /// Interface do repositório de AspNetUsers. Estende IRepository&lt;AspNetUsers&gt;.
    /// </summary>
    public interface IAspNetUsersRepository : IRepository<AspNetUsers>
        {

        IEnumerable<SelectListItem> GetAspNetUsersListForDropDown();

        void Update(AspNetUsers aspNetUsers);

        }
    }


