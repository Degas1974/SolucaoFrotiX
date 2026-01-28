// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewFluxoEconomildo.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para exibição do fluxo de viagens Economildo (transporte         ║
// ║ coletivo interno). Usado em telas de gestão de fluxo de passageiros.        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • VeiculoId - Veículo utilizado no fluxo                                    ║
// ║ • ViagemEconomildoId - Identificador único da viagem                        ║
// ║ • MotoristaId - Motorista responsável                                       ║
// ║                                                                              ║
// ║ Dados da Viagem:                                                             ║
// ║ • TipoCondutor - Tipo do condutor (titular, reserva, terceiro)              ║
// ║ • Data - Data da viagem                                                     ║
// ║ • MOB - Código MOB (Movimento Operacional Básico)                           ║
// ║ • HoraInicio/HoraFim - Horários da viagem                                   ║
// ║ • QtdPassageiros - Quantidade de passageiros transportados                  ║
// ║                                                                              ║
// ║ Dados para Exibição:                                                         ║
// ║ • NomeMotorista - Nome do motorista                                         ║
// ║ • DescricaoVeiculo - Descrição do veículo (marca/modelo/placa)              ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
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
    public class ViewFluxoEconomildo
        {

        public Guid VeiculoId { get; set; }

        public Guid ViagemEconomildoId { get; set; }

        public Guid MotoristaId { get; set; }

        public string? TipoCondutor { get; set; }

        public DateTime? Data { get; set; }

        public string? MOB { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFim { get; set; }

        public int? QtdPassageiros { get; set; }

        public string? NomeMotorista { get; set; }

        public string? DescricaoVeiculo { get; set; }


        }
    }


