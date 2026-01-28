// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewOcorrenciasAbertasVeiculo.cs                                   ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para VIEW de banco que lista ocorrências abertas por veículo.        ║
// ║ Desnormalizada para performance em consultas de dashboard.                  ║
// ║                                                                              ║
// ║ CAMPOS DA VIEW:                                                              ║
// ║ - Dados da ocorrência: OcorrenciaViagemId, ViagemId, Resumo, Descricao      ║
// ║ - Dados do veículo: VeiculoId, Placa, DescricaoMarca, DescricaoModelo       ║
// ║ - Dados do motorista: MotoristaId, NomeMotorista                            ║
// ║ - Controle: DataCriacao, UsuarioCriacao, DiasEmAberto                       ║
// ║ - Classificação: Urgencia (Baixa/Média/Alta), CorUrgencia (CSS)             ║
// ║                                                                              ║
// ║ USO: Dashboard de ocorrências, alertas de manutenção                        ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    [Table("ViewOcorrenciasAbertasVeiculo")]
    public class ViewOcorrenciasAbertasVeiculo
    {
        [Key]
        public Guid OcorrenciaViagemId { get; set; }
        public Guid ViagemId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid? MotoristaId { get; set; }
        public string? Resumo { get; set; }
        public string? Descricao { get; set; }
        public string? ImagemOcorrencia { get; set; }
        public DateTime DataCriacao { get; set; }
        public string? UsuarioCriacao { get; set; }
        public string? Placa { get; set; }
        public string? DescricaoMarca { get; set; }
        public string? DescricaoModelo { get; set; }
        public string? VeiculoCompleto { get; set; }
        public DateTime? DataViagem { get; set; }
        public int? NoFichaVistoria { get; set; }
        public string? NomeMotorista { get; set; }
        public int? DiasEmAberto { get; set; }
        public string? Urgencia { get; set; }
        public string? CorUrgencia { get; set; }
    }
}
