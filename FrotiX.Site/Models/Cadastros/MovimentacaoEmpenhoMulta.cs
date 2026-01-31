/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: MovimentacaoEmpenhoMulta.cs                                                             ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Registrar movimentações de empenhos vinculadas a multas de trânsito.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MovimentacaoEmpenhoMultaViewModel, MovimentacaoEmpenhoMulta                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core, SelectListItem, Validations                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar movimentação e lista de empenhos de multa para seleção.
    // ==================================================================================================
    public class MovimentacaoEmpenhoMultaViewModel
    {
        // Identificador da movimentação.
        public Guid MovimentacaoId { get; set; }

        // Entidade principal do formulário.
        public MovimentacaoEmpenhoMulta? MovimentacaoEmpenhoMulta { get; set; }

        // Lista de empenhos de multa para seleção.
        public IEnumerable<SelectListItem>? EmpenhoMultaList { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa uma movimentação financeira ligada a multa de trânsito.
    // ==================================================================================================
    public class MovimentacaoEmpenhoMulta
    {
        // Identificador único da movimentação.
        [Key]
        public Guid MovimentacaoId { get; set; }

        // Descrição da movimentação.
        public string? Descricao { get; set; }

        // Tipo de movimentação (débito/crédito).
        public string? TipoMovimentacao { get; set; }

        // Valor movimentado.
        public double? Valor { get; set; }

        // Data da movimentação.
        public DateTime? DataMovimentacao { get; set; }

        // Multa relacionada.
        public Guid MultaId { get; set; }

        // Navegação para multa.
        [ForeignKey("MultaId")]
        public virtual Multa? Multa { get; set; }

        // Empenho de multa relacionado.
        public Guid EmpenhoMultaId { get; set; }

        // Navegação para empenho de multa.
        [ForeignKey("EmpenhoMultaId")]
        public virtual EmpenhoMulta? EmpenhoMulta { get; set; }
    }
}
