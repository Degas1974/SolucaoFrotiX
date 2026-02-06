/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IEmpenhoMultaRepository.cs                                                                          ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de EmpenhoMulta, gerenciando empenhos específicos para pagamento de      ║
║              multas de trânsito.                                                                                ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetEmpenhoMultaListForDropDown() → DropDown de empenhos de multa                                          ║
║     • Update() → Atualização de empenho-multa                                                                   ║
║  🔗 DEPENDÊNCIAS: IRepository<EmpenhoMulta>, SelectListItem                                                    ║
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

    // Interface do repositório de EmpenhoMulta. Estende IRepository&lt;EmpenhoMulta&gt;.

    public interface IEmpenhoMultaRepository : IRepository<EmpenhoMulta>
        {

        IEnumerable<SelectListItem> GetEmpenhoMultaListForDropDown();

        void Update(EmpenhoMulta empenhomulta);

        }
    }


