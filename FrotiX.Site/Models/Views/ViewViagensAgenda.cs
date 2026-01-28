// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewViagensAgenda.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para integração com calendário FullCalendar JS.                  ║
// ║ Inclui propriedades específicas para renderização de eventos no calendário. ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • ViagemId - Identificador único da viagem                                  ║
// ║ • VeiculoId, MotoristaId, EventoId - Recursos relacionados                  ║
// ║                                                                              ║
// ║ Dados da Viagem:                                                             ║
// ║ • DataInicial/Final, HoraInicio/Fim - Período da viagem                     ║
// ║ • Status, StatusAgendamento, FoiAgendamento - Estados                       ║
// ║ • Finalidade, Descricao - Detalhes da viagem                                ║
// ║                                                                              ║
// ║ Dados de Evento (FullCalendar):                                              ║
// ║ • Titulo - Título do evento no calendário                                   ║
// ║ • Start, End - Datas de início/fim para FullCalendar                        ║
// ║ • CorEvento, CorTexto - Cores do evento                                     ║
// ║ • DescricaoEvento, DescricaoMontada - Textos formatados                     ║
// ║ • NomeEvento, NomeEventoFull - Nome do evento associado                     ║
// ║                                                                              ║
// ║ Tooltips (adicionados em 16/01/2026):                                       ║
// ║ • Placa - Placa do veículo para tooltip                                     ║
// ║ • NomeMotorista - Nome do motorista para tooltip                            ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Alimenta calendário FullCalendar na tela de agenda                        ║
// ║ • Serializado como JSON para JavaScript                                     ║
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
    public class ViewViagensAgenda
        {

        public Guid ViagemId { get; set; }

        public DateTime? DataInicial { get; set; }

        public DateTime? HoraInicio { get; set; }

        public string? Status { get; set; }

        public bool? StatusAgendamento { get; set; }

        public bool? FoiAgendamento { get; set; }

        public string? Finalidade { get; set; }

        public string? NomeEvento { get; set; }

        public Guid? VeiculoId { get; set; }

        public Guid? MotoristaId { get; set; }

        public Guid? EventoId { get; set; }

        public string? Titulo { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime? DataFinal { get; set; }

        public DateTime? HoraFim { get; set; }

        public string? CorEvento { get; set; }

        public string? CorTexto { get; set; }

        public string? DescricaoEvento { get; set; }

        public string? DescricaoMontada { get; set; }

        public string? Descricao { get; set; }

        // Campos adicionados em 16/01/2026 para tooltips customizadas
        public string? Placa { get; set; }

        public string? NomeMotorista { get; set; }

        public string? NomeEventoFull { get; set; }

        }
    }


