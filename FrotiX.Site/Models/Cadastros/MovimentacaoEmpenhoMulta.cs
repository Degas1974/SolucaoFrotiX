// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MovimentacaoEmpenhoMulta.cs                                        ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de movimentações de empenhos específicos de multas.  ║
// ║ Vincula pagamentos de multas aos empenhos de multas.                        ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • MovimentacaoEmpenhoMultaViewModel - ViewModel com dropdown de empenhos    ║
// ║ • MovimentacaoEmpenhoMulta - Entidade de movimentação                       ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • MovimentacaoId [Key] - Identificador único                                ║
// ║ • Descricao - Descrição da movimentação                                     ║
// ║ • TipoMovimentacao - Tipo (Débito por pagamento, Estorno, etc)              ║
// ║ • Valor - Valor da movimentação em R$                                       ║
// ║ • DataMovimentacao - Data da movimentação                                   ║
// ║ • MultaId → Multa (FK) - Multa sendo paga                                   ║
// ║ • EmpenhoMultaId → EmpenhoMulta (FK) - Empenho utilizado                    ║
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
