// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Lavagem.cs                                                         ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de lavagens de veículos da frota.                    ║
// ║ Inclui data/hora de início e fim para cálculo de duração.                   ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • LavagemId [Key] - Identificador único                                     ║
// ║ • Data - Data da lavagem                                                    ║
// ║ • HorarioInicio - Hora de início da lavagem                                 ║
// ║ • HorarioFim - Hora de término da lavagem                                   ║
// ║ • VeiculoId → Veiculo (FK) - Veículo lavado                                 ║
// ║ • MotoristaId → Motorista (FK) - Motorista responsável pelo veículo         ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • LavadoresLavagem - Lavadores que realizaram a lavagem (N:N)               ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Controle de higienização da frota                                         ║
// ║ • Relatórios de frequência de lavagem por veículo                           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    public class Lavagem
    {
        [Key]
        public Guid LavagemId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }

        [Display(Name = "Horário Início")]
        public DateTime? HorarioInicio { get; set; }

        [Display(Name = "Horário Fim")]
        public DateTime? HorarioFim { get; set; }

        [Display(Name = "Veículo Lavado")]
        public Guid VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        [Display(Name = "Motorista")]
        public Guid MotoristaId { get; set; }

        [ForeignKey("MotoristaId")]
        public virtual Motorista? Motorista { get; set; }
    }
}
