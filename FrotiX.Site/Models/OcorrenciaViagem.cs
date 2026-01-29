/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: OcorrenciaViagem.cs                                                                     ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade para registro de ocorrências durante viagens (acidentes, problemas, etc).    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: OcorrenciaViagemId, ViagemId, VeiculoId, MotoristaId?, Resumo, Descricao                  ║
   ║    ImagemOcorrencia, Status (Aberta/Baixada), StatusOcorrencia, DataCriacao, DataBaixa              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    [Table("OcorrenciaViagem")]
    public class OcorrenciaViagem
    {
        [Key]
        public Guid OcorrenciaViagemId { get; set; }

        [Required]
        public Guid ViagemId { get; set; }

        [Required]
        public Guid VeiculoId { get; set; }

        public Guid? MotoristaId { get; set; }

        [StringLength(200)]
        public string Resumo { get; set; } = "";

        public string Descricao { get; set; } = "";
        public string ImagemOcorrencia { get; set; } = "";

        [StringLength(20)]
        public string Status { get; set; } = "Aberta";

        /// <summary>
        /// Status da ocorrência: NULL ou true = Aberta, false = Baixada
        /// </summary>
        public bool? StatusOcorrencia { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataBaixa { get; set; }

        [StringLength(100)]
        public string UsuarioCriacao { get; set; } = "";

        [StringLength(100)]
        public string UsuarioBaixa { get; set; } = "";

        public Guid? ItemManutencaoId { get; set; }

        [StringLength(500)]
        public string Observacoes { get; set; } = "";

        [StringLength(500)]
        public string Solucao { get; set; } = "";

        //[ForeignKey("ViagemId")]
        //public virtual Viagem? Viagem { get; set; }
        //[ForeignKey("VeiculoId")]
        //public virtual Veiculo? Veiculo { get; set; }
        //[ForeignKey("MotoristaId")]
        //public virtual Motorista? Motorista { get; set; }
    }
}
