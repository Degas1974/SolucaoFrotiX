// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MovimentacaoEmpenho.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de movimentações financeiras de empenhos.            ║
// ║ Controla créditos, débitos e ajustes nos saldos de empenhos.                ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • MovimentacaoEmpenhoViewModel - ViewModel com dropdown de empenhos         ║
// ║ • MovimentacaoEmpenho - Entidade de movimentação                            ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • MovimentacaoId [Key] - Identificador único                                ║
// ║ • Descricao - Descrição da movimentação                                     ║
// ║ • TipoMovimentacao - Tipo (Crédito, Débito, Ajuste)                         ║
// ║ • Valor - Valor da movimentação em R$                                       ║
// ║ • DataMovimentacao - Data da movimentação                                   ║
// ║ • EmpenhoId → Empenho (FK)                                                  ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Registro de notas fiscais que consomem empenho                            ║
// ║ • Ajustes de saldo por cancelamentos ou correções                           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
