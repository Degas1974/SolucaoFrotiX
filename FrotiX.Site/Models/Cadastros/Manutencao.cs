// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Manutencao.cs                                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade principal para gestão de ordens de serviço de manutenção.          ║
// ║ Controla todo o ciclo de vida: abertura, execução, entrega e fechamento.    ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • Manutencao - Entidade principal                                           ║
// ║ • ManutencaoViewModel - ViewModel simples                                   ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • ManutencaoId [Key] - Identificador único                                  ║
// ║ • NumOS - Número da ordem de serviço (max 50 chars)                         ║
// ║ • ResumoOS - Resumo do serviço (max 500 chars)                              ║
// ║ • StatusOS - Status (Aberta, Em Andamento, Concluída, Cancelada)            ║
// ║ • ManutencaoPreventiva - Flag de manutenção preventiva                      ║
// ║ • QuilometragemManutencao - Km no momento da manutenção                     ║
// ║ • ReservaEnviado - Flag de notificação de reserva enviada                   ║
// ║                                                                              ║
// ║ Datas do Ciclo:                                                              ║
// ║ • DataSolicitacao, DataDisponibilidade - Abertura                           ║
// ║ • DataRecolhimento, DataEntrega - Ida/volta oficina                         ║
// ║ • DataRecebimentoReserva, DataDevolucaoReserva - Carro reserva              ║
// ║ • DataDevolucao, DataFinalizacao - Fechamento                               ║
// ║ • DataCriacao, DataAlteracao, DataCancelamento - Auditoria                  ║
// ║                                                                              ║
// ║ Auditoria:                                                                    ║
// ║ • IdUsuarioCriacao/Alteracao/Cancelamento/Finalizacao                       ║
// ║                                                                              ║
// ║ Relacionamentos:                                                              ║
// ║ • VeiculoId → Veiculo - Veículo em manutenção                               ║
// ║ • VeiculoReservaId → Veiculo - Veículo reserva                              ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    public class Manutencao
    {
        public DateTime? DataAlteracao { get; set; }

        public DateTime? DataCancelamento { get; set; }

        public DateTime? DataCriacao { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Devolução")]
        public DateTime? DataDevolucao { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Devolução do Reserva")]
        public DateTime? DataDevolucaoReserva { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Entrega")]
        public DateTime? DataEntrega { get; set; }

        public DateTime? DataFinalizacao { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Entrega do Reserva")]
        public DateTime? DataRecebimentoReserva { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Recolhimento")]
        public DateTime? DataRecolhimento { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Solicitação")]
        public DateTime? DataSolicitacao { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Disponibilização")]
        public DateTime? DataDisponibilidade { get; set; }

        [Display(Name = "IdUsuarioAlteracao")]
        public string? IdUsuarioAlteracao { get; set; }

        [Display(Name = "IdUsuarioCancelamento")]
        public string? IdUsuarioCancelamento { get; set; }

        [Display(Name = "IdUsuarioAlteracao")]
        public string? IdUsuarioCriacao { get; set; }

        [Display(Name = "IdUsuarioCancelamento")]
        public string? IdUsuarioFinalizacao { get; set; }

        [Key]
        public Guid ManutencaoId { get; set; }

        [Display(Name = "Man.Preventiva")]
        public bool ManutencaoPreventiva { get; set; }

        [StringLength(50, ErrorMessage = "O nº da OS não pode exceder 50 caracteres")]
        [Display(Name = "Número da OS")]
        public string? NumOS { get; set; }

        [Display(Name = "Quilometragem Manutenção")]
        public int? QuilometragemManutencao { get; set; }

        [Display(Name = "Carro Reserva")]
        public bool ReservaEnviado { get; set; }

        [StringLength(500, ErrorMessage = "O resumo da OS não pode exceder 500 caracteres")]
        [Display(Name = "Resumo da OS")]
        public string? ResumoOS { get; set; }

        [Display(Name = "Status")]
        public string? StatusOS { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        [Display(Name = "Veículo")]
        public Guid? VeiculoId { get; set; }

        [ForeignKey("VeiculoReservaId")]
        public virtual Veiculo? VeiculoReserva { get; set; }

        [Display(Name = "Veículo Reserva")]
        public Guid? VeiculoReservaId { get; set; }
    }

    public class ManutencaoViewModel
    {
        public Manutencao? Manutencao { get; set; }
        public Guid ManutencaoId { get; set; }
    }
}
