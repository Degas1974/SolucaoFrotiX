/* ****************************************************************************************
 * ⚡ ARQUIVO: Lavagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar lavagens de veículos com horário e vínculos.
 *
 * 📥 ENTRADAS     : Datas/horários e identificadores de veículo/motorista.
 *
 * 📤 SAÍDAS       : Entidade persistida para controle de lavagens.
 *
 * 🔗 CHAMADA POR  : Processos de lavagem e relatórios.
 *
 * 🔄 CHAMA        : DataAnnotations e ForeignKey.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ MODEL: Lavagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar a lavagem de um veículo em data e horário específico.
     *
     * 📥 ENTRADAS     : Data, horário da lavagem e vínculos.
     *
     * 📤 SAÍDAS       : Registro persistido de lavagem.
     *
     * 🔗 CHAMADA POR  : Processos de lavagem.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class Lavagem
    {
        // Identificador único da lavagem.
        [Key]
        public Guid LavagemId { get; set; }

        // Data da lavagem.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }

        // Horário da lavagem.
        [Display(Name = "Horário da Lavagem")]
        public DateTime? HorarioLavagem { get; set; }

        // Horário (coluna adicional no banco).
        [Display(Name = "Horário")]
        public DateTime? Horario { get; set; }

        // Veículo lavado.
        [Display(Name = "Veículo Lavado")]
        public Guid VeiculoId { get; set; }

        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        // Motorista associado à lavagem.
        [Display(Name = "Motorista")]
        public Guid MotoristaId { get; set; }

        // Navegação para motorista.
        [ForeignKey("MotoristaId")]
        public virtual Motorista? Motorista { get; set; }
    }
}
