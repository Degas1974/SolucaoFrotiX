// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewViagensAgendaTodosMeses.cs                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para exibição de viagens agendadas em todos os meses do ano.     ║
// ║ Usado em calendários e visões de planejamento anual.                        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • ViagemId - Identificador único da viagem                                  ║
// ║ • VeiculoId, MotoristaId - Recursos alocados                                ║
// ║                                                                              ║
// ║ Dados da Viagem:                                                             ║
// ║ • Descricao - Descrição/destino                                             ║
// ║ • DataInicial, HoraInicio - Data e hora da viagem                           ║
// ║ • Status - Status atual                                                     ║
// ║ • StatusAgendamento - Flag de agendamento                                   ║
// ║ • Finalidade - Finalidade da viagem                                         ║
// ║                                                                              ║
// ║ Dados de Evento:                                                             ║
// ║ • NomeEvento - Nome do evento associado (se houver)                         ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Calendário de viagens com visão anual/trimestral                          ║
// ║ • Planejamento de recursos de longo prazo                                   ║
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
    public class ViewViagensAgendaTodosMeses
        {

        public Guid ViagemId { get; set; }

        public string? Descricao { get; set; }

        public DateTime? DataInicial { get; set; }

        public DateTime? HoraInicio { get; set; }

        public string? Status { get; set; }

        public bool StatusAgendamento { get; set; }

        public string? Finalidade { get; set; }

        public string? NomeEvento { get; set; }

        public Guid VeiculoId { get; set; }

        public Guid MotoristaId { get; set; }

        }
    }


