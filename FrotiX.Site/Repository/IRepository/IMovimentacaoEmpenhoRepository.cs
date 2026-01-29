// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IMovimentacaoEmpenhoRepository.cs                               ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de MovimentacaoEmpenho, gerenciando movimentações   ║
// ║ financeiras vinculadas a empenhos (créditos, débitos, anulações).           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetMovimentacaoEmpenhoListForDropDown() → DropDown de movimentações        ║
// ║ • Update() → Atualização de movimentação                                      ║
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
    /// Interface do repositório de MovimentacaoEmpenho. Estende IRepository&lt;MovimentacaoEmpenho&gt;.
    /// </summary>
    public interface IMovimentacaoEmpenhoRepository : IRepository<MovimentacaoEmpenho>
        {

        IEnumerable<SelectListItem> GetMovimentacaoEmpenhoListForDropDown();

        void Update(MovimentacaoEmpenho movimentacaoempenho);

        }
    }


