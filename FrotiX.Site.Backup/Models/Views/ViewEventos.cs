/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewEventos.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de eventos para listagens e filtros.
 *
 * 📥 ENTRADAS     : Requisitante, setor, datas e custos.
 *
 * 📤 SAÍDAS       : DTO de leitura para relatórios.
 *
 * 🔗 CHAMADA POR  : Listagens de eventos.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Services, FrotiX.Validations.
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ MODEL: ViewEventos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de eventos.
     *
     * 📥 ENTRADAS     : Identificadores, datas e dados de requisitante.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e relatórios.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewEventos
        {

        // Identificador do evento.
        public Guid EventoId { get; set; }

        // Nome do evento.
        public string? Nome { get; set; }

        // Descrição do evento.
        public string? Descricao { get; set; }

        // Quantidade de participantes.
        public int? QtdParticipantes { get; set; }

        // Data inicial (formatada).
        public string? DataInicial { get; set; }

        // Data final (formatada).
        public string? DataFinal { get; set; }

        // Nome do requisitante.
        public string? NomeRequisitante { get; set; }

        // Nome do setor.
        public string? NomeSetor { get; set; }

        // Custo da viagem.
        public double? CustoViagem { get; set; }

        // Status do evento.
        public string? Status { get; set; }


        }
    }
