/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: RegistroCupomAbastecimento.cs                                                           ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Registrar cupons de abastecimento e seus comprovantes anexados.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: RegistroCupomAbastecimentoViewModel, RegistroCupomAbastecimento                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations                                                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar o registro de cupom nas telas de cadastro.
    // ==================================================================================================
    public class RegistroCupomAbastecimentoViewModel
    {
        // Identificador do registro.
        public Guid RegistroCupomId { get; set; }

        // Entidade principal do formulário.
        public RegistroCupomAbastecimento? RegistroCupomAbastecimento { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o registro de cupons de abastecimento.
    // ==================================================================================================
    public class RegistroCupomAbastecimento
    {
        // Identificador único do registro.
        [Key]
        public Guid RegistroCupomId { get; set; }

        // Data do registro dos cupons.
        [Display(Name = "Data do Registro dos Cupons")]
        public DateTime? DataRegistro { get; set; }

        // Observações do registro.
        public string? Observacoes { get; set; }

        // Caminho/identificador do PDF anexado.
        public string? RegistroPDF { get; set; }
    }
}
