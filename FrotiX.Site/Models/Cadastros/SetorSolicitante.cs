/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: SetorSolicitante.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Cadastrar setores solicitantes de viagens (departamentos clientes).                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: SetorSolicitanteViewModel, SetorSolicitante                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, Validations                                                       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar setor solicitante e metadados de alteração.
    // ==================================================================================================
    public class SetorSolicitanteViewModel
    {
        // Identificador do setor solicitante.
        public Guid SetorSolicitanteId { get; set; }

        // Entidade principal do formulário.
        public SetorSolicitante? SetorSolicitante { get; set; }

        // Nome do usuário responsável pela última alteração.
        public string? NomeUsuarioAlteracao { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa um setor solicitante.
    // ==================================================================================================
    public class SetorSolicitante
    {
        // Identificador único do setor.
        [Key]
        public Guid SetorSolicitanteId { get; set; }

        // Nome do setor solicitante.
        [StringLength(200, ErrorMessage = "o Nome não pode exceder 200 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Setor")]
        public string? Nome { get; set; }

        // Sigla do setor.
        [StringLength(50, ErrorMessage = "A Sigla não pode exceder 50 caracteres")]
        [Display(Name = "Sigla")]
        public string? Sigla { get; set; }

        // Setor pai (hierarquia).
        [Display(Name = "CNH")]
        public Guid? SetorPaiId { get; set; }

        // Ramal do setor.
        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal")]
        public int? Ramal { get; set; }

        // Status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        // Data da última alteração.
        public DateTime? DataAlteracao { get; set; }

        // Usuário responsável pela alteração.
        public string? UsuarioIdAlteracao { get; set; }
    }
}
