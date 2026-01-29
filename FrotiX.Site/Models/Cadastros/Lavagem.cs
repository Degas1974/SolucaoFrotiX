/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Lavagem.cs                                                                              ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para registros de lavagens de veículos (data, horário, veículo).║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENTIDADE: Lavagem (LavagemId, Data, HorarioInicio, HorarioFim, VeiculoId) + FK para Veiculo      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Validations, Veiculo                                                                ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    public class Lavagem
    {
        [Key]
        public Guid LavagemId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }

        [Display(Name = "Horário Início")]
        public DateTime? HorarioInicio { get; set; }

        [Display(Name = "Horário Fim")]
        public DateTime? HorarioFim { get; set; }

        [Display(Name = "Veículo Lavado")]
        public Guid VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        [Display(Name = "Motorista")]
        public Guid MotoristaId { get; set; }

        [ForeignKey("MotoristaId")]
        public virtual Motorista? Motorista { get; set; }
    }
}
