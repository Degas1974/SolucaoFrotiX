/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IControleAcessoRepository.cs                                                                        ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de ControleAcesso, gerenciando registros de acesso de veículos em         ║
║              portarias e áreas controladas.                                                                       ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetControleAcessoListForDropDown() → DropDown de registros de acesso                                      ║
║     • Update() → Atualização de controle de acesso                                                              ║
║  🔗 DEPENDÊNCIAS: IRepository<ControleAcesso>, SelectListItem                                                  ║
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
    
    // Interface do repositório de ControleAcesso. Estende IRepository&lt;ControleAcesso&gt;.
    
    public interface IControleAcessoRepository : IRepository<ControleAcesso>
        {

        IEnumerable<SelectListItem> GetControleAcessoListForDropDown();

        void Update(ControleAcesso controleacesso);

        }
    }


