// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewEventos.cs                                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para VIEW de eventos com dados de viagens associadas.                ║
// ║ Usado no grid de eventos e relatórios.                                      ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - EventoId, Nome, Descricao, QtdParticipantes                               ║
// ║ - DataInicial, DataFinal (formatadas como string)                           ║
// ║ - NomeRequisitante, NomeSetor                                               ║
// ║ - CustoViagem: Custo total das viagens do evento                            ║
// ║ - Status: Status do evento                                                  ║
// ║                                                                              ║
// ║ USO: Grid de eventos, Dashboard de eventos                                  ║
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
    public class ViewEventos
        {

        public Guid EventoId { get; set; }

        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public int? QtdParticipantes { get; set; }

        public string? DataInicial { get; set; }

        public string? DataFinal { get; set; }

        public string? NomeRequisitante { get; set; }

        public string? NomeSetor { get; set; }

        public double? CustoViagem { get; set; }

        public string? Status { get; set; }


        }
    }


