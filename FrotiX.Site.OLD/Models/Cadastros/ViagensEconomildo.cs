/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagensEconomildo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar viagens do app mobile Economildo e seus vínculos.
 *
 * 📥 ENTRADAS     : Dados da viagem, veículo e motorista.
 *
 * 📤 SAÍDAS       : Entidade persistida para consultas e relatórios.
 *
 * 🔗 CHAMADA POR  : Integrações do app Economildo.
 *
 * 🔄 CHAMA        : DataAnnotations, ForeignKey.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
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
     * ⚡ MODEL: ViagensEconomildo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar uma viagem registrada no app Economildo.
     *
     * 📥 ENTRADAS     : Datas, trajetos e vínculos.
     *
     * 📤 SAÍDAS       : Registro persistido de viagem mobile.
     *
     * 🔗 CHAMADA POR  : Processos de integração e auditoria.
     ****************************************************************************************/
    public class ViagensEconomildo
    {
        // Identificador da viagem.
        [Key]
        public Guid ViagemEconomildoId { get; set; }

        // Data da viagem.
        public DateTime? Data { get; set; }

        // Identificador MOB.
        public string? MOB { get; set; }

        // Responsável pela viagem.
        public string? Responsavel { get; set; }

        // Veículo utilizado.
        public Guid VeiculoId { get; set; }

        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        // Motorista responsável.
        public Guid MotoristaId { get; set; }

        // Navegação para motorista.
        [ForeignKey("MotoristaId")]
        public virtual Motorista? Motorista { get; set; }

        // Indica se é ida/volta.
        public string? IdaVolta { get; set; }

        // Hora de início.
        public string? HoraInicio { get; set; }

        // Hora de fim.
        public string? HoraFim { get; set; }

        // Quantidade de passageiros.
        public int? QtdPassageiros { get; set; }

        // Trajeto da viagem.
        public string? Trajeto { get; set; }

        // Duração da viagem.
        public int? Duracao { get; set; }
    }
}
