/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: MovimentacaoPatrimonio.cs                                                               ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Registrar movimentações patrimoniais entre setores/seções e o patrimônio associado.   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MovimentacaoPatrimonioViewModel, MovimentacaoPatrimonio                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: consolidar dados da movimentação e metadados de exibição.
    // ==================================================================================================
    public class MovimentacaoPatrimonioViewModel
    {
        // Entidade principal do formulário.
        public MovimentacaoPatrimonio? MovimentacaoPatrimonio { get; set; }

        // Identificador da movimentação.
        public Guid? MovimentacaoPatrimonioId { get; set; }

        // Patrimônio relacionado.
        public Guid? PatrimonioId { get; set; }

        // Estes já estão corretos (nullable):
        // Seção/setor de origem/destino da movimentação.
        public Guid? SecaoOrigemId { get; set; } // ✓
        public Guid? SetorOrigemId { get; set; } // ✓
        public Guid? SecaoDestinoId { get; set; } // ✓
        public Guid? SetorDestinoId { get; set; } // ✓

        // Strings já são nullable por padrío no C# recente
        // Metadados para exibição na UI.
        public string? NomeUsuarioAlteracao { get; set; }
        public string? PatrimonioNome { get; set; }
        public string? SetorOrigemNome { get; set; }
        public string? SecaoOrigemNome { get; set; }
        public string? SetorDestinoNome { get; set; }
        public string? SecaoDestinoNome { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa a movimentação de um patrimônio entre setores/seções.
    // ==================================================================================================
    public class MovimentacaoPatrimonio
    {
        // Identificador único da movimentação.
        [Key]
        public Guid MovimentacaoPatrimonioId { get; set; }

        // Data da movimentação.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime? DataMovimentacao { get; set; }

        // Responsável pela movimentação.
        public string? ResponsavelMovimentacao { get; set; }

        // Setor de origem.
        public Guid? SetorOrigemId { get; set; }

        // Setor de destino.
        public Guid? SetorDestinoId { get; set; }

        // Seção de origem.
        public Guid? SecaoOrigemId { get; set; }

        // Seção de destino.
        public Guid? SecaoDestinoId { get; set; }

        // Patrimônio movimentado.
        public Guid? PatrimonioId { get; set; }
    }
}
