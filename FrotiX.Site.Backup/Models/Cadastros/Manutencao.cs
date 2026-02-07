/* ****************************************************************************************
 * ⚡ ARQUIVO: Manutencao.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar manutenções de veículos com datas, status e veículo reserva.
 *
 * 📥 ENTRADAS     : Datas-chave da manutenção, status e vínculos com veículos.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para formulários.
 *
 * 🔗 CHAMADA POR  : Módulos de manutenção e relatórios.
 *
 * 🔄 CHAMA        : DataAnnotations, ForeignKey.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 **************************************************************************************** */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: Manutencao
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar registro de manutenção de veículo com datas e status.
     *
     * 📥 ENTRADAS     : Datas, status, OS e vínculos.
     *
     * 📤 SAÍDAS       : Registro persistido para controle de manutenção.
     *
     * 🔗 CHAMADA POR  : Fluxos de manutenção.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class Manutencao
    {
        // Data de alteração do registro.
        public DateTime? DataAlteracao { get; set; }

        // Data de cancelamento.
        public DateTime? DataCancelamento { get; set; }

        // Data de criação.
        public DateTime? DataCriacao { get; set; }

        // Data de devolução do veículo.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Devolução")]
        public DateTime? DataDevolucao { get; set; }

        // Data de devolução do veículo reserva.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Devolução do Reserva")]
        public DateTime? DataDevolucaoReserva { get; set; }

        // Data de entrega do veículo.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Entrega")]
        public DateTime? DataEntrega { get; set; }

        // Data de finalização da manutenção.
        public DateTime? DataFinalizacao { get; set; }

        // Data de recebimento do veículo reserva.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Entrega do Reserva")]
        public DateTime? DataRecebimentoReserva { get; set; }

        // Data de recolhimento do veículo.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Recolhimento")]
        public DateTime? DataRecolhimento { get; set; }

        // Data de solicitação da manutenção.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Solicitação")]
        public DateTime? DataSolicitacao { get; set; }

        // Data de disponibilização do veículo.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Disponibilização")]
        public DateTime? DataDisponibilidade { get; set; }

        // Usuário responsável pela última alteração.
        [Display(Name = "IdUsuarioAlteracao")]
        public string? IdUsuarioAlteracao { get; set; }

        // Usuário responsável pelo cancelamento.
        [Display(Name = "IdUsuarioCancelamento")]
        [Column(TypeName = "nvarchar(450)")]
        public string? IdUsuarioCancelamento { get; set; }

        // Usuário responsável pela criação.
        [Display(Name = "IdUsuarioAlteracao")]
        [Column(TypeName = "nvarchar(450)")]
        public string? IdUsuarioCriacao { get; set; }

        // Usuário responsável pela finalização.
        [Display(Name = "IdUsuarioCancelamento")]
        [Column(TypeName = "nvarchar(450)")]
        public string? IdUsuarioFinalizacao { get; set; }

        // Identificador único da manutenção.
        [Key]
        public Guid ManutencaoId { get; set; }

        // Indica manutenção preventiva.
        [Display(Name = "Man.Preventiva")]
        public bool ManutencaoPreventiva { get; set; }

        // Número da OS.
        [StringLength(50, ErrorMessage = "O nº da OS não pode exceder 50 caracteres")]
        [Display(Name = "Número da OS")]
        public string? NumOS { get; set; }

        // Quilometragem registrada para manutenção.
        [Display(Name = "Quilometragem Manutenção")]
        public int? QuilometragemManutencao { get; set; }

        // Indica se veículo reserva foi enviado.
        [Display(Name = "Carro Reserva")]
        public bool ReservaEnviado { get; set; }

        // Resumo da OS.
        [StringLength(500, ErrorMessage = "O resumo da OS não pode exceder 500 caracteres")]
        [Display(Name = "Resumo da OS")]
        public string? ResumoOS { get; set; }

        // Status da OS/manutenção.
        [Display(Name = "Status")]
        public string? StatusOS { get; set; }

        // Navegação para veículo principal.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        // Veículo principal relacionado.
        [Display(Name = "Veículo")]
        public Guid? VeiculoId { get; set; }

        // Navegação para veículo reserva.
        [ForeignKey("VeiculoReservaId")]
        public virtual Veiculo? VeiculoReserva { get; set; }

        // Veículo reserva relacionado.
        [Display(Name = "Veículo Reserva")]
        public Guid? VeiculoReservaId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ManutencaoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar manutenção e chave em contextos de formulário.
     *
     * 📥 ENTRADAS     : Manutencao e ManutencaoId.
     *
     * 📤 SAÍDAS       : ViewModel para telas/rotas.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de manutenção.
     ****************************************************************************************/
    public class ManutencaoViewModel
    {
        // Registro de manutenção carregado/alterado.
        public Manutencao? Manutencao { get; set; }

        // Identificador utilizado em telas/rotas.
        public Guid ManutencaoId { get; set; }
    }
}
