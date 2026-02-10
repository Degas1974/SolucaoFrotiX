/* ****************************************************************************************
 * ⚡ ARQUIVO: ItensManutencao.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar itens de manutenção de veículos (peças e serviços).
 *
 * 📥 ENTRADAS     : Dados do item, vínculo com manutenção, motorista e viagem.
 *
 * 📤 SAÍDAS       : Entidade persistida para controle de manutenção.
 *
 * 🔗 CHAMADA POR  : Fluxos de manutenção e relatórios técnicos.
 *
 * 🔄 CHAMA        : ForeignKey, NotMapped.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 *
 * ⚠️ ATENÇÃO      : NumOS e DataOS são NotMapped (uso apenas em UI).
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ItensManutencao
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Registrar itens vinculados à manutenção, motorista e viagem.
     *
     * 📥 ENTRADAS     : Tipo, resumo, descrição e status do item.
     *
     * 📤 SAÍDAS       : Registro persistido para auditoria e acompanhamento.
     *
     * 🔗 CHAMADA POR  : Fluxos de manutenção.
     *
     * 🔄 CHAMA        : ForeignKey, NotMapped.
     *
     * ⚠️ ATENÇÃO      : NumOS e DataOS são NotMapped (uso na UI).
     ****************************************************************************************/
    public class ItensManutencao
    {
        [Key]
        public Guid ItemManutencaoId { get; set; }

        public string? TipoItem { get; set; }

        public string? NumFicha { get; set; }

        public DateTime? DataItem { get; set; }

        public string? Resumo { get; set; }

        public string? Descricao { get; set; }

        public string? Status { get; set; }

        public string? ImagemOcorrencia { get; set; }

        public Guid? ManutencaoId { get; set; }

        [ForeignKey("ManutencaoId")]
        public virtual Manutencao Manutencao { get; set; }

        public Guid? MotoristaId { get; set; }

        [ForeignKey("MotoristaId")]
        public virtual Motorista Motorista { get; set; }

        public Guid? ViagemId { get; set; }

        [ForeignKey("ViagemId")]
        public virtual Viagem Viagem { get; set; }

        [NotMapped]
        public string NumOS { get; set; }

        [NotMapped]
        public string DataOS { get; set; }
    }
}
