/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/MovimentacaoEmpenho.cs                        ║
 * ║  Descrição: Entidade e ViewModels para movimentações de empenhos         ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
    public class MovimentacaoEmpenhoViewModel
    {
        public Guid MovimentacaoId { get; set; }
        public MovimentacaoEmpenho? MovimentacaoEmpenho { get; set; }
        public IEnumerable<SelectListItem>? EmpenhoList { get; set; }
    }

    public class MovimentacaoEmpenho
    {
        [Key]
        public Guid MovimentacaoId { get; set; }

        public string? Descricao { get; set; }

        public string? TipoMovimentacao { get; set; }

        public double? Valor { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de emissão é obrigatória)")]
        [Display(Name = "Data de Emissão")]
        public DateTime? DataMovimentacao { get; set; }

        [Display(Name = "Empenho")]
        public Guid EmpenhoId { get; set; }

        [ForeignKey("EmpenhoId")]
        public virtual Empenho? Empenho { get; set; }
    }
}
