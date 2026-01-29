/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/ViewOcorrenciasAbertasVeiculo.cs                        ║
 * ║  Descrição: Modelo mapeado da View de ocorrências abertas por veículo    ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
