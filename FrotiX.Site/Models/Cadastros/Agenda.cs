// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Agenda.cs                                                          ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para representação de eventos de agenda no calendário de viagens.    ║
// ║ Usado para integração com FullCalendar JS na interface do usuário.          ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • AgendaViewModel - Filtro de status para a agenda                          ║
// ║ • Agenda - Dados do evento para renderização no calendário                  ║
// ║                                                                              ║
// ║ PROPRIEDADES AGENDA:                                                         ║
// ║ • ViagemId - Referência à viagem                                            ║
// ║ • HoraInicial/Final, DataInicial - Período do evento                        ║
// ║ • Descricao - Descrição/destino da viagem                                   ║
// ║ • Titulo - Título para exibição no calendário                               ║
// ║ • Status - Status da viagem                                                 ║
// ║ • DiaTodo - Flag se ocupa o dia inteiro                                     ║
// ║ • CorEvento, CorTexto - Cores para renderização visual                      ║
// ║ • Finalidade - Finalidade da viagem                                         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
    {

    public class AgendaViewModel
        {
        public string Status { get; set; }
        }


    public class Agenda
        {

        public Guid ViagemId { get; set; }

        public DateTime HoraInicial { get; set; }

        public DateTime HoraFinal { get; set; }

        public DateTime DataInicial { get; set; }

        public string Descricao { get; set; }

        public string Titulo { get; set; }

        public string Status { get; set; }

        public bool DiaTodo { get; set; }

        public string CorEvento { get; set; }

        public string CorTexto { get; set; }

        public string Finalidade { get; set; }
        }
    }


