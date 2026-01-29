/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/MovimentacaoPatrimonio.cs                     ║
 * ║  Descrição: Entidade e ViewModels para movimentações patrimoniais        ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace FrotiX.Models
{
    public class MovimentacaoPatrimonioViewModel
    {
        public MovimentacaoPatrimonio? MovimentacaoPatrimonio { get; set; }

        public Guid? MovimentacaoPatrimonioId { get; set; }

        public Guid? PatrimonioId { get; set; }

        // Estes já estão corretos (nullable):
        public Guid? SecaoOrigemId { get; set; } // ✓
        public Guid? SetorOrigemId { get; set; } // ✓
        public Guid? SecaoDestinoId { get; set; } // ✓
        public Guid? SetorDestinoId { get; set; } // ✓

        // Strings já são nullable por padrío no C# recente
        public string? NomeUsuarioAlteracao { get; set; }
        public string? PatrimonioNome { get; set; }
        public string? SetorOrigemNome { get; set; }
        public string? SecaoOrigemNome { get; set; }
        public string? SetorDestinoNome { get; set; }
        public string? SecaoDestinoNome { get; set; }
    }

    public class MovimentacaoPatrimonio
    {
        [Key]
        public Guid MovimentacaoPatrimonioId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime? DataMovimentacao { get; set; }

        public string? ResponsavelMovimentacao { get; set; }

        public Guid? SetorOrigemId { get; set; }

        public Guid? SetorDestinoId { get; set; }

        public Guid? SecaoOrigemId { get; set; }

        public Guid? SecaoDestinoId { get; set; }

        public Guid? PatrimonioId { get; set; }
    }
}
