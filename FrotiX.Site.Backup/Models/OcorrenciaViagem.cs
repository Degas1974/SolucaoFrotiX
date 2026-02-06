/* ****************************************************************************************
 * ⚡ ARQUIVO: OcorrenciaViagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar ocorrências durante viagens (acidentes, problemas, etc.).
 *
 * 📥 ENTRADAS     : Identificadores, descrição, status e evidências.
 *
 * 📤 SAÍDAS       : Registro persistido para acompanhamento.
 *
 * 🔗 CHAMADA POR  : Fluxos de viagem e manutenção.
 *
 * 🔄 CHAMA        : DataAnnotations, EF Core (Table).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: OcorrenciaViagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar ocorrência associada a uma viagem.
     *
     * 📥 ENTRADAS     : Viagem, veículo, motorista e detalhes da ocorrência.
     *
     * 📤 SAÍDAS       : Entidade persistida para controle de ocorrências.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : Key, Required, StringLength.
     ****************************************************************************************/
    [Table("OcorrenciaViagem")]
    public class OcorrenciaViagem
    {
        // Identificador da ocorrência.
        [Key]
        public Guid OcorrenciaViagemId { get; set; }

        // Viagem associada.
        [Required]
        public Guid ViagemId { get; set; }

        // Veículo associado.
        [Required]
        public Guid VeiculoId { get; set; }

        // Motorista associado (opcional).
        public Guid? MotoristaId { get; set; }

        // Resumo da ocorrência.
        [StringLength(200)]
        public string Resumo { get; set; } = "";

        // Descrição detalhada.
        public string Descricao { get; set; } = "";
        // Imagem da ocorrência (base64 ou caminho).
        public string ImagemOcorrencia { get; set; } = "";

        // Status textual (Aberta/Baixada).
        [StringLength(20)]
        public string Status { get; set; } = "Aberta";

        // Status lógico: null/true = aberta, false = baixada.
        public bool? StatusOcorrencia { get; set; }

        // Data de criação.
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        // Data de baixa.
        public DateTime? DataBaixa { get; set; }

        // Usuário que criou.
        [StringLength(100)]
        public string UsuarioCriacao { get; set; } = "";

        // Usuário que baixou.
        [StringLength(100)]
        public string UsuarioBaixa { get; set; } = "";

        // Item de manutenção relacionado.
        public Guid? ItemManutencaoId { get; set; }

        // Observações adicionais.
        [StringLength(500)]
        public string Observacoes { get; set; } = "";

        // Solução aplicada.
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
