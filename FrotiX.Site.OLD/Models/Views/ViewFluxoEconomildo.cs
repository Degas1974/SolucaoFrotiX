/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewFluxoEconomildo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL do fluxo Economildo (viagens, tempos e motorista)
 *
 * 📥 ENTRADAS     : Veículo, viagem economildo, motorista, data/hora e tipo de condutor
 *
 * 📤 SAÍDAS       : DTO de leitura para dashboards do app Economildo
 *
 * 🔗 CHAMADA POR  : Consultas e relatórios de fluxo
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
     * ⚡ MODEL: ViewFluxoEconomildo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de fluxo Economildo
     *
     * 📥 ENTRADAS     : Dados de viagem, veículo, motorista e horários
     *
     * 📤 SAÍDAS       : Registro somente leitura para dashboards
     *
     * 🔗 CHAMADA POR  : Consultas de fluxo e relatórios
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewFluxoEconomildo
    {
        // [DADOS] Identificador do veículo
        public Guid VeiculoId { get; set; }

        // [DADOS] Identificador da viagem no app Economildo
        public Guid ViagemEconomildoId { get; set; }

        // [DADOS] Identificador do motorista
        public Guid MotoristaId { get; set; }

        // [DADOS] Tipo de condutor (motorista/cobrador/etc)
        public string? TipoCondutor { get; set; }

        // [DADOS] Data da viagem
        public DateTime? Data { get; set; }

        // [DADOS] MOB (Modo Operacional - sigla do serviço)
        public string? MOB { get; set; }

        // [DADOS] Hora de início (formatada)
        public string? HoraInicio { get; set; }

        // [DADOS] Hora de término (formatada)
        public string? HoraFim { get; set; }

        // [DADOS] Quantidade de passageiros transportados
        public int? QtdPassageiros { get; set; }

        // [DADOS] Nome completo do motorista
        public string? NomeMotorista { get; set; }

        // [DADOS] Descrição consolidada do veículo (placa + marca/modelo)
        public string? DescricaoVeiculo { get; set; }


        }
    }


