using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiXApi.Models
{
    [Table("ViagensEconomildo")]
    public class ViagensEconomildo
    {
        [Key]
        public Guid ViagemEconomildoId { get; set; } = Guid.NewGuid();

        public DateTime? Data { get; set; }

        public Guid? VeiculoId { get; set; }

        [ForeignKey(nameof(VeiculoId))]
        public virtual Veiculo? Veiculo { get; set; }

        public Guid? MotoristaId { get; set; }

        [ForeignKey(nameof(MotoristaId))]
        public virtual Motorista? Motorista { get; set; }

        public string? MOB { get; set; }

        public string? Responsavel { get; set; }

        public string? IdaVolta { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFim { get; set; }

        public int? QtdPassageiros { get; set; }

        // Campos adicionais para controle local
        [NotMapped]
        public bool Transmitido { get; set; } = false;

        [NotMapped]
        public string? TransmitidoStr => Transmitido ? "Sim" : "Não";
    }

    // ViewModel para armazenamento local
    public class ViagemEconomildoLocal
    {
        public Guid ViagemEconomildoId { get; set; }
        public DateTime Data { get; set; }
        public Guid VeiculoId { get; set; }
        public string? VeiculoPlaca { get; set; }
        public Guid MotoristaId { get; set; }
        public string? MotoristaNome { get; set; }
        public string? MOB { get; set; }
        public string? Responsavel { get; set; }
        public string? IdaVolta { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFim { get; set; }
        public int QtdPassageiros { get; set; }
        public bool Transmitido { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    // ViewModel para exibição
    public class ViagemEconomildoViewModel
    {
        public Guid ViagemEconomildoId { get; set; }
        public DateTime Data { get; set; }
        public string? DataStr => Data.ToString("dd/MM/yyyy");
        public string? VeiculoPlaca { get; set; }
        public string? MotoristaNome { get; set; }
        public string? MOB { get; set; }
        public string? IdaVolta { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFim { get; set; }
        public int QtdPassageiros { get; set; }
        public bool Transmitido { get; set; }
        public string? TransmitidoStr => Transmitido ? "Sim" : "Não";
    }
}