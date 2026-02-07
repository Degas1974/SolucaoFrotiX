/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IEmpenhoRepository.cs                                                                               ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de Empenho, gerenciando notas de empenho para controle orçamentário de    ║
║              despesas da frota (DESPFIN, DESPMA).                                                              ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetEmpenhoListForDropDown() → DropDown de empenhos disponíveis                                            ║
║     • Update() → Atualização de empenho                                                                         ║
║  🔗 DEPENDÊNCIAS: IRepository<Empenho>, SelectListItem                                                        ║
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
    
    // Interface do repositório de Empenho. Estende IRepository&lt;Empenho&gt;.
    
    public interface IEmpenhoRepository : IRepository<Empenho>
        {

        IEnumerable<SelectListItem> GetEmpenhoListForDropDown();

        void Update(Empenho empenho);

        }
    }


