/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LotacaoMotorista.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para lotação de motoristas em unidades/setores.                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENTIDADE: LotacaoMotorista (LotacaoMotoristaId, MotoristaId, UnidadeId, DataInicio, DataFim)     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Validations                                                                         ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;

namespace FrotiX.Models
    {
    public class LotacaoMotorista
        {
        [Key]
        public Guid LotacaoMotoristaId { get; set; }

        public Guid MotoristaId { get; set; }

        public Guid MotoristaCoberturaId { get; set; }

        public Guid UnidadeId { get; set; }

        [Required(ErrorMessage = "(A data de início da lotação é obrigatória)")]
        [Display(Name = "Data de Início")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime? DataFim { get; set; }

        [Display(Name = "Lotado (S/N)")]
        public bool Lotado { get; set; }

        [Required(ErrorMessage = "(O motivo de mudança da lotação é obrigatório)")]
        [Display(Name = "Motivo de Mudança")]
#pragma warning disable CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.
        public string? Motivo { get; set; }
#pragma warning restore CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.

        }
    }


