/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewFluxoEconomildoData.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de fluxo Economildo com filtros por data
 *
 * 📥 ENTRADAS     : Veículo, viagem, motorista, data/hora e dados operacionais
 *
 * 📤 SAÍDAS       : DTO de leitura para dashboards diários do Economildo
 *
 * 🔗 CHAMADA POR  : Consultas de fluxo com filtros de período
 *
 * 🔄 CHAMA        : Não se aplica
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Services, FrotiX.Validations
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
     * ⚡ MODEL: ViewFluxoEconomildoData
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de fluxo Economildo com dados por data
     *
     * 📥 ENTRADAS     : Viagens consolidadas por data, veículo, motorista
     *
     * 📤 SAÍDAS       : Registro somente leitura para filtros e relatórios
     *
     * 🔗 CHAMADA POR  : Consultas com filtro de período
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewFluxoEconomildoData
    {
        // [DADOS] Identificador do veículo
        public Guid VeiculoId { get; set; }

        // [DADOS] Identificador da viagem no app Economildo
        public Guid ViagemEconomildoId { get; set; }

        // [DADOS] Identificador do motorista
        public Guid MotoristaId { get; set; }

        // [DADOS] Tipo de condutor (motorista/cobrador/etc)
        public string? TipoCondutor { get; set; }

        // [DADOS] Data da viagem (filtro principal)
        public DateTime? Data { get; set; }

        // [DADOS] MOB (Modo Operacional)
        public string? MOB { get; set; }

        // [DADOS] Hora de início (formatada)
        public string? HoraInicio { get; set; }

        // [DADOS] Hora de término (formatada)
        public string? HoraFim { get; set; }

        // [DADOS] Quantidade de passageiros
        public int? QtdPassageiros { get; set; }

        // [DADOS] Nome do motorista
        public string? NomeMotorista { get; set; }

        // [DADOS] Descrição do veículo
        public string? DescricaoVeiculo { get; set; }


        }
    }

