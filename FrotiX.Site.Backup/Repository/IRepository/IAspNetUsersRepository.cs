/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IAspNetUsersRepository.cs                                                                           ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de AspNetUsers, gerenciando acesso a usuários do ASP.NET Identity para   ║
║              operações específicas do sistema.                                                                   ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetAspNetUsersListForDropDown() → DropDown de usuários                                                    ║
║     • Update() → Atualização de usuário                                                                         ║
║  🔗 DEPENDÊNCIAS: IRepository<AspNetUsers>, SelectListItem                                                      ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    
    // Interface do repositório de AspNetUsers. Estende IRepository&lt;AspNetUsers&gt;.
    
    public interface IAspNetUsersRepository : IRepository<AspNetUsers>
        {

        IEnumerable<SelectListItem> GetAspNetUsersListForDropDown();

        void Update(AspNetUsers aspNetUsers);

        }
    }


