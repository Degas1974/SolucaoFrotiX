/* ****************************************************************************************
 * ⚡ ARQUIVO: LotacaoMotorista.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar lotações de motoristas em unidades/setores com período e motivo.
 *
 * 📥 ENTRADAS     : Motorista, unidade, datas e motivo de mudança.
 *
 * 📤 SAÍDAS       : Entidade persistida para histórico de lotação.
 *
 * 🔗 CHAMADA POR  : Gestão de lotação e movimentações internas.
 *
 * 🔄 CHAMA        : DataAnnotations.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ MODEL: LotacaoMotorista
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar lotação de motorista em unidade/setor.
     *
     * 📥 ENTRADAS     : Motorista, unidade, datas e motivo.
     *
     * 📤 SAÍDAS       : Registro persistido para consultas e auditoria.
     *
     * 🔗 CHAMADA POR  : Fluxos de lotação.
     ****************************************************************************************/
    public class LotacaoMotorista
        {
        // Identificador único da lotação.
        [Key]
        public Guid LotacaoMotoristaId { get; set; }

        // Motorista titular da lotação.
        public Guid MotoristaId { get; set; }

        // Motorista de cobertura (quando aplicável).
        public Guid MotoristaCoberturaId { get; set; }

        // Unidade/órgão de lotação.
        public Guid UnidadeId { get; set; }

        // Data de início da lotação.
        [Required(ErrorMessage = "(A data de início da lotação é obrigatória)")]
        [Display(Name = "Data de Início")]
        public DateTime? DataInicio { get; set; }

        // Data final da lotação.
        [Display(Name = "Data de Fim")]
        public DateTime? DataFim { get; set; }

        // Indica se o motorista está lotado (S/N).
        [Display(Name = "Lotado (S/N)")]
        public bool Lotado { get; set; }

        // Motivo da mudança de lotação.
        [Required(ErrorMessage = "(O motivo de mudança da lotação é obrigatório)")]
        [Display(Name = "Motivo de Mudança")]
#pragma warning disable CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.
        public string? Motivo { get; set; }
#pragma warning restore CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.

        }
    }

