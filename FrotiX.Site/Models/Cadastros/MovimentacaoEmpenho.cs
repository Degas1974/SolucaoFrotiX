/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: MovimentacaoEmpenho.cs                                                                  ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Registrar movimentações de empenho (tipo, valor e data) vinculadas ao empenho.        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MovimentacaoEmpenhoViewModel, MovimentacaoEmpenho                                       ║
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
    // Finalidade: transportar a movimentação e a lista de empenhos para seleção.
    // ==================================================================================================
    public class MovimentacaoEmpenhoViewModel
    {
        // Identificador da movimentação.
        public Guid MovimentacaoId { get; set; }

        // Entidade principal do formulário.
        public MovimentacaoEmpenho? MovimentacaoEmpenho { get; set; }

        // Lista de empenhos disponível para seleção.
        public IEnumerable<SelectListItem>? EmpenhoList { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa uma movimentação financeira associada a um empenho.
    // ==================================================================================================
    public class MovimentacaoEmpenho
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
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de emissão é obrigatória)")]
        [Display(Name = "Data de Emissão")]
        public DateTime? DataMovimentacao { get; set; }

        // Empenho associado.
        [Display(Name = "Empenho")]
        public Guid EmpenhoId { get; set; }

        // Navegação para empenho.
        [ForeignKey("EmpenhoId")]
        public virtual Empenho? Empenho { get; set; }
    }
}
