/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrenciasViagem.cs                                                                ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Modelo mapeado da View SQL de ocorrências de viagens (status, origem/destino).        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: OcorrenciaViagemId, ViagemId, Resumo, Status, DataBaixa, Origem, Destino, etc.           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations | TABLE: ViewOcorrenciasViagem                       ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    [Table("ViewOcorrenciasViagem")]
    public class ViewOcorrenciasViagem
    {
        [Key]
        public Guid OcorrenciaViagemId { get; set; }
        public Guid ViagemId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid? MotoristaId { get; set; }
        public string? Resumo { get; set; }
        public string? Descricao { get; set; }
        public string? ImagemOcorrencia { get; set; }
        public string? Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataBaixa { get; set; }
        public string? UsuarioCriacao { get; set; }
        public string? UsuarioBaixa { get; set; }
        public Guid? ItemManutencaoId { get; set; }
        public string? Observacoes { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFim { get; set; }
        public int? NoFichaVistoria { get; set; }
        public string? Origem { get; set; }
        public string? Destino { get; set; }
        public string? FinalidadeViagem { get; set; }
        public string? StatusViagem { get; set; }
        public string? Placa { get; set; }
        public string? DescricaoMarca { get; set; }
        public string? DescricaoModelo { get; set; }
        public string? VeiculoCompleto { get; set; }
        public string? MarcaModelo { get; set; }
        public string? NomeMotorista { get; set; }
        public string? FotoMotorista { get; set; }
        public int? DiasEmAberto { get; set; }
        public string? Urgencia { get; set; }
        public string? CorUrgencia { get; set; }
    }
}
