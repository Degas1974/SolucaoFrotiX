/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewEventos.cs                                                                         ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de eventos para listagens e filtros (requisitante, setor, datas).            ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: EventoId, Nome, Descricao, DataInicial/Final, NomeRequisitante, NomeSetor                ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                        ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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


