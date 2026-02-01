/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewCustosViagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de custos calculados de viagens.
 *
 * 📥 ENTRADAS     : Dados de viagem, veículo, motorista e custos.
 *
 * 📤 SAÍDAS       : DTO de leitura para relatórios de custos.
 *
 * 🔗 CHAMADA POR  : Relatórios e dashboards de custos.
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
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewCustosViagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de custos por viagem.
     *
     * 📥 ENTRADAS     : Identificadores e métricas de custos.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e relatórios de custos.
     *
     * 🔄 CHAMA        : NotMapped, IFormFile.
     ****************************************************************************************/
    public class ViewCustosViagem
    {
        // Identificador da viagem.
        public Guid ViagemId { get; set; }

        // Identificador do motorista.
        public Guid? MotoristaId { get; set; }

        // Identificador do veículo.
        public Guid? VeiculoId { get; set; }

        // Identificador do setor solicitante.
        public Guid? SetorSolicitanteId { get; set; }

        // Número da ficha de vistoria.
        public int? NoFichaVistoria { get; set; }

        // Data inicial (formatada).
        public string? DataInicial { get; set; }

        // Data final (formatada).
        public string? DataFinal { get; set; }

        // Hora de início (formatada).
        public string? HoraInicio { get; set; }

        // Hora de fim (formatada).
        public string? HoraFim { get; set; }

        // Finalidade da viagem.
        public string? Finalidade { get; set; }

        // Km inicial.
        public int? KmInicial { get; set; }

        // Km final.
        public int? KmFinal { get; set; }

        // Quilometragem percorrida.
        public int? Quilometragem { get; set; }

        // Status da viagem.
        public string? Status { get; set; }

        // Descrição do veículo.
        public string? DescricaoVeiculo { get; set; }

        // Nome do motorista.
        public string? NomeMotorista { get; set; }

        // Custo do motorista (formatado).
        public string? CustoMotorista { get; set; }

        // Custo do veículo (formatado).
        public string? CustoVeiculo { get; set; }

        // Custo de combustível (formatado).
        public string? CustoCombustivel { get; set; }

        // Status do agendamento.
        public bool StatusAgendamento { get; set; }

        // Número de linha (uso em paginação).
        public long? RowNum { get; set; }

        // Upload de foto (não mapeado).
        [NotMapped]
        public IFormFile FotoUpload { get; set; }
    }
}
