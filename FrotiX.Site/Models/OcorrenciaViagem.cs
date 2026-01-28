// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: OcorrenciaViagem.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de ocorrências/problemas identificados em viagens.   ║
// ║ Permite rastreabilidade de defeitos, avarias e manutenções necessárias.     ║
// ║                                                                              ║
// ║ CAMPOS PRINCIPAIS:                                                           ║
// ║ - ViagemId, VeiculoId (obrigatórios), MotoristaId (opcional)               ║
// ║ - Resumo (200 chars), Descricao, ImagemOcorrencia                           ║
// ║ - Status: "Aberta", "Em Análise", "Baixada"                                 ║
// ║ - StatusOcorrencia: null/true = Aberta, false = Baixada                     ║
// ║ - DataCriacao, DataBaixa, UsuarioCriacao, UsuarioBaixa                      ║
// ║ - ItemManutencaoId: Link para item de manutenção gerado                     ║
// ║ - Observacoes, Solucao: Campos de texto livre                               ║
// ║                                                                              ║
// ║ FLUXO: Viagem gera Ocorrência → Análise → Baixa (com/sem Manutenção)       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
