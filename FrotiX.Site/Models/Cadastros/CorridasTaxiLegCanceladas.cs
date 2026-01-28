// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: CorridasTaxiLegCanceladas.cs                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de corridas canceladas do serviço TaxiLeg.           ║
// ║ Armazena motivos e dados de cancelamentos para análise e glosa.             ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • CorridaCanceladaId [Key] - Identificador único                            ║
// ║ • Origem - Origem da solicitação                                            ║
// ║ • Setor, SetorExtra - Setor solicitante e informações extras                ║
// ║ • Unidade, UnidadeExtra - Unidade solicitante                               ║
// ║ • QtdPassageiros - Quantidade de passageiros prevista                       ║
// ║ • MotivoUso - Motivo original da corrida                                    ║
// ║ • DataAgenda, HoraAgenda - Data e hora agendada                             ║
// ║ • DataHoraCancelamento, HoraCancelamento - Momento do cancelamento          ║
// ║ • TipoCancelamento - Tipo (usuário, motorista, sistema)                     ║
// ║ • MotivoCancelamento - Motivo informado para o cancelamento                 ║
// ║ • TempoEspera - Tempo de espera até o cancelamento (minutos)                ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Análise de cancelamentos para melhoria do serviço                         ║
// ║ • Cálculo de glosas por cancelamentos indevidos                             ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
    {

    public class CorridasCanceladasTaxiLeg
        {

        [Key]
        public Guid CorridaCanceladaId { get; set; }

        public string? Origem { get; set; }

        public string? Setor { get; set; }

        public string? SetorExtra { get; set; }

        public string? Unidade { get; set; }

        public string? UnidadeExtra { get; set; }

        public int? QtdPassageiros { get; set; }

        public string? MotivoUso { get; set; }

        public DateTime? DataAgenda { get; set; }

        public string? HoraAgenda { get; set; }

        public DateTime? DataHoraCancelamento { get; set; }

        public string? HoraCancelamento { get; set; }

        public string? TipoCancelamento { get; set; }

        public string? MotivoCancelamento { get; set; }

        public int? TempoEspera { get; set; }

        }
    }


