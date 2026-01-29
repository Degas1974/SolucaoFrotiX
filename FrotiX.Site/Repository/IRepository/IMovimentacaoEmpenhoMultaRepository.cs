// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IMovimentacaoEmpenhoMultaRepository.cs                          ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de MovimentacaoEmpenhoMulta, gerenciando            ║
// ║ movimentações financeiras específicas de empenhos de multas.                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetMovimentacaoEmpenhoMultaListForDropDown() → DropDown de movimentações   ║
// ║ • Update() → Atualização de movimentação empenho-multa                        ║
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
    /// Interface do repositório de MovimentacaoEmpenhoMulta. Estende IRepository&lt;MovimentacaoEmpenhoMulta&gt;.
    /// </summary>
    public interface IMovimentacaoEmpenhoMultaRepository : IRepository<MovimentacaoEmpenhoMulta>
        {

        IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown();

        void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta);

        }
    }


