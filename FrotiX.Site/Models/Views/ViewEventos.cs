/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Views/ViewEventos.cs                                    ║
 * ║  Descrição: Modelo mapeado da View de eventos no banco de dados          ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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


