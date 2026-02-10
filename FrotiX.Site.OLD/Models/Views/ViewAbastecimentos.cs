/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewAbastecimentos.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de abastecimentos para relatórios e filtros.
 *
 * 📥 ENTRADAS     : Campos consolidados de abastecimentos e veículos.
 *
 * 📤 SAÍDAS       : DTO de leitura para consultas.
 *
 * 🔗 CHAMADA POR  : Relatórios e telas de abastecimento.
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
     * ⚡ MODEL: ViewAbastecimentos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de abastecimentos.
     *
     * 📥 ENTRADAS     : Dados de abastecimento, veículo e motorista.
     *
     * 📤 SAÍDAS       : Registro somente leitura para relatórios.
     *
     * 🔗 CHAMADA POR  : Consultas e dashboards.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewAbastecimentos
        {

        // Identificador do abastecimento.
        public Guid AbastecimentoId { get; set; }

        // Data e hora do abastecimento.
        public DateTime? DataHora { get; set; }

        // Data formatada.
        public String? Data { get; set; }

        // Hora formatada.
        public String? Hora { get; set; }

        // Placa do veículo.
        public string? Placa { get; set; }

        // Tipo do veículo.
        public string? TipoVeiculo { get; set; }

        // Nome do veículo ou item relacionado.
        public string? Nome { get; set; }

        // Motorista/condutor.
        public string? MotoristaCondutor { get; set; }

        // Tipo de combustível.
        public string? TipoCombustivel { get; set; }

        // Sigla do combustível ou unidade.
        public string? Sigla { get; set; }

        // Litros abastecidos (string formatada).
        public string? Litros { get; set; }

        // Valor unitário formatado.
        public string? ValorUnitario { get; set; }

        // Valor total formatado.
        public string? ValorTotal { get; set; }

        // Consumo formatado.
        public string? Consumo { get; set; }

        // Consumo geral numérico.
        public decimal? ConsumoGeral { get; set; }

        // Km rodado no período.
        public int KmRodado { get; set; }

        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

        // Identificador do combustível.
        public Guid CombustivelId { get; set; }

        // Identificador da unidade.
        public Guid UnidadeId { get; set; }

        // Identificador do motorista.
        public Guid MotoristaId { get; set; }

        }
    }
