// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViagensEconomildo.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de viagens do sistema Economildo.                    ║
// ║ Controle de deslocamentos de transporte coletivo interno.                   ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • ViagensEconomildo - Entidade única (sem ViewModel separada)               ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • ViagemEconomildoId [Key] - Identificador único                            ║
// ║ • Data - Data da viagem                                                     ║
// ║ • MOB - Código MOB da viagem                                                ║
// ║ • Responsavel - Responsável pela viagem                                     ║
// ║ • VeiculoId → Veiculo - Veículo utilizado                                   ║
// ║ • MotoristaId → Motorista - Motorista da viagem                             ║
// ║ • IdaVolta - Indicador de ida/volta                                         ║
// ║ • HoraInicio, HoraFim - Horários da viagem                                  ║
// ║ • QtdPassageiros - Quantidade de passageiros                                ║
// ║ • Trajeto - Descrição do trajeto                                            ║
// ║ • Duracao - Duração em minutos                                              ║
// ║                                                                              ║
// ║ DIFERENÇA DE VIAGEM:                                                         ║
// ║ • Economildo = transporte coletivo interno (sem requisitante)               ║
// ║ • Viagem regular = transporte sob demanda (com requisitante)                ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class ViagensEconomildo
    {
        [Key]
        public Guid ViagemEconomildoId { get; set; }

        public DateTime? Data { get; set; }

        public string? MOB { get; set; }

        public string? Responsavel { get; set; }

        public Guid VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        public Guid MotoristaId { get; set; }

        [ForeignKey("MotoristaId")]
        public virtual Motorista? Motorista { get; set; }

        public string? IdaVolta { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFim { get; set; }

        public int? QtdPassageiros { get; set; }

        public string? Trajeto { get; set; }

        public int? Duracao { get; set; }
    }
}
