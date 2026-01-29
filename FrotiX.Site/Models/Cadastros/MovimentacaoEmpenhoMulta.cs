/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/MovimentacaoEmpenhoMulta.cs                   ║
 * ║  Descrição: Entidade e ViewModels para movimentações de empenhos de multa║
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
    public class MovimentacaoEmpenhoMultaViewModel
    {
        public Guid MovimentacaoId { get; set; }
        public MovimentacaoEmpenhoMulta? MovimentacaoEmpenhoMulta { get; set; }
        public IEnumerable<SelectListItem>? EmpenhoMultaList { get; set; }
    }

    public class MovimentacaoEmpenhoMulta
    {
        [Key]
        public Guid MovimentacaoId { get; set; }

        public string? Descricao { get; set; }

        public string? TipoMovimentacao { get; set; }

        public double? Valor { get; set; }

        public DateTime? DataMovimentacao { get; set; }

        public Guid MultaId { get; set; }

        [ForeignKey("MultaId")]
        public virtual Multa? Multa { get; set; }

        public Guid EmpenhoMultaId { get; set; }

        [ForeignKey("EmpenhoMultaId")]
        public virtual EmpenhoMulta? EmpenhoMulta { get; set; }
    }
}
