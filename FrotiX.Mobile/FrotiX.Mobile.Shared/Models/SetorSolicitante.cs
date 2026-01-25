using System.ComponentModel.DataAnnotations;

namespace FrotiX.Mobile.Shared.Models
{
    public class SetorSolicitante
    {
        [Key]
        public Guid SetorSolicitanteId { get; set; }

        [StringLength(200, ErrorMessage = "o Nome não pode exceder 200 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Setor")]
        public string? Nome { get; set; }

        [StringLength(50, ErrorMessage = "A Sigla não pode exceder 50 caracteres")]
        [Display(Name = "Sigla")]
        public string? Sigla { get; set; }

        [Display(Name = "CNH")]
        public Guid? SetorPaiId { get; set; }

        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal")]
        public int? Ramal { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string? UsuarioIdAlteracao { get; set; }
    }
}
