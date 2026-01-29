// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IEmpenhoMultaRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de EmpenhoMulta, gerenciando empenhos específicos   ║
// ║ para pagamento de multas de trânsito.                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetEmpenhoMultaListForDropDown() → DropDown de empenhos de multa           ║
// ║ • Update() → Atualização de empenho-multa                                    ║
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
    /// Interface do repositório de EmpenhoMulta. Estende IRepository&lt;EmpenhoMulta&gt;.
    /// </summary>
    public interface IEmpenhoMultaRepository : IRepository<EmpenhoMulta>
        {

        IEnumerable<SelectListItem> GetEmpenhoMultaListForDropDown();

        void Update(EmpenhoMulta empenhomulta);

        }
    }


